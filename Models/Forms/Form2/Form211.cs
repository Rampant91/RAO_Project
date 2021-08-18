using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using Spravochniki;
using System.Linq;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.11: Радионуклидный состав загрязненных участков территорий")]
    public class Form211 : Abstracts.Form2
    {
        public Form211() : base()
        {
            FormNum.Value = "2.11";
            //NumberOfFields.Value = 11;
            Validate_all();
        }

        private void Validate_all()
        {
            Radionuclids_Validation(Radionuclids);
            PlotName_Validation(PlotName);
            PlotKadastrNumber_Validation(PlotKadastrNumber);
            PlotCode_Validation(PlotCode);
            InfectedArea_Validation(InfectedArea);
            SpecificActivityOfPlot_Validation(SpecificActivityOfPlot);
            SpecificActivityOfLiquidPart_Validation(SpecificActivityOfLiquidPart);
            SpecificActivityOfDensePart_Validation(SpecificActivityOfDensePart);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //PlotName property
#region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Наименование участка")]
        public RamAccess<string> PlotName
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(PlotName_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    PlotName_DB = value.Value;
                }
                OnPropertyChanged(nameof(PlotName));
            }
        }

        private bool PlotName_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //PlotName property
        #endregion

        //PlotKadastrNumber property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Кадастровый номер участка")]
        public RamAccess<string> PlotKadastrNumber
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(PlotKadastrNumber_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    PlotKadastrNumber_DB = value.Value;
                }
                OnPropertyChanged(nameof(PlotKadastrNumber));
            }
        }

        private bool PlotKadastrNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //PlotKadastrNumber property
        #endregion

        //PlotCode property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Код участка")]
        public RamAccess<string> PlotCode
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(PlotCode_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    PlotCode_DB = value.Value;
                }
                OnPropertyChanged(nameof(PlotCode));
            }
        }
        //6 symbols code
        private bool PlotCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //PlotCode property
        #endregion

        //InfectedArea property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Площадь загрязненной территории, кв. м")]
        public RamAccess<int?> InfectedArea
        {
            get
            {

                {
                    var tmp = new RamAccess<int?>(InfectedArea_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    InfectedArea_DB = value.Value;
                }
                OnPropertyChanged(nameof(InfectedArea));
            }
        }

        private bool InfectedArea_Validation(RamAccess<int?> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //InfectedArea property
        #endregion

        //Radionuclids property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Наименования радионуклидов")]
        public RamAccess<string> Radionuclids
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(Radionuclids_Validation, _DB);//OK
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;

                }

                {

                }
            }
            set
            {



                {
                    Radionuclids_DB = value.Value;
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation
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
        //Radionuclids property
        #endregion

        ////RadionuclidNameNote property
        //public RamAccess<string> RadionuclidNameNote
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<string>(RadionuclidNameNote_Validation, _DB);
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            RadionuclidNameNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(RadionuclidNameNote));
        //    }
        //}

        //private bool RadionuclidNameNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////RadionuclidNameNote property

        //SpecificActivityOfPlot property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Удельная активность, Бк/г")]
        public RamAccess<string> SpecificActivityOfPlot
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(SpecificActivityOfPlot_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    SpecificActivityOfPlot_DB = value.Value;
                }
                OnPropertyChanged(nameof(SpecificActivityOfPlot));
            }
        }

        private bool SpecificActivityOfPlot_Validation(RamAccess<string> value)//TODO
        {
            return true;
            //value.ClearErrors();
            //var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //   NumberStyles.AllowExponent;
            //try
            //{
            //    if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля");return false;}
            //}
            //catch
            //{
            //    value.AddError( "Недопустимое значение");
            //}
        }
        //SpecificActivityOfPlot property
        #endregion

        //SpecificActivityOfLiquidPart property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Удельная активность жидкой части, Бк/г")]
        public RamAccess<string> SpecificActivityOfLiquidPart
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(SpecificActivityOfLiquidPart_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    SpecificActivityOfLiquidPart_DB = value.Value;
                }
                OnPropertyChanged(nameof(SpecificActivityOfLiquidPart));
            }
        }

        private bool SpecificActivityOfLiquidPart_Validation(RamAccess<string> value)//TODO
        {
            return true;
            //value.ClearErrors();
            //var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //   NumberStyles.AllowExponent;
            //try
            //{
            //    if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля");return false;}
            //}
            //catch
            //{
            //    value.AddError( "Недопустимое значение");
            //}
        }
        //SpecificActivityOfLiquidPart property
        #endregion

        //SpecificActivityOfDensePart property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Удельная активность твердой части, Бк/г")]
        public RamAccess<string> SpecificActivityOfDensePart
        {
            get => new RamAccess<string>(SpecificActivityOfDensePart_Validation, _DB);
            set
            {


                {
                    SpecificActivityOfDensePart_DB = value.Value;
                }
                OnPropertyChanged(nameof(SpecificActivityOfDensePart));
            }
        }

        private bool SpecificActivityOfDensePart_Validation(RamAccess<string> value)//TODO
        {
            return true;
            //value.ClearErrors();
            //var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //   NumberStyles.AllowExponent;
            //try
            //{
            //    if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля");return false;}
            //}
            //catch
            //{
            //    value.AddError( "Недопустимое значение");
            //}
        }
        //SpecificActivityOfDensePart property
        #endregion
    }
}
