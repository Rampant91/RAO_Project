using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using DBRealization;
using Microsoft.EntityFrameworkCore;

namespace Collections
{
    public class DBObservable:INotifyPropertyChanged
    {
        [NotMapped] private bool _isChanged = true;

        public int Id { get; set; }

        public DBObservable()
        {
            Reports_Collection_DB = new ObservableCollectionWithItemPropertyChanged<Reports>();
            Reports_Collection.CollectionChanged += CollectionChanged;
        }

        ObservableCollectionWithItemPropertyChanged<Reports> Reports_Collection_DB;

        public virtual ObservableCollectionWithItemPropertyChanged<Reports> Reports_Collection
        {
            get
            {
                return Reports_Collection_DB;
            }
            set
            {
                Reports_Collection_DB = value;
                OnPropertyChanged(nameof(Reports_Collection));
            }
        }

        public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Reports_Collection));
        }

        private bool Reports_Collection_Validation(DbSet<Reports> value)
        {
            return true;
        }

        //Property Changed
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}