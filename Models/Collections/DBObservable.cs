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
using Models.Collections;

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
            _dataAccess.Init<ObservableCollectionWithItemPropertyChanged<Reports>>(nameof(Reports_Collection), Reports_Collection_Validation,null);
            Reports_Collection = new ObservableCollectionWithItemPropertyChanged<Reports>();
            Reports_Collection.CollectionChanged += CollectionChanged;
        }

        public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Reports_Collection));
        }

        [Key]
        public int DBObservableId { get; set; }

        public virtual ObservableCollectionWithItemPropertyChanged<Reports> Reports_Collection
        {
            get
            {
                return _dataAccess.Get<ObservableCollectionWithItemPropertyChanged<Reports>>(nameof(Reports_Collection)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollectionWithItemPropertyChanged<Reports>>(nameof(Reports_Collection)).Value=value;
                OnPropertyChanged(nameof(Reports_Collection));
            }
        }
        private bool Reports_Collection_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Reports>> value)
        {
            return true;
        }

        public bool Equals(object obj)
        {
            if (obj is DBObservable)
            {
                var obj1 = this;
                var obj2 = obj as DBObservable;

                return obj1._dataAccess == obj2._dataAccess;
            }
            else
            {
                return false;
            }
            return true;
        }

        public static bool operator ==(DBObservable obj1, DBObservable obj2)
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
        public static bool operator !=(DBObservable obj1, DBObservable obj2)
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
