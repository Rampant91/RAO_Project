using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;
using Spravochniki;

namespace Models.Forms.Form1;

[Form_Class("Форма 1.7: Сведения о твердых кондиционированных РАО")]
[Table (name: "form_17")]
public partial class Form17 : Form1
{
    #region Constructor
    
    public Form17()
    {
        FormNum.Value = "1.7";
        Validate_all();
    }

    #endregion

    #region Validation

    private void Validate_all()
    {
        CodeRAO_Validation(CodeRAO);
        PackName_Validation(PackName);
        PackNumber_Validation(PackNumber);
        PackFactoryNumber_Validation(PackFactoryNumber);
        PackType_Validation(PackType);
        Volume_Validation(Volume);
        Mass_Validation(Mass);
        Radionuclids_Validation(Radionuclids);
        ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
        TransporterOKPO_Validation(TransporterOKPO);
        TritiumActivity_Validation(TritiumActivity);
        BetaGammaActivity_Validation(BetaGammaActivity);
        AlphaActivity_Validation(AlphaActivity);
        TransuraniumActivity_Validation(TransuraniumActivity);
        FormingDate_Validation(FormingDate);
        PassportNumber_Validation(PassportNumber);
        RefineOrSortRAOCode_Validation(RefineOrSortRAOCode);
        Subsidy_Validation(Subsidy);
        FcpNumber_Validation(FcpNumber);
        StatusRAO_Validation(StatusRAO);
        VolumeOutOfPack_Validation(VolumeOutOfPack);
        MassOutOfPack_Validation(MassOutOfPack);
        StoragePlaceName_Validation(StoragePlaceName);
        StoragePlaceCode_Validation(StoragePlaceCode);
        SpecificActivity_Validation(SpecificActivity);
        Quantity_Validation(Quantity);
    }

    public override bool Object_Validation()
    {
        return !(CodeRAO.HasErrors ||
                 PackName.HasErrors ||
                 PackNumber.HasErrors ||
                 PackFactoryNumber.HasErrors ||
                 PackType.HasErrors ||
                 Volume.HasErrors ||
                 Mass.HasErrors ||
                 Radionuclids.HasErrors ||
                 ProviderOrRecieverOKPO.HasErrors ||
                 TransporterOKPO.HasErrors ||
                 TritiumActivity.HasErrors ||
                 BetaGammaActivity.HasErrors ||
                 AlphaActivity.HasErrors ||
                 TransuraniumActivity.HasErrors ||
                 FormingDate.HasErrors ||
                 PassportNumber.HasErrors ||
                 RefineOrSortRAOCode.HasErrors ||
                 Subsidy.HasErrors ||
                 FcpNumber.HasErrors ||
                 StatusRAO.HasErrors ||
                 VolumeOutOfPack.HasErrors ||
                 MassOutOfPack.HasErrors ||
                 StoragePlaceName.HasErrors ||
                 StoragePlaceCode.HasErrors ||
                 SpecificActivity.HasErrors ||
                 Quantity.HasErrors);
    }

    #endregion

    #region Properties

    #region  Sum

    // ReSharper disable once MemberCanBePrivate.Global - не делай private, сломается выгрузка форм 1.7
    public bool Sum_DB { get; set; }

