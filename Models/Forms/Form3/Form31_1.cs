using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Характеристики экспортируемых ЗРИ/ОЗИИИ:")]
    public class Form31_1 : Abstracts.Form3
    {
        public Form31_1() : base()
        {
            FormNum.Value = "31_1";
            NumberOfFields.Value = 3;
        }

        [Attributes.Form_Property("Форма")]

        public override bool Object_Validation()
        {
            return false;
        }

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

        //SummaryActivity property
        [Attributes.Form_Property("Суммарная активность, Бк")]
        public RamAccess<string> SummaryActivity
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(SummaryActivity));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SummaryActivity), value);
                }
                OnPropertyChanged(nameof(SummaryActivity));
            }
        }

        
        private void SummaryActivity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || (value.Value.Equals("")))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (!(value.Value.Contains('e')))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            string tmp=value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //SummaryActivity property
    }
}
