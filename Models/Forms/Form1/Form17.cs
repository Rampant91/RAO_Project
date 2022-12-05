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
    [FormProperty(true,"Сведения об упаковке", "null-4", "наименование","4")]
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
    [FormProperty(true,"Сведения об упаковке", "null-5", "тип", "5")]
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
    [FormProperty(true,"Сведения об упаковке", "null-7", "номер упаковки (идентификационный код)","7")]
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
    [FormProperty(true,"Сведения об упаковке", "null-6", "заводской номер","6")]
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
    [FormProperty(true,"Сведения об упаковке", "null-8", "дата формирования","8")]
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
        if (string.IsNullOrEmpty(value.Value))
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
    [FormProperty(true,"Сведения об упаковке", "null-10", "объем, куб. м","10")]
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
        if (string.IsNullOrEmpty(value.Value)||value.Value=="-")
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
    [FormProperty(true,"Сведения об упаковке", "null-11", "масса брутто, т","11")]
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
                try
                {
                    var value2 = Convert.ToDouble(value1);
                    value1 = $"{value2:0.######################################################e+00}";
                }
                catch (Exception) { }
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
    [FormProperty(true,"Сведения об упаковке", "null-9", "номер паспорта","9")]
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

    #region Radionuclids
    public string Radionuclids_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения об упаковке", "Радионуклидный состав", "наименование радионуклида","12")]
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
    [FormProperty(true,"Сведения об упаковке", "Радионуклидный состав", "удельная активность, Бк/г", "13")]
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
    [FormProperty(true, "null-17-18", "ОКПО","поставщика или получателя","17")]
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
    [FormProperty(true, "null-17-18", "ОКПО","перевозчика","18")]
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
    [FormProperty(true, "null-19-20", "Пункт хранения","наименование","19")]
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
    [FormProperty(true, "null-19-20", "Пункт хранения","код","20")]
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
        if (String.IsNullOrEmpty(value.Value)|| value.Value == "-")
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

    #region Subsidy
    public string Subsidy_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "null-31-1", "null-31-2","Субсидия, %","31")]
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
            if (!(tmp > 0 && tmp <= 100))
            {
                value.AddError("Недопустимое значение"); return false;
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
    [FormProperty(true, "null-32_1", "null-32_2","Номер мероприятия ФЦП","32")]
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

    #region CodeRAO
    public string CodeRAO_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения о РАО", "null-21", "код","21")]
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
        if(string.IsNullOrEmpty(value.Value))
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
    [FormProperty(true,"Сведения о РАО", "null-22", "статус","22")]
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
    [FormProperty(true,"Сведения о РАО", "null-23", "объем без упаковки, куб. м","23")]
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
    [FormProperty(true,"Сведения о РАО", "null-24", "масса без упаковки (нетто), т","24")]
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
    [FormProperty(true,"Сведения о РАО", "null-25", "количество ОЗИИИ, шт","25")]
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
    [FormProperty(true,"Сведения о РАО", "Суммарная активность", "тритий","26")]
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
        if(string.IsNullOrEmpty(value.Value))
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
    [FormProperty(true,"Сведения о РАО", "Суммарная активность", "бета-, гамма-излучающие радионуклиды (исключая тритий)","27")]
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
        if(string.IsNullOrEmpty(value.Value))
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
    [FormProperty(true,"Сведения о РАО", "Суммарная активность", "альфа-излучающие радионуклиды (исключая трансурановые)","28")]
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
        if(string.IsNullOrEmpty(value.Value))
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
    [FormProperty(true,"Сведения о РАО", "Суммарная активность", "трансурановые радионуклиды","28")]
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
        if(string.IsNullOrEmpty(value.Value))
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
    [FormProperty(true,"Сведения о РАО", "null-30", "Код переработки/сортировки РАО","30")]
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

    protected override bool OperationCode_Validation(RamAccess<string> value)//OK
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            return true;
        }
        if (!Spravochniks.SprOpCodes.Contains(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        var a0 = value.Value == "01";
        var a1 = value.Value == "10";
        var a2 = value.Value == "18";
        var a3 = value.Value == "55";
        var a4 = value.Value == "63";
        var a5 = value.Value == "64";
        var a6 = value.Value == "68";
        var a7 = value.Value == "97";
        var a8 = value.Value == "98";
        var a9 = value.Value == "99";
        var a12 = value.Value == "51";
        var a13 = value.Value == "52";
        var a10 = int.Parse(value.Value) >= 21 && int.Parse(value.Value) <= 29;
        var a11 = int.Parse(value.Value) >= 31 && int.Parse(value.Value) <= 39;
        if (!(a0 || a1 || a2 || a3 || a4 || a5 || a6 || a7 || a8 || a9 || a10 || a11 || a12 || a13))
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
        if(string.IsNullOrEmpty(value.Value))
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
    public void ExcelGetRow(ExcelWorksheet worksheet, int Row)
    {
        base.ExcelGetRow(worksheet, Row);
        PackName_DB = Convert.ToString(worksheet.Cells[Row, 4].Value);
        PackType_DB = Convert.ToString(worksheet.Cells[Row, 5].Value);
        PackFactoryNumber_DB = Convert.ToString(worksheet.Cells[Row, 6].Value);
        PackNumber_DB = Convert.ToString(worksheet.Cells[Row, 7].Value);
        FormingDate_DB = Convert.ToString(worksheet.Cells[Row, 8].Value);
        PassportNumber_DB = Convert.ToString(worksheet.Cells[Row, 9].Value);
        Volume_DB = Convert.ToString(worksheet.Cells[Row, 10].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 10].Value), out var val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 10].Value);
        Mass_DB = Convert.ToString(worksheet.Cells[Row, 11].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 11].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 11].Value);
        Radionuclids_DB = Convert.ToString(worksheet.Cells[Row, 12].Value);
        SpecificActivity_DB = Convert.ToString(worksheet.Cells[Row, 13].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 13].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 13].Value);
        DocumentVid_DB = Convert.ToByte(worksheet.Cells[Row, 14].Value);
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[Row, 15].Value);
        DocumentDate_DB = Convert.ToString(worksheet.Cells[Row, 16].Value);
        ProviderOrRecieverOKPO_DB = Convert.ToString(worksheet.Cells[Row, 17].Value);
        TransporterOKPO_DB = Convert.ToString(worksheet.Cells[Row, 18].Value);
        StoragePlaceName_DB = Convert.ToString(worksheet.Cells[Row, 19].Value);
        StoragePlaceCode_DB = Convert.ToString(worksheet.Cells[Row, 20].Value);
        CodeRAO_DB = Convert.ToString(worksheet.Cells[Row, 21].Value);
        StatusRAO_DB = Convert.ToString(worksheet.Cells[Row, 22].Value);
        VolumeOutOfPack_DB = Convert.ToString(worksheet.Cells[Row, 23].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 23].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 23].Value);
        MassOutOfPack_DB = Convert.ToString(worksheet.Cells[Row, 24].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 24].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 24].Value);
        Quantity_DB = Convert.ToString(worksheet.Cells[Row, 25].Value);
        TritiumActivity_DB = Convert.ToString(worksheet.Cells[Row, 26].Value);
        BetaGammaActivity_DB = Convert.ToString(worksheet.Cells[Row, 27].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 27].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 27].Value);
        AlphaActivity_DB = Convert.ToString(worksheet.Cells[Row, 28].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 28].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 28].Value);
        TransuraniumActivity_DB = Convert.ToString(worksheet.Cells[Row, 29].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 29].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 29].Value);
        RefineOrSortRAOCode_DB = Convert.ToString(worksheet.Cells[Row, 30].Value);
        Subsidy_DB = Convert.ToString(worksheet.Cells[Row, 31].Value);
        FcpNumber_DB = Convert.ToString(worksheet.Cells[Row, 32].Value);
    }
    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpon = true, string sumNumber = "")
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpon);
        column += transpon ? cnt : 0;
        row += !transpon ? cnt : 0;

        worksheet.Cells[row + 0, column + 0].Value = PackName_DB;
        worksheet.Cells[row + (!transpon ? 1 : 0), column + (transpon ? 1 : 0)].Value = PackType_DB;
        worksheet.Cells[row + (!transpon ? 2 : 0), column + (transpon ? 2 : 0)].Value = PackFactoryNumber_DB;
        worksheet.Cells[row + (!transpon ? 3 : 0), column + (transpon ? 3 : 0)].Value = PackNumber_DB;
        worksheet.Cells[row + (!transpon ? 4 : 0), column + (transpon ? 4 : 0)].Value = FormingDate_DB;
        worksheet.Cells[row + (!transpon ? 5 : 0), column + (transpon ? 5 : 0)].Value = PassportNumber_DB;
        worksheet.Cells[row + (!transpon ? 6 : 0), column + (transpon ? 6 : 0)].Value = string.IsNullOrEmpty(Volume_DB) || Volume_DB == "-" ? 0 : double.TryParse(Volume_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out var val) ? val : Volume_DB;
        worksheet.Cells[row + (!transpon ? 7 : 0), column + (transpon ? 7 : 0)].Value = string.IsNullOrEmpty(Mass_DB) || Mass_DB == "-" ? 0  : double.TryParse(Mass_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : Mass_DB;
        worksheet.Cells[row + (!transpon ? 8 : 0), column + (transpon ? 8 : 0)].Value = Radionuclids_DB;
        worksheet.Cells[row + (!transpon ? 9 : 0), column + (transpon ? 9 : 0)].Value = string.IsNullOrEmpty(SpecificActivity_DB) || SpecificActivity_DB == "-" ? 0  : double.TryParse(SpecificActivity_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : SpecificActivity_DB;
        worksheet.Cells[row + (!transpon ? 10 : 0), column + (transpon ? 10 : 0)].Value = DocumentVid_DB;
        worksheet.Cells[row + (!transpon ? 11 : 0), column + (transpon ? 11 : 0)].Value = DocumentNumber_DB;
        worksheet.Cells[row + (!transpon ? 12 : 0), column + (transpon ? 12 : 0)].Value = DocumentDate_DB;
        worksheet.Cells[row + (!transpon ? 13 : 0), column + (transpon ? 13 : 0)].Value = ProviderOrRecieverOKPO_DB;
        worksheet.Cells[row + (!transpon ? 14 : 0), column + (transpon ? 14 : 0)].Value = TransporterOKPO_DB;
        worksheet.Cells[row + (!transpon ? 15 : 0), column + (transpon ? 15 : 0)].Value = StoragePlaceName_DB;
        worksheet.Cells[row + (!transpon ? 16 : 0), column + (transpon ? 16 : 0)].Value = StoragePlaceCode_DB;
        worksheet.Cells[row + (!transpon ? 17 : 0), column + (transpon ? 17 : 0)].Value = CodeRAO_DB;
        worksheet.Cells[row + (!transpon ? 18 : 0), column + (transpon ? 18 : 0)].Value = StatusRAO_DB;
        worksheet.Cells[row + (!transpon ? 19 : 0), column + (transpon ? 19 : 0)].Value = string.IsNullOrEmpty(VolumeOutOfPack_DB) || VolumeOutOfPack_DB == "-" ? 0 : double.TryParse(VolumeOutOfPack_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : VolumeOutOfPack_DB;
        worksheet.Cells[row + (!transpon ? 20 : 0), column + (transpon ? 20 : 0)].Value = string.IsNullOrEmpty(MassOutOfPack_DB) || MassOutOfPack_DB == "-" ? 0 : double.TryParse(MassOutOfPack_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : MassOutOfPack_DB;
        worksheet.Cells[row + (!transpon ? 21 : 0), column + (transpon ? 21 : 0)].Value = Quantity_DB;
        worksheet.Cells[row + (!transpon ? 22 : 0), column + (transpon ? 22 : 0)].Value = TritiumActivity_DB;
        worksheet.Cells[row + (!transpon ? 23 : 0), column + (transpon ? 23 : 0)].Value = string.IsNullOrEmpty(BetaGammaActivity_DB) || BetaGammaActivity_DB == "-" ? 0 : double.TryParse(BetaGammaActivity_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : BetaGammaActivity_DB;
        worksheet.Cells[row + (!transpon ? 24 : 0), column + (transpon ? 24 : 0)].Value = string.IsNullOrEmpty(AlphaActivity_DB) || AlphaActivity_DB == "-" ? 0 : double.TryParse(AlphaActivity_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : AlphaActivity_DB;
        worksheet.Cells[row + (!transpon ? 25 : 0), column + (transpon ? 25 : 0)].Value = string.IsNullOrEmpty(TransuraniumActivity_DB) || TransuraniumActivity_DB == "-" ? 0 : double.TryParse(TransuraniumActivity_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : TransuraniumActivity_DB;
        worksheet.Cells[row + (!transpon ? 26 : 0), column + (transpon ? 26 : 0)].Value = RefineOrSortRAOCode_DB;
        worksheet.Cells[row + (!transpon ? 27 : 0), column + (transpon ? 27 : 0)].Value = Subsidy_DB;
        worksheet.Cells[row + (!transpon ? 28 : 0), column + (transpon ? 28 : 0)].Value = FcpNumber_DB;
        return 29;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpon = true)
    {
        var cnt = Form1.ExcelHeader(worksheet, row, column, transpon);
        column += transpon ? cnt : 0;
        row += !transpon ? cnt : 0;

        worksheet.Cells[row + 0, column + 0].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(PackName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 1 : 0), column + (transpon ? 1 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(PackType))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 2 : 0), column + (transpon ? 2 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(PackFactoryNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 3 : 0), column + (transpon ? 3 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(PackNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 4 : 0), column + (transpon ? 4 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(FormingDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 5 : 0), column + (transpon ? 5 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(PassportNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 6 : 0), column + (transpon ? 6 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(Volume))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 7 : 0), column + (transpon ? 7 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(Mass))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 8 : 0), column + (transpon ? 8 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(Radionuclids))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 9 : 0), column + (transpon ? 9 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(SpecificActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 10 : 0), column + (transpon ? 10 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(DocumentVid))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpon ? 11 : 0), column + (transpon ? 11 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(DocumentNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpon ? 12 : 0), column + (transpon ? 12 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(DocumentDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpon ? 13 : 0), column + (transpon ? 13 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(ProviderOrRecieverOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpon ? 14 : 0), column + (transpon ? 14 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(TransporterOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpon ? 15 : 0), column + (transpon ? 15 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(StoragePlaceName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpon ? 16 : 0), column + (transpon ? 16 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(StoragePlaceCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpon ? 17 : 0), column + (transpon ? 17 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(CodeRAO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 18 : 0), column + (transpon ? 18 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(StatusRAO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 19 : 0), column + (transpon ? 19 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(VolumeOutOfPack))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 20 : 0), column + (transpon ? 20 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(MassOutOfPack))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 21 : 0), column + (transpon ? 21 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(Quantity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 22 : 0), column + (transpon ? 22 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(TritiumActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 23 : 0), column + (transpon ? 23 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(BetaGammaActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 24 : 0), column + (transpon ? 24 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(AlphaActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 25 : 0), column + (transpon ? 25 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(TransuraniumActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 26 : 0), column + (transpon ? 26 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(RefineOrSortRAOCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 27 : 0), column + (transpon ? 27 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(Subsidy))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpon ? 28 : 0), column + (transpon ? 28 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form17,Models")?.GetProperty(nameof(FcpNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
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
            var NumberInOrderR = ((FormPropertyAttribute)typeof(Form).GetProperty(nameof(NumberInOrder))?.GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())?.GetDataColumnStructureD();
            NumberInOrderR.SetSizeColToAllLevels(50);
            NumberInOrderR.Binding = nameof(NumberInOrder);
            NumberInOrderR.Blocked = true;
            NumberInOrderR.ChooseLine = true;
            #endregion

            #region OperationCode (2)
            var OperationCodeR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(OperationCode)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            OperationCodeR.SetSizeColToAllLevels(88);
            OperationCodeR.Binding = nameof(OperationCode);
            NumberInOrderR += OperationCodeR;
            #endregion
            #region OperationDate (3)
            var OperationDateR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(OperationDate)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            OperationDateR.SetSizeColToAllLevels(88);
            OperationDateR.Binding = nameof(OperationDate);
            NumberInOrderR += OperationDateR;
            #endregion

            #region PackName (4)
            var PackNameR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(PackName)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PackNameR.SetSizeColToAllLevels(163);
            PackNameR.Binding = nameof(PackName);
            NumberInOrderR += PackNameR;
            #endregion
            #region PackType (5)
            var PackTypeR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(PackType)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PackTypeR.SetSizeColToAllLevels(88);
            PackTypeR.Binding = nameof(PackType);
            NumberInOrderR += PackTypeR;
            #endregion
            #region PackFactoryNumber (6)
            var PackFactoryNumberR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(PackFactoryNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PackFactoryNumberR.SetSizeColToAllLevels(170);
            PackFactoryNumberR.Binding = nameof(PackFactoryNumber);
            NumberInOrderR += PackFactoryNumberR;
            #endregion
            #region PackNumber (7)
            var PackNumberR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(PackNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PackNumberR.SetSizeColToAllLevels(260);
            PackNumberR.Binding = nameof(PackNumber);
            NumberInOrderR += PackNumberR;
            #endregion
            #region FormingDate (8)
            var FormingDateR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(FormingDate)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            FormingDateR.SetSizeColToAllLevels(133);
            FormingDateR.Binding = nameof(FormingDate);
            NumberInOrderR += FormingDateR;
            #endregion
            #region PassportNumber (9)
            var PassportNumberR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(PassportNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PassportNumberR.SetSizeColToAllLevels(143);
            PassportNumberR.Binding = nameof(PassportNumber);
            NumberInOrderR += PassportNumberR;
            #endregion
            #region Volume (10)
            var VolumeR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(Volume)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            VolumeR.SetSizeColToAllLevels(88);
            VolumeR.Binding = nameof(Volume);
            NumberInOrderR += VolumeR;
            #endregion
            #region Mass (11)
            var MassR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(Mass)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            MassR.SetSizeColToAllLevels(95);
            MassR.Binding = nameof(Mass);
            NumberInOrderR += MassR;
            #endregion
            #region Radionuclids (12)
            var RadionuclidsR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(Radionuclids)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            RadionuclidsR.SetSizeColToAllLevels(170);
            RadionuclidsR.Binding = nameof(Radionuclids);
            NumberInOrderR += RadionuclidsR;
            #endregion
            #region SpecificActivity (13)
            var SpecificActivityR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(SpecificActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            SpecificActivityR.SetSizeColToAllLevels(163);
            SpecificActivityR.Binding = nameof(SpecificActivity);
            NumberInOrderR += SpecificActivityR;
            #endregion

            #region DocumentVid (14)
            var DocumentVidR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentVid)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentVidR.SetSizeColToAllLevels(88);
            DocumentVidR.Binding = nameof(DocumentVid);
            NumberInOrderR += DocumentVidR;
            #endregion
            #region DocumentNumber (15)
            var DocumentNumberR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentNumberR.SetSizeColToAllLevels(103);
            DocumentNumberR.Binding = nameof(DocumentNumber);
            NumberInOrderR += DocumentNumberR;
            #endregion
            #region DocumentDate (16)
            var DocumentDateR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentDate)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentDateR.SetSizeColToAllLevels(88);
            DocumentDateR.Binding = nameof(DocumentDate);
            NumberInOrderR += DocumentDateR;
            #endregion

            #region ProviderOrRecieverOKPO (17)
            var ProviderOrRecieverOKPOR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(ProviderOrRecieverOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            ProviderOrRecieverOKPOR.SetSizeColToAllLevels(100);
            ProviderOrRecieverOKPOR.Binding = nameof(ProviderOrRecieverOKPO);
            NumberInOrderR += ProviderOrRecieverOKPOR;
            #endregion
            #region TransporterOKPO (18)
            var TransporterOKPOR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(TransporterOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            TransporterOKPOR.SetSizeColToAllLevels(163);
            TransporterOKPOR.Binding = nameof(TransporterOKPO);
            NumberInOrderR += TransporterOKPOR;
            #endregion

            #region StoragePlaceName (19)
            var StoragePlaceNameR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(StoragePlaceName)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            StoragePlaceNameR.SetSizeColToAllLevels(103);
            StoragePlaceNameR.Binding = nameof(StoragePlaceName);
            NumberInOrderR += StoragePlaceNameR;
            #endregion
            #region StoragePlaceCode (20)
            var StoragePlaceCodeR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(StoragePlaceCode)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            StoragePlaceCodeR.SetSizeColToAllLevels(88);
            StoragePlaceCodeR.Binding = nameof(StoragePlaceCode);
            NumberInOrderR += StoragePlaceCodeR;
            #endregion

            #region CodeRAO (21)
            var CodeRAOR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(CodeRAO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            CodeRAOR.SetSizeColToAllLevels(88);
            CodeRAOR.Binding = nameof(CodeRAO);
            NumberInOrderR += CodeRAOR;
            #endregion
            #region StatusRAO (22)
            var StatusRAOR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(StatusRAO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            StatusRAOR.SetSizeColToAllLevels(88);
            StatusRAOR.Binding = nameof(StatusRAO);
            NumberInOrderR += StatusRAOR;
            #endregion
            #region VolumeOutOfPack (23)
            var VolumeOutOfPackR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(VolumeOutOfPack)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            VolumeOutOfPackR.SetSizeColToAllLevels(163);
            VolumeOutOfPackR.Binding = nameof(VolumeOutOfPack);
            NumberInOrderR += VolumeOutOfPackR;
            #endregion
            #region MassOutOfPack (24)
            var MassOutOfPackR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(MassOutOfPack)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            MassOutOfPackR.SetSizeColToAllLevels(170);
            MassOutOfPackR.Binding = nameof(MassOutOfPack);
            NumberInOrderR += MassOutOfPackR;
            #endregion
            #region Quantity (25)
            var QuantityR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(Quantity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            QuantityR.SetSizeColToAllLevels(140);
            QuantityR.Binding = nameof(Quantity);
            NumberInOrderR += QuantityR;
            #endregion

            #region TritiumActivity (26)
            var TritiumActivityR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(TritiumActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            TritiumActivityR.SetSizeColToAllLevels(163);
            TritiumActivityR.Binding = nameof(TritiumActivity);
            NumberInOrderR += TritiumActivityR;
            #endregion
            #region BetaGammaActivity (27)
            var BetaGammaActivityR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(BetaGammaActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            BetaGammaActivityR.SetSizeColToAllLevels(180);
            BetaGammaActivityR.Binding = nameof(BetaGammaActivity);
            NumberInOrderR += BetaGammaActivityR;
            #endregion
            #region AlphaActivity (28)
            var AlphaActivityR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(AlphaActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            AlphaActivityR.SetSizeColToAllLevels(185);
            AlphaActivityR.Binding = nameof(AlphaActivity);
            NumberInOrderR += AlphaActivityR;
            #endregion
            #region TransuraniumActivity (29)
            var TransuraniumActivityR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(TransuraniumActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            TransuraniumActivityR.SetSizeColToAllLevels(200);
            TransuraniumActivityR.Binding = nameof(TransuraniumActivity);
            NumberInOrderR += TransuraniumActivityR;
            #endregion
            #region RefineOrSortRAOCode (30)
            var RefineOrSortRAOCodeR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(RefineOrSortRAOCode)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            RefineOrSortRAOCodeR.SetSizeColToAllLevels(120);
            RefineOrSortRAOCodeR.Binding = nameof(RefineOrSortRAOCode);
            NumberInOrderR += RefineOrSortRAOCodeR;
            #endregion

            #region Subsidy (31)
            var SubsidyR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(Subsidy)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            SubsidyR.SetSizeColToAllLevels(88);
            SubsidyR.Binding = nameof(Subsidy);
            NumberInOrderR += SubsidyR;
            #endregion
            #region FcpNumber (32)
            var FcpNumberR = ((FormPropertyAttribute)typeof(Form17).GetProperty(nameof(FcpNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            FcpNumberR.SetSizeColToAllLevels(163);
            FcpNumberR.Binding = nameof(FcpNumber);
            NumberInOrderR += FcpNumberR;
            #endregion
            _DataGridColumns = NumberInOrderR;
        }
        return _DataGridColumns;
    }
    #endregion
}