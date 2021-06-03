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
            FormNum = "210";
            NumberOfFields = 12;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //IndicatorName property
        [Attributes.Form_Property("Наименование показателя")]
        public IDataAccess<string> IndicatorName
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

                private void IndicatorName_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //IndicatorName property

        //PlotName property
        [Attributes.Form_Property("Наименование участка")]
        public IDataAccess<string> PlotName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PlotName));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PlotName), _PlotName_Not_Valid);
                }
                OnPropertyChanged(nameof(PlotName));
            }
        }

                private void PlotName_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //PlotName property

        //PlotKadastrNumber property
        [Attributes.Form_Property("Кадастровый номер участка")]
        public IDataAccess<string> PlotKadastrNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PlotKadastrNumber));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PlotKadastrNumber), _PlotKadastrNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PlotKadastrNumber));
            }
        }

                private void PlotKadastrNumber_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //PlotKadastrNumber property

        //PlotCode property
        [Attributes.Form_Property("Код участка")]
        public IDataAccess<string> PlotCode
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PlotCode));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PlotCode), _PlotCode_Not_Valid);
                }
                OnPropertyChanged(nameof(PlotCode));
            }
        }
        //6 symbols code
                private void PlotCode_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //PlotCode property

        //InfectedArea property
        [Attributes.Form_Property("Площадь загрязненной территории, кв. м")]
        public int InfectedArea
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(InfectedArea));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(InfectedArea), _InfectedArea_Not_Valid);
                }
                OnPropertyChanged(nameof(InfectedArea));
            }
        }

                private void InfectedArea_Validation(int value)//TODO
        {
            value.ClearErrors();
        }
        //InfectedArea property

        //AvgGammaRaysDosePower property
        [Attributes.Form_Property("Средняя мощность дозы гамма-излучения, мкЗв/час")]
        public double AvgGammaRaysDosePower
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(AvgGammaRaysDosePower));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(AvgGammaRaysDosePower), _AvgGammaRaysDosePower_Not_Valid);
                }
                OnPropertyChanged(nameof(AvgGammaRaysDosePower));
            }
        }

                private void AvgGammaRaysDosePower_Validation(double value)//TODO
        {
            value.ClearErrors();
        }
        //AvgGammaRaysDosePower property

        //MaxGammaRaysDosePower property
        [Attributes.Form_Property("Максимальная мощность дозы гамма-излучения, мкЗв/час")]
        public double MaxGammaRaysDosePower
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MaxGammaRaysDosePower));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(MaxGammaRaysDosePower), _MaxGammaRaysDosePower_Not_Valid);
                }
                OnPropertyChanged(nameof(MaxGammaRaysDosePower));
            }
        }

                private void MaxGammaRaysDosePower_Validation(double value)//TODO
        {
            value.ClearErrors();
        }
        //MaxGammaRaysDosePower property

        //WasteDensityAlpha property
        [Attributes.Form_Property("Средняя плотность загрязнения альфа-излучающими радионуклидами, Бк/кв. м")]
        public double WasteDensityAlpha
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(WasteDensityAlpha));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(WasteDensityAlpha), _WasteDensityAlpha_Not_Valid);
                }
                OnPropertyChanged(nameof(WasteDensityAlpha));
            }
        }

                private void WasteDensityAlpha_Validation(double value)//TODO
        {
            value.ClearErrors();
        }
        //WasteDensityAlpha property

        //WasteDensityBeta property
        [Attributes.Form_Property("Средняя плотность загрязнения бета-излучающими радионуклидами, Бк/кв. м")]
        public double WasteDensityBeta
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(WasteDensityBeta));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(WasteDensityBeta), _WasteDensityBeta_Not_Valid);
                }
                OnPropertyChanged(nameof(WasteDensityBeta));
            }
        }

                private void WasteDensityBeta_Validation(double value)//TODO
        {
            value.ClearErrors();
        }
        //WasteDensityBeta property

        //FcpNumber property
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public IDataAccess<string> FcpNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FcpNumber));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(FcpNumber), _FcpNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

                private void FcpNumber_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //FcpNumber property
    }
}
