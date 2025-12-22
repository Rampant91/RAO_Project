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
[Table (name: "form_21")]
public partial class Form21 : Form2, IBaseColor
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

    #region Properties

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
            tmp.PropertyChanged += Sum_ValueChanged;
            return tmp;
        }
        set
        {
            Sum_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void Sum_ValueChanged(object value, PropertyChangedEventArgs args)
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
            tmp.PropertyChanged += SumGroup_ValueChanged;
            return tmp;
        }
        set
        {
            SumGroup_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void SumGroup_ValueChanged(object value, PropertyChangedEventArgs args)
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

    #region NumberInOrder (1)

    public string NumberInOrderSum_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-1-1", "null-1", "№ п/п", "1")]
    public RamAccess<string> NumberInOrderSum
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(NumberInOrderSum), out var value))
            {
                ((RamAccess<string>)value).Value = !string.IsNullOrEmpty(NumberInOrderSum_DB)
                    ? NumberInOrderSum_DB
                    : NumberInOrder_DB.ToString();
                return (RamAccess<string>)value;
            }
            var rm = !string.IsNullOrEmpty(NumberInOrderSum_DB)
                ? new RamAccess<string>(NumberInOrderSum_Validation, NumberInOrderSum_DB)
                : new RamAccess<string>(NumberInOrderSum_Validation, NumberInOrder_DB.ToString());
            rm.PropertyChanged += NumberInOrderSum_ValueChanged;
            Dictionary.Add(nameof(NumberInOrderSum), rm);
            return (RamAccess<string>)Dictionary[nameof(NumberInOrderSum)];
        }
        set
        {
            NumberInOrderSum_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void NumberInOrderSum_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            NumberInOrderSum_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool NumberInOrderSum_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        return true;
    }
    
    #endregion

    #region RefineMachineName (2)

    public string RefineMachineName_DB { get; set; } = "";

    public bool _RefineMachineName_Hidden_Get { get; set; } = true;

    [NotMapped]
    public RefBool RefineMachineName_Hidden_Get
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(RefineMachineName_Hidden_Get), out var value))
            {
                ((RefBool)value).Set(_RefineMachineName_Hidden_Get);
                return (RefBool)value;

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
            if (Dictionary.TryGetValue(nameof(RefineMachineName_Hidden_Set), out var value))
            {
                ((RefBool)value).Set(_RefineMachineName_Hidden_Set);
                return (RefBool)value;

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
            if (Dictionary.TryGetValue(nameof(RefineMachineName), out var value))
            {
                ((RamAccess<string>)value).Value = RefineMachineName_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(RefineMachineName_Validation, RefineMachineName_DB, RefineMachineName_Hidden_Get, RefineMachineName_Hidden_Set);
            rm.PropertyChanged += RefineMachineName_ValueChanged;
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

    private void RefineMachineName_ValueChanged(object value, PropertyChangedEventArgs args)
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

    #region MachineCode (3)

    public byte? MachineCode_DB { 
        get; 
        set; 
    }

    public bool _MachineCode_Hidden_Get { get; set; } = true;

    [NotMapped]
    public RefBool MachineCode_Hidden_Get
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MachineCode_Hidden_Get), out var value))
            {
                ((RefBool)value).Set(_MachineCode_Hidden_Get);
                return (RefBool)value;

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
            if (Dictionary.TryGetValue(nameof(MachineCode_Hidden_Set), out var value))
            {
                ((RefBool)value).Set(_MachineCode_Hidden_Set);
                return (RefBool)value;

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
            if (Dictionary.TryGetValue(nameof(MachineCode), out var value))
            {
                ((RamAccess<byte?>)value).Value = MachineCode_DB;
                return (RamAccess<byte?>)value;
            }
            var rm = new RamAccess<byte?>(MachineCode_Validation, MachineCode_DB, MachineCode_Hidden_Get, MachineCode_Hidden_Set);
            rm.PropertyChanged += MachineCode_ValueChanged;
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

    private void MachineCode_ValueChanged(object value, PropertyChangedEventArgs args)
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

    #region MachinePower (4)

    public bool _MachinePower_Hidden_Get { get; set; } = true;

    [NotMapped]
    public RefBool MachinePower_Hidden_Get
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MachinePower_Hidden_Get), out var value))
            {
                ((RefBool)value).Set(_MachinePower_Hidden_Get);
                return (RefBool)value;

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
            if (Dictionary.TryGetValue(nameof(MachinePower_Hidden_Set), out var value))
            {
                ((RefBool)value).Set(_MachinePower_Hidden_Set);
                return (RefBool)value;

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
            if (Dictionary.TryGetValue(nameof(MachinePower), out var value))
            {
                ((RamAccess<string>)value).Value = MachinePower_DB;
                return (RamAccess<string>)value;

            }
            var rm = new RamAccess<string>(MachinePower_Validation, MachinePower_DB, MachinePower_Hidden_Get, MachinePower_Hidden_Set);
            rm.PropertyChanged += MachinePower_ValueChanged;
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

    private void MachinePower_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        MachinePower_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool MachinePower_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region NumberOfHoursPerYear (5)

    public string NumberOfHoursPerYear_DB { get; set; } = "";

    public bool _NumberOfHoursPerYear_Hidden_Get { get; set; } = true;

    [NotMapped]
    public RefBool NumberOfHoursPerYear_Hidden_Get
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(NumberOfHoursPerYear_Hidden_Get), out var value))
            {
                ((RefBool)value).Set(_NumberOfHoursPerYear_Hidden_Get);
                return (RefBool)value;

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
            if (Dictionary.TryGetValue(nameof(NumberOfHoursPerYear_Hidden_Set), out var value))
            {
                ((RefBool)value).Set(_NumberOfHoursPerYear_Hidden_Set);
                return (RefBool)value;

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
            if (Dictionary.TryGetValue(nameof(NumberOfHoursPerYear), out var value))
            {
                ((RamAccess<string>)value).Value = NumberOfHoursPerYear_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(NumberOfHoursPerYear_Validation, NumberOfHoursPerYear_DB, NumberOfHoursPerYear_Hidden_Get, NumberOfHoursPerYear_Hidden_Set);
            rm.PropertyChanged += NumberOfHoursPerYear_ValueChanged;
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

    private void NumberOfHoursPerYear_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        NumberOfHoursPerYear_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool NumberOfHoursPerYear_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region CodeRAOIn (6)

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
            if (Dictionary.TryGetValue(nameof(CodeRAOIn), out var value))
            {
                ((RamAccess<string>)value).Value = CodeRAOIn_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(CodeRAOIn_Validation, CodeRAOIn_DB);
            rm.PropertyChanged += CodeRAOIn_ValueChanged;
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

    private void CodeRAOIn_ValueChanged(object value, PropertyChangedEventArgs args)
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
        if (!CodeRaoRegex().IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmp.Length != 11) return true;
        if (!CodeRaoInRegex1().IsMatch(tmp[..1]))
        {
            value.AddError($"Недопустимое агрегатное состояние - {tmp[..1]}");
        }
        if (!CodeRaoInRegex2().IsMatch(tmp.AsSpan(1, 1)))
        {
            value.AddError($"Недопустимое категория РАО - {tmp.Substring(1, 1)}");
        }
        if (!CodeRaoInRegex3().IsMatch(tmp.AsSpan(2, 1)))
        {
            value.AddError($"Недопустимый радионуклидный состав РАО - {tmp.Substring(2, 1)}");
        }
        if (!CodeRaoInRegex4().IsMatch(tmp.AsSpan(3, 1)))
        {
            value.AddError($"Недопустимое содержание ядерных материалов - {tmp.Substring(3, 1)}");
        }
        if (!CodeRaoInRegex4().IsMatch(tmp.AsSpan(4, 1)))
        {
            value.AddError($"Недопустимый период полураспада - {tmp.Substring(4, 1)}");
        }
        if (!CodeRaoInRegex5().IsMatch(tmp.AsSpan(5, 1)))
        {
            value.AddError($"Недопустимый период потенциальной опасности РАО - {tmp.Substring(5, 1)}");
        }
        if (!CodeRaoInRegex2().IsMatch(tmp.AsSpan(6, 1)))
        {
            value.AddError($"Недопустимый способ переработки - {tmp.Substring(6, 1)}");
        }
        if (!CodeRaoInRegex6().IsMatch(tmp.AsSpan(7, 1)))
        {
            value.AddError($"Недопустимый класс РАО - {tmp.Substring(7, 1)}");
        }
        if (!CodeRaoInRegex7().IsMatch(tmp.AsSpan(8, 2)))
        {
            value.AddError($"Недопустимый код типа РАО - {tmp.Substring(8, 2)}");
        }
        if (!CodeRaoInRegex4().IsMatch(tmp.AsSpan(10, 1)))
        {
            value.AddError($"Недопустимая горючесть - {tmp.Substring(10, 1)}");
        }
        return !value.HasErrors;
    }

    #endregion

    #region  StatusRAOIn (7)

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
    public RamAccess<string> StatusRAOIn  //1 digit or OKPO.
    {
        get
        {
            if (StatusRAOIn_Hidden) return new RamAccess<string>(null, null);
            if (Dictionary.TryGetValue(nameof(StatusRAOIn), out var value))
            {
                ((RamAccess<string>)value).Value = StatusRAOIn_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(StatusRAOIn_Validation, StatusRAOIn_DB);
            rm.PropertyChanged += StatusRAOIn_ValueChanged;
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

    private void StatusRAOIn_ValueChanged(object value, PropertyChangedEventArgs args)
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
            || !OkpoRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region VolumeIn (8)

    public string VolumeIn_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "куб. м", "8")]
    public RamAccess<string> VolumeIn//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(VolumeIn), out var value))
            {
                ((RamAccess<string>)value).Value = VolumeIn_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(VolumeIn_Validation, VolumeIn_DB);
            rm.PropertyChanged += VolumeIn_ValueChanged;
            Dictionary.Add(nameof(VolumeIn), rm);
            return (RamAccess<string>)Dictionary[nameof(VolumeIn)];
        }
        set
        {
            VolumeIn_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void VolumeIn_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        VolumeIn_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool VolumeIn_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region MassIn (9)

    public string MassIn_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "т", "9")]
    public RamAccess<string> MassIn//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MassIn), out var value))
            {
                ((RamAccess<string>)value).Value = MassIn_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(MassIn_Validation, MassIn_DB);
            rm.PropertyChanged += MassIn_ValueChanged;
            Dictionary.Add(nameof(MassIn), rm);
            return (RamAccess<string>)Dictionary[nameof(MassIn)];
        }
        set
        {
            MassIn_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void MassIn_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        MassIn_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool MassIn_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region QuantityIn (10)

    public string QuantityIn_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "ОЗИИИ, шт", "10")]
    public RamAccess<string> QuantityIn//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(QuantityIn), out var value))
            {
                ((RamAccess<string>)value).Value = QuantityIn_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(QuantityIn_Validation, QuantityIn_DB);
            rm.PropertyChanged += QuantityIn_ValueChanged;
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

    private void QuantityIn_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            QuantityIn_DB = ((RamAccess<string>)value).Value?.Trim();
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

    #region TritiumActivityIn (11)

    public string TritiumActivityIn_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "тритий", "11")]
    public RamAccess<string> TritiumActivityIn//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(TritiumActivityIn), out var value))
            {
                ((RamAccess<string>)value).Value = TritiumActivityIn_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(TritiumActivityIn_Validation, TritiumActivityIn_DB);
            rm.PropertyChanged += TritiumActivityIn_ValueChanged;
            Dictionary.Add(nameof(TritiumActivityIn), rm);
            return (RamAccess<string>)Dictionary[nameof(TritiumActivityIn)];
        }
        set
        {
            TritiumActivityIn_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TritiumActivityIn_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        TritiumActivityIn_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool TritiumActivityIn_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region BetaGammaActivityIn (12)

    public string BetaGammaActivityIn_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "12")]
    public RamAccess<string> BetaGammaActivityIn//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(BetaGammaActivityIn), out var value))
            {
                ((RamAccess<string>)value).Value = BetaGammaActivityIn_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(BetaGammaActivityIn_Validation, BetaGammaActivityIn_DB);
            rm.PropertyChanged += BetaGammaActivityIn_ValueChanged;
            Dictionary.Add(nameof(BetaGammaActivityIn), rm);
            return (RamAccess<string>)Dictionary[nameof(BetaGammaActivityIn)];
        }
        set
        {
            BetaGammaActivityIn_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void BetaGammaActivityIn_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        BetaGammaActivityIn_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool BetaGammaActivityIn_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region AlphaActivityIn (13)

    public string AlphaActivityIn_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "альфа-излучающие радионуклиды (исключая трансурановые)", "13")]
    public RamAccess<string> AlphaActivityIn//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(AlphaActivityIn), out var value))
            {
                ((RamAccess<string>)value).Value = AlphaActivityIn_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(AlphaActivityIn_Validation, AlphaActivityIn_DB);
            rm.PropertyChanged += AlphaActivityIn_ValueChanged;
            Dictionary.Add(nameof(AlphaActivityIn), rm);
            return (RamAccess<string>)Dictionary[nameof(AlphaActivityIn)];
        }
        set
        {
            AlphaActivityIn_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void AlphaActivityIn_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        AlphaActivityIn_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool AlphaActivityIn_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region TransuraniumActivityIn (14)

    public string TransuraniumActivityIn_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Поступило РАО на переработку, кондиционирование", "трансурановые радионуклиды", "14")]
    public RamAccess<string> TransuraniumActivityIn//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(TransuraniumActivityIn), out var value))
            {
                ((RamAccess<string>)value).Value = TransuraniumActivityIn_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(TransuraniumActivityIn_Validation, TransuraniumActivityIn_DB);
            rm.PropertyChanged += TransuraniumActivityIn_ValueChanged;
            Dictionary.Add(nameof(TransuraniumActivityIn), rm);
            return (RamAccess<string>)Dictionary[nameof(TransuraniumActivityIn)];
        }
        set
        {
            TransuraniumActivityIn_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TransuraniumActivityIn_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        TransuraniumActivityIn_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool TransuraniumActivityIn_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region CodeRAOout (15)

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
            if (Dictionary.TryGetValue(nameof(CodeRAOout), out var value))
            {
                ((RamAccess<string>)value).Value = CodeRAOout_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(CodeRAOout_Validation, CodeRAOout_DB);
            rm.PropertyChanged += CodeRAOout_ValueChanged;
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

    private void CodeRAOout_ValueChanged(object value, PropertyChangedEventArgs args)
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
        if (!CodeRaoRegex().IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }

        if (tmp.Length != 11) return true;
        if (!CodeRaoInRegex1().IsMatch(tmp[..1]))
        {
            value.AddError($"Недопустимое агрегатное состояние - {tmp[..1]}");
        }
        if (!CodeRaoInRegex2().IsMatch(tmp.AsSpan(1, 1)))
        {
            value.AddError($"Недопустимое категория РАО - {tmp.Substring(1, 1)}");
        }
        if (!CodeRaoInRegex3().IsMatch(tmp.AsSpan(2, 1)))
        {
            value.AddError($"Недопустимый радионуклидный состав РАО - {tmp.Substring(2, 1)}");
        }
        if (!CodeRaoInRegex4().IsMatch(tmp.AsSpan(3, 1)))
        {
            value.AddError($"Недопустимое содержание ядерных материалов - {tmp.Substring(3, 1)}");
        }
        if (!CodeRaoInRegex4().IsMatch(tmp.AsSpan(4, 1)))
        {
            value.AddError($"Недопустимый период полураспада - {tmp.Substring(4, 1)}");
        }
        if (!CodeRaoInRegex5().IsMatch(tmp.AsSpan(5, 1)))
        {
            value.AddError($"Недопустимый период потенциальной опасности РАО - {tmp.Substring(5, 1)}");
        }
        if (!CodeRaoInRegex2().IsMatch(tmp.AsSpan(6, 1)))
        {
            value.AddError($"Недопустимый способ переработки - {tmp.Substring(6, 1)}");
        }
        if (!CodeRaoInRegex6().IsMatch(tmp.AsSpan(7, 1)))
        {
            value.AddError($"Недопустимый класс РАО - {tmp.Substring(7, 1)}");
        }
        if (!CodeRaoInRegex7().IsMatch(tmp.AsSpan(8, 2)))
        {
            value.AddError($"Недопустимый код типа РАО - {tmp.Substring(8, 2)}");
        }
        if (!CodeRaoInRegex4().IsMatch(tmp.AsSpan(10, 1)))
        {
            value.AddError($"Недопустимая горючесть - {tmp.Substring(10, 1)}");
        }
        return !value.HasErrors;
    }

    #endregion

    #region StatusRAOout (16)

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
    public RamAccess<string> StatusRAOout  //1 digit or OKPO.
    {
        get
        {
            if (!StatusRAOout_Hidden)
            {
                if (Dictionary.TryGetValue(nameof(StatusRAOout), out var value))
                {
                    ((RamAccess<string>)value).Value = StatusRAOout_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(StatusRAOout_Validation, StatusRAOout_DB);
                rm.PropertyChanged += StatusRAOout_ValueChanged;
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

    private void StatusRAOout_ValueChanged(object value, PropertyChangedEventArgs args)
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
            if (!int.TryParse(value.Value, out var tmpInt)
                || tmpInt > 4 && tmpInt != 6 && tmpInt != 9)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        if (value.Value.Length != 8 && value.Value.Length != 14
            || !OkpoRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region VolumeOut (17)

    public string VolumeOut_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "куб. м", "17")]
    public RamAccess<string> VolumeOut//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(VolumeOut), out var value))
            {
                ((RamAccess<string>)value).Value = VolumeOut_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(VolumeOut_Validation, VolumeOut_DB);
            rm.PropertyChanged += VolumeOut_ValueChanged;
            Dictionary.Add(nameof(VolumeOut), rm);
            return (RamAccess<string>)Dictionary[nameof(VolumeOut)];
        }
        set
        {
            VolumeOut_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void VolumeOut_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        VolumeOut_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool VolumeOut_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region MassOut (18)

    public string MassOut_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "т", "18")]
    public RamAccess<string> MassOut//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MassOut), out var value))
            {
                ((RamAccess<string>)value).Value = MassOut_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(MassOut_Validation, MassOut_DB);
            rm.PropertyChanged += MassOut_ValueChanged;
            Dictionary.Add(nameof(MassOut), rm);
            return (RamAccess<string>)Dictionary[nameof(MassOut)];
        }
        set
        {
            MassOut_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void MassOut_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        MassOut_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool MassOut_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region QuantityOZIIIout (19)

    public string QuantityOZIIIout_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "ОЗИИИ, шт", "19")]
    public RamAccess<string> QuantityOZIIIout//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(QuantityOZIIIout), out var value))
            {
                ((RamAccess<string>)value).Value = QuantityOZIIIout_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(QuantityOZIIIout_Validation, QuantityOZIIIout_DB);
            rm.PropertyChanged += QuantityOZIIIout_ValueChanged;
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

    private void QuantityOZIIIout_ValueChanged(object value, PropertyChangedEventArgs args)
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
        if (!int.TryParse(tmp, StyleDecimalThousandExp, CultureInfo.CreateSpecificCulture("ru-RU"), out var tmpInt))
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

    #region TritiumActivityOut (20)

    public string TritiumActivityOut_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "тритий", "20")]
    public RamAccess<string> TritiumActivityOut//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(TritiumActivityOut), out var value))
            {
                ((RamAccess<string>)value).Value = TritiumActivityOut_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(TritiumActivityOut_Validation, TritiumActivityOut_DB);
            rm.PropertyChanged += TritiumActivityOut_ValueChanged;
            Dictionary.Add(nameof(TritiumActivityOut), rm);
            return (RamAccess<string>)Dictionary[nameof(TritiumActivityOut)];
        }
        set
        {
            TritiumActivityOut_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TritiumActivityOut_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        TritiumActivityOut_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool TritiumActivityOut_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region BetaGammaActivityOut (21)

    public string BetaGammaActivityOut_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "21")]
    public RamAccess<string> BetaGammaActivityOut//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(BetaGammaActivityOut), out var value))
            {
                ((RamAccess<string>)value).Value = BetaGammaActivityOut_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(BetaGammaActivityOut_Validation, BetaGammaActivityOut_DB);
            rm.PropertyChanged += BetaGammaActivityOut_ValueChanged;
            Dictionary.Add(nameof(BetaGammaActivityOut), rm);
            return (RamAccess<string>)Dictionary[nameof(BetaGammaActivityOut)];
        }
        set
        {
            BetaGammaActivityOut_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void BetaGammaActivityOut_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        BetaGammaActivityOut_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool BetaGammaActivityOut_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region AlphaActivityOut (22)

    public string AlphaActivityOut_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "альфа-излучающие радионуклиды (исключая трансурановые)", "22")]
    public RamAccess<string> AlphaActivityOut //SUMMARIZABLE
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(AlphaActivityOut), out var value))
            {
                ((RamAccess<string>)value).Value = AlphaActivityOut_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(AlphaActivityOut_Validation, AlphaActivityOut_DB);
            rm.PropertyChanged += AlphaActivityOut_ValueChanged;
            Dictionary.Add(nameof(AlphaActivityOut), rm);
            return (RamAccess<string>)Dictionary[nameof(AlphaActivityOut)];
        }
        set
        {
            AlphaActivityOut_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void AlphaActivityOut_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        AlphaActivityOut_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool AlphaActivityOut_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region TransuraniumActivityOut (23)

    public string TransuraniumActivityOut_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Образовалось РАО после переработки, кондиционирования", "трансурановые радионуклиды", "23")]
    public RamAccess<string> TransuraniumActivityOut//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(TransuraniumActivityOut), out var value))
            {
                ((RamAccess<string>)value).Value = TransuraniumActivityOut_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(TransuraniumActivityOut_Validation, TransuraniumActivityOut_DB);
            rm.PropertyChanged += TransuraniumActivityOut_ValueChanged;
            Dictionary.Add(nameof(TransuraniumActivityOut), rm);
            return (RamAccess<string>)Dictionary[nameof(TransuraniumActivityOut)];
        }
        set
        {
            TransuraniumActivityOut_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TransuraniumActivityOut_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        TransuraniumActivityOut_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool TransuraniumActivityOut_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion 

    #endregion

    #region IExcel

    public new void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        base.ExcelGetRow(worksheet, row);
        RefineMachineName.Value = Convert.ToString(worksheet.Cells[row, 2].Value);
        MachineCode.Value = byte.TryParse(worksheet.Cells[row, 3].Value.ToString(), out var byteVal)
            ? byteVal
            : null;
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(NumberInOrderSum))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(RefineMachineName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(MachineCode))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(MachinePower))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(NumberOfHoursPerYear))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(CodeRAOIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(StatusRAOIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(VolumeIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(MassIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(QuantityIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(TritiumActivityIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(BetaGammaActivityIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(AlphaActivityIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(TransuraniumActivityIn))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(CodeRAOout))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(StatusRAOout))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(VolumeOut))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(MassOut))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(QuantityOZIIIout))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(TritiumActivityOut))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(BetaGammaActivityOut))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(AlphaActivityOut))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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
            ((FormPropertyAttribute)typeof(Form21)
                .GetProperty(nameof(TransuraniumActivityOut))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
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

    #region GeneratedRegex
    
    [GeneratedRegex("^[0-9x+]{11}$")]
    private static partial Regex CodeRaoRegex();

    [GeneratedRegex("^[1-3x+]")]
    private static partial Regex CodeRaoInRegex1();

    [GeneratedRegex("^[0-49x+]")]
    private static partial Regex CodeRaoInRegex2();

    [GeneratedRegex("^[0-6x+]")]
    private static partial Regex CodeRaoInRegex3();

    [GeneratedRegex("^[12x+]")]
    private static partial Regex CodeRaoInRegex4();

    [GeneratedRegex("^[0-3x+]")]
    private static partial Regex CodeRaoInRegex5();

    [GeneratedRegex("^[0-79x+]")]
    private static partial Regex CodeRaoInRegex6();

    [GeneratedRegex("^[1]{1}[1-9]{1}|^[0]{1}[1]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1-9]{1}|^[4]{1}[1-6]{1}|^[5]{1}[1-9]{1}|^[6]{1}[1-9]{1}|^[7]{1}[1-9]{1}|^[8]{1}[1-9]{1}|^[9]{1}[1-9]{1}")]
    private static partial Regex CodeRaoInRegex7();

    #endregion

    #region ConvertToTSVstring

    /// <summary>
    /// </summary>
    /// <returns>Возвращает строку с записанными данными в формате TSV(Tab-Separated Values) </returns>
    public override string ConvertToTSVstring()
    {
        // Создаем текстовое представление (TSV - tab-separated values)
        var str =
            $"{NumberInOrder.Value}\t" +
            $"{RefineMachineName.Value}\t" +
            $"{MachineCode.Value}\t" +
            $"{MachinePower.Value}\t" +
            $"{NumberOfHoursPerYear.Value}\t" +
            $"{CodeRAOIn.Value}\t" +
            $"{StatusRAOIn.Value}\t" +
            $"{VolumeIn.Value}\t" +
            $"{MassIn.Value}\t" +
            $"{QuantityIn.Value}\t" +
            $"{TritiumActivityIn.Value}\t" +
            $"{BetaGammaActivityIn.Value}\t" +
            $"{AlphaActivityIn.Value}\t" +
            $"{TransuraniumActivityIn.Value}\t" +
            $"{CodeRAOout.Value}\t" +
            $"{StatusRAOout.Value}\t" +
            $"{VolumeOut.Value}\t" +
            $"{MassOut.Value}\t" +
            $"{QuantityOZIIIout.Value}\t" +
            $"{TritiumActivityOut.Value}\t" +
            $"{BetaGammaActivityOut.Value}\t" +
            $"{TransuraniumActivityOut.Value}";
        return str;
    }

    #endregion
}