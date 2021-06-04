using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DataAccess
{
    public interface IDataAccessCollection
    {
        void Init<T>(string name, Func<RamAccess<T>, bool> handler, T value);
        RamAccess<T> Get<T>(string key);
        void Set<T>(string key, RamAccess<T> data);
    }
}
