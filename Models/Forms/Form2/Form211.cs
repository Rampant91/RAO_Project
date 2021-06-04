using Models.DataAccess;
using System.Collections.Generic;
using System;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.11: Радионуклидный состав загрязненных участков территорий")]
    public class Form211 : Abstracts.Form2
    {
        public Form211() : base()
        {
            FormNum.Value = "211";
            NumberOfFields.Value = 11;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //PlotName property
        [Attributes.Form_Property("Наименование участка")]
        public RamAccess<string> PlotName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PlotName));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PlotName), value);
                }
                OnPropertyChanged(nameof(PlotName));
            }
        }

                private void PlotName_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //PlotName property

        //PlotKadastrNumber property
        [Attributes.Form_Property("Кадастровый номер участка")]
        public RamAccess<string> PlotKadastrNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PlotKadastrNumber));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PlotKadastrNumber), value);
                }
                OnPropertyChanged(nameof(PlotKadastrNumber));
            }
        }

                private void PlotKadastrNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //PlotKadastrNumber property

        //PlotCode property
        [Attributes.Form_Property("Код участка")]
        public RamAccess<string> PlotCode
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PlotCode));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PlotCode), value);
                }
                OnPropertyChanged(nameof(PlotCode));
            }
        }
        //6 symbols code
                private void PlotCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //PlotCode property

        //InfectedArea property
        [Attributes.Form_Property("Площадь загрязненной территории, кв. м")]
        public RamAccess<int> InfectedArea
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(InfectedArea));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(InfectedArea), value);
                }
                OnPropertyChanged(nameof(InfectedArea));
            }
        }

                private void InfectedArea_Validation(RamAccess<int> value)//TODO
        {
            value.ClearErrors();
        }
        //InfectedArea property

        //Radionuclids property
        [Attributes.Form_Property("Наименования радионуклидов")]
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

        //RadionuclidNameNote property
        public RamAccess<string> RadionuclidNameNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RadionuclidNameNote));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(RadionuclidNameNote), value);
                }
                OnPropertyChanged(nameof(RadionuclidNameNote));
            }
        }

                private void RadionuclidNameNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
        }
        //RadionuclidNameNote property

        //SpecificActivityOfPlot property
        [Attributes.Form_Property("Удельная активность, Бк/г")]
        public RamAccess<string> SpecificActivityOfPlot
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(SpecificActivityOfPlot));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SpecificActivityOfPlot), value);
                }
                OnPropertyChanged(nameof(SpecificActivityOfPlot));
            }
        }

                private void SpecificActivityOfPlot_Validation(RamAccess<string> value)//TODO
        {
            //value.ClearErrors();
            //var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //   NumberStyles.AllowExponent;
            //try
            //{
            //    if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
            //        value.AddError( "Число должно быть больше нуля");
            //}
            //catch
            //{
            //    value.AddError( "Недопустимое значение");
            //}
        }
        //SpecificActivityOfPlot property

        //SpecificActivityOfLiquidPart property
        [Attributes.Form_Property("Удельная активность жидкой части, Бк/г")]
        public RamAccess<string> SpecificActivityOfLiquidPart
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(SpecificActivityOfLiquidPart));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SpecificActivityOfLiquidPart), value);
                }
                OnPropertyChanged(nameof(SpecificActivityOfLiquidPart));
            }
        }

                private void SpecificActivityOfLiquidPart_Validation(RamAccess<string> value)//TODO
        {
            //value.ClearErrors();
            //var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //   NumberStyles.AllowExponent;
            //try
            //{
            //    if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
            //        value.AddError( "Число должно быть больше нуля");
            //}
            //catch
            //{
            //    value.AddError( "Недопустимое значение");
            //}
        }
        //SpecificActivityOfLiquidPart property

        //SpecificActivityOfDensePart property
        [Attributes.Form_Property("Удельная активность твердой части, Бк/г")]
        public RamAccess<string> SpecificActivityOfDensePart
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(SpecificActivityOfDensePart));
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SpecificActivityOfDensePart), value);
                }
                OnPropertyChanged(nameof(SpecificActivityOfDensePart));
            }
        }

                private void SpecificActivityOfDensePart_Validation(RamAccess<string> value)//TODO
        {
            //value.ClearErrors();
            //var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //   NumberStyles.AllowExponent;
            //try
            //{
            //    if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
            //        value.AddError( "Число должно быть больше нуля");
            //}
            //catch
            //{
            //    value.AddError( "Недопустимое значение");
            //}
        }
        //SpecificActivityOfDensePart property
    }
}
