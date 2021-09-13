using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Models;
using Models.Collections;
using OfficeOpenXml;

namespace Collections
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


            base.OnCollectionChanged(e);
        }

        public void Sum()
        {
            if (Items.Count > 0)
            {
                if (Items[0] is Form17)
                {
                    Form17();
                }

                if (Items[0] is Form18)
                {
                    Form18();
                }

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

        private void Form17()
        {

        }
        private void Form18()
        {

        }

        private void Form21()
        {
            var tItems = Items.GroupBy(x=>(x as Form21).RefineMachineName_DB+
                                          (x as Form21).MachineCode_DB +
                                          (x as Form21).MachinePower_DB +
                                          (x as Form21).NumberOfHoursPerYear_DB).ToList();
            this.ClearItems();
            int rowCount = 1;
            var y = tItems.ToList();
            foreach (var item in y)
            {
                if (item.Key == "")
                {
                    foreach (var t in item)
                    {
                        (t as Form21).NumberInOrder_DB = rowCount;
                        this.Add(t);
                        rowCount++;
                    }
                    tItems.Remove(item);
                }
            }

            foreach (var itemT in tItems)
            {
                var sums = itemT.Where(x => (x as Form21).Sum == true).FirstOrDefault();

                Form21 sumRow = sums as Form21;
                if ((itemT.Count() > 1 &&sumRow==null)|| (itemT.Count() > 2 && sumRow != null))
                {
                    if (sumRow == null)
                    {
                        sumRow = (Form21) FormCreator.Create("2.1");

                        var first = itemT.FirstOrDefault() as Form21;
                        sumRow.RefineMachineName_DB = first.RefineMachineName_DB;
                        sumRow.MachineCode_DB = first.MachineCode_DB;
                        sumRow.MachinePower_DB = first.MachinePower_DB;
                        sumRow.NumberOfHoursPerYear_DB = first.NumberOfHoursPerYear_DB;
                        sumRow.Sum = true;

                    }

                    sumRow.NumberInOrder_DB = rowCount;
                    rowCount++;

                    double volumeInSum = 0;
                    double massInSum = 0;
                    double quantityInSum = 0;
                    double alphaInSum = 0;
                    double betaInSum = 0;

                    double volumeOutSum = 0;
                    double massOutSum = 0;
                    double quantityOutSum = 0;
                    double alphaOutSum = 0;
                    double betaOutSum = 0;

                    List<T> lst = new List<T>();

                    foreach (var item in itemT)
                    {
                        var form = item as Form21;
                        if (form.Sum != true)
                        {
                            form.RefineMachineName_Hidden = true;
                            form.MachineCode_Hidden = true;
                            form.MachinePower_Hidden = true;
                            form.NumberOfHoursPerYear_Hidden = true;
                            form.NumberInOrder.Value = rowCount;

                            volumeInSum += StringToNumber(form.VolumeIn_DB);
                            massInSum += StringToNumber(form.MassIn_DB);
                            quantityInSum += StringToNumber(form.QuantityIn_DB);
                            alphaInSum += StringToNumber(form.AlphaActivityIn_DB);
                            betaInSum += StringToNumber(form.BetaGammaActivityIn_DB);

                            volumeOutSum += StringToNumber(form.VolumeOut_DB);
                            massOutSum += StringToNumber(form.MassOut_DB);
                            quantityOutSum += StringToNumber(form.QuantityOZIIIout_DB);
                            alphaOutSum += StringToNumber(form.AlphaActivityOut_DB);
                            betaOutSum += StringToNumber(form.BetaGammaActivityOut_DB);

                            lst.Add(form as T);
                            rowCount++;
                        }

                    }

                    sumRow.VolumeIn.Value = volumeInSum.ToString("E2");
                    sumRow.MassIn.Value = massInSum.ToString("E2");
                    sumRow.QuantityIn.Value = quantityInSum.ToString("E2");
                    sumRow.AlphaActivityIn.Value = alphaInSum.ToString("E2");
                    sumRow.BetaGammaActivityIn.Value = betaInSum.ToString("E2");

                    sumRow.VolumeOut.Value = volumeOutSum.ToString("E2");
                    sumRow.MassOut.Value = massOutSum.ToString("E2");
                    sumRow.QuantityOZIIIout.Value = quantityOutSum.ToString("E2");
                    sumRow.AlphaActivityOut.Value = alphaOutSum.ToString("E2");
                    sumRow.BetaGammaActivityOut.Value = betaOutSum.ToString("E2");
                    this.Add(sumRow as T);

                    foreach (var r in lst)
                    {
                        this.Add(r);
                    }
                }
                else
                {
                    foreach (var t in itemT)
                    {
                        if ((t as Form21).Sum != true)
                        {
                            var form = (t as Form21);
                            form.RefineMachineName_Hidden = false;
                            form.MachineCode_Hidden = false;
                            form.MachinePower_Hidden = false;
                            form.NumberOfHoursPerYear_Hidden = false;
                            form.NumberInOrder.Value = rowCount;
                            this.Add(t);
                            rowCount++;
                        }
                    }
                }
            }
        }

        double StringToNumber(string Num)
        {
            string tmp = Num;
            int len = tmp.Length;
            if (len > 2)
            {
                if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
                {
                    tmp = tmp.Remove(len - 1, 1);
                    tmp = tmp.Remove(0, 1);
                }

                NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                                      NumberStyles.AllowExponent;
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

        private void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            T typedSender = (T) sender;
            int i = Items.IndexOf(typedSender);

            if (i < 0)
                throw new ArgumentException("Received property notification from item not in collection");

            OnItemPropertyChanged(i, e);
        }

        #region IExcel
        public void ExcelRow(ExcelWorksheet worksheet, int Row)
        {
            throw new System.NotImplementedException();
        }

        public void ExcelHeader(ExcelWorksheet worksheet)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}