using System;
using System.Collections.Generic;
using System.Text;

namespace Models.AccessInterface.Form
{
    public interface IDataAccess
    {
        int FormID { get; set; }
        object Get(string ParamName);
        void Set(string ParamName, object value);
    }
}
