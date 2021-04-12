using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections;

namespace Models
{
    public class Note : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        //RowNumber property
        [Attributes.Form_Property("№ строки")]
        public int RowNumber
        {
            get
            {
                if (GetErrors(nameof(Type)) != null)
                {
                    return (int)_RowNumber.Get();
                }
                else
                {
                    return _RowNumber_Not_Valid;
                }
            }
            set
            {
                _RowNumber_Not_Valid = value;
                if (GetErrors(nameof(Type)) != null)
                {
                    _RowNumber.Set(_RowNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(Type));
            }
        }
        private IDataLoadEngine _RowNumber;
        private int _RowNumber_Not_Valid = -1;
        private void RowNumber_Validation()
        {
            ClearErrors(nameof(Type));
        }
        //RowNumber property

        //GraphNumber property
        [Attributes.Form_Property("№ графы")]
        public int GraphNumber
        {
            get
            {
                if (GetErrors(nameof(Type)) != null)
                {
                    return (int)_GraphNumber.Get();
                }
                else
                {
                    return _GraphNumber_Not_Valid;
                }
            }
            set
            {
                _GraphNumber_Not_Valid = value;
                if (GetErrors(nameof(Type)) != null)
                {
                    _GraphNumber.Set(_GraphNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(Type));
            }
        }
        private IDataLoadEngine _GraphNumber;
        private int _GraphNumber_Not_Valid = -1;
        private void GraphNumber_Validation()
        {
            ClearErrors(nameof(Type));
        }
        //GraphNumber property

        //Comment property
        [Attributes.Form_Property("Пояснения")]
        public string Comment
        {
            get
            {
                if (GetErrors(nameof(Type)) != null)
                {
                    return (string)_Comment.Get();
                }
                else
                {
                    return _Comment_Not_Valid;
                }
            }
            set
            {
                _Comment_Not_Valid = value;
                if (GetErrors(nameof(Type)) != null)
                {
                    _Comment.Set(_Comment_Not_Valid);
                }
                OnPropertyChanged(nameof(Type));
            }
        }
        private IDataLoadEngine _Comment;
        private string _Comment_Not_Valid = "";
        private void Comment_Validation()
        {
            ClearErrors(nameof(Type));
        }
        //Comment property

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
