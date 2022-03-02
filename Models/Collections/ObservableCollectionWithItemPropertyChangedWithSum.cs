using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Models;
using Models.Collections;
using OfficeOpenXml;
using Models.Abstracts;

namespace Models.Collections
{
    public class ObservableCollectionWithItemPropertyChangedWithSum<T> : ObservableCollectionWithItemPropertyChanged<T>
        where T : class, IKey
    {

        public ObservableCollectionWithItemPropertyChangedWithSum() : base()
        {
        }

        public ObservableCollectionWithItemPropertyChangedWithSum(List<T> list) : base(list)
        {
            ObserveAll();
        }

        public ObservableCollectionWithItemPropertyChangedWithSum(IEnumerable<T> enumerable) : base(enumerable)
        {
            ObserveAll();
        }
        public void Sum()
        {
            if (Items.Count > 0)
            {
                if (Items[0] is Form21)
                {
                    Form21();
                }

                if (Items[0] is Form22)
                {
                    Form22();
                }
            }
        }

        private void Form21()
        {
            var tItems = Items.GroupBy(x=>(x as Form21).RefineMachineName_DB+
                                          (x as Form21).MachineCode_DB +
                                          (x as Form21).MachinePower_DB +
                                          (x as Form21).NumberOfHoursPerYear_DB).ToList();
            var y = tItems.ToList();

            var ito = new Dictionary<string,List<T>>();
            foreach (var item in y)
            {
                ito.Add(item.Key,new List<T>());
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
            List<Thread> lsth = new List<Thread>();
            foreach (var itemT in tItems)
            {
                //var tr = new Thread(x =>
                //{
                var x = itemT;
                var ty = (x as IGrouping<string, T>);
                var sums = ty.Where(x => (x as Form21).Sum_DB == true).FirstOrDefault();

                Form21 sumRow = sums as Form21;
                if ((itemT.Count() > 1 && sumRow == null) || (itemT.Count() > 2 && sumRow != null))
                {
                    if (sumRow == null)
                    {
                        var first = ty.FirstOrDefault() as Form21;
                        sumRow = (Form21)FormCreator.Create("2.1");

                        sumRow.RefineMachineName_Hidden_Set = new DataAccess.RefBool(false);
                        sumRow.MachineCode_Hidden_Set = new DataAccess.RefBool(false);
                        sumRow.MachinePower_Hidden_Set = new DataAccess.RefBool(false);
                        sumRow.NumberOfHoursPerYear_Hidden_Set = new DataAccess.RefBool(false);

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
                        sumRow.BaseColor = Interfaces.ColorType.Green;

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

                    List<T> lst = new List<T>();
                    var u = ty.OrderBy(x => (x as Form21).NumberInOrder_DB);
                    foreach (var itemThread in u)
                    {
                        var item = itemThread;
                        var form = item as Form21;
                        if (form.Sum_DB != true)
                        {
                            form.RefineMachineName_Hidden_Get.Set(false);
                            form.MachineCode_Hidden_Get.Set(false);
                            form.MachinePower_Hidden_Get.Set(false);
                            form.NumberOfHoursPerYear_Hidden_Get.Set(false);
                            form.BaseColor = Interfaces.ColorType.Yellow;

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

                            lst.Add(form as T);
                        }
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

                    lock (ito)
                    {
                        ito[itemT.Key].Add(sumRow as T);
                        foreach (var r in lst)
                        {
                            ito[itemT.Key].Add(r);
                        }
                    }
                }
                else
                {
                    lock (ito)
                    {
                        foreach (var t in ty)
                        {
                            if ((t as Form21).Sum_DB != true)
                            {
                                var form = (t as Form21);
                                form.RefineMachineName_Hidden_Get.Set(true);
                                form.MachineCode_Hidden_Get.Set(true);
                                form.MachinePower_Hidden_Get.Set(true);
                                form.NumberOfHoursPerYear_Hidden_Get.Set(true);
                                ito[itemT.Key].Add(t);
                            }
                        }
                    }
                }
                //});
                //tr.Start(itemT);
                //lsth.Add(tr);
            }

            foreach (var item in lsth)
            {
                item.Join();
            }

            this.ClearItems();
            var yu = ito.OrderBy(x => x.Key);
            var count = 1;
            foreach (var item in yu)
            {
                var o=item.Value as List<Form21>;
                foreach(var i in o)
                {
                    i.NumberInOrder.Value = count;
                    count++;
                }

                this.AddRange(item.Value);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
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

        private void Form22()
        {
            var tItems = Items.GroupBy(x => (x as Form22).StoragePlaceName_DB +
                              (x as Form22).StoragePlaceCode_DB +
                              (x as Form22).PackName_DB +
                              (x as Form22).PackType_DB).ToList();
            var y = tItems.ToList();

            var ito = new Dictionary<string, List<T>>();
            foreach (var item in y)
            {
                ito.Add(item.Key, new List<T>());
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
            List<Thread> lsth = new List<Thread>();
            foreach (var itemT in tItems)
            {
                var tr = new Thread(x =>
                {
                    var ty = x as IGrouping<string, T>;
                    var sums = ty.Where(x => (x as Form22).Sum_DB == true).FirstOrDefault();

                    Form22 sumRow = sums as Form22;
                    if ((itemT.Count() > 1 && sumRow == null) || (itemT.Count() > 2 && sumRow != null))
                    {
                        if (sumRow == null)
                        {
                            var first = ty.FirstOrDefault() as Form22;
                            sumRow = (Form22) FormCreator.Create("2.2");

                            sumRow.StoragePlaceName_DB = first.StoragePlaceName_DB;
                            sumRow.StoragePlaceCode_DB = first.StoragePlaceCode_DB;
                            sumRow.PackName_DB = first.PackName_DB;
                            sumRow.PackType_DB = first.PackType_DB;

                            sumRow.StoragePlaceName_Hidden = true;
                            sumRow.StoragePlaceCode_Hidden = true;
                            sumRow.PackName_Hidden = true;
                            sumRow.PackType_Hidden = true;

                            sumRow.StoragePlaceName_Hidden2 = true;
                            sumRow.StoragePlaceCode_Hidden2 = true;
                            sumRow.PackName_Hidden2 = true;
                            sumRow.PackType_Hidden2 = true;

                            sumRow.CodeRAO_Hidden = true;
                            sumRow.StatusRAO_Hidden = true;
                            sumRow.MainRadionuclids_Hidden = true;
                            sumRow.Subsidy_Hidden = true;
                            sumRow.FcpNumber_Hidden = true;

                            sumRow.Sum_DB = true;
                        }

                        double volumeSum = 0;
                        double massSum = 0;
                        double quantitySum = 0;

                        double alphaSum = 0;
                        double betaSum = 0;
                        double tritSum = 0;
                        double transSum = 0;

                        List<T> lst = new List<T>();
                        var u = ty.OrderBy(x => (x as Form22).NumberInOrder_DB);
                        foreach (var itemThread in u)
                        {
                            var item = itemThread;
                            var form = item as Form22;
                            if (form.Sum_DB != true)
                            {
                                form.StoragePlaceName_Hidden = true;
                                form.StoragePlaceCode_Hidden = true;
                                form.PackName_Hidden = true;
                                form.PackType_Hidden = true;

                                form.VolumeInPack_Hidden = true;
                                form.MassInPack_Hidden = true;
                                volumeSum += StringToNumber(form.VolumeOutOfPack_DB);
                                massSum += StringToNumber(form.MassOutOfPack_DB);
                                quantitySum += StringToNumber(form.QuantityOZIII_DB);
                                alphaSum += StringToNumber(form.AlphaActivity_DB);
                                betaSum += StringToNumber(form.BetaGammaActivity_DB);
                                tritSum += StringToNumber(form.TritiumActivity_DB);
                                transSum += StringToNumber(form.TransuraniumActivity_DB);


                                lst.Add(form as T);
                            }
                        }

                        sumRow.VolumeOutOfPack_DB = volumeSum >= double.Epsilon ? volumeSum.ToString("E2") : "-";
                        sumRow.MassOutOfPack_DB = massSum >= double.Epsilon ? massSum.ToString("E2") : "-"; 
                        sumRow.QuantityOZIII_DB = quantitySum >= double.Epsilon ? quantitySum.ToString("E2") : "-";
                        sumRow.AlphaActivity_DB = alphaSum >= double.Epsilon ? alphaSum.ToString("E2") : "-";
                        sumRow.BetaGammaActivity_DB = betaSum >= double.Epsilon ? betaSum.ToString("E2") : "-";
                        sumRow.TritiumActivity_DB = tritSum >= double.Epsilon ? tritSum.ToString("E2") : "-";
                        sumRow.TransuraniumActivity_DB = transSum >= double.Epsilon ? transSum.ToString("E2") : "-";

                        lock (ito)
                        {
                            ito[itemT.Key].Add(sumRow as T);
                            foreach (var r in lst)
                            {
                                ito[itemT.Key].Add(r);
                            }
                        }
                    }
                    else
                    {
                        lock (ito)
                        {
                            foreach (var t in ty)
                            {
                                if ((t as Form22).Sum_DB != true)
                                {
                                    var form = (t as Form22);
                                    form.StoragePlaceName_Hidden = false;
                                    form.StoragePlaceCode_Hidden = false;
                                    form.PackName_Hidden = false;
                                    form.PackType_Hidden = false;
                                    ito[itemT.Key].Add(t);
                                }
                            }
                        }
                    }
                });
                tr.Start(itemT);
                lsth.Add(tr);
            }

            foreach (var item in lsth)
            {
                item.Join();
            }

            this.ClearItems();
            var yu = ito.OrderBy(x => x.Key);
            var count = 1;
            foreach (var item in yu)
            {
                var o = item.Value as List<Form22>;
                foreach (var i in o)
                {
                    i.NumberInOrder.Value = count;
                    count++;
                }

                this.AddRange(item.Value);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}