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
        public override string FormNum { get { return "2.4"; } }
        public override void Object_Validation()
        {

        }
        public override int NumberOfFields { get; } = 26;

        //CodeOYAT property
        [Attributes.FormVisual("Код ОЯТ")]
        public string CodeOYAT
        {
            get
            {
                if (GetErrors(nameof(CodeOYAT)) != null)
                {
                    return (string)_CodeOYAT.Get();
                }
                else
                {
                    return _CodeOYAT_Not_Valid;
                }
            }
            set
            {
                _CodeOYAT_Not_Valid = value;
                if (GetErrors(nameof(CodeOYAT)) != null)
                {
                    _CodeOYAT.Set(_CodeOYAT_Not_Valid);
                }
                OnPropertyChanged(nameof(CodeOYAT));
            }
        }
        private IDataLoadEngine _CodeOYAT;
        private string _CodeOYAT_Not_Valid = "";
        private void CodeOYAT_Validation()
        {
            ClearErrors(nameof(CodeOYAT));
        }
        //CodeOYAT property

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

        //FcpNumber property
        [Attributes.FormVisual("Номер мероприятия ФЦП")]
        public string FcpNumber
        {
            get
            {
                if (GetErrors(nameof(FcpNumber)) != null)
                {
                    return (string)_FcpNumber.Get();
                }
                else
                {
                    return _FcpNumber_Not_Valid;
                }
            }
            set
            {
                _FcpNumber_Not_Valid = value;
                if (GetErrors(nameof(FcpNumber)) != null)
                {
                    _FcpNumber.Set(_FcpNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }
        private IDataLoadEngine _FcpNumber;
        private string _FcpNumber_Not_Valid = "";
        private void FcpNuber_Validation(string value)//TODO
        {
            ClearErrors(nameof(FcpNumber));
        }
        //FcpNumber property

        //MassCreated Property
        [Attributes.FormVisual("Масса образованного, т")]
        public double MassCreated
        {
            get
            {
                if (GetErrors(nameof(MassCreated)) != null)
                {
                    return (double)_MassCreated.Get();
                }
                else
                {
                    return _MassCreated_Not_Valid;
                }
            }
            set
            {
                _MassCreated_Not_Valid = value;
                if (GetErrors(nameof(MassCreated)) != null)
                {
                    _MassCreated.Set(_MassCreated_Not_Valid);
                }
                OnPropertyChanged(nameof(MassCreated));
            }
        }
        private IDataLoadEngine _MassCreated;
        private double _MassCreated_Not_Valid = -1;
        private void MassCreated_Validation()//TODO
        {
            ClearErrors(nameof(MassCreated));
        }
        //MassCreated Property

        //QuantityCreated property
        [Attributes.FormVisual("Количество образованного, шт.")]
        public int QuantityCreated
        {
            get
            {
                if (GetErrors(nameof(QuantityCreated)) != null)
                {
                    return (int)_QuantityCreated.Get();
                }
                else
                {
                    return _QuantityCreated_Not_Valid;
                }
            }
            set
            {
                _QuantityCreated_Not_Valid = value;
                if (GetErrors(nameof(QuantityCreated)) != null)
                {
                    _QuantityCreated.Set(_QuantityCreated_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityCreated));
            }
        }
        private IDataLoadEngine _QuantityCreated;  // positive int.
        private int _QuantityCreated_Not_Valid = -1;
        private void QuantityCreated_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityCreated));
            if (value <= 0)
                AddError(nameof(QuantityCreated), "Недопустимое значение");
        }
        //QuantityCreated property

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

        //MassFromAnothers Property
        [Attributes.FormVisual("Масса поступившего от сторонних, т")]
        public double MassFromAnothers
        {
            get
            {
                if (GetErrors(nameof(MassFromAnothers)) != null)
                {
                    return (double)_MassFromAnothers.Get();
                }
                else
                {
                    return _MassFromAnothers_Not_Valid;
                }
            }
            set
            {
                _MassFromAnothers_Not_Valid = value;
                if (GetErrors(nameof(MassFromAnothers)) != null)
                {
                    _MassFromAnothers.Set(_MassFromAnothers_Not_Valid);
                }
                OnPropertyChanged(nameof(MassFromAnothers));
            }
        }
        private IDataLoadEngine _MassFromAnothers;
        private double _MassFromAnothers_Not_Valid = -1;
        private void MassFromAnothers_Validation()//TODO
        {
            ClearErrors(nameof(MassFromAnothers));
        }
        //MassFromAnothers Property

        //QuantityFromAnothers property
        [Attributes.FormVisual("Количество поступившего от сторонних, шт.")]
        public int QuantityFromAnothers
        {
            get
            {
                if (GetErrors(nameof(QuantityFromAnothers)) != null)
                {
                    return (int)_QuantityFromAnothers.Get();
                }
                else
                {
                    return _QuantityFromAnothers_Not_Valid;
                }
            }
            set
            {
                _QuantityFromAnothers_Not_Valid = value;
                if (GetErrors(nameof(QuantityFromAnothers)) != null)
                {
                    _QuantityFromAnothers.Set(_QuantityFromAnothers_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityFromAnothers));
            }
        }
        private IDataLoadEngine _QuantityFromAnothers;  // positive int.
        private int _QuantityFromAnothers_Not_Valid = -1;
        private void QuantityFromAnothers_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityFromAnothers));
            if (value <= 0)
                AddError(nameof(QuantityFromAnothers), "Недопустимое значение");
        }
        //QuantityFromAnothers property

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

        //MassFromAnothersImported Property
        [Attributes.FormVisual("Масса импортированного от сторонних, т")]
        public double MassFromAnothersImported
        {
            get
            {
                if (GetErrors(nameof(MassFromAnothersImported)) != null)
                {
                    return (double)_MassFromAnothersImported.Get();
                }
                else
                {
                    return _MassFromAnothersImported_Not_Valid;
                }
            }
            set
            {
                _MassFromAnothersImported_Not_Valid = value;
                if (GetErrors(nameof(MassFromAnothersImported)) != null)
                {
                    _MassFromAnothersImported.Set(_MassFromAnothersImported_Not_Valid);
                }
                OnPropertyChanged(nameof(MassFromAnothersImported));
            }
        }
        private IDataLoadEngine _MassFromAnothersImported;
        private double _MassFromAnothersImported_Not_Valid = -1;
        private void MassFromAnothersImported_Validation()//TODO
        {
            ClearErrors(nameof(MassFromAnothersImported));
        }
        //MassFromAnothersImported Property

        //QuantityFromAnothersImported property
        [Attributes.FormVisual("Количество импортированного от сторонних, шт.")]
        public int QuantityFromAnothersImported
        {
            get
            {
                if (GetErrors(nameof(QuantityFromAnothersImported)) != null)
                {
                    return (int)_QuantityFromAnothersImported.Get();
                }
                else
                {
                    return _QuantityFromAnothersImported_Not_Valid;
                }
            }
            set
            {
                _QuantityFromAnothersImported_Not_Valid = value;
                if (GetErrors(nameof(QuantityFromAnothersImported)) != null)
                {
                    _QuantityFromAnothersImported.Set(_QuantityFromAnothersImported_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityFromAnothersImported));
            }
        }
        private IDataLoadEngine _QuantityFromAnothersImported;  // positive int.
        private int _QuantityFromAnothersImported_Not_Valid = -1;
        private void QuantityFromAnothersImported_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityFromAnothersImported));
            if (value <= 0)
                AddError(nameof(QuantityFromAnothersImported), "Недопустимое значение");
        }
        //QuantityFromAnothersImported property

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

        //MassAnotherReasons Property
        [Attributes.FormVisual("Масса поставленного на учет по другим причинам, т")]
        public double MassAnotherReasons
        {
            get
            {
                if (GetErrors(nameof(MassAnotherReasons)) != null)
                {
                    return (double)_MassAnotherReasons.Get();
                }
                else
                {
                    return _MassAnotherReasons_Not_Valid;
                }
            }
            set
            {
                _MassAnotherReasons_Not_Valid = value;
                if (GetErrors(nameof(MassAnotherReasons)) != null)
                {
                    _MassAnotherReasons.Set(_MassAnotherReasons_Not_Valid);
                }
                OnPropertyChanged(nameof(MassAnotherReasons));
            }
        }
        private IDataLoadEngine _MassAnotherReasons;
        private double _MassAnotherReasons_Not_Valid = -1;
        private void MassAnotherReasons_Validation()//TODO
        {
            ClearErrors(nameof(MassAnotherReasons));
        }
        //MassAnotherReasons Property

        //QuantityAnotherReasons property
        [Attributes.FormVisual("Количество поступившего на учет по другим причинам, шт.")]
        public int QuantityAnotherReasons
        {
            get
            {
                if (GetErrors(nameof(QuantityAnotherReasons)) != null)
                {
                    return (int)_QuantityAnotherReasons.Get();
                }
                else
                {
                    return _QuantityAnotherReasons_Not_Valid;
                }
            }
            set
            {
                _QuantityAnotherReasons_Not_Valid = value;
                if (GetErrors(nameof(QuantityAnotherReasons)) != null)
                {
                    _QuantityAnotherReasons.Set(_QuantityAnotherReasons_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityAnotherReasons));
            }
        }
        private IDataLoadEngine _QuantityAnotherReasons;  // positive int.
        private int _QuantityAnotherReasons_Not_Valid = -1;
        private void QuantityAnotherReasons_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityAnotherReasons));
            if (value <= 0)
                AddError(nameof(QuantityAnotherReasons), "Недопустимое значение");
        }
        //QuantityAnotherReasons property

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

        //MassTransferredToAnother Property
        [Attributes.FormVisual("Масса переданного сторонним, т")]
        public double MassTransferredToAnother
        {
            get
            {
                if (GetErrors(nameof(MassTransferredToAnother)) != null)
                {
                    return (double)_MassTransferredToAnother.Get();
                }
                else
                {
                    return _MassTransferredToAnother_Not_Valid;
                }
            }
            set
            {
                _MassTransferredToAnother_Not_Valid = value;
                if (GetErrors(nameof(MassTransferredToAnother)) != null)
                {
                    _MassTransferredToAnother.Set(_MassTransferredToAnother_Not_Valid);
                }
                OnPropertyChanged(nameof(MassTransferredToAnother));
            }
        }
        private IDataLoadEngine _MassTransferredToAnother;
        private double _MassTransferredToAnother_Not_Valid = -1;
        private void MassTransferredToAnother_Validation()//TODO
        {
            ClearErrors(nameof(MassTransferredToAnother));
        }
        //MassTransferredToAnother Property

        //QuantityTransferredToAnother property
        [Attributes.FormVisual("Количество переданного сторонним, шт.")]
        public int QuantityTransferredToAnother
        {
            get
            {
                if (GetErrors(nameof(QuantityTransferredToAnother)) != null)
                {
                    return (int)_QuantityTransferredToAnother.Get();
                }
                else
                {
                    return _QuantityTransferredToAnother_Not_Valid;
                }
            }
            set
            {
                _QuantityTransferredToAnother_Not_Valid = value;
                if (GetErrors(nameof(QuantityTransferredToAnother)) != null)
                {
                    _QuantityTransferredToAnother.Set(_QuantityTransferredToAnother_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityTransferredToAnother));
            }
        }
        private IDataLoadEngine _QuantityTransferredToAnother;  // positive int.
        private int _QuantityTransferredToAnother_Not_Valid = -1;
        private void QuantityTransferredToAnother_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityTransferredToAnother));
            if (value <= 0)
                AddError(nameof(QuantityTransferredToAnother), "Недопустимое значение");
        }
        //QuantityTransferredToAnother property

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

        //MassRefined Property
        [Attributes.FormVisual("Масса переработанного, т")]
        public double MassRefined
        {
            get
            {
                if (GetErrors(nameof(MassRefined)) != null)
                {
                    return (double)_MassRefined.Get();
                }
                else
                {
                    return _MassRefined_Not_Valid;
                }
            }
            set
            {
                _MassRefined_Not_Valid = value;
                if (GetErrors(nameof(MassRefined)) != null)
                {
                    _MassRefined.Set(_MassRefined_Not_Valid);
                }
                OnPropertyChanged(nameof(MassRefined));
            }
        }
        private IDataLoadEngine _MassRefined;
        private double _MassRefined_Not_Valid = -1;
        private void MassRefined_Validation()//TODO
        {
            ClearErrors(nameof(MassRefined));
        }
        //MassRefined Property

        //QuantityRefined property
        [Attributes.FormVisual("Количество переработанного, шт.")]
        public int QuantityRefined
        {
            get
            {
                if (GetErrors(nameof(QuantityRefined)) != null)
                {
                    return (int)_QuantityRefined.Get();
                }
                else
                {
                    return _QuantityRefined_Not_Valid;
                }
            }
            set
            {
                _QuantityRefined_Not_Valid = value;
                if (GetErrors(nameof(QuantityRefined)) != null)
                {
                    _QuantityRefined.Set(_QuantityRefined_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityRefined));
            }
        }
        private IDataLoadEngine _QuantityRefined;  // positive int.
        private int _QuantityRefined_Not_Valid = -1;
        private void QuantityRefined_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityRefined));
            if (value <= 0)
                AddError(nameof(QuantityRefined), "Недопустимое значение");
        }
        //QuantityRefined property

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

        //MassRemovedFromAccount Property
        [Attributes.FormVisual("Масса снятого с учета, т")]
        public double MassRemovedFromAccount
        {
            get
            {
                if (GetErrors(nameof(MassRemovedFromAccount)) != null)
                {
                    return (double)_MassRemovedFromAccount.Get();
                }
                else
                {
                    return _MassRemovedFromAccount_Not_Valid;
                }
            }
            set
            {
                _MassRemovedFromAccount_Not_Valid = value;
                if (GetErrors(nameof(MassRemovedFromAccount)) != null)
                {
                    _MassRemovedFromAccount.Set(_MassRemovedFromAccount_Not_Valid);
                }
                OnPropertyChanged(nameof(MassRemovedFromAccount));
            }
        }
        private IDataLoadEngine _MassRemovedFromAccount;
        private double _MassRemovedFromAccount_Not_Valid = -1;
        private void MassRemovedFromAccount_Validation()//TODO
        {
            ClearErrors(nameof(MassRemovedFromAccount));
        }
        //MassRemovedFromAccount Property

        //QuantityRemovedFromAccount property
        [Attributes.FormVisual("Количество снятого с учета, шт.")]
        public int QuantityRemovedFromAccount
        {
            get
            {
                if (GetErrors(nameof(QuantityRemovedFromAccount)) != null)
                {
                    return (int)_QuantityRemovedFromAccount.Get();
                }
                else
                {
                    return _QuantityRemovedFromAccount_Not_Valid;
                }
            }
            set
            {
                _QuantityRemovedFromAccount_Not_Valid = value;
                if (GetErrors(nameof(QuantityRemovedFromAccount)) != null)
                {
                    _QuantityRemovedFromAccount.Set(_QuantityRemovedFromAccount_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityRemovedFromAccount));
            }
        }
        private IDataLoadEngine _QuantityRemovedFromAccount;  // positive int.
        private int _QuantityRemovedFromAccount_Not_Valid = -1;
        private void QuantityRemovedFromAccount_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityRemovedFromAccount));
            if (value <= 0)
                AddError(nameof(QuantityRemovedFromAccount), "Недопустимое значение");
        }
        //QuantityRemovedFromAccount property

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
