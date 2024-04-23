using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;

namespace Models.Forms.Form2;

[Serializable]
[Form_Class("Форма 2.4: Постановка на учет и снятие с учета РВ, содержащихся в отработавшем ядерном топливе")]
[Table("form_24")]
public partial class Form24 : Form2
{
    #region Constructor

    public Form24()
    {
        FormNum.Value = "2.4";
        //NumberOfFields.Value = 26;
        Validate_all();
    }

    #endregion

    #region Validation
    
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

    [FormProperty(true, "Форма")]
    public override bool Object_Validation()
    {
        return !(CodeOYAT.HasErrors ||
                 FcpNumber.HasErrors ||
                 QuantityFromAnothers.HasErrors ||
                 QuantityFromAnothersImported.HasErrors ||
                 QuantityCreated.HasErrors ||
                 QuantityRemovedFromAccount.HasErrors ||
                 MassCreated.HasErrors ||
                 MassFromAnothers.HasErrors ||
                 MassFromAnothersImported.HasErrors ||
                 MassRemovedFromAccount.HasErrors ||
                 QuantityTransferredToAnother.HasErrors ||
                 MassAnotherReasons.HasErrors ||
                 MassTransferredToAnother.HasErrors ||
                 QuantityAnotherReasons.HasErrors ||
                 QuantityRefined.HasErrors ||
                 MassRefined.HasErrors);
    }

    #endregion

    #region Properties
    
    #region  CodeOYAT (2)

