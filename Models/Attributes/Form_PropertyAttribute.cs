using Models.Collections;
using System;

namespace Models.Attributes
{
    public class Form_PropertyAttribute : System.Attribute
    {
        public string[] Names { get; set; }
        public bool OneLevel { get; set; }
        public Form_PropertyAttribute(params string[] Names)
        {
            this.Names = Names;
            //this.OneLevel = oneLevel;
        }

        public DataGridColumns GetDataColumnStructureD()
        {
                DataGridColumns tmp = new();
            try {
                tmp.name = Names[0];
                if (Names.Length > 1)
                {
                    if (Names[1] != null)
                    {
                        if (Names[1] != Names[0])
                        {
                            tmp.innertCol = new System.Collections.Generic.List<DataGridColumns>() { };
                        }
                    }
                    DataGridColumns _tmp = tmp;
                    for (int i = 1; i < Names.Length; i++)
                    {
                        if (_tmp.name != Names[i])
                        {
                            DataGridColumns it = new();
                            it.name = Names[i];
                            it.parent = _tmp;
                            _tmp.innertCol.Add(it);
                            try
                            {
                                if (Names[i + 1] != null)
                                {
                                    it.innertCol = new System.Collections.Generic.List<DataGridColumns>();
                                    _tmp = it;
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception e) { }
            return tmp;
        }
    }
}
