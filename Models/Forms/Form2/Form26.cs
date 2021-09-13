using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Linq;
using Spravochniki;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.6: Контроль загрязнения подземных вод РВ")]
    public class Form26 : Abstracts.Form2
    {
        public Form26() : base()
        {
            FormNum.Value = "2.6";
            //NumberOfFields.Value = 11;
            Validate_all();
        }

        private void Validate_all()
        {
            ObservedSourceNumber_Validation(ObservedSourceNumber);
            ControlledAreaName_Validation(ControlledAreaName);
            SupposedWasteSource_Validation(SupposedWasteSource);
            DistanceToWasteSource_Validation(DistanceToWasteSource);
            TestDepth_Validation(TestDepth);
            RadionuclidName_Validation(RadionuclidName);
            AverageYearConcentration_Validation(AverageYearConcentration);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return !(ObservedSourceNumber.HasErrors||
            ControlledAreaName.HasErrors||
            SupposedWasteSource.HasErrors||
            DistanceToWasteSource.HasErrors||
            TestDepth.HasErrors||
            RadionuclidName.HasErrors||
            AverageYearConcentration.HasErrors);
        }

        //ObservedSourceNumber property
        #region ObservedSourceNumber
        public string ObservedSourceNumber_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Номер наблюдательной скважины")]
        public RamAccess<string> ObservedSourceNumber
        {
            get
{
var tmp = new RamAccess<string>(ObservedSourceNumber_Validation, ObservedSourceNumber_DB);
tmp.PropertyChanged += ObservedSourceNumberValueChanged;
return tmp;
}            set
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
            value.ClearErrors(); return true;
        }
        //ObservedSourceNumber property
        #endregion

        //ControlledAreaName property
        #region ControlledAreaName
        public string ControlledAreaName_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Наименование зоны контроля")]
        public RamAccess<string> ControlledAreaName
        {
            get
{
var tmp = new RamAccess<string>(ControlledAreaName_Validation, ControlledAreaName_DB);
tmp.PropertyChanged += ControlledAreaNameValueChanged;
return tmp;
}            set
            {
                ControlledAreaName_DB = value.Value;
                OnPropertyChanged(nameof(ControlledAreaName));
            }
        }
        //If change this change validation
        private void ControlledAreaNameValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                ControlledAreaName_DB = ((RamAccess<string>)Value).Value;
}
}
private bool ControlledAreaName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            List<string> spr = new List<string>()
            {
                "ПП",
                "СЗЗ",
                "ЗН",
                "прим."
            };
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //ControlledAreaName property
        #endregion

        //SupposedWasteSource property
        #region SupposedWasteSource
        public string SupposedWasteSource_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Предполагаемый источник поступления радиоактивных веществ")]
        public RamAccess<string> SupposedWasteSource
        {
            get
{
var tmp = new RamAccess<string>(SupposedWasteSource_Validation, SupposedWasteSource_DB);
tmp.PropertyChanged += SupposedWasteSourceValueChanged;
return tmp;
}            set
            {
                SupposedWasteSource_DB = value.Value;
                OnPropertyChanged(nameof(SupposedWasteSource));
            }
        }

        private void SupposedWasteSourceValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                SupposedWasteSource_DB = ((RamAccess<string>)Value).Value;
}
}
private bool SupposedWasteSource_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();//done
            return true;
        }
        //SupposedWasteSource property
        #endregion

        //DistanceToWasteSource property
        #region DistanceToWasteSource
        public string DistanceToWasteSource_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м")]
        public RamAccess<string> DistanceToWasteSource
        {
            get
{
var tmp = new RamAccess<string>(DistanceToWasteSource_Validation, DistanceToWasteSource_DB);
tmp.PropertyChanged += DistanceToWasteSourceValueChanged;
return tmp;
}            set
            {
                    DistanceToWasteSource_DB = value.Value;
                OnPropertyChanged(nameof(DistanceToWasteSource));
            }
        }

        private void DistanceToWasteSourceValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                DistanceToWasteSource_DB = ((RamAccess<string>)Value).Value;
}
}
private bool DistanceToWasteSource_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            if (value.Value.Equals("прим."))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //DistanceToWasteSource property
        #endregion

        //TestDepth property
        #region TestDepth
        public string TestDepth_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Глубина отбора проб, м")]
        public RamAccess<string> TestDepth
        {
            get
            {
                    var tmp = new RamAccess<string>(TestDepth_Validation, TestDepth_DB);
                    tmp.PropertyChanged += TestDepthValueChanged;
                    return tmp;
            }
            set
            {
                    TestDepth_DB = value.Value;
                OnPropertyChanged(nameof(TestDepth));
            }
        }

        private void TestDepthValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                TestDepth_DB = ((RamAccess<string>)Value).Value;
}
}
private bool TestDepth_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            if (value.Value.Equals("прим."))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //TestDepth property
        #endregion

        //RadionuclidName property
        #region RadionuclidName
        public string RadionuclidName_DB { get; set; } = ""; [NotMapped]
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

        //AverageYearConcentration property
        #region AverageYearConcentration 
        public string AverageYearConcentration_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Среднегодовое содержание радионуклида, Бк/кг")]
        public RamAccess<string> AverageYearConcentration
        {
            get
            {
                    var tmp = new RamAccess<string>(AverageYearConcentration_Validation, AverageYearConcentration_DB);
                    tmp.PropertyChanged += AverageYearConcentrationValueChanged;
                    return tmp;
            }
            set
            {
                    AverageYearConcentration_DB = value.Value;
                OnPropertyChanged(nameof(AverageYearConcentration));
            }
        }

        private void AverageYearConcentrationValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                AverageYearConcentration_DB = ((RamAccess<string>)Value).Value;
}
}
private bool AverageYearConcentration_Validation(RamAccess<string> value)//TODO
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
        //AverageYearConcentration property
        #endregion

        #region IExcel
        public void ExcelRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelRow(worksheet, Row);
            worksheet.Cells[Row, 2].Value = ObservedSourceNumber_DB;
            worksheet.Cells[Row, 3].Value = ControlledAreaName_DB;
            worksheet.Cells[Row, 4].Value = SupposedWasteSource_DB;
            worksheet.Cells[Row, 5].Value = DistanceToWasteSource_DB;
            worksheet.Cells[Row, 6].Value = TestDepth_DB;
            worksheet.Cells[Row, 7].Value = RadionuclidName_DB;
            worksheet.Cells[Row, 8].Value = AverageYearConcentration_DB;
        }

        public static void ExcelHeader(ExcelWorksheet worksheet)
        {
            Form2.ExcelHeader(worksheet);
            worksheet.Cells[1, 2].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form26,Models").GetProperty(nameof(ObservedSourceNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 3].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form26,Models").GetProperty(nameof(ControlledAreaName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 4].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form26,Models").GetProperty(nameof(SupposedWasteSource)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 5].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form26,Models").GetProperty(nameof(DistanceToWasteSource)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 6].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form26,Models").GetProperty(nameof(TestDepth)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 7].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form26,Models").GetProperty(nameof(RadionuclidName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 8].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form26,Models").GetProperty(nameof(AverageYearConcentration)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
        }
        #endregion
    }
}
