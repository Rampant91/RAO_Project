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
        [Attributes.Form_Property("Номер наблюдательной скважины")]
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
        [Attributes.Form_Property("Наименование зоны контроля")]
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
        
        //SupposedWasteSource property
        [Attributes.Form_Property("Предполагаемый источник поступления радиоактивных веществ")]
        public string SupposedWasteSource
        {
            get
            {
                if (GetErrors(nameof(SupposedWasteSource)) != null)
                {
                    return (string)_SupposedWasteSource.Get();
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
                    _SupposedWasteSource.Set(_SupposedWasteSource_Not_Valid);
                }
                OnPropertyChanged(nameof(SupposedWasteSource));
            }
        }
        private IDataLoadEngine _SupposedWasteSource;
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
                    return (int)_DistanceToWasteSource.Get();
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
                    _DistanceToWasteSource.Set(_DistanceToWasteSource_Not_Valid);
                }
                OnPropertyChanged(nameof(DistanceToWasteSource));
            }
        }
        private IDataLoadEngine _DistanceToWasteSource;
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
                    return (int)_TestDepth.Get();
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
                    _TestDepth.Set(_TestDepth_Not_Valid);
                }
                OnPropertyChanged(nameof(TestDepth));
            }
        }
        private IDataLoadEngine _TestDepth;
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
                    return (int)_TestDepthNote.Get();
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
                    _TestDepthNote.Set(_TestDepthNote_Not_Valid);
                }
                OnPropertyChanged(nameof(TestDepthNote));
            }
        }
        private IDataLoadEngine _TestDepthNote;
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
                    return (string)_RadionuclidName.Get();
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
                    _RadionuclidName.Set(_RadionuclidName_Not_Valid);
                }
                OnPropertyChanged(nameof(RadionuclidName));
            }
        }
        private IDataLoadEngine _RadionuclidName;//If change this change validation
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
                    return (double)_AverageYearConcentration.Get();
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
                    _AverageYearConcentration.Set(_AverageYearConcentration_Not_Valid);
                }
                OnPropertyChanged(nameof(AverageYearConcentration));
            }
        }
        private IDataLoadEngine _AverageYearConcentration;
        private double _AverageYearConcentration_Not_Valid = -1;
        private void AverageYearConcentration_Validation()//TODO
        {
            ClearErrors(nameof(AverageYearConcentration));
        }
        //AverageYearConcentration property
    }
}
