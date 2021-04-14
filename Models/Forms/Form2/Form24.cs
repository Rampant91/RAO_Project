using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.4: Постановка на учет и снятие с учета РВ, содержащихся в отработавшем ядерном топливе")]
    public class Form24: Abstracts.Form2
    {
        public static string SQLCommandParams()
        {
            string strNotNullDeclaration = " varchar(255) not null, ";
            string intNotNullDeclaration = " int not null, ";
            string shortNotNullDeclaration = " smallint not null, ";
            string byteNotNullDeclaration = " tinyint not null, ";
            string dateNotNullDeclaration = " ????, ";
            string doubleNotNullDeclaration = " float(53) not null, ";
            return
                Abstracts.Form2.SQLCommandParamsBase() +
            nameof(CodeOYATnote) + strNotNullDeclaration +
            nameof(MassCreated) + doubleNotNullDeclaration +
            nameof(QuantityCreated) + intNotNullDeclaration +
            nameof(QuantityCreatedNote) + intNotNullDeclaration +
            nameof(MassFromAnothers) + doubleNotNullDeclaration +
            nameof(QuantityFromAnothers) + intNotNullDeclaration +
            nameof(QuantityFromAnothersNote) + intNotNullDeclaration +
            nameof(MassFromAnothersImported) + doubleNotNullDeclaration +
            nameof(QuantityFromAnothersImported) + intNotNullDeclaration +
            nameof(QuantityFromAnothersImportedNote) + intNotNullDeclaration +
            nameof(MassAnotherReasons) + doubleNotNullDeclaration +
            nameof(QuantityAnotherReasons) + intNotNullDeclaration +
            nameof(QuantityAnotherReasonsNote) + intNotNullDeclaration +
            nameof(MassRefined) + doubleNotNullDeclaration +
            nameof(MassTransferredToAnother) + doubleNotNullDeclaration +
            nameof(QuantityTransferredToAnother) + intNotNullDeclaration +
            nameof(QuantityTransferredToAnotherNote) + intNotNullDeclaration +
            nameof(QuantityRefined) + intNotNullDeclaration +
            nameof(QuantityRefinedNote) + intNotNullDeclaration +
            nameof(MassRemovedFromAccount) + doubleNotNullDeclaration +
            nameof(QuantityRemovedFromAccount) + intNotNullDeclaration +
            nameof(QuantityRemovedFromAccountNote) + intNotNullDeclaration +
            nameof(CodeOYAT) + strNotNullDeclaration +
            nameof(FcpNumber) + " varchar(255) not null";
        }
        public Form24(int RowID) : base(RowID)
        {
            FormNum = "24";
            NumberOfFields = 26;
        }

        [Attributes.Form_Property("Форма")]
        public override void Object_Validation()
        {

        }

        //CodeOYAT property
        [Attributes.Form_Property("Код ОЯТ")]
        public string CodeOYAT
        {
            get
            {
                if (GetErrors(nameof(CodeOYAT)) != null)
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
                if (GetErrors(nameof(CodeOYAT)) != null)
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
                if (GetErrors(nameof(CodeOYATnote)) != null)
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
                if (GetErrors(nameof(CodeOYATnote)) != null)
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
                if (GetErrors(nameof(FcpNumber)) != null)
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
                if (GetErrors(nameof(FcpNumber)) != null)
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
                if (GetErrors(nameof(MassCreated)) != null)
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
                if (GetErrors(nameof(MassCreated)) != null)
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
                if (GetErrors(nameof(QuantityCreated)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityCreated));
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
                if (GetErrors(nameof(QuantityCreatedNote)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityCreatedNote));
                }
                else
                {
                    return _QuantityCreatedNote_Not_Valid;
                }
            }
            set
            {
                _QuantityCreatedNote_Not_Valid = value;
                if (GetErrors(nameof(QuantityCreatedNote)) != null)
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
                if (GetErrors(nameof(MassFromAnothers)) != null)
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
                if (GetErrors(nameof(MassFromAnothers)) != null)
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
                if (GetErrors(nameof(QuantityFromAnothers)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityFromAnothers));
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
                if (GetErrors(nameof(QuantityFromAnothersNote)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityFromAnothersNote));
                }
                else
                {
                    return _QuantityFromAnothersNote_Not_Valid;
                }
            }
            set
            {
                _QuantityFromAnothersNote_Not_Valid = value;
                if (GetErrors(nameof(QuantityFromAnothersNote)) != null)
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
                if (GetErrors(nameof(MassFromAnothersImported)) != null)
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
                if (GetErrors(nameof(MassFromAnothersImported)) != null)
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
                if (GetErrors(nameof(QuantityFromAnothersImported)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityFromAnothersImported));
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

        //QuantityFromAnothersImportedNote property
        public int QuantityFromAnothersImportedNote
        {
            get
            {
                if (GetErrors(nameof(QuantityFromAnothersImportedNote)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityFromAnothersImportedNote));
                }
                else
                {
                    return _QuantityFromAnothersImportedNote_Not_Valid;
                }
            }
            set
            {
                _QuantityFromAnothersImportedNote_Not_Valid = value;
                if (GetErrors(nameof(QuantityFromAnothersNote)) != null)
                {
                    _dataAccess.Set(nameof(QuantityFromAnothersImportedNote), _QuantityFromAnothersImportedNote_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityFromAnothersImportedNote));
            }
        }
          // positive int.
        private int _QuantityFromAnothersImportedNote_Not_Valid = -1;
        private void QuantityFromAnothersImportedNote_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityFromAnothersImportedNote));
            if (value <= 0)
                AddError(nameof(QuantityFromAnothersImportedNote), "Недопустимое значение");
        }
        //QuantityFromAnothersImportedNote property

        //MassAnotherReasons Property
        [Attributes.Form_Property("Масса поставленного на учет по другим причинам, т")]
        public double MassAnotherReasons
        {
            get
            {
                if (GetErrors(nameof(MassAnotherReasons)) != null)
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
                if (GetErrors(nameof(MassAnotherReasons)) != null)
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
                if (GetErrors(nameof(QuantityAnotherReasons)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityAnotherReasons));
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
                if (GetErrors(nameof(QuantityAnotherReasonsNote)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityAnotherReasonsNote));
                }
                else
                {
                    return _QuantityAnotherReasonsNote_Not_Valid;
                }
            }
            set
            {
                _QuantityAnotherReasonsNote_Not_Valid = value;
                if (GetErrors(nameof(QuantityAnotherReasonsNote)) != null)
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
                if (GetErrors(nameof(MassTransferredToAnother)) != null)
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
                if (GetErrors(nameof(MassTransferredToAnother)) != null)
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
                if (GetErrors(nameof(QuantityTransferredToAnother)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityTransferredToAnother));
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

        //QuantityTransferredToAnotherNote property
        public int QuantityTransferredToAnotherNote
        {
            get
            {
                if (GetErrors(nameof(QuantityTransferredToAnotherNote)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityTransferredToAnotherNote));
                }
                else
                {
                    return _QuantityTransferredToAnotherNote_Not_Valid;
                }
            }
            set
            {
                _QuantityTransferredToAnotherNote_Not_Valid = value;
                if (GetErrors(nameof(QuantityTransferredToAnotherNote)) != null)
                {
                    _dataAccess.Set(nameof(QuantityTransferredToAnotherNote), _QuantityTransferredToAnotherNote_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityTransferredToAnotherNote));
            }
        }
          // positive int.
        private int _QuantityTransferredToAnotherNote_Not_Valid = -1;
        private void QuantityTransferredToAnotherNote_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityTransferredToAnotherNote));
            if (value <= 0)
                AddError(nameof(QuantityTransferredToAnotherNote), "Недопустимое значение");
        }
        //QuantityTransferredToAnotherNote property

        //MassRefined Property
        [Attributes.Form_Property("Масса переработанного, т")]
        public double MassRefined
        {
            get
            {
                if (GetErrors(nameof(MassRefined)) != null)
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
                if (GetErrors(nameof(MassRefined)) != null)
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
                if (GetErrors(nameof(QuantityRefined)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityRefined));
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
                if (GetErrors(nameof(QuantityRefinedNote)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityRefinedNote));
                }
                else
                {
                    return _QuantityRefinedNote_Not_Valid;
                }
            }
            set
            {
                _QuantityRefinedNote_Not_Valid = value;
                if (GetErrors(nameof(QuantityRefinedNote)) != null)
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
                if (GetErrors(nameof(MassRemovedFromAccount)) != null)
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
                if (GetErrors(nameof(MassRemovedFromAccount)) != null)
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
                if (GetErrors(nameof(QuantityRemovedFromAccount)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityRemovedFromAccount));
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

        //QuantityRemovedFromAccountNote property
        [Attributes.Form_Property("Количество снятого с учета, шт.")]
        public int QuantityRemovedFromAccountNote
        {
            get
            {
                if (GetErrors(nameof(QuantityRemovedFromAccountNote)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityRemovedFromAccountNote));
                }
                else
                {
                    return _QuantityRemovedFromAccountNote_Not_Valid;
                }
            }
            set
            {
                _QuantityRemovedFromAccountNote_Not_Valid = value;
                if (GetErrors(nameof(QuantityRemovedFromAccountNote)) != null)
                {
                    _dataAccess.Set(nameof(QuantityRemovedFromAccountNote), _QuantityRemovedFromAccountNote_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityRemovedFromAccountNote));
            }
        }
          // positive int.
        private int _QuantityRemovedFromAccountNote_Not_Valid = -1;
        private void QuantityRemovedFromAccountNote_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityRemovedFromAccountNote));
            if (value <= 0)
                AddError(nameof(QuantityRemovedFromAccountNote), "Недопустимое значение");
        }
        //QuantityRemovedFromAccountNote property
    }
}
