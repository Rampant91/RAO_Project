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
            //FormNum.Value = "29";
            //NumberOfFields.Value = 8;
            Init();
            Validate_all();
        }

        private void Init()
        {
            DataAccess.Init<string>(nameof(WasteSourceName), WasteSourceName_Validation, null);
            WasteSourceName.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(RadionuclidName), RadionuclidName_Validation, null);
            RadionuclidName.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(AllowedActivity), AllowedActivity_Validation, null);
            AllowedActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(FactedActivity), FactedActivity_Validation, null);
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
        public int? WasteSourceNameId { get; set; }
        [Attributes.Form_Property("Наименование, номер выпуска сточных вод")]
        public virtual RamAccess<string> WasteSourceName
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(WasteSourceName));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(WasteSourceName), value);
                }
                OnPropertyChanged(nameof(WasteSourceName));
            }
        }


        private bool WasteSourceName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //WasteSourceName property

        //RadionuclidName property
        public int? RadionuclidNameId { get; set; }
        [Attributes.Form_Property("Радионуклид")]
        public virtual RamAccess<string> RadionuclidName
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(RadionuclidName));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(RadionuclidName), value);
                }
                OnPropertyChanged(nameof(RadionuclidName));
            }
        }
        //If change this change validation

        private bool RadionuclidName_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //RadionuclidName property

        //AllowedActivity property
        public int? AllowedActivityId { get; set; }
        [Attributes.Form_Property("Допустимая активность радионуклида, Бк")]
        public virtual RamAccess<string> AllowedActivity
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(AllowedActivity));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(AllowedActivity), value);
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
            }
            return true;
        }
        //AllowedActivity property

        ////AllowedActivityNote property
        //public virtual RamAccess<string> AllowedActivityNote
        //{
        //    get
        //    {

        //        {
        //            return DataAccess.Get<string>(nameof(AllowedActivityNote));
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            DataAccess.Set(nameof(AllowedActivityNote), value);
        //        }
        //        OnPropertyChanged(nameof(AllowedActivityNote));
        //    }
        //}


        //private bool AllowedActivityNote_Validation(RamAccess<string> value)//Ready
        //{
        //    return true;
        //}
        ////AllowedActivityNote property

        //FactedActivity property
        public int? FactedActivityId { get; set; }
        [Attributes.Form_Property("Фактическая активность радионуклида, Бк")]
        public virtual RamAccess<string> FactedActivity
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(FactedActivity));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(FactedActivity), value);
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
        //FactedActivity property

        ////FactedActivityNote property
        //public virtual RamAccess<string> FactedActivityNote
        //{
        //    get
        //    {

        //        {
        //            return DataAccess.Get<string>(nameof(FactedActivityNote));
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            DataAccess.Set(nameof(FactedActivityNote), value);
        //        }
        //        OnPropertyChanged(nameof(FactedActivityNote));
        //    }
        //}


        //private bool FactedActivityNote_Validation(RamAccess<string> value)//Ready
        //{
        //    return true;
        //}
        ////FactedActivityNote property
    }
}
