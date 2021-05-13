using Models.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Models
{
    public class Note : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        protected IDataAccess _dataAccess { get; set; }
        public Note(IDataAccess Access)
        {

        }

        //RowNumber property
        [Attributes.Form_Property("Номер строки")]
        public int RowNumber
        {
            get
            {
                if (GetErrors(nameof(RowNumber)) == null)
                {
                    return (int)_dataAccess.Get(nameof(RowNumber));
                }
                else
                {
                    return _RowNumber_Not_Valid;
                }
            }
            set
            {
                _RowNumber_Not_Valid = value;
                if (GetErrors(nameof(RowNumber)) == null)
                {
                    _dataAccess.Set(nameof(RowNumber), _RowNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(RowNumber));
            }
        }
        private int _RowNumber_Not_Valid;
        private void RowNumber_Validation()
        {
            ClearErrors(nameof(RowNumber));
        }
        //RowNumber property

        //GraphNumber property
        [Attributes.Form_Property("Номер графы")]
        public int GraphNumber
        {
            get
            {
                if (GetErrors(nameof(GraphNumber)) == null)
                {
                    return (int)_dataAccess.Get(nameof(GraphNumber));
                }
                else
                {
                    return _GraphNumber_Not_Valid;
                }
            }
            set
            {
                _GraphNumber_Not_Valid = value;
                if (GetErrors(nameof(GraphNumber)) == null)
                {
                    _dataAccess.Set(nameof(GraphNumber), _GraphNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(GraphNumber));
            }
        }
        private int _GraphNumber_Not_Valid;
        private void GraphNumber_Validation()
        {
            ClearErrors(nameof(RowNumber));
        }
        //GraphNumber property

        //Comment property
        [Attributes.Form_Property("Комментарий")]
        public string Comment
        {
            get
            {
                if (GetErrors(nameof(Comment)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Comment));
                }
                else
                {
                    return _Comment_Not_Valid;
                }
            }
            set
            {
                _Comment_Not_Valid = value;
                if (GetErrors(nameof(Comment)) == null)
                {
                    _dataAccess.Set(nameof(Comment), _Comment_Not_Valid);
                }
                OnPropertyChanged(nameof(Comment));
            }
        }
        private string _Comment_Not_Valid;
        private void Comment_Validation()
        {
            ClearErrors(nameof(RowNumber));
        }
        //Comment property

        //Для валидации
        public bool Object_Validation()
        {
            return true;
        }
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
