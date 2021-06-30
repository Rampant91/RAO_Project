using Collections;
using Models.DataAccess;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Collections
{
    public class Reports : IChanged, IKey
    {
        protected DataAccessCollection DataAccess { get; set; }
        protected DBRealization.DBModel dbm { get; set; }

        public Reports(DataAccessCollection Access)
        {
            dbm = DBRealization.StaticConfiguration.DBModel;
            DataAccess = Access;
            Init();
        }
        public Reports()
        {
            dbm = DBRealization.StaticConfiguration.DBModel;
            DataAccess = new DataAccessCollection();
            Init();
        }

        private void Init()
        {
            DataAccess.Init(nameof(Report_Collection), Report_Collection_Validation, new ObservableCollectionWithItemPropertyChanged<Report>());
            DataAccess.Init(nameof(Master), Master_Validation, new Report());

            dbm.Add(Master);
            //dbm.Add(Report_Collection);

            Report_Collection.CollectionChanged += CollectionChanged;
        }

        public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Report_Collection));
        }

        public int Id { get; set; }

        public int? MasterId { get; set; }
        public virtual RamAccess<Report> Master
        {
            get
            {
                var tmp = DataAccess.Get<Report>(nameof(Master));
                return tmp;
            }
            set
            {
                DataAccess.Set(nameof(Master), value);
                OnPropertyChanged(nameof(Master));
            }
        }
        private bool Master_Validation(RamAccess<Report> value)
        {
            return true;
        }

        public int? Report_CollectionId { get; set; }
        public virtual ObservableCollectionWithItemPropertyChanged<Report> Report_Collection
        {
            get
            {
                var tmp = DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Report>>(nameof(Report_Collection));
                return tmp.Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Report>>(nameof(Report_Collection)).Value = value;
                OnPropertyChanged(nameof(Report_Collection));
            }
        }
        private bool Report_Collection_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Report>> value)
        {
            return true;
        }

        [NotMapped]
        private bool _isChanged = true;
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

        //Property Changed
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (prop != nameof(IsChanged))
            {
                IsChanged = true;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //Property Changed
    }
}
