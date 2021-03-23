using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Client_Model
{
    public interface IDataLoadEngine
    {
        object Get();
        void Set(object arg);
    }
    public class File:IDataLoadEngine
    {
        public object value = null;
        public object Get()
        {
            return value;
        }
        public void Set(object arg)
        {
            value = arg;
        }
    }

    public class SQLite:IDataLoadEngine
    {
        public object value = null;
        public object Get()
        {
            return value;
        }
        public void Set(object arg)
        {
            value = arg;
        }
    }
}
