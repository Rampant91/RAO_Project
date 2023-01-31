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

[Form_Class("Форма 1.4: Сведения об ОРИ, кроме отдельных изделий")]
public class Form14 : Form1
{
    public Form14() : base()
    {
        FormNum.Value = "1.4";
        Validate_all();
    }
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

    #region PassportNumber
    public string PassportNumber_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта на открытый радионуклидный источник", "номер паспорта","4")]
    public RamAccess<string> PassportNumber
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PassportNumber)))
            {
                ((RamAccess<string>)Dictionary[nameof(PassportNumber)]).Value = PassportNumber_DB;
                return (RamAccess<string>)Dictionary[nameof(PassportNumber)];
            }
            else
            {
                var rm = new RamAccess<string>(PassportNumber_Validation, PassportNumber_DB);
                rm.PropertyChanged += PassportNumberValueChanged;
                Dictionary.Add(nameof(PassportNumber), rm);
                return (RamAccess<string>)Dictionary[nameof(PassportNumber)];
            }
        }
        set
        {
            PassportNumber_DB = value.Value;
            OnPropertyChanged(nameof(PassportNumber));
        }
    }
    private void PassportNumberValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PassportNumber_DB = ((RamAccess<string>)Value).Value;
        }
    }
    protected bool PassportNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if(string.IsNullOrEmpty(value.Value))
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

    #region Name
    public string Name_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта на открытый радионуклидный источник", "наименование","5")]
    public RamAccess<string> Name
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Name)))
            {
                ((RamAccess<string>)Dictionary[nameof(Name)]).Value = Name_DB;
                return (RamAccess<string>)Dictionary[nameof(Name)];
            }
            else
            {
                var rm = new RamAccess<string>(Name_Validation, Name_DB);
                rm.PropertyChanged += NameValueChanged;
                Dictionary.Add(nameof(Name), rm);
                return (RamAccess<string>)Dictionary[nameof(Name)];
            }
        }
        set
        {
            Name_DB = value.Value;
            OnPropertyChanged(nameof(Name));
        }
    }
    private void NameValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Name_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool Name_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if(string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        return true;
    }
    #endregion

    #region Sort
    public byte? Sort_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта на открытый радионуклидный источник", "вид","6")]
    public RamAccess<byte?> Sort
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Sort)))
            {
                ((RamAccess<byte?>)Dictionary[nameof(Sort)]).Value = Sort_DB;
                return (RamAccess<byte?>)Dictionary[nameof(Sort)];
            }
            else
            {
                var rm = new RamAccess<byte?>(Sort_Validation, Sort_DB);
                rm.PropertyChanged += SortValueChanged;
                Dictionary.Add(nameof(Sort), rm);
                return (RamAccess<byte?>)Dictionary[nameof(Sort)];
            }
        }
        set
        {
            Sort_DB = value.Value;
            OnPropertyChanged(nameof(Sort));
        }
    }//If change this change validation

    private void SortValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Sort_DB = ((RamAccess<byte?>)Value).Value;
        }
    }
    private bool Sort_Validation(RamAccess<byte?> value)//TODO
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (!(value.Value >= 4 && value.Value <= 12))
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
    [FormProperty(true,"Сведения из паспорта на открытый радионуклидный источник", "радионуклиды","7")]
    public RamAccess<string> Radionuclids
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Radionuclids)))
            {
                ((RamAccess<string>)Dictionary[nameof(Radionuclids)]).Value = Radionuclids_DB;
                return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
            }
            else
            {
                var rm = new RamAccess<string>(Radionuclids_Validation, Radionuclids_DB);
                rm.PropertyChanged += RadionuclidsValueChanged;
                Dictionary.Add(nameof(Radionuclids), rm);
                return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
            }
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

    #region Activity
    public string Activity_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта на открытый радионуклидный источник", "активность, Бк","8")]
    public RamAccess<string> Activity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Activity)))
            {
                ((RamAccess<string>)Dictionary[nameof(Activity)]).Value = Activity_DB;
                return (RamAccess<string>)Dictionary[nameof(Activity)];
            }
            else
            {
                var rm = new RamAccess<string>(Activity_Validation, Activity_DB);
                rm.PropertyChanged += ActivityValueChanged;
                Dictionary.Add(nameof(Activity), rm);
                return (RamAccess<string>)Dictionary[nameof(Activity)];
            }
        }
        set
        {
            Activity_DB = value.Value;
            OnPropertyChanged(nameof(Activity));
        }
    }
    private void ActivityValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
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
                catch (Exception ex)
                { }
            }
            Activity_DB = value1;
        }
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
            return true;
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
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region ActivityMeasurementDate
    public string ActivityMeasurementDate_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта на открытый радионуклидный источник", "дата измерения активности","9")]
    public RamAccess<string> ActivityMeasurementDate
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(ActivityMeasurementDate)))
            {
                ((RamAccess<string>)Dictionary[nameof(ActivityMeasurementDate)]).Value = ActivityMeasurementDate_DB;
                return (RamAccess<string>)Dictionary[nameof(ActivityMeasurementDate)];
            }
            else
            {
                var rm = new RamAccess<string>(ActivityMeasurementDate_Validation, ActivityMeasurementDate_DB);
                rm.PropertyChanged += ActivityMeasurementDateValueChanged;
                Dictionary.Add(nameof(ActivityMeasurementDate), rm);
                return (RamAccess<string>)Dictionary[nameof(ActivityMeasurementDate)];
            }
        }
        set
        {
            ActivityMeasurementDate_DB = value.Value;
            OnPropertyChanged(nameof(ActivityMeasurementDate));
        }
    }//if change this change validation

    private void ActivityMeasurementDateValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var tmp = ((RamAccess<string>)Value).Value;
            Regex b = new("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            ActivityMeasurementDate_DB = tmp;
        }
    }
    private bool ActivityMeasurementDate_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if(string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
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
    public string Volume_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта на открытый радионуклидный источник", "объем, куб. м","10")]
    public RamAccess<string> Volume
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Volume)))
            {
                ((RamAccess<string>)Dictionary[nameof(Volume)]).Value = Volume_DB;
                return (RamAccess<string>)Dictionary[nameof(Volume)];
            }
            else
            {
                var rm = new RamAccess<string>(Volume_Validation, Volume_DB);
                rm.PropertyChanged += VolumeValueChanged;
                Dictionary.Add(nameof(Volume), rm);
                return (RamAccess<string>)Dictionary[nameof(Volume)];
            }
        }
        set
        {
            Volume_DB = value.Value;
            OnPropertyChanged(nameof(Volume));
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
                catch (Exception ex)
                { }
            }
            Volume_DB = value1;
        }
    }
    private bool Volume_Validation(RamAccess<string> value)//TODO
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
        if (value.Value.Equals("-"))
        {
            return true;
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
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    #endregion

    #region Mass
    public string Mass_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта на открытый радионуклидный источник", "масса, кг","11")]
    public RamAccess<string> Mass
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Mass)))
            {
                ((RamAccess<string>)Dictionary[nameof(Mass)]).Value = Mass_DB;
                return (RamAccess<string>)Dictionary[nameof(Mass)];
            }
            else
            {
                var rm = new RamAccess<string>(Mass_Validation, Mass_DB);
                rm.PropertyChanged += MassValueChanged;
                Dictionary.Add(nameof(Mass), rm);
                return (RamAccess<string>)Dictionary[nameof(Mass)];
            }
        }
        set
        {
            Mass_DB = value.Value;
            OnPropertyChanged(nameof(Mass));
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
                catch (Exception ex)
                { }
            }
            Mass_DB = value1;
        }
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
        if (value.Value.Equals("-"))
        {
            return true;
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
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
            {
                value.AddError("Число должно быть больше нуля"); return false;
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

    #region AggregateState
    public byte? AggregateState_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта на открытый радионуклидный источник", "агрегатное состояние","12")]
    public RamAccess<byte?> AggregateState//1 2 3
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(AggregateState)))
            {
                ((RamAccess<byte?>)Dictionary[nameof(AggregateState)]).Value = AggregateState_DB;
                return (RamAccess<byte?>)Dictionary[nameof(AggregateState)];
            }
            else
            {
                var rm = new RamAccess<byte?>(AggregateState_Validation, AggregateState_DB);
                rm.PropertyChanged += AggregateStateValueChanged;
                Dictionary.Add(nameof(AggregateState), rm);
                return (RamAccess<byte?>)Dictionary[nameof(AggregateState)];
            }
        }
        set
        {
            AggregateState_DB = value.Value;
            OnPropertyChanged(nameof(AggregateState));
        }
    }
    private void AggregateStateValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            AggregateState_DB = ((RamAccess<byte?>)Value).Value;
        }
    }
    private bool AggregateState_Validation(RamAccess<byte?> value)//Ready
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value != 1 && value.Value != 2 && value.Value != 3)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region PropertyCode
    public byte? PropertyCode_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Право собственности на ОРИ", "код формы собственности","13")]
    public RamAccess<byte?> PropertyCode
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PropertyCode)))
            {
                ((RamAccess<byte?>)Dictionary[nameof(PropertyCode)]).Value = PropertyCode_DB;
                return (RamAccess<byte?>)Dictionary[nameof(PropertyCode)];
            }
            else
            {
                var rm = new RamAccess<byte?>(PropertyCode_Validation, PropertyCode_DB);
                rm.PropertyChanged += PropertyCodeValueChanged;
                Dictionary.Add(nameof(PropertyCode), rm);
                return (RamAccess<byte?>)Dictionary[nameof(PropertyCode)];
            }
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
        if (value.Value == null)//ok
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (!(value.Value >= 1 && value.Value <= 9))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region Owner
    public string Owner_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Право собственности на ОРИ", "код ОКПО правообладателя","14")]
    public RamAccess<string> Owner
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Owner)))
            {
                ((RamAccess<string>)Dictionary[nameof(Owner)]).Value = Owner_DB;
                return (RamAccess<string>)Dictionary[nameof(Owner)];
            }
            else
            {
                var rm = new RamAccess<string>(Owner_Validation, Owner_DB);
                rm.PropertyChanged += OwnerValueChanged;
                Dictionary.Add(nameof(Owner), rm);
                return (RamAccess<string>)Dictionary[nameof(Owner)];
            }
        }
        set
        {
            Owner_DB = value.Value;
            OnPropertyChanged(nameof(Owner));
        }
    }//if change this change validation

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

    #region ProviderOrRecieverOKPO
    public string ProviderOrRecieverOKPO_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Код ОКПО", "поставщика или получателя","18")]
    public RamAccess<string> ProviderOrRecieverOKPO
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(ProviderOrRecieverOKPO)))
            {
                ((RamAccess<string>)Dictionary[nameof(ProviderOrRecieverOKPO)]).Value = ProviderOrRecieverOKPO_DB;
                return (RamAccess<string>)Dictionary[nameof(ProviderOrRecieverOKPO)];
            }
            else
            {
                var rm = new RamAccess<string>(ProviderOrRecieverOKPO_Validation, ProviderOrRecieverOKPO_DB);
                rm.PropertyChanged += ProviderOrRecieverOKPOValueChanged;
                Dictionary.Add(nameof(ProviderOrRecieverOKPO), rm);
                return (RamAccess<string>)Dictionary[nameof(ProviderOrRecieverOKPO)];
            }
        }
        set
        {
            ProviderOrRecieverOKPO_DB = value.Value;
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
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
            return true;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((ProviderOrRecieverOKPONote.Value == null) || ProviderOrRecieverOKPONote.Value.Equals(""))
            //    value.AddError( "Заполните примечание");
            return true;
        }
        if (value.Value.Equals("Минобороны"))
        {
            return true;
        }
        try
        {
            var tmp = short.Parse(OperationCode.Value);
            var a = tmp >= 10 && tmp <= 12;
            var b = tmp >= 41 && tmp <= 43;
            var c = tmp >= 71 && tmp <= 73;
            var d = tmp is 15 or 17 or 18 or 46 or 47 or 48 or 53 or 54 or 58 or 61 or 62 or 65 or 67 or 68 or 75 or 76;
            if (a || b || c || d)
            {
                //ProviderOrRecieverOKPO.Value = "ОКПО ОТЧИТЫВАЮЩЕЙСЯ ОРГ";
                //return false;
            }
        }
        catch (Exception) { }
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

    #region TransporterOKPO
    public string TransporterOKPO_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Код ОКПО", "перевозчика","19")]
    public RamAccess<string> TransporterOKPO
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(TransporterOKPO)))
            {
                ((RamAccess<string>)Dictionary[nameof(TransporterOKPO)]).Value = TransporterOKPO_DB;
                return (RamAccess<string>)Dictionary[nameof(TransporterOKPO)];
            }
            else
            {
                var rm = new RamAccess<string>(TransporterOKPO_Validation, TransporterOKPO_DB);
                rm.PropertyChanged += TransporterOKPOValueChanged;
                Dictionary.Add(nameof(TransporterOKPO), rm);
                return (RamAccess<string>)Dictionary[nameof(TransporterOKPO)];
            }
        }
        set
        {
            TransporterOKPO_DB = value.Value;
            OnPropertyChanged(nameof(TransporterOKPO));
        }
    }
    private void TransporterOKPOValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
            if (value1 != null)
                if (Spravochniks.OKSM.Contains(value1.ToUpper()))
                {
                    value1 = value1.ToUpper();
                }
            TransporterOKPO_DB = value1;
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
        if (value.Value.Equals("-"))
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
        if (value.Value.Equals("прим."))
        {
            //if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
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

    #region PackName
    public string PackName_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Прибор (установка), УКТ или иная упаковка", "наименование","20")]
    public RamAccess<string> PackName
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PackName)))
            {
                ((RamAccess<string>)Dictionary[nameof(PackName)]).Value = PackName_DB;
                return (RamAccess<string>)Dictionary[nameof(PackName)];
            }
            else
            {
                var rm = new RamAccess<string>(PackName_Validation, PackName_DB);
                rm.PropertyChanged += PackNameValueChanged;
                Dictionary.Add(nameof(PackName), rm);
                return (RamAccess<string>)Dictionary[nameof(PackName)];
            }
        }
        set
        {
            PackName_DB = value.Value;
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
            //    value.AddError( "Заполните примечание");//to do note handling
            return true;
        }
        return true;
    }
    #endregion

    #region PackType
    public string PackType_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Прибор (установка), УКТ или иная упаковка", "тип","21")]
    public RamAccess<string> PackType
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PackType)))
            {
                ((RamAccess<string>)Dictionary[nameof(PackType)]).Value = PackType_DB;
                return (RamAccess<string>)Dictionary[nameof(PackType)];
            }
            else
            {
                var rm = new RamAccess<string>(PackType_Validation, PackType_DB);
                rm.PropertyChanged += PackTypeValueChanged;
                Dictionary.Add(nameof(PackType), rm);
                return (RamAccess<string>)Dictionary[nameof(PackType)];
            }
        }
        set
        {
            PackType_DB = value.Value;
        }
    }//If change this change validation

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
            //    value.AddError( "Заполните примечание");// to do note handling
            return true;
        }
        return true;
    }
    #endregion

    #region PackNumber
    public string PackNumber_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Прибор (установка), УКТ или иная упаковка", "номер упаковки","22")]
    public RamAccess<string> PackNumber
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PackNumber)))
            {
                ((RamAccess<string>)Dictionary[nameof(PackNumber)]).Value = PackNumber_DB;
                return (RamAccess<string>)Dictionary[nameof(PackNumber)];
            }
            else
            {
                var rm = new RamAccess<string>(PackNumber_Validation, PackNumber_DB);
                rm.PropertyChanged += PackNumberValueChanged;
                Dictionary.Add(nameof(PackNumber), rm);
                return (RamAccess<string>)Dictionary[nameof(PackNumber)];
            }
        }
        set
        {
            PackNumber_DB = value.Value;
        }
    }//If change this change validation

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
            //    value.AddError( "Заполните примечание");// to do note handling
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

    #region IExcel
    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        base.ExcelGetRow(worksheet, row);
        PassportNumber_DB = Convert.ToString(worksheet.Cells[row, 4].Value);
        Name_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        Sort_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 6].Value), out var byteValue)
            ? byteValue
            : null;
        Radionuclids_DB = Convert.ToString(worksheet.Cells[row, 7].Value);
        Activity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 8].Value);
        ActivityMeasurementDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 9].Value);
        Volume_DB = ConvertFromExcelDouble(worksheet.Cells[row, 10].Value);
        Mass_DB = ConvertFromExcelDouble(worksheet.Cells[row, 11].Value);
        AggregateState_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 12].Value), out byteValue)
            ? byteValue
            : null;
        PropertyCode_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 13].Value), out byteValue)
            ? byteValue
            : null;
        Owner_DB = Convert.ToString(worksheet.Cells[row, 14].Value);
        DocumentVid_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 15].Value), out byteValue)
            ? byteValue
            : null;
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[row, 16].Value);
        DocumentDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 17].Value);
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
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelDate(ActivityMeasurementDate_DB);
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDouble(Volume_DB);
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelDouble(Mass_DB);
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = AggregateState_DB is null ? "-" : AggregateState_DB;
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = PropertyCode_DB is null ? "-" : PropertyCode_DB;
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ConvertToExcelString(Owner_DB);
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = DocumentVid_DB is null ? "-" : DocumentVid_DB;
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ConvertToExcelString(DocumentNumber_DB);
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ConvertToExcelDate(DocumentDate_DB);
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

            #region PassportNumber (4)
            var PassportNumberR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(PassportNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PassportNumberR.SetSizeColToAllLevels(95);
            PassportNumberR.Binding = nameof(PassportNumber);
            NumberInOrderR += PassportNumberR;
            #endregion

            #region Name (5)
            var NameR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(Name)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            NameR.SetSizeColToAllLevels(125);
            NameR.Binding = nameof(Name);
            NumberInOrderR += NameR;
            #endregion

            #region Sort (6)
            var SortR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(Sort)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            SortR.SetSizeColToAllLevels(88);
            SortR.Binding = nameof(Sort);
            NumberInOrderR += SortR;
            #endregion

            #region Radionuclids (7)
            var RadionuclidsR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(Radionuclids)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            RadionuclidsR.SetSizeColToAllLevels(143);
            RadionuclidsR.Binding = nameof(Radionuclids);
            NumberInOrderR += RadionuclidsR;
            #endregion

            #region Activity (8)
            var ActivityR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(Activity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            ActivityR.SetSizeColToAllLevels(88);
            ActivityR.Binding = nameof(Activity);
            NumberInOrderR += ActivityR;
            #endregion

            #region ActivityMeasurementDate (9)
            var ActivityMeasurementDateR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(ActivityMeasurementDate)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            ActivityMeasurementDateR.SetSizeColToAllLevels(163);
            ActivityMeasurementDateR.Binding = nameof(ActivityMeasurementDate);
            NumberInOrderR += ActivityMeasurementDateR;
            #endregion

            #region Volume (10)
            var VolumeR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(Volume)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            VolumeR.SetSizeColToAllLevels(88);
            VolumeR.Binding = nameof(Volume);
            NumberInOrderR += VolumeR;
            #endregion

            #region Mass (11)
            var MassR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(Mass)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            MassR.SetSizeColToAllLevels(88);
            MassR.Binding = nameof(Mass);
            NumberInOrderR += MassR;
            #endregion

            #region AggregateState (12)
            var AggregateStateR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(AggregateState)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            AggregateStateR.SetSizeColToAllLevels(163);
            AggregateStateR.Binding = nameof(AggregateState);
            NumberInOrderR += AggregateStateR;
            #endregion

            #region PropertyCode (13)
            var PropertyCodeR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(PropertyCode)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PropertyCodeR.SetSizeColToAllLevels(90);
            PropertyCodeR.Binding = nameof(PropertyCode);
            NumberInOrderR += PropertyCodeR;
            #endregion

            #region Owner (14)
            var OwnerR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(Owner)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            OwnerR.SetSizeColToAllLevels(100);
            OwnerR.Binding = nameof(Owner);
            NumberInOrderR += OwnerR;
            #endregion

            #region DocumentVid (15)
            var DocumentVidR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentVid)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentVidR.SetSizeColToAllLevels(60);
            DocumentVidR.Binding = nameof(DocumentVid);
            NumberInOrderR += DocumentVidR;
            #endregion

            #region DocumentNumber (16)
            var DocumentNumberR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentNumberR.SetSizeColToAllLevels(100);
            DocumentNumberR.Binding = nameof(DocumentNumber);
            NumberInOrderR += DocumentNumberR;
            #endregion

            #region DocumentDate (17)
            var DocumentDateR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentDate)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentDateR.SetSizeColToAllLevels(80);
            DocumentDateR.Binding = nameof(DocumentDate);
            NumberInOrderR += DocumentDateR;
            #endregion

            #region ProviderOrRecieverOKPO (18)
            var ProviderOrRecieverOKPOR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(ProviderOrRecieverOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            ProviderOrRecieverOKPOR.SetSizeColToAllLevels(100);
            ProviderOrRecieverOKPOR.Binding = nameof(ProviderOrRecieverOKPO);
            NumberInOrderR += ProviderOrRecieverOKPOR;
            #endregion

            #region TransporterOKPO (19)
            var TransporterOKPOR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(TransporterOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            TransporterOKPOR.SetSizeColToAllLevels(90);
            TransporterOKPOR.Binding = nameof(TransporterOKPO);
            NumberInOrderR += TransporterOKPOR;
            #endregion

            #region PackName (20)
            var PackNameR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(PackName)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PackNameR.SetSizeColToAllLevels(143);
            PackNameR.Binding = nameof(PackName);
            NumberInOrderR += PackNameR;
            #endregion

            #region PackType (21)
            var PackTypeR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(PackType)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PackTypeR.SetSizeColToAllLevels(88);
            PackTypeR.Binding = nameof(PackType);
            NumberInOrderR += PackTypeR;
            #endregion

            #region PackNumber (22)
            var PackNumberR = ((FormPropertyAttribute)typeof(Form14).GetProperty(nameof(PackNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PackNumberR.SetSizeColToAllLevels(125);
            PackNumberR.Binding = nameof(PackNumber);
            NumberInOrderR += PackNumberR;
            #endregion

            _DataGridColumns = NumberInOrderR;
        }
        return _DataGridColumns;
    }
    #endregion
}