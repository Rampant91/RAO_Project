using System;
using System.Collections.Generic;
using System.Linq;
using Models.Collections;

namespace Client_App.Controls.DataGrid
{
    public class CellCollection
    {
        public CellCollection(Row Cells)
        {
            this.Cells = new ObservableDictionary<string, Cell>();
            SCells = Cells;
            foreach (Cell item in SCells.Children)
            {
                var str = item.CellRow + ";" + item.CellColumn;
                this.Cells.Add(str, item);
            }
        }

        public Row SCells { get; set; }
        private ObservableDictionary<string, Cell> Cells { get; }

        public int Count => Cells.Count();

        public Cell this[int Row, int Column]
        {
            get
            {
                foreach (var item in Cells)
                    if (item.Value.CellRow == Row && item.Value.CellColumn == Column)
                        return item.Value;
                return null;
            }
        }

        public void Reorgonize(string LastRow, string NewRow)
        {
            SCells.SRow = Convert.ToInt32(NewRow);
            foreach (var item in Cells)
                if (item.Value.CellRow == Convert.ToInt32(LastRow))
                    item.Value.CellRow = Convert.ToInt32(NewRow);
        }

        public IEnumerable<Cell> GetIEnumerable()
        {
            foreach (var item in Cells) yield return item.Value;
        }

        public void Add(Cell cell, int Row, int Column)
        {
            var str = Row + ";" + Column;
            if (Cells.ContainsKey(str)) Remove(Row, Column);
            Cells.Add(str, cell);
            SCells.Children.Add(cell);
        }

        public void Remove(int Row, int Column)
        {
            var str = Row + ";" + Column;
            if (Cells.ContainsKey(str)) Cells.Remove(str);
            RemoveFromRows(Row, Column);
        }

        private void RemoveFromRows(int Row, int Column)
        {
            for (var i = 0; i < SCells.Children.Count; i++)
            {
                var child = (Cell) SCells.Children[i];
                if (child.CellRow == Row && child.CellColumn == Column)
                {
                    SCells.Children.RemoveAt(i);
                    return;
                }
            }
        }
    }
}