using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 1.9: Сведения о результатах инвентаризации РВ не в составе ЗРИ")]
    public class Form19: Form
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 11;

        private byte _correctionNumber = 255;

        private void CorrectionNumber_Validation(byte value)//TODO
        {
            ClearErrors(nameof(CorrectionNumber));
            //Пример
            if (value < 10)
                AddError(nameof(CorrectionNumber), "Значение должно быть больше 10.");
        }

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

        private short _operationCode = -1;

        private void OperationCode_Validation(short value)//TODO
        {
        }

        [Attributes.FormVisual("Код операции")]
        public short OperationCode
        {
            get { return _operationCode; }
            set
            {
                _operationCode = value;
                OperationCode_Validation(value);
                OnPropertyChanged("OperationCode");
            }
        }

        private DateTime _operationDate = DateTime.MinValue;

        private void OperationDate_Validation(DateTime value)//Ready
        {
        }

        [Attributes.FormVisual("Дата операции")]
        public DateTime OperationDate
        {
            get { return _operationDate; }
            set
            {
                _operationDate = value;
                OperationDate_Validation(value);
                OnPropertyChanged("OperationDate");
            }
        }

        private byte _documentVid = 255;

        private void DocumentVid_Validation(byte value)//TODO
        {
        }

        [Attributes.FormVisual("Вид документа")]
        public byte DocumentVid
        {
            get { return _documentVid; }
            set
            {
                _documentVid = value;
                DocumentVid_Validation(value);
                OnPropertyChanged("DocumentVid");
            }
        }

        private string _documentNumber = "";

        private void DocumentNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(DocumentNumber));
        }

        [Attributes.FormVisual("Номер документа")]
        public string DocumentNumber
        {
            get { return _documentNumber; }
            set
            {
                _documentNumber = value;
                DocumentNumber_Validation(value);
                OnPropertyChanged("DocumentNumber");
            }
        }

        private string _documentNumberRecoded = "";
        public string DocumentNumberRecoded
        {
            get { return _documentNumberRecoded; }
            set
            {
                _documentNumberRecoded = value;
                OnPropertyChanged("DocumentNumberRecoded");
            }
        }

        private DateTime _documentDate = DateTime.MinValue;//if change this change validation

        private void DocumentDate_Validation(DateTime value)//Ready
        {
            ClearErrors(nameof(DocumentDate));
        }

        [Attributes.FormVisual("Дата документа")]
        public DateTime DocumentDate
        {
            get { return _documentDate; }
            set
            {
                _documentDate = value;
                DocumentDate_Validation(value);
                OnPropertyChanged("DocumentDate");
            }
        }

        private short _codeTypeAccObject = 0;

        private void CodeTypeAccObject_Validation(short value)//TODO
        {
        }

        [Attributes.FormVisual("Код типа объектов учета")]
        public short CodeTypeAccObject
        {
            get { return _codeTypeAccObject; }
            set
            {
                _codeTypeAccObject = value;
                CodeTypeAccObject_Validation(value);
                OnPropertyChanged("CodeTypeAccObject");
            }
        }

        private string _radionuclids = "";//If change this change validation

        private void Radionuclids_Validation(string value)//TODO
        {
            ClearErrors(nameof(Radionuclids));
        }

        [Attributes.FormVisual("Радионуклиды")]
        public string Radionuclids
        {
            get { return _radionuclids; }
            set
            {
                _radionuclids = value;
                Radionuclids_Validation(value);
                OnPropertyChanged("Radionuclids");
            }
        }

        private string _activity = "";

        private void Activity_Validation(string value)//Ready
        {
            ClearErrors(nameof(Activity));
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(Activity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(Activity), "Недопустимое значение");
            }
        }

        [Attributes.FormVisual("Активность, Бк")]
        public string Activity
        {
            get { return _activity; }
            set
            {
                _activity = value;
                Activity_Validation(value);
                OnPropertyChanged("Activity");
            }
        }
    }
}
