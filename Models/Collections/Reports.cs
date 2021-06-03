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
        IDataAccess _dataAccess { get; set; }

        public Reports(IDataAccess Access)
        {
            _dataAccess = Access;
            Init();
        }
        public Reports()
        {
            _dataAccess = new Models.DataAccess.RamAccess();
            Init();
        }

        void Init()
        {
            Report_Collection.CollectionChanged += CollectionChanged;
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
        public int ReportsId { get; set; }

        public virtual Report Master
        {
            get
            {
                if (GetErrors(nameof(Master)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Master));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Master), new Collections.Report());
                    }
                    tmp = _dataAccess.Get(nameof(Master));
                    return (Collections.Report)tmp;
                }
                else
                {
                    return _Master_Not_Valid;
                }
            }
            set
            {
                _Master_Not_Valid = value;
                if (GetErrors(nameof(Master)) == null)
                {
                    _dataAccess.Set(nameof(Master), _Master_Not_Valid);
                }
                OnPropertyChanged(nameof(Master));
            }
        }
        private Report _Master_Not_Valid = new Report();
        private bool Master_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Report> Report_Collection
        {
            get
            {
                if (GetErrors(nameof(Report_Collection)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Report_Collection));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Report_Collection), new ObservableCollection<Collections.Report>());
                    }
                    tmp = _dataAccess.Get(nameof(Report_Collection));
                    return (ObservableCollection<Collections.Report>)tmp;
                }
                else
                {
                    return _Report_Collection_Not_Valid;
                }
            }
            set
            {
                _Report_Collection_Not_Valid = value;
                if (GetErrors(nameof(Report_Collection)) == null)
                {
                    _dataAccess.Set(nameof(Report_Collection), _Report_Collection_Not_Valid);
                }
                OnPropertyChanged(nameof(Report_Collection));
            }
        }
        private ObservableCollection<Report> _Report_Collection_Not_Valid = new ObservableCollection<Report>();
        private bool Report_Collection_Validation()
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
