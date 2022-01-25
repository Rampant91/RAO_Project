using Models.Collections;

namespace Models.Attributes
{
    public class Form_PropertyAttribute : System.Attribute
    {
        public string[] Names { get; set; }

        public Form_PropertyAttribute(params string[] Names)
        {
            this.Names = Names;
        }

        public DataGridColumns GetDataColumnStructureD()
        {
            DataGridColumns tmp = new();
            tmp.name = Names[0];
            if (Names.Length > 1)
            {
                if (Names[1] != null)
                {
                    tmp.innertCol = new System.Collections.Generic.List<DataGridColumns>() { };
                }
                DataGridColumns _tmp = tmp;
                for (int i = 1; i < Names.Length; i++)
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
            return tmp;
        }
    }
}
