using Models.DataAccess;
using System;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 5.6: Сведения о наличии в подведомственных организациях изделий из обедненного урана")]
    public class Form56 : Abstracts.Form5
    {
        public Form56() : base()
        {
            //FormNum.Value = "56";
            //NumberOfFields.Value = 5;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //NameIOU property
        [Attributes.Form_Property("Наименование ИОУ")]public int? NameIOUId { get; set; }
        public virtual RamAccess<string> NameIOU
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(NameIOU));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(NameIOU), value);
                }
                OnPropertyChanged(nameof(NameIOU));
            }
        }


        private bool NameIOU_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors(); return true;}
        //NameIOU property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]public int? QuantityId { get; set; }
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
            if (value.Value <= 0) {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Quantity property

        //Mass Property
        [Attributes.Form_Property("Масса, кг")]public int? MassId { get; set; }
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
