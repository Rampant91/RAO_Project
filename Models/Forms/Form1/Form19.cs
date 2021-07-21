using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Spravochniki;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.9: Сведения о результатах инвентаризации РВ не в составе ЗРИ")]
    public class Form19 : Abstracts.Form1
    {
        public Form19() : base()
        {
            //FormNum.Value = "19";
            //NumberOfFields.Value = 13;
            Init();
            Validate_all();
        }

        private void Init()
        {
            DataAccess.Init<int?>(nameof(Quantity), Quantity_Validation, null);
            Quantity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Activity), Activity_Validation, null);
            Activity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<short?>(nameof(CodeTypeAccObject), CodeTypeAccObject_Validation, null);
            CodeTypeAccObject.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Radionuclids), Radionuclids_Validation, null);
            Radionuclids.PropertyChanged += InPropertyChanged;
            OperationCode.Value = 10;
        }

        private void Validate_all()
        {
            Quantity_Validation(Quantity);
            CodeTypeAccObject_Validation(CodeTypeAccObject);
            Activity_Validation(Activity);
            Radionuclids_Validation(Radionuclids);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //Quantity property
        public int? QuantityId { get; set; }
        [Attributes.Form_Property("Количество, шт.")]
        public virtual RamAccess<int?> Quantity
        {
            get
            {

                {
                    return DataAccess.Get<int?>(nameof(Quantity));//OK;
                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(Quantity), value);
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.

        private bool Quantity_Validation(RamAccess<int?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Quantity property

        //CodeTypeAccObject property
        public int? CodeTypeAccObjectId { get; set; }
        [Attributes.Form_Property("Код типа объектов учета")]
        public virtual RamAccess<short?> CodeTypeAccObject
        {
            get
            {

                {
                    return DataAccess.Get<short?>(nameof(CodeTypeAccObject));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(CodeTypeAccObject), value);
                }
                OnPropertyChanged(nameof(CodeTypeAccObject));
            }
        }


        private bool CodeTypeAccObject_Validation(RamAccess<short?> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            List<short> spr = new List<short>();
            if (!spr.Contains((short)value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //CodeTypeAccObject property

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
            foreach (var item in Spravochniks.SprRadionuclids)
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
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Activity property

        protected override bool OperationCode_Validation(RamAccess<short?> value)//OK
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value != 10)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }

        protected override bool OperationDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            DateTimeOffset date = DateTimeOffset.Parse(value.Value);
            if (date.Date > DateTimeOffset.Now.Date)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }

        protected override bool DocumentDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            DateTimeOffset date = DateTimeOffset.Parse(value.Value);
            if (date.Date > DateTimeOffset.Now.Date)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }

        protected override bool DocumentNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                return true;
            }
            return true;
        }
    }
}
