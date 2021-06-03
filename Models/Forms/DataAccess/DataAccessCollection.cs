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

        public IDataAccess<T> Get<T>(string key)
        {
            if (Data.ContainsKey(key))
            {
                return (IDataAccess<T>)Data[key];
            }
            else
            {
                return null;
            }
        }
        public void Set<T>(string key, IDataAccess<T> obj)
        {
            if (obj != null)
            {
                Data[key] = obj;
            }
        }
    }
}
