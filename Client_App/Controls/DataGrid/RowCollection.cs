using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using System.Collections.Concurrent;
using System.Linq;
using Models.Collections;

namespace Client_App.Controls.DataGrid
{
    public class RowCollection
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

        private void AddToSRows(int Row, CellCollection cellCollection)
        {
            SRows.Children.Add(cellCollection.SCells);
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