    [NotMapped]
    public RamAccess<bool> Sum
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Sum), out RamAccess value))
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

    #region OpecationCode (2)

    [NotMapped]
    [FormProperty(true, "null-1-1", "Сведения об операции", "код", "2")]
    public override RamAccess<string> OperationCode
    {
        get
        {
            if (!OperationCode_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(OperationCode), out var value))
                {
                    ((RamAccess<string>)value).Value = OperationCode_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(OperationCode_Validation, OperationCode_DB);
                rm.PropertyChanged += OperationCode_ValueChanged;
                Dictionary.Add(nameof(OperationCode), rm);
                return (RamAccess<string>)Dictionary[nameof(OperationCode)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!OperationCode_Hidden_Priv)
            {
                OperationCode_DB = value.Value;
                OnPropertyChanged();
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
            || byteValue is not (1 or 10 or 18 or >= 21 and <= 29 or >= 31 and <= 39 or 51 or 52 or 55 or 63 or 64 or 68 or 97 or 98 or 99))
        {
            value.AddError("Код операции не может быть использован в форме 1.7");
            return false;
        }

        return true;
    }

    private protected override void OperationCode_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;

        var value1 = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        if (OperationCode_DB != value1)
        {
            OperationCode_DB = value1;
            if (Report is { AutoReplace: true })
            {
                AutoReplaceByOpCode(value1);
            }
                
        }
    }

    #region AutoRplaceByOpCode

    private void AutoReplaceByOpCode(string opCode)
    {
        const string dash = "-";
        var masterOkpo = Report?.Reports?.Master_DB?.OkpoRep.Value ?? string.Empty;
        switch (opCode)
        {
            #region 10, 18, 43, 51, 52, 68, 97, 98

            case "10" or "18" or "43" or "51" or "52" or "68" or "97" or "98":
            {
                #region ProviderOrRecieverOKPO (17)

                if (!string.IsNullOrWhiteSpace(masterOkpo)
                    && ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (18)

                if (TransporterOKPO_DB != dash)
                {
                    TransporterOKPO.Value = dash;
                }

                #endregion

                #region RefineOrSortRAOCode (30)

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
                #region ProviderOrRecieverOKPO (17)

                if (!string.IsNullOrWhiteSpace(masterOkpo)
                    && ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (18)

                if (TransporterOKPO_DB != dash)
                {
                    TransporterOKPO.Value = dash;
                }

                #endregion

                #region StatusRAO (22)

                if (!string.IsNullOrWhiteSpace(masterOkpo)
                    && StatusRAO_DB != masterOkpo)
                {
                    StatusRAO.Value = masterOkpo;
                }

                #endregion

                #region RefineOrSortRAOCode (30)

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
                #region RefineOrSortRAOCode (30)

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
                #region ProviderOrRecieverOKPO (17)

                if (!string.IsNullOrWhiteSpace(masterOkpo)
                    && ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (18)

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

    [NotMapped]
    [FormProperty(true, "null-1-1", "Сведения об операции", "дата", "3")]
    public override RamAccess<string> OperationDate
    {
        get
        {
            if (!OperationDate_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(OperationDate), out RamAccess value))
                {
                    ((RamAccess<string>)value).Value = OperationDate_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(OperationDate_Validation, OperationDate_DB);
                rm.PropertyChanged += OperationDate_ValueChanged;
                Dictionary.Add(nameof(OperationDate), rm);
                return (RamAccess<string>)Dictionary[nameof(OperationDate)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!OperationDate_Hidden_Priv)
            {
                OperationDate_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    protected override bool OperationDate_Validation(RamAccess<string> value)
        => string.IsNullOrWhiteSpace(value.Value) || DateString_Validation(value);

    #endregion

    #region PackName (4)

    public string PackName_DB { get; set; } = "";

    //нельзя делать private
    public bool PackName_Hidden_Priv { get; set; }

    [NotMapped]
    public bool PackName_Hidden
    {
        get => PackName_Hidden_Priv;
        set => PackName_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-4", "наименование", "4")]
    public RamAccess<string> PackName
    {
        get
        {
            if (!PackName_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(PackName), out var value))
                {
                    ((RamAccess<string>)value).Value = PackName_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(PackName_Validation, PackName_DB);
                rm.PropertyChanged += PackName_ValueChanged;
                Dictionary.Add(nameof(PackName), rm);
                return (RamAccess<string>)Dictionary[nameof(PackName)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!PackName_Hidden_Priv)
            {
                PackName_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void PackName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        PackName_DB = tmp;
    }

    private static bool PackName_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        //List<string> spr = new List<string>();
        //if (!spr.Contains(value.Value))
        //{
        //    value.AddError("Недопустимое значение");
        //    return false;
        //}
        return true;
    }

    #endregion

    #region PackType (5)

    public string PackType_DB { get; set; } = "";

    //нельзя делать private
    public bool PackType_Hidden_Priv { get; set; }

    [NotMapped]
    public bool PackType_Hidden
    {
        get => PackType_Hidden_Priv;
        set => PackType_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-5", "тип", "5")]
    public RamAccess<string> PackType
    {
        get
        {
            if (!PackType_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(PackType), out var value))
                {
                    ((RamAccess<string>)value).Value = PackType_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(PackType_Validation, PackType_DB);
                rm.PropertyChanged += PackType_ValueChanged;
                Dictionary.Add(nameof(PackType), rm);
                return (RamAccess<string>)Dictionary[nameof(PackType)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!PackType_Hidden_Priv)
            {
                PackType_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void PackType_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        PackType_DB = tmp;
    }

    private static bool PackType_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region PackFactoryNumber (6)

    public string PackFactoryNumber_DB { get; set; } = "";

    //нельзя делать private
    public bool PackFactoryNumber_Hidden_Priv { get; set; }

    [NotMapped]
    public bool PackFactoryNumber_Hidden
    {
        get => PackFactoryNumber_Hidden_Priv;
        set => PackFactoryNumber_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-6", "заводской номер", "6")]
    public RamAccess<string> PackFactoryNumber
    {
        get
        {
            if (!PackFactoryNumber_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(PackFactoryNumber), out var value))
                {
                    ((RamAccess<string>)value).Value = PackFactoryNumber_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(PackFactoryNumber_Validation, PackFactoryNumber_DB);
                rm.PropertyChanged += PackFactoryNumber_ValueChanged;
                Dictionary.Add(nameof(PackFactoryNumber), rm);
                return (RamAccess<string>)Dictionary[nameof(PackFactoryNumber)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!PackFactoryNumber_Hidden_Priv)
            {
                PackFactoryNumber_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void PackFactoryNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        PackFactoryNumber_DB = tmp;
    }

    private static bool PackFactoryNumber_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region PackNumber (7)

    public string PackNumber_DB { get; set; } = "";

    //нельзя делать private
    public bool PackNumber_Hidden_Priv { get; set; }

    [NotMapped]
    public bool PackNumber_Hidden
    {
        get => PackNumber_Hidden_Priv;
        set => PackNumber_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-7", "номер упаковки (идентификационный код)", "7")]
    public RamAccess<string> PackNumber
    {
        get
        {
            if (!PackNumber_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(PackNumber), out var value))
                {
                    ((RamAccess<string>)value).Value = PackNumber_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(PackNumber_Validation, PackNumber_DB);
                rm.PropertyChanged += PackNumber_ValueChanged;
                Dictionary.Add(nameof(PackNumber), rm);
                return (RamAccess<string>)Dictionary[nameof(PackNumber)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!PackNumber_Hidden_Priv)
            {
                PackNumber_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void PackNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        PackNumber_DB = tmp;
    }

    private static bool PackNumber_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region FormingDate (8)

    public string FormingDate_DB { get; set; } = "";

    //нельзя делать private
    public bool FormingDate_Hidden_Priv { get; set; }

    [NotMapped]
    public bool FormingDate_Hidden
    {
        get => FormingDate_Hidden_Priv;
        set => FormingDate_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-8", "дата формирования", "8")]
    public RamAccess<string> FormingDate
    {
        get
        {
            if (!FormingDate_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(FormingDate), out var value))
                {
                    ((RamAccess<string>)value).Value = FormingDate_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(FormingDate_Validation, FormingDate_DB);
                rm.PropertyChanged += FormingDate_ValueChanged;
                Dictionary.Add(nameof(FormingDate), rm);
                return (RamAccess<string>)Dictionary[nameof(FormingDate)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!FormingDate_Hidden_Priv)
            {
                FormingDate_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void FormingDate_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        FormingDate_DB = DateString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool FormingDate_Validation(RamAccess<string> value) 
        => string.IsNullOrWhiteSpace(value.Value) || DateString_Validation(value);

    #endregion

    #region PassportNumber (9)

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
    [FormProperty(true, "Сведения об упаковке", "null-9", "номер паспорта", "9")]
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
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        PassportNumber_DB = tmp;
    }

    private static bool PassportNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region Volume (10)

    public string Volume_DB { get; set; } = "";

    //нельзя делать private
    public bool Volume_Hidden_Priv { get; set; }

    [NotMapped]
    public bool Volume_Hidden
    {
        get => Volume_Hidden_Priv;
        set => Volume_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-10", "объем, куб. м", "10")]
    public RamAccess<string> Volume
    {
        get
        {
            if (!Volume_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(Volume), out var value))
                {
                    ((RamAccess<string>)value).Value = Volume_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Volume_Validation, Volume_DB);
                rm.PropertyChanged += Volume_ValueChanged;
                Dictionary.Add(nameof(Volume), rm);
                return (RamAccess<string>)Dictionary[nameof(Volume)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!Volume_Hidden_Priv)
            {
                Volume_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void Volume_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        Volume_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool Volume_Validation(RamAccess<string> value) 
        => string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region Mass (11)

    public string Mass_DB { get; set; } = "";

    //нельзя делать private
    public bool Mass_Hidden_Priv { get; set; }

    [NotMapped]
    public bool Mass_Hidden
    {
        get => Mass_Hidden_Priv;
        set => Mass_Hidden_Priv = value;
    }

    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-11", "масса брутто, т", "11")]
    public RamAccess<string> Mass
    {
        get
        {
            if (!Mass_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(Mass), out var value))
                {
                    ((RamAccess<string>)value).Value = Mass_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Mass_Validation, Mass_DB);
                rm.PropertyChanged += Mass_ValueChanged;
                Dictionary.Add(nameof(Mass), rm);
                return (RamAccess<string>)Dictionary[nameof(Mass)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!Mass_Hidden_Priv)
            {
                Mass_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void Mass_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        Mass_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool Mass_Validation(RamAccess<string> value) 
        => string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region Radionuclids (12)

    public string Radionuclids_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "Радионуклидный состав", "наименование радионуклида", "12")]
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
        }
        set
        {
            Radionuclids_DB = value.Value;
            OnPropertyChanged();
        }
    }//If change this change validation

    private void Radionuclids_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        Radionuclids_DB = tmp;
    }

    private static bool Radionuclids_Validation(RamAccess<string> value) => NuclidString_Validation(value);

    #endregion

    #region SpecificActivity (13)

    public string SpecificActivity_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "Радионуклидный состав", "удельная активность, Бк/г", "13")]
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

    private static bool SpecificActivity_Validation(RamAccess<string> value) 
        => string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region DocumentVid (14)

    [NotMapped]
    [FormProperty(true, "null-n", "Документ", "вид", "14")]
    public override RamAccess<byte?> DocumentVid
    {
        get
        {
            if (!DocumentVid_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(DocumentVid), out RamAccess value))
                {
                    ((RamAccess<byte?>)value).Value = DocumentVid_DB;
                    return (RamAccess<byte?>)value;
                }
                var rm = new RamAccess<byte?>(DocumentVid_Validation, DocumentVid_DB);
                rm.PropertyChanged += DocumentVid_ValueChanged;
                Dictionary.Add(nameof(DocumentVid), rm);
                return (RamAccess<byte?>)Dictionary[nameof(DocumentVid)];
            }
            var tmp = new RamAccess<byte?>(null, null);
            return tmp;
        }
        set
        {
            DocumentVid_DB = value.Value;
            OnPropertyChanged();
        }
    }

    protected override bool DocumentVid_Validation(RamAccess<byte?> value)
    {
        value.ClearErrors();
        if (Spravochniks.SprDocumentVidName.Count != 0)
        {
            return false;
        }
        value.AddError("Недопустимое значение");
        return true;
    }

    #endregion

    #region DocumentNumber (15)

    [NotMapped]
    [FormProperty(true, "null-n", "Документ", "номер", "15")]
    public override RamAccess<string> DocumentNumber
    {
        get
        {
            if (!DocumentNumber_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(DocumentNumber), out RamAccess value))
                {
                    ((RamAccess<string>)value).Value = DocumentNumber_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(DocumentNumber_Validation, DocumentNumber_DB);
                rm.PropertyChanged += DocumentNumber_ValueChanged;
                Dictionary.Add(nameof(DocumentNumber), rm);
                return (RamAccess<string>)Dictionary[nameof(DocumentNumber)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!DocumentNumber_Hidden_Priv)
            {
                DocumentNumber_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    protected override bool DocumentNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region DocumentDate (16)

    [NotMapped]
    [FormProperty(true, "null-n", "Документ", "дата", "16")]
    public override RamAccess<string> DocumentDate
    {
        get
        {
            if (!DocumentDate_Hidden_Priv)
            {
                if (Dictionary.TryGetValue(nameof(DocumentDate), out RamAccess value))
                {
                    ((RamAccess<string>)value).Value = DocumentDate_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(DocumentDate_Validation, DocumentDate_DB);
                rm.PropertyChanged += DocumentDate_ValueChanged;
                Dictionary.Add(nameof(DocumentDate), rm);
                return (RamAccess<string>)Dictionary[nameof(DocumentDate)];
            }
            var tmp = new RamAccess<string>(null, null);
            return tmp;
        }
        set
        {
            if (!DocumentDate_Hidden_Priv)
            {
                DocumentDate_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    protected override bool DocumentDate_Validation(RamAccess<string> value)
        => value.Value is null or "" || DateString_Validation(value);

    #endregion

    #region ProviderOrRecieverOKPO (17)

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
    [FormProperty(true, "null-17-18", "ОКПО", "поставщика или получателя", "17")]
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
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
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
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            return true;
        }
        if (value.Value.Equals("Минобороны"))
        {
            return true;
        }
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()))
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

    #region TransporterOKPO (18)

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
    [FormProperty(true, "null-17-18", "ОКПО", "перевозчика", "18")]
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
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        TransporterOKPO_DB = tmp;
    }

    private static bool TransporterOKPO_Validation(RamAccess<string> value)//Done
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        if (value.Value.Equals("Минобороны") || value.Value.Equals("-"))
        {
            return true;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((TransporterOKPONote.Value == null) || TransporterOKPONote.Value.Equals(""))
            //    value.AddError( "Заполните примечание");
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

    #region StoragePlaceName (19)

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
    [FormProperty(true, "null-19-20", "Пункт хранения", "наименование", "19")]
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
            if (!StoragePlaceName_Hidden_Priv)
            {
                StoragePlaceName_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void StoragePlaceName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
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

    #region StoragePlaceCode (20)

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
    [FormProperty(true, "null-19-20", "Пункт хранения", "код", "20")]
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
            if (!StoragePlaceCode_Hidden_Priv)
            {
                StoragePlaceCode_DB = value.Value;
                OnPropertyChanged();
            }
        }
    }

    private void StoragePlaceCode_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
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
        var tmp = value.Value.Trim();
        if (!StoragePlaceCodeRegex().IsMatch(tmp))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
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

    #region CodeRAO (21)

    public string CodeRAO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "null-21", "код", "21")]
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
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty)
            .Trim()
            .ToLower()
            .Replace("х", "x");
        CodeRAO_DB = tmp;
    }

    private static bool CodeRAO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value
            .ToLower()
            .Replace("х", "x");
        if (!CodeRaoRegex().IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region StatusRAO (22)

    public string StatusRAO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "null-22", "статус", "22")]
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
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        StatusRAO_DB = tmp;
    }

    private static bool StatusRAO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        if (value.Value.Length == 1)
        {
            if (!int.TryParse(value.Value, out var intValue) || intValue is not (>= 1 and <= 4 or 6 or 9))
            {
                value.AddError("Недопустимое значение"); 
                return false;
            }
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

    #region VolumeOutOfPack (23)

    public string VolumeOutOfPack_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "null-23", "объем без упаковки, куб. м", "23")]
    public RamAccess<string> VolumeOutOfPack
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(VolumeOutOfPack), out var value))
            {
                ((RamAccess<string>)value).Value = VolumeOutOfPack_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(VolumeOutOfPack_Validation, VolumeOutOfPack_DB);
            rm.PropertyChanged += VolumeOutOfPack_ValueChanged;
            Dictionary.Add(nameof(VolumeOutOfPack), rm);
            return (RamAccess<string>)Dictionary[nameof(VolumeOutOfPack)];
        }
        set
        {
            VolumeOutOfPack_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void VolumeOutOfPack_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        VolumeOutOfPack_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool VolumeOutOfPack_Validation(RamAccess<string> value) 
        => string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region MassOutOfPack (24)

    public string MassOutOfPack_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "null-24", "масса без упаковки (нетто), т", "24")]
    public RamAccess<string> MassOutOfPack
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MassOutOfPack), out RamAccess value))
            {
                ((RamAccess<string>)value).Value = MassOutOfPack_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(MassOutOfPack_Validation, MassOutOfPack_DB);
            rm.PropertyChanged += MassOutOfPack_ValueChanged;
            Dictionary.Add(nameof(MassOutOfPack), rm);
            return (RamAccess<string>)Dictionary[nameof(MassOutOfPack)];
        }
        set
        {
            MassOutOfPack_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void MassOutOfPack_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        MassOutOfPack_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool MassOutOfPack_Validation(RamAccess<string> value) 
        => string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region Quantity (25)

    public string Quantity_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "null-25", "количество ОЗИИИ, шт", "25")]
    public RamAccess<string> Quantity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Quantity), out RamAccess value))
            {
                ((RamAccess<string>)value).Value = Quantity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Quantity_Validation, Quantity_DB);
            rm.PropertyChanged += Quantity_ValueChanged;
            Dictionary.Add(nameof(Quantity), rm);
            return (RamAccess<string>)Dictionary[nameof(Quantity)];
        }
        set
        {
            Quantity_DB = value.Value;
            OnPropertyChanged();
        }
    }// positive int.

    private void Quantity_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        Quantity_DB = tmp;
    }

    private static bool Quantity_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (value.Value is null or "-" or "") 
        {
            return true;
        }
        var tmp = value.Value.Trim();
        if (!int.TryParse(tmp, out var intValue))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (intValue <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }

    #endregion

    #region TritiumActivity (26)

    public string TritiumActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "Суммарная активность", "тритий", "26")]
    public RamAccess<string> TritiumActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(TritiumActivity), out RamAccess value))
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

    private static bool TritiumActivity_Validation(RamAccess<string> value) 
        => string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region BetaGammaActivity (27)

    public string BetaGammaActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "Суммарная активность", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "27")]
    public RamAccess<string> BetaGammaActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(BetaGammaActivity), out RamAccess value))
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

    private static bool BetaGammaActivity_Validation(RamAccess<string> value) 
        => string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region AlphaActivity (28)

    public string AlphaActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "Суммарная активность", "альфа-излучающие радионуклиды (исключая трансурановые)", "28")]
    public RamAccess<string> AlphaActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(AlphaActivity), out RamAccess value))
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

    private static bool AlphaActivity_Validation(RamAccess<string> value) 
        => string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region TransuraniumActivity (29)

    public string TransuraniumActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "Суммарная активность", "трансурановые радионуклиды", "29")]
    public RamAccess<string> TransuraniumActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(TransuraniumActivity), out RamAccess value))
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

    private static bool TransuraniumActivity_Validation(RamAccess<string> value) 
        => string.IsNullOrWhiteSpace(value.Value) || ExponentialString_Validation(value);

    #endregion

    #region RefineOrSortRAOCode (30)

    public string RefineOrSortRAOCode_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "null-30", "Код переработки/сортировки РАО", "30")]
    public RamAccess<string> RefineOrSortRAOCode
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(RefineOrSortRAOCode), out RamAccess value))
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
    }

    private void RefineOrSortRAOCode_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        RefineOrSortRAOCode_DB = tmp;
    }

    private static bool RefineOrSortRAOCode_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (value.Value is null or "" or "-")
        {
            return true;
        }
        if (!Spravochniks.SprRefineOrSortCodes.Contains(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region Subsidy (31)

    public string Subsidy_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-31-1", "null-31-2", "Субсидия, %", "31")]
    public RamAccess<string> Subsidy // 0<number<=100 or empty.
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Subsidy), out RamAccess value))
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
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
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

    #region FcpNumber (32)

    public string FcpNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-32_1", "null-32_2", "Номер мероприятия ФЦП", "32")]
    public RamAccess<string> FcpNumber
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(FcpNumber), out RamAccess value))
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
            OnPropertyChanged();
        }
    }

    private void FcpNumberValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        FcpNumber_DB = tmp;
    }

    private static bool FcpNumber_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        return true;
    }

    #endregion 

    #region ContractNumber (33)

    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string ContractNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-33", "Договор", "номер", "33")]
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
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
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
        PackName_DB = Convert.ToString(worksheet.Cells[row, 4].Value);
        PackType_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        PackFactoryNumber_DB = Convert.ToString(worksheet.Cells[row, 6].Value);
        PackNumber_DB = Convert.ToString(worksheet.Cells[row, 7].Value);
        FormingDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 8].Text);
        PassportNumber_DB = Convert.ToString(worksheet.Cells[row, 9].Value);
        Volume_DB = ConvertFromExcelDouble(worksheet.Cells[row, 10].Value);
        Mass_DB = ConvertFromExcelDouble(worksheet.Cells[row, 11].Value);
        Radionuclids_DB = Convert.ToString(worksheet.Cells[row, 12].Value);
        SpecificActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 13].Value);
        DocumentVid_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 14].Value), out var byteValue) ? byteValue : null;
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[row, 15].Value);
        DocumentDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 16].Text);
        ProviderOrRecieverOKPO_DB = Convert.ToString(worksheet.Cells[row, 17].Value);
        TransporterOKPO_DB = Convert.ToString(worksheet.Cells[row, 18].Value);
        StoragePlaceName_DB = Convert.ToString(worksheet.Cells[row, 19].Value);
        StoragePlaceCode_DB = Convert.ToString(worksheet.Cells[row, 20].Value);
        CodeRAO_DB = Convert.ToString(worksheet.Cells[row, 21].Value);
        StatusRAO_DB = Convert.ToString(worksheet.Cells[row, 22].Value);
        VolumeOutOfPack_DB = ConvertFromExcelDouble(worksheet.Cells[row, 23].Value);
        MassOutOfPack_DB = ConvertFromExcelDouble(worksheet.Cells[row, 24].Value);
        Quantity_DB = ConvertFromExcelInt(worksheet.Cells[row, 25].Value);
        TritiumActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 26].Value);
        BetaGammaActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 27].Value);
        AlphaActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 28].Value);
        TransuraniumActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 29].Value);
        RefineOrSortRAOCode_DB = Convert.ToString(worksheet.Cells[row, 30].Value);
        Subsidy_DB = Convert.ToString(worksheet.Cells[row, 31].Value);
        FcpNumber_DB = Convert.ToString(worksheet.Cells[row, 32].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row + 0, column + 0].Value = ConvertToExcelString(PackName_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(PackType_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelString(PackFactoryNumber_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelString(PackNumber_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelDate(FormingDate_DB, worksheet, row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0));
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelString(PassportNumber_DB);
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDouble(Volume_DB);
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelDouble(Mass_DB);
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ConvertToExcelString(Radionuclids_DB);
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ConvertToExcelDouble(SpecificActivity_DB);
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = DocumentVid_DB is null ? "-" : DocumentVid_DB;
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ConvertToExcelString(DocumentNumber_DB);
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ConvertToExcelDate(DocumentDate_DB, worksheet, row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0));
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ConvertToExcelString(ProviderOrRecieverOKPO_DB);
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ConvertToExcelString(TransporterOKPO_DB);
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ConvertToExcelString(StoragePlaceName_DB);
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ConvertToExcelString(StoragePlaceCode_DB);
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ConvertToExcelString(CodeRAO_DB);
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ConvertToExcelString(StatusRAO_DB);
        worksheet.Cells[row + (!transpose ? 19 : 0), column + (transpose ? 19 : 0)].Value = ConvertToExcelDouble(VolumeOutOfPack_DB);
        worksheet.Cells[row + (!transpose ? 20 : 0), column + (transpose ? 20 : 0)].Value = ConvertToExcelDouble(MassOutOfPack_DB);
        worksheet.Cells[row + (!transpose ? 21 : 0), column + (transpose ? 21 : 0)].Value = ConvertToExcelInt(Quantity_DB);
        worksheet.Cells[row + (!transpose ? 22 : 0), column + (transpose ? 22 : 0)].Value = ConvertToExcelDouble(TritiumActivity_DB);
        worksheet.Cells[row + (!transpose ? 23 : 0), column + (transpose ? 23 : 0)].Value = ConvertToExcelDouble(BetaGammaActivity_DB);
        worksheet.Cells[row + (!transpose ? 24 : 0), column + (transpose ? 24 : 0)].Value = ConvertToExcelDouble(AlphaActivity_DB);
        worksheet.Cells[row + (!transpose ? 25 : 0), column + (transpose ? 25 : 0)].Value = ConvertToExcelDouble(TransuraniumActivity_DB);
        worksheet.Cells[row + (!transpose ? 26 : 0), column + (transpose ? 26 : 0)].Value = ConvertToExcelString(RefineOrSortRAOCode_DB);
        worksheet.Cells[row + (!transpose ? 27 : 0), column + (transpose ? 27 : 0)].Value = ConvertToExcelString(Subsidy_DB);
        worksheet.Cells[row + (!transpose ? 28 : 0), column + (transpose ? 28 : 0)].Value = ConvertToExcelString(FcpNumber_DB);
        if (worksheet.Name is "Отчеты 1.7")
        {
            worksheet.Cells[row + (!transpose ? 29 : 0), column + (transpose ? 29 : 0)].Value = ConvertToExcelString(ContractNumber_DB);
        }

        return 30;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form1.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row + 0, column + 0].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(PackName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(PackType))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(PackFactoryNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(PackNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(FormingDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(PassportNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(Volume))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(Mass))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(Radionuclids))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(SpecificActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(DocumentVid))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(DocumentNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(DocumentDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(ProviderOrRecieverOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(TransporterOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(StoragePlaceName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(StoragePlaceCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(CodeRAO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(StatusRAO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 19 : 0), column + (transpose ? 19 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(VolumeOutOfPack))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 20 : 0), column + (transpose ? 20 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(MassOutOfPack))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 21 : 0), column + (transpose ? 21 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(Quantity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 22 : 0), column + (transpose ? 22 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(TritiumActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 23 : 0), column + (transpose ? 23 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(BetaGammaActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 24 : 0), column + (transpose ? 24 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(AlphaActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 25 : 0), column + (transpose ? 25 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(TransuraniumActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 26 : 0), column + (transpose ? 26 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(RefineOrSortRAOCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 27 : 0), column + (transpose ? 27 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(Subsidy))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 28 : 0), column + (transpose ? 28 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(FcpNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 29 : 0), column + (transpose ? 29 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(ContractNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        
        return 30;
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

        #region PackName (4)

        var packNameR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(PackName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (packNameR != null)
        {
            packNameR.SetSizeColToAllLevels(163);
            packNameR.Binding = nameof(PackName);
            numberInOrderR += packNameR;
        }

        #endregion

        #region PackType (5)

        var packTypeR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(PackType))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (packTypeR != null)
        {
            packTypeR.SetSizeColToAllLevels(88);
            packTypeR.Binding = nameof(PackType);
            numberInOrderR += packTypeR;
        }

        #endregion

        #region PackFactoryNumber (6)

        var packFactoryNumberR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(PackFactoryNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (packFactoryNumberR != null)
        {
            packFactoryNumberR.SetSizeColToAllLevels(170);
            packFactoryNumberR.Binding = nameof(PackFactoryNumber);
            numberInOrderR += packFactoryNumberR;
        }

        #endregion

        #region PackNumber (7)

        var packNumberR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(PackNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (packNumberR != null)
        {
            packNumberR.SetSizeColToAllLevels(260);
            packNumberR.Binding = nameof(PackNumber);
            numberInOrderR += packNumberR;
        }

        #endregion

        #region FormingDate (8)

        var formingDateR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(FormingDate))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        formingDateR.SetSizeColToAllLevels(133);
        formingDateR.Binding = nameof(FormingDate);
        numberInOrderR += formingDateR;
        
        #endregion

        #region PassportNumber (9)

        var passportNumberR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(PassportNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        passportNumberR.SetSizeColToAllLevels(143);
        passportNumberR.Binding = nameof(PassportNumber);
        numberInOrderR += passportNumberR;

        #endregion

        #region Volume (10)

        var volumeR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(Volume))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        volumeR.SetSizeColToAllLevels(88);
        volumeR.Binding = nameof(Volume);
        numberInOrderR += volumeR;
        
        #endregion

        #region Mass (11)

        var massR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(Mass))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        massR.SetSizeColToAllLevels(95);
        massR.Binding = nameof(Mass);
        numberInOrderR += massR;
        
        #endregion

        #region Radionuclids (12)

        var radionuclidsR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(Radionuclids))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        radionuclidsR.SetSizeColToAllLevels(170);
        radionuclidsR.Binding = nameof(Radionuclids);
        numberInOrderR += radionuclidsR;
        
        #endregion

        #region SpecificActivity (13)

        var specificActivityR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(SpecificActivity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        specificActivityR.SetSizeColToAllLevels(163);
        specificActivityR.Binding = nameof(SpecificActivity);
        numberInOrderR += specificActivityR;
        
        #endregion

        #region DocumentVid (14)

        var documentVidR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentVid))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentVidR.SetSizeColToAllLevels(88);
        documentVidR.Binding = nameof(DocumentVid);
        numberInOrderR += documentVidR;
        
        #endregion

        #region DocumentNumber (15)

        var documentNumberR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentNumberR.SetSizeColToAllLevels(103);
        documentNumberR.Binding = nameof(DocumentNumber);
        numberInOrderR += documentNumberR;

        #endregion

        #region DocumentDate (16)

        var documentDateR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentDate))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentDateR.SetSizeColToAllLevels(88);
        documentDateR.Binding = nameof(DocumentDate);
        numberInOrderR += documentDateR;

        #endregion

        #region ProviderOrRecieverOKPO (17)

        var providerOrRecieverOkpoR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(ProviderOrRecieverOKPO))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        providerOrRecieverOkpoR.SetSizeColToAllLevels(100);
        providerOrRecieverOkpoR.Binding = nameof(ProviderOrRecieverOKPO);
        numberInOrderR += providerOrRecieverOkpoR;

        #endregion

        #region TransporterOKPO (18)

        var transporterOkpoR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(TransporterOKPO))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        transporterOkpoR.SetSizeColToAllLevels(163);
        transporterOkpoR.Binding = nameof(TransporterOKPO);
        numberInOrderR += transporterOkpoR;

        #endregion

        #region StoragePlaceName (19)

        var storagePlaceNameR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(StoragePlaceName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        storagePlaceNameR.SetSizeColToAllLevels(103);
        storagePlaceNameR.Binding = nameof(StoragePlaceName);
        numberInOrderR += storagePlaceNameR;

        #endregion

        #region StoragePlaceCode (20)

        var storagePlaceCodeR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(StoragePlaceCode))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        storagePlaceCodeR.SetSizeColToAllLevels(88);
        storagePlaceCodeR.Binding = nameof(StoragePlaceCode);
        numberInOrderR += storagePlaceCodeR;

        #endregion

        #region CodeRAO (21)

        var codeRaoR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(CodeRAO))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        codeRaoR.SetSizeColToAllLevels(88);
        codeRaoR.Binding = nameof(CodeRAO);
        numberInOrderR += codeRaoR;

        #endregion

        #region StatusRAO (22)

        var statusRaoR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(StatusRAO))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        statusRaoR.SetSizeColToAllLevels(88);
        statusRaoR.Binding = nameof(StatusRAO);
        numberInOrderR += statusRaoR;

        #endregion

        #region VolumeOutOfPack (23)

        var volumeOutOfPackR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(VolumeOutOfPack))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        volumeOutOfPackR.SetSizeColToAllLevels(163);
        volumeOutOfPackR.Binding = nameof(VolumeOutOfPack);
        numberInOrderR += volumeOutOfPackR;

        #endregion

        #region MassOutOfPack (24)

        var massOutOfPackR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(MassOutOfPack))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        massOutOfPackR.SetSizeColToAllLevels(170);
        massOutOfPackR.Binding = nameof(MassOutOfPack);
        numberInOrderR += massOutOfPackR;

        #endregion

        #region Quantity (25)

        var quantityR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(Quantity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        quantityR.SetSizeColToAllLevels(140);
        quantityR.Binding = nameof(Quantity);
        numberInOrderR += quantityR;

        #endregion

        #region TritiumActivity (26)

        var tritiumActivityR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(TritiumActivity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        tritiumActivityR.SetSizeColToAllLevels(163);
        tritiumActivityR.Binding = nameof(TritiumActivity);
        numberInOrderR += tritiumActivityR;

        #endregion

        #region BetaGammaActivity (27)

        var betaGammaActivityR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(BetaGammaActivity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        betaGammaActivityR.SetSizeColToAllLevels(180);
        betaGammaActivityR.Binding = nameof(BetaGammaActivity);
        numberInOrderR += betaGammaActivityR;

        #endregion

        #region AlphaActivity (28)

        var alphaActivityR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(AlphaActivity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        alphaActivityR.SetSizeColToAllLevels(185);
        alphaActivityR.Binding = nameof(AlphaActivity);
        numberInOrderR += alphaActivityR;

        #endregion

        #region TransuraniumActivity (29)

        var transuraniumActivityR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(TransuraniumActivity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        transuraniumActivityR.SetSizeColToAllLevels(200);
        transuraniumActivityR.Binding = nameof(TransuraniumActivity);
        numberInOrderR += transuraniumActivityR;

        #endregion

        #region RefineOrSortRAOCode (30)

        var refineOrSortRaoCodeR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(RefineOrSortRAOCode))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        refineOrSortRaoCodeR.SetSizeColToAllLevels(120);
        refineOrSortRaoCodeR.Binding = nameof(RefineOrSortRAOCode);
        numberInOrderR += refineOrSortRaoCodeR;

        #endregion

        #region Subsidy (31)

        var subsidyR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(Subsidy))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        subsidyR.SetSizeColToAllLevels(88);
        subsidyR.Binding = nameof(Subsidy);
        numberInOrderR += subsidyR;

        #endregion

        #region FcpNumber (32)

        var fcpNumberR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(FcpNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        fcpNumberR.SetSizeColToAllLevels(163);
        fcpNumberR.Binding = nameof(FcpNumber);
        numberInOrderR += fcpNumberR;

        #endregion

        #region ContractNumber (33)

        var contractNumberR = ((FormPropertyAttribute)typeof(Form17)
                .GetProperty(nameof(ContractNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        contractNumberR.SetSizeColToAllLevels(163);
        contractNumberR.Binding = nameof(ContractNumber);
        numberInOrderR += contractNumberR;

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
        string str =
            $"{this.NumberInOrder.Value}\t" +
            $"{this.OperationCode.Value}\t" +
            $"{this.OperationDate.Value}\t" +
            $"{this.PackName.Value}\t" +
            $"{this.PackType.Value}\t" +
            $"{this.PackFactoryNumber.Value}\t" +
            $"{this.PackNumber.Value}\t" +
            $"{this.FormingDate.Value}\t" +
            $"{this.PassportNumber.Value}\t" +
            $"{this.Volume.Value}\t" +
            $"{this.Mass.Value}\t" +
            $"{this.Radionuclids.Value}\t" +
            $"{this.SpecificActivity.Value}\t" +
            $"{this.DocumentVid.Value}\t" +
            $"{this.DocumentNumber.Value}\t" +
            $"{this.DocumentDate.Value}\t" +
            $"{this.ProviderOrRecieverOKPO.Value}\t" +
            $"{this.TransporterOKPO.Value}\t" +
            $"{this.StoragePlaceName.Value}\t" +
            $"{this.StoragePlaceCode.Value}\t" +
            $"{this.CodeRAO.Value}\t" +
            $"{this.StatusRAO.Value}\t" +
            $"{this.VolumeOutOfPack.Value}\t" +
            $"{this.MassOutOfPack.Value}\t" +
            $"{this.Quantity.Value}\t" +
            $"{this.TritiumActivity.Value}\t" +
            $"{this.BetaGammaActivity.Value}\t" +
            $"{this.AlphaActivity.Value}\t" +
            $"{this.TransuraniumActivity.Value}\t" +
            $"{this.RefineOrSortRAOCode.Value}\t" +
            $"{this.Subsidy.Value}\t" +
            $"{this.FcpNumber.Value}\t" +
            $"{this.ContractNumber.Value}";
        return str;
    }
    #endregion
}