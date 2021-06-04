using Models.DataAccess;
using System;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.9: Активность радионуклидов, отведенных со сточными водами")]
    public class Form29 : Abstracts.Form2
    {
        public Form29() : base()
        {
            FormNum.Value = "29";
            NumberOfFields.Value = 8;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //WasteSourceName property
        [Attributes.Form_Property("Наименование, номер выпуска сточных вод")]
        public IDataAccess<string> WasteSourceName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(WasteSourceName));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(WasteSourceName), value);
                }
                OnPropertyChanged(nameof(WasteSourceName));
            }
        }

        
        private void WasteSourceName_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //WasteSourceName property

        //RadionuclidName property
        [Attributes.Form_Property("Радионуклид")]
        public IDataAccess<string> RadionuclidName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RadionuclidName));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(RadionuclidName), value);
                }
                OnPropertyChanged(nameof(RadionuclidName));
            }
        }
        //If change this change validation
        
        private void RadionuclidName_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //RadionuclidName property

        //AllowedActivity property
        [Attributes.Form_Property("Допустимая активность радионуклида, Бк")]
        public IDataAccess<string> AllowedActivity
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(AllowedActivity));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(AllowedActivity), value);
                }
                OnPropertyChanged(nameof(AllowedActivity));
            }
        }

        
        private void AllowedActivity_Validation(IDataAccess<string> value)//Ready
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
            if (value.Value != "прим.")
            {
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
        }
        //AllowedActivity property

        //AllowedActivityNote property
        public IDataAccess<string> AllowedActivityNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(AllowedActivityNote));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(AllowedActivityNote), value);
                }
                OnPropertyChanged(nameof(AllowedActivityNote));
            }
        }

        
        private void AllowedActivityNote_Validation(IDataAccess<string> value)//Ready
        {

        }
        //AllowedActivityNote property

        //FactedActivity property
        [Attributes.Form_Property("Фактическая активность радионуклида, Бк")]
        public IDataAccess<string> FactedActivity
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FactedActivity));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(FactedActivity), value);
                }
                OnPropertyChanged(nameof(FactedActivity));
            }
        }

        
        private void FactedActivity_Validation(IDataAccess<string> value)//Ready
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
        //FactedActivity property

        //FactedActivityNote property
        public IDataAccess<string> FactedActivityNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FactedActivityNote));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(FactedActivityNote), value);
                }
                OnPropertyChanged(nameof(FactedActivityNote));
            }
        }

        
        private void FactedActivityNote_Validation(IDataAccess<string> value)//Ready
        {

        }
        //FactedActivityNote property
    }
}
