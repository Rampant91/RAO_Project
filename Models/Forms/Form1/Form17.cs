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
using Spravochniki;

namespace Models.Forms.Form1;

[Form_Class("Форма 1.7: Сведения о твердых кондиционированных РАО")]
public class Form17 : Form1
{
    public Form17() : base()
    {
        FormNum.Value = "1.7";
        Validate_all();
    }
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
        return !(CodeRAO.HasErrors||
                 PackName.HasErrors||
                 PackNumber.HasErrors||
                 PackFactoryNumber.HasErrors||
                 PackType.HasErrors||
                 Volume.HasErrors||
                 Mass.HasErrors||
                 Radionuclids.HasErrors||
                 ProviderOrRecieverOKPO.HasErrors||
                 TransporterOKPO.HasErrors||
                 TritiumActivity.HasErrors||
                 BetaGammaActivity.HasErrors||
                 AlphaActivity.HasErrors||
                 TransuraniumActivity.HasErrors||
                 FormingDate.HasErrors||
                 PassportNumber.HasErrors||
                 RefineOrSortRAOCode.HasErrors||
                 Subsidy.HasErrors||
                 FcpNumber.HasErrors||
                 StatusRAO.HasErrors||
                 VolumeOutOfPack.HasErrors||
                 MassOutOfPack.HasErrors||
                 StoragePlaceName.HasErrors||
                 StoragePlaceCode.HasErrors||
                 SpecificActivity.HasErrors||
                 Quantity.HasErrors);
    }

    #region Properties

    #region  Sum
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
            OnPropertyChanged(nameof(Sum));
        }
    }

    private void SumValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Sum_DB = ((RamAccess<bool>)Value).Value;
        }
    }

    private bool Sum_Validation(RamAccess<bool> value)
    {
        value.ClearErrors();
        return true;
    }
    #endregion

    #region PackName
    public string PackName_DB { get; set; } = "";
    public bool PackName_Hidden_Priv { get; set; }
    [NotMapped]
    public bool PackName_Hidden
    {
        get => PackName_Hidden_Priv;
        set
        {
            PackName_Hidden_Priv = value;
        }
    }
    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-4", "наименование", "4")]
    public RamAccess<string> PackName
    {
        get
        {
            if (!PackName_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(PackName)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PackName)]).Value = PackName_DB;
                    return (RamAccess<string>)Dictionary[nameof(PackName)];
                }
                var rm = new RamAccess<string>(PackName_Validation, PackName_DB);
                rm.PropertyChanged += PackNameValueChanged;
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
                OnPropertyChanged(nameof(PackName));
            }
        }
    }
    private void PackNameValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PackName_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool PackName_Validation(RamAccess<string> value)
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

    #region PackType
    public string PackType_DB { get; set; } = "";
    public bool PackType_Hidden_Priv { get; set; }
    [NotMapped]
    public bool PackType_Hidden
    {
        get => PackType_Hidden_Priv;
        set
        {
            PackType_Hidden_Priv = value;
        }
    }
    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-5", "тип", "5")]
    public RamAccess<string> PackType
    {
        get
        {
            if (!PackType_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(PackType)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PackType)]).Value = PackType_DB;
                    return (RamAccess<string>)Dictionary[nameof(PackType)];
                }
                var rm = new RamAccess<string>(PackType_Validation, PackType_DB);
                rm.PropertyChanged += PackTypeValueChanged;
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
                OnPropertyChanged(nameof(PackType));
            }
        }
    }

    private void PackTypeValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PackType_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool PackType_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        return true;
    }
    #endregion

    #region PackFactoryNumber
    public string PackFactoryNumber_DB { get; set; } = "";
    public bool PackFactoryNumber_Hidden_Priv { get; set; }
    [NotMapped]
    public bool PackFactoryNumber_Hidden
    {
        get => PackFactoryNumber_Hidden_Priv;
        set
        {
            PackFactoryNumber_Hidden_Priv = value;
        }
    }
    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-6", "заводской номер", "6")]
    public RamAccess<string> PackFactoryNumber
    {
        get
        {
            if (!PackFactoryNumber_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(PackFactoryNumber)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PackFactoryNumber)]).Value = PackFactoryNumber_DB;
                    return (RamAccess<string>)Dictionary[nameof(PackFactoryNumber)];
                }
                var rm = new RamAccess<string>(PackFactoryNumber_Validation, PackFactoryNumber_DB);
                rm.PropertyChanged += PackFactoryNumberValueChanged;
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
                OnPropertyChanged(nameof(PackFactoryNumber));
            }
        }
    }
    private void PackFactoryNumberValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PackFactoryNumber_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool PackFactoryNumber_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        return true;
    }
    #endregion

    #region PackNumber
    public string PackNumber_DB { get; set; } = "";
    public bool PackNumber_Hidden_Priv { get; set; }
    [NotMapped]
    public bool PackNumber_Hidden
    {
        get => PackNumber_Hidden_Priv;
        set
        {
            PackNumber_Hidden_Priv = value;
        }
    }
    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-7", "номер упаковки (идентификационный код)", "7")]
    public RamAccess<string> PackNumber
    {
        get
        {
            if (!PackNumber_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(PackNumber)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PackNumber)]).Value = PackNumber_DB;
                    return (RamAccess<string>)Dictionary[nameof(PackNumber)];
                }
                var rm = new RamAccess<string>(PackNumber_Validation, PackNumber_DB);
                rm.PropertyChanged += PackNumberValueChanged;
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
                OnPropertyChanged(nameof(PackNumber));
            }
        }
    }

    private void PackNumberValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PackNumber_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool PackNumber_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        return true;
    }
    #endregion

    #region FormingDate
    public string FormingDate_DB { get; set; } = "";
    public bool FormingDate_Hidden_Priv { get; set; }
    [NotMapped]
    public bool FormingDate_Hidden
    {
        get => FormingDate_Hidden_Priv;
        set
        {
            FormingDate_Hidden_Priv = value;
        }
    }
    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-8", "дата формирования", "8")]
    public RamAccess<string> FormingDate
    {
        get
        {
            if (!FormingDate_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(FormingDate)))
                {
                    ((RamAccess<string>)Dictionary[nameof(FormingDate)]).Value = FormingDate_DB;
                    return (RamAccess<string>)Dictionary[nameof(FormingDate)];
                }
                var rm = new RamAccess<string>(FormingDate_Validation, FormingDate_DB);
                rm.PropertyChanged += FormingDateValueChanged;
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
                OnPropertyChanged(nameof(FormingDate));
            }
        }
    }
    private void FormingDateValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var tmp = ((RamAccess<string>)Value).Value;
            Regex b = new("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            FormingDate_DB = tmp;
        }
    }
    private bool FormingDate_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value == "-")
        {
            return true;
        }
        var tmp = value.Value;
        Regex b = new("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
        if (b.IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        Regex a = new("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
        if (!a.IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        try { DateTimeOffset.Parse(tmp); }
        catch (Exception)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region PassportNumber
    public string PassportNumber_DB { get; set; } = "";
    public bool PassportNumber_Hidden_Priv { get; set; }
    [NotMapped]
    public bool PassportNumber_Hidden
    {
        get => PassportNumber_Hidden_Priv;
        set
        {
            PassportNumber_Hidden_Priv = value;
        }
    }
    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-9", "номер паспорта", "9")]
    public RamAccess<string> PassportNumber
    {
        get
        {
            if (!PassportNumber_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(PassportNumber)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PassportNumber)]).Value = PassportNumber_DB;
                    return (RamAccess<string>)Dictionary[nameof(PassportNumber)];
                }
                var rm = new RamAccess<string>(PassportNumber_Validation, PassportNumber_DB);
                rm.PropertyChanged += PassportNumberValueChanged;
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
                OnPropertyChanged(nameof(PassportNumber));
            }
        }
    }
    private void PassportNumberValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PassportNumber_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool PassportNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }
    #endregion

    #region Volume
    public string Volume_DB { get; set; } = "";
    public bool Volume_Hidden_Priv { get; set; }
    [NotMapped]
    public bool Volume_Hidden
    {
        get => Volume_Hidden_Priv;
        set
        {
            Volume_Hidden_Priv = value;
        }
    }
    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-10", "объем, куб. м", "10")]
    public RamAccess<string> Volume
    {
        get
        {
            if (!Volume_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(Volume)))
                {
                    ((RamAccess<string>)Dictionary[nameof(Volume)]).Value = Volume_DB;
                    return (RamAccess<string>)Dictionary[nameof(Volume)];
                }
                var rm = new RamAccess<string>(Volume_Validation, Volume_DB);
                rm.PropertyChanged += VolumeValueChanged;
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
                OnPropertyChanged(nameof(Volume));
            }
        }
    }
    private void VolumeValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
            if (value1 != null)
            {
                value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    Volume_DB = value1;
                    return;
                }
                if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                try
                {
                    var value2 = Convert.ToDouble(value1);
                    value1 = $"{value2:0.######################################################e+00}";
                }
                catch (Exception) { }
            }
            Volume_DB = value1;
        }
    }
    private bool Volume_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value == "-")
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch (Exception)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region Mass
    public string Mass_DB { get; set; } = "";
    public bool Mass_Hidden_Priv { get; set; }
    [NotMapped]
    public bool Mass_Hidden
    {
        get => Mass_Hidden_Priv;
        set
        {
            Mass_Hidden_Priv = value;
        }
    }
    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "null-11", "масса брутто, т", "11")]
    public RamAccess<string> Mass
    {
        get
        {
            if (!Mass_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(Mass)))
                {
                    ((RamAccess<string>)Dictionary[nameof(Mass)]).Value = Mass_DB;
                    return (RamAccess<string>)Dictionary[nameof(Mass)];
                }
                var rm = new RamAccess<string>(Mass_Validation, Mass_DB);
                rm.PropertyChanged += MassValueChanged;
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
                OnPropertyChanged(nameof(Mass));
            }
        }
    }
    private void MassValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
            if (value1 != null)
            {
                value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    Mass_DB = value1;
                    return;
                }
                if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                //try
                //{
                    if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var value2))
                    {
                        value1 = $"{value2:0.######################################################e+00}";
                    }
                    //var value2 = Convert.ToDouble(value1);
                    //value1 = $"{value2:0.######################################################e+00}";
                //}
                //catch (Exception) { }
            }
            Mass_DB = value1;
        }
    }
    private bool Mass_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value == "-")
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch (Exception)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region Radionuclids
    public string Radionuclids_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "Радионуклидный состав", "наименование радионуклида", "12")]
    public RamAccess<string> Radionuclids
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Radionuclids)))
            {
                ((RamAccess<string>)Dictionary[nameof(Radionuclids)]).Value = Radionuclids_DB;
                return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
            }
            var rm = new RamAccess<string>(Radionuclids_Validation, Radionuclids_DB);
            rm.PropertyChanged += RadionuclidsValueChanged;
            Dictionary.Add(nameof(Radionuclids), rm);
            return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
        }
        set
        {
            Radionuclids_DB = value.Value;
            OnPropertyChanged(nameof(Radionuclids));
        }
    }//If change this change validation

    private void RadionuclidsValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Radionuclids_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool Radionuclids_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
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
            if (!tmp.Any())
                flag = false;
        }
        if (!flag)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region SpecificActivity
    public string SpecificActivity_DB { get; set; }
    [NotMapped]
    [FormProperty(true, "Сведения об упаковке", "Радионуклидный состав", "удельная активность, Бк/г", "13")]
    public RamAccess<string> SpecificActivity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(SpecificActivity)))
            {
                ((RamAccess<string>)Dictionary[nameof(SpecificActivity)]).Value = SpecificActivity_DB;
                return (RamAccess<string>)Dictionary[nameof(SpecificActivity)];
            }
            var rm = new RamAccess<string>(SpecificActivity_Validation, SpecificActivity_DB);
            rm.PropertyChanged += SpecificActivityValueChanged;
            Dictionary.Add(nameof(SpecificActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(SpecificActivity)];
        }
        set
        {
            SpecificActivity_DB = value.Value;
            OnPropertyChanged(nameof(SpecificActivity));
        }
    }
    private void SpecificActivityValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
            if (value1 != null)
            {
                value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    SpecificActivity_DB = value1;
                    return;
                }
                if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                try
                {
                    var value2 = Convert.ToDouble(value1);
                    value1 = $"{value2:0.######################################################e+00}";
                }
                catch (Exception) { }
            }
            SpecificActivity_DB = value1;
        }
    }
    private bool SpecificActivity_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
            {
                value.AddError("Число должно быть больше нуля");
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
    #endregion

    #region ProviderOrRecieverOKPO
    public string ProviderOrRecieverOKPO_DB { get; set; } = "";
    public bool ProviderOrRecieverOKPO_Hidden_Priv { get; set; }
    [NotMapped]
    public bool ProviderOrRecieverOKPO_Hidden
    {
        get => ProviderOrRecieverOKPO_Hidden_Priv;
        set
        {
            ProviderOrRecieverOKPO_Hidden_Priv = value;
        }
    }
    [NotMapped]
    [FormProperty(true, "null-17-18", "ОКПО", "поставщика или получателя", "17")]
    public RamAccess<string> ProviderOrRecieverOKPO
    {
        get
        {
            if (!ProviderOrRecieverOKPO_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(ProviderOrRecieverOKPO)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ProviderOrRecieverOKPO)]).Value = ProviderOrRecieverOKPO_DB;
                    return (RamAccess<string>)Dictionary[nameof(ProviderOrRecieverOKPO)];
                }
                var rm = new RamAccess<string>(ProviderOrRecieverOKPO_Validation, ProviderOrRecieverOKPO_DB);
                rm.PropertyChanged += ProviderOrRecieverOKPOValueChanged;
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
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }
    }
    private void ProviderOrRecieverOKPOValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
            if (value1 != null)
                if (Spravochniks.OKSM.Contains(value1.ToUpper()))
                {
                    value1 = value1.ToUpper();
                }
            ProviderOrRecieverOKPO_DB = value1;
        }
    }
    private bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
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
        if (value.Value.Length != 8 && value.Value.Length != 14)
        {
            value.AddError("Недопустимое значение"); return false;
        }

        Regex mask = new("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
        if (!mask.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    #endregion

    #region TransporterOKPO
    public string TransporterOKPO_DB { get; set; } = "";
    public bool TransporterOKPO_Hidden_Priv { get; set; }
    [NotMapped]
    public bool TransporterOKPO_Hidden
    {
        get => TransporterOKPO_Hidden_Priv;
        set
        {
            TransporterOKPO_Hidden_Priv = value;
        }
    }
    [NotMapped]
    [FormProperty(true, "null-17-18", "ОКПО", "перевозчика", "18")]
    public RamAccess<string> TransporterOKPO
    {
        get
        {
            if (!TransporterOKPO_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(TransporterOKPO)))
                {
                    ((RamAccess<string>)Dictionary[nameof(TransporterOKPO)]).Value = TransporterOKPO_DB;
                    return (RamAccess<string>)Dictionary[nameof(TransporterOKPO)];
                }
                var rm = new RamAccess<string>(TransporterOKPO_Validation, TransporterOKPO_DB);
                rm.PropertyChanged += TransporterOKPOValueChanged;
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
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }
    }
    private void TransporterOKPOValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            TransporterOKPO_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool TransporterOKPO_Validation(RamAccess<string> value)//Done
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
        if (value.Value.Length != 8 && value.Value.Length != 14)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        Regex mask = new("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
        if (!mask.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region StoragePlaceName
    public string StoragePlaceName_DB { get; set; } = "";
    public bool StoragePlaceName_Hidden_Priv { get; set; }
    [NotMapped]
    public bool StoragePlaceName_Hidden
    {
        get => StoragePlaceName_Hidden_Priv;
        set
        {
            StoragePlaceName_Hidden_Priv = value;
        }
    }
    [NotMapped]
    [FormProperty(true, "null-19-20", "Пункт хранения", "наименование", "19")]
    public RamAccess<string> StoragePlaceName
    {
        get
        {
            if (!StoragePlaceName_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(StoragePlaceName)))
                {
                    ((RamAccess<string>)Dictionary[nameof(StoragePlaceName)]).Value = StoragePlaceName_DB;
                    return (RamAccess<string>)Dictionary[nameof(StoragePlaceName)];
                }
                var rm = new RamAccess<string>(StoragePlaceName_Validation, StoragePlaceName_DB);
                rm.PropertyChanged += StoragePlaceNameValueChanged;
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
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
    }

    private void StoragePlaceNameValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            StoragePlaceName_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool StoragePlaceName_Validation(RamAccess<string> value)//Ready
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

    #region StoragePlaceCode
    public string StoragePlaceCode_DB { get; set; } = "";
    public bool StoragePlaceCode_Hidden_Priv { get; set; }
    [NotMapped]
    public bool StoragePlaceCode_Hidden
    {
        get => StoragePlaceCode_Hidden_Priv;
        set
        {
            StoragePlaceCode_Hidden_Priv = value;
        }
    }
    [NotMapped]
    [FormProperty(true, "null-19-20", "Пункт хранения", "код", "20")]
    public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
    {
        get
        {
            if (!StoragePlaceCode_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(StoragePlaceCode)))
                {
                    ((RamAccess<string>)Dictionary[nameof(StoragePlaceCode)]).Value = StoragePlaceCode_DB;
                    return (RamAccess<string>)Dictionary[nameof(StoragePlaceCode)];
                }
                var rm = new RamAccess<string>(StoragePlaceCode_Validation, StoragePlaceCode_DB);
                rm.PropertyChanged += StoragePlaceCodeValueChanged;
                Dictionary.Add(nameof(StoragePlaceCode), rm);
                return (RamAccess<string>)Dictionary[nameof(StoragePlaceCode)];
            }
            else
            {
                var tmp = new RamAccess<string>(null, null);
                return tmp;
            }
        }
        set
        {
            if (!StoragePlaceCode_Hidden_Priv)
            {
                StoragePlaceCode_DB = value.Value;
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
    }

    private void StoragePlaceCodeValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            StoragePlaceCode_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        //List<string> lst = new List<string>();//HERE binds spr
        //if (!lst.Contains(value.Value))
        //{
        //    value.AddError("Недопустимое значение"); return false;
        //}
        //return true;
        if (String.IsNullOrEmpty(value.Value) || value.Value == "-")
        {
            return true;
        }
        Regex a = new("^[0-9]{8}$");
        if (!a.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); return false;
        }
        var tmp = value.Value;
        if (tmp.Length == 8)
        {
            Regex a0 = new("^[1-9]");
            if (!a0.IsMatch(tmp[..1]))
            {
                value.AddError($"Недопустимый вид пункта - {tmp[..1]}");
            }
            Regex a1 = new("^[1-3]");
            if (!a1.IsMatch(tmp.Substring(1, 1)))
            {
                value.AddError($"Недопустимое состояние пункта - {tmp.Substring(1, 1)}");
            }
            Regex a2 = new("^[1-2]");
            if (!a2.IsMatch(tmp.Substring(2, 1)))
            {
                value.AddError($"Недопустимая изоляция от окружающей среды - {tmp.Substring(2, 1)}");
            }
            Regex a3 = new("^[1-59]");
            if (!a3.IsMatch(tmp.Substring(3, 1)))
            {
                value.AddError($"Недопустимая зона нахождения пунтка - {tmp.Substring(3, 1)}");
            }
            Regex a4 = new("^[0-4]");
            if (!a4.IsMatch(tmp.Substring(4, 1)))
            {
                value.AddError($"Недопустимое значение пункта - {tmp.Substring(4, 1)}");
            }
            Regex a5 = new("^[1-49]");
            if (!a5.IsMatch(tmp.Substring(5, 1)))
            {
                value.AddError(
                    $"Недопустимое размещение пункта хранения относительно поверхности земли - {tmp.Substring(5, 1)}");
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

    #region CodeRAO
    public string CodeRAO_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "null-21", "код", "21")]
    public RamAccess<string> CodeRAO
    {
        get
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
        set
        {
            CodeRAO_DB = value.Value;
            OnPropertyChanged(nameof(CodeRAO));
        }
    }
    private void CodeRAOValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var tmp = ((RamAccess<string>)Value).Value.ToLower();
            tmp = tmp.Replace("х", "x");
            CodeRAO_DB = tmp;
        }
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
        Regex a = new("^[0-9x+]{11}$");
        if (!a.IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region StatusRAO
    public string StatusRAO_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "null-22", "статус", "22")]
    public RamAccess<string> StatusRAO  //1 cyfer or OKPO.
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(StatusRAO)))
            {
                ((RamAccess<string>)Dictionary[nameof(StatusRAO)]).Value = StatusRAO_DB;
                return (RamAccess<string>)Dictionary[nameof(StatusRAO)];
            }
            var rm = new RamAccess<string>(StatusRAO_Validation, StatusRAO_DB);
            rm.PropertyChanged += StatusRAOValueChanged;
            Dictionary.Add(nameof(StatusRAO), rm);
            return (RamAccess<string>)Dictionary[nameof(StatusRAO)];
        }
        set
        {
            StatusRAO_DB = value.Value;
            OnPropertyChanged(nameof(StatusRAO));
        }
    }
    private void StatusRAOValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            StatusRAO_DB = ((RamAccess<string>)Value).Value;
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
            int tmp;
            try
            {
                tmp = int.Parse(value.Value);
                if (tmp < 1 || (tmp > 4 && tmp != 6 && tmp != 9))
                {
                    value.AddError("Недопустимое значение"); return false;
                }
            }
            catch (Exception)
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        if (value.Value.Length != 8 && value.Value.Length != 14)
        {
            value.AddError("Недопустимое значение"); return false;
        }
        Regex mask = new("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
        if (!mask.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    #endregion

    #region VolumeOutOfPack
    public string VolumeOutOfPack_DB { get; set; }
    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "null-23", "объем без упаковки, куб. м", "23")]
    public RamAccess<string> VolumeOutOfPack
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(VolumeOutOfPack)))
            {
                ((RamAccess<string>)Dictionary[nameof(VolumeOutOfPack)]).Value = VolumeOutOfPack_DB;
                return (RamAccess<string>)Dictionary[nameof(VolumeOutOfPack)];
            }
            else
            {
                var rm = new RamAccess<string>(VolumeOutOfPack_Validation, VolumeOutOfPack_DB);
                rm.PropertyChanged += VolumeOutOfPackValueChanged;
                Dictionary.Add(nameof(VolumeOutOfPack), rm);
                return (RamAccess<string>)Dictionary[nameof(VolumeOutOfPack)];
            }
        }
        set
        {
            VolumeOutOfPack_DB = value.Value;
            OnPropertyChanged(nameof(VolumeOutOfPack));
        }
    }
    private void VolumeOutOfPackValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
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
                try
                {
                    var value2 = Convert.ToDouble(value1);
                    value1 = $"{value2:0.######################################################e+00}";
                }
                catch (Exception) { }
            }
            VolumeOutOfPack_DB = value1;
        }
    }
    private bool VolumeOutOfPack_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    #endregion

    #region MassOutOfPack
    public string MassOutOfPack_DB { get; set; }
    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "null-24", "масса без упаковки (нетто), т", "24")]
    public RamAccess<string> MassOutOfPack
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
            OnPropertyChanged(nameof(MassOutOfPack));
        }
    }
    private void MassOutOfPackValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
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
                try
                {
                    var value2 = Convert.ToDouble(value1);
                    value1 = $"{value2:0.######################################################e+00}";
                }
                catch (Exception) { }
            }
            MassOutOfPack_DB = value1;
        }
    }
    private bool MassOutOfPack_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region Quantity
    public string Quantity_DB { get; set; }
    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "null-25", "количество ОЗИИИ, шт", "25")]
    public RamAccess<string> Quantity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Quantity)))
            {
                ((RamAccess<string>)Dictionary[nameof(Quantity)]).Value = Quantity_DB;
                return (RamAccess<string>)Dictionary[nameof(Quantity)];
            }
            else
            {
                var rm = new RamAccess<string>(Quantity_Validation, Quantity_DB);
                rm.PropertyChanged += QuantityValueChanged;
                Dictionary.Add(nameof(Quantity), rm);
                return (RamAccess<string>)Dictionary[nameof(Quantity)];
            }
        }
        set
        {
            Quantity_DB = value.Value;
            OnPropertyChanged(nameof(Quantity));
        }
    }// positive int.

    private void QuantityValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Quantity_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool Quantity_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        if (value.Value.Equals("-"))
        {
            return true;
        }
        try
        {
            if (int.Parse(value.Value) <= 0)
            {
                value.AddError("Число должно быть больше нуля");
                return false;
            }
        }
        catch (Exception)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region TritiumActivity
    public string TritiumActivity_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "Суммарная активность", "тритий", "26")]
    public RamAccess<string> TritiumActivity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(TritiumActivity)))
            {
                ((RamAccess<string>)Dictionary[nameof(TritiumActivity)]).Value = TritiumActivity_DB;
                return (RamAccess<string>)Dictionary[nameof(TritiumActivity)];
            }
            else
            {
                var rm = new RamAccess<string>(TritiumActivity_Validation, TritiumActivity_DB);
                rm.PropertyChanged += TritiumActivityValueChanged;
                Dictionary.Add(nameof(TritiumActivity), rm);
                return (RamAccess<string>)Dictionary[nameof(TritiumActivity)];
            }
        }
        set
        {
            TritiumActivity_DB = value.Value;
            OnPropertyChanged(nameof(TritiumActivity));
        }
    }
    private void TritiumActivityValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
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
                try
                {
                    var value2 = Convert.ToDouble(value1);
                    value1 = $"{value2:0.######################################################e+00}";
                }
                catch (Exception) { }
            }
            TritiumActivity_DB = value1;
        }
    }
    private bool TritiumActivity_Validation(RamAccess<string> value)//TODO
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
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    #endregion

    #region BetaGammaActivity
    public string BetaGammaActivity_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "Суммарная активность", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "27")]
    public RamAccess<string> BetaGammaActivity
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
            OnPropertyChanged(nameof(BetaGammaActivity));
        }
    }
    private void BetaGammaActivityValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
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
                try
                {
                    var value2 = Convert.ToDouble(value1);
                    value1 = $"{value2:0.######################################################e+00}";
                }
                catch (Exception) { }
            }
            BetaGammaActivity_DB = value1;
        }
    }
    private bool BetaGammaActivity_Validation(RamAccess<string> value)//TODO
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
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    #endregion

    #region AlphaActivity
    public string AlphaActivity_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "Суммарная активность", "альфа-излучающие радионуклиды (исключая трансурановые)", "28")]
    public RamAccess<string> AlphaActivity
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
            OnPropertyChanged(nameof(AlphaActivity));
        }
    }
    private void AlphaActivityValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
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
                try
                {
                    var value2 = Convert.ToDouble(value1);
                    value1 = $"{value2:0.######################################################e+00}";
                }
                catch (Exception) { }
            }
            AlphaActivity_DB = value1;
        }
    }
    private bool AlphaActivity_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
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
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    #endregion

    #region TransuraniumActivity
    public string TransuraniumActivity_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "Суммарная активность", "трансурановые радионуклиды", "29")]
    public RamAccess<string> TransuraniumActivity
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
            OnPropertyChanged(nameof(TransuraniumActivity));
        }
    }
    private void TransuraniumActivityValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
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
                try
                {
                    var value2 = Convert.ToDouble(value1);
                    value1 = $"{value2:0.######################################################e+00}";
                }
                catch (Exception) { }
            }
            TransuraniumActivity_DB = value1;
        }
    }
    private bool TransuraniumActivity_Validation(RamAccess<string> value)//TODO
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
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region RefineOrSortRAOCode
    public string RefineOrSortRAOCode_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Сведения о РАО", "null-30", "Код переработки/сортировки РАО", "30")]
    public RamAccess<string> RefineOrSortRAOCode //2 cyfer code or empty.
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(RefineOrSortRAOCode)))
            {
                ((RamAccess<string>)Dictionary[nameof(RefineOrSortRAOCode)]).Value = RefineOrSortRAOCode_DB;
                return (RamAccess<string>)Dictionary[nameof(RefineOrSortRAOCode)];
            }
            else
            {
                var rm = new RamAccess<string>(RefineOrSortRAOCode_Validation, RefineOrSortRAOCode_DB);
                rm.PropertyChanged += RefineOrSortRAOCodeValueChanged;
                Dictionary.Add(nameof(RefineOrSortRAOCode), rm);
                return (RamAccess<string>)Dictionary[nameof(RefineOrSortRAOCode)];
            }
        }
        set
        {
            RefineOrSortRAOCode_DB = value.Value;
            OnPropertyChanged(nameof(RefineOrSortRAOCode));
        }
    }//If change this change validation

    private void RefineOrSortRAOCodeValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            RefineOrSortRAOCode_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool RefineOrSortRAOCode_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        if (!Spravochniks.SprRifineOrSortCodes.Contains(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region Subsidy
    public string Subsidy_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "null-31-1", "null-31-2", "Субсидия, %", "31")]
    public RamAccess<string> Subsidy // 0<number<=100 or empty.
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Subsidy)))
            {
                ((RamAccess<string>)Dictionary[nameof(Subsidy)]).Value = Subsidy_DB;
                return (RamAccess<string>)Dictionary[nameof(Subsidy)];
            }
            var rm = new RamAccess<string>(Subsidy_Validation, Subsidy_DB);
            rm.PropertyChanged += SubsidyValueChanged;
            Dictionary.Add(nameof(Subsidy), rm);
            return (RamAccess<string>)Dictionary[nameof(Subsidy)];
        }
        set
        {
            Subsidy_DB = value.Value;
            OnPropertyChanged(nameof(Subsidy));
        }
    }
    private void SubsidyValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Subsidy_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool Subsidy_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
        {
            return true;
        }
        try
        {
            var tmp = int.Parse(value.Value);
            if (tmp is not (>= 0 and <= 100))
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
    #endregion

    #region FcpNumber

    public string FcpNumber_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "null-32_1", "null-32_2", "Номер мероприятия ФЦП", "32")]
    public RamAccess<string> FcpNumber
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(FcpNumber)))
            {
                ((RamAccess<string>)Dictionary[nameof(FcpNumber)]).Value = FcpNumber_DB;
                return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
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

    #endregion 

    #endregion

    protected override bool OperationCode_Validation(RamAccess<string> value)//OK
    {
        value.ClearErrors();
        if (value.Value is null or "-")
        {
            return true;
        }
        if (!Spravochniks.SprOpCodes.Contains(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (!new Regex(@"^\d{2}$").IsMatch(value.Value)
            || !byte.TryParse(value.Value, out var byteValue)
            || byteValue is not (1 or 10 or 18 or >= 21 and <= 29 or >= 31 and <= 39 or 51 or 52 or 55 or 63 or 64 or 68
                or 97 or 98 or 99))
        {
            value.AddError("Код операции не может быть использован в форме 1.7");
            return false;
        }

        return true;
    }
    protected override bool DocumentNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors(); return true;
    }
    protected override bool DocumentVid_Validation(RamAccess<byte?> value)
    {
        value.ClearErrors();
        if (Spravochniks.SprDocumentVidName.Any())
        {
            return false;
        }
        value.AddError("Недопустимое значение");
        return true;
    }
    protected override bool DocumentDate_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if(value.Value is null or "" or "-")
        {
            return true;
        }
        var tmp = value.Value;
        Regex b = new("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
        if (b.IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        Regex a = new("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
        if (!a.IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        try { DateTimeOffset.Parse(tmp); }
        catch (Exception)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        var ab = OperationCode.Value is "51" or "52";
        var c =  OperationCode.Value == "68";
        var d =  OperationCode.Value is "18" or "55";
        if (ab || c || d)
        {
            if (!tmp.Equals(OperationDate))
            {
                //value.AddError("Заполните примечание");// to do note handling
                return true;
            }
        }
        return true;
    }

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
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelDate(FormingDate_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelString(PassportNumber_DB);
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDouble(Volume_DB);
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelDouble(Mass_DB);
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ConvertToExcelString(Radionuclids_DB);
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ConvertToExcelDouble(SpecificActivity_DB);
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = DocumentVid_DB is null ? "-" : DocumentVid_DB;
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ConvertToExcelString(DocumentNumber_DB);
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ConvertToExcelDate(DocumentDate_DB);
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

        return 29;
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
        
        return 29;
    }
    #endregion

    #region IDataGridColumn
    private static DataGridColumns _DataGridColumns { get; set; }
    public override DataGridColumns GetColumnStructure(string param = "")
    {
        if (_DataGridColumns == null)
        {
            #region NumberInOrder (1)
            var numberInOrderR = ((FormPropertyAttribute)typeof(Form).GetProperty(nameof(NumberInOrder))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())?.GetDataColumnStructureD();
            numberInOrderR.SetSizeColToAllLevels(50);
            numberInOrderR.Binding = nameof(NumberInOrder);
            numberInOrderR.Blocked = true;
            numberInOrderR.ChooseLine = true;
            #endregion

            #region OperationCode (2)
            var operationCodeR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(OperationCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())?.GetDataColumnStructureD(numberInOrderR);
            operationCodeR.SetSizeColToAllLevels(88);
            operationCodeR.Binding = nameof(OperationCode);
            numberInOrderR += operationCodeR;
            #endregion

            #region OperationDate (3)
            var operationDateR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(OperationDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())?.GetDataColumnStructureD(numberInOrderR);
            operationDateR.SetSizeColToAllLevels(88);
            operationDateR.Binding = nameof(OperationDate);
            numberInOrderR += operationDateR;
            #endregion

            #region PackName (4)
            var packNameR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(PackName))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())?.GetDataColumnStructureD(numberInOrderR);
            packNameR.SetSizeColToAllLevels(163);
            packNameR.Binding = nameof(PackName);
            numberInOrderR += packNameR;
            #endregion

            #region PackType (5)
            var packTypeR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(PackType))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())?.GetDataColumnStructureD(numberInOrderR);
            packTypeR.SetSizeColToAllLevels(88);
            packTypeR.Binding = nameof(PackType);
            numberInOrderR += packTypeR;
            #endregion

            #region PackFactoryNumber (6)
            var packFactoryNumberR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(PackFactoryNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())?.GetDataColumnStructureD(numberInOrderR);
            packFactoryNumberR.SetSizeColToAllLevels(170);
            packFactoryNumberR.Binding = nameof(PackFactoryNumber);
            numberInOrderR += packFactoryNumberR;
            #endregion

            #region PackNumber (7)
            var packNumberR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(PackNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())?.GetDataColumnStructureD(numberInOrderR);
            packNumberR.SetSizeColToAllLevels(260);
            packNumberR.Binding = nameof(PackNumber);
            numberInOrderR += packNumberR;
            #endregion

            #region FormingDate (8)
            var formingDateR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(FormingDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())?.GetDataColumnStructureD(numberInOrderR);
            formingDateR.SetSizeColToAllLevels(133);
            formingDateR.Binding = nameof(FormingDate);
            numberInOrderR += formingDateR;
            #endregion

            #region PassportNumber (9)
            var passportNumberR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(PassportNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())?.GetDataColumnStructureD(numberInOrderR);
            passportNumberR.SetSizeColToAllLevels(143);
            passportNumberR.Binding = nameof(PassportNumber);
            numberInOrderR += passportNumberR;
            #endregion

            #region Volume (10)
            var volumeR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(Volume))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())?.GetDataColumnStructureD(numberInOrderR);
            volumeR.SetSizeColToAllLevels(88);
            volumeR.Binding = nameof(Volume);
            numberInOrderR += volumeR;
            #endregion

            #region Mass (11)
            var massR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(Mass))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())?.GetDataColumnStructureD(numberInOrderR);
            massR.SetSizeColToAllLevels(95);
            massR.Binding = nameof(Mass);
            numberInOrderR += massR;
            #endregion

            #region Radionuclids (12)
            var radionuclidsR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(Radionuclids))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())?.GetDataColumnStructureD(numberInOrderR);
            radionuclidsR.SetSizeColToAllLevels(170);
            radionuclidsR.Binding = nameof(Radionuclids);
            numberInOrderR += radionuclidsR;
            #endregion

            #region SpecificActivity (13)
            var specificActivityR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(SpecificActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            specificActivityR.SetSizeColToAllLevels(163);
            specificActivityR.Binding = nameof(SpecificActivity);
            numberInOrderR += specificActivityR;
            #endregion

            #region DocumentVid (14)
            var documentVidR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentVid))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            documentVidR.SetSizeColToAllLevels(88);
            documentVidR.Binding = nameof(DocumentVid);
            numberInOrderR += documentVidR;
            #endregion

            #region DocumentNumber (15)
            var documentNumberR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            documentNumberR.SetSizeColToAllLevels(103);
            documentNumberR.Binding = nameof(DocumentNumber);
            numberInOrderR += documentNumberR;
            #endregion

            #region DocumentDate (16)
            var documentDateR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            documentDateR.SetSizeColToAllLevels(88);
            documentDateR.Binding = nameof(DocumentDate);
            numberInOrderR += documentDateR;
            #endregion

            #region ProviderOrRecieverOKPO (17)
            var providerOrRecieverOkpor = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(ProviderOrRecieverOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            providerOrRecieverOkpor.SetSizeColToAllLevels(100);
            providerOrRecieverOkpor.Binding = nameof(ProviderOrRecieverOKPO);
            numberInOrderR += providerOrRecieverOkpor;
            #endregion

            #region TransporterOKPO (18)
            var transporterOkpor = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(TransporterOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            transporterOkpor.SetSizeColToAllLevels(163);
            transporterOkpor.Binding = nameof(TransporterOKPO);
            numberInOrderR += transporterOkpor;
            #endregion

            #region StoragePlaceName (19)
            var storagePlaceNameR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(StoragePlaceName))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            storagePlaceNameR.SetSizeColToAllLevels(103);
            storagePlaceNameR.Binding = nameof(StoragePlaceName);
            numberInOrderR += storagePlaceNameR;
            #endregion

            #region StoragePlaceCode (20)
            var storagePlaceCodeR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(StoragePlaceCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            storagePlaceCodeR.SetSizeColToAllLevels(88);
            storagePlaceCodeR.Binding = nameof(StoragePlaceCode);
            numberInOrderR += storagePlaceCodeR;
            #endregion

            #region CodeRAO (21)
            var codeRaor = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(CodeRAO))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            codeRaor.SetSizeColToAllLevels(88);
            codeRaor.Binding = nameof(CodeRAO);
            numberInOrderR += codeRaor;
            #endregion

            #region StatusRAO (22)
            var statusRaor = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(StatusRAO))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            statusRaor.SetSizeColToAllLevels(88);
            statusRaor.Binding = nameof(StatusRAO);
            numberInOrderR += statusRaor;
            #endregion

            #region VolumeOutOfPack (23)
            var volumeOutOfPackR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(VolumeOutOfPack))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            volumeOutOfPackR.SetSizeColToAllLevels(163);
            volumeOutOfPackR.Binding = nameof(VolumeOutOfPack);
            numberInOrderR += volumeOutOfPackR;
            #endregion

            #region MassOutOfPack (24)
            var massOutOfPackR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(MassOutOfPack))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            massOutOfPackR.SetSizeColToAllLevels(170);
            massOutOfPackR.Binding = nameof(MassOutOfPack);
            numberInOrderR += massOutOfPackR;
            #endregion

            #region Quantity (25)
            var quantityR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(Quantity))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            quantityR.SetSizeColToAllLevels(140);
            quantityR.Binding = nameof(Quantity);
            numberInOrderR += quantityR;
            #endregion

            #region TritiumActivity (26)
            var tritiumActivityR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(TritiumActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            tritiumActivityR.SetSizeColToAllLevels(163);
            tritiumActivityR.Binding = nameof(TritiumActivity);
            numberInOrderR += tritiumActivityR;
            #endregion

            #region BetaGammaActivity (27)
            var betaGammaActivityR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(BetaGammaActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            betaGammaActivityR.SetSizeColToAllLevels(180);
            betaGammaActivityR.Binding = nameof(BetaGammaActivity);
            numberInOrderR += betaGammaActivityR;
            #endregion

            #region AlphaActivity (28)
            var alphaActivityR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(AlphaActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            alphaActivityR.SetSizeColToAllLevels(185);
            alphaActivityR.Binding = nameof(AlphaActivity);
            numberInOrderR += alphaActivityR;
            #endregion

            #region TransuraniumActivity (29)
            var transuraniumActivityR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(TransuraniumActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            transuraniumActivityR.SetSizeColToAllLevels(200);
            transuraniumActivityR.Binding = nameof(TransuraniumActivity);
            numberInOrderR += transuraniumActivityR;
            #endregion

            #region RefineOrSortRAOCode (30)
            var refineOrSortRaoCodeR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(RefineOrSortRAOCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            refineOrSortRaoCodeR.SetSizeColToAllLevels(120);
            refineOrSortRaoCodeR.Binding = nameof(RefineOrSortRAOCode);
            numberInOrderR += refineOrSortRaoCodeR;
            #endregion

            #region Subsidy (31)
            var subsidyR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(Subsidy))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            subsidyR.SetSizeColToAllLevels(88);
            subsidyR.Binding = nameof(Subsidy);
            numberInOrderR += subsidyR;
            #endregion

            #region FcpNumber (32)
            var fcpNumberR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(FcpNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(numberInOrderR);
            fcpNumberR.SetSizeColToAllLevels(163);
            fcpNumberR.Binding = nameof(FcpNumber);
            numberInOrderR += fcpNumberR;
            #endregion

            _DataGridColumns = numberInOrderR;
        }
        return _DataGridColumns;
    }
    #endregion
}