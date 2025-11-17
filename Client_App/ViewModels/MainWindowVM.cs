
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.Calculator;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Commands.AsyncCommands.Delete;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;
using Client_App.Commands.AsyncCommands.ExcelExport.Passports;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk;
using Client_App.Commands.AsyncCommands.Hidden;
using Client_App.Commands.AsyncCommands.Import;
using Client_App.Commands.AsyncCommands.Import.ImportJson;
using Client_App.Commands.AsyncCommands.Passports;
using Client_App.Commands.AsyncCommands.RaodbExport;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Client_App.Views.Forms.Forms4;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData.Binding;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
namespace Client_App.ViewModels;

public class MainWindowVM : ObservableObject, INotifyPropertyChanged
{
    #region SelectedReportType

    private byte _selectedReportType = 1;

    public byte SelectedReportType
    {
        get => _selectedReportType;
        set
        {
            if (_selectedReportType != value)
            {
                SelectedReports = null; // узко специализрованное решение: очищает выбранную организацию при переключении на другую панель
                _selectedReportType = (byte)(value);
                OnPropertyChanged();
                UpdateReports();
                UpdatePageInfo();
            }
        }
    }
    public string SelectedReportTypeToString => $"{_selectedReportType}.0";

    #endregion

    #region Current_Db

    private string _current_Db = "";

