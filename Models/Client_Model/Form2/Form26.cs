using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 2.6: Контроль загрязнения подземных вод РВ")]
    public class Form26 : Form2
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 11;

        private byte _correctionNumber = 255;

        //Data Validation
        private void CorrectionNumber_Validation(byte value)//TODO
        {
            ClearErrors(nameof(CorrectionNumber));
            //Пример
            if (value < 10)
                AddError(nameof(CorrectionNumber), "Значение должно быть больше 10.");
        }
        //Data Validation

        [Attributes.FormVisual("Номер корректировки")]
        public byte CorrectionNumber
        {
            get { return _correctionNumber; }
            set
            {
                _correctionNumber = value;
                CorrectionNumber_Validation(value);
                OnPropertyChanged("CorrectionNumber");
            }
        }

        private int _sourcesQuantity = -1;
        [Attributes.FormVisual("Количество источников, шт.")]
        public int SourcesQuantity
        {
            get { return _sourcesQuantity; }
            set
            {
                _sourcesQuantity = value;
                OnPropertyChanged("SourcesQuantity");
            }
        }

        private int _numberInOrder = -1;
        [Attributes.FormVisual("№ п/п")]
        public int NumberInOrder
        {
            get { return _numberInOrder; }
            set
            {
                _numberInOrder = value;
                OnPropertyChanged("NumberInOrder");
            }
        }

        private string _observedSourceNumber = "";
        [Attributes.FormVisual("Номер наблюдательной скважины")]
        public string ObservedSourceNumber
        {
            get { return _observedSourceNumber; }
            set
            {
                _observedSourceNumber = value;
                OnPropertyChanged("ObservedSourceNumber");
            }
        }

        private string _controlledAreaName = "";
        [Attributes.FormVisual("Наименование зоны контроля")]
        public string ControlledAreaName
        {
            get { return _controlledAreaName; }
            set
            {
                _controlledAreaName = value;
                OnPropertyChanged("ControlledAreaName");
            }
        }

        private string _supposedWasteSource = "";
        [Attributes.FormVisual("Предполагаемый источник поступления радиоактивных веществ")]
        public string SupposedWasteSource
        {
            get { return _supposedWasteSource; }
            set
            {
                _supposedWasteSource = value;
                OnPropertyChanged("SupposedWasteSource");
            }
        }

        private int _distanceToWasteSource = -1;
        [Attributes.FormVisual("Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м")]
        public int DistanceToWasteSource
        {
            get { return _distanceToWasteSource; }
            set
            {
                _distanceToWasteSource = value;
                OnPropertyChanged("DistanceToWasteSource");
            }
        }

        private int _testDepth = -1;
        [Attributes.FormVisual("Глубина отбора проб, м")]
        public int TestDepth
        {
            get { return _testDepth; }
            set
            {
                _testDepth = value;
                OnPropertyChanged("TestDepth");
            }
        }

        private int _testDepthNote = -1;
        public int TestDepthNote
        {
            get { return _testDepthNote; }
            set
            {
                _testDepthNote = value;
                OnPropertyChanged("TestDepthNote");
            }
        }

        private string _radionuclidName = "";
        [Attributes.FormVisual("Наименование радионуклида")]
        public string RadionuclidName
        {
            get { return _radionuclidName; }
            set
            {
                _radionuclidName = value;
                OnPropertyChanged("RadionuclidName");
            }
        }

        private double _averageYearConcentration = -1;
        [Attributes.FormVisual("Среднегодовое содержание радионуклида, Бк/кг")]
        public double AverageYearConcentration
        {
            get { return _averageYearConcentration; }
            set
            {
                _averageYearConcentration = value;
                OnPropertyChanged("AverageYearConcentration");
            }
        }
    }
}
