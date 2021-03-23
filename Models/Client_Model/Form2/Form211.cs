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
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 11;

        private int _numberInOrder = -1;
        [Attributes.FormVisual("№ п/п")]
        public int NumberInOrder
        {
            get { return _numberInOrder; }
            set
            {
                _numberInOrder = value;
                OnPropertyChanged("NumberInOrder");
            }
        }

        //CorrectionNumber property
        [Attributes.FormVisual("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    return (byte)_CorrectionNumber.Get();
                }
                else
                {
                    return _CorrectionNumber_Not_Valid;
                }
            }
            set
            {
                _CorrectionNumber_Not_Valid = value;
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    _CorrectionNumber.Set(_CorrectionNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private IDataLoadEngine _CorrectionNumber;
        private byte _CorrectionNumber_Not_Valid = 255;
        private void CorrectionNumber_Validation()
        {
            ClearErrors(nameof(CorrectionNumber));
        }
        //CorrectionNumber property

        private string _plotName = "";
        [Attributes.FormVisual("Наименование участка")]
        public string PlotName
        {
            get { return _plotName; }
            set
            {
                _plotName = value;
                OnPropertyChanged("PlotName");
            }
        }

        private string _plotKadastrNumber = "";
        [Attributes.FormVisual("Кадастровый номер участка")]
        public string PlotKadastrNumber
        {
            get { return _plotKadastrNumber; }
            set
            {
                _plotKadastrNumber = value;
                OnPropertyChanged("PlotKadastrNumber");
            }
        }

        private string _plotCode = ""; //6 symbols code
        public string PlotCode
        {
            get { return _plotCode; }
            set
            {
                _plotCode = value;
                OnPropertyChanged("PlotCode");
            }
        }

        private int _infectedArea = -1;
        [Attributes.FormVisual("Площадь загрязненной территории, кв. м")]
        public int InfectedArea
        {
            get { return _infectedArea; }
            set
            {
                _infectedArea = value;
                OnPropertyChanged("InfectedArea");
            }
        }

        private string _radionuclidName = "";
        [Attributes.FormVisual("Наименования радионуклидов")]
        public string RadionuclidName
        {
            get { return _radionuclidName; }
            set
            {
                _radionuclidName = value;
                OnPropertyChanged("RadionuclidName");
            }
        }

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
