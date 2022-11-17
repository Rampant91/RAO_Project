using Models.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Models.Attributes;

public class FormPropertyAttribute : Attribute
{
    public string[] Names { get; set; }
    public bool OneLevel { get; set; }

    public string Number { get; set; }

    public FormPropertyAttribute(bool isLastEnabled = true, params string[] names)
    {
        try
        {
            Convert.ToInt32(names[^1]);
            Number = names[^1];
            List<string> lst = new(names);
            if (!isLastEnabled)
            {
                lst.RemoveAt(lst.Count - 1);
            }
            Names = lst.ToArray();
        }
        catch
        {
            Names = names;
        }
    }

    public DataGridColumns GetDataColumnStructureD(DataGridColumns prevData = null)
    {
        DataGridColumns tmp = new() { name = Names[0] };
        if (Names.Length > 1)
        {
            if (Names[1] != null)
            {
                if (Names[1] != Names[0])
                {
                    tmp.innertCol = new List<DataGridColumns> { };
                }
            }
            var _tmp = tmp;
            for (var i = 1; i < Names.Length; i++)
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
                        if (Names[i] == Names[^1])
                        {
                            if (prevData != null)
                            {
                                var inner = prevData.innertCol[^1];
                                while (inner.innertCol != null)
                                {
                                    inner = inner.innertCol[^1];
                                }
                                var currentNum = Convert.ToInt32(inner.name);
                                it.name = Convert.ToString(currentNum + 1);
                            }
                        }
                    }
                    catch (Exception) { }

                    _tmp.innertCol.Add(it);
                    try
                    {
                        if (Names[i + 1] != null)
                        {
                            it.innertCol = new List<DataGridColumns>();
                            _tmp = it;
                        }
                    }
                    catch(Exception){ }
                }
            }
        }
        if (prevData != null)
        {
            DataGridColumns _tmp = new();
            if (prevData.Level > tmp.Level)
            {
                if (prevData.Level - 2 == tmp.Level)
                {
                    var newTmp = new DataGridColumns
                    {
                        name = "null-n",
                        innertCol = new List<DataGridColumns> { tmp }
                    };

                    _tmp.name = prevData.name;
                    _tmp.innertCol = new List<DataGridColumns> { newTmp };
                }
                else
                {
                    _tmp.name = prevData.name;
                    _tmp.innertCol = new List<DataGridColumns> { tmp };
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