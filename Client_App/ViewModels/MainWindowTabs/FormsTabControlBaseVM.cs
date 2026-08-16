using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.Delete;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.RaodbExport;
using Client_App.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.MainWindowTabs;

public abstract class FormsTabControlBaseVM : INotifyPropertyChanged
{
    #region Commands

    /// <summary>
    /// Создать и открыть новое окно с отчётом по форме 1.x, 4.1 и 5.x для выбранной организации.
    /// Для 2.x используется старая команда, пока не обновим там интерфейс.
    /// </summary>
    public ICommand AddReport { get; private set; }

    /// <summary>
    /// Редактировать выбранный отчёт
    /// </summary>
    public ICommand NewChangeReport => new NewChangeReportAsyncCommand(this);

    /// <summary>
    /// Редактировать выбранный отчёт
    /// </summary>
    public ICommand NewChangeReports => new NewChangeReportsAsyncCommand(this);

    /// <summary>
    /// Удалить выбранный отчёт
    /// </summary>
    public ICommand DeleteReport => new NewDeleteFormAsyncCommand(this);

    /// <summary>
    /// Выбранная форма -> Выгрузка Excel -> Для печати
    /// </summary>
    public ICommand ExcelExportFormPrint => new ExcelExportFormPrintAsyncCommand(this);

    /// <summary>
    /// Экспорт отчёта в файл .RAODB
    /// </summary>
    public ICommand ExportReport => new ExportReportAsyncCommand(this); 
    
    #endregion

    #region Constructor

    protected FormsTabControlBaseVM()
    {
        // Конструктор пуст - настройки загружаются лениво при первом доступе
    }

    protected FormsTabControlBaseVM(MainWindowVM mainWindowVM)
    {
        AddReport = new AddReportAsyncCommand();

        MainWindowVM = mainWindowVM;
    }

    #endregion

    #region Properties

    private CancellationTokenSource? _debounceCts;

    public MainWindowVM MainWindowVM { get; }

    private protected abstract byte DefaultOrgsPerPage { get; }

    private protected abstract byte DefaultFormsPerPage { get; }

    private protected abstract char FormNum { get; }

    private protected abstract ObservableCollection<Report>? ReportCollection { get; }

    private protected abstract ObservableCollection<Reports>? ReportsCollection { get; }

    private protected abstract Dictionary<string, Func<IQueryable<Report>, IQueryable<object>>> RowSelectors { get; }

    private protected abstract int TotalPagesForms { get; }

    private protected abstract int TotalPagesOrgs { get; }

    private protected abstract int TotalRowsForms { get; }

    #region CurrentPageForms

    private int _currentPageForms = 1;
    private protected int CurrentPageForms
    {
        get
        {
            if (_currentPageForms > TotalPagesForms && TotalPagesForms > 0)
                _currentPageForms = TotalPagesForms;

            return _currentPageForms;
        }
        set
        {
            _currentPageForms = value;
            OnPropertyChanged();
            UpdateReportCollection();
        }
    }

    #endregion

    #region CurrentPageOrgs

    private int _currentPageOrgs = 1;
    public int CurrentPageOrgs
    {
        get
        {
            if (_currentPageOrgs > TotalPagesOrgs)
                _currentPageOrgs = TotalPagesOrgs;
            return _currentPageOrgs;
        }
        set
        {
            _currentPageOrgs = value;
            OnPropertyChanged(nameof(ReportsCollection));
            OnPropertyChanged();
            // TotalReportCount не зависит от страницы org — не пересчитываем (дорого на Firebird).
        }
    }

    #endregion

    #region InSelectedReportFormsCount
    
