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
[Form_Class("Форма 1.5: Сведения о РАО в виде отработавших ЗРИ")]
public class Form15 : Form1
{
    #region Constructor
    
    public Form15()
    {
        FormNum.Value = "1.5";
        Validate_all();
    }

    #endregion

    #region Validation

    private void Validate_all()
    {
        Type_Validation(Type);
        PackName_Validation(PackName);
        PackNumber_Validation(PackNumber);
        PackType_Validation(PackType);
        PassportNumber_Validation(PassportNumber);
        FactoryNumber_Validation(FactoryNumber);
        ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
        TransporterOKPO_Validation(TransporterOKPO);
        Activity_Validation(Activity);
        Radionuclids_Validation(Radionuclids);
        Quantity_Validation(Quantity);
        CreationDate_Validation(CreationDate);
        Subsidy_Validation(Subsidy);
        FcpNumber_Validation(FcpNumber);
        StatusRAO_Validation(StatusRAO);
        RefineOrSortRAOCode_Validation(RefineOrSortRAOCode);
        StoragePlaceName_Validation(StoragePlaceName);
        StoragePlaceCode_Validation(StoragePlaceCode);
    }

    public override bool Object_Validation()
    {
        return !(Type.HasErrors ||
                 PackName.HasErrors ||
                 PackNumber.HasErrors ||
                 PackType.HasErrors ||
                 PassportNumber.HasErrors ||
                 FactoryNumber.HasErrors ||
                 ProviderOrRecieverOKPO.HasErrors ||
                 TransporterOKPO.HasErrors ||
                 Activity.HasErrors ||
                 Radionuclids.HasErrors ||
                 Quantity.HasErrors ||
                 CreationDate.HasErrors ||
                 Subsidy.HasErrors ||
                 FcpNumber.HasErrors ||
                 StatusRAO.HasErrors ||
                 RefineOrSortRAOCode.HasErrors ||
                 StoragePlaceName.HasErrors ||
                 StoragePlaceCode.HasErrors);
    }

