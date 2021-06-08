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
    public class Reports : INotifyPropertyChanged
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
            _dataAccess.Init<ObservableCollectionWithItemPropertyChanged<Report>>(nameof(Report_Collection), Report_Collection_Validation, null);
            _dataAccess.Init<Report>(nameof(Master), Master_Validation, null);

            Report_Collection = new ObservableCollectionWithItemPropertyChanged<Report>();
            Report_Collection.CollectionChanged += CollectionChanged;
        }

        public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Report_Collection));
        }

        [Key]
        public int ReportsId { get; set; }

        public virtual RamAccess<Report> Master
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
        private bool Master_Validation(RamAccess<Report> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Report> Report_Collection
        {
            get
            {
                return _dataAccess.Get<ObservableCollectionWithItemPropertyChanged<Report>>(nameof(Report_Collection)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollectionWithItemPropertyChanged<Report>>(nameof(Report_Collection)).Value=value;
                OnPropertyChanged(nameof(Report_Collection));
            }
        }
        private bool Report_Collection_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Report>> value)
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
