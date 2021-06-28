using Models.DataAccess;
using System;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Идентификаторы:")]
    public class Form32_3 : Abstracts.Form3
    {
        public Form32_3() : base()
        {
            //FormNum.Valueue = "32_3";
            //NumberOfFields.Valueue = 2;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //IdName property
        [Attributes.Form_Property("Идентификатор")]public int? IdNameId { get; set; }
        public virtual RamAccess<string> IdName
        {
            get
            {

                {
                    return _dataAccess.Get<string>(nameof(IdName));
                }

                {

                }
            }
            set
            {


                {
                    _dataAccess.Set(nameof(IdName), value);
                }
                OnPropertyChanged(nameof(IdName));
            }
        }


        //IdName Property

        //Value property
        [Attributes.Form_Property("Значение")]public int? ValueId { get; set; }
        public virtual RamAccess<string> Value
        {
            get
            {

                {
                    return _dataAccess.Get<string>(nameof(Value));
                }

                {

                }
            }
            set
            {


                {
                    _dataAccess.Set(nameof(Value), value);
                }
                OnPropertyChanged(nameof(Value));
            }
        }


        //Value property
    }
}
