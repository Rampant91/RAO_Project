using Models.Attributes;
using Models.Collections;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.Delete;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.Passports;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Commands.SyncCommands;

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
                NotifyPropertyChanged("isSum");
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
                NotifyPropertyChanged("Storages");
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
    public ICommand ChangeReportOrder { get; set; }                 //  Поменять местами юр. лицо и обособленное подразделение
    public ICommand CheckReport { get; set; }                       //  Бесполезная команда, ничего не делает, активируется при нажатии кнопки "Проверить"
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
    public ICommand SetNumberOrder { get; set; }                    //  Выставление порядкового номера

    #endregion

    #region Constructor

    //  При изменении формы или организации
    public ChangeOrCreateVM(string param, in Report rep, Reports reps, DBObservable localReports)
    {
        Storage = rep;
        Storages = reps;
        FormType = param;
        LocalReports = localReports;
        var sumR21 = rep.Rows21.Count(x => x.Sum_DB || x.SumGroup_DB);
        var sumR22 = rep.Rows22.Count(x => x.Sum_DB || x.SumGroup_DB);
        isSum = sumR21 > 0 || sumR22 > 0;
        Init();
    }

    //  При добавлении новой формы
    public ChangeOrCreateVM(string param, in Reports reps)
    {
        Storage = new Report { FormNum_DB = param };

        switch (param)
        {
            case "1.0":
            {
                var ty1 = (Form10)FormCreator.Create(param);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form10)FormCreator.Create(param);
                ty2.NumberInOrder_DB = 2;
                Storage.Rows10.Add(ty1);
                Storage.Rows10.Add(ty2);
                break;
            }
            case "2.0":
            {
                var ty1 = (Form20)FormCreator.Create(param);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form20)FormCreator.Create(param);
                ty2.NumberInOrder_DB = 2;
                Storage.Rows20.Add(ty1);
                Storage.Rows20.Add(ty2);
                break;
            }
            default:
            {
                if (param.StartsWith('1') || param.StartsWith('2'))
                {
                    try
                    {
                        var ty = reps.Report_Collection
                            .Where(t => t.FormNum_DB == param && t.EndPeriod_DB != "")
                            .OrderBy(t => DateTimeOffset.Parse(t.EndPeriod_DB))
                            .Select(t => t.EndPeriod_DB)
                            .LastOrDefault();
                        FormType = param;
                        Storage.StartPeriod.Value = ty;
                    }
                    catch
                    {
                        // ignored
                    }
                }
                break;
            }
        }
        Storages = reps;
        FormType = param;
        Init();
    }

    //  При добавлении новой организации
    public ChangeOrCreateVM(string param, in DBObservable reps)
    {
        Storage = new Report { FormNum_DB = param };
        switch (param)
        {
            case "1.0":
            {
                var ty1 = (Form10)FormCreator.Create(param);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form10)FormCreator.Create(param);
                ty2.NumberInOrder_DB = 2;
                Storage.Rows10.Add(ty1);
                Storage.Rows10.Add(ty2);
                break;
            }
            case "2.0":
            {
                var ty1 = (Form20)FormCreator.Create(param);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form20)FormCreator.Create(param);
                ty2.NumberInOrder_DB = 2;
                Storage.Rows20.Add(ty1);
                Storage.Rows20.Add(ty2);
                break;
            }
        }
        FormType = param;
        DBO = reps;
        Init();
    }

    #endregion

    #region Interaction
    public Interaction<int, int> ShowDialogIn { get; protected set; }
    public Interaction<object, int> ShowDialog { get; protected set; }
    public Interaction<List<string>, string> ShowMessageT { get; protected set; }
    #endregion

    private void Init()
    {
        var a = FormType.Replace(".", "");
        if (FormType.Split('.')[1] != "0" && FormType.Split('.')[0] is "1" or "2")
        {
            WindowHeader = $"{((Form_ClassAttribute)Type.GetType($"Models.Forms.Form{a[0]}.Form{a},Models")!.GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name} "
                           + $"{Storages.Master_DB.RegNoRep.Value} "
                           + $"{Storages.Master_DB.ShortJurLicoRep.Value} "
                           + $"{Storages.Master_DB.OkpoRep.Value}";
        }
        else if (FormType is "1.0" or "2.0")
        {
            WindowHeader = ((Form_ClassAttribute)Type.GetType($"Models.Forms.Form{a[0]}.Form{a},Models")!.GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
        }

        AddNote = new AddNoteAsyncCommand(this);
        AddNotes = new AddNotesAsyncCommand(this);
        AddRow = new AddRowAsyncCommand(this);
        AddRows = new AddRowsAsyncCommand(this);
        AddRowsIn = new AddRowsInAsyncCommand(this);
        ChangeReportOrder = new ChangeReportOrderAsyncCommand(this);
        CheckReport = new CheckReportSyncCommand(this);
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
        SetNumberOrder = new SetNumberOrderSyncCommand(this);

        ShowDialog = new Interaction<object, int>();
        ShowDialogIn = new Interaction<int, int>();
        ShowMessageT = new Interaction<List<string>, string>();
        if (!isSum)
        {
            Storage.Sort();
        }
    }
}