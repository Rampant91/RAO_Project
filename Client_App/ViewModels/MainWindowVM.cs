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
using Client_App.Services;
using Client_App.Services.DataAccess;
using Client_App.ViewModels.MainWindowTabs;
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Collections;
using Models.DBRealization;
using ReactiveUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ExcelExportCheckPairingOfCode41AsyncCommand = Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.ExcelExportCheckPairingOfCode41AsyncCommand;
using ExcelExportCheckTransferReceiveAsyncCommand = Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;
using ExcelExportCompareFormPrintAsyncCommand = Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare.ExcelExportCompareFormPrintAsyncCommand;

namespace Client_App.ViewModels;

public class MainWindowVM : ObservableObject, INotifyPropertyChanged
{
    #region SelectedReportType

    private byte _selectedReportType = 1;
    private object _selectedTabContent;

    public byte SelectedReportType
    {
        get => _selectedReportType;
        set
        {
            if (_selectedReportType != value)
            {
                _selectedReportType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedReports));
                OnPropertyChanged(nameof(IsExcelSelectedOrganizationMenuEnabled));

                GetTabVm(value)?.ActivateTab();

                if (!string.IsNullOrEmpty(StaticConfiguration.DBPath))
                {
                    MainWindowPrefetchService.Instance.ScheduleWarmInactiveTabs(
                        value,
                        StaticConfiguration.DBPath,
                        MainWindowPagingDefaults.DefaultOrgsPerPage,
                        MainWindowPagingDefaults.DefaultFormsPerPage);
                }
            }
        }
    }

    private FormsTabControlBaseVM? GetTabVm(byte formTab) => formTab switch
    {
        1 => Forms1TabControlVM,
        2 => Forms2TabControlVM,
        4 => Forms4TabControlVM,
        5 => Forms5TabControlVM,
        _ => null
    };

    /// <summary>
    /// После SaveReport: обновить счётчик строк выбранного отчёта без полного reload org-списка.
    /// </summary>
    public void RefreshAfterFormReportSaved(string formType, int? reportsId, int reportId)
    {
        var tabKey = formType?.Length > 0 && char.IsDigit(formType[0])
            ? (byte)(formType[0] - '0')
            : SelectedReportType;
        GetTabVm(tabKey)?.RefreshAfterFormReportSaved(reportsId, reportId);
    }

    public string SelectedReportTypeToString => $"{_selectedReportType}.0";

    #endregion

    #region LaunchedAtNORAO

    private bool _appLaunchedAtNorao;
    public bool AppLaunchedAtNorao
    {
        get => _appLaunchedAtNorao;
        set
        {
            if (SetProperty(ref _appLaunchedAtNorao, value))
            {
                // Сохраняем в настройки при изменении
                Settings.Default.AppLaunchedInNorao = value;
                Settings.Default.Save();

                OnPropertyChanged();
                OnPropertyChanged(nameof(CanRollbackPreviousRelease));
            }
        }
    }

    private bool _developerModeEverEnabled;
    public bool DeveloperModeEverEnabled
    {
        get => _developerModeEverEnabled;
        private set => SetProperty(ref _developerModeEverEnabled, value);
    }

    /// <summary>
    /// Доступен откат на previous (только режим отдела и есть бэкап).
    /// </summary>
    public bool CanRollbackPreviousRelease =>
        AppLaunchedAtNorao && _updateService.CanRollback();

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

    #region Forms5TabControlVM
    public Forms5TabControlVM Forms5TabControlVM{ get; }

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
                5 => Forms5TabControlVM.SelectedReports,
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
                    OnPropertyChanged(nameof(IsExcelSelectedOrganizationMenuEnabled));
                    break;
                case 2:
                    Forms2TabControlVM.SelectedReports = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsExcelSelectedOrganizationMenuEnabled));
                    break;
                case 4:
                    Forms4TabControlVM.SelectedReports = value;
                    OnPropertyChanged();
                    break;
                case 5:
                    Forms5TabControlVM.SelectedReports = value;
                    OnPropertyChanged();
                    break;
            }
        }
    }

    /// <summary>
    /// Пункт меню «Аналитика → Выбранная организация»: доступен на вкладках «Формы 1» / «Формы 2»
    /// при выбранной строке в списке организаций.
    /// </summary>
    public bool IsExcelSelectedOrganizationMenuEnabled =>
        (SelectedReportType == 1 || SelectedReportType == 2) && SelectedReports is not null;

    #endregion

    #region UpdateReportsCollection
    public void UpdateReportsCollection() =>
        GetTabVm(SelectedReportType)?.UpdateReportsCollection();
    #endregion

    #region UpdateReportCollection
    public void UpdateReportCollection() =>
        GetTabVm(SelectedReportType)?.UpdateReportCollection();
    #endregion

    #region UpdateOrgsPageInfo
    public void UpdateOrgsPageInfo()
    {
        GetTabVm(SelectedReportType)?.UpdateOrgsPageInfo();
    }
    #endregion
    
    #region UpdateFormsPageInfo
    public void UpdateFormsPageInfo() =>
        GetTabVm(SelectedReportType)?.UpdateFormsPageInfo();
    #endregion

    public void UpdateTotalReportCount() =>
        GetTabVm(SelectedReportType)?.UpdateTotalReportCount();

    public void UpdateTotalReportsCount() =>
        GetTabVm(SelectedReportType)?.UpdateTotalReportsCount();

    /// <summary>
    /// После фоновой санитизации титулов — обновить org-гриды всех вкладок 1/2/4/5.
    /// </summary>
    public void RefreshAllTabsAfterTitleSanitizer()
    {
        Forms1TabControlVM.UpdateOrgsPageInfo();
        Forms2TabControlVM.UpdateOrgsPageInfo();
        Forms4TabControlVM.UpdateOrgsPageInfo();
        Forms5TabControlVM.UpdateOrgsPageInfo();
    }

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

    private readonly UpdateService _updateService;

    public ICommand ChangeForm { get; set; }                        //  Редактировать выбранный отчёт (для старых форм 2.х)
    public ICommand AddReports { get; set; }                        //  Создать и открыть новое окно формы организации (1.0, 2.0, 4.0)
    public ICommand ChangePasFolder { get; set; }                   //  Excel -> Паспорта -> Изменить расположение паспортов по умолчанию
    public ICommand ConvertExcelToRaodb { get; set; }               //  Дополнительно -> Конвертер из Excel в .RAODB
    public ICommand ExcelExportCheckAllForms { get; set; }          //  Проверить все формы у организации
    public ICommand ExcelExportCheckPairingOfCode41 { get; set; }   //  Непарные операции 41 (org / вся БД, формы 1.1–1.6)
    public ICommand ExcelExportCheckTransferReceive { get; set; }   //  Проверка операций приёма-передачи (org / вся БД, формы 1.1–1.5)
    public ICommand ExcelExportCompareFormPrint { get; set; }       //  Сравнение отчётов (режим разработчика)
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
    /// Excel -> Проверка последней инвентаризации.
    /// </summary>
    public ICommand ExcelExportCheckLastInventoryDate => new ExcelExportCheckLastInventoryDateAsyncCommand();


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
    /// Excel -> Список форм 4
    /// </summary>
    public ICommand ExcelExportListOfForms4 => new ExcelExportListOfForms4AsyncCommand();

    /// <summary>
    /// Excel -> Список форм 5
    /// </summary>
    public ICommand ExcelExportListOfForms5 => new ExcelExportListOfForms5AsyncCommand();

    /// <summary>
    /// Excel -> Списки форм 1, 2, 4 и 5 в одном файле
    /// </summary>
    public ICommand ExcelExportListOfFormsAll => new ExcelExportListOfFormsAllAsyncCommand();

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
    /// Экспорт всех организаций организации в отдельные файлы .RAODB
    /// </summary>
    public ICommand ExportAllReport => new ExportAllReportAsyncCommand();

    /// <summary>
    /// Групповая выгрузка отчётов форм 1.1–1.9 по списку организаций из .xlsx
    /// </summary>
    public ICommand GroupBulkExportReports => new GroupBulkExportReportsAsyncCommand();

    /// <summary>
    /// Выгрузка всех отчётов указанной формы (1.1-1.9, 2.1-2.12) или всех форм группы (all-1, all-2)
    /// организации в отдельные .xlsx файлы
    /// </summary>
    public ICommand ExcelExportAllFormsByFormNumber => new ExcelExportAllFormsByFormNumberAsyncCommand(this);

    /// <summary>
    /// Экспорт всех организаций организации в один файл .RAODB
    /// </summary>
    public ICommand ExportAllReportsOneFile => new ExportAllReportsOneFileAsyncCommand();

    /// <summary>
    /// Экспорт всех организаций организации в один файл .RAODB
    /// </summary>
    public ICommand ExportAllReportsFromSubjectRFOneFile => new ExportAllReportsFromSubjectRFOneFileAsyncCommand();


    /// <summary>
    /// Экспорт организации в файл .RAODB
    /// </summary>
    public ICommand ExportReports => new ExportReportsAsyncCommand(this);

    /// <summary>
    /// Экспорт организации в файл .RAODB с указанием диапазона дат выгружаемых форм
    /// </summary>
    public ICommand ExportReportsWithDateRange => new ExportReportsWithDateRangeAsyncCommand(this);

    /// <summary>
    /// Импорт отчёта из Excel.
    /// </summary>
    public ICommand ImportExcel { get; set; }                               //  Импорт -> Из Excel
    public ICommand ImportJson { get; set; }                                //  Импорт -> Из Json
    public ICommand ImportRaodb { get; set; }                               //  Импорт -> Из RAODB
    public ICommand MaxGraphsLength { get; set; }                           //  Excel -> Максимальное число символов в каждой колонке
    public ICommand OpenCalculator { get; set; }                            //  Открыть калькулятор пересчёта активности
    public ICommand CheckForUpdates { get; set; }                           //  Сервис -> Проверить обновления
    public ICommand RollbackPreviousRelease { get; set; }                   //  Сервис -> Откат (отдел)
    public ICommand OpenFile { get; set; }                                  //  Открыть файл
    public ICommand OpenFolder { get; set; }                                //  Открыть папку
    public ICommand SaveReports { get; set; }                               //  Сохраняет текущую базу, используется только для сохранения комментария формы
                                                                            //public ICommand UnaccountedRad { get; set; }                    
                                                                            //  Радионуклиды, отсутствующие в справочнике

    public ICommand OpenPassportMenu { get; set; }
    public ICommand OpenStoragePointMenu { get; set; }
    public ICommand SetWhiteList { get; set; }

    public ICommand CalculateVolumesRaoProcessingByPeriod { get; set; }

    #endregion

    #region Constructor

    public MainWindowVM()
    {
        _updateService = new UpdateService();
        CheckForUpdates = new CheckForUpdatesAsyncCommand(_updateService, () => AppLaunchedAtNorao);
        RollbackPreviousRelease = new RollbackPreviousReleaseAsyncCommand(_updateService);

        AddReports = new AddReportsAsyncCommand();
        ChangeForm = new ChangeFormAsyncCommand();
        ChangePasFolder = new ChangePasFolderAsyncCommand();
        ConvertExcelToRaodb = new ConvertExcelToRaodbAsyncCommand();
        DeleteReports = new DeleteReportsAsyncCommand(this);
        ExcelExportCheckAllForms = new ExcelExportCheckAllFormsAsyncCommand(this);
        ExcelExportCheckPairingOfCode41 = new ExcelExportCheckPairingOfCode41AsyncCommand(this);
        ExcelExportCheckTransferReceive = new ExcelExportCheckTransferReceiveAsyncCommand(this);
        ExcelExportCompareFormPrint = new ExcelExportCompareFormPrintAsyncCommand(this);
        ImportExcel = new ImportExcelAsyncCommand();
        ImportJson = new ImportJsonAsyncCommand();
        ImportRaodb = new ImportRaodbAsyncCommand();
        MaxGraphsLength = new MaxGraphsLengthAsyncCommand();
        SaveReports = new SaveReportsAsyncCommand();
        OpenCalculator = new OpenCalculatorAsyncCommand();
        OpenFile = new OpenFileAsyncCommand();
        OpenFolder = new OpenFolderAsyncCommand();
        OpenPassportMenu = new OpenPassportMenuWindowAsyncCommand();
        OpenStoragePointMenu = new OpenStoragePointMenuWindowAsyncCommand();
        SetWhiteList = new SetWhiteListNumAsyncCommand(this);
        CalculateVolumesRaoProcessingByPeriod = new CalculateVolumesRaoProcessingByPeriodAsyncCommand();

        Forms1TabControlVM = new Forms1TabControlVM(this);
        Forms2TabControlVM = new Forms2TabControlVM(this);
        Forms4TabControlVM = new Forms4TabControlVM(this);
        Forms5TabControlVM = new Forms5TabControlVM(this);

        Forms1TabControlVM.PropertyChanged += OnForms1Or2TabSelectedReportsChanged;
        Forms2TabControlVM.PropertyChanged += OnForms1Or2TabSelectedReportsChanged;

        //UpdateReportsCollection();

        _appLaunchedAtNorao = Settings.Default.AppLaunchedInNorao;
        _developerModeEverEnabled = _appLaunchedAtNorao;

        OnPropertyChanged(nameof(AppLaunchedAtNorao));
        OnPropertyChanged(nameof(DeveloperModeEverEnabled));
        OnPropertyChanged(nameof(CanRollbackPreviousRelease));

        // Блокируем конструктор до завершения проверки (сайт или сетевая шара для отдела)
        _updateService.CheckAndNotifyAsync(AppLaunchedAtNorao).Wait();
    }

    #endregion

    private void OnForms1Or2TabSelectedReportsChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(FormsTabControlBaseVM.SelectedReports))
            return;

        OnPropertyChanged(nameof(SelectedReports));
        OnPropertyChanged(nameof(IsExcelSelectedOrganizationMenuEnabled));
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