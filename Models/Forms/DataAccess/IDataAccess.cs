using System.Collections.Generic;

namespace Models.DataAccess
{
    public interface IDataAccess
    {
        object Get(string key);
        void Set(string key,object obj);
    }
}
