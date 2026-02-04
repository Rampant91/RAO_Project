using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;
using Spravochniki;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace Models.Forms.Form1;

[Serializable]
[Form_Class("Форма 1.4: Сведения об ОРИ, кроме отдельных изделий")]
[Table (name: "form_14")]
public class Form14 : Form1
{
    #region Constructor
    
    public Form14()
    {
        FormNum.Value = "1.4";
        Validate_all();
    }

    #endregion

    #region Validation
    
    private void Validate_all()
    {
        Owner_Validation(Owner);
        PackName_Validation(PackName);
        PackNumber_Validation(PackNumber);
        PackType_Validation(PackType);
        PassportNumber_Validation(PassportNumber);
        PropertyCode_Validation(PropertyCode);
        ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
        TransporterOKPO_Validation(TransporterOKPO);
        Activity_Validation(Activity);
        Radionuclids_Validation(Radionuclids);
        Name_Validation(Name);
        Sort_Validation(Sort);
        Volume_Validation(Volume);
        Mass_Validation(Mass);
        ActivityMeasurementDate_Validation(ActivityMeasurementDate);
        AggregateState_Validation(AggregateState);
    }

    public override bool Object_Validation()
    {
        return !(Owner.HasErrors ||
                 PackName.HasErrors ||
                 PackNumber.HasErrors ||
                 PackType.HasErrors ||
                 PassportNumber.HasErrors ||
                 PropertyCode.HasErrors ||
                 ProviderOrRecieverOKPO.HasErrors ||
                 TransporterOKPO.HasErrors ||
                 Activity.HasErrors ||
                 Radionuclids.HasErrors ||
                 Name.HasErrors ||
                 Sort.HasErrors ||
                 Volume.HasErrors ||
                 Mass.HasErrors ||
                 ActivityMeasurementDate.HasErrors ||
                 AggregateState.HasErrors);
    }

