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
            Regex a = new Regex("^[0-9]{7}$");
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
        public double? InfectedArea_DB { get; set; } = null; [NotMapped]        [Attributes.Form_Property("Площадь загрязненной территории, кв. м")]
        public RamAccess<double?> InfectedArea
        {
            get
            {
                    var tmp = new RamAccess<double?>(InfectedArea_Validation, InfectedArea_DB);
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
                InfectedArea_DB = ((RamAccess<double?>)Value).Value;
}
}
private bool InfectedArea_Validation(RamAccess<double?> value)//TODO
        {
            value.ClearErrors();
            if (value.Value==null)
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
        //InfectedArea property
        #endregion

        //AvgGammaRaysDosePower property
        #region  AvgGammaRaysDosePower
        public double? AvgGammaRaysDosePower_DB { get; set; } = null; [NotMapped]        [Attributes.Form_Property("Средняя мощность дозы гамма-излучения, мкЗв/час")]
        public RamAccess<double?> AvgGammaRaysDosePower
        {
            get
            {
                    var tmp = new RamAccess<double?>(AvgGammaRaysDosePower_Validation, AvgGammaRaysDosePower_DB);
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
                AvgGammaRaysDosePower_DB = ((RamAccess<double?>)Value).Value;
}
}
private bool AvgGammaRaysDosePower_Validation(RamAccess<double?> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //AvgGammaRaysDosePower property
        #endregion

        //MaxGammaRaysDosePower property
        #region  MaxGammaRaysDosePower
        public double? MaxGammaRaysDosePower_DB { get; set; } = null; [NotMapped]        [Attributes.Form_Property("Максимальная мощность дозы гамма-излучения, мкЗв/час")]
        public RamAccess<double?> MaxGammaRaysDosePower
        {
            get
            {
                    var tmp = new RamAccess<double?>(MaxGammaRaysDosePower_Validation, MaxGammaRaysDosePower_DB);
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
                MaxGammaRaysDosePower_DB = ((RamAccess<double?>)Value).Value;
}
}
private bool MaxGammaRaysDosePower_Validation(RamAccess<double?> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //MaxGammaRaysDosePower property
        #endregion

        //WasteDensityAlpha property
        #region  WasteDensityAlpha
        public double? WasteDensityAlpha_DB { get; set; } = null; [NotMapped]        [Attributes.Form_Property("Плотность загрязнения альфа-излучающими радионуклидами (средняя), Бк/кв. м")]
        public RamAccess<double?> WasteDensityAlpha
        {
            get
            {
                    var tmp = new RamAccess<double?>(WasteDensityAlpha_Validation, WasteDensityAlpha_DB);
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
                WasteDensityAlpha_DB = ((RamAccess<double?>)Value).Value;
}
}
private bool WasteDensityAlpha_Validation(RamAccess<double?> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //WasteDensityAlpha property
        #endregion

        //WasteDensityBeta property
        #region  WasteDensityBeta
        public double? WasteDensityBeta_DB { get; set; } = null; [NotMapped]        [Attributes.Form_Property("Плотность загрязнения бета-излучающими радионуклидами (средняя), Бк/кв. м")]
        public RamAccess<double?> WasteDensityBeta
        {
            get
            {
                    var tmp = new RamAccess<double?>(WasteDensityBeta_Validation, WasteDensityBeta_DB);
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
                WasteDensityBeta_DB = ((RamAccess<double?>)Value).Value;
}
}
private bool WasteDensityBeta_Validation(RamAccess<double?> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
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
            value.ClearErrors(); return true;
        }
        //FcpNumber property
        #endregion

        #region IExcel
        public void ExcelRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelRow(worksheet, Row);
            worksheet.Cells[Row, 2].Value = IndicatorName_DB;
            worksheet.Cells[Row, 3].Value = PlotName_DB;
            worksheet.Cells[Row, 4].Value = PlotKadastrNumber_DB;
            worksheet.Cells[Row, 5].Value = PlotCode_DB;
            worksheet.Cells[Row, 6].Value = InfectedArea_DB;
            worksheet.Cells[Row, 7].Value = AvgGammaRaysDosePower_DB;
            worksheet.Cells[Row, 8].Value = MaxGammaRaysDosePower_DB;
            worksheet.Cells[Row, 9].Value =WasteDensityAlpha_DB;
            worksheet.Cells[Row, 10].Value = WasteDensityBeta_DB;
            worksheet.Cells[Row, 11].Value = FcpNumber_DB;
        }

        public static void ExcelHeader(ExcelWorksheet worksheet)
        {
            Form2.ExcelHeader(worksheet);
            worksheet.Cells[1, 2].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(IndicatorName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 3].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(PlotName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 4].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(PlotKadastrNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 5].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(PlotCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 6].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(InfectedArea)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 7].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(AvgGammaRaysDosePower)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 8].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(MaxGammaRaysDosePower)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 9].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(WasteDensityAlpha)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 10].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(WasteDensityBeta)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 11].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(FcpNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;

        }
        #endregion
    }
}
