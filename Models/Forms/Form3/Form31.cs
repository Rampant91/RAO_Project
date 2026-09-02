using Models.Attributes;
using Models.Collections;
using Models.Comparers.FormContent;
using Models.Forms.DataAccess;
using Models.Forms.Form1;
using Models.Interfaces;
using Models.Passports;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Models.Forms.Form3
{
    [Serializable]
    [Form_Class("Форма 3.1")]
    [Table(name: "form_31")]
    public class Form31 : Form
    {
        #region Constructor
        public Form31()
        {
            ExportedZriOziiiInfoCollection = new();
        }
        #endregion

        #region Properties

        #region RecipientName (1.1)

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

        #region RecipientJurLicoAddress (1.2)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string RecipientJurLicoAddress_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> RecipientJurLicoAddress
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(RecipientJurLicoAddress), out var value))
                {
                    ((RamAccess<string>)value).Value = RecipientJurLicoAddress_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(RecipientJurLicoAddress_Validation, RecipientJurLicoAddress_DB);
                rm.PropertyChanged += RecipientJurLicoAddress_ValueChanged;
                Dictionary.Add(nameof(RecipientJurLicoAddress), rm);
                return (RamAccess<string>)Dictionary[nameof(RecipientJurLicoAddress)];
            }
            set
            {
                RecipientJurLicoAddress_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void RecipientJurLicoAddress_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (RecipientJurLicoAddress_DB != value1)
            {
                RecipientJurLicoAddress_DB = value1;
            }
        }

        protected static bool RecipientJurLicoAddress_Validation(RamAccess<string> value)//Ready
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

        #region RecipientWorkplaceAddress (1.3)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string RecipientWorkplaceAddress_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> RecipientWorkplaceAddress
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(RecipientWorkplaceAddress), out var value))
                {
                    ((RamAccess<string>)value).Value = RecipientWorkplaceAddress_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(RecipientWorkplaceAddress_Validation, RecipientWorkplaceAddress_DB);
                rm.PropertyChanged += RecipientWorkplaceAddress_ValueChanged;
                Dictionary.Add(nameof(RecipientWorkplaceAddress), rm);
                return (RamAccess<string>)Dictionary[nameof(RecipientWorkplaceAddress)];
            }
            set
            {
                RecipientWorkplaceAddress_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void RecipientWorkplaceAddress_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (RecipientWorkplaceAddress_DB != value1)
            {
                RecipientWorkplaceAddress_DB = value1;
            }
        }

        protected static bool RecipientWorkplaceAddress_Validation(RamAccess<string> value)//Ready
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

        #region LicenseNum (2)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string LicenseNum_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> LicenseNum
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(LicenseNum), out var value))
                {
                    ((RamAccess<string>)value).Value = LicenseNum_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(LicenseNum_Validation, LicenseNum_DB);
                rm.PropertyChanged += LicenseNum_ValueChanged;
                Dictionary.Add(nameof(LicenseNum), rm);
                return (RamAccess<string>)Dictionary[nameof(LicenseNum)];
            }
            set
            {
                LicenseNum_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void LicenseNum_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (LicenseNum_DB != value1)
            {
                LicenseNum_DB = value1;
            }
        }

        protected static bool LicenseNum_Validation(RamAccess<string> value)//Ready
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

        #region ValidityPeriod (3)

        public DateOnly? ValidityPeriod_DB { get; set; } 

        [NotMapped]
        public RamAccess<DateOnly?> ValidityPeriod
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(ValidityPeriod), out var value))
                {
                    ((RamAccess<DateOnly?>)value).Value = ValidityPeriod_DB;
                    return (RamAccess<DateOnly?>)value;
                }
                var rm = new RamAccess<DateOnly?>(ValidityPeriod_Validation, ValidityPeriod_DB);
                rm.PropertyChanged += ValidityPeriod_ValueChanged;
                Dictionary.Add(nameof(ValidityPeriod), rm);
                return (RamAccess<DateOnly?>)Dictionary[nameof(ValidityPeriod)];
            }
            set
            {
                ValidityPeriod_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void ValidityPeriod_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<DateOnly?>)value).Value;
            if (ValidityPeriod_DB != value1)
            {
                ValidityPeriod_DB = value1;
            }
        }

        protected static bool ValidityPeriod_Validation(RamAccess<DateOnly?> value)//Ready
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

        #region ExpectedDecisionTimeframe (4)

        public DateOnly? ExpectedDecisionTimeframe_DB { get; set; }

        [NotMapped]
        public RamAccess<DateOnly?> ExpectedDecisionTimeframe
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(ExpectedDecisionTimeframe), out var value))
                {
                    ((RamAccess<DateOnly?>)value).Value = ExpectedDecisionTimeframe_DB;
                    return (RamAccess<DateOnly?>)value;
                }
                var rm = new RamAccess<DateOnly?>(ExpectedDecisionTimeframe_Validation, ExpectedDecisionTimeframe_DB);
                rm.PropertyChanged += ExpectedDecisionTimeframe_ValueChanged;
                Dictionary.Add(nameof(ExpectedDecisionTimeframe), rm);
                return (RamAccess<DateOnly?>)Dictionary[nameof(ExpectedDecisionTimeframe)];
            }
            set
            {
                ExpectedDecisionTimeframe_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void ExpectedDecisionTimeframe_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<DateOnly?>)value).Value;
            if (ExpectedDecisionTimeframe_DB != value1)
            {
                ExpectedDecisionTimeframe_DB = value1;
            }
        }

        protected static bool ExpectedDecisionTimeframe_Validation(RamAccess<DateOnly?> value)//Ready
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

        #region FinalUserName (5.1)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string FinalUserName_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> FinalUserName
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(FinalUserName), out var value))
                {
                    ((RamAccess<string>)value).Value = FinalUserName_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(FinalUserName_Validation, FinalUserName_DB);
                rm.PropertyChanged += FinalUserName_ValueChanged;
                Dictionary.Add(nameof(FinalUserName), rm);
                return (RamAccess<string>)Dictionary[nameof(FinalUserName)];
            }
            set
            {
                FinalUserName_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void FinalUserName_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (FinalUserName_DB != value1)
            {
                FinalUserName_DB = value1;
            }
        }

        protected static bool FinalUserName_Validation(RamAccess<string> value)//Ready
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

        #region FinalUserJurLicoAddress (5.2)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string FinalUserJurLicoAddress_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> FinalUserJurLicoAddress
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(FinalUserJurLicoAddress), out var value))
                {
                    ((RamAccess<string>)value).Value = FinalUserJurLicoAddress_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(FinalUserJurLicoAddress_Validation, FinalUserJurLicoAddress_DB);
                rm.PropertyChanged += FinalUserJurLicoAddress_ValueChanged;
                Dictionary.Add(nameof(FinalUserJurLicoAddress), rm);
                return (RamAccess<string>)Dictionary[nameof(FinalUserJurLicoAddress)];
            }
            set
            {
                FinalUserJurLicoAddress_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void FinalUserJurLicoAddress_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (FinalUserJurLicoAddress_DB != value1)
            {
                FinalUserJurLicoAddress_DB = value1;
            }
        }

        protected static bool FinalUserJurLicoAddress_Validation(RamAccess<string> value)//Ready
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

        #region FinalUserWorkplaceAddress (5.3)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string FinalUserWorkplaceAddress_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> FinalUserWorkplaceAddress
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(FinalUserWorkplaceAddress), out var value))
                {
                    ((RamAccess<string>)value).Value = FinalUserWorkplaceAddress_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(FinalUserWorkplaceAddress_Validation, FinalUserWorkplaceAddress_DB);
                rm.PropertyChanged += FinalUserWorkplaceAddress_ValueChanged;
                Dictionary.Add(nameof(FinalUserWorkplaceAddress), rm);
                return (RamAccess<string>)Dictionary[nameof(FinalUserWorkplaceAddress)];
            }
            set
            {
                FinalUserWorkplaceAddress_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void FinalUserWorkplaceAddress_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (FinalUserWorkplaceAddress_DB != value1)
            {
                FinalUserWorkplaceAddress_DB = value1;
            }
        }

        protected static bool FinalUserWorkplaceAddress_Validation(RamAccess<string> value)//Ready
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

        #region FinalUserTelephone (5.4)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string FinalUserTelephone_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> FinalUserTelephone
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(FinalUserTelephone), out var value))
                {
                    ((RamAccess<string>)value).Value = FinalUserTelephone_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(FinalUserTelephone_Validation, FinalUserTelephone_DB);
                rm.PropertyChanged += FinalUserTelephone_ValueChanged;
                Dictionary.Add(nameof(FinalUserTelephone), rm);
                return (RamAccess<string>)Dictionary[nameof(FinalUserTelephone)];
            }
            set
            {
                FinalUserTelephone_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void FinalUserTelephone_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (FinalUserTelephone_DB != value1)
            {
                FinalUserTelephone_DB = value1;
            }
        }

        protected static bool FinalUserTelephone_Validation(RamAccess<string> value)//Ready
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

        #region FinalUserEmail (5.5)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string FinalUserEmail_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> FinalUserEmail
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(FinalUserEmail), out var value))
                {
                    ((RamAccess<string>)value).Value = FinalUserEmail_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(FinalUserEmail_Validation, FinalUserEmail_DB);
                rm.PropertyChanged += FinalUserEmail_ValueChanged;
                Dictionary.Add(nameof(FinalUserEmail), rm);
                return (RamAccess<string>)Dictionary[nameof(FinalUserEmail)];
            }
            set
            {
                FinalUserEmail_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void FinalUserEmail_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (FinalUserEmail_DB != value1)
            {
                FinalUserEmail_DB = value1;
            }
        }

        protected static bool FinalUserEmail_Validation(RamAccess<string> value)//Ready
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

        #region ApplicationScope (6)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string ApplicationScope_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> ApplicationScope
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(ApplicationScope), out var value))
                {
                    ((RamAccess<string>)value).Value = ApplicationScope_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(ApplicationScope_Validation, ApplicationScope_DB);
                rm.PropertyChanged += ApplicationScope_ValueChanged;
                Dictionary.Add(nameof(ApplicationScope), rm);
                return (RamAccess<string>)Dictionary[nameof(ApplicationScope)];
            }
            set
            {
                ApplicationScope_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void ApplicationScope_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (ApplicationScope_DB != value1)
            {
                ApplicationScope_DB = value1;
            }
        }

        protected static bool ApplicationScope_Validation(RamAccess<string> value)//Ready
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

        #region ContractNum (7.1)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string ContractNum_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> ContractNum
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(ContractNum), out var value))
                {
                    ((RamAccess<string>)value).Value = ContractNum_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(ContractNum_Validation, ContractNum_DB);
                rm.PropertyChanged += ContractNum_ValueChanged;
                Dictionary.Add(nameof(ContractNum), rm);
                return (RamAccess<string>)Dictionary[nameof(ContractNum)];
            }
            set
            {
                ContractNum_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void ContractNum_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
            if (ContractNum_DB != value1)
            {
                ContractNum_DB = value1;
            }
        }

        protected static bool ContractNum_Validation(RamAccess<string> value)//Ready
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

        #region ContractDate (7.2)

        public DateOnly? ContractDate_DB { get; set; }

        [NotMapped]
        public RamAccess<DateOnly?> ContractDate
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(ContractDate), out var value))
                {
                    ((RamAccess<DateOnly?>)value).Value = ContractDate_DB;
                    return (RamAccess<DateOnly?>)value;
                }
                var rm = new RamAccess<DateOnly?>(ContractDate_Validation, ContractDate_DB);
                rm.PropertyChanged += ContractDate_ValueChanged;
                Dictionary.Add(nameof(ContractDate), rm);
                return (RamAccess<DateOnly?>)Dictionary[nameof(ContractDate)];
            }
            set
            {
                ContractDate_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void ContractDate_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<DateOnly?>)value).Value;
            if (ContractDate_DB != value1)
            {
                ContractDate_DB = value1;
            }
        }

        protected static bool ContractDate_Validation(RamAccess<DateOnly?> value)//Ready
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

        #region ManufacturerOksm (7.3)

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

        #region Характеристика экспортируемых ЗРИ/ОЗИИИ (8)
        public ObservableCollection<Form31ExportedZriOziiiInfo> ExportedZriOziiiInfoCollection { get; set; } = new();
        #endregion
        #endregion

        #region CleanIds
        public void CleanIds()
        {
            Id = 0;
            foreach (var item in ExportedZriOziiiInfoCollection)
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

        #region IExcel

        public override void ExcelGetRow(ExcelWorksheet worksheet, int row)
        {
            throw new NotImplementedException();
        }

        //Третьи формы представляют собой не таблицу, а единичный набор данных с другими таблицами, поэтому функция ExcelRow перезаписана не по шаблону
        public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
        {
            worksheet.Cells["C12"].Value = RecipientName_DB;
            worksheet.Cells["C13"].Value = RecipientJurLicoAddress_DB;
            worksheet.Cells["C14"].Value = RecipientWorkplaceAddress_DB;
            worksheet.Cells["C15"].Value = LicenseNum_DB;
            worksheet.Cells["C16"].Value = ValidityPeriod_DB;
            worksheet.Cells["C17"].Value = ExpectedDecisionTimeframe_DB;

            worksheet.Cells["C19"].Value = FinalUserName_DB;
            worksheet.Cells["C20"].Value = FinalUserJurLicoAddress_DB;
            worksheet.Cells["C21"].Value = FinalUserWorkplaceAddress_DB;
            worksheet.Cells["C22"].Value = FinalUserTelephone_DB;
            worksheet.Cells["C23"].Value = FinalUserEmail_DB;
            worksheet.Cells["C24"].Value = ApplicationScope_DB;
            
            worksheet.Cells["C26"].Value = ContractNum_DB;
            worksheet.Cells["C27"].Value = ContractDate_DB;
            worksheet.Cells["C28"].Value = ManufacturerOksm_DB;


            int start = 31;
            worksheet.InsertRow(start + 1, 1, start);
            worksheet.DeleteRow(start);

            for (int i =0; i< ExportedZriOziiiInfoCollection.Count; i++)
            {
                var info = ExportedZriOziiiInfoCollection[i];
                worksheet.Cells[$"A{start + i}"].Value = $"8.{i + 1}";
                worksheet.Cells[$"B{start + i}"].Value = info.RadionuclidComposition;
                worksheet.Cells[$"C{start + i}"].Value = info.Count;
                worksheet.Cells[$"D{start + i}"].Value = info.TotalActivity;

                if (i + 1  < ExportedZriOziiiInfoCollection.Count)
                worksheet.InsertRow(start + i + 1, 1, start);
            }

            return 0; //Рудимент
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string id = "")
        {
            throw new NotImplementedException();
        }

        #endregion

        #region InheritedMethods

        public override bool Object_Validation()
        {
            throw new NotImplementedException();
        }
        public override bool IsContentEqual(Form otherForm)
        {
            if (otherForm is not Form31 formToCompare) return false;

            //Сравниваю количество элементов в коллекции
            if (ExportedZriOziiiInfoCollection.Count() != formToCompare.ExportedZriOziiiInfoCollection.Count())
                return false;

            //Поштучно сравниваю содержимое коллекции
            for (int i = 0; i< ExportedZriOziiiInfoCollection.Count(); i++)
            {
                if (!ExportedZriOziiiInfoCollection[i]
                    .IsContentEqual(formToCompare.ExportedZriOziiiInfoCollection[i]))
                {
                    return false;
                }
            }

            //Сравниваю содержимое формы 3.1
            return FormTextEquality.Equals(RecipientName_DB, formToCompare.RecipientName_DB)
                   && FormTextEquality.Equals(RecipientJurLicoAddress_DB, formToCompare.RecipientJurLicoAddress_DB)
                   && FormTextEquality.Equals(RecipientWorkplaceAddress_DB, formToCompare.RecipientWorkplaceAddress_DB)
                   && FormTextEquality.Equals(LicenseNum_DB, formToCompare.LicenseNum_DB)
                   && ValidityPeriod_DB == formToCompare.ValidityPeriod_DB
                   && ExpectedDecisionTimeframe_DB == formToCompare.ExpectedDecisionTimeframe_DB
                   && FormTextEquality.Equals(FinalUserName_DB, formToCompare.FinalUserName_DB)
                   && FormTextEquality.Equals(FinalUserJurLicoAddress_DB, formToCompare.FinalUserJurLicoAddress_DB)
                   && FormTextEquality.Equals(FinalUserWorkplaceAddress_DB, formToCompare.FinalUserWorkplaceAddress_DB)
                   && FormTextEquality.Equals(FinalUserTelephone_DB, formToCompare.FinalUserTelephone_DB)
                   && FormTextEquality.Equals(FinalUserEmail_DB, formToCompare.FinalUserEmail_DB)
                   && FormTextEquality.Equals(ApplicationScope_DB, formToCompare.ApplicationScope_DB)
                   && FormTextEquality.Equals(ContractNum_DB, formToCompare.ContractNum_DB)
                   && ContractDate_DB == formToCompare.ContractDate_DB
                   && FormTextEquality.Equals(ManufacturerOksm_DB, formToCompare.ManufacturerOksm_DB);
        }
        #endregion

    }

    
}
