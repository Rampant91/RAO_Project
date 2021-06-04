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
            FormNum.Value = "24";
            NumberOfFields.Value = 26;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //CodeOYAT property
        [Attributes.Form_Property("Код ОЯТ")]
        public RamAccess<string> CodeOYAT
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CodeOYAT));
                }
                
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
        public RamAccess<string> CodeOYATnote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CodeOYATnote));
                }
                
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
        public RamAccess<string> FcpNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FcpNumber));
                }
                
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
        public RamAccess<double> MassCreated
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(MassCreated));
                }
                
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

                private void MassCreated_Validation(IDataAccess<double> value)//TODO
        {
            value.ClearErrors();
        }
        //MassCreated Property

        //QuantityCreated property
        [Attributes.Form_Property("Количество образованного, шт.")]
        public RamAccess<int> QuantityCreated
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(QuantityCreated));//OK
                    
                }
                
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
                private void QuantityCreated_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityCreated property

        //QuantityCreatedNote property
        public RamAccess<int> QuantityCreatedNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(QuantityCreatedNote));//OK
                    
                }
                
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
                private void QuantityCreatedNote_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityCreatedNote property

        //MassFromAnothers Property
        [Attributes.Form_Property("Масса поступившего от сторонних, т")]
        public RamAccess<double> MassFromAnothers
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(MassFromAnothers));
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(MassFromAnothers), value);
                OnPropertyChanged(nameof(MassFromAnothers));
            }
        }

                private void MassFromAnothers_Validation(IDataAccess<double> value)//TODO
        {
            value.ClearErrors();
        }
        //MassFromAnothers Property

        //QuantityFromAnothers property
        [Attributes.Form_Property("Количество поступившего от сторонних, шт.")]
        public RamAccess<int> QuantityFromAnothers
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(QuantityFromAnothers));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(QuantityFromAnothers), value);
                OnPropertyChanged(nameof(QuantityFromAnothers));
            }
        }
        // positive int.
                private void QuantityFromAnothers_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityFromAnothers property

        //QuantityFromAnothersNote property
        public RamAccess<int> QuantityFromAnothersNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(QuantityFromAnothersNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(QuantityFromAnothersNote), value);
                OnPropertyChanged(nameof(QuantityFromAnothersNote));
            }
        }
        // positive int.
                private void QuantityFromAnothersNote_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityFromAnothersNote property

        //MassFromAnothersImported Property
        [Attributes.Form_Property("Масса импортированного от сторонних, т")]
        public RamAccess<double> MassFromAnothersImported
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(MassFromAnothersImported));
                }
                
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

                private void MassFromAnothersImported_Validation(IDataAccess<double> value)//TODO
        {
            value.ClearErrors();
        }
        //MassFromAnothersImported Property

        //QuantityFromAnothersImported property
        [Attributes.Form_Property("Количество импортированного от сторонних, шт.")]
        public RamAccess<int> QuantityFromAnothersImported
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(QuantityFromAnothersImported));//OK
                    
                }
                
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
                private void QuantityFromAnothersImported_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityFromAnothersImported property

        //QuantityFromImportedNote property
        public RamAccess<int> QuantityFromImportedNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(QuantityFromImportedNote));//OK
                    
                }
                
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
                private void QuantityFromImportedNote_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityFromImportedNote property

        //MassAnotherReasons Property
        [Attributes.Form_Property("Масса поставленного на учет по другим причинам, т")]
        public RamAccess<double> MassAnotherReasons
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(MassAnotherReasons));
                }
                
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

                private void MassAnotherReasons_Validation(IDataAccess<double> value)//TODO
        {
            value.ClearErrors();
        }
        //MassAnotherReasons Property

        //QuantityAnotherReasons property
        [Attributes.Form_Property("Количество поступившего на учет по другим причинам, шт.")]
        public RamAccess<int> QuantityAnotherReasons
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(QuantityAnotherReasons));//OK
                    
                }
                
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
                private void QuantityAnotherReasons_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityAnotherReasons property

        //QuantityAnotherReasonsNote property
        public RamAccess<int> QuantityAnotherReasonsNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(QuantityAnotherReasonsNote));//OK
                    
                }
                
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
                private void QuantityAnotherReasonsNote_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityAnotherReasonsNote property

        //MassTransferredToAnother Property
        [Attributes.Form_Property("Масса переданного сторонним, т")]
        public RamAccess<double> MassTransferredToAnother
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(MassTransferredToAnother));
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(MassTransferredToAnother), value);
                OnPropertyChanged(nameof(MassTransferredToAnother));
            }
        }

                private void MassTransferredToAnother_Validation(IDataAccess<double?> value)//TODO
        {
            value.ClearErrors();
        }
        //MassTransferredToAnother Property

        //QuantityTransferredToAnother property
        [Attributes.Form_Property("Количество переданного сторонним, шт.")]
        public RamAccess<int> QuantityTransferredToAnother
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(QuantityTransferredToAnother));//OK
                    
                }
                
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
                private void QuantityTransferredToAnother_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityTransferredToAnother property

        //QuantityTransferredToNote property
        public RamAccess<int> QuantityTransferredToNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(QuantityTransferredToNote));//OK
                    
                }
                
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
                private void QuantityTransferredToNote_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityTransferredToNote property

        //MassRefined Property
        [Attributes.Form_Property("Масса переработанного, т")]
        public RamAccess<double> MassRefined
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(MassRefined));
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(MassRefined), value);
                OnPropertyChanged(nameof(MassRefined));
            }
        }

                private void MassRefined_Validation(IDataAccess<double> value)//TODO
        {
            value.ClearErrors();
        }
        //MassRefined Property

        //QuantityRefined property
        [Attributes.Form_Property("Количество переработанного, шт.")]
        public RamAccess<int> QuantityRefined
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(QuantityRefined));//OK
                    
                }
                
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
                private void QuantityRefined_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityRefined property

        //QuantityRefinedNote property
        public RamAccess<int> QuantityRefinedNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(QuantityRefinedNote));//OK
                    
                }
                
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
                private void QuantityRefinedNote_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityRefinedNote property

        //MassRemovedFromAccount Property
        [Attributes.Form_Property("Масса снятого с учета, т")]
        public RamAccess<double> MassRemovedFromAccount
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(MassRemovedFromAccount));
                }
                
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

                private void MassRemovedFromAccount_Validation(IDataAccess<double> value)//TODO
        {
            value.ClearErrors();
        }
        //MassRemovedFromAccount Property

        //QuantityRemovedFromAccount property
        [Attributes.Form_Property("Количество снятого с учета, шт.")]
        public RamAccess<int> QuantityRemovedFromAccount
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(QuantityRemovedFromAccount));//OK
                    
                }
                
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
                private void QuantityRemovedFromAccount_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityRemovedFromAccount property

        //QuantityRemovedFromNote property
        [Attributes.Form_Property("Количество снятого с учета, шт.")]
        public RamAccess<int> QuantityRemovedFromNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(QuantityRemovedFromNote));//OK
                    
                }
                
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
                private void QuantityRemovedFromNote_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityRemovedFromNote property
    }
}
