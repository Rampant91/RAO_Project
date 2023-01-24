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
using Spravochniki;

namespace Models.Forms.Form2;

[Serializable]
[Form_Class("Форма 2.2: Наличие РАО в пунктах хранения, местах сбора и/или временного хранения")]
public class Form22 : Form2, IBaseColor
{
    #region Constructor
    public Form22()
    {
        FormNum.Value = "2.2";
        //NumberOfFields.Value = 25;
        Validate_all();
    }
    #endregion

    #region Validation
    private void Validate_all()
    {
        StoragePlaceName_Validation(StoragePlaceName);
        StoragePlaceCode_Validation(StoragePlaceCode);
        PackName_Validation(PackName);
        PackType_Validation(PackType);
        CodeRAO_Validation(CodeRAO);
        StatusRAO_Validation(StatusRAO);
        VolumeOutOfPack_Validation(VolumeOutOfPack);
        MassInPack_Validation(MassInPack);
        QuantityOZIII_Validation(QuantityOZIII);
        TritiumActivity_Validation(TritiumActivity);
        BetaGammaActivity_Validation(BetaGammaActivity);
        TransuraniumActivity_Validation(TransuraniumActivity);
        AlphaActivity_Validation(AlphaActivity);
        VolumeInPack_Validation(VolumeInPack);
        MassOutOfPack_Validation(MassOutOfPack);
        MainRadionuclids_Validation(MainRadionuclids);
        Subsidy_Validation(Subsidy);
        FcpNumber_Validation(FcpNumber);
        PackQuantity_Validation(PackQuantity);
    }

    [FormProperty(true, "Форма")]
    public override bool Object_Validation()
    {
        return !(StoragePlaceName.HasErrors ||
                 StoragePlaceCode.HasErrors ||
                 PackName.HasErrors ||
                 PackType.HasErrors ||
                 CodeRAO.HasErrors ||
                 StatusRAO.HasErrors ||
                 VolumeOutOfPack.HasErrors ||
                 MassInPack.HasErrors ||
                 QuantityOZIII.HasErrors ||
                 TritiumActivity.HasErrors ||
                 BetaGammaActivity.HasErrors ||
                 TransuraniumActivity.HasErrors ||
                 AlphaActivity.HasErrors ||
                 VolumeInPack.HasErrors ||
                 MassOutOfPack.HasErrors ||
                 MainRadionuclids.HasErrors ||
                 Subsidy.HasErrors ||
                 FcpNumber.HasErrors ||
                 PackQuantity.HasErrors);
    } 
    #endregion

