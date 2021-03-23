using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 2.4: Постановка на учет и снятие с учета РВ, содержащихся в отработавшем ядерном топливе")]
    public class Form24: Form2
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 26;

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

        private string _codeOYAT = "";
        [Attributes.FormVisual("Код ОЯТ")]
        public string CodeOYAT
        {
            get { return _codeOYAT; }
            set
            {
                _codeOYAT = value;
                OnPropertyChanged("CodeOYAT");
            }
        }

        private string _codeOYATnote = "";
        public string CodeOYATnote
        {
            get { return _codeOYATnote; }
            set
            {
                _codeOYATnote = value;
                OnPropertyChanged("CodeOYATnote");
            }
        }

        private string _fcpNumber = "";

        private void FcpNuber_Validation(string value)//TODO
        {
            ClearErrors(nameof(FcpNumber));
        }

        [Attributes.FormVisual("Номер мероприятия ФЦП")]
        public string FcpNumber
        {
            get { return _fcpNumber; }
            set
            {
                _fcpNumber = value;
                FcpNuber_Validation(value);
                OnPropertyChanged("FcpNumber");
            }
        }

        private double _massCreated = -1;
        private void MassCreated_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("Масса образованного, т")]
        public double MassCreated
        {
            get { return _massCreated; }
            set
            {
                _massCreated = value;
                MassCreated_Validation(value);
                OnPropertyChanged("MassCreated");
            }
        }

        private int _quantityCreated = -1;  // positive int.
        private void QuantityCreated_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityCreated));
            if (value <= 0)
                AddError(nameof(QuantityCreated), "Недопустимое значение");
        }

        [Attributes.FormVisual("Количество образованного, шт.")]
        public int QuantityCreated
        {
            get { return _quantityCreated; }
            set
            {
                _quantityCreated = value;
                QuantityCreated_Validation(value);
                OnPropertyChanged("QuantityCreated");
            }
        }

        private int _quantityCreatedNote = -1;  // positive int.
        public int QuantityCreatedNote
        {
            get { return _quantityCreatedNote; }
            set
            {
                _quantityCreatedNote = value;
                OnPropertyChanged("QuantityCreatedNote");
            }
        }

        private double _massFromAnothers = -1;
        private void MassFromAnothers_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("Масса поступившего от сторонних, т")]
        public double MassFromAnothers
        {
            get { return _massFromAnothers; }
            set
            {
                _massFromAnothers = value;
                MassFromAnothers_Validation(value);
                OnPropertyChanged("MassFromAnothers");
            }
        }

        private int _quantityFromAnothers = -1;  // positive int.
        private void QuantityFromAnothers_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityFromAnothers));
            if (value <= 0)
                AddError(nameof(QuantityFromAnothers), "Недопустимое значение");
        }

        [Attributes.FormVisual("Количество поступившего от сторонних, шт.")]
        public int QuantityFromAnothers
        {
            get { return _quantityFromAnothers; }
            set
            {
                _quantityFromAnothers = value;
                QuantityFromAnothers_Validation(value);
                OnPropertyChanged("QuantityFromAnothers");
            }
        }

        private int _quantityFromAnothersNote = -1;  // positive int.
        public int QuantityFromAnothersNote
        {
            get { return _quantityFromAnothersNote; }
            set
            {
                _quantityFromAnothersNote = value;
                OnPropertyChanged("QuantityFromAnothersNote");
            }
        }

        private double _massFromAnothersImported = -1;
        private void MassFromAnothersImported_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("Масса импортированного от сторонних, т")]
        public double MassFromAnothersImported
        {
            get { return _massFromAnothersImported; }
            set
            {
                _massFromAnothersImported = value;
                MassFromAnothersImported_Validation(value);
                OnPropertyChanged("MassFromAnothersImported");
            }
        }

        private int _quantityFromAnothersImported = -1;  // positive int.
        private void QuantityFromAnothersImported_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityFromAnothersImported));
            if (value <= 0)
                AddError(nameof(QuantityFromAnothersImported), "Недопустимое значение");
        }

        [Attributes.FormVisual("Количество импортированного от сторонних, шт.")]
        public int QuantityFromAnothersImported
        {
            get { return _quantityFromAnothersImported; }
            set
            {
                _quantityFromAnothersImported = value;
                QuantityFromAnothersImported_Validation(value);
                OnPropertyChanged("QuantityFromAnothersImported");
            }
        }

        private int _quantityFromAnothersImportedNote = -1;  // positive int.
        public int QuantityFromAnothersImportedNote
        {
            get { return _quantityFromAnothersImportedNote; }
            set
            {
                _quantityFromAnothersImportedNote = value;
                OnPropertyChanged("QuantityFromAnothersImportedNote");
            }
        }

        private double _massAnotherReasons = -1;
        private void MassAnotherReasons_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("Масса поставленного на учет по другим причинам, т")]
        public double MassAnotherReasons
        {
            get { return _massAnotherReasons; }
            set
            {
                _massAnotherReasons = value;
                MassAnotherReasons_Validation(value);
                OnPropertyChanged("MassAnotherReasons");
            }
        }

        private int _quantityAnotherReasons = -1;  // positive int.
        private void QuantityAnotherReasons_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityAnotherReasons));
            if (value <= 0)
                AddError(nameof(QuantityAnotherReasons), "Недопустимое значение");
        }

        [Attributes.FormVisual("Количество поставленного на учет по другим причинам, шт.")]
        public int QuantityAnotherReasons
        {
            get { return _quantityAnotherReasons; }
            set
            {
                _quantityAnotherReasons = value;
                QuantityAnotherReasons_Validation(value);
                OnPropertyChanged("QuantityAnotherReasons");
            }
        }

        private int _quantityAnotherReasonsNote = -1;  // positive int.
        public int QuantityAnotherReasonsNote
        {
            get { return _quantityAnotherReasonsNote; }
            set
            {
                _quantityAnotherReasonsNote = value;
                OnPropertyChanged("QuantityAnotherReasonsNote");
            }
        }

        private double _massTransferredToAnother = -1;
        private void MassTransferredToAnother_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("Масса переданного сторонним, т")]
        public double MassTransferredToAnother
        {
            get { return _massTransferredToAnother; }
            set
            {
                _massTransferredToAnother = value;
                MassTransferredToAnother_Validation(value);
                OnPropertyChanged("MassTransferredToAnother");
            }
        }

        private int _quantityTransferredToAnother = -1;  // positive int.
        private void QuantityTransferredToAnother_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityTransferredToAnother));
            if (value <= 0)
                AddError(nameof(QuantityTransferredToAnother), "Недопустимое значение");
        }

        [Attributes.FormVisual("Количество переданного сторонним, шт.")]
        public int QuantityTransferredToAnother
        {
            get { return _quantityTransferredToAnother; }
            set
            {
                _quantityTransferredToAnother = value;
                QuantityTransferredToAnother_Validation(value);
                OnPropertyChanged("QuantityTransferredToAnother");
            }
        }

        private int _quantityTransferredToAnotherNote = -1;  // positive int.
        public int QuantityTransferredToAnotherNote
        {
            get { return _quantityTransferredToAnotherNote; }
            set
            {
                _quantityTransferredToAnotherNote = value;
                OnPropertyChanged("QuantityTransferredToAnotherNote");
            }
        }

        private double _massRefined = -1;
        private void MassRefined_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("Масса переработанного, т")]
        public double MassRefined
        {
            get { return _massRefined; }
            set
            {
                _massRefined = value;
                MassRefined_Validation(value);
                OnPropertyChanged("MassRefined");
            }
        }

        private int _quantityRefined = -1;  // positive int.
        private void QuantityRefined_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityRefined));
            if (value <= 0)
                AddError(nameof(QuantityRefined), "Недопустимое значение");
        }

        [Attributes.FormVisual("Количество переработанного, шт.")]
        public int QuantityRefined
        {
            get { return _quantityRefined; }
            set
            {
                _quantityRefined = value;
                QuantityRefined_Validation(value);
                OnPropertyChanged("QuantityRefined");
            }
        }

        private int _quantityRefinedNote = -1;  // positive int.
        public int QuantityRefinedNote
        {
            get { return _quantityRefinedNote; }
            set
            {
                _quantityRefinedNote = value;
                OnPropertyChanged("QuantityRefinedNote");
            }
        }

        private double _massRemovedFromAccount = -1;
        private void MassRemovedFromAccount_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("Масса снятого с учета, т")]
        public double MassRemovedFromAccount
        {
            get { return _massRemovedFromAccount; }
            set
            {
                _massRemovedFromAccount = value;
                MassRemovedFromAccount_Validation(value);
                OnPropertyChanged("MassRemovedFromAccount");
            }
        }

        private int _quantityRemovedFromAccount = -1;  // positive int.
        private void QuantityRemovedFromAccount_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityRemovedFromAccount));
            if (value <= 0)
                AddError(nameof(QuantityRemovedFromAccount), "Недопустимое значение");
        }

        [Attributes.FormVisual("Количество снятого с учета, шт.")]
        public int QuantityRemovedFromAccount
        {
            get { return _quantityRemovedFromAccount; }
            set
            {
                _quantityRemovedFromAccount = value;
                QuantityRemovedFromAccount_Validation(value);
                OnPropertyChanged("QuantityRemovedFromAccount");
            }
        }

        private int _quantityRemovedFromAccountNote = -1;  // positive int.
        public int QuantityRemovedFromAccountNote
        {
            get { return _quantityRemovedFromAccountNote; }
            set
            {
                _quantityRemovedFromAccountNote = value;
                OnPropertyChanged("QuantityRemovedFromAccountNote");
            }
        }
    }
}
