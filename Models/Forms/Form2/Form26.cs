using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.6: Контроль загрязнения подземных вод РВ")]
    public class Form26 : Abstracts.Form2
    {
        public Form26(int RowID) : base(RowID)
        {
            FormNum = "26";
            NumberOfFields = 11;
        }

        [Attributes.Form_Property("Форма")]
        public override void Object_Validation()
        {

        }

        //SourcesQuantity property
        [Attributes.Form_Property("Количество источников, шт.")]
        public int SourcesQuantity
        {
            get
            {
                if (GetErrors(nameof(SourcesQuantity)) != null)
                {
                    return (int)_dataAccess.Get(nameof(SourcesQuantity));
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
                    _dataAccess.Set(nameof(SourcesQuantity), _SourcesQuantity_Not_Valid);
                }
                OnPropertyChanged(nameof(SourcesQuantity));
            }
        }
          // positive int.
        private int _SourcesQuantity_Not_Valid = -1;
        private void SourcesQuantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(SourcesQuantity));
            if (value <= 0)
                AddError(nameof(SourcesQuantity), "Недопустимое значение");
        }
        //SourcesQuantity property

        //ObservedSourceNumber property
        [Attributes.Form_Property("Номер наблюдательной скважины")]
        public string ObservedSourceNumber
        {
            get
            {
                if (GetErrors(nameof(ObservedSourceNumber)) != null)
                {
                    return (string)_dataAccess.Get(nameof(ObservedSourceNumber));
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
                    _dataAccess.Set(nameof(ObservedSourceNumber), _ObservedSourceNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(ObservedSourceNumber));
            }
        }
        //If change this change validation
        private string _ObservedSourceNumber_Not_Valid = "";
        private void ObservedSourceNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(ObservedSourceNumber));
        }
        //ObservedSourceNumber property

        //ControlledAreaName property
        [Attributes.Form_Property("Наименование зоны контроля")]
        public string ControlledAreaName
        {
            get
            {
                if (GetErrors(nameof(ControlledAreaName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(ControlledAreaName));
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
                    _dataAccess.Set(nameof(ControlledAreaName), _ControlledAreaName_Not_Valid);
                }
                OnPropertyChanged(nameof(ControlledAreaName));
            }
        }
        //If change this change validation
        private string _ControlledAreaName_Not_Valid = "";
        private void ControlledAreaName_Validation(string value)//Ready
        {
            ClearErrors(nameof(ControlledAreaName));
        }
        //ControlledAreaName property
        
        //SupposedWasteSource property
        [Attributes.Form_Property("Предполагаемый источник поступления радиоактивных веществ")]
        public string SupposedWasteSource
        {
            get
            {
                if (GetErrors(nameof(SupposedWasteSource)) != null)
                {
                    return (string)_dataAccess.Get(nameof(SupposedWasteSource));
                }
                else
                {
                    return _SupposedWasteSource_Not_Valid;
                }
            }
            set
            {
                _SupposedWasteSource_Not_Valid = value;
                if (GetErrors(nameof(SupposedWasteSource)) != null)
                {
                    _dataAccess.Set(nameof(SupposedWasteSource), _SupposedWasteSource_Not_Valid);
                }
                OnPropertyChanged(nameof(SupposedWasteSource));
            }
        }
        
        private string _SupposedWasteSource_Not_Valid = "";
        private void SupposedWasteSource_Validation(string value)//Ready
        {
            ClearErrors(nameof(SupposedWasteSource));
        }
        //SupposedWasteSource property

        //DistanceToWasteSource property
        [Attributes.Form_Property("Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м")]
        public int DistanceToWasteSource
        {
            get
            {
                if (GetErrors(nameof(DistanceToWasteSource)) != null)
                {
                    return (int)_dataAccess.Get(nameof(DistanceToWasteSource));
                }
                else
                {
                    return _DistanceToWasteSource_Not_Valid;
                }
            }
            set
            {
                _DistanceToWasteSource_Not_Valid = value;
                if (GetErrors(nameof(DistanceToWasteSource)) != null)
                {
                    _dataAccess.Set(nameof(DistanceToWasteSource), _DistanceToWasteSource_Not_Valid);
                }
                OnPropertyChanged(nameof(DistanceToWasteSource));
            }
        }
        
        private int _DistanceToWasteSource_Not_Valid = -1;
        private void DistanceToWasteSource_Validation(int value)//Ready
        {
            ClearErrors(nameof(DistanceToWasteSource));
        }
        //DistanceToWasteSource property

        //TestDepth property
        [Attributes.Form_Property("Глубина отбора проб, м")]
        public int TestDepth
        {
            get
            {
                if (GetErrors(nameof(TestDepth)) != null)
                {
                    return (int)_dataAccess.Get(nameof(TestDepth));
                }
                else
                {
                    return _TestDepth_Not_Valid;
                }
            }
            set
            {
                _TestDepth_Not_Valid = value;
                if (GetErrors(nameof(TestDepth)) != null)
                {
                    _dataAccess.Set(nameof(TestDepth), _TestDepth_Not_Valid);
                }
                OnPropertyChanged(nameof(TestDepth));
            }
        }
        
        private int _TestDepth_Not_Valid = -1;
        private void TestDepth_Validation(int value)//Ready
        {
            ClearErrors(nameof(TestDepth));
        }
        //TestDepth property

        //TestDepthNote property
        public int TestDepthNote
        {
            get
            {
                if (GetErrors(nameof(TestDepthNote)) != null)
                {
                    return (int)_dataAccess.Get(nameof(TestDepthNote));
                }
                else
                {
                    return _TestDepthNote_Not_Valid;
                }
            }
            set
            {
                _TestDepthNote_Not_Valid = value;
                if (GetErrors(nameof(TestDepthNote)) != null)
                {
                    _dataAccess.Set(nameof(TestDepthNote), _TestDepthNote_Not_Valid);
                }
                OnPropertyChanged(nameof(TestDepthNote));
            }
        }
        
        private int _TestDepthNote_Not_Valid = -1;
        private void TestDepthNote_Validation(int value)//Ready
        {
            ClearErrors(nameof(TestDepthNote));
        }
        //TestDepthNote property

        //RadionuclidName property
        [Attributes.Form_Property("Радионуклид")]
        public string RadionuclidName
        {
            get
            {
                if (GetErrors(nameof(RadionuclidName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(RadionuclidName));
                }
                else
                {
                    return _RadionuclidName_Not_Valid;
                }
            }
            set
            {
                _RadionuclidName_Not_Valid = value;
                if (GetErrors(nameof(RadionuclidName)) != null)
                {
                    _dataAccess.Set(nameof(RadionuclidName), _RadionuclidName_Not_Valid);
                }
                OnPropertyChanged(nameof(RadionuclidName));
            }
        }
        //If change this change validation
        private string _RadionuclidName_Not_Valid = "";
        private void RadionuclidName_Validation()//TODO
        {
            ClearErrors(nameof(RadionuclidName));
        }
        //RadionuclidName property

        //AverageYearConcentration property
        [Attributes.Form_Property("Среднегодовое содержание радионуклида, Бк/кг")]
        public double AverageYearConcentration
        {
            get
            {
                if (GetErrors(nameof(AverageYearConcentration)) != null)
                {
                    return (double)_dataAccess.Get(nameof(AverageYearConcentration));
                }
                else
                {
                    return _AverageYearConcentration_Not_Valid;
                }
            }
            set
            {
                _AverageYearConcentration_Not_Valid = value;
                if (GetErrors(nameof(AverageYearConcentration)) != null)
                {
                    _dataAccess.Set(nameof(AverageYearConcentration), _AverageYearConcentration_Not_Valid);
                }
                OnPropertyChanged(nameof(AverageYearConcentration));
            }
        }
        
        private double _AverageYearConcentration_Not_Valid = -1;
        private void AverageYearConcentration_Validation()//TODO
        {
            ClearErrors(nameof(AverageYearConcentration));
        }
        //AverageYearConcentration property
    }
}
