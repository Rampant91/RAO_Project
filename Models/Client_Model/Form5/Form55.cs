using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 5.5: Сведения о поступлении/передаче в подведомственные организации от сторонних организаций и переводе в РАО изделий из обедненного урана")]
    public class Form55 : Form5
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 8;

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

        private string _name = "";

        private void Name_Validation(string value)//TODO
        {
        }

        [Attributes.FormVisual("Наименование")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                Name_Validation(value);
                OnPropertyChanged("Name");
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
