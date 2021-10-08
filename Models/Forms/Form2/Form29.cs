using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel;
using System.Globalization;
using Spravochniki;
using System.Linq;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;

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
        public string RadionuclidName_DB { get; set; } = ""; 
        [NotMapped]        
        [Attributes.Form_Property("Радионуклид")]
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
                RadionuclidName_DB = ((RamAccess<string>)Value).Value;
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
                AllowedActivity_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool AllowedActivity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value == "прим.")
            {
                return true;
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
        //AllowedActivity property
        #endregion

        //FactedActivity property
        #region FactedActivity
        public double? FactedActivity_DB { get; set; } = null; [NotMapped]        [Attributes.Form_Property("Фактическая активность радионуклида, Бк")]
        public RamAccess<double?> FactedActivity
        {
            get
            {
                    var tmp = new RamAccess<double?>(FactedActivity_Validation, FactedActivity_DB);
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
                FactedActivity_DB = ((RamAccess<double?>)Value).Value;
            }
        }

        private bool FactedActivity_Validation(RamAccess<double?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if(value.Value<=0)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //FactedActivity property
        #endregion

        #region IExcel
        public void ExcelRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelRow(worksheet, Row);
            worksheet.Cells[Row, 2].Value = WasteSourceName_DB;
            worksheet.Cells[Row, 3].Value = RadionuclidName_DB;
            worksheet.Cells[Row, 4].Value = AllowedActivity_DB;
            worksheet.Cells[Row, 5].Value = FactedActivity_DB;
        }

        public static void ExcelHeader(ExcelWorksheet worksheet)
        {
            Form2.ExcelHeader(worksheet);

            worksheet.Cells[1, 2].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form29,Models").GetProperty(nameof(WasteSourceName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 3].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form29,Models").GetProperty(nameof(RadionuclidName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 4].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form29,Models").GetProperty(nameof(AllowedActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 5].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form29,Models").GetProperty(nameof(FactedActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
        }
        #endregion
    }
}
