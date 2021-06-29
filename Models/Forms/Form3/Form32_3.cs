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
        public int? IdNameId { get; set; }
        [Attributes.Form_Property("Идентификатор")]
        public virtual RamAccess<string> IdName
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(IdName));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(IdName), value);
                }
                OnPropertyChanged(nameof(IdName));
            }
        }


        //IdName Property

        //Value property
        public int? ValueId { get; set; }
        [Attributes.Form_Property("Значение")]
        public virtual RamAccess<string> Value
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Value));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Value), value);
                }
                OnPropertyChanged(nameof(Value));
            }
        }


        //Value property
    }
}
