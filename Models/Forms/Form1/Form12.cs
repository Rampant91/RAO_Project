using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;
using Spravochniki;

namespace Models.Forms.Form1;

[Serializable]
[Form_Class("Форма 1.2: Сведения об изделиях из обедненного урана")]
[Table (name: "form_12")]
public partial class Form12 : Form1
{
    #region Constructor
    
    public Form12()
    {
        FormNum.Value = "1.2";
        Validate_all();
    }

    #endregion

    #region Validation
    
    private void Validate_all()
    {
        CreationDate_Validation(CreationDate);
        CreatorOKPO_Validation(CreatorOKPO);
        Owner_Validation(Owner);
        PackName_Validation(PackName);
        PackNumber_Validation(PackNumber);
        PackType_Validation(PackType);
        PassportNumber_Validation(PassportNumber);
        PropertyCode_Validation(PropertyCode);
        ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
        SignedServicePeriod_Validation(SignedServicePeriod);
        TransporterOKPO_Validation(TransporterOKPO);
        FactoryNumber_Validation(FactoryNumber);
        Mass_Validation(Mass);
        NameIOU_Validation(NameIOU);
    }

    [FormProperty(true, "Форма")]
    public override bool Object_Validation()
    {
        return !(CreationDate.HasErrors ||
                 CreatorOKPO.HasErrors ||
                 Owner.HasErrors ||
                 PackName.HasErrors ||
                 PackNumber.HasErrors ||
                 PackType.HasErrors ||
                 PassportNumber.HasErrors ||
                 PropertyCode.HasErrors ||
                 ProviderOrRecieverOKPO.HasErrors ||
                 SignedServicePeriod.HasErrors ||
                 TransporterOKPO.HasErrors ||
                 FactoryNumber.HasErrors ||
                 Mass.HasErrors ||
                 NameIOU.HasErrors);
    }

    protected override bool DocumentNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (value.Value == "прим.")
        {
            //if ((DocumentNumberNote == null) || DocumentNumberNote.Equals(""))
            //    value.AddError( "Заполните примечание");//to do note handling
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
        if (value.Value is "01" or "13" or "14" or "16" or "26" or "36" or "44" or "45" or "49" or "51" or "52" or "55" or "56" or "57" or "59" or "76")
        {
            value.AddError("Код операции не может быть использован для РВ");
            return false;
        }
        return true;
    }

    #endregion

    #region Properties

    #region PassportNumber (4)