    private int _inSelectedReportFormsCount;
    public int InSelectedReportFormsCount
    {
        get => _inSelectedReportFormsCount;
        private set
        {
            if (_inSelectedReportFormsCount != value)
            {
                _inSelectedReportFormsCount = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region RowsCountForms

    private int _rowsCountForms;
    private protected int RowsCountForms
    {
        get
        {
            if (_rowsCountForms == 0) // If not loaded yet
            {
                var (_, forms) = Properties.RowCountSettings.RowCountSettingsManager.LoadSettings(
                    "form" + FormNum, DefaultOrgsPerPage, DefaultFormsPerPage);
                _rowsCountForms = forms;
            }
            return _rowsCountForms;
        }
        set
        {
            if (_rowsCountForms != value)
            {
                _rowsCountForms = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPagesForms));
                UpdateReportCollection();
                SaveRowCountSettings();
            }
        }
    } 
    
    #endregion

    #region RowsCountOrgs

    private int _rowsCountOrgs;
    private protected int RowsCountOrgs
    {
        get
        {
            if (_rowsCountOrgs == 0) // If not loaded yet
            {
                var (orgs, _) = Properties.RowCountSettings.RowCountSettingsManager.LoadSettings(
                    "form" + FormNum, DefaultOrgsPerPage, DefaultFormsPerPage);
                _rowsCountOrgs = orgs;
            }
            return _rowsCountOrgs;
        }
        set
        {
            if (_rowsCountOrgs != value)
            {
                _rowsCountOrgs = value;
                NotifyRowsChanged();
                SaveRowCountSettings();
            }
        }

    }

    #endregion

    #region SearchText

    private string _searchText;
    private protected string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText == value) return;

            _searchText = value;
            OnPropertyChanged();

            // Отменяем предыдущий таймер
            _debounceCts?.Cancel();

            // Создаем новый таймер
            _debounceCts = new CancellationTokenSource();

            // Задержка 300мс перед фильтрацией
            Task.Delay(300, _debounceCts.Token)
                .ContinueWith(t =>
                {
                    if (!t.IsCanceled)
                    {
                        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            if (CurrentPageOrgs != 1)
                                CurrentPageOrgs = 1;

                            NotifySearchTextChanged();
                        });
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    #endregion

    #region SelectedReport

    private Report? _selectedReport;
    public Report? SelectedReport
    {
        get => _selectedReport;
        set
        {
            if (_selectedReport == value) return;

            _selectedReport = value;
            OnPropertyChanged();

            _ = UpdateInSelectedReportFormsCountAsync();
        }
    }

    #endregion

    #region SelectedReports

    private Reports? _selectedReports;
    public Reports? SelectedReports
    {
        get => _selectedReports;
        set
        {
            _selectedReports = value;

            _ = UpdateInSelectedReportFormsCountAsync();

            OnPropertyChanged();

            // Проверяем и сбрасываем фильтр если нет отчётов для текущего фильтра
            CheckAndResetFilterIfNeeded();

            // UpdateReportCollection выполняется в CurrentPageForms
            // Чтобы не вызывать метод дважды используется if else
            if (CurrentPageForms != 1)
                CurrentPageForms = 1;
            else
                UpdateReportCollection();

            UpdateFormsPageInfo();


        }
    }

    #endregion

    public int TotalReportCount
    {
        get
        {
            var db = StaticConfiguration.DBModel;
            return FormNum switch
            {
                '1' => MainWindowListQuery.CountAllReportsForFormType(db, '1', SearchText, "1.0"),
                '2' => MainWindowListQuery.CountAllReportsForFormType(db, '2', SearchText, "2.0"),
                '4' => MainWindowListQuery.CountAllReportsForForm40(db, SearchText),
                '5' => MainWindowListQuery.CountAllReportsForForm50(db, SearchText),
                _ => 0
            };
        }
    }

    /// <summary>
    /// Всего организаций с учётом фильтра.
    /// </summary>
    private protected abstract int FilteredRowsOrgs { get; }

    /// <summary>
    /// Дополнительные условия поиска для переопределения в дочерних классах.
    /// </summary>
    protected virtual bool GetAdditionalSearchConditions(Reports reps, string search) => false;

    /// <summary>
    /// Проверяет и сбрасывает фильтр если при переключении организации нет отчётов для текущего фильтра.
    /// Переопределяется в дочерних классах.
    /// </summary>
    protected virtual void CheckAndResetFilterIfNeeded() { }

    /// <summary>
    /// Всего организаций.
    /// </summary>
    public int TotalRowsOrgs => StaticConfiguration.DBModel.ReportsCollectionDbSet
        .Where(x => x.DBObservable != null)
        .Count(reps => reps.Master_DB.FormNum_DB == FormNum + ".0");

    #endregion

    #region Methods

    /// <summary>
    /// Возвращает количество строчек форм у отчёта.
    /// </summary>
    /// <param name="rep">Отчёт, у которого нужно посчитать количество строчек форм.</param>
    /// <returns>Количество строчек форм.</returns>
    private async Task<int> GetReportRowsCount(Report? rep)
    {
        if (rep == null || rep.FormNum == null) return 0;

        while (StaticConfiguration.IsFileLocked(null))
            await Task.Delay(50);

        await using var db = new DBModel(StaticConfiguration.DBPath);

        var baseQuery = db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id);

        if (RowSelectors.TryGetValue(rep.FormNum_DB, out var selector))
        {
            return await selector(baseQuery).CountAsync();
        }

        return 0;
    }

