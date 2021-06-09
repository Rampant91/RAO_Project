using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Collections;
using System.Collections.Concurrent;
using Avalonia.Controls;
using System.ComponentModel;

namespace Client_App.Controls.DataGrid
{
    public class CellCollection
    {
        public Row SCells { get; set; }
        ObservableConcurrentDictionary<string,Cell> Cells { get; set; }
        public int Count
        {
            get
            {
                return Cells.Count();
            }
        }
        public IEnumerable<Cell> GetIEnumerable()
        {
            foreach(var item in Cells)
            {
                yield return item.Value;
            }
        }
        public void Add(Cell cell,int Row,int Column)
        {
            var str = Row.ToString() + ";" + Column.ToString();
            if (Cells.ContainsKey(str))
            {
                Remove(Row,Column);
            }
            Cells.Add(str, cell);
            SCells.Children.Add(cell);
        }
        public void Remove(int Row,int Column)
        {
            var str = Row.ToString() + ";" + Column.ToString();
            if (Cells.ContainsKey(str))
            {
                Cells.Remove(str);
            }
            RemoveFromRows(Row,Column);
        }
        void RemoveFromRows(int Row,int Column)
        {
            for(int i =0;i< SCells.Children.Count;i++)
            {
                var child = (Cell)SCells.Children[i];
                if(child.CellRow==Row&&child.CellColumn==Column)
                {
                    SCells.Children.RemoveAt(i);
                    return;
                }
            }
        }
        public Cell this[int Row,int Column]
        {
            get
            {
                foreach(var item in Cells)
                {
                    if(item.Value.CellRow==Row&& item.Value.CellColumn ==Column)
                    {
                        return item.Value;
                    }
                }
                return null;
            }
        }
        public CellCollection(Row Cells,PropertyChangedEventHandler handler)
        {
            this.Cells = new ObservableConcurrentDictionary<string, Cell>();
            this.SCells = Cells;
            foreach(Cell item in SCells.Children)
            {
                var str = item.CellRow.ToString() + ";" + item.CellColumn.ToString();
                item.PropertyChanged += handler;
                this.Cells.Add(str,item);
            }
        }
    }
}
