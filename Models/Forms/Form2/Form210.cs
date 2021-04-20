using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DBRealization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.10: Территории, загрязненные радионуклидами")]
    public class Form210 : Abstracts.Form2
    {
        //public static string SQLCommandParams()
        //{
        //    return
        //        Abstracts.Form2.SQLCommandParamsBase() +
        //    nameof(IndicatorName) + SQLconsts.strNotNullDeclaration +
        //    nameof(PlotName) + SQLconsts.strNotNullDeclaration +
        //    nameof(PlotKadastrNumber) + SQLconsts.strNotNullDeclaration +
        //    nameof(PlotCode) + SQLconsts.strNotNullDeclaration +
        //    nameof(InfectedArea) + SQLconsts.intNotNullDeclaration +
        //    nameof(AvgGammaRaysDosePower) + SQLconsts.doubleNotNullDeclaration +
        //    nameof(MaxGammaRaysDosePower) + SQLconsts.doubleNotNullDeclaration +
        //    nameof(WasteDensityAlpha) + SQLconsts.doubleNotNullDeclaration +
        //    nameof(WasteDensityBeta) + SQLconsts.doubleNotNullDeclaration +
        //    nameof(FcpNumber) + " varchar(255) not null";
        //}
        public Form210(IDataAccess Access) : base(Access)
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
        public string IndicatorName
        {
            get
            {
                if (GetErrors(nameof(IndicatorName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(IndicatorName));
                }
                else
                {
                    return _IndicatorName_Not_Valid;
                }
            }
            set
            {
                _IndicatorName_Not_Valid = value;
                if (GetErrors(nameof(IndicatorName)) != null)
                {
                    _dataAccess.Set(nameof(IndicatorName), _IndicatorName_Not_Valid);
                }
                OnPropertyChanged(nameof(IndicatorName));
            }
        }
        
        private string _IndicatorName_Not_Valid = "";
        private void IndicatorName_Validation(string value)//TODO
        {
            ClearErrors(nameof(IndicatorName));
        }
        //IndicatorName property

        //PlotName property
        [Attributes.Form_Property("Наименование участка")]
        public string PlotName
        {
            get
            {
                if (GetErrors(nameof(PlotName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PlotName));
                }
                else
                {
                    return _PlotName_Not_Valid;
                }
            }
            set
            {
                _PlotName_Not_Valid = value;
                if (GetErrors(nameof(PlotName)) != null)
                {
                    _dataAccess.Set(nameof(PlotName), _PlotName_Not_Valid);
                }
                OnPropertyChanged(nameof(PlotName));
            }
        }
        
        private string _PlotName_Not_Valid = "";
        private void PlotName_Validation(string value)//TODO
        {
            ClearErrors(nameof(PlotName));
        }
        //PlotName property

        //PlotKadastrNumber property
        [Attributes.Form_Property("Кадастровый номер участка")]
        public string PlotKadastrNumber
        {
            get
            {
                if (GetErrors(nameof(PlotKadastrNumber)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PlotKadastrNumber));
                }
                else
                {
                    return _PlotKadastrNumber_Not_Valid;
                }
            }
            set
            {
                _PlotKadastrNumber_Not_Valid = value;
                if (GetErrors(nameof(PlotKadastrNumber)) != null)
                {
                    _dataAccess.Set(nameof(PlotKadastrNumber), _PlotKadastrNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PlotKadastrNumber));
            }
        }
        
        private string _PlotKadastrNumber_Not_Valid = "";
        private void PlotKadastrNumber_Validation(string value)//TODO
        {
            ClearErrors(nameof(PlotKadastrNumber));
        }
        //PlotKadastrNumber property

        //PlotCode property
        [Attributes.Form_Property("Код участка")]
        public string PlotCode
        {
            get
            {
                if (GetErrors(nameof(PlotCode)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PlotCode));                }
                else
                {
                    return _PlotCode_Not_Valid;
                }
            }
            set
            {
                _PlotCode_Not_Valid = value;
                if (GetErrors(nameof(PlotCode)) != null)
                {
                    _dataAccess.Set(nameof(PlotCode), _PlotCode_Not_Valid);                }
                OnPropertyChanged(nameof(PlotCode));
            }
        }
         //6 symbols code
        private string _PlotCode_Not_Valid = ""; //6 symbols code
        private void PlotCode_Validation(string value)//TODO
        {
            ClearErrors(nameof(PlotCode));
        }
        //PlotCode property

        //InfectedArea property
        [Attributes.Form_Property("Площадь загрязненной территории, кв. м")]
        public int InfectedArea
        {
            get
            {
                if (GetErrors(nameof(InfectedArea)) != null)
                {
                    return (int)_dataAccess.Get(nameof(InfectedArea));
                }
                else
                {
                    return _InfectedArea_Not_Valid;
                }
            }
            set
            {
                _InfectedArea_Not_Valid = value;
                if (GetErrors(nameof(InfectedArea)) != null)
                {
                    _dataAccess.Set(nameof(InfectedArea), _InfectedArea_Not_Valid);
                }
                OnPropertyChanged(nameof(InfectedArea));
            }
        }
        
        private int _InfectedArea_Not_Valid = -1;
        private void InfectedArea_Validation(int value)//TODO
        {
            ClearErrors(nameof(InfectedArea));
        }
        //InfectedArea property

        //AvgGammaRaysDosePower property
        [Attributes.Form_Property("Средняя мощность дозы гамма-излучения, мкЗв/час")]
        public double AvgGammaRaysDosePower
        {
            get
            {
                if (GetErrors(nameof(AvgGammaRaysDosePower)) != null)
                {
                    return (double)_dataAccess.Get(nameof(AvgGammaRaysDosePower));
                }
                else
                {
                    return _AvgGammaRaysDosePower_Not_Valid;
                }
            }
            set
            {
                _AvgGammaRaysDosePower_Not_Valid = value;
                if (GetErrors(nameof(AvgGammaRaysDosePower)) != null)
                {
                    _dataAccess.Set(nameof(AvgGammaRaysDosePower), _AvgGammaRaysDosePower_Not_Valid);
                }
                OnPropertyChanged(nameof(AvgGammaRaysDosePower));
            }
        }
        
        private double _AvgGammaRaysDosePower_Not_Valid = -1;
        private void AvgGammaRaysDosePower_Validation(double value)//TODO
        {
            ClearErrors(nameof(AvgGammaRaysDosePower));
        }
        //AvgGammaRaysDosePower property

        //MaxGammaRaysDosePower property
        [Attributes.Form_Property("Максимальная мощность дозы гамма-излучения, мкЗв/час")]
        public double MaxGammaRaysDosePower
        {
            get
            {
                if (GetErrors(nameof(MaxGammaRaysDosePower)) != null)
                {
                    return (double)_dataAccess.Get(nameof(MaxGammaRaysDosePower));
                }
                else
                {
                    return _MaxGammaRaysDosePower_Not_Valid;
                }
            }
            set
            {
                _MaxGammaRaysDosePower_Not_Valid = value;
                if (GetErrors(nameof(MaxGammaRaysDosePower)) != null)
                {
                    _dataAccess.Set(nameof(MaxGammaRaysDosePower), _MaxGammaRaysDosePower_Not_Valid);
                }
                OnPropertyChanged(nameof(MaxGammaRaysDosePower));
            }
        }
        
        private double _MaxGammaRaysDosePower_Not_Valid = -1;
        private void MaxGammaRaysDosePower_Validation(double value)//TODO
        {
            ClearErrors(nameof(MaxGammaRaysDosePower));
        }
        //MaxGammaRaysDosePower property

        //WasteDensityAlpha property
        [Attributes.Form_Property("Средняя плотность загрязнения альфа-излучающими радионуклидами, Бк/кв. м")]
        public double WasteDensityAlpha
        {
            get
            {
                if (GetErrors(nameof(WasteDensityAlpha)) != null)
                {
                    return (double)_dataAccess.Get(nameof(WasteDensityAlpha));
                }
                else
                {
                    return _WasteDensityAlpha_Not_Valid;
                }
            }
            set
            {
                _WasteDensityAlpha_Not_Valid = value;
                if (GetErrors(nameof(WasteDensityAlpha)) != null)
                {
                    _dataAccess.Set(nameof(WasteDensityAlpha), _WasteDensityAlpha_Not_Valid);
                }
                OnPropertyChanged(nameof(WasteDensityAlpha));
            }
        }
        
        private double _WasteDensityAlpha_Not_Valid = -1;
        private void WasteDensityAlpha_Validation(double value)//TODO
        {
            ClearErrors(nameof(WasteDensityAlpha));
        }
        //WasteDensityAlpha property

        //WasteDensityBeta property
        [Attributes.Form_Property("Средняя плотность загрязнения бета-излучающими радионуклидами, Бк/кв. м")]
        public double WasteDensityBeta
        {
            get
            {
                if (GetErrors(nameof(WasteDensityBeta)) != null)
                {
                    return (double)_dataAccess.Get(nameof(WasteDensityBeta));
                }
                else
                {
                    return _WasteDensityBeta_Not_Valid;
                }
            }
            set
            {
                _WasteDensityBeta_Not_Valid = value;
                if (GetErrors(nameof(WasteDensityBeta)) != null)
                {
                    _dataAccess.Set(nameof(WasteDensityBeta), _WasteDensityBeta_Not_Valid);
                }
                OnPropertyChanged(nameof(WasteDensityBeta));
            }
        }
        
        private double _WasteDensityBeta_Not_Valid = -1;
        private void WasteDensityBeta_Validation(double value)//TODO
        {
            ClearErrors(nameof(WasteDensityBeta));
        }
        //WasteDensityBeta property

        //FcpNumber property
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public string FcpNumber
        {
            get
            {
                if (GetErrors(nameof(FcpNumber)) != null)
                {
                    return (string)_dataAccess.Get(nameof(FcpNumber));
                }
                else
                {
                    return _FcpNumber_Not_Valid;
                }
            }
            set
            {
                _FcpNumber_Not_Valid = value;
                if (GetErrors(nameof(FcpNumber)) != null)
                {
                    _dataAccess.Set(nameof(FcpNumber), _FcpNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }
        
        private string _FcpNumber_Not_Valid = "";
        private void FcpNuber_Validation(string value)//TODO
        {
            ClearErrors(nameof(FcpNumber));
        }
        //FcpNumber property
    }
}
