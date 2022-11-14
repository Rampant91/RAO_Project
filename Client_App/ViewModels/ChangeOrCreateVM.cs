using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Metadata;
using DynamicData;
using Models;
using Models.Abstracts;
using Models.Attributes;
using Models.Classes;
using Models.Collections;
using Models.DataAccess;
using Models.DBRealization;
using OfficeOpenXml;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client_App.ViewModels;

public class ChangeOrCreateVM : BaseVM, INotifyPropertyChanged
{
    public string WindowHeader { get; set; } = "default";
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
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
    private DBObservable _localReports = new();
    public DBObservable LocalReports
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
            int count = Storage.Rows10.Count;
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
            int count = Storage.Rows20.Count;
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
        if (Storage.FormNum_DB == "2.1")
        {
            await Sum21();
            isSum = true;
        }
        if (Storage.FormNum_DB == "2.2")
        {
            await Sum22();
            isSum = true;
        }
    }

    public async Task Sum21()
    {
        var tItems = Storage.Rows21.GroupBy(x => (x as Form21).RefineMachineName_DB +
                                                 (x as Form21).MachineCode_DB +
                                                 (x as Form21).MachinePower_DB +
                                                 (x as Form21).NumberOfHoursPerYear_DB).ToList();
        var y = tItems.ToList();

        var ito = new Dictionary<string, List<Form21>>();

        foreach (var item in y)
        {
            ito.Add(item.Key, new List<Form21>());
        }

        foreach (var item in y)
        {
            if (item.Key == "")
            {
                foreach (var t in item)
                {
                    ito[item.Key].Add(t);
                }
                tItems.Remove(item);
            }
        }
        List<Task> rtl = new();

        Parallel.ForEach(tItems, itemT =>
        {
            var sums = itemT.Where(x => (x as Form21).Sum_DB == true).FirstOrDefault();

            Form21 sumRow = sums as Form21;
            if ((itemT.Count() > 1 && sumRow == null) || (itemT.Count() > 2 && sumRow != null))
            {
                if (sumRow == null)
                {
                    var first = itemT.FirstOrDefault() as Form21;
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
                    sumRow.BaseColor = Models.Interfaces.ColorType.Green;

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

                string refinemachinename = "";
                byte? machinecode = 0;

                List<Form21> lst = new();
                var u = itemT.OrderBy(x => (x as Form21).NumberInOrder_DB);
                foreach (var itemThread in u)
                {
                    var form = itemThread;
                    form.SumGroup_DB = true;

                    form.RefineMachineName_Hidden_Set = new RefBool(false);
                    form.MachineCode_Hidden_Set = new RefBool(false);
                    form.MachinePower_Hidden_Set = new RefBool(false);
                    form.NumberOfHoursPerYear_Hidden_Set = new RefBool(false);

                    form.RefineMachineName_Hidden_Get.Set(false);
                    form.MachineCode_Hidden_Get.Set(false);
                    form.MachinePower_Hidden_Get.Set(false);
                    form.NumberOfHoursPerYear_Hidden_Get.Set(false);

                    form.BaseColor = Models.Interfaces.ColorType.Yellow;

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

                    lst.Add(form as Form21);

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


                ito[itemT.Key].Add(sumRow as Form21);
                foreach (var r in lst)
                {
                    ito[itemT.Key].Add(r);
                }
            }
            else
            {
                foreach (var t in itemT)
                {
                    if ((t as Form21).Sum_DB != true)
                    {
                        var form = t as Form21;
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
        var yu = ito.OrderBy(x => x.Value.Count);
        var count = new LetterAlgebra("A");

        foreach (var item in yu)
        {
            if (item.Value.Count != 0 && item.Value.Count != 1)
            {
                ((List<Form21>)item.Value).FirstOrDefault().NumberInOrderSum = new RamAccess<string>(null, count.ToString());
                Storage.Rows21.AddRange(item.Value);
                count++;
            }
            else
            {
                Storage.Rows21.AddRange(item.Value);
            }
        }
    }

    public async Task Sum22()
    {
        var tItems = Storage.Rows22.GroupBy(x => (x as Form22).StoragePlaceName_DB +
                                                 (x as Form22).StoragePlaceCode_DB +
                                                 (x as Form22).PackName_DB +
                                                 (x as Form22).PackType_DB).ToList();
        var y = tItems.ToList();

        var ito = new Dictionary<string, List<Form22>>();

        foreach (var item in y)
        {
            ito.Add(item.Key, new List<Form22>());
        }

        foreach (var item in y)
        {
            if (item.Key == "")
            {
                foreach (var t in item)
                {
                    ito[item.Key].Add(t);
                }
                tItems.Remove(item);
            }
        }
        List<Task> rtl = new();

        Parallel.ForEach(tItems, itemT =>
        {
            var sums = itemT.Where(x => (x as Form22).Sum_DB == true).FirstOrDefault();

            Form22 sumRow = sums as Form22;
            if ((itemT.Count() > 1 && sumRow == null) || (itemT.Count() > 2 && sumRow != null))
            {
                if (sumRow == null)
                {
                    var first = itemT.FirstOrDefault() as Form22;
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
                    sumRow.BaseColor = Models.Interfaces.ColorType.Green;

                }

                sumRow.NumberInOrder_DB = 0;

                double volumeSum = 0;
                double massSum = 0;
                int quantitySum = 0;

                double alphaSum = 0;
                double betaSum = 0;
                double tritSum = 0;
                double transSum = 0;

                List<Form22> lst = new();
                var u = itemT.OrderBy(x => (x as Form22).NumberInOrder_DB);
                foreach (var itemThread in u)
                {
                    var form = itemThread;

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
                    form.BaseColor = Models.Interfaces.ColorType.Yellow;

                    volumeSum += StringToNumber(form.VolumeOutOfPack_DB);
                    massSum += StringToNumber(form.MassOutOfPack_DB);
                    quantitySum += StringToNumberInt(form.QuantityOZIII_DB);
                    alphaSum += StringToNumber(form.AlphaActivity_DB);
                    betaSum += StringToNumber(form.BetaGammaActivity_DB);
                    tritSum += StringToNumber(form.TritiumActivity_DB);
                    transSum += StringToNumber(form.TransuraniumActivity_DB);

                    lst.Add(form as Form22);

                }

                sumRow.VolumeOutOfPack_DB = volumeSum >= double.Epsilon ? volumeSum.ToString("E2") : "-";
                sumRow.MassOutOfPack_DB = massSum >= double.Epsilon ? massSum.ToString("E2") : "-";
                sumRow.QuantityOZIII_DB = quantitySum >= 0 ? quantitySum.ToString() : "-";
                sumRow.AlphaActivity_DB = alphaSum >= double.Epsilon ? alphaSum.ToString("E2") : "-";
                sumRow.BetaGammaActivity_DB = betaSum >= double.Epsilon ? betaSum.ToString("E2") : "-";
                sumRow.TritiumActivity_DB = tritSum >= double.Epsilon ? tritSum.ToString("E2") : "-";
                sumRow.TransuraniumActivity_DB = transSum >= double.Epsilon ? transSum.ToString("E2") : "-";

                ito[itemT.Key].Add(sumRow as Form22);
                foreach (var r in lst)
                {
                    ito[itemT.Key].Add(r);
                }
            }
            else
            {

                foreach (var t in itemT)
                {
                    if ((t as Form22).Sum_DB != true)
                    {
                        var form = t as Form22;
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

        foreach (var item in yu)
        {
            if (item.Value.Count != 0 && item.Value.Count != 1)
            {
                var o = ((List<Form22>)item.Value).FirstOrDefault().NumberInOrderSum = new RamAccess<string>(null, count.ToString());
                Storage.Rows22.AddRange(item.Value);
                count++;
            }
            else
            {
                Storage.Rows22.AddRange(item.Value);
            }
        }
    }

    double StringToNumber(string Num)
    {
        if (Num != null)
        {
            string tmp = Num;
            tmp.Replace(" ", "");
            int len = tmp.Length;
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
                NumberStyles styles = NumberStyles.Any;
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

    int StringToNumberInt(string Num)
    {
        if (Num != null)
        {
            string tmp = Num;
            tmp.Replace(" ", "");
            int len = tmp.Length;
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
                NumberStyles styles = NumberStyles.Any;
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
        var sum_rows = Storage.Rows21.Where(x => x.Sum_DB == true);

        Storage.Rows21.RemoveMany(sum_rows);

        var sum_rows_group = Storage.Rows21.Where(x => x.SumGroup_DB == true);
        foreach (var row in sum_rows_group)
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
            row.BaseColor = Models.Interfaces.ColorType.None;
        }
        var rows = Storage.Rows21.GetEnumerable();
        var count = 1;
        foreach (Form21 row in rows)
        {
            row.SetOrder(count);
            count++;
            row.NumberInOrderSum = new RamAccess<string>(null, "");
        }
    }

    public async Task UnSum22()
    {
        var sum_rows = Storage.Rows22.Where(x => x.Sum_DB == true);

        Storage.Rows22.RemoveMany(sum_rows);

        var sum_rows_group = Storage.Rows22.Where(x => x.SumGroup_DB == true);
        foreach (var row in sum_rows_group)
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
            row.BaseColor = Models.Interfaces.ColorType.None;
        }
        var rows = Storage.Rows22.GetEnumerable();
        var count = 1;
        foreach (Form22 row in rows)
        {
            row.SetOrder(count);
            count++;
            row.NumberInOrderSum = new RamAccess<string>(null, "");
        }
    }
    #endregion

    #region AddSort
    public ReactiveCommand<string, Unit> AddSort { get; protected set; }
    #endregion

    #region AddNote
    public ReactiveCommand<object, Unit> AddNote { get; protected set; }
    private async Task _AddNote(object param)
    {
        Note? nt = new()
        {
            Order = GetNumberInOrder(Storage.Notes)
        };
        Storage.Notes.Add(nt);
        await Storage.SortAsync();
    }
    #endregion

    #region SetNumberOrder
    public ReactiveCommand<object, Unit> SetNumberOrder { get; protected set; }
    private async Task _SetNumberOrder(object Param)
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

    #region AddRow
    public ReactiveCommand<object, Unit> AddRow { get; protected set; }
    private async Task _AddRow(object Param)
    {
        var frm = FormCreator.Create(FormType);
        frm.NumberInOrder_DB = GetNumberInOrder(Storage[Storage.FormNum_DB]);
        Storage[Storage.FormNum_DB].Add(frm);
        await Storage.SortAsync();
    }
    #endregion

    #region AddRowIn
    public ReactiveCommand<object, Unit> AddRowIn { get; protected set; }
    private async Task _AddRowIn(object _param)
    {
        var param = (IEnumerable)_param;
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            List<Form> lst = new();
            foreach (object? item1 in param)
            {
                lst.Add((Form)item1);
            }

            var item = lst.FirstOrDefault();

            if (item != null)
            {
                var number_cell = item.NumberInOrder_DB;
                var t2 = await ShowDialogIn.Handle(number_cell);

                if (t2 > 0)
                {
                    foreach (Form it in Storage[item.FormNum_DB])
                    {
                        if (it.NumberInOrder_DB > number_cell - 1)
                        {
                            it.NumberInOrder.Value = it.NumberInOrder_DB + t2;
                        }
                    }

                    List<Form> _lst = new();
                    for (int i = 0; i < t2; i++)
                    {
                        var frm = FormCreator.Create(FormType);
                        frm.NumberInOrder_DB = number_cell;

                        _lst.Add(frm);
                        number_cell++;
                    }
                    Storage[Storage.FormNum_DB].AddRange(_lst);
                    await Storage.SortAsync();
                }
            }
        }
    }
    #endregion

    #region DeleteRow
    public ReactiveCommand<object, Unit> DeleteRow { get; protected set; }
    private async Task _DeleteRow(object _param)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var param = (IEnumerable<IKey>)_param;
            var answ = await ShowMessageT.Handle(new List<string> { "Вы действительно хотите удалить строчку?", "Да", "Нет" });
            if (answ == "Да")
            {
                //var lst = new List<IKey>(Storage.Rows.GetEnumerable());
                var countParam = param;
                var minItem = param.Min(x => x.Order);
                var maxItem = param.Max(x => x.Order);
                foreach (var item in param)
                {
                    if (item != null)
                    {
                        Storage.Rows.Remove(item);
                    }
                }
                //var itemQ = Storage.Rows.GetEnumerable().Where(x=>x.Order>maxItem);
                //foreach(var item in itemQ)
                //{
                //    item.SetOrder(minItem);
                //    minItem++;
                //}
                var rows = Storage[Storage.FormNum_DB].GetEnumerable();
                if ((rows.FirstOrDefault() as Form).FormNum_DB.Equals("2.1"))
                {
                    var count = 1;
                    foreach (Form21 row in rows)
                    {
                        row.SetOrder(count);
                        count++;
                        row.NumberInOrderSum = new RamAccess<string>(null, "");
                    }
                }
                else if ((rows.FirstOrDefault() as Form).FormNum_DB.Equals("2.2"))
                {
                    var count = 1;
                    foreach (Form22 row in rows)
                    {
                        row.SetOrder(count);
                        count++;
                        row.NumberInOrderSum = new RamAccess<string>(null, "");
                    }
                }
                else
                {
                    //var count = 1;
                    //foreach (var row in Storage[Storage.FormNum_DB])
                    //{
                    //    row.SetOrder(count);
                    //    count++;
                    //}
                    await Storage.SortAsync();
                    var itemQ = Storage.Rows.GetEnumerable().Where(x => x.Order > minItem);
                    foreach (var item in itemQ)
                    {
                        item.SetOrder(minItem);
                        minItem++;
                    }
                }
                //await Storage.SortAsync();
            }
        }
    }
    #endregion

    #region DuplicateRowsx1
    public ReactiveCommand<object, Unit> DuplicateRowsx1 { get; protected set; }
    private async Task _DuplicateRowsx1(object param)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var t = await ShowDialog.Handle(desktop.MainWindow);
            if (t > 0)
            {
                var number = GetNumberInOrder(Storage.Rows);
                for (int i = 0; i < t; i++)
                {
                    var frm = FormCreator.Create(FormType);
                    frm.NumberInOrder_DB = number;
                    Storage.Rows.Add(frm);
                    number++;
                }
            }
        }
    }
    #endregion

    #region DuplicateNotes
    public ReactiveCommand<object, Unit> DuplicateNotes { get; protected set; }
    private async Task _DuplicateNotes(object param)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var t = await ShowDialog.Handle(desktop.MainWindow);

            if (t > 0)
            {
                var r = GetNumberInOrder(Storage.Notes);
                for (int i = 0; i < t; i++)
                {
                    var frm = new Note
                    {
                        Order = r
                    };
                    Storage.Notes.Add(frm);
                    r++;
                }
            }
        }
    }
    #endregion

    #region DeleteDataInRows
    public ReactiveCommand<object, Unit> DeleteDataInRows { get; protected set; }
    private async Task _DeleteDataInRows(object _param)
    {
        object[] param = _param as object[];
        IKeyCollection collection = param[0] as IKeyCollection;
        int minColumn = Convert.ToInt32(param[1]) + 1;
        int maxColumn = Convert.ToInt32(param[2]) + 1;
        var collectionEn = collection.GetEnumerable();
        if (collectionEn.FirstOrDefault() is Form1 || collectionEn.FirstOrDefault() is Form2)
        {
            if (minColumn == 1) minColumn++;
        }

        if (Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard clip)
        {
            foreach (IKey item in collectionEn.OrderBy(x => x.Order))
            {
                var props = item.GetType().GetProperties();
                var dStructure = (IDataGridColumn)item;
                var findStructure = dStructure.GetColumnStructure();
                var Level = findStructure.Level;
                var tre = findStructure.GetLevel(Level - 1);


                foreach (var prop in props)
                {
                    var attr = (Form_PropertyAttribute)prop.GetCustomAttributes(typeof(Form_PropertyAttribute), false).FirstOrDefault();
                    if (attr != null)
                    {
                        try
                        {
                            var columnNum = 0;
                            if (attr.Names.Count() > 1 && attr.Names[0] != "null-1-1")
                            {
                                columnNum = Convert.ToInt32(tre.Where(x => x.name == attr.Names[0]).FirstOrDefault().innertCol.Where(x => x.name == attr.Names[1]).FirstOrDefault().innertCol[0].name);
                            }
                            else
                            {
                                columnNum = Convert.ToInt32(attr.Number);
                            }
                            if (columnNum >= minColumn && columnNum <= maxColumn)
                            {
                                var midvalue = prop.GetMethod.Invoke(item, null);
                                if (midvalue is RamAccess<int?>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { int.Parse("") });
                                else if (midvalue is RamAccess<float?>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { float.Parse("") });
                                else if (midvalue is RamAccess<short>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { short.Parse("") });
                                else if (midvalue is RamAccess<short?>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { short.Parse("") });
                                else if (midvalue is RamAccess<int>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { int.Parse("") });
                                else if (midvalue is RamAccess<string>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { "" });
                                else if (midvalue is RamAccess<byte?>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { null });
                                else if (midvalue is RamAccess<bool>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { null });
                                else
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { "" });
                            }
                        }
                        catch (Exception e)
                        {
                            int k = 8;
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region CopyRows
    public ReactiveCommand<object, Unit> CopyRows { get; protected set; }
    private async Task _CopyRows(object _param)
    {
        object[] param = _param as object[];
        IKeyCollection collection = param[0] as IKeyCollection;
        int minColumn = Convert.ToInt32(param[1]) + 1;
        int maxColumn = Convert.ToInt32(param[2]) + 1;
        if (param[0] is Form1 || param[0] is Form2)
        {
            if (minColumn == 1) minColumn++;
        }
        string txt = "";

        Dictionary<long, Dictionary<int, string>> dic = new();

        foreach (IKey item in collection.GetEnumerable().OrderBy(x => x.Order))
        {
            dic.Add(item.Order, new Dictionary<int, string>());
            var dStructure = (IDataGridColumn)item;
            var findStructure = dStructure.GetColumnStructure();
            var Level = findStructure.Level;
            var tre = findStructure.GetLevel(Level - 1);
            var props = item.GetType().GetProperties();
            foreach (var prop in props)
            {
                var attr = (Form_PropertyAttribute)prop.GetCustomAttributes(typeof(Form_PropertyAttribute), false).FirstOrDefault();
                if (attr != null)
                {
                    var newNum = 0;
                    if (attr.Names.Count() > 1 && attr.Names[0] != "null-1-1" && attr.Names[0] == "Документ")
                    {
                        newNum = Convert.ToInt32(tre.Where(x => x.name == attr.Names[0]).FirstOrDefault().innertCol.Where(x => x.name == attr.Names[1]).FirstOrDefault().innertCol[0].name);
                    }
                    else if (attr.Names.Count() > 1 && attr.Names[0] != "null-1-1")
                    {
                        newNum = Convert.ToInt32(tre.Where(x => x.name == attr.Names[0]).FirstOrDefault().innertCol.Where(x => x.name == attr.Names[1]).FirstOrDefault().innertCol[0].name);
                    }
                    else
                    {
                        newNum = Convert.ToInt32(attr.Number);
                    }
                    //var numAttr = Convert.ToInt32(attr.Number);

                    try
                    {
                        var columnNum = newNum;
                        if (columnNum >= minColumn && columnNum <= maxColumn)
                        {
                            var midvalue = prop.GetMethod.Invoke(item, null);
                            var value = midvalue.GetType().GetProperty("Value").GetMethod.Invoke(midvalue, null);
                            if (value != null)
                            {
                                try
                                {
                                    if (dic[item.Order][columnNum] == "")
                                    {
                                        dic[item.Order][columnNum] = value.ToString();
                                    }
                                }
                                catch
                                {
                                    dic[item.Order].Add(columnNum, value.ToString());
                                }
                            }
                            else
                            {
                                dic[item.Order].Add(columnNum, "");
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        foreach (var item in dic.OrderBy(x => x.Key))
        {
            foreach (var it in item.Value.OrderBy(x => x.Key))
            {
                if (it.Value.Contains('\n') || it.Value.Contains('\r'))
                {
                    txt += $"\"{it.Value}\"\t";
                }
                else
                {
                    txt += $"{it.Value}\t";
                }
            }
            txt = txt.Remove(txt.Length - 1, 1);
            txt += "\n";
        }
        txt = txt.Remove(txt.Length - 1, 1);

        if (Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard clip)
        {
            await clip.ClearAsync();
            await clip.SetTextAsync(txt);
        }
    }
    #endregion

    #region PasteRows
    public ReactiveCommand<object, Unit> PasteRows { get; protected set; }
    private async Task _PasteRows(object _param)
    {
        object[] param = _param as object[];
        IKeyCollection collection = param[0] as IKeyCollection;
        int minColumn = Convert.ToInt32(param[1]) + 1;
        int maxColumn = Convert.ToInt32(param[2]) + 1;
        var collectionEn = collection.GetEnumerable();
        if (collectionEn.FirstOrDefault() is Form1 || collectionEn.FirstOrDefault() is Form2)
        {
            if (minColumn == 1) minColumn++;
        }

        if (Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard clip)
        {
            string? text = await clip.GetTextAsync();
            var rowsText = ParseInnerTextRows(text);

            foreach (IKey item in collectionEn.OrderBy(x => x.Order))
            {
                var props = item.GetType().GetProperties();

                var rowText = rowsText[item.Order - collectionEn.Min(x => x.Order)];
                if (Convert.ToInt32(param[1]) == 0 && !(collectionEn.FirstOrDefault() is Note))
                {
                    var newText = rowText.ToArray();
                    var count = 0;
                    foreach (char t in newText)
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
                    var attr = (Form_PropertyAttribute)prop.GetCustomAttributes(typeof(Form_PropertyAttribute), false).FirstOrDefault();
                    if (attr != null)
                    {
                        try
                        {
                            var columnNum = 0;
                            if (attr.Names.Count() > 1 && attr.Names.Count() != 4 && attr.Names[0] != "null-1-1" && attr.Names[0] != "Документ" && attr.Names[0] != "Сведения об операции")
                            {
                                columnNum = Convert.ToInt32(tre.Where(x => x.name == attr.Names[0]).FirstOrDefault().innertCol.Where(x => x.name == attr.Names[1]).FirstOrDefault().innertCol[0].name);
                            }
                            else if (attr.Names[0] == "Документ")
                            {
                                var findDock = tre.Where(x => x.name == "null-n");
                                if (findDock.Count() >= 1)
                                {
                                    columnNum = Convert.ToInt32(findDock.FirstOrDefault().innertCol.Where(x => x.name == attr.Names[0]).FirstOrDefault().innertCol.Where(x => x.name == attr.Names[1]).FirstOrDefault().innertCol[0].name);
                                }
                                else
                                    columnNum = Convert.ToInt32(tre.Where(x => x.name == attr.Names[0]).FirstOrDefault().innertCol.Where(x => x.name == attr.Names[1]).FirstOrDefault().innertCol[0].name);
                            }
                            else
                            {
                                columnNum = Convert.ToInt32(attr.Number);
                            }
                            //var columnNum = Convert.ToInt32(attr.Number);
                            if (columnNum >= minColumn && columnNum <= maxColumn)
                            {
                                var midvalue = prop.GetMethod.Invoke(item, null);
                                if (midvalue is RamAccess<int?>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { int.Parse(columnsText[columnNum - minColumn]) });
                                else if (midvalue is RamAccess<float?>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { float.Parse(columnsText[columnNum - minColumn]) });
                                else if (midvalue is RamAccess<short>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { short.Parse(columnsText[columnNum - minColumn]) });
                                else if (midvalue is RamAccess<short?>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { short.Parse(columnsText[columnNum - minColumn]) });
                                else if (midvalue is RamAccess<int>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { int.Parse(columnsText[columnNum - minColumn]) });
                                else if (midvalue is RamAccess<string>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { columnsText[columnNum - minColumn] });
                                else if (midvalue is RamAccess<byte?>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { byte.Parse(columnsText[columnNum - minColumn]) });
                                else if (midvalue is RamAccess<bool>)
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { bool.Parse(columnsText[columnNum - minColumn]) });
                                else
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { columnsText[columnNum - minColumn] });
                            }
                        }
                        catch (Exception e)
                        {
                            int k = 8;
                        }
                    }
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
            var answ = await ShowMessageT.Handle(new List<string> { "Вы действительно хотите удалить комментарий?", "Да", "Нет" });
            if (answ == "Да")
            {
                foreach (Note item in param)
                {
                    if (item != null)
                    {
                        foreach (Note it in Storage.Notes)
                        {
                            if (it.Order > item.Order)
                            {
                                it.Order -= 1;
                            }
                        }
                        foreach (Note nt in param)
                        {
                            Storage.Notes.Remove((Note)nt);
                        }
                    }
                }
                await Storage.SortAsync();
            }
        }
    }
    #endregion

    #region Pasport
    #region ExcelPasport
    public ReactiveCommand<object, Unit> ExcelPasport { get; protected set; }
    private async Task _ExcelPasport(object param)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            PasportUniqParam(param, out string? okpo, out string? type, out string? year, out string? pasNum, out string? factoryNum);
            SaveFileDialog saveFileDialog = new();
            FileDialogFilter filter = new() { Name = "Excel", Extensions = { "xlsx" } };
            saveFileDialog.Filters.Add(filter);
            var res = await saveFileDialog.ShowAsync(desktop.MainWindow);
            if (res != null)
            {
                if (res.Length != 0)
                {
                    var path = res;
                    if (!path.Contains(".xlsx"))
                    {
                        path += ".xlsx";
                    }
                    if (File.Exists(path))
                    {
                        try
                        {
                            File.Delete(path);
                        }
                        catch (Exception e)
                        {
                            await ShowMessageT.Handle(new List<string>
                            {
                                $"Не удалось сохранить файл по пути: {path}{Environment.NewLine}Файл с таким именем уже существует в этом расположении и используется другим процессом.", "Ок" });
                            return;
                        }
                    }
                    if (path != null)
                    {
                        using ExcelPackage excelPackage = new(new FileInfo(path));
                        excelPackage.Workbook.Properties.Author = "RAO_APP";
                        excelPackage.Workbook.Properties.Title = "Report";
                        excelPackage.Workbook.Properties.Created = DateTime.Now;
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add($"Операции с паспортом {pasNum}");

                        #region ColumnHeaders
                        worksheet.Cells[1, 1].Value = "Рег. №";
                        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                        worksheet.Cells[1, 3].Value = "ОКПО";
                        worksheet.Cells[1, 4].Value = "Форма";
                        worksheet.Cells[1, 5].Value = "Дата начала периода";
                        worksheet.Cells[1, 6].Value = "Дата конца периода";
                        worksheet.Cells[1, 7].Value = "Номер корректировки";
                        worksheet.Cells[1, 8].Value = "Количество строк";
                        worksheet.Cells[1, 9].Value = "№ п/п";
                        worksheet.Cells[1, 10].Value = "код";
                        worksheet.Cells[1, 11].Value = "дата";
                        worksheet.Cells[1, 12].Value = "номер паспорта (сертификата)";
                        worksheet.Cells[1, 13].Value = "тип";
                        worksheet.Cells[1, 14].Value = "радионуклиды";
                        worksheet.Cells[1, 15].Value = "номер";
                        worksheet.Cells[1, 16].Value = "количество, шт";
                        worksheet.Cells[1, 17].Value = "суммарная активность, Бк";
                        worksheet.Cells[1, 18].Value = "код ОКПО изготовителя";
                        worksheet.Cells[1, 19].Value = "дата выпуска";
                        worksheet.Cells[1, 20].Value = "категория";
                        worksheet.Cells[1, 21].Value = "НСС, мес";
                        worksheet.Cells[1, 22].Value = "код формы собственности";
                        worksheet.Cells[1, 23].Value = "код ОКПО правообладателя";
                        worksheet.Cells[1, 24].Value = "вид";
                        worksheet.Cells[1, 25].Value = "номер";
                        worksheet.Cells[1, 26].Value = "дата";
                        worksheet.Cells[1, 27].Value = "поставщика или получателя";
                        worksheet.Cells[1, 28].Value = "перевозчика";
                        worksheet.Cells[1, 29].Value = "наименование";
                        worksheet.Cells[1, 30].Value = "тип";
                        worksheet.Cells[1, 31].Value = "номер";
                        #endregion

                        int lastRow = 1;
                        foreach (Reports reps in LocalReports.Reports_Collection10)
                        {
                            var form11 = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.1") && x.Rows11 != null);
                            foreach (Report rep in form11)
                            {
                                var repPas = rep.Rows11.Where(x =>
                                    MainWindowVM.ComparePasParam(x.CreatorOKPO_DB, okpo)
                                    && MainWindowVM.ComparePasParam(x.Type_DB, type)
                                    && MainWindowVM.ComparePasParam(x.CreationDate_DB.Substring(Math.Max(0, x.CreationDate_DB.Length - 4)), year.Substring(Math.Max(0, year.Length - 4)))
                                    && MainWindowVM.ComparePasParam(x.PassportNumber_DB, pasNum)
                                    && MainWindowVM.ComparePasParam(x.FactoryNumber_DB, factoryNum));
                                foreach (Form11 repForm in repPas)
                                {
                                    if (lastRow == 1)
                                    {
                                        #region BindingCells
                                        worksheet.Cells[2, 1].Value = reps.Master.RegNoRep.Value;
                                        worksheet.Cells[2, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                                        worksheet.Cells[2, 3].Value = reps.Master.OkpoRep.Value;
                                        worksheet.Cells[2, 4].Value = rep.FormNum_DB;
                                        worksheet.Cells[2, 5].Value = rep.StartPeriod_DB;
                                        worksheet.Cells[2, 6].Value = rep.EndPeriod_DB;
                                        worksheet.Cells[2, 7].Value = rep.CorrectionNumber_DB;
                                        worksheet.Cells[2, 8].Value = rep.Rows.Count;
                                        worksheet.Cells[2, 9].Value = repForm.NumberInOrder_DB;
                                        worksheet.Cells[2, 10].Value = repForm.OperationCode_DB;
                                        worksheet.Cells[2, 11].Value = repForm.OperationDate_DB;
                                        worksheet.Cells[2, 12].Value = repForm.PassportNumber_DB;
                                        worksheet.Cells[2, 13].Value = repForm.Type_DB;
                                        worksheet.Cells[2, 14].Value = repForm.Radionuclids_DB;
                                        worksheet.Cells[2, 15].Value = repForm.FactoryNumber_DB;
                                        worksheet.Cells[2, 16].Value = repForm.Quantity_DB;
                                        worksheet.Cells[2, 17].Value = repForm.Activity_DB;
                                        worksheet.Cells[2, 18].Value = repForm.CreatorOKPO_DB;
                                        worksheet.Cells[2, 19].Value = repForm.CreationDate_DB;
                                        worksheet.Cells[2, 20].Value = repForm.Category_DB;
                                        worksheet.Cells[2, 21].Value = repForm.SignedServicePeriod_DB;
                                        worksheet.Cells[2, 22].Value = repForm.PropertyCode_DB;
                                        worksheet.Cells[2, 23].Value = repForm.Owner_DB;
                                        worksheet.Cells[2, 24].Value = repForm.DocumentVid_DB;
                                        worksheet.Cells[2, 25].Value = repForm.DocumentNumber_DB;
                                        worksheet.Cells[2, 26].Value = repForm.DocumentDate_DB;
                                        worksheet.Cells[2, 27].Value = repForm.ProviderOrRecieverOKPO_DB;
                                        worksheet.Cells[2, 28].Value = repForm.TransporterOKPO_DB;
                                        worksheet.Cells[2, 29].Value = repForm.PackName_DB;
                                        worksheet.Cells[2, 30].Value = repForm.PackType_DB;
                                        worksheet.Cells[2, 31].Value = repForm.PackNumber_DB;
                                        #endregion
                                    }
                                    for (int currentRow = lastRow; currentRow >= 2; currentRow--)
                                    {
                                        if (new CustomStringDateComparer(StringComparer.CurrentCulture).Compare(repForm.OperationDate_DB, (string)worksheet.Cells[currentRow, 11].Value) >= 0)
                                        {
                                            worksheet.InsertRow(currentRow + 1, 1);
                                            #region BindingCells
                                            worksheet.Cells[currentRow + 1, 1].Value = reps.Master.RegNoRep.Value;
                                            worksheet.Cells[currentRow + 1, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                                            worksheet.Cells[currentRow + 1, 3].Value = reps.Master.OkpoRep.Value;
                                            worksheet.Cells[currentRow + 1, 4].Value = rep.FormNum_DB;
                                            worksheet.Cells[currentRow + 1, 5].Value = rep.StartPeriod_DB;
                                            worksheet.Cells[currentRow + 1, 6].Value = rep.EndPeriod_DB;
                                            worksheet.Cells[currentRow + 1, 7].Value = rep.CorrectionNumber_DB;
                                            worksheet.Cells[currentRow + 1, 8].Value = rep.Rows.Count;
                                            worksheet.Cells[currentRow + 1, 9].Value = repForm.NumberInOrder_DB;
                                            worksheet.Cells[currentRow + 1, 10].Value = repForm.OperationCode_DB;
                                            worksheet.Cells[currentRow + 1, 11].Value = repForm.OperationDate_DB;
                                            worksheet.Cells[currentRow + 1, 12].Value = repForm.PassportNumber_DB;
                                            worksheet.Cells[currentRow + 1, 13].Value = repForm.Type_DB;
                                            worksheet.Cells[currentRow + 1, 14].Value = repForm.Radionuclids_DB;
                                            worksheet.Cells[currentRow + 1, 15].Value = repForm.FactoryNumber_DB;
                                            worksheet.Cells[currentRow + 1, 16].Value = repForm.Quantity_DB;
                                            worksheet.Cells[currentRow + 1, 17].Value = repForm.Activity_DB;
                                            worksheet.Cells[currentRow + 1, 18].Value = repForm.CreatorOKPO_DB;
                                            worksheet.Cells[currentRow + 1, 19].Value = repForm.CreationDate_DB;
                                            worksheet.Cells[currentRow + 1, 20].Value = repForm.Category_DB;
                                            worksheet.Cells[currentRow + 1, 21].Value = repForm.SignedServicePeriod_DB;
                                            worksheet.Cells[currentRow + 1, 22].Value = repForm.PropertyCode_DB;
                                            worksheet.Cells[currentRow + 1, 23].Value = repForm.Owner_DB;
                                            worksheet.Cells[currentRow + 1, 24].Value = repForm.DocumentVid_DB;
                                            worksheet.Cells[currentRow + 1, 25].Value = repForm.DocumentNumber_DB;
                                            worksheet.Cells[currentRow + 1, 26].Value = repForm.DocumentDate_DB;
                                            worksheet.Cells[currentRow + 1, 27].Value = repForm.ProviderOrRecieverOKPO_DB;
                                            worksheet.Cells[currentRow + 1, 28].Value = repForm.TransporterOKPO_DB;
                                            worksheet.Cells[currentRow + 1, 29].Value = repForm.PackName_DB;
                                            worksheet.Cells[currentRow + 1, 30].Value = repForm.PackType_DB;
                                            worksheet.Cells[currentRow + 1, 31].Value = repForm.PackNumber_DB;
                                            #endregion
                                            break;
                                        }
                                    }
                                    lastRow++;
                                }
                            }
                        }
                        try
                        {
                            excelPackage.Save();
                            res = await ShowMessageT.Handle(new List<string>
                            {
                                $"Выгрузка всех записей паспорта №{pasNum} сохранена по пути:{Environment.NewLine}{path}", "Ок", "Открыть выгрузку" });
                            if (res.Equals("Открыть выгрузку"))
                            {
                                ProcessStartInfo procInfo = new() { FileName = path, UseShellExecute = true };
                                Process.Start(procInfo);
                            }
                        }
                        catch (Exception e)
                        {
                            await ShowMessageT.Handle(new List<string>
                            {
                                $"Не удалось сохранить файл по пути: {path}{Environment.NewLine}Файл с таким именем уже существует в этом расположении и используется другим процессом.", "Ок" });
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region OpenPasport
    public ReactiveCommand<object, Unit> OpenPasport { get; protected set; }
    private async Task _OpenPasport(object param)
    {
        PasportUniqParam(param, out string? okpo, out string? type, out string? year, out string? pasNum, out string? factoryNum);
        string PasFolderPath = @"C:\Test\";
        string uniqPasName = $"{okpo}#{type}#{year}#{pasNum}#{factoryNum}.pdf";
        uniqPasName = Regex.Replace(uniqPasName, "[\\\\/:*?\"<>|]", "_");
        uniqPasName = Regex.Replace(uniqPasName, @"\s+", "");

        ProcessStartInfo procInfo = new() { FileName = PasFolderPath + uniqPasName, UseShellExecute = true };
        if (File.Exists(PasFolderPath + uniqPasName))
        {
            Process.Start(procInfo);
        }
        else if (File.Exists(PasFolderPath + TransliteToEng(uniqPasName)))
        {
            procInfo.FileName = PasFolderPath + TransliteToEng(uniqPasName);
            Process.Start(procInfo);
        }
        else if (File.Exists(PasFolderPath + TransliteToRus(uniqPasName)))
        {
            procInfo.FileName = PasFolderPath + TransliteToRus(uniqPasName);
            Process.Start(procInfo);
        }
        else
        {
            await ShowMessageT.Handle(new List<string> { "Паспорт отсутствует в сетевом хранилище", "Ок" });
        }
    }

    #region Translite
    public static string TransliteToEng(string pasName)
    {
        Dictionary<string, string> dictRusToEng = new()
        {
            {"а", "a"},
            {"А", "A"},
            {"е", "e"},
            {"Е", "E"},
            {"к", "k"},
            {"К", "K"},
            {"м", "m"},
            {"М", "M"},
            {"о", "o"},
            {"О", "O"},
            {"р", "p"},
            {"Р", "P"},
            {"с", "c"},
            {"С", "C"},
            {"Т", "T"},
            {"у", "y"},
            {"У", "Y"},
            {"х", "x"},
            {"Х", "X"},
        };
        string newPasName = "";
        foreach (var ch in pasName)
        {
            var ss = "";
            if (dictRusToEng.TryGetValue(ch.ToString(), out ss)) newPasName += ss;
            else newPasName += ch;
        }
        return newPasName;
    }

    public static string TransliteToRus(string pasName)
    {
        Dictionary<string, string> dictEngToRus = new()
        {
            {"a", "а"},
            {"A", "А"},
            {"e", "е"},
            {"E", "Е"},
            {"k", "к"},
            {"K", "К"},
            {"m", "м"},
            {"M", "М"},
            {"o", "о"},
            {"O", "О"},
            {"p", "р"},
            {"P", "Р"},
            {"c", "с"},
            {"C", "С"},
            {"T", "Т"},
            {"y", "у"},
            {"Y", "У"},
            {"x", "х"},
            {"X", "Х"},
        };
        string newPasName = "";
        foreach (var ch in pasName)
        {
            var ss = "";
            if (dictEngToRus.TryGetValue(ch.ToString(), out ss)) newPasName += ss;
            else newPasName += ch;
        }
        return newPasName;
    }
    #endregion
    #endregion

    #region CopyPasName
    public ReactiveCommand<object, Unit> CopyPasName { get; protected set; }
    private async Task _CopyPasName(object param)
    {
        PasportUniqParam(param, out string? okpo, out string? type, out string? year, out string? pasNum, out string? factoryNum);
        string uniqPasName = $"{okpo}#{type}#{year}#{pasNum}#{factoryNum}";

        uniqPasName = Regex.Replace(uniqPasName, "[\\\\/:*?\"<>|]", "_");
        uniqPasName = Regex.Replace(uniqPasName, "\\s+", "");
        await Application.Current.Clipboard.SetTextAsync(uniqPasName);
    }
    #endregion

    #region PasportUniqParam
    private void PasportUniqParam(object param, out string? okpo, out string? type, out string? year, out string? pasNum, out string? factoryNum)
    {
        object[] par = param as object[];
        IKeyCollection collection = par[0] as IKeyCollection;
        var item = collection.GetEnumerable().OrderBy(x => x.Order).FirstOrDefault();
        var dStructure = (IDataGridColumn)item;
        var findStructure = dStructure.GetColumnStructure();
        var Level = findStructure.Level;
        var tre = findStructure.GetLevel(Level - 1);
        var props = item.GetType().GetProperties();

        okpo = "";
        type = "";
        year = "";
        pasNum = "";
        factoryNum = "";

        foreach (var prop in props)
        {
            var attr = (Form_PropertyAttribute)prop.GetCustomAttributes(typeof(Form_PropertyAttribute), false).FirstOrDefault();
            if (attr != null && attr.Names.Count() > 1 && attr.Names[0] == "Сведения из паспорта (сертификата) на закрытый радионуклидный источник")
            {
                if (attr.Names[1] == "код ОКПО изготовителя")
                {
                    var midvalue = prop.GetMethod.Invoke(item, null);
                    okpo = midvalue.GetType().GetProperty("Value").GetMethod.Invoke(midvalue, null).ToString();
                }
                if (attr.Names[1] == "тип")
                {
                    var midvalue = prop.GetMethod.Invoke(item, null);
                    type = midvalue.GetType().GetProperty("Value").GetMethod.Invoke(midvalue, null).ToString();
                }
                if (attr.Names[1] == "дата выпуска")
                {
                    var midvalue = prop.GetMethod.Invoke(item, null);
                    year = midvalue.GetType().GetProperty("Value").GetMethod.Invoke(midvalue, null).ToString();

                }
                if (attr.Names[1] == "номер паспорта (сертификата)")
                {
                    var midvalue = prop.GetMethod.Invoke(item, null);
                    pasNum = midvalue.GetType().GetProperty("Value").GetMethod.Invoke(midvalue, null).ToString();
                }

                if (attr.Names[1] == "номер")
                {
                    var midvalue = prop.GetMethod.Invoke(item, null);
                    factoryNum = midvalue.GetType().GetProperty("Value").GetMethod.Invoke(midvalue, null).ToString();
                }
            }
        }
    }
    #endregion
    #endregion

    #region CustomStringDateComparer
    private class CustomStringDateComparer : IComparer<string>
    {
        public CustomStringDateComparer(IComparer<string> baseComparer) { }
        public int Compare(string? date1, string? date2)
        {
            if (string.IsNullOrEmpty(date1))
                return 1;
            if (string.IsNullOrEmpty(date2))
                return -1;
            Regex r = new(@"^(\d{1,2})\.(\d{1,2})\.(\d{2,4})$");
            if (r.IsMatch(date1) && r.IsMatch(date2))
            {
                date1 = StringDateReverse(date1);
                date2 = StringDateReverse(date2);
                return string.CompareOrdinal(date1, date2);
            }
            if (!r.IsMatch(date1) && !r.IsMatch(date2))
                return string.CompareOrdinal(date1, date2);
            if (!r.IsMatch(date2))
                return 1;
            return -1;
        }
    }

    private static string StringDateReverse(string date)
    {
        var charArray = date.Replace("_", "0").Replace("/", ".").Split(".");
        if (charArray[0].Length == 1)
            charArray[0] = $"0{charArray[0]}";
        if (charArray[1].Length == 1)
            charArray[1] = $"0{charArray[0]}";
        if (charArray[2].Length == 2)
            charArray[2] = $"20{charArray[0]}";
        Array.Reverse(charArray);
        return string.Join("", charArray);
    }
    #endregion

    #region CopyExecutorData
    public ReactiveCommand<object, Unit> CopyExecutorData { get; protected set; }
    private async Task _CopyExecutorData(object param)
    {
        var comparator = new CustomStringDateComparer(StringComparer.CurrentCulture);
        var lastReportWithExecutor = Storages.Report_Collection
            .Where(rep => rep.FormNum_DB == FormType
                          && !rep.Equals(Storage)
                          && (!string.IsNullOrEmpty(rep.FIOexecutor_DB) && rep.FIOexecutor_DB != "-" 
                          || !string.IsNullOrEmpty(rep.ExecEmail_DB) && rep.ExecEmail_DB != "-"
                          || !string.IsNullOrEmpty(rep.ExecPhone_DB) && rep.ExecPhone_DB != "-"
                          || !string.IsNullOrEmpty(rep.GradeExecutor_DB) && rep.GradeExecutor_DB != "-"))
            .MaxBy(rep => rep.EndPeriod_DB, comparator);

        var lastReport = Storages.Report_Collection
            .Where(rep => rep.FormNum_DB.Equals(FormType) && !rep.Equals(Storage))
            .MaxBy(rep => rep.EndPeriod_DB, comparator);

        if (lastReportWithExecutor is null)
        {
            #region ShowMessageMissingExecutorData
            var orgName = "данной организации";
            if ((FormType.ToCharArray()[0]) == '1')
            {
                if (!string.IsNullOrEmpty(Storages.Master_DB.Rows10[1].ShortJurLico_DB) && Storages.Master_DB.Rows10[1].ShortJurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows10[1].ShortJurLico_DB;
                else if (!string.IsNullOrEmpty(Storages.Master_DB.Rows10[1].JurLico_DB) && Storages.Master_DB.Rows10[1].JurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows10[1].JurLico_DB;
                else if (!string.IsNullOrEmpty(Storages.Master_DB.Rows10[0].ShortJurLico_DB) && Storages.Master_DB.Rows10[0].ShortJurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows10[1].ShortJurLico_DB;
                else if (!string.IsNullOrEmpty(Storages.Master_DB.Rows10[0].JurLico_DB) && Storages.Master_DB.Rows10[0].JurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows10[1].JurLico_DB;
            }
            if ((FormType.ToCharArray()[0]) == '2')
            {
                if (!string.IsNullOrEmpty(Storages.Master_DB.Rows20[1].ShortJurLico_DB) && Storages.Master_DB.Rows20[1].ShortJurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows20[1].ShortJurLico_DB;
                else if (!string.IsNullOrEmpty(Storages.Master_DB.Rows20[1].JurLico_DB) && Storages.Master_DB.Rows20[1].JurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows20[1].JurLico_DB;
                else if (!string.IsNullOrEmpty(Storages.Master_DB.Rows20[0].ShortJurLico_DB) && Storages.Master_DB.Rows20[0].ShortJurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows20[1].ShortJurLico_DB;
                else if (!string.IsNullOrEmpty(Storages.Master_DB.Rows20[0].JurLico_DB) && Storages.Master_DB.Rows20[0].JurLico_DB != "-")
                    orgName = Storages.Master_DB.Rows20[1].JurLico_DB;
            }
            var msg = lastReport is null
                ? $"У {orgName}" + Environment.NewLine + $"отсутствуют другие формы {FormType}"
                : $"У {orgName}" + Environment.NewLine + $"в формах {FormType} не заполнены данные исполнителя";
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Уведомление", msg)
                    .ShowDialog(desktop.MainWindow.OwnedWindows.First());
            }
            #endregion
            return;
        } 
        Storage.FIOexecutor.Value = lastReportWithExecutor.FIOexecutor_DB;
        Storage.ExecEmail.Value = lastReportWithExecutor.ExecEmail_DB;
        Storage.ExecPhone.Value = lastReportWithExecutor.ExecPhone_DB;
        Storage.GradeExecutor.Value = lastReportWithExecutor.GradeExecutor_DB;
    }
    #endregion

    #region Constacture
    public ChangeOrCreateVM(string param, in Report rep, Reports reps, DBObservable localReports)
    {
        Storage = rep;
        Storages = reps;
        FormType = param;
        LocalReports = localReports;
        var sumR21 = rep.Rows21.Where(x => x.Sum_DB == true || x.SumGroup_DB == true).Count();
        var sumR22 = rep.Rows22.Where(x => x.Sum_DB == true || x.SumGroup_DB == true).Count();
        if (sumR21 > 0 || sumR22 > 0)
        {
            isSum = true;
        }
        else
        {
            isSum = false;
        }
        Init();
    }
    public ChangeOrCreateVM(string param, in Reports reps)
    {
        Storage = new Report
        {
            FormNum_DB = param
        };

        if (param.Split('.')[0] == "1")
        {
            if (param != "1.0")
            {
                try
                {
                    var ty = (from Report t in reps.Report_Collection where t.FormNum_DB == param && t.EndPeriod_DB != "" orderby DateTimeOffset.Parse(t.EndPeriod_DB) select t.EndPeriod_DB).LastOrDefault();

                    FormType = param;
                    Storage.StartPeriod.Value = ty;
                }
                catch
                {

                }
            }
        }
        else
        {
            if (param != "2.0")
            {
                try
                {
                    var ty = (from Report t in reps.Report_Collection where t.FormNum_DB == param && t.Year_DB != null orderby t.Year_DB select t.Year_DB).LastOrDefault();

                    FormType = param;
                    if (ty != null)
                    {
                        int ty_int = Convert.ToInt32(ty) + 1;
                        string ty_str = Convert.ToString(ty_int);
                        Storage.Year.Value = ty_str;
                    }
                }
                catch
                {

                }
            }
        }

        if (param == "1.0")
        {
            var ty1 = (Form10)FormCreator.Create(param);
            ty1.NumberInOrder_DB = 1;
            var ty2 = (Form10)FormCreator.Create(param);
            ty2.NumberInOrder_DB = 2;
            Storage.Rows10.Add(ty1);
            Storage.Rows10.Add(ty2);
        }
        if (param == "2.0")
        {
            var ty1 = (Form20)FormCreator.Create(param);
            ty1.NumberInOrder_DB = 1;
            var ty2 = (Form20)FormCreator.Create(param);
            ty2.NumberInOrder_DB = 2;
            Storage.Rows20.Add(ty1);
            Storage.Rows20.Add(ty2);
        }
        Storages = reps;
        FormType = param;
        Init();
    }
    public ChangeOrCreateVM(string param, in DBObservable reps)
    {
        Storage = new Report
        {
            FormNum_DB = param
        };

        if (param == "1.0")
        {
            var ty1 = (Form10)FormCreator.Create(param);
            ty1.NumberInOrder_DB = 1;
            var ty2 = (Form10)FormCreator.Create(param);
            ty2.NumberInOrder_DB = 2;
            Storage.Rows10.Add(ty1);
            Storage.Rows10.Add(ty2);
        }
        if (param == "2.0")
        {
            var ty1 = (Form20)FormCreator.Create(param);
            ty1.NumberInOrder_DB = 1;
            var ty2 = (Form20)FormCreator.Create(param);
            ty2.NumberInOrder_DB = 2;
            Storage.Rows20.Add(ty1);
            Storage.Rows20.Add(ty2);
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

    public void Init()
    {
        string a = FormType.Replace(".", "");
        if ((FormType.Split('.')[1] != "0" && FormType.Split('.')[0] == "1") || (FormType.Split('.')[1] != "0" && FormType.Split('.')[0] == "2"))
        {
            WindowHeader =
                $"{((Form_ClassAttribute)Type.GetType($"Models.Form{a},Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name} {Storages.Master_DB.RegNoRep.Value} {Storages.Master_DB.ShortJurLicoRep.Value} {Storages.Master_DB.OkpoRep.Value}";
        }
        if (FormType is "1.0" or "2.0")
        {
            WindowHeader = ((Form_ClassAttribute)Type.GetType($"Models.Form{a},Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
        }
        AddRow = ReactiveCommand.CreateFromTask<object>(_AddRow);
        AddRowIn = ReactiveCommand.CreateFromTask<object>(_AddRowIn);
        DeleteRow = ReactiveCommand.CreateFromTask<object>(_DeleteRow);
        ChangeReportOrder = ReactiveCommand.CreateFromTask(_ChangeReportOrder);
        CheckReport = ReactiveCommand.Create(_CheckReport);
        PasteRows = ReactiveCommand.CreateFromTask<object>(_PasteRows);
        DuplicateRowsx1 = ReactiveCommand.CreateFromTask<object>(_DuplicateRowsx1);
        CopyRows = ReactiveCommand.CreateFromTask<object>(_CopyRows);
        AddNote = ReactiveCommand.CreateFromTask<object>(_AddNote);
        DeleteNote = ReactiveCommand.CreateFromTask<object>(_DeleteNote);
        DuplicateNotes = ReactiveCommand.CreateFromTask<object>(_DuplicateNotes);
        SetNumberOrder = ReactiveCommand.CreateFromTask<object>(_SetNumberOrder);
        DeleteDataInRows = ReactiveCommand.CreateFromTask<object>(_DeleteDataInRows);
        OpenPasport = ReactiveCommand.CreateFromTask<object>(_OpenPasport);
        ExcelPasport = ReactiveCommand.CreateFromTask<object>(_ExcelPasport);
        CopyPasName = ReactiveCommand.CreateFromTask<object>(_CopyPasName);
        CopyExecutorData = ReactiveCommand.CreateFromTask<object>(_CopyExecutorData);


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

    private bool IsCanSaveReportEnabled
    {
        get => _isCanSaveReportEnabled;
        set
        {
            if (value == _isCanSaveReportEnabled)
            {
                return;
            }

            _isCanSaveReportEnabled = value;
            PropertyChanged?
                .Invoke(this, new PropertyChangedEventArgs(nameof(IsCanSaveReportEnabled)));
        }
    }

    [DependsOn(nameof(IsCanSaveReportEnabled))]
    #endregion

    private bool CanSaveReport(object parameter)
    {
        return _isCanSaveReportEnabled;
    }

    public void SaveReport()
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (DBO != null)
            {
                var tmp = new Reports
                {
                    Master = Storage
                };
                if (tmp.Master.Rows10.Count != 0)
                {
                    tmp.Master.Rows10[1].OrganUprav.Value = tmp.Master.Rows10[0].OrganUprav.Value;
                    tmp.Master.Rows10[1].RegNo.Value = tmp.Master.Rows10[0].RegNo.Value;
                }
                if (tmp.Master.Rows20.Count != 0)
                {
                    tmp.Master.Rows20[1].OrganUprav.Value = tmp.Master.Rows20[0].OrganUprav.Value;
                    tmp.Master.Rows20[1].RegNo.Value = tmp.Master.Rows20[0].RegNo.Value;
                }

                DBO.Reports_Collection.Add(tmp);
                DBO = null;
            }
            else
            {
                if (Storages != null)
                {
                    if (FormType != "1.0" && FormType != "2.0")
                    {
                        if (!Storages.Report_Collection.Contains(Storage))
                        {
                            Storages.Report_Collection.Add(Storage);
                        }
                    }
                }
                else
                {

                }
            }
            if (Storages != null)
            {
                if (Storages.Master.Rows10.Count != 0)
                {
                    Storages.Master.Rows10[1].OrganUprav.Value = Storages.Master.Rows10[0].OrganUprav.Value;
                    Storages.Master.Rows10[1].RegNo.Value = Storages.Master.Rows10[0].RegNo.Value;
                }
                if (Storages.Master.Rows20.Count != 0)
                {
                    Storages.Master.Rows20[1].OrganUprav.Value = Storages.Master.Rows20[0].OrganUprav.Value;
                    Storages.Master.Rows20[1].RegNo.Value = Storages.Master.Rows20[0].RegNo.Value;
                }
                Storages.Report_Collection.Sorted = false;
                Storages.Report_Collection.QuickSort();
            }

            //Storages.Report_Collection.Sorted = false;
            //Storages.Report_Collection.QuickSort();
        }

        var dbm = StaticConfiguration.DBModel;
        dbm.SaveChanges();
        IsCanSaveReportEnabled = false;
    }

    //public void _AddRow10()
    //{
    //    Form10? frm = new Form10(); Storage.Rows10.Add(frm); Storage.LastAddedForm = Report.Forms.Form10;
    //}
    //public void _AddRow20()
    //{
    //    Form20? frm = new Form20(); Storage.Rows20.Add(frm); Storage.LastAddedForm = Report.Forms.Form20;
    //}

    int GetNumberInOrder(IKeyCollection lst)
    {
        int maxNum = 0;

        foreach (var item in lst)
        {
            var frm = (INumberInOrder)item;
            if (frm.Order >= maxNum)
            {
                maxNum++;
            }
        }

        return maxNum + 1;
    }

    #region ParseInnerText
    private string[] ParseInnerTextRows(string Text)
    {
        List<string> lst = new();

        bool comaFlag = false;
        Text = Text.Replace("\r\n", "\n");

        string txt = "";
        foreach (char item in Text)
        {
            if (item == '\"')
            {
                txt += item;
                comaFlag = !comaFlag;
            }
            else
            {
                if (item == '\n')//||(item=='\t'))
                {
                    if (!comaFlag)
                    {
                        lst.Add(txt);
                        txt = "";
                    }
                    else
                    {
                        txt += item;
                    }
                }
                else
                {
                    txt += item;
                }
            }
        }
        if (txt != "")
        {
            lst.Add(txt);
        }
        lst.Add("");
        return lst.ToArray();
    }
    private string[] ParseInnerTextColumn(string Text)
    {
        List<string> lst = new();

        bool comaFlag = false;

        string txt = "";
        foreach (char item in Text)
        {
            if (item == '\"')
            {
                comaFlag = true;
            }
            else
            {
                if (item == '\t')
                {
                    if (!comaFlag)
                    {
                        lst.Add(txt);
                        txt = "";
                    }
                    else
                    {
                        txt += item;
                    }
                }
                else
                {
                    txt += item;
                }
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