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
    public ICommand ExcelExportAll { get; set; }                    //  Excel -> Все формы и Excel -> Выбранная организация -> Все формы
    public ICommand ExcelExportExecutors { get; set; }              //  Excel -> Список исполнителей
    public ICommand ExcelExportFormAnalysis { get; set; }           //  Выбранная форма -> Выгрузка Excel -> Для анализа
    public ICommand ExcelExportFormPrint { get; set; }              //  Выбранная форма -> Выгрузка Excel -> Для печати
    public ICommand ExcelExportForms { get; set; }                  //  Excel -> Формы 1.x, 2.x и Excel -> Выбранная организация -> Формы 1.x, 2.x
    public ICommand ExcelExportIntersections { get; set; }          //  Excel -> Разрывы и пересечения
    public ICommand ExcelExportListOfForms1 { get; set; }           //  Excel -> Список форм 1
    public ICommand ExcelExportListOfForms2 { get; set; }           //  Excel -> Список форм 2
    public ICommand ExcelExportListOfOrgs { get; set; }             //  Excel -> Список организаций
    public ICommand ExcelExportPasWithoutRep { get; set; }          //  Excel -> Паспорта -> Паспорта без отчетов
    public ICommand ExcelExportRepWithoutPas { get; set; }          //  Excel -> Паспорта -> Отчеты без паспортов
    public ICommand ExportAllReports { get; set; }                  //  Экспорт всех организаций организации в отдельные файлы .raodb
    //public ICommand ExportAllReportsOneFile { get; set; }           //  Экспорт всех организаций организации в один файл .raodb
    public static ICommand ExportForm => new ExportFormAsyncCommand();          //  Экспорт формы в файл .raodb
    public static ICommand ExportReports => new ExportReportsAsyncCommand();    //  Экспорт организации в файл .raodb
    public ICommand ExportReportsWithDateRange { get; set; }        //  Экспорт организации в файл .raodb с указанием диапазона дат выгружаемых форм
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
        ExcelExportExecutors = new ExcelExportExecutorsAsyncCommand();
        ExcelExportFormAnalysis = new ExcelExportFormAnalysisAsyncCommand();
        ExcelExportFormPrint = new ExcelExportFormPrintAsyncCommand(); 
        ExcelExportForms = new ExcelExportFormsAsyncCommand();
        ExcelExportIntersections = new ExcelExportIntersectionsAsyncCommand();
        ExcelExportListOfForms1 = new ExcelExportListOfForms1AsyncCommand();
        ExcelExportListOfForms2 = new ExcelExportListOfForms2AsyncCommand();
        ExcelExportListOfOrgs = new ExcelExportListOfOrgsAsyncCommand();
        ExcelExportPasWithoutRep = new ExcelExportPasWithoutRepAsyncCommand();
        ExcelExportRepWithoutPas = new ExcelExportRepWithoutPasAsyncCommand();
        ExcelExportAll = new ExcelExportAllAsyncCommandAsyncCommand();
        ExportAllReports = new ExportAllReportsAsyncCommand();
        //ExportAllReportsOneFile = new ExportAllReportsOneFileAsyncCommand();
        ExportReportsWithDateRange = new ExportReportsWithDateRangeAsyncCommand();
        ImportExcel = new ImportExcelAsyncCommand();
        ImportJson = new ImportJsonAsyncCommand();
        ImportRaodb = new ImportRaodbAsyncCommand();
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