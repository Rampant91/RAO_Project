using Avalonia.Controls;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Client_App.Controls.DataGrid
{
    public class RowCollection
    {
        public StackPanel SRows { get; set; }
        private ObservableConcurrentDictionary<string, CellCollection> Rows { get; set; }
        public int Count => Rows.Count();
        public void Add(CellCollection stck, int Row)
        {
            string? str = Row.ToString();
            if (Rows.ContainsKey(str))
            {
                Remove(Row);
            }
            Rows.Add(str, stck);
            AddToSRows(Row, stck);
        }

        private void AddToSRows(int Row, CellCollection cellCollection)
        {
            SRows.Children.Add(cellCollection.SCells);
        }
        public void Remove(int Row)
        {
            string? str = Row.ToString() + ";";
            if (Rows.ContainsKey(str))
            {
                Rows.Remove(str);
            }
            RemoveFromRows(Row);
        }

        private void RemoveFromRows(int Row)
        {
            for (int i = 0; i < SRows.Children.Count; i++)
            {
                Row? child = (Row)SRows.Children[i];
                if (child.SRow == Row)
                {
                    SRows.Children.RemoveAt(i);
                    return;
                }
            }
        }
        public CellCollection this[int Row]
        {
            get
            {
                foreach (KeyValuePair<string, CellCollection> item in Rows)
                {
                    if (item.Key == Row.ToString())
                    {
                        return item.Value;
                    }
                }
                return null;
            }
        }
        public Cell this[int Row, int Column]
        {
            get
            {
                CellCollection? tp = this[Row];
                if (tp != null)
                {
                    return tp[Row, Column];
                }
                return null;
            }
        }
        public void Clear()
        {
            foreach (string? item in Rows.Keys)
            {
                Rows.Remove(item);
            }
            SRows.Children.Clear();
        }
        public RowCollection(StackPanel Rows, PropertyChangedEventHandler handler)
        {
            this.Rows = new ObservableConcurrentDictionary<string, CellCollection>();
            SRows = Rows;
            foreach (Row item in SRows.Children)
            {
                string? str = item.SRow.ToString();
                this.Rows.Add(str, new CellCollection(item, handler));
            }
        }
    }
}
