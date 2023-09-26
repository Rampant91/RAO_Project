using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using Models.Interfaces;
using OfficeOpenXml;

namespace Models.Forms.Form2;

[Serializable]
[Form_Class("Форма 2.1: Сортировка, переработка и кондиционирование РАО на установках")]
public class Form21 : Form2, IBaseColor
{
    #region Constructor
    public Form21()
    {
        FormNum.Value = "2.1";
        //NumberOfFields.Value = 24;
        Validate_all();
    }
    #endregion

    #region Validation
    private void Validate_all()
    {
        MachinePower_Validation(MachinePower);
        MachineCode_Validation(MachineCode);
        RefineMachineName_Validation(RefineMachineName);
        NumberOfHoursPerYear_Validation(NumberOfHoursPerYear);
        CodeRAOIn_Validation(CodeRAOIn);
        StatusRAOIn_Validation(StatusRAOIn);
        VolumeIn_Validation(VolumeIn);
        MassIn_Validation(MassIn);
        QuantityIn_Validation(QuantityIn);
        TritiumActivityIn_Validation(TritiumActivityIn);
        TritiumActivityOut_Validation(TritiumActivityOut);
        BetaGammaActivityIn_Validation(BetaGammaActivityIn);
        BetaGammaActivityOut_Validation(BetaGammaActivityOut);
        TransuraniumActivityIn_Validation(TransuraniumActivityIn);
        TransuraniumActivityOut_Validation(TransuraniumActivityOut);
        AlphaActivityIn_Validation(AlphaActivityIn);
        AlphaActivityOut_Validation(AlphaActivityOut);
        VolumeOut_Validation(VolumeOut);
        MassOut_Validation(MassOut);
        QuantityOZIIIout_Validation(QuantityOZIIIout);
        CodeRAOout_Validation(CodeRAOout);
        StatusRAOout_Validation(StatusRAOout);
    }
    public override bool Object_Validation()
    {
        return !(MachinePower.HasErrors ||
                 MachineCode.HasErrors ||
                 RefineMachineName.HasErrors ||
                 NumberOfHoursPerYear.HasErrors ||
                 CodeRAOIn.HasErrors ||
                 StatusRAOIn.HasErrors ||
                 VolumeIn.HasErrors ||
                 MassIn.HasErrors ||
                 QuantityIn.HasErrors ||
                 TritiumActivityIn.HasErrors ||
                 TritiumActivityOut.HasErrors ||
                 BetaGammaActivityIn.HasErrors ||
                 BetaGammaActivityOut.HasErrors ||
                 TransuraniumActivityIn.HasErrors ||
                 TransuraniumActivityOut.HasErrors ||
                 AlphaActivityIn.HasErrors ||
                 AlphaActivityOut.HasErrors ||
                 VolumeOut.HasErrors ||
                 MassOut.HasErrors ||
                 QuantityOZIIIout.HasErrors ||
                 CodeRAOout.HasErrors ||
                 StatusRAOout.HasErrors);
    } 
    #endregion

