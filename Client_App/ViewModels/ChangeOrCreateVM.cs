using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Metadata;
using Models.Collections;
using Models;
using ReactiveUI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using Models.DataAccess;
using Avalonia.LogicalTree;
using Client_App.Controls.DataGrid;
using Models.DBRealization;
using System;
using Models.Attributes;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Models.Abstracts;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using Avalonia;
using Client_App.Views;
using DynamicData;
using OfficeOpenXml;
using Spravochniki;
using System.IO;
using System.Threading;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using System.Reactive.Linq;
using Avalonia.Media;
using Models.DataAccess;
using System.Globalization;
using Models.Classes;

namespace Client_App.ViewModels
{
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
        private bool _isSum = false;
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

            foreach (var itemT in tItems)
            {
                var sums = itemT.Where(x => (x as Form21).Sum_DB == true).FirstOrDefault();

                Form21 sumRow = sums as Form21;
                if ((itemT.Count() > 1 && sumRow == null) || (itemT.Count() > 2 && sumRow != null))
                {
                    if (sumRow == null)
                    {
                        var first = itemT.FirstOrDefault() as Form21;
                        sumRow = (Form21)FormCreator.Create("2.1");

                        sumRow.RefineMachineName_Hidden_Set = new Models.DataAccess.RefBool(false);
                        sumRow.MachineCode_Hidden_Set = new Models.DataAccess.RefBool(false);
                        sumRow.MachinePower_Hidden_Set = new Models.DataAccess.RefBool(false);
                        sumRow.NumberOfHoursPerYear_Hidden_Set = new Models.DataAccess.RefBool(false);

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

                    List<Form21> lst = new List<Form21>();
                    var u = itemT.OrderBy(x => (x as Form21).NumberInOrder_DB);
                    foreach (var itemThread in u)
                    {
                        var form = itemThread;
                        form.SumGroup_DB = true;

                        form.RefineMachineName_Hidden_Set=new Models.DataAccess.RefBool(false);
                        form.MachineCode_Hidden_Set = new Models.DataAccess.RefBool(false);
                        form.MachinePower_Hidden_Set = new Models.DataAccess.RefBool(false);
                        form.NumberOfHoursPerYear_Hidden_Set = new Models.DataAccess.RefBool(false);

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
                            var form = (t as Form21);
                            //form.RefineMachineName_Hidden_Get.Set(true);
                            //form.MachineCode_Hidden_Get.Set(true);
                            //form.MachinePower_Hidden_Get.Set(true);
                            //form.NumberOfHoursPerYear_Hidden_Get.Set(true);
                            ito[itemT.Key].Add(t);
                        }
                    }
                }
            }

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

