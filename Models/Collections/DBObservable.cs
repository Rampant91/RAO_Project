using Models.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Collections
{
    public class DBObservable : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        IDataAccess _dataAccess { get; set; }

        public DBObservable(IDataAccess Access)
        {
            _dataAccess = Access;

        }
        public DBObservable()
        {
            _dataAccess = new Models.DataAccess.RamAccess();

        }
        [Key]
        public int DBObservableId { get; set; }

        public ObservableCollection<Reports> Reps
        {
            get
            {
                if (GetErrors(nameof(Reps)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Reps));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Reps), new ObservableCollection<Collections.Reports>());
                    }
                    tmp = _dataAccess.Get(nameof(Reps));
                    return (ObservableCollection<Collections.Reports>)tmp;
                }
                else
                {
                    return _Reps_Not_Valid;
                }
            }
            set
            {
                _Reps_Not_Valid = value;
                if (GetErrors(nameof(Reps)) == null)
                {
                    _dataAccess.Set(nameof(Reps), _Reps_Not_Valid);
                }
                OnPropertyChanged(nameof(Reps));
            }
        }
        private ObservableCollection<Reports> _Reps_Not_Valid = new ObservableCollection<Reports>();
        private bool Reps_Validation()
        {
            return true;
        }


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
