using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DataAccess
{
    public class DataAccessCollection:IDataAccessCollection
    {
        Dictionary<string, object> Data { get; set; }

        public DataAccessCollection()
        {
            Data = new Dictionary<string, object>();
        }

        public void Init<T>(string name,Func<RamAccess<T>, bool> handler,T value)
        {
            Data.Add(name,new RamAccess<T>(handler,value));
        }

        public RamAccess<T> Get<T>(string key)
        {
            return (RamAccess<T>)Data[key];
        }
        public void Set<T>(string key, RamAccess<T> obj)
        {
            Data[key] = obj;
        }
    }
}
