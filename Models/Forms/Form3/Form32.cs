using Models.Attributes;
using Models.Comparers.FormContent;
using Models.Forms.DataAccess;
using Models.Forms.Form1;
using Models.Interfaces;
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
    public class Form32 : Form, ICopiable
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

        #region IsRvProduction

        public bool IsRvProduction_DB { get; set; }

        [NotMapped]
        public RamAccess<bool> IsRvProduction
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(IsRvProduction), out RamAccess value))
                {
                    ((RamAccess<bool>)value).Value = IsRvProduction_DB;
                    return (RamAccess<bool>)value;
                }
                var rm = new RamAccess<bool>(IsRvProduction_Validation, IsRvProduction_DB);
                rm.PropertyChanged += IsRvProductionValueChanged;
                Dictionary.Add(nameof(IsRvProduction), rm);
                return (RamAccess<bool>)Dictionary[nameof(IsRvProduction)];
            }
            set
            {
                IsRvProduction_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void IsRvProductionValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            IsRvProduction_DB = ((RamAccess<bool>)value).Value;
        }

        private static bool IsRvProduction_Validation(RamAccess<bool> value)
        {
            return true;
        }

        #endregion

        #region IsRvTransportation

        public bool IsRvTransportation_DB { get; set; }

        [NotMapped]
        public RamAccess<bool> IsRvTransportation
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(IsRvTransportation), out RamAccess value))
                {
                    ((RamAccess<bool>)value).Value = IsRvTransportation_DB;
                    return (RamAccess<bool>)value;
                }
                var rm = new RamAccess<bool>(IsRvTransportation_Validation, IsRvTransportation_DB);
                rm.PropertyChanged += IsRvTransportationValueChanged;
                Dictionary.Add(nameof(IsRvTransportation), rm);
                return (RamAccess<bool>)Dictionary[nameof(IsRvTransportation)];
            }
            set
            {
                IsRvTransportation_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void IsRvTransportationValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            IsRvTransportation_DB = ((RamAccess<bool>)value).Value;
        }

        private static bool IsRvTransportation_Validation(RamAccess<bool> value)
        {
            return true;
        }

        #endregion

        #region IsRvExploitation

        public bool IsRvExploitation_DB { get; set; }

        [NotMapped]
        public RamAccess<bool> IsRvExploitation
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(IsRvExploitation), out RamAccess value))
                {
                    ((RamAccess<bool>)value).Value = IsRvExploitation_DB;
                    return (RamAccess<bool>)value;
                }
                var rm = new RamAccess<bool>(IsRvExploitation_Validation, IsRvExploitation_DB);
                rm.PropertyChanged += IsRvExploitationValueChanged;
                Dictionary.Add(nameof(IsRvExploitation), rm);
                return (RamAccess<bool>)Dictionary[nameof(IsRvExploitation)];
            }
            set
            {
                IsRvExploitation_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void IsRvExploitationValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            IsRvExploitation_DB = ((RamAccess<bool>)value).Value;
        }

        private static bool IsRvExploitation_Validation(RamAccess<bool> value)
        {
            return true;
        }

        #endregion

        #region IsRvStoring

        public bool IsRvStoring_DB { get; set; }

        [NotMapped]
        public RamAccess<bool> IsRvStoring
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(IsRvStoring), out RamAccess value))
                {
                    ((RamAccess<bool>)value).Value = IsRvStoring_DB;
                    return (RamAccess<bool>)value;
                }
                var rm = new RamAccess<bool>(IsRvStoring_Validation, IsRvStoring_DB);
                rm.PropertyChanged += IsRvStoringValueChanged;
                Dictionary.Add(nameof(IsRvStoring), rm);
                return (RamAccess<bool>)Dictionary[nameof(IsRvStoring)];
            }
            set
            {
                IsRvStoring_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void IsRvStoringValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            IsRvStoring_DB = ((RamAccess<bool>)value).Value;
        }

        private static bool IsRvStoring_Validation(RamAccess<bool> value)
        {
            return true;
        }

        #endregion

        #region IsRvRecycling

        public bool IsRvRecycling_DB { get; set; }

        [NotMapped]
        public RamAccess<bool> IsRvRecycling
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(IsRvRecycling), out RamAccess value))
                {
                    ((RamAccess<bool>)value).Value = IsRvRecycling_DB;
                    return (RamAccess<bool>)value;
                }
                var rm = new RamAccess<bool>(IsRvRecycling_Validation, IsRvRecycling_DB);
                rm.PropertyChanged += IsRvRecyclingValueChanged;
                Dictionary.Add(nameof(IsRvRecycling), rm);
                return (RamAccess<bool>)Dictionary[nameof(IsRvRecycling)];
            }
            set
            {
                IsRvRecycling_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void IsRvRecyclingValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            IsRvRecycling_DB = ((RamAccess<bool>)value).Value;
        }

        private static bool IsRvRecycling_Validation(RamAccess<bool> value)
        {
            return true;
        }

        #endregion

        #region IsRaoTransportation

        public bool IsRaoTransportation_DB { get; set; }

        [NotMapped]
        public RamAccess<bool> IsRaoTransportation
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(IsRaoTransportation), out RamAccess value))
                {
                    ((RamAccess<bool>)value).Value = IsRaoTransportation_DB;
                    return (RamAccess<bool>)value;
                }
                var rm = new RamAccess<bool>(IsRaoTransportation_Validation, IsRaoTransportation_DB);
                rm.PropertyChanged += IsRaoTransportationValueChanged;
                Dictionary.Add(nameof(IsRaoTransportation), rm);
                return (RamAccess<bool>)Dictionary[nameof(IsRaoTransportation)];
            }
            set
            {
                IsRaoTransportation_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void IsRaoTransportationValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            IsRaoTransportation_DB = ((RamAccess<bool>)value).Value;
        }

        private static bool IsRaoTransportation_Validation(RamAccess<bool> value)
        {
            return true;
        }

        #endregion

        #region IsRaoStoring

        public bool IsRaoStoring_DB { get; set; }

        [NotMapped]
        public RamAccess<bool> IsRaoStoring
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(IsRaoStoring), out RamAccess value))
                {
                    ((RamAccess<bool>)value).Value = IsRaoStoring_DB;
                    return (RamAccess<bool>)value;
                }
                var rm = new RamAccess<bool>(IsRaoStoring_Validation, IsRaoStoring_DB);
                rm.PropertyChanged += IsRaoStoringValueChanged;
                Dictionary.Add(nameof(IsRaoStoring), rm);
                return (RamAccess<bool>)Dictionary[nameof(IsRaoStoring)];
            }
            set
            {
                IsRaoStoring_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void IsRaoStoringValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            IsRaoStoring_DB = ((RamAccess<bool>)value).Value;
        }

        private static bool IsRaoStoring_Validation(RamAccess<bool> value)
        {
            return true;
        }

        #endregion

        #region IsRaoRecycling

        public bool IsRaoRecycling_DB { get; set; }

        [NotMapped]
        public RamAccess<bool> IsRaoRecycling
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(IsRaoRecycling), out RamAccess value))
                {
                    ((RamAccess<bool>)value).Value = IsRaoRecycling_DB;
                    return (RamAccess<bool>)value;
                }
                var rm = new RamAccess<bool>(IsRaoRecycling_Validation, IsRaoRecycling_DB);
                rm.PropertyChanged += IsRaoRecyclingValueChanged;
                Dictionary.Add(nameof(IsRaoRecycling), rm);
                return (RamAccess<bool>)Dictionary[nameof(IsRaoRecycling)];
            }
            set
            {
                IsRaoRecycling_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void IsRaoRecyclingValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            IsRaoRecycling_DB = ((RamAccess<bool>)value).Value;
        }

        private static bool IsRaoRecycling_Validation(RamAccess<bool> value)
        {
            return true;
        }

        #endregion
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

        public ObservableCollection<Form32ExportedZriInfo> ExportedZriInfoCollection { get; set; }
        public ObservableCollection<Form32ContainerInfo> ContainersInfoCollection { get; set; }
        public ObservableCollection<Form32Identificator> IdentificatorsCollection { get; set; }
        #endregion
        
        #region CleanIds
        public void CleanIds()
        {
            Id = 0;
            foreach (var item in ExportedZriInfoCollection)
            {
                item.Id = 0;
            }
            foreach (var item in ContainersInfoCollection)
            {
                item.Id = 0;
            }
            foreach (var item in IdentificatorsCollection)
            {
                item.Id = 0;
            }
        }
        #endregion

        #region ICopiable
        #region ConvertToTSVstring

        /// <summary>
        /// </summary>
        /// <returns>Возвращает строку с записанными данными в формате TSV(Tab-Separated Values) </returns>
        public string ConvertToTSVstring()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region PasteParsedTSVstring
        public void PasteParsedTSVstring(string[] parsedTSVstring)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        #region InheritedMethods
        public override void ExcelGetRow(ExcelWorksheet worksheet, int row)
        {
            throw new NotImplementedException();
        }
        public override bool Object_Validation()
        {
            throw new NotImplementedException();
        }

        //Третьи формы представляют собой не таблицу, а единичный набор данных с другими таблицами, поэтому функция ExcelRow перезаписана не по шаблону
        public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
        {
            worksheet.Cells["C12"].Value = AgreementIdNum_DB;
            worksheet.Cells["C13"].Value = DeliveryDay_DB;

            worksheet.Cells["C15"].Value = RecipientName_DB;

            worksheet.Cells["C17"].Value = IsRvProduction_DB ? "*": null;
            worksheet.Cells["C18"].Value = IsRvTransportation_DB ? "*" : null;
            worksheet.Cells["C19"].Value = IsRvExploitation_DB ? "*" : null;
            worksheet.Cells["C20"].Value = IsRvStoring_DB ? "*" : null;
            worksheet.Cells["C21"].Value = IsRvRecycling_DB ? "*" : null;

            worksheet.Cells["C23"].Value = IsRaoTransportation_DB ? "*" : null;
            worksheet.Cells["C24"].Value = IsRaoStoring_DB ? "*" : null;
            worksheet.Cells["C25"].Value = IsRaoRecycling_DB ? "*" : null;

            worksheet.Cells["C27"].Value = LicenseNumRv_DB;
            worksheet.Cells["C28"].Value = LicenseNumRao_DB;

            worksheet.Cells["C30"].Value = LicenseExpirationDateRv_DB;
            worksheet.Cells["C31"].Value = LicenseExpirationDateRao_DB;
            worksheet.Cells["C32"].Value = DeliveryAddress_DB;
            worksheet.Cells["C33"].Value = RadionuclidCompositionZri_DB;
            worksheet.Cells["C34"].Value = TotalActivity_DB;
            worksheet.Cells["C35"].Value = TotalCount_DB;

            #region Идентификаторы (10)
            int start = 47;
            worksheet.InsertRow(start + 1, 1, start);
            worksheet.DeleteRow(start);
            for (int i = 0; i < IdentificatorsCollection.Count; i++)
            {
                var info = IdentificatorsCollection[i];
                worksheet.Cells[$"A{start + i}"].Value = $"10.{i + 1}";
                worksheet.Cells[$"B{start + i}"].Value = info.IdName;
                worksheet.Cells[$"C{start + i}"].Value = info.IdValue;

                if (i + 1 < IdentificatorsCollection.Count)
                    worksheet.InsertRow(start + i + 1, 1, start);
            }
            #endregion

            #region Сведения о контейнере, приборе, установке (9)
            start = 43;
            worksheet.InsertRow(start + 1, 1, start);
            worksheet.DeleteRow(start);
            for (int i = 0; i < ContainersInfoCollection.Count; i++)
            {
                var info = ContainersInfoCollection[i];
                worksheet.Cells[$"A{start + i}"].Value = $"9.{i + 1}";
                worksheet.Cells[$"B{start + i}"].Value = info.Name;
                worksheet.Cells[$"C{start + i}"].Value = info.Type;
                worksheet.Cells[$"D{start + i}"].Value = info.IdNum;
                worksheet.Cells[$"E{start + i}"].Value = info.ReleaseYear;
                worksheet.Cells[$"F{start + i}"].Value = info.DepletedUraniumMass;

                if (i + 1 < ContainersInfoCollection.Count)
                    worksheet.InsertRow(start + i + 1, 1, start);
            }
            #endregion

            #region Сведения о постовляемых ЗРИ (8)
            start = 39;
            worksheet.InsertRow(start + 1, 1, start);
            worksheet.DeleteRow(start);

            for (int i = 0; i < ExportedZriInfoCollection.Count; i++)
            {
                var info = ExportedZriInfoCollection[i];
                worksheet.Cells[$"A{start + i}"].Value = $"8.{i + 1}";
                worksheet.Cells[$"B{start + i}"].Value = info.PassportNum;
                worksheet.Cells[$"C{start + i}"].Value = info.Type;
                worksheet.Cells[$"D{start + i}"].Value = info.FactoryNum;
                worksheet.Cells[$"E{start + i}"].Value = info.RadionuclidComposition;
                worksheet.Cells[$"F{start + i}"].Value = info.ReleaseDate;
                worksheet.Cells[$"G{start + i}"].Value = info.ActivityOnRealeseDate;
                worksheet.Cells[$"H{start + i}"].Value = info.NuclearMaterials;
                worksheet.Cells[$"I{start + i}"].Value = info.Category;
                worksheet.Cells[$"J{start + i}"].Value = info.ManufacturerOksm;
                worksheet.Cells[$"K{start + i}"].Value = info.CertificateNum;
                worksheet.Cells[$"L{start + i}"].Value = info.CertificateExpirationDate;

                if (i + 1 < ExportedZriInfoCollection.Count)
                    worksheet.InsertRow(start + i + 1, 1, start);
            }
            #endregion

            return 0; //Рудимент
        }
        public override bool IsContentEqual(Form otherForm)
        {
            if (otherForm is not Form32 formToCompare) return false;

            //Сравниваю количество элементов в коллекциях
            if (ExportedZriInfoCollection.Count() != formToCompare.ExportedZriInfoCollection.Count()
                && ContainersInfoCollection.Count() != formToCompare.ContainersInfoCollection.Count()
                && IdentificatorsCollection.Count() != formToCompare.IdentificatorsCollection.Count())
                return false;

            //Поштучно сравниваю содержимое коллекции ExportedZriInfoCollection
            for (int i = 0; i < ExportedZriInfoCollection.Count(); i++)
            {
                if (!ExportedZriInfoCollection[i]
                    .IsContentEqual(formToCompare.ExportedZriInfoCollection[i]))
                {
                    return false;
                }
            }
            //Поштучно сравниваю содержимое коллекции ContainersInfoCollection
            for (int i = 0; i < ContainersInfoCollection.Count(); i++)
            {
                if (!ContainersInfoCollection[i]
                    .IsContentEqual(formToCompare.ContainersInfoCollection[i]))
                {
                    return false;
                }
            }
            //Поштучно сравниваю содержимое коллекции IdentificatorsCollection
            for (int i = 0; i < IdentificatorsCollection.Count(); i++)
            {
                if (!IdentificatorsCollection[i]
                    .IsContentEqual(formToCompare.IdentificatorsCollection[i]))
                {
                    return false;
                }
            }

            return FormTextEquality.Equals(AgreementIdNum_DB, formToCompare.AgreementIdNum_DB)
                   && DeliveryDay_DB == formToCompare.DeliveryDay_DB
                   && FormTextEquality.Equals(RecipientName_DB, formToCompare.RecipientName_DB)
                   && IsRvProduction_DB== formToCompare.IsRvProduction_DB
                   && IsRvTransportation_DB == formToCompare.IsRvTransportation_DB
                   && IsRvExploitation_DB == formToCompare.IsRvExploitation_DB
                   && IsRvStoring_DB == formToCompare.IsRvStoring_DB
                   && IsRvRecycling_DB == formToCompare.IsRvRecycling_DB
                   && IsRaoTransportation_DB == formToCompare.IsRaoTransportation_DB
                   && IsRaoStoring_DB == formToCompare.IsRaoStoring_DB
                   && IsRaoRecycling_DB == formToCompare.IsRaoRecycling_DB
                   && FormTextEquality.Equals(LicenseNumRv_DB, formToCompare.LicenseNumRv_DB)
                   && FormTextEquality.Equals(LicenseNumRao_DB, formToCompare.LicenseNumRao_DB)
                   && LicenseExpirationDateRv_DB == formToCompare.LicenseExpirationDateRv_DB
                   && LicenseExpirationDateRao_DB == formToCompare.LicenseExpirationDateRao_DB
                   && FormTextEquality.Equals(DeliveryAddress_DB, formToCompare.DeliveryAddress_DB)
                   && FormTextEquality.Equals(RadionuclidCompositionZri_DB, formToCompare.RadionuclidCompositionZri_DB)
                   && FormDoubleEquality.Equals(TotalActivity_DB, formToCompare.TotalActivity_DB)
                   && TotalCount_DB == formToCompare.TotalCount_DB;
        }
        #endregion
    }
}
