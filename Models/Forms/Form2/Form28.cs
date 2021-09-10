using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Linq;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using Spravochniki;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.8: Отведение сточных вод, содержащих радионуклиды")]
    public class Form28 : Abstracts.Form2
    {
        public Form28() : base()
        {
            FormNum.Value = "2.8";
            //NumberOfFields.Value = 24;
            Validate_all();
        }

        private void Validate_all()
        {
            PermissionNumber2_Validation(PermissionNumber2);
            PermissionIssueDate2_Validation(PermissionIssueDate2);
            ValidBegin2_Validation(ValidBegin2);
            ValidThru2_Validation(ValidThru2);
            PermissionDocumentName2_Validation(PermissionDocumentName2);
            PermissionNumber_Validation(PermissionNumber);
            PermissionIssueDate_Validation(PermissionIssueDate);
            ValidBegin_Validation(ValidBegin);
            ValidThru_Validation(ValidThru);
            PermissionDocumentName_Validation(PermissionDocumentName);
            PermissionNumber1_Validation(PermissionNumber1);
            PermissionIssueDate1_Validation(PermissionIssueDate1);
            ValidBegin1_Validation(ValidBegin1);
            ValidThru1_Validation(ValidThru1);
            PermissionDocumentName1_Validation(PermissionDocumentName1);
            WasteSourceName_Validation(WasteSourceName);
            WasteRecieverName_Validation(WasteRecieverName);
            RecieverTypeCode_Validation(RecieverTypeCode);
            AllowedWasteRemovalVolume_Validation(AllowedWasteRemovalVolume);
            RemovedWasteVolume_Validation(RemovedWasteVolume);
            PoolDistrictName_Validation(PoolDistrictName);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return !(PermissionNumber2.HasErrors||
            PermissionIssueDate2.HasErrors||
            ValidBegin2.HasErrors||
            ValidThru2.HasErrors||
            PermissionDocumentName2.HasErrors||
            PermissionNumber.HasErrors||
            PermissionIssueDate.HasErrors||
            ValidBegin.HasErrors||
            ValidThru.HasErrors||
            PermissionDocumentName.HasErrors||
            PermissionNumber1.HasErrors||
            PermissionIssueDate1.HasErrors||
            ValidBegin1.HasErrors||
            ValidThru1.HasErrors||
            PermissionDocumentName1.HasErrors||
            WasteSourceName.HasErrors||
            WasteRecieverName.HasErrors||
            RecieverTypeCode.HasErrors||
            AllowedWasteRemovalVolume.HasErrors||
            RemovedWasteVolume.HasErrors||
            PoolDistrictName.HasErrors);
        }

        //PermissionNumber property
        #region PermissionNumber 
        public string PermissionNumber_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Номер разрешительного документа")]
        public RamAccess<string> PermissionNumber
        {
            get
            {
                    var tmp = new RamAccess<string>(PermissionNumber_Validation, PermissionNumber_DB);
                    tmp.PropertyChanged += PermissionNumberValueChanged;
                    return tmp;
            }
            set
            {
                    PermissionNumber_DB = value.Value;
                OnPropertyChanged(nameof(PermissionNumber));
            }
        }

        private void PermissionNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionNumber_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PermissionNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }
        //PermissionNumber property
        #endregion

        //PermissionIssueDate property
        #region PermissionIssueDate
        public string PermissionIssueDate_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public RamAccess<string> PermissionIssueDate
        {
            get
            {
                    var tmp = new RamAccess<string>(PermissionIssueDate_Validation, PermissionIssueDate_DB);
                    tmp.PropertyChanged += PermissionIssueDateValueChanged;
                    return tmp;
            }
            set
            {
                    PermissionIssueDate_DB = value.Value;
                OnPropertyChanged(nameof(PermissionIssueDate));
            }
        }

        private void PermissionIssueDateValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionIssueDate_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PermissionIssueDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionIssueDate property
        #endregion

        //PermissionDocumentName property
        #region PermissionDocumentName 
        public string PermissionDocumentName_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Наименование разрешительного документа")]
        public RamAccess<string> PermissionDocumentName
        {
            get
            {
                    var tmp = new RamAccess<string>(PermissionDocumentName_Validation, PermissionDocumentName_DB);
                    tmp.PropertyChanged += PermissionDocumentNameValueChanged;
                    return tmp;
            }
            set
            {
                    PermissionDocumentName_DB = value.Value;
                OnPropertyChanged(nameof(PermissionDocumentName));
            }
        }
        private void PermissionDocumentNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionDocumentName_DB = ((RamAccess<string>)Value).Value;
            }
        }

        private bool PermissionDocumentName_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionDocumentName property
        #endregion

        //ValidBegin property
        #region ValidBegin
        public string ValidBegin_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Действует с")]
        public RamAccess<string> ValidBegin
        {
            get
            {
                    var tmp = new RamAccess<string>(ValidBegin_Validation, ValidBegin_DB);
                    tmp.PropertyChanged += ValidBeginValueChanged;
                    return tmp;
            }
            set
            {
                    ValidBegin_DB = value.Value;
                OnPropertyChanged(nameof(ValidBegin));
            }
        }

        private void ValidBeginValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ValidBegin_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool ValidBegin_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //ValidBegin property
        #endregion

        //ValidThru property
        #region ValidThru
        public string ValidThru_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Действует по")]
        public RamAccess<string> ValidThru
        {
            get
            {
                    var tmp = new RamAccess<string>(ValidThru_Validation, ValidThru_DB);
                    tmp.PropertyChanged += ValidThruValueChanged;
                    return tmp;
            }
            set
            {
                    ValidThru_DB = value.Value;
                OnPropertyChanged(nameof(ValidThru));
            }
        }

        private void ValidThruValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ValidThru_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool ValidThru_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //ValidThru property
        #endregion

        //PermissionNumber1 property
        #region PermissionNumber1 
        public string PermissionNumber1_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Номер разрешительного документа")]
        public RamAccess<string> PermissionNumber1
        {
            get
            {
                var tmp = new RamAccess<string>(PermissionNumber1_Validation, PermissionNumber1_DB);
                tmp.PropertyChanged += PermissionNumber1ValueChanged;
                return tmp;
            }
            set
            {
                PermissionNumber1_DB = value.Value;
                OnPropertyChanged(nameof(PermissionNumber1));
            }
        }
        private void PermissionNumber1ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionNumber1_DB = ((RamAccess<string>)Value).Value;
            }
        }

        private bool PermissionNumber1_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionNumber1 property
        #endregion

        //PermissionIssueDate1 property
        #region PermissionIssueDate1 
        public string PermissionIssueDate1_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public RamAccess<string> PermissionIssueDate1
        {
            get
            {
                    var tmp = new RamAccess<string>(PermissionIssueDate1_Validation, PermissionIssueDate1_DB);
                    tmp.PropertyChanged += PermissionIssueDate1ValueChanged;
                    return tmp;
            }
            set
            {
                PermissionIssueDate1_DB = value.Value;
                OnPropertyChanged(nameof(PermissionIssueDate1));
            }
        }

        private void PermissionIssueDate1ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionIssueDate1_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PermissionIssueDate1_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                return true;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //PermissionIssueDate1 property
        #endregion

        //PermissionDocumentName1 property
        #region PermissionDocumentName1
        public string PermissionDocumentName1_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Наименование разрешительного документа")]
        public RamAccess<string> PermissionDocumentName1
        {
            get
            {
                    var tmp = new RamAccess<string>(PermissionDocumentName1_Validation, PermissionDocumentName1_DB);
                    tmp.PropertyChanged += PermissionDocumentName1ValueChanged;
                    return tmp;
            }
            set
            {
                PermissionDocumentName1_DB = value.Value;
                OnPropertyChanged(nameof(PermissionDocumentName1));
            }
        }

        private void PermissionDocumentName1ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionDocumentName1_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PermissionDocumentName1_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionDocumentName1 property
        #endregion

        //ValidBegin1 property
        #region ValidBegin1
        public string ValidBegin1_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Действует с")]
        public RamAccess<string> ValidBegin1
        {
            get
            {
                    var tmp = new RamAccess<string>(ValidBegin1_Validation, ValidBegin1_DB);
                    tmp.PropertyChanged += ValidBegin1ValueChanged;
                    return tmp;
            }
            set
            {
                ValidBegin1_DB = value.Value;
                OnPropertyChanged(nameof(ValidBegin1));
            }
        }

        private void ValidBegin1ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ValidBegin1_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool ValidBegin1_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                return true;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //ValidBegin1 property
        #endregion

        //ValidThru1 property
        #region ValidThru1 
        public string ValidThru1_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Действует по")]
        public RamAccess<string> ValidThru1
        {
            get
            {
                var tmp = new RamAccess<string>(ValidThru1_Validation, ValidThru1_DB);
                tmp.PropertyChanged += ValidThru1ValueChanged;
                return tmp;
            }
            set
            {
                ValidThru1_DB = value.Value;
                OnPropertyChanged(nameof(ValidThru1));
            }
        }

        private void ValidThru1ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ValidThru1_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool ValidThru1_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                return true;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //ValidThru1 property
        #endregion

        //PermissionNumber2 property
        #region PermissionNumber2 
        public string PermissionNumber2_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Номер разрешительного документа")]
        public RamAccess<string> PermissionNumber2
        {
            get
            {
                var tmp = new RamAccess<string>(PermissionNumber2_Validation, PermissionNumber2_DB);
                tmp.PropertyChanged += PermissionNumber2ValueChanged;
                return tmp;
            }
            set
            {
                PermissionNumber2_DB = value.Value;
                OnPropertyChanged(nameof(PermissionNumber2));
            }
        }

        private void PermissionNumber2ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionNumber2_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PermissionNumber2_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionNumber2 property
        #endregion

        //PermissionIssueDate2 property
        #region PermissionIssueDate2
        public string PermissionIssueDate2_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public RamAccess<string> PermissionIssueDate2
        {
            get
            {
                    var tmp = new RamAccess<string>(PermissionIssueDate2_Validation, PermissionIssueDate2_DB);
                    tmp.PropertyChanged += PermissionIssueDate2ValueChanged;
                    return tmp;
            }
            set
            {
                PermissionIssueDate2_DB = value.Value;
                OnPropertyChanged(nameof(PermissionIssueDate2));
            }
        }

        private void PermissionIssueDate2ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionIssueDate2_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PermissionIssueDate2_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                return true;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //PermissionIssueDate property
        #endregion

        //PermissionDocumentName2 property
        #region PermissionDocumentName2
        public string PermissionDocumentName2_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Наименование разрешительного документа")]
        public RamAccess<string> PermissionDocumentName2
        {
            get
            {
                    var tmp = new RamAccess<string>(PermissionDocumentName2_Validation, PermissionDocumentName2_DB);
                    tmp.PropertyChanged += PermissionDocumentName2ValueChanged;
                    return tmp;
            }
            set
            {
                    PermissionDocumentName2_DB = value.Value;
                OnPropertyChanged(nameof(PermissionDocumentName2));
            }
        }

        private void PermissionDocumentName2ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PermissionDocumentName2_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PermissionDocumentName2_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionDocumentName2 property
        #endregion

        //ValidBegin2 property
        #region ValidBegin2
        public string ValidBegin2_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Действует с")]
        public RamAccess<string> ValidBegin2
        {
            get
            {
                var tmp = new RamAccess<string>(ValidBegin2_Validation, ValidBegin2_DB);
                tmp.PropertyChanged += ValidBegin2ValueChanged;
                return tmp;
            }
            set
            {
                ValidBegin2_DB = value.Value;
                OnPropertyChanged(nameof(ValidBegin2));
            }
        }

        private void ValidBegin2ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ValidBegin2_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool ValidBegin2_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                return true;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //ValidBegin2 property
        #endregion

        //ValidThru2 property
        #region ValidThru2
        public string ValidThru2_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Действует по")]
        public RamAccess<string> ValidThru2
        {
            get
            {
                    var tmp = new RamAccess<string>(ValidThru2_Validation, ValidThru2_DB);
                    tmp.PropertyChanged += ValidThru2ValueChanged;
                    return tmp;
            }
            set
            {
                ValidThru2_DB = value.Value;
                OnPropertyChanged(nameof(ValidThru2));
            }
        }

        private void ValidThru2ValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ValidThru2_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool ValidThru2_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                return true;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //ValidThru2 property
        #endregion

        //WasteSourceName property
        #region WasteSourceName
        public string WasteSourceName_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Наименование, номер выпуска сточных вод")]
        public RamAccess<string> WasteSourceName
        {
            get
            {
                    var tmp = new RamAccess<string>(WasteSourceName_Validation, WasteSourceName_DB);
                    tmp.PropertyChanged += WasteSourceNameValueChanged;
                    return tmp;
            }
            set
            {
                    WasteSourceName_DB = value.Value;
                OnPropertyChanged(nameof(WasteSourceName));
            }
        }

        private void WasteSourceNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                WasteSourceName_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool WasteSourceName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //WasteSourceName property
        #endregion

        //WasteRecieverName property
        #region WasteRecieverName
        public string WasteRecieverName_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Наименование приемника отведенных вод")]
        public RamAccess<string> WasteRecieverName
        {
            get
            {
                    var tmp = new RamAccess<string>(WasteRecieverName_Validation, WasteRecieverName_DB);
                    tmp.PropertyChanged += WasteRecieverNameValueChanged;
                    return tmp;
            }
            set
            {
                    WasteRecieverName_DB = value.Value;
                OnPropertyChanged(nameof(WasteRecieverName));
            }
        }

        private void WasteRecieverNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                WasteRecieverName_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool WasteRecieverName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //WasteRecieverName property
        #endregion

        //RecieverTypeCode property
        #region RecieverTypeCode
        public string RecieverTypeCode_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Код типа приемника отведенных вод")]
        public RamAccess<string> RecieverTypeCode
        {
            get
            {
                    var tmp = new RamAccess<string>(RecieverTypeCode_Validation, RecieverTypeCode_DB);
                    tmp.PropertyChanged += RecieverTypeCodeValueChanged;
                    return tmp;
            }
            set
            {
                    RecieverTypeCode_DB = value.Value;
                OnPropertyChanged(nameof(RecieverTypeCode));
            }
        }

        private void RecieverTypeCodeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                RecieverTypeCode_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool RecieverTypeCode_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (Spravochniks.SprRecieverTypeCode.Contains(value.Value))
            {
                return true;
            }
            value.AddError("Недопустимое значение");
            return false;
        }
        //RecieverTypeCode property
        #endregion

        //PoolDistrictName property
        #region PoolDistrictName
        public string PoolDistrictName_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Наименование бассейнового округа приемника отведенных вод")]
        public RamAccess<string> PoolDistrictName
        {
            get
            {
                    var tmp = new RamAccess<string>(PoolDistrictName_Validation, PoolDistrictName_DB);
                    tmp.PropertyChanged += PoolDistrictNameValueChanged;
                    return tmp;
            }
            set
            {
                    PoolDistrictName_DB = value.Value;
                OnPropertyChanged(nameof(PoolDistrictName));
            }
        }

        private void PoolDistrictNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PoolDistrictName_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PoolDistrictName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            List<string> spr = new List<string>();
            if (spr.Contains(value.Value))
            {
                return true;
            }
            value.AddError("Недопустимое значение");
            return false;
        }
        //PoolDistrictName property
        #endregion

        //AllowedWasteRemovalVolume property
        #region AllowedWasteRemovalVolume
        public string AllowedWasteRemovalVolume_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Допустимый объем водоотведения за год, тыс. куб. м")]
        public RamAccess<string> AllowedWasteRemovalVolume
        {
            get
            {
                    var tmp = new RamAccess<string>(AllowedWasteRemovalVolume_Validation, AllowedWasteRemovalVolume_DB);
                    tmp.PropertyChanged += AllowedWasteRemovalVolumeValueChanged;
                    return tmp;
            }
            set
            {
                    AllowedWasteRemovalVolume_DB = value.Value;
                OnPropertyChanged(nameof(AllowedWasteRemovalVolume));
            }
        }

        private void AllowedWasteRemovalVolumeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                AllowedWasteRemovalVolume_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool AllowedWasteRemovalVolume_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //AllowedWasteRemovalVolume property
        #endregion

        //RemovedWasteVolume property
        #region RemovedWasteVolume
        public string RemovedWasteVolume_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Отведено за отчетный период, тыс. куб. м")]
        public RamAccess<string> RemovedWasteVolume
        {
            get
            {
                    var tmp = new RamAccess<string>(RemovedWasteVolume_Validation, RemovedWasteVolume_DB);
                    tmp.PropertyChanged += RemovedWasteVolumeValueChanged;
                    return tmp;
            }
            set
            {
                    RemovedWasteVolume_DB = value.Value;
                OnPropertyChanged(nameof(RemovedWasteVolume));
            }
        }

        private void RemovedWasteVolumeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                RemovedWasteVolume_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool RemovedWasteVolume_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //RemovedWasteVolume property
        #endregion

        #region IExcel
        public void ExcelRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelRow(worksheet, Row);
            worksheet.Cells[Row, 2].Value = WasteSourceName_DB;
            worksheet.Cells[Row, 3].Value = WasteRecieverName_DB;
            worksheet.Cells[Row, 4].Value = RecieverTypeCode_DB;
            worksheet.Cells[Row, 5].Value = PoolDistrictName_DB;
            worksheet.Cells[Row, 6].Value = AllowedWasteRemovalVolume_DB;
            worksheet.Cells[Row, 7].Value = RemovedWasteVolume_DB;
        }

        public static void ExcelHeader(ExcelWorksheet worksheet)
        {
            Form2.ExcelHeader(worksheet);
            worksheet.Cells[1, 2].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(WasteSourceName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 3].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(WasteRecieverName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 4].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(RecieverTypeCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 5].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(PoolDistrictName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 6].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(AllowedWasteRemovalVolume)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 7].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(RemovedWasteVolume)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
        }
        #endregion
    }
}
