using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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

        [NotMapped]
        public ObservableCollectionWithItemPropertyChanged<Reports> Reports_Collection10
        {
            get
            {
                var obj = new ObservableCollectionWithItemPropertyChanged<Reports>();
                var sm = from t in Reports_Collection_DB where t.Master.FormNum.Value == "1.0" select t;
                foreach (var item in sm)
                {
                    obj.Add(item);
                }
                
                obj.CollectionChanged += CollectionChanged;
                return obj;
            }
        }

        [NotMapped]
        public ObservableCollectionWithItemPropertyChanged<Reports> Reports_Collection20
        {
            get
            {
                var obj = new ObservableCollectionWithItemPropertyChanged<Reports>();
                var sm = from t in Reports_Collection_DB where t.Master.FormNum.Value == "2.0" select t;
                foreach (var item in sm)
                {
                    obj.Add(item);
                }

                obj.CollectionChanged += CollectionChanged;
                return obj;
            }
        }

        public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Reports_Collection));
            OnPropertyChanged(nameof(Reports_Collection10));
            OnPropertyChanged(nameof(Reports_Collection20));
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