            foreach (var itemT in tItems)
            {
                var sums = itemT.Where(x => (x as Form22).Sum_DB == true).FirstOrDefault();

                Form22 sumRow = sums as Form22;
                if ((itemT.Count() > 1 && sumRow == null) || (itemT.Count() > 2 && sumRow != null))
                {
                    if (sumRow == null)
                    {
                        var first = itemT.FirstOrDefault() as Form22;
                        sumRow = (Form22)FormCreator.Create("2.2");

                        sumRow.StoragePlaceName_Hidden_Set = new Models.DataAccess.RefBool(false);
                        sumRow.StoragePlaceCode_Hidden_Set = new Models.DataAccess.RefBool(false);
                        sumRow.PackName_Hidden_Set = new Models.DataAccess.RefBool(false);
                        sumRow.PackType_Hidden_Set = new Models.DataAccess.RefBool(false);

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

                    List<Form22> lst = new List<Form22>();
                    var u = itemT.OrderBy(x => (x as Form22).NumberInOrder_DB);
                    foreach (var itemThread in u)
                    {
                        var form = itemThread;

                        form.VolumeInPack_Hidden = true;
                        form.MassInPack_Hidden = true;

                        form.SumGroup_DB = true;
                        form.StoragePlaceName_Hidden_Set = new Models.DataAccess.RefBool(false);
                        form.StoragePlaceCode_Hidden_Set = new Models.DataAccess.RefBool(false);
                        form.PackName_Hidden_Set = new Models.DataAccess.RefBool(false);
                        form.PackType_Hidden_Set = new Models.DataAccess.RefBool(false);

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
                            var form = (t as Form22);
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
            }

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
                        if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
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
                        if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
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
        }
        #endregion

        #region AddSort
        public ReactiveCommand<string, Unit> AddSort { get; protected set; }
        #endregion

        #region AddNote
        public ReactiveCommand<object, Unit> AddNote { get; protected set; }
        private async Task _AddNote(object param)
        {
            Note? nt = new Note();
            nt.Order = GetNumberInOrder(Storage.Notes);
            Storage.Notes.Add(nt);
            await Storage.SortAsync();
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
                List<Models.Abstracts.Form> lst = new List<Models.Abstracts.Form>();
                foreach (object? item1 in param)
                {
                    lst.Add((Models.Abstracts.Form)item1);
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

                        List<Form> _lst = new List<Form>();
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
                var answ = await ShowMessageT.Handle(new List<string>() { "Вы действительно хотите удалить строчку?", "Да", "Нет" });
                if (answ == "Да")
                {
                    var lst = new List<IKey>(Storage.Rows.GetEnumerable());
                    var countParam = param;
                    var minItem = param.Min(x=>x.Order);
                    var maxItem = param.Max(x=>x.Order);
                    foreach (Form item in param) 
                    {
                        if (item != null)
                        {
                            Storage.Rows.Remove(item);
                        }
                    }
                    var itemQ = lst.Where(x=>x.Order>maxItem);
                    foreach(var item in itemQ)
                    {
                        item.SetOrder(minItem);
                        minItem++;
                    }
                }
                await Storage.SortAsync();
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
                        var frm = new Note();
                        frm.Order = r;
                        Storage.Notes.Add(frm);
                        r++;
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
            if ((param[0] is Form1) || (param[0] is Form2))
            {
                if (minColumn == 1) minColumn++;
            }
            string txt = "";

            Dictionary<long, Dictionary<int, string>> dic = new Dictionary<long, Dictionary<int, string>>();

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
                        if (attr.Names.Count() > 1 && attr.Names[0] != "null-1-1")
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
                                    dic[item.Order].Add(columnNum, value.ToString());
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
                        txt += "\"" + it.Value + "\"" + "\t";
                    }
                    else
                    {
                        txt += it.Value + "\t";
                    }
                }
                txt = txt.Remove(txt.Length - 1, 1);
                txt += "\n";
            }
            txt = txt.Remove(txt.Length - 1, 1);

            if (Avalonia.Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard clip)
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
            if ((collectionEn.FirstOrDefault() is Form1) || (collectionEn.FirstOrDefault() is Form2))
            {
                if (minColumn == 1) minColumn++;
            }

            if (Avalonia.Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard clip)
            {
                string? text = await clip.GetTextAsync();
                var rowsText = ParseInnerTextRows(text);

                foreach (IKey item in collectionEn.OrderBy(x => x.Order))
                {
                    var props = item.GetType().GetProperties();

                    var rowText = rowsText[item.Order - collectionEn.Min(x => x.Order)];
                    if (Convert.ToInt32(param[1]) == 0 && !(collectionEn.FirstOrDefault() is Note))
                    {
                        rowText = rowText.Remove(0, 2);
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
                                if (attr.Names.Count() > 1 && attr.Names[0] != "null-1-1")
                                {
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
                                    else if(midvalue is RamAccess<byte?>)
                                        midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { byte.Parse(columnsText[columnNum - minColumn]) });
                                    else if (midvalue is RamAccess<bool>)
                                        midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { bool.Parse(columnsText[columnNum - minColumn]) });
                                    else
                                        midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { columnsText[columnNum - minColumn] });
                                }
                            }
                            catch(Exception e)
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
                var answ = await ShowMessageT.Handle(new List<string>() { "Вы действительно хотите удалить комментарий?", "Да", "Нет" });
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
                                    it.Order = it.Order - 1;
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

        #region Constacture
        public ChangeOrCreateVM(string param, in Report rep,Reports reps)
        {
            Storage = rep;
            Storages = reps;
            FormType = param;
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
            Storage = new Report()
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
            Storage = new Report()
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
            if ((FormType.Split('.')[1] != "0" && FormType.Split('.')[0] == "1")||(FormType.Split('.')[1] != "0" && FormType.Split('.')[0] == "2"))
            {
                WindowHeader = ((Form_ClassAttribute)Type.GetType("Models.Form" + a + ",Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name +
                    " " + Storages.Master_DB.RegNoRep.Value + " " + Storages.Master_DB.ShortJurLicoRep.Value + " " + Storages.Master_DB.OkpoRep.Value;
            }
            if(FormType=="1.0"||FormType=="2.0")
            {
                WindowHeader = ((Form_ClassAttribute)Type.GetType("Models.Form" + a + ",Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
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

            ShowDialog = new Interaction<object,int>();
            ShowDialogIn = new Interaction<int, int>();
            ShowMessageT = new Interaction<List<string>, string>();
            if (!isSum)
            {
                Storage.Sort();
            }
        }

        #region IsCanSaveReportEnabled
        private bool _isCanSaveReportEnabled = false;

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
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (DBO != null)
                {
                    var tmp = new Reports();
                    tmp.Master = Storage;
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
            List<string> lst = new List<string>();

            bool comaFlag = false;
            Text = Text.Replace("\r\n","\n");

            string txt = "";
            foreach(char item in Text)
            {
                if(item=='\"')
                {
                    txt += item;
                    comaFlag = !comaFlag;
                }
                else
                {
                    if((item=='\n'))//||(item=='\t'))
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
            if(txt!="")
            {
                lst.Add(txt);
            }
            lst.Add("");
            return lst.ToArray();
        }
        private string[] ParseInnerTextColumn(string Text)
        {
            List<string> lst = new List<string>();

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
            if(txt!="")
            {
                lst.Add(txt);
            }
            
            return lst.ToArray();
        }
        #endregion
    }
}
