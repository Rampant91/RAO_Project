using Models.DataAccess;
using System;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.4: Постановка на учет и снятие с учета РВ, содержащихся в отработавшем ядерном топливе")]
    public class Form24 : Abstracts.Form2
    {
        public Form24() : base()
        {
            FormNum = "24";
            NumberOfFields = 26;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //CodeOYAT property
        [Attributes.Form_Property("Код ОЯТ")]
        public string CodeOYAT
        {
            get
            {
                if (GetErrors(nameof(CodeOYAT)) == null)
                {
                    return (string)_dataAccess.Get(nameof(CodeOYAT));
                }
                else
                {
                    return _CodeOYAT_Not_Valid;
                }
            }
            set
            {
                _CodeOYAT_Not_Valid = value;
                if (GetErrors(nameof(CodeOYAT)) == null)
                {
                    _dataAccess.Set(nameof(CodeOYAT), _CodeOYAT_Not_Valid);
                }
                OnPropertyChanged(nameof(CodeOYAT));
            }
        }

        private string _CodeOYAT_Not_Valid = "";
        private void CodeOYAT_Validation()
        {
            ClearErrors(nameof(CodeOYAT));
        }
        //CodeOYAT property

        //CodeOYATnote property
        public string CodeOYATnote
        {
            get
            {
                if (GetErrors(nameof(CodeOYATnote)) == null)
                {
                    return (string)_dataAccess.Get(nameof(CodeOYATnote));
                }
                else
                {
                    return _CodeOYATnote_Not_Valid;
                }
            }
            set
            {
                _CodeOYATnote_Not_Valid = value;
                if (GetErrors(nameof(CodeOYATnote)) == null)
                {
                    _dataAccess.Set(nameof(CodeOYATnote), _CodeOYATnote_Not_Valid);
                }
                OnPropertyChanged(nameof(CodeOYATnote));
            }
        }

        private string _CodeOYATnote_Not_Valid = "";
        private void CodeOYATnote_Validation()
        {
            ClearErrors(nameof(CodeOYATnote));
        }
        //CodeOYATnote property

        //FcpNumber property
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public string FcpNumber
        {
            get
            {
                if (GetErrors(nameof(FcpNumber)) == null)
                {
                    return (string)_dataAccess.Get(nameof(FcpNumber));
                }
                else
                {
                    return _FcpNumber_Not_Valid;
                }
            }
            set
            {
                _FcpNumber_Not_Valid = value;
                if (GetErrors(nameof(FcpNumber)) == null)
                {
                    _dataAccess.Set(nameof(FcpNumber), _FcpNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

        private string _FcpNumber_Not_Valid = "";
        private void FcpNuber_Validation(string value)//TODO
        {
            ClearErrors(nameof(FcpNumber));
        }
        //FcpNumber property

        //MassCreated Property
        [Attributes.Form_Property("Масса образованного, т")]
        public double MassCreated
        {
            get
            {
                if (GetErrors(nameof(MassCreated)) == null)
                {
                    return (double)_dataAccess.Get(nameof(MassCreated));
                }
                else
                {
                    return _MassCreated_Not_Valid;
                }
            }
            set
            {
                _MassCreated_Not_Valid = value;
                if (GetErrors(nameof(MassCreated)) == null)
                {
                    _dataAccess.Set(nameof(MassCreated), _MassCreated_Not_Valid);
                }
                OnPropertyChanged(nameof(MassCreated));
            }
        }

        private double _MassCreated_Not_Valid = -1;
        private void MassCreated_Validation()//TODO
        {
            ClearErrors(nameof(MassCreated));
        }
        //MassCreated Property

        //QuantityCreated property
        [Attributes.Form_Property("Количество образованного, шт.")]
        public int QuantityCreated
        {
            get
            {
                if (GetErrors(nameof(QuantityCreated)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityCreated));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _QuantityCreated_Not_Valid;
                }
            }
            set
            {
                QuantityCreated_Validation(value);
                //_QuantityCreated_Not_Valid = value;

                if (GetErrors(nameof(QuantityCreated)) == null)
                {
                    _dataAccess.Set(nameof(QuantityCreated), _QuantityCreated_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityCreated));
            }
        }
        // positive int.
        private int _QuantityCreated_Not_Valid = -1;
        private void QuantityCreated_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityCreated));
            if (value <= 0)
                AddError(nameof(QuantityCreated), "Недопустимое значение");
        }
        //QuantityCreated property

        //QuantityCreatedNote property
        public int QuantityCreatedNote
        {
            get
            {
                if (GetErrors(nameof(QuantityCreatedNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityCreatedNote));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _QuantityCreatedNote_Not_Valid;
                }
            }
            set
            {
                _QuantityCreatedNote_Not_Valid = value;
                if (GetErrors(nameof(QuantityCreatedNote)) == null)
                {
                    _dataAccess.Set(nameof(QuantityCreatedNote), _QuantityCreatedNote_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityCreatedNote));
            }
        }
        // positive int.
        private int _QuantityCreatedNote_Not_Valid = -1;
        private void QuantityCreatedNote_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityCreatedNote));
            if (value <= 0)
                AddError(nameof(QuantityCreatedNote), "Недопустимое значение");
        }
        //QuantityCreatedNote property

        //MassFromAnothers Property
        [Attributes.Form_Property("Масса поступившего от сторонних, т")]
        public double MassFromAnothers
        {
            get
            {
                if (GetErrors(nameof(MassFromAnothers)) == null)
                {
                    return (double)_dataAccess.Get(nameof(MassFromAnothers));
                }
                else
                {
                    return _MassFromAnothers_Not_Valid;
                }
            }
            set
            {
                _MassFromAnothers_Not_Valid = value;
                if (GetErrors(nameof(MassFromAnothers)) == null)
                {
                    _dataAccess.Set(nameof(MassFromAnothers), _MassFromAnothers_Not_Valid);
                }
                OnPropertyChanged(nameof(MassFromAnothers));
            }
        }

        private double _MassFromAnothers_Not_Valid = -1;
        private void MassFromAnothers_Validation()//TODO
        {
            ClearErrors(nameof(MassFromAnothers));
        }
        //MassFromAnothers Property

        //QuantityFromAnothers property
        [Attributes.Form_Property("Количество поступившего от сторонних, шт.")]
        public int QuantityFromAnothers
        {
            get
            {
                if (GetErrors(nameof(QuantityFromAnothers)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityFromAnothers));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _QuantityFromAnothers_Not_Valid;
                }
            }
            set
            {
                QuantityFromAnothers_Validation(value);
                //_QuantityFromAnothers_Not_Valid = value;
                
                if (GetErrors(nameof(QuantityFromAnothers)) == null)
                {
                    _dataAccess.Set(nameof(QuantityFromAnothers), _QuantityFromAnothers_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityFromAnothers));
            }
        }
        // positive int.
        private int _QuantityFromAnothers_Not_Valid = -1;
        private void QuantityFromAnothers_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityFromAnothers));
            if (value <= 0)
                AddError(nameof(QuantityFromAnothers), "Недопустимое значение");
        }
        //QuantityFromAnothers property

        //QuantityFromAnothersNote property
        public int QuantityFromAnothersNote
        {
            get
            {
                if (GetErrors(nameof(QuantityFromAnothersNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityFromAnothersNote));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _QuantityFromAnothersNote_Not_Valid;
                }
            }
            set
            {
                _QuantityFromAnothersNote_Not_Valid = value;
                if (GetErrors(nameof(QuantityFromAnothersNote)) == null)
                {
                    _dataAccess.Set(nameof(QuantityFromAnothersNote), _QuantityFromAnothersNote_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityFromAnothersNote));
            }
        }
        // positive int.
        private int _QuantityFromAnothersNote_Not_Valid = -1;
        private void QuantityFromAnothersNote_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityFromAnothersNote));
            if (value <= 0)
                AddError(nameof(QuantityFromAnothersNote), "Недопустимое значение");
        }
        //QuantityFromAnothersNote property

        //MassFromAnothersImported Property
        [Attributes.Form_Property("Масса импортированного от сторонних, т")]
        public double MassFromAnothersImported
        {
            get
            {
                if (GetErrors(nameof(MassFromAnothersImported)) == null)
                {
                    return (double)_dataAccess.Get(nameof(MassFromAnothersImported));
                }
                else
                {
                    return _MassFromAnothersImported_Not_Valid;
                }
            }
            set
            {
                _MassFromAnothersImported_Not_Valid = value;
                if (GetErrors(nameof(MassFromAnothersImported)) == null)
                {
                    _dataAccess.Set(nameof(MassFromAnothersImported), _MassFromAnothersImported_Not_Valid);
                }
                OnPropertyChanged(nameof(MassFromAnothersImported));
            }
        }

        private double _MassFromAnothersImported_Not_Valid = -1;
        private void MassFromAnothersImported_Validation()//TODO
        {
            ClearErrors(nameof(MassFromAnothersImported));
        }
        //MassFromAnothersImported Property

        //QuantityFromAnothersImported property
        [Attributes.Form_Property("Количество импортированного от сторонних, шт.")]
        public int QuantityFromAnothersImported
        {
            get
            {
                if (GetErrors(nameof(QuantityFromAnothersImported)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityFromAnothersImported));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _QuantityFromAnothersImported_Not_Valid;
                }
            }
            set
            {
                QuantityFromAnothersImported_Validation(value);
                //_QuantityFromAnothersImported_Not_Valid = value;

                if (GetErrors(nameof(QuantityFromAnothersImported)) == null)
                {
                    _dataAccess.Set(nameof(QuantityFromAnothersImported), _QuantityFromAnothersImported_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityFromAnothersImported));
            }
        }
        // positive int.
        private int _QuantityFromAnothersImported_Not_Valid = -1;
        private void QuantityFromAnothersImported_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityFromAnothersImported));
            if (value <= 0)
                AddError(nameof(QuantityFromAnothersImported), "Недопустимое значение");
        }
        //QuantityFromAnothersImported property

        //QuantityFromImportedNote property
        public int QuantityFromImportedNote
        {
            get
            {
                if (GetErrors(nameof(QuantityFromImportedNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityFromImportedNote));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _QuantityFromImportedNote_Not_Valid;
                }
            }
            set
            {
                _QuantityFromImportedNote_Not_Valid = value;
                if (GetErrors(nameof(QuantityFromAnothersNote)) == null)
                {
                    _dataAccess.Set(nameof(QuantityFromImportedNote), _QuantityFromImportedNote_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityFromImportedNote));
            }
        }
        // positive int.
        private int _QuantityFromImportedNote_Not_Valid = -1;
        private void QuantityFromImportedNote_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityFromImportedNote));
            if (value <= 0)
                AddError(nameof(QuantityFromImportedNote), "Недопустимое значение");
        }
        //QuantityFromImportedNote property

        //MassAnotherReasons Property
        [Attributes.Form_Property("Масса поставленного на учет по другим причинам, т")]
        public double MassAnotherReasons
        {
            get
            {
                if (GetErrors(nameof(MassAnotherReasons)) == null)
                {
                    return (double)_dataAccess.Get(nameof(MassAnotherReasons));
                }
                else
                {
                    return _MassAnotherReasons_Not_Valid;
                }
            }
            set
            {
                _MassAnotherReasons_Not_Valid = value;
                if (GetErrors(nameof(MassAnotherReasons)) == null)
                {
                    _dataAccess.Set(nameof(MassAnotherReasons), _MassAnotherReasons_Not_Valid);
                }
                OnPropertyChanged(nameof(MassAnotherReasons));
            }
        }

        private double _MassAnotherReasons_Not_Valid = -1;
        private void MassAnotherReasons_Validation()//TODO
        {
            ClearErrors(nameof(MassAnotherReasons));
        }
        //MassAnotherReasons Property

        //QuantityAnotherReasons property
        [Attributes.Form_Property("Количество поступившего на учет по другим причинам, шт.")]
        public int QuantityAnotherReasons
        {
            get
            {
                if (GetErrors(nameof(QuantityAnotherReasons)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityAnotherReasons));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _QuantityAnotherReasons_Not_Valid;
                }
            }
            set
            {
                QuantityAnotherReasons_Validation(value);

                if (GetErrors(nameof(QuantityAnotherReasons)) == null)
                {
                    _dataAccess.Set(nameof(QuantityAnotherReasons), _QuantityAnotherReasons_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityAnotherReasons));
            }
        }
        // positive int.
        private int _QuantityAnotherReasons_Not_Valid = -1;
        private void QuantityAnotherReasons_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityAnotherReasons));
            if (value <= 0)
                AddError(nameof(QuantityAnotherReasons), "Недопустимое значение");
        }
        //QuantityAnotherReasons property

        //QuantityAnotherReasonsNote property
        public int QuantityAnotherReasonsNote
        {
            get
            {
                if (GetErrors(nameof(QuantityAnotherReasonsNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityAnotherReasonsNote));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _QuantityAnotherReasonsNote_Not_Valid;
                }
            }
            set
            {
                _QuantityAnotherReasonsNote_Not_Valid = value;
                if (GetErrors(nameof(QuantityAnotherReasonsNote)) == null)
                {
                    _dataAccess.Set(nameof(QuantityAnotherReasonsNote), _QuantityAnotherReasonsNote_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityAnotherReasonsNote));
            }
        }
        // positive int.
        private int _QuantityAnotherReasonsNote_Not_Valid = -1;
        private void QuantityAnotherReasonsNote_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityAnotherReasonsNote));
            if (value <= 0)
                AddError(nameof(QuantityAnotherReasonsNote), "Недопустимое значение");
        }
        //QuantityAnotherReasonsNote property

        //MassTransferredToAnother Property
        [Attributes.Form_Property("Масса переданного сторонним, т")]
        public double MassTransferredToAnother
        {
            get
            {
                if (GetErrors(nameof(MassTransferredToAnother)) == null)
                {
                    return (double)_dataAccess.Get(nameof(MassTransferredToAnother));
                }
                else
                {
                    return _MassTransferredToAnother_Not_Valid;
                }
            }
            set
            {
                _MassTransferredToAnother_Not_Valid = value;
                if (GetErrors(nameof(MassTransferredToAnother)) == null)
                {
                    _dataAccess.Set(nameof(MassTransferredToAnother), _MassTransferredToAnother_Not_Valid);
                }
                OnPropertyChanged(nameof(MassTransferredToAnother));
            }
        }

        private double _MassTransferredToAnother_Not_Valid = -1;
        private void MassTransferredToAnother_Validation()//TODO
        {
            ClearErrors(nameof(MassTransferredToAnother));
        }
        //MassTransferredToAnother Property

        //QuantityTransferredToAnother property
        [Attributes.Form_Property("Количество переданного сторонним, шт.")]
        public int QuantityTransferredToAnother
        {
            get
            {
                if (GetErrors(nameof(QuantityTransferredToAnother)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityTransferredToAnother));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _QuantityTransferredToAnother_Not_Valid;
                }
            }
            set
            {
                QuantityTransferredToAnother_Validation(value);

                if (GetErrors(nameof(QuantityTransferredToAnother)) == null)
                {
                    _dataAccess.Set(nameof(QuantityTransferredToAnother), _QuantityTransferredToAnother_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityTransferredToAnother));
            }
        }
        // positive int.
        private int _QuantityTransferredToAnother_Not_Valid = -1;
        private void QuantityTransferredToAnother_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityTransferredToAnother));
            if (value <= 0)
                AddError(nameof(QuantityTransferredToAnother), "Недопустимое значение");
        }
        //QuantityTransferredToAnother property

        //QuantityTransferredToNote property
        public int QuantityTransferredToNote
        {
            get
            {
                if (GetErrors(nameof(QuantityTransferredToNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityTransferredToNote));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _QuantityTransferredToNote_Not_Valid;
                }
            }
            set
            {
                _QuantityTransferredToNote_Not_Valid = value;
                if (GetErrors(nameof(QuantityTransferredToNote)) == null)
                {
                    _dataAccess.Set(nameof(QuantityTransferredToNote), _QuantityTransferredToNote_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityTransferredToNote));
            }
        }
        // positive int.
        private int _QuantityTransferredToNote_Not_Valid = -1;
        private void QuantityTransferredToNote_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityTransferredToNote));
            if (value <= 0)
                AddError(nameof(QuantityTransferredToNote), "Недопустимое значение");
        }
        //QuantityTransferredToNote property

        //MassRefined Property
        [Attributes.Form_Property("Масса переработанного, т")]
        public double MassRefined
        {
            get
            {
                if (GetErrors(nameof(MassRefined)) == null)
                {
                    return (double)_dataAccess.Get(nameof(MassRefined));
                }
                else
                {
                    return _MassRefined_Not_Valid;
                }
            }
            set
            {
                _MassRefined_Not_Valid = value;
                if (GetErrors(nameof(MassRefined)) == null)
                {
                    _dataAccess.Set(nameof(MassRefined), _MassRefined_Not_Valid);
                }
                OnPropertyChanged(nameof(MassRefined));
            }
        }

        private double _MassRefined_Not_Valid = -1;
        private void MassRefined_Validation()//TODO
        {
            ClearErrors(nameof(MassRefined));
        }
        //MassRefined Property

        //QuantityRefined property
        [Attributes.Form_Property("Количество переработанного, шт.")]
        public int QuantityRefined
        {
            get
            {
                if (GetErrors(nameof(QuantityRefined)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityRefined));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _QuantityRefined_Not_Valid;
                }
            }
            set
            {
                QuantityRefined_Validation(value);

                if (GetErrors(nameof(QuantityRefined)) == null)
                {
                    _dataAccess.Set(nameof(QuantityRefined), _QuantityRefined_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityRefined));
            }
        }
        // positive int.
        private int _QuantityRefined_Not_Valid = -1;
        private void QuantityRefined_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityRefined));
            if (value <= 0)
                AddError(nameof(QuantityRefined), "Недопустимое значение");
        }
        //QuantityRefined property

        //QuantityRefinedNote property
        public int QuantityRefinedNote
        {
            get
            {
                if (GetErrors(nameof(QuantityRefinedNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityRefinedNote));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _QuantityRefinedNote_Not_Valid;
                }
            }
            set
            {
                _QuantityRefinedNote_Not_Valid = value;
                if (GetErrors(nameof(QuantityRefinedNote)) == null)
                {
                    _dataAccess.Set(nameof(QuantityRefinedNote), _QuantityRefinedNote_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityRefinedNote));
            }
        }
        // positive int.
        private int _QuantityRefinedNote_Not_Valid = -1;
        private void QuantityRefinedNote_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityRefinedNote));
            if (value <= 0)
                AddError(nameof(QuantityRefinedNote), "Недопустимое значение");
        }
        //QuantityRefinedNote property

        //MassRemovedFromAccount Property
        [Attributes.Form_Property("Масса снятого с учета, т")]
        public double MassRemovedFromAccount
        {
            get
            {
                if (GetErrors(nameof(MassRemovedFromAccount)) == null)
                {
                    return (double)_dataAccess.Get(nameof(MassRemovedFromAccount));
                }
                else
                {
                    return _MassRemovedFromAccount_Not_Valid;
                }
            }
            set
            {
                _MassRemovedFromAccount_Not_Valid = value;
                if (GetErrors(nameof(MassRemovedFromAccount)) == null)
                {
                    _dataAccess.Set(nameof(MassRemovedFromAccount), _MassRemovedFromAccount_Not_Valid);
                }
                OnPropertyChanged(nameof(MassRemovedFromAccount));
            }
        }

        private double _MassRemovedFromAccount_Not_Valid = -1;
        private void MassRemovedFromAccount_Validation()//TODO
        {
            ClearErrors(nameof(MassRemovedFromAccount));
        }
        //MassRemovedFromAccount Property

        //QuantityRemovedFromAccount property
        [Attributes.Form_Property("Количество снятого с учета, шт.")]
        public int QuantityRemovedFromAccount
        {
            get
            {
                if (GetErrors(nameof(QuantityRemovedFromAccount)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityRemovedFromAccount));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _QuantityRemovedFromAccount_Not_Valid;
                }
            }
            set
            {
                QuantityRemovedFromAccount_Validation(value);

                if (GetErrors(nameof(QuantityRemovedFromAccount)) == null)
                {
                    _dataAccess.Set(nameof(QuantityRemovedFromAccount), _QuantityRemovedFromAccount_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityRemovedFromAccount));
            }
        }
        // positive int.
        private int _QuantityRemovedFromAccount_Not_Valid = -1;
        private void QuantityRemovedFromAccount_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityRemovedFromAccount));
            if (value <= 0)
                AddError(nameof(QuantityRemovedFromAccount), "Недопустимое значение");
        }
        //QuantityRemovedFromAccount property

        //QuantityRemovedFromNote property
        [Attributes.Form_Property("Количество снятого с учета, шт.")]
        public int QuantityRemovedFromNote
        {
            get
            {
                if (GetErrors(nameof(QuantityRemovedFromNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityRemovedFromNote));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _QuantityRemovedFromNote_Not_Valid;
                }
            }
            set
            {
                _QuantityRemovedFromNote_Not_Valid = value;
                if (GetErrors(nameof(QuantityRemovedFromNote)) == null)
                {
                    _dataAccess.Set(nameof(QuantityRemovedFromNote), _QuantityRemovedFromNote_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityRemovedFromNote));
            }
        }
        // positive int.
        private int _QuantityRemovedFromNote_Not_Valid = -1;
        private void QuantityRemovedFromNote_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityRemovedFromNote));
            if (value <= 0)
                AddError(nameof(QuantityRemovedFromNote), "Недопустимое значение");
        }
        //QuantityRemovedFromNote property
    }
}
