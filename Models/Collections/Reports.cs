using Models.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;

namespace Collections
{
    public class Reports : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        IDataAccessCollection _dataAccess { get; set; }

        public Reports(IDataAccessCollection Access)
        {
            _dataAccess = Access;
            Init();
        }
        public Reports()
        {
            _dataAccess = new DataAccessCollection();
            Init();
        }

        void Init()
        {
            Report_Collection.Value.CollectionChanged += CollectionChanged;
        }

        public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Report_Collection));

            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                foreach(var item in args.NewItems)
                {
                    ((Report)item).PropertyChanged += (x, e) =>
                    {
                        OnPropertyChanged(nameof(Report_Collection));
                    };
                }
            }
        }

        [Key]
        public IDataAccess<int> ReportsId
        {
            get
            {
                return _dataAccess.Get<int>(nameof(ReportsId));
            }
            set
            {
                _dataAccess.Set(nameof(ReportsId), value);
                OnPropertyChanged(nameof(ReportsId));
            }
        }

        public virtual IDataAccess<Report> Master
        {
            get
            {
                return _dataAccess.Get<Report>(nameof(Master));
            }
            set
            {
                _dataAccess.Set(nameof(Master), value);
                OnPropertyChanged(nameof(Master));
            }
        }
        private bool Master_Validation(IDataAccess<Report> value)
        {
            return true;
        }

        public virtual IDataAccess<ObservableCollection<Report>> Report_Collection
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Report>>(nameof(Master));
            }
            set
            {
                _dataAccess.Set(nameof(Master), value);
                OnPropertyChanged(nameof(Master));
            }
        }
        private bool Report_Collection_Validation(IDataAccess<ObservableCollection<Report>> value)
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
