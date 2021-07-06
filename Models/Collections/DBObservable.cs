using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using DBRealization;
using Microsoft.EntityFrameworkCore;

namespace Collections
{
    public class DBObservable
    {
        [NotMapped] private bool _isChanged = true;

        private ObservableCollectionWithItemPropertyChanged<Reports> _reports_Collection=new ObservableCollectionWithItemPropertyChanged<Reports>();

        public int Id { get; set; }

        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                if (_isChanged != value)
                {
                    _isChanged = value;
                    OnPropertyChanged(nameof(IsChanged));
                }
            }
        }

        public DBObservable()
        {
            Reports_Collection.CollectionChanged += CollectionChanged;
        }

        public int? Reports_CollectionId { get; set; }

        public virtual ObservableCollectionWithItemPropertyChanged<Reports> Reports_Collection
        {
            get
            {
                return _reports_Collection;
            }
            set
            {
                _reports_Collection = value;
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
            if (prop != nameof(IsChanged))
            {
                IsChanged = true;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}