    #region BaseColor
    public ColorType _BaseColor { get; set; } = ColorType.None;
    [NotMapped]
    public ColorType BaseColor {

        get => _BaseColor;
        set
        {
            if (_BaseColor == value) return;
            _BaseColor = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region Sum
    public bool Sum_DB { get; set; }

    [NotMapped]
    public RamAccess<bool> Sum
    {
        get
        {
            var tmp = new RamAccess<bool>(Sum_Validation, Sum_DB);
            tmp.PropertyChanged += SumValueChanged;
            return tmp;
        }
        set
        {
            Sum_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void SumValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Sum_DB = ((RamAccess<bool>)value).Value;
        }
    }

    private bool Sum_Validation(RamAccess<bool> value)
    {
        value.ClearErrors();
        return true;
    }
    #endregion

    #region SumGroup
    public bool SumGroup_DB { get; set; }

    [NotMapped]
    public RamAccess<bool> SumGroup
    {
        get
        {
            var tmp = new RamAccess<bool>(SumGroup_Validation, SumGroup_DB);
            tmp.PropertyChanged += SumGroupValueChanged;
            return tmp;
        }
        set
        {
            SumGroup_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void SumGroupValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            SumGroup_DB = ((RamAccess<bool>)value).Value;
        }
    }

    private bool SumGroup_Validation(RamAccess<bool> value)
    {
        value.ClearErrors();
        return true;
    }
    #endregion

    #region Columns

    #region NumberInOrder_1
    public string NumberInOrderSum_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-1-1", "null-1", "№ п/п", "1")]
    public RamAccess<string> NumberInOrderSum
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(NumberInOrderSum)))
            {
                ((RamAccess<string>)Dictionary[nameof(NumberInOrderSum)]).Value = !string.IsNullOrEmpty(NumberInOrderSum_DB)
                    ? NumberInOrderSum_DB
                    : NumberInOrder_DB.ToString();
                return (RamAccess<string>)Dictionary[nameof(NumberInOrderSum)];
            }
            var rm = !string.IsNullOrEmpty(NumberInOrderSum_DB)
                ? new RamAccess<string>(NumberInOrderSum_Validation, NumberInOrderSum_DB)
                : new RamAccess<string>(NumberInOrderSum_Validation, NumberInOrder_DB.ToString());
            rm.PropertyChanged += NumberInOrderSumValueChanged;
            Dictionary.Add(nameof(NumberInOrderSum), rm);
            return (RamAccess<string>)Dictionary[nameof(NumberInOrderSum)];
        }
        set
        {
            NumberInOrderSum_DB = value.Value;
            OnPropertyChanged();
        }
    }
    private void NumberInOrderSumValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            NumberInOrderSum_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool NumberInOrderSum_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        return true;
    }
    #endregion

    #region RefineMachineName_2
    public string RefineMachineName_DB { get; set; } = "";
    public bool _RefineMachineName_Hidden_Get { get; set; } = true;
    [NotMapped]
    public RefBool RefineMachineName_Hidden_Get
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(RefineMachineName_Hidden_Get)))
            {
                ((RefBool)Dictionary[nameof(RefineMachineName_Hidden_Get)]).Set(_RefineMachineName_Hidden_Get);
                return (RefBool)Dictionary[nameof(RefineMachineName_Hidden_Get)];

            }
            var rm = new RefBool(_RefineMachineName_Hidden_Get);
            Dictionary.Add(nameof(RefineMachineName_Hidden_Get), rm);
            return rm;
        }
        set
        {
            if (_RefineMachineName_Hidden_Get == value.Get()) return;
            _RefineMachineName_Hidden_Get = value.Get();
            OnPropertyChanged();
        }
    }
    public bool _RefineMachineName_Hidden_Set { get; set; } = true;
    [NotMapped]
    public RefBool RefineMachineName_Hidden_Set
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(RefineMachineName_Hidden_Set)))
            {
                ((RefBool)Dictionary[nameof(RefineMachineName_Hidden_Set)]).Set(_RefineMachineName_Hidden_Set);
                return (RefBool)Dictionary[nameof(RefineMachineName_Hidden_Set)];

            }
            var rm = new RefBool(_RefineMachineName_Hidden_Set);
            Dictionary.Add(nameof(RefineMachineName_Hidden_Set), rm);
            return rm;
        }
        set
        {
            if (_RefineMachineName_Hidden_Set == value.Get()) return;
            _RefineMachineName_Hidden_Set = value.Get();
            OnPropertyChanged();
        }
    }

    [NotMapped]
    [FormProperty(true, "Установки переработки", "наименование", "2")]
    public RamAccess<string> RefineMachineName
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(RefineMachineName)))
            {
                ((RamAccess<string>)Dictionary[nameof(RefineMachineName)]).Value = RefineMachineName_DB;
                return (RamAccess<string>)Dictionary[nameof(RefineMachineName)];
            }
            var rm = new RamAccess<string>(RefineMachineName_Validation, RefineMachineName_DB, RefineMachineName_Hidden_Get, RefineMachineName_Hidden_Set);
            rm.PropertyChanged += RefineMachineNameValueChanged;
            Dictionary.Add(nameof(RefineMachineName), rm);
            return (RamAccess<string>)Dictionary[nameof(RefineMachineName)];
        }
        set
        {
            if (RefineMachineName.Value == value.Value) return;
            RefineMachineName_DB = value.Value;
            OnPropertyChanged();
        }

    }

    private void RefineMachineNameValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value" && _RefineMachineName_Hidden_Set)
        {
            RefineMachineName_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool RefineMachineName_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }
    #endregion

    #region MachineCode_3
    public byte? MachineCode_DB { get; set; }
    public bool _MachineCode_Hidden_Get { get; set; } = true;
    [NotMapped]
    public RefBool MachineCode_Hidden_Get
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(MachineCode_Hidden_Get)))
            {
                ((RefBool)Dictionary[nameof(MachineCode_Hidden_Get)]).Set(_MachineCode_Hidden_Get);
                return (RefBool)Dictionary[nameof(MachineCode_Hidden_Get)];

            }
            var rm = new RefBool(_MachineCode_Hidden_Get);
            Dictionary.Add(nameof(MachineCode_Hidden_Get), rm);
            return rm;
        }
        set
        {
            if (_MachineCode_Hidden_Get == value.Get()) return;
            _MachineCode_Hidden_Get = value.Get();
            OnPropertyChanged();
        }
    }
    public bool _MachineCode_Hidden_Set { get; set; } = true;
    [NotMapped]
    public RefBool MachineCode_Hidden_Set
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(MachineCode_Hidden_Set)))
            {
                ((RefBool)Dictionary[nameof(MachineCode_Hidden_Set)]).Set(_MachineCode_Hidden_Set);
                return (RefBool)Dictionary[nameof(MachineCode_Hidden_Set)];

            }
            var rm = new RefBool(_MachineCode_Hidden_Set);
            Dictionary.Add(nameof(MachineCode_Hidden_Set), rm);
            return rm;
        }
        set
        {
            if (_MachineCode_Hidden_Set == value.Get()) return;
            _MachineCode_Hidden_Set = value.Get();
            OnPropertyChanged();
        }
    }

    [NotMapped]
    [FormProperty(true, "Установки переработки", "код", "3")]
    public RamAccess<byte?> MachineCode
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(MachineCode)))
            {
                ((RamAccess<byte?>)Dictionary[nameof(MachineCode)]).Value = MachineCode_DB;
                return (RamAccess<byte?>)Dictionary[nameof(MachineCode)];
            }
            var rm = new RamAccess<byte?>(MachineCode_Validation, MachineCode_DB, MachineCode_Hidden_Get, MachineCode_Hidden_Set);
            rm.PropertyChanged += MachineCodeValueChanged;
            Dictionary.Add(nameof(MachineCode), rm);
            return (RamAccess<byte?>)Dictionary[nameof(MachineCode)];
        }
        set
        {
            if (MachineCode.Value == value.Value) return;
            MachineCode_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void MachineCodeValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        if (_MachineCode_Hidden_Set)
        {
            MachineCode_DB = ((RamAccess<byte?>)value).Value;
        }
    }

    private bool MachineCode_Validation(RamAccess<byte?> value) //TODO
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var a = value.Value is >= 11 and <= 17;
        var b = value.Value is >= 21 and <= 24;
        var c = value.Value is >= 31 and <= 32;
        var d = value.Value is >= 41 and <= 43;
        var e = value.Value is >= 51 and <= 56;
        var f = value.Value is >= 61 and <= 63;
        var g = value.Value is >= 71 and <= 73;
        var h = value.Value is 19 or 29 or 39 or 49 or 79 or 99;
        if (!(a || b || c || d || e || f || g || h))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region MachinePower_4
    public bool _MachinePower_Hidden_Get { get; set; } = true;
    [NotMapped]
    public RefBool MachinePower_Hidden_Get
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(MachinePower_Hidden_Get)))
            {
                ((RefBool)Dictionary[nameof(MachinePower_Hidden_Get)]).Set(_MachinePower_Hidden_Get);
                return (RefBool)Dictionary[nameof(MachinePower_Hidden_Get)];

            }
            var rm = new RefBool(_MachinePower_Hidden_Get);
            Dictionary.Add(nameof(MachinePower_Hidden_Get), rm);
            return rm;
        }
        set
        {
            if (_MachinePower_Hidden_Get == value.Get()) return;
            _MachinePower_Hidden_Get = value.Get();
            OnPropertyChanged();
        }
    }
    public bool _MachinePower_Hidden_Set { get; set; } = true;
    [NotMapped]
    public RefBool MachinePower_Hidden_Set
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(MachinePower_Hidden_Set)))
            {
                ((RefBool)Dictionary[nameof(MachinePower_Hidden_Set)]).Set(_MachinePower_Hidden_Set);
                return (RefBool)Dictionary[nameof(MachinePower_Hidden_Set)];

            }
            var rm = new RefBool(_MachinePower_Hidden_Set);
            Dictionary.Add(nameof(MachinePower_Hidden_Set), rm);
            return rm;
        }
        set
        {
            if (_MachinePower_Hidden_Set == value.Get()) return;
            _MachinePower_Hidden_Set = value.Get();
            OnPropertyChanged();
        }
    }

    public string MachinePower_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Установки переработки", "мощность, куб. м/год", "4")]
    public RamAccess<string> MachinePower
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(MachinePower)))
            {
                ((RamAccess<string>)Dictionary[nameof(MachinePower)]).Value = MachinePower_DB;
                return (RamAccess<string>)Dictionary[nameof(MachinePower)];

            }
            var rm = new RamAccess<string>(MachinePower_Validation, MachinePower_DB, MachinePower_Hidden_Get, MachinePower_Hidden_Set);
            rm.PropertyChanged += MachinePowerValueChanged;
            Dictionary.Add(nameof(MachinePower), rm);
            return rm;
        }
        set
        {
            if (MachinePower.Value == value.Value) return;
            MachinePower_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void MachinePowerValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value" || !_MachinePower_Hidden_Set) return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                MachinePower_DB = value1;
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
        MachinePower_DB = value1;
    }

    private bool MachinePower_Validation(RamAccess<string> value) //TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var tmp = value1;
        if (tmp[0] == '(' && tmp[^1] == ')')
        {
            tmp = tmp.Remove(tmp.Length - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpDouble))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpDouble <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region NumberOfHoursPerYear_5
    public string NumberOfHoursPerYear_DB { get; set; } = "";
    public bool _NumberOfHoursPerYear_Hidden_Get { get; set; } = true;
    [NotMapped]
    public RefBool NumberOfHoursPerYear_Hidden_Get
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(NumberOfHoursPerYear_Hidden_Get)))
            {
                ((RefBool)Dictionary[nameof(NumberOfHoursPerYear_Hidden_Get)]).Set(_NumberOfHoursPerYear_Hidden_Get);
                return (RefBool)Dictionary[nameof(NumberOfHoursPerYear_Hidden_Get)];

            }
            var rm = new RefBool(_NumberOfHoursPerYear_Hidden_Get);
            Dictionary.Add(nameof(NumberOfHoursPerYear_Hidden_Get), rm);
            return rm;
        }
        set
        {
            if (_NumberOfHoursPerYear_Hidden_Get == value.Get()) return;
            _NumberOfHoursPerYear_Hidden_Get = value.Get();
            OnPropertyChanged();
        }
    }
    public bool _NumberOfHoursPerYear_Hidden_Set { get; set; } = true;
    [NotMapped]
    public RefBool NumberOfHoursPerYear_Hidden_Set
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(NumberOfHoursPerYear_Hidden_Set)))
            {
                ((RefBool)Dictionary[nameof(NumberOfHoursPerYear_Hidden_Set)]).Set(_NumberOfHoursPerYear_Hidden_Set);
                return (RefBool)Dictionary[nameof(NumberOfHoursPerYear_Hidden_Set)];

            }
            var rm = new RefBool(_NumberOfHoursPerYear_Hidden_Set);
            Dictionary.Add(nameof(NumberOfHoursPerYear_Hidden_Set), rm);
            return rm;
        }
        set
        {
            if (_NumberOfHoursPerYear_Hidden_Set == value.Get()) return;
            _NumberOfHoursPerYear_Hidden_Set = value.Get();
            OnPropertyChanged();
        }
    }

    [NotMapped]
    [FormProperty(true, "Установки переработки", "количество часов работы за год", "5")]
    public RamAccess<string> NumberOfHoursPerYear
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(NumberOfHoursPerYear)))
            {
                ((RamAccess<string>)Dictionary[nameof(NumberOfHoursPerYear)]).Value = NumberOfHoursPerYear_DB;
                return (RamAccess<string>)Dictionary[nameof(NumberOfHoursPerYear)];
            }
            var rm = new RamAccess<string>(NumberOfHoursPerYear_Validation, NumberOfHoursPerYear_DB, NumberOfHoursPerYear_Hidden_Get, NumberOfHoursPerYear_Hidden_Set);
            rm.PropertyChanged += NumberOfHoursPerYearValueChanged;
            Dictionary.Add(nameof(NumberOfHoursPerYear), rm);
            return (RamAccess<string>)Dictionary[nameof(NumberOfHoursPerYear)];
        }
        set
        {
            if (NumberOfHoursPerYear.Value == value.Value) return;
            NumberOfHoursPerYear_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void NumberOfHoursPerYearValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value" || !_NumberOfHoursPerYear_Hidden_Set) return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                NumberOfHoursPerYear_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, out var valueDbl))
            {
                value1 = $"{valueDbl:0.######################################################e+00}";
            }
        }
        NumberOfHoursPerYear_DB = value1;
    }

    private bool NumberOfHoursPerYear_Validation(RamAccess<string> value) //TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return false;
        }
        if (value.Value.Equals("прим.") || value.Value.Equals("0"))
        {
            return true;
            //TODO
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var tmp = value1;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpDouble))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpDouble <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region CodeRAOIn_6
    public string CodeRAOIn_DB { get; set; } = "";
    public bool CodeRAOIn_Hidden_Priv { get; set; }
    [NotMapped]
    public bool CodeRAOIn_Hidden
    {
        get => CodeRAOIn_Hidden_Priv;
        set => CodeRAOIn_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "код РАО", "6")]
    public RamAccess<string> CodeRAOIn
    {
        get
        {
            if (CodeRAOIn_Hidden) return new RamAccess<string>(null, null);
            if (Dictionary.ContainsKey(nameof(CodeRAOIn)))
            {
                ((RamAccess<string>)Dictionary[nameof(CodeRAOIn)]).Value = CodeRAOIn_DB;
                return (RamAccess<string>)Dictionary[nameof(CodeRAOIn)];
            }
            var rm = new RamAccess<string>(CodeRAOIn_Validation, CodeRAOIn_DB);
            rm.PropertyChanged += CodeRAOInValueChanged;
            Dictionary.Add(nameof(CodeRAOIn), rm);
            return (RamAccess<string>)Dictionary[nameof(CodeRAOIn)];
        }
        set
        {
            if (CodeRAOIn_Hidden) return;
            CodeRAOIn_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void CodeRAOInValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        CodeRAOIn_DB = ((RamAccess<string>)value).Value.ToLower().Replace("х", "x");
    }

    private bool CodeRAOIn_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return false;
        }
        var tmp = value.Value.ToLower();
        tmp = tmp.Replace("х", "x");
        if (!new Regex("^[0-9x+]{11}$").IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmp.Length == 11)
        {
            if (!new Regex("^[1-3x+]").IsMatch(tmp[..1]))
            {
                value.AddError($"Недопустимое агрегатное состояние - {tmp[..1]}");
            }
            if (!new Regex("^[0-49x+]").IsMatch(tmp.Substring(1, 1)))
            {
                value.AddError($"Недопустимое категория РАО - {tmp.Substring(1, 1)}");
            }
            if (!new Regex("^[0-6x+]").IsMatch(tmp.Substring(2, 1)))
            {
                value.AddError($"Недопустимый радионуклидный состав РАО - {tmp.Substring(2, 1)}");
            }
            if (!new Regex("^[12x+]").IsMatch(tmp.Substring(3, 1)))
            {
                value.AddError($"Недопустимое содержание ядерных материалов - {tmp.Substring(3, 1)}");
            }
            if (!new Regex("^[12x+]").IsMatch(tmp.Substring(4, 1)))
            {
                value.AddError($"Недопустимый период полураспада - {tmp.Substring(4, 1)}");
            }
            if (!new Regex("^[0-3x+]").IsMatch(tmp.Substring(5, 1)))
            {
                value.AddError($"Недопустимый период потенциальной опасности РАО - {tmp.Substring(5, 1)}");
            }
            if (!new Regex("^[0-49x+]").IsMatch(tmp.Substring(6, 1)))
            {
                value.AddError($"Недопустимый способ переработки - {tmp.Substring(6, 1)}");
            }
            if (!new Regex("^[0-79x+]").IsMatch(tmp.Substring(7, 1)))
            {
                value.AddError($"Недопустимый класс РАО - {tmp.Substring(7, 1)}");
            }
            Regex a89 = new("^[1]{1}[1-9]{1}|^[0]{1}[1]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1-9]{1}|^[4]{1}[1-6]{1}|^[5]{1}[1-9]{1}|^[6]{1}[1-9]{1}|^[7]{1}[1-9]{1}|^[8]{1}[1-9]{1}|^[9]{1}[1-9]{1}");
            if (!a89.IsMatch(tmp.Substring(8, 2)))
            {
                value.AddError($"Недопустимый код типа РАО - {tmp.Substring(8, 2)}");
            }
            if (!new Regex("^[12x+]").IsMatch(tmp.Substring(10, 1)))
            {
                value.AddError($"Недопустимая горючесть - {tmp.Substring(10, 1)}");
            }
            if (value.HasErrors)
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region  StatusRAOIn_7
    public string StatusRAOIn_DB { get; set; } = "";
    public bool StatusRAOIn_Hidden_Priv { get; set; }
    [NotMapped]
    public bool StatusRAOIn_Hidden
    {
        get => StatusRAOIn_Hidden_Priv;
        set => StatusRAOIn_Hidden_Priv = value;
    }
    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "статус РАО", "7")]
    public RamAccess<string> StatusRAOIn  //1 cyfer or OKPO.
    {
        get
        {
            if (StatusRAOIn_Hidden) return new RamAccess<string>(null, null);
            if (Dictionary.ContainsKey(nameof(StatusRAOIn)))
            {
                ((RamAccess<string>)Dictionary[nameof(StatusRAOIn)]).Value = StatusRAOIn_DB;
                return (RamAccess<string>)Dictionary[nameof(StatusRAOIn)];
            }
            var rm = new RamAccess<string>(StatusRAOIn_Validation, StatusRAOIn_DB);
            rm.PropertyChanged += StatusRAOInValueChanged;
            Dictionary.Add(nameof(StatusRAOIn), rm);
            return (RamAccess<string>)Dictionary[nameof(StatusRAOIn)];
        }
        set
        {
            if (StatusRAOIn_Hidden) return;
            StatusRAOIn_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void StatusRAOInValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            StatusRAOIn_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool StatusRAOIn_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value == "-")
        {
            return true;
        }
        if (value.Value.Length == 1)
        {
            if (!int.TryParse(value.Value, out var tmpInt) || tmpInt < 1 || (tmpInt > 4 && tmpInt != 6 && tmpInt != 9))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        if (value.Value.Length != 8 && value.Value.Length != 14
            || !new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$").IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region VolumeIn_8
    public string VolumeIn_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "куб. м", "8")]
    public RamAccess<string> VolumeIn//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(VolumeIn)))
            {
                ((RamAccess<string>)Dictionary[nameof(VolumeIn)]).Value = VolumeIn_DB;
                return (RamAccess<string>)Dictionary[nameof(VolumeIn)];
            }
            var rm = new RamAccess<string>(VolumeIn_Validation, VolumeIn_DB);
            rm.PropertyChanged += VolumeInValueChanged;
            Dictionary.Add(nameof(VolumeIn), rm);
            return (RamAccess<string>)Dictionary[nameof(VolumeIn)];
        }
        set
        {
            VolumeIn_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void VolumeInValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                VolumeIn_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, out var valueDbl))
            {
                value1 = $"{valueDbl:0.######################################################e+00}";
            }
        }
        VolumeIn_DB = value1;
    }

    private bool VolumeIn_Validation(RamAccess<string> value) //TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("-"))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var tmp = value1;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpDouble))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpDouble <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region MassIn_9
    public string MassIn_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "т", "9")]
    public RamAccess<string> MassIn//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(MassIn)))
            {
                ((RamAccess<string>)Dictionary[nameof(MassIn)]).Value = MassIn_DB;
                return (RamAccess<string>)Dictionary[nameof(MassIn)];
            }
            var rm = new RamAccess<string>(MassIn_Validation, MassIn_DB);
            rm.PropertyChanged += MassInValueChanged;
            Dictionary.Add(nameof(MassIn), rm);
            return (RamAccess<string>)Dictionary[nameof(MassIn)];
        }
        set
        {
            MassIn_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void MassInValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                MassIn_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, out var valueDbl))
            {
                value1 = $"{valueDbl:0.######################################################e+00}";
            }
        }
        MassIn_DB = value1;
    }
    private bool MassIn_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("-"))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var tmp = value1;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpDouble))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpDouble <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region QuantityIn_10
    public string QuantityIn_DB { get; set; } = ""; [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "ОЗИИИ, шт", "10")]
    public RamAccess<string> QuantityIn//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(QuantityIn)))
            {
                ((RamAccess<string>)Dictionary[nameof(QuantityIn)]).Value = QuantityIn_DB;
                return (RamAccess<string>)Dictionary[nameof(QuantityIn)];
            }
            var rm = new RamAccess<string>(QuantityIn_Validation, QuantityIn_DB);
            rm.PropertyChanged += QuantityInValueChanged;
            Dictionary.Add(nameof(QuantityIn), rm);
            return (RamAccess<string>)Dictionary[nameof(QuantityIn)];
        }
        set
        {
            QuantityIn_DB = value.Value;
            OnPropertyChanged();
        }
    }
    // positive int.

    private void QuantityInValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            QuantityIn_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool QuantityIn_Validation(RamAccess<string> value1)//Ready
    {
        value1.ClearErrors();
        if (value1.Value is null or "" or "-" or "прим.")
        {
            return true;
        }
        var tmp = value1.Value;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!int.TryParse(tmp, out var tmpInt))
        {
            value1.AddError("Недопустимое значение");
            return false;
        }
        if (tmpInt <= 0)
        {
            value1.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region TritiumActivityIn_11
    public string TritiumActivityIn_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "тритий", "11")]
    public RamAccess<string> TritiumActivityIn//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(TritiumActivityIn)))
            {
                ((RamAccess<string>)Dictionary[nameof(TritiumActivityIn)]).Value = TritiumActivityIn_DB;
                return (RamAccess<string>)Dictionary[nameof(TritiumActivityIn)];
            }
            var rm = new RamAccess<string>(TritiumActivityIn_Validation, TritiumActivityIn_DB);
            rm.PropertyChanged += TritiumActivityInValueChanged;
            Dictionary.Add(nameof(TritiumActivityIn), rm);
            return (RamAccess<string>)Dictionary[nameof(TritiumActivityIn)];
        }
        set
        {
            TritiumActivityIn_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TritiumActivityInValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                TritiumActivityIn_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, out var valueDbl))
            {
                value1 = $"{valueDbl:0.######################################################e+00}";
            }
        }
        TritiumActivityIn_DB = value1;
    }

    private bool TritiumActivityIn_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (value.Value is null or "" or "-")
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var tmp = value1;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpDouble))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpDouble <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region BetaGammaActivityIn_12
    public string BetaGammaActivityIn_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "12")]
    public RamAccess<string> BetaGammaActivityIn//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(BetaGammaActivityIn)))
            {
                ((RamAccess<string>)Dictionary[nameof(BetaGammaActivityIn)]).Value = BetaGammaActivityIn_DB;
                return (RamAccess<string>)Dictionary[nameof(BetaGammaActivityIn)];
            }
            var rm = new RamAccess<string>(BetaGammaActivityIn_Validation, BetaGammaActivityIn_DB);
            rm.PropertyChanged += BetaGammaActivityInValueChanged;
            Dictionary.Add(nameof(BetaGammaActivityIn), rm);
            return (RamAccess<string>)Dictionary[nameof(BetaGammaActivityIn)];
        }
        set
        {
            BetaGammaActivityIn_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void BetaGammaActivityInValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                BetaGammaActivityIn_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, out var valueDbl))
            {
                value1 = $"{valueDbl:0.######################################################e+00}";
            }
        }
        BetaGammaActivityIn_DB = value1;
    }
    private bool BetaGammaActivityIn_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (value.Value is null or "" or "-")
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var tmp = value1;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpDouble))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpDouble <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region AlphaActivityIn_13
    public string AlphaActivityIn_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "альфа-излучающие радионуклиды (исключая трансурановые)", "13")]
    public RamAccess<string> AlphaActivityIn//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(AlphaActivityIn)))
            {
                ((RamAccess<string>)Dictionary[nameof(AlphaActivityIn)]).Value = AlphaActivityIn_DB;
                return (RamAccess<string>)Dictionary[nameof(AlphaActivityIn)];
            }
            var rm = new RamAccess<string>(AlphaActivityIn_Validation, AlphaActivityIn_DB);
            rm.PropertyChanged += AlphaActivityInValueChanged;
            Dictionary.Add(nameof(AlphaActivityIn), rm);
            return (RamAccess<string>)Dictionary[nameof(AlphaActivityIn)];
        }
        set
        {
            AlphaActivityIn_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void AlphaActivityInValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                AlphaActivityIn_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, out var valueDbl))
            {
                value1 = $"{valueDbl:0.######################################################e+00}";
            }
        }
        AlphaActivityIn_DB = value1;
    }

    private bool AlphaActivityIn_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (value.Value is null or "" or "-")
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var tmp = value1;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpDouble))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpDouble <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region TransuraniumActivityIn_14
    public string TransuraniumActivityIn_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "трансурановые радионуклиды", "14")]
    public RamAccess<string> TransuraniumActivityIn//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(TransuraniumActivityIn)))
            {
                ((RamAccess<string>)Dictionary[nameof(TransuraniumActivityIn)]).Value = TransuraniumActivityIn_DB;
                return (RamAccess<string>)Dictionary[nameof(TransuraniumActivityIn)];
            }
            var rm = new RamAccess<string>(TransuraniumActivityIn_Validation, TransuraniumActivityIn_DB);
            rm.PropertyChanged += TransuraniumActivityInValueChanged;
            Dictionary.Add(nameof(TransuraniumActivityIn), rm);
            return (RamAccess<string>)Dictionary[nameof(TransuraniumActivityIn)];
        }
        set
        {
            TransuraniumActivityIn_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TransuraniumActivityInValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                TransuraniumActivityIn_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, out var valueDbl))
            {
                value1 = $"{valueDbl:0.######################################################e+00}";
            }
        }
        TransuraniumActivityIn_DB = value1;
    }

    private bool TransuraniumActivityIn_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (value.Value is null or "" or "-")
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var tmp = value1;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpDouble))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpDouble <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region CodeRAOout_15
    public string CodeRAOout_DB { get; set; } = "";
    public bool CodeRAOout_Hidden_Priv { get; set; }
    [NotMapped]
    public bool CodeRAOout_Hidden
    {
        get => CodeRAOout_Hidden_Priv;
        set => CodeRAOout_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "код РАО", "15")]
    public RamAccess<string> CodeRAOout
    {
        get
        {
            if (CodeRAOout_Hidden) return new RamAccess<string>(null, null);
            if (Dictionary.ContainsKey(nameof(CodeRAOout)))
            {
                ((RamAccess<string>)Dictionary[nameof(CodeRAOout)]).Value = CodeRAOout_DB;
                return (RamAccess<string>)Dictionary[nameof(CodeRAOout)];
            }
            var rm = new RamAccess<string>(CodeRAOout_Validation, CodeRAOout_DB);
            rm.PropertyChanged += CodeRAOoutValueChanged;
            Dictionary.Add(nameof(CodeRAOout), rm);
            return (RamAccess<string>)Dictionary[nameof(CodeRAOout)];
        }
        set
        {
            if (CodeRAOout_Hidden) return;
            CodeRAOout_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void CodeRAOoutValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        CodeRAOout_DB = ((RamAccess<string>)value).Value.ToLower().Replace("х", "x");
    }

    private bool CodeRAOout_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return false;
        }
        var tmp = value.Value.ToLower();
        tmp = tmp.Replace("х", "x");
        if (!new Regex("^[0-9x+]{11}$").IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmp.Length == 11)
        {
            if (!new Regex("^[1-3x+]").IsMatch(tmp[..1]))
            {
                value.AddError($"Недопустимое агрегатное состояние - {tmp[..1]}");
            }
            if (!new Regex("^[0-49x+]").IsMatch(tmp.Substring(1, 1)))
            {
                value.AddError($"Недопустимое категория РАО - {tmp.Substring(1, 1)}");
            }
            if (!new Regex("^[0-6x+]").IsMatch(tmp.Substring(2, 1)))
            {
                value.AddError($"Недопустимый радионуклидный состав РАО - {tmp.Substring(2, 1)}");
            }
            if (!new Regex("^[12x+]").IsMatch(tmp.Substring(3, 1)))
            {
                value.AddError($"Недопустимое содержание ядерных материалов - {tmp.Substring(3, 1)}");
            }
            if (!new Regex("^[12x+]").IsMatch(tmp.Substring(4, 1)))
            {
                value.AddError($"Недопустимый период полураспада - {tmp.Substring(4, 1)}");
            }
            if (!new Regex("^[0-3x+]").IsMatch(tmp.Substring(5, 1)))
            {
                value.AddError($"Недопустимый период потенциальной опасности РАО - {tmp.Substring(5, 1)}");
            }
            if (!new Regex("^[0-49x+]").IsMatch(tmp.Substring(6, 1)))
            {
                value.AddError($"Недопустимый способ переработки - {tmp.Substring(6, 1)}");
            }
            if (!new Regex("^[0-79x+]").IsMatch(tmp.Substring(7, 1)))
            {
                value.AddError($"Недопустимый класс РАО - {tmp.Substring(7, 1)}");
            }
            Regex a89 = new("^[1]{1}[1-9]{1}|^[0]{1}[1]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1-9]{1}|^[4]{1}[1-6]{1}|^[5]{1}[1-9]{1}|^[6]{1}[1-9]{1}|^[7]{1}[1-9]{1}|^[8]{1}[1-9]{1}|^[9]{1}[1-9]{1}");
            if (!a89.IsMatch(tmp.Substring(8, 2)))
            {
                value.AddError($"Недопустимый код типа РАО - {tmp.Substring(8, 2)}");
            }
            if (!new Regex("^[12x+]").IsMatch(tmp.Substring(10, 1)))
            {
                value.AddError($"Недопустимая горючесть - {tmp.Substring(10, 1)}");
            }
            if (value.HasErrors)
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region StatusRAOout_16
    public string StatusRAOout_DB { get; set; } = "";
    public bool StatusRAOout_Hidden_Priv { get; set; }
    [NotMapped]
    public bool StatusRAOout_Hidden
    {
        get => StatusRAOout_Hidden_Priv;
        set => StatusRAOout_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "статус РАО", "16")]
    public RamAccess<string> StatusRAOout  //1 cyfer or OKPO.
    {
        get
        {
            if (!StatusRAOout_Hidden)
            {
                if (Dictionary.ContainsKey(nameof(StatusRAOout)))
                {
                    ((RamAccess<string>)Dictionary[nameof(StatusRAOout)]).Value = StatusRAOout_DB;
                    return (RamAccess<string>)Dictionary[nameof(StatusRAOout)];
                }
                var rm = new RamAccess<string>(StatusRAOout_Validation, StatusRAOout_DB);
                rm.PropertyChanged += StatusRAOoutValueChanged;
                Dictionary.Add(nameof(StatusRAOout), rm);
                return (RamAccess<string>)Dictionary[nameof(StatusRAOout)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (StatusRAOout_Hidden) return;
            StatusRAOout_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void StatusRAOoutValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            StatusRAOout_DB = ((RamAccess<string>)value).Value;
        }
    }
    private bool StatusRAOout_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        if (value.Value.Length == 1)
        {
            if (!int.TryParse(value.Value, out var tmpInt) || tmpInt > 4 && tmpInt != 6 && tmpInt != 9)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        if (value.Value.Length != 8 && value.Value.Length != 14
            || !new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$").IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region VolumeOut_17
    public string VolumeOut_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "куб. м", "17")]
    public RamAccess<string> VolumeOut//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(VolumeOut)))
            {
                ((RamAccess<string>)Dictionary[nameof(VolumeOut)]).Value = VolumeOut_DB;
                return (RamAccess<string>)Dictionary[nameof(VolumeOut)];
            }
            var rm = new RamAccess<string>(VolumeOut_Validation, VolumeOut_DB);
            rm.PropertyChanged += VolumeOutValueChanged;
            Dictionary.Add(nameof(VolumeOut), rm);
            return (RamAccess<string>)Dictionary[nameof(VolumeOut)];
        }
        set
        {
            VolumeOut_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void VolumeOutValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                VolumeOut_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, out var valueDbl))
            {
                value1 = $"{valueDbl:0.######################################################e+00}";
            }
        }
        VolumeOut_DB = value1;
    }

    private bool VolumeOut_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("-"))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var tmp = value1;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpDouble))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpDouble <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region MassOut_18
    public string MassOut_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "т", "18")]
    public RamAccess<string> MassOut//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(MassOut)))
            {
                ((RamAccess<string>)Dictionary[nameof(MassOut)]).Value = MassOut_DB;
                return (RamAccess<string>)Dictionary[nameof(MassOut)];
            }
            var rm = new RamAccess<string>(MassOut_Validation, MassOut_DB);
            rm.PropertyChanged += MassOutValueChanged;
            Dictionary.Add(nameof(MassOut), rm);
            return (RamAccess<string>)Dictionary[nameof(MassOut)];
        }
        set
        {
            MassOut_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void MassOutValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                MassOut_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, out var valueDbl))
            {
                value1 = $"{valueDbl:0.######################################################e+00}";
            }
        }
        MassOut_DB = value1;
    }

    private bool MassOut_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("-"))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var tmp = value1;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpDouble))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpDouble <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region QuantityOZIIIout_19
    public string QuantityOZIIIout_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "ОЗИИИ, шт", "19")]
    public RamAccess<string> QuantityOZIIIout//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(QuantityOZIIIout)))
            {
                ((RamAccess<string>)Dictionary[nameof(QuantityOZIIIout)]).Value = QuantityOZIIIout_DB;
                return (RamAccess<string>)Dictionary[nameof(QuantityOZIIIout)];
            }
            var rm = new RamAccess<string>(QuantityOZIIIout_Validation, QuantityOZIIIout_DB);
            rm.PropertyChanged += QuantityOZIIIoutValueChanged;
            Dictionary.Add(nameof(QuantityOZIIIout), rm);
            return (RamAccess<string>)Dictionary[nameof(QuantityOZIIIout)];
        }
        set
        {
            QuantityOZIIIout_DB = value.Value;
            OnPropertyChanged();
        }
    }
    // positive int.

    private void QuantityOZIIIoutValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            QuantityOZIIIout_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool QuantityOZIIIout_Validation(RamAccess<string> value1)//Ready
    {
        value1.ClearErrors();
        if (string.IsNullOrEmpty(value1.Value))
        {
            value1.AddError("Поле не заполнено");
            return false;
        }
        if (value1.Value is "-" or "прим.")
        {
            return true;
        }
        var tmp = value1.Value;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!int.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpInt))
        {
            value1.AddError("Недопустимое значение");
            return false;
        }
        if (tmpInt <= 0)
        {
            value1.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region TritiumActivityOut_20
    public string TritiumActivityOut_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "тритий", "20")]
    public RamAccess<string> TritiumActivityOut//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(TritiumActivityOut)))
            {
                ((RamAccess<string>)Dictionary[nameof(TritiumActivityOut)]).Value = TritiumActivityOut_DB;
                return (RamAccess<string>)Dictionary[nameof(TritiumActivityOut)];
            }
            var rm = new RamAccess<string>(TritiumActivityOut_Validation, TritiumActivityOut_DB);
            rm.PropertyChanged += TritiumActivityOutValueChanged;
            Dictionary.Add(nameof(TritiumActivityOut), rm);
            return (RamAccess<string>)Dictionary[nameof(TritiumActivityOut)];
        }
        set
        {
            TritiumActivityOut_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TritiumActivityOutValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                TritiumActivityOut_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, out var valueDbl))
            {
                value1 = $"{valueDbl:0.######################################################e+00}";
            }
        }
        TritiumActivityOut_DB = value1;
    }

    private bool TritiumActivityOut_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value == "-")
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var tmp = value1;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpDouble))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpDouble <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region BetaGammaActivityOut_21
    public string BetaGammaActivityOut_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "21")]
    public RamAccess<string> BetaGammaActivityOut//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(BetaGammaActivityOut)))
            {
                ((RamAccess<string>)Dictionary[nameof(BetaGammaActivityOut)]).Value = BetaGammaActivityOut_DB;
                return (RamAccess<string>)Dictionary[nameof(BetaGammaActivityOut)];
            }
            var rm = new RamAccess<string>(BetaGammaActivityOut_Validation, BetaGammaActivityOut_DB);
            rm.PropertyChanged += BetaGammaActivityOutValueChanged;
            Dictionary.Add(nameof(BetaGammaActivityOut), rm);
            return (RamAccess<string>)Dictionary[nameof(BetaGammaActivityOut)];
        }
        set
        {
            BetaGammaActivityOut_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void BetaGammaActivityOutValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                BetaGammaActivityOut_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, out var valueDbl))
            {
                value1 = $"{valueDbl:0.######################################################e+00}";
            }
        }
        BetaGammaActivityOut_DB = value1;
    }

    private bool BetaGammaActivityOut_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value == "-")
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var tmp = value1;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpDouble))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpDouble <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region AlphaActivityOut_22
    public string AlphaActivityOut_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "альфа-излучающие радионуклиды (исключая трансурановые)", "22")]
    public RamAccess<string> AlphaActivityOut //SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(AlphaActivityOut)))
            {
                ((RamAccess<string>)Dictionary[nameof(AlphaActivityOut)]).Value = AlphaActivityOut_DB;
                return (RamAccess<string>)Dictionary[nameof(AlphaActivityOut)];
            }
            var rm = new RamAccess<string>(AlphaActivityOut_Validation, AlphaActivityOut_DB);
            rm.PropertyChanged += AlphaActivityOutValueChanged;
            Dictionary.Add(nameof(AlphaActivityOut), rm);
            return (RamAccess<string>)Dictionary[nameof(AlphaActivityOut)];
        }
        set
        {
            AlphaActivityOut_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void AlphaActivityOutValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                AlphaActivityOut_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, out var valueDbl))
            {
                value1 = $"{valueDbl:0.######################################################e+00}";
            }
        }
        AlphaActivityOut_DB = value1;
    }

    private bool AlphaActivityOut_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value == "-")
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var tmp = value1;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpDouble))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpDouble <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region TransuraniumActivityOut_23
    public string TransuraniumActivityOut_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "трансурановые радионуклиды", "23")]
    public RamAccess<string> TransuraniumActivityOut//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(TransuraniumActivityOut)))
            {
                ((RamAccess<string>)Dictionary[nameof(TransuraniumActivityOut)]).Value = TransuraniumActivityOut_DB;
                return (RamAccess<string>)Dictionary[nameof(TransuraniumActivityOut)];
            }
            var rm = new RamAccess<string>(TransuraniumActivityOut_Validation, TransuraniumActivityOut_DB);
            rm.PropertyChanged += TransuraniumActivityOutValueChanged;
            Dictionary.Add(nameof(TransuraniumActivityOut), rm);
            return (RamAccess<string>)Dictionary[nameof(TransuraniumActivityOut)];
        }
        set
        {
            TransuraniumActivityOut_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TransuraniumActivityOutValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e')
                .Replace('E', 'e').Replace(" ", "")
                .Replace("\n", "").Replace("\t", "").Replace("\r", "");
            if (value1.Equals("-"))
            {
                TransuraniumActivityOut_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, out var valueDbl))
            {
                value1 = $"{valueDbl:0.######################################################e+00}";
            }
        }
        TransuraniumActivityOut_DB = value1;
    }

    private bool TransuraniumActivityOut_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value == "-")
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e')
            .Replace('E', 'e').Replace(" ", "")
            .Replace("\n", "").Replace("\t", "").Replace("\r", "");
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var tmp = value1;
        var len = tmp.Length;
        if (tmp[0] == '(' && tmp[len - 1] == ')')
        {
            tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
        }
        if (!double.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("en-GB"), out var tmpDouble))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpDouble <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion 

    #endregion

    #region IExcel
    public new void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        base.ExcelGetRow(worksheet, row);
        RefineMachineName.Value = Convert.ToString(worksheet.Cells[row, 2].Value);
        MachineCode.Value = byte.TryParse(worksheet.Cells[row, 3].Value.ToString(), out var byteVal) ? byteVal : null;
        MachinePower.Value = ConvertFromExcelDouble(worksheet.Cells[row, 4].Value);
        NumberOfHoursPerYear.Value = ConvertFromExcelDouble(worksheet.Cells[row, 5].Value);
        CodeRAOIn_DB = Convert.ToString(worksheet.Cells[row, 6].Value);
        StatusRAOIn_DB = Convert.ToString(worksheet.Cells[row, 7].Value);
        VolumeIn_DB = ConvertFromExcelDouble(worksheet.Cells[row, 8].Value);
        MassIn_DB = ConvertFromExcelDouble(worksheet.Cells[row, 9].Value);
        QuantityIn_DB = ConvertFromExcelInt(worksheet.Cells[row, 10].Value);
        TritiumActivityIn_DB = ConvertFromExcelDouble(worksheet.Cells[row, 11].Value);
        BetaGammaActivityIn_DB = ConvertFromExcelDouble(worksheet.Cells[row, 12].Value);
        AlphaActivityIn_DB = ConvertFromExcelDouble(worksheet.Cells[row, 13].Value);
        TransuraniumActivityIn_DB = ConvertFromExcelDouble(worksheet.Cells[row, 14].Value);
        CodeRAOout_DB = Convert.ToString(worksheet.Cells[row, 15].Value);
        StatusRAOout_DB = Convert.ToString(worksheet.Cells[row, 16].Value);
        VolumeOut_DB = ConvertFromExcelDouble(worksheet.Cells[row, 17].Value);
        MassOut_DB = ConvertFromExcelDouble(worksheet.Cells[row, 18].Value);
        QuantityOZIIIout_DB = ConvertFromExcelInt(worksheet.Cells[row, 19].Value);
        TritiumActivityOut_DB = ConvertFromExcelDouble(worksheet.Cells[row, 20].Value);
        BetaGammaActivityOut_DB = ConvertFromExcelDouble(worksheet.Cells[row, 21].Value);
        AlphaActivityOut_DB = ConvertFromExcelDouble(worksheet.Cells[row, 22].Value);
        TransuraniumActivityOut_DB = ConvertFromExcelDouble(worksheet.Cells[row, 23].Value);
    }

    public new int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose, sumNumber);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;
        worksheet.Cells[row, column].Value = ConvertToExcelString(RefineMachineName.Value);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = MachineCode.Value is null ? "-" : MachineCode.Value;
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelDouble(MachinePower.Value);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelDouble(NumberOfHoursPerYear.Value);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelString(CodeRAOIn_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelString(StatusRAOIn_DB);
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDouble(VolumeIn_DB);
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelDouble(MassIn_DB);
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ConvertToExcelInt(QuantityIn_DB);
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ConvertToExcelDouble(TritiumActivityIn_DB);
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ConvertToExcelDouble(BetaGammaActivityIn_DB);
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ConvertToExcelDouble(AlphaActivityIn_DB);
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ConvertToExcelDouble(TransuraniumActivityIn_DB);
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ConvertToExcelString(CodeRAOout_DB);
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ConvertToExcelString(StatusRAOout_DB);
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ConvertToExcelDouble(VolumeOut_DB);
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ConvertToExcelDouble(MassOut_DB);
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ConvertToExcelInt(QuantityOZIIIout_DB);
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ConvertToExcelDouble(TritiumActivityOut_DB);
        worksheet.Cells[row + (!transpose ? 19 : 0), column + (transpose ? 19 : 0)].Value = ConvertToExcelDouble(BetaGammaActivityOut_DB);
        worksheet.Cells[row + (!transpose ? 20 : 0), column + (transpose ? 20 : 0)].Value = ConvertToExcelDouble(AlphaActivityOut_DB);
        worksheet.Cells[row + (!transpose ? 21 : 0), column + (transpose ? 21 : 0)].Value = ConvertToExcelDouble(TransuraniumActivityOut_DB);
        return 22;
    }

    public new static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form2.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;
        worksheet.Cells[row, column].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(RefineMachineName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(MachineCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(MachinePower))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(NumberOfHoursPerYear))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(CodeRAOIn))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(StatusRAOIn))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(VolumeIn))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(MassIn))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(QuantityIn))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(TritiumActivityIn))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(BetaGammaActivityIn))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(AlphaActivityIn))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(TransuraniumActivityIn))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(CodeRAOout))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(StatusRAOout))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(VolumeOut))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(MassOut))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(QuantityOZIIIout))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(TritiumActivityOut))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 19 : 0), column + (transpose ? 19 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(BetaGammaActivityOut))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 20 : 0), column + (transpose ? 20 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(AlphaActivityOut))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 21 : 0), column + (transpose ? 21 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form21,Models")?.GetProperty(nameof(TransuraniumActivityOut))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        return 22;
    }
    #endregion

    #region IDataGridColumn
    private static DataGridColumns _DataGridColumns { get; set; }
    public override DataGridColumns GetColumnStructure(string param = "")
    {
        if (_DataGridColumns != null) return _DataGridColumns;

        #region NumberInOrder (1)
        var numberInOrderR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(NumberInOrderSum))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD();
        if (numberInOrderR != null)
        {
            numberInOrderR.SetSizeColToAllLevels(50);
            numberInOrderR.Binding = nameof(NumberInOrderSum);
            numberInOrderR.Blocked = true;
            numberInOrderR.ChooseLine = true;
        }
        #endregion

        #region RefineMachineName (2)
        var refineMachineNameR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(RefineMachineName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (refineMachineNameR != null)
        {
            refineMachineNameR.SetSizeColToAllLevels(200);
            refineMachineNameR.Binding = nameof(RefineMachineName);
            numberInOrderR += refineMachineNameR;
        }
        #endregion

        #region MachineCode (3)
        var machineCodeR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(MachineCode))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (machineCodeR != null)
        {
            machineCodeR.SetSizeColToAllLevels(60);
            machineCodeR.Binding = nameof(MachineCode);
            numberInOrderR += machineCodeR;
        }
        #endregion

        #region MachinePower (4)
        var machinePowerR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(MachinePower))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (machinePowerR != null)
        {
            machinePowerR.SetSizeColToAllLevels(80);
            machinePowerR.Binding = nameof(MachinePower);
            numberInOrderR += machinePowerR;
        }
        #endregion

        #region NumberOfHoursPerYear (5)
        var numberOfHoursPerYearR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(NumberOfHoursPerYear))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (numberOfHoursPerYearR != null)
        {
            numberOfHoursPerYearR.SetSizeColToAllLevels(110);
            numberOfHoursPerYearR.Binding = nameof(NumberOfHoursPerYear);
            numberInOrderR += numberOfHoursPerYearR;
        }
        #endregion

        #region CodeRAOIn (6)
        var codeRaoInR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(CodeRAOIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (codeRaoInR != null)
        {
            codeRaoInR.SetSizeColToAllLevels(88);
            codeRaoInR.Binding = nameof(CodeRAOIn);
            numberInOrderR += codeRaoInR;
        }
        #endregion

        #region StatusRAOIn (7)
        var statusRaoInR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(StatusRAOIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (statusRaoInR != null)
        {
            statusRaoInR.SetSizeColToAllLevels(88);
            statusRaoInR.Binding = nameof(StatusRAOIn);
            numberInOrderR += statusRaoInR;
        }
        #endregion

        #region VolumeIn (8)
        var volumeInR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(VolumeIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (volumeInR != null)
        {
            volumeInR.SetSizeColToAllLevels(88);
            volumeInR.Binding = nameof(VolumeIn);
            numberInOrderR += volumeInR;
        }
        #endregion

        #region MassIn (9)
        var massInR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(MassIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (massInR != null)
        {
            massInR.SetSizeColToAllLevels(88);
            massInR.Binding = nameof(MassIn);
            numberInOrderR += massInR;
        }
        #endregion

        #region QuantityIn (10)
        var quantityInR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(QuantityIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (quantityInR != null)
        {
            quantityInR.SetSizeColToAllLevels(88);
            quantityInR.Binding = nameof(QuantityIn);
            numberInOrderR += quantityInR;
        }
        #endregion

        #region TritiumActivityIn (11)
        var tritiumActivityInR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(TritiumActivityIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (tritiumActivityInR != null)
        {
            tritiumActivityInR.SetSizeColToAllLevels(163);
            tritiumActivityInR.Binding = nameof(TritiumActivityIn);
            numberInOrderR += tritiumActivityInR;
        }
        #endregion

        #region BetaGammaActivityIn (12)
        var betaGammaActivityInR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(BetaGammaActivityIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (betaGammaActivityInR != null)
        {
            betaGammaActivityInR.SetSizeColToAllLevels(160);
            betaGammaActivityInR.Binding = nameof(BetaGammaActivityIn);
            numberInOrderR += betaGammaActivityInR;
        }
        #endregion

        #region AlphaActivity (13)
        var alphaActivityR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(AlphaActivityIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (alphaActivityR != null)
        {
            alphaActivityR.SetSizeColToAllLevels(170);
            alphaActivityR.Binding = nameof(AlphaActivityIn);
            numberInOrderR += alphaActivityR;
        }
        #endregion

        #region TransuraniumActivity (14)
        var transuraniumActivityR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(TransuraniumActivityIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (transuraniumActivityR != null)
        {
            transuraniumActivityR.SetSizeColToAllLevels(200);
            transuraniumActivityR.Binding = nameof(TransuraniumActivityIn);
            numberInOrderR += transuraniumActivityR;
        }
        #endregion

        #region CodeRAOout (15)
        var codeRaoOutR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(CodeRAOout))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        if (codeRaoOutR != null)
        {
            codeRaoOutR.SetSizeColToAllLevels(88);
            codeRaoOutR.Binding = nameof(CodeRAOout);
            numberInOrderR += codeRaoOutR;
        }
        #endregion

        #region StatusRAOout (16)
        var statusRaoOutR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(StatusRAOout))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        if (statusRaoOutR != null)
        {
            statusRaoOutR.SetSizeColToAllLevels(88);
            statusRaoOutR.Binding = nameof(StatusRAOout);
            numberInOrderR += statusRaoOutR;
        }
        #endregion

        #region VolumeOut (17)
        var volumeOutR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(VolumeOut))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        if (volumeOutR != null)
        {
            volumeOutR.SetSizeColToAllLevels(88);
            volumeOutR.Binding = nameof(VolumeOut);
            numberInOrderR += volumeOutR;
        }
        #endregion

        #region MassOut (18)
        var massOutR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(MassOut))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        if (massOutR != null)
        {
            massOutR.SetSizeColToAllLevels(88);
            massOutR.Binding = nameof(MassOut);
            numberInOrderR += massOutR;
        }
        #endregion

        #region QuantityOZIIIout (19)
        var quantityOziiiOutR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(QuantityOZIIIout))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        if (quantityOziiiOutR != null)
        {
            quantityOziiiOutR.SetSizeColToAllLevels(88);
            quantityOziiiOutR.Binding = nameof(QuantityOZIIIout);
            numberInOrderR += quantityOziiiOutR;
        }
        #endregion

        #region TritiumActivityOut (20)
        var tritiumActivityOutR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(TritiumActivityOut))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        if (tritiumActivityOutR != null)
        {
            tritiumActivityOutR.SetSizeColToAllLevels(163);
            tritiumActivityOutR.Binding = nameof(TritiumActivityOut);
            numberInOrderR += tritiumActivityOutR;
        }
        #endregion

        #region BetaGammaActivityOut (21)
        var betaGammaActivityOutR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(BetaGammaActivityOut))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        if (betaGammaActivityOutR != null)
        {
            betaGammaActivityOutR.SetSizeColToAllLevels(170);
            betaGammaActivityOutR.Binding = nameof(BetaGammaActivityOut);
            numberInOrderR += betaGammaActivityOutR;
        }
        #endregion

        #region AlphaActivityOut (22)
        var alphaActivityOutR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(AlphaActivityOut))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        if (alphaActivityOutR != null)
        {
            alphaActivityOutR.SetSizeColToAllLevels(170);
            alphaActivityOutR.Binding = nameof(AlphaActivityOut);
            numberInOrderR += alphaActivityOutR;
        }
        #endregion

        #region TransuraniumActivityOut (23)
        var transuraniumActivityOutR =
            ((FormPropertyAttribute)typeof(Form21).GetProperty(nameof(TransuraniumActivityOut))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        if (transuraniumActivityOutR != null)
        {
            transuraniumActivityOutR.SetSizeColToAllLevels(163);
            transuraniumActivityOutR.Binding = nameof(TransuraniumActivityOut);
            numberInOrderR += transuraniumActivityOutR;
        }
        #endregion

        _DataGridColumns = numberInOrderR;

        return _DataGridColumns;
    }
    #endregion
}