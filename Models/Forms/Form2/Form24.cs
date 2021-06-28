using Models.DataAccess;
using System.Globalization;
using System;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.4: Постановка на учет и снятие с учета РВ, содержащихся в отработавшем ядерном топливе")]
    public class Form24 : Abstracts.Form2
    {
        public Form24() : base()
        {
            //FormNum.Value = "24";
            //NumberOfFields.Value = 26;
            Init();
            Validate_all();
        }

        private void Init()
        {
            _dataAccess.Init<string>(nameof(CodeOYAT), CodeOYAT_Validation, null);
            CodeOYAT.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(FcpNumber), FcpNumber_Validation, null);
            FcpNumber.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(QuantityFromAnothers), QuantityFromAnothers_Validation, null);
            QuantityFromAnothers.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(QuantityFromAnothersImported), QuantityFromAnothersImported_Validation, null);
            QuantityFromAnothersImported.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(QuantityCreated), QuantityCreated_Validation, null);
            QuantityCreated.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(QuantityRemovedFromAccount), QuantityRemovedFromAccount_Validation, null);
            QuantityRemovedFromAccount.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(MassCreated), MassCreated_Validation, null);
            MassCreated.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(MassFromAnothers), MassFromAnothers_Validation, null);
            MassFromAnothers.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(MassFromAnothersImported), MassFromAnothersImported_Validation, null);
            MassFromAnothersImported.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(MassRemovedFromAccount), MassRemovedFromAccount_Validation, null);
            MassRemovedFromAccount.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(QuantityTransferredToAnother), QuantityTransferredToAnother_Validation, null);
            QuantityTransferredToAnother.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(MassAnotherReasons), MassAnotherReasons_Validation, null);
            MassAnotherReasons.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(MassTransferredToAnother), MassTransferredToAnother_Validation, null);
            MassTransferredToAnother.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(QuantityAnotherReasons), QuantityAnotherReasons_Validation, null);
            QuantityAnotherReasons.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(QuantityRefined), QuantityRefined_Validation, null);
            QuantityRefined.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(MassRefined), MassRefined_Validation, null);
            MassRefined.PropertyChanged += InPropertyChanged;
        }

        private void Validate_all()
        {
            CodeOYAT_Validation(CodeOYAT);
            FcpNumber_Validation(FcpNumber);
            QuantityFromAnothers_Validation(QuantityFromAnothers);
            QuantityFromAnothersImported_Validation(QuantityFromAnothersImported);
            QuantityCreated_Validation(QuantityCreated);
            QuantityRemovedFromAccount_Validation(QuantityRemovedFromAccount);
            MassCreated_Validation(MassCreated);
            MassFromAnothers_Validation(MassFromAnothers);
            MassFromAnothersImported_Validation(MassFromAnothersImported);
            MassRemovedFromAccount_Validation(MassRemovedFromAccount);
            QuantityTransferredToAnother_Validation(QuantityTransferredToAnother);
            MassAnotherReasons_Validation(MassAnotherReasons);
            MassTransferredToAnother_Validation(MassTransferredToAnother);
            QuantityAnotherReasons_Validation(QuantityAnotherReasons);
            QuantityRefined_Validation(QuantityRefined);
            MassRefined_Validation(MassRefined);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //CodeOYAT property
        [Attributes.Form_Property("Код ОЯТ")]public int? CodeOYATId { get; set; }
        public virtual RamAccess<string> CodeOYAT
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

                
                {
                    _dataAccess.Set(nameof(CodeOYAT), value);
                }
                OnPropertyChanged(nameof(CodeOYAT));
            }
        }

        private bool CodeOYAT_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }
        //CodeOYAT property

        //CodeOYATnote property
        public virtual RamAccess<string> CodeOYATnote
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

                
                {
                    _dataAccess.Set(nameof(CodeOYATnote), value);
                }
                OnPropertyChanged(nameof(CodeOYATnote));
            }
        }

                private bool CodeOYATnote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //CodeOYATnote property

        //FcpNumber property
        [Attributes.Form_Property("Номер мероприятия ФЦП")]public int? FcpNumberId { get; set; }
        public virtual RamAccess<string> FcpNumber
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

                
                {
                    _dataAccess.Set(nameof(FcpNumber), value);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

                private bool FcpNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            return true;
        }
        //FcpNumber property

        //MassCreated Property
        [Attributes.Form_Property("Масса образованного, т")]public int? MassCreatedId { get; set; }
        public virtual RamAccess<string> MassCreated
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassCreated));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(MassCreated), value);
                }
                OnPropertyChanged(nameof(MassCreated));
            }
        }

                private bool MassCreated_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //MassCreated Property

        //QuantityCreated property
        [Attributes.Form_Property("Количество образованного, шт.")]public int? QuantityCreatedId { get; set; }
        public virtual RamAccess<string> QuantityCreated
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(QuantityCreated));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {



                
                {
                    _dataAccess.Set(nameof(QuantityCreated), value);
                }
                OnPropertyChanged(nameof(QuantityCreated));
            }
        }
        // positive int.
                private bool QuantityCreated_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityCreated property

        ////QuantityCreatedNote property
        //public virtual RamAccess<string> QuantityCreatedNote
        //{
        //    get
        //    {
                
        //        {
        //            return _dataAccess.Get<string>(nameof(QuantityCreatedNote));//OK
                    
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {

                
        //        {
        //            _dataAccess.Set(nameof(QuantityCreatedNote), value);
        //        }
        //        OnPropertyChanged(nameof(QuantityCreatedNote));
        //    }
        //}
        //// positive int.
        //        private bool QuantityCreatedNote_Validation(RamAccess<string> value)//Ready
        //{
        //    value.ClearErrors();
        //    if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        //    {
        //        return true;
        //    }
        //    try
        //    {
        //        int k = int.Parse(value.Value);
        //        if (k <= 0)
        //        {
        //            value.AddError("Недопустимое значение");
        //            return false;
        //        }
        //    }
        //    catch
        //    {
        //        value.AddError("Недопустимое значение");
        //        return false;
        //    }
        //    return true;
        //}
        ////QuantityCreatedNote property

        //MassFromAnothers Property
        [Attributes.Form_Property("Масса поступившего от сторонних, т")]public int? MassFromAnothersId { get; set; }
        public virtual RamAccess<string> MassFromAnothers
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassFromAnothers));
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

                private bool MassFromAnothers_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //MassFromAnothers Property

        //QuantityFromAnothers property
        [Attributes.Form_Property("Количество поступившего от сторонних, шт.")]public int? QuantityFromAnothersId { get; set; }
        public virtual RamAccess<string> QuantityFromAnothers
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(QuantityFromAnothers));//OK
                    
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
                private bool QuantityFromAnothers_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityFromAnothers property

        //QuantityFromAnothersNote property
        public virtual RamAccess<string> QuantityFromAnothersNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(QuantityFromAnothersNote));//OK
                    
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
                private bool QuantityFromAnothersNote_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityFromAnothersNote property

        //MassFromAnothersImported Property
        [Attributes.Form_Property("Масса импортированного от сторонних, т")]public int? MassFromAnothersImportedId { get; set; }
        public virtual RamAccess<string> MassFromAnothersImported
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassFromAnothersImported));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(MassFromAnothersImported), value);
                }
                OnPropertyChanged(nameof(MassFromAnothersImported));
            }
        }

                private bool MassFromAnothersImported_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //MassFromAnothersImported Property

        //QuantityFromAnothersImported property
        [Attributes.Form_Property("Количество импортированного от сторонних, шт.")]public int? QuantityFromAnothersImportedId { get; set; }
        public virtual RamAccess<string> QuantityFromAnothersImported
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(QuantityFromAnothersImported));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {



                
                {
                    _dataAccess.Set(nameof(QuantityFromAnothersImported), value);
                }
                OnPropertyChanged(nameof(QuantityFromAnothersImported));
            }
        }
        // positive int.
                private bool QuantityFromAnothersImported_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityFromAnothersImported property

        //QuantityFromImportedNote property
        public virtual RamAccess<string> QuantityFromImportedNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(QuantityFromImportedNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(QuantityFromImportedNote), value);
                }
                OnPropertyChanged(nameof(QuantityFromImportedNote));
            }
        }
        // positive int.
                private bool QuantityFromImportedNote_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityFromImportedNote property

        //MassAnotherReasons Property
        [Attributes.Form_Property("Масса поставленного на учет по другим причинам, т")]public int? MassAnotherReasonsId { get; set; }
        public virtual RamAccess<string> MassAnotherReasons
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassAnotherReasons));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(MassAnotherReasons), value);
                }
                OnPropertyChanged(nameof(MassAnotherReasons));
            }
        }

                private bool MassAnotherReasons_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //MassAnotherReasons Property

        //QuantityAnotherReasons property
        [Attributes.Form_Property("Количество поступившего на учет по другим причинам, шт.")]public int? QuantityAnotherReasonsId { get; set; }
        public virtual RamAccess<string> QuantityAnotherReasons
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(QuantityAnotherReasons));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(QuantityAnotherReasons), value);
                }
                OnPropertyChanged(nameof(QuantityAnotherReasons));
            }
        }
        // positive int.
                private bool QuantityAnotherReasons_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityAnotherReasons property

        //QuantityAnotherReasonsNote property
        public virtual RamAccess<string> QuantityAnotherReasonsNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(QuantityAnotherReasonsNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(QuantityAnotherReasonsNote), value);
                }
                OnPropertyChanged(nameof(QuantityAnotherReasonsNote));
            }
        }
        // positive int.
                private bool QuantityAnotherReasonsNote_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityAnotherReasonsNote property

        //MassTransferredToAnother Property
        [Attributes.Form_Property("Масса переданного сторонним, т")]public int? MassTransferredToAnotherId { get; set; }
        public virtual RamAccess<string> MassTransferredToAnother
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassTransferredToAnother));
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

                private bool MassTransferredToAnother_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //MassTransferredToAnother Property

        //QuantityTransferredToAnother property
        [Attributes.Form_Property("Количество переданного сторонним, шт.")]public int? QuantityTransferredToAnotherId { get; set; }
        public virtual RamAccess<string> QuantityTransferredToAnother
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(QuantityTransferredToAnother));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(QuantityTransferredToAnother), value);
                }
                OnPropertyChanged(nameof(QuantityTransferredToAnother));
            }
        }
        // positive int.
                private bool QuantityTransferredToAnother_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityTransferredToAnother property

        //QuantityTransferredToNote property
        public virtual RamAccess<string> QuantityTransferredToNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(QuantityTransferredToNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(QuantityTransferredToNote), value);
                }
                OnPropertyChanged(nameof(QuantityTransferredToNote));
            }
        }
        // positive int.
                private bool QuantityTransferredToNote_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityTransferredToNote property

        //MassRefined Property
        [Attributes.Form_Property("Масса переработанного, т")]public int? MassRefinedId { get; set; }
        public virtual RamAccess<string> MassRefined
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassRefined));
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

                private bool MassRefined_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //MassRefined Property

        //QuantityRefined property
        [Attributes.Form_Property("Количество переработанного, шт.")]public int? QuantityRefinedId { get; set; }
        public virtual RamAccess<string> QuantityRefined
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(QuantityRefined));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(QuantityRefined), value);
                }
                OnPropertyChanged(nameof(QuantityRefined));
            }
        }
        // positive int.
                private bool QuantityRefined_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityRefined property

        //QuantityRefinedNote property
        public virtual RamAccess<string> QuantityRefinedNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(QuantityRefinedNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(QuantityRefinedNote), value);
                }
                OnPropertyChanged(nameof(QuantityRefinedNote));
            }
        }
        // positive int.
                private bool QuantityRefinedNote_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityRefinedNote property

        //MassRemovedFromAccount Property
        [Attributes.Form_Property("Масса снятого с учета, т")]public int? MassRemovedFromAccountId { get; set; }
        public virtual RamAccess<string> MassRemovedFromAccount
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassRemovedFromAccount));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(MassRemovedFromAccount), value);
                }
                OnPropertyChanged(nameof(MassRemovedFromAccount));
            }
        }

                private bool MassRemovedFromAccount_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //MassRemovedFromAccount Property

        //QuantityRemovedFromAccount property
        [Attributes.Form_Property("Количество снятого с учета, шт.")]public int? QuantityRemovedFromAccountId { get; set; }
        public virtual RamAccess<string> QuantityRemovedFromAccount
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(QuantityRemovedFromAccount));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(QuantityRemovedFromAccount), value);
                }
                OnPropertyChanged(nameof(QuantityRemovedFromAccount));
            }
        }
        // positive int.
                private bool QuantityRemovedFromAccount_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                int k = int.Parse(value.Value);
                if (k <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityRemovedFromAccount property

        ////QuantityRemovedFromNote property
        //public virtual RamAccess<string> QuantityRemovedFromNote
        //{
        //    get
        //    {
                
        //        {
        //            return _dataAccess.Get<string>(nameof(QuantityRemovedFromNote));//OK
                    
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {

                
        //        {
        //            _dataAccess.Set(nameof(QuantityRemovedFromNote), value);
        //        }
        //        OnPropertyChanged(nameof(QuantityRemovedFromNote));
        //    }
        //}
        //// positive int.
        //        private bool QuantityRemovedFromNote_Validation(RamAccess<string> value)//Ready
        //{
        //    value.ClearErrors();
        //    if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        //    {
        //        return true;
        //    }
        //    try
        //    {
        //        int k = int.Parse(value.Value);
        //        if (k <= 0)
        //        {
        //            value.AddError("Недопустимое значение");
        //            return false;
        //        }
        //    }
        //    catch
        //    {
        //        value.AddError("Недопустимое значение");
        //        return false;
        //    }
        //    return true;
        //}
        ////QuantityRemovedFromNote property
    }
}
