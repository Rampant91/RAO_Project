using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using ClassLibrary1;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 5.2: Сведения о наличии ЗРИ в подведомственных организациях")]
    public class Form52 : Abstracts.Form5
    {
        public Form52() : base()
        {
            //FormNum.Value = "52";
            //NumberOfFields.Value = 6;
        }
        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //Radionuclids property
        public int? RadionuclidsId { get; set; }
        [Attributes.Form_Property("Радионуклиды")]
        public virtual RamAccess<string> Radionuclids
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Radionuclids));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(Radionuclids), value);
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
                value.AddError("Поле не заполнено");
                return false;
            }
            foreach (var item in Spravochniki.SprRadionuclids)
            {
                if (item.Item1.Equals(value.Value))
                {
                    return true;
                }
            }
            value.AddError("Недопустимое значение");
            return false;
        }
        //Radionuclids property

        //Kategory property
        public int? KategoryId { get; set; }
        [Attributes.Form_Property("Категория")]
        public virtual RamAccess<short> Kategory
        {
            get
            {

                {
                    return DataAccess.Get<short>(nameof(Kategory));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Kategory), value);
                }
                OnPropertyChanged(nameof(Kategory));
            }
        }


        private bool Kategory_Validation(RamAccess<short> value)//TODO
        {
            value.ClearErrors(); return true;
        }
        //Kategory property

        //Activity property
        public int? ActivityId { get; set; }
        [Attributes.Form_Property("Активность, Бк")]
        public virtual RamAccess<string> Activity
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Activity));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Activity), value);
                }
                OnPropertyChanged(nameof(Activity));
            }
        }


        private bool Activity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
                value.AddError("Недопустимое значение");
                return false;
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
                    return DataAccess.Get<int>(nameof(Quantity));//OK

                }

                {

                }
            }
            set
            {




                {
                    DataAccess.Set(nameof(Quantity), value);
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
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Quantity property
    }
}
