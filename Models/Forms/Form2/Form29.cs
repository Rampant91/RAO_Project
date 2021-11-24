using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
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
            var tmpstr = value.Value.ToLower().Replace(" ", "");
            var tmp = from item in Spravochniks.SprRadionuclids where tmpstr == item.Item1 select item.Item1;
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
        public string AllowedActivity_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("допустимая")]
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
                var value1=((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    AllowedActivity_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                AllowedActivity_DB = value1;
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
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e'))&& (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
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
        public string FactedActivity_DB { get; set; } = null; [NotMapped]        [Attributes.Form_Property("фактическая")]
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    FactedActivity_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                FactedActivity_DB = value1;
            }
        }

        private bool FactedActivity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
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

        #region IExcel
        public int ExcelRow(ExcelWorksheet worksheet, int Row, int Column,bool Transpon=true)
        {
            var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = WasteSourceName_DB;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = RadionuclidName_DB;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = AllowedActivity_DB;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = FactedActivity_DB;

            return 4;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = Form2.ExcelHeader(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form29,Models").GetProperty(nameof(WasteSourceName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form29,Models").GetProperty(nameof(RadionuclidName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form29,Models").GetProperty(nameof(AllowedActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form29,Models").GetProperty(nameof(FactedActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            return 4;
        }
        #endregion
    }
}
