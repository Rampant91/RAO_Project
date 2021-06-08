using Models.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using Avalonia.Data;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DataAccess
{
    public class RamAccess<T> : INotifyDataErrorInfo, INotifyPropertyChanged
    {
        [NotMapped]
        public Func<RamAccess<T>, bool> Handler { get; set; }

        [Key]
        public int RamAccessID { get; set; }

        T _value;
        [NotMapped]
        public T Value 
        {
            get
            {
                return _value;
            }
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
        public RamAccess(Func<RamAccess<T>, bool> Handler,T Value)
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
            AddError("Value",error);
        }


        //Data Validation
        protected readonly List<string> _errorsByPropertyName = new List<string>();
        public bool HasErrors => _errorsByPropertyName.Any();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public IEnumerable GetErrors(string propertyName)
        {
            var tmp = _errorsByPropertyName.Count>0 ?
                _errorsByPropertyName : null;
            if (tmp != null)
            {
                List<Exception> lst = new List<Exception>();
                foreach (var item in tmp)
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
            if (_errorsByPropertyName.Count>0)
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

        //Property Changed
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //Property Changed
    }
}