    private protected abstract void NotifySearchTextChanged();

    private void NotifyRowsChanged()
    {
        OnPropertyChanged(nameof(RowsCountOrgs));
        OnPropertyChanged(nameof(ReportsCollection));
        OnPropertyChanged(nameof(TotalPagesOrgs));
    }

    private void SaveRowCountSettings()
    {
        Properties.RowCountSettings.RowCountSettingsManager.SaveSettings(
            "form" + FormNum,
            _rowsCountOrgs,
            _rowsCountForms);
    }

    public void UpdateFormsPageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsForms));
        OnPropertyChanged(nameof(TotalPagesForms));
    }

    private async Task UpdateInSelectedReportFormsCountAsync()
    {
        if (SelectedReport == null)
        {
            InSelectedReportFormsCount = 0;
            return;
        }

        // Асинхронно получаем данные
        var count = await GetReportRowsCount(SelectedReport);

        // Записываем в свойство - UI автоматически обновится через OnPropertyChanged
        InSelectedReportFormsCount = count;
    }

    public virtual void UpdateReportCollection()
    {
        OnPropertyChanged(nameof(ReportCollection));
    }

    public void UpdateReportsCollection()
    {
        OnPropertyChanged(nameof(ReportsCollection));
    }

    /// <summary>
    /// Обновляет содержимое коллекции организаций без пересоздания объекта.
    /// Сохраняет выбранную организацию.
    /// </summary>
    public virtual void UpdateReportsCollectionWithoutReCreation()
    {
        var currentCollection = ReportsCollection;
        if (currentCollection == null) return;

        var selectedId = SelectedReports?.Master_DB?.Id;
        var page = MainWindowListQuery.GetOrgPage(
            StaticConfiguration.DBModel,
            FormNum,
            SearchText,
            CurrentPageOrgs,
            RowsCountOrgs);

        currentCollection.Clear();
        foreach (var item in page.Items)
            currentCollection.Add(item);

        if (selectedId.HasValue)
        {
            var restored = currentCollection.FirstOrDefault(r => r.Master_DB?.Id == selectedId.Value);
            if (restored != null)
                SetSelectedReportsWithoutReload(restored);
        }
    }

    /// <summary>
    /// Меняет SelectedReports без перезагрузки списка отчётов (восстановление после refresh org page).
    /// </summary>
    protected void SetSelectedReportsWithoutReload(Reports? reports)
    {
        _selectedReports = reports;
        OnPropertyChanged(nameof(SelectedReports));
    }

    public void UpdateTotalReportCount()
    {
        OnPropertyChanged(nameof(TotalReportCount));
    }

    public void UpdateTotalReportsCount()
    {
        OnPropertyChanged(nameof(FilteredRowsOrgs));
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}