    public string PassportNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на изделие из обедненного урана", "номер паспорта", "4")]
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
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        PassportNumber_DB = tmp.Trim();
    }

    private bool PassportNumber_Validation(RamAccess<string> value)
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
            //    value.AddError( "Заполните примечание");//to do note handling
            return true;
        }
        return true;
    }

    #endregion

    #region NameIOU (5)

    public string NameIOU_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на изделие из обедненного урана", "наименование", "5")]
    public RamAccess<string> NameIOU
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(NameIOU), out var value))
            {
                ((RamAccess<string>)value).Value = NameIOU_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(NameIOU_Validation, NameIOU_DB);
            rm.PropertyChanged += NameIOU_ValueChanged;
            Dictionary.Add(nameof(NameIOU), rm);
            return (RamAccess<string>)Dictionary[nameof(NameIOU)];
        }
        set
        {
            NameIOU_DB = value.Value; 
            OnPropertyChanged();
        }
    }

    private void NameIOU_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        NameIOU_DB = tmp.Trim();
    }

    private bool NameIOU_Validation(RamAccess<string> value)//TODO
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

    #region FactoryNumber (6)

    public string FactoryNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на изделие из обедненного урана", "номер", "6")]
    public RamAccess<string> FactoryNumber
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(FactoryNumber), out var value))
            {
                ((RamAccess<string>)value).Value = FactoryNumber_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(FactoryNumber_Validation, FactoryNumber_DB);
            rm.PropertyChanged += FactoryNumber_ValueChanged;
            Dictionary.Add(nameof(FactoryNumber), rm);
            return (RamAccess<string>)Dictionary[nameof(FactoryNumber)];
        }
        set
        {
            FactoryNumber_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void FactoryNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        FactoryNumber_DB = tmp.Trim();
    }

    private bool FactoryNumber_Validation(RamAccess<string> value)
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

    #region Mass (7)

    public string Mass_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на изделие из обедненного урана", "масса обедненного урана, кг", "7")]
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
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        tmp = tmp.Trim()
            .TrimStart('(')
            .TrimEnd(')')
            .ToLower()
            .Replace('.', ',')
            .Replace('е', 'e');
        if (tmp.Equals("-"))
        {
            Mass_DB = tmp;
            return;
        }
        if (double.TryParse(tmp, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
            {
                tmp = $"{doubleValue:0.######################################################e+00}";
            }
        Mass_DB = tmp;
    }

    private bool Mass_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            return true;
        }
        var tmp = value.Value
            .Trim()
            .TrimStart('(')
            .TrimEnd(')')
            .ToLower()
            .Replace('.', ',')
            .Replace('е', 'e');
        if (!double.TryParse(tmp, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
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

    #region CreatorOKPO (8)

    public string CreatorOKPO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на изделие из обедненного урана", "код ОКПО изготовителя", "8")]
    public RamAccess<string> CreatorOKPO
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(CreatorOKPO), out var value))
            {
                ((RamAccess<string>)value).Value = CreatorOKPO_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(CreatorOKPO_Validation, CreatorOKPO_DB);
            rm.PropertyChanged += CreatorOKPO_ValueChanged;
            Dictionary.Add(nameof(CreatorOKPO), rm);
            return (RamAccess<string>)Dictionary[nameof(CreatorOKPO)];
        }
        set
        {
            CreatorOKPO_DB = value.Value; 
            OnPropertyChanged();
        }
    }
    //If change this change validation

    private void CreatorOKPO_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        tmp = tmp.Trim();
        if (Spravochniks.OKSM.Contains(tmp.ToUpper()))
        {
            tmp = tmp.ToUpper();
        }
        CreatorOKPO_DB = tmp;
    }

    private bool CreatorOKPO_Validation(RamAccess<string> value)//TODO
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
            //if ((CreatorOKPONote.Value == null) || (CreatorOKPONote.Value == ""))
            //    value.AddError( "Заполните примечание");
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

    #region CreationDate (9)

    public string CreationDate_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на изделие из обедненного урана", "дата выпуска", "9")]
    public RamAccess<string> CreationDate
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(CreationDate), out var value))
            {
                ((RamAccess<string>)value).Value = CreationDate_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(CreationDate_Validation, CreationDate_DB);
            rm.PropertyChanged += CreationDate_ValueChanged;
            Dictionary.Add(nameof(CreationDate), rm);
            return (RamAccess<string>)Dictionary[nameof(CreationDate)];
        }
        set
        {
            CreationDate_DB = value.Value; 
            OnPropertyChanged();
        }
    }
    //If change this change validation

    private void CreationDate_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        tmp = tmp.Trim();
        CreationDate_DB = DateOnly.TryParse(tmp, CultureInfo.CreateSpecificCulture("ru-RU"), out var date)
            ? date.ToShortDateString()
            : tmp;
    }

    private bool CreationDate_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            //    if ((CreationDateNote.Value == null) || (CreationDateNote.Value == ""))
            //        value.AddError( "Заполните примечание");
            return true;
        }
        var tmp = value.Value.Trim();
        if (!Date8NumRegex().IsMatch(tmp) || !DateOnly.TryParse(tmp, CultureInfo.CreateSpecificCulture("ru-RU"), out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region SignedServicePeriod (10)

    public string SignedServicePeriod_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на изделие из обедненного урана", "НСС, мес", "10")]
    public RamAccess<string> SignedServicePeriod
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(SignedServicePeriod), out var value))
            {
                ((RamAccess<string>)value).Value = SignedServicePeriod_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(SignedServicePeriod_Validation, SignedServicePeriod_DB);
            rm.PropertyChanged += SignedServicePeriod_ValueChanged;
            Dictionary.Add(nameof(SignedServicePeriod), rm);
            return (RamAccess<string>)Dictionary[nameof(SignedServicePeriod)];
        }
        set
        {
            SignedServicePeriod_DB = value.Value; 
            OnPropertyChanged();
        }
    }

    private void SignedServicePeriod_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        SignedServicePeriod_DB = tmp.Trim();
    }

    private bool SignedServicePeriod_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var tmp = value.Value ?? string.Empty;
        tmp = tmp.Trim();
        if (!double.TryParse(tmp,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var floatValue) 
            || floatValue <= 0)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region PropertyCode (11)

    public byte? PropertyCode_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Право собственности на ИОУ", "код формы собственности", "11")]
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
        }
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

    private bool PropertyCode_Validation(RamAccess<byte?> value)//Ready
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        //if (value.Value == 255)//ok
        //{
        //    value.AddError( "Поле не заполнено");
        //}
        if (value.Value is not (>= 1 and <= 9))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        return true;
    }

    #endregion

    #region Owner (12)

    public string Owner_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Право собственности на ИОУ", "код ОКПО правообладателя", "12")]
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
    }
    //if change this change validation
    private void Owner_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        tmp = tmp.Trim();
        if (Spravochniks.OKSM.Contains(tmp.ToUpper()))
        {
            tmp = tmp.ToUpper();
        }
        Owner_DB = tmp;
    }

    private bool Owner_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        var tmp = value.Value ?? string.Empty;
        tmp = tmp.Trim();
        if (string.IsNullOrEmpty(tmp))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (Spravochniks.OKSM.Contains(tmp.ToUpper()))
        {
            return true;
        }
        if (tmp.Equals("прим."))
        {
            //if ((OwnerNote.Value == null) || (OwnerNote.Value == ""))
            //    value.AddError( "Заполните примечание");
            return true;
        }
        if (tmp.Length != 8 && tmp.Length != 14
            || !OkpoRegex().IsMatch(tmp))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        return true;
    }

    #endregion

    #region ProviderOrRecieverOKPO (16)

    public string ProviderOrRecieverOKPO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Код ОКПО", "поставщика или получателя", "16")]
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
        set
        {
            ProviderOrRecieverOKPO_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void ProviderOrRecieverOKPO_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        tmp = tmp.Trim();
        if (Spravochniks.OKSM.Contains(tmp.ToUpper()))
        {
            tmp = tmp.ToUpper();
        }
        ProviderOrRecieverOKPO_DB = tmp;
    }

    private bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        var tmp = value.Value ?? string.Empty;
        tmp = tmp.Trim();
        if (string.IsNullOrEmpty(tmp))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (tmp.Equals("Минобороны") || tmp.Equals("прим.") || Spravochniks.OKSM.Contains(tmp.ToUpper()))
        {
            return true;
        }
        if (tmp.Length != 8 && value.Value.Length != 14
            || !OkpoRegex().IsMatch(tmp))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        return true;
    }

    #endregion

    #region TransporterOKPO (17)

    public string TransporterOKPO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Код ОКПО", "перевозчика", "17")]
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
        tmp = tmp.Trim();
        if (Spravochniks.OKSM.Contains(tmp.ToUpper()))
        {
            tmp = tmp.ToUpper();
        }
        TransporterOKPO_DB = tmp;
    }

    private bool TransporterOKPO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        var tmp = value.Value ?? string.Empty;
        tmp = tmp.Trim();
        if (string.IsNullOrEmpty(tmp))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (tmp.Equals("-") || tmp.Equals("Минобороны") || tmp.Equals("прим.") || Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
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

    #region PackName (18)

    public string PackName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "наименование", "18")]
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
        set
        {
            PackName_DB = value.Value; 
            OnPropertyChanged();
        }
    }

    private void PackName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        PackName_DB = tmp.Trim();
    }

    private bool PackName_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            return true;
        }
        return true;
    }

    #endregion

    #region PackType (19)

    public string PackType_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "тип", "19")]
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
        set
        {
            PackType_DB = value.Value; 
            OnPropertyChanged();
        }
    }
    //If change this change validation

    private void PackType_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        PackType_DB = tmp.Trim();
    }

    private bool PackType_Validation(RamAccess<string> value)//Ready
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
            //    value.AddError( "Заполните примечание");//to do note handling
            return true;
        }
        return true;
    }

    #endregion

    #region PackNumber (20)

    public string PackNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "номер", "20")]
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
        set
        {
            PackNumber_DB = value.Value;
            OnPropertyChanged();
        }
    }
    //If change this change validation

    private void PackNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        PackNumber_DB = tmp.Trim();
    }

    private bool PackNumber_Validation(RamAccess<string> value)//Ready
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
            //    value.AddError( "Заполните примечание");//to do note handling
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
        NameIOU_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        FactoryNumber_DB = Convert.ToString(worksheet.Cells[row, 6].Value);
        Mass_DB = ConvertFromExcelDouble(worksheet.Cells[row, 7].Value);
        CreatorOKPO_DB = Convert.ToString(worksheet.Cells[row, 8].Value);
        CreationDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 9].Text);
        SignedServicePeriod_DB = ConvertFromExcelDouble(worksheet.Cells[row, 10].Value);
        PropertyCode_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 11].Value), out var byteValue) ? byteValue : null;
        Owner_DB = Convert.ToString(worksheet.Cells[row, 12].Value);
        DocumentVid_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 13].Value), out byteValue) ? byteValue : null;
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[row, 14].Value);
        DocumentDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 15].Text);
        ProviderOrRecieverOKPO_DB = Convert.ToString(worksheet.Cells[row, 16].Value);
        TransporterOKPO_DB = Convert.ToString(worksheet.Cells[row, 17].Value);
        PackName_DB = Convert.ToString(worksheet.Cells[row, 18].Value);
        PackType_DB = Convert.ToString(worksheet.Cells[row, 19].Value);
        PackNumber_DB = Convert.ToString(worksheet.Cells[row, 20].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ConvertToExcelString(PassportNumber_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(NameIOU_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelString(FactoryNumber_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelDouble(Mass_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelString(CreatorOKPO_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelDate(CreationDate_DB, worksheet, row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0));
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDouble(SignedServicePeriod_DB);
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = PropertyCode_DB is null ? "-" : PropertyCode_DB;
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ConvertToExcelString(Owner_DB);
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = DocumentVid_DB is null ? "-" : DocumentVid_DB;
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ConvertToExcelString(DocumentNumber_DB);
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ConvertToExcelDate(DocumentDate_DB, worksheet, row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0));
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ConvertToExcelString(ProviderOrRecieverOKPO_DB);
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ConvertToExcelString(TransporterOKPO_DB);
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ConvertToExcelString(PackName_DB);
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ConvertToExcelString(PackType_DB);
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ConvertToExcelString(PackNumber_DB);

        return 17;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form1.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(PassportNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(NameIOU))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(FactoryNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(Mass))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(CreatorOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(CreationDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(SignedServicePeriod))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(PropertyCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(Owner))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(DocumentVid))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(DocumentNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(DocumentDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(ProviderOrRecieverOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(TransporterOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(PackName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(PackType))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form12,Models")?.GetProperty(nameof(PackNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        
        return 17;
    }

    #endregion

    #region IDataGridColumn

    public override DataGridColumns GetColumnStructure(string param)
    {
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
            operationCodeR.SetSizeColToAllLevels(80);
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
            operationDateR.SetSizeColToAllLevels(80);
            operationDateR.Binding = nameof(OperationDate);
            numberInOrderR += operationDateR;
        }

        #endregion

        #region PassportNumber (4)

        var passportNumberR = ((FormPropertyAttribute)typeof(Form12)
                .GetProperty(nameof(PassportNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (passportNumberR != null)
        {
            passportNumberR.SetSizeColToAllLevels(125);
            passportNumberR.Binding = nameof(PassportNumber);
            numberInOrderR += passportNumberR;
        }

        #endregion

        #region NameIOU (5)

        var nameIouR = ((FormPropertyAttribute)typeof(Form12)
                .GetProperty(nameof(NameIOU))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (nameIouR != null)
        {
            nameIouR.SetSizeColToAllLevels(110);
            nameIouR.Binding = nameof(NameIOU);
            numberInOrderR += nameIouR;
        }

        #endregion

        #region FactoryNumber (6)

        var factoryNumberR = ((FormPropertyAttribute)typeof(Form12)
                .GetProperty(nameof(FactoryNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (factoryNumberR != null)
        {
            factoryNumberR.SetSizeColToAllLevels(80);
            factoryNumberR.Binding = nameof(FactoryNumber);
            numberInOrderR += factoryNumberR;
        }

        #endregion

        #region Mass (7)

        var massR = ((FormPropertyAttribute)typeof(Form12)
                .GetProperty(nameof(Mass))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (massR != null)
        {
            massR.SetSizeColToAllLevels(125);
            massR.Binding = nameof(Mass);
            numberInOrderR += massR;
        }

        #endregion

        #region CreatorOKPO (8)

        var creatorOkpoR = ((FormPropertyAttribute)typeof(Form12)
                .GetProperty(nameof(CreatorOKPO))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        creatorOkpoR.SetSizeColToAllLevels(90);
        creatorOkpoR.Binding = nameof(CreatorOKPO);
        numberInOrderR += creatorOkpoR;

        #endregion

        #region CreationDate (9)

        var creationDateR = ((FormPropertyAttribute)typeof(Form12)
                .GetProperty(nameof(CreationDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        creationDateR.SetSizeColToAllLevels(90);
        creationDateR.Binding = nameof(CreationDate);
        numberInOrderR += creationDateR;

        #endregion

        #region SignedServicePeriod (10)

        var signedServicePeriodR = ((FormPropertyAttribute)typeof(Form12)
                .GetProperty(nameof(SignedServicePeriod))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        signedServicePeriodR.SetSizeColToAllLevels(80);
        signedServicePeriodR.Binding = nameof(SignedServicePeriod);
        numberInOrderR += signedServicePeriodR;

        #endregion

        #region PropertyCode (11)

        var propertyCodeR = ((FormPropertyAttribute)typeof(Form12)
                .GetProperty(nameof(PropertyCode))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        propertyCodeR.SetSizeColToAllLevels(100);
        propertyCodeR.Binding = nameof(PropertyCode);
        numberInOrderR += propertyCodeR;

        #endregion

        #region Owner (12)

        var ownerR = ((FormPropertyAttribute)typeof(Form12)
                .GetProperty(nameof(Owner))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        ownerR.SetSizeColToAllLevels(110);
        ownerR.Binding = nameof(Owner);
        numberInOrderR += ownerR;

        #endregion

        #region DocumentVid (13)

        var documentVidR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentVid))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentVidR.SetSizeColToAllLevels(60);
        documentVidR.Binding = nameof(DocumentVid);
        numberInOrderR += documentVidR;

        #endregion

        #region DocumentNumber (14)

        var documentNumberR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentNumberR.SetSizeColToAllLevels(80);
        documentNumberR.Binding = nameof(DocumentNumber);
        numberInOrderR += documentNumberR;

        #endregion

        #region DocumentDate (15)

        var documentDateR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentDateR.SetSizeColToAllLevels(80);
        documentDateR.Binding = nameof(DocumentDate);
        numberInOrderR += documentDateR;

        #endregion

        #region ProviderOrReciverOKPO (16)

        var providerOrRecieverOkpoR = ((FormPropertyAttribute)typeof(Form12)
                .GetProperty(nameof(ProviderOrRecieverOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        providerOrRecieverOkpoR.SetSizeColToAllLevels(100);
        providerOrRecieverOkpoR.Binding = nameof(ProviderOrRecieverOKPO);
        numberInOrderR += providerOrRecieverOkpoR;

        #endregion

        #region TransporterOKPO (17)

        var transporterOkpoR = ((FormPropertyAttribute)typeof(Form12)
                .GetProperty(nameof(TransporterOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        transporterOkpoR.SetSizeColToAllLevels(130);
        transporterOkpoR.Binding = nameof(TransporterOKPO);
        numberInOrderR += transporterOkpoR;

        #endregion

        #region PackName (18)

        var packNameR = ((FormPropertyAttribute)typeof(Form12)
                .GetProperty(nameof(PackName))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packNameR.SetSizeColToAllLevels(130);
        packNameR.Binding = nameof(PackName);
        numberInOrderR += packNameR;

        #endregion

        #region PackType (19)

        var packTypeR = ((FormPropertyAttribute)typeof(Form12)
                .GetProperty(nameof(PackType))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packTypeR.SetSizeColToAllLevels(80);
        packTypeR.Binding = nameof(PackType);
        numberInOrderR += packTypeR;

        #endregion

        #region PackNumber (20)

        var packNumberR = ((FormPropertyAttribute)typeof(Form12)
                .GetProperty(nameof(PackNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packNumberR.SetSizeColToAllLevels(80);
        packNumberR.Binding = nameof(PackNumber);
        numberInOrderR += packNumberR;

        #endregion

        return numberInOrderR;
    }

    #endregion
}