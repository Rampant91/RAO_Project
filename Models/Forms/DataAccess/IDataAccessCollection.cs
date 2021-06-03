using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DataAccess
{
    public interface IDataAccessCollection
    {
        IDataAccess<T> Get<T>(string key);
        void Set<T>(string key,IDataAccess<T> data);
    }
}
