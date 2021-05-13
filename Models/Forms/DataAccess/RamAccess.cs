using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DataAccess
{
    public class RamAccess : IDataAccess
    {
        Dictionary<string,object> Data { get; set; }

        public RamAccess()
        {
            Data = new Dictionary<string, object>();
        }

        public object Get(string key)
        {
            if(Data.ContainsKey(key))
            {
                return Data[key];
            }
            else
            {
                return null;
            }
        }
        public void Set(string key,object obj)
        {
            if (obj != null)
            {
                Data[key] = obj;
            }
        }
    }
}
