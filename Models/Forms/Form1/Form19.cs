using Models.DataAccess;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.9: Сведения о результатах инвентаризации РВ не в составе ЗРИ")]
    public class Form19 : Abstracts.Form1
    {
        public Form19() : base()
        {
            FormNum.Value = "19";
            NumberOfFields.Value = 13;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public RamAccess<int>? Quantity
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(Quantity));//OK;
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(Quantity), value);
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.

        private void Quantity_Validation(RamAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null) return;
            if (value.Value <= 0)
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //Quantity property

        //CodeTypeAccObject property
        [Attributes.Form_Property("Код типа объектов учета")]
        public RamAccess<short?> CodeTypeAccObject
        {
            get
            {
                
                {
                    return _dataAccess.Get<short?>(nameof(CodeTypeAccObject));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(CodeTypeAccObject), value);
                }
                OnPropertyChanged(nameof(CodeTypeAccObject));
            }
        }


        private void CodeTypeAccObject_Validation(RamAccess<short?> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            List<short> spr = new List<short>();
            if (!spr.Contains((short)value.Value))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //CodeTypeAccObject property

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

        private void Radionuclids_Validation(RamAccess<string> value)//TODO
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


        private void Activity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (!(value.Value.Contains('e')||value.Value.Contains('E')))
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

        protected override void OperationCode_Validation(RamAccess<short?> value)//OK
        {
            value.ClearErrors();
            OperationCode.Value = 10;
        }

        protected override void OperationDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            var date = DateTimeOffset.Parse(value.Value);
            if (date.Date > DateTimeOffset.Now.Date)
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }

        protected override void DocumentDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            var date = DateTimeOffset.Parse(value.Value);
            if (date.Date > DateTimeOffset.Now.Date)
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }

        protected override void DocumentNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))//ok
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Value.Equals("прим."))
            {

            }
        }
    }
}