    protected override bool DocumentNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
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
        if (value.Value is "15" or "17" or "46" or "47" or "53" or "54" or "58" or "61"
            or "62" or "65" or "66" or "67" or "81" or "82" or "83" or "85" or "86" or "87")
        {
            value.AddError("Код операции не может быть использован для РАО");
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
    [FormProperty(true, "Сведения об отработавших закрытых источниках ионизирующего излучения", "номер паспорта (сертификата) ЗРИ, акта определения характеристик ОЗИИ", "4")]
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
            //    value.AddError( "Заполните примечание");
            return true;
        }
        return true;
    }

    #endregion

    #region Type (5)

    public string Type_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения об отработавших закрытых источниках ионизирующего излучения", "тип", "5")]
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

    private void TypeValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Type_DB = ((RamAccess<string>)value).Value;
        }
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
    [FormProperty(true, "Сведения об отработавших закрытых источниках ионизирующего излучения", "радионуклиды", "6")]
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

    private void RadionuclidsValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Radionuclids_DB = ((RamAccess<string>)value).Value;
        }
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

    #region FactoryNumber (7)
    public string FactoryNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения об отработавших закрытых источниках ионизирующего излучения", "номер", "7")]
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

    private void FactoryNumberValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            FactoryNumber_DB = ((RamAccess<string>)value).Value;
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

    #region Quantity (8)

    public int? Quantity_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения об отработавших закрытых источниках ионизирующего излучения", "количество, шт", "8")]
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
    }// positive int.

    private void QuantityValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Quantity_DB = ((RamAccess<int?>)value).Value;
        }
    }

    private bool Quantity_Validation(RamAccess<int?> value)//Ready
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value <= 0)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region Activity (9)

    public string Activity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения об отработавших закрытых источниках ионизирующего излучения", "суммарная активность, Бк", "9")]
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

    private void ActivityValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
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
            if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
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
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var len = value1.Length;
        if (value1[0] == '(' && value1[len - 1] == ')')
        {
            value1 = value1.Remove(len - 1, 1).Remove(0, 1);
        }
        const NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent;
        if (!double.TryParse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB"), out var doubleValue))
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

    #region CreationDate (10)

    public string CreationDate_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения об отработавших закрытых источниках ионизирующего излучения", "дата выпуска", "10")]
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
    }//If change this change validation

    private void CreationDateValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        CreationDate_DB = tmp;
    }

    private bool CreationDate_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp) || !DateTimeOffset.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region StatusRAO (11)

    public string StatusRAO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-11", "Статус РАО", "11")]
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

    private void StatusRAOValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            StatusRAO_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool StatusRAO_Validation(RamAccess<string> value)//rdy
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Length == 1)
        {
            if (!int.TryParse(value.Value, out var intValue) || intValue < 1 || (intValue > 4 && intValue != 6 && intValue != 9))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return false;
        }
        if (value.Value.Length != 8 && value.Value.Length != 14)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (!new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$").IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region ProviderOrRecieverOKPO (15)

    public string ProviderOrRecieverOKPO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Код ОКПО", "поставщика или получателя", "15")]
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
        }
        set
        {
            ProviderOrRecieverOKPO_DB = value.Value;
            OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
        }
    }

    private void ProviderOrRecieverOKPOValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
            if (Spravochniks.OKSM.Contains(value1.ToUpper()))
            {
                value1 = value1.ToUpper();
            }
        ProviderOrRecieverOKPO_DB = value1;
    }

    private bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим.") || Spravochniks.OKSM.Contains(value.Value.ToUpper()) || value.Value.Equals("Минобороны"))
        {
            return true;
        }
        if (OperationCode.Value != null)
        {
            var tmp = short.Parse(OperationCode.Value);
            if (tmp is 1 or >= 10 and <= 14 or 16 or 18 or >= 41 and <= 45 or 48 or 49
                or 51 or 52 or >= 55 and <= 57 or 59 or 68 or >= 71 and <= 73 or 75 or 76)
            {
                //ProviderOrRecieverOKPO.Value = "ОКПО ОТЧИТЫВАЮЩЕЙСЯ ОРГ";
                //return true;
            }
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

    #region TransporterOKPO (16)

    public string TransporterOKPO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Код ОКПО", "перевозчика", "16")]
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
        }
        set
        {
            TransporterOKPO_DB = value.Value;
            OnPropertyChanged(nameof(TransporterOKPO));
        }
    }

    private void TransporterOKPOValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            TransporterOKPO_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool TransporterOKPO_Validation(RamAccess<string> value)//Done
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
            //    value.AddError( "Заполните примечание");
            return true;
        }
        if (value.Value.Equals("-") || value.Value.Equals("Минобороны"))
        {
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

    #region PackName (17)

    public string PackName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "наименование", "17")]
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
        }
        set
        {
            PackName_DB = value.Value;
            OnPropertyChanged(nameof(PackName));
        }
    }

    private void PackNameValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
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
        return true;
    }

    #endregion

    #region PackType (18)

    public string PackType_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "тип", "18")]
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
        }
        set
        {
            PackType_DB = value.Value;
            OnPropertyChanged(nameof(PackType));
        }
    }//If change this change validation

    private void PackTypeValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PackType_DB = ((RamAccess<string>)value).Value;
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
            //    value.AddError( "Заполните примечание");// to do note handling
            return true;
        }
        return true;
    }

    #endregion

    #region PackNumber (19)

    public string PackNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Прибор (установка), УКТ или иная упаковка", "заводской номер", "19")]
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
        }
        set
        {
            PackNumber_DB = value.Value;
            OnPropertyChanged(nameof(PackNumber));
        }
    }//If change this change validation

    private void PackNumberValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PackNumber_DB = ((RamAccess<string>)value).Value;
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
        return true;
    }

    #endregion

    #region StoragePlaceName (20)

    public string StoragePlaceName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Пункт хранения", "наименование", "20")]
    public RamAccess<string> StoragePlaceName
    {
        get
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
        set
        {
            StoragePlaceName_DB = value.Value;
            OnPropertyChanged(nameof(StoragePlaceName));
        }
    }//If change this change validation

    private void StoragePlaceNameValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            StoragePlaceName_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool StoragePlaceName_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        //List<string> a = new List<string>();//here binds spr
        //if (a.Contains(value.Value))
        //{
        //    return true;
        //}
        //value.AddError("Недопустимое значение");
        //return false;
        return true;
    }

    #endregion

    #region StoragePlaceCode (21)

    public string StoragePlaceCode_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Пункт хранения", "код", "21")]
    public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
    {
        get
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
        set
        {
            StoragePlaceCode_DB = value.Value;
            OnPropertyChanged(nameof(StoragePlaceCode));
        }
    }//if change this change validation

    private void StoragePlaceCodeValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            StoragePlaceCode_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value == "-") return true;
        if (!new Regex("^[0-9]{8}$").IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        var tmp = value.Value;
        if (tmp.Length != 8) return true;
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
        if (!new Regex("^[1]{1}[1-9]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1]{1}|^[4]{1}[1-49]{1}|^[5]{1}[1-69]{1}|^[6]{1}[1]{1}|^[7]{1}[1349]{1}|^[8]{1}[1-69]{1}|^[9]{1}[9]{1}")
                .IsMatch(tmp.Substring(6, 2)))
        {
            value.AddError($"Недопустимый код типа РАО - {tmp.Substring(6, 2)}");
        }
        return !value.HasErrors;
    }

    #endregion

    #region RefineOrSortRAOCode (22)

    public string RefineOrSortRAOCode_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-22", "Код переработки / сортировки РАО", "22")]
    public RamAccess<string> RefineOrSortRAOCode //2 cyfer code or empty.
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(RefineOrSortRAOCode)))
            {
                ((RamAccess<string>)Dictionary[nameof(RefineOrSortRAOCode)]).Value = RefineOrSortRAOCode_DB;
                return (RamAccess<string>)Dictionary[nameof(RefineOrSortRAOCode)];
            }
            var rm = new RamAccess<string>(RefineOrSortRAOCode_Validation, RefineOrSortRAOCode_DB);
            rm.PropertyChanged += RefineOrSortRAOCodeValueChanged;
            Dictionary.Add(nameof(RefineOrSortRAOCode), rm);
            return (RamAccess<string>)Dictionary[nameof(RefineOrSortRAOCode)];
        }
        set
        {
            RefineOrSortRAOCode_DB = value.Value;
            OnPropertyChanged(nameof(RefineOrSortRAOCode));
        }
    }//If change this change validation

    private void RefineOrSortRAOCodeValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            RefineOrSortRAOCode_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool RefineOrSortRAOCode_Validation(RamAccess<string> value)//TODO
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
        if (!Spravochniks.SprRifineOrSortCodes.Contains(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region Subsidy (23)

    public string Subsidy_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-23", "Субсидия, %", "23")]
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
        if (value.Value.Equals("-"))
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

    #region FcpNumber (24)

    public string FcpNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-24", "Номер мероприятия ФЦП", "24")]
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
        PassportNumber_DB = Convert.ToString(worksheet.Cells[row, 4].Value);
        Type_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        Radionuclids_DB = Convert.ToString(worksheet.Cells[row, 6].Value);
        FactoryNumber_DB = Convert.ToString(worksheet.Cells[row, 7].Value);
        Quantity_DB = int.TryParse(Convert.ToString(worksheet.Cells[row, 8].Value), out var intValue) ? intValue : null;
        Activity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 9].Value);
        CreationDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 10].Text);
        StatusRAO_DB = Convert.ToString(worksheet.Cells[row, 11].Value);
        DocumentVid_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 12].Value), out var byteValue) ? byteValue : null;
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[row, 13].Value);
        DocumentDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 14].Text);
        ProviderOrRecieverOKPO_DB = Convert.ToString(worksheet.Cells[row, 15].Value);
        TransporterOKPO_DB = Convert.ToString(worksheet.Cells[row, 16].Value);
        PackName_DB = Convert.ToString(worksheet.Cells[row, 17].Value);
        PackType_DB = Convert.ToString(worksheet.Cells[row, 18].Value);
        PackNumber_DB = Convert.ToString(worksheet.Cells[row, 19].Value);
        StoragePlaceName_DB = Convert.ToString(worksheet.Cells[row, 20].Value);
        StoragePlaceCode_DB = Convert.ToString(worksheet.Cells[row, 21].Value);
        RefineOrSortRAOCode_DB = Convert.ToString(worksheet.Cells[row, 22].Value);
        Subsidy_DB = Convert.ToString(worksheet.Cells[row, 23].Value);
        FcpNumber_DB = Convert.ToString(worksheet.Cells[row, 24].Value);
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
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDate(CreationDate_DB);
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelString(StatusRAO_DB);
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = DocumentVid_DB is null ? "-" : DocumentVid_DB;
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ConvertToExcelString(DocumentNumber_DB);
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ConvertToExcelDate(DocumentDate_DB);
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ConvertToExcelString(ProviderOrRecieverOKPO_DB);
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ConvertToExcelString(TransporterOKPO_DB);
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ConvertToExcelString(PackName_DB);
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ConvertToExcelString(PackType_DB);
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ConvertToExcelString(PackNumber_DB);
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ConvertToExcelString(StoragePlaceName_DB);
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ConvertToExcelString(StoragePlaceCode_DB);
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ConvertToExcelString(RefineOrSortRAOCode_DB);
        worksheet.Cells[row + (!transpose ? 19 : 0), column + (transpose ? 19 : 0)].Value = ConvertToExcelString(Subsidy_DB);
        worksheet.Cells[row + (!transpose ? 20 : 0), column + (transpose ? 20 : 0)].Value = ConvertToExcelString(FcpNumber_DB);

        return 21;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form1.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(PassportNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(Type))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(Radionuclids))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(FactoryNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(Quantity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(Activity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(CreationDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(StatusRAO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(DocumentVid))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(DocumentNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(DocumentDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(ProviderOrRecieverOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(TransporterOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(PackName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(PackType))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(PackNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(StoragePlaceName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(StoragePlaceCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(RefineOrSortRAOCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 19 : 0), column + (transpose ? 19 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(Subsidy))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 20 : 0), column + (transpose ? 20 : 0)].Value = ((FormPropertyAttribute)System.Type.GetType("Models.Forms.Form1.Form15,Models")?.GetProperty(nameof(FcpNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        return 21;
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
        if (numberInOrderR is not null)
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

        var passportNumberR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(PassportNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (passportNumberR != null)
        {
            passportNumberR.SetSizeColToAllLevels(240);
            passportNumberR.Binding = nameof(PassportNumber);
            numberInOrderR += passportNumberR;
        }

        #endregion

        #region Type (5)

        var typeR = ((FormPropertyAttribute)typeof(Form15)
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

        var radionuclidsR = ((FormPropertyAttribute)typeof(Form15)
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

        var factoryNumberR = ((FormPropertyAttribute)typeof(Form15)
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

        #region Quantity (8)

        var quantityR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(Quantity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        quantityR.SetSizeColToAllLevels(70);
        quantityR.Binding = nameof(Quantity);
        numberInOrderR += quantityR;
        
        #endregion

        #region Activity (9)

        var activityR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(Activity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        activityR.SetSizeColToAllLevels(140);
        activityR.Binding = nameof(Activity);
        numberInOrderR += activityR;
        
        #endregion

        #region CreationDate (10)

        var creationDateR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(CreationDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        creationDateR.SetSizeColToAllLevels(130);
        creationDateR.Binding = nameof(CreationDate);
        numberInOrderR += creationDateR;
        
        #endregion

        #region StatusRAO (11)

        var statusRaoR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(StatusRAO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        statusRaoR.SetSizeColToAllLevels(78);
        statusRaoR.Binding = nameof(StatusRAO);
        numberInOrderR += statusRaoR;
        
        #endregion

        #region DocumentVid (12)

        var documentVidR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentVid))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentVidR.SetSizeColToAllLevels(50);
        documentVidR.Binding = nameof(DocumentVid);
        numberInOrderR += documentVidR;
        
        #endregion

        #region DocumentNumber (13)

        var documentNumberR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentNumberR.SetSizeColToAllLevels(60);
        documentNumberR.Binding = nameof(DocumentNumber);
        numberInOrderR += documentNumberR;
        
        #endregion

        #region DocumentDate (14)

        var documentDateR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentDateR.SetSizeColToAllLevels(78);
        documentDateR.Binding = nameof(DocumentDate);
        numberInOrderR += documentDateR;
        
        #endregion

        #region ProviderOrRecieverOKPO (15)

        var providerOrRecieverOkpoR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(ProviderOrRecieverOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        providerOrRecieverOkpoR.SetSizeColToAllLevels(100);
        providerOrRecieverOkpoR.Binding = nameof(ProviderOrRecieverOKPO);
        numberInOrderR += providerOrRecieverOkpoR;
        
        #endregion

        #region TransporterOKPO (16)

        var transporterOkpoR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(TransporterOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        transporterOkpoR.SetSizeColToAllLevels(125);
        transporterOkpoR.Binding = nameof(TransporterOKPO);
        numberInOrderR += transporterOkpoR;
        
        #endregion

        #region PackName (17)

        var packNameR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(PackName))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packNameR.SetSizeColToAllLevels(163);
        packNameR.Binding = nameof(PackName);
        numberInOrderR += packNameR;
        
        #endregion

        #region PackType (18)

        var packTypeR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(PackType))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packTypeR.SetSizeColToAllLevels(88);
        packTypeR.Binding = nameof(PackType);
        numberInOrderR += packTypeR;
        
        #endregion

        #region PackNumber (19)

        var packNumberR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(PackNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packNumberR.SetSizeColToAllLevels(140);
        packNumberR.Binding = nameof(PackNumber);
        numberInOrderR += packNumberR;
        
        #endregion

        #region StoragePlaceName (20)

        var storagePlaceNameR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(StoragePlaceName))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        storagePlaceNameR.SetSizeColToAllLevels(103);
        storagePlaceNameR.Binding = nameof(StoragePlaceName);
        numberInOrderR += storagePlaceNameR;
        
        #endregion

        #region StoragePlaceCode (21)

        var storagePlaceCodeR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(StoragePlaceCode))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        storagePlaceCodeR.SetSizeColToAllLevels(88);
        storagePlaceCodeR.Binding = nameof(StoragePlaceCode);
        numberInOrderR += storagePlaceCodeR;
        
        #endregion

        #region RefineOrSortRAOCode (22)

        var refineOrSortRAOCodeR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(RefineOrSortRAOCode))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        refineOrSortRAOCodeR.SetSizeColToAllLevels(115);
        refineOrSortRAOCodeR.Binding = nameof(RefineOrSortRAOCode);
        numberInOrderR += refineOrSortRAOCodeR;
        
        #endregion

        #region Subsidy (23)

        var subsidyR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(Subsidy))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        subsidyR.SetSizeColToAllLevels(88);
        subsidyR.Binding = nameof(Subsidy);
        numberInOrderR += subsidyR;
        
        #endregion

        #region FcpNumber (24)

        var fcpNumberR = ((FormPropertyAttribute)typeof(Form15)
                .GetProperty(nameof(FcpNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        fcpNumberR.SetSizeColToAllLevels(163);
        fcpNumberR.Binding = nameof(FcpNumber);
        numberInOrderR += fcpNumberR;
        
        #endregion

        _DataGridColumns = numberInOrderR;
        return _DataGridColumns;
    }

    #endregion
}