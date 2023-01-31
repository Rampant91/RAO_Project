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

[Serializable]
[Form_Class("Форма 1.1: Сведения о ЗРИ")]
public class Form11 : Form1
{
    public Form11()
    {
        FormNum.Value = "1.1";
        Validate_all();
    }
    public bool _autoRN;
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

    #region PassportNumber
    public string PassportNumber_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "номер паспорта (сертификата)", "4")]
    public RamAccess<string> PassportNumber
    {
        get
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
        set
        {
            PassportNumber_DB = value.Value;
            OnPropertyChanged(nameof(PassportNumber));
        }
    }
    private void PassportNumberValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PassportNumber_DB = ((RamAccess<string>)value).Value;
        }
    }
    protected bool PassportNumber_Validation(RamAccess<string> value)//Ready
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

    #region Type
    public string Type_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "тип", "5")]
    public RamAccess<string> Type
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Type)))
            {
                ((RamAccess<string>)Dictionary[nameof(Type)]).Value = Type_DB;
                return (RamAccess<string>)Dictionary[nameof(Type)];
            }
            var rm = new RamAccess<string>(Type_Validation, Type_DB);
            rm.PropertyChanged += TypeValueChanged;
            Dictionary.Add(nameof(Type), rm);
            return (RamAccess<string>)Dictionary[nameof(Type)];
        }
        set
        {
            Type_DB = value.Value;
            OnPropertyChanged(nameof(Type));
        }
    }
    private void TypeValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Type_DB = ((RamAccess<string>)Value).Value;
        }
    }

    protected bool Type_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var a = from item in Spravochniks.SprTypesToRadionuclids where item.Item1 == value.Value select item.Item2;
        if (string.IsNullOrEmpty(Radionuclids.Value) && a.Count() == 1)
        {
            _autoRN = true;
            Radionuclids.Value = a.First();
        }
        return true;
    }
    #endregion

    #region Radionuclids
    public string Radionuclids_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "радионуклиды", "6")]
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
    }
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
        if (_autoRN)
        {
            _autoRN = false;
            return true;
        }
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
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
            var tmp = from item in Spravochniks.SprRadionuclids where nucl == item.Item1 select item.Item1;
            if (tmp.Count() == 0)
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

    #region FactoryNumber
    public string FactoryNumber_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "номер", "7")]
    public RamAccess<string> FactoryNumber
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(FactoryNumber)))
            {
                ((RamAccess<string>)Dictionary[nameof(FactoryNumber)]).Value = FactoryNumber_DB;
                return (RamAccess<string>)Dictionary[nameof(FactoryNumber)];
            }
            var rm = new RamAccess<string>(FactoryNumber_Validation, FactoryNumber_DB);
            rm.PropertyChanged += FactoryNumberValueChanged;
            Dictionary.Add(nameof(FactoryNumber), rm);
            return (RamAccess<string>)Dictionary[nameof(FactoryNumber)];
        }
        set
        {
            FactoryNumber_DB = value.Value;
            OnPropertyChanged(nameof(FactoryNumber));
        }
    }
    private void FactoryNumberValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            FactoryNumber_DB = ((RamAccess<string>)Value).Value;
        }
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

    #region Quantity
    public int? Quantity_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "количество, шт", "8")]
    public RamAccess<int?> Quantity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Quantity)))
            {
                ((RamAccess<int?>)Dictionary[nameof(Quantity)]).Value = Quantity_DB;
                return (RamAccess<int?>)Dictionary[nameof(Quantity)];
            }
            var rm = new RamAccess<int?>(Quantity_Validation, Quantity_DB);
            rm.PropertyChanged += QuantityValueChanged;
            Dictionary.Add(nameof(Quantity), rm);
            return (RamAccess<int?>)Dictionary[nameof(Quantity)];
        }
        set
        {
            Quantity_DB = value.Value;
            OnPropertyChanged(nameof(Quantity));
        }
    }
    private void QuantityValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Quantity_DB = ((RamAccess<int?>)Value).Value;
        }
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

    #region Activity
    public string Activity_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "суммарная активность, Бк", "9")]
    public RamAccess<string> Activity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Activity)))
            {
                ((RamAccess<string>)Dictionary[nameof(Activity)]).Value = Activity_DB;
                return (RamAccess<string>)Dictionary[nameof(Activity)];
            }
            var rm = new RamAccess<string>(Activity_Validation, Activity_DB);
            rm.PropertyChanged += ActivityValueChanged;
            Dictionary.Add(nameof(Activity), rm);
            return (RamAccess<string>)Dictionary[nameof(Activity)];
        }
        set
        {
            Activity_DB = value.Value;
            OnPropertyChanged(nameof(Activity));
        }
    }
    private void ActivityValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)Value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                Activity_DB = value1;
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
            catch(Exception)
            { }
        }
        Activity_DB = value1;
    }

    private bool Activity_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            return false;
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
            tmp = tmp.Remove(len - 1, 1);
            tmp = tmp.Remove(0, 1);
        }
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) 
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

    #region CreationDate
    public string CreationDate_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "дата выпуска", "11")]
    public RamAccess<string> CreationDate
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(CreationDate)))
            {
                ((RamAccess<string>)Dictionary[nameof(CreationDate)]).Value = CreationDate_DB;
                return (RamAccess<string>)Dictionary[nameof(CreationDate)];
            }
            var rm = new RamAccess<string>(CreationDate_Validation, CreationDate_DB);
            rm.PropertyChanged += CreationDateValueChanged;
            Dictionary.Add(nameof(CreationDate), rm);
            return (RamAccess<string>)Dictionary[nameof(CreationDate)];
        }
        set
        {
            CreationDate_DB = value.Value;
            OnPropertyChanged(nameof(CreationDate));
        }
    }
    private void CreationDateValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var date = ((RamAccess<string>)value).Value;
        CreationDate_DB = DateTime.TryParse(date, out var dateTime)
            ? dateTime.ToShortDateString()
            : date;
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
            //if ((CreationDateNote.Value == null) || (CreationDateNote.Value == ""))
            //    value.AddError("Заполните примечание");
            return false;
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

    #region CreatorOKPO
    public string CreatorOKPO_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "код ОКПО изготовителя", "10")]
    public RamAccess<string> CreatorOKPO
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(CreatorOKPO)))
            {
                ((RamAccess<string>)Dictionary[nameof(CreatorOKPO)]).Value = CreatorOKPO_DB;
                return (RamAccess<string>)Dictionary[nameof(CreatorOKPO)];
            }
            var rm = new RamAccess<string>(CreatorOKPO_Validation, CreatorOKPO_DB);
            rm.PropertyChanged += CreatorOKPOValueChanged;
            Dictionary.Add(nameof(CreatorOKPO), rm);
            return (RamAccess<string>)Dictionary[nameof(CreatorOKPO)];
        }//OK
        set
        {
            CreatorOKPO_DB = value.Value;
            OnPropertyChanged(nameof(CreatorOKPO));
        }
    }
    private void CreatorOKPOValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)Value).Value;
        if (value1 != null && Spravochniks.OKSM.Contains(value1.ToUpper()))
        {
            value1 = value1.ToUpper();
        }
        CreatorOKPO_DB = value1;
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
        Regex mask = new("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
        if (value.Value.Length != 8 && value.Value.Length != 14 || !mask.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        return true;
    }
    #endregion
        
    #region Category
    public short? Category_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "категория", "12")]
    public RamAccess<short?> Category
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Category)))
            {
                ((RamAccess<short?>)Dictionary[nameof(Category)]).Value = Category_DB;
                return (RamAccess<short?>)Dictionary[nameof(Category)];
            }
            var rm = new RamAccess<short?>(Category_Validation, Category_DB);
            rm.PropertyChanged += CategoryValueChanged;
            Dictionary.Add(nameof(Category), rm);
            return (RamAccess<short?>)Dictionary[nameof(Category)];
        }//OK
        set
        {
            Category_DB = value.Value;
            OnPropertyChanged(nameof(Category));
        }
    }
    private void CategoryValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Category_DB = ((RamAccess<short?>)value).Value;
        }
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

    #region SignedServicePeriod
    public float? SignedServicePeriod_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "НСС, мес", "13")]
    public RamAccess<float?> SignedServicePeriod
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(SignedServicePeriod)))
            {
                ((RamAccess<float?>)Dictionary[nameof(SignedServicePeriod)]).Value = SignedServicePeriod_DB;
                return (RamAccess<float?>)Dictionary[nameof(SignedServicePeriod)];
            }
            var rm = new RamAccess<float?>(SignedServicePeriod_Validation, SignedServicePeriod_DB);
            rm.PropertyChanged += SignedServicePeriodValueChanged;
            Dictionary.Add(nameof(SignedServicePeriod), rm);
            return (RamAccess<float?>)Dictionary[nameof(SignedServicePeriod)];
        }//OK
        set
        {
            SignedServicePeriod_DB = value.Value;
            OnPropertyChanged(nameof(SignedServicePeriod));
        }
    }

    private void SignedServicePeriodValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            SignedServicePeriod_DB = ((RamAccess<float?>)value).Value;
        }
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

    #region PropertyCode
    public byte? PropertyCode_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Право собственности на ЗРИ", "код формы собственности", "14")]
    public RamAccess<byte?> PropertyCode
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PropertyCode)))
            {
                ((RamAccess<byte?>)Dictionary[nameof(PropertyCode)]).Value = PropertyCode_DB;
                return (RamAccess<byte?>)Dictionary[nameof(PropertyCode)];
            }
            var rm = new RamAccess<byte?>(PropertyCode_Validation, PropertyCode_DB);
            rm.PropertyChanged += PropertyCodeValueChanged;
            Dictionary.Add(nameof(PropertyCode), rm);
            return (RamAccess<byte?>)Dictionary[nameof(PropertyCode)];
        }//OK
        set
        {
            PropertyCode_DB = value.Value;
            OnPropertyChanged(nameof(PropertyCode));
        }
    }
    private void PropertyCodeValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PropertyCode_DB = ((RamAccess<byte?>)Value).Value;
        }
    }
    private bool PropertyCode_Validation(RamAccess<byte?> value)//Ready
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value is not (>= 1 and <= 9))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        return true;
    }
    #endregion

    #region Owner
    public string Owner_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Право собственности на ЗРИ", "код ОКПО правообладателя", "15")]
    public RamAccess<string> Owner
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Owner)))
            {
                ((RamAccess<string>)Dictionary[nameof(Owner)]).Value = Owner_DB;
                return (RamAccess<string>)Dictionary[nameof(Owner)];
            }
            var rm = new RamAccess<string>(Owner_Validation, Owner_DB);
            rm.PropertyChanged += OwnerValueChanged;
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
    private void OwnerValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
            if (value1 != null)
                if (Spravochniks.OKSM.Contains(value1.ToUpper()))
                {
                    value1 = value1.ToUpper();
                }
            Owner_DB = value1;
        }
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
        Regex mask = new("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
        if (value.Value.Length is not (8 or 14) || !mask.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        return true;
    }
    #endregion

    #region ProviderOrRecieverOKPO
    public string ProviderOrRecieverOKPO_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Код ОКПО", "поставщика или получателя", "19")]
    public RamAccess<string> ProviderOrRecieverOKPO
    {
        get
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
        }//OK
        set
        {
            ProviderOrRecieverOKPO_DB = value.Value;
            OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
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
        Regex mask = new("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
        if (value.Value.Length is not(8 or 14) || !mask.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region TransporterOKPO
    public string TransporterOKPO_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Код ОКПО", "перевозчика", "20")]
    public RamAccess<string> TransporterOKPO
    {
        get
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
        }//OK
        set
        {
            TransporterOKPO_DB = value.Value;
            OnPropertyChanged(nameof(TransporterOKPO));
        }
    }

    private void TransporterOKPOValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)Value).Value;
        if (value1 != null && Spravochniks.OKSM.Contains(value1.ToUpper()))
        {
            value1 = value1.ToUpper();
        }
        TransporterOKPO_DB = value1;
    }
    private bool TransporterOKPO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        switch (value.Value)
        {
            case "-":
            //if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
            //    value.AddError("Заполните примечание");
            case "прим.":
            case "Минобороны":
                return true;
        }
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
            return true;
        }
        Regex mask = new("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
        if (value.Value.Length is not (8 or 14) || !mask.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        return true;
    }
    #endregion

    #region PackName
    public string PackName_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Прибор (установка), УКТ или иная упаковка", "наименование", "21")]
    public RamAccess<string> PackName
    {
        get
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
        }//OK
        set
        {
            PackName_DB = value.Value;
            OnPropertyChanged(nameof(PackName));
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

    #region PackType
    public string PackType_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Прибор (установка), УКТ или иная упаковка", "тип", "22")]
    public RamAccess<string> PackType
    {
        get
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
        }//OK
        set
        {
            PackType_DB = value.Value;
            OnPropertyChanged(nameof(PackType));
        }
    }
    //If change this change validation
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

    #region PackNumber
    public string PackNumber_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Прибор (установка), УКТ или иная упаковка", "номер", "23")]
    public RamAccess<string> PackNumber
    {
        get
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
        }//OK
        set
        {
            PackNumber_DB = value.Value;
            OnPropertyChanged(nameof(PackNumber));
        }
    }
    //If change this change validation
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


    #region IExcel
    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        base.ExcelGetRow(worksheet, row);
        PassportNumber_DB = Convert.ToString(worksheet.Cells[row, 4].Value);
        Type_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        Radionuclids_DB = Convert.ToString(worksheet.Cells[row, 6].Value);
        FactoryNumber_DB = Convert.ToString(worksheet.Cells[row, 7].Value);
        Quantity_DB = int.TryParse(Convert.ToString(worksheet.Cells[row, 8].Value), out var intValue)
            ? intValue
            : null;
        Activity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 9].Value);
        CreatorOKPO_DB = Convert.ToString(worksheet.Cells[row, 10].Value);
        CreationDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 11].Value);
        Category_DB = short.TryParse(Convert.ToString(worksheet.Cells[row, 12].Value), out var shortValue)
            ? shortValue
            : null;
        SignedServicePeriod_DB = float.TryParse(Convert.ToString(worksheet.Cells[row, 13].Value), out var floatValue)
            ? floatValue
            : null;
        PropertyCode_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 14].Value), out var byteValue)
            ? byteValue
            : null;
        Owner_DB = Convert.ToString(worksheet.Cells[row, 15].Value);
        DocumentVid_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 16].Value), out byteValue)
            ? byteValue
            : null;
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[row, 17].Value);
        DocumentDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 18].Value);
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
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelDate(CreationDate_DB);
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = Category_DB is null ? "-" : Category_DB;
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = SignedServicePeriod_DB is null ? "-" : SignedServicePeriod_DB;
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = PropertyCode_DB is null ? "-" : PropertyCode_DB;
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ConvertToExcelString(Owner_DB);
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = DocumentVid_DB is null ? "-" : DocumentVid_DB;
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ConvertToExcelString(DocumentNumber_DB);
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ConvertToExcelDate(DocumentDate_DB);
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
        if (_DataGridColumns == null)
        {
            #region NumberInOrder (1)
            var NumberInOrderR = ((FormPropertyAttribute)typeof(Form).GetProperty(nameof(NumberInOrder)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            NumberInOrderR.SetSizeColToAllLevels(50);
            NumberInOrderR.Binding = nameof(NumberInOrder);
            NumberInOrderR.Blocked = true;
            NumberInOrderR.ChooseLine = true;
            #endregion

            #region OperationCode (2)
            var OperationCodeR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(OperationCode)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            OperationCodeR.SetSizeColToAllLevels(80);
            OperationCodeR.Binding = nameof(OperationCode);
            NumberInOrderR += OperationCodeR;
            #endregion

            #region OperationDate (3)
            var OperationDateR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(OperationDate)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            OperationDateR.SetSizeColToAllLevels(80);
            OperationDateR.Binding = nameof(OperationDate);
            NumberInOrderR += OperationDateR;
            #endregion

            #region PassportNumber (4)
            var PassportNumberR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(PassportNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PassportNumberR.SetSizeColToAllLevels(125);
            PassportNumberR.Binding = nameof(PassportNumber);
            NumberInOrderR += PassportNumberR;
            #endregion

            #region Type (5)
            var TypeR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(Type)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            TypeR.SetSizeColToAllLevels(90);
            TypeR.Binding = nameof(Type);
            NumberInOrderR += TypeR;
            #endregion

            #region Radionuclids (6)
            var RadionuclidsR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(Radionuclids)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            RadionuclidsR.SetSizeColToAllLevels(100);
            RadionuclidsR.Binding = nameof(Radionuclids);
            NumberInOrderR += RadionuclidsR;
            #endregion

            #region FactoryNumber (7)
            var FactoryNumberR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(FactoryNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            FactoryNumberR.SetSizeColToAllLevels(90);
            FactoryNumberR.Binding = nameof(FactoryNumber);
            NumberInOrderR += FactoryNumberR;
            #endregion

            #region Quantity (8)
            var QuantityR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(Quantity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            QuantityR.SetSizeColToAllLevels(80);
            QuantityR.Binding = nameof(Quantity);
            NumberInOrderR += QuantityR;
            #endregion

            #region Activity (9)
            var ActivityR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(Activity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            ActivityR.SetSizeColToAllLevels(160);
            ActivityR.Binding = nameof(Activity);
            NumberInOrderR += ActivityR;
            #endregion

            #region CreatorOKPO (10)
            var CreatorOKPOR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(CreatorOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            CreatorOKPOR.SetSizeColToAllLevels(150);
            CreatorOKPOR.Binding = nameof(CreatorOKPO);
            NumberInOrderR += CreatorOKPOR;
            #endregion

            #region CreationDate (11)
            var CreationDateR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(CreationDate)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            CreationDateR.SetSizeColToAllLevels(100);
            CreationDateR.Binding = nameof(CreationDate);
            NumberInOrderR += CreationDateR;
            #endregion

            #region Category (12)
            var CategoryR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(Category)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            CategoryR.SetSizeColToAllLevels(70);
            CategoryR.Binding = nameof(Category);
            NumberInOrderR += CategoryR;
            #endregion

            #region SignedServicePeriod (13)
            var SignedServicePeriodR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(SignedServicePeriod)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            SignedServicePeriodR.SetSizeColToAllLevels(70);
            SignedServicePeriodR.Binding = nameof(SignedServicePeriod);
            NumberInOrderR += SignedServicePeriodR;
            #endregion

            #region PropertyCode (14)
            var PropertyCodeR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(PropertyCode)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PropertyCodeR.SetSizeColToAllLevels(120);
            PropertyCodeR.Binding = nameof(PropertyCode);
            NumberInOrderR += PropertyCodeR;
            #endregion

            #region Owner (15)
            var OwnerR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(Owner)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            OwnerR.SetSizeColToAllLevels(160);
            OwnerR.Binding = nameof(Owner);
            NumberInOrderR += OwnerR;
            #endregion

            #region DocumetVid (16)
            var DocumentVidR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentVid)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentVidR.SetSizeColToAllLevels(60);
            DocumentVidR.Binding = nameof(DocumentVid);
            NumberInOrderR += DocumentVidR;
            #endregion

            #region DocumentNumber (17)
            var DocumentNumberR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentNumberR.SetSizeColToAllLevels(100);
            DocumentNumberR.Binding = nameof(DocumentNumber);
            NumberInOrderR += DocumentNumberR;
            #endregion

            #region DocumentDate (18)
            var DocumentDateR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentDate)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentDateR.SetSizeColToAllLevels(80);
            DocumentDateR.Binding = nameof(DocumentDate);
            NumberInOrderR += DocumentDateR;
            #endregion

            #region ProviderOrRecieverOKPO (19)
            var ProviderOrRecieverOKPOR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(ProviderOrRecieverOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            ProviderOrRecieverOKPOR.SetSizeColToAllLevels(150);
            ProviderOrRecieverOKPOR.Binding = nameof(ProviderOrRecieverOKPO);
            NumberInOrderR += ProviderOrRecieverOKPOR;
            #endregion

            #region TransporterOKPO (20)
            var TransporterOKPOR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(TransporterOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            TransporterOKPOR.SetSizeColToAllLevels(120);
            TransporterOKPOR.Binding = nameof(TransporterOKPO);
            NumberInOrderR += TransporterOKPOR;
            #endregion

            #region PackName (21)
            var PackNameR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(PackName)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PackNameR.SetSizeColToAllLevels(130);
            PackNameR.Binding = nameof(PackName);
            NumberInOrderR += PackNameR;
            #endregion

            #region PackType (22)
            var PackTypeR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(PackType)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PackTypeR.SetSizeColToAllLevels(80);
            PackTypeR.Binding = nameof(PackType);
            NumberInOrderR += PackTypeR;
            #endregion

            #region PackNumber (23)
            var PackNumberR = ((FormPropertyAttribute)typeof(Form11).GetProperty(nameof(PackNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PackNumberR.SetSizeColToAllLevels(70);
            PackNumberR.Binding = nameof(PackNumber);
            NumberInOrderR += PackNumberR;
            #endregion

            _DataGridColumns = NumberInOrderR;
        }
        return _DataGridColumns;
    }
    #endregion
}