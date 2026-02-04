
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
using Client_App.Properties;
using Client_App.ViewModels.MainWindowTabs;
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Collections;
using ReactiveUI;
using System.Collections.Generic;
using System.ComponentModel;
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
                _selectedReportType = value;
                OnPropertyChanged();
                UpdateReportsCollection();
                UpdateOrgsPageInfo();
                UpdateFormsPageInfo();
            }
        }
    }
    public string SelectedReportTypeToString => $"{_selectedReportType}.0";

    #endregion

    #region LaunchedAtNORAO
    
#pragma warning disable CA1822

    // ReSharper disable once MemberCanBeMadeStatic.Global
    public bool AppLaunchedAtNorao => Settings.Default.AppLaunchedInNorao;

#pragma warning restore CA1822

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

    #region Forms1TabControlVM

    public Forms1TabControlVM Forms1TabControlVM { get; }

    #endregion

    #region Forms2TabControlVM

    public Forms2TabControlVM Forms2TabControlVM { get; }

    #endregion

    #region Forms4TabControlVM

    public Forms4TabControlVM Forms4TabControlVM { get; }

    #endregion

    #region SelectedReports
    public Reports? SelectedReports
    {
        get
        {
            return SelectedReportType switch
            {
                1 => Forms1TabControlVM.SelectedReports,
                2 => Forms2TabControlVM.SelectedReports,
                4 => Forms4TabControlVM.SelectedReports,
                _ => null
            };
        }
        set
        {
            switch (SelectedReportType)
            {
                case 1:
                    Forms1TabControlVM.SelectedReports = value;
                    OnPropertyChanged();
                    break;
                case 2:
                    Forms2TabControlVM.SelectedReports = value;
                    OnPropertyChanged();
                    break;
                case 4:
                    Forms4TabControlVM.SelectedReports = value;
                    OnPropertyChanged();
                    break;
            }
        }
    }
    #endregion


    #region UpdateReportsCollection
    public void UpdateReportsCollection()
    {
        switch (SelectedReportType)
        {
            case 1:
                Forms1TabControlVM.UpdateReportsCollection();
                break;
            case 2:
                Forms2TabControlVM.UpdateReportsCollection();
                break;
            case 4:
                Forms4TabControlVM.UpdateReportsCollection();
                break;
            default:
                break;
        }
    }
    #endregion

    #region UpdateReportCollection
    public void UpdateReportCollection()
    {
        switch (SelectedReportType)
        {
            case 1:
                Forms1TabControlVM.UpdateReportCollection();
                break;
            case 2:
                Forms2TabControlVM.UpdateReportCollection();
                break;
            case 4:
                Forms4TabControlVM.UpdateReportCollection();
                break;
            default:
                break;
        }
    }
    #endregion

    #region UpdateOrgsPageInfo
    public void UpdateOrgsPageInfo()
    {
        switch (SelectedReportType)
        {
            case 1:
                Forms1TabControlVM.UpdateOrgsPageInfo();
                break;
            case 2:
                Forms2TabControlVM.UpdateOrgsPageInfo();
                break;
            case 4:
                Forms4TabControlVM.UpdateOrgsPageInfo();
                break;
            default:
                break;
        }
    }
    #endregion
    
    #region UpdateFormsPageInfo
    public void UpdateFormsPageInfo()
    {
        switch (SelectedReportType)
        {
            case 1:
                Forms1TabControlVM.UpdateFormsPageInfo();
                break;
            case 2:
                Forms2TabControlVM.UpdateFormsPageInfo();
                break;
            case 4:
                Forms4TabControlVM.UpdateFormsPageInfo();
                break;
            default:
                break;
        }
    }
    #endregion

    #region TotalReportCount
    public void UpdateTotalReportCount()
    {
        switch (SelectedReportType)
        {
            case 1:
                Forms1TabControlVM.UpdateTotalReportCount();
                break;
            case 2:
                Forms2TabControlVM.UpdateTotalReportCount();
                break;
            case 4:
                Forms4TabControlVM.UpdateTotalReportCount();
                break;
            default:
                break;
        }
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
    public ICommand NewChangeForm { get; set; }                     //  Открыть окно редактирования выбранной формы (4.0) (После перерисовки интерфейса будет использоваться и для 1.0, 2.0)
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
    public ICommand ExcelExportAll => new ExcelExportAllAsyncCommand(this);

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
    public ICommand ExcelExportForms => new ExcelExportFormsAsyncCommand(this);

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
    /// Excel -> Проблемные источники по региону
    /// </summary>
    public ICommand ExcelExportLostAndExtraUnitsByRegion => new ExcelExportLostAndExtraUnitsByRegionAsyncCommand();

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
    public ICommand ExcelExportSnk => new ExcelExportSnkAsyncCommand(this);

    /// <summary>
    /// Excel -> Выбранная организация -> Проверка инвентаризаций
    /// </summary>
    public ICommand ExcelExportCheckInventories => new ExcelExportCheckInventoriesAsyncCommand(this);

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

    public ICommand GoToFormNum { get; set; }
    
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
        NewDeleteForm = new NewDeleteFormAsyncCommand();
        DeleteForm = new DeleteReportAsyncCommand();
        DeleteReports = new DeleteReportsAsyncCommand();
        ExcelExportCheckAllForms = new ExcelExportCheckAllFormsAsyncCommand();
        ImportExcel = new ImportExcelAsyncCommand(this);
        ImportJson = new ImportJsonAsyncCommand();
        ImportRaodb = new ImportRaodbAsyncCommand();
        MaxGraphsLength = new MaxGraphsLengthAsyncCommand();
        SaveReports = new SaveReportsAsyncCommand();
        OpenCalculator = new OpenCalculatorAsyncCommand();
        OpenFile = new OpenFileAsyncCommand();
        OpenFolder = new OpenFolderAsyncCommand();
        GoToFormNum = new GoToFormNumAsyncCommand(this);

        Forms1TabControlVM = new Forms1TabControlVM(this);
        Forms2TabControlVM = new Forms2TabControlVM(this);
        Forms4TabControlVM = new Forms4TabControlVM(this);

        UpdateReportsCollection();
    }

    #endregion

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