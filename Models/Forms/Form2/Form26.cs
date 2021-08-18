using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.6: Контроль загрязнения подземных вод РВ")]
    public class Form26 : Abstracts.Form2
    {
        public Form26() : base()
        {
            FormNum.Value = "2.6";
            //NumberOfFields.Value = 11;
            Validate_all();
        }

        private void Validate_all()
        {
            ObservedSourceNumber_Validation(ObservedSourceNumber);
            ControlledAreaName_Validation(ControlledAreaName);
            SupposedWasteSource_Validation(SupposedWasteSource);
            DistanceToWasteSource_Validation(DistanceToWasteSource);
            TestDepth_Validation(TestDepth);
            RadionuclidName_Validation(RadionuclidName);
            AverageYearConcentration_Validation(AverageYearConcentration);
            SourcesQuantity_Validation(SourcesQuantity);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //SourcesQuantity property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Количество источников, шт.")]
        public RamAccess<int?> SourcesQuantity
        {
            get
            {

                {
                    var tmp = new RamAccess<int?>(SourcesQuantity_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
            }
            set
            {


                {
                    SourcesQuantity_DB = value.Value;
                }
                OnPropertyChanged(nameof(SourcesQuantity));
            }
        }
        // positive int.
        private bool SourcesQuantity_Validation(RamAccess<int?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //SourcesQuantity property
        #endregion

        //ObservedSourceNumber property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Номер наблюдательной скважины")]
        public RamAccess<string> ObservedSourceNumber
        {
            get => new RamAccess<string>(ObservedSourceNumber_Validation, _DB);
            set
            {
                ObservedSourceNumber_DB = value.Value;
                OnPropertyChanged(nameof(ObservedSourceNumber));
            }
        }
        //If change this change validation
        private bool ObservedSourceNumber_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        //ObservedSourceNumber property
        #endregion

        //ControlledAreaName property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Наименование зоны контроля")]
        public RamAccess<string> ControlledAreaName
        {
            get => new RamAccess<string>(ControlledAreaName_Validation, _DB);
            set
            {
                ControlledAreaName_DB = value.Value;
                OnPropertyChanged(nameof(ControlledAreaName));
            }
        }
        //If change this change validation
        private bool ControlledAreaName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            List<string> spr = new List<string>()
            {
                "ПП",
                "СЗЗ",
                "ЗН",
                "прим."
            };
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //ControlledAreaName property
        #endregion

        //SupposedWasteSource property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Предполагаемый источник поступления радиоактивных веществ")]
        public RamAccess<string> SupposedWasteSource
        {
            get => new RamAccess<string>(SupposedWasteSource_Validation, _DB);
            set
            {
                SupposedWasteSource_DB = value.Value;
                OnPropertyChanged(nameof(SupposedWasteSource));
            }
        }

        private bool SupposedWasteSource_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();//done
            return true;
        }
        //SupposedWasteSource property
        #endregion

        //DistanceToWasteSource property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м")]
        public RamAccess<string> DistanceToWasteSource
        {
            get => new RamAccess<string>(DistanceToWasteSource_Validation, _DB);
            set
            {


                {
                    DistanceToWasteSource_DB = value.Value;
                }
                OnPropertyChanged(nameof(DistanceToWasteSource));
            }
        }

        private bool DistanceToWasteSource_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            if (value.Value.Equals("прим."))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //DistanceToWasteSource property
        #endregion

        //TestDepth property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Глубина отбора проб, м")]
        public RamAccess<string> TestDepth
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(TestDepth_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    TestDepth_DB = value.Value;
                }
                OnPropertyChanged(nameof(TestDepth));
            }
        }

        private bool TestDepth_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            if (value.Value.Equals("прим."))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //TestDepth property
        #endregion

        ////TestDepthNote property
        //public RamAccess<string> TestDepthNote
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<string>(TestDepthNote_Validation, _DB);
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            TestDepthNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(TestDepthNote));
        //    }
        //}

        //private bool TestDepthNote_Validation(RamAccess<string> value)//Ready
        //{
        //    value.ClearErrors();
        //    return true;
        //}
        ////TestDepthNote property

        //RadionuclidName property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Радионуклид")]
        public RamAccess<string> RadionuclidName
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(RadionuclidName_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {
                RadionuclidName_DB = value.Value;
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
            List<string> spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //RadionuclidName property
        #endregion

        //AverageYearConcentration property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Среднегодовое содержание радионуклида, Бк/кг")]
        public RamAccess<string> AverageYearConcentration
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(AverageYearConcentration_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    AverageYearConcentration_DB = value.Value;
                }
                OnPropertyChanged(nameof(AverageYearConcentration));
            }
        }

        private bool AverageYearConcentration_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //AverageYearConcentration property
        #endregion
    }
}