    public string Current_Db
    {
        get => _current_Db;
        set
        {
            if (_current_Db != value)
            {
                _current_Db = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region LocalReports

    private static DBObservable LocalReports
    {
        get => ReportsStorage.LocalReports;
        set
        {
            if (ReportsStorage.LocalReports != value)
            {
                ReportsStorage.LocalReports = value;
            }
            
        }
    }

    #endregion

    #region Reports40

    public ObservableCollection<Reports> Reports40 => ReportsStorage.LocalReports.Reports_Collection40;

    #endregion

    #region SearchText
    private string _searchText = "";

    public string SearchText
    {
        get
        {
            return _searchText;
        }
        set
        {
            _searchText = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ReportsCollection));
        }
    }
    #endregion
    public ObservableCollection<Reports> ReportsCollection
    {
        get
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                var search = SearchText.ToLower().Trim();
                return new ObservableCollection<Reports>(Reports40
                .Where(reps => reps.Master_DB.Rows40[0].CodeSubjectRF_DB.ToString().Contains(search)
                || reps.Master_DB.Rows40[0].SubjectRF_DB.ToLower().Contains(search)
                ||(!string.IsNullOrEmpty(reps.Master_DB.Rows40[0].ShortNameOrganUprav_DB)
                   && reps.Master_DB.Rows40[0].ShortNameOrganUprav_DB.ToLower().Contains(search))
                  )
                .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                .Take(RowsCountOrgs));
            }
            else
                return new ObservableCollection<Reports>(Reports40
                .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                .Take(RowsCountOrgs)); 
        }
    }
    #region SelectedReports

    private Reports? _selectedReports;
    public Reports? SelectedReports
    {
        get
        {
            return _selectedReports;
        }
        set
        {
            _selectedReports = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ReportCollection));
            UpdatePageInfo();
        }
    }
    #endregion
    public int TotalPagesOrgs
    {
        get
        {
            var result = TotalRowsOrgs / RowsCountOrgs;
            if (TotalRowsOrgs % RowsCountOrgs > 0)
                result++;
            return result;
        }
    }
    public int TotalRowsOrgs
    {
        get
        {
            return Reports40.Count;
        }
    }
    private int _rowsCountOrgs = 10;
    public int RowsCountOrgs
    {
        get
        {
            return _rowsCountOrgs;
        }
        set
        {
            _rowsCountOrgs = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ReportsCollection));
            OnPropertyChanged(nameof(TotalPagesOrgs));
        }
    }
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
        }
    }
    public int TotalPagesForms
    {
        get
        {
            var result = TotalRowsForms / RowsCountForms;
            if (TotalRowsForms % RowsCountForms > 0)
                result++;
            return result;
        }
    }
    public int TotalRowsForms
    {
        get
        {
            if (SelectedReports!= null)
                return SelectedReports.Report_Collection.Count;
            return 0;
        }
    }
    private int _rowsCountForms = 10;
    public int RowsCountForms
    {
        get
        {
            return _rowsCountForms;
        }
        set
        {
            _rowsCountForms = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TotalPagesForms));
        }
    }
    private int _currentPageForms = 1;
    public int CurrentPageForms
    {
        get
        {
            if (_currentPageForms > TotalPagesForms)
                _currentPageForms = TotalPagesForms;

            return _currentPageForms;
        }
        set
        {
            _currentPageForms = value;
            OnPropertyChanged();
        }
    }
    public int TotalForms
    {
        get
        {

            var result = StaticConfiguration.DBModel.ReportCollectionDbSet
                .Where(rep => rep.FormNum_DB.StartsWith($"{SelectedReportType}")
                    && !rep.FormNum_DB.EndsWith(".0"))
                .CountAsync().Result;
            return result;
        }
    }

    public int NumFormInReport
    {
        get
        {
            if (SelectedReport!=null)
                return ReportsStorage.GetReportRowsCount(SelectedReport).Result;
            return 0;
        }
    }

    #region ReportCollection

    public ObservableCollection<Report> ReportCollection
    {
        get
        {
            if (SelectedReports is null) return null;

            CurrentPageForms = 1;
            return new ObservableCollection<Report>(
                SelectedReports
                .Report_Collection
                .Skip((CurrentPageForms - 1) * RowsCountForms)
                .Take(RowsCountForms));
        }
    }

    #endregion

    #region SelectedReport

    private Report? _selectedReport;
    public Report? SelectedReport
    {
        get
        {
            return _selectedReport;
        }
        set
        {
            _selectedReport = value;
            OnPropertyChanged();
            UpdatePageInfo();
        }
    }

    #endregion

    #region UpdateReports
    public void UpdateReports()
    {
        OnPropertyChanged(nameof(ReportsCollection));
    }

    #endregion

    #region UpdateReport
    public void UpdateReport()
    {
        OnPropertyChanged(nameof(ReportCollection));
    }

    #endregion

    #region OnStartProgressBar

    private double _OnStartProgressBar;

    public double OnStartProgressBar
    {
        get => _OnStartProgressBar;
        set
        {
            if (_OnStartProgressBar.Equals(value)) return;
            _OnStartProgressBar = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Commands

    public ICommand AddForm { get; set; }                           //  Создать и открыть новое окно формы для выбранной организации (1.0, 2.0)
    public ICommand NewAddForm { get; set; }                        //  Создать и открыть новое окно формы для выбранной организации (4.0) (После перерисовки интерфейса будет использоваться и для 1.0, 2.0)
    public ICommand AddReports { get; set; }                        //  Создать и открыть новое окно формы организации (1.0, 2.0, 4.0)
    public ICommand ChangeForm { get; set; }                        //  Открыть окно редактирования выбранной формы (1.0, 2.0)
    public ICommand NewChangeForm { get; set; }                        //  Открыть окно редактирования выбранной формы (4.0) (После перерисовки интерфейса будет использоваться и для 1.0, 2.0)
    public ICommand ChangePasFolder { get; set; }                   //  Excel -> Паспорта -> Изменить расположение паспортов по умолчанию
    public ICommand ChangeReports { get; set; }                     //  Изменить Формы организации (1.0 и 2.0)
    public ICommand NewChangeReports { get; set; }                  //  Изменить Формы организации (4.0) (После перерисовки интерфейса будет использоваться и для 1.0, 2.0)
    public ICommand ExcelExportCheckAllForms { get; set; }          //  Проверить все формы у организации
    public ICommand CheckFormFromMain { get; set; }                 //  Проверить форму
    public ICommand DeleteForm { get; set; }                        //  Удалить выбранную форму у выбранной организации (1.0, 2.0)
    public ICommand NewDeleteForm { get; set; }                     //  Удалить выбранную форму у выбранной организации  (4.0) (После перерисовки интерфейса будет использоваться и для 1.0, 2.0)
    public ICommand DeleteReports { get; set; }                     //  Удалить выбранную организацию (1.0, 2.0, 4.0)

    /// <summary>
    /// Excel -> Все формы и Excel -> Выбранная организация -> Все формы
    /// </summary>
    public ICommand ExcelExportAll => new ExcelExportAllAsyncCommand();

    /// <summary>
    /// Excel -> Список исполнителей
    /// </summary>
    public ICommand ExcelExportExecutors => new ExcelExportExecutorsAsyncCommand();

    /// <summary>
    /// Экспорт формы в файл .RAODB
    /// </summary>
    public ICommand ExportForm => new ExportFormAsyncCommand();

    /// <summary>
    /// Excel -> Проверка последней инвентаризации.
    /// </summary>
    public ICommand ExcelExportCheckLastInventoryDate => new ExcelExportCheckLastInventoryDateAsyncCommand();

    /// <summary>
    /// Выбранная форма -> Выгрузка Excel -> Для анализа
    /// </summary>
    public ICommand ExcelExportFormAnalysis => new ExcelExportFormAnalysisAsyncCommand();

    /// <summary>
    /// Выбранная форма -> Выгрузка Excel -> Для печати
    /// </summary>
    public ICommand ExcelExportFormPrint => new ExcelExportFormPrintAsyncCommand();

    /// <summary>
    /// Excel -> Формы 1.x, 2.x и Excel -> Выбранная организация -> Формы 1.x, 2.x
    /// </summary>
    public ICommand ExcelExportForms => new ExcelExportFormsAsyncCommand();

    /// <summary>
    /// Excel -> Разрывы и пересечения
    /// </summary>
    public ICommand ExcelExportIntersections => new ExcelExportIntersectionsAsyncCommand();

    /// <summary>
    /// Excel -> Список форм 1
    /// </summary>
    public ICommand ExcelExportListOfForms1 => new ExcelExportListOfForms1AsyncCommand();

    /// <summary>
    /// Excel -> Список форм 2
    /// </summary>
    public ICommand ExcelExportListOfForms2 => new ExcelExportListOfForms2AsyncCommand();

    /// <summary>
    /// Excel -> Список организаций
    /// </summary>
    public ICommand ExcelExportListOfOrgs => new ExcelExportListOfOrgsAsyncCommand();

    /// <summary>
    /// Excel -> Паспорта -> Паспорта без отчетов
    /// </summary>
    public ICommand ExcelExportPasWithoutRep => new ExcelExportPasWithoutRepAsyncCommand();

    /// <summary>
    /// Excel -> Паспорта -> Отчеты без паспортов
    /// </summary>
    public ICommand ExcelExportRepWithoutPas => new ExcelExportRepWithoutPasAsyncCommand();

    /// <summary>
    /// Excel -> Выбранная организация -> СНК
    /// </summary>
    public ICommand ExcelExportSnk => new ExcelExportSnkAsyncCommand();

    /// <summary>
    /// Excel -> Выбранная организация -> Проверка инвентаризаций
    /// </summary>
    public ICommand ExcelExportCheckInventories => new ExcelExportCheckInventoriesAsyncCommand();

    /// <summary>
    /// Экспорт всех организаций организации в отдельные файлы .RAODB
    /// </summary>
    public ICommand ExportAllReports => new ExportAllReportsAsyncCommand();

    /// <summary>
    /// Экспорт всех организаций организации в один файл .RAODB
    /// </summary>
    public ICommand ExportAllReportsOneFile => new ExportAllReportsOneFileAsyncCommand();

    /// <summary>
    /// Экспорт организации в файл .RAODB
    /// </summary>
    public ICommand ExportReports => new ExportReportsAsyncCommand();

    /// <summary>
    /// Экспорт организации в файл .RAODB с указанием диапазона дат выгружаемых форм
    /// </summary>
    public ICommand ExportReportsWithDateRange => new ExportReportsWithDateRangeAsyncCommand();

    /// <summary>
    /// Импорт отчёта из Excel.
    /// </summary>
    public ICommand ImportExcel { get; set; }
    public ICommand ImportJson { get; set; }                                //  Импорт -> Из Json
    public ICommand ImportRaodb { get; set; }                               //  Импорт -> Из RAODB
    public ICommand MaxGraphsLength { get; set; }                           //  Excel -> Максимальное число символов в каждой колонке
    public ICommand OpenCalculator { get; set; }                            //  Открыть калькулятор пересчёта активности
    public ICommand OpenFile { get; set; }                                  //  Открыть файл
    public ICommand OpenFolder { get; set; }                                //  Открыть папку
    public ICommand SaveReports { get; set; }                               //  Сохраняет текущую базу, используется только для сохранения комментария формы
                                                                            //public ICommand UnaccountedRad { get; set; }                    
                                                                            //  Радионуклиды, отсутствующие в справочнике


    #endregion

    #region Constructor

    public MainWindowVM()
    {
        AddForm = new AddFormAsyncCommand();
        NewAddForm = new NewAddFormAsyncCommand();
        AddReports = new AddReportsAsyncCommand();
        ChangeForm = new ChangeFormAsyncCommand();
        NewChangeForm = new NewChangeFormAsyncCommand();
        ChangePasFolder = new ChangePasFolderAsyncCommand();
        ChangeReports = new ChangeReportsAsyncCommand();
        NewChangeReports = new NewChangeReportsAsyncCommand();
        CheckFormFromMain = new CheckFormFromMainAsyncCommand();
        DeleteForm = new DeleteFormAsyncCommand();
        NewDeleteForm = new NewDeleteFormAsyncCommand();
        DeleteReports = new DeleteReportsAsyncCommand();
        ExcelExportCheckAllForms = new ExcelExportCheckAllFormsAsyncCommand();
        ImportExcel = new ImportExcelAsyncCommand();
        ImportJson = new ImportJsonAsyncCommand();
        ImportRaodb = new ImportRaodbAsyncCommand(this);
        MaxGraphsLength = new MaxGraphsLengthAsyncCommand();
        SaveReports = new SaveReportsAsyncCommand();
        //UnaccountedRad = new UnaccountedRadAsyncCommand(); 
        OpenCalculator = new OpenCalculatorAsyncCommand();
        OpenFile = new OpenFileAsyncCommand();
        OpenFolder = new OpenFolderAsyncCommand();

        UpdateReports();
    }

    #endregion
    public void UpdatePageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsOrgs));
        OnPropertyChanged(nameof(TotalPagesOrgs));

        OnPropertyChanged(nameof(TotalRowsForms));
        OnPropertyChanged(nameof(TotalPagesForms));

        OnPropertyChanged(nameof(TotalForms));
        OnPropertyChanged(nameof(NumFormInReport));
    }

    #region Interactions

    public static Interaction<ChangeOrCreateVM, object> ShowDialog  { get; } = new();
    public static Interaction<List<string>, string> ShowMessage { get; } = new();

    #endregion

    #region INotifyPropertyChanged

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
}