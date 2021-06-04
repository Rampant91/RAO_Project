using Models.DataAccess;
using System;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.10: Территории, загрязненные радионуклидами")]
    public class Form210 : Abstracts.Form2
    {
        public Form210() : base()
        {
            FormNum.Value = "210";
            NumberOfFields.Value = 12;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //IndicatorName property
        [Attributes.Form_Property("Наименование показателя")]
        public RamAccess<string> IndicatorName
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(IndicatorName));
            }
            set
            {
                    _dataAccess.Set(nameof(IndicatorName), value);
                OnPropertyChanged(nameof(IndicatorName));
            }
        }

                private void IndicatorName_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //IndicatorName property

        //PlotName property
        [Attributes.Form_Property("Наименование участка")]
        public RamAccess<string> PlotName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PlotName));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PlotName), value);
                }
                OnPropertyChanged(nameof(PlotName));
            }
        }

                private void PlotName_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //PlotName property

        //PlotKadastrNumber property
        [Attributes.Form_Property("Кадастровый номер участка")]
        public RamAccess<string> PlotKadastrNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PlotKadastrNumber));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PlotKadastrNumber), value);
                }
                OnPropertyChanged(nameof(PlotKadastrNumber));
            }
        }

                private void PlotKadastrNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //PlotKadastrNumber property

        //PlotCode property
        [Attributes.Form_Property("Код участка")]
        public RamAccess<string> PlotCode
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PlotCode));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PlotCode), value);
                }
                OnPropertyChanged(nameof(PlotCode));
            }
        }
        //6 symbols code
                private void PlotCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //PlotCode property

        //InfectedArea property
        [Attributes.Form_Property("Площадь загрязненной территории, кв. м")]
        public RamAccess<int?> InfectedArea
        {
            get
            {
                
                {
                    return _dataAccess.Get<int?>(nameof(InfectedArea));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(InfectedArea), value);
                }
                OnPropertyChanged(nameof(InfectedArea));
            }
        }

                private void InfectedArea_Validation(RamAccess<int?> value)//TODO
        {
            value.ClearErrors();
        }
        //InfectedArea property

        //AvgGammaRaysDosePower property
        [Attributes.Form_Property("Средняя мощность дозы гамма-излучения, мкЗв/час")]
        public RamAccess<double> AvgGammaRaysDosePower
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(AvgGammaRaysDosePower));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(AvgGammaRaysDosePower), value);
                }
                OnPropertyChanged(nameof(AvgGammaRaysDosePower));
            }
        }

                private void AvgGammaRaysDosePower_Validation(RamAccess<double> value)//TODO
        {
            value.ClearErrors();
        }
        //AvgGammaRaysDosePower property

        //MaxGammaRaysDosePower property
        [Attributes.Form_Property("Максимальная мощность дозы гамма-излучения, мкЗв/час")]
        public RamAccess<double> MaxGammaRaysDosePower
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(MaxGammaRaysDosePower));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(MaxGammaRaysDosePower), value);
                }
                OnPropertyChanged(nameof(MaxGammaRaysDosePower));
            }
        }

                private void MaxGammaRaysDosePower_Validation(RamAccess<double> value)//TODO
        {
            value.ClearErrors();
        }
        //MaxGammaRaysDosePower property

        //WasteDensityAlpha property
        [Attributes.Form_Property("Средняя плотность загрязнения альфа-излучающими радионуклидами, Бк/кв. м")]
        public RamAccess<double> WasteDensityAlpha
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(WasteDensityAlpha));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(WasteDensityAlpha), value);
                }
                OnPropertyChanged(nameof(WasteDensityAlpha));
            }
        }

                private void WasteDensityAlpha_Validation(RamAccess<double> value)//TODO
        {
            value.ClearErrors();
        }
        //WasteDensityAlpha property

        //WasteDensityBeta property
        [Attributes.Form_Property("Средняя плотность загрязнения бета-излучающими радионуклидами, Бк/кв. м")]
        public RamAccess<double> WasteDensityBeta
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(WasteDensityBeta));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(WasteDensityBeta), value);
                }
                OnPropertyChanged(nameof(WasteDensityBeta));
            }
        }

                private void WasteDensityBeta_Validation(RamAccess<double> value)//TODO
        {
            value.ClearErrors();
        }
        //WasteDensityBeta property

        //FcpNumber property
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public RamAccess<string> FcpNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FcpNumber));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(FcpNumber), value);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

                private void FcpNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //FcpNumber property
    }
}
