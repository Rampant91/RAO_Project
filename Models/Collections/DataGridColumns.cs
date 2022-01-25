using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Models.Collections;
using Models.DataAccess;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using OfficeOpenXml;
using System.Collections;

namespace Models.Collections
{
    public class DataGridColumns:INotifyPropertyChanged
    {
        public string name;
        public List<DataGridColumns> innertCol = null;
        public DataGridColumns parent = null;
        bool _Blocked = false;
        public bool Blocked { 
            get 
            {
                return _Blocked;
            }
            set
            {
                if (_Blocked != value)
                {
                    _Blocked = value;
                    if (innertCol != null)
                    {
                        foreach (var item in innertCol)
                        {
                            item.Blocked = value;
                        }
                    }
                }
            }

        }

        bool _ChooseLine = false;
        public bool ChooseLine
        {
            get
            {
                return _ChooseLine;
            }
            set
            {
                if (_ChooseLine != value)
                {
                    _ChooseLine = value;
                    if (innertCol != null)
                    {
                        foreach (var item in innertCol)
                        {
                            item.ChooseLine = value;
                        }
                    }
                }
            }

        }

        private string binding = "";
        public string Binding { 
            get 
            {
                if (innertCol != null)
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
                else
                {
                    return binding;
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

        public string GridLength
        {
            get
            {
                return SizeCol +"*";
            }
            set
            {
                if (value != "")
                {
                    try
                    {
                        var tmp = Convert.ToDouble(value.Replace("*", ""));
                        SizeCol = tmp;
                    }
                    catch
                    {

                    }
                }
            }
        }

        double sizeCol = 0;
        public double SizeCol
        {
            get 
            {
                if (innertCol == null)
                {
                    return sizeCol;
                }
                else
                {
                    var t = 0.0;
                    foreach (var elem in innertCol) 
                    {
                        t += elem.SizeCol;
                    }
                    return t;
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
                if (parent != null)
                {
                    parent.OnPropertyChanged(nameof(GridLength));
                }
                else
                {
                    OnPropertyChanged(nameof(GridLength));
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

        public void SetSizeColToAllLevels(int SizeCol)
        {
            if (innertCol != null)
            {
                foreach (var item in innertCol)
                {
                    item.sizeCol = SizeCol;
                    item.SetSizeColToAllLevels(SizeCol);
                }
            }
        }
        public List<DataGridColumns> GetLevel(int Level)
        {
            var lstar = new List<DataGridColumns>();
            if (this.Level == 1)
            {
                lstar.Add(this);
                return lstar;
            }
            else
            {
                if (this.Level > Level)
                {
                    int allLevel = this.Level-1;
                    List<DataGridColumns> lst = new List<DataGridColumns>(innertCol);
                    while (allLevel != Level)
                    {
                        List<DataGridColumns> lst2 = new List<DataGridColumns>();
                        foreach (var item in lst)
                        {
                            lst2.AddRange(item.innertCol);

                        }
                        lst = lst2;

                        bool flag = true;
                        foreach (var item in lst)
                        {
                            if (item.Level != Level)
                            {
                                flag = false;
                            }
                        }
                        if (flag)
                        {
                            allLevel = Level;
                        }
                    }

                    return lst;
                }
                else
                {
                    if (this.Level == Level)
                    {
                        return innertCol;
                    }
                    else
                    {
                        return lstar;
                    }
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
                            item.parent = ret;
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
                            item.parent = ret;
                            ret.innertCol.Add(item);
                        }
                    }
                    else
                    {
                        col1.parent = ret;
                        ret.innertCol.Add(col1);
                    }
                    col2.parent = ret;
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
                        tr.parent = ret;
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
                            item.parent = ret;
                            ret.innertCol.Add(item);
                        }
                    }
                    else
                    {
                        col1.parent = ret;
                        ret.innertCol.Add(col1);
                    }

                    col2.parent = ret;
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
                        tr.parent = _ret;
                        _ret.innertCol.Add(tr);
                    }
                    return _ret;
                }
            }
        }

        #region INotifyPropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
