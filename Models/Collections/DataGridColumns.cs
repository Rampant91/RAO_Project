using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Collections;

public class DataGridColumns:INotifyPropertyChanged
{
    public string name;
    public List<DataGridColumns> innertCol;
    public DataGridColumns parent;

    #region Blocked
    bool _Blocked;
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
    #endregion

    #region ChooseLine
    bool _ChooseLine;
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
    #endregion

    #region Binding
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
    #endregion

    public string GridLength
    {
        get => $"{SizeCol}*";
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
                    // ignored
                }
            }
        }
    }


    bool textW;
    public bool IsTextWrapping
    {
        get => textW;
        set 
        {
            if (textW != value) textW = value;
        }
    }

    #region SizeCol
    double sizeCol;
    public double SizeCol
    {
        get => innertCol?.Sum(elem => elem.SizeCol) ?? sizeCol;
        set 
        {
            if (innertCol == null && Math.Abs(sizeCol - value) > 0.001)
            {
                sizeCol = value;
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
    #endregion

    public int Level
    {
        get
        {
            var tmp = innertCol?.FirstOrDefault();
            if(tmp == null)
            {
                return 1;
            }
            return tmp.Level + 1;
        }
    }

    public void SetSizeColToAllLevels(int SizeCol)
    {
        if (innertCol == null) return;
        foreach (var item in innertCol)
        {
            item.sizeCol = SizeCol;
            item.SetSizeColToAllLevels(SizeCol);
        }
    }
    public List<DataGridColumns> GetLevel(int level)
    {
        var lstar = new List<DataGridColumns>();
        if (Level == 1)
        {
            lstar.Add(this);
            return lstar;
        }

        if (Level <= level)
        {
            return Level == level
                ? innertCol
                : lstar;
        }
        var allLevel = Level-1;
        List<DataGridColumns> lst = new(innertCol);
        while (allLevel != level)
        {
            List<DataGridColumns> lst2 = new();
            foreach (var item in lst)
            {
                lst2.AddRange(item.innertCol);
            }
            lst = lst2;
            var flag = true;
            foreach (var item in lst.Where(item => item.Level != level))
            {
                flag = false;
            }
            if (flag)
            {
                allLevel = level;
            }
        }
        return lst;
    }

    public static DataGridColumns operator+(DataGridColumns col1, DataGridColumns col2)
    {
        if(col1.innertCol == null)
        {
            if (col1.name == col2.name)
            {
                DataGridColumns ret = new()
                {
                    name = col1.name,
                    binding = col1.binding,
                    sizeCol = col1.sizeCol
                };
                if (col2.innertCol == null) return ret;
                ret.innertCol = new List<DataGridColumns>();
                foreach (var item in col2.innertCol)
                {
                    item.parent = ret;
                    ret.innertCol.Add(item);
                }
                return ret;
            }
            else
            {
                DataGridColumns ret = new()
                {
                    name = "",
                    binding = "",
                    innertCol = new List<DataGridColumns>()
                };
                if (ret.name == col1.name && col1.innertCol != null)
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
        if (col1.name==col2.name)
        {
            var tmp = col1.innertCol.ToList();
            tmp.AddRange(col2.innertCol);
            var group = tmp.GroupBy(x => x.name);
            DataGridColumns ret = new()
            {
                name = col1.name,
                innertCol = new List<DataGridColumns>()
            };
            foreach(var item in group)
            {
                var tr = item.FirstOrDefault();
                for(var i=1;i<item.Count();i++)
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
            DataGridColumns ret = new()
            {
                name = "",
                binding = "",
                innertCol = new List<DataGridColumns>()
            };
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
            DataGridColumns _ret = new()
            {
                name = ret.name,
                innertCol = new List<DataGridColumns>()
            };
            foreach (var item in group)
            {
                var tr = item.FirstOrDefault();
                for (var i = 1; i < item.Count(); i++)
                {
                    tr += item.ElementAt(i);
                }
                if (tr == null) continue;
                tr.parent = _ret;
                _ret.innertCol.Add(tr);
            }
            return _ret;
        }
    }

    #region INotifyPropertyChanged
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion
}