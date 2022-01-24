using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Collections
{
    public interface IDataGridColumn
    {
        DataGridColumns GetColumnStructure(string param = "");
    }
}
