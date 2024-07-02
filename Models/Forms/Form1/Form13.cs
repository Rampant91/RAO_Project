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
[Form_Class("Форма 1.3: Сведения об ОРИ в виде отдельных изделий")]
[Table (name: "form_13")]
public partial class Form13 : Form1
{
    #region Constructor
    
    public Form13()
    {
        FormNum.Value = "1.3";
        Validate_all();
    }

    #endregion

    #region Validation
    
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
                 TransporterOKPO.HasErrors ||
                 FactoryNumber.HasErrors ||
                 AggregateState.HasErrors ||
                 Activity.HasErrors ||
                 Radionuclids.HasErrors ||
                 Type.HasErrors);
    }

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
        TransporterOKPO_Validation(TransporterOKPO);
        FactoryNumber_Validation(FactoryNumber);
        AggregateState_Validation(AggregateState);
        Activity_Validation(Activity);
        Radionuclids_Validation(Radionuclids);
        Type_Validation(Type);
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
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "номер паспорта (сертификата)", "4")]
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

    protected static bool PassportNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((PassportNumberNote.Value == null)||(PassportNumberNote.Value == ""))
            //    value.AddError( "Заполните примечание");//to do note handling
            return true;
        }
        return true;
    }

    #endregion

    #region Type (5)

    public string Type_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "тип", "5")]
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
            OnPropertyChanged();
        }
    }

    private void Type_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        Type_DB = tmp.Trim();
    }

    private bool Type_Validation(RamAccess<string> value)
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
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "радионуклиды", "6")]
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
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        Radionuclids_DB = tmp.Trim();
    }

    private bool Radionuclids_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (AutoRn)
        {
            AutoRn = false;
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
        foreach (var nuclid in nuclids)
        {
            if (!Spravochniks.SprRadionuclids
                    .Where(item => nuclid == item.Item1)
                    .Select(item => item.Item1)
                    .Any())
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

    #region FactoryNumber (7)

    public string FactoryNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "номер", "7")]
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

    #region Activity (8)

    public string Activity_DB { get; set; } = "";

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
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        tmp = tmp.Trim()
            .TrimStart('(')
            .TrimEnd(')')
            .ToLower()
            .Replace('.', ',')
            .Replace('е', 'e');
        if (tmp.Equals("-"))
        {
            Activity_DB = tmp;
            return;
        }
        if (double.TryParse(tmp, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            tmp = $"{doubleValue:0.######################################################e+00}";
        }
        Activity_DB = tmp;
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
        var value1 = value.Value
            .Trim()
            .TrimStart('(')
            .TrimEnd(')')
            .ToLower()
            .Replace('.', ',')
            .Replace('е', 'e');
        if (!double.TryParse(value1, 
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

    #region CreatorOKPO (9)

    public string CreatorOKPO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "код ОКПО изготовителя", "9")]
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
    }//If change this change validation

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
            //if ((CreatorOKPONote.Value == null) || CreatorOKPONote.Value.Equals(""))
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

    #region CreationDate (10)

    public string CreationDate_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "дата выпуска", "10")]
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
    }//If change this change validation

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
            //if ((CreationDateNote == null) || CreationDateNote.Equals(""))
            //    value.AddError( "Заполните примечание");
            return true;
        }
        var tmp = value.Value.Trim(); ;
        if (!Date8NumRegex().IsMatch(tmp) 
            || !DateOnly.TryParse(tmp, CultureInfo.CreateSpecificCulture("ru-RU"), out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region AggregateState (11)

    public byte? AggregateState_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "агрегатное состояние", "11")]
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

    private bool AggregateState_Validation(RamAccess<byte?> value)//Ready
    {
        value.ClearErrors();
        if (value.Value == null || value.Value != 1 && value.Value != 2 && value.Value != 3)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region PropertyCode (12)

    public byte? PropertyCode_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Право собственности на ОРИ", "код формы собственности", "12")]
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
        if (value.Value == null)//ok
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

    #region Owner (13)

    public string Owner_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Право собственности на ОРИ", "код ОКПО правообладателя", "13")]
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
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
            return true;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((OwnerNote.Value == null) || OwnerNote.Value.Equals(""))
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

    #region ProviderOrRecieverOKPO (17)

    public string ProviderOrRecieverOKPO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Код ОКПО", "поставщика или получателя", "17")]
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
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("Минобороны") || value.Value.Equals("прим.") || Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
            return true;
        }
        //try
        //{
        //    var a = int.Parse(OperationCode.Value) >= 10 && int.Parse(OperationCode.Value) <= 12;
        //    var b = int.Parse(OperationCode.Value) >= 41 && int.Parse(OperationCode.Value) <= 43;
        //    var c = int.Parse(OperationCode.Value) >= 71 && int.Parse(OperationCode.Value) <= 73;
        //    var d = OperationCode.Value is "15" or "17" or "18" or "46" or "47" or "48" or "53" or "54" or "58" or "61" or "62" or "65" or "67" or "68" or "75" or "76";
        //    if (a || b || c || d)
        //    {
        //        //ProviderOrRecieverOKPO.Value = "ОКПО ОТЧИТЫВАЮЩЕЙСЯ ОРГ";
        //        //return true;
        //    }
        //}
        //catch (Exception) { }

        if (value.Value.Length != 8 && value.Value.Length != 14
            || !OkpoRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); 
            return false;

        }
        return true;
    }

    #endregion

    #region TransporterOKPO (18)

    public string TransporterOKPO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Код ОКПО", "перевозчика", "18")]
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
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("-") 
            || value.Value.Equals("Минобороны") 
            || value.Value.Equals("прим.") 
            || Spravochniks.OKSM.Contains(value.Value.ToUpper()))
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

    #region PackName (19)

    public string PackName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "наименование", "19")]
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
            //if ((PackNameNote == null) || PackNameNote.Equals(""))
            //    value.AddError( "Заполните примечание");//to do note handling
            return true;
        }
        return true;
    }

    #endregion

    #region PackType (20)

    public string PackType_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "тип", "20")]
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
    }//If change this change validation

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

    #region PackNumber (21)

    public string PackNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "номер", "21")]
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
    }//If change this change validation

    private void PackNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value;
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
        Type_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        Radionuclids_DB = Convert.ToString(worksheet.Cells[row, 6].Value);
        FactoryNumber_DB = Convert.ToString(worksheet.Cells[row, 7].Value);
        Activity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 8].Value);
        CreatorOKPO_DB = Convert.ToString(worksheet.Cells[row, 9].Value);
        CreationDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 10].Text);
        AggregateState_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 11].Value), out var byteValue) ? byteValue : null;
        PropertyCode_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 12].Value), out byteValue) ? byteValue : null;
        Owner_DB = Convert.ToString(worksheet.Cells[row, 13].Value);
        DocumentVid_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 14].Value), out byteValue) ? byteValue : null;
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[row, 15].Value);
        DocumentDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 16].Text);
        ProviderOrRecieverOKPO_DB = Convert.ToString(worksheet.Cells[row, 17].Value);
        TransporterOKPO_DB = Convert.ToString(worksheet.Cells[row, 18].Value);
        PackName_DB = Convert.ToString(worksheet.Cells[row, 19].Value);
        PackType_DB = Convert.ToString(worksheet.Cells[row, 20].Value);
        PackNumber_DB = Convert.ToString(worksheet.Cells[row, 21].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ConvertToExcelString(PassportNumber_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(Type_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelString(Radionuclids_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelString(FactoryNumber_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelDouble(Activity_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelString(CreatorOKPO_DB);
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDate(CreationDate_DB, worksheet, row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0));
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = AggregateState_DB is null ? "-" : AggregateState_DB;
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = PropertyCode_DB is null ? "-" : PropertyCode_DB;
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ConvertToExcelString(Owner_DB);
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = DocumentVid_DB is null ? "-" : DocumentVid_DB;
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ConvertToExcelString(DocumentNumber_DB);
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ConvertToExcelDate(DocumentDate_DB, worksheet, row + (!transpose ? 12: 0), column + (transpose ? 12: 0));
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ConvertToExcelString(ProviderOrRecieverOKPO_DB);
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ConvertToExcelString(TransporterOKPO_DB);
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ConvertToExcelString(PackName_DB);
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ConvertToExcelString(PackType_DB);
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ConvertToExcelString(PackNumber_DB);

        return 18;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form1.ExcelHeader(worksheet, row: row, column, transpose);
        column += +(transpose ? cnt : 0);
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(PassportNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(Type))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(Radionuclids))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(FactoryNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(Activity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(CreatorOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(CreationDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(AggregateState))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(PropertyCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(Owner))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(DocumentVid))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(DocumentNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(DocumentDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(ProviderOrRecieverOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(TransporterOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(PackName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(PackType))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form13,Models")?.GetProperty(nameof(PackNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];

        return 18;
    }

    #endregion

    #region IDataGridColumn

    public override DataGridColumns GetColumnStructure(string param = "")
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

        var passportNumberR = ((FormPropertyAttribute)typeof(Form13)
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

        var typeR = ((FormPropertyAttribute)typeof(Form13)
                .GetProperty(nameof(Type))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (typeR != null)
        {
            typeR.SetSizeColToAllLevels(88);
            typeR.Binding = nameof(Type);
            numberInOrderR += typeR;
        }

        #endregion

        #region Radionuclids (6)

        var radionuclidsR = ((FormPropertyAttribute)typeof(Form13)
                .GetProperty(nameof(Radionuclids))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (radionuclidsR != null)
        {
            radionuclidsR.SetSizeColToAllLevels(88);
            radionuclidsR.Binding = nameof(Radionuclids);
            numberInOrderR += radionuclidsR;
        }

        #endregion

        #region FactoryNumber (7)

        var factoryNumberR = ((FormPropertyAttribute)typeof(Form13)
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

        #region Activity (8)

        var activityR = ((FormPropertyAttribute)typeof(Form13)
                .GetProperty(nameof(Activity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        activityR.SetSizeColToAllLevels(78);
        activityR.Binding = nameof(Activity);
        numberInOrderR += activityR;

        #endregion

        #region CreatorOKPO (9)

        var creatorOkpoR = ((FormPropertyAttribute)typeof(Form13)
                .GetProperty(nameof(CreatorOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        creatorOkpoR.SetSizeColToAllLevels(90);
        creatorOkpoR.Binding = nameof(CreatorOKPO);
        numberInOrderR += creatorOkpoR;

        #endregion

        #region CreationDate (10)

        var creationDateR = ((FormPropertyAttribute)typeof(Form13)
                .GetProperty(nameof(CreationDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        creationDateR.SetSizeColToAllLevels(90);
        creationDateR.Binding = nameof(CreationDate);
        numberInOrderR += creationDateR;

        #endregion

        #region AggregateState (11)

        var aggregateStateR = ((FormPropertyAttribute)typeof(Form13)
                .GetProperty(nameof(AggregateState))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        aggregateStateR.SetSizeColToAllLevels(90);
        aggregateStateR.Binding = nameof(AggregateState);
        numberInOrderR += aggregateStateR;

        #endregion

        #region PropertyCode (12)

        var propertyCodeR = ((FormPropertyAttribute)typeof(Form13)
                .GetProperty(nameof(PropertyCode))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        propertyCodeR.SetSizeColToAllLevels(90);
        propertyCodeR.Binding = nameof(PropertyCode);
        numberInOrderR += propertyCodeR;

        #endregion

        #region Owner (13)

        var ownerR = ((FormPropertyAttribute)typeof(Form13)
                .GetProperty(nameof(Owner))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        ownerR.SetSizeColToAllLevels(100);
        ownerR.Binding = nameof(Owner);
        numberInOrderR += ownerR;

        #endregion

        #region DocumentVid (14)

        var documentVidR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentVid))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentVidR.SetSizeColToAllLevels(80);
        documentVidR.Binding = nameof(DocumentVid);
        numberInOrderR += documentVidR;

        #endregion

        #region DocumentNumber (15)

        var documentNumberR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentNumberR.SetSizeColToAllLevels(125);
        documentNumberR.Binding = nameof(DocumentNumber);
        numberInOrderR += documentNumberR;

        #endregion

        #region DocumentDate (16)

        var documentDateR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentDateR.SetSizeColToAllLevels(88);
        documentDateR.Binding = nameof(DocumentDate);
        numberInOrderR += documentDateR;

        #endregion

        #region ProviderOrRecieverOKPO (17)

        var providerOrRecieverOkpoR = ((FormPropertyAttribute)typeof(Form13)
                .GetProperty(nameof(ProviderOrRecieverOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        providerOrRecieverOkpoR.SetSizeColToAllLevels(90);
        providerOrRecieverOkpoR.Binding = nameof(ProviderOrRecieverOKPO);
        numberInOrderR += providerOrRecieverOkpoR;

        #endregion

        #region TransporterOKPO (18)

        var transporterOkpoR = ((FormPropertyAttribute)typeof(Form13)
                .GetProperty(nameof(TransporterOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        transporterOkpoR.SetSizeColToAllLevels(105);
        transporterOkpoR.Binding = nameof(TransporterOKPO);
        numberInOrderR += transporterOkpoR;

        #endregion

        #region PackName (19)

        var packNameR = ((FormPropertyAttribute)typeof(Form13)
                .GetProperty(nameof(PackName)).GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packNameR.SetSizeColToAllLevels(163);
        packNameR.Binding = nameof(PackName);
        numberInOrderR += packNameR;

        #endregion

        #region PackType (20)

        var packTypeR = ((FormPropertyAttribute)typeof(Form13)
                .GetProperty(nameof(PackType))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packTypeR.SetSizeColToAllLevels(75);
        packTypeR.Binding = nameof(PackType);
        numberInOrderR += packTypeR;

        #endregion

        #region PackNumber (21)

        var packNumberR = ((FormPropertyAttribute)typeof(Form13)
                .GetProperty(nameof(PackNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packNumberR.SetSizeColToAllLevels(100);
        packNumberR.Binding = nameof(PackNumber);
        numberInOrderR += packNumberR;

        #endregion

        return numberInOrderR;
    }

    #endregion
}