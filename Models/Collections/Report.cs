using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System;
using System.Text.RegularExpressions;
using Avalonia.Collections.Pooled;
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

        #region OkpoRep
        [NotMapped]
        public string _OkpoRep { get; set; } = "";
        [NotMapped]
        public RamAccess<string> OkpoRep
        {
            get
            {
                var tmp = Rows10[0].Okpo;
                tmp.PropertyChanged += OkpoRepValueChanged;
                return tmp;
            }
            set
            {
                _OkpoRep = value.Value;
                OnPropertyChanged(nameof(OkpoRep));
            }
        }
        private void OkpoRepValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                _OkpoRep = ((RamAccess<string>)Value).Value;
                OnPropertyChanged(nameof(OkpoRep));
            }
        }
#endregion

        #region RegNoRep
        [NotMapped]
        public string _RegNoRep { get; set; } = "";
        [NotMapped]
        public RamAccess<string> RegNoRep
        {
            get
            {
                var tmp = Rows10.First().RegNo;
                tmp.PropertyChanged += RegNoRepRepValueChanged;
                return tmp;
            }
            set
            {
                _RegNoRep = value.Value;
                OnPropertyChanged(nameof(RegNoRep));
            }
        }
        private void RegNoRepRepValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                _RegNoRep = ((RamAccess<string>)Value).Value;
            }
        }
        #endregion

        #region ShortJurLicoRep
        [NotMapped]
        public string _ShortJurLicoRep { get; set; } = "";
        [NotMapped]
        public RamAccess<string> ShortJurLicoRep
        {
            get
            {
                var tmp = Rows10.First().ShortJurLico;
                tmp.PropertyChanged += ShortJurLicoRepValueChanged;
                return tmp;
            }
            set
            {
                _ShortJurLicoRep = value.Value;
                OnPropertyChanged(nameof(ShortJurLicoRep));
            }
        }
        private void ShortJurLicoRepValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                _ShortJurLicoRep = ((RamAccess<string>)Value).Value;
            }
        }
        #endregion

        #region OkpoRep1
        [NotMapped]
        public string _OkpoRep1 { get; set; } = "";
        [NotMapped]
        public RamAccess<string> OkpoRep1
        {
            get
            {
                var tmp = Rows20.First().Okpo;
                tmp.PropertyChanged += OkpoRepValueChanged;
                return tmp;
            }
            set
            {
                _OkpoRep1 = value.Value;
                OnPropertyChanged(nameof(OkpoRep1));
            }
        }
        private void OkpoRepValue1Changed(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                _OkpoRep1 = ((RamAccess<string>)Value).Value;
            }
        }
        #endregion

        #region RegNoRep1
        [NotMapped]
        public string _RegNoRep1 { get; set; } = "";
        [NotMapped]
        public RamAccess<string> RegNoRep1
        {
            get
            {
                var tmp = Rows20.First().RegNo;
                tmp.PropertyChanged += RegNoRepRep1ValueChanged;
                return tmp;
            }
            set
            {
                _RegNoRep1 = value.Value;
                OnPropertyChanged(nameof(RegNoRep1));
            }
        }
        private void RegNoRepRep1ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                _RegNoRep1 = ((RamAccess<string>)Value).Value;
            }
        }
        #endregion

        #region ShortJurLicoRep1
        [NotMapped]
        public string _ShortJurLicoRep1 { get; set; } = "";
        [NotMapped]
        public RamAccess<string> ShortJurLicoRep1
        {
            get
            {
                var tmp = Rows20.First().ShortJurLico;
                tmp.PropertyChanged += ShortJurLicoRep1ValueChanged;
                return tmp;
            }
            set
            {
                _ShortJurLicoRep1 = value.Value;
                OnPropertyChanged(nameof(ShortJurLicoRep1));
            }
        }
        private void ShortJurLicoRep1ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                _ShortJurLicoRep1 = ((RamAccess<string>)Value).Value;
            }
        }
        #endregion

        #region  Forms10
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

        protected void CollectionChanged10(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(RegNoRep));
            OnPropertyChanged(nameof(OkpoRep));
            OnPropertyChanged(nameof(ShortJurLicoRep));

            OnPropertyChanged(nameof(Rows10));
        }
        #endregion

        #region Forms11
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

        protected void CollectionChanged11(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows11));
        }
        #endregion

        #region Forms12
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
        protected void CollectionChanged12(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows12));
        }
        #endregion

        #region Forms13
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
        protected void CollectionChanged13(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows13));
        }
        #endregion

        #region Forms14
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
        protected void CollectionChanged14(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows14));
        }
        #endregion

        #region Forms15
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
        protected void CollectionChanged15(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows15));
        }
        #endregion

        #region Forms16
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
        protected void CollectionChanged16(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows16));
        }
        #endregion

        #region Forms17
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
        protected void CollectionChanged17(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows17));
        }
        #endregion

        #region Forms18
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
        protected void CollectionChanged18(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows18));
        }
        #endregion

        #region Forms19
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
        protected void CollectionChanged19(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows19));
        }
        #endregion

        #region Forms20
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
        protected void CollectionChanged20(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(RegNoRep1));
            OnPropertyChanged(nameof(OkpoRep1));
            OnPropertyChanged(nameof(ShortJurLicoRep1));

            OnPropertyChanged(nameof(Rows20));
        }
        #endregion

        #region Forms21
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
        protected void CollectionChanged21(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows21));
        }
        #endregion

        #region Forms22
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
        protected void CollectionChanged22(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows22));
        }
        #endregion

        #region Forms23
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
        protected void CollectionChanged23(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows23));
        }
        #endregion

        #region Forms24
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
        protected void CollectionChanged24(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows24));
        }
        #endregion

        #region Forms25
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
        protected void CollectionChanged25(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows25));
        }
        #endregion

        #region Forms26
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
        protected void CollectionChanged26(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows26));
        }
        #endregion

        #region Forms27
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
        protected void CollectionChanged27(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows27));
        }
        #endregion

        #region Forms28
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
        protected void CollectionChanged28(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows28));
        }
        #endregion

        #region Forms29
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
        protected void CollectionChanged29(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows29));
        }
        #endregion

        #region Forms210
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
        protected void CollectionChanged210(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows210));
        }
        #endregion

        #region Forms211
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
        protected void CollectionChanged211(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows211));
        }
        #endregion

        #region Forms212
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
        protected void CollectionChanged212(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows212));
        }
