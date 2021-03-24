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
        public override string FormNum { get { return "2.6"; } }
        public override void Object_Validation()
        {

        }
        public override int NumberOfFields { get; } = 11;

        //SourcesQuantity property
        [Attributes.FormVisual("Количество источников, шт.")]
        public int SourcesQuantity
        {
            get
            {
                if (GetErrors(nameof(SourcesQuantity)) != null)
                {
                    return (int)_SourcesQuantity.Get();
                }
                else
                {
                    return _SourcesQuantity_Not_Valid;
                }
            }
            set
            {
                _SourcesQuantity_Not_Valid = value;
                if (GetErrors(nameof(SourcesQuantity)) != null)
                {
                    _SourcesQuantity.Set(_SourcesQuantity_Not_Valid);
                }
                OnPropertyChanged(nameof(SourcesQuantity));
            }
        }
        private IDataLoadEngine _SourcesQuantity;  // positive int.
        private int _SourcesQuantity_Not_Valid = -1;
        private void SourcesQuantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(SourcesQuantity));
            if (value <= 0)
                AddError(nameof(SourcesQuantity), "Недопустимое значение");
        }
        //SourcesQuantity property

        //ObservedSourceNumber property
        [Attributes.FormVisual("Номер наблюдательной скважины")]
        public string ObservedSourceNumber
        {
            get
            {
                if (GetErrors(nameof(ObservedSourceNumber)) != null)
                {
                    return (string)_ObservedSourceNumber.Get();
                }
                else
                {
                    return _ObservedSourceNumber_Not_Valid;
                }
            }
            set
            {
                _ObservedSourceNumber_Not_Valid = value;
                if (GetErrors(nameof(ObservedSourceNumber)) != null)
                {
                    _ObservedSourceNumber.Set(_ObservedSourceNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(ObservedSourceNumber));
            }
        }
        private IDataLoadEngine _ObservedSourceNumber;//If change this change validation
        private string _ObservedSourceNumber_Not_Valid = "";
        private void ObservedSourceNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(ObservedSourceNumber));
        }
        //ObservedSourceNumber property

        //ControlledAreaName property
        [Attributes.FormVisual("Наименование зоны контроля")]
        public string ControlledAreaName
        {
            get
            {
                if (GetErrors(nameof(ControlledAreaName)) != null)
                {
                    return (string)_ControlledAreaName.Get();
                }
                else
                {
                    return _ControlledAreaName_Not_Valid;
                }
            }
            set
            {
                _ControlledAreaName_Not_Valid = value;
                if (GetErrors(nameof(ControlledAreaName)) != null)
                {
                    _ControlledAreaName.Set(_ControlledAreaName_Not_Valid);
                }
                OnPropertyChanged(nameof(ControlledAreaName));
            }
        }
        private IDataLoadEngine _ControlledAreaName;//If change this change validation
        private string _ControlledAreaName_Not_Valid = "";
        private void ControlledAreaName_Validation(string value)//Ready
        {
            ClearErrors(nameof(ControlledAreaName));
        }
        //ControlledAreaName property

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
