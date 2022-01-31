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
using Models.Collections;

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
        public string IndicatorName_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("null-2","Наименование показателя","2")]
        public RamAccess<string> IndicatorName
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(IndicatorName)))
                {
                    ((RamAccess<string>)Dictionary[nameof(IndicatorName)]).Value = IndicatorName_DB;
                    return (RamAccess<string>)Dictionary[nameof(IndicatorName)];
                }
                else
                {
                    var rm = new RamAccess<string>(IndicatorName_Validation, IndicatorName_DB);
                    rm.PropertyChanged += IndicatorNameValueChanged;
                    Dictionary.Add(nameof(IndicatorName), rm);
                    return (RamAccess<string>)Dictionary[nameof(IndicatorName)];
                }
            }
            set
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
        public string PlotName_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("null-3","Наименование участка","3")]
        public RamAccess<string> PlotName
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PlotName)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PlotName)]).Value = PlotName_DB;
                    return (RamAccess<string>)Dictionary[nameof(PlotName)];
                }
                else
                {
                    var rm = new RamAccess<string>(PlotName_Validation, PlotName_DB);
                    rm.PropertyChanged += PlotNameValueChanged;
                    Dictionary.Add(nameof(PlotName), rm);
                    return (RamAccess<string>)Dictionary[nameof(PlotName)];
                }
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
        public string PlotKadastrNumber_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("null-4","Кадастровый номер участка","4")]
        public RamAccess<string> PlotKadastrNumber
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PlotKadastrNumber)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PlotKadastrNumber)]).Value = PlotKadastrNumber_DB;
                    return (RamAccess<string>)Dictionary[nameof(PlotKadastrNumber)];
                }
                else
                {
                    var rm = new RamAccess<string>(PlotKadastrNumber_Validation, PlotKadastrNumber_DB);
                    rm.PropertyChanged += PlotKadastrNumberValueChanged;
                    Dictionary.Add(nameof(PlotKadastrNumber), rm);
                    return (RamAccess<string>)Dictionary[nameof(PlotKadastrNumber)];
                }
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
        public string PlotCode_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("null-5","Код участка","5")]
        public RamAccess<string> PlotCode
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PlotCode)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PlotCode)]).Value = PlotCode_DB;
                    return (RamAccess<string>)Dictionary[nameof(PlotCode)];
                }
                else
                {
                    var rm = new RamAccess<string>(PlotCode_Validation, PlotCode_DB);
                    rm.PropertyChanged += PlotCodeValueChanged;
                    Dictionary.Add(nameof(PlotCode), rm);
                    return (RamAccess<string>)Dictionary[nameof(PlotCode)];
                }
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
        public string InfectedArea_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property("null-6","Площадь загрязненной территории, кв. м","6")]
        public RamAccess<string> InfectedArea
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(InfectedArea)))
                {
                    ((RamAccess<string>)Dictionary[nameof(InfectedArea)]).Value = InfectedArea_DB;
                    return (RamAccess<string>)Dictionary[nameof(InfectedArea)];
                }
                else
                {
                    var rm = new RamAccess<string>(InfectedArea_Validation, InfectedArea_DB);
                    rm.PropertyChanged += InfectedAreaValueChanged;
                    Dictionary.Add(nameof(InfectedArea), rm);
                    return (RamAccess<string>)Dictionary[nameof(InfectedArea)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        InfectedArea_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
        public string AvgGammaRaysDosePower_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property("Мощность дозы гамма-излучения, мкЗв/час", "средняя","7")]
        public RamAccess<string> AvgGammaRaysDosePower
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(AvgGammaRaysDosePower)))
                {
                    ((RamAccess<string>)Dictionary[nameof(AvgGammaRaysDosePower)]).Value = AvgGammaRaysDosePower_DB;
                    return (RamAccess<string>)Dictionary[nameof(AvgGammaRaysDosePower)];
                }
                else
                {
                    var rm = new RamAccess<string>(AvgGammaRaysDosePower_Validation, AvgGammaRaysDosePower_DB);
                    rm.PropertyChanged += AvgGammaRaysDosePowerValueChanged;
                    Dictionary.Add(nameof(AvgGammaRaysDosePower), rm);
                    return (RamAccess<string>)Dictionary[nameof(AvgGammaRaysDosePower)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        AvgGammaRaysDosePower_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
        public string MaxGammaRaysDosePower_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property("Мощность дозы гамма-излучения, мкЗв/час", "максимальная","8")]
        public RamAccess<string> MaxGammaRaysDosePower
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MaxGammaRaysDosePower)))
                {
                    ((RamAccess<string>)Dictionary[nameof(MaxGammaRaysDosePower)]).Value = MaxGammaRaysDosePower_DB;
                    return (RamAccess<string>)Dictionary[nameof(MaxGammaRaysDosePower)];
                }
                else
                {
                    var rm = new RamAccess<string>(MaxGammaRaysDosePower_Validation, MaxGammaRaysDosePower_DB);
                    rm.PropertyChanged += MaxGammaRaysDosePowerValueChanged;
                    Dictionary.Add(nameof(MaxGammaRaysDosePower), rm);
                    return (RamAccess<string>)Dictionary[nameof(MaxGammaRaysDosePower)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        MaxGammaRaysDosePower_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
        public string WasteDensityAlpha_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property("Плотность загрязнения (средняя), Бк/кв.м", "альфа-излучающие радионуклиды","9")]
        public RamAccess<string> WasteDensityAlpha
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(WasteDensityAlpha)))
                {
                    ((RamAccess<string>)Dictionary[nameof(WasteDensityAlpha)]).Value = WasteDensityAlpha_DB;
                    return (RamAccess<string>)Dictionary[nameof(WasteDensityAlpha)];
                }
                else
                {
                    var rm = new RamAccess<string>(WasteDensityAlpha_Validation, WasteDensityAlpha_DB);
                    rm.PropertyChanged += WasteDensityAlphaValueChanged;
                    Dictionary.Add(nameof(WasteDensityAlpha), rm);
                    return (RamAccess<string>)Dictionary[nameof(WasteDensityAlpha)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        WasteDensityAlpha_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
            if (value.Value.Equals("-"))
            {
                return true;
            }
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
        public string WasteDensityBeta_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property("Плотность загрязнения (средняя), Бк/кв.м", "бета-излучающие радионуклиды","10")]
        public RamAccess<string> WasteDensityBeta
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(WasteDensityBeta)))
                {
                    ((RamAccess<string>)Dictionary[nameof(WasteDensityBeta)]).Value = WasteDensityBeta_DB;
                    return (RamAccess<string>)Dictionary[nameof(WasteDensityBeta)];
                }
                else
                {
                    var rm = new RamAccess<string>(WasteDensityBeta_Validation, WasteDensityBeta_DB);
                    rm.PropertyChanged += WasteDensityBetaValueChanged;
                    Dictionary.Add(nameof(WasteDensityBeta), rm);
                    return (RamAccess<string>)Dictionary[nameof(WasteDensityBeta)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        WasteDensityBeta_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
            if (value.Value.Equals("-"))
            {
                return true;
            }
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
        public string FcpNumber_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("null-7","Номер мероприятия ФЦП","11")]
        public RamAccess<string> FcpNumber
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(FcpNumber)))
                {
                    ((RamAccess<string>)Dictionary[nameof(FcpNumber)]).Value = FcpNumber_DB;
                    return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
                }
                else
                {
                    var rm = new RamAccess<string>(FcpNumber_Validation, FcpNumber_DB);
                    rm.PropertyChanged += FcpNumberValueChanged;
                    Dictionary.Add(nameof(FcpNumber), rm);
                    return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
                }
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

           worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(IndicatorName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(PlotName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(PlotKadastrNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(PlotCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(InfectedArea)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(AvgGammaRaysDosePower)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(MaxGammaRaysDosePower)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(WasteDensityAlpha)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(WasteDensityBeta)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form210,Models").GetProperty(nameof(FcpNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            return 10;
        }
        #endregion
        #region IDataGridColumn
        public override DataGridColumns GetColumnStructure(string param = "")
        {
            #region NumberInOrder (1)
            DataGridColumns NumberInOrderR = ((Attributes.Form_PropertyAttribute)typeof(Form).GetProperty(nameof(Form.NumberInOrder)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            NumberInOrderR.SetSizeColToAllLevels(88);
            NumberInOrderR.Binding = nameof(Form.NumberInOrder);
            #endregion
            #region IndicatorName (2)
            DataGridColumns IndicatorNameR = ((Attributes.Form_PropertyAttribute)typeof(Form210).GetProperty(nameof(Form210.IndicatorName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            IndicatorNameR.SetSizeColToAllLevels(163);
            IndicatorNameR.Binding = nameof(Form210.IndicatorName);
            NumberInOrderR += IndicatorNameR;
            #endregion
            #region PlotName (3)
            DataGridColumns PlotNameR = ((Attributes.Form_PropertyAttribute)typeof(Form210).GetProperty(nameof(Form210.PlotName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            PlotNameR.SetSizeColToAllLevels(163);
            PlotNameR.Binding = nameof(Form210.PlotName);
            NumberInOrderR += PlotNameR;
            #endregion
            #region PlotKadastrNumber (4)
            DataGridColumns PlotKadastrNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form210).GetProperty(nameof(Form210.PlotKadastrNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            PlotKadastrNumberR.SetSizeColToAllLevels(163);
            PlotKadastrNumberR.Binding = nameof(Form210.PlotKadastrNumber);
            NumberInOrderR += PlotKadastrNumberR;
            #endregion
            #region PlotCode (5)
            DataGridColumns PlotCodeR = ((Attributes.Form_PropertyAttribute)typeof(Form210).GetProperty(nameof(Form210.PlotCode)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            PlotCodeR.SetSizeColToAllLevels(88);
            PlotCodeR.Binding = nameof(Form210.PlotCode);
            NumberInOrderR += PlotCodeR;
            #endregion
            #region InfectedArea (6)
            DataGridColumns InfectedAreaR = ((Attributes.Form_PropertyAttribute)typeof(Form210).GetProperty(nameof(Form210.InfectedArea)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            InfectedAreaR.SetSizeColToAllLevels(238);
            InfectedAreaR.Binding = nameof(Form210.InfectedArea);
            NumberInOrderR += InfectedAreaR;
            #endregion
            #region AvgGammaRaysDosePower (7)
            DataGridColumns AvgGammaRaysDosePowerR = ((Attributes.Form_PropertyAttribute)typeof(Form210).GetProperty(nameof(Form210.AvgGammaRaysDosePower)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            AvgGammaRaysDosePowerR.SetSizeColToAllLevels(103);
            AvgGammaRaysDosePowerR.Binding = nameof(Form210.AvgGammaRaysDosePower);
            NumberInOrderR += AvgGammaRaysDosePowerR;
            #endregion
            #region MaxGammaRaysDosePower (8)
            DataGridColumns MaxGammaRaysDosePowerR = ((Attributes.Form_PropertyAttribute)typeof(Form210).GetProperty(nameof(Form210.MaxGammaRaysDosePower)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            MaxGammaRaysDosePowerR.SetSizeColToAllLevels(133);
            MaxGammaRaysDosePowerR.Binding = nameof(Form210.MaxGammaRaysDosePower);
            NumberInOrderR += MaxGammaRaysDosePowerR;
            #endregion
            #region WasteDensityAlpha (9)
            DataGridColumns WasteDensityAlphaR = ((Attributes.Form_PropertyAttribute)typeof(Form210).GetProperty(nameof(Form210.WasteDensityAlpha)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            WasteDensityAlphaR.SetSizeColToAllLevels(193);
            WasteDensityAlphaR.Binding = nameof(Form210.WasteDensityAlpha);
            NumberInOrderR += WasteDensityAlphaR;
            #endregion
            #region WasteDensityBeta (10)
            DataGridColumns WasteDensityBetaR = ((Attributes.Form_PropertyAttribute)typeof(Form210).GetProperty(nameof(Form210.WasteDensityBeta)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            WasteDensityBetaR.SetSizeColToAllLevels(193);
            WasteDensityBetaR.Binding = nameof(Form210.WasteDensityBeta);
            NumberInOrderR += WasteDensityBetaR;
            #endregion
            #region FcpNumber (11)
            DataGridColumns FcpNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form210).GetProperty(nameof(Form210.FcpNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            FcpNumberR.SetSizeColToAllLevels(163);
            FcpNumberR.Binding = nameof(Form210.FcpNumber);
            NumberInOrderR += FcpNumberR;
            #endregion
            return NumberInOrderR;
        }
        #endregion
    }
}
