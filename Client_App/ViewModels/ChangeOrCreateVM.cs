using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Commands.AsyncCommands.Delete;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.PassportFill;
using Client_App.Commands.AsyncCommands.Passports;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.Commands.SyncCommands;
using Models.Attributes;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Client_App.Commands.AsyncCommands.Calculator;

namespace Client_App.ViewModels;

public class ChangeOrCreateVM : BaseVM, INotifyPropertyChanged
{
    private string WindowHeader { get; set; } = "default";

    public event PropertyChangedEventHandler PropertyChanged;
    internal void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #region FormType

    private string _FormType;
    public string FormType
    {
        get => _FormType;
        set
        {
            if (_FormType != value)
            {
                _FormType = value;
                NotifyPropertyChanged("FormType");
            }
        }
    }

    #endregion

    private bool _isAutoReplaceEnabled = true;
    public bool IsAutoReplaceEnabled
    {
        get => _isAutoReplaceEnabled;
        set
        {
            if (_isAutoReplaceEnabled != value)
            {
                _isAutoReplaceEnabled = value;
                Storage.AutoReplace = value;
                NotifyPropertyChanged();
            }
        }
    }

    #region IsCanSaveReportEnabled

    private bool _isCanSaveReportEnabled;

    public bool IsCanSaveReportEnabled
    {
        get => _isCanSaveReportEnabled;
        set
        {
            if (value == _isCanSaveReportEnabled)
            {
                return;
            }
            _isCanSaveReportEnabled = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCanSaveReportEnabled)));
        }
    }

    #endregion

    #region isSum

    private bool _isSum;
    public bool isSum
    {
        get => _isSum;
        set
        {
            if (_isSum != value)
            {
                _isSum = value;
                NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region Storage

    private Report _Storage;
    public Report Storage
    {
        get => _Storage;
        set
        {
            if (_Storage != value)
            {
                _Storage = value;
                NotifyPropertyChanged("Storage");
            }
        }
    }

    #endregion

    #region Storages
    private Reports _Storages;
    public Reports Storages
    {
        get => _Storages;
        set
        {
            if (_Storages != value)
            {
                _Storages = value;
                NotifyPropertyChanged();
            }
        }
    }
    #endregion

    #region LocalReports

    private static DBObservable _localReports = new();
    public static DBObservable LocalReports
    {
        get => _localReports;
        set
        {
            if (_localReports != value)
            {
                _localReports = value;
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
                NotifyPropertyChanged("DBO");
            }
        }
    }
    #endregion

    #region Commands

    public ICommand AddNote { get; set; }                           //  Добавить примечание в форму
    public ICommand AddNotes { get; set; }                          //  Добавить N примечаний в форму
    public ICommand AddRow { get; set; }                            //  Добавить строку в форму
    public ICommand AddRows { get; set; }                           //  Добавить N строк в форму
    public ICommand AddRowsIn { get; set; }                         //  Добавить N строк в форму перед выбранной строкой
    public ICommand CategoryCalculationFromReport { get; set; }     //  Расчёт категории источника ЗРИ
    public ICommand ChangeReportOrder { get; set; }                 //  Поменять местами юр. лицо и обособленное подразделение
    public ICommand CheckReport { get; set; }                       //  Открывает окно проверки текущей формы при нажатии кнопки "Проверить"
    public ICommand CopyExecutorData { get; set; }                  //  Скопировать данные исполнителя из предыдущей формы
    public ICommand CopyPasName { get; set; }                       //  Скопировать в буфер обмена уникальное имя паспорта
    public ICommand CopyRows { get; set; }                          //  Скопировать в буфер обмена уникальное имя паспорта
    public ICommand DeleteDataInRows { get; set; }                  //  Удалить данные в выделенных ячейках
    public ICommand DeleteNote { get; set; }                        //  Удалить выбранный комментарий
    public ICommand DeleteRows { get; set; }                        //  Удалить выбранные строчки из формы
    public ICommand ExcelExportSourceMovementHistory { get; set; }  //  Выгрузка в Excel истории движения источника
    public ICommand OpenPas { get; set; }                           //  Найти и открыть соответствующий файл паспорта в сетевом хранилище
    public ICommand PasteRows { get; set; }                         //  Вставить значения из буфера обмена
    public ICommand SaveReport { get; set; }                        //  Сохранить отчет
    public ICommand SetAutoReplace { get; set; }                     
    public ICommand SetNumberOrder { get; set; }                    //  Выставление порядкового номера
    public ICommand SortForm { get; set; }                          //  Сортировка по порядковому номеру
    public ICommand SourceTransmission { get; set; }                //  Перевод источника из РВ в РАО
    public ICommand SourceTransmissionAll { get; set; }             //  Перевод всех источников в форме из РВ в РАО
    public ICommand PassportFill { get; set; }                      //  Заполнение шаблона паспорта в 1.7 из выделенных строк
    public ICommand PassportFillAll { get; set; }                   //  Заполнения шаблона паспорта в 1.7 из всех строк

    #endregion

    #region Constructor

    #region ChangeFormOrOrg

    public ChangeOrCreateVM(string formNum, in Report rep)
    {
        if (rep.FormNum_DB is "1.0" or "2.0")
        {
            Storage = rep;
        }
        else
        {
            var id = rep.Id;
            while (StaticConfiguration.IsFileLocked(null)) Thread.Sleep(50);
            Task myTask = Task.Factory.StartNew(() => ReportsStorage.GetReportAsync(id, this));  //при открытии формы загружаем все формы из БД
            myTask.Wait();
        }
        
        FormType = formNum;
        LocalReports = ReportsStorage.LocalReports;
        var sumR21 = rep.Rows21.Count(x => x.Sum_DB || x.SumGroup_DB);
        var sumR22 = rep.Rows22.Count(x => x.Sum_DB || x.SumGroup_DB);
        isSum = sumR21 > 0 || sumR22 > 0;
        Init();
        StaticConfiguration.DBModel.SaveChanges();
    }

    #endregion

    #region AddNewReport

    public ChangeOrCreateVM(string formNum, in Reports reps)
    {
        Storage = new Report { FormNum_DB = formNum };

        switch (formNum)
        {
            case "1.0":
            {
                var ty1 = (Form10)FormCreator.Create(formNum);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form10)FormCreator.Create(formNum);
                ty2.NumberInOrder_DB = 2;
                Storage.Rows10.Add(ty1);
                Storage.Rows10.Add(ty2);
                break;
            }
            case "2.0":
            {
                var ty1 = (Form20)FormCreator.Create(formNum);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form20)FormCreator.Create(formNum);
                ty2.NumberInOrder_DB = 2;
                Storage.Rows20.Add(ty1);
                Storage.Rows20.Add(ty2);
                break;
            }
            default:
            {
                if (formNum.StartsWith('1') || formNum.StartsWith('2'))
                {
                    FormType = formNum;

                    Storage.StartPeriod.Value = reps.Report_Collection
                        .Where(x => x.FormNum_DB == formNum && DateOnly.TryParse(x.EndPeriod_DB, out _))
                        .OrderBy(x => DateOnly.Parse(x.EndPeriod_DB))
                        .Select(x => x.EndPeriod_DB)
                        .LastOrDefault() ?? "";
                }
                break;
            }
        }
        Storages = reps;
        FormType = formNum;
        Init();
    }

    #endregion

    #region AddNewOrg
    
    public ChangeOrCreateVM(string formNum, in DBObservable reps)
    {
        Storage = new Report { FormNum_DB = formNum };
        switch (formNum)
        {
            case "1.0":
                {
                    var ty1 = (Form10)FormCreator.Create(formNum);
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form10)FormCreator.Create(formNum);
                    ty2.NumberInOrder_DB = 2;
                    Storage.Rows10.Add(ty1);
                    Storage.Rows10.Add(ty2);
                    break;
                }
            case "2.0":
                {
                    var ty1 = (Form20)FormCreator.Create(formNum);
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form20)FormCreator.Create(formNum);
                    ty2.NumberInOrder_DB = 2;
                    Storage.Rows20.Add(ty1);
                    Storage.Rows20.Add(ty2);
                    break;
                }
        }
        FormType = formNum;
        DBO = reps;
        Init();
    }

    #endregion

    #endregion

    #region Interaction

    public Interaction<int, int> ShowDialogIn { get; protected set; }
    public Interaction<object, int> ShowDialog { get; protected set; }
    public Interaction<List<string>, string> ShowMessageT { get; protected set; }

    #endregion

    private void Init()
    {
        var formNum = FormType.Replace(".", "");
        if (FormType.Split('.')[1] != "0" && FormType.Split('.')[0] is "1" or "2")
        {
            WindowHeader = $"{((Form_ClassAttribute)Type.GetType($"Models.Forms.Form{formNum[0]}.Form{formNum},Models")!.GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name} "
                           + $"{Storages.Master_DB.RegNoRep.Value} "
                           + $"{Storages.Master_DB.ShortJurLicoRep.Value} "
                           + $"{Storages.Master_DB.OkpoRep.Value}";
        }
        else if (FormType is "1.0" or "2.0")
        {
            WindowHeader = ((Form_ClassAttribute)Type.GetType($"Models.Forms.Form{formNum[0]}.Form{formNum},Models")!.GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
        }

        AddNote = new AddNoteAsyncCommand(this);
        AddNotes = new AddNotesAsyncCommand(this);
        AddRow = new AddRowAsyncCommand(this);
        AddRows = new AddRowsAsyncCommand(this);
        AddRowsIn = new AddRowsInAsyncCommand(this);
        CategoryCalculationFromReport = new CategoryCalculationFromReportAsyncCommand();
        ChangeReportOrder = new ChangeReportOrderAsyncCommand(this);
        CheckReport = new CheckFormAsyncCommand(this);
        CopyExecutorData = new CopyExecutorDataAsyncCommand(this);
        CopyPasName = new CopyPasNameAsyncCommand();
        CopyRows = new CopyRowsAsyncCommand();
        DeleteDataInRows = new DeleteDataInRowsSyncCommand();
        DeleteNote = new DeleteNoteAsyncCommand(this);
        DeleteRows = new DeleteRowsAsyncCommand(this);
        ExcelExportSourceMovementHistory = new ExcelExportSourceMovementHistoryAsyncCommand();
        OpenPas = new OpenPasAsyncCommand();
        PasteRows = new PasteRowsAsyncCommand();
        SaveReport = new SaveReportAsyncCommand(this);
        SetAutoReplace = new SetAutoReplaceAsyncCommand(this);
        SetNumberOrder = new SetNumberOrderSyncCommand(this);
        SortForm = new SortFormSyncCommand(this);
        PassportFill = new PassportFillSyncCommand(this);
        PassportFillAll = new PassportFillAllSyncCommand(this);
        ShowDialog = new Interaction<object, int>();
        ShowDialogIn = new Interaction<int, int>();
        ShowMessageT = new Interaction<List<string>, string>();
        if (!isSum)
        {
            //Storage.Sort();
        }
    }
}