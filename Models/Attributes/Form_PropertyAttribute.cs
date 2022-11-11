using Models.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Models.Attributes;

public class Form_PropertyAttribute : System.Attribute
{
    public string[] Names { get; set; }
    public bool OneLevel { get; set; }

    public string Number { get; set; }

    public Form_PropertyAttribute(bool IsLastEnabled=true,params string[] Names)
    {
        try
        {
            Convert.ToInt32(Names[Names.Length-1]);
            Number = Names[Names.Length - 1];
            List<string> lst = new(Names);
            if (!IsLastEnabled)
            {
                lst.RemoveAt(lst.Count - 1);
            }
            this.Names = lst.ToArray();
        }
        catch
        {
            this.Names = Names;
        }
    }

    public DataGridColumns GetDataColumnStructureD(DataGridColumns prev_data = null)
    {
        DataGridColumns tmp = new()
        {
            name = Names[0]
        };
        if (Names.Length > 1)
        {
            if (Names[1] != null)
            {
                if (Names[1] != Names[0])
                {
                    tmp.innertCol = new List<DataGridColumns> { };
                }
            }
            DataGridColumns _tmp = tmp;
            for (int i = 1; i < Names.Length; i++)
            {
                if (_tmp.name != Names[i])
                {
                    DataGridColumns it = new()
                    {
                        name = Names[i],
                        parent = _tmp
                    };

                    try
                    {
                        if (Names[i] == Names[Names.Count()-1])
                        {
                            if (prev_data != null)
                            {
                                var inner = prev_data.innertCol[prev_data.innertCol.Count() - 1];
                                while (inner.innertCol != null)
                                {
                                    inner = inner.innertCol[inner.innertCol.Count()-1];
                                }
                                var current_num = Convert.ToInt32(inner.name);
                                it.name = Convert.ToString(current_num+1);
                            }
                        }
                    }
                    catch (Exception e) { }

                    _tmp.innertCol.Add(it);
                    try
                    {
                        if (Names[i + 1] != null)
                        {
                            it.innertCol = new List<DataGridColumns>();
                            _tmp = it;
                        }
                    }
                    catch(Exception e){ }
                }
            }
        }
        if (prev_data != null)
        {
            DataGridColumns _tmp = new();
            if (prev_data.Level > tmp.Level)
            {
                if (prev_data.Level - 2 == tmp.Level)
                {
                    var new_tmp = new DataGridColumns
                    {
                        name = "null-n",
                        innertCol = new List<DataGridColumns>
                        {
                            tmp
                        }
                    };

                    _tmp.name = prev_data.name;
                    _tmp.innertCol = new List<DataGridColumns> { };
                    _tmp.innertCol.Add(new_tmp);

                }
                else
                {
                    _tmp.name = prev_data.name;
                    _tmp.innertCol = new List<DataGridColumns> { };
                    _tmp.innertCol.Add(tmp);
                }
            }
            else
            {
                _tmp = tmp;
            }
            return _tmp;
        }
        return tmp;
    }
}