using Models.Collections;
using ReactiveUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.Delete;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.ExcelExport.Passports;
using Client_App.Commands.AsyncCommands.Import;
using Client_App.Commands.AsyncCommands.Import.ImportJson;
using Client_App.Commands.AsyncCommands.Passports;
using Client_App.Commands.AsyncCommands.RaodbExport;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Commands.AsyncCommands.Hidden;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk;

namespace Client_App.ViewModels;

public class MainWindowVM : BaseVM, INotifyPropertyChanged
{
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

    public ICommand AddForm { get; set; }                           //  Создать и открыть новое окно формы для выбранной организации
    public ICommand AddReports { get; set; }                        //  Создать и открыть новое окно формы организации (1.0 и 2.0)
    public ICommand ChangeForm { get; set; }                        //  Открыть окно редактирования выбранной формы
    public ICommand ChangePasFolder { get; set; }                   //  Excel -> Паспорта -> Изменить расположение паспортов по умолчанию
    public ICommand ChangeReports { get; set; }                     //  Изменить Формы организации (1.0 и 2.0)
    public ICommand ExcelExportCheckAllForms { get; set; }          //  Проверить все формы у организации
    public ICommand CheckFormFromMain { get; set; }                 //  Проверить форму
    public ICommand DeleteForm { get; set; }                        //  Удалить выбранную форму у выбранной организации
    public ICommand DeleteReports { get; set; }                     //  Удалить выбранную организацию

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
    /// Excel -> Проверка наличия формы 2.2.
    /// </summary>
    public ICommand ExcelExportBalance22 => new ExcelExportBalance22AsyncCommand();

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
    public static ICommand ExportReports => new ExportReportsAsyncCommand();

    /// <summary>
    /// Экспорт организации в файл .RAODB с указанием диапазона дат выгружаемых форм
    /// </summary>
    public static ICommand ExportReportsWithDateRange => new ExportReportsWithDateRangeAsyncCommand();
    public ICommand ImportExcel { get; set; }                       //  Импорт -> Из Excel
    public ICommand ImportJson { get; set; }                        //  Импорт -> Из Json
    public ICommand ImportRaodb { get; set; }                       //  Импорт -> Из RAODB
    public ICommand MaxGraphsLength { get; set; }                   //  Excel -> Максимальное число символов в каждой колонке
    public ICommand SaveReports { get; set; }                       //  Сохраняет текущую базу, используется только для сохранения комментария формы
    //public ICommand UnaccountedRad { get; set; }                    //  Радионуклиды, отсутствующие в справочнике

    #endregion

    #region Constructor

    public MainWindowVM()
    {
        AddForm = new AddFormAsyncCommand();
        AddReports = new AddReportsAsyncCommand();
        ChangeForm = new ChangeFormAsyncCommand();
        ChangePasFolder = new ChangePasFolderAsyncCommand();
        ChangeReports = new ChangeReportsAsyncCommand();
        CheckFormFromMain = new CheckFormFromMainAsyncCommand();
        DeleteForm = new DeleteFormAsyncCommand();
        DeleteReports = new DeleteReportsAsyncCommand();
        ExcelExportCheckAllForms = new ExcelExportCheckAllFormsAsyncCommand();
        ImportExcel = new ImportExcelAsyncCommand();
        ImportJson = new ImportJsonAsyncCommand();
        ImportRaodb = new ImportRaodbAsyncCommand(this);
        MaxGraphsLength = new MaxGraphsLengthAsyncCommand();
        SaveReports = new SaveReportsAsyncCommand();
        //UnaccountedRad = new UnaccountedRadAsyncCommand();
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