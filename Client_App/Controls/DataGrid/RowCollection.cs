using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using System.Collections.Concurrent;
using System.Linq;
using Avalonia.Data;
using Models.Collections;
using Avalonia;
using Avalonia.Media;

namespace Client_App.Controls.DataGrid
{
    public class RowCollection:IEnumerable<KeyValuePair<string,CellCollection>>
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

        public IEnumerator<KeyValuePair<string, CellCollection>> GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        public int Count
        {
            get { return Rows.Count(); }
        }

        public CellCollection this[int Row]
        {
            get
            {
                foreach (KeyValuePair<string, CellCollection> item in Rows)
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

        public void Add(CellCollection stck, int Row)
        {
            var str = Row.ToString();
            if (Rows.ContainsKey(str)) Remove(Row);
            Rows.Add(str, stck);
            AddToSRows(Row, stck);
        }

        public int Add(CellCollection stck)
        {
            int counter = 0;
            int max = Rows.Max((x)=>Convert.ToInt32(x.Key));
            for (int i = 1; i < max; i++)
            {
                if (this[i] == null)
                {
                    counter = i;
                }
            }

            if (counter == 0)
            {
                counter = max + 1;
            }

            this.Add(stck, counter);

            return counter;
        }
        public int GetFreeRow()
        {
            int counter = 0;
            int max = 0;
            if (Rows.Count() > 0)
            {
                max = Rows.Max((x) => Convert.ToInt32(x.Key));
            }

            ;
            for (int i = 1; i <= max; i++)
            {
                if (this[i] == null)
                {
                    counter = i;
                }
            }
            
            if (counter == 0)
            {
                counter = max + 1;
            }

            return counter;
        }

        private void AddToSRows(int Row, CellCollection cellCollection)
        {
            SRows.Children.Insert(Row-1,cellCollection.SCells);
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
                    this.Rows.Add(count.ToString(),Rows[item.SRow.ToString()]);
                    this.Rows.Remove(item.SRow.ToString());
                    item.SRow = count;

                    Binding b = new Binding
                    {
                        Path = "Items[" + (count - 1).ToString() + "]",
                        ElementName = topName,
                        NameScope = new WeakReference<INameScope>(scp)
                    };

                    item.Bind(StackPanel.DataContextProperty, b);

                    this[count].Reorgonize(item.SRow.ToString(),count.ToString());
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
            foreach (string? item in Rows.Keys) Rows.Remove(item);
            SRows.Children.Clear();
        }
    }
}