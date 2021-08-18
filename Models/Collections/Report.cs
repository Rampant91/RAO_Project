using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using Models;
using Models.Attributes;
using Models.DataAccess;

namespace Collections
{
    public class Report : IKey, INotifyPropertyChanged
    {
        //ExportDate

        public enum Forms
        {
            None,
            Form10,
            Form11,
            Form12,
            Form13,
            Form14,
            Form15,
            Form16,
            Form17,
            Form18,
            Form19,
            Form20,
            Form21,
            Form22,
            Form23,
            Form24,
            Form25,
            Form26,
            Form27,
            Form28,
            Form29,
            Form210,
            Form211,
            Form212
        }

        [NotMapped] private bool _isChanged = true;
        [NotMapped] private Forms _lastAddedForm = Forms.None;

        [NotMapped]
        public Forms LastAddedForm
        {
            get
            {
                return _lastAddedForm;
            }
            set
            {
                if (!(value == _lastAddedForm)) _lastAddedForm = value;
            }
        }
        public Report()
        {
            Init();
        }

        ObservableCollectionWithItemPropertyChanged<Form10> Rows10_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form10> Rows10
        {
            get
            {
                return Rows10_DB;
            }
            set
            {
                Rows10_DB = value;
                OnPropertyChanged(nameof(Rows10));
            }
        }


        ObservableCollectionWithItemPropertyChanged<Form11> Rows11_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form11> Rows11
        {
            get
            {
                return Rows11_DB;
            }
            set
            {
                Rows11_DB = value;
                OnPropertyChanged(nameof(Rows11));
            }
        }


        ObservableCollectionWithItemPropertyChanged<Form12> Rows12_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form12> Rows12
        {
            get
            {
                return Rows12_DB;
            }
            set
            {
                Rows12_DB = value;
                OnPropertyChanged(nameof(Rows12));
            }
        }

        ObservableCollectionWithItemPropertyChanged<Form13> Rows13_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form13> Rows13
        {
            get
            {
                return Rows13_DB;
            }
            set
            {
                Rows13_DB = value;
                OnPropertyChanged(nameof(Rows13));
            }
        }

        ObservableCollectionWithItemPropertyChanged<Form14> Rows14_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form14> Rows14
        {
            get
            {
                return Rows14_DB;
            }
            set
            {
                Rows14_DB = value;
                OnPropertyChanged(nameof(Rows14));
            }
        }

        ObservableCollectionWithItemPropertyChanged<Form15> Rows15_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form15> Rows15
        {
            get
            {
                return Rows15_DB;
            }
            set
            {
                Rows15_DB = value;
                OnPropertyChanged(nameof(Rows15));
            }
        }

        ObservableCollectionWithItemPropertyChanged<Form16> Rows16_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form16> Rows16
        {
            get
            {
                return Rows16_DB;
            }
            set
            {
                Rows16_DB = value;
                OnPropertyChanged(nameof(Rows16));
            }
        }

        ObservableCollectionWithItemPropertyChanged<Form17> Rows17_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form17> Rows17
        {
            get
            {
                return Rows17_DB;
            }
            set
            {
                Rows17_DB = value;
                OnPropertyChanged(nameof(Rows17));
            }
        }

        ObservableCollectionWithItemPropertyChanged<Form18> Rows18_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form18> Rows18
        {
            get
            {
                return Rows18_DB;
            }
            set
            {
                Rows18_DB = value;
                OnPropertyChanged(nameof(Rows18));
            }
        }

        ObservableCollectionWithItemPropertyChanged<Form19> Rows19_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form19> Rows19
        {
            get
            {
                return Rows19_DB;
            }
            set
            {
                Rows19_DB = value;
                OnPropertyChanged(nameof(Rows19));
            }
        }

        ObservableCollectionWithItemPropertyChanged<Form20> Rows20_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form20> Rows20
        {
            get
            {
                return Rows20_DB;
            }
            set
            {
                Rows20_DB = value;
                OnPropertyChanged(nameof(Rows20));
            }
        }

        ObservableCollectionWithItemPropertyChanged<Form21> Rows21_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form21> Rows21
        {
            get
            {
                return Rows21_DB;
            }
            set
            {
                Rows21_DB = value;
                OnPropertyChanged(nameof(Rows21));
            }
        }


        ObservableCollectionWithItemPropertyChanged<Form22> Rows22_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form22> Rows22
        {
            get
            {
                return Rows22_DB;
            }
            set
            {
                Rows22_DB = value;
                OnPropertyChanged(nameof(Rows22));
            }
        }


