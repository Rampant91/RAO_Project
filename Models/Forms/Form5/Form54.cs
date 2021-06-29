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
            //FormNum.Value = "54";
            //NumberOfFields.Value = 10;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //TypeOfAccountedParts property
        public int? TypeOfAccountedPartsId { get; set; }
        [Attributes.Form_Property("Тип учетных единиц")]
        public virtual RamAccess<int> TypeOfAccountedParts
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
        private bool TypeOfAccountedParts_Validation(RamAccess<int> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value != 1) && (value.Value != 2))
            {
                value.AddError("Недопустимое значение");
            }
            return true;
        }
        //TypeOfAccountedParts property

        //KindOri property
        public int? KindOriId { get; set; }
        [Attributes.Form_Property("Вид ОРИ")]
        public virtual RamAccess<int> KindOri
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


        private bool KindOri_Validation(RamAccess<int> value)//TODO
        {
            return true;
        }
        //KindOri property

        //AggregateState property
public int? AggregateStateId { get; set; }
        [Attributes.Form_Property("Агрегатное состояние")]
        public virtual RamAccess<byte> AggregateState//1 2 3
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


        private bool AggregateState_Validation(RamAccess<byte> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value != 1) && (value.Value != 2) && (value.Value != 3))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //AggregateState property

        //Radionuclids property
        public int? RadionuclidsId { get; set; }
        [Attributes.Form_Property("Радионуклиды")]
        public virtual RamAccess<string> Radionuclids
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


                
                {
                    _dataAccess.Set(nameof(Radionuclids), value);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation

        private bool Radionuclids_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            List<Tuple<string, string>> spr = new List<Tuple<string, string>>();//Here binds spravochnik
            foreach (var item in spr)
            {
                if (item.Item2.Equals(value))
                {
                    Radionuclids.Value =item.Item2;
                    return true;
                }
            }
            return false;
        }
        //Radionuclids property

        //Activity property
        public int? ActivityId { get; set; }
        [Attributes.Form_Property("Активность, Бк")]
        public virtual RamAccess<string> Activity
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


        private bool Activity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (!(value.Value.Contains('e')))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                {
                    value.AddError("Число должно быть больше нуля"); return false;
                }
            }
            catch
            {
                value.AddError( "Недопустимое значение"); return false;
            }
            return true;
        }
        //Activity property

        //Quantity property
        public int? QuantityId { get; set; }
        [Attributes.Form_Property("Количество, шт.")]
        public virtual RamAccess<int> Quantity
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



                
                {
                    _dataAccess.Set(nameof(Quantity), value);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.

        private bool Quantity_Validation(RamAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Quantity property

        //Volume property
        public int? VolumeId { get; set; }
        [Attributes.Form_Property("Объем, куб. м")]
        public virtual RamAccess<double> Volume
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


        private bool Volume_Validation(RamAccess<double> value)//TODO
        {
            value.ClearErrors();
            if (value.Value <= 0)
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            return true;
        }
        //Volume property

        //Mass Property
        public int? MassId { get; set; }
        [Attributes.Form_Property("Масса, кг")]
        public virtual RamAccess<double> Mass
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


        private bool Mass_Validation(RamAccess<double> value)//TODO
        {
            value.ClearErrors();
            if (value.Value <= 0)
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            return true;
        }
        //Mass Property
    }
}
