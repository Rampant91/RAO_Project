using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;
using Spravochniki;

namespace Models.Forms.Form1;

[Form_Class("Форма 1.8: Сведения о жидких кондиционированных РАО")]
[Table (name: "form_18")]
public partial class Form18 : Form1
{
    #region Constructor
    
    public Form18()
    {
        FormNum.Value = "1.8";
        Validate_all();
    }

    #endregion

    #region Validation
    
    private void Validate_all()
    {
        CodeRAO_Validation(CodeRAO);
        IndividualNumberZHRO_Validation(IndividualNumberZHRO);
        SpecificActivity_Validation(SpecificActivity);
        SaltConcentration_Validation(SaltConcentration);
        Radionuclids_Validation(Radionuclids);
        ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
        TransporterOKPO_Validation(TransporterOKPO);
        TritiumActivity_Validation(TritiumActivity);
        BetaGammaActivity_Validation(BetaGammaActivity);
        AlphaActivity_Validation(AlphaActivity);
        TransuraniumActivity_Validation(TransuraniumActivity);
        PassportNumber_Validation(PassportNumber);
        RefineOrSortRAOCode_Validation(RefineOrSortRAOCode);
        Subsidy_Validation(Subsidy);
        FcpNumber_Validation(FcpNumber);
        StatusRAO_Validation(StatusRAO);
        Volume6_Validation(Volume6);
        Mass7_Validation(Mass7);
        Volume20_Validation(Volume20);
        Mass21_Validation(Mass21);
        StoragePlaceName_Validation(StoragePlaceName);
        StoragePlaceCode_Validation(StoragePlaceCode);
    }

    public override bool Object_Validation()
    {
        return !(CodeRAO.HasErrors ||
                 IndividualNumberZHRO.HasErrors ||
                 SpecificActivity.HasErrors ||
                 SaltConcentration.HasErrors ||
                 Radionuclids.HasErrors ||
                 ProviderOrRecieverOKPO.HasErrors ||
                 TransporterOKPO.HasErrors ||
                 TritiumActivity.HasErrors ||
                 BetaGammaActivity.HasErrors ||
                 AlphaActivity.HasErrors ||
                 TransuraniumActivity.HasErrors ||
                 PassportNumber.HasErrors ||
                 RefineOrSortRAOCode.HasErrors ||
                 Subsidy.HasErrors ||
                 FcpNumber.HasErrors ||
                 StatusRAO.HasErrors ||
                 Volume6.HasErrors ||
                 Mass7.HasErrors ||
                 Volume20.HasErrors ||
                 Mass21.HasErrors ||
                 StoragePlaceName.HasErrors ||
                 StoragePlaceCode.HasErrors);
    }

    protected override bool DocumentNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    protected override bool DocumentVid_Validation(RamAccess<byte?> value)
    {
        value.ClearErrors();
        if (Spravochniks.SprDocumentVidName.Any(item => value.Value == item.Item1))
        {
            return true;
        }
        value.AddError("Недопустимое значение");
        return false;
    }

