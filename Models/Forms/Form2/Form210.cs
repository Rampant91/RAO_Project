using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            return false;
        }

        //IndicatorName property
#region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Наименование показателя")]
        public RamAccess<string> IndicatorName
        {
            get => new RamAccess<string>(IndicatorName_Validation, _DB);
            set
            {
                IndicatorName_DB = value.Value;
                OnPropertyChanged(nameof(IndicatorName));
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
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Наименование участка")]
        public RamAccess<string> PlotName
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(PlotName_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    PlotName_DB = value.Value;
                }
                OnPropertyChanged(nameof(PlotName));
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
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Кадастровый номер участка")]
        public RamAccess<string> PlotKadastrNumber
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(PlotKadastrNumber_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    PlotKadastrNumber_DB = value.Value;
                }
                OnPropertyChanged(nameof(PlotKadastrNumber));
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
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Код участка")]
        public RamAccess<string> PlotCode
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(PlotCode_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    PlotCode_DB = value.Value;
                }
                OnPropertyChanged(nameof(PlotCode));
            }
        }
        //6 symbols code
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
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Площадь загрязненной территории, кв. м")]
        public RamAccess<int?> InfectedArea
        {
            get
            {

                {
                    var tmp = new RamAccess<int?>(InfectedArea_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    InfectedArea_DB = value.Value;
                }
                OnPropertyChanged(nameof(InfectedArea));
            }
        }

        private bool InfectedArea_Validation(RamAccess<int?> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //InfectedArea property
        #endregion

        //AvgGammaRaysDosePower property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Средняя мощность дозы гамма-излучения, мкЗв/час")]
        public RamAccess<double?> AvgGammaRaysDosePower
        {
            get
            {

                {
                    var tmp = new RamAccess<double?>(AvgGammaRaysDosePower_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    AvgGammaRaysDosePower_DB = value.Value;
                }
                OnPropertyChanged(nameof(AvgGammaRaysDosePower));
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
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Максимальная мощность дозы гамма-излучения, мкЗв/час")]
        public RamAccess<double?> MaxGammaRaysDosePower
        {
            get
            {

                {
                    var tmp = new RamAccess<double?>(MaxGammaRaysDosePower_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    MaxGammaRaysDosePower_DB = value.Value;
                }
                OnPropertyChanged(nameof(MaxGammaRaysDosePower));
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
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Плотность загрязнения альфа-излучающими радионуклидами (средняя), Бк/кв. м")]
        public RamAccess<double?> WasteDensityAlpha
        {
            get
            {

                {
                    var tmp = new RamAccess<double?>(WasteDensityAlpha_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    WasteDensityAlpha_DB = value.Value;
                }
                OnPropertyChanged(nameof(WasteDensityAlpha));
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
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Плотность загрязнения бета-излучающими радионуклидами (средняя), Бк/кв. м")]
        public RamAccess<double?> WasteDensityBeta
        {
            get
            {

                {
                    var tmp = new RamAccess<double?>(WasteDensityBeta_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    WasteDensityBeta_DB = value.Value;
                }
                OnPropertyChanged(nameof(WasteDensityBeta));
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
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public RamAccess<string> FcpNumber
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(FcpNumber_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    FcpNumber_DB = value.Value;
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

        private bool FcpNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors(); return true;
        }
        //FcpNumber property
        #endregion
    }
}
