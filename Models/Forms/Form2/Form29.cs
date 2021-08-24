using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel;
using System.Globalization;
using Spravochniki;
using System.Linq;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.9: Активность радионуклидов, отведенных со сточными водами")]
    public class Form29 : Abstracts.Form2
    {
        public Form29() : base()
        {
            FormNum.Value = "2.9";
            //NumberOfFields.Value = 8;
            Validate_all();
        }

        private void Validate_all()
        {
            WasteSourceName_Validation(WasteSourceName);
            RadionuclidName_Validation(RadionuclidName);
            AllowedActivity_Validation(AllowedActivity);
            FactedActivity_Validation(FactedActivity);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return !(WasteSourceName.HasErrors||
            RadionuclidName.HasErrors||
            AllowedActivity.HasErrors||
            FactedActivity.HasErrors);
        }

        //WasteSourceName property
        #region WasteSourceName 
        public string WasteSourceName_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Наименование, номер выпуска сточных вод")]
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

        //RadionuclidName property
        #region RadionuclidName
        public string RadionuclidName_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Радионуклид")]
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
        //If change this change validation
        private void RadionuclidNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                WasteSourceName_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool RadionuclidName_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var tmp = from item in Spravochniks.SprRadionuclids where value.Value == item.Item1 select item.Item1;
            if (!tmp.Any())
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //RadionuclidName property
        #endregion

        //AllowedActivity property
        #region AllowedActivity
        public string AllowedActivity_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Допустимая активность радионуклида, Бк")]
        public RamAccess<string> AllowedActivity
        {
            get
            {
                    var tmp = new RamAccess<string>(AllowedActivity_Validation, AllowedActivity_DB);
                    tmp.PropertyChanged += AllowedActivityValueChanged;
                    return tmp;
            }
            set
            {
                    AllowedActivity_DB = value.Value;
                OnPropertyChanged(nameof(AllowedActivity));
            }
        }

        private void AllowedActivityValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                WasteSourceName_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool AllowedActivity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || (value.Value.Equals("")))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            if (value.Value != "прим.")
            {
                NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                   NumberStyles.AllowExponent;
                try
                {
                    if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    {
                        value.AddError("Число должно быть больше нуля"); return false;
                    }
                }
                catch
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            return true;
        }
        //AllowedActivity property
        #endregion

        ////AllowedActivityNote property
        //public RamAccess<string> AllowedActivityNote
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<string>(AllowedActivityNote_Validation, _DB);
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            AllowedActivityNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(AllowedActivityNote));
        //    }
        //}


        //private bool AllowedActivityNote_Validation(RamAccess<string> value)//Ready
        //{
        //    return true;
        //}
        ////AllowedActivityNote property

        //FactedActivity property
        #region FactedActivity
        public string FactedActivity_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Фактическая активность радионуклида, Бк")]
        public RamAccess<string> FactedActivity
        {
            get
            {
                    var tmp = new RamAccess<string>(FactedActivity_Validation, FactedActivity_DB);
                    tmp.PropertyChanged += FactedActivityValueChanged;
                    return tmp;
            }
            set
            {
                    FactedActivity_DB = value.Value;
                OnPropertyChanged(nameof(FactedActivity));
            }
        }
        private void FactedActivityValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                WasteSourceName_DB = ((RamAccess<string>)Value).Value;
            }
        }

        private bool FactedActivity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || (value.Value.Equals("")))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                {
                    value.AddError("Число должно быть больше нуля"); return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //FactedActivity property
        #endregion

        ////FactedActivityNote property
        //public RamAccess<string> FactedActivityNote
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<string>(FactedActivityNote_Validation, _DB);
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            FactedActivityNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(FactedActivityNote));
        //    }
        //}


        //private bool FactedActivityNote_Validation(RamAccess<string> value)//Ready
        //{
        //    return true;
        //}
        ////FactedActivityNote property
    }
}
