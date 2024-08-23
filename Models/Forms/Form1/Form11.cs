using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;
using Spravochniki;

namespace Models.Forms.Form1;

[Serializable]
[Form_Class("Форма 1.1: Сведения о ЗРИ")]
[Table (name: "form_11")]
public class Form11 : Form1
{
    #region Constructor
    
    public Form11()
    {
        FormNum.Value = "1.1";
        Validate_all();
    }

    #endregion

    #region Validation
    
    private void Validate_all()
    {
        Activity_Validation(Activity);
        Category_Validation(Category);
        CreationDate_Validation(CreationDate);
        CreatorOKPO_Validation(CreatorOKPO);
        FactoryNumber_Validation(FactoryNumber);
        Owner_Validation(Owner);
        PackName_Validation(PackName);
        PackNumber_Validation(PackNumber);
        PackType_Validation(PackType);
        PassportNumber_Validation(PassportNumber);
        PropertyCode_Validation(PropertyCode);
        ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
        Quantity_Validation(Quantity);
        Radionuclids_Validation(Radionuclids);
        SignedServicePeriod_Validation(SignedServicePeriod);
        TransporterOKPO_Validation(TransporterOKPO);
        Type_Validation(Type);
    }

    public override bool Object_Validation()
    {
        return !(Activity.HasErrors ||
                 Category.HasErrors ||
                 CreationDate.HasErrors ||
                 CreatorOKPO.HasErrors ||
                 FactoryNumber.HasErrors ||
                 Owner.HasErrors ||
                 PackName.HasErrors ||
                 PackNumber.HasErrors ||
                 PackType.HasErrors ||
                 PassportNumber.HasErrors ||
                 PropertyCode.HasErrors ||
                 ProviderOrRecieverOKPO.HasErrors ||
                 Quantity.HasErrors ||
                 Radionuclids.HasErrors ||
                 SignedServicePeriod.HasErrors ||
                 TransporterOKPO.HasErrors ||
                 Type.HasErrors);
    }

