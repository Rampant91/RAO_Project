using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.10: Территории, загрязненные радионуклидами")]
    public class Form210 : Abstracts.Form2
    {
        public Form210(int RowID) : base(RowID)
        {
            FormNum = "210";
            NumberOfFields = 12;
        }

        [Attributes.Form_Property("Форма")]
        public override void Object_Validation()
        {

        }

        //IndicatorName property
        [Attributes.Form_Property("Наименование показателя")]
        public string IndicatorName
        {
            get
            {
                if (GetErrors(nameof(IndicatorName)) != null)
                {
                    return (string)_IndicatorName.Get();
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
                    _IndicatorName.Set(_IndicatorName_Not_Valid);
                }
                OnPropertyChanged(nameof(IndicatorName));
            }
        }
        private IDataLoadEngine _IndicatorName;
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
                    return (string)_PlotName.Get();
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
                    _PlotName.Set(_PlotName_Not_Valid);
                }
                OnPropertyChanged(nameof(PlotName));
            }
        }
        private IDataLoadEngine _PlotName;
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
                    return (string)_PlotKadastrNumber.Get();
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
                    _PlotKadastrNumber.Set(_PlotKadastrNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PlotKadastrNumber));
            }
        }
        private IDataLoadEngine _PlotKadastrNumber;
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
                    return (string)_PlotCode.Get();
                }
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
                    _PlotCode.Set(_PlotCode_Not_Valid);
                }
                OnPropertyChanged(nameof(PlotCode));
            }
        }
        private IDataLoadEngine _PlotCode; //6 symbols code
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
                    return (int)_InfectedArea.Get();
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
                    _InfectedArea.Set(_InfectedArea_Not_Valid);
                }
                OnPropertyChanged(nameof(InfectedArea));
            }
        }
        private IDataLoadEngine _InfectedArea;
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
                    return (double)_AvgGammaRaysDosePower.Get();
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
                    _AvgGammaRaysDosePower.Set(_AvgGammaRaysDosePower_Not_Valid);
                }
                OnPropertyChanged(nameof(AvgGammaRaysDosePower));
            }
        }
        private IDataLoadEngine _AvgGammaRaysDosePower;
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
                    return (double)_MaxGammaRaysDosePower.Get();
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
                    _MaxGammaRaysDosePower.Set(_MaxGammaRaysDosePower_Not_Valid);
                }
                OnPropertyChanged(nameof(MaxGammaRaysDosePower));
            }
        }
        private IDataLoadEngine _MaxGammaRaysDosePower;
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
                    return (double)_WasteDensityAlpha.Get();
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
                    _WasteDensityAlpha.Set(_WasteDensityAlpha_Not_Valid);
                }
                OnPropertyChanged(nameof(WasteDensityAlpha));
            }
        }
        private IDataLoadEngine _WasteDensityAlpha;
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
                    return (double)_WasteDensityBeta.Get();
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
                    _WasteDensityBeta.Set(_WasteDensityBeta_Not_Valid);
                }
                OnPropertyChanged(nameof(WasteDensityBeta));
            }
        }
        private IDataLoadEngine _WasteDensityBeta;
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
                    return (string)_FcpNumber.Get();
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
                    _FcpNumber.Set(_FcpNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }
        private IDataLoadEngine _FcpNumber;
        private string _FcpNumber_Not_Valid = "";
        private void FcpNuber_Validation(string value)//TODO
        {
            ClearErrors(nameof(FcpNumber));
        }
        //FcpNumber property
    }
}
