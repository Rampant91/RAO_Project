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
            FormNum = "56";
            NumberOfFields = 5;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //NameIOU property
        [Attributes.Form_Property("Наименование ИОУ")]
        public IDataAccess<string> NameIOU
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(NameIOU));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
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


        private void NameIOU_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //NameIOU property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public int Quantity
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(Quantity));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
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

        private void Quantity_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //Quantity property

        //Mass Property
        [Attributes.Form_Property("Масса, кг")]
        public double Mass
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Mass));
                }
                else
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


        private void Mass_Validation()//TODO
        {
            value.ClearErrors();
            if (Mass <= 0)
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //Mass Property
    }
}
