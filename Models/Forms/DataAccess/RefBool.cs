using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DataAccess
{
    public class RefBool: RamAccess
    {

        private bool _value;
        public RefBool(bool value):base()
        {
            this._value = value;
        }

        public bool Get()
        {
            return _value;
        }
        public void Set(bool value)
        {

            _value = value;
            OnPropertyChanged();

        }
        public bool Equals(RefBool obj)
        {
            if (obj == null)
                return false;
            return this._value == obj._value;
        }
        public bool Equals(bool obj)
        {
            return this._value == obj;
        }
    }
}
