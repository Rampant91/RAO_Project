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
        public int Offset
        {
            get
            {
                int offset = 0;
                int maxKey = 0;
                foreach (var key in Rows.Keys)
                {
                    var num = Convert.ToInt32(key);
                    if (maxKey < num)
                    {
                        maxKey = num;
                    }
                }
                for (int i = 1; i < maxKey; i++)
                {
                    if (this[i] == null)
                    {
                        offset++;
                    }
                    else
                    {
                        break;
                    }
                }

                return offset;
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
            var max = 0;
            if (Rows.Count() != 0)
            {
                max = Rows.Max(x => Convert.ToInt32(x.Key));
            }
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
            int offset = 0;
            int maxKey = 0;
            foreach (var key in Rows.Keys)
            {
                var num = Convert.ToInt32(key);
                if (maxKey < num)
                {
                    maxKey = num;
                }
            }
            for (int i = 1; i < maxKey; i++)
            {
                if (this[i] == null)
                {
                    offset++;
                }
                else
                {
                    break;
                }
            }

            if (Rows.Count() > 0) max = Rows.Max(x => Convert.ToInt32(x.Key));
            for (var i = offset+1; i <= max; i++)
                if (this[i] == null)
                {
                    counter = i;
                    break;
                }


            if (counter == 0) counter = max + 1;

            return counter-offset;
        }

        private void AddToSRows(int Row, CellCollection cellCollection)
        {
            int offset = 0;
            int maxKey = 0;
            foreach(var key in Rows.Keys)
            {
                var num = Convert.ToInt32(key);
                if(maxKey<num)
                {
                    maxKey = num;
                }
            }
            for(int i=1;i<maxKey;i++)
            {
                if(this[i]==null)
                {
                    offset++;
                }
                else
                {
                    break;
                }
            }
            SRows.Children.Insert(Row - 1-offset, cellCollection.SCells);
        }

        public void Reorgonize(NameScope scp, string topName)
        {
            int offset = 0;
            int maxKey = 0;
            foreach (var key in Rows.Keys)
            {
                var num = Convert.ToInt32(key);
                if (maxKey < num)
                {
                    maxKey = num;
                }
            }
            for (int i = 1; i < maxKey; i++)
            {
                if (this[i] == null)
                {
                    offset++;
                }
                else
                {
                    break;
                }
            }

            int count = offset + 1;
            foreach (Row item in SRows.Children)
            {
                var rw = Rows[item.SRow.ToString()];

                Rows.Remove(item.SRow.ToString());
                Rows.Add(count.ToString(), rw);

                this[count].Reorgonize(item.SRow.ToString(), count.ToString());
                item.SRow = count;
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
            foreach (var item in Rows.Keys) 
                this.Remove(Convert.ToInt32(item));
        }
    }
}