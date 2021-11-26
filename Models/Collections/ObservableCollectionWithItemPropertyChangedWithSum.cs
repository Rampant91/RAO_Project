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
    public class ObservableCollectionWithItemPropertyChangedWithSum<T> : ObservableCollection<T>, IKey
        where T : class, IKey
    {
        /// <summary>
        /// Occurs when a property is changed within an item.
        /// </summary>
        public event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged;

        public int Id { get; set; }

        public ObservableCollectionWithItemPropertyChangedWithSum() : base()
        {
        }

        public void CleanIds()
        {
            foreach (var item in Items)
            {
                item.Id = 0;
            }
        }

        public ObservableCollectionWithItemPropertyChangedWithSum(List<T> list) : base(list)
        {
            ObserveAll();
        }

        public ObservableCollectionWithItemPropertyChangedWithSum(IEnumerable<T> enumerable) : base(enumerable)
        {
            ObserveAll();
        }
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove ||
                e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (T item in e.OldItems)
                    item.PropertyChanged -= ChildPropertyChanged;
            }

            if (e.Action == NotifyCollectionChangedAction.Add ||
                e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (T item in e.NewItems)
                    if (item != null)
                    {
                        item.PropertyChanged += ChildPropertyChanged;
                    }
            }

            var flag = false;
            var bsT = typeof(T).BaseType;
            if (bsT != null)
            {
                if (bsT == typeof(Form))
                {
                    flag = true;
                }
                else
                {
                    var bsT2 = bsT.BaseType;
                    if (bsT2 != null)
                    {
                        if (bsT2 == typeof(Form))
                        {
                            flag = true;
                        }
                    }
                }
            }
            if (flag)
            {
                QuickSort();
            }

            base.OnCollectionChanged(e);
        }

        //метод для обмена элементов массива
        void Swap(int index1, int index2)
        {
            var t = Items[index1];
            Items[index1] = Items[index2];
            Items[index2] = t;
        }
        int Partition(int minIndex, int maxIndex)
        {
            var pivot = minIndex - 1;
            for (var i = minIndex; i < maxIndex; i++)
            {
                if ((Items[i] as Form).NumberInOrder_DB < (Items[maxIndex] as Form).NumberInOrder_DB)
                {
                    pivot++;
                    Swap(pivot, i);
                }
            }

            pivot++;
            Swap(pivot, maxIndex);
            return pivot;
        }
        void QuickSort(int minIndex, int maxIndex)
        {
            if (minIndex >= maxIndex)
            {
                return;
            }

            var pivotIndex = Partition(minIndex, maxIndex);
            QuickSort(minIndex, pivotIndex - 1);
            QuickSort(pivotIndex + 1, maxIndex);
        }

        public void QuickSort()
        {
            QuickSort(0, Items.Count - 1);
        }

        protected override void RemoveItem(int index)
        {
            this[index].PropertyChanged -= ChildPropertyChanged;
            base.RemoveItem(index);
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
            int rowCount = 1;
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
                        (t as Form21).NumberInOrder_DB = rowCount;
                        ito[item.Key].Add(t);
                        rowCount++;
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
                    var sums = ty.Where(x => (x as Form21).Sum_DB == true).FirstOrDefault();

                    Form21 sumRow = sums as Form21;
                    if ((itemT.Count() > 1 && sumRow == null) || (itemT.Count() > 2 && sumRow != null))
                    {
                        if (sumRow == null)
                        {
                            var first = ty.FirstOrDefault() as Form21;
                            sumRow = (Form21)FormCreator.Create("2.1");
                            sumRow.RefineMachineName_DB = first.RefineMachineName_DB;
                            sumRow.MachineCode_DB = first.MachineCode_DB;
                            sumRow.MachinePower_DB = first.MachinePower_DB;
                            sumRow.NumberOfHoursPerYear_DB = first.NumberOfHoursPerYear_DB;
                            sumRow.RefineMachineName_Hidden = true;
                            sumRow.MachineCode_Hidden = true;
                            sumRow.MachinePower_Hidden = true;
                            sumRow.NumberOfHoursPerYear_Hidden = true;
                            sumRow.RefineMachineName_Hidden2 = true;
                            sumRow.MachineCode_Hidden2 = true;
                            sumRow.MachinePower_Hidden2 = true;
                            sumRow.NumberOfHoursPerYear_Hidden2 = true;
                            sumRow.CodeRAOIn.Value = "";
                            sumRow.CodeRAOIn_Hidden = true;
                            sumRow.StatusRAOIn.Value = "";
                            sumRow.StatusRAOIn_Hidden = true;
                            sumRow.CodeRAOout.Value = "";
                            sumRow.CodeRAOout_Hidden = true;
                            sumRow.StatusRAOout.Value = "";
                            sumRow.StatusRAOout_Hidden = true;

                            sumRow.Sum_DB = true;
                        }

                        sumRow.NumberInOrder_DB = rowCount;
                        rowCount++;

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

                        List<T> lst = new List<T>();
                        foreach (var itemThread in ty)
                        {
                            var item = itemThread;
                            var form = item as Form21;
                            if (form.Sum_DB != true)
                            {
                                form.RefineMachineName_Hidden = true;
                                form.MachineCode_Hidden = true;
                                form.MachinePower_Hidden = true;
                                form.NumberOfHoursPerYear_Hidden = true;
                                form.NumberInOrder_DB = rowCount;

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
                                rowCount++;
                            }
                        }

                        sumRow.VolumeIn_DB = volumeInSum >=double.Epsilon ? volumeInSum.ToString("E2") : "-";
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
                                    form.RefineMachineName_Hidden = false;
                                    form.MachineCode_Hidden = false;
                                    form.MachinePower_Hidden = false;
                                    form.NumberOfHoursPerYear_Hidden = false;
                                    form.NumberInOrder.Value = rowCount;
                                    ito[itemT.Key].Add(t);
                                    rowCount++;
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
            foreach (var item in yu)
            {
                this.AddRange(item.Value);
            }
        }

        double StringToNumber(string Num)
        {
            string tmp = Num;
            tmp.Replace(" ","");
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
                if(len==1)
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
            return 0;
        }

        private void Form22()
        {
            var tItems = Items.GroupBy(x => (x as Form22).StoragePlaceName_DB +
                              (x as Form22).StoragePlaceCode_DB +
                              (x as Form22).PackName_DB +
                              (x as Form22).PackType_DB).ToList();
            int rowCount = 1;
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
                        (t as Form22).NumberInOrder_DB = rowCount;
                        ito[item.Key].Add(t);
                        rowCount++;
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


                            sumRow.Sum_DB = true;
                        }

                        sumRow.NumberInOrder_DB = rowCount;
                        rowCount++;

                        double volumeSum = 0;
                        double massSum = 0;
                        //double quantitySum = 0;

                        double alphaSum = 0;
                        double betaSum = 0;
                        double tritSum = 0;
                        double transSum = 0;

                        List<T> lst = new List<T>();
                        foreach (var itemThread in ty)
                        {
                            var item = itemThread;
                            var form = item as Form22;
                            if (form.Sum_DB != true)
                            {
                                form.NumberInOrder_DB = rowCount;

                                volumeSum += StringToNumber(form.VolumeOutOfPack_DB);
                                massSum += StringToNumber(form.MassOutOfPack_DB);
                                //quantitySum += StringToNumber(form.QuantityIn_DB);
                                alphaSum += StringToNumber(form.AlphaActivity_DB);
                                betaSum += StringToNumber(form.BetaGammaActivity_DB);
                                tritSum += StringToNumber(form.TritiumActivity_DB);
                                transSum += StringToNumber(form.TransuraniumActivity_DB);


                                lst.Add(form as T);
                                rowCount++;
                            }
                        }

                        sumRow.CodeRAO_Hidden = true;
                        sumRow.StatusRAO_Hidden = true;
                        sumRow.MainRadionuclids_Hidden = true;

                        sumRow.VolumeOutOfPack_DB = volumeSum.ToString("E2");
                        sumRow.MassOutOfPack_DB = massSum.ToString("E2");
                        //sumRow.QuantityOZIII_DB = quantityInSum.ToString("E2");
                        sumRow.AlphaActivity_DB = alphaSum.ToString("E2");
                        sumRow.BetaGammaActivity_DB = betaSum.ToString("E2");
                        sumRow.TritiumActivity_DB = tritSum.ToString("E2");
                        sumRow.TransuraniumActivity_DB = transSum.ToString("E2");

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
                                    form.NumberInOrder_DB = rowCount;
                                    ito[itemT.Key].Add(t);
                                    rowCount++;
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
            foreach (var item in yu)
            {
                this.AddRange(item.Value);
            }
        }

        protected void OnItemPropertyChanged(ItemPropertyChangedEventArgs e)
        {
            ItemPropertyChanged?.Invoke(this, e);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected void OnItemPropertyChanged(int index, PropertyChangedEventArgs e)
        {
            OnItemPropertyChanged(new ItemPropertyChangedEventArgs(index, e));
        }

        protected override void ClearItems()
        {
            foreach (T item in Items)
                item.PropertyChanged -= ChildPropertyChanged;

            base.ClearItems();
        }

        private void ObserveAll()
        {
            foreach (T item in Items)
                item.PropertyChanged += ChildPropertyChanged;
        }

        public void AddRange(IEnumerable<T>items)
        {
            foreach (var item in items)
            {
                this.Add(item);
            }
        }

        private void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            T typedSender = (T) sender;
            int i = Items.IndexOf(typedSender);

            if (i < 0)
                throw new ArgumentException("Received property notification from item not in collection");

            OnItemPropertyChanged(i, e);
        }

        #region IExcel
        public int ExcelRow(ExcelWorksheet worksheet, int Row, int Column, bool Tanspon = true)
        {
            throw new System.NotImplementedException();
        }

        public int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}