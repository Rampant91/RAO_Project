using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Collections
{
    public class DataGridColumns
    {
        public string name;
        public List<DataGridColumns> innertCol = null;

        private string binding = "";
        public string Binding { 
            get 
            {
                var tmp = innertCol.FirstOrDefault();
                if (tmp == null)
                {
                    return binding;
                }
                else
                {
                    return tmp.Binding;
                }
            }
            set 
            {
                if (innertCol == null)
                {
                    binding = value;
                }
                else
                {
                    foreach (var item in innertCol)
                    {
                        item.Binding = value;
                    }
                }
            }
        }

        int sizeCol = 0;
        public int SizeCol
        {
            get 
            {
                if (innertCol == null)
                {
                    return sizeCol;
                }
                else
                {
                    foreach (var elem in innertCol) 
                    {
                        sizeCol += elem.sizeCol;
                    }
                    return sizeCol;
                }
            
            }
            set 
            {
                if (innertCol == null)
                {
                    if (sizeCol != value)
                    {
                        sizeCol = value;
                    }
                }
                else
                {
                    foreach (var elem in innertCol)
                    {
                        elem.sizeCol=value;
                    }
                }
            }
        }

        public int Level
        {
            get
            {
                if(innertCol==null)
                {
                    return 1;
                }
                var tmp = innertCol.FirstOrDefault();
                if(tmp==null)
                {
                    return 1;
                }
                else
                {
                    return tmp.Level + 1;
                }
            }
        }

        public static DataGridColumns operator+(DataGridColumns col1,DataGridColumns col2)
        {
            if(col1.innertCol==null)
            {
                if (col1.name == col2.name)
                {
                    DataGridColumns ret = new DataGridColumns();
                    ret.name = col1.name;
                    ret.binding = col1.binding;
                    ret.sizeCol = col1.sizeCol;
                    if (col2.innertCol != null)
                    {
                        ret.innertCol = new List<DataGridColumns>();
                        foreach (var item in col2.innertCol)
                        {
                            ret.innertCol.Add(item);
                        }
                    }
                    return ret;
                }
                else
                {
                    DataGridColumns ret = new DataGridColumns();
                    ret.name = "";
                    ret.binding = "";
                    ret.innertCol = new List<DataGridColumns>();
                    if (ret.name == col1.name)
                    {
                        foreach (var item in col1.innertCol)
                        {
                            ret.innertCol.Add(item);
                        }
                    }
                    else
                    {
                        ret.innertCol.Add(col1);
                    }
                    ret.innertCol.Add(col2);
                    return ret;
                }
            }
            else
            {
                if(col1.name==col2.name)
                {
                    var tmp = new List<DataGridColumns>();
                    foreach (var item in col1.innertCol)
                    {
                        tmp.Add(item);
                    }
                    foreach (var item in col2.innertCol)
                    {
                        tmp.Add(item);
                    }

                    var group = tmp.GroupBy(x => x.name);
                    DataGridColumns ret = new DataGridColumns();
                    ret.name = col1.name;
                    ret.innertCol = new List<DataGridColumns>();
                    foreach(var item in group)
                    {
                        DataGridColumns tr = item.FirstOrDefault();
                        for(int i=1;i<item.Count();i++)
                        {
                            tr += item.ElementAt(i);
                        }
                        ret.innertCol.Add(tr) ;
                    }
                    return ret;
                }
                else
                {
                    DataGridColumns ret = new DataGridColumns();
                    ret.name = "";
                    ret.binding = "";
                    ret.innertCol = new List<DataGridColumns>();
                    if (ret.name == col1.name)
                    {
                        foreach (var item in col1.innertCol)
                        {
                            ret.innertCol.Add(item);
                        }
                    }
                    else
                    {
                        ret.innertCol.Add(col1);
                    }

                    ret.innertCol.Add(col2);

                    var group = ret.innertCol.GroupBy(x => x.name);
                    DataGridColumns _ret = new DataGridColumns();
                    _ret.name = ret.name;
                    _ret.innertCol = new List<DataGridColumns>();
                    foreach (var item in group)
                    {
                        DataGridColumns tr = item.FirstOrDefault();
                        for (int i = 1; i < item.Count(); i++)
                        {
                            tr += item.ElementAt(i);
                        }
                        _ret.innertCol.Add(tr);
                    }
                    return _ret;
                }
            }
        }
    }
}