    #region BaseColor
    public ColorType _BaseColor { get; set; } = ColorType.None;
    [NotMapped]
    public ColorType BaseColor
    {

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
            if (Dictionary.ContainsKey(nameof(Sum)))
            {
                ((RamAccess<bool>)Dictionary[nameof(Sum)]).Value = Sum_DB;
                return (RamAccess<bool>)Dictionary[nameof(Sum)];
            }
            var rm = new RamAccess<bool>(Sum_Validation, Sum_DB);
            rm.PropertyChanged += SumValueChanged;
            Dictionary.Add(nameof(Sum), rm);
            return (RamAccess<bool>)Dictionary[nameof(Sum)];
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

    #region  SumGroup
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

    private void NumberInOrderSumValueChanged(object value, PropertyChangedEventArgs args)
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

    #region StoragePlaceName_2

    public string StoragePlaceName_DB { get; set; } = "";
    public bool _StoragePlaceName_Hidden_Get { get; set; } = true;
    [NotMapped]
    public RefBool StoragePlaceName_Hidden_Get
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(StoragePlaceName_Hidden_Get)))
            {
                ((RefBool)Dictionary[nameof(StoragePlaceName_Hidden_Get)]).Set(_StoragePlaceName_Hidden_Get);
                return (RefBool)Dictionary[nameof(StoragePlaceName_Hidden_Get)];
            }
            var rm = new RefBool(_StoragePlaceName_Hidden_Get);
            Dictionary.Add(nameof(StoragePlaceName_Hidden_Get), rm);
            return rm;
        }
        set
        {
            if (_StoragePlaceName_Hidden_Get == value.Get()) return;
            _StoragePlaceName_Hidden_Get = value.Get();
            OnPropertyChanged();
        }
    }
    public bool _StoragePlaceName_Hidden_Set { get; set; } = true;
    [NotMapped]
    public RefBool StoragePlaceName_Hidden_Set
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(StoragePlaceName_Hidden_Set)))
            {
                ((RefBool)Dictionary[nameof(StoragePlaceName_Hidden_Set)]).Set(_StoragePlaceName_Hidden_Set);
                return (RefBool)Dictionary[nameof(StoragePlaceName_Hidden_Set)];

            }
            var rm = new RefBool(_StoragePlaceName_Hidden_Set);
            Dictionary.Add(nameof(StoragePlaceName_Hidden_Set), rm);
            return rm;
        }
        set
        {
            if (_StoragePlaceName_Hidden_Set == value.Get()) return;
            _StoragePlaceName_Hidden_Set = value.Get();
            OnPropertyChanged();
        }
    }

    [NotMapped]
    [FormProperty(true, "Пункт хранения", "наименование", "2")]
    public RamAccess<string> StoragePlaceName
    {
        get
        {

            if (Dictionary.ContainsKey(nameof(StoragePlaceName)))
            {
                ((RamAccess<string>)Dictionary[nameof(StoragePlaceName)]).Value = StoragePlaceName_DB;
                return (RamAccess<string>)Dictionary[nameof(StoragePlaceName)];
            }
            var rm = new RamAccess<string>(StoragePlaceName_Validation, StoragePlaceName_DB, StoragePlaceName_Hidden_Get, StoragePlaceName_Hidden_Set);
            rm.PropertyChanged += StoragePlaceNameValueChanged;
            Dictionary.Add(nameof(StoragePlaceName), rm);
            return (RamAccess<string>)Dictionary[nameof(StoragePlaceName)];
        }
        set
        {
            if (StoragePlaceName.Value == value.Value) return;
            StoragePlaceName_DB = value.Value;
            OnPropertyChanged();
        }
    }
    //If change this change validation

    private void StoragePlaceNameValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        if (_StoragePlaceName_Hidden_Set)
        {
            StoragePlaceName_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool StoragePlaceName_Validation(RamAccess<string> value) //Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }

        //            var spr = new List<string>();//here binds spr
        //            if (!spr.Contains(value.Value))
        //            {
        //                value.AddError( "Недопустимое значение");
        //return false;
        //            }
        return true;
    }
    #endregion

    #region StoragePlaceCode_3

    public string StoragePlaceCode_DB { get; set; } = "";
    public bool _StoragePlaceCode_Hidden_Get { get; set; } = true;
    [NotMapped]
    public RefBool StoragePlaceCode_Hidden_Get
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(StoragePlaceCode_Hidden_Get)))
            {
                ((RefBool)Dictionary[nameof(StoragePlaceCode_Hidden_Get)]).Set(_StoragePlaceCode_Hidden_Get);
                return (RefBool)Dictionary[nameof(StoragePlaceCode_Hidden_Get)];
            }
            var rm = new RefBool(_StoragePlaceCode_Hidden_Get);
            Dictionary.Add(nameof(StoragePlaceCode_Hidden_Get), rm);
            return rm;
        }
        set
        {
            if (_StoragePlaceCode_Hidden_Get == value.Get()) return;
            _StoragePlaceCode_Hidden_Get = value.Get();
            OnPropertyChanged();
        }
    }

    public bool _StoragePlaceCode_Hidden_Set { get; set; } = true;
    [NotMapped]
    public RefBool StoragePlaceCode_Hidden_Set
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(StoragePlaceCode_Hidden_Set)))
            {
                ((RefBool)Dictionary[nameof(StoragePlaceCode_Hidden_Set)]).Set(_StoragePlaceCode_Hidden_Set);
                return (RefBool)Dictionary[nameof(StoragePlaceCode_Hidden_Set)];

            }
            var rm = new RefBool(_StoragePlaceCode_Hidden_Set);
            Dictionary.Add(nameof(StoragePlaceCode_Hidden_Set), rm);
            return rm;
        }
        set
        {
            if (_StoragePlaceCode_Hidden_Set == value.Get()) return;
            _StoragePlaceCode_Hidden_Set = value.Get();
            OnPropertyChanged();
        }
    }

    [NotMapped]
    [FormProperty(true, "Пункт хранения", "код", "3")]
    public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(StoragePlaceCode)))
            {
                ((RamAccess<string>)Dictionary[nameof(StoragePlaceCode)]).Value = StoragePlaceCode_DB;
                return (RamAccess<string>)Dictionary[nameof(StoragePlaceCode)];
            }
            var rm = new RamAccess<string>(StoragePlaceCode_Validation, StoragePlaceCode_DB, StoragePlaceCode_Hidden_Get, StoragePlaceCode_Hidden_Set);
            rm.PropertyChanged += StoragePlaceCodeValueChanged;
            Dictionary.Add(nameof(StoragePlaceCode), rm);
            return (RamAccess<string>)Dictionary[nameof(StoragePlaceCode)];
        }
        set
        {
            if (StoragePlaceCode.Value == value.Value) return;
            StoragePlaceCode_DB = value.Value;
            OnPropertyChanged();
        }
    }
    //if change this change validation

    private void StoragePlaceCodeValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        if (_StoragePlaceCode_Hidden_Set)
        {
            StoragePlaceCode_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool StoragePlaceCode_Validation(RamAccess<string> value) //TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        //var spr = new List<string>();//here binds spr
        //if (!spr.Contains(value.Value))
        //{
        //    value.AddError("Недопустимое значение");
        //    return false;
        //}
        //return true;
        if (value.Value == "-") return true;
        Regex a = new("^[0-9]{8}$");
        if (!a.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        var tmp = value.Value;
        if (tmp.Length == 8)
        {
            if (!new Regex("^[1-9]").IsMatch(tmp[..1]))
            {
                value.AddError($"Недопустимый вид пункта - {tmp[..1]}");
            }
            if (!new Regex("^[1-3]").IsMatch(tmp.Substring(1, 1)))
            {
                value.AddError($"Недопустимое состояние пункта - {tmp.Substring(1, 1)}");
            }
            if (!new Regex("^[1-2]").IsMatch(tmp.Substring(2, 1)))
            {
                value.AddError($"Недопустимая изоляция от окружающей среды - {tmp.Substring(2, 1)}");
            }
            if (!new Regex("^[1-59]").IsMatch(tmp.Substring(3, 1)))
            {
                value.AddError($"Недопустимая зона нахождения пункта - {tmp.Substring(3, 1)}");
            }
            if (!new Regex("^[0-4]").IsMatch(tmp.Substring(4, 1)))
            {
                value.AddError($"Недопустимое значение пункта - {tmp.Substring(4, 1)}");
            }
            if (!new Regex("^[1-49]").IsMatch(tmp.Substring(5, 1)))
            {
                value.AddError($"Недопустимое размещение пункта хранения относительно поверхности земли - {tmp.Substring(5, 1)}");
            }
            Regex a67 = new("^[1]{1}[1-9]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1]{1}|^[4]{1}[1-49]{1}|^[5]{1}[1-69]{1}|^[6]{1}[1]{1}|^[7]{1}[1349]{1}|^[8]{1}[1-69]{1}|^[9]{1}[9]{1}");
            if (!a67.IsMatch(tmp.Substring(6, 2)))
            {
                value.AddError($"Недопустимый код типа РАО - {tmp.Substring(6, 2)}");
            }
            if (value.HasErrors)
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region PackName_4

    public string PackName_DB { get; set; } = "";
    public bool _PackName_Hidden_Get { get; set; } = true;
    [NotMapped]
    public RefBool PackName_Hidden_Get
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PackName_Hidden_Get)))
            {
                ((RefBool)Dictionary[nameof(PackName_Hidden_Get)]).Set(_PackName_Hidden_Get);
                return (RefBool)Dictionary[nameof(PackName_Hidden_Get)];
            }
            var rm = new RefBool(_PackName_Hidden_Get);
            Dictionary.Add(nameof(PackName_Hidden_Get), rm);
            return rm;
        }
        set
        {
            if (_PackName_Hidden_Get == value.Get()) return;
            _PackName_Hidden_Get = value.Get();
            OnPropertyChanged();
        }
    }

    public bool _PackName_Hidden_Set { get; set; } = true;
    [NotMapped]
    public RefBool PackName_Hidden_Set
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PackName_Hidden_Set)))
            {
                ((RefBool)Dictionary[nameof(PackName_Hidden_Set)]).Set(_PackName_Hidden_Set);
                return (RefBool)Dictionary[nameof(PackName_Hidden_Set)];

            }
            var rm = new RefBool(_PackName_Hidden_Set);
            Dictionary.Add(nameof(PackName_Hidden_Set), rm);
            return rm;
        }
        set
        {
            if (_PackName_Hidden_Set == value.Get()) return;
            _PackName_Hidden_Set = value.Get();
            OnPropertyChanged();
        }
    }

    [NotMapped]
    [FormProperty(true, "УКТ, упаковка ли иная учетная единица", "наименование", "4")]
    public RamAccess<string> PackName
    {
        get
        {

            if (Dictionary.ContainsKey(nameof(PackName)))
            {
                ((RamAccess<string>)Dictionary[nameof(PackName)]).Value = PackName_DB;
                return (RamAccess<string>)Dictionary[nameof(PackName)];
            }
            var rm = new RamAccess<string>(PackName_Validation, PackName_DB, PackName_Hidden_Get, PackName_Hidden_Set);
            rm.PropertyChanged += PackNameValueChanged;
            Dictionary.Add(nameof(PackName), rm);
            return (RamAccess<string>)Dictionary[nameof(PackName)];
        }
        set
        {
            if (PackName.Value != value.Value) return;
            PackName_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void PackNameValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        if (_PackName_Hidden_Set)
        {
            PackName_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool PackName_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        //if (value.Value is "без упаковки")
        //{
        //    return true;
        //}
        //            var spr = new List<string>(); //here binds spr
        //            if (!spr.Contains(value.Value))
        //            {
        //                value.AddError( "Недопустимое значение");
        //return false;
        //            }
        return true;
    }
    #endregion

    #region PackType_5

    public string PackType_DB { get; set; } = "";
    public bool _PackType_Hidden_Get { get; set; } = true;
    [NotMapped]
    public RefBool PackType_Hidden_Get
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PackType_Hidden_Get)))
            {
                ((RefBool)Dictionary[nameof(PackType_Hidden_Get)]).Set(_PackType_Hidden_Get);
                return (RefBool)Dictionary[nameof(PackType_Hidden_Get)];

            }
            var rm = new RefBool(_PackType_Hidden_Get);
            Dictionary.Add(nameof(PackType_Hidden_Get), rm);
            return rm;
        }
        set
        {
            if (_PackType_Hidden_Get == value.Get()) return;
            _PackType_Hidden_Get = value.Get();
            OnPropertyChanged();
        }
    }
    public bool _PackType_Hidden_Set { get; set; } = true;
    [NotMapped]
    public RefBool PackType_Hidden_Set
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PackType_Hidden_Set)))
            {
                ((RefBool)Dictionary[nameof(PackType_Hidden_Set)]).Set(_PackType_Hidden_Set);
                return (RefBool)Dictionary[nameof(PackType_Hidden_Set)];
            }
            var rm = new RefBool(_PackType_Hidden_Set);
            Dictionary.Add(nameof(PackType_Hidden_Set), rm);
            return rm;
        }
        set
        {
            if (_PackType_Hidden_Set == value.Get()) return;
            _PackType_Hidden_Set = value.Get();
            OnPropertyChanged();
        }
    }

    [NotMapped]
    [FormProperty(true, "УКТ, упаковка ли иная учетная единица", "тип", "5")]
    public RamAccess<string> PackType
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PackType)))
            {
                ((RamAccess<string>)Dictionary[nameof(PackType)]).Value = PackType_DB;
                return (RamAccess<string>)Dictionary[nameof(PackType)];
            }
            var rm = new RamAccess<string>(PackType_Validation, PackType_DB, PackType_Hidden_Get, PackType_Hidden_Set);
            rm.PropertyChanged += PackTypeValueChanged;
            Dictionary.Add(nameof(PackType), rm);
            return (RamAccess<string>)Dictionary[nameof(PackType)];
        }
        set
        {
            if (PackType.Value == value.Value) return;
            PackType_DB = value.Value;
            OnPropertyChanged();
        }
    }
    //If change this change validation

    private void PackTypeValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        if (_PackType_Hidden_Set)
        {
            PackType_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool PackType_Validation(RamAccess<string> value) //Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }

        //if (value.Equals("прим."))
        //{
        //    //if ((PackTypeNote == null) || PackTypeNote.Equals(""))
        //    //    value.AddError( "Заполните примечание");
        //    return true;
        //}

        //var spr = new List<string>();
        //if (!spr.Contains(value.Value))
        //{
        //    value.AddError("Недопустимое значение");
        //    return false;
        //}

        return true;
    }
    #endregion

    #region PackQuantity_6

    public string PackQuantity_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "УКТ, упаковка ли иная учетная единица", "количество, шт", "6")]
    public RamAccess<string> PackQuantity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PackQuantity)))
            {
                ((RamAccess<string>)Dictionary[nameof(PackQuantity)]).Value = PackQuantity_DB;
                return (RamAccess<string>)Dictionary[nameof(PackQuantity)];
            }
            else
            {
                var rm = new RamAccess<string>(PackQuantity_Validation, PackQuantity_DB);
                rm.PropertyChanged += PackQuantityValueChanged;
                Dictionary.Add(nameof(PackQuantity), rm);
                return (RamAccess<string>)Dictionary[nameof(PackQuantity)];
            }
        }
        set
        {
            PackQuantity_DB = value.Value;
            OnPropertyChanged(nameof(PackQuantity));
        }
    }
    // positive int.

    private void PackQuantityValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PackQuantity_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool PackQuantity_Validation(RamAccess<string> value) //Ready
    {
        value.ClearErrors();
        switch (value.Value)
        {
            case null:
                value.AddError("Поле не заполнено");
                return false;
            case "-":
                return true;
            default:
                if (!int.TryParse(value.Value, out var tmpInt) || tmpInt <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
                return true;
        }
    }
    #endregion

    #region CodeRAO_7

    public string CodeRAO_DB { get; set; } = "";
    public bool CodeRAO_Hidden_Priv { get; set; }

    [NotMapped]
    public bool CodeRAO_Hidden
    {
        get => CodeRAO_Hidden_Priv;
        set => CodeRAO_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "null-7", "Код РАО", "7")]
    public RamAccess<string> CodeRAO
    {
        get
        {
            if (!CodeRAO_Hidden)
            {
                if (Dictionary.ContainsKey(nameof(CodeRAO)))
                {
                    ((RamAccess<string>)Dictionary[nameof(CodeRAO)]).Value = CodeRAO_DB;
                    return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
                }
                var rm = new RamAccess<string>(CodeRAO_Validation, CodeRAO_DB);
                rm.PropertyChanged += CodeRAOValueChanged;
                Dictionary.Add(nameof(CodeRAO), rm);
                return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
            }
            else
            {
                if (Dictionary.ContainsKey(nameof(CodeRAO)))
                {
                    return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
                }
                else
                {
                    var rm = new RamAccess<string>(null, null);
                    Dictionary.Add(nameof(CodeRAO), rm);
                    return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
                }
            }
        }

        set
        {
            if (CodeRAO_Hidden) return;
            CodeRAO_DB = value.Value;
            OnPropertyChanged();
        }
    }
    private void CodeRAOValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        CodeRAO_DB = ((RamAccess<string>)value).Value.ToLower().Replace("х", "x");
    }
    private bool CodeRAO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
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

    #region StatusRAO_8
    public string StatusRAO_DB { get; set; } = "";
    public bool StatusRAO_Hidden_Priv { get; set; }

    [NotMapped]
    public bool StatusRAO_Hidden
    {
        get => StatusRAO_Hidden_Priv;
        set => StatusRAO_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "null-8", "Статус РАО", "8")]
    public RamAccess<string> StatusRAO  //1 cyfer or OKPO.
    {
        get
        {
            if (!StatusRAO_Hidden)
            {
                if (Dictionary.ContainsKey(nameof(StatusRAO)))
                {
                    ((RamAccess<string>)Dictionary[nameof(StatusRAO)]).Value = StatusRAO_DB;
                    return (RamAccess<string>)Dictionary[nameof(StatusRAO)];
                }
                else
                {
                    var rm = new RamAccess<string>(StatusRAO_Validation, StatusRAO_DB);
                    rm.PropertyChanged += StatusRAOValueChanged;
                    Dictionary.Add(nameof(StatusRAO), rm);
                    return (RamAccess<string>)Dictionary[nameof(StatusRAO)];
                }
            }
            else
            {
                if (Dictionary.ContainsKey(nameof(StatusRAO)))
                {
                    return (RamAccess<string>)Dictionary[nameof(StatusRAO)];
                }
                else
                {
                    var rm = new RamAccess<string>(null, null);
                    Dictionary.Add(nameof(StatusRAO), rm);
                    return (RamAccess<string>)Dictionary[nameof(StatusRAO)];
                }
            }
        }
        set
        {
            if (StatusRAO_Hidden) return;
            StatusRAO_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void StatusRAOValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            StatusRAO_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool StatusRAO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
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

    #region VolumeOutOfPack_9
    public string VolumeOutOfPack_DB { get; set; } = ""; 
    [NotMapped]
    [FormProperty(true, "Объем, куб. м", "РАО без упаковки", "9")]
    public RamAccess<string> VolumeOutOfPack//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(VolumeOutOfPack)))
            {
                ((RamAccess<string>)Dictionary[nameof(VolumeOutOfPack)]).Value = VolumeOutOfPack_DB;
                return (RamAccess<string>)Dictionary[nameof(VolumeOutOfPack)];
            }
            var rm = new RamAccess<string>(VolumeOutOfPack_Validation, VolumeOutOfPack_DB);
            rm.PropertyChanged += VolumeOutOfPackValueChanged;
            Dictionary.Add(nameof(VolumeOutOfPack), rm);
            return (RamAccess<string>)Dictionary[nameof(VolumeOutOfPack)];
        }
        set
        {
            VolumeOutOfPack_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void VolumeOutOfPackValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                VolumeOutOfPack_DB = value1;
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
        VolumeOutOfPack_DB = value1;
    }

    private bool VolumeOutOfPack_Validation(RamAccess<string> value)//TODO
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

    #region VolumeInPack_10
    public string VolumeInPack_DB { get; set; } = "";
    public bool VolumeInPack_Hidden_Priv { get; set; }

    [NotMapped]
    public bool VolumeInPack_Hidden
    {
        get => VolumeInPack_Hidden_Priv;
        set => VolumeInPack_Hidden_Priv = value;
    }
    public bool VolumeInPack_Hidden_Priv2 { get; set; }

    [NotMapped]
    public bool VolumeInPack_Hidden2
    {
        get => VolumeInPack_Hidden_Priv2;
        set => VolumeInPack_Hidden_Priv2 = value;
    }
    [NotMapped]
    [FormProperty(true, "Объем, куб. м", "РАО с упаковкой", "10")]
    public RamAccess<string> VolumeInPack
    {
        get
        {
            if (!VolumeInPack_Hidden || VolumeInPack_Hidden2)
            {
                if (Dictionary.ContainsKey(nameof(VolumeInPack)))
                {
                    ((RamAccess<string>)Dictionary[nameof(VolumeInPack)]).Value = VolumeInPack_DB;
                    return (RamAccess<string>)Dictionary[nameof(VolumeInPack)];
                }
                else
                {
                    var rm = new RamAccess<string>(VolumeInPack_Validation, VolumeInPack_DB);
                    rm.PropertyChanged += VolumeInPackValueChanged;
                    Dictionary.Add(nameof(VolumeInPack), rm);
                    return (RamAccess<string>)Dictionary[nameof(VolumeInPack)];
                }
            }
            else
            {
                if (Dictionary.ContainsKey(nameof(VolumeInPack)))
                {
                    return (RamAccess<string>)Dictionary[nameof(VolumeInPack)];
                }
                else
                {
                    var rm = new RamAccess<string>(null, null);
                    Dictionary.Add(nameof(VolumeInPack), rm);
                    return (RamAccess<string>)Dictionary[nameof(VolumeInPack)];
                }
            }
        }
        set
        {
            if (VolumeInPack_Hidden) return;
            VolumeInPack_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void VolumeInPackValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                VolumeInPack_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
        }
        VolumeInPack_DB = value1;
    }

    private bool VolumeInPack_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (value1.Equals("-"))
        {
            return true;
        }
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

    #region MassOutOfPack_11
    public string MassOutOfPack_DB { get; set; } = ""; 
    [NotMapped]
    [FormProperty(true, "Масса, т", "РАО без упаковки (нетто)", "11")]
    public RamAccess<string> MassOutOfPack//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(MassOutOfPack)))
            {
                ((RamAccess<string>)Dictionary[nameof(MassOutOfPack)]).Value = MassOutOfPack_DB;
                return (RamAccess<string>)Dictionary[nameof(MassOutOfPack)];
            }
            var rm = new RamAccess<string>(MassOutOfPack_Validation, MassOutOfPack_DB);
            rm.PropertyChanged += MassOutOfPackValueChanged;
            Dictionary.Add(nameof(MassOutOfPack), rm);
            return (RamAccess<string>)Dictionary[nameof(MassOutOfPack)];
        }
        set
        {
            MassOutOfPack_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void MassOutOfPackValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                MassOutOfPack_DB = value1;
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
        MassOutOfPack_DB = value1;
    }

    private bool MassOutOfPack_Validation(RamAccess<string> value)//TODO
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

    #region MassInPack_12
    public string MassInPack_DB { get; set; } = "";
    public bool MassInPack_Hidden_Priv { get; set; }

    [NotMapped]
    public bool MassInPack_Hidden
    {
        get => MassInPack_Hidden_Priv;
        set => MassInPack_Hidden_Priv = value;
    }
    public bool MassInPack_Hidden_Priv2 { get; set; }

    [NotMapped]
    public bool MassInPack_Hidden2
    {
        get => MassInPack_Hidden_Priv2;
        set => MassInPack_Hidden_Priv2 = value;
    }

    [NotMapped]
    [FormProperty(true, "Масса, т", "РАО с упаковкой (брутто)", "12")]
    public RamAccess<string> MassInPack
    {
        get
        {
            if (!MassInPack_Hidden || MassInPack_Hidden2)
            {
                if (Dictionary.ContainsKey(nameof(MassInPack)))
                {
                    ((RamAccess<string>)Dictionary[nameof(MassInPack)]).Value = MassInPack_DB;
                    return (RamAccess<string>)Dictionary[nameof(MassInPack)];
                }
                else
                {
                    var rm = new RamAccess<string>(MassInPack_Validation, MassInPack_DB);
                    rm.PropertyChanged += MassInPackValueChanged;
                    Dictionary.Add(nameof(MassInPack), rm);
                    return (RamAccess<string>)Dictionary[nameof(MassInPack)];
                }
            }
            else
            {
                if (Dictionary.ContainsKey(nameof(MassInPack)))
                {
                    return (RamAccess<string>)Dictionary[nameof(MassInPack)];
                }
                else
                {
                    var rm = new RamAccess<string>(null, null);
                    Dictionary.Add(nameof(MassInPack), rm);
                    return (RamAccess<string>)Dictionary[nameof(MassInPack)];
                }
            }
        }
        set
        {
            if (MassInPack_Hidden) return;
            MassInPack_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void MassInPackValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                MassInPack_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
        }
        MassInPack_DB = value1;
    }

    private bool MassInPack_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (value1.Equals("-"))
        {
            return true;
        }
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

    #region QuantityOZIII_DB_13
    public string QuantityOZIII_DB { get; set; }
    [NotMapped]
    [FormProperty(true, "null-13", "Количество ОЗИИИ, шт", "13")]
    public RamAccess<string> QuantityOZIII//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(QuantityOZIII)))
            {
                ((RamAccess<string>)Dictionary[nameof(QuantityOZIII)]).Value = QuantityOZIII_DB;
                return (RamAccess<string>)Dictionary[nameof(QuantityOZIII)];
            }
            var rm = new RamAccess<string>(QuantityOZIII_Validation, QuantityOZIII_DB);
            rm.PropertyChanged += QuantityOZIIIValueChanged;
            Dictionary.Add(nameof(QuantityOZIII), rm);
            return (RamAccess<string>)Dictionary[nameof(QuantityOZIII)];
        }
        set
        {
            QuantityOZIII_DB = value.Value;
            OnPropertyChanged();
        }
    }
    // positive int.

    private void QuantityOZIIIValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            QuantityOZIII_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool QuantityOZIII_Validation(RamAccess<string> value)//Ready
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
        if (!int.TryParse(value.Value, out var tmpInt))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmpInt <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }
    #endregion

    #region TritiumActivity_14
    public string TritiumActivity_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Суммарная активность, Бк", "тритий", "14")]
    public RamAccess<string> TritiumActivity//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(TritiumActivity)))
            {
                ((RamAccess<string>)Dictionary[nameof(TritiumActivity)]).Value = TritiumActivity_DB;
                return (RamAccess<string>)Dictionary[nameof(TritiumActivity)];
            }
            var rm = new RamAccess<string>(TritiumActivity_Validation, TritiumActivity_DB);
            rm.PropertyChanged += TritiumActivityValueChanged;
            Dictionary.Add(nameof(TritiumActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(TritiumActivity)];
        }
        set
        {
            TritiumActivity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TritiumActivityValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                TritiumActivity_DB = value1;
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
        TritiumActivity_DB = value1;
    }

    private bool TritiumActivity_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (value.Value == "-")
        {
            return true;
        }
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var tmp = value.Value;
        tmp = tmp.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!tmp.Contains('e') && tmp.Contains('+') ^ tmp.Contains('-'))
        {
            tmp = tmp.Replace("+", "e+").Replace("-", "e-");
        }
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

    #region BetaGammaActivity_15
    public string BetaGammaActivity_DB { get; set; } = ""; [NotMapped]
    [FormProperty(true, "Суммарная активность, Бк", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "15")]
    public RamAccess<string> BetaGammaActivity//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(BetaGammaActivity)))
            {
                ((RamAccess<string>)Dictionary[nameof(BetaGammaActivity)]).Value = BetaGammaActivity_DB;
                return (RamAccess<string>)Dictionary[nameof(BetaGammaActivity)];
            }
            var rm = new RamAccess<string>(BetaGammaActivity_Validation, BetaGammaActivity_DB);
            rm.PropertyChanged += BetaGammaActivityValueChanged;
            Dictionary.Add(nameof(BetaGammaActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(BetaGammaActivity)];
        }
        set
        {
            BetaGammaActivity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void BetaGammaActivityValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                BetaGammaActivity_DB = value1;
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
        BetaGammaActivity_DB = value1;
    }

    private bool BetaGammaActivity_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        if (value.Value == "-")
        {
            return true;
        }
        var tmp = value1;
        var len = tmp.Length;
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

    #region AlphaActivity_16
    public string AlphaActivity_DB { get; set; } = ""; [NotMapped]
    [FormProperty(true, "Суммарная активность, Бк", "альфа-излучающие радионуклиды (исключая трансурановые)", "16")]
    public RamAccess<string> AlphaActivity//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(AlphaActivity)))
            {
                ((RamAccess<string>)Dictionary[nameof(AlphaActivity)]).Value = AlphaActivity_DB;
                return (RamAccess<string>)Dictionary[nameof(AlphaActivity)];
            }
            var rm = new RamAccess<string>(AlphaActivity_Validation, AlphaActivity_DB);
            rm.PropertyChanged += AlphaActivityValueChanged;
            Dictionary.Add(nameof(AlphaActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(AlphaActivity)];
        }
        set
        {
            AlphaActivity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void AlphaActivityValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                AlphaActivity_DB = value1;
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
        AlphaActivity_DB = value1;
    }

    private bool AlphaActivity_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        if (value.Value == "-")
        {
            return true;
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

    #region TransuraniumActivity_17
    public string TransuraniumActivity_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Суммарная активность, Бк", "трансурановые радионуклиды", "17")]
    public RamAccess<string> TransuraniumActivity//SUMMARIZABLE
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(TransuraniumActivity)))
            {
                ((RamAccess<string>)Dictionary[nameof(TransuraniumActivity)]).Value = TransuraniumActivity_DB;
                return (RamAccess<string>)Dictionary[nameof(TransuraniumActivity)];
            }
            var rm = new RamAccess<string>(TransuraniumActivity_Validation, TransuraniumActivity_DB);
            rm.PropertyChanged += TransuraniumActivityValueChanged;
            Dictionary.Add(nameof(TransuraniumActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(TransuraniumActivity)];
        }
        set
        {
            TransuraniumActivity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TransuraniumActivityValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                TransuraniumActivity_DB = value1;
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
        TransuraniumActivity_DB = value1;
    }

    private bool TransuraniumActivity_Validation(RamAccess<string> value)//TODO
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

    #region MainRadionuclids_18
    public string MainRadionuclids_DB { get; set; } = "";
    public bool MainRadionuclids_Hidden_Priv { get; set; }

    [NotMapped]
    public bool MainRadionuclids_Hidden
    {
        get => MainRadionuclids_Hidden_Priv;
        set => MainRadionuclids_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "null-18", "Основные радионуклиды", "18")]
    public RamAccess<string> MainRadionuclids
    {
        get
        {
            if (!MainRadionuclids_Hidden)
            {
                if (Dictionary.ContainsKey(nameof(MainRadionuclids)))
                {
                    ((RamAccess<string>)Dictionary[nameof(MainRadionuclids)]).Value = MainRadionuclids_DB;
                    return (RamAccess<string>)Dictionary[nameof(MainRadionuclids)];
                }
                else
                {
                    var rm = new RamAccess<string>(MainRadionuclids_Validation, MainRadionuclids_DB);
                    rm.PropertyChanged += MainRadionuclidsValueChanged;
                    Dictionary.Add(nameof(MainRadionuclids), rm);
                    return (RamAccess<string>)Dictionary[nameof(MainRadionuclids)];
                }
            }
            else
            {
                if (Dictionary.ContainsKey(nameof(MainRadionuclids)))
                {
                    return (RamAccess<string>)Dictionary[nameof(MainRadionuclids)];
                }
                else
                {
                    var rm = new RamAccess<string>(null, null);
                    Dictionary.Add(nameof(MainRadionuclids), rm);
                    return (RamAccess<string>)Dictionary[nameof(MainRadionuclids)];
                }
            }
        }
        set
        {
            if (MainRadionuclids_Hidden) return;
            MainRadionuclids_DB = value.Value;
            OnPropertyChanged();
        }
    }
    //If change this change validation

    private void MainRadionuclidsValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            MainRadionuclids_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool MainRadionuclids_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var nuclids = value.Value.Split(";");
        for (var k = 0; k < nuclids.Length; k++)
        {
            nuclids[k] = nuclids[k].ToLower().Replace(" ", "");
        }
        var flag = true;
        foreach (var nucl in nuclids)
        {
            var tmp = Spravochniks.SprRadionuclids
                .Where(item => nucl == item.Item1)
                .Select(item => item.Item1);
            if (!tmp.Any()) flag = false;
        }
        if (!flag)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region Subsidy_19
    public string Subsidy_DB { get; set; } = "";
    public bool Subsidy_Hidden_Priv { get; set; }

    [NotMapped]
    public bool Subsidy_Hidden
    {
        get => Subsidy_Hidden_Priv;
        set => Subsidy_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "null-19", "Субсидия, %", "19")]
    public RamAccess<string> Subsidy
    {
        get
        {
            if (!Subsidy_Hidden)
            {
                if (Dictionary.ContainsKey(nameof(Subsidy)))
                {
                    ((RamAccess<string>)Dictionary[nameof(Subsidy)]).Value = Subsidy_DB;
                    return (RamAccess<string>)Dictionary[nameof(Subsidy)];
                }
                else
                {
                    var rm = new RamAccess<string>(Subsidy_Validation, Subsidy_DB);
                    rm.PropertyChanged += SubsidyValueChanged;
                    Dictionary.Add(nameof(Subsidy), rm);
                    return (RamAccess<string>)Dictionary[nameof(Subsidy)];
                }
            }
            else
            {
                if (Dictionary.ContainsKey(nameof(Subsidy)))
                {
                    return (RamAccess<string>)Dictionary[nameof(Subsidy)];
                }
                else
                {
                    var rm = new RamAccess<string>(null, null);
                    Dictionary.Add(nameof(Subsidy), rm);
                    return (RamAccess<string>)Dictionary[nameof(Subsidy)];
                }
            }
        }
        set
        {
            if (Subsidy_Hidden) return;
            Subsidy_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void SubsidyValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Subsidy_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool Subsidy_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        if (!int.TryParse(value.Value, out var tmpInt) || tmpInt is not (> 0 and < 100))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region FcpNumber
    public string FcpNumber_DB { get; set; } = "";
    public bool FcpNumber_Hidden_Priv { get; set; }

    [NotMapped]
    public bool FcpNumber_Hidden
    {
        get => FcpNumber_Hidden_Priv;
        set => FcpNumber_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "null-20", "Номер мероприятия ФЦП", "20")]
    public RamAccess<string> FcpNumber
    {
        get
        {
            if (!FcpNumber_Hidden)
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
            else
            {
                if (Dictionary.ContainsKey(nameof(FcpNumber)))
                {
                    return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
                }
                else
                {
                    var rm = new RamAccess<string>(null, null);
                    Dictionary.Add(nameof(FcpNumber), rm);
                    return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
                }
            }
        }
        set
        {
            if (FcpNumber_Hidden) return;
            FcpNumber_DB = value.Value;
            OnPropertyChanged();
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

    #endregion

    #region IExcel
    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        base.ExcelGetRow(worksheet, row);
        StoragePlaceName.Value = Convert.ToString(worksheet.Cells[row, 2].Value);
        StoragePlaceCode.Value = Convert.ToString(worksheet.Cells[row, 3].Value);
        PackName.Value = Convert.ToString(worksheet.Cells[row, 4].Value);
        PackType.Value = Convert.ToString(worksheet.Cells[row, 5].Value);
        PackQuantity_DB = Convert.ToString(worksheet.Cells[row, 6].Value) is "0"
            ? "-"
            : Convert.ToString(worksheet.Cells[row, 6].Value);
        CodeRAO_DB = Convert.ToString(worksheet.Cells[row, 7].Value);
        StatusRAO_DB = Convert.ToString(worksheet.Cells[row, 8].Value);
        VolumeOutOfPack_DB = Convert.ToString(worksheet.Cells[row, 9].Value) is "0"
            ? "-"
            : double.TryParse(Convert.ToString(worksheet.Cells[row, 9].Value), out var val)
                ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture)
                : Convert.ToString(worksheet.Cells[row, 9].Value);
        VolumeInPack_DB = Convert.ToString(worksheet.Cells[row, 10].Value) is "0"
            ? "-"
            : double.TryParse(Convert.ToString(worksheet.Cells[row, 10].Value), out val)
                ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture)
                : Convert.ToString(worksheet.Cells[row, 10].Value);
        MassOutOfPack_DB = Convert.ToString(worksheet.Cells[row, 11].Value) is "0"
            ? "-"
            : double.TryParse(Convert.ToString(worksheet.Cells[row, 11].Value), out val)
                ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture)
                : Convert.ToString(worksheet.Cells[row, 11].Value);
        MassInPack_DB = Convert.ToString(worksheet.Cells[row, 12].Value) is "0"
            ? "-"
            : double.TryParse(Convert.ToString(worksheet.Cells[row, 12].Value), out val)
                ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture)
                : Convert.ToString(worksheet.Cells[row, 12].Value);
        QuantityOZIII_DB = Convert.ToString(worksheet.Cells[row, 13].Value) is "0"
            ? "-"
            : Convert.ToString(worksheet.Cells[row, 13].Value);
        TritiumActivity_DB = Convert.ToString(worksheet.Cells[row, 14].Value) is "0"
            ? "-"
            : double.TryParse(Convert.ToString(worksheet.Cells[row, 14].Value), out val)
                ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture)
                : Convert.ToString(worksheet.Cells[row, 14].Value);
        BetaGammaActivity_DB = Convert.ToString(worksheet.Cells[row, 15].Value) is "0"
            ? "-"
            : double.TryParse(Convert.ToString(worksheet.Cells[row, 15].Value), out val)
                ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture)
                : Convert.ToString(worksheet.Cells[row, 15].Value);
        AlphaActivity_DB = Convert.ToString(worksheet.Cells[row, 16].Value) is "0"
            ? "-"
            : double.TryParse(Convert.ToString(worksheet.Cells[row, 16].Value), out val)
                ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture)
                : Convert.ToString(worksheet.Cells[row, 16].Value);
        TransuraniumActivity_DB = Convert.ToString(worksheet.Cells[row, 17].Value) is "0"
            ? "-"
            : double.TryParse(Convert.ToString(worksheet.Cells[row, 17].Value), out val)
                ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture)
                : Convert.ToString(worksheet.Cells[row, 17].Value);
        MainRadionuclids_DB = Convert.ToString(worksheet.Cells[row, 18].Value);
        Subsidy_DB = Convert.ToString(worksheet.Cells[row, 19].Value);
        FcpNumber_DB = Convert.ToString(worksheet.Cells[row, 20].Value);

    }
    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose, sumNumber);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;
        worksheet.Cells[row, column].Value = StoragePlaceName.Value ?? "";
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = StoragePlaceCode.Value ?? "";
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = PackName.Value ?? "";
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = PackType.Value ?? "";
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = PackQuantity_DB is null or "" or "-"
            ? 0
            : int.TryParse(PackQuantity_DB.Replace("(", "").Replace(")", "").Replace(".", ","), out var valInt)
                ? valInt
                : PackQuantity_DB;
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = CodeRAO_DB;
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = StatusRAO_DB;
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = VolumeOutOfPack_DB is null or "" or "-"
            ? 0
            : double.TryParse(ReplaceE(VolumeOutOfPack_DB), out var val)
                ? val
                : VolumeOutOfPack_DB;
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = VolumeInPack_DB is null or "" or "-"
            ? 0
            : double.TryParse(ReplaceE(VolumeInPack_DB), out val)
                ? val
                : VolumeInPack_DB;
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = MassOutOfPack_DB is null or "" or "-"
            ? 0
            : double.TryParse(ReplaceE(MassOutOfPack_DB), out val)
                ? val
                : MassOutOfPack_DB;
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = MassInPack_DB is null or "" or "-"
            ? 0
            : double.TryParse(ReplaceE(MassInPack_DB), out val)
                ? val
                : MassInPack_DB;
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = QuantityOZIII_DB is null or "" or "-"
            ? 0
            : int.TryParse(QuantityOZIII_DB.Replace("(", "").Replace(")", "").Replace(".", ","), out valInt)
                ? valInt
                : QuantityOZIII_DB;
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = TritiumActivity_DB is null or "" or "-"
            ? 0
            : double.TryParse(ReplaceE(TritiumActivity_DB), out val)
                ? val
                : TritiumActivity_DB;
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = BetaGammaActivity_DB is null or "" or "-"
            ? 0
            : double.TryParse(ReplaceE(BetaGammaActivity_DB), out val)
                ? val
                : BetaGammaActivity_DB;
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = AlphaActivity_DB is null or "" or "-"
            ? 0
            : double.TryParse(ReplaceE(AlphaActivity_DB), out val)
                ? val
                : AlphaActivity_DB;
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = TransuraniumActivity_DB is null or "" or "-"
            ? 0
            : double.TryParse(ReplaceE(TransuraniumActivity_DB), out val)
                ? val
                : TransuraniumActivity_DB;
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = MainRadionuclids_DB;
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = Subsidy_DB;
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = FcpNumber_DB;
        return 19;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form2.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;
        worksheet.Cells[row, column].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(StoragePlaceName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(StoragePlaceCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(PackName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(PackType))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(PackQuantity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(CodeRAO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(StatusRAO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(VolumeOutOfPack))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(VolumeInPack))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(MassOutOfPack))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(MassInPack))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(QuantityOZIII))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(TritiumActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(BetaGammaActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(AlphaActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(TransuraniumActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(MainRadionuclids))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(Subsidy))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form22,Models")?.GetProperty(nameof(FcpNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        return 19;
    }
    #endregion

    #region IDataGridColumn
    private static DataGridColumns _DataGridColumns { get; set; }
    public override DataGridColumns GetColumnStructure(string param = "")
    {
        if (_DataGridColumns != null) return _DataGridColumns;

        #region NumberInOrder (1)
        var numberInOrderR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(NumberInOrderSum))
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

        #region StoragePlaceName (2)
        var storagePlaceNameR = 
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(StoragePlaceName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (storagePlaceNameR != null)
        {
            storagePlaceNameR.SetSizeColToAllLevels(163);
            storagePlaceNameR.Binding = nameof(StoragePlaceName);
            numberInOrderR += storagePlaceNameR;
        }
        #endregion

        #region StoragePlaceCode (3)
        var storagePlaceCodeR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(StoragePlaceCode))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (storagePlaceCodeR != null)
        {
            storagePlaceCodeR.SetSizeColToAllLevels(88);
            storagePlaceCodeR.Binding = nameof(StoragePlaceCode);
            numberInOrderR += storagePlaceCodeR;
        }
        #endregion

        #region PackName (4)
        var packNameR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(PackName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (packNameR != null)
        {
            packNameR.SetSizeColToAllLevels(163);
            packNameR.Binding = nameof(PackName);
            numberInOrderR += packNameR;
        }
        #endregion

        #region PackType (5)
        var packTypeR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(PackType))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (packTypeR != null)
        {
            packTypeR.SetSizeColToAllLevels(88);
            packTypeR.Binding = nameof(PackType);
            numberInOrderR += packTypeR;
        }
        #endregion

        #region PackQuantity (6)
        var packQuantityR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(PackQuantity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (packQuantityR != null)
        {
            packQuantityR.SetSizeColToAllLevels(70);
            packQuantityR.Binding = nameof(PackQuantity);
            numberInOrderR += packQuantityR;
        }
        #endregion

        #region CodeRAO (7)
        var codeRaoR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(CodeRAO))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (codeRaoR != null)
        {
            codeRaoR.SetSizeColToAllLevels(88);
            codeRaoR.Binding = nameof(CodeRAO);
            numberInOrderR += codeRaoR;
        }
        #endregion

        #region StatusRAO (8)
        var statusRaoR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(StatusRAO))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (statusRaoR != null)
        {
            statusRaoR.SetSizeColToAllLevels(88);
            statusRaoR.Binding = nameof(StatusRAO);
            numberInOrderR += statusRaoR;
        }
        #endregion

        #region VolumeOutOfPack (9)
        var volumeOutOfPackR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(VolumeOutOfPack))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (volumeOutOfPackR != null)
        {
            volumeOutOfPackR.SetSizeColToAllLevels(123);
            volumeOutOfPackR.Binding = nameof(VolumeOutOfPack);
            numberInOrderR += volumeOutOfPackR;
        }
        #endregion

        #region VolumeInPack (10)
        var volumeInPackR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(VolumeInPack))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (volumeInPackR != null)
        {
            volumeInPackR.SetSizeColToAllLevels(123);
            volumeInPackR.Binding = nameof(VolumeInPack);
            numberInOrderR += volumeInPackR;
        }
        #endregion

        #region MassOutOfPack (11)
        var massOutOfPackR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(MassOutOfPack))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (massOutOfPackR != null)
        {
            massOutOfPackR.SetSizeColToAllLevels(123);
            massOutOfPackR.Binding = nameof(MassOutOfPack);
            numberInOrderR += massOutOfPackR;
        }
        #endregion

        #region MassInPack (12)
        var massInPackR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(MassInPack))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (massInPackR != null)
        {
            massInPackR.SetSizeColToAllLevels(123);
            massInPackR.Binding = nameof(MassInPack);
            numberInOrderR += massInPackR;
        }
        #endregion

        #region QuantityOZIII (13)
        var quantityOziiiR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(QuantityOZIII))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (quantityOziiiR != null)
        {
            quantityOziiiR.SetSizeColToAllLevels(113);
            quantityOziiiR.Binding = nameof(QuantityOZIII);
            numberInOrderR += quantityOziiiR;
        }
        #endregion

        #region TritiumActivity (14)
        var tritiumActivityR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(TritiumActivity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (tritiumActivityR != null)
        {
            tritiumActivityR.SetSizeColToAllLevels(163);
            tritiumActivityR.Binding = nameof(TritiumActivity);
            numberInOrderR += tritiumActivityR;
        }
        #endregion

        #region BetaGammaActivity (15)
        var betaGammaActivityR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(BetaGammaActivity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (betaGammaActivityR != null)
        {
            betaGammaActivityR.SetSizeColToAllLevels(170);
            betaGammaActivityR.Binding = nameof(BetaGammaActivity);
            numberInOrderR += betaGammaActivityR;
        }
        #endregion

        #region AlphaActivity (16)
        var alphaActivityR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(AlphaActivity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (alphaActivityR != null)
        {
            alphaActivityR.SetSizeColToAllLevels(170);
            alphaActivityR.Binding = nameof(AlphaActivity);
            numberInOrderR += alphaActivityR;
        }
        #endregion

        #region TransuraniumActivity (17)
        var transuraniumActivityR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(TransuraniumActivity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (transuraniumActivityR != null)
        {
            transuraniumActivityR.SetSizeColToAllLevels(200);
            transuraniumActivityR.Binding = nameof(TransuraniumActivity);
            numberInOrderR += transuraniumActivityR;
        }
        #endregion

        #region MainRadionuclids (18)
        var mainRadionuclidsR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(MainRadionuclids))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (mainRadionuclidsR != null)
        {
            mainRadionuclidsR.SetSizeColToAllLevels(153);
            mainRadionuclidsR.Binding = nameof(MainRadionuclids);
            numberInOrderR += mainRadionuclidsR;
        }
        #endregion

        #region Subsidy (19)
        var subsidyR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(Subsidy))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (subsidyR != null)
        {
            subsidyR.SetSizeColToAllLevels(88);
            subsidyR.Binding = nameof(Subsidy);
            numberInOrderR += subsidyR;
        }
        #endregion

        #region FcpNumber (20)
        var fcpNumberR =
            ((FormPropertyAttribute)typeof(Form22).GetProperty(nameof(FcpNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (fcpNumberR != null)
        {
            fcpNumberR.SetSizeColToAllLevels(163);
            fcpNumberR.Binding = nameof(FcpNumber);
            numberInOrderR += fcpNumberR;
        }
        #endregion

        _DataGridColumns = numberInOrderR;
        return _DataGridColumns;
    }
    #endregion
}