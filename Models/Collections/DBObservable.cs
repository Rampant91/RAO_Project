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

        }
        public DBObservable()
        {
            _dataAccess = new DataAccessCollection();
        }
        [Key]
        public IDataAccess<int> DBObservableId 
        {
            get
            {
                return _dataAccess.Get<int>(nameof(DBObservableId));
            }
            set
            {
                _dataAccess.Set(nameof(DBObservableId), value);
                OnPropertyChanged(nameof(DBObservableId));
            }
        }

        public virtual IDataAccess<ObservableCollection<Reports>> Reports_Collection
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Reports>>(nameof(Reports_Collection));
            }
            set
            {
                _dataAccess.Set(nameof(Reports_Collection), value);
                OnPropertyChanged(nameof(Reports_Collection));
            }
        }
        private bool Reports_Collection_Validation(IDataAccess<ObservableCollection<Reports>> value)
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