#endregion

        #region FormNum
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
        #endregion

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
        private void IsCorrectionValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                IsCorrection_DB = ((RamAccess<bool>)Value).Value;
                OnPropertyChanged(nameof(IsCorrection));
            }
        }
        private bool IsCorrection_Validation(RamAccess<bool> value)
        {
            return true;
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
        private void CorrectionNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                CorrectionNumber_DB = ((RamAccess<byte>)Value).Value;
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private bool CorrectionNumber_Validation(RamAccess<byte> value)
        {
            value.ClearErrors();
            return true;
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
        private void CommentsValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Comments_DB = ((RamAccess<string>)Value).Value;
                OnPropertyChanged(nameof(Comments));
            }
        }
        private bool Comments_Validation(RamAccess<string> value)
        {
            return true;
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
        protected void CollectionChangedNotes(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Notes));
        }
        #endregion

        #region Year
        public int? Year_DB { get; set; } = null;
        [NotMapped]
        [Form_Property("Отчетный год:")]
        public RamAccess<int?> Year
        {
            get
            {
                var tmp = new RamAccess<int?>(Year_Validation, Year_DB);
                tmp.PropertyChanged += YearValueChanged;
                return tmp;
            }
            set
            {
                Year_DB = value.Value;
                OnPropertyChanged(nameof(Year));
            }
        }
        private void YearValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Year_DB = ((RamAccess<int?>)Value).Value;
                OnPropertyChanged(nameof(Year));
            }
        }
        private bool Year_Validation(RamAccess<int?> value)
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
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
        private void StartPeriodValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                StartPeriod_DB = ((RamAccess<string>)Value).Value;
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        private bool StartPeriod_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try
            {
                var start = DateTimeOffset.Parse(value.Value);
                //var end = DateTimeOffset.Parse(EndPeriod_DB);
                //if (start.Date >= end.Date)
                //{
                //    value.AddError("Начало периода должно быть раньше его конца");
                //    return false;
                //}
            }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                    //+ " начала или конца периода");
                return false;
            }
            return true;
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
        private void EndPeriodValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                EndPeriod_DB = ((RamAccess<string>)Value).Value;
                OnPropertyChanged(nameof(EndPeriod));
            }
        }
        private bool EndPeriod_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try
            {
                var end = DateTimeOffset.Parse(value.Value);
                var start = DateTimeOffset.Parse(StartPeriod_DB);
                if (start.Date > end.Date)
                {
                    value.AddError("Начало периода должно быть раньше его конца");
                    return false;
                }
            }
            catch (Exception)
            {
                value.AddError("Недопустимое значение начала или конца периода");
                return false;
            }
            return true;
        }
        #endregion

        #region ExportDate

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
        private void ExportDateValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ExportDate_DB = ((RamAccess<string>)Value).Value;
                OnPropertyChanged(nameof(ExportDate));
            }
        }
        private bool ExportDate_Validation(RamAccess<string> value)
        {
            return true;
        }
        #endregion

        public void CleanIds()
        {
            Id = 0;
            Rows10.CleanIds();
            Rows11.CleanIds();
            Rows12.CleanIds();
            Rows13.CleanIds();
            Rows14.CleanIds();
            Rows15.CleanIds();
            Rows16.CleanIds();
            Rows17.CleanIds();
            Rows18.CleanIds();
            Rows19.CleanIds();

            Rows20.CleanIds();
            Rows21.CleanIds();
            Rows22.CleanIds();
            Rows23.CleanIds();
            Rows24.CleanIds();
            Rows25.CleanIds();
            Rows26.CleanIds();
            Rows27.CleanIds();
            Rows28.CleanIds();
            Rows29.CleanIds();
            Rows210.CleanIds();
            Rows211.CleanIds();
            Rows212.CleanIds();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        private void Init()
        {
            Rows10 = new ObservableCollectionWithItemPropertyChanged<Form10>();
            Rows10.CollectionChanged += CollectionChanged10;

            Rows11 = new ObservableCollectionWithItemPropertyChanged<Form11>();
            Rows11.CollectionChanged += CollectionChanged11;

            Rows12 = new ObservableCollectionWithItemPropertyChanged<Form12>();
            Rows12.CollectionChanged += CollectionChanged12;

            Rows13 = new ObservableCollectionWithItemPropertyChanged<Form13>();
            Rows13.CollectionChanged += CollectionChanged13;

            Rows14 = new ObservableCollectionWithItemPropertyChanged<Form14>();
            Rows14.CollectionChanged += CollectionChanged14;

            Rows15 = new ObservableCollectionWithItemPropertyChanged<Form15>();
            Rows15.CollectionChanged += CollectionChanged15;

            Rows16 = new ObservableCollectionWithItemPropertyChanged<Form16>();
            Rows16.CollectionChanged += CollectionChanged16;
            
            Rows17 = new ObservableCollectionWithItemPropertyChanged<Form17>();
            Rows17.CollectionChanged += CollectionChanged17;

            Rows18 = new ObservableCollectionWithItemPropertyChanged<Form18>();
            Rows18.CollectionChanged += CollectionChanged18;

            Rows19 = new ObservableCollectionWithItemPropertyChanged<Form19>();
            Rows19.CollectionChanged += CollectionChanged19;

            Rows20 = new ObservableCollectionWithItemPropertyChanged<Form20>();
            Rows20.CollectionChanged += CollectionChanged20;

            Rows21 = new ObservableCollectionWithItemPropertyChanged<Form21>();
            Rows21.CollectionChanged += CollectionChanged21;

            Rows22 = new ObservableCollectionWithItemPropertyChanged<Form22>();
            Rows22.CollectionChanged += CollectionChanged22;

            Rows23 = new ObservableCollectionWithItemPropertyChanged<Form23>();
            Rows23.CollectionChanged += CollectionChanged23;

            Rows24 = new ObservableCollectionWithItemPropertyChanged<Form24>();
            Rows24.CollectionChanged += CollectionChanged24;

            Rows25 = new ObservableCollectionWithItemPropertyChanged<Form25>();
            Rows25.CollectionChanged += CollectionChanged25;

            Rows26 = new ObservableCollectionWithItemPropertyChanged<Form26>();
            Rows26.CollectionChanged += CollectionChanged26;

            Rows27 = new ObservableCollectionWithItemPropertyChanged<Form27>();
            Rows27.CollectionChanged += CollectionChanged27;

            Rows28 = new ObservableCollectionWithItemPropertyChanged<Form28>();
            Rows28.CollectionChanged += CollectionChanged28;

            Rows29 = new ObservableCollectionWithItemPropertyChanged<Form29>();
            Rows29.CollectionChanged += CollectionChanged29;

            Rows210 = new ObservableCollectionWithItemPropertyChanged<Form210>();
            Rows210.CollectionChanged += CollectionChanged210;

            Rows211 = new ObservableCollectionWithItemPropertyChanged<Form211>();
            Rows211.CollectionChanged += CollectionChanged211;

            Rows212 = new ObservableCollectionWithItemPropertyChanged<Form212>();
            Rows212.CollectionChanged += CollectionChanged212;

            Notes = new ObservableCollectionWithItemPropertyChanged<Note>();
            Notes.CollectionChanged += CollectionChangedNotes;
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

        #region Property Changed
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}