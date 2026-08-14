using Models.Attributes;
using Models.Forms.DataAccess;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Forms.Form3
{
    [Serializable]
    [Form_Class("Форма 3.2")]
    [Table(name: "form_32")]
    public class Form32 : Form
    {
        #region Consructor
        public Form32()
        {
            ExportedZriInfoCollection = new();
            ContainersInfoCollection = new();
            IdentificatorsCollection = new();
        }
        #endregion

        #region Properties

        #region AgreementIdNum (1)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string AgreementIdNum_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> AgreementIdNum
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(AgreementIdNum), out var value))
                {
                    ((RamAccess<string>)value).Value = AgreementIdNum_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(AgreementIdNum_Validation, AgreementIdNum_DB);
                rm.PropertyChanged += AgreementIdNum_ValueChanged;
                Dictionary.Add(nameof(AgreementIdNum), rm);
                return (RamAccess<string>)Dictionary[nameof(AgreementIdNum)];
            }
            set
            {
                AgreementIdNum_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void AgreementIdNum_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (AgreementIdNum_DB != value1)
            {
                AgreementIdNum_DB = value1;
            }
        }

        protected static bool AgreementIdNum_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region DeliveryDay (2)

        public DateOnly? DeliveryDay_DB { get; set; }

        [NotMapped]
        public RamAccess<DateOnly?> DeliveryDay
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(DeliveryDay), out var value))
                {
                    ((RamAccess<DateOnly?>)value).Value = DeliveryDay_DB;
                    return (RamAccess<DateOnly?>)value;
                }
                var rm = new RamAccess<DateOnly?>(DeliveryDay_Validation, DeliveryDay_DB);
                rm.PropertyChanged += DeliveryDay_ValueChanged;
                Dictionary.Add(nameof(DeliveryDay), rm);
                return (RamAccess<DateOnly?>)Dictionary[nameof(DeliveryDay)];
            }
            set
            {
                DeliveryDay_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void DeliveryDay_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<DateOnly?>)value).Value;
            if (DeliveryDay_DB != value1)
            {
                DeliveryDay_DB = value1;
            }
        }

        protected static bool DeliveryDay_Validation(RamAccess<DateOnly?> value)//Ready
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

        #region RecipientName (3.1)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string RecipientName_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> RecipientName
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(RecipientName), out var value))
                {
                    ((RamAccess<string>)value).Value = RecipientName_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(RecipientName_Validation, RecipientName_DB);
                rm.PropertyChanged += RecipientName_ValueChanged;
                Dictionary.Add(nameof(RecipientName), rm);
                return (RamAccess<string>)Dictionary[nameof(RecipientName)];
            }
            set
            {
                RecipientName_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void RecipientName_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (RecipientName_DB != value1)
            {
                RecipientName_DB = value1;
            }
        }

        protected static bool RecipientName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region WorkTypes (3.2)
        public bool IsRvProduction { get; set; }
        public bool IsRvTransportation { get; set; }
        public bool IsRvExploitation { get; set; }
        public bool IsRvStoring { get; set; }
        public bool IsRvRecycling { get; set; }
        public bool IsRaoTransportation { get; set; }
        public bool IsRaoStoring { get; set; }
        public bool IsRaoRecycling { get; set; }
        #endregion

        #region LicenseNumRv (3.3.1)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string LicenseNumRv_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> LicenseNumRv
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(LicenseNumRv), out var value))
                {
                    ((RamAccess<string>)value).Value = LicenseNumRv_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(LicenseNumRv_Validation, LicenseNumRv_DB);
                rm.PropertyChanged += LicenseNumRv_ValueChanged;
                Dictionary.Add(nameof(LicenseNumRv), rm);
                return (RamAccess<string>)Dictionary[nameof(LicenseNumRv)];
            }
            set
            {
                LicenseNumRv_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void LicenseNumRv_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (LicenseNumRv_DB != value1)
            {
                LicenseNumRv_DB = value1;
            }
        }

        protected static bool LicenseNumRv_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region LicenseNumRao (3.3.2)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string LicenseNumRao_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> LicenseNumRao
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(LicenseNumRao), out var value))
                {
                    ((RamAccess<string>)value).Value = LicenseNumRao_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(LicenseNumRao_Validation, LicenseNumRao_DB);
                rm.PropertyChanged += LicenseNumRao_ValueChanged;
                Dictionary.Add(nameof(LicenseNumRao), rm);
                return (RamAccess<string>)Dictionary[nameof(LicenseNumRao)];
            }
            set
            {
                LicenseNumRao_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void LicenseNumRao_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (LicenseNumRao_DB != value1)
            {
                LicenseNumRao_DB = value1;
            }
        }

        protected static bool LicenseNumRao_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region LicenseExpirationDateRv (3.4.1)

        public DateOnly? LicenseExpirationDateRv_DB { get; set; }

        [NotMapped]
        public RamAccess<DateOnly?> LicenseExpirationDateRv
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(LicenseExpirationDateRv), out var value))
                {
                    ((RamAccess<DateOnly?>)value).Value = LicenseExpirationDateRv_DB;
                    return (RamAccess<DateOnly?>)value;
                }
                var rm = new RamAccess<DateOnly?>(LicenseExpirationDateRv_Validation, LicenseExpirationDateRv_DB);
                rm.PropertyChanged += LicenseExpirationDateRv_ValueChanged;
                Dictionary.Add(nameof(LicenseExpirationDateRv), rm);
                return (RamAccess<DateOnly?>)Dictionary[nameof(LicenseExpirationDateRv)];
            }
            set
            {
                LicenseExpirationDateRv_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void LicenseExpirationDateRv_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<DateOnly?>)value).Value;
            if (LicenseExpirationDateRv_DB != value1)
            {
                LicenseExpirationDateRv_DB = value1;
            }
        }

        protected static bool LicenseExpirationDateRv_Validation(RamAccess<DateOnly?> value)//Ready
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

        #region LicenseExpirationDateRao (3.4.2)

        public DateOnly? LicenseExpirationDateRao_DB { get; set; }

        [NotMapped]
        public RamAccess<DateOnly?> LicenseExpirationDateRao
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(LicenseExpirationDateRao), out var value))
                {
                    ((RamAccess<DateOnly?>)value).Value = LicenseExpirationDateRao_DB;
                    return (RamAccess<DateOnly?>)value;
                }
                var rm = new RamAccess<DateOnly?>(LicenseExpirationDateRao_Validation, LicenseExpirationDateRao_DB);
                rm.PropertyChanged += LicenseExpirationDateRao_ValueChanged;
                Dictionary.Add(nameof(LicenseExpirationDateRao), rm);
                return (RamAccess<DateOnly?>)Dictionary[nameof(LicenseExpirationDateRao)];
            }
            set
            {
                LicenseExpirationDateRao_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void LicenseExpirationDateRao_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<DateOnly?>)value).Value;
            if (LicenseExpirationDateRao_DB != value1)
            {
                LicenseExpirationDateRao_DB = value1;
            }
        }

        protected static bool LicenseExpirationDateRao_Validation(RamAccess<DateOnly?> value)//Ready
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

        #region DeliveryAddress (4)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string DeliveryAddress_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> DeliveryAddress
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(DeliveryAddress), out var value))
                {
                    ((RamAccess<string>)value).Value = DeliveryAddress_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(DeliveryAddress_Validation, DeliveryAddress_DB);
                rm.PropertyChanged += DeliveryAddress_ValueChanged;
                Dictionary.Add(nameof(DeliveryAddress), rm);
                return (RamAccess<string>)Dictionary[nameof(DeliveryAddress)];
            }
            set
            {
                DeliveryAddress_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void DeliveryAddress_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (DeliveryAddress_DB != value1)
            {
                DeliveryAddress_DB = value1;
            }
        }

        protected static bool DeliveryAddress_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region RadionuclidCompositionZri (5)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string RadionuclidCompositionZri_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> RadionuclidCompositionZri
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(RadionuclidCompositionZri), out var value))
                {
                    ((RamAccess<string>)value).Value = RadionuclidCompositionZri_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(RadionuclidCompositionZri_Validation, RadionuclidCompositionZri_DB);
                rm.PropertyChanged += RadionuclidCompositionZri_ValueChanged;
                Dictionary.Add(nameof(RadionuclidCompositionZri), rm);
                return (RamAccess<string>)Dictionary[nameof(RadionuclidCompositionZri)];
            }
            set
            {
                RadionuclidCompositionZri_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void RadionuclidCompositionZri_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (RadionuclidCompositionZri_DB != value1)
            {
                RadionuclidCompositionZri_DB = value1;
            }
        }

        protected static bool RadionuclidCompositionZri_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region TotalActivity (6)
        public double? TotalActivity_DB { get; set; } = 0;

        [NotMapped]
        public RamAccess<double?> TotalActivity
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(TotalActivity), out var value))
                {
                    ((RamAccess<double?>)value).Value = TotalActivity_DB;
                    return (RamAccess<double?>)value;
                }
                var rm = new RamAccess<double?>(TotalActivity_Validation, TotalActivity_DB);
                rm.PropertyChanged += TotalActivity_ValueChanged;
                Dictionary.Add(nameof(TotalActivity), rm);
                return (RamAccess<double?>)Dictionary[nameof(TotalActivity)];
            }
            set
            {
                TotalActivity_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void TotalActivity_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<double?>)value).Value;
            if (TotalActivity_DB != value1)
            {
                TotalActivity_DB = value1;
            }
        }

        private bool TotalActivity_Validation(RamAccess<double?> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region TotalCount (7)
        public int? TotalCount_DB { get; set; } = 0;

        [NotMapped]
        public RamAccess<int?> TotalCount
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(TotalCount), out var value))
                {
                    ((RamAccess<int?>)value).Value = TotalCount_DB;
                    return (RamAccess<int?>)value;
                }
                var rm = new RamAccess<int?>(TotalCount_Validation, TotalCount_DB);
                rm.PropertyChanged += TotalCount_ValueChanged;
                Dictionary.Add(nameof(TotalCount), rm);
                return (RamAccess<int?>)Dictionary[nameof(TotalCount)];
            }
            set
            {
                TotalCount_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void TotalCount_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<int?>)value).Value;
            if (TotalCount_DB != value1)
            {
                TotalCount_DB = value1;
            }
        }

        private bool TotalCount_Validation(RamAccess<int?> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        public ObservableCollection<ExportedZriInfo> ExportedZriInfoCollection { get; set; }
        public ObservableCollection<ContainerInfo> ContainersInfoCollection { get; set; }
        public ObservableCollection<Identificator> IdentificatorsCollection { get; set; }
        #endregion

        #region InheritedMethods
        public override string ConvertToTSVstring()
        {
            throw new NotImplementedException();
        }
        public override void ExcelGetRow(ExcelWorksheet worksheet, int row)
        {
            throw new NotImplementedException();
        }
        public override bool Object_Validation()
        {
            throw new NotImplementedException();
        }
        public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
        {
            throw new NotImplementedException();
        }
        public override bool IsContentEqual(Form other)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    [Serializable]
    [Table(name: "form_32_table_1")]
    public class ExportedZriInfo : Form
    {
        #region ForeignKey Form32Id
        public int Form32Id { get; set; }

        private Form32 _form32;

        [ForeignKey(nameof(Form32Id))]
        public Form32 Form32
        {
            get
            {
                return _form32;
            }
            private set
            {
                _form32 = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region PassportNum (8.1)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string PassportNum_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> PassportNum
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(PassportNum), out var value))
                {
                    ((RamAccess<string>)value).Value = PassportNum_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(PassportNum_Validation, PassportNum_DB);
                rm.PropertyChanged += PassportNum_ValueChanged;
                Dictionary.Add(nameof(PassportNum), rm);
                return (RamAccess<string>)Dictionary[nameof(PassportNum)];
            }
            set
            {
                PassportNum_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void PassportNum_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (PassportNum_DB != value1)
            {
                PassportNum_DB = value1;
            }
        }

        protected static bool PassportNum_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region Type (8.2)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string Type_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> Type
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(Type), out var value))
                {
                    ((RamAccess<string>)value).Value = Type_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Type_Validation, Type_DB);
                rm.PropertyChanged += Type_ValueChanged;
                Dictionary.Add(nameof(Type), rm);
                return (RamAccess<string>)Dictionary[nameof(Type)];
            }
            set
            {
                Type_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void Type_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (Type_DB != value1)
            {
                Type_DB = value1;
            }
        }

        protected static bool Type_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region FactoryNum (8.3)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string FactoryNum_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> FactoryNum
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(FactoryNum), out var value))
                {
                    ((RamAccess<string>)value).Value = FactoryNum_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(FactoryNum_Validation, FactoryNum_DB);
                rm.PropertyChanged += FactoryNum_ValueChanged;
                Dictionary.Add(nameof(FactoryNum), rm);
                return (RamAccess<string>)Dictionary[nameof(FactoryNum)];
            }
            set
            {
                FactoryNum_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void FactoryNum_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (FactoryNum_DB != value1)
            {
                FactoryNum_DB = value1;
            }
        }

        protected static bool FactoryNum_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region RadionuclidComposition (8.4)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string RadionuclidComposition_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> RadionuclidComposition
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(RadionuclidComposition), out var value))
                {
                    ((RamAccess<string>)value).Value = RadionuclidComposition_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(RadionuclidComposition_Validation, RadionuclidComposition_DB);
                rm.PropertyChanged += RadionuclidComposition_ValueChanged;
                Dictionary.Add(nameof(RadionuclidComposition), rm);
                return (RamAccess<string>)Dictionary[nameof(RadionuclidComposition)];
            }
            set
            {
                RadionuclidComposition_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void RadionuclidComposition_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (RadionuclidComposition_DB != value1)
            {
                RadionuclidComposition_DB = value1;
            }
        }

        protected static bool RadionuclidComposition_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region ReleaseDate (8.5)

        public DateOnly? ReleaseDate_DB { get; set; }

        [NotMapped]
        public RamAccess<DateOnly?> ReleaseDate
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(ReleaseDate), out var value))
                {
                    ((RamAccess<DateOnly?>)value).Value = ReleaseDate_DB;
                    return (RamAccess<DateOnly?>)value;
                }
                var rm = new RamAccess<DateOnly?>(ReleaseDate_Validation, ReleaseDate_DB);
                rm.PropertyChanged += ReleaseDate_ValueChanged;
                Dictionary.Add(nameof(ReleaseDate), rm);
                return (RamAccess<DateOnly?>)Dictionary[nameof(ReleaseDate)];
            }
            set
            {
                ReleaseDate_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void ReleaseDate_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<DateOnly?>)value).Value;
            if (ReleaseDate_DB != value1)
            {
                ReleaseDate_DB = value1;
            }
        }

        protected static bool ReleaseDate_Validation(RamAccess<DateOnly?> value)//Ready
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

        #region ActivityOnRealeseDate (8.6)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string ActivityOnRealeseDate_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> ActivityOnRealeseDate
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(ActivityOnRealeseDate), out var value))
                {
                    ((RamAccess<string>)value).Value = ActivityOnRealeseDate_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(ActivityOnRealeseDate_Validation, ActivityOnRealeseDate_DB);
                rm.PropertyChanged += ActivityOnRealeseDate_ValueChanged;
                Dictionary.Add(nameof(ActivityOnRealeseDate), rm);
                return (RamAccess<string>)Dictionary[nameof(ActivityOnRealeseDate)];
            }
            set
            {
                ActivityOnRealeseDate_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void ActivityOnRealeseDate_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (ActivityOnRealeseDate_DB != value1)
            {
                ActivityOnRealeseDate_DB = value1;
            }
        }

        protected static bool ActivityOnRealeseDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region NuclearMaterials (8.7)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string NuclearMaterials_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> NuclearMaterials
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(NuclearMaterials), out var value))
                {
                    ((RamAccess<string>)value).Value = NuclearMaterials_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(NuclearMaterials_Validation, NuclearMaterials_DB);
                rm.PropertyChanged += NuclearMaterials_ValueChanged;
                Dictionary.Add(nameof(NuclearMaterials), rm);
                return (RamAccess<string>)Dictionary[nameof(NuclearMaterials)];
            }
            set
            {
                NuclearMaterials_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void NuclearMaterials_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (NuclearMaterials_DB != value1)
            {
                NuclearMaterials_DB = value1;
            }
        }

        protected static bool NuclearMaterials_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region Category (8.8)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string Category_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> Category
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(Category), out var value))
                {
                    ((RamAccess<string>)value).Value = Category_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Category_Validation, Category_DB);
                rm.PropertyChanged += Category_ValueChanged;
                Dictionary.Add(nameof(Category), rm);
                return (RamAccess<string>)Dictionary[nameof(Category)];
            }
            set
            {
                Category_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void Category_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (Category_DB != value1)
            {
                Category_DB = value1;
            }
        }

        protected static bool Category_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region ManufacturerOksm (8.9)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string ManufacturerOksm_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> ManufacturerOksm
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(ManufacturerOksm), out var value))
                {
                    ((RamAccess<string>)value).Value = ManufacturerOksm_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(ManufacturerOksm_Validation, ManufacturerOksm_DB);
                rm.PropertyChanged += ManufacturerOksm_ValueChanged;
                Dictionary.Add(nameof(ManufacturerOksm), rm);
                return (RamAccess<string>)Dictionary[nameof(ManufacturerOksm)];
            }
            set
            {
                ManufacturerOksm_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void ManufacturerOksm_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (ManufacturerOksm_DB != value1)
            {
                ManufacturerOksm_DB = value1;
            }
        }

        protected static bool ManufacturerOksm_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region CertificateNum (8.10)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string CertificateNum_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> CertificateNum
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(CertificateNum), out var value))
                {
                    ((RamAccess<string>)value).Value = CertificateNum_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(CertificateNum_Validation, CertificateNum_DB);
                rm.PropertyChanged += CertificateNum_ValueChanged;
                Dictionary.Add(nameof(CertificateNum), rm);
                return (RamAccess<string>)Dictionary[nameof(CertificateNum)];
            }
            set
            {
                CertificateNum_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void CertificateNum_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (CertificateNum_DB != value1)
            {
                CertificateNum_DB = value1;
            }
        }

        protected static bool CertificateNum_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region CertificateExpirationDate (8.11)

        public DateOnly? CertificateExpirationDate_DB { get; set; }

        [NotMapped]
        public RamAccess<DateOnly?> CertificateExpirationDate
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(CertificateExpirationDate), out var value))
                {
                    ((RamAccess<DateOnly?>)value).Value = CertificateExpirationDate_DB;
                    return (RamAccess<DateOnly?>)value;
                }
                var rm = new RamAccess<DateOnly?>(CertificateExpirationDate_Validation, CertificateExpirationDate_DB);
                rm.PropertyChanged += CertificateExpirationDate_ValueChanged;
                Dictionary.Add(nameof(CertificateExpirationDate), rm);
                return (RamAccess<DateOnly?>)Dictionary[nameof(CertificateExpirationDate)];
            }
            set
            {
                CertificateExpirationDate_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void CertificateExpirationDate_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<DateOnly?>)value).Value;
            if (CertificateExpirationDate_DB != value1)
            {
                CertificateExpirationDate_DB = value1;
            }
        }

        protected static bool CertificateExpirationDate_Validation(RamAccess<DateOnly?> value)//Ready
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


        #region InheritedMethods
        public override string ConvertToTSVstring()
        {
            throw new NotImplementedException();
        }
        public override void ExcelGetRow(ExcelWorksheet worksheet, int row)
        {
            throw new NotImplementedException();
        }
        public override bool Object_Validation()
        {
            throw new NotImplementedException();
        }
        public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
        {
            throw new NotImplementedException();
        }
        public override bool IsContentEqual(Form other)
        {
            throw new NotImplementedException();
        }
        #endregion
    }


    [Serializable]
    [Table(name: "form_32_table_2")]
    public class ContainerInfo : Form
    {
        #region ForeignKey Form32Id
        public int Form32Id { get; set; }

        private Form32 _form32;

        [ForeignKey(nameof(Form32Id))]
        public Form32 Form32
        {
            get
            {
                return _form32;
            }
            private set
            {
                _form32 = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Name (9.1)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string Name_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> Name
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(Name), out var value))
                {
                    ((RamAccess<string>)value).Value = Name_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Name_Validation, Name_DB);
                rm.PropertyChanged += Name_ValueChanged;
                Dictionary.Add(nameof(Name), rm);
                return (RamAccess<string>)Dictionary[nameof(Name)];
            }
            set
            {
                Name_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void Name_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (Name_DB != value1)
            {
                Name_DB = value1;
            }
        }

        protected static bool Name_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region Type (9.2)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string Type_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> Type
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(Type), out var value))
                {
                    ((RamAccess<string>)value).Value = Type_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Type_Validation, Type_DB);
                rm.PropertyChanged += Type_ValueChanged;
                Dictionary.Add(nameof(Type), rm);
                return (RamAccess<string>)Dictionary[nameof(Type)];
            }
            set
            {
                Type_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void Type_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (Type_DB != value1)
            {
                Type_DB = value1;
            }
        }

        protected static bool Type_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region IdNum (9.3)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string IdNum_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> IdNum
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(IdNum), out var value))
                {
                    ((RamAccess<string>)value).Value = IdNum_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(IdNum_Validation, IdNum_DB);
                rm.PropertyChanged += IdNum_ValueChanged;
                Dictionary.Add(nameof(IdNum), rm);
                return (RamAccess<string>)Dictionary[nameof(IdNum)];
            }
            set
            {
                IdNum_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void IdNum_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (IdNum_DB != value1)
            {
                IdNum_DB = value1;
            }
        }

        protected static bool IdNum_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region ReleaseYear (9.4)
        public int? ReleaseYear_DB { get; set; } = 0;

        [NotMapped]
        public RamAccess<int?> ReleaseYear
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(ReleaseYear), out var value))
                {
                    ((RamAccess<int?>)value).Value = ReleaseYear_DB;
                    return (RamAccess<int?>)value;
                }
                var rm = new RamAccess<int?>(ReleaseYear_Validation, ReleaseYear_DB);
                rm.PropertyChanged += ReleaseYear_ValueChanged;
                Dictionary.Add(nameof(ReleaseYear), rm);
                return (RamAccess<int?>)Dictionary[nameof(ReleaseYear)];
            }
            set
            {
                ReleaseYear_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void ReleaseYear_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<int?>)value).Value;
            if (ReleaseYear_DB != value1)
            {
                ReleaseYear_DB = value1;
            }
        }

        private bool ReleaseYear_Validation(RamAccess<int?> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region DepletedUraniumMass (9.5)
        public double? DepletedUraniumMass_DB { get; set; } = 0;

        [NotMapped]
        public RamAccess<double?> DepletedUraniumMass
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(DepletedUraniumMass), out var value))
                {
                    ((RamAccess<double?>)value).Value = DepletedUraniumMass_DB;
                    return (RamAccess<double?>)value;
                }
                var rm = new RamAccess<double?>(DepletedUraniumMass_Validation, DepletedUraniumMass_DB);
                rm.PropertyChanged += DepletedUraniumMass_ValueChanged;
                Dictionary.Add(nameof(DepletedUraniumMass), rm);
                return (RamAccess<double?>)Dictionary[nameof(DepletedUraniumMass)];
            }
            set
            {
                DepletedUraniumMass_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void DepletedUraniumMass_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<double?>)value).Value;
            if (DepletedUraniumMass_DB != value1)
            {
                DepletedUraniumMass_DB = value1;
            }
        }

        private bool DepletedUraniumMass_Validation(RamAccess<double?> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion


        #region InheritedMethods
        public override string ConvertToTSVstring()
        {
            throw new NotImplementedException();
        }
        public override void ExcelGetRow(ExcelWorksheet worksheet, int row)
        {
            throw new NotImplementedException();
        }
        public override bool Object_Validation()
        {
            throw new NotImplementedException();
        }
        public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
        {
            throw new NotImplementedException();
        }
        public override bool IsContentEqual(Form other)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    [Serializable]
    [Table(name: "form_32_table_3")]
    public class Identificator : Form
    {
        #region ForeignKey Form32Id
        public int Form32Id { get; set; }

        private Form32 _form32;

        [ForeignKey(nameof(Form32Id))]
        public Form32 Form32
        {
            get
            {
                return _form32;
            }
            private set
            {
                _form32 = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region IdName (10.1)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string IdName_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> IdName
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(IdName), out var value))
                {
                    ((RamAccess<string>)value).Value = IdName_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(IdName_Validation, IdName_DB);
                rm.PropertyChanged += IdName_ValueChanged;
                Dictionary.Add(nameof(IdName), rm);
                return (RamAccess<string>)Dictionary[nameof(IdName)];
            }
            set
            {
                IdName_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void IdName_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (IdName_DB != value1)
            {
                IdName_DB = value1;
            }
        }

        protected static bool IdName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion

        #region IdValue (10.2)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string IdValue_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> IdValue
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(IdValue), out var value))
                {
                    ((RamAccess<string>)value).Value = IdValue_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(IdValue_Validation, IdValue_DB);
                rm.PropertyChanged += IdValue_ValueChanged;
                Dictionary.Add(nameof(IdValue), rm);
                return (RamAccess<string>)Dictionary[nameof(IdValue)];
            }
            set
            {
                IdValue_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void IdValue_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (IdValue_DB != value1)
            {
                IdValue_DB = value1;
            }
        }

        protected static bool IdValue_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        #endregion


        #region InheritedMethods
        public override string ConvertToTSVstring()
        {
            throw new NotImplementedException();
        }
        public override void ExcelGetRow(ExcelWorksheet worksheet, int row)
        {
            throw new NotImplementedException();
        }
        public override bool Object_Validation()
        {
            throw new NotImplementedException();
        }
        public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
        {
            throw new NotImplementedException();
        }
        public override bool IsContentEqual(Form other)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
