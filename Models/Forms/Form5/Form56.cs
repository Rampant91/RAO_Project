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
            FormNum.Value = "56";
            NumberOfFields.Value = 5;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //NameIOU property
        [Attributes.Form_Property("Наименование ИОУ")]
        public RamAccess<string> NameIOU
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


        private void NameIOU_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //NameIOU property

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

        private void Quantity_Validation(RamAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //Quantity property

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


        private void Mass_Validation(RamAccess<double> value)//TODO
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
