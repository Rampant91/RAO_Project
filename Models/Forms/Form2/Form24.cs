using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization; using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.ComponentModel;
using System.Linq;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using Models.Collections;

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

        [Attributes.Form_Property(true,"Форма")]
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
        [Attributes.Form_Property(true,"null-2","Код ОЯТ","2")]
        public RamAccess<string> CodeOYAT
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(CodeOYAT)))
                {
                    ((RamAccess<string>)Dictionary[nameof(CodeOYAT)]).Value = CodeOYAT_DB;
                    return (RamAccess<string>)Dictionary[nameof(CodeOYAT)];
                }
                else
                {
                    var rm = new RamAccess<string>(CodeOYAT_Validation, CodeOYAT_DB);
                    rm.PropertyChanged += CodeOYATValueChanged;
                    Dictionary.Add(nameof(CodeOYAT), rm);
                    return (RamAccess<string>)Dictionary[nameof(CodeOYAT)];
                }
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
        [Attributes.Form_Property(true, "null-3","Номер мероприятия ФЦП","3")]
        public RamAccess<string> FcpNumber
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(FcpNumber)))
                {
                    ((RamAccess<string>)Dictionary[nameof(FcpNumber)]).Value = FcpNumber_DB;
                    return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
                }
                else
                {
                    var rm = new RamAccess<string>(FcpNumber_Validation, FcpNumber_DB);
                    rm.PropertyChanged += FcpNumberValueChanged;
                    Dictionary.Add(nameof(FcpNumber), rm);
                    return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
                }
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
        [Attributes.Form_Property(true, "Поставлено на учет в организации", "масса образованного, т","4")]
        public RamAccess<string> MassCreated
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MassCreated)))
                {
                    ((RamAccess<string>)Dictionary[nameof(MassCreated)]).Value = MassCreated_DB;
                    return (RamAccess<string>)Dictionary[nameof(MassCreated)];
                }
                else
                {
                    var rm = new RamAccess<string>(MassCreated_Validation, MassCreated_DB);
                    rm.PropertyChanged += MassCreatedValueChanged;
                    Dictionary.Add(nameof(MassCreated), rm);
                    return (RamAccess<string>)Dictionary[nameof(MassCreated)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        MassCreated_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        public string QuantityCreated_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true, "Поставлено на учет в организации", "количество образованного, шт","5")]
        public RamAccess<string> QuantityCreated
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(QuantityCreated)))
                {
                    ((RamAccess<string>)Dictionary[nameof(QuantityCreated)]).Value = QuantityCreated_DB;
                    return (RamAccess<string>)Dictionary[nameof(QuantityCreated)];
                }
                else
                {
                    var rm = new RamAccess<string>(QuantityCreated_Validation, QuantityCreated_DB);
                    rm.PropertyChanged += QuantityCreatedValueChanged;
                    Dictionary.Add(nameof(QuantityCreated), rm);
                    return (RamAccess<string>)Dictionary[nameof(QuantityCreated)];
                }
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
        public string MassFromAnothers_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Поставлено на учет в организации", "масса поступивших от сторонних, т","6")]
        public RamAccess<string> MassFromAnothers
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MassFromAnothers)))
                {
                    ((RamAccess<string>)Dictionary[nameof(MassFromAnothers)]).Value = MassFromAnothers_DB;
                    return (RamAccess<string>)Dictionary[nameof(MassFromAnothers)];
                }
                else
                {
                    var rm = new RamAccess<string>(MassFromAnothers_Validation, MassFromAnothers_DB);
                    rm.PropertyChanged += MassFromAnothersValueChanged;
                    Dictionary.Add(nameof(MassFromAnothers), rm);
                    return (RamAccess<string>)Dictionary[nameof(MassFromAnothers)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        MassFromAnothers_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        [Attributes.Form_Property(true,"Поставлено на учет в организации", "количество поступиших от сторонних, шт","7")]
        public RamAccess<string> QuantityFromAnothers
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(QuantityFromAnothers)))
                {
                    ((RamAccess<string>)Dictionary[nameof(QuantityFromAnothers)]).Value = QuantityFromAnothers_DB;
                    return (RamAccess<string>)Dictionary[nameof(QuantityFromAnothers)];
                }
                else
                {
                    var rm = new RamAccess<string>(QuantityFromAnothers_Validation, QuantityFromAnothers_DB);
                    rm.PropertyChanged += QuantityFromAnothersValueChanged;
                    Dictionary.Add(nameof(QuantityFromAnothers), rm);
                    return (RamAccess<string>)Dictionary[nameof(QuantityFromAnothers)];
                }
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
        [Attributes.Form_Property(true,"Поставлено на учет в организации", "масса импортированных от сторонних, т","8")]
        public RamAccess<string> MassFromAnothersImported
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MassFromAnothersImported)))
                {
                    ((RamAccess<string>)Dictionary[nameof(MassFromAnothersImported)]).Value = MassFromAnothersImported_DB;
                    return (RamAccess<string>)Dictionary[nameof(MassFromAnothersImported)];
                }
                else
                {
                    var rm = new RamAccess<string>(MassFromAnothersImported_Validation, MassFromAnothersImported_DB);
                    rm.PropertyChanged += MassFromAnothersImportedValueChanged;
                    Dictionary.Add(nameof(MassFromAnothersImported), rm);
                    return (RamAccess<string>)Dictionary[nameof(MassFromAnothersImported)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        MassFromAnothersImported_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        [Attributes.Form_Property(true,"Поставлено на учет в организации","количество импортированных от сторонних, шт","9")]
        public RamAccess<string> QuantityFromAnothersImported
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(QuantityFromAnothersImported)))
                {
                    ((RamAccess<string>)Dictionary[nameof(QuantityFromAnothersImported)]).Value = QuantityFromAnothersImported_DB;
                    return (RamAccess<string>)Dictionary[nameof(QuantityFromAnothersImported)];
                }
                else
                {
                    var rm = new RamAccess<string>(QuantityFromAnothersImported_Validation, QuantityFromAnothersImported_DB);
                    rm.PropertyChanged += QuantityFromAnothersImportedValueChanged;
                    Dictionary.Add(nameof(QuantityFromAnothersImported), rm);
                    return (RamAccess<string>)Dictionary[nameof(QuantityFromAnothersImported)];
                }
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
        [Attributes.Form_Property(true,"Поставлено на учет в организации","масса учтенных по другим причинам, т","10")]
        public RamAccess<string> MassAnotherReasons
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MassAnotherReasons)))
                {
                    ((RamAccess<string>)Dictionary[nameof(MassAnotherReasons)]).Value = MassAnotherReasons_DB;
                    return (RamAccess<string>)Dictionary[nameof(MassAnotherReasons)];
                }
                else
                {
                    var rm = new RamAccess<string>(MassAnotherReasons_Validation, MassAnotherReasons_DB);
                    rm.PropertyChanged += MassAnotherReasonsValueChanged;
                    Dictionary.Add(nameof(MassAnotherReasons), rm);
                    return (RamAccess<string>)Dictionary[nameof(MassAnotherReasons)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        MassAnotherReasons_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        public string QuantityAnotherReasons_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Поставлено на учет в организации","количество учтенных по другим причинам, шт","11")]
        public RamAccess<string> QuantityAnotherReasons
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(QuantityAnotherReasons)))
                {
                    ((RamAccess<string>)Dictionary[nameof(QuantityAnotherReasons)]).Value = QuantityAnotherReasons_DB;
                    return (RamAccess<string>)Dictionary[nameof(QuantityAnotherReasons)];
                }
                else
                {
                    var rm = new RamAccess<string>(QuantityAnotherReasons_Validation, QuantityAnotherReasons_DB);
                    rm.PropertyChanged += QuantityAnotherReasonsValueChanged;
                    Dictionary.Add(nameof(QuantityAnotherReasons), rm);
                    return (RamAccess<string>)Dictionary[nameof(QuantityAnotherReasons)];
                }
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
        public string MassTransferredToAnother_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Снято с учета в организации","масса переданных сторонним, т","12")]
        public RamAccess<string> MassTransferredToAnother
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MassTransferredToAnother)))
                {
                    ((RamAccess<string>)Dictionary[nameof(MassTransferredToAnother)]).Value = MassTransferredToAnother_DB;
                    return (RamAccess<string>)Dictionary[nameof(MassTransferredToAnother)];
                }
                else
                {
                    var rm = new RamAccess<string>(MassTransferredToAnother_Validation, MassTransferredToAnother_DB);
                    rm.PropertyChanged += MassTransferredToAnotherValueChanged;
                    Dictionary.Add(nameof(MassTransferredToAnother), rm);
                    return (RamAccess<string>)Dictionary[nameof(MassTransferredToAnother)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        MassTransferredToAnother_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        [Attributes.Form_Property(true,"Снято с учета в организации","количество переданных сторонним, шт","13")]
        public RamAccess<string> QuantityTransferredToAnother
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(QuantityTransferredToAnother)))
                {
                    ((RamAccess<string>)Dictionary[nameof(QuantityTransferredToAnother)]).Value = QuantityTransferredToAnother_DB;
                    return (RamAccess<string>)Dictionary[nameof(QuantityTransferredToAnother)];
                }
                else
                {
                    var rm = new RamAccess<string>(QuantityTransferredToAnother_Validation, QuantityTransferredToAnother_DB);
                    rm.PropertyChanged += QuantityTransferredToAnotherValueChanged;
                    Dictionary.Add(nameof(QuantityTransferredToAnother), rm);
                    return (RamAccess<string>)Dictionary[nameof(QuantityTransferredToAnother)];
                }
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
        [Attributes.Form_Property(true,"Снято с учета в организации","масса переработанных, т","14")]
        public RamAccess<string> MassRefined
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MassRefined)))
                {
                    ((RamAccess<string>)Dictionary[nameof(MassRefined)]).Value = MassRefined_DB;
                    return (RamAccess<string>)Dictionary[nameof(MassRefined)];
                }
                else
                {
                    var rm = new RamAccess<string>(MassRefined_Validation, MassRefined_DB);
                    rm.PropertyChanged += MassRefinedValueChanged;
                    Dictionary.Add(nameof(MassRefined), rm);
                    return (RamAccess<string>)Dictionary[nameof(MassRefined)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        MassRefined_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        [Attributes.Form_Property(true,"Снято с учета в организации","количество переработанных, шт","15")]
        public RamAccess<string> QuantityRefined
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(QuantityRefined)))
                {
                    ((RamAccess<string>)Dictionary[nameof(QuantityRefined)]).Value = QuantityRefined_DB;
                    return (RamAccess<string>)Dictionary[nameof(QuantityRefined)];
                }
                else
                {
                    var rm = new RamAccess<string>(QuantityRefined_Validation, QuantityRefined_DB);
                    rm.PropertyChanged += QuantityRefinedValueChanged;
                    Dictionary.Add(nameof(QuantityRefined), rm);
                    return (RamAccess<string>)Dictionary[nameof(QuantityRefined)];
                }
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
        [Attributes.Form_Property(true,"Снято с учета в организации","масса снятых с учета, т","16")]
        public RamAccess<string> MassRemovedFromAccount
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MassRemovedFromAccount)))
                {
                    ((RamAccess<string>)Dictionary[nameof(MassRemovedFromAccount)]).Value = MassRemovedFromAccount_DB;
                    return (RamAccess<string>)Dictionary[nameof(MassRemovedFromAccount)];
                }
                else
                {
                    var rm = new RamAccess<string>(MassRemovedFromAccount_Validation, MassRemovedFromAccount_DB);
                    rm.PropertyChanged += MassRemovedFromAccountValueChanged;
                    Dictionary.Add(nameof(MassRemovedFromAccount), rm);
                    return (RamAccess<string>)Dictionary[nameof(MassRemovedFromAccount)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        MassRemovedFromAccount_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        [Attributes.Form_Property(true,"Снято с учета в организации","количество снятых с учета, шт","17")]
        public RamAccess<string> QuantityRemovedFromAccount
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(QuantityRemovedFromAccount)))
                {
                    ((RamAccess<string>)Dictionary[nameof(QuantityRemovedFromAccount)]).Value = QuantityRemovedFromAccount_DB;
                    return (RamAccess<string>)Dictionary[nameof(QuantityRemovedFromAccount)];
                }
                else
                {
                    var rm = new RamAccess<string>(QuantityRemovedFromAccount_Validation, QuantityRemovedFromAccount_DB);
                    rm.PropertyChanged += QuantityRemovedFromAccountValueChanged;
                    Dictionary.Add(nameof(QuantityRemovedFromAccount), rm);
                    return (RamAccess<string>)Dictionary[nameof(QuantityRemovedFromAccount)];
                }
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
        public void ExcelGetRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelGetRow(worksheet, Row);
            CodeOYAT_DB = Convert.ToString(worksheet.Cells[Row, 2].Value);
            FcpNumber_DB = Convert.ToString(worksheet.Cells[Row, 3].Value);
            MassCreated_DB = Convert.ToString(worksheet.Cells[Row, 4].Value);
            QuantityCreated_DB = Convert.ToString(worksheet.Cells[Row, 5].Value);
            MassFromAnothers_DB = Convert.ToString(worksheet.Cells[Row, 6].Value);
            QuantityFromAnothers_DB = Convert.ToString(worksheet.Cells[Row, 7].Value);
            MassFromAnothersImported_DB = Convert.ToString(worksheet.Cells[Row, 8].Value);
            QuantityFromAnothersImported_DB = Convert.ToString(worksheet.Cells[Row, 9].Value);
            MassAnotherReasons_DB = Convert.ToString(worksheet.Cells[Row, 10].Value);
            QuantityAnotherReasons_DB = Convert.ToString(worksheet.Cells[Row, 11].Value);
            MassTransferredToAnother_DB = Convert.ToString(worksheet.Cells[Row, 12].Value);
            QuantityTransferredToAnother_DB = Convert.ToString(worksheet.Cells[Row, 13].Value);
            MassRefined_DB = Convert.ToString(worksheet.Cells[Row, 14].Value);
            QuantityRefined_DB = Convert.ToString(worksheet.Cells[Row, 15].Value);
            MassRemovedFromAccount_DB = Convert.ToString(worksheet.Cells[Row, 16].Value);
            QuantityRemovedFromAccount_DB = Convert.ToString(worksheet.Cells[Row, 17].Value);

        }
        public int ExcelRow(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);
            int valInt;
            double val;

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = CodeOYAT_DB;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = FcpNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = MassCreated_DB== "" || MassCreated_DB == "-" || MassCreated_DB == null ? 0  : double.TryParse(MassCreated_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : MassCreated_DB;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = QuantityCreated_DB== "" || QuantityCreated_DB == "-" || QuantityCreated_DB == null ? 0  : int.TryParse(QuantityCreated_DB.Replace("(", "").Replace(")", "").Replace(".", ","), out valInt) ? valInt : QuantityCreated_DB;
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = MassFromAnothers_DB== "" || MassFromAnothers_DB == "-" || MassFromAnothers_DB == null ? 0  : double.TryParse(MassFromAnothers_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : MassFromAnothers_DB;
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = QuantityFromAnothers_DB== ""|| QuantityFromAnothers_DB == "-"|| QuantityFromAnothers_DB == null ? 0  : int.TryParse(QuantityFromAnothers_DB.Replace("(", "").Replace(")", "").Replace(".", ","), out valInt) ? valInt : QuantityFromAnothers_DB;
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = MassFromAnothersImported_DB== "" || MassFromAnothersImported_DB == "-"|| MassFromAnothersImported_DB == null ? 0  : double.TryParse(MassFromAnothersImported_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : MassFromAnothersImported_DB;
            worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = QuantityFromAnothersImported_DB== "" || QuantityFromAnothersImported_DB == "-" || QuantityFromAnothersImported_DB == null ? 0  : int.TryParse(QuantityFromAnothersImported_DB.Replace("(", "").Replace(")", "").Replace(".", ","), out valInt) ? valInt : QuantityFromAnothersImported_DB;
            worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = MassAnotherReasons_DB== "" || MassAnotherReasons_DB == "-" || MassAnotherReasons_DB == null ? 0  : double.TryParse(MassAnotherReasons_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : MassAnotherReasons_DB;
            worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = QuantityAnotherReasons_DB== "" || QuantityAnotherReasons_DB == "-" || QuantityAnotherReasons_DB == null ? 0  : int.TryParse(QuantityAnotherReasons_DB.Replace("(", "").Replace(")", "").Replace(".", ","), out valInt) ? valInt : QuantityAnotherReasons_DB;
            worksheet.Cells[Row + (Transpon == false ? 10 : 0), Column + (Transpon == true ? 10 : 0)].Value = MassTransferredToAnother_DB== "" || MassTransferredToAnother_DB == "-" || MassTransferredToAnother_DB == null ? 0  : double.TryParse(MassTransferredToAnother_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : MassTransferredToAnother_DB;
            worksheet.Cells[Row + (Transpon == false ? 11 : 0), Column + (Transpon == true ? 11 : 0)].Value = QuantityTransferredToAnother_DB== "" || QuantityTransferredToAnother_DB == "-" || QuantityTransferredToAnother_DB == null ? 0  : int.TryParse(QuantityTransferredToAnother_DB.Replace("(", "").Replace(")", "").Replace(".", ","), out valInt) ? valInt : QuantityTransferredToAnother_DB;
            worksheet.Cells[Row + (Transpon == false ? 12 : 0), Column + (Transpon == true ? 12 : 0)].Value = MassRefined_DB== "" || MassRefined_DB == "-" || MassRefined_DB == null ? 0  : double.TryParse(MassRefined_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : MassRefined_DB;
            worksheet.Cells[Row + (Transpon == false ? 13 : 0), Column + (Transpon == true ? 13 : 0)].Value = QuantityRefined_DB== "" || QuantityRefined_DB == "-" || QuantityRefined_DB == null ? 0  : int.TryParse(QuantityRefined_DB.Replace("(", "").Replace(")", "").Replace(".", ","), out valInt) ? valInt : QuantityRefined_DB;
            worksheet.Cells[Row + (Transpon == false ? 14 : 0), Column + (Transpon == true ? 14 : 0)].Value = MassRemovedFromAccount_DB== "" || MassRemovedFromAccount_DB == "-" || MassRemovedFromAccount_DB == null ? 0  : double.TryParse(MassRemovedFromAccount_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : MassRemovedFromAccount_DB;
            worksheet.Cells[Row + (Transpon == false ? 15 : 0), Column + (Transpon == true ? 15 : 0)].Value = QuantityRemovedFromAccount_DB== "" || QuantityRemovedFromAccount_DB == "-" || QuantityRemovedFromAccount_DB == null ? 0  : int.TryParse(QuantityRemovedFromAccount_DB.Replace("(", "").Replace(")", "").Replace(".", ","), out valInt) ? valInt : QuantityRemovedFromAccount_DB;
            return 16;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = Form2.ExcelHeader(worksheet, Row, Column, Transpon);
            Column = Column +(Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(CodeOYAT)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(FcpNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(MassCreated)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(QuantityCreated)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(MassFromAnothers)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(QuantityFromAnothers)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(MassFromAnothersImported)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(QuantityFromAnothersImported)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(MassAnotherReasons)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(QuantityAnotherReasons)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 10 : 0), Column + (Transpon == true ? 10 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(MassTransferredToAnother)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 11 : 0), Column + (Transpon == true ? 11 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(QuantityTransferredToAnother)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 12 : 0), Column + (Transpon == true ? 12 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(MassRefined)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 13 : 0), Column + (Transpon == true ? 13 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(QuantityRefined)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 14 : 0), Column + (Transpon == true ? 14 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(MassRemovedFromAccount)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 15 : 0), Column + (Transpon == true ? 15 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form24,Models").GetProperty(nameof(QuantityRemovedFromAccount)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            return 16;
        }
        #endregion

        #region IDataGridColumn
        private static DataGridColumns _DataGridColumns { get; set; } = null;
        public override DataGridColumns GetColumnStructure(string param = "")
        {
            if (_DataGridColumns == null)
            {
                #region NumberInOrder (1)
                //DataGridColumns insert = new DataGridColumns(); insert.name = ".kj";
                //DataGridColumns insert2 = new DataGridColumns(); insert2.name = "po["; insert2.parent = insert;
                //insert.innertCol = new List<DataGridColumns> { insert2 };
                var NumberInOrderR = ((Attributes.Form_PropertyAttribute)typeof(Form).GetProperty(nameof(Form.NumberInOrder)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                //insert.parent = NumberInOrderR;
                //List<DataGridColumns> child = NumberInOrderR.innertCol;
                //child[0].parent = insert.innertCol[0];
                //insert.innertCol[0].innertCol = child;
                //NumberInOrderR.innertCol = new List<DataGridColumns> { insert };
                NumberInOrderR.SetSizeColToAllLevels(50);
                
                NumberInOrderR.Binding = nameof(Form.NumberInOrder);
                NumberInOrderR.Blocked = true;
                NumberInOrderR.ChooseLine = true;
                #endregion
                #region CodeOYAT (2)
                //insert = new DataGridColumns(); insert.name = "gfh";
                //insert2 = new DataGridColumns(); insert2.name = "yiu"; insert2.parent = insert;
                //insert.innertCol = new List<DataGridColumns> { insert2 };
                DataGridColumns CodeOYATR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.CodeOYAT)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = CodeOYATR;
                //child = CodeOYATR.innertCol;
                //child[0].parent = insert.innertCol[0];
                //insert.innertCol[0].innertCol = child;
                //CodeOYATR.innertCol = new List<DataGridColumns> { insert };
                CodeOYATR.SetSizeColToAllLevels(88);
                CodeOYATR.Binding = nameof(Form24.CodeOYAT);
                NumberInOrderR += CodeOYATR;
                #endregion
                #region FcpNumber (3)
                //insert = new DataGridColumns(); insert.name = "nmm";
                //insert2 = new DataGridColumns(); insert2.name = "dsgf"; insert2.parent = insert;
                //insert.innertCol = new List<DataGridColumns> { insert2 };
                DataGridColumns FcpNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.FcpNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = FcpNumberR;
                //child = FcpNumberR.innertCol;
                //child[0].parent = insert.innertCol[0];
                //insert.innertCol[0].innertCol = child;
                //FcpNumberR.innertCol = new List<DataGridColumns> { insert };
                FcpNumberR.SetSizeColToAllLevels(140);
                FcpNumberR.Binding = nameof(Form24.FcpNumber);
                NumberInOrderR += FcpNumberR;
                #endregion
                #region MassCreated (4)
                //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
                DataGridColumns MassCreatedR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.MassCreated)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = MassCreatedR;
                //List<DataGridColumns> child = MassCreatedR.innertCol;
                //child[0].parent = insert;
                //insert.innertCol = child;
                //MassCreatedR.innertCol = new List<DataGridColumns> { insert };
                MassCreatedR.SetSizeColToAllLevels(160);
                MassCreatedR.Binding = nameof(Form24.MassCreated);
                NumberInOrderR += MassCreatedR;
                #endregion
                #region QuantityCreated (5)
                //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
                DataGridColumns QuantityCreatedR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.QuantityCreated)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = QuantityCreatedR;
                //child = QuantityCreatedR.innertCol;
                //child[0].parent = insert;
                //insert.innertCol = child;
                //QuantityCreatedR.innertCol = new List<DataGridColumns> { insert };
                QuantityCreatedR.SetSizeColToAllLevels(160);
                QuantityCreatedR.Binding = nameof(Form24.QuantityCreated);
                NumberInOrderR += QuantityCreatedR;
                #endregion
                #region MassFromAnothers (6)
                //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
                DataGridColumns MassFromAnothersR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.MassFromAnothers)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = MassFromAnothersR;
                //child = MassFromAnothersR.innertCol;
                //child[0].parent = insert;
                //insert.innertCol = child;
                //MassFromAnothersR.innertCol = new List<DataGridColumns> { insert };
                MassFromAnothersR.SetSizeColToAllLevels(160);
                MassFromAnothersR.Binding = nameof(Form24.MassFromAnothers);
                NumberInOrderR += MassFromAnothersR;
                #endregion
                #region QuantityFromAnothers (7)
                //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
                DataGridColumns QuantityFromAnothersR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.QuantityFromAnothers)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = QuantityFromAnothersR;
                //child = QuantityFromAnothersR.innertCol;
                //child[0].parent = insert;
                //insert.innertCol = child;
                //QuantityFromAnothersR.innertCol = new List<DataGridColumns> { insert };
                QuantityFromAnothersR.SetSizeColToAllLevels(160);
                QuantityFromAnothersR.Binding = nameof(Form24.QuantityFromAnothers);
                NumberInOrderR += QuantityFromAnothersR;
                #endregion
                #region MassFromAnothersImported (8)
                //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
                DataGridColumns MassFromAnothersImportedR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.MassFromAnothersImported)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = MassFromAnothersImportedR;
                //child = MassFromAnothersImportedR.innertCol;
                //child[0].parent = insert;
                //insert.innertCol = child;
                //MassFromAnothersImportedR.innertCol = new List<DataGridColumns> { insert };
                MassFromAnothersImportedR.SetSizeColToAllLevels(160);
                MassFromAnothersImportedR.Binding = nameof(Form24.MassFromAnothersImported);
                NumberInOrderR += MassFromAnothersImportedR;
                #endregion
                #region QuantityFromAnothersImported (9)
                //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
                DataGridColumns QuantityFromAnothersImportedR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.QuantityFromAnothersImported)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = QuantityFromAnothersImportedR;
                //child = QuantityFromAnothersImportedR.innertCol;
                //child[0].parent = insert;
                //insert.innertCol = child;
                //QuantityFromAnothersImportedR.innertCol = new List<DataGridColumns> { insert };
                QuantityFromAnothersImportedR.SetSizeColToAllLevels(190);
                QuantityFromAnothersImportedR.Binding = nameof(Form24.QuantityFromAnothersImported);
                NumberInOrderR += QuantityFromAnothersImportedR;
                #endregion
                #region MassAnotherReasons (10)
                //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
                DataGridColumns MassAnotherReasonsR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.MassAnotherReasons)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = MassAnotherReasonsR;
                //child = MassAnotherReasonsR.innertCol;
                //child[0].parent = insert;
                //insert.innertCol = child;
                //MassAnotherReasonsR.innertCol = new List<DataGridColumns> { insert };
                MassAnotherReasonsR.SetSizeColToAllLevels(160);
                MassAnotherReasonsR.Binding = nameof(Form24.MassAnotherReasons);
                NumberInOrderR += MassAnotherReasonsR;
                #endregion
                #region QuantityAnotherReasons (11)
                //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
                DataGridColumns QuantityAnotherReasonsR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.QuantityAnotherReasons)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = QuantityAnotherReasonsR;
                //child = QuantityAnotherReasonsR.innertCol;
                //child[0].parent = insert;
                //insert.innertCol = child;
                //QuantityAnotherReasonsR.innertCol = new List<DataGridColumns> { insert };
                QuantityAnotherReasonsR.SetSizeColToAllLevels(160);
                QuantityAnotherReasonsR.Binding = nameof(Form24.QuantityAnotherReasons);
                NumberInOrderR += QuantityAnotherReasonsR;
                #endregion
                #region MassTransferredToAnother (12)
                //insert = new DataGridColumns(); insert.name = "Снято с учета в организации";
                DataGridColumns MassTransferredToAnotherR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.MassTransferredToAnother)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = MassTransferredToAnotherR;
                //child = MassTransferredToAnotherR.innertCol;
                //child[0].parent = insert;
                //insert.innertCol = child;
                //MassTransferredToAnotherR.innertCol = new List<DataGridColumns> { insert };
                MassTransferredToAnotherR.SetSizeColToAllLevels(160);
                MassTransferredToAnotherR.Binding = nameof(Form24.MassTransferredToAnother);
                NumberInOrderR += MassTransferredToAnotherR;
                #endregion
                #region QuantityTransferredToAnother (13)
                //insert = new DataGridColumns(); insert.name = "Снято с учета в организации";
                DataGridColumns QuantityTransferredToAnotherR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.QuantityTransferredToAnother)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = QuantityTransferredToAnotherR;
                //child = QuantityTransferredToAnotherR.innertCol;
                //child[0].parent = insert;
                //insert.innertCol = child;
                //QuantityTransferredToAnotherR.innertCol = new List<DataGridColumns> { insert };
                QuantityTransferredToAnotherR.SetSizeColToAllLevels(160);
                QuantityTransferredToAnotherR.Binding = nameof(Form24.QuantityTransferredToAnother);
                NumberInOrderR += QuantityTransferredToAnotherR;
                #endregion
                #region MassRefined (14)
                //insert = new DataGridColumns(); insert.name = "Снято с учета в организации";
                DataGridColumns MassRefinedR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.MassRefined)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = MassRefinedR;
                //child = MassRefinedR.innertCol;
                //child[0].parent = insert;
                //insert.innertCol = child;
                //MassRefinedR.innertCol = new List<DataGridColumns> { insert };
                MassRefinedR.SetSizeColToAllLevels(160);
                MassRefinedR.Binding = nameof(Form24.MassRefined);
                NumberInOrderR += MassRefinedR;
                #endregion
                #region QuantityRefined (15)
                //insert = new DataGridColumns(); insert.name = "Снято с учета в организации";
                DataGridColumns QuantityRefinedR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.QuantityRefined)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = QuantityRefinedR;
                //child = QuantityRefinedR.innertCol;
                //child[0].parent = insert;
                //insert.innertCol = child;
                //QuantityRefinedR.innertCol = new List<DataGridColumns> { insert };
                QuantityRefinedR.SetSizeColToAllLevels(160);
                QuantityRefinedR.Binding = nameof(Form24.QuantityRefined);
                NumberInOrderR += QuantityRefinedR;
                #endregion
                #region MassRemovedFromAccount (16)
                //insert = new DataGridColumns(); insert.name = "Снято с учета в организации";
                DataGridColumns MassRemovedFromAccountR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.MassRemovedFromAccount)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = MassRemovedFromAccountR;
                //child = MassRemovedFromAccountR.innertCol;
                //child[0].parent = insert;
                //insert.innertCol = child;
                //MassRemovedFromAccountR.innertCol = new List<DataGridColumns> { insert };
                MassRemovedFromAccountR.SetSizeColToAllLevels(160);
                MassRemovedFromAccountR.Binding = nameof(Form24.MassRemovedFromAccount);
                NumberInOrderR += MassRemovedFromAccountR;
                #endregion
                #region QuantityRemovedFromAccount (17)
                //insert = new DataGridColumns(); insert.name = "Снято с учета в организации";
                DataGridColumns QuantityRemovedFromAccountR = ((Attributes.Form_PropertyAttribute)typeof(Form24).GetProperty(nameof(Form24.QuantityRemovedFromAccount)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                //insert.parent = QuantityRemovedFromAccountR;
                //child = QuantityRemovedFromAccountR.innertCol;
                //child[0].parent = insert;
                //insert.innertCol = child;
                //QuantityRemovedFromAccountR.innertCol = new List<DataGridColumns> { insert };
                QuantityRemovedFromAccountR.SetSizeColToAllLevels(160);
                QuantityRemovedFromAccountR.Binding = nameof(Form24.QuantityRemovedFromAccount);
                NumberInOrderR += QuantityRemovedFromAccountR;
                #endregion
                _DataGridColumns = NumberInOrderR;
                //_DataGridColumns = MassCreatedR;
            }
            return _DataGridColumns;
        }
        #endregion
    }
}