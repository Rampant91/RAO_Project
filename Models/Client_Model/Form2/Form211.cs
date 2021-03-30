using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 2.11: Радионуклидный состав загрязненных участков территорий")]
    public class Form211 : Form2
    {
        public override string FormNum { get { return "2.11"; } }
        public override void Object_Validation()
        {

        }
        public override int NumberOfFields { get; } = 11;

        //PlotName property
        [Attributes.FormVisual("Наименование участка")]
        public string PlotName
        {
            get
            {
                if (GetErrors(nameof(PlotName)) != null)
                {
                    return (string)_PlotName.Get();
                }
                else
                {
                    return _PlotName_Not_Valid;
                }
            }
            set
            {
                _PlotName_Not_Valid = value;
                if (GetErrors(nameof(PlotName)) != null)
                {
                    _PlotName.Set(_PlotName_Not_Valid);
                }
                OnPropertyChanged(nameof(PlotName));
            }
        }
        private IDataLoadEngine _PlotName;
        private string _PlotName_Not_Valid = "";
        private void PlotName_Validation(string value)//TODO
        {
            ClearErrors(nameof(PlotName));
        }
        //PlotName property

        //PlotKadastrNumber property
        [Attributes.FormVisual("Кадастровый номер участка")]
        public string PlotKadastrNumber
        {
            get
            {
                if (GetErrors(nameof(PlotKadastrNumber)) != null)
                {
                    return (string)_PlotKadastrNumber.Get();
                }
                else
                {
                    return _PlotKadastrNumber_Not_Valid;
                }
            }
            set
            {
                _PlotKadastrNumber_Not_Valid = value;
                if (GetErrors(nameof(PlotKadastrNumber)) != null)
                {
                    _PlotKadastrNumber.Set(_PlotKadastrNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PlotKadastrNumber));
            }
        }
        private IDataLoadEngine _PlotKadastrNumber;
        private string _PlotKadastrNumber_Not_Valid = "";
        private void PlotKadastrNumber_Validation(string value)//TODO
        {
            ClearErrors(nameof(PlotKadastrNumber));
        }
        //PlotKadastrNumber property

        //PlotCode property
        [Attributes.FormVisual("Код участка")]
        public string PlotCode
        {
            get
            {
                if (GetErrors(nameof(PlotCode)) != null)
                {
                    return (string)_PlotCode.Get();
                }
                else
                {
                    return _PlotCode_Not_Valid;
                }
            }
            set
            {
                _PlotCode_Not_Valid = value;
                if (GetErrors(nameof(PlotCode)) != null)
                {
                    _PlotCode.Set(_PlotCode_Not_Valid);
                }
                OnPropertyChanged(nameof(PlotCode));
            }
        }
        private IDataLoadEngine _PlotCode; //6 symbols code
        private string _PlotCode_Not_Valid = ""; //6 symbols code
        private void PlotCode_Validation(string value)//TODO
        {
            ClearErrors(nameof(PlotCode));
        }
        //PlotCode property

        //InfectedArea property
        [Attributes.FormVisual("Площадь загрязненной территории, кв. м")]
        public int InfectedArea
        {
            get
            {
                if (GetErrors(nameof(InfectedArea)) != null)
                {
                    return (int)_InfectedArea.Get();
                }
                else
                {
                    return _InfectedArea_Not_Valid;
                }
            }
            set
            {
                _InfectedArea_Not_Valid = value;
                if (GetErrors(nameof(InfectedArea)) != null)
                {
                    _InfectedArea.Set(_InfectedArea_Not_Valid);
                }
                OnPropertyChanged(nameof(InfectedArea));
            }
        }
        private IDataLoadEngine _InfectedArea;
        private int _InfectedArea_Not_Valid = -1;
        private void InfectedArea_Validation(int value)//TODO
        {
            ClearErrors(nameof(InfectedArea));
        }
        //InfectedArea property

        //Radionuclids property
        [Attributes.FormVisual("Наименования радионуклидов")]
        public string Radionuclids
        {
            get
            {
                if (GetErrors(nameof(Radionuclids)) != null)
                {
                    return (string)_Radionuclids.Get();
                }
                else
                {
                    return _Radionuclids_Not_Valid;
                }
            }
            set
            {
                _Radionuclids_Not_Valid = value;
                if (GetErrors(nameof(Radionuclids)) != null)
                {
                    _Radionuclids.Set(_Radionuclids_Not_Valid);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        private IDataLoadEngine _Radionuclids;//If change this change validation
        private string _Radionuclids_Not_Valid = "";
        private void Radionuclids_Validation()//TODO
        {
            ClearErrors(nameof(Radionuclids));
        }
        //Radionuclids property

        private string _radionuclidNameNote = "";
        public string RadionuclidNameNote
        {
            get { return _radionuclidNameNote; }
            set
            {
                _radionuclidNameNote = value;
                OnPropertyChanged("RadionuclidNameNote");
            }
        }

        //SpecificActivityOfPlot property
        [Attributes.FormVisual("Удельная активность, Бк/г")]
        public string SpecificActivityOfPlot
        {
            get
            {
                if (GetErrors(nameof(SpecificActivityOfPlot)) != null)
                {
                    return (string)_SpecificActivityOfPlot.Get();
                }
                else
                {
                    return _SpecificActivityOfPlot_Not_Valid;
                }
            }
            set
            {
                _SpecificActivityOfPlot_Not_Valid = value;
                if (GetErrors(nameof(SpecificActivityOfPlot)) != null)
                {
                    _SpecificActivityOfPlot.Set(_SpecificActivityOfPlot_Not_Valid);
                }
                OnPropertyChanged(nameof(SpecificActivityOfPlot));
            }
        }
        private IDataLoadEngine _SpecificActivityOfPlot;
        private string _SpecificActivityOfPlot_Not_Valid = "";
        private void SpecificActivityOfPlot_Validation(string value)//TODO
        {
            //ClearErrors(nameof(SpecificActivityOfPlot));
            //var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //   NumberStyles.AllowExponent;
            //try
            //{
            //    if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
            //        AddError(nameof(SpecificActivityOfPlot), "Число должно быть больше нуля");
            //}
            //catch
            //{
            //    AddError(nameof(SpecificActivityOfPlot), "Недопустимое значение");
            //}
        }
        //SpecificActivityOfPlot property

        //SpecificActivityOfLiquidPart property
        [Attributes.FormVisual("Удельная активность жидкой части, Бк/г")]
        public string SpecificActivityOfLiquidPart
        {
            get
            {
                if (GetErrors(nameof(SpecificActivityOfLiquidPart)) != null)
                {
                    return (string)_SpecificActivityOfLiquidPart.Get();
                }
                else
                {
                    return _SpecificActivityOfLiquidPart_Not_Valid;
                }
            }
            set
            {
                _SpecificActivityOfLiquidPart_Not_Valid = value;
                if (GetErrors(nameof(SpecificActivityOfLiquidPart)) != null)
                {
                    _SpecificActivityOfLiquidPart.Set(_SpecificActivityOfLiquidPart_Not_Valid);
                }
                OnPropertyChanged(nameof(SpecificActivityOfLiquidPart));
            }
        }
        private IDataLoadEngine _SpecificActivityOfLiquidPart;
        private string _SpecificActivityOfLiquidPart_Not_Valid = "";
        private void SpecificActivityOfLiquidPart_Validation(string value)//TODO
        {
            //ClearErrors(nameof(SpecificActivityOfLiquidPart));
            //var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //   NumberStyles.AllowExponent;
            //try
            //{
            //    if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
            //        AddError(nameof(SpecificActivityOfLiquidPart), "Число должно быть больше нуля");
            //}
            //catch
            //{
            //    AddError(nameof(SpecificActivityOfLiquidPart), "Недопустимое значение");
            //}
        }
        //SpecificActivityOfLiquidPart property

        //SpecificActivityOfDensePart property
        [Attributes.FormVisual("Удельная активность твердой части, Бк/г")]
        public string SpecificActivityOfDensePart
        {
            get
            {
                if (GetErrors(nameof(SpecificActivityOfDensePart)) != null)
                {
                    return (string)_SpecificActivityOfDensePart.Get();
                }
                else
                {
                    return _SpecificActivityOfDensePart_Not_Valid;
                }
            }
            set
            {
                _SpecificActivityOfDensePart_Not_Valid = value;
                if (GetErrors(nameof(SpecificActivityOfDensePart)) != null)
                {
                    _SpecificActivityOfDensePart.Set(_SpecificActivityOfDensePart_Not_Valid);
                }
                OnPropertyChanged(nameof(SpecificActivityOfDensePart));
            }
        }
        private IDataLoadEngine _SpecificActivityOfDensePart;
        private string _SpecificActivityOfDensePart_Not_Valid = "";
        private void SpecificActivityOfDensePart_Validation(string value)//TODO
        {
            //ClearErrors(nameof(SpecificActivityOfDensePart));
            //var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //   NumberStyles.AllowExponent;
            //try
            //{
            //    if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
            //        AddError(nameof(SpecificActivityOfDensePart), "Число должно быть больше нуля");
            //}
            //catch
            //{
            //    AddError(nameof(SpecificActivityOfDensePart), "Недопустимое значение");
            //}
        }
        //SpecificActivityOfDensePart property
    }
}
