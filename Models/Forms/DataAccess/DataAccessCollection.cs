using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace Models.DataAccess
{
    public class DataAccessCollection
    {
        public Dictionary<string, object> Data { get; set; }

        public DataAccessCollection()
        {
            Data = new Dictionary<string, object>();
        }

        public override bool Equals(object obj)
        {
            if (obj is DataAccessCollection)
            {
                DataAccessCollection obj1 = this;
                DataAccessCollection obj2 = obj as DataAccessCollection;
                if (obj1.Data.Count == obj2.Data.Count)
                {
                    foreach (KeyValuePair<string, object> item1 in obj1.Data)
                    {
                        if (obj2.Data.ContainsKey(item1.Key))
                        {
                            dynamic tmp1 = obj2.Data[item1.Key];
                            dynamic tmp2 = item1;
                            if (tmp1 != tmp2)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator ==(DataAccessCollection obj1, DataAccessCollection obj2)
        {
            if (obj1 != null)
            {
                return !obj1.Equals(obj2);
            }
            else
            {
                return obj2 != null ? true : false;
            }
        }
        public static bool operator !=(DataAccessCollection obj1, DataAccessCollection obj2)
        {
            if (obj1 as object != null)
            {
                return !obj1.Equals(obj2);
            }
            else
            {
                return obj2 as object != null ? true : false;
            }
        }

        public void Init<T>(string name, Func<RamAccess<T>, bool> handler, T value)
        {
            Data.Add(name, new RamAccess<T>(handler, value));
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