    public string CodeOYAT_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-2", "Код ОЯТ", "2")]
    public RamAccess<string> CodeOYAT
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(CodeOYAT), out var value))
            {
                ((RamAccess<string>)value).Value = CodeOYAT_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(CodeOYAT_Validation, CodeOYAT_DB);
            rm.PropertyChanged += CodeOYATValueChanged;
            Dictionary.Add(nameof(CodeOYAT), rm);
            return (RamAccess<string>)Dictionary[nameof(CodeOYAT)];
        }
        set
        {
            CodeOYAT_DB = value.Value;
            OnPropertyChanged(nameof(CodeOYAT));
        }
    }

    private void CodeOYATValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            CodeOYAT_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool CodeOYAT_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено"); return false;
        }
        if (!FiveNumRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }

    #endregion

    #region  FcpNumber (3)

    public string FcpNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-3", "Номер мероприятия ФЦП", "3")]
    public RamAccess<string> FcpNumber
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(FcpNumber), out var value))
            {
                ((RamAccess<string>)value).Value = FcpNumber_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(FcpNumber_Validation, FcpNumber_DB);
            rm.PropertyChanged += FcpNumberValueChanged;
            Dictionary.Add(nameof(FcpNumber), rm);
            return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
        }
        set
        {
            FcpNumber_DB = value.Value;
            OnPropertyChanged(nameof(FcpNumber));
        }
    }

    private void FcpNumberValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            FcpNumber_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool FcpNumber_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region  MassCreated (4)

    public string MassCreated_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поставлено на учет в организации", "масса образованного, т", "4")]
    public RamAccess<string> MassCreated
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MassCreated), out var value))
            {
                ((RamAccess<string>)value).Value = MassCreated_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(MassCreated_Validation, MassCreated_DB);
            rm.PropertyChanged += MassCreatedValueChanged;
            Dictionary.Add(nameof(MassCreated), rm);
            return (RamAccess<string>)Dictionary[nameof(MassCreated)];
        }
        set
        {
            MassCreated_DB = value.Value;
            OnPropertyChanged(nameof(MassCreated));
        }
    }

    private void MassCreatedValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                MassCreated_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        MassCreated_DB = value1;
    }

    private bool MassCreated_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        if (value1[0] == '(' && value1[^1] == ')')
        {
            value1 = value1.Remove(value1.Length - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(value1, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (doubleValue <= 0)
        {
            value.AddError("Число должно быть больше нуля"); 
            return false;
        }
        return true;
    }

    #endregion

    #region  QuantityCreated (5)

    public string QuantityCreated_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поставлено на учет в организации", "количество образованного, шт", "5")]
    public RamAccess<string> QuantityCreated
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(QuantityCreated), out var value))
            {
                ((RamAccess<string>)value).Value = QuantityCreated_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(QuantityCreated_Validation, QuantityCreated_DB);
            rm.PropertyChanged += QuantityCreatedValueChanged;
            Dictionary.Add(nameof(QuantityCreated), rm);
            return (RamAccess<string>)Dictionary[nameof(QuantityCreated)];
        }
        set
        {
            QuantityCreated_DB = value.Value;
            OnPropertyChanged(nameof(QuantityCreated));
        }
    }
    // positive int.

    private void QuantityCreatedValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            QuantityCreated_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool QuantityCreated_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }

        if (!int.TryParse(value.Value, out var intValue) || intValue <= 0)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  MassFromAnothers (6)

    public string MassFromAnothers_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поставлено на учет в организации", "масса поступивших от сторонних, т", "6")]
    public RamAccess<string> MassFromAnothers
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MassFromAnothers), out var value))
            {
                ((RamAccess<string>)value).Value = MassFromAnothers_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(MassFromAnothers_Validation, MassFromAnothers_DB);
            rm.PropertyChanged += MassFromAnothersValueChanged;
            Dictionary.Add(nameof(MassFromAnothers), rm);
            return (RamAccess<string>)Dictionary[nameof(MassFromAnothers)];
        }
        set
        {
            MassFromAnothers_DB = value.Value;
            OnPropertyChanged(nameof(MassFromAnothers));
        }
    }

    private void MassFromAnothersValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                MassFromAnothers_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        MassFromAnothers_DB = value1;
    }

    private bool MassFromAnothers_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var len = value1.Length;
        if (value1[0] == '(' && value1[^1] == ')')
        {
            value1 = value1.Remove(value1.Length - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(value1, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (doubleValue <= 0)
        {
            value.AddError("Число должно быть больше нуля"); 
            return false;
        }
        return true;
    }

    #endregion

    #region  QuantityFromAnothers (7)

    public string QuantityFromAnothers_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поставлено на учет в организации", "количество поступивших от сторонних, шт", "7")]
    public RamAccess<string> QuantityFromAnothers
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(QuantityFromAnothers), out var value))
            {
                ((RamAccess<string>)value).Value = QuantityFromAnothers_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(QuantityFromAnothers_Validation, QuantityFromAnothers_DB);
            rm.PropertyChanged += QuantityFromAnothersValueChanged;
            Dictionary.Add(nameof(QuantityFromAnothers), rm);
            return (RamAccess<string>)Dictionary[nameof(QuantityFromAnothers)];
        }
        set
        {
            QuantityFromAnothers_DB = value.Value;
            OnPropertyChanged(nameof(QuantityFromAnothers));
        }
    }
    // positive int.

    private void QuantityFromAnothersValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            QuantityFromAnothers_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool QuantityFromAnothers_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }
        if (!int.TryParse(value.Value, out var intValue) || intValue <= 0)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  MassFromAnothersImported (8)

    public string MassFromAnothersImported_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поставлено на учет в организации", "масса импортированных от сторонних, т", "8")]
    public RamAccess<string> MassFromAnothersImported
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MassFromAnothersImported), out var value))
            {
                ((RamAccess<string>)value).Value = MassFromAnothersImported_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(MassFromAnothersImported_Validation, MassFromAnothersImported_DB);
            rm.PropertyChanged += MassFromAnothersImportedValueChanged;
            Dictionary.Add(nameof(MassFromAnothersImported), rm);
            return (RamAccess<string>)Dictionary[nameof(MassFromAnothersImported)];
        }
        set
        {
            MassFromAnothersImported_DB = value.Value;
            OnPropertyChanged(nameof(MassFromAnothersImported));
        }
    }

    private void MassFromAnothersImportedValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                MassFromAnothersImported_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        MassFromAnothersImported_DB = value1;
    }

    private bool MassFromAnothersImported_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        if (value1[0] == '(' && value1[^1] == ')')
        {
            value1 = value1.Remove(value1.Length - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(value1, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (doubleValue <= 0)
        {
            value.AddError("Число должно быть больше нуля"); 
            return false;
        }
        return true;
    }

    #endregion

    #region  QuantityFromAnothersImported (9)

    public string QuantityFromAnothersImported_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поставлено на учет в организации", "количество импортированных от сторонних, шт", "9")]
    public RamAccess<string> QuantityFromAnothersImported
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(QuantityFromAnothersImported), out var value))
            {
                ((RamAccess<string>)value).Value = QuantityFromAnothersImported_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(QuantityFromAnothersImported_Validation, QuantityFromAnothersImported_DB);
            rm.PropertyChanged += QuantityFromAnothersImportedValueChanged;
            Dictionary.Add(nameof(QuantityFromAnothersImported), rm);
            return (RamAccess<string>)Dictionary[nameof(QuantityFromAnothersImported)];
        }
        set
        {
            QuantityFromAnothersImported_DB = value.Value;
            OnPropertyChanged(nameof(QuantityFromAnothersImported));
        }
    }
    // positive int.

    private void QuantityFromAnothersImportedValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            QuantityFromAnothersImported_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool QuantityFromAnothersImported_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }
        if (!int.TryParse(value.Value, out var intValue) || intValue <= 0)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  MassAnotherReasons (10)

    public string MassAnotherReasons_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поставлено на учет в организации", "масса учтенных по другим причинам, т", "10")]
    public RamAccess<string> MassAnotherReasons
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MassAnotherReasons), out var value))
            {
                ((RamAccess<string>)value).Value = MassAnotherReasons_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(MassAnotherReasons_Validation, MassAnotherReasons_DB);
            rm.PropertyChanged += MassAnotherReasonsValueChanged;
            Dictionary.Add(nameof(MassAnotherReasons), rm);
            return (RamAccess<string>)Dictionary[nameof(MassAnotherReasons)];
        }
        set
        {
            MassAnotherReasons_DB = value.Value;
            OnPropertyChanged(nameof(MassAnotherReasons));
        }
    }

    private void MassAnotherReasonsValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                MassAnotherReasons_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        MassAnotherReasons_DB = value1;
    }

    private bool MassAnotherReasons_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        if (value1[0] == '(' && value1[^1] == ')')
        {
            value1 = value1.Remove(value1.Length - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(value1, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (doubleValue <= 0)
        {
            value.AddError("Число должно быть больше нуля"); 
            return false;
        }
        return true;
    }

    #endregion

    #region  QuantityAnotherReasons (11)

    public string QuantityAnotherReasons_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поставлено на учет в организации", "количество учтенных по другим причинам, шт", "11")]
    public RamAccess<string> QuantityAnotherReasons
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(QuantityAnotherReasons), out var value))
            {
                ((RamAccess<string>)value).Value = QuantityAnotherReasons_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(QuantityAnotherReasons_Validation, QuantityAnotherReasons_DB);
            rm.PropertyChanged += QuantityAnotherReasonsValueChanged;
            Dictionary.Add(nameof(QuantityAnotherReasons), rm);
            return (RamAccess<string>)Dictionary[nameof(QuantityAnotherReasons)];
        }
        set
        {
            QuantityAnotherReasons_DB = value.Value;
            OnPropertyChanged(nameof(QuantityAnotherReasons));
        }
    }
    // positive int.

    private void QuantityAnotherReasonsValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            QuantityAnotherReasons_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool QuantityAnotherReasons_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }
        if (!int.TryParse(value.Value, out var intValue) || intValue <= 0)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  MassTransferredToAnother (12)

    public string MassTransferredToAnother_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Снято с учета в организации", "масса переданных сторонним, т", "12")]
    public RamAccess<string> MassTransferredToAnother
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MassTransferredToAnother), out var value))
            {
                ((RamAccess<string>)value).Value = MassTransferredToAnother_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(MassTransferredToAnother_Validation, MassTransferredToAnother_DB);
            rm.PropertyChanged += MassTransferredToAnotherValueChanged;
            Dictionary.Add(nameof(MassTransferredToAnother), rm);
            return (RamAccess<string>)Dictionary[nameof(MassTransferredToAnother)];
        }
        set
        {
            MassTransferredToAnother_DB = value.Value;
            OnPropertyChanged(nameof(MassTransferredToAnother));
        }
    }

    private void MassTransferredToAnotherValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                MassTransferredToAnother_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        MassTransferredToAnother_DB = value1;
    }

    private bool MassTransferredToAnother_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        if (value1[0] == '(' && value1[^1] == ')')
        {
            value1 = value1.Remove(value1.Length - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(value1, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (doubleValue <= 0)
        {
            value.AddError("Число должно быть больше нуля"); 
            return false;
        }
        return true;
    }

    #endregion

    #region  QuantityTransferredToAnother (13)

    public string QuantityTransferredToAnother_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Снято с учета в организации", "количество переданных сторонним, шт", "13")]
    public RamAccess<string> QuantityTransferredToAnother
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(QuantityTransferredToAnother), out var value))
            {
                ((RamAccess<string>)value).Value = QuantityTransferredToAnother_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(QuantityTransferredToAnother_Validation, QuantityTransferredToAnother_DB);
            rm.PropertyChanged += QuantityTransferredToAnotherValueChanged;
            Dictionary.Add(nameof(QuantityTransferredToAnother), rm);
            return (RamAccess<string>)Dictionary[nameof(QuantityTransferredToAnother)];
        }
        set
        {
            QuantityTransferredToAnother_DB = value.Value;
            OnPropertyChanged(nameof(QuantityTransferredToAnother));
        }
    }
    // positive int.

    private void QuantityTransferredToAnotherValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            QuantityTransferredToAnother_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool QuantityTransferredToAnother_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }

        if (!int.TryParse(value.Value, out var intValue) || intValue <= 0)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  MassRefined (14)

    public string MassRefined_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Снято с учета в организации", "масса переработанных, т", "14")]
    public RamAccess<string> MassRefined
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MassRefined), out var value))
            {
                ((RamAccess<string>)value).Value = MassRefined_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(MassRefined_Validation, MassRefined_DB);
            rm.PropertyChanged += MassRefinedValueChanged;
            Dictionary.Add(nameof(MassRefined), rm);
            return (RamAccess<string>)Dictionary[nameof(MassRefined)];
        }
        set
        {
            MassRefined_DB = value.Value;
            OnPropertyChanged(nameof(MassRefined));
        }
    }

    private void MassRefinedValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                MassRefined_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        MassRefined_DB = value1;
    }

    private bool MassRefined_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        if (value1[0] == '(' && value1[^1] == ')')
        {
            value1 = value1.Remove(value1.Length - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(value1, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (doubleValue <= 0)
        {
            value.AddError("Число должно быть больше нуля"); 
            return false;
        }
        return true;
    }

    #endregion

    #region  QuantityRefined (15)

    public string QuantityRefined_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Снято с учета в организации", "количество переработанных, шт", "15")]
    public RamAccess<string> QuantityRefined
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(QuantityRefined), out var value))
            {
                ((RamAccess<string>)value).Value = QuantityRefined_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(QuantityRefined_Validation, QuantityRefined_DB);
            rm.PropertyChanged += QuantityRefinedValueChanged;
            Dictionary.Add(nameof(QuantityRefined), rm);
            return (RamAccess<string>)Dictionary[nameof(QuantityRefined)];
        }
        set
        {
            QuantityRefined_DB = value.Value;
            OnPropertyChanged(nameof(QuantityRefined));
        }
    }
    // positive int.

    private void QuantityRefinedValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            QuantityRefined_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool QuantityRefined_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }
        if (!int.TryParse(value.Value, out var intValue) || intValue <= 0)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  MassRemovedFromAccount (16)

    public string MassRemovedFromAccount_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Снято с учета в организации", "масса снятых с учета, т", "16")]
    public RamAccess<string> MassRemovedFromAccount
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MassRemovedFromAccount), out var value))
            {
                ((RamAccess<string>)value).Value = MassRemovedFromAccount_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(MassRemovedFromAccount_Validation, MassRemovedFromAccount_DB);
            rm.PropertyChanged += MassRemovedFromAccountValueChanged;
            Dictionary.Add(nameof(MassRemovedFromAccount), rm);
            return (RamAccess<string>)Dictionary[nameof(MassRemovedFromAccount)];
        }
        set
        {
            MassRemovedFromAccount_DB = value.Value;
            OnPropertyChanged(nameof(MassRemovedFromAccount));
        }
    }

    private void MassRemovedFromAccountValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                MassRemovedFromAccount_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        MassRemovedFromAccount_DB = value1;
    }

    private bool MassRemovedFromAccount_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        if (value1[0] == '(' && value1[^1] == ')')
        {
            value1 = value1.Remove(value1.Length - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(value1, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (doubleValue <= 0)
        {
            value.AddError("Число должно быть больше нуля"); 
            return false;
        }
        return true;
    }

    #endregion

    #region  QuantityRemovedFromAccount (17)

    public string QuantityRemovedFromAccount_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Снято с учета в организации", "количество снятых с учета, шт", "17")]
    public RamAccess<string> QuantityRemovedFromAccount
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(QuantityRemovedFromAccount), out var value))
            {
                ((RamAccess<string>)value).Value = QuantityRemovedFromAccount_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(QuantityRemovedFromAccount_Validation, QuantityRemovedFromAccount_DB);
            rm.PropertyChanged += QuantityRemovedFromAccountValueChanged;
            Dictionary.Add(nameof(QuantityRemovedFromAccount), rm);
            return (RamAccess<string>)Dictionary[nameof(QuantityRemovedFromAccount)];
        }
        set
        {
            QuantityRemovedFromAccount_DB = value.Value;
            OnPropertyChanged(nameof(QuantityRemovedFromAccount));
        }
    }
    // positive int.

    private void QuantityRemovedFromAccountValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            QuantityRemovedFromAccount_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool QuantityRemovedFromAccount_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }
        if (!int.TryParse(value.Value, out var intValue) || intValue <= 0)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion 
    
    #endregion

    #region IExcel

    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        base.ExcelGetRow(worksheet, row);
        CodeOYAT_DB = Convert.ToString(worksheet.Cells[row, 2].Value);
        FcpNumber_DB = Convert.ToString(worksheet.Cells[row, 3].Value);
        MassCreated_DB = ConvertFromExcelDouble(worksheet.Cells[row, 4].Value);
        QuantityCreated_DB = ConvertFromExcelInt(worksheet.Cells[row, 5].Value);
        MassFromAnothers_DB = ConvertFromExcelDouble(worksheet.Cells[row, 6].Value);
        QuantityFromAnothers_DB = ConvertFromExcelInt(worksheet.Cells[row, 7].Value);
        MassFromAnothersImported_DB = ConvertFromExcelDouble(worksheet.Cells[row, 8].Value);
        QuantityFromAnothersImported_DB = ConvertFromExcelInt(worksheet.Cells[row, 9].Value);
        MassAnotherReasons_DB = ConvertFromExcelDouble(worksheet.Cells[row, 10].Value);
        QuantityAnotherReasons_DB = ConvertFromExcelInt(worksheet.Cells[row, 11].Value);
        MassTransferredToAnother_DB = ConvertFromExcelDouble(worksheet.Cells[row, 12].Value);
        QuantityTransferredToAnother_DB = ConvertFromExcelInt(worksheet.Cells[row, 13].Value);
        MassRefined_DB = ConvertFromExcelDouble(worksheet.Cells[row, 14].Value);
        QuantityRefined_DB = ConvertFromExcelInt(worksheet.Cells[row, 15].Value);
        MassRemovedFromAccount_DB = ConvertFromExcelDouble(worksheet.Cells[row, 16].Value);
        QuantityRemovedFromAccount_DB = ConvertFromExcelInt(worksheet.Cells[row, 17].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ConvertToExcelString(CodeOYAT_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(FcpNumber_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelDouble(MassCreated_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelInt(QuantityCreated_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelDouble(MassFromAnothers_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelInt(QuantityFromAnothers_DB);
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDouble(MassFromAnothersImported_DB);
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelInt(QuantityFromAnothersImported_DB);
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ConvertToExcelDouble(MassAnotherReasons_DB);
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ConvertToExcelInt(QuantityAnotherReasons_DB);
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ConvertToExcelDouble(MassTransferredToAnother_DB);
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ConvertToExcelInt(QuantityTransferredToAnother_DB);
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ConvertToExcelDouble(MassRefined_DB);
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ConvertToExcelInt(QuantityRefined_DB);
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ConvertToExcelDouble(MassRemovedFromAccount_DB);
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ConvertToExcelInt(QuantityRemovedFromAccount_DB);
        
        return 16;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form2.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(CodeOYAT))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(FcpNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(MassCreated))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(QuantityCreated))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(MassFromAnothers))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(QuantityFromAnothers))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(MassFromAnothersImported))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(QuantityFromAnothersImported))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(MassAnotherReasons))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(QuantityAnotherReasons))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(MassTransferredToAnother))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(QuantityTransferredToAnother))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(MassRefined))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(QuantityRefined))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(MassRemovedFromAccount))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form24,Models")?.GetProperty(nameof(QuantityRemovedFromAccount))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        
        return 16;
    }
    
    #endregion

    #region IDataGridColumn

    private static DataGridColumns _DataGridColumns { get; set; }

    public override DataGridColumns GetColumnStructure(string param = "")
    {
        if (_DataGridColumns != null) return _DataGridColumns;

        #region NumberInOrder (1)

        //DataGridColumns insert = new DataGridColumns(); insert.name = ".kj";
        //DataGridColumns insert2 = new DataGridColumns(); insert2.name = "po["; insert2.parent = insert;
        //insert.innertCol = new List<DataGridColumns> { insert2 };
        var numberInOrderR = ((FormPropertyAttribute)typeof(Form)
                .GetProperty(nameof(NumberInOrder))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD();
        //insert.parent = NumberInOrderR;
        //List<DataGridColumns> child = NumberInOrderR.innertCol;
        //child[0].parent = insert.innertCol[0];
        //insert.innertCol[0].innertCol = child;
        //NumberInOrderR.innertCol = new List<DataGridColumns> { insert };
        if (numberInOrderR != null)
        {
            numberInOrderR.SetSizeColToAllLevels(50);
                
            numberInOrderR.Binding = nameof(NumberInOrder);
            numberInOrderR.Blocked = true;
            numberInOrderR.ChooseLine = true;
        }

        #endregion

        #region CodeOYAT (2)

        //insert = new DataGridColumns(); insert.name = "gfh";
        //insert2 = new DataGridColumns(); insert2.name = "yiu"; insert2.parent = insert;
        //insert.innertCol = new List<DataGridColumns> { insert2 };
        var codeOyatR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(CodeOYAT))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        //insert.parent = CodeOYATR;
        //child = CodeOYATR.innertCol;
        //child[0].parent = insert.innertCol[0];
        //insert.innertCol[0].innertCol = child;
        //CodeOYATR.innertCol = new List<DataGridColumns> { insert };
        if (codeOyatR != null)
        {
            codeOyatR.SetSizeColToAllLevels(88);
            codeOyatR.Binding = nameof(CodeOYAT);
            numberInOrderR += codeOyatR;
        }

        #endregion

        #region FcpNumber (3)

        //insert = new DataGridColumns(); insert.name = "nmm";
        //insert2 = new DataGridColumns(); insert2.name = "dsgf"; insert2.parent = insert;
        //insert.innertCol = new List<DataGridColumns> { insert2 };
        var fcpNumberR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(FcpNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        //insert.parent = FcpNumberR;
        //child = FcpNumberR.innertCol;
        //child[0].parent = insert.innertCol[0];
        //insert.innertCol[0].innertCol = child;
        //FcpNumberR.innertCol = new List<DataGridColumns> { insert };
        if (fcpNumberR != null)
        {
            fcpNumberR.SetSizeColToAllLevels(140);
            fcpNumberR.Binding = nameof(FcpNumber);
            numberInOrderR += fcpNumberR;
        }

        #endregion

        #region MassCreated (4)

        //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
        var massCreatedR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(MassCreated))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        //insert.parent = MassCreatedR;
        //List<DataGridColumns> child = MassCreatedR.innertCol;
        //child[0].parent = insert;
        //insert.innertCol = child;
        //MassCreatedR.innertCol = new List<DataGridColumns> { insert };
        if (massCreatedR != null)
        {
            massCreatedR.SetSizeColToAllLevels(160);
            massCreatedR.Binding = nameof(MassCreated);
            numberInOrderR += massCreatedR;
        }

        #endregion

        #region QuantityCreated (5)

        //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
        var quantityCreatedR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(QuantityCreated))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        //insert.parent = QuantityCreatedR;
        //child = QuantityCreatedR.innertCol;
        //child[0].parent = insert;
        //insert.innertCol = child;
        //QuantityCreatedR.innertCol = new List<DataGridColumns> { insert };
        if (quantityCreatedR != null)
        {
            quantityCreatedR.SetSizeColToAllLevels(160);
            quantityCreatedR.Binding = nameof(QuantityCreated);
            numberInOrderR += quantityCreatedR;
        }

        #endregion

        #region MassFromAnothers (6)

        //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
        var massFromAnothersR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(MassFromAnothers))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        //insert.parent = MassFromAnothersR;
        //child = MassFromAnothersR.innertCol;
        //child[0].parent = insert;
        //insert.innertCol = child;
        //MassFromAnothersR.innertCol = new List<DataGridColumns> { insert };
        if (massFromAnothersR != null)
        {
            massFromAnothersR.SetSizeColToAllLevels(160);
            massFromAnothersR.Binding = nameof(MassFromAnothers);
            numberInOrderR += massFromAnothersR;
        }

        #endregion

        #region QuantityFromAnothers (7)

        //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
        var quantityFromAnothersR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(QuantityFromAnothers))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        //insert.parent = QuantityFromAnothersR;
        //child = QuantityFromAnothersR.innertCol;
        //child[0].parent = insert;
        //insert.innertCol = child;
        //QuantityFromAnothersR.innertCol = new List<DataGridColumns> { insert };
        if (quantityFromAnothersR != null)
        {
            quantityFromAnothersR.SetSizeColToAllLevels(160);
            quantityFromAnothersR.Binding = nameof(QuantityFromAnothers);
            numberInOrderR += quantityFromAnothersR;
        }

        #endregion

        #region MassFromAnothersImported (8)

        //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
        var massFromAnothersImportedR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(MassFromAnothersImported))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        //insert.parent = MassFromAnothersImportedR;
        //child = MassFromAnothersImportedR.innertCol;
        //child[0].parent = insert;
        //insert.innertCol = child;
        //MassFromAnothersImportedR.innertCol = new List<DataGridColumns> { insert };
        massFromAnothersImportedR.SetSizeColToAllLevels(160);
        massFromAnothersImportedR.Binding = nameof(MassFromAnothersImported);
        numberInOrderR += massFromAnothersImportedR;

        #endregion

        #region QuantityFromAnothersImported (9)

        //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
        var quantityFromAnothersImportedR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(QuantityFromAnothersImported))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        //insert.parent = QuantityFromAnothersImportedR;
        //child = QuantityFromAnothersImportedR.innertCol;
        //child[0].parent = insert;
        //insert.innertCol = child;
        //QuantityFromAnothersImportedR.innertCol = new List<DataGridColumns> { insert };
        quantityFromAnothersImportedR.SetSizeColToAllLevels(190);
        quantityFromAnothersImportedR.Binding = nameof(QuantityFromAnothersImported);
        numberInOrderR += quantityFromAnothersImportedR;

        #endregion

        #region MassAnotherReasons (10)

        //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
        var massAnotherReasonsR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(MassAnotherReasons))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        //insert.parent = MassAnotherReasonsR;
        //child = MassAnotherReasonsR.innertCol;
        //child[0].parent = insert;
        //insert.innertCol = child;
        //MassAnotherReasonsR.innertCol = new List<DataGridColumns> { insert };
        massAnotherReasonsR.SetSizeColToAllLevels(160);
        massAnotherReasonsR.Binding = nameof(MassAnotherReasons);
        numberInOrderR += massAnotherReasonsR;

        #endregion

        #region QuantityAnotherReasons (11)

        //insert = new DataGridColumns(); insert.name = "Поставлено на учет в организации";
        var quantityAnotherReasonsR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(QuantityAnotherReasons))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        //insert.parent = QuantityAnotherReasonsR;
        //child = QuantityAnotherReasonsR.innertCol;
        //child[0].parent = insert;
        //insert.innertCol = child;
        //QuantityAnotherReasonsR.innertCol = new List<DataGridColumns> { insert };
        quantityAnotherReasonsR.SetSizeColToAllLevels(160);
        quantityAnotherReasonsR.Binding = nameof(QuantityAnotherReasons);
        numberInOrderR += quantityAnotherReasonsR;

        #endregion

        #region MassTransferredToAnother (12)

        //insert = new DataGridColumns(); insert.name = "Снято с учета в организации";
        var massTransferredToAnotherR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(MassTransferredToAnother))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        //insert.parent = MassTransferredToAnotherR;
        //child = MassTransferredToAnotherR.innertCol;
        //child[0].parent = insert;
        //insert.innertCol = child;
        //MassTransferredToAnotherR.innertCol = new List<DataGridColumns> { insert };
        massTransferredToAnotherR.SetSizeColToAllLevels(160);
        massTransferredToAnotherR.Binding = nameof(MassTransferredToAnother);
        numberInOrderR += massTransferredToAnotherR;

        #endregion

        #region QuantityTransferredToAnother (13)

        //insert = new DataGridColumns(); insert.name = "Снято с учета в организации";
        var quantityTransferredToAnotherR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(QuantityTransferredToAnother))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        //insert.parent = QuantityTransferredToAnotherR;
        //child = QuantityTransferredToAnotherR.innertCol;
        //child[0].parent = insert;
        //insert.innertCol = child;
        //QuantityTransferredToAnotherR.innertCol = new List<DataGridColumns> { insert };
        quantityTransferredToAnotherR.SetSizeColToAllLevels(160);
        quantityTransferredToAnotherR.Binding = nameof(QuantityTransferredToAnother);
        numberInOrderR += quantityTransferredToAnotherR;

        #endregion

        #region MassRefined (14)

        //insert = new DataGridColumns(); insert.name = "Снято с учета в организации";
        var massRefinedR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(MassRefined))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        //insert.parent = MassRefinedR;
        //child = MassRefinedR.innertCol;
        //child[0].parent = insert;
        //insert.innertCol = child;
        //MassRefinedR.innertCol = new List<DataGridColumns> { insert };
        massRefinedR.SetSizeColToAllLevels(160);
        massRefinedR.Binding = nameof(MassRefined);
        numberInOrderR += massRefinedR;

        #endregion

        #region QuantityRefined (15)

        //insert = new DataGridColumns(); insert.name = "Снято с учета в организации";
        var quantityRefinedR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(QuantityRefined))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        //insert.parent = QuantityRefinedR;
        //child = QuantityRefinedR.innertCol;
        //child[0].parent = insert;
        //insert.innertCol = child;
        //QuantityRefinedR.innertCol = new List<DataGridColumns> { insert };
        quantityRefinedR.SetSizeColToAllLevels(160);
        quantityRefinedR.Binding = nameof(QuantityRefined);
        numberInOrderR += quantityRefinedR;

        #endregion

        #region MassRemovedFromAccount (16)

        //insert = new DataGridColumns(); insert.name = "Снято с учета в организации";
        var massRemovedFromAccountR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(MassRemovedFromAccount))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        //insert.parent = MassRemovedFromAccountR;
        //child = MassRemovedFromAccountR.innertCol;
        //child[0].parent = insert;
        //insert.innertCol = child;
        //MassRemovedFromAccountR.innertCol = new List<DataGridColumns> { insert };
        massRemovedFromAccountR.SetSizeColToAllLevels(160);
        massRemovedFromAccountR.Binding = nameof(MassRemovedFromAccount);
        numberInOrderR += massRemovedFromAccountR;

        #endregion

        #region QuantityRemovedFromAccount (17)

        //insert = new DataGridColumns(); insert.name = "Снято с учета в организации";
        var quantityRemovedFromAccountR = ((FormPropertyAttribute)typeof(Form24)
                .GetProperty(nameof(QuantityRemovedFromAccount))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        //insert.parent = QuantityRemovedFromAccountR;
        //child = QuantityRemovedFromAccountR.innertCol;
        //child[0].parent = insert;
        //insert.innertCol = child;
        //QuantityRemovedFromAccountR.innertCol = new List<DataGridColumns> { insert };
        quantityRemovedFromAccountR.SetSizeColToAllLevels(160);
        quantityRemovedFromAccountR.Binding = nameof(QuantityRemovedFromAccount);
        numberInOrderR += quantityRemovedFromAccountR;

        #endregion

        _DataGridColumns = numberInOrderR;
        //_DataGridColumns = MassCreatedR;
        return _DataGridColumns;
    }

    #endregion

    #region GeneratedRegex
    
    [GeneratedRegex("^[0-9]{5}$")]
    private static partial Regex FiveNumRegex();

    #endregion
}