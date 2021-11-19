using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.RegularExpressions;
using System;
using System.ComponentModel;
using System.Linq;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.4: Постановка на учет и снятие с учета РВ, содержащихся в отработавшем ядерном топливе")]
    public class Form24 : Abstracts.Form2
    {
        public Form24() : base()
        {
            FormNum.Value = "2.4";
            //NumberOfFields.Value = 26;
            Validate_all();
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
            return !(CodeOYAT.HasErrors||
            FcpNumber.HasErrors||
            QuantityFromAnothers.HasErrors||
            QuantityFromAnothersImported.HasErrors||
            QuantityCreated.HasErrors||
            QuantityRemovedFromAccount.HasErrors||
            MassCreated.HasErrors||
            MassFromAnothers.HasErrors||
            MassFromAnothersImported.HasErrors||
            MassRemovedFromAccount.HasErrors||
            QuantityTransferredToAnother.HasErrors||
            MassAnotherReasons.HasErrors||
            MassTransferredToAnother.HasErrors||
            QuantityAnotherReasons.HasErrors||
            QuantityRefined.HasErrors||
            MassRefined.HasErrors);
        }

        //CodeOYAT property
        #region  CodeOYAT
        public string CodeOYAT_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Код ОЯТ")]
        public RamAccess<string> CodeOYAT
        {
            get
            {
                    var tmp = new RamAccess<string>(CodeOYAT_Validation, CodeOYAT_DB);
                    tmp.PropertyChanged += CodeOYATValueChanged;
                    return tmp;
            }
            set
            {
                    CodeOYAT_DB = value.Value;
                OnPropertyChanged(nameof(CodeOYAT));
            }
        }

        private void CodeOYATValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                CodeOYAT_DB = ((RamAccess<string>)Value).Value;
}
}
private bool CodeOYAT_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено"); return false;
            }
            Regex a = new Regex("^[0-9]{5}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //CodeOYAT property
        #endregion

        //FcpNumber property
        #region  FcpNumber
        public string FcpNumber_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public RamAccess<string> FcpNumber
        {
            get
            {
                    var tmp = new RamAccess<string>(FcpNumber_Validation, FcpNumber_DB);
                    tmp.PropertyChanged += FcpNumberValueChanged;
                    return tmp;
            }
            set
            {
                    FcpNumber_DB = value.Value;
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

                private void FcpNumberValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                FcpNumber_DB = ((RamAccess<string>)Value).Value;
}
}
private bool FcpNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            return true;
        }
        //FcpNumber property
        #endregion

        //MassCreated Property
        #region  MassCreated
        public string MassCreated_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("масса, т")]
        public RamAccess<string> MassCreated
        {
            get
            {
                    var tmp = new RamAccess<string>(MassCreated_Validation, MassCreated_DB);
                    tmp.PropertyChanged += MassCreatedValueChanged;
                    return tmp;
            }
            set
            {
                    MassCreated_DB = value.Value;
                OnPropertyChanged(nameof(MassCreated));
            }
        }

        private void MassCreatedValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                MassCreated_DB = value1;
            }
        }
        private bool MassCreated_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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
        #endregion

        //QuantityCreated property
        #region  QuantityCreated
        public string QuantityCreated_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("количество, шт.")]
        public RamAccess<string> QuantityCreated
        {
            get
            {
                    var tmp = new RamAccess<string>(QuantityCreated_Validation, QuantityCreated_DB);//OK
                    tmp.PropertyChanged += QuantityCreatedValueChanged;
                    return tmp;
            }
            set
            {
                    QuantityCreated_DB = value.Value;
                OnPropertyChanged(nameof(QuantityCreated));
            }
        }
        // positive int.
                private void QuantityCreatedValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                QuantityCreated_DB = ((RamAccess<string>)Value).Value;
}
}
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
        #endregion

        //MassFromAnothers Property
        #region  MassFromAnothers
        public string MassFromAnothers_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("масса, т")]
        public RamAccess<string> MassFromAnothers
        {
            get
            {
                    var tmp = new RamAccess<string>(MassFromAnothers_Validation, MassFromAnothers_DB);
                    tmp.PropertyChanged += MassFromAnothersValueChanged;
                    return tmp;
            }
            set
            {
                    MassFromAnothers_DB = value.Value;
                OnPropertyChanged(nameof(MassFromAnothers));
            }
        }

        private void MassFromAnothersValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                MassFromAnothers_DB = value1;
            }
        }
        private bool MassFromAnothers_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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
        #endregion

        //QuantityFromAnothers property
        #region  QuantityFromAnothers
        public string QuantityFromAnothers_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("количество, шт.")]
        public RamAccess<string> QuantityFromAnothers
        {
            get
            {
                    var tmp = new RamAccess<string>(QuantityFromAnothers_Validation, QuantityFromAnothers_DB);//OK
                    tmp.PropertyChanged += QuantityFromAnothersValueChanged;
                    return tmp;
            }
            set
            {
                    QuantityFromAnothers_DB = value.Value;
                OnPropertyChanged(nameof(QuantityFromAnothers));
            }
        }
        // positive int.
                private void QuantityFromAnothersValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                QuantityFromAnothers_DB = ((RamAccess<string>)Value).Value;
}
}
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
        #endregion

        //MassFromAnothersImported Property
        #region  MassFromAnothersImported
        public string MassFromAnothersImported_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("масса, т")]
        public RamAccess<string> MassFromAnothersImported
        {
            get
            {
                    var tmp = new RamAccess<string>(MassFromAnothersImported_Validation, MassFromAnothersImported_DB);
                    tmp.PropertyChanged += MassFromAnothersImportedValueChanged;
                    return tmp;
            }
            set
            {
                    MassFromAnothersImported_DB = value.Value;
                OnPropertyChanged(nameof(MassFromAnothersImported));
            }
        }

        private void MassFromAnothersImportedValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                MassFromAnothersImported_DB = value1;
            }
        }
        private bool MassFromAnothersImported_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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
        #endregion

        //QuantityFromAnothersImported property
        #region  QuantityFromAnothersImported
        public string QuantityFromAnothersImported_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("количество, шт.")]
        public RamAccess<string> QuantityFromAnothersImported
        {
            get
            {
                    var tmp = new RamAccess<string>(QuantityFromAnothersImported_Validation, QuantityFromAnothersImported_DB);//OK
                    tmp.PropertyChanged += QuantityFromAnothersImportedValueChanged;
                    return tmp;
            }
            set
            {
                    QuantityFromAnothersImported_DB = value.Value;
                OnPropertyChanged(nameof(QuantityFromAnothersImported));
            }
        }
        // positive int.
                private void QuantityFromAnothersImportedValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                QuantityFromAnothersImported_DB = ((RamAccess<string>)Value).Value;
}
}
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
        #endregion

        //MassAnotherReasons Property
        #region  MassAnotherReasons
        public string MassAnotherReasons_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("масса, т")]
        public RamAccess<string> MassAnotherReasons
        {
            get
            {
                    var tmp = new RamAccess<string>(MassAnotherReasons_Validation, MassAnotherReasons_DB);
                    tmp.PropertyChanged += MassAnotherReasonsValueChanged;
                    return tmp;
            }
            set
            {
                    MassAnotherReasons_DB = value.Value;
                OnPropertyChanged(nameof(MassAnotherReasons));
            }
        }

        private void MassAnotherReasonsValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                MassAnotherReasons_DB = value1;
            }
        }
        private bool MassAnotherReasons_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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
        #endregion

        //QuantityAnotherReasons property
        #region  QuantityAnotherReasons
        public string QuantityAnotherReasons_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("количество, шт.")]
        public RamAccess<string> QuantityAnotherReasons
        {
            get
            {
                var tmp = new RamAccess<string>(QuantityAnotherReasons_Validation, QuantityAnotherReasons_DB);//OK
                tmp.PropertyChanged += QuantityAnotherReasonsValueChanged;
                return tmp;
            }
            set
            {
                    QuantityAnotherReasons_DB = value.Value;
                OnPropertyChanged(nameof(QuantityAnotherReasons));
            }
        }
        // positive int.
                private void QuantityAnotherReasonsValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                QuantityAnotherReasons_DB = ((RamAccess<string>)Value).Value;
}
}
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
        #endregion

        //MassTransferredToAnother Property
        #region  MassTransferredToAnother
        public string MassTransferredToAnother_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("масса, т")]
        public RamAccess<string> MassTransferredToAnother
        {
            get
            {
                    var tmp = new RamAccess<string>(MassTransferredToAnother_Validation, MassTransferredToAnother_DB);
                    tmp.PropertyChanged += MassTransferredToAnotherValueChanged;
                    return tmp;
            }
            set
            {
                    MassTransferredToAnother_DB = value.Value;
                OnPropertyChanged(nameof(MassTransferredToAnother));
            }
        }

        private void MassTransferredToAnotherValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                MassTransferredToAnother_DB = value1;
            }
        }
        private bool MassTransferredToAnother_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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
        #endregion

        //QuantityTransferredToAnother property
        #region  QuantityTransferredToAnother
        public string QuantityTransferredToAnother_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("количество, шт.")]
        public RamAccess<string> QuantityTransferredToAnother
        {
            get
            {
                    var tmp = new RamAccess<string>(QuantityTransferredToAnother_Validation, QuantityTransferredToAnother_DB);//OK
                    tmp.PropertyChanged += QuantityTransferredToAnotherValueChanged;
                    return tmp;
            }
            set
            {
                    QuantityTransferredToAnother_DB = value.Value;
                OnPropertyChanged(nameof(QuantityTransferredToAnother));
            }
        }
        // positive int.
                private void QuantityTransferredToAnotherValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                QuantityTransferredToAnother_DB = ((RamAccess<string>)Value).Value;
}
}
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
        #endregion

        //MassRefined Property
        #region  MassRefined
        public string MassRefined_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("масса, т")]
        public RamAccess<string> MassRefined
        {
            get
            {
                    var tmp = new RamAccess<string>(MassRefined_Validation, MassRefined_DB);
                    tmp.PropertyChanged += MassRefinedValueChanged;
                    return tmp;
            }
            set
            {
                    MassRefined_DB = value.Value;
                OnPropertyChanged(nameof(MassRefined));
            }
        }

        private void MassRefinedValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                MassRefined_DB = value1;
            }
        }
        private bool MassRefined_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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
        #endregion

        //QuantityRefined property
        #region  QuantityRefined
        public string QuantityRefined_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("количество, шт.")]
        public RamAccess<string> QuantityRefined
        {
            get
            {
                    var tmp = new RamAccess<string>(QuantityRefined_Validation, QuantityRefined_DB);//OK
                    tmp.PropertyChanged += QuantityRefinedValueChanged;
                    return tmp;
            }
            set
            {
                    QuantityRefined_DB = value.Value;
                OnPropertyChanged(nameof(QuantityRefined));
            }
        }
        // positive int.
                private void QuantityRefinedValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                QuantityRefined_DB = ((RamAccess<string>)Value).Value;
}
}
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
        #endregion

        //MassRemovedFromAccount Property
        #region  MassRemovedFromAccount
        public string MassRemovedFromAccount_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("масса, т")]
        public RamAccess<string> MassRemovedFromAccount
        {
            get
            {
                    var tmp = new RamAccess<string>(MassRemovedFromAccount_Validation, MassRemovedFromAccount_DB);
                    tmp.PropertyChanged += MassRemovedFromAccountValueChanged;
                    return tmp;
            }
            set
            {
                    MassRemovedFromAccount_DB = value.Value;
                OnPropertyChanged(nameof(MassRemovedFromAccount));
            }
        }

        private void MassRemovedFromAccountValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                MassRemovedFromAccount_DB = value1;
            }
        }
        private bool MassRemovedFromAccount_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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
        #endregion

        //QuantityRemovedFromAccount property
        #region  QuantityRemovedFromAccount
        public string QuantityRemovedFromAccount_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("количество, шт.")]
        public RamAccess<string> QuantityRemovedFromAccount
        {
            get
            {
                    var tmp = new RamAccess<string>(QuantityRemovedFromAccount_Validation, QuantityRemovedFromAccount_DB);//OK
                    tmp.PropertyChanged += QuantityRemovedFromAccountValueChanged;
                    return tmp;
            }
            set
            {
                    QuantityRemovedFromAccount_DB = value.Value;
                OnPropertyChanged(nameof(QuantityRemovedFromAccount));
            }
        }
        // positive int.
                private void QuantityRemovedFromAccountValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                QuantityRemovedFromAccount_DB = ((RamAccess<string>)Value).Value;
}
}
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
        #endregion

        #region IExcel
        public void ExcelRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelRow(worksheet, Row);
            worksheet.Cells[Row, 2].Value = CodeOYAT_DB;
            worksheet.Cells[Row, 3].Value = FcpNumber_DB;
            worksheet.Cells[Row, 4].Value = MassCreated_DB;
            worksheet.Cells[Row, 5].Value = QuantityCreated_DB;
            worksheet.Cells[Row, 6].Value = MassFromAnothers_DB;
            worksheet.Cells[Row, 7].Value = QuantityFromAnothers_DB;
            worksheet.Cells[Row, 8].Value = MassFromAnothersImported_DB;
            worksheet.Cells[Row, 9].Value = QuantityFromAnothersImported_DB;
            worksheet.Cells[Row, 10].Value = MassAnotherReasons_DB;
            worksheet.Cells[Row, 11].Value = QuantityAnotherReasons_DB;
            worksheet.Cells[Row, 12].Value = MassTransferredToAnother_DB;
            worksheet.Cells[Row, 13].Value = QuantityTransferredToAnother_DB;
            worksheet.Cells[Row, 14].Value = MassRefined_DB;
            worksheet.Cells[Row, 15].Value = QuantityRefined_DB;
            worksheet.Cells[Row, 16].Value = MassRemovedFromAccount_DB;
            worksheet.Cells[Row, 17].Value = QuantityRemovedFromAccount_DB;
        }

        public static void ExcelHeader(ExcelWorksheet worksheet)
        {
            Form2.ExcelHeader(worksheet);
            worksheet.Cells[1, 2].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(CodeOYAT)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 3].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(FcpNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 4].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(MassCreated)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 5].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(QuantityCreated)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 6].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(MassFromAnothers)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 7].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(QuantityFromAnothers)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 8].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(MassFromAnothersImported)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 9].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(QuantityFromAnothersImported)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 10].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(MassAnotherReasons)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 11].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(QuantityAnotherReasons)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 12].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(MassTransferredToAnother)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 13].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(QuantityTransferredToAnother)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 14].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(MassRefined)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 15].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(QuantityRefined)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 16].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(MassRemovedFromAccount)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 17].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(QuantityRemovedFromAccount)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
        }
        #endregion
    }
}