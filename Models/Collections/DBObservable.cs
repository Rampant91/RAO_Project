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
    public class DBObservable : INotifyPropertyChanged
    {
        IDataAccessCollection _dataAccess { get; set; }

        public DBObservable(IDataAccessCollection Access)
        {
            _dataAccess = Access;
            Init();
        }
        public DBObservable()
        {
            _dataAccess = new DataAccessCollection();
            Init();
        }

        void Init()
        {
            _dataAccess.Init<ObservableCollection<Reports>>(nameof(Reports_Collection), Reports_Collection_Validation,null);
            Reports_Collection = new ObservableCollection<Reports>();
        }

        [Key]
        public int DBObservableId { get; set; }

        public virtual ObservableCollection<Reports> Reports_Collection
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Reports>>(nameof(Reports_Collection)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Reports>>(nameof(Reports_Collection)).Value=value;
                OnPropertyChanged(nameof(Reports_Collection));
            }
        }
        private bool Reports_Collection_Validation(RamAccess<ObservableCollection<Reports>> value)
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
    }
}
