using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using DBRealization;
using Models.DataAccess;

namespace Collections
{
    public class Reports : IChanged, IKey
    {
        [NotMapped] private bool _isChanged = true;

        public Reports(DataAccessCollection Access)
        {
            DataAccess = Access;
            Init();
        }

        public Reports()
        {
            DataAccess = new DataAccessCollection();
            Init();
        }

        protected DataAccessCollection DataAccess { get; set; }
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

        public int? Report_CollectionId { get; set; }

        public virtual ObservableCollectionWithItemPropertyChanged<Report> Report_Collection
        {
            get
            {
                var tmp = DataAccess
                    .Get<ObservableCollectionWithItemPropertyChanged<Report>>(nameof(Report_Collection));
                return tmp.Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Report>>(nameof(Report_Collection)).Value =
                    value;
                OnPropertyChanged(nameof(Report_Collection));
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        private void Init()
        {
            DataAccess.Init(nameof(Report_Collection), Report_Collection_Validation,
                new ObservableCollectionWithItemPropertyChanged<Report>());
            DataAccess.Init(nameof(Master), Master_Validation, new Report());
            Report_Collection.CollectionChanged += CollectionChanged;
        }

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
            if (prop != nameof(IsChanged))
            {
                IsChanged = true;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        //Property Changed
    }
}