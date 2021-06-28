using Models.DataAccess;
using System;
using System.Globalization;

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
            Init();
            Validate_all();
        }

        private void Init()
        {
            DataAccess.Init<string>(nameof(CodeOYAT), CodeOYAT_Validation, null);
            CodeOYAT.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(FcpNumber), FcpNumber_Validation, null);
            FcpNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(QuantityFromAnothers), QuantityFromAnothers_Validation, null);
            QuantityFromAnothers.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(QuantityFromAnothersImported), QuantityFromAnothersImported_Validation, null);
            QuantityFromAnothersImported.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(QuantityCreated), QuantityCreated_Validation, null);
            QuantityCreated.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(QuantityRemovedFromAccount), QuantityRemovedFromAccount_Validation, null);
            QuantityRemovedFromAccount.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(MassCreated), MassCreated_Validation, null);
            MassCreated.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(MassFromAnothers), MassFromAnothers_Validation, null);
            MassFromAnothers.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(MassFromAnothersImported), MassFromAnothersImported_Validation, null);
            MassFromAnothersImported.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(MassRemovedFromAccount), MassRemovedFromAccount_Validation, null);
            MassRemovedFromAccount.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(QuantityTransferredToAnother), QuantityTransferredToAnother_Validation, null);
            QuantityTransferredToAnother.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(MassAnotherReasons), MassAnotherReasons_Validation, null);
            MassAnotherReasons.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(MassTransferredToAnother), MassTransferredToAnother_Validation, null);
            MassTransferredToAnother.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(QuantityAnotherReasons), QuantityAnotherReasons_Validation, null);
            QuantityAnotherReasons.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(QuantityRefined), QuantityRefined_Validation, null);
            QuantityRefined.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(MassRefined), MassRefined_Validation, null);
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
        [Attributes.Form_Property("Код ОЯТ")]
        public RamAccess<string> CodeOYAT
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(CodeOYAT));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(CodeOYAT), value);
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
        public RamAccess<string> CodeOYATnote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(CodeOYATnote));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(CodeOYATnote), value);
                }
                OnPropertyChanged(nameof(CodeOYATnote));
            }
        }

        private bool CodeOYATnote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //CodeOYATnote property

        //FcpNumber property
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public RamAccess<string> FcpNumber
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(FcpNumber));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(FcpNumber), value);
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
        [Attributes.Form_Property("Масса образованного, т")]
        public RamAccess<string> MassCreated
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(MassCreated));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(MassCreated), value);
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        [Attributes.Form_Property("Количество образованного, шт.")]
        public RamAccess<string> QuantityCreated
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(QuantityCreated));//OK

                }

                {

                }
            }
            set
            {




                {
                    DataAccess.Set(nameof(QuantityCreated), value);
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

        //QuantityCreatedNote property
        public RamAccess<string> QuantityCreatedNote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(QuantityCreatedNote));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(QuantityCreatedNote), value);
                }
                OnPropertyChanged(nameof(QuantityCreatedNote));
            }
        }
        // positive int.
        private bool QuantityCreatedNote_Validation(RamAccess<string> value)//Ready
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
        //QuantityCreatedNote property

        //MassFromAnothers Property
        [Attributes.Form_Property("Масса поступившего от сторонних, т")]
        public RamAccess<string> MassFromAnothers
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(MassFromAnothers));
                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(MassFromAnothers), value);
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        [Attributes.Form_Property("Количество поступившего от сторонних, шт.")]
        public RamAccess<string> QuantityFromAnothers
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(QuantityFromAnothers));//OK

                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(QuantityFromAnothers), value);
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
        public RamAccess<string> QuantityFromAnothersNote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(QuantityFromAnothersNote));//OK

                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(QuantityFromAnothersNote), value);
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
        [Attributes.Form_Property("Масса импортированного от сторонних, т")]
        public RamAccess<string> MassFromAnothersImported
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(MassFromAnothersImported));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(MassFromAnothersImported), value);
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        [Attributes.Form_Property("Количество импортированного от сторонних, шт.")]
        public RamAccess<string> QuantityFromAnothersImported
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(QuantityFromAnothersImported));//OK

                }

                {

                }
            }
            set
            {




                {
                    DataAccess.Set(nameof(QuantityFromAnothersImported), value);
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
        public RamAccess<string> QuantityFromImportedNote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(QuantityFromImportedNote));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(QuantityFromImportedNote), value);
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
        [Attributes.Form_Property("Масса поставленного на учет по другим причинам, т")]
        public RamAccess<string> MassAnotherReasons
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(MassAnotherReasons));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(MassAnotherReasons), value);
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        [Attributes.Form_Property("Количество поступившего на учет по другим причинам, шт.")]
        public RamAccess<string> QuantityAnotherReasons
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(QuantityAnotherReasons));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(QuantityAnotherReasons), value);
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
        public RamAccess<string> QuantityAnotherReasonsNote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(QuantityAnotherReasonsNote));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(QuantityAnotherReasonsNote), value);
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
        [Attributes.Form_Property("Масса переданного сторонним, т")]
        public RamAccess<string> MassTransferredToAnother
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(MassTransferredToAnother));
                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(MassTransferredToAnother), value);
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        [Attributes.Form_Property("Количество переданного сторонним, шт.")]
        public RamAccess<string> QuantityTransferredToAnother
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(QuantityTransferredToAnother));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(QuantityTransferredToAnother), value);
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
        public RamAccess<string> QuantityTransferredToNote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(QuantityTransferredToNote));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(QuantityTransferredToNote), value);
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
        [Attributes.Form_Property("Масса переработанного, т")]
        public RamAccess<string> MassRefined
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(MassRefined));
                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(MassRefined), value);
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        [Attributes.Form_Property("Количество переработанного, шт.")]
        public RamAccess<string> QuantityRefined
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(QuantityRefined));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(QuantityRefined), value);
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
        public RamAccess<string> QuantityRefinedNote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(QuantityRefinedNote));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(QuantityRefinedNote), value);
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
        [Attributes.Form_Property("Масса снятого с учета, т")]
        public RamAccess<string> MassRemovedFromAccount
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(MassRemovedFromAccount));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(MassRemovedFromAccount), value);
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        [Attributes.Form_Property("Количество снятого с учета, шт.")]
        public RamAccess<string> QuantityRemovedFromAccount
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(QuantityRemovedFromAccount));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(QuantityRemovedFromAccount), value);
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

        //QuantityRemovedFromNote property
        [Attributes.Form_Property("Количество снятого с учета, шт.")]
        public RamAccess<string> QuantityRemovedFromNote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(QuantityRemovedFromNote));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(QuantityRemovedFromNote), value);
                }
                OnPropertyChanged(nameof(QuantityRemovedFromNote));
            }
        }
        // positive int.
        private bool QuantityRemovedFromNote_Validation(RamAccess<string> value)//Ready
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
        //QuantityRemovedFromNote property
    }
}
