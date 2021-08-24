using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Spravochniki;
using System.Linq;
using System.ComponentModel;

namespace Models
{
    [Attributes.Form_Class("Форма 1.9: Сведения о результатах инвентаризации РВ не в составе ЗРИ")]
    public class Form19 : Abstracts.Form1
    {
        public Form19() : base()
        {
            FormNum.Value = "1.9";
            OperationCode.Value = 10;
            Validate_all();
        }

        private void Validate_all()
        {
            //Quantity_Validation(Quantity);
            CodeTypeAccObject_Validation(CodeTypeAccObject);
            Activity_Validation(Activity);
            Radionuclids_Validation(Radionuclids);
        }
        public override bool Object_Validation()
        {
            return false;
        }

        //#region Quantity
        //public int? Quantity_DB { get; set; } = null;
        //[NotMapped]
        //[Attributes.Form_Property("Количество, шт.")]
        //public RamAccess<int?> Quantity
        //{
        //    get
        //    {
        //        var tmp = new RamAccess<int?>(Quantity_Validation, Quantity_DB);
        //        tmp.PropertyChanged += QuantityValueChanged;
        //        return tmp;
        //    }
        //    set
        //    {
        //        Quantity_DB = value.Value;
        //        OnPropertyChanged(nameof(Quantity));
        //    }
        //}// positive int.

        //private void QuantityValueChanged(object Value, PropertyChangedEventArgs args)
        //{
        //    if (args.PropertyName == "Value")
        //    {
        //        Quantity_DB = ((RamAccess<int?>)Value).Value;
        //    }
        //}
        //private bool Quantity_Validation(RamAccess<int?> value)//Ready
        //{
        //    value.ClearErrors();
        //    if (value.Value == null)
        //    {
        //        value.AddError("Поле не заполнено");
        //        return false;
        //    }
        //    if (value.Value <= 0)
        //    {
        //        value.AddError("Недопустимое значение");
        //        return false;
        //    }
        //    return true;
        //}
        //#endregion

        #region CodeTypeAccObject
        public short? CodeTypeAccObject_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property("Код типа объектов учета")]
        public RamAccess<short?> CodeTypeAccObject
        {
            get
            {
                var tmp = new RamAccess<short?>(CodeTypeAccObject_Validation, CodeTypeAccObject_DB);
                tmp.PropertyChanged += CodeTypeAccObjectValueChanged;
                return tmp;
            }
            set
            {
                CodeTypeAccObject_DB = value.Value;
                OnPropertyChanged(nameof(CodeTypeAccObject));
            }
        }
        private void CodeTypeAccObjectValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                CodeTypeAccObject_DB = ((RamAccess<short?>)Value).Value;
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
        #endregion

        #region Radionuclids
        public string Radionuclids_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Радионуклиды")]
        public RamAccess<string> Radionuclids
        {
            get
            {
                var tmp = new RamAccess<string>(Radionuclids_Validation, Radionuclids_DB);
                tmp.PropertyChanged += RadionuclidsValueChanged;
                return tmp;
            }
            set
            {
                Radionuclids_DB = value.Value;
                OnPropertyChanged(nameof(Radionuclids));
            }
        }//If change this change validation

        private void RadionuclidsValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Radionuclids_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool Radionuclids_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            string[] nuclids = value.Value.Split("; ");
            bool flag = true;
            foreach (var nucl in nuclids)
            {
                var tmp = from item in Spravochniks.SprRadionuclids where nucl == item.Item1 select item.Item1;
                if (tmp.Count() == 0)
                    flag = false;
            }
            if (!flag)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region Activity
        public string Activity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Активность, Бк")]
        public RamAccess<string> Activity
        {
            get
            {
                var tmp = new RamAccess<string>(Activity_Validation, Activity_DB);
                tmp.PropertyChanged += ActivityValueChanged;
                return tmp;
            }
            set
            {
                Activity_DB = value.Value;
                OnPropertyChanged(nameof(Activity));
            }
        }
        private void ActivityValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Activity_DB = ((RamAccess<string>)Value).Value;
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
        #endregion

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
