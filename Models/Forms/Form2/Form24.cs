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
        public IDataAccess<string> CodeOYAT
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CodeOYAT));
                }
                else
                {
                    
                }
            }
            set
            {
                CodeOYAT_Validation(value);
                
                {
                    _dataAccess.Set(nameof(CodeOYAT), value);
                }
                OnPropertyChanged(nameof(CodeOYAT));
            }
        }

                private void CodeOYAT_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();

        }
        //CodeOYAT property

        //CodeOYATnote property
        public IDataAccess<string> CodeOYATnote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CodeOYATnote));
                }
                else
                {
                    
                }
            }
            set
            {
                CodeOYATnote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(CodeOYATnote), value);
                }
                OnPropertyChanged(nameof(CodeOYATnote));
            }
        }

                private void CodeOYATnote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //CodeOYATnote property

        //FcpNumber property
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public IDataAccess<string> FcpNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FcpNumber));
                }
                else
                {
                    
                }
            }
            set
            {
                FcpNumber_Validation(value);
                
                {
                    _dataAccess.Set(nameof(FcpNumber), value);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

                private void FcpNumber_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //FcpNumber property

        //MassCreated Property
        [Attributes.Form_Property("Масса образованного, т")]
        public double MassCreated
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassCreated));
                }
                else
                {
                    
                }
            }
            set
            {
                MassCreated_Validation(value);
                
                {
                    _dataAccess.Set(nameof(MassCreated), value);
                }
                OnPropertyChanged(nameof(MassCreated));
            }
        }

                private void MassCreated_Validation(double value)//TODO
        {
            value.ClearErrors();
        }
        //MassCreated Property

        //QuantityCreated property
        [Attributes.Form_Property("Количество образованного, шт.")]
        public int QuantityCreated
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityCreated));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityCreated_Validation(value);
                //_QuantityCreated_Validation(value);

                
                {
                    _dataAccess.Set(nameof(QuantityCreated), value);
                }
                OnPropertyChanged(nameof(QuantityCreated));
            }
        }
        // positive int.
                private void QuantityCreated_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityCreated property

        //QuantityCreatedNote property
        public int QuantityCreatedNote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityCreatedNote));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityCreatedNote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(QuantityCreatedNote), value);
                }
                OnPropertyChanged(nameof(QuantityCreatedNote));
            }
        }
        // positive int.
                private void QuantityCreatedNote_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityCreatedNote property

        //MassFromAnothers Property
        [Attributes.Form_Property("Масса поступившего от сторонних, т")]
        public double MassFromAnothers
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassFromAnothers));
                }
                else
                {
                    
                }
            }
            set
            {
                MassFromAnothers_Validation(value);
                
                {
                    _dataAccess.Set(nameof(MassFromAnothers), value);
                }
                OnPropertyChanged(nameof(MassFromAnothers));
            }
        }

                private void MassFromAnothers_Validation(double value)//TODO
        {
            value.ClearErrors();
        }
        //MassFromAnothers Property

        //QuantityFromAnothers property
        [Attributes.Form_Property("Количество поступившего от сторонних, шт.")]
        public int QuantityFromAnothers
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityFromAnothers));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityFromAnothers_Validation(value);
                //_QuantityFromAnothers_Validation(value);
                
                
                {
                    _dataAccess.Set(nameof(QuantityFromAnothers), value);
                }
                OnPropertyChanged(nameof(QuantityFromAnothers));
            }
        }
        // positive int.
                private void QuantityFromAnothers_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityFromAnothers property

        //QuantityFromAnothersNote property
        public int QuantityFromAnothersNote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityFromAnothersNote));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityFromAnothersNote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(QuantityFromAnothersNote), value);
                }
                OnPropertyChanged(nameof(QuantityFromAnothersNote));
            }
        }
        // positive int.
                private void QuantityFromAnothersNote_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityFromAnothersNote property

        //MassFromAnothersImported Property
        [Attributes.Form_Property("Масса импортированного от сторонних, т")]
        public double MassFromAnothersImported
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassFromAnothersImported));
                }
                else
                {
                    
                }
            }
            set
            {
                MassFromAnothersImported_Validation(value);
                
                {
                    _dataAccess.Set(nameof(MassFromAnothersImported), value);
                }
                OnPropertyChanged(nameof(MassFromAnothersImported));
            }
        }

                private void MassFromAnothersImported_Validation(double value)//TODO
        {
            value.ClearErrors();
        }
        //MassFromAnothersImported Property

        //QuantityFromAnothersImported property
        [Attributes.Form_Property("Количество импортированного от сторонних, шт.")]
        public int QuantityFromAnothersImported
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityFromAnothersImported));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityFromAnothersImported_Validation(value);
                //_QuantityFromAnothersImported_Validation(value);

                
                {
                    _dataAccess.Set(nameof(QuantityFromAnothersImported), value);
                }
                OnPropertyChanged(nameof(QuantityFromAnothersImported));
            }
        }
        // positive int.
                private void QuantityFromAnothersImported_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityFromAnothersImported property

        //QuantityFromImportedNote property
        public int QuantityFromImportedNote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityFromImportedNote));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityFromImportedNote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(QuantityFromImportedNote), value);
                }
                OnPropertyChanged(nameof(QuantityFromImportedNote));
            }
        }
        // positive int.
                private void QuantityFromImportedNote_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityFromImportedNote property

        //MassAnotherReasons Property
        [Attributes.Form_Property("Масса поставленного на учет по другим причинам, т")]
        public double MassAnotherReasons
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassAnotherReasons));
                }
                else
                {
                    
                }
            }
            set
            {
                MassAnotherReasons_Validation(value);
                
                {
                    _dataAccess.Set(nameof(MassAnotherReasons), value);
                }
                OnPropertyChanged(nameof(MassAnotherReasons));
            }
        }

                private void MassAnotherReasons_Validation(double value)//TODO
        {
            value.ClearErrors();
        }
        //MassAnotherReasons Property

        //QuantityAnotherReasons property
        [Attributes.Form_Property("Количество поступившего на учет по другим причинам, шт.")]
        public int QuantityAnotherReasons
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityAnotherReasons));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityAnotherReasons_Validation(value);

                
                {
                    _dataAccess.Set(nameof(QuantityAnotherReasons), value);
                }
                OnPropertyChanged(nameof(QuantityAnotherReasons));
            }
        }
        // positive int.
                private void QuantityAnotherReasons_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityAnotherReasons property

        //QuantityAnotherReasonsNote property
        public int QuantityAnotherReasonsNote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityAnotherReasonsNote));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityAnotherReasonsNote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(QuantityAnotherReasonsNote), value);
                }
                OnPropertyChanged(nameof(QuantityAnotherReasonsNote));
            }
        }
        // positive int.
                private void QuantityAnotherReasonsNote_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityAnotherReasonsNote property

        //MassTransferredToAnother Property
        [Attributes.Form_Property("Масса переданного сторонним, т")]
        public double MassTransferredToAnother
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassTransferredToAnother));
                }
                else
                {
                    
                }
            }
            set
            {
                MassTransferredToAnother_Validation(value);
                
                {
                    _dataAccess.Set(nameof(MassTransferredToAnother), value);
                }
                OnPropertyChanged(nameof(MassTransferredToAnother));
            }
        }

                private void MassTransferredToAnother_Validation(double value)//TODO
        {
            value.ClearErrors();
        }
        //MassTransferredToAnother Property

        //QuantityTransferredToAnother property
        [Attributes.Form_Property("Количество переданного сторонним, шт.")]
        public int QuantityTransferredToAnother
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityTransferredToAnother));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityTransferredToAnother_Validation(value);

                
                {
                    _dataAccess.Set(nameof(QuantityTransferredToAnother), value);
                }
                OnPropertyChanged(nameof(QuantityTransferredToAnother));
            }
        }
        // positive int.
                private void QuantityTransferredToAnother_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityTransferredToAnother property

        //QuantityTransferredToNote property
        public int QuantityTransferredToNote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityTransferredToNote));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityTransferredToNote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(QuantityTransferredToNote), value);
                }
                OnPropertyChanged(nameof(QuantityTransferredToNote));
            }
        }
        // positive int.
                private void QuantityTransferredToNote_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityTransferredToNote property

        //MassRefined Property
        [Attributes.Form_Property("Масса переработанного, т")]
        public double MassRefined
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassRefined));
                }
                else
                {
                    
                }
            }
            set
            {
                MassRefined_Validation(value);
                
                {
                    _dataAccess.Set(nameof(MassRefined), value);
                }
                OnPropertyChanged(nameof(MassRefined));
            }
        }

                private void MassRefined_Validation(double value)//TODO
        {
            value.ClearErrors();
        }
        //MassRefined Property

        //QuantityRefined property
        [Attributes.Form_Property("Количество переработанного, шт.")]
        public int QuantityRefined
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityRefined));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityRefined_Validation(value);

                
                {
                    _dataAccess.Set(nameof(QuantityRefined), value);
                }
                OnPropertyChanged(nameof(QuantityRefined));
            }
        }
        // positive int.
                private void QuantityRefined_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityRefined property

        //QuantityRefinedNote property
        public int QuantityRefinedNote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityRefinedNote));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityRefinedNote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(QuantityRefinedNote), value);
                }
                OnPropertyChanged(nameof(QuantityRefinedNote));
            }
        }
        // positive int.
                private void QuantityRefinedNote_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityRefinedNote property

        //MassRemovedFromAccount Property
        [Attributes.Form_Property("Масса снятого с учета, т")]
        public double MassRemovedFromAccount
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassRemovedFromAccount));
                }
                else
                {
                    
                }
            }
            set
            {
                MassRemovedFromAccount_Validation(value);
                
                {
                    _dataAccess.Set(nameof(MassRemovedFromAccount), value);
                }
                OnPropertyChanged(nameof(MassRemovedFromAccount));
            }
        }

                private void MassRemovedFromAccount_Validation(double value)//TODO
        {
            value.ClearErrors();
        }
        //MassRemovedFromAccount Property

        //QuantityRemovedFromAccount property
        [Attributes.Form_Property("Количество снятого с учета, шт.")]
        public int QuantityRemovedFromAccount
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityRemovedFromAccount));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityRemovedFromAccount_Validation(value);

                
                {
                    _dataAccess.Set(nameof(QuantityRemovedFromAccount), value);
                }
                OnPropertyChanged(nameof(QuantityRemovedFromAccount));
            }
        }
        // positive int.
                private void QuantityRemovedFromAccount_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityRemovedFromAccount property

        //QuantityRemovedFromNote property
        [Attributes.Form_Property("Количество снятого с учета, шт.")]
        public int QuantityRemovedFromNote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityRemovedFromNote));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityRemovedFromNote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(QuantityRemovedFromNote), value);
                }
                OnPropertyChanged(nameof(QuantityRemovedFromNote));
            }
        }
        // positive int.
                private void QuantityRemovedFromNote_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityRemovedFromNote property
    }
}
