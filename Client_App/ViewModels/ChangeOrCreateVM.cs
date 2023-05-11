using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData;
using Models.Attributes;
using Models.Classes;
using Models.Collections;
using Models.Interfaces;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Client_App.Commands.AsyncCommands;
using Client_App.Resources;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Forms;
using Models.Forms.DataAccess;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Client_App.Commands.AsyncCommands.Excel;
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

    #region Storage10
    private Form10 _Storage10;
    public Form10 Storage10
    {
        get
        {
            var count = Storage.Rows10.Count;
            return Storage.Rows10[count - 1];
        }
        set
        {
            if (_Storage10 != value)
            {
                _Storage10 = value;
                NotifyPropertyChanged("Storage10");
            }
        }
    }
    #endregion

    #region Storage20
    private Form20 _Storage20;
    public Form20 Storage20
    {
        get
        {
            var count = Storage.Rows20.Count;
            return Storage.Rows20[count - 1];
        }
        set
        {
            if (_Storage20 != value)
            {
                _Storage20 = value;
                NotifyPropertyChanged("Storage20");
            }
        }
    }
    #endregion

    #region SetNumberOrder
    public ReactiveCommand<object, Unit> SetNumberOrder { get; protected set; }
    private async Task _SetNumberOrder(object param)
    {
        var count = 1;
        var rows = Storage.Rows.GetEnumerable();
        foreach (var row in rows)
        {
            row.SetOrder(count);
            count++;
        }
    }
    #endregion

    #region DeleteDataInRows
    public ReactiveCommand<object, Unit> DeleteDataInRows { get; protected set; }
    private async Task _DeleteDataInRows(object _param)
    {
        var param = _param as object[];
        var collection = param[0] as IKeyCollection;
        var minColumn = Convert.ToInt32(param[1]) + 1;
        var maxColumn = Convert.ToInt32(param[2]) + 1;
        var collectionEn = collection.GetEnumerable();
        if (minColumn == 1 && collectionEn.FirstOrDefault() is Form1 or Form2)
        {
            minColumn++;
        }
        if (Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard)
        {
            foreach (var item in collectionEn.OrderBy(x => x.Order))
            {
                var props = item.GetType().GetProperties();
                var dStructure = (IDataGridColumn)item;
                var findStructure = dStructure.GetColumnStructure();
                var level = findStructure.Level;
                var tre = findStructure.GetLevel(level - 1);
                foreach (var prop in props)
                {
                    var attr = (FormPropertyAttribute)prop.GetCustomAttributes(typeof(FormPropertyAttribute), false).FirstOrDefault();
                    if (attr is null) continue;
                    try
                    {
                        var columnNum = attr.Names.Length > 1 && attr.Names[0] != "null-1-1"
                            ? Convert.ToInt32(tre.FirstOrDefault(x => x.name == attr.Names[0])?.innertCol
                                .FirstOrDefault(x => x.name == attr.Names[1])?.innertCol[0].name)
                            : Convert.ToInt32(attr.Number);
                        if (columnNum >= minColumn && columnNum <= maxColumn)
                        {
                            var midValue = prop.GetMethod?.Invoke(item, null);
                            switch (midValue)
                            {
                                case RamAccess<int?>:
                                    midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { int.Parse("") });
                                    break;
                                case RamAccess<float?>:
                                    midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { float.Parse("") });
                                    break;
                                case RamAccess<short>:
                                case RamAccess<short?>:
                                    midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { short.Parse("") });
                                    break;
                                case RamAccess<int>:
                                    midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { int.Parse("") });
                                    break;
                                case RamAccess<string>:
                                    midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { "" });
                                    break;
                                case RamAccess<byte?>:
                                case RamAccess<bool>:
                                    midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { null });
                                    break;
                                default:
                                    midValue?.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { "" });
                                    break;
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
        }
    }
    #endregion

    #region DeleteNote
    public ReactiveCommand<object, Unit> DeleteNote { get; protected set; }
    private async Task _DeleteNote(object _param)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var param = (IEnumerable)_param;
            #region MessageDeleteNote
            var answer = await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition { Name = "Да" },
                        new ButtonDefinition { Name = "Нет" }
                    },
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Вы действительно хотите удалить комментарий?",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow); 
            #endregion
            if (answer == "Да")
            {
                foreach (Note item in param)
                {
                    if (item == null) continue;
                    foreach (var key in Storage.Notes)
                    {
                        var it = (Note)key;
                        if (it.Order > item.Order)
                        {
                            it.Order -= 1;
                        }
                    }
                    foreach (Note nt in param)
                    {
                        Storage.Notes.Remove(nt);
                    }
                }
                await Storage.SortAsync();
            }
        }
    }
    #endregion

    #region Passport

    #region PassportUniqParam
    public static void PassportUniqParam(object param, out string? okpo, out string? type, out string? date, out string? pasNum, out string? factoryNum)
    {
        var par = param as object[];
        var collection = par?[0] as IKeyCollection;
        var item = collection?.GetEnumerable().MinBy(x => x.Order);
        var props = item?.GetType().GetProperties();
        okpo = "";
        type = "";
        date = "";
        pasNum = "";
        factoryNum = "";
        foreach (var prop in props!)
        {
            var attr = (FormPropertyAttribute?)prop
                .GetCustomAttributes(typeof(FormPropertyAttribute), false)
                .FirstOrDefault();
            if (attr is null
                || attr.Names.Length <= 1
                || attr.Names[0] is not ("Сведения из паспорта (сертификата) на закрытый радионуклидный источник"
                    or "Сведения об отработавших закрытых источниках ионизирующего излучения")
                || attr.Names[1] is not ("код ОКПО изготовителя" or "тип" or "дата выпуска"
                    or "номер паспорта (сертификата)"
                    or "номер паспорта (сертификата) ЗРИ, акта определения характеристик ОЗИИ" or "номер"))
            {
                continue;
            }
            var midValue = prop.GetMethod?.Invoke(item, null);
            if (midValue?.GetType().GetProperty("Value")?.GetMethod?.Invoke(midValue, null) is not (null or ""))
            {
                switch (attr.Names[1])
                {
                    case "код ОКПО изготовителя":
                        okpo = midValue.GetType().GetProperty("Value")?.GetMethod?.Invoke(midValue, null)?.ToString();
                        okpo = StaticStringMethods.ConvertPrimToDash(okpo);
                        break;
                    case "тип":
                        type = midValue.GetType().GetProperty("Value")?.GetMethod?.Invoke(midValue, null)?.ToString();
                        type = StaticStringMethods.ConvertPrimToDash(type);
                        break;
                    case "дата выпуска":
                        date = midValue.GetType().GetProperty("Value")?.GetMethod?.Invoke(midValue, null)?.ToString();
                        date = DateTime.TryParse(date, out var dateTime)
                            ? dateTime.ToShortDateString()
                            : date;
                        break;
                    case "номер паспорта (сертификата)" or "номер паспорта (сертификата) ЗРИ, акта определения характеристик ОЗИИ":
                        pasNum = midValue.GetType().GetProperty("Value")?.GetMethod?.Invoke(midValue, null)?.ToString();
                        pasNum = StaticStringMethods.ConvertPrimToDash(pasNum);
                        break;
                    case "номер":
                        factoryNum = midValue.GetType().GetProperty("Value")?.GetMethod?.Invoke(midValue, null)?.ToString();
                        factoryNum = StaticStringMethods.ConvertPrimToDash(factoryNum);
                        break;
                }
            }
        }
    }
    #endregion

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
    public ICommand DeleteRows { get; set; }                        //  Удалить выбранные строчки из формы
    public ICommand ExcelExportSourceMovementHistory { get; set; }  //  Выгрузка в Excel истории движения источника
    public ICommand OpenPas { get; set; }                           //  Найти и открыть соответствующий файл паспорта в сетевом хранилище
    public ICommand PasteRows { get; set; }                         //  Вставить значения из буфера обмена
    public ICommand SaveReport { get; set; }                        //  Сохранить отчет

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
                if (param.StartsWith('1'))
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
                else if (param.StartsWith('2'))
                {
                    var ty1 = (Form20)FormCreator.Create(param);
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form20)FormCreator.Create(param);
                    ty2.NumberInOrder_DB = 2;
                    Storage.Rows20.Add(ty1);
                    Storage.Rows20.Add(ty2);
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
        CheckReport = new CheckReportAsyncCommand(this);
        CopyExecutorData = new CopyExecutorDataAsyncCommand(this);
        CopyPasName = new CopyPasNameAsyncCommand();
        CopyRows = new CopyRowsAsyncCommand();
        DeleteRows = new DeleteRowsAsyncCommand(this);
        ExcelExportSourceMovementHistory = new ExcelExportSourceMovementHistoryAsyncCommand();
        OpenPas = new OpenPasAsyncCommand();
        PasteRows = new PasteRowsAsyncCommand();
        SaveReport = new SaveReportAsyncCommand(this);

        DeleteNote = ReactiveCommand.CreateFromTask<object>(_DeleteNote);
        SetNumberOrder = ReactiveCommand.CreateFromTask<object>(_SetNumberOrder);
        DeleteDataInRows = ReactiveCommand.CreateFromTask<object>(_DeleteDataInRows);
        ShowDialog = new Interaction<object, int>();
        ShowDialogIn = new Interaction<int, int>();
        ShowMessageT = new Interaction<List<string>, string>();
        if (!isSum)
        {
            Storage.Sort();
        }
    }
}