    protected override bool DocumentNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (value.Value == "прим.")
        {
            //if ((DocumentNumberNote == null) || DocumentNumberNote.Equals(""))
            //    value.AddError( "Заполните примечание");
            return true;
        }
        if (string.IsNullOrEmpty(value.Value))//ok
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        return true;
    }

    protected override bool OperationCode_Validation(RamAccess<string> value)//OK
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (!Spravochniks.SprOpCodes.Contains(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (value.Value is "01" or "13" or "14" or "16" or "26" or "36" or "44" or "45" or "49" or "51" or "52" or "55"
            or "56" or "57" or "59" or "76")
        {
            value.AddError("Код операции не может быть использован для РВ");
        }

        return false;
    }

    #endregion

    #region Properties

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

    #region AutoReplaceByOpCode

    private void AutoReplaceByOpCode(string opCode)
    {
        const string dash = "-";
        var masterOkpo = Report?.Reports?.Master_DB?.OkpoRep.Value ?? string.Empty;
        switch (opCode)
        {
            #region 10, 12, 42, 97, 98, 99

            case "10" or "12" or "42" or "97" or "98" or "99":
            {
                #region ProviderOrRecieverOKPO (18)

                if (!string.IsNullOrWhiteSpace(masterOkpo)
                    && ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (19)

                if (TransporterOKPO_DB != dash)
                {
                    TransporterOKPO.Value = dash;
                }

                #endregion

                break;
            }

            #endregion

            #region 11

            case "11":
            {
                #region DocumentVid (15)

                const byte documentVidValue = 9;
                if (DocumentVid_DB != documentVidValue)
                {
                    DocumentVid.Value = documentVidValue;
                }

                #endregion

                #region DocumentNumber (16)

                if (DocumentNumber_DB != PassportNumber_DB)
                {
                    DocumentNumber.Value = PassportNumber_DB;
                }

                #endregion

                #region DocumentDate (17)

                if (DateOnly.TryParse(OperationDate_DB, new CultureInfo("ru-RU", useUserOverride: false), out var operationDate)
                    && DocumentDate_DB != operationDate.ToShortDateString())
                {
                    DocumentDate.Value = operationDate.ToShortDateString();
                }

                #endregion

                #region ProviderOrRecieverOKPO (18)

                if (!string.IsNullOrWhiteSpace(masterOkpo)
                    && ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (19)

                if (TransporterOKPO_DB != dash)
                {
                    TransporterOKPO.Value = dash;
                }

                #endregion

                break;
            }

            #endregion

            #region 17, 18, 43, 46, 47, 48, 53, 58, 65, 67, 68, 71, 72, 73, 74, 75

            case "17" or "18" or "43" or "46" or "47" or "48" or "53" or "58" or "65" or "67" or "68" or "71" or "72" or "73" or "74" or "75":
            {
                #region DocumentDate (17)

                if (DateOnly.TryParse(OperationDate_DB, new CultureInfo("ru-RU", useUserOverride: false), out var operationDate)
                    && DocumentDate_DB != operationDate.ToShortDateString())
                {
                    DocumentDate.Value = operationDate.ToShortDateString();
                }

                #endregion

                #region ProviderOrRecieverOKPO (18)

                if (!string.IsNullOrWhiteSpace(masterOkpo)
                    && ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (19)

                if (TransporterOKPO_DB != dash)
                {
                    TransporterOKPO.Value = dash;
                }

                #endregion

                break;
            }

            #endregion

            #region 21, 25, 27, 28, 29, 31, 35, 37, 38, 39, 81, 82, 83, 84, 85, 86, 87, 88

            case "21" or "25" or "27" or "28" or "29" or "31" or "35" or "37" or "38"
                or "39" or "81" or "82" or "83" or "84" or "85" or "86" or "87" or "88":
            {
                #region ProviderOrRecieverOKPO (18)

                if (ProviderOrRecieverOKPO_DB is not "")
                {
                    ProviderOrRecieverOKPO.Value = string.Empty;
                }

                #endregion

                #region TransporterOKPO (19)

                if (TransporterOKPO_DB is not "")
                {
                    TransporterOKPO.Value = string.Empty;
                }

                #endregion

                break;
            }

            #endregion

            #region 22, 32

            case "22" or "32":
            {
                #region ProviderOrRecieverOKPO (18)

                const string providerOrRecieverOkpoValue = "Минобороны";
                if (ProviderOrRecieverOKPO_DB != providerOrRecieverOkpoValue)
                {
                    ProviderOrRecieverOKPO.Value = providerOrRecieverOkpoValue;
                }

                #endregion

                break;
            }

            #endregion

            #region 41

            case "41":
            {
                #region DocumentVid (15)

                const byte documentVidValue = 1;
                if (DocumentVid_DB != documentVidValue)
                {
                    DocumentVid.Value = documentVidValue;
                }

                #endregion

                #region DocumentDate (17)

                if (DateOnly.TryParse(OperationDate_DB, new CultureInfo("ru-RU", useUserOverride: false), out var operationDate)
                    && DocumentDate_DB != operationDate.ToShortDateString())
                {
                    DocumentDate.Value = operationDate.ToShortDateString();
                }

                #endregion

                #region ProviderOrRecieverOKPO (18)

                if (!string.IsNullOrWhiteSpace(masterOkpo)
                    && ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (19)

                if (TransporterOKPO_DB != dash)
                {
                    TransporterOKPO.Value = dash;
                }

                #endregion

                break;
            }

            #endregion

            #region 54

            case "54":
            {
                #region DocumentDate (17)

                if (DateOnly.TryParse(OperationDate_DB, new CultureInfo("ru-RU", useUserOverride: false), out var operationDate)
                    && DocumentDate_DB != operationDate.ToShortDateString())
                {
                    DocumentDate.Value = operationDate.ToShortDateString();
                }

                #endregion

                #region TransporterOKPO (19)

                if (TransporterOKPO_DB != dash)
                {
                    TransporterOKPO.Value = dash;
                }

                #endregion

                break;
            }

            #endregion

            #region 61, 62

            case "61" or "62":
            {
                #region ProviderOrRecieverOKPO (18)

                if (!string.IsNullOrWhiteSpace(masterOkpo)
                    && ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                break;
            }

            #endregion

            #region 63, 64

            case "63" or "64":
            {
                #region TransporterOKPO (19)

                if (TransporterOKPO_DB != dash)
                {
                    TransporterOKPO.Value = dash;
                }

                #endregion

                break;
            }

            #endregion
        }
    }

    #endregion

    #endregion

    #region OperationDate (3)

    private protected override void OperationDate_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;

        var value1 = ((RamAccess<string>)value).Value ?? string.Empty;
        if (OperationDate_DB != value1)
        {
            OperationDate_DB = DateString_ValueChanged(value1);
            if (Report is { AutoReplace: true })
            {
                AutoReplaceByOpDate();
            }
        }
    }

    #region AutoReplaceByOpDate

    private void AutoReplaceByOpDate()
    {
        if (!DateOnly.TryParse(OperationDate_DB, new CultureInfo("ru-RU", useUserOverride: false), out var opDate)) return;

        switch (OperationCode_DB)
        {
            case "11" or "17" or "18" or "41" or "43" or "46" or "47" or "48" or "53" or "54" or "58"
                or "65" or "67" or "68" or "71" or "72" or "73" or "74" or "75":
            {
                DocumentDate.Value = opDate.ToShortDateString();
                break;
            }

            default: return;
        }
    }

    #endregion

    #endregion

    #region PassportNumber (4)

    public string PassportNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "номер паспорта", "4")]
    public RamAccess<string> PassportNumber
    {
        get
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
        set
        {
            PassportNumber_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void PassportNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;

        var value1 = (((RamAccess<string>)value).Value ?? string.Empty);
        if (PassportNumber_DB != value1)
        {
            PassportNumber_DB = value1;
            if (Report is { AutoReplace: true })
            {
                AutoReplaceByPasNum();
            }
        }
    }

    private static bool PassportNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((PassportNumberNote.Value == null) || (PassportNumberNote.Value == ""))
            //    value.AddError("Заполните примечание");

            return true;
        }
        return true;
    }

    #region AutoReplaceByPasNum

    private void AutoReplaceByPasNum()
    {
        if (OperationCode_DB is "11" && DocumentNumber_DB != PassportNumber_DB)
        {
            DocumentNumber.Value = PassportNumber_DB;
        }
    }

    #endregion

    #endregion

    #region Name (5)

    public string Name_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "наименование", "5")]
    public RamAccess<string> Name
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Name), out var value))
            {
                ((RamAccess<string>)value).Value = Name_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Name_Validation, Name_DB);
            rm.PropertyChanged += Name_ValueChanged;
            Dictionary.Add(nameof(Name), rm);
            return (RamAccess<string>)Dictionary[nameof(Name)];
        }
        set
        {
            Name_DB = value.Value;
            OnPropertyChanged(nameof(Name));
        }
    }

    private void Name_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        Name_DB = tmp;
    }

    private static bool Name_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        return true;
    }

    #endregion

    #region Sort (6)

    public byte? Sort_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "вид", "6")]
    public RamAccess<byte?> Sort
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Sort), out var value))
            {
                ((RamAccess<byte?>)value).Value = Sort_DB;
                return (RamAccess<byte?>)value;
            }
            var rm = new RamAccess<byte?>(Sort_Validation, Sort_DB);
            rm.PropertyChanged += Sort_ValueChanged;
            Dictionary.Add(nameof(Sort), rm);
            return (RamAccess<byte?>)Dictionary[nameof(Sort)];
        }
        set
        {
            Sort_DB = value.Value;
            OnPropertyChanged(nameof(Sort));
        }
    }//If change this change validation

    private void Sort_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        Sort_DB = ((RamAccess<byte?>)value).Value;
    }

    private static bool Sort_Validation(RamAccess<byte?> value)//TODO
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value is not (>= 4 and <= 12))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region Radionuclids (7)

    public string Radionuclids_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "радионуклиды", "7")]
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
            OnPropertyChanged(nameof(Radionuclids));
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

    #region Activity (8)

    public string Activity_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "активность, Бк", "8")]
    public RamAccess<string> Activity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Activity), out var value))
            {
                ((RamAccess<string>)value).Value = Activity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Activity_Validation, Activity_DB);
            rm.PropertyChanged += Activity_ValueChanged;
            Dictionary.Add(nameof(Activity), rm);
            return (RamAccess<string>)Dictionary[nameof(Activity)];
        }
        set
        {
            Activity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void Activity_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        Activity_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool Activity_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region ActivityMeasurementDate (9)

    public string ActivityMeasurementDate_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "дата измерения активности", "9")]
    public RamAccess<string> ActivityMeasurementDate
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ActivityMeasurementDate), out var value))
            {
                ((RamAccess<string>)value).Value = ActivityMeasurementDate_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(ActivityMeasurementDate_Validation, ActivityMeasurementDate_DB);
            rm.PropertyChanged += ActivityMeasurementDate_ValueChanged;
            Dictionary.Add(nameof(ActivityMeasurementDate), rm);
            return (RamAccess<string>)Dictionary[nameof(ActivityMeasurementDate)];
        }
        set
        {
            ActivityMeasurementDate_DB = value.Value;
            OnPropertyChanged();
        }
    }//if change this change validation

    private void ActivityMeasurementDate_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        ActivityMeasurementDate_DB = DateString_ValueChanged(((RamAccess<string>)value).Value);

    }

    private static bool ActivityMeasurementDate_Validation(RamAccess<string> value) => DateString_Validation(value);

    #endregion

    #region Volume (10)

    public string Volume_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "объем, куб. м", "10")]
    public RamAccess<string> Volume
    {
        get
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
        set
        {
            Volume_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void Volume_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        Volume_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool Volume_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region Mass (11)

    public string Mass_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "масса, кг", "11")]
    public RamAccess<string> Mass
    {
        get
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
        set
        {
            Mass_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void Mass_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        Mass_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private static bool Mass_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region AggregateState (12)

    public byte? AggregateState_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "агрегатное состояние", "12")]
    public RamAccess<byte?> AggregateState//1 2 3
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(AggregateState), out var value))
            {
                ((RamAccess<byte?>)value).Value = AggregateState_DB;
                return (RamAccess<byte?>)value;
            }
            var rm = new RamAccess<byte?>(AggregateState_Validation, AggregateState_DB);
            rm.PropertyChanged += AggregateState_ValueChanged;
            Dictionary.Add(nameof(AggregateState), rm);
            return (RamAccess<byte?>)Dictionary[nameof(AggregateState)];
        }
        set
        {
            AggregateState_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void AggregateState_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        AggregateState_DB = ((RamAccess<byte?>)value).Value;
    }

    private static bool AggregateState_Validation(RamAccess<byte?> value)//Ready
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value is not (1 or 2 or 3))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region PropertyCode (13)

    public byte? PropertyCode_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Право собственности на ОРИ", "код формы собственности", "13")]
    public RamAccess<byte?> PropertyCode
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(PropertyCode), out var value))
            {
                ((RamAccess<byte?>)value).Value = PropertyCode_DB;
                return (RamAccess<byte?>)value;
            }
            var rm = new RamAccess<byte?>(PropertyCode_Validation, PropertyCode_DB);
            rm.PropertyChanged += PropertyCode_ValueChanged;
            Dictionary.Add(nameof(PropertyCode), rm);
            return (RamAccess<byte?>)Dictionary[nameof(PropertyCode)];
        }//OK
        set
        {
            PropertyCode_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void PropertyCode_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        PropertyCode_DB = ((RamAccess<byte?>)value).Value;
    }

    private static bool PropertyCode_Validation(RamAccess<byte?> value)//Ready
    {
        value.ClearErrors();
        if (value.Value == null)//ok
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value is not (>= 1 and <= 6 or 9))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region Owner (14)

    public string Owner_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Право собственности на ОРИ", "код ОКПО правообладателя", "14")]
    public RamAccess<string> Owner
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Owner), out var value))
            {
                ((RamAccess<string>)value).Value = Owner_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Owner_Validation, Owner_DB);
            rm.PropertyChanged += Owner_ValueChanged;
            Dictionary.Add(nameof(Owner), rm);
            return (RamAccess<string>)Dictionary[nameof(Owner)];
        }
        set
        {
            Owner_DB = value.Value;
            OnPropertyChanged();
        }
    }//if change this change validation

    private void Owner_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        if (Spravochniks.OKSM.Contains(tmp.ToUpper()))
        {
            tmp = tmp.ToUpper();
        }
        Owner_DB = tmp;
    }

    private static bool Owner_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
            return true;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((OwnerNote == null) || OwnerNote.Equals(""))
            //    value.AddError( "Заполните примечание");
            return true;
        }
        if (value.Value.Length is not (8 or 14)
            || !OkpoRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); 
            return false;

        }
        return true;
    }

    #endregion

    #region ProviderOrRecieverOKPO (18)

    public string ProviderOrRecieverOKPO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Код ОКПО", "поставщика или получателя", "18")]
    public RamAccess<string> ProviderOrRecieverOKPO
    {
        get
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
        set => ProviderOrRecieverOKPO_DB = value.Value;
    }

    private void ProviderOrRecieverOKPO_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty);
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
            value.AddError("Поле не заполнено");
            return false;
        }
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()) || value.Value.Equals("Минобороны"))
        {
            return true;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((ProviderOrRecieverOKPONote.Value == null) || ProviderOrRecieverOKPONote.Value.Equals(""))
            //    value.AddError("Заполните примечание");
            return true;
        }
        //try
        //{
        //    var tmp = short.Parse(OperationCode.Value);
        //    var a = tmp is >= 10 and <= 12;
        //    var b = tmp is >= 41 and <= 43;
        //    var c = tmp is >= 71 and <= 73;
        //    var d = tmp is 15 or 17 or 18 or 46 or 47 or 48 or 53 or 54 or 58 or 61 or 62 or 65 or 67 or 68 or 75 or 76;
        //    if (a || b || c || d)
        //    {
        //        //ProviderOrRecieverOKPO.Value = "ОКПО ОТЧИТЫВАЮЩЕЙСЯ ОРГ";
        //        //return false;
        //    }
        //}
        //catch (Exception)
        //{
        //    // ignored
        //}

        if (value.Value.Length is not (8 or 14)
            || !OkpoRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;

        }
        return true;
    }
    
    #endregion

    #region TransporterOKPO (19)

    public string TransporterOKPO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Код ОКПО", "перевозчика", "19")]
    public RamAccess<string> TransporterOKPO
    {
        get
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
        set
        {
            TransporterOKPO_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TransporterOKPO_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        if (Spravochniks.OKSM.Contains(tmp.ToUpper()))
        {
            tmp = tmp.ToUpper();
        }
        TransporterOKPO_DB = tmp;
    }

    private static bool TransporterOKPO_Validation(RamAccess<string> value)//Done
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("-") || value.Value.Equals("Минобороны") || Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
            return true;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
            //    value.AddError( "Заполните примечание");
            return true;
        }
        if (value.Value.Length is not (8 or 14)
            || !OkpoRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region PackName (20)

    public string PackName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "наименование", "20")]
    public RamAccess<string> PackName
    {
        get
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
        set => PackName_DB = value.Value;
    }

    private void PackName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        PackName_DB = tmp;
    }

    private static bool PackName_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((PackNameNote == null) || PackNameNote.Equals(""))
            //    value.AddError( "Заполните примечание");//to do note handling
            return true;
        }
        return true;
    }

    #endregion

    #region PackType (21)

    public string PackType_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "тип", "21")]
    public RamAccess<string> PackType
    {
        get
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
        set => PackType_DB = value.Value;
    }//If change this change validation

    private void PackType_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        PackType_DB = tmp;
    }

    private static bool PackType_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((PackTypeNote == null) || PackTypeNote.Equals(""))
            //    value.AddError( "Заполните примечание");// to do note handling
            return true;
        }
        return true;
    }

    #endregion

    #region PackNumber (22)

    public string PackNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "номер упаковки", "22")]
    public RamAccess<string> PackNumber
    {
        get
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
        set => PackNumber_DB = value.Value;
    }//If change this change validation

    private void PackNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        PackNumber_DB = tmp;
    }

    private static bool PackNumber_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))//ok
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((PackNumberNote == null) || PackNumberNote.Equals(""))
            //    value.AddError( "Заполните примечание");// to do note handling
            return true;
        }
        return true;
    }

    #endregion

    #endregion

    #region IExcel

    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        base.ExcelGetRow(worksheet, row);
        PassportNumber_DB = Convert.ToString(worksheet.Cells[row, 4].Value);
        Name_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        Sort_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 6].Value), out var byteValue) ? byteValue : null;
        Radionuclids_DB = Convert.ToString(worksheet.Cells[row, 7].Value);
        Activity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 8].Value);
        ActivityMeasurementDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 9].Text);
        Volume_DB = ConvertFromExcelDouble(worksheet.Cells[row, 10].Value);
        Mass_DB = ConvertFromExcelDouble(worksheet.Cells[row, 11].Value);
        AggregateState_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 12].Value), out byteValue) ? byteValue : null;
        PropertyCode_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 13].Value), out byteValue) ? byteValue : null;
        Owner_DB = Convert.ToString(worksheet.Cells[row, 14].Value);
        DocumentVid_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 15].Value), out byteValue) ? byteValue : null;
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[row, 16].Value);
        DocumentDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 17].Text);
        ProviderOrRecieverOKPO_DB = Convert.ToString(worksheet.Cells[row, 18].Value);
        TransporterOKPO_DB = Convert.ToString(worksheet.Cells[row, 19].Value);
        PackName_DB = Convert.ToString(worksheet.Cells[row, 20].Value);
        PackType_DB = Convert.ToString(worksheet.Cells[row, 21].Value);
        PackNumber_DB = Convert.ToString(worksheet.Cells[row, 22].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose=true, string sumNumber = "")
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ConvertToExcelString(PassportNumber_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(Name_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = Sort_DB is null ? "-" : Sort_DB;
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelString(Radionuclids_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelDouble(Activity_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelDate(ActivityMeasurementDate_DB, worksheet, row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0));
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDouble(Volume_DB);
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelDouble(Mass_DB);
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = AggregateState_DB is null ? "-" : AggregateState_DB;
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = PropertyCode_DB is null ? "-" : PropertyCode_DB;
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ConvertToExcelString(Owner_DB);
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = DocumentVid_DB is null ? "-" : DocumentVid_DB;
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ConvertToExcelString(DocumentNumber_DB);
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ConvertToExcelDate(DocumentDate_DB, worksheet, row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0));
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ConvertToExcelString(ProviderOrRecieverOKPO_DB);
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ConvertToExcelString(TransporterOKPO_DB);
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ConvertToExcelString(PackName_DB);
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ConvertToExcelString(PackType_DB);
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ConvertToExcelString(PackNumber_DB);

        return 19;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form1.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(PassportNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(Name))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(Sort))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(Radionuclids))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(Activity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(ActivityMeasurementDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(Volume))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(Mass))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(AggregateState))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(PropertyCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(Owner))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(DocumentVid))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(DocumentNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(DocumentDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(ProviderOrRecieverOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(TransporterOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(PackName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(PackType))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form14,Models")?.GetProperty(nameof(PackNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];

        return 19;
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

        #region PassportNumber (4)

        var passportNumberR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(PassportNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (passportNumberR != null)
        {
            passportNumberR.SetSizeColToAllLevels(95);
            passportNumberR.Binding = nameof(PassportNumber);
            numberInOrderR += passportNumberR;
        }

        #endregion

        #region Name (5)

        var nameR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(Name))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (nameR != null)
        {
            nameR.SetSizeColToAllLevels(125);
            nameR.Binding = nameof(Name);
            numberInOrderR += nameR;
        }

        #endregion

        #region Sort (6)

        var sortR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(Sort))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (sortR != null)
        {
            sortR.SetSizeColToAllLevels(88);
            sortR.Binding = nameof(Sort);
            numberInOrderR += sortR;
        }

        #endregion

        #region Radionuclids (7)

        var radionuclidsR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(Radionuclids))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (radionuclidsR != null)
        {
            radionuclidsR.SetSizeColToAllLevels(143);
            radionuclidsR.Binding = nameof(Radionuclids);
            numberInOrderR += radionuclidsR;
        }

        #endregion

        #region Activity (8)

        var activityR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(Activity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        activityR.SetSizeColToAllLevels(88);
        activityR.Binding = nameof(Activity);
        numberInOrderR += activityR;

        #endregion

        #region ActivityMeasurementDate (9)

        var activityMeasurementDateR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(ActivityMeasurementDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        activityMeasurementDateR.SetSizeColToAllLevels(163);
        activityMeasurementDateR.Binding = nameof(ActivityMeasurementDate);
        numberInOrderR += activityMeasurementDateR;

        #endregion

        #region Volume (10)

        var volumeR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(Volume))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        volumeR.SetSizeColToAllLevels(88);
        volumeR.Binding = nameof(Volume);
        numberInOrderR += volumeR;

        #endregion

        #region Mass (11)

        var massR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(Mass))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        massR.SetSizeColToAllLevels(88);
        massR.Binding = nameof(Mass);
        numberInOrderR += massR;

        #endregion

        #region AggregateState (12)

        var aggregateStateR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(AggregateState))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        aggregateStateR.SetSizeColToAllLevels(163);
        aggregateStateR.Binding = nameof(AggregateState);
        numberInOrderR += aggregateStateR;

        #endregion

        #region PropertyCode (13)

        var propertyCodeR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(PropertyCode))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        propertyCodeR.SetSizeColToAllLevels(90);
        propertyCodeR.Binding = nameof(PropertyCode);
        numberInOrderR += propertyCodeR;

        #endregion

        #region Owner (14)

        var ownerR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(Owner))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        ownerR.SetSizeColToAllLevels(100);
        ownerR.Binding = nameof(Owner);
        numberInOrderR += ownerR;

        #endregion

        #region DocumentVid (15)

        var documentVidR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentVid))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentVidR.SetSizeColToAllLevels(60);
        documentVidR.Binding = nameof(DocumentVid);
        numberInOrderR += documentVidR;

        #endregion

        #region DocumentNumber (16)

        var documentNumberR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentNumberR.SetSizeColToAllLevels(100);
        documentNumberR.Binding = nameof(DocumentNumber);
        numberInOrderR += documentNumberR;

        #endregion

        #region DocumentDate (17)

        var documentDateR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentDateR.SetSizeColToAllLevels(80);
        documentDateR.Binding = nameof(DocumentDate);
        numberInOrderR += documentDateR;

        #endregion

        #region ProviderOrRecieverOKPO (18)

        var providerOrRecieverOkpoR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(ProviderOrRecieverOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        providerOrRecieverOkpoR.SetSizeColToAllLevels(100);
        providerOrRecieverOkpoR.Binding = nameof(ProviderOrRecieverOKPO);
        numberInOrderR += providerOrRecieverOkpoR;

        #endregion

        #region TransporterOKPO (19)

        var transporterOkpoR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(TransporterOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        transporterOkpoR.SetSizeColToAllLevels(90);
        transporterOkpoR.Binding = nameof(TransporterOKPO);
        numberInOrderR += transporterOkpoR;

        #endregion

        #region PackName (20)

        var packNameR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(PackName))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packNameR.SetSizeColToAllLevels(143);
        packNameR.Binding = nameof(PackName);
        numberInOrderR += packNameR;

        #endregion

        #region PackType (21)

        var packTypeR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(PackType))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packTypeR.SetSizeColToAllLevels(88);
        packTypeR.Binding = nameof(PackType);
        numberInOrderR += packTypeR;

        #endregion

        #region PackNumber (22)

        var packNumberR = ((FormPropertyAttribute)typeof(Form14)
                .GetProperty(nameof(PackNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packNumberR.SetSizeColToAllLevels(125);
        packNumberR.Binding = nameof(PackNumber);
        numberInOrderR += packNumberR;

        #endregion

        _DataGridColumns = numberInOrderR;
        return _DataGridColumns;
    }

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
            $"{PassportNumber.Value}\t" +
            $"{Name.Value}\t" +
            $"{Sort.Value}\t" +
            $"{Radionuclids.Value}\t" +
            $"{Activity.Value}\t" +
            $"{ActivityMeasurementDate.Value}\t" +
            $"{Volume.Value}\t" +
            $"{Mass.Value}\t" +
            $"{AggregateState.Value}\t" +
            $"{PropertyCode.Value}\t" +
            $"{Owner.Value}\t" +
            $"{DocumentVid.Value}\t" +
            $"{DocumentNumber.Value}\t" +
            $"{DocumentDate.Value}\t" +
            $"{ProviderOrRecieverOKPO.Value}\t" +
            $"{TransporterOKPO.Value}\t" +
            $"{PackName.Value}\t" +
            $"{PackType.Value}\t" +
            $"{PackNumber.Value}";
        return str;
    }

    #endregion
}