    protected override bool DocumentNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (value.Value == "прим.")
        {
            //if ((DocumentNumberNote.Value == null) || DocumentNumberNote.Value.Equals(""))
            //    value.AddError("Заполните примечание");
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
            return false;
        }
        return true;
    }

    #endregion

    #region Properties

    public bool AutoRn;

    #region PassportNumber (4)

    public string PassportNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "номер паспорта (сертификата)", "4")]
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
            OnPropertyChanged(nameof(PassportNumber));
        }
    }

    private void PassportNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        PassportNumber_DB = tmp.Trim();
    }

    protected static bool PassportNumber_Validation(RamAccess<string> value)//Ready
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

    #region Type (5)

    public string Type_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "тип", "5")]
    public RamAccess<string> Type
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Type), out var value))
            {
                ((RamAccess<string>)value).Value = Type_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Type_Validation, Type_DB);
            rm.PropertyChanged += Type_ValueChanged;
            Dictionary.Add(nameof(Type), rm);
            return (RamAccess<string>)Dictionary[nameof(Type)];
        }
        set
        {
            Type_DB = value.Value;
            OnPropertyChanged(nameof(Type));
        }
    }

    private void Type_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        Type_DB = tmp.Trim();
    }

    protected bool Type_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var a = Spravochniks.SprTypesToRadionuclids
            .Where(item => item.Item1 == value.Value)
            .Select(item => item.Item2)
            .ToList();
        if (string.IsNullOrEmpty(Radionuclids.Value) && a.Count == 1)
        {
            AutoRn = true;
            Radionuclids.Value = a.First();
        }
        return true;
    }

    #endregion

    #region Radionuclids (6)

    public string Radionuclids_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "радионуклиды", "6")]
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
    }

    private void Radionuclids_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        Radionuclids_DB = tmp.Trim();
    }

    private bool Radionuclids_Validation(RamAccess<string> value) => NuclidString_Validation(value);

    #endregion

    #region FactoryNumber (7)

    public string FactoryNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "номер", "7")]
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
            OnPropertyChanged(nameof(FactoryNumber));
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

    #region Quantity (8)

    public int? Quantity_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "количество, шт", "8")]
    public RamAccess<int?> Quantity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Quantity), out var value))
            {
                ((RamAccess<int?>)value).Value = Quantity_DB;
                return (RamAccess<int?>)value;
            }
            var rm = new RamAccess<int?>(Quantity_Validation, Quantity_DB);
            rm.PropertyChanged += Quantity_ValueChanged;
            Dictionary.Add(nameof(Quantity), rm);
            return (RamAccess<int?>)Dictionary[nameof(Quantity)];
        }
        set
        {
            Quantity_DB = value.Value;
            OnPropertyChanged(nameof(Quantity));
        }
    }

    private void Quantity_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        Quantity_DB = ((RamAccess<int?>)value).Value;
    }

    private bool Quantity_Validation(RamAccess<int?> value)//Ready
    {
        value.ClearErrors();
        switch (value.Value)
        {
            case null:
                value.AddError("Поле не заполнено");
                return false;
            case <= 0:
                value.AddError("Недопустимое значение");
                return false;
            default:
                return true;
        }
    }

    #endregion

    #region Activity (9)

    public string Activity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "суммарная активность, Бк", "9")]
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

    private bool Activity_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region CreatorOKPO (10)

    public string CreatorOKPO_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "код ОКПО изготовителя", "10")]
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
        }//OK
        set
        {
            CreatorOKPO_DB = value.Value;
            OnPropertyChanged(nameof(CreatorOKPO));
        }
    }

    private void CreatorOKPO_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
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
        if (value.Value.Equals("прим."))
        {
            //if ((PassportNumberNote.Value == null) || (PassportNumberNote.Value == ""))
            //    value.AddError("Заполните примечание");
            return false;
        }
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
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

    #region CreationDate (11)

    public string CreationDate_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "дата выпуска", "11")]
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

    private void CreationDate_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        CreationDate_DB = DateString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool CreationDate_Validation(RamAccess<string> value) => DateString_Validation(value);

    #endregion

    #region Category (12)

    public short? Category_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "категория", "12")]
    public RamAccess<short?> Category
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Category), out var value))
            {
                ((RamAccess<short?>)value).Value = Category_DB;
                return (RamAccess<short?>)value;
            }
            var rm = new RamAccess<short?>(Category_Validation, Category_DB);
            rm.PropertyChanged += Category_ValueChanged;
            Dictionary.Add(nameof(Category), rm);
            return (RamAccess<short?>)Dictionary[nameof(Category)];
        }//OK
        set
        {
            Category_DB = value.Value;
            OnPropertyChanged(nameof(Category));
        }
    }

    private void Category_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        Category_DB = ((RamAccess<short?>)value).Value;
    }

    private bool Category_Validation(RamAccess<short?> value)//TODO
    {
        value.ClearErrors();
        switch (value.Value)
        {
            case null:
                value.AddError("Поле не заполнено");
                return false;
            case < 1 or > 5:
                value.AddError("Недопустимое значение");
                return false;
            default:
                return true;
        }
    }

    #endregion

    #region SignedServicePeriod (13)

    public float? SignedServicePeriod_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "НСС, мес", "13")]
    public RamAccess<float?> SignedServicePeriod
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(SignedServicePeriod), out var value))
            {
                ((RamAccess<float?>)value).Value = SignedServicePeriod_DB;
                return (RamAccess<float?>)value;
            }
            var rm = new RamAccess<float?>(SignedServicePeriod_Validation, SignedServicePeriod_DB);
            rm.PropertyChanged += SignedServicePeriod_ValueChanged;
            Dictionary.Add(nameof(SignedServicePeriod), rm);
            return (RamAccess<float?>)Dictionary[nameof(SignedServicePeriod)];
        }//OK
        set
        {
            SignedServicePeriod_DB = value.Value;
            OnPropertyChanged(nameof(SignedServicePeriod));
        }
    }

    private void SignedServicePeriod_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        SignedServicePeriod_DB = ((RamAccess<float?>)value).Value;
    }

    private bool SignedServicePeriod_Validation(RamAccess<float?> value)//Ready
    {
        value.ClearErrors();
        switch (value.Value)
        {
            case null:
                value.AddError("Поле не заполнено");
                return false;
            case <= 0:
                value.AddError("Недопустимое значение");
                return false;
            default:
                return true;
        }
    }

    #endregion

    #region PropertyCode (14)

    public byte? PropertyCode_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Право собственности на ЗРИ", "код формы собственности", "14")]
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
            OnPropertyChanged(nameof(PropertyCode));
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
        if (value.Value is not (>= 1 and <= 6 or 9))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region Owner (15)

    public string Owner_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Право собственности на ЗРИ", "код ОКПО правообладателя", "15")]
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
        }//OK
        set
        {
            Owner_DB = value.Value;
            OnPropertyChanged(nameof(Owner));
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
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        switch (value.Value)
        {
            case "прим.":
            case "Минобороны":
                //if ((OwnerNote == null) || OwnerNote.Equals(""))
                //    value.AddError("Заполните примечание");
                return true;
        }
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
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

    #region ProviderOrRecieverOKPO (19)

    public string ProviderOrRecieverOKPO_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Код ОКПО", "поставщика или получателя", "19")]
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
        }//OK
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
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        switch (value.Value)
        {
            case "Минобороны":
            //if ((ProviderOrRecieverOKPONote == null) || ProviderOrRecieverOKPONote.Equals(""))
            //    value.AddError("Заполните примечание");
            case "прим.":
                return true;
        }
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
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

    #region TransporterOKPO (20)

    public string TransporterOKPO_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Код ОКПО", "перевозчика", "20")]
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
        }//OK
        set
        {
            TransporterOKPO_DB = value.Value;
            OnPropertyChanged(nameof(TransporterOKPO));
        }
    }

    private void TransporterOKPO_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        if (Spravochniks.OKSM.Contains(tmp.ToUpper()))
        {
            tmp = tmp.ToUpper();
        }
        TransporterOKPO_DB = tmp;
    }

    private bool TransporterOKPO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        var tmp = ReplaceDashes(value.Value);
        if (string.IsNullOrEmpty(tmp))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        switch (tmp)
        {
            case "-":
            //if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
            //    value.AddError("Заполните примечание");
            case "прим.":
            case "Минобороны":
                return true;
        }
        if (Spravochniks.OKSM.Contains(tmp.ToUpper()))
        {
            return true;
        }
        if (tmp.Length is not (8 or 14)
            || !OkpoRegex().IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region PackName (21)

    public string PackName_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "наименование", "21")]
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
        }//OK
        set
        {
            PackName_DB = value.Value;
            OnPropertyChanged(nameof(PackName));
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
            //if ((PackNameNote == null) || PackNameNote.Equals(""))
            //    value.AddError("Заполните примечание");//to do note handling
            return true;
        }
        return true;
    }

    #endregion

    #region PackType (22)

    public string PackType_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "тип", "22")]
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
        }//OK
        set
        {
            PackType_DB = value.Value;
            OnPropertyChanged(nameof(PackType));
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
            //    value.AddError("Заполните примечание");//to do note handling
            return true;
        }
        return true;
    }

    #endregion

    #region PackNumber (23)

    public string PackNumber_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "номер", "23")]
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
        }//OK
        set
        {
            PackNumber_DB = value.Value;
            OnPropertyChanged(nameof(PackNumber));
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
            //    value.AddError("Заполните примечание");//to do note handling
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
        Type_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        Radionuclids_DB = Convert.ToString(worksheet.Cells[row, 6].Value);
        FactoryNumber_DB = Convert.ToString(worksheet.Cells[row, 7].Value);
        Quantity_DB = int.TryParse(Convert.ToString(worksheet.Cells[row, 8].Value), out var intValue) ? intValue : null;
        Activity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 9].Value);
        CreatorOKPO_DB = Convert.ToString(worksheet.Cells[row, 10].Value);
        CreationDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 11].Text);
        Category_DB = short.TryParse(Convert.ToString(worksheet.Cells[row, 12].Value), out var shortValue) ? shortValue : null;
        SignedServicePeriod_DB = float.TryParse(Convert.ToString(worksheet.Cells[row, 13].Value), out var floatValue) ? floatValue : null;
        PropertyCode_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 14].Value), out var byteValue) ? byteValue : null;
        Owner_DB = Convert.ToString(worksheet.Cells[row, 15].Value);
        DocumentVid_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 16].Value), out byteValue) ? byteValue : null;
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[row, 17].Value);
        DocumentDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 18].Text);
        ProviderOrRecieverOKPO_DB = Convert.ToString(worksheet.Cells[row, 19].Value);
        TransporterOKPO_DB = Convert.ToString(worksheet.Cells[row, 20].Value);
        PackName_DB = Convert.ToString(worksheet.Cells[row, 21].Value);
        PackType_DB = Convert.ToString(worksheet.Cells[row, 22].Value);
        PackNumber_DB = Convert.ToString(worksheet.Cells[row, 23].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row + 0, column + 0].Value = ConvertToExcelString(PassportNumber_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(Type_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelString(Radionuclids_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelString(FactoryNumber_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = Quantity_DB is null ? "-" : Quantity_DB;
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelDouble(Activity_DB);
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelString(CreatorOKPO_DB);
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelDate(CreationDate_DB, worksheet, row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0));
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = Category_DB is null ? "-" : Category_DB;
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = SignedServicePeriod_DB is null ? "-" : SignedServicePeriod_DB;
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = PropertyCode_DB is null ? "-" : PropertyCode_DB;
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ConvertToExcelString(Owner_DB);
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = DocumentVid_DB is null ? "-" : DocumentVid_DB;
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ConvertToExcelString(DocumentNumber_DB);
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ConvertToExcelDate(DocumentDate_DB, worksheet, row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0));
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ConvertToExcelString(ProviderOrRecieverOKPO_DB);
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ConvertToExcelString(TransporterOKPO_DB);
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ConvertToExcelString(PackName_DB);
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ConvertToExcelString(PackType_DB);
        worksheet.Cells[row + (!transpose ? 19 : 0), column + (transpose ? 19 : 0)].Value = ConvertToExcelString(PackNumber_DB);

        return 24;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form1.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row + 0, column + 0].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(PassportNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(Type))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(Radionuclids))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(FactoryNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(Quantity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(Activity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(CreatorOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(CreationDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(Category))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(SignedServicePeriod))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(PropertyCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(Owner))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(DocumentVid))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(DocumentNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(DocumentDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(ProviderOrRecieverOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(TransporterOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(PackName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(PackType))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 19 : 0), column + (transpose ? 19 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form11,Models")?.GetProperty(nameof(PackNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];

        return 24;
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

        var passportNumberR = ((FormPropertyAttribute)typeof(Form11)
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

        #region Type (5)

        var typeR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(Type))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (typeR != null)
        {
            typeR.SetSizeColToAllLevels(90);
            typeR.Binding = nameof(Type);
            numberInOrderR += typeR;
        }

        #endregion

        #region Radionuclids (6)

        var radionuclidsR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(Radionuclids))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (radionuclidsR != null)
        {
            radionuclidsR.SetSizeColToAllLevels(100);
            radionuclidsR.Binding = nameof(Radionuclids);
            numberInOrderR += radionuclidsR;
        }

        #endregion

        #region FactoryNumber (7)

        var factoryNumberR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(FactoryNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (factoryNumberR != null)
        {
            factoryNumberR.SetSizeColToAllLevels(90);
            factoryNumberR.Binding = nameof(FactoryNumber);
            numberInOrderR += factoryNumberR;
        }

        #endregion

        #region Quantity (8)

        var quantityR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(Quantity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        quantityR.SetSizeColToAllLevels(80);
        quantityR.Binding = nameof(Quantity);
        numberInOrderR += quantityR;

        #endregion

        #region Activity (9)

        var activityR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(Activity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        activityR.SetSizeColToAllLevels(160);
        activityR.Binding = nameof(Activity);
        numberInOrderR += activityR;

        #endregion

        #region CreatorOKPO (10)

        var creatorOkpoR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(CreatorOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        creatorOkpoR.SetSizeColToAllLevels(150);
        creatorOkpoR.Binding = nameof(CreatorOKPO);
        numberInOrderR += creatorOkpoR;

        #endregion

        #region CreationDate (11)

        var creationDateR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(CreationDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        creationDateR.SetSizeColToAllLevels(100);
        creationDateR.Binding = nameof(CreationDate);
        numberInOrderR += creationDateR;

        #endregion

        #region Category (12)

        var categoryR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(Category))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        categoryR.SetSizeColToAllLevels(70);
        categoryR.Binding = nameof(Category);
        numberInOrderR += categoryR;

        #endregion

        #region SignedServicePeriod (13)

        var signedServicePeriodR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(SignedServicePeriod))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        signedServicePeriodR.SetSizeColToAllLevels(70);
        signedServicePeriodR.Binding = nameof(SignedServicePeriod);
        numberInOrderR += signedServicePeriodR;

        #endregion

        #region PropertyCode (14)

        var propertyCodeR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(PropertyCode))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        propertyCodeR.SetSizeColToAllLevels(120);
        propertyCodeR.Binding = nameof(PropertyCode);
        numberInOrderR += propertyCodeR;

        #endregion

        #region Owner (15)

        var ownerR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(Owner))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        ownerR.SetSizeColToAllLevels(160);
        ownerR.Binding = nameof(Owner);
        numberInOrderR += ownerR;

        #endregion

        #region DocumetVid (16)

        var documentVidR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentVid))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentVidR.SetSizeColToAllLevels(60);
        documentVidR.Binding = nameof(DocumentVid);
        numberInOrderR += documentVidR;

        #endregion

        #region DocumentNumber (17)

        var documentNumberR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentNumberR.SetSizeColToAllLevels(100);
        documentNumberR.Binding = nameof(DocumentNumber);
        numberInOrderR += documentNumberR;

        #endregion

        #region DocumentDate (18)

        var documentDateR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentDateR.SetSizeColToAllLevels(80);
        documentDateR.Binding = nameof(DocumentDate);
        numberInOrderR += documentDateR;

        #endregion

        #region ProviderOrRecieverOKPO (19)

        var providerOrRecieverOkpoR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(ProviderOrRecieverOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        providerOrRecieverOkpoR.SetSizeColToAllLevels(150);
        providerOrRecieverOkpoR.Binding = nameof(ProviderOrRecieverOKPO);
        numberInOrderR += providerOrRecieverOkpoR;

        #endregion

        #region TransporterOKPO (20)

        var transporterOkpoR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(TransporterOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        transporterOkpoR.SetSizeColToAllLevels(120);
        transporterOkpoR.Binding = nameof(TransporterOKPO);
        numberInOrderR += transporterOkpoR;

        #endregion

        #region PackName (21)

        var packNameR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(PackName))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packNameR.SetSizeColToAllLevels(130);
        packNameR.Binding = nameof(PackName);
        numberInOrderR += packNameR;

        #endregion

        #region PackType (22)

        var packTypeR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(PackType))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packTypeR.SetSizeColToAllLevels(80);
        packTypeR.Binding = nameof(PackType);
        numberInOrderR += packTypeR;

        #endregion

        #region PackNumber (23)

        var packNumberR = ((FormPropertyAttribute)typeof(Form11)
                .GetProperty(nameof(PackNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packNumberR.SetSizeColToAllLevels(70);
        packNumberR.Binding = nameof(PackNumber);
        numberInOrderR += packNumberR;

        #endregion

        _DataGridColumns = numberInOrderR;
        return _DataGridColumns;
    }

    #endregion
}