using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Commands.AsyncCommands.Delete;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.Commands.AsyncCommands.SwitchReport;
using Client_App.Commands.SyncCommands;
using Client_App.Services.DataAccess;
using Client_App.ViewModels.Controls;
using Models.Collections;
using Models.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Models.Attributes;
using Models.DBRealization;

namespace Client_App.ViewModels.Forms;

public abstract class BaseFormVM : BaseVM, INotifyPropertyChanged
{
    #region Properties

    public abstract string FormType { get; }

    public string WindowTitle
    {
        get
        {
            var formNum = FormType.Replace(".", "");
            var typeName = $"Models.Forms.Form{formNum[0]}.Form{formNum},Models";
            var formName = ((Form_ClassAttribute)Type.GetType(typeName)!
                .GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;

            var master = Reports?.Master_DB;
            string reg = "", shortName = "", okpo = "";
            try
            {
                if (master != null)
                {
                    reg = master.RegNoRep?.Value ?? "";
                    shortName = master.ShortJurLicoRep?.Value ?? "";
                    okpo = master.OkpoRep?.Value ?? "";
                }
            }
            catch
            {
                // Rows10/20 могут быть ещё не загружены
            }

            return $"{formName} {reg}  {shortName}  {okpo}";
        }
    }

    private ObservableCollection<Form> _formList = [];
    public ObservableCollection<Form> FormList
    {
        get => _formList;
        set
        {
            _formList = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Note> _noteList = [];
    public ObservableCollection<Note> NoteList
    {
        get => _noteList;
        set
        {
            _noteList = value;
            OnPropertyChanged();
        }
    }

    #region Report

    private Report _report;
    public Report Report
    {
        get => _report;
        set
        {
            _report = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Reports

    private Reports _reports;
    public Reports Reports
    {
        get => _reports;
        set
        {
            if (_reports != value)
            {
                _reports = value;
                OnPropertyChanged();
            }
        }
    }
    #endregion

    #region DBO

    private DBObservable _DBO;
    public DBObservable DBO
    {
        get => _DBO;
        set
        {
            if (_DBO != value)
            {
                _DBO = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region IsCanSaveReportEnabled

    private bool _isCanSaveReportEnabled;
    public bool IsCanSaveReportEnabled
    {
        get => _isCanSaveReportEnabled;
        set
        {
            if (_isCanSaveReportEnabled != value)
            {
                _isCanSaveReportEnabled = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion


    private Form? _selectedForm;
    public Form? SelectedForm
    {
        get => _selectedForm;
        set
        {
            _selectedForm = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Form> _selectedForms = [];
    public ObservableCollection<Form> SelectedForms
    {
        get => _selectedForms;
        set
        {
            if (_selectedForms != value)
            {
                UnsubscribeSelectedForms(_selectedForms);
                _selectedForms = value;
                SubscribeSelectedForms(_selectedForms);
                OnPropertyChanged();
                OnPropertyChanged(nameof(AnyRowSelected));
                OnPropertyChanged(nameof(OnlyOneRowSelected));
            }
        }
    }

    #region AnyRowSelected

    private bool _anyRowSelected;
    public bool AnyRowSelected
    {
        get => SelectedForms.Any();
        set
        {
            if (_anyRowSelected != value)
            {
                _anyRowSelected = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    private bool _dataGridIsEditing;
    public bool DataGridIsEditing
    {
        get => _dataGridIsEditing;
        set
        {
            if (_dataGridIsEditing != value)
            {
                _dataGridIsEditing = value;
                OnPropertyChanged();
            }
        }
    }

    #region OnlyOneRowSelected

    private bool _onlyOneRowSelected;
    public bool OnlyOneRowSelected
    {
        get => SelectedForms.Count == 1;
        set
        {
            if (_onlyOneRowSelected != value)
            {
                _onlyOneRowSelected = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region SkipChangeTacking

    private bool _skipChangeTacking;
    public bool SkipChangeTacking
    {
        get => _skipChangeTacking;
        set
        {
            if (_skipChangeTacking != value)
            {
                _skipChangeTacking = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    private Note _selectedNote;
    public Note SelectedNote
    {
        get => _selectedNote;
        set
        {
            _selectedNote = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Note> _selectedNotes = [];
    public ObservableCollection<Note> SelectedNotes
    {
        get => _selectedNotes;
        set
        {
            if (_selectedNotes != value)
            {
                _selectedNotes = value;
                OnPropertyChanged();
            }
        }
    }

    private int _rowCount = 30;
    public int RowCount
    {
        get => _rowCount;
        set
        {
            var result = value;
            if (value <= 0)
                result = 1;
            else if (value > 50) //Хардкод максимального кол-ва загруженных строк
                result = 50;

            if (_rowCount != result)
            {
                if (ShouldBlockPageChange())
                {
                    OnPropertyChanged();
                    _ = NotifyUnsavedPageChangeAsync();
                    return;
                }

                _rowCount = result;
                OnPropertyChanged();
                _ = RefreshFormListForPagingAsync();
            }
        }
    }

    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            var result = value;
            if (value <= 0)
                result = 1;
            else if (value > TotalPages && TotalPages > 0)
                result = TotalPages;

            if (result == _currentPage)
                return;

            if (ShouldBlockPageChange())
            {
                OnPropertyChanged();
                _ = NotifyUnsavedPageChangeAsync();
                return;
            }

            _currentPage = result;
            OnPropertyChanged();
            _ = RefreshFormListForPagingAsync();
        }
    }

    /// <summary>
    /// true — в Report.Rows только текущая страница из БД; TotalRows берётся из DbTotalRows.
    /// </summary>
    public bool UseDbPaging { get; set; }

    public int? DbTotalRows { get; set; }

    public int TotalPages
    {
        get
        {
            var total = TotalRows;
            if (total <= 0 || RowCount <= 0) return 0;
            var result = total / RowCount;
            if (total % RowCount != 0)
                result++;
            return result;
        }
    }

    public int TotalRows => DbTotalRows ?? Report.Rows.Count;


    private bool _isAutoReplaceEnabled = true;
    public bool IsAutoReplaceEnabled
    {
        get => _isAutoReplaceEnabled;
        set
        {
            if (_isAutoReplaceEnabled != value)
            {
                _isAutoReplaceEnabled = value;
                Report.AutoReplace = value;
                OnPropertyChanged();
            }
        }
    }

    private int _selectedYear = DateTime.Now.Year;
    public int SelectedYear
    {
        get => _selectedYear;
        set
        {
            _selectedYear = value;
            OnPropertyChanged();
        }
    }

    private ExecutorDataControlVM _executorDataControlVM;
    public ExecutorDataControlVM ExecutorDataControlVM
    {
        get
        {
            return _executorDataControlVM;
        }
    }

    private SelectReportPopupVM _selectReportVM;
    public SelectReportPopupVM SelectReportPopupVM
    {
        get
        {
            return _selectReportVM;
        }
        set
        {
            _selectReportVM = value;
        }
    }
    #endregion

    #region Constructors

    public BaseFormVM() 
    { 
        SubscribeSelectedForms(_selectedForms);
    }

    /// <summary>
    /// Редактирование ранее созданного отчёта.
    /// </summary>
    /// <param name="report">Отчёт.</param>
    public BaseFormVM(Report report)
    {
        _report = report;
        _reports = report.Reports; 
        //_DBO = report.Reports.DBObservable;
        UpdateFormList();
        UpdatePageInfo();

        //Загружаем примечаний
        Report.Notes = new ObservableCollectionWithItemPropertyChanged<Note>(
            StaticConfiguration.DBModel.notes
            .Where(note => note.ReportId == Report.Id));
        NoteList = Report.Notes;

        SubscribeSelectedForms(_selectedForms);
        InitializeUserControls();
    }

    public void InitializeUserControls()
    {
        _selectReportVM = new SelectReportPopupVM(this);
        _executorDataControlVM = new ExecutorDataControlVM(this.Report);
    }
    #endregion

    #region Commands

    public ICommand CheckForm => new NewCheckFormAsyncCommand(this);    //  Кнопка "Проверить"
    //public ICommand CopyExecutorDate => new NewCopyExecutorDataAsyncCommand(this); //После привязки кнопка неактивна
    public ICommand SourceTransmissionAll => new SourceTransmissionAllAsyncCommand(this);    //  Кнопка "Перевести данные предыдущей формы"
    public ICommand AddRow => new NewAddRowAsyncCommand(this);
    public ICommand AddRows => new NewAddRowsAsyncCommand(this);
    public ICommand AddRowsIn => new NewAddRowsInAsyncCommand(this);
    public ICommand DeleteDataInRows => new NewDeleteDataInRowsAsyncCommand();
    public ICommand DeleteRows => new NewDeleteRowsAsyncCommand(this);
    public ICommand SortForm => new NewSortFormSyncCommand(this);
    public ICommand SetNumberOrder => new NewSetNumberOrderSyncCommand(this);
    public ICommand SortForms => new NewSortFormSyncCommand(this);
    public ICommand CopyRows => new NewCopyRowsAsyncCommand();
    public ICommand PasteRows => new NewPasteRowsAsyncCommand(this);
    public ICommand SelectAll => new SelectAllRowsAsyncCommand(this);
    public ICommand SaveReport => new SaveReportAsyncCommand(this);
    public ICommand AddNote => new NewAddNoteAsyncCommand(this);
    public ICommand AddNotes => new NewAddNotesAsyncCommand(this);
    public ICommand CopyNotes => new NewCopyNotesAsyncCommand();
    public ICommand PasteNotes => new NewPasteNotesAsyncCommand(this);
    public ICommand DeleteNotes => new NewDeleteNoteAsyncCommand(this);
    public ICommand SetDefaultColumnWidth => new SetDefaultColumnWidthAsyncCommand();
    #endregion

    #region SelectionChangeWiring

    /// <summary>
    /// Обновление списка выделенных строчек.
    /// </summary>
    /// <param name="collection"></param>
    private void SubscribeSelectedForms(ObservableCollection<Form> collection)
    {
        if (collection is INotifyCollectionChanged notify)
        {
            notify.CollectionChanged += SelectedForms_CollectionChanged;
        }
    }

    private void UnsubscribeSelectedForms(ObservableCollection<Form> collection)
    {
        if (collection is INotifyCollectionChanged notify)
        {
            notify.CollectionChanged -= SelectedForms_CollectionChanged;
        }
    }

    private void SelectedForms_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(AnyRowSelected));
        OnPropertyChanged(nameof(OnlyOneRowSelected));
    }

    #endregion

    #region  UpdateNoteList

    public void UpdateNoteList()
    {
        NoteList = new ObservableCollection<Note>( Report.Notes.ToList<Note>());
    }

    #endregion

    #region UpdateFormList

    /// <summary>
    /// Обновляет отображение ячеек DataGrid'а.
    /// </summary>
    public void UpdateFormList()
    {
        if (UseDbPaging)
        {
            // В памяти уже только текущая страница
            FormList = new ObservableCollection<Form>(Report.Rows.ToList<Form>());
        }
        else
        {
            FormList = new ObservableCollection<Form>(
                Report.Rows
                    .ToList<Form>()
                    .Skip((CurrentPage - 1) * RowCount)
                    .Take(RowCount));
        }
    }

    /// <summary>
    /// Перед операциями над всеми строками (проверка/сортировка/№ п/п) догружает полный набор.
    /// </summary>
    public async Task EnsureAllRowsForMutationAsync()
    {
        await FormRowsEnsureService.EnsureAllRowsLoadedAsync(Report);
        UseDbPaging = false;
        DbTotalRows = null;
        ClearFormPageCache();
        UpdateFormList();
        UpdatePageInfo();
    }

    private bool ShouldBlockPageChange()
    {
        if (!UseDbPaging || Report?.Id <= 0)
            return false;
        return FormRowsPageLoader.HasPendingFormRowChanges(
            StaticConfiguration.DBModel, Report.Id, FormType);
    }

    private async Task NotifyUnsavedPageChangeAsync()
    {
        try
        {
            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBox.Avalonia.DTO.MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Несохранённые изменения",
                        ContentHeader = "Уведомление",
                        ContentMessage =
                            "Сначала сохраните или отмените изменения строк," +
                            $"{Environment.NewLine}прежде чем перелистывать страницу формы.",
                        MinWidth = 400,
                        WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterOwner,
                        Topmost = true,
                    })
                    .ShowDialog(
                        (Avalonia.Application.Current?.ApplicationLifetime as
                            Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime)
                        ?.MainWindow));
        }
        catch
        {
            // UI-уведомление не должно ронять смену свойства
        }
    }

    private readonly object _formPageCacheGate = new();
    private readonly Dictionary<(int Page, int RowCount), List<Form>> _formPageCache = new();
    private CancellationTokenSource? _formPrefetchCts;

    private void ClearFormPageCache()
    {
        lock (_formPageCacheGate)
            _formPageCache.Clear();
        try { _formPrefetchCts?.Cancel(); } catch { /* ignore */ }
        _formPrefetchCts = null;
    }

    private async Task RefreshFormListForPagingAsync()
    {
        if (UseDbPaging && FormRowsPageLoader.SupportsDbPaging(FormType) && Report?.Id > 0)
        {
            var key = (CurrentPage, RowCount);
            List<Form>? cached;
            lock (_formPageCacheGate)
                _formPageCache.TryGetValue(key, out cached);

            if (cached != null)
            {
                FormRowsPageLoader.ApplyPageToReport(Report, FormType, cached);
            }
            else
            {
                var skip = (CurrentPage - 1) * RowCount;
                await FormRowsPageLoader.LoadPageIntoReportAsync(
                    StaticConfiguration.DBModel, Report, FormType, skip, RowCount);
                CacheCurrentFormPage();
            }

            ScheduleFormPagePrefetch();
        }

        UpdateFormList();
        UpdatePageInfo();
    }

    private void CacheCurrentFormPage()
    {
        if (Report?.Rows == null) return;
        var key = (CurrentPage, RowCount);
        lock (_formPageCacheGate)
        {
            _formPageCache[key] = Report.Rows.ToList<Form>();
            TrimFormPageCache_NoLock();
        }
    }

    private void TrimFormPageCache_NoLock()
    {
        if (_formPageCache.Count <= 4) return;
        var keep = new HashSet<(int, int)>
        {
            (CurrentPage, RowCount),
            (CurrentPage - 1, RowCount),
            (CurrentPage + 1, RowCount)
        };
        foreach (var k in _formPageCache.Keys.Where(k => !keep.Contains(k)).ToList())
            _formPageCache.Remove(k);
    }

    private void ScheduleFormPagePrefetch()
    {
        if (!UseDbPaging || Report?.Id <= 0) return;
        try { _formPrefetchCts?.Cancel(); } catch { /* ignore */ }
        var cts = new CancellationTokenSource();
        _formPrefetchCts = cts;
        var reportId = Report.Id;
        var formType = FormType;
        var rowCount = RowCount;
        var totalPages = TotalPages;
        var current = CurrentPage;
        var dbPath = StaticConfiguration.DBPath;
        var gate = _formPageCacheGate;
        var cache = _formPageCache;

        _ = Task.Run(async () =>
        {
            try
            {
                await using var db = new DBModel(dbPath);
                foreach (var p in new[] { current - 1, current + 1 })
                {
                    if (cts.IsCancellationRequested || p < 1 || p > totalPages) continue;
                    var key = (p, rowCount);
                    lock (gate)
                    {
                        if (cache.ContainsKey(key)) continue;
                    }

                    var skip = (p - 1) * rowCount;
                    var items = await FormRowsPageLoader.LoadPageListAsync(
                        db, reportId, formType, skip, rowCount, cts.Token);
                    lock (gate)
                    {
                        cache[key] = items;
                    }
                }
            }
            catch
            {
                // best-effort
            }
        }, cts.Token);
    }

    #endregion

    #region UpdatePageInfo

    /// <summary>
    /// Обновляет отображаемое количество страниц и строчек.
    /// </summary>
    public void UpdatePageInfo()
    {
        OnPropertyChanged(nameof(TotalPages));
        OnPropertyChanged(nameof(TotalRows));
    }

    #endregion

    #region OnPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}