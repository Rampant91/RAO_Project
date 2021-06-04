using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 5.4: Сведения о наличии в подведомственных организациях ОРИ")]
    public class Form54 : Abstracts.Form5
    {
        public Form54() : base()
        {
            FormNum.Value = "54";
            NumberOfFields.Value = 10;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //TypeOfAccountedParts property
        [Attributes.Form_Property("Тип учетных единиц")]
        public RamAccess<int> TypeOfAccountedParts
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(TypeOfAccountedParts));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(TypeOfAccountedParts), value);
                }
                OnPropertyChanged(nameof(TypeOfAccountedParts));
            }
        }
        //1 or 2
 //1 or 2
        private void TypeOfAccountedParts_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value != 1) && (value.Value != 2))
                value.AddError( "Недопустимое значение");
        }
        //TypeOfAccountedParts property

        //KindOri property
        [Attributes.Form_Property("Вид ОРИ")]
        public RamAccess<int> KindOri
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(KindOri));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(KindOri), value);
                }
                OnPropertyChanged(nameof(KindOri));
            }
        }


        private void KindOri_Validation(IDataAccess<int> value)//TODO
        {
        }
        //KindOri property

        //AggregateState property
        [Attributes.Form_Property("Агрегатное состояние")]
        public RamAccess<byte> AggregateState//1 2 3
        {
            get
            {
                
                {
                    return _dataAccess.Get<byte>(nameof(AggregateState));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(AggregateState), value);
                }
                OnPropertyChanged(nameof(AggregateState));
            }
        }


        private void AggregateState_Validation(IDataAccess<byte> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value != 1) && (value.Value != 2) && (value.Value != 3))
                value.AddError( "Недопустимое значение");
        }
        //AggregateState property

        //Radionuclids property
        [Attributes.Form_Property("Радионуклиды")]
        public RamAccess<string> Radionuclids
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Radionuclids));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                Radionuclids_Validation(value);

                
                {
                    _dataAccess.Set(nameof(Radionuclids), value);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation

        private void Radionuclids_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            List<Tuple<string, string>> spr = new List<Tuple<string, string>>();//Here binds spravochnik
            foreach (var item in spr)
            {
                if (item.Item2.Equals(value))
                {
                    Radionuclids.Value =item.Item2;
                    return;
                }
            }
        }
        //Radionuclids property

        //Activity property
        [Attributes.Form_Property("Активность, Бк")]
        public RamAccess<string> Activity
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Activity));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Activity), value);
                }
                OnPropertyChanged(nameof(Activity));
            }
        }


        private void Activity_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (!(value.Value.Contains('e')))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //Activity property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public RamAccess<int> Quantity
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(Quantity));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                Quantity_Validation(value);


                
                {
                    _dataAccess.Set(nameof(Quantity), value);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.

        private void Quantity_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //Quantity property

        //Volume property
        [Attributes.Form_Property("Объем, куб. м")]
        public RamAccess<double> Volume
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(Volume));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Volume), value);
                }
                OnPropertyChanged(nameof(Volume));
            }
        }


        private void Volume_Validation(IDataAccess<double> value)//TODO
        {
            value.ClearErrors();
            if (value.Value <= 0)
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //Volume property

        //Mass Property
        [Attributes.Form_Property("Масса, кг")]
        public RamAccess<double> Mass
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(Mass));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Mass), value);
                }
                OnPropertyChanged(nameof(Mass));
            }
        }


        private void Mass_Validation(IDataAccess<double> value)//TODO
        {
            value.ClearErrors();
            if (value.Value <= 0)
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //Mass Property
    }
}
