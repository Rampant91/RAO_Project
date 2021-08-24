using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using DBRealization;
using Models.DataAccess;

namespace Collections
{
    public class Reports : IKey, INotifyPropertyChanged
    {
        public Reports()
        {
            Init();
        }
        private void Init()
        {

            Report_Collection = new ObservableCollectionWithItemPropertyChanged<Report>();
            Report_Collection.CollectionChanged += CollectionChanged;
        }

        private Report _master = new Report();
        public virtual Report Master
        {
            get
            {
                return _master;
            }
            set
            {
                _master = value;
                OnPropertyChanged(nameof(Master));
            }
        }

        ObservableCollectionWithItemPropertyChanged<Report> Report_Collection_DB;

        public virtual ObservableCollectionWithItemPropertyChanged<Report> Report_Collection
        {
            get
            {
                return Report_Collection_DB;
            }
            set
            {
                Report_Collection_DB = value;
                OnPropertyChanged(nameof(Report_Collection));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Report_Collection));
        }

        private bool Master_Validation(RamAccess<Report> value)
        {
            return true;
        }

        private bool Report_Collection_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Report>> value)
        {
            return true;
        }

        //Property Changed
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        //Property Changed
    }
}