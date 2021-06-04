using Models.DataAccess;
using System;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.6: Контроль загрязнения подземных вод РВ")]
    public class Form26 : Abstracts.Form2
    {
        public Form26() : base()
        {
            FormNum = "26";
            NumberOfFields = 11;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //SourcesQuantity property
        [Attributes.Form_Property("Количество источников, шт.")]
        public IDataAccess<int SourcesQuantity
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(SourcesQuantity));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SourcesQuantity), _SourcesQuantity_Not_Valid);
                }
                OnPropertyChanged(nameof(SourcesQuantity));
            }
        }
        // positive int.
                private void SourcesQuantity_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //SourcesQuantity property

        //ObservedSourceNumber property
        [Attributes.Form_Property("Номер наблюдательной скважины")]
        public IDataAccess<string> ObservedSourceNumber
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(ObservedSourceNumber));
            }
            set
            {
                    _dataAccess.Set(nameof(ObservedSourceNumber), _ObservedSourceNumber_Not_Valid);
                OnPropertyChanged(nameof(ObservedSourceNumber));
            }
        }
        //If change this change validation
                private void ObservedSourceNumber_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //ObservedSourceNumber property

        //ControlledAreaName property
        [Attributes.Form_Property("Наименование зоны контроля")]
        public IDataAccess<string ControlledAreaName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ControlledAreaName));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(ControlledAreaName), _ControlledAreaName_Not_Valid);
                }
                OnPropertyChanged(nameof(ControlledAreaName));
            }
        }
        //If change this change validation
                private void ControlledAreaName_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //ControlledAreaName property

        //SupposedWasteSource property
        [Attributes.Form_Property("Предполагаемый источник поступления радиоактивных веществ")]
        public IDataAccess<string SupposedWasteSource
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(SupposedWasteSource));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SupposedWasteSource), _SupposedWasteSource_Not_Valid);
                }
                OnPropertyChanged(nameof(SupposedWasteSource));
            }
        }

                private void SupposedWasteSource_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //SupposedWasteSource property

        //DistanceToWasteSource property
        [Attributes.Form_Property("Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м")]
        public IDataAccess<int DistanceToWasteSource
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(DistanceToWasteSource));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(DistanceToWasteSource), _DistanceToWasteSource_Not_Valid);
                }
                OnPropertyChanged(nameof(DistanceToWasteSource));
            }
        }

                private void DistanceToWasteSource_Validation(int value)//Ready
        {
            value.ClearErrors();
        }
        //DistanceToWasteSource property

        //TestDepth property
        [Attributes.Form_Property("Глубина отбора проб, м")]
        public IDataAccess<int TestDepth
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(TestDepth));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(TestDepth), _TestDepth_Not_Valid);
                }
                OnPropertyChanged(nameof(TestDepth));
            }
        }

                private void TestDepth_Validation(int value)//Ready
        {
            value.ClearErrors();
        }
        //TestDepth property

        //TestDepthNote property
        public IDataAccess<int TestDepthNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(TestDepthNote));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(TestDepthNote), _TestDepthNote_Not_Valid);
                }
                OnPropertyChanged(nameof(TestDepthNote));
            }
        }

                private void TestDepthNote_Validation(int value)//Ready
        {
            value.ClearErrors();
        }
        //TestDepthNote property

        //RadionuclidName property
        [Attributes.Form_Property("Радионуклид")]
        public IDataAccess<string RadionuclidName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RadionuclidName));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(RadionuclidName), _RadionuclidName_Not_Valid);
                }
                OnPropertyChanged(nameof(RadionuclidName));
            }
        }
        //If change this change validation
                private void RadionuclidName_Validation()//TODO
        {
            value.ClearErrors();
        }
        //RadionuclidName property

        //AverageYearConcentration property
        [Attributes.Form_Property("Среднегодовое содержание радионуклида, Бк/кг")]
        public IDataAccess<double AverageYearConcentration
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(AverageYearConcentration));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(AverageYearConcentration), _AverageYearConcentration_Not_Valid);
                }
                OnPropertyChanged(nameof(AverageYearConcentration));
            }
        }

                private void AverageYearConcentration_Validation()//TODO
        {
            value.ClearErrors();
        }
        //AverageYearConcentration property
    }
}
