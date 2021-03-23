using System;
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

        [Attributes.FormVisual("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    return _correctionNumber;
                }
                else
                {
                    return _correctionNumber_Not_Valid;
                }
            }
            set
            {
                _correctionNumber_Not_Valid = value;
                if (CorrectionNumber_Validation())
                {
                    _correctionNumber = _correctionNumber_Not_Valid;
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private byte _correctionNumber = 255;
        private byte _correctionNumber_Not_Valid = 255;
        private bool CorrectionNumber_Validation()
        {
            return true;
            //ClearErrors(nameof(CorrectionNumber));
            ////Пример
            //if (value < 10)
            //    AddError(nameof(CorrectionNumber), "Значение должно быть больше 10.");
        }

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

        private string _specificActivityOfPlot = "";
        [Attributes.FormVisual("Удельная активность земельного участка, Бк/г")]
        public string SpecificActivityOfPlot
        {
            get { return _specificActivityOfPlot; }
            set
            {
                _specificActivityOfPlot = value;
                OnPropertyChanged("SpecificActivityOfPlot");
            }
        }

        private string _specificActivityOfLiquidPart = "";
        [Attributes.FormVisual("Удельная активность жидкой фазы водного объекта, Бк/г")]
        public string SpecificActivityOfLiquidPart
        {
            get { return _specificActivityOfLiquidPart; }
            set
            {
                _specificActivityOfLiquidPart = value;
                OnPropertyChanged("SpecificActivityOfLiquidPart");
            }
        }

        private string _specificActivityOfDensePart = "";
        [Attributes.FormVisual("Удельная активность донных отложений водного объекта, Бк/г")]
        public string SpecificActivityOfDensePart
        {
            get { return _specificActivityOfDensePart; }
            set
            {
                _specificActivityOfDensePart = value;
                OnPropertyChanged("SpecificActivityOfDensePart");
            }
        }
    }
}
