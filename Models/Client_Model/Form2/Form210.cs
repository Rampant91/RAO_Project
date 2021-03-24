using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 2.10: Территории, загрязненные радионуклидами")]
    public class Form210 : Form2
    {
        public override string FormNum { get { return "2.10"; } }
        public override void Object_Validation()
        {

        }
        public override int NumberOfFields { get; } = 12;

        private string _indicatorName = "";
        [Attributes.FormVisual("Наименование показателя")]
        public string IndicatorName
        {
            get { return _indicatorName; }
            set
            {
                _indicatorName = value;
                OnPropertyChanged("IndicatorName");
            }
        }

        private string _plotName = "";
        [Attributes.FormVisual("Наименование участка")]
        public string PlotName
        {
            get { return _plotName; }
            set
            {
                _plotName = value;
                OnPropertyChanged("PlotName");
            }
        }

        private string _plotKadastrNumber = "";
        private void PlotKadastrNumber_Validation(string value)//TODO
        {

        }

        [Attributes.FormVisual("Кадастровый номер участка")]
        public string PlotKadastrNumber
        {
            get { return _plotKadastrNumber; }
            set
            {
                _plotKadastrNumber = value;
                PlotKadastrNumber_Validation(value);
                OnPropertyChanged("PlotKadastrNumber");
            }
        }

        private string _plotCode = ""; //6 symbols code
        private void PlotCode_Validation(string value)//TODO
        {

        }

        [Attributes.FormVisual("Код участка")]
        public string PlotCode
        {
            get { return _plotCode; }
            set
            {
                _plotCode = value;
                PlotCode_Validation(value);
                OnPropertyChanged("PlotCode");
            }
        }

        private int _infectedArea = -1;
        private void InfectedArea_Validation(int value)//TODO
        {

        }

        [Attributes.FormVisual("Площадь загрязненной территории, кв. м")]
        public int InfectedArea
        {
            get { return _infectedArea; }
            set
            {
                _infectedArea = value;
                InfectedArea_Validation(value);
                OnPropertyChanged("InfectedArea");
            }
        }

        private double _avgGammaRaysDosePower = -1;
        private void AvgGammaRaysDosePower_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("Средняя мощность дозы гамма-излучения, мкЗв/час")]
        public double AvgGammaRaysDosePower
        {
            get { return _avgGammaRaysDosePower; }
            set
            {
                _avgGammaRaysDosePower = value;
                AvgGammaRaysDosePower_Validation(value);
                OnPropertyChanged("AvgGammaRaysDosePower");
            }
        }

        private double _maxGammaRaysDosePower = -1;
        private void MaxGammaRaysDosePower_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("Максимальная мощность дозы гамма-излучения, мкЗв/час")]
        public double MaxGammaRaysDosePower
        {
            get { return _maxGammaRaysDosePower; }
            set
            {
                _maxGammaRaysDosePower = value;
                MaxGammaRaysDosePower_Validation(value);
                OnPropertyChanged("MaxGammaRaysDosePower");
            }
        }

        private double _wasteDensityAlpha = -1;
        private void WasteDensityAlpha_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("Средняя плотность загрязнения альфа-излучающими радионуклидами, Бк/кв. м")]
        public double WasteDensityAlpha
        {
            get { return _wasteDensityAlpha; }
            set
            {
                _wasteDensityAlpha = value;
                WasteDensityAlpha_Validation(value);
                OnPropertyChanged("WasteDensityAlpha");
            }
        }

        private double _wasteDensityBeta = -1;
        private void WasteDensityBeta_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("Средняя плотность загрязнения бета-излучающими радионуклидами, Бк/кв. м")]
        public double WasteDensityBeta
        {
            get { return _wasteDensityBeta; }
            set
            {
                _wasteDensityBeta = value;
                WasteDensityBeta_Validation(value);
                OnPropertyChanged("WasteDensityBeta");
            }
        }

        //FcpNumber property
        [Attributes.FormVisual("Номер мероприятия ФЦП")]
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