        ObservableCollectionWithItemPropertyChanged<Form23> Rows23_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form23> Rows23
        {
            get
            {
                return Rows23_DB;
            }
            set
            {
                Rows23_DB = value;
                OnPropertyChanged(nameof(Rows23));
            }
        }


        ObservableCollectionWithItemPropertyChanged<Form24> Rows24_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form24> Rows24
        {
            get
            {
                return Rows24_DB;
            }
            set
            {
                Rows24_DB = value;
                OnPropertyChanged(nameof(Rows24));
            }
        }


        ObservableCollectionWithItemPropertyChanged<Form25> Rows25_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form25> Rows25
        {
            get
            {
                return Rows25_DB;
            }
            set
            {
                Rows25_DB = value;
                OnPropertyChanged(nameof(Rows25));
            }
        }

        ObservableCollectionWithItemPropertyChanged<Form26> Rows26_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form26> Rows26
        {
            get
            {
                return Rows26_DB;
            }
            set
            {
                Rows26_DB = value;
                OnPropertyChanged(nameof(Rows26));
            }
        }

        ObservableCollectionWithItemPropertyChanged<Form27> Rows27_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form27> Rows27
        {
            get
            {
                return Rows27_DB;
            }
            set
            {
                Rows27_DB = value;
                OnPropertyChanged(nameof(Rows27));
            }
        }


        ObservableCollectionWithItemPropertyChanged<Form28> Rows28_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form28> Rows28
        {
            get
            {
                return Rows28_DB;
            }
            set
            {
                Rows28_DB = value;
                OnPropertyChanged(nameof(Rows28));
            }
        }


        ObservableCollectionWithItemPropertyChanged<Form29> Rows29_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form29> Rows29
        {
            get
            {
                return Rows29_DB;
            }
            set
            {
                Rows29_DB = value;
                OnPropertyChanged(nameof(Rows29));
            }
        }


        ObservableCollectionWithItemPropertyChanged<Form210> Rows210_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form210> Rows210
        {
            get
            {
                return Rows210_DB;
            }
            set
            {
                Rows210_DB = value;
                OnPropertyChanged(nameof(Rows210));
            }
        }


        ObservableCollectionWithItemPropertyChanged<Form211> Rows211_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form211> Rows211
        {
            get
            {
                return Rows211_DB;
            }
            set
            {
                Rows211_DB = value;
                OnPropertyChanged(nameof(Rows211));
            }
        }


        ObservableCollectionWithItemPropertyChanged<Form212> Rows212_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Form212> Rows212
        {
            get
            {
                return Rows212_DB;
            }
            set
            {
                Rows212_DB = value;
                OnPropertyChanged(nameof(Rows212));
            }
        }

        public string FormNum_DB { get; set; } = "";
        [NotMapped]
        [Form_Property("Форма")]
        public RamAccess<string> FormNum
        {
            get
            {
                var tmp = new RamAccess<string>(null, FormNum_DB);
                tmp.PropertyChanged += FormNumValueChanged;
                return tmp;
            }
            set
            {
                FormNum_DB = value.Value;
                OnPropertyChanged(nameof(FormNum));
            }
        }

        #region IsCorrection
        public bool IsCorrection_DB { get; set; } = false;
        [NotMapped]
        [Form_Property("Корректирующий отчет")]
        public RamAccess<bool> IsCorrection
        {
            get
            {
                var tmp = new RamAccess<bool>(IsCorrection_Validation, IsCorrection_DB);
                tmp.PropertyChanged += IsCorrectionValueChanged;
                return tmp;
            }
            set
            {
                IsCorrection_DB = value.Value;
                OnPropertyChanged(nameof(IsCorrection));
            }
        }
        #endregion

