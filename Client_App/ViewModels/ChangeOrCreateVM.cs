using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Metadata;
using DynamicData;
using Models.Attributes;
using Models.Classes;
using Models.Collections;
using Models.DBRealization;
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

    #region CheckReport
    public ReactiveCommand<Unit, Unit> CheckReport { get; protected set; }
    private void _CheckReport()
    {
        IsCanSaveReportEnabled = true;
    }
    #endregion

    #region ChangeReportOrder
    public ReactiveCommand<Unit, Unit> ChangeReportOrder { get; protected set; }
    private async Task _ChangeReportOrder()
    {
        if (Storage.FormNum.Value == "1.0")
        {
            Storage.Rows10.Sorted = false;
            var tmp = Storage.Rows10[0].Order;
            Storage.Rows10[0].SetOrder(Storage.Rows10[1].Order);
            Storage.Rows10[1].SetOrder(tmp);
            await Storage.Rows10.QuickSortAsync();
        }
        if (Storage.FormNum.Value == "2.0")
        {
            Storage.Rows20.Sorted = false;
            var tmp = Storage.Rows20[0].Order;
            Storage.Rows20[0].SetOrder(Storage.Rows20[1].Order);
            Storage.Rows20[1].SetOrder(tmp);
            await Storage.Rows20.QuickSortAsync();
        }
    }
    #endregion

    #region SumRow
    public async void _SumRow(object sender, Avalonia.Interactivity.RoutedEventArgs args)
    {
        switch (Storage.FormNum_DB)
        {
            case "2.1":
                await Sum21();
                isSum = true;
                break;
            case "2.2":
                await Sum22();
                isSum = true;
                break;
        }
    }

    public async Task Sum21()
    {
        var tItems = Storage.Rows21
            .GroupBy(x => x.RefineMachineName_DB
                          + x.MachineCode_DB
                          + x.MachinePower_DB
                          + x.NumberOfHoursPerYear_DB)
            .ToList();
        var y = tItems.ToList();

        var ito = y.ToDictionary(item => item.Key, _ => new List<Form21>());

        foreach (var item in y.Where(item => item.Key == ""))
        {
            foreach (var t in item)
            {
                ito[item.Key].Add(t);
            }
            tItems.Remove(item);
        }

        Parallel.ForEach(tItems, itemT =>
        {
            var sums = itemT.FirstOrDefault(x => x.Sum_DB);

            var sumRow = sums;
            if ((itemT.Count() > 1 && sumRow == null) || (itemT.Count() > 2 && sumRow != null))
            {
                if (sumRow == null)
                {
                    var first = itemT.FirstOrDefault();
                    sumRow = (Form21)FormCreator.Create("2.1");

                    sumRow.RefineMachineName_Hidden_Set = new RefBool(false);
                    sumRow.MachineCode_Hidden_Set = new RefBool(false);
                    sumRow.MachinePower_Hidden_Set = new RefBool(false);
                    sumRow.NumberOfHoursPerYear_Hidden_Set = new RefBool(false);

                    sumRow.RefineMachineName_DB = first.RefineMachineName_DB;
                    sumRow.MachineCode_DB = first.MachineCode_DB;
                    sumRow.MachinePower_DB = first.MachinePower_DB;
                    sumRow.NumberOfHoursPerYear_DB = first.NumberOfHoursPerYear_DB;

                    sumRow.CodeRAOIn.Value = "";
                    sumRow.CodeRAOIn_Hidden = true;
                    sumRow.StatusRAOIn.Value = "";
                    sumRow.StatusRAOIn_Hidden = true;
                    sumRow.CodeRAOout.Value = "";
                    sumRow.CodeRAOout_Hidden = true;
                    sumRow.StatusRAOout.Value = "";
                    sumRow.StatusRAOout_Hidden = true;

                    sumRow.Sum_DB = true;
                    sumRow.BaseColor = ColorType.Green;

                }

                sumRow.NumberInOrder_DB = 0;

                double volumeInSum = 0;
                double massInSum = 0;
                double quantityInSum = 0;
                double alphaInSum = 0;
                double betaInSum = 0;
                double tritInSum = 0;
                double transInSum = 0;

                double volumeOutSum = 0;
                double massOutSum = 0;
                double quantityOutSum = 0;
                double alphaOutSum = 0;
                double betaOutSum = 0;
                double tritOutSum = 0;
                double transOutSum = 0;

                var refinemachinename = "";
                byte? machinecode = 0;

                List<Form21> lst = new();
                var u = itemT.OrderBy(x => x.NumberInOrder_DB).ToArray();
                foreach (var form in u)
                {
                    form.SumGroup_DB = true;

                    form.RefineMachineName_Hidden_Set = new RefBool(false);
                    form.MachineCode_Hidden_Set = new RefBool(false);
                    form.MachinePower_Hidden_Set = new RefBool(false);
                    form.NumberOfHoursPerYear_Hidden_Set = new RefBool(false);

                    form.RefineMachineName_Hidden_Get.Set(false);
                    form.MachineCode_Hidden_Get.Set(false);
                    form.MachinePower_Hidden_Get.Set(false);
                    form.NumberOfHoursPerYear_Hidden_Get.Set(false);

                    form.BaseColor = ColorType.Yellow;

                    volumeInSum += StringToNumber(form.VolumeIn_DB);
                    massInSum += StringToNumber(form.MassIn_DB);
                    quantityInSum += StringToNumber(form.QuantityIn_DB);
                    alphaInSum += StringToNumber(form.AlphaActivityIn_DB);
                    betaInSum += StringToNumber(form.BetaGammaActivityIn_DB);
                    tritInSum += StringToNumber(form.TritiumActivityIn_DB);
                    transInSum += StringToNumber(form.TransuraniumActivityIn_DB);

                    volumeOutSum += StringToNumber(form.VolumeOut_DB);
                    massOutSum += StringToNumber(form.MassOut_DB);
                    quantityOutSum += StringToNumber(form.QuantityOZIIIout_DB);
                    alphaOutSum += StringToNumber(form.AlphaActivityOut_DB);
                    betaOutSum += StringToNumber(form.BetaGammaActivityOut_DB);
                    tritOutSum += StringToNumber(form.TritiumActivityOut_DB);
                    transOutSum += StringToNumber(form.TransuraniumActivityOut_DB);

                    lst.Add(form);
                }

                sumRow.VolumeIn_DB = volumeInSum >= double.Epsilon ? volumeInSum.ToString("E2") : "-";
                sumRow.MassIn_DB = massInSum >= double.Epsilon ? sumRow.MassIn_DB = massInSum.ToString("E2") : "-";
                sumRow.QuantityIn_DB = quantityInSum >= double.Epsilon ? sumRow.QuantityIn_DB = quantityInSum.ToString("F0") : "-";
                sumRow.AlphaActivityIn_DB = alphaInSum >= double.Epsilon ? sumRow.AlphaActivityIn_DB = alphaInSum.ToString("E2") : "-";
                sumRow.BetaGammaActivityIn_DB = betaInSum >= double.Epsilon ? sumRow.BetaGammaActivityIn_DB = betaInSum.ToString("E2") : "-";
                sumRow.TritiumActivityIn_DB = tritInSum >= double.Epsilon ? sumRow.TritiumActivityIn_DB = tritInSum.ToString("E2") : "-";
                sumRow.TransuraniumActivityIn_DB = transInSum >= double.Epsilon ? sumRow.TransuraniumActivityIn_DB = transInSum.ToString("E2") : "-";
                sumRow.VolumeOut_DB = volumeOutSum >= double.Epsilon ? sumRow.VolumeOut_DB = volumeOutSum.ToString("E2") : "-";
                sumRow.MassOut_DB = massOutSum >= double.Epsilon ? sumRow.MassOut_DB = massOutSum.ToString("E2") : "-";
                sumRow.QuantityOZIIIout_DB = quantityOutSum >= double.Epsilon ? sumRow.QuantityOZIIIout_DB = quantityOutSum.ToString("F0") : "-";
                sumRow.AlphaActivityOut_DB = alphaOutSum >= double.Epsilon ? sumRow.AlphaActivityOut_DB = alphaOutSum.ToString("E2") : "-";
                sumRow.BetaGammaActivityOut_DB = betaOutSum >= double.Epsilon ? sumRow.BetaGammaActivityOut_DB = betaOutSum.ToString("E2") : "-";
                sumRow.TritiumActivityOut_DB = tritOutSum >= double.Epsilon ? sumRow.TritiumActivityOut_DB = tritOutSum.ToString("E2") : "-";
                sumRow.TransuraniumActivityOut_DB = transOutSum >= double.Epsilon ? sumRow.TransuraniumActivityOut_DB = transOutSum.ToString("E2") : "-";

                ito[itemT.Key].Add(sumRow);
                foreach (var r in lst)
                {
                    ito[itemT.Key].Add(r);
                }
            }
            else
            {
                foreach (var t in itemT)
                {
                    if (!t.Sum_DB)
                    {
                        //var form = t;
                        //form.RefineMachineName_Hidden_Get.Set(true);
                        //form.MachineCode_Hidden_Get.Set(true);
                        //form.MachinePower_Hidden_Get.Set(true);
                        //form.NumberOfHoursPerYear_Hidden_Get.Set(true);
                        ito[itemT.Key].Add(t);
                    }
                }
            }
        });
        Storage.Rows21.Clear();
        var yu = ito.OrderBy(x => x.Value.Count).ToArray();
        var count = new LetterAlgebra("A");
        var lst = new List<Form21>();
        foreach (var item in yu)
        {
            if (item.Value.Count != 0 && item.Value.Count != 1)
            {
                item.Value.First().NumberInOrderSum = new RamAccess<string>(null, count.ToString());
                lst.AddRange(item.Value);
                count++;
            }
            else
            {
                lst.AddRange(item.Value);
            }
        }
        Storage.Rows21.AddRange(lst);
    }

    public async Task Sum22()
    {
        var tItems = Storage.Rows22
            .GroupBy(x => x.StoragePlaceName_DB
                          + x.StoragePlaceCode_DB
                          + x.PackName_DB
                          + x.PackType_DB)
            .ToList();
        var y = tItems.ToList();
        var ito = y.ToDictionary(item => item.Key, _ => new List<Form22>());
        foreach (var item in y.Where(item => item.Key == ""))
        {
            foreach (var t in item)
            {
                ito[item.Key].Add(t);
            }
            tItems.Remove(item);
        }
        Parallel.ForEach(tItems, itemT =>
        {
            var sums = itemT.FirstOrDefault(x => x.Sum_DB);
            var sumRow = sums;
            if ((itemT.Count() > 1 && sumRow == null) || (itemT.Count() > 2 && sumRow != null))
            {
                if (sumRow == null)
                {
                    var first = itemT.FirstOrDefault();
                    sumRow = (Form22)FormCreator.Create("2.2");

                    sumRow.StoragePlaceName_Hidden_Set = new RefBool(false);
                    sumRow.StoragePlaceCode_Hidden_Set = new RefBool(false);
                    sumRow.PackName_Hidden_Set = new RefBool(false);
                    sumRow.PackType_Hidden_Set = new RefBool(false);
                    sumRow.StoragePlaceName_DB = first.StoragePlaceName_DB;
                    sumRow.StoragePlaceCode_DB = first.StoragePlaceCode_DB;
                    sumRow.PackName_DB = first.PackName_DB;
                    sumRow.PackType_DB = first.PackType_DB;
                    sumRow.CodeRAO_Hidden = true;
                    sumRow.StatusRAO_Hidden = true;
                    sumRow.MainRadionuclids_Hidden = true;
                    sumRow.Subsidy_Hidden = true;
                    sumRow.FcpNumber_Hidden = true;
                    sumRow.Sum_DB = true;
                    sumRow.BaseColor = ColorType.Green;
                }

                sumRow.NumberInOrder_DB = 0;
                double volumeSum = 0;
                double massSum = 0;
                var quantitySum = 0;
                double alphaSum = 0;
                double betaSum = 0;
                double tritSum = 0;
                double transSum = 0;

                List<Form22> lst = new();
                var u = itemT.OrderBy(x => x.NumberInOrder_DB);
                foreach (var form in u)
                {
                    form.VolumeInPack_Hidden = true;
                    form.MassInPack_Hidden = true;

                    form.SumGroup_DB = true;
                    form.StoragePlaceName_Hidden_Set = new RefBool(false);
                    form.StoragePlaceCode_Hidden_Set = new RefBool(false);
                    form.PackName_Hidden_Set = new RefBool(false);
                    form.PackType_Hidden_Set = new RefBool(false);

                    //form.StoragePlaceName_Hidden_Get.Set(false);
                    //form.StoragePlaceCode_Hidden_Get.Set(false);
                    //form.PackName_Hidden_Get.Set(false);
                    //form.PackType_Hidden_Get.Set(false);
                    form.BaseColor = ColorType.Yellow;

                    volumeSum += StringToNumber(form.VolumeOutOfPack_DB);
                    massSum += StringToNumber(form.MassOutOfPack_DB);
                    quantitySum += StringToNumberInt(form.QuantityOZIII_DB);
                    alphaSum += StringToNumber(form.AlphaActivity_DB);
                    betaSum += StringToNumber(form.BetaGammaActivity_DB);
                    tritSum += StringToNumber(form.TritiumActivity_DB);
                    transSum += StringToNumber(form.TransuraniumActivity_DB);

                    lst.Add(form);
                }

                sumRow.VolumeOutOfPack_DB = volumeSum >= double.Epsilon ? volumeSum.ToString("E2") : "-";
                sumRow.MassOutOfPack_DB = massSum >= double.Epsilon ? massSum.ToString("E2") : "-";
                sumRow.QuantityOZIII_DB = quantitySum >= 0 ? quantitySum.ToString() : "-";
                sumRow.AlphaActivity_DB = alphaSum >= double.Epsilon ? alphaSum.ToString("E2") : "-";
                sumRow.BetaGammaActivity_DB = betaSum >= double.Epsilon ? betaSum.ToString("E2") : "-";
                sumRow.TritiumActivity_DB = tritSum >= double.Epsilon ? tritSum.ToString("E2") : "-";
                sumRow.TransuraniumActivity_DB = transSum >= double.Epsilon ? transSum.ToString("E2") : "-";

                ito[itemT.Key].Add(sumRow);
                foreach (var r in lst)
                {
                    ito[itemT.Key].Add(r);
                }
            }
            else
            {
                foreach (var t in itemT)
                {
                    if (!t.Sum_DB)
                    {
                        //var form = t;
                        //form.StoragePlaceName_Hidden_Get.Set(true);
                        //form.StoragePlaceCode_Hidden_Get.Set(true);
                        //form.PackName_Hidden_Get.Set(true);
                        //form.PackType_Hidden_Get.Set(true);

                        //form.StoragePlaceName_Hidden_Set.Set(true);
                        //form.StoragePlaceCode_Hidden_Set.Set(true);
                        //form.PackName_Hidden_Set.Set(true);
                        //form.PackType_Hidden_Set.Set(true);
                        ito[itemT.Key].Add(t);
                    }
                }
            }
        });
        Storage.Rows22.Clear();
        var yu = ito.OrderBy(x => x.Value.Count);
        var count = new LetterAlgebra("A");
        var lst = new List<Form22>();
        foreach (var item in yu)
        {
            if (item.Value.Count != 0 && item.Value.Count != 1)
            {
                var o = item.Value.First().NumberInOrderSum_DB = count.ToString();
                lst.AddRange(item.Value);
                //Storage.Rows22.AddRange(item.Value);
                count++;
            }
            else
            {
                lst.AddRange(item.Value);
                //Storage.Rows22.AddRange(item.Value);
            }
        }
        Storage.Rows22.AddRange(lst);
    }

    private static double StringToNumber(string num)
    {
        if (num != null)
        {
            var tmp = num;
            tmp.Replace(" ", "");
            var len = tmp.Length;
            if (len >= 1)
            {
                if (len > 2)
                {
                    if (tmp[0] == '(' && tmp[len - 1] == ')')
                    {
                        tmp = tmp.Remove(len - 1, 1);
                        tmp = tmp.Remove(0, 1);
                    }
                }
                if (len == 1)
                {
                    tmp = tmp.Replace("-", "0");
                }

                tmp = tmp.Replace(",", ".");
                var styles = NumberStyles.Any;
                try
                {
                    return double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB"));
                }
                catch
                {
                    return 0;
                }
            }
        }
        return 0;
    }

    private static int StringToNumberInt(string num)
    {
        if (num != null)
        {
            var tmp = num;
            tmp.Replace(" ", "");
            var len = tmp.Length;
            if (len >= 1)
            {
                if (len > 2)
                {
                    if (tmp[0] == '(' && tmp[len - 1] == ')')
                    {
                        tmp = tmp.Remove(len - 1, 1);
                        tmp = tmp.Remove(0, 1);
                    }
                }
                if (len == 1)
                {
                    tmp = tmp.Replace("-", "0");
                }

                tmp = tmp.Replace(",", ".");
                var styles = NumberStyles.Any;
                try
                {
                    return int.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB"));
                }
                catch
                {
                    return 0;
                }
            }
        }
        return 0;
    }
    #endregion

    #region CancelSumRow
    public async void _CancelSumRow(object sender, Avalonia.Interactivity.RoutedEventArgs args)
    {
        if (Storage.FormNum_DB == "2.1")
        {
            await UnSum21();
            Storage.Rows21.Sorted = false;
            await Storage.Rows21.QuickSortAsync();
        }
        if (Storage.FormNum_DB == "2.2")
        {
            await UnSum22();
            Storage.Rows21.Sorted = false;
            await Storage.Rows22.QuickSortAsync();
        }
    }

    public async Task UnSum21()
    {
        var sumRows = Storage.Rows21.Where(x => x.Sum_DB);

        Storage.Rows21.RemoveMany(sumRows);

        var sumRowsGroup = Storage.Rows21.Where(x => x.SumGroup_DB);
        foreach (var row in sumRowsGroup)
        {
            row.RefineMachineName_Hidden_Set.Set(true);
            row.MachineCode_Hidden_Set.Set(true);
            row.MachinePower_Hidden_Set.Set(true);
            row.NumberOfHoursPerYear_Hidden_Set.Set(true);
            row.RefineMachineName_Hidden_Get.Set(true);
            row.MachineCode_Hidden_Get.Set(true);
            row.MachinePower_Hidden_Get.Set(true);
            row.NumberOfHoursPerYear_Hidden_Get.Set(true);
            row.SumGroup_DB = false;
            row.BaseColor = ColorType.None;
        }
        var rows = Storage.Rows21.GetEnumerable();
        var count = 1;
        foreach (var item in rows)
        {
            var row = (Form21)item;
            row.SetOrder(count);
            count++;
            row.NumberInOrderSum = new RamAccess<string>(null, "");
        }
    }

    public async Task UnSum22()
    {
        var sumRows = Storage.Rows22.Where(x => x.Sum_DB);

        Storage.Rows22.RemoveMany(sumRows);

        var sumRowsGroup = Storage.Rows22.Where(x => x.SumGroup_DB);
        foreach (var row in sumRowsGroup)
        {
            row.StoragePlaceName_Hidden_Set.Set(true);
            row.StoragePlaceCode_Hidden_Set.Set(true);
            row.PackName_Hidden_Set.Set(true);
            row.PackType_Hidden_Set.Set(true);
            row.StoragePlaceName_Hidden_Get.Set(true);
            row.StoragePlaceCode_Hidden_Get.Set(true);
            row.PackName_Hidden_Get.Set(true);
            row.PackType_Hidden_Get.Set(true);
            row.SumGroup_DB = false;
            row.BaseColor = ColorType.None;
        }
        var rows = Storage.Rows22.GetEnumerable();
        var count = 1;
        foreach (var item in rows)
        {
            var row = (Form22)item;
            row.SetOrder(count);
            count++;
            row.NumberInOrderSum = new RamAccess<string>(null, "");
        }
    }
    #endregion

    //#region AddSort
    //public ReactiveCommand<string, Unit> AddSort { get; protected set; }
    //#endregion

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

    #region PasteRows
    public ReactiveCommand<object, Unit> PasteRows { get; protected set; }
    private async Task _PasteRows(object _param)
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

        if (Application.Current.Clipboard is { } clip)
        {
            var text = await clip.GetTextAsync();
            var rowsText = ParseInnerTextRows(text);

            foreach (var item in collectionEn.OrderBy(x => x.Order))
            {
                var props = item.GetType().GetProperties();
                var rowText = rowsText[item.Order - collectionEn.Min(x => x.Order)];
                if (Convert.ToInt32(param[1]) == 0 && collectionEn.FirstOrDefault() is not Note)
                {
                    var newText = rowText.ToArray();
                    var count = 0;
                    foreach (var t in newText)
                    {
                        count++;
                        if (t.Equals('\t'))
                        {
                            break;
                        }
                    }
                    rowText = rowText.Remove(0, count);
                }
                var columnsText = ParseInnerTextColumn(rowText);
                var dStructure = (IDataGridColumn)item;
                var findStructure = dStructure.GetColumnStructure();
                var Level = findStructure.Level;
                var tre = findStructure.GetLevel(Level - 1);

                //if (maxColumn-1 != columnsText.Length) 
                //{
                //    columnsText = columnsText[1..columnsText.Length];
                //}

                foreach (var prop in props)
                {
                    var attr = (FormPropertyAttribute)prop.GetCustomAttributes(typeof(FormPropertyAttribute), false).FirstOrDefault();
                    if (attr == null) continue;
                    try
                    {
                        int columnNum;
                        if (attr.Names.Length > 1 && attr.Names.Length != 4 && attr.Names[0] is not("null-1-1" or "Документ" or "Сведения об операции"))
                        {
                            columnNum = Convert.ToInt32(tre.FirstOrDefault(x => x.name == attr.Names[0])?.innertCol.FirstOrDefault(x => x.name == attr.Names[1])?.innertCol[0].name);
                        }
                        else if (attr.Names[0] == "Документ")
                        {
                            var findDock = tre.Where(x => x.name == "null-n");
                            columnNum = Convert.ToInt32(findDock.Any() 
                                ? findDock.FirstOrDefault()?.innertCol.FirstOrDefault(x => x.name == attr.Names[0])
                                    ?.innertCol.FirstOrDefault(x => x.name == attr.Names[1])?.innertCol[0].name 
                                : tre.FirstOrDefault(x => x.name == attr.Names[0])?.innertCol
                                    .FirstOrDefault(x => x.name == attr.Names[1])?.innertCol[0].name);
                        }
                        else
                        {
                            columnNum = Convert.ToInt32(attr.Number);
                        }
                        //var columnNum = Convert.ToInt32(attr.Number);
                        if (columnNum >= minColumn && columnNum <= maxColumn)
                        {
                            var midValue = prop.GetMethod?.Invoke(item, null);
                            switch (midValue)
                            {
                                case RamAccess<int?>:
                                    midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { int.Parse(columnsText[columnNum - minColumn]) });
                                    break;
                                case RamAccess<float?>:
                                    midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { float.Parse(columnsText[columnNum - minColumn]) });
                                    break;
                                case RamAccess<short>:
                                case RamAccess<short?>:
                                    midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { short.Parse(columnsText[columnNum - minColumn]) });
                                    break;
                                case RamAccess<int>:
                                    midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { int.Parse(columnsText[columnNum - minColumn]) });
                                    break;
                                case RamAccess<string>:
                                    midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { columnsText[columnNum - minColumn] });
                                    break;
                                case RamAccess<byte?>:
                                    midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { byte.Parse(columnsText[columnNum - minColumn]) });
                                    break;
                                case RamAccess<bool>:
                                    midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { bool.Parse(columnsText[columnNum - minColumn]) });
                                    break;
                                default:
                                    midValue?.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue, new object[] { columnsText[columnNum - minColumn] });
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
    public ICommand CopyExecutorData { get; set; }                  //  Скопировать данные исполнителя из предыдущей формы
    public ICommand CopyPasName { get; set; }                       //  Скопировать в буфер обмена уникальное имя паспорта
    public ICommand CopyRows { get; set; }                          //  Скопировать в буфер обмена уникальное имя паспорта
    public ICommand DeleteRows { get; set; }                        //  Удалить выбранные строчки из формы
    public ICommand ExcelExportSourceMovementHistory { get; set; }  //  Выгрузка в Excel истории движения источника
    public ICommand OpenPas { get; set; }                           //  Найти и открыть соответствующий файл паспорта в сетевом хранилище
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

        if (param.Split('.')[0] == "1")
        {
            if (param != "1.0")
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
        }
        else
        {
            if (param != "2.0")
            {
                try
                {
                    var ty = reps.Report_Collection
                        .Where(t => t.FormNum_DB == param && t.Year_DB != null)
                        .OrderBy(t => t.Year_DB)
                        .Select(t => t.Year_DB)
                        .LastOrDefault();
                    FormType = param;
                    if (ty != null)
                    {
                        Storage.Year.Value = (Convert.ToInt32(ty) + 1).ToString();
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }
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
        CopyExecutorData = new CopyExecutorDataAsyncCommand(this);
        CopyPasName = new CopyPasNameAsyncCommand();
        CopyRows = new CopyRowsAsyncCommand();
        DeleteRows = new DeleteRowsAsyncCommand(this);
        ExcelExportSourceMovementHistory = new ExcelExportSourceMovementHistoryAsyncCommand();
        OpenPas = new OpenPasAsyncCommand();
        SaveReport = new SaveReportAsyncCommand(this);

        ChangeReportOrder = ReactiveCommand.CreateFromTask(_ChangeReportOrder);
        CheckReport = ReactiveCommand.Create(_CheckReport);
        PasteRows = ReactiveCommand.CreateFromTask<object>(_PasteRows);
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

    [DependsOn(nameof(IsCanSaveReportEnabled))]
    #endregion

    //private bool CanSaveReport(object parameter)
    //{
    //    return _isCanSaveReportEnabled;
    //}

    //public void _AddRow10()
    //{
    //    Form10? frm = new Form10(); Storage.Rows10.Add(frm); Storage.LastAddedForm = Report.Forms.Form10;
    //}
    //public void _AddRow20()
    //{
    //    Form20? frm = new Form20(); Storage.Rows20.Add(frm); Storage.LastAddedForm = Report.Forms.Form20;
    //}

    #region ParseInnerText
    private static string[] ParseInnerTextRows(string text)
    {
        List<string> lst = new();
        var comaFlag = false;
        text = text.Replace("\r\n", "\n");
        var txt = "";
        foreach (var item in text)
        {
            switch (item)
            {
                case '\"':
                    txt += item;
                    comaFlag = !comaFlag;
                    break;
                //||(item=='\t'))
                case '\n' when !comaFlag:
                    lst.Add(txt);
                    txt = "";
                    break;
                case '\n':
                    txt += item;
                    break;
                default:
                    txt += item;
                    break;
            }
        }
        if (txt != "")
        {
            lst.Add(txt);
        }
        lst.Add("");
        return lst.ToArray();
    }
    private static string[] ParseInnerTextColumn(string text)
    {
        List<string> lst = new();
        var comaFlag = false;
        var txt = "";
        foreach (var item in text)
        {
            switch (item)
            {
                case '\"':
                    comaFlag = true;
                    break;
                case '\t' when !comaFlag:
                    lst.Add(txt);
                    txt = "";
                    break;
                case '\t':
                    txt += item;
                    break;
                default:
                    txt += item;
                    break;
            }
        }
        if (txt != "")
        {
            lst.Add(txt);
        }
        return lst.ToArray();
    }
    #endregion
}