using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 2.9: Активность радионуклидов, отведенных со сточными водами")]
    public class Form29 : Form2
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 8;

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

        private byte _correctionNumber = 255;

        //Data Validation
        private void CorrectionNumber_Validation(byte value)//TODO
        {
            ClearErrors(nameof(CorrectionNumber));
            //Пример
            if (value < 10)
                AddError(nameof(CorrectionNumber), "Значение должно быть больше 10.");
        }
        //Data Validation

        [Attributes.FormVisual("Номер корректировки")]
        public byte CorrectionNumber
        {
            get { return _correctionNumber; }
            set
            {
                _correctionNumber = value;
                CorrectionNumber_Validation(value);
                OnPropertyChanged("CorrectionNumber");
            }
        }

        private string _wasteSourceName = "";
        [Attributes.FormVisual("Наименование, номер выпуска сточных вод")]
        public string WasteSourceName
        {
            get { return _wasteSourceName; }
            set
            {
                _wasteSourceName = value;
                OnPropertyChanged("WasteSourceName");
            }
        }

        private string _radionuclidName = "";
        [Attributes.FormVisual("Наименование радионуклида")]
        public string RadionuclidName
        {
            get { return _radionuclidName; }
            set
            {
                _radionuclidName = value;
                OnPropertyChanged("RadionuclidName");
            }
        }

        private string _allowedActivity = "";
        [Attributes.FormVisual("Допустимая активность радионуклида, Бк")]
        public string AllowedActivity
        {
            get { return _allowedActivity; }
            set
            {
                _allowedActivity = value;
                OnPropertyChanged("AllowedActivity");
            }
        }

        private string _allowedActivityNote = "";
        public string AllowedActivityNote
        {
            get { return _allowedActivityNote; }
            set
            {
                _allowedActivityNote = value;
                OnPropertyChanged("AllowedActivityNote");
            }
        }

        private string _factedActivity = "";
        [Attributes.FormVisual("Фактическая активность радионуклида, Бк")]
        public string FactedActivity
        {
            get { return _factedActivity; }
            set
            {
                _factedActivity = value;
                OnPropertyChanged("FactedActivity");
            }
        }

        private string _factedActivityNote = "";
        public string FactedActivityNote
        {
            get { return _factedActivityNote; }
            set
            {
                _factedActivityNote = value;
                OnPropertyChanged("FactedActivityNote");
            }
        }
    }
}
