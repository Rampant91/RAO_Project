using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 5.3: Сведения о поступлении/передаче в подведомственные организации от сторонних организаций, расходовании и переводе в РАО ОРИ")]
    public class Form53 : Form5
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 13;

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

        [Attributes.FormVisual("Код")]
        public short OperationCode
        {
            get
            {
                if (GetErrors(nameof(OperationCode)) != null)
                {
                    return _OperationCode;
                }
                else
                {
                    return _OperationCode_Not_Valid;
                }
            }
            set
            {
                _OperationCode_Not_Valid = value;
                if (OperationCode_Validation())
                {
                    _OperationCode = _OperationCode_Not_Valid;
                }
                OnPropertyChanged(nameof(OperationCode));
            }
        }
        private short _OperationCode = -1;
        private short _OperationCode_Not_Valid = -1;
        private bool OperationCode_Validation()
        {
            return true;
        }

        private int _typeOfAccountedParts = -1; //1 or 2

        private void TypeOfAccountedParts_Validation(int value)//Ready
        {
            ClearErrors(nameof(TypeOfAccountedParts));
            if ((value != 1) && (value != 2))
                AddError(nameof(TypeOfAccountedParts), "Недопустимое значение");
        }

        [Attributes.FormVisual("Тип учетных единиц")]
        public int TypeOfAccountedParts
        {
            get { return _typeOfAccountedParts; }
            set
            {
                _typeOfAccountedParts = value;
                TypeOfAccountedParts_Validation(value);
                OnPropertyChanged("TypeOfAccountedParts");
            }
        }

        private int _kindOri = -1;

        private void KindOri_Validation(int value)//TODO
        {
        }

        [Attributes.FormVisual("Вид ОРИ")]
        public int KindOri
        {
            get { return _kindOri; }
            set
            {
                _kindOri = value;
                KindOri_Validation(value);
                OnPropertyChanged("KindOri");
            }
        }

        private byte _aggregateState = 0;

        private void AggregateState_Validation(byte value)//Ready
        {
            ClearErrors(nameof(AggregateState));
            if ((value != 1) && (value != 2) && (value != 3))
                AddError(nameof(AggregateState), "Недопустимое значение");
        }

        [Attributes.FormVisual("Агрегатное состояние")]
        public byte AggregateState//1 2 3
        {
            get { return _aggregateState; }
            set
            {
                _aggregateState = value;
                AggregateState_Validation(value);
                OnPropertyChanged("AggregateState");
            }
        }

        private string _providerOrRecieverOKPO = "";

        private void ProviderOrRecieverOKPO_Validation(string value)//TODO
        {
        }

        [Attributes.FormVisual("ОКПО поставщика/получателя")]
        public string ProviderOrRecieverOKPO
        {
            get { return _providerOrRecieverOKPO; }
            set
            {
                _providerOrRecieverOKPO = value;
                ProviderOrRecieverOKPO_Validation(value);
                OnPropertyChanged("ProviderOrRecieverOKPO");
            }
        }

        private string _providerOrRecieverOKPONote = "";
        public string ProviderOrRecieverOKPONote
        {
            get { return _providerOrRecieverOKPONote; }
            set
            {
                _providerOrRecieverOKPONote = value;
                OnPropertyChanged("ProviderOrRecieverOKPONote");
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

        private int _quantity = -1;  // positive int.

        private void Quantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(Quantity));
            if (value <= 0)
                AddError(nameof(Quantity), "Недопустимое значение");
        }

        [Attributes.FormVisual("Количество, шт.")]
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                Quantity_Validation(value);
                OnPropertyChanged("Quantity");
            }
        }

        private double _volume = -1;

        private void Volume_Validation(double value)//TODO
        {
        }

        [Attributes.FormVisual("Объем, куб. м")]
        public double Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                Volume_Validation(value);
                OnPropertyChanged("Volume");
            }
        }

        private double _mass = -1;

        private void Mass_Validation(double value)//TODO
        {
        }

        [Attributes.FormVisual("Масса, кг")]
        public double Mass
        {
            get { return _mass; }
            set
            {
                _mass = value;
                Mass_Validation(value);
                OnPropertyChanged("Mass");
            }
        }
    }
}
