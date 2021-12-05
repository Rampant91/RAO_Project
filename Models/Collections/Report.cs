using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System;
using System.Text.RegularExpressions;
using Models.DBRealization;
using Models.Attributes;
using Models.DataAccess;
using OfficeOpenXml;
using System.Collections.Generic;

namespace Models.Collections
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
        ObservableCollectionWithItemPropertyChangedWithSum<Form17> Rows17_DB;
        public virtual ObservableCollectionWithItemPropertyChangedWithSum<Form17> Rows17
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
        ObservableCollectionWithItemPropertyChangedWithSum<Form18> Rows18_DB;
        public virtual ObservableCollectionWithItemPropertyChangedWithSum<Form18> Rows18
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
        ObservableCollectionWithItemPropertyChangedWithSum<Form21> Rows21_DB;
        public virtual ObservableCollectionWithItemPropertyChangedWithSum<Form21> Rows21
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
        ObservableCollectionWithItemPropertyChangedWithSum<Form22> Rows22_DB;
        public virtual ObservableCollectionWithItemPropertyChangedWithSum<Form22> Rows22
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

        [NotMapped]
        Dictionary<string, RamAccess> Dictionary { get; set; } = new Dictionary<string, RamAccess>();
        #region FormNum
        public string FormNum_DB { get; set; } = "";
        [NotMapped]
        [Form_Property("Форма")]
        public RamAccess<string> FormNum
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(FormNum)))
                {
                    ((RamAccess<string>)Dictionary[nameof(FormNum)]).Value = FormNum_DB;
                    return (RamAccess<string>)Dictionary[nameof(FormNum)];
                }
                else
                {
                    var rm = new RamAccess<string>(FormNum_Validation, FormNum_DB);
                    rm.PropertyChanged += FormNumValueChanged;
                    Dictionary.Add(nameof(FormNum), rm);
                    return (RamAccess<string>)Dictionary[nameof(FormNum)];
                }
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
        private bool FormNum_Validation(RamAccess<string> value)
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
                if (Dictionary.ContainsKey(nameof(IsCorrection)))
                {
                    ((RamAccess<bool>)Dictionary[nameof(IsCorrection)]).Value = IsCorrection_DB;
                    return (RamAccess<bool>)Dictionary[nameof(IsCorrection)];
                }
                else
                {
                    var rm = new RamAccess<bool>(IsCorrection_Validation, IsCorrection_DB);
                    rm.PropertyChanged += IsCorrectionValueChanged;
                    Dictionary.Add(nameof(IsCorrection), rm);
                    return (RamAccess<bool>)Dictionary[nameof(IsCorrection)];
                }
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
                if (Dictionary.ContainsKey(nameof(CorrectionNumber)))
                {
                    ((RamAccess<byte>)Dictionary[nameof(CorrectionNumber)]).Value = CorrectionNumber_DB;
                    return (RamAccess<byte>)Dictionary[nameof(CorrectionNumber)];
                }
                else
                {
                    var rm = new RamAccess<byte>(CorrectionNumber_Validation, CorrectionNumber_DB);
                    rm.PropertyChanged += CorrectionNumberValueChanged;
                    Dictionary.Add(nameof(CorrectionNumber), rm);
                    return (RamAccess<byte>)Dictionary[nameof(CorrectionNumber)];
                }
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

        #region FIOexecutor
        public string FIOexecutor_DB { get; set; } = "";
        [NotMapped]
        [Form_Property("Номер корректировки")]
        public RamAccess<string> FIOexecutor
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(FIOexecutor)))
                {
                    ((RamAccess<string>)Dictionary[nameof(FIOexecutor)]).Value = FIOexecutor_DB;
                    return (RamAccess<string>)Dictionary[nameof(FIOexecutor)];
                }
                else
                {
                    var rm = new RamAccess<string>(FIOexecutor_Validation, FIOexecutor_DB);
                    rm.PropertyChanged += FIOexecutorValueChanged;
                    Dictionary.Add(nameof(FIOexecutor), rm);
                    return (RamAccess<string>)Dictionary[nameof(FIOexecutor)];
                }
            }
            set
            {
                FIOexecutor_DB = value.Value;
                OnPropertyChanged(nameof(FIOexecutor));
            }
        }
        private void FIOexecutorValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                FIOexecutor_DB = ((RamAccess<string>)Value).Value;
                //OnPropertyChanged(nameof(FIOexecutor));
            }
        }
        private bool FIOexecutor_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region GradeExecutor
        public string GradeExecutor_DB { get; set; } = "";
        [NotMapped]
        [Form_Property("Номер корректировки")]
        public RamAccess<string> GradeExecutor
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(GradeExecutor)))
                {
                    ((RamAccess<string>)Dictionary[nameof(GradeExecutor)]).Value = GradeExecutor_DB;
                    return (RamAccess<string>)Dictionary[nameof(GradeExecutor)];
                }
                else
                {
                    var rm = new RamAccess<string>(GradeExecutor_Validation, GradeExecutor_DB);
                    rm.PropertyChanged += GradeExecutorValueChanged;
                    Dictionary.Add(nameof(GradeExecutor), rm);
                    return (RamAccess<string>)Dictionary[nameof(GradeExecutor)];
                }
            }
            set
            {
                GradeExecutor_DB = value.Value;
                OnPropertyChanged(nameof(GradeExecutor));
            }
        }
        private void GradeExecutorValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                GradeExecutor_DB = ((RamAccess<string>)Value).Value;
                //OnPropertyChanged(nameof(GradeExecutor));
            }
        }
        private bool GradeExecutor_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region ExecPhone
        public string ExecPhone_DB { get; set; } = "";
        [NotMapped]
        [Form_Property("Номер корректировки")]
        public RamAccess<string> ExecPhone
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ExecPhone)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ExecPhone)]).Value = ExecPhone_DB;
                    return (RamAccess<string>)Dictionary[nameof(ExecPhone)];
                }
                else
                {
                    var rm = new RamAccess<string>(ExecPhone_Validation, ExecPhone_DB);
                    rm.PropertyChanged += ExecPhoneValueChanged;
                    Dictionary.Add(nameof(ExecPhone), rm);
                    return (RamAccess<string>)Dictionary[nameof(ExecPhone)];
                }
            }
            set
            {
                ExecPhone_DB = value.Value;
                OnPropertyChanged(nameof(ExecPhone));
            }
        }
        private void ExecPhoneValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ExecPhone_DB = ((RamAccess<string>)Value).Value;
                //OnPropertyChanged(nameof(ExecPhone));
            }
        }
        private bool ExecPhone_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region ExecEmail
        public string ExecEmail_DB { get; set; } = "";
        [NotMapped]
        [Form_Property("Номер корректировки")]
        public RamAccess<string> ExecEmail
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ExecEmail)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ExecEmail)]).Value = ExecEmail_DB;
                    return (RamAccess<string>)Dictionary[nameof(ExecEmail)];
                }
                else
                {
                    var rm = new RamAccess<string>(ExecEmail_Validation, ExecEmail_DB);
                    rm.PropertyChanged += ExecEmailValueChanged;
                    Dictionary.Add(nameof(ExecEmail), rm);
                    return (RamAccess<string>)Dictionary[nameof(ExecEmail)];
                }
            }
            set
            {
                ExecEmail_DB = value.Value;
                OnPropertyChanged(nameof(ExecEmail));
            }
        }
        private void ExecEmailValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ExecEmail_DB = ((RamAccess<string>)Value).Value;
                //OnPropertyChanged(nameof(ExecEmail));
            }
        }
        private bool ExecEmail_Validation(RamAccess<string> value)
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
                if (Dictionary.ContainsKey(nameof(NumberInOrder)))
                {
                    ((RamAccess<string>)Dictionary[nameof(NumberInOrder)]).Value = NumberInOrder_DB;
                    return (RamAccess<string>)Dictionary[nameof(NumberInOrder)];
                }
                else
                {
                    var rm = new RamAccess<string>(NumberInOrder_Validation, NumberInOrder_DB);
                    rm.PropertyChanged += NumberInOrderValueChanged;
                    Dictionary.Add(nameof(NumberInOrder), rm);
                    return (RamAccess<string>)Dictionary[nameof(NumberInOrder)];
                }
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
                if (Dictionary.ContainsKey(nameof(Comments)))
                {
                    ((RamAccess<string>)Dictionary[nameof(Comments)]).Value = Comments_DB;
                    return (RamAccess<string>)Dictionary[nameof(Comments)];
                }
                else
                {
                    var rm = new RamAccess<string>(Comments_Validation, Comments_DB);
                    rm.PropertyChanged += CommentsValueChanged;
                    Dictionary.Add(nameof(Comments), rm);
                    return (RamAccess<string>)Dictionary[nameof(Comments)];
                }
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

        #region PermissionNumber_28 
        public string PermissionNumber_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Номер разрешительного документа")]
        public RamAccess<string> PermissionNumber_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PermissionNumber_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PermissionNumber_28)]).Value = PermissionNumber_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(PermissionNumber_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(PermissionNumber_28_Validation, PermissionNumber_28_DB);
                    rm.PropertyChanged += PermissionNumber_28ValueChanged;
                    Dictionary.Add(nameof(PermissionNumber_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(PermissionNumber_28)];
                }
            }
            set
            {
                PermissionNumber_28_DB = value.Value;
                OnPropertyChanged(nameof(PermissionNumber_28));
            }
        }

        private void PermissionNumber_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionNumber_28_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PermissionNumber_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region PermissionIssueDate_28
        public string PermissionIssueDate_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Дата выпуска разрешительного документа")]
        public RamAccess<string> PermissionIssueDate_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PermissionIssueDate_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PermissionIssueDate_28)]).Value = PermissionIssueDate_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(PermissionIssueDate_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(PermissionIssueDate_28_Validation, PermissionIssueDate_28_DB);
                    rm.PropertyChanged += PermissionIssueDate_28ValueChanged;
                    Dictionary.Add(nameof(PermissionIssueDate_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(PermissionIssueDate_28)];
                }
            }
            set
            {
                PermissionIssueDate_28_DB = value.Value;
                OnPropertyChanged(nameof(PermissionIssueDate_28));
            }
        }

        private void PermissionIssueDate_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                PermissionIssueDate_28_DB = tmp;
            }
        }
        private bool PermissionIssueDate_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region PermissionDocumentName_28 
        public string PermissionDocumentName_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Наименование разрешительного документа")]
        public RamAccess<string> PermissionDocumentName_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PermissionDocumentName_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PermissionDocumentName_28)]).Value = PermissionDocumentName_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(PermissionDocumentName_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(PermissionDocumentName_28_Validation, PermissionDocumentName_28_DB);
                    rm.PropertyChanged += PermissionDocumentName_28ValueChanged;
                    Dictionary.Add(nameof(PermissionDocumentName_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(PermissionDocumentName_28)];
                }
            }
            set
            {
                PermissionDocumentName_28_DB = value.Value;
                OnPropertyChanged(nameof(PermissionDocumentName_28));
            }
        }
        private void PermissionDocumentName_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionDocumentName_28_DB = ((RamAccess<string>)Value).Value;
            }
        }

        private bool PermissionDocumentName_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        #endregion

        #region ValidBegin_28
        public string ValidBegin_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Действует с")]
        public RamAccess<string> ValidBegin_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ValidBegin_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ValidBegin_28)]).Value = ValidBegin_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(ValidBegin_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(ValidBegin_28_Validation, ValidBegin_28_DB);
                    rm.PropertyChanged += ValidBegin_28ValueChanged;
                    Dictionary.Add(nameof(ValidBegin_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(ValidBegin_28)];
                }
            }
            set
            {
                ValidBegin_28_DB = value.Value;
                OnPropertyChanged(nameof(ValidBegin_28));
            }
        }

        private void ValidBegin_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                ValidBegin_28_DB = tmp;
            }
        }
        private bool ValidBegin_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region ValidThru_28
        public string ValidThru_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Действует по")]
        public RamAccess<string> ValidThru_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ValidThru_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ValidThru_28)]).Value = ValidThru_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(ValidThru_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(ValidThru_28_Validation, ValidThru_28_DB);
                    rm.PropertyChanged += ValidThru_28ValueChanged;
                    Dictionary.Add(nameof(ValidThru_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(ValidThru_28)];
                }
            }
            set
            {
                ValidThru_28_DB = value.Value;
                OnPropertyChanged(nameof(ValidThru_28));
            }
        }

        private void ValidThru_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                ValidThru_28_DB = tmp;
            }
        }
        private bool ValidThru_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region PermissionNumber1_28 
        public string PermissionNumber1_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Номер разрешительного документа")]
        public RamAccess<string> PermissionNumber1_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PermissionNumber1_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PermissionNumber1_28)]).Value = PermissionNumber1_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(PermissionNumber1_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(PermissionNumber1_28_Validation, PermissionNumber1_28_DB);
                    rm.PropertyChanged += PermissionNumber1_28ValueChanged;
                    Dictionary.Add(nameof(PermissionNumber1_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(PermissionNumber1_28)];
                }
            }
            set
            {
                PermissionNumber1_28_DB = value.Value;
                OnPropertyChanged(nameof(PermissionNumber1_28));
            }
        }
        private void PermissionNumber1_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionNumber1_28_DB = ((RamAccess<string>)Value).Value;
            }
        }

        private bool PermissionNumber1_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        #endregion

        #region PermissionIssueDate1_28 
        public string PermissionIssueDate1_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Дата выпуска разрешительного документа")]
        public RamAccess<string> PermissionIssueDate1_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PermissionIssueDate1_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PermissionIssueDate1_28)]).Value = PermissionIssueDate1_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(PermissionIssueDate1_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(PermissionIssueDate1_28_Validation, PermissionIssueDate1_28_DB);
                    rm.PropertyChanged += PermissionIssueDate1_28ValueChanged;
                    Dictionary.Add(nameof(PermissionIssueDate1_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(PermissionIssueDate1_28)];
                }
            }
            set
            {
                PermissionIssueDate1_28_DB = value.Value;
                OnPropertyChanged(nameof(PermissionIssueDate1_28));
            }
        }

        private void PermissionIssueDate1_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                PermissionIssueDate1_28_DB = tmp;
            }
        }
        private bool PermissionIssueDate1_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region PermissionDocumentName1_28
        public string PermissionDocumentName1_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Наименование разрешительного документа")]
        public RamAccess<string> PermissionDocumentName1_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PermissionDocumentName1_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PermissionDocumentName1_28)]).Value = PermissionDocumentName1_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(PermissionDocumentName1_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(PermissionDocumentName1_28_Validation, PermissionDocumentName1_28_DB);
                    rm.PropertyChanged += PermissionDocumentName1_28ValueChanged;
                    Dictionary.Add(nameof(PermissionDocumentName1_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(PermissionDocumentName1_28)];
                }
            }
            set
            {
                PermissionDocumentName1_28_DB = value.Value;
                OnPropertyChanged(nameof(PermissionDocumentName1_28));
            }
        }

        private void PermissionDocumentName1_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionDocumentName1_28_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PermissionDocumentName1_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        #endregion

        #region ValidBegin1_28
        public string ValidBegin1_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Действует с")]
        public RamAccess<string> ValidBegin1_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ValidBegin1_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ValidBegin1_28)]).Value = ValidBegin1_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(ValidBegin1_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(ValidBegin1_28_Validation, ValidBegin1_28_DB);
                    rm.PropertyChanged += ValidBegin1_28ValueChanged;
                    Dictionary.Add(nameof(ValidBegin1_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(ValidBegin1_28)];
                }
            }
            set
            {
                ValidBegin1_28_DB = value.Value;
                OnPropertyChanged(nameof(ValidBegin1_28));
            }
        }

        private void ValidBegin1_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                ValidBegin1_28_DB = tmp;
            }
        }
        private bool ValidBegin1_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region ValidThru1_28 
        public string ValidThru1_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Действует по")]
        public RamAccess<string> ValidThru1_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ValidThru1_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ValidThru1_28)]).Value = ValidThru1_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(ValidThru1_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(ValidThru1_28_Validation, ValidThru1_28_DB);
                    rm.PropertyChanged += ValidThru1_28ValueChanged;
                    Dictionary.Add(nameof(ValidThru1_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(ValidThru1_28)];
                }
            }
            set
            {
                ValidThru1_28_DB = value.Value;
                OnPropertyChanged(nameof(ValidThru1_28));
            }
        }

        private void ValidThru1_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                ValidThru1_28_DB = tmp;
            }
        }
        private bool ValidThru1_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region ContractNumber_28 
        public string ContractNumber_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Номер договора на передачу сточных вод")]
        public RamAccess<string> ContractNumber_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ContractNumber_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ContractNumber_28)]).Value = ContractNumber_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(ContractNumber_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(ContractNumber_28_Validation, ContractNumber_28_DB);
                    rm.PropertyChanged += ContractNumber_28ValueChanged;
                    Dictionary.Add(nameof(ContractNumber_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(ContractNumber_28)];
                }
            }
            set
            {
                ContractNumber_28_DB = value.Value;
                OnPropertyChanged(nameof(ContractNumber_28));
            }
        }

        private void ContractNumber_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ContractNumber_28_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool ContractNumber_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        #endregion

        #region ContractIssueDate2_28
        public string ContractIssueDate2_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Дата выпуска разрешительного документа")]
        public RamAccess<string> ContractIssueDate2_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ContractIssueDate2_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ContractIssueDate2_28)]).Value = ContractIssueDate2_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(ContractIssueDate2_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(ContractIssueDate2_28_Validation, ContractIssueDate2_28_DB);
                    rm.PropertyChanged += ContractIssueDate2_28ValueChanged;
                    Dictionary.Add(nameof(ContractIssueDate2_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(ContractIssueDate2_28)];
                }
            }
            set
            {
                ContractIssueDate2_28_DB = value.Value;
                OnPropertyChanged(nameof(ContractIssueDate2_28));
            }
        }

        private void ContractIssueDate2_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                ContractIssueDate2_28_DB = tmp;
            }
        }
        private bool ContractIssueDate2_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region OrganisationReciever_28
        public string OrganisationReciever_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Организация, осуществляющая прием сточных вод")]
        public RamAccess<string> OrganisationReciever_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(OrganisationReciever_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(OrganisationReciever_28)]).Value = OrganisationReciever_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(OrganisationReciever_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(OrganisationReciever_28_Validation, OrganisationReciever_28_DB);
                    rm.PropertyChanged += OrganisationReciever_28ValueChanged;
                    Dictionary.Add(nameof(OrganisationReciever_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(OrganisationReciever_28)];
                }
            }
            set
            {
                OrganisationReciever_28_DB = value.Value;
                OnPropertyChanged(nameof(OrganisationReciever_28));
            }
        }

        private void OrganisationReciever_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                OrganisationReciever_28_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool OrganisationReciever_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        #endregion

        #region ValidBegin2_28
        public string ValidBegin2_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Действует с")]
        public RamAccess<string> ValidBegin2_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ValidBegin2_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ValidBegin2_28)]).Value = ValidBegin2_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(ValidBegin2_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(ValidBegin2_28_Validation, ValidBegin2_28_DB);
                    rm.PropertyChanged += ValidBegin2_28ValueChanged;
                    Dictionary.Add(nameof(ValidBegin2_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(ValidBegin2_28)];
                }
            }
            set
            {
                ValidBegin2_28_DB = value.Value;
                OnPropertyChanged(nameof(ValidBegin2_28));
            }
        }

        private void ValidBegin2_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                ValidBegin2_28_DB = tmp;
            }
        }
        private bool ValidBegin2_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region ValidThru2_28
        public string ValidThru2_28_DB { get; set; } = "";[NotMapped]
        [Form_Property("Действует по")]
        public RamAccess<string> ValidThru2_28
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ValidThru2_28)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ValidThru2_28)]).Value = ValidThru2_28_DB;
                    return (RamAccess<string>)Dictionary[nameof(ValidThru2_28)];
                }
                else
                {
                    var rm = new RamAccess<string>(ValidThru2_28_Validation, ValidThru2_28_DB);
                    rm.PropertyChanged += ValidThru2_28ValueChanged;
                    Dictionary.Add(nameof(ValidThru2_28), rm);
                    return (RamAccess<string>)Dictionary[nameof(ValidThru2_28)];
                }
            }
            set
            {
                ValidThru2_28_DB = value.Value;
                OnPropertyChanged(nameof(ValidThru2_28));
            }
        }

        private void ValidThru2_28ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                ValidThru2_28_DB = tmp;
            }
        }
        private bool ValidThru2_28_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region  PermissionNumber27
        public string PermissionNumber27_DB { get; set; } = "";[NotMapped]
        [Form_Property("Номер разрешительного документа")]
        public RamAccess<string> PermissionNumber27
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PermissionNumber27)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PermissionNumber27)]).Value = PermissionNumber27_DB;
                    return (RamAccess<string>)Dictionary[nameof(PermissionNumber27)];
                }
                else
                {
                    var rm = new RamAccess<string>(PermissionNumber27_Validation, PermissionNumber27_DB);
                    rm.PropertyChanged += PermissionNumber27ValueChanged;
                    Dictionary.Add(nameof(PermissionNumber27), rm);
                    return (RamAccess<string>)Dictionary[nameof(PermissionNumber27)];
                }
            }
            set
            {
                PermissionNumber27_DB = value.Value;
                OnPropertyChanged(nameof(PermissionNumber27));
            }
        }

        private void PermissionNumber27ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionNumber27_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PermissionNumber27_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region  PermissionIssueDate27
        public string PermissionIssueDate27_DB { get; set; } = "";[NotMapped]
        [Form_Property("Дата выпуска разрешительного документа")]
        public RamAccess<string> PermissionIssueDate27
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PermissionIssueDate27)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PermissionIssueDate27)]).Value = PermissionIssueDate27_DB;
                    return (RamAccess<string>)Dictionary[nameof(PermissionIssueDate27)];
                }
                else
                {
                    var rm = new RamAccess<string>(PermissionIssueDate27_Validation, PermissionIssueDate27_DB);
                    rm.PropertyChanged += PermissionIssueDate27ValueChanged;
                    Dictionary.Add(nameof(PermissionIssueDate27), rm);
                    return (RamAccess<string>)Dictionary[nameof(PermissionIssueDate27)];
                }
            }
            set
            {
                PermissionIssueDate27_DB = value.Value;
                OnPropertyChanged(nameof(PermissionIssueDate27));
            }
        }

        private void PermissionIssueDate27ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                PermissionIssueDate27_DB = tmp;
            }
        }
        private bool PermissionIssueDate27_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region  PermissionDocumentName27
        public string PermissionDocumentName27_DB { get; set; } = "";[NotMapped]
        [Form_Property("Наименование разрешительного документа")]
        public RamAccess<string> PermissionDocumentName27
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PermissionDocumentName27)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PermissionDocumentName27)]).Value = PermissionDocumentName27_DB;
                    return (RamAccess<string>)Dictionary[nameof(PermissionDocumentName27)];
                }
                else
                {
                    var rm = new RamAccess<string>(PermissionDocumentName27_Validation, PermissionDocumentName27_DB);
                    rm.PropertyChanged += PermissionDocumentName27ValueChanged;
                    Dictionary.Add(nameof(PermissionDocumentName27), rm);
                    return (RamAccess<string>)Dictionary[nameof(PermissionDocumentName27)];
                }
            }
            set
            {
                PermissionDocumentName27_DB = value.Value;
                OnPropertyChanged(nameof(PermissionDocumentName27));
            }
        }


        private void PermissionDocumentName27ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionDocumentName27_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PermissionDocumentName27_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        #endregion

        #region  ValidBegin27
        public string ValidBegin27_DB { get; set; } = "";[NotMapped]
        [Form_Property("Действует с")]
        public RamAccess<string> ValidBegin27
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ValidBegin27)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ValidBegin27)]).Value = ValidBegin27_DB;
                    return (RamAccess<string>)Dictionary[nameof(ValidBegin27)];
                }
                else
                {
                    var rm = new RamAccess<string>(ValidBegin27_Validation, ValidBegin27_DB);
                    rm.PropertyChanged += ValidBegin27ValueChanged;
                    Dictionary.Add(nameof(ValidBegin27), rm);
                    return (RamAccess<string>)Dictionary[nameof(ValidBegin27)];
                }
            }
            set
            {
                ValidBegin27_DB = value.Value;
                OnPropertyChanged(nameof(ValidBegin27));
            }
        }


        private void ValidBegin27ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                ValidBegin27_DB = tmp;
            }
        }
        private bool ValidBegin27_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region  ValidThru27
        public string ValidThru27_DB { get; set; } = "";[NotMapped]
        [Form_Property("Действует по")]
        public RamAccess<string> ValidThru27
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ValidThru27)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ValidThru27)]).Value = ValidThru27_DB;
                    return (RamAccess<string>)Dictionary[nameof(ValidThru27)];
                }
                else
                {
                    var rm = new RamAccess<string>(ValidThru27_Validation, ValidThru27_DB);
                    rm.PropertyChanged += ValidThru27ValueChanged;
                    Dictionary.Add(nameof(ValidThru27), rm);
                    return (RamAccess<string>)Dictionary[nameof(ValidThru27)];
                }
            }
            set
            {
                ValidThru27_DB = value.Value;
                OnPropertyChanged(nameof(ValidThru27));
            }
        }


        private void ValidThru27ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                ValidThru27_DB = tmp;
            }
        }
        private bool ValidThru27_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region SourcesQuantity26
        public int? SourcesQuantity26_DB { get; set; } = null;
        [NotMapped]
        [Form_Property("Количество источников, шт.")]
        public RamAccess<int?> SourcesQuantity26
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(SourcesQuantity26)))
                {
                    ((RamAccess<int?>)Dictionary[nameof(SourcesQuantity26)]).Value = SourcesQuantity26_DB;
                    return (RamAccess<int?>)Dictionary[nameof(SourcesQuantity26)];
                }
                else
                {
                    var rm = new RamAccess<int?>(SourcesQuantity26_Validation, SourcesQuantity26_DB);
                    rm.PropertyChanged += SourcesQuantity26ValueChanged;
                    Dictionary.Add(nameof(SourcesQuantity26), rm);
                    return (RamAccess<int?>)Dictionary[nameof(SourcesQuantity26)];
                }
            }
            set
            {
                SourcesQuantity26_DB = value.Value;
                OnPropertyChanged(nameof(SourcesQuantity26));
            }
        }
        // positive int.
        private void SourcesQuantity26ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                SourcesQuantity26_DB = ((RamAccess<int?>)Value).Value;
            }
        }
        private bool SourcesQuantity26_Validation(RamAccess<int?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region Year
        public int? Year_DB { get; set; } = null;
        [NotMapped]
        [Form_Property("Отчетный год")]
        public RamAccess<int?> Year
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(Year)))
                {
                    ((RamAccess<int?>)Dictionary[nameof(Year)]).Value = Year_DB;
                    return (RamAccess<int?>)Dictionary[nameof(Year)];
                }
                else
                {
                    var rm = new RamAccess<int?>(Year_Validation, Year_DB);
                    rm.PropertyChanged += YearValueChanged;
                    Dictionary.Add(nameof(Year), rm);
                    return (RamAccess<int?>)Dictionary[nameof(Year)];
                }
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
                int k = (int)((RamAccess<int?>)Value).Value;
                if ((k >= 0) && (k < 100)) k += 2000;
                Year_DB = k;
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
            int k = (int)value.Value;
            if ((k >= 0) && (k < 100)) k += 2000;
            if ((k < 2010) || (k > 2060))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region StartPeriod
        public string StartPeriod_DB { get; set; } = "";
        [NotMapped]
        [Form_Property("Дата начала периода")]
        public RamAccess<string> StartPeriod
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(StartPeriod)))
                {
                    ((RamAccess<string>)Dictionary[nameof(StartPeriod)]).Value = StartPeriod_DB;
                    return (RamAccess<string>)Dictionary[nameof(StartPeriod)];
                }
                else
                {
                    var rm = new RamAccess<string>(StartPeriod_Validation, StartPeriod_DB);
                    rm.PropertyChanged += StartPeriodValueChanged;
                    Dictionary.Add(nameof(StartPeriod), rm);
                    return (RamAccess<string>)Dictionary[nameof(StartPeriod)];
                }
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
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                StartPeriod_DB = tmp;
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        private bool StartPeriod_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region EndPeriod
        public string EndPeriod_DB { get; set; } = "";
        [NotMapped]
        [Form_Property("Дата конца периода")]
        public RamAccess<string> EndPeriod
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(EndPeriod)))
                {
                    ((RamAccess<string>)Dictionary[nameof(EndPeriod)]).Value = EndPeriod_DB;
                    return (RamAccess<string>)Dictionary[nameof(EndPeriod)];
                }
                else
                {
                    var rm = new RamAccess<string>(EndPeriod_Validation, EndPeriod_DB);
                    rm.PropertyChanged += EndPeriodValueChanged;
                    Dictionary.Add(nameof(EndPeriod), rm);
                    return (RamAccess<string>)Dictionary[nameof(EndPeriod)];
                }
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
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                EndPeriod_DB = tmp;
                OnPropertyChanged(nameof(EndPeriod));
            }
        }
        private bool EndPeriod_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try
            {
                var end = DateTimeOffset.Parse(tmp);
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
                if (Dictionary.ContainsKey(nameof(ExportDate)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ExportDate)]).Value = ExportDate_DB;
                    return (RamAccess<string>)Dictionary[nameof(ExportDate)];
                }
                else
                {
                    var rm = new RamAccess<string>(ExportDate_Validation, ExportDate_DB);
                    rm.PropertyChanged += ExportDateValueChanged;
                    Dictionary.Add(nameof(ExportDate), rm);
                    return (RamAccess<string>)Dictionary[nameof(ExportDate)];
                }
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

            Notes.CleanIds();
        }

        [NotMapped]
        public List<IKey> this[string FormNum]
        {
            get
            {
                if(FormNum=="1.1")
                {
                    return Rows11.ToList<IKey>();
                }
                if (FormNum == "1.2")
                {
                    return Rows12.ToList<IKey>();
                }
                if (FormNum == "1.3")
                {
                    return Rows13.ToList<IKey>();
                }
                if (FormNum == "1.4")
                {
                    return Rows14.ToList<IKey>();
                }
                if (FormNum == "1.5")
                {
                    return Rows15.ToList<IKey>();
                }
                if (FormNum == "1.6")
                {
                    return Rows16.ToList<IKey>();
                }
                if (FormNum == "1.7")
                {
                    return Rows17.ToList<IKey>();
                }
                if (FormNum == "1.8")
                {
                    return Rows18.ToList<IKey>();
                }
                if (FormNum == "1.9")
                {
                    return Rows19.ToList<IKey>();
                }

                if (FormNum == "2.1")
                {
                    return Rows21.ToList<IKey>();
                }
                if (FormNum == "2.2")
                {
                    return Rows22.ToList<IKey>();
                }
                if (FormNum == "2.3")
                {
                    return Rows23.ToList<IKey>();
                }
                if (FormNum == "2.4")
                {
                    return Rows24.ToList<IKey>();
                }
                if (FormNum == "2.5")
                {
                    return Rows25.ToList<IKey>();
                }
                if (FormNum == "2.6")
                {
                    return Rows26.ToList<IKey>();
                }
                if (FormNum == "2.7")
                {
                    return Rows27.ToList<IKey>();
                }
                if (FormNum == "2.8")
                {
                    return Rows28.ToList<IKey>();
                }
                if (FormNum == "2.9")
                {
                    return Rows29.ToList<IKey>();
                }
                if (FormNum == "2.10")
                {
                    return Rows210.ToList<IKey>();
                }
                if (FormNum == "2.11")
                {
                    return Rows211.ToList<IKey>();
                }
                if (FormNum == "2.12")
                {
                    return Rows212.ToList<IKey>();
                }
                return null;
            }
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

            Rows17 = new ObservableCollectionWithItemPropertyChangedWithSum<Form17>();
            Rows17.CollectionChanged += CollectionChanged17;

            Rows18 = new ObservableCollectionWithItemPropertyChangedWithSum<Form18>();
            Rows18.CollectionChanged += CollectionChanged18;

            Rows19 = new ObservableCollectionWithItemPropertyChanged<Form19>();
            Rows19.CollectionChanged += CollectionChanged19;

            Rows20 = new ObservableCollectionWithItemPropertyChanged<Form20>();
            Rows20.CollectionChanged += CollectionChanged20;

            Rows21 = new ObservableCollectionWithItemPropertyChangedWithSum<Form21>();
            Rows21.CollectionChanged += CollectionChanged21;

            Rows22 = new ObservableCollectionWithItemPropertyChangedWithSum<Form22>();
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

        ~Report()
        {
            Rows10.CollectionChanged -= CollectionChanged10;
            Rows11.CollectionChanged -= CollectionChanged11;
            Rows12.CollectionChanged -= CollectionChanged12;
            Rows13.CollectionChanged -= CollectionChanged13;
            Rows14.CollectionChanged -= CollectionChanged14;
            Rows15.CollectionChanged -= CollectionChanged15;
            Rows16.CollectionChanged -= CollectionChanged16;
            Rows17.CollectionChanged -= CollectionChanged17;
            Rows18.CollectionChanged -= CollectionChanged18;
            Rows19.CollectionChanged -= CollectionChanged19;
            Rows20.CollectionChanged -= CollectionChanged20;
            Rows21.CollectionChanged -= CollectionChanged21;
            Rows22.CollectionChanged -= CollectionChanged22;
            Rows23.CollectionChanged -= CollectionChanged23;
            Rows24.CollectionChanged -= CollectionChanged24;
            Rows25.CollectionChanged -= CollectionChanged25;
            Rows26.CollectionChanged -= CollectionChanged26;
            Rows27.CollectionChanged -= CollectionChanged27;
            Rows28.CollectionChanged -= CollectionChanged28;
            Rows29.CollectionChanged -= CollectionChanged29;
            Rows210.CollectionChanged -= CollectionChanged210;
            Rows211.CollectionChanged -= CollectionChanged211;
            Rows212.CollectionChanged -= CollectionChanged212;
            Notes.CollectionChanged -= CollectionChangedNotes;
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

        #region IExcel

        public int ExcelRow(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true)
        {
            if (FormNum_DB.Split('.')[0] == "1")
            {
                worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = CorrectionNumber_DB;
                worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = StartPeriod_DB;
                worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = EndPeriod_DB;
                return 3;
            }
            if (FormNum_DB.Split('.')[0] == "2")
            {
                worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = CorrectionNumber_DB;
                worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = Year_DB;
                return 2;
            }
            return 0;
        }


        public static int ExcelHeader(ExcelWorksheet worksheet, string FormNum, int Row, int Column, bool Transpon = true)
        {
            if (FormNum.Split('.')[0] == "1")
            {
                worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Collections.Report,Models").GetProperty(nameof(CorrectionNumber))
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
                worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Collections.Report,Models").GetProperty(nameof(StartPeriod))
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
                worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Collections.Report,Models").GetProperty(nameof(EndPeriod))
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
                return 3;
            }
            if (FormNum.Split('.')[0] == "2")
            {
                worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Collections.Report,Models").GetProperty(nameof(CorrectionNumber))
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
                worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Collections.Report,Models").GetProperty(nameof(Year))
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
                return 2;
            }
            return 0;
        }

        #endregion

        #region Property Changed
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}