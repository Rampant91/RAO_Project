using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections;
using DBRealization;

namespace Models.Abstracts
{
    public abstract class Form:INotifyPropertyChanged,INotifyDataErrorInfo
    {
        protected IDataAccess _dataAccess { get; set; }

        public Form(IDataAccess Access)
        {
            _dataAccess = Access;
        }

        int _ID = -1;
        public int ID
        {
            get
            {
                return _dataAccess.RowID;
            }
        }

        //FormNum property
        [Attributes.Form_Property("Форма")]
        public string FormNum
        {
            get
            {
                if (GetErrors(nameof(FormNum)) == null)
                {
                    return (string)_dataAccess.Get(nameof(FormNum));
                }
                else
                {
                    return _FormNum_Not_Valid;
                }
            }
            set
            {
                _FormNum_Not_Valid = value;
                if (GetErrors(nameof(FormNum)) == null)
                {
                    _dataAccess.Set(nameof(FormNum), _FormNum_Not_Valid);
                }
                OnPropertyChanged(nameof(FormNum));
            }
        }
        private string _FormNum_Not_Valid;
        private void FormNum_Validation(string value)//Ready
        {
            ClearErrors(nameof(FormNum));
        }
        //FormNum property

        //NumberOfFields property
        public int NumberOfFields
        {
            get
            {
                if (GetErrors(nameof(NumberOfFields)) == null)
                {
                    return (int)_dataAccess.Get(nameof(NumberOfFields));
                }
                else
                {
                    return _NumberOfFields_Not_Valid;
                }
            }
            set
            {
                _NumberOfFields_Not_Valid = value;
                if (GetErrors(nameof(NumberOfFields)) == null)
                {
                    _dataAccess.Set(nameof(NumberOfFields), _NumberOfFields_Not_Valid);
                }
                OnPropertyChanged(nameof(NumberOfFields));
            }
        }
        private int _NumberOfFields_Not_Valid = -1;
        private void NumberOfFields_Validation()
        {
            ClearErrors(nameof(NumberOfFields));
        }
        //NumberOfFields property

        //Для валидации
        public abstract bool Object_Validation();
        //Для валидации

        //Property Changed
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //Property Changed

        //Data Validation
        protected readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        public bool HasErrors => _errorsByPropertyName.Any();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public IEnumerable GetErrors(string propertyName)
        {
            var tmp = _errorsByPropertyName.ContainsKey(propertyName) ?
                _errorsByPropertyName[propertyName] : null;
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
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
        protected void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }
        //Data Validation
    }
}
