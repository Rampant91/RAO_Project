using Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Models.DataAccess
{
    public class RamAccess<T> : INotifyDataErrorInfo, IChanged, IKey
    {
        [NotMapped]
        public Func<RamAccess<T>, bool> Handler { get; set; }

        public int Id { get; set; }

        public int? ValueId { get; set; }
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
                if (Handler != null)
                {
                    Handler(this);
                }
            }
        }
        public RamAccess(Func<RamAccess<T>, bool> Handler, T Value)
        {
            this.Handler = Handler;
            this.Value = Value;
        }
        public RamAccess()
        {

        }

        public void ClearErrors()
        {
            ClearErrors("Value");
        }
        public void AddError(string error)
        {
            AddError("Value", error);
        }

        public override bool Equals(object obj)
        {
            if (obj is RamAccess<T>)
            {
                dynamic val1 = Value;
                dynamic val2 = (obj as RamAccess<T>).Value;
                return val1 == val2;
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(RamAccess<T> obj1, RamAccess<T> obj2)
        {
            if (obj1 as object != null)
            {
                return obj1.Equals(obj2);
            }
            else
            {
                return obj2 as object == null ? true : false;
            }
        }
        public static bool operator !=(RamAccess<T> obj1, RamAccess<T> obj2)
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


        //Data Validation
        protected readonly List<string> _errorsByPropertyName = new List<string>();
        public bool HasErrors => _errorsByPropertyName.Any();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public IEnumerable GetErrors(string propertyName)
        {
            List<string> tmp = _errorsByPropertyName.Count > 0 ?
                _errorsByPropertyName : null;
            if (tmp != null)
            {
                List<Exception> lst = new List<Exception>();
                foreach (string item in tmp)
                {
                    lst.Add(new Exception(item));
                }
                return lst;
            }
            else
            {
                return null;
            }
        }
        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        protected void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.Count > 0)
            {
                _errorsByPropertyName.Clear();
                OnErrorsChanged(propertyName);
            }
        }
        protected void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.Contains(error))
            {
                _errorsByPropertyName.Add(error);
                OnErrorsChanged(propertyName);
            }
        }
        //Data Validation

        [NotMapped]
        private bool _isChanged = true;
        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                if (_isChanged != value)
                {
                    _isChanged = value;
                    OnPropertyChanged(nameof(IsChanged));
                }
            }
        }

        //Property Changed
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (prop != nameof(IsChanged))
            {
                IsChanged = true;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
