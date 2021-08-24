using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Spravochniki;
using System.Globalization;
using System.ComponentModel;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.7: Поступление радионуклидов в атмосферный воздух")]
    public class Form27 : Abstracts.Form2
    {
        public Form27() : base()
        {
            FormNum.Value = "2.7";
            //NumberOfFields.Value = 13;
            Validate_all();
        }

        private void Validate_all()
        {
            ValidThru_Validation(ValidThru);
            ValidBegin_Validation(ValidBegin);
            PermissionNumber_Validation(PermissionNumber);
            PermissionIssueDate_Validation(PermissionIssueDate);
            PermissionDocumentName_Validation(PermissionDocumentName);
            ObservedSourceNumber_Validation(ObservedSourceNumber);
            RadionuclidName_Validation(RadionuclidName);
            AllowedWasteValue_Validation(AllowedWasteValue);
            FactedWasteValue_Validation(FactedWasteValue);
            WasteOutbreakPreviousYear_Validation(WasteOutbreakPreviousYear);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return !(ValidThru.HasErrors||
            ValidBegin.HasErrors||
            PermissionNumber.HasErrors||
            PermissionIssueDate.HasErrors||
            PermissionDocumentName.HasErrors||
            ObservedSourceNumber.HasErrors||
            RadionuclidName.HasErrors||
            AllowedWasteValue.HasErrors||
            FactedWasteValue.HasErrors||
            WasteOutbreakPreviousYear.HasErrors);
        }

        //ObservedSourceNumber property
        #region  ObservedSourceNumber
        public string ObservedSourceNumber_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Номер источника выбросов")]
        public RamAccess<string> ObservedSourceNumber
        {
            get
            {
                var tmp = new RamAccess<string>(ObservedSourceNumber_Validation, ObservedSourceNumber_DB);
                tmp.PropertyChanged += ObservedSourceNumberValueChanged;
                return tmp;
            }
            set
            {
                ObservedSourceNumber_DB = value.Value;
                OnPropertyChanged(nameof(ObservedSourceNumber));
            }
        }
        //If change this change validation
        private void ObservedSourceNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ObservedSourceNumber_DB = ((RamAccess<string>)Value).Value;
            }
        }
private bool ObservedSourceNumber_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //ObservedSourceNumber property
        #endregion

        //PermissionNumber property
        #region  PermissionNumber
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
        #region  PermissionIssueDate
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

        //PermissionDocumentName property
        #region  PermissionDocumentName
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
        #region  ValidBegin
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
        //ValidBegin property
        #endregion

        //ValidThru property
        #region  ValidThru
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
        //ValidThru property
        #endregion

        //RadionuclidName property
        #region  RadionuclidName
        public string RadionuclidName_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Наименование радионуклида")]
        public RamAccess<string> RadionuclidName
        {
            get
            {
                    var tmp = new RamAccess<string>(RadionuclidName_Validation, RadionuclidName_DB);
                    tmp.PropertyChanged += RadionuclidNameValueChanged;
                    return tmp;
            }
            set
            {
                    RadionuclidName_DB = value.Value;
                OnPropertyChanged(nameof(RadionuclidName));
            }
        }


        private void RadionuclidNameValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                RadionuclidName_DB = ((RamAccess<string>)Value).Value;
}
}
private bool RadionuclidName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var query = from item in Spravochniks.SprRadionuclids where item.Item1 == value.Value select item.Item1;
            if (!query.Any())
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //RadionuclidName property
        #endregion

        ////RadionuclidNameNote property
        //public RamAccess<string> RadionuclidNameNote
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<string>(RadionuclidNameNote_Validation, _DB);
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            RadionuclidNameNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(RadionuclidNameNote));
        //    }
        //}


        //        //private void ValueChanged(object Value, PropertyChangedEventArgs args)
        //{
        //if (args.PropertyName == "Value")
        //{
        //_DB = ((RamAccess<string>)Value).Value;
        //}
        //}
        //private bool RadionuclidNameNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////RadionuclidNameNote property

        //AllowedWasteValue property
        #region  AllowedWasteValue
        public string AllowedWasteValue_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Разрешенный выброс радионуклида в атмосферу за отчетный год, Бк")]
        public RamAccess<string> AllowedWasteValue
        {
            get
            {
                    var tmp = new RamAccess<string>(AllowedWasteValue_Validation, AllowedWasteValue_DB);
                    tmp.PropertyChanged += AllowedWasteValueValueChanged;
                    return tmp;
            }
            set
            {
                    AllowedWasteValue_DB = value.Value;
                OnPropertyChanged(nameof(AllowedWasteValue));
            }
        }


        private void AllowedWasteValueValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                AllowedWasteValue_DB = ((RamAccess<string>)Value).Value;
}
}
private bool AllowedWasteValue_Validation(RamAccess<string> value)
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
        //AllowedWasteValue property
        #endregion

        ////AllowedWasteValueNote property
        //public RamAccess<string> AllowedWasteValueNote
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<string>(AllowedWasteValueNote_Validation, _DB);
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            AllowedWasteValueNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(AllowedWasteValueNote));
        //    }
        //}


        //private bool AllowedWasteValueNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////AllowedWasteValueNote property

        //FactedWasteValue property
        #region  FactedWasteValue
        public string FactedWasteValue_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Фактический выброс радионуклида в атмосферу за отчетный год, Бк")]
        public RamAccess<string> FactedWasteValue
        {
            get
            {
                    var tmp = new RamAccess<string>(FactedWasteValue_Validation, FactedWasteValue_DB);
                    tmp.PropertyChanged += FactedWasteValueValueChanged;
                    return tmp;
            }
            set
            {
                    FactedWasteValue_DB = value.Value;
                OnPropertyChanged(nameof(FactedWasteValue));
            }
        }


        private void FactedWasteValueValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                FactedWasteValue_DB = ((RamAccess<string>)Value).Value;
}
}
private bool FactedWasteValue_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("-"))
            {
                return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //FactedWasteValue property
        #endregion

        ////FactedWasteValueNote property
        //public RamAccess<string> FactedWasteValueNote
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<string>(FactedWasteValueNote_Validation, _DB);
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            FactedWasteValueNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(FactedWasteValueNote));
        //    }
        //}


        //private bool FactedWasteValueNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////FactedWasteValueNote property

        //WasteOutbreakPreviousYear property
        #region  WasteOutbreakPreviousYear
        public string WasteOutbreakPreviousYear_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Фактический выброс радионуклида в атмосферу за предыдущий год, Бк")]
        public RamAccess<string> WasteOutbreakPreviousYear
        {
            get
            {
                    var tmp = new RamAccess<string>(WasteOutbreakPreviousYear_Validation, WasteOutbreakPreviousYear_DB);
                    tmp.PropertyChanged += WasteOutbreakPreviousYearValueChanged;
                    return tmp;
            }
            set
            {
                    WasteOutbreakPreviousYear_DB = value.Value;
                OnPropertyChanged(nameof(WasteOutbreakPreviousYear));
            }
        }


        private void WasteOutbreakPreviousYearValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                WasteOutbreakPreviousYear_DB = ((RamAccess<string>)Value).Value;
}
}
private bool WasteOutbreakPreviousYear_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("-"))
            {
                return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //WasteOutbreakPreviousYear property
        #endregion
    }
}
