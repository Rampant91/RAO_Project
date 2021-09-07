using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Models.Collections;

namespace Client_App.Controls.DataGrid
{
    public class RowCollection : IEnumerable<KeyValuePair<string, CellCollection>>
    {
        public RowCollection(StackPanel Rows)
        {
            this.Rows = new ObservableDictionary<string, CellCollection>();
            SRows = Rows;
            foreach (Row item in SRows.Children)
            {
                var str = item.SRow.ToString();
                this.Rows.Add(str, new CellCollection(item));
            }
        }

        public StackPanel SRows { get; set; }
        private ObservableDictionary<string, CellCollection> Rows { get; }

        public int Count => Rows.Count();

        public CellCollection this[int Row]
        {
            get
            {
                foreach (var item in Rows)
                    if (item.Key == Row.ToString())
                        return item.Value;
                return null;
            }
        }

        public Cell this[int Row, int Column]
        {
            get
            {
                var tp = this[Row];
                if (tp != null) return tp[Row, Column];
                return null;
            }
        }

        public IEnumerator<KeyValuePair<string, CellCollection>> GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        public void Add(CellCollection stck, int Row)
        {
            var str = Row.ToString();
            if (Rows.ContainsKey(str)) Remove(Row);
            Rows.Add(str, stck);
            AddToSRows(Row, stck);
        }

        public int Add(CellCollection stck)
        {
            var counter = 0;
            var max = Rows.Max(x => Convert.ToInt32(x.Key));
            for (var i = 1; i < max; i++)
                if (this[i] == null)
                    counter = i;

            if (counter == 0) counter = max + 1;

            Add(stck, counter);

            return counter;
        }

        public int GetFreeRow()
        {
            var counter = 0;
            var max = 0;
            if (Rows.Count() > 0) max = Rows.Max(x => Convert.ToInt32(x.Key));

            ;
            for (var i = 1; i <= max; i++)
                if (this[i] == null)
                    counter = i;

            if (counter == 0) counter = max + 1;

            return counter;
        }

        private void AddToSRows(int Row, CellCollection cellCollection)
        {
            SRows.Children.Insert(Row - 1, cellCollection.SCells);
            //SRows.Children.Add(cellCollection.SCells);
            //var t=SRows.Children.IndexOf(cellCollection.SCells);
            //SRows.Children.Move(t,Row-1);
        }

        public void Reorgonize(NameScope scp, string topName)
        {
            var count = 1;
            foreach (Row item in SRows.Children)
            {
                if (item.SRow != count)
                {
                    Rows.Add(count.ToString(), Rows[item.SRow.ToString()]);
                    Rows.Remove(item.SRow.ToString());

                    Binding b = new()
                    {
                        Path = "Items[" + (count - 1) + "]",
                        ElementName = topName,
                        NameScope = new WeakReference<INameScope>(scp)
                    };

                    item.Bind(StyledElement.DataContextProperty, b);

                    this[count].Reorgonize(item.SRow.ToString(), count.ToString());
                    item.SRow = count;
                }

                count++;
            }
        }

        public void Remove(int Row)
        {
            var str = Row.ToString();
            if (Rows.ContainsKey(str)) Rows.Remove(str);
            RemoveFromRows(Row);
        }

        private void RemoveFromRows(int Row)
        {
            for (var i = 0; i < SRows.Children.Count; i++)
            {
                var child = (Row) SRows.Children[i];
                if (child.SRow == Row)
                {
                    SRows.Children.RemoveAt(i);
                    return;
                }
            }
        }

        public void Clear()
        {
            foreach (var item in Rows.Keys) Rows.Remove(item);
            SRows.Children.Clear();
        }
    }
}