        #region CorrectionNumber
        public byte CorrectionNumber_DB { get; set; } = 0;
        [NotMapped]
        [Form_Property("Номер корректировки")]
        public RamAccess<byte> CorrectionNumber
        {
            get
            {
                var tmp = new RamAccess<byte>(CorrectionNumber_Validation, CorrectionNumber_DB);
                tmp.PropertyChanged += CorrectionNumberValueChanged;
                return tmp;
            }
            set
            {
                CorrectionNumber_DB = value.Value;
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        #endregion

        #region NumberInOrder
        public string NumberInOrder_DB { get; set; } = "";
        [NotMapped]
        [Form_Property("Номер")]
        public RamAccess<string> NumberInOrder
        {
            get
            {
                var tmp = new RamAccess<string>(NumberInOrder_Validation, NumberInOrder_DB);
                tmp.PropertyChanged += NumberInOrderValueChanged;
                return tmp;
            }
            set
            {
                NumberInOrder_DB = value.Value;
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        #endregion

        #region Comments
        public string Comments_DB { get; set; } = "";
        [NotMapped]
        [Form_Property("Комментарий")]
        public RamAccess<string> Comments
        {
            get
            {
                var tmp = new RamAccess<string>(Comments_Validation, Comments_DB);
                tmp.PropertyChanged += CommentsValueChanged;
                return tmp;
            }
            set
            {
                Comments_DB = value.Value;
                OnPropertyChanged(nameof(Comments));
            }
        }
        #endregion

        #region Notes
        public int? NotesId { get; set; }

        [Form_Property("Примечания")]
        ObservableCollectionWithItemPropertyChanged<Note> Notes_DB;
        public virtual ObservableCollectionWithItemPropertyChanged<Note> Notes
        {
            get
            {
                return Notes_DB;
            }
            set
            {
                Notes_DB = value;
                OnPropertyChanged(nameof(Notes));
            }
        }
        #endregion

        #region StartPeriod
        public string StartPeriod_DB { get; set; } = "";
        [NotMapped]
        [Form_Property("Начало")]
        public RamAccess<string> StartPeriod
        {
            get
            {
                var tmp = new RamAccess<string>(StartPeriod_Validation, StartPeriod_DB);
                tmp.PropertyChanged += StartPeriodValueChanged;
                return tmp;
            }
            set
            {
                StartPeriod_DB = value.Value;
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        #endregion

        #region EndPeriod
        public string EndPeriod_DB { get; set; } = "";
        [NotMapped]
        [Form_Property("Конец")]
        public RamAccess<string> EndPeriod
        {
            get
            {
                var tmp = new RamAccess<string>(EndPeriod_Validation, EndPeriod_DB);
                tmp.PropertyChanged += EndPeriodValueChanged;
                return tmp;
            }
            set
            {
                EndPeriod_DB = value.Value;
                OnPropertyChanged(nameof(EndPeriod));
            }
        }
        #endregion

        //ExportDate
        public string ExportDate_DB { get; set; } = "";
        [NotMapped]
        [Form_Property("Дата выгрузки")]
        public RamAccess<string> ExportDate
        {
            get
            {
                var tmp = new RamAccess<string>(ExportDate_Validation, ExportDate_DB);
                tmp.PropertyChanged += ExportDateValueChanged;
                return tmp;
            }
            set
            {
                ExportDate_DB = value.Value;
                OnPropertyChanged(nameof(ExportDate));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        private void Init()
        {
            Rows10 = new ObservableCollectionWithItemPropertyChanged<Form10>();
            Rows10.CollectionChanged += CollectionChanged;

            Rows11 = new ObservableCollectionWithItemPropertyChanged<Form11>();
            Rows11.CollectionChanged += CollectionChanged;

            Rows12 = new ObservableCollectionWithItemPropertyChanged<Form12>();
            Rows12.CollectionChanged += CollectionChanged;

            Rows13 = new ObservableCollectionWithItemPropertyChanged<Form13>();
            Rows13.CollectionChanged += CollectionChanged;

            Rows14 = new ObservableCollectionWithItemPropertyChanged<Form14>();
            Rows14.CollectionChanged += CollectionChanged;

            Rows15 = new ObservableCollectionWithItemPropertyChanged<Form15>();
            Rows15.CollectionChanged += CollectionChanged;

            Rows16 = new ObservableCollectionWithItemPropertyChanged<Form16>();
            Rows16.CollectionChanged += CollectionChanged;

            Rows17 = new ObservableCollectionWithItemPropertyChanged<Form17>();
            Rows17.CollectionChanged += CollectionChanged;

            Rows18 = new ObservableCollectionWithItemPropertyChanged<Form18>();
            Rows18.CollectionChanged += CollectionChanged;

            Rows19 = new ObservableCollectionWithItemPropertyChanged<Form19>();
            Rows19.CollectionChanged += CollectionChanged;

            Rows20 = new ObservableCollectionWithItemPropertyChanged<Form20>();
            Rows20.CollectionChanged += CollectionChanged;

            Rows21 = new ObservableCollectionWithItemPropertyChanged<Form21>();
            Rows21.CollectionChanged += CollectionChanged;

            Rows22 = new ObservableCollectionWithItemPropertyChanged<Form22>();
            Rows22.CollectionChanged += CollectionChanged;

            Rows23 = new ObservableCollectionWithItemPropertyChanged<Form23>();
            Rows23.CollectionChanged += CollectionChanged;

            Rows24 = new ObservableCollectionWithItemPropertyChanged<Form24>();
            Rows24.CollectionChanged += CollectionChanged;

            Rows25 = new ObservableCollectionWithItemPropertyChanged<Form25>();
            Rows25.CollectionChanged += CollectionChanged;

            Rows26 = new ObservableCollectionWithItemPropertyChanged<Form26>();
            Rows26.CollectionChanged += CollectionChanged;

            Rows27 = new ObservableCollectionWithItemPropertyChanged<Form27>();
            Rows27.CollectionChanged += CollectionChanged;

            Rows28 = new ObservableCollectionWithItemPropertyChanged<Form28>();
            Rows28.CollectionChanged += CollectionChanged;

            Rows29 = new ObservableCollectionWithItemPropertyChanged<Form29>();
            Rows29.CollectionChanged += CollectionChanged;

            Rows210 = new ObservableCollectionWithItemPropertyChanged<Form210>();
            Rows210.CollectionChanged += CollectionChanged;

            Rows211 = new ObservableCollectionWithItemPropertyChanged<Form211>();
            Rows211.CollectionChanged += CollectionChanged;

            Rows212 = new ObservableCollectionWithItemPropertyChanged<Form212>();
            Rows212.CollectionChanged += CollectionChanged;

            Notes = new ObservableCollectionWithItemPropertyChanged<Note>();
            Notes.CollectionChanged += CollectionChanged;
        }

        private void FormNumValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                FormNum_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool FormNum_Validation(RamAccess<bool> value)
        {
            return true;
        }
        protected void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Notes));

            OnPropertyChanged(nameof(Rows10));
            OnPropertyChanged(nameof(Rows11));
            OnPropertyChanged(nameof(Rows12));
            OnPropertyChanged(nameof(Rows13));
            OnPropertyChanged(nameof(Rows14));
            OnPropertyChanged(nameof(Rows15));
            OnPropertyChanged(nameof(Rows16));
            OnPropertyChanged(nameof(Rows17));
            OnPropertyChanged(nameof(Rows18));
            OnPropertyChanged(nameof(Rows19));

            OnPropertyChanged(nameof(Rows20));
            OnPropertyChanged(nameof(Rows21));
            OnPropertyChanged(nameof(Rows22));
            OnPropertyChanged(nameof(Rows23));
            OnPropertyChanged(nameof(Rows24));
            OnPropertyChanged(nameof(Rows25));
            OnPropertyChanged(nameof(Rows26));
            OnPropertyChanged(nameof(Rows27));
            OnPropertyChanged(nameof(Rows28));
            OnPropertyChanged(nameof(Rows29));
            OnPropertyChanged(nameof(Rows210));
            OnPropertyChanged(nameof(Rows211));
            OnPropertyChanged(nameof(Rows212));
        }

        private void IsCorrectionValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                IsCorrection_DB = ((RamAccess<bool>)Value).Value;
            }
        }
        private bool IsCorrection_Validation(RamAccess<bool> value)
        {
            return true;
        }

        private void CorrectionNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                CorrectionNumber_DB = ((RamAccess<byte>)Value).Value;
            }
        }
        private bool CorrectionNumber_Validation(RamAccess<byte> value)
        {
            value.ClearErrors();
            return true;
        }

        private void NumberInOrderValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                NumberInOrder_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool NumberInOrder_Validation(RamAccess<string> value)
        {
            return true;
        }

        private void CommentsValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Comments_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool Comments_Validation(RamAccess<string> value)
        {
            return true;
        }

        private void NotesValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Notes_DB = ((RamAccess<ObservableCollectionWithItemPropertyChanged<Note>>)Value).Value;
            }
        }
        private bool Notes_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Note>> value)
        {
            return true;
        }

        private void StartPeriodValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                StartPeriod_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool StartPeriod_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        private void EndPeriodValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                EndPeriod_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool EndPeriod_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        private void ExportDateValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ExportDate_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool ExportDate_Validation(RamAccess<string> value)
        {
            return true;
        }

        //Property Changed
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        //Property Changed
    }
}