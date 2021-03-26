using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DB_Model
{
    public enum TypeOfErrorAfterValidation
    {
        type1,
        type2,
        type3,
        none
    }
    class SPath
    {
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
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
        private TypeOfErrorAfterValidation _errorType = TypeOfErrorAfterValidation.none;
        public TypeOfErrorAfterValidation ErrorType
        {
            get
            {
                return _errorType;
            }
            set
            {
                _errorType = value;
            }
        }
        public SPath() { }
        public SPath(int id, object value, TypeOfErrorAfterValidation errorType)
        {
            Id = id;
            Value = value;
            ErrorType = errorType;
        }
    }
}