    protected override bool DocumentDate_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (tmp == "прим.")
        {
            return true;
        }
        if (!DateOnly.TryParse(tmp, CultureInfo.CreateSpecificCulture("ru-RU"), out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        //var b = OperationCode.Value == "68";
        //var c = OperationCode.Value is "52" or "55";
        //var d = OperationCode.Value is "18" or "51";
        //if (b || c || d)
        //{
        //    if (!tmp.Equals(OperationDate.Value))
        //    {
        //        //value.AddError("Заполните примечание");//to do note handling
        //        return true;
        //    }
        //}
        return true;
    }

    #endregion

    #region Properties

    #region  Sum

    // ReSharper disable once MemberCanBePrivate.Global - не делай private!!!
    public bool Sum_DB { get; set; }

    [NotMapped]
    public RamAccess<bool> Sum
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Sum), out var value))
            {
                ((RamAccess<bool>)value).Value = Sum_DB;
                return (RamAccess<bool>)value;
            }
            var rm = new RamAccess<bool>(Sum_Validation, Sum_DB);
            rm.PropertyChanged += Sum_ValueChanged;
            Dictionary.Add(nameof(Sum), rm);
            return (RamAccess<bool>)Dictionary[nameof(Sum)];
        }
        set
        {
            Sum_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void Sum_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        Sum_DB = ((RamAccess<bool>)value).Value;
    }

    private static bool Sum_Validation(RamAccess<bool> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region OperationCode (2)

    private protected override void OperationCode_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;

        var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
        if (OperationCode_DB != value1)
        {
            OperationCode_DB = value1;
            if (Report is { AutoReplace: true })
            {
                AutoReplaceByOpCode(value1);
            }
        }
    }

    protected override bool OperationCode_Validation(RamAccess<string> value)//OK
    {
        value.ClearErrors();
        if (value.Value is null or "-" or "")
        {
            return true;
        }
        if (!Spravochniks.SprOpCodes.Contains(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (!TwoNumRegex().IsMatch(value.Value)
            || !byte.TryParse(value.Value, out var byteValue)
            || byteValue is not (1 or >= 10 and <= 13 or >= 25 and <= 29 or 31 or 32 or >= 35 and <= 39
            or 42 or 51 or 52 or 55 or 63 or 64 or 68 or 97 or 98))
        {
            value.AddError("Код операции не может быть использован в форме 1.8");
            return false;
        }

        return true;
    }

    #region AutoReplaceByOpCode

    private void AutoReplaceByOpCode(string opCode)
    {
        const string dash = "-";
        var masterOkpo = Report?.Reports?.Master_DB?.OkpoRep.Value ?? string.Empty;
        switch (opCode)
        {
            #region 10, 18, 43, 51, 52, 68, 97, 98
            
            case "10" or "18" or "43" or "51" or "52" or "68" or "97" or "98":
            {
                #region ProviderOrRecieverOKPO (14)

                if (!string.IsNullOrWhiteSpace(masterOkpo)
                    && ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (15)

                if (TransporterOKPO_DB != dash)
                {
                    TransporterOKPO.Value = dash;
                }

                #endregion

                #region RefineOrSortRAOCode (26)

                if (RefineOrSortRAOCode_DB != dash)
                {
                    RefineOrSortRAOCode.Value = dash;
                }

                #endregion

                break;
            }

            #endregion

            #region 11, 13, 16
            
            case "11" or "13" or "16":
            {
                #region ProviderOrRecieverOKPO (14)

                if (!string.IsNullOrWhiteSpace(masterOkpo)
                    && ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (15)

                if (TransporterOKPO_DB != dash)
                {
                    TransporterOKPO.Value = dash;
                }

                #endregion

                #region StatusRAO (19)

                if (!string.IsNullOrWhiteSpace(masterOkpo)
                    && StatusRAO_DB != masterOkpo)
                {
                    StatusRAO.Value = masterOkpo;
                }

                #endregion

                #region RefineOrSortRAOCode (26)

                if (RefineOrSortRAOCode_DB != dash)
                {
                    RefineOrSortRAOCode.Value = dash;
                }

                #endregion

                break;
            }

            #endregion

            #region 21, 22, 25, 26, 27, 28, 29, 31, 32, 35, 36, 37, 38, 39
            
            case "21" or "22" or "25" or "26" or "27" or "28" or "29" or "31" or "32" or "35" or "36" or "37" or "38" or "39":
            {
                #region RefineOrSortRAOCode (26)

                if (RefineOrSortRAOCode_DB != dash)
                {
                    RefineOrSortRAOCode.Value = dash;
                }

                #endregion

                break;
            }

            #endregion

            #region 55
            
            case "55":
            {
                #region ProviderOrRecieverOKPO (14)

                if (!string.IsNullOrWhiteSpace(masterOkpo)
                    && ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (15)

                if (TransporterOKPO_DB != dash)
                {
                    TransporterOKPO.Value = dash;
                }

                #endregion

                break;
            }

            #endregion

            default: return;
        }
    }

    #endregion

    #endregion

    #region OperationDate (3)

    protected override bool OperationDate_Validation(RamAccess<string> value)
        => string.IsNullOrWhiteSpace(value.Value) || DateString_Validation(value);

    #endregion

    #region IndividualNumberZHRO (4)

    public string IndividualNumberZHRO_DB { get; set; } = "";

    //нельзя делать private
    public bool IndividualNumberZHRO_Hidden_Priv { get; set; }

    [NotMapped]
    public bool IndividualNumberZHRO_Hidden
    {
        get => IndividualNumberZHRO_Hidden_Priv;
        set => IndividualNumberZHRO_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Сведения о партии ЖРО", "индивидуальный номер (идентификационный код) партии ЖРО", "4")]
    public RamAccess<string> IndividualNumberZHRO
    {
        get
        {
            if (!IndividualNumberZHRO_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(IndividualNumberZHRO), out RamAccess value))
                {
                    ((RamAccess<string>)value).Value = IndividualNumberZHRO_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(IndividualNumberZHRO_Validation, IndividualNumberZHRO_DB);
                rm.PropertyChanged += IndividualNumberZHRO_ValueChanged;
                Dictionary.Add(nameof(IndividualNumberZHRO), rm);
                return (RamAccess<string>)Dictionary[nameof(IndividualNumberZHRO)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!IndividualNumberZHRO_Hidden_Priv)
            {
                IndividualNumberZHRO_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void IndividualNumberZHRO_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        IndividualNumberZHRO_DB = tmp;
    }

    private static bool IndividualNumberZHRO_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region PassportNumber (5)

    public string PassportNumber_DB { get; set; } = "";

    //нельзя делать private
    public bool PassportNumber_Hidden_Priv { get; set; }

    [NotMapped]
    public bool PassportNumber_Hidden
    {
        get => PassportNumber_Hidden_Priv;
        set => PassportNumber_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Сведения о партии ЖРО", "номер паспорта", "5")]
    public RamAccess<string> PassportNumber
    {
        get
        {
            if (!PassportNumber_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(PassportNumber), out var value))
                {
                    ((RamAccess<string>)value).Value = PassportNumber_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(PassportNumber_Validation, PassportNumber_DB);
                rm.PropertyChanged += PassportNumber_ValueChanged;
                Dictionary.Add(nameof(PassportNumber), rm);
                return (RamAccess<string>)Dictionary[nameof(PassportNumber)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!PassportNumber_Hidden_Priv)
            {
                PassportNumber_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void PassportNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        PassportNumber_DB = tmp;
    }

    private static bool PassportNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        if (value.Value == "-")
        {
            return true;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((PassportNumberNote.Value == null) || (PassportNumberNote.Value == ""))
            //{
            //    value.AddError("Поле не может быть пустым");//to do note handling
            //}
            return true;
        }
        return true;
    }

    #endregion

    #region Volume6 (6)

    public string Volume6_DB { get; set; }

    //нельзя делать private
    public bool Volume6_Hidden_Priv { get; set; }

    [NotMapped]
    public bool Volume6_Hidden
    {
        get => Volume6_Hidden_Priv;
        set => Volume6_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Сведения о партии ЖРО", "объем, куб. м", "6")]
    public RamAccess<string> Volume6
    {
        get
        {
            if (!Volume6_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(Volume6), out var value))
                {
                    ((RamAccess<string>)value).Value = Volume6_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Volume6_Validation, Volume6_DB);
                rm.PropertyChanged += Volume6_ValueChanged;
                Dictionary.Add(nameof(Volume6), rm);
                return (RamAccess<string>)Dictionary[nameof(Volume6)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!Volume6_Hidden_Priv)
            {
                Volume6_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void Volume6_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        Volume6_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool Volume6_Validation(RamAccess<string> value) => 
        string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region Mass7 (7)

    public string Mass7_DB { get; set; }

    //нельзя делать private
    public bool Mass7_Hidden_Priv { get; set; }

    [NotMapped]
    public bool Mass7_Hidden
    {
        get => Mass7_Hidden_Priv;
        set => Mass7_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Сведения о партии ЖРО", "масса, т", "7")]
    public RamAccess<string> Mass7
    {
        get
        {
            if (!Mass7_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(Mass7), out var value))
                {
                    ((RamAccess<string>)value).Value = Mass7_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Mass7_Validation, Mass7_DB);
                rm.PropertyChanged += Mass7_ValueChanged;
                Dictionary.Add(nameof(Mass7), rm);
                return (RamAccess<string>)Dictionary[nameof(Mass7)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!Mass7_Hidden_Priv)
            {
                Mass7_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void Mass7_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        Mass7_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool Mass7_Validation(RamAccess<string> value) => 
        string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region SaltConcentration (8)

    public string SaltConcentration_DB { get; set; }

    //нельзя делать private
    public bool SaltConcentration_Hidden_Priv { get; set; }

    [NotMapped]
    public bool SaltConcentration_Hidden
    {
        get => SaltConcentration_Hidden_Priv;
        set => SaltConcentration_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Сведения о партии ЖРО", "солесодержание, г/л", "8")]
    public RamAccess<string> SaltConcentration
    {
        get
        {
            if (!SaltConcentration_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(SaltConcentration), out var value))
                {
                    ((RamAccess<string>)value).Value = SaltConcentration_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(SaltConcentration_Validation, SaltConcentration_DB);
                rm.PropertyChanged += SaltConcentration_ValueChanged;
                Dictionary.Add(nameof(SaltConcentration), rm);
                return (RamAccess<string>)Dictionary[nameof(SaltConcentration)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!SaltConcentration_Hidden_Priv)
            {
                SaltConcentration_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void SaltConcentration_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        SaltConcentration_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool SaltConcentration_Validation(RamAccess<string> value) => 
        string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region Radionuclids (9)

    public string Radionuclids_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения о партии ЖРО", "наименование радионуклида", "9")]
    public RamAccess<string> Radionuclids
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Radionuclids), out var value))
            {
                ((RamAccess<string>)value).Value = Radionuclids_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Radionuclids_Validation, Radionuclids_DB);
            rm.PropertyChanged += Radionuclids_ValueChanged;
            Dictionary.Add(nameof(Radionuclids), rm);
            return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
        }//OK
        set
        {
            Radionuclids_DB = value.Value;
            OnPropertyChanged();
        }
    }//If change this change validation

    private void Radionuclids_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        Radionuclids_DB = tmp;
    }

    private static bool Radionuclids_Validation(RamAccess<string> value) => NuclidString_Validation(value);

    #endregion

    #region SpecificActivity (10)

    public string SpecificActivity_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения о партии ЖРО", "удельная активность, Бк/г", "10")]
    public RamAccess<string> SpecificActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(SpecificActivity), out var value))
            {
                ((RamAccess<string>)value).Value = SpecificActivity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(SpecificActivity_Validation, SpecificActivity_DB);
            rm.PropertyChanged += SpecificActivity_ValueChanged;
            Dictionary.Add(nameof(SpecificActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(SpecificActivity)];
        }
        set
        {
            SpecificActivity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void SpecificActivity_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        
        SpecificActivity_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool SpecificActivity_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region ProviderOrRecieverOKPO (14)

    public string ProviderOrRecieverOKPO_DB { get; set; } = "";

    //нельзя делать private
    public bool ProviderOrRecieverOKPO_Hidden_Priv { get; set; }

    [NotMapped]
    public bool ProviderOrRecieverOKPO_Hidden
    {
        get => ProviderOrRecieverOKPO_Hidden_Priv;
        set => ProviderOrRecieverOKPO_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "ОКПО", "поставщика или получателя", "14")]
    public RamAccess<string> ProviderOrRecieverOKPO
    {
        get
        {
            if (!ProviderOrRecieverOKPO_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(ProviderOrRecieverOKPO), out var value))
                {
                    ((RamAccess<string>)value).Value = ProviderOrRecieverOKPO_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(ProviderOrRecieverOKPO_Validation, ProviderOrRecieverOKPO_DB);
                rm.PropertyChanged += ProviderOrRecieverOKPO_ValueChanged;
                Dictionary.Add(nameof(ProviderOrRecieverOKPO), rm);
                return (RamAccess<string>)Dictionary[nameof(ProviderOrRecieverOKPO)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!ProviderOrRecieverOKPO_Hidden_Priv)
            {
                ProviderOrRecieverOKPO_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void ProviderOrRecieverOKPO_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        if (Spravochniks.OKSM.Contains(tmp.ToUpper()))
        {
            tmp = tmp.ToUpper();
        }
        ProviderOrRecieverOKPO_DB = tmp;
    }

    private static bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
            return true;
        }
        if (value.Value.Equals("Минобороны"))
        {
            return true;
        }
        if (value.Value.Length is not (8 or 14))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        if (!OkpoRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        return true;
    }

    #endregion

    #region TransporterOKPO (15)

    public string TransporterOKPO_DB { get; set; } = "";

    //нельзя делать private
    public bool TransporterOKPO_Hidden_Priv { get; set; }

    [NotMapped]
    public bool TransporterOKPO_Hidden
    {
        get => TransporterOKPO_Hidden_Priv;
        set => TransporterOKPO_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "ОКПО", "перевозчика", "15")]
    public RamAccess<string> TransporterOKPO
    {
        get
        {
            if (!TransporterOKPO_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(TransporterOKPO), out var value))
                {
                    ((RamAccess<string>)value).Value = TransporterOKPO_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(TransporterOKPO_Validation, TransporterOKPO_DB);
                rm.PropertyChanged += TransporterOKPO_ValueChanged;
                Dictionary.Add(nameof(TransporterOKPO), rm);
                return (RamAccess<string>)Dictionary[nameof(TransporterOKPO)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!TransporterOKPO_Hidden_Priv)
            {
                TransporterOKPO_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void TransporterOKPO_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        TransporterOKPO_DB = tmp;
    }

    private static bool TransporterOKPO_Validation(RamAccess<string> value)//Done
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (tmp.Equals("-") || tmp.Equals("Минобороны"))
        {
            return true;
        }
        if (tmp.Equals("прим."))
        {
            //if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
            //    value.AddError( "Заполните примечание");
            return true;
        }
        if (tmp.Length is not (8 or 14))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        if (!OkpoRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        return true;
    }

    #endregion

    #region StoragePlaceName (16)

    public string StoragePlaceName_DB { get; set; } = "";

    //нельзя делать private
    public bool StoragePlaceName_Hidden_Priv { get; set; }

    [NotMapped]
    public bool StoragePlaceName_Hidden
    {
        get => StoragePlaceName_Hidden_Priv;
        set => StoragePlaceName_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Пункт хранения", "наименование", "16")]
    public RamAccess<string> StoragePlaceName
    {
        get
        {
            if (!StoragePlaceName_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(StoragePlaceName), out var value))
                {
                    ((RamAccess<string>)value).Value = StoragePlaceName_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(StoragePlaceName_Validation, StoragePlaceName_DB);
                rm.PropertyChanged += StoragePlaceName_ValueChanged;
                Dictionary.Add(nameof(StoragePlaceName), rm);
                return (RamAccess<string>)Dictionary[nameof(StoragePlaceName)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (StoragePlaceName_Hidden_Priv) return;
            StoragePlaceName_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void StoragePlaceName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        StoragePlaceName_DB = tmp;
    }

    private static bool StoragePlaceName_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        //List<string> spr = new List<string>();
        //if (!spr.Contains(value.Value))
        //{
        //    value.AddError("Недопустимое значение");
        //    return false;
        //}
        return true;
    }

    #endregion

    #region StoragePlaceCode (17)

    public string StoragePlaceCode_DB { get; set; } = "";

    //нельзя делать private
    public bool StoragePlaceCode_Hidden_Priv { get; set; }

    [NotMapped]
    public bool StoragePlaceCode_Hidden
    {
        get => StoragePlaceCode_Hidden_Priv;
        set => StoragePlaceCode_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Пункт хранения", "код", "17")]
    public RamAccess<string> StoragePlaceCode //8 digits code or - .
    {
        get
        {
            if (!StoragePlaceCode_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(StoragePlaceCode), out var value))
                {
                    ((RamAccess<string>)value).Value = StoragePlaceCode_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(StoragePlaceCode_Validation, StoragePlaceCode_DB);
                rm.PropertyChanged += StoragePlaceCode_ValueChanged;
                Dictionary.Add(nameof(StoragePlaceCode), rm);
                return (RamAccess<string>)Dictionary[nameof(StoragePlaceCode)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (StoragePlaceCode_Hidden_Priv) return;
            StoragePlaceCode_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void StoragePlaceCode_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        StoragePlaceCode_DB = tmp;
    }

    private static bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        //List<string> lst = new List<string>();//HERE binds spr
        //if (!lst.Contains(value.Value))
        //{
        //    value.AddError("Недопустимое значение"); return false;
        //}
        //return true;
        if (value.Value is null or "" or "-")
        {
            return true;
        }
        if (!StoragePlaceCodeRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        var tmp = value.Value;
        if (tmp.Length != 8) return true;
        if (!StoragePlaceCodeRegex1().IsMatch(tmp[..1]))
        {
            value.AddError($"Недопустимый вид пункта - {tmp[..1]}");
        }
        if (!StoragePlaceCodeRegex2().IsMatch(tmp.AsSpan(1, 1)))
        {
            value.AddError($"Недопустимое состояние пункта - {tmp.Substring(1, 1)}");
        }
        if (!StoragePlaceCodeRegex3().IsMatch(tmp.AsSpan(2, 1)))
        {
            value.AddError($"Недопустимая изоляция от окружающей среды - {tmp.Substring(2, 1)}");
        }
        if (!StoragePlaceCodeRegex4().IsMatch(tmp.AsSpan(3, 1)))
        {
            value.AddError($"Недопустимая зона нахождения пункта - {tmp.Substring(3, 1)}");
        }
        if (!StoragePlaceCodeRegex5().IsMatch(tmp.AsSpan(4, 1)))
        {
            value.AddError($"Недопустимое значение пункта - {tmp.Substring(4, 1)}");
        }
        if (!StoragePlaceCodeRegex6().IsMatch(tmp.AsSpan(5, 1)))
        {
            value.AddError($"Недопустимое размещение пункта хранения относительно поверхности земли - {tmp.Substring(5, 1)}");
        }
        if (!StoragePlaceCodeRegex7().IsMatch(tmp.AsSpan(6, 2)))
        {
            value.AddError($"Недопустимый код типа РАО - {tmp.Substring(6, 2)}");
        }
        return !value.HasErrors;
    }

    #endregion

    #region CodeRAO (18)

    public string CodeRAO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Характеристика ЖРО", "код", "18")]
    public RamAccess<string> CodeRAO
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(CodeRAO), out var value))
            {
                ((RamAccess<string>)value).Value = CodeRAO_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(CodeRAO_Validation, CodeRAO_DB);
            rm.PropertyChanged += CodeRAO_ValueChanged;
            Dictionary.Add(nameof(CodeRAO), rm);
            return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
        }
        set
        {
            CodeRAO_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void CodeRAO_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty.ToLower().Replace("х", "x");
        CodeRAO_DB = tmp;
    }

    private static bool CodeRAO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value.ToLower().Replace("х", "x");
        if (!CodeRaoRegex().IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region StatusRAO (19)

    public string StatusRAO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Характеристика ЖРО", "статус", "19")]
    public RamAccess<string> StatusRAO  //1 digit or OKPO.
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(StatusRAO), out var value))
            {
                ((RamAccess<string>)value).Value = StatusRAO_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(StatusRAO_Validation, StatusRAO_DB);
            rm.PropertyChanged += StatusRAO_ValueChanged;
            Dictionary.Add(nameof(StatusRAO), rm);
            return (RamAccess<string>)Dictionary[nameof(StatusRAO)];
        }
        set
        {
            StatusRAO_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void StatusRAO_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        StatusRAO_DB = tmp;
    }

    private static bool StatusRAO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (tmp.Length == 1)
        {
            if (!int.TryParse(tmp, out var intValue) 
                || intValue is not (>= 1 and <= 4 or 6 or 9))
            {
                value.AddError("Недопустимое значение"); 
                return false;
            }
        }
        if (tmp.Length is not (8 or 14))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        if (!OkpoRegex().IsMatch(tmp))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        return true;
    }

    #endregion

    #region Volume20 (20)

    public string Volume20_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Характеристика ЖРО", "объем, куб. м", "20")]
    public RamAccess<string> Volume20
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Volume20), out var value))
            {
                ((RamAccess<string>)value).Value = Volume20_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Volume20_Validation, Volume20_DB);
            rm.PropertyChanged += Volume20_ValueChanged;
            Dictionary.Add(nameof(Volume20), rm);
            return (RamAccess<string>)Dictionary[nameof(Volume20)];
        }
        set
        {
            Volume20_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void Volume20_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        Volume20_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool Volume20_Validation(RamAccess<string> value) => 
        string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region Mass21 (21)

    public string Mass21_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Характеристика ЖРО", "масса, т", "21")]
    public RamAccess<string> Mass21
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Mass21), out var value))
            {
                ((RamAccess<string>)value).Value = Mass21_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Mass21_Validation, Mass21_DB);
            rm.PropertyChanged += Mass21_ValueChanged;
            Dictionary.Add(nameof(Mass21), rm);
            return (RamAccess<string>)Dictionary[nameof(Mass21)];
        }
        set
        {
            Mass21_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void Mass21_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        Mass21_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool Mass21_Validation(RamAccess<string> value) => 
        string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region TritiumActivity (22)

    public string TritiumActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Характеристика ЖРО", "тритий", "22")]
    public RamAccess<string> TritiumActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(TritiumActivity), out var value))
            {
                ((RamAccess<string>)value).Value = TritiumActivity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(TritiumActivity_Validation, TritiumActivity_DB);
            rm.PropertyChanged += TritiumActivity_ValueChanged;
            Dictionary.Add(nameof(TritiumActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(TritiumActivity)];
        }
        set
        {
            TritiumActivity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TritiumActivity_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        TritiumActivity_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool TritiumActivity_Validation(RamAccess<string> value) => 
        string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region BetaGammaActivity (23)

    public string BetaGammaActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Характеристика ЖРО", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "23")]
    public RamAccess<string> BetaGammaActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(BetaGammaActivity), out var value))
            {
                ((RamAccess<string>)value).Value = BetaGammaActivity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(BetaGammaActivity_Validation, BetaGammaActivity_DB);
            rm.PropertyChanged += BetaGammaActivity_ValueChanged;
            Dictionary.Add(nameof(BetaGammaActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(BetaGammaActivity)];
        }
        set
        {
            BetaGammaActivity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void BetaGammaActivity_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        BetaGammaActivity_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool BetaGammaActivity_Validation(RamAccess<string> value) => 
        string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region AlphaActivity (24)

    public string AlphaActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Характеристика ЖРО", "альфа-излучающие радионуклиды (исключая трансурановые)", "24")]
    public RamAccess<string> AlphaActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(AlphaActivity), out var value))
            {
                ((RamAccess<string>)value).Value = AlphaActivity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(AlphaActivity_Validation, AlphaActivity_DB);
            rm.PropertyChanged += AlphaActivity_ValueChanged;
            Dictionary.Add(nameof(AlphaActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(AlphaActivity)];
        }
        set
        {
            AlphaActivity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void AlphaActivity_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        AlphaActivity_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool AlphaActivity_Validation(RamAccess<string> value) => 
        string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region TransuraniumActivity (25)

    public string TransuraniumActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Характеристика ЖРО", "трансурановые радионуклиды", "25")]
    public RamAccess<string> TransuraniumActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(TransuraniumActivity), out var value))
            {
                ((RamAccess<string>)value).Value = TransuraniumActivity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(TransuraniumActivity_Validation, TransuraniumActivity_DB);
            rm.PropertyChanged += TransuraniumActivity_ValueChanged;
            Dictionary.Add(nameof(TransuraniumActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(TransuraniumActivity)];
        }
        set
        {
            TransuraniumActivity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TransuraniumActivity_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        TransuraniumActivity_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool TransuraniumActivity_Validation(RamAccess<string> value) => 
        string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region RefineOrSortRAOCode (26)

    public string RefineOrSortRAOCode_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Характеристика ЖРО", "Код переработки / сортировки РАО", "26")]
    public RamAccess<string> RefineOrSortRAOCode //2 digits code or empty.
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(RefineOrSortRAOCode), out var value))
            {
                ((RamAccess<string>)value).Value = RefineOrSortRAOCode_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(RefineOrSortRAOCode_Validation, RefineOrSortRAOCode_DB);
            rm.PropertyChanged += RefineOrSortRAOCode_ValueChanged;
            Dictionary.Add(nameof(RefineOrSortRAOCode), rm);
            return (RamAccess<string>)Dictionary[nameof(RefineOrSortRAOCode)];
        }
        set
        {
            RefineOrSortRAOCode_DB = value.Value;
            OnPropertyChanged();
        }
    }//If change this change validation

    private void RefineOrSortRAOCode_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        RefineOrSortRAOCode_DB = tmp;
    }

    private bool RefineOrSortRAOCode_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        if (value.Value == "-")
        {
            return true;
        }
        if (OperationCode.Value == "55")
        {
            if (!Spravochniks.SprRefineOrSortCodes.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
        }
        else if (string.IsNullOrEmpty(OperationCode.Value))
        {
            value.AddError("Не указан код операции");
            return false;
        }
        return true;
    }

    #endregion

    #region Subsidy (27)

    public string Subsidy_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-27", "Субсидия, %", "27")]
    public RamAccess<string> Subsidy // 0<number<=100 or empty.
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Subsidy), out var value))
            {
                ((RamAccess<string>)value).Value = Subsidy_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Subsidy_Validation, Subsidy_DB);
            rm.PropertyChanged += Subsidy_ValueChanged;
            Dictionary.Add(nameof(Subsidy), rm);
            return (RamAccess<string>)Dictionary[nameof(Subsidy)];
        }
        set
        {
            Subsidy_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void Subsidy_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        Subsidy_DB = tmp;
    }

    private static bool Subsidy_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }
        if (!int.TryParse(value.Value, out var intValue) || intValue is not (>= 0 and <= 100))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region FcpNumber (28)

    public string FcpNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-28", "Номер мероприятия ФЦП", "28")]
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
            rm.PropertyChanged += FcpNumber_ValueChanged;
            Dictionary.Add(nameof(FcpNumber), rm);
            return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
        }
        set
        {
            FcpNumber_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void FcpNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        FcpNumber_DB = tmp;
    }

    private static bool FcpNumber_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region ContractNumber (29)

    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string ContractNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Договор", "номер", "29")]
    public RamAccess<string> ContractNumber
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ContractNumber), out var value))
            {
                ((RamAccess<string>)value).Value = ContractNumber_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(ContractNumber_Validation, ContractNumber_DB);
            rm.PropertyChanged += ContractNumber_ValueChanged;
            Dictionary.Add(nameof(ContractNumber), rm);
            return (RamAccess<string>)Dictionary[nameof(ContractNumber)];
        }
        set
        {
            ContractNumber_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void ContractNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        ContractNumber_DB = tmp.Length > 100
            ? tmp[..100]
            : tmp;
    }

    private static bool ContractNumber_Validation(RamAccess<string> value)
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
        IndividualNumberZHRO_DB = Convert.ToString(worksheet.Cells[row, 4].Value);
        PassportNumber_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        Volume6_DB = ConvertFromExcelDouble(worksheet.Cells[row, 6].Value);
        Mass7_DB = ConvertFromExcelDouble(worksheet.Cells[row, 7].Value);
        SaltConcentration_DB = ConvertFromExcelDouble(worksheet.Cells[row, 8].Value);
        Radionuclids_DB = Convert.ToString(worksheet.Cells[row, 9].Value);
        SpecificActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 10].Value);
        DocumentVid_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 11].Value), out var byteValue) ? byteValue : null;
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[row, 12].Value);
        DocumentDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 13].Text);
        ProviderOrRecieverOKPO_DB = Convert.ToString(worksheet.Cells[row, 14].Value);
        TransporterOKPO_DB = Convert.ToString(worksheet.Cells[row, 15].Value);
        StoragePlaceName_DB = Convert.ToString(worksheet.Cells[row, 16].Value);
        StoragePlaceCode_DB = Convert.ToString(worksheet.Cells[row, 17].Value);
        CodeRAO_DB = Convert.ToString(worksheet.Cells[row, 18].Value);
        StatusRAO_DB = Convert.ToString(worksheet.Cells[row, 19].Value);
        Volume20_DB = ConvertFromExcelDouble(worksheet.Cells[row, 20].Value);
        Mass21_DB = ConvertFromExcelDouble(worksheet.Cells[row, 21].Value);
        TritiumActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 22].Value);
        BetaGammaActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 23].Value);
        AlphaActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 24].Value);
        TransuraniumActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 25].Value);
        RefineOrSortRAOCode_DB = Convert.ToString(worksheet.Cells[row, 26].Value);
        Subsidy_DB = Convert.ToString(worksheet.Cells[row, 27].Value);
        FcpNumber_DB = Convert.ToString(worksheet.Cells[row, 28].Value);
        ContractNumber_DB = Convert.ToString(worksheet.Cells[row, 29].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ConvertToExcelString(IndividualNumberZHRO_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(PassportNumber_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelDouble(Volume6_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelDouble(Mass7_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelDouble(SaltConcentration_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelString(Radionuclids_DB);
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDouble(SpecificActivity_DB);
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = DocumentVid_DB is null ? "-" : DocumentVid_DB;
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ConvertToExcelString(DocumentNumber_DB);
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ConvertToExcelDate(DocumentDate_DB, worksheet, row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0));
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ConvertToExcelString(ProviderOrRecieverOKPO_DB);
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ConvertToExcelString(TransporterOKPO_DB);
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ConvertToExcelString(StoragePlaceName_DB);
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ConvertToExcelString(StoragePlaceCode_DB);
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ConvertToExcelString(CodeRAO_DB);
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ConvertToExcelString(StatusRAO_DB);
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ConvertToExcelDouble(Volume20_DB);
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ConvertToExcelDouble(Mass21_DB);
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ConvertToExcelDouble(TritiumActivity_DB);
        worksheet.Cells[row + (!transpose ? 19 : 0), column + (transpose ? 19 : 0)].Value = ConvertToExcelDouble(BetaGammaActivity_DB);
        worksheet.Cells[row + (!transpose ? 20 : 0), column + (transpose ? 20 : 0)].Value = ConvertToExcelDouble(AlphaActivity_DB);
        worksheet.Cells[row + (!transpose ? 21 : 0), column + (transpose ? 21 : 0)].Value = ConvertToExcelDouble(TransuraniumActivity_DB);
        worksheet.Cells[row + (!transpose ? 22 : 0), column + (transpose ? 22 : 0)].Value = ConvertToExcelString(RefineOrSortRAOCode_DB);
        worksheet.Cells[row + (!transpose ? 23 : 0), column + (transpose ? 23 : 0)].Value = ConvertToExcelString(Subsidy_DB);
        worksheet.Cells[row + (!transpose ? 24 : 0), column + (transpose ? 24 : 0)].Value = ConvertToExcelString(FcpNumber_DB);
        worksheet.Cells[row + (!transpose ? 25 : 0), column + (transpose ? 25 : 0)].Value = ConvertToExcelString(ContractNumber_DB);

        return 26;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form1.ExcelHeader(worksheet, row, column, transpose);
        column += +(transpose ? cnt : 0);
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(IndividualNumberZHRO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(PassportNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(Volume6))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(Mass7))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(SaltConcentration))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(Radionuclids))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(SpecificActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(DocumentVid))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(DocumentNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(DocumentDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(ProviderOrRecieverOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(TransporterOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(StoragePlaceName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(StoragePlaceCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(CodeRAO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(StatusRAO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(Volume20))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(Mass21))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(TritiumActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 19 : 0), column + (transpose ? 19 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(BetaGammaActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 20 : 0), column + (transpose ? 20 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(AlphaActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 21 : 0), column + (transpose ? 21 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(TransuraniumActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 22 : 0), column + (transpose ? 22 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(RefineOrSortRAOCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 23 : 0), column + (transpose ? 23 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(Subsidy))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 24 : 0), column + (transpose ? 24 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(FcpNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 25 : 0), column + (transpose ? 25 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form18,Models")?.GetProperty(nameof(ContractNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];

        return 26;
    }

    #endregion

    #region IDataGridColumn

    private static DataGridColumns _DataGridColumns { get; set; }

    public override DataGridColumns GetColumnStructure(string param = "")
    {
        if (_DataGridColumns != null) return _DataGridColumns;

        #region NumberInOrder (1)

        var numberInOrderR = ((FormPropertyAttribute)typeof(Form)
                .GetProperty(nameof(NumberInOrder))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD();
        if (numberInOrderR != null)
        {
            numberInOrderR.SetSizeColToAllLevels(50);
            numberInOrderR.Binding = nameof(NumberInOrder);
            numberInOrderR.Blocked = true;
            numberInOrderR.ChooseLine = true;
        }

        #endregion

        #region OperationCode (2)

        var operationCodeR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(OperationCode))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (operationCodeR != null)
        {
            operationCodeR.SetSizeColToAllLevels(88);
            operationCodeR.Binding = nameof(OperationCode);
            numberInOrderR += operationCodeR;
        }

        #endregion

        #region OperationDate (3)

        var operationDateR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(OperationDate))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (operationDateR != null)
        {
            operationDateR.SetSizeColToAllLevels(88);
            operationDateR.Binding = nameof(OperationDate);
            numberInOrderR += operationDateR;
        }

        #endregion

        #region IndividualNumberZHRO (4)

        var individualNumberZhroR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(IndividualNumberZHRO))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (individualNumberZhroR != null)
        {
            individualNumberZhroR.SetSizeColToAllLevels(320);
            individualNumberZhroR.Binding = nameof(IndividualNumberZHRO);
            numberInOrderR += individualNumberZhroR;
        }

        #endregion

        #region PassportNumber (5)

        var passportNumberR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(PassportNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (passportNumberR != null)
        {
            passportNumberR.SetSizeColToAllLevels(100);
            passportNumberR.Binding = nameof(PassportNumber);
            numberInOrderR += passportNumberR;
        }

        #endregion

        #region Volume6 (6)

        var volume6R = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(Volume6))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (volume6R != null)
        {
            volume6R.SetSizeColToAllLevels(88);
            volume6R.Binding = nameof(Volume6);
            numberInOrderR += volume6R;
        }

        #endregion

        #region Mass7 (7)

        var mass7R = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(Mass7))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (mass7R != null)
        {
            mass7R.SetSizeColToAllLevels(88);
            mass7R.Binding = nameof(Mass7);
            numberInOrderR += mass7R;
        }

        #endregion

        #region SaltConcentration (8)

        var saltConcentrationR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(SaltConcentration))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        saltConcentrationR.SetSizeColToAllLevels(125);
        saltConcentrationR.Binding = nameof(SaltConcentration);
        numberInOrderR += saltConcentrationR;
        
        #endregion

        #region Radionuclids (9)

        var radionuclidsR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(Radionuclids))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        radionuclidsR.SetSizeColToAllLevels(170);
        radionuclidsR.Binding = nameof(Radionuclids);
        numberInOrderR += radionuclidsR;

        #endregion

        #region SpecificActivity (10)

        var specificActivityR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(SpecificActivity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        specificActivityR.SetSizeColToAllLevels(163);
        specificActivityR.Binding = nameof(SpecificActivity);
        numberInOrderR += specificActivityR;
        
        #endregion

        #region DocumentVid (11)

        var documentVidR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentVid))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentVidR.SetSizeColToAllLevels(88);
        documentVidR.Binding = nameof(DocumentVid);
        numberInOrderR += documentVidR;
        
        #endregion

        #region DocumentNumber (12)

        var documentNumberR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentNumberR.SetSizeColToAllLevels(103);
        documentNumberR.Binding = nameof(DocumentNumber);
        numberInOrderR += documentNumberR;
        
        #endregion

        #region DocumentDate (13)

        var documentDateR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentDateR.SetSizeColToAllLevels(88);
        documentDateR.Binding = nameof(DocumentDate);
        numberInOrderR += documentDateR;
        
        #endregion

        #region ProviderOrRecieverOKPO (14)

        var providerOrRecieverOkpoR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(ProviderOrRecieverOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        providerOrRecieverOkpoR.SetSizeColToAllLevels(100);
        providerOrRecieverOkpoR.Binding = nameof(ProviderOrRecieverOKPO);
        numberInOrderR += providerOrRecieverOkpoR;
        
        #endregion

        #region TransporterOKPO (15)

        var transporterOkpoR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(TransporterOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        transporterOkpoR.SetSizeColToAllLevels(163);
        transporterOkpoR.Binding = nameof(TransporterOKPO);
        numberInOrderR += transporterOkpoR;
        
        #endregion

        #region StoragePlaceName (16)

        var storagePlaceNameR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(StoragePlaceName))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        storagePlaceNameR.SetSizeColToAllLevels(103);
        storagePlaceNameR.Binding = nameof(StoragePlaceName);
        numberInOrderR += storagePlaceNameR;
        
        #endregion

        #region StoragePlaceCode (17)

        var storagePlaceCodeR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(StoragePlaceCode))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        storagePlaceCodeR.SetSizeColToAllLevels(88);
        storagePlaceCodeR.Binding = nameof(StoragePlaceCode);
        numberInOrderR += storagePlaceCodeR;
        
        #endregion

        #region CodeRAO (18)

        var codeRaoR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(CodeRAO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        codeRaoR.SetSizeColToAllLevels(88);
        codeRaoR.Binding = nameof(CodeRAO);
        numberInOrderR += codeRaoR;
        
        #endregion

        #region StatusRAO (19)

        var statusRaoR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(StatusRAO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        statusRaoR.SetSizeColToAllLevels(88);
        statusRaoR.Binding = nameof(StatusRAO);
        numberInOrderR += statusRaoR;
        
        #endregion

        #region Volume20 (20)

        var volume20R = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(Volume20))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        volume20R.SetSizeColToAllLevels(88);
        volume20R.Binding = nameof(Volume20);
        numberInOrderR += volume20R;
        
        #endregion

        #region Mass21 (21)

        var mass21R = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(Mass21))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        mass21R.SetSizeColToAllLevels(88);
        mass21R.Binding = nameof(Mass21);
        numberInOrderR += mass21R;

        #endregion

        #region TritiumActivity (22)

        var tritiumActivityR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(TritiumActivity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        tritiumActivityR.SetSizeColToAllLevels(163);
        tritiumActivityR.Binding = nameof(TritiumActivity);
        numberInOrderR += tritiumActivityR;
        
        #endregion

        #region BetaGammaActivity (23)

        var betaGammaActivityR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(BetaGammaActivity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        betaGammaActivityR.SetSizeColToAllLevels(160);
        betaGammaActivityR.Binding = nameof(BetaGammaActivity);
        numberInOrderR += betaGammaActivityR;
        
        #endregion

        #region AlphaActivity (24)

        var alphaActivityR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(AlphaActivity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        alphaActivityR.SetSizeColToAllLevels(163);
        alphaActivityR.Binding = nameof(AlphaActivity);
        numberInOrderR += alphaActivityR;
        
        #endregion

        #region TransuraniumActivity (25)

        var transuraniumActivityR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(TransuraniumActivity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        transuraniumActivityR.SetSizeColToAllLevels(200);
        transuraniumActivityR.Binding = nameof(TransuraniumActivity);
        numberInOrderR += transuraniumActivityR;
        
        #endregion

        #region RefineOrSortRAOCode (26)

        var refineOrSortRAOCodeR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(RefineOrSortRAOCode))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        refineOrSortRAOCodeR.SetSizeColToAllLevels(120);
        refineOrSortRAOCodeR.Binding = nameof(RefineOrSortRAOCode);
        numberInOrderR += refineOrSortRAOCodeR;
        
        #endregion

        #region Subsidy (27)

        var subsidyR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(Subsidy))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        subsidyR.SetSizeColToAllLevels(88);
        subsidyR.Binding = nameof(Subsidy);
        numberInOrderR += subsidyR;

        #endregion

        #region FcpNumber (28)

        var fcpNumberR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(FcpNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        fcpNumberR.SetSizeColToAllLevels(163);
        fcpNumberR.Binding = nameof(FcpNumber);
        numberInOrderR += fcpNumberR;

        #endregion

        #region ContractNumber (29)

        var сontractNumberR = ((FormPropertyAttribute)typeof(Form18)
                .GetProperty(nameof(ContractNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        сontractNumberR.SetSizeColToAllLevels(163);
        сontractNumberR.Binding = nameof(ContractNumber);
        numberInOrderR += сontractNumberR;

        #endregion

        _DataGridColumns = numberInOrderR;
        return _DataGridColumns;
    }

    #endregion

    #region GeneratedRegex
    
    [GeneratedRegex(@"^\d{2}$")]
    private static partial Regex TwoNumRegex();

    [GeneratedRegex("^[0-9]{8}$")]
    private static partial Regex StoragePlaceCodeRegex();

    [GeneratedRegex("^[1-9]")]
    private static partial Regex StoragePlaceCodeRegex1();

    [GeneratedRegex("^[1-3]")]
    private static partial Regex StoragePlaceCodeRegex2();

    [GeneratedRegex("^[1-2]")]
    private static partial Regex StoragePlaceCodeRegex3();

    [GeneratedRegex("^[1-59]")]
    private static partial Regex StoragePlaceCodeRegex4();

    [GeneratedRegex("^[0-4]")]
    private static partial Regex StoragePlaceCodeRegex5();

    [GeneratedRegex("^[1-49]")]
    private static partial Regex StoragePlaceCodeRegex6();

    [GeneratedRegex("^[1]{1}[1-9]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1]{1}|^[4]{1}[1-49]{1}|^[5]{1}[1-69]{1}|^[6]{1}[1]{1}|^[7]{1}[1349]{1}|^[8]{1}[1-69]{1}|^[9]{1}[9]{1}")]
    private static partial Regex StoragePlaceCodeRegex7();

    [GeneratedRegex("^[0-9x+]{11}$")]
    private static partial Regex CodeRaoRegex();

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
            $"{OperationCode.Value}\t" +
            $"{OperationDate.Value}\t" +
            $"{IndividualNumberZHRO.Value}\t" +
            $"{PassportNumber.Value}\t" +
            $"{Volume6.Value}\t" +
            $"{Mass7.Value}\t" +
            $"{SaltConcentration.Value}\t" +
            $"{Radionuclids.Value}\t" +
            $"{SpecificActivity.Value}\t" +
            $"{DocumentVid.Value}\t" +
            $"{DocumentNumber.Value}\t" +
            $"{DocumentDate.Value}\t" +
            $"{ProviderOrRecieverOKPO.Value}\t" +
            $"{TransporterOKPO.Value}\t" +
            $"{StoragePlaceName.Value}\t" +
            $"{StoragePlaceCode.Value}\t" +
            $"{CodeRAO.Value}\t" +
            $"{StatusRAO.Value}\t" +
            $"{Volume20.Value}\t" +
            $"{Mass21.Value}\t" +
            $"{TritiumActivity.Value}\t" +
            $"{BetaGammaActivity.Value}\t" +
            $"{AlphaActivity.Value}\t" +
            $"{TransuraniumActivity.Value}\t" +
            $"{RefineOrSortRAOCode.Value}\t" +
            $"{Subsidy.Value}\t" +
            $"{FcpNumber.Value}\t" +
            $"{ContractNumber.Value}";
        return str;
    }

    #endregion
}