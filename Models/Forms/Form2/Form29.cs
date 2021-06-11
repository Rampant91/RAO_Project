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
            Init();
            Validate_all();
        }

        private void Init()
        {
            _dataAccess.Init<string>(nameof(WasteSourceName), WasteSourceName_Validation, null);
            WasteSourceName.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(RadionuclidName), RadionuclidName_Validation, null);
            RadionuclidName.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(AllowedActivity), AllowedActivity_Validation, null);
            AllowedActivity.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(FactedActivity), FactedActivity_Validation, null);
            FactedActivity.PropertyChanged += InPropertyChanged;
        }

        private void Validate_all()
        {
            WasteSourceName_Validation(WasteSourceName);
            RadionuclidName_Validation(RadionuclidName);
            AllowedActivity_Validation(AllowedActivity);
            FactedActivity_Validation(FactedActivity);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //WasteSourceName property
        [Attributes.Form_Property("Наименование, номер выпуска сточных вод")]
        public RamAccess<string> WasteSourceName
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


        private bool WasteSourceName_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //WasteSourceName property

        //RadionuclidName property
        [Attributes.Form_Property("Радионуклид")]
        public RamAccess<string> RadionuclidName
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

        private bool RadionuclidName_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors(); return true;
        }
        //RadionuclidName property

        //AllowedActivity property
        [Attributes.Form_Property("Допустимая активность радионуклида, Бк")]
        public RamAccess<string> AllowedActivity
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


        private bool AllowedActivity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || (value.Value.Equals("")))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            if (value.Value != "прим.")
            {
                var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
            }
            return true;
        }
        //AllowedActivity property

        //AllowedActivityNote property
        public RamAccess<string> AllowedActivityNote
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


        private bool AllowedActivityNote_Validation(RamAccess<string> value)//Ready
        {
            return true;
        }
        //AllowedActivityNote property

        //FactedActivity property
        [Attributes.Form_Property("Фактическая активность радионуклида, Бк")]
        public RamAccess<string> FactedActivity
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


        private bool FactedActivity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || (value.Value.Equals("")))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        //FactedActivity property

        //FactedActivityNote property
        public RamAccess<string> FactedActivityNote
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


        private bool FactedActivityNote_Validation(RamAccess<string> value)//Ready
        {
            return true;
        }
        //FactedActivityNote property
    }
}
