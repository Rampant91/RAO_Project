using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.10: Территории, загрязненные радионуклидами")]
    public class Form210 : Abstracts.Form2
    {
        public Form210() : base()
        {
            FormNum.Value = "2.10";
            //NumberOfFields.Value = 12;
            Validate_all();
        }

        private void Validate_all()
        {
            IndicatorName_Validation(IndicatorName);
            PlotName_Validation(PlotName);
            PlotKadastrNumber_Validation(PlotKadastrNumber);
            PlotCode_Validation(PlotCode);
            InfectedArea_Validation(InfectedArea);
            AvgGammaRaysDosePower_Validation(AvgGammaRaysDosePower);
            MaxGammaRaysDosePower_Validation(MaxGammaRaysDosePower);
            WasteDensityAlpha_Validation(WasteDensityAlpha);
            WasteDensityBeta_Validation(WasteDensityBeta);
            FcpNumber_Validation(FcpNumber);
        }

        [Attributes.Form_Property("Форма")]    
        public override bool Object_Validation()
        {
            return !(IndicatorName.HasErrors||
            PlotName.HasErrors||
            PlotKadastrNumber.HasErrors||
            PlotCode.HasErrors||
            InfectedArea.HasErrors||
            AvgGammaRaysDosePower.HasErrors||
            MaxGammaRaysDosePower.HasErrors||
            WasteDensityAlpha.HasErrors||
            WasteDensityBeta.HasErrors||
            FcpNumber.HasErrors);
        }

        //IndicatorName property
        #region  IndicatorName
        public string IndicatorName_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Наименование показателя")]
        public RamAccess<string> IndicatorName
        {
            get
{
var tmp = new RamAccess<string>(IndicatorName_Validation, IndicatorName_DB);
tmp.PropertyChanged += IndicatorNameValueChanged;
return tmp;
}            set
            {
                IndicatorName_DB = value.Value;
                OnPropertyChanged(nameof(IndicatorName));
            }
        }

        private void IndicatorNameValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                IndicatorName_DB = ((RamAccess<string>)Value).Value;
}
}
private bool IndicatorName_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var spr = new List<string> { 
                "З","Р","Н"
            };
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //IndicatorName property
        #endregion

        //PlotName property
        #region  PlotName
        public string PlotName_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Наименование участка")]
        public RamAccess<string> PlotName
        {
            get
            {
                    var tmp = new RamAccess<string>(PlotName_Validation, PlotName_DB);
                    tmp.PropertyChanged += PlotNameValueChanged;
                    return tmp;
            }
            set
            {
                    PlotName_DB = value.Value;
                OnPropertyChanged(nameof(PlotName));
            }
        }

        private void PlotNameValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                PlotName_DB = ((RamAccess<string>)Value).Value;
}
}
private bool PlotName_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //PlotName property
        #endregion

        //PlotKadastrNumber property
        #region PlotKadastrNumber 
        public string PlotKadastrNumber_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Кадастровый номер участка")]
        public RamAccess<string> PlotKadastrNumber
        {
            get
            {
                    var tmp = new RamAccess<string>(PlotKadastrNumber_Validation, PlotKadastrNumber_DB);
                    tmp.PropertyChanged += PlotKadastrNumberValueChanged;
                    return tmp;
            }
            set
            {
                    PlotKadastrNumber_DB = value.Value;
                OnPropertyChanged(nameof(PlotKadastrNumber));
            }
        }

        private void PlotKadastrNumberValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                PlotKadastrNumber_DB = ((RamAccess<string>)Value).Value;
}
}
private bool PlotKadastrNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //PlotKadastrNumber property
        #endregion

        //PlotCode property
        #region  PlotCode
        public string PlotCode_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Код участка")]
        public RamAccess<string> PlotCode
        {
            get
            {
                    var tmp = new RamAccess<string>(PlotCode_Validation, PlotCode_DB);
                    tmp.PropertyChanged += PlotCodeValueChanged;
                    return tmp;
            }
            set
            {
                    PlotCode_DB = value.Value;
                OnPropertyChanged(nameof(PlotCode));
            }
        }
        //6 symbols code
        private void PlotCodeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PlotCode_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PlotCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex a = new Regex("^[0-9]{6}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //PlotCode property
        #endregion

        //InfectedArea property
        #region  InfectedArea
        public string InfectedArea_DB { get; set; } = null; [NotMapped]        [Attributes.Form_Property("Площадь загрязненной территории, кв. м")]
        public RamAccess<string> InfectedArea
        {
            get
            {
                    var tmp = new RamAccess<string>(InfectedArea_Validation, InfectedArea_DB);
                    tmp.PropertyChanged += InfectedAreaValueChanged;
                    return tmp;
            }
            set
            {
                    InfectedArea_DB = value.Value;
                OnPropertyChanged(nameof(InfectedArea));
            }
        }

        private void InfectedAreaValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    InfectedArea_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                InfectedArea_DB = value1;
            }
        }
        private bool InfectedArea_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //InfectedArea property
        #endregion

        //AvgGammaRaysDosePower property
        #region  AvgGammaRaysDosePower
        public string AvgGammaRaysDosePower_DB { get; set; } = null; [NotMapped]        [Attributes.Form_Property("средняя")]
        public RamAccess<string> AvgGammaRaysDosePower
        {
            get
            {
                    var tmp = new RamAccess<string>(AvgGammaRaysDosePower_Validation, AvgGammaRaysDosePower_DB);
                    tmp.PropertyChanged += AvgGammaRaysDosePowerValueChanged;
                    return tmp;
            }
            set
            {
                    AvgGammaRaysDosePower_DB = value.Value;
                OnPropertyChanged(nameof(AvgGammaRaysDosePower));
            }
        }

        private void AvgGammaRaysDosePowerValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    AvgGammaRaysDosePower_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                AvgGammaRaysDosePower_DB = value1;
            }
        }
        private bool AvgGammaRaysDosePower_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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
        //AvgGammaRaysDosePower property
        #endregion

        //MaxGammaRaysDosePower property
        #region  MaxGammaRaysDosePower
        public string MaxGammaRaysDosePower_DB { get; set; } = null; [NotMapped]        [Attributes.Form_Property("максимальная")]
        public RamAccess<string> MaxGammaRaysDosePower
        {
            get
            {
                var tmp = new RamAccess<string>(MaxGammaRaysDosePower_Validation, MaxGammaRaysDosePower_DB);
                tmp.PropertyChanged += MaxGammaRaysDosePowerValueChanged;
                return tmp;
            }
            set
            {
                MaxGammaRaysDosePower_DB = value.Value;
                OnPropertyChanged(nameof(MaxGammaRaysDosePower));
            }
        }

        private void MaxGammaRaysDosePowerValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    MaxGammaRaysDosePower_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                MaxGammaRaysDosePower_DB = value1;
            }
        }
        private bool MaxGammaRaysDosePower_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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
        //MaxGammaRaysDosePower property
        #endregion

        //WasteDensityAlpha property
        #region  WasteDensityAlpha
        public string WasteDensityAlpha_DB { get; set; } = null; [NotMapped]        [Attributes.Form_Property("альфа-излучающие радионуклиды")]
        public RamAccess<string> WasteDensityAlpha
        {
            get
            {
                    var tmp = new RamAccess<string>(WasteDensityAlpha_Validation, WasteDensityAlpha_DB);
                    tmp.PropertyChanged += WasteDensityAlphaValueChanged;
                    return tmp;
            }
            set
            {
                    WasteDensityAlpha_DB = value.Value;
                OnPropertyChanged(nameof(WasteDensityAlpha));
            }
        }

        private void WasteDensityAlphaValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    WasteDensityAlpha_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                WasteDensityAlpha_DB = value1;
            }
        }
        private bool WasteDensityAlpha_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
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
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //WasteDensityAlpha property
        #endregion

        //WasteDensityBeta property
        #region  WasteDensityBeta
        public string WasteDensityBeta_DB { get; set; } = null; [NotMapped]        [Attributes.Form_Property("бета-излучающие радионуклиды")]
        public RamAccess<string> WasteDensityBeta
        {
            get
            {
                    var tmp = new RamAccess<string>(WasteDensityBeta_Validation, WasteDensityBeta_DB);
                    tmp.PropertyChanged += WasteDensityBetaValueChanged;
                    return tmp;
            }
            set
            {
                    WasteDensityBeta_DB = value.Value;
                OnPropertyChanged(nameof(WasteDensityBeta));
            }
        }
        private void WasteDensityBetaValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    WasteDensityBeta_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                WasteDensityBeta_DB = value1;
            }
        }
        private bool WasteDensityBeta_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
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
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //WasteDensityBeta property
        #endregion

        //FcpNumber property
        #region  FcpNumber
        public string FcpNumber_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public RamAccess<string> FcpNumber
        {
            get
            {
                    var tmp = new RamAccess<string>(FcpNumber_Validation, FcpNumber_DB);
                    tmp.PropertyChanged += FcpNumberValueChanged;
                    return tmp;
            }
            set
            {
                    FcpNumber_DB = value.Value;
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

        private void FcpNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                FcpNumber_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool FcpNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            return true;
        }
        //FcpNumber property
        #endregion

        #region IExcel
        public int ExcelRow(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);


            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = IndicatorName_DB;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = PlotName_DB;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = PlotKadastrNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = PlotCode_DB;
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = InfectedArea_DB;
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = AvgGammaRaysDosePower_DB;
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = MaxGammaRaysDosePower_DB;
            worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value =WasteDensityAlpha_DB;
            worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = WasteDensityBeta_DB;
            worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = FcpNumber_DB;
            return 10;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = Form2.ExcelHeader(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(IndicatorName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(PlotName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(PlotKadastrNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(PlotCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(InfectedArea)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(AvgGammaRaysDosePower)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(MaxGammaRaysDosePower)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(WasteDensityAlpha)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(WasteDensityBeta)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(FcpNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            return 10;
        }
        #endregion
    }
}
