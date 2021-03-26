using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DB_Model
{
    class OrgInfoPath
    {
        private int _id;
        public int Id {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        private ValueInfo _valueInfo = new ValueInfo();
        public ValueInfo ValueInfo
        {
            get
            {
                return _valueInfo;
            }
            set
            {
                _valueInfo = value;
            }
        }
        private object _value = new object();
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        public OrgInfoPath() { }
        public OrgInfoPath(int id, ValueInfo valueInfo, object value)
        {
            Id = id;
            ValueInfo = valueInfo;
            Value = value;
        }
    }
}
