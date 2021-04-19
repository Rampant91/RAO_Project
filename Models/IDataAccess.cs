using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public interface IDataAccess
    {
        object Get(string ParamName);
        void Set(string ParamName, object obj);
    }
}
