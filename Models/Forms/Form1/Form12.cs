using Models.DataAccess;
using System;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Spravochniki;
using System.Linq;
using System.ComponentModel;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using Models.Collections;
using Models.Forms;

namespace Models;

[Serializable]
[Form_Class("Форма 1.2: Сведения об изделиях из обедненного урана")]
public class Form12 : Form1
{
    public Form12() : base()
    {
        FormNum.Value = "1.2";
        Validate_all();
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
        SignedServicePeriod_Validation(SignedServicePeriod);
        TransporterOKPO_Validation(TransporterOKPO);
        FactoryNumber_Validation(FactoryNumber);
        Mass_Validation(Mass);
        NameIOU_Validation(NameIOU);
    }

    [FormProperty(true,"Форма")]
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

    #region PassportNumber
    public string PassportNumber_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта на изделие из обедненного урана", "номер паспорта", "4")]
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
        } set
        {
            PassportNumber_DB = value.Value; OnPropertyChanged(nameof(PassportNumber));
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

    #region NameIOU
    public string NameIOU_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта на изделие из обедненного урана", "наименование", "5")]
    public RamAccess<string> NameIOU
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(NameIOU)))
            {
                ((RamAccess<string>)Dictionary[nameof(NameIOU)]).Value = NameIOU_DB;
                return (RamAccess<string>)Dictionary[nameof(NameIOU)];
            }
            else
            {
                var rm = new RamAccess<string>(NameIOU_Validation, NameIOU_DB);
                rm.PropertyChanged += NameIOUValueChanged;
                Dictionary.Add(nameof(NameIOU), rm);
                return (RamAccess<string>)Dictionary[nameof(NameIOU)];
            }
        } set
        {
            NameIOU_DB = value.Value; OnPropertyChanged(nameof(NameIOU));
        }
    }


    private void NameIOUValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            NameIOU_DB = ((RamAccess<string>)Value).Value;
        }
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

    #region FactoryNumber
    public string FactoryNumber_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта на изделие из обедненного урана", "номер", "6")]
    public RamAccess<string> FactoryNumber
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(FactoryNumber)))
            {
                ((RamAccess<string>)Dictionary[nameof(FactoryNumber)]).Value = FactoryNumber_DB;
                return (RamAccess<string>)Dictionary[nameof(FactoryNumber)];
            }
            else
            {
                var rm = new RamAccess<string>(FactoryNumber_Validation, FactoryNumber_DB);
                rm.PropertyChanged += FactoryNumberValueChanged;
                Dictionary.Add(nameof(FactoryNumber), rm);
                return (RamAccess<string>)Dictionary[nameof(FactoryNumber)];
            }
        } set
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

    #region Mass
    public string Mass_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта на изделие из обедненного урана", "масса обедненного урана, кг", "7")]
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
        } set
        {
            Mass_DB = value.Value; OnPropertyChanged(nameof(Mass));
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

    #region CreatorOKPO
    public string CreatorOKPO_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта на изделие из обедненного урана", "код ОКПО изготовителя", "8")]
    public RamAccess<string> CreatorOKPO
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(CreatorOKPO)))
            {
                ((RamAccess<string>)Dictionary[nameof(CreatorOKPO)]).Value = CreatorOKPO_DB;
                return (RamAccess<string>)Dictionary[nameof(CreatorOKPO)];
            }
            else
            {
                var rm = new RamAccess<string>(CreatorOKPO_Validation, CreatorOKPO_DB);
                rm.PropertyChanged += CreatorOKPOValueChanged;
                Dictionary.Add(nameof(CreatorOKPO), rm);
                return (RamAccess<string>)Dictionary[nameof(CreatorOKPO)];
            }
        } set
        {
            CreatorOKPO_DB = value.Value; OnPropertyChanged(nameof(CreatorOKPO));
        }
    }
    //If change this change validation

    private void CreatorOKPOValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
            if (value1 != null)
                if (Spravochniks.OKSM.Contains(value1.ToUpper()))
                {
                    value1 = value1.ToUpper();
                }
            CreatorOKPO_DB = value1;
        }
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
        if (value.Value.Length != 8 && value.Value.Length != 14)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
        if (!mask.IsMatch(value.Value))
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
    [FormProperty(true,"Сведения из паспорта на изделие из обедненного урана", "дата выпуска", "9")]
    public RamAccess<string> CreationDate
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(CreationDate)))
            {
                ((RamAccess<string>)Dictionary[nameof(CreationDate)]).Value = CreationDate_DB;
                return (RamAccess<string>)Dictionary[nameof(CreationDate)];
            }
            else
            {
                var rm = new RamAccess<string>(CreationDate_Validation, CreationDate_DB);
                rm.PropertyChanged += CreationDateValueChanged;
                Dictionary.Add(nameof(CreationDate), rm);
                return (RamAccess<string>)Dictionary[nameof(CreationDate)];
            }
        } set
        {
            CreationDate_DB = value.Value; OnPropertyChanged(nameof(CreationDate));
        }
    }
    //If change this change validation

    private void CreationDateValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var tmp = ((RamAccess<string>)Value).Value;
            if (!tmp.Equals("прим."))
            {
                Regex b = new("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
            }
            CreationDate_DB = tmp;
        }
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

    #region SignedServicePeriod
    public string SignedServicePeriod_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения из паспорта на изделие из обедненного урана", "НСС, мес", "10")]
    public RamAccess<string> SignedServicePeriod
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(SignedServicePeriod)))
            {
                ((RamAccess<string>)Dictionary[nameof(SignedServicePeriod)]).Value = SignedServicePeriod_DB;
                return (RamAccess<string>)Dictionary[nameof(SignedServicePeriod)];
            }
            else
            {
                var rm = new RamAccess<string>(SignedServicePeriod_Validation, SignedServicePeriod_DB);
                rm.PropertyChanged += SignedServicePeriodValueChanged;
                Dictionary.Add(nameof(SignedServicePeriod), rm);
                return (RamAccess<string>)Dictionary[nameof(SignedServicePeriod)];
            }
        }
        set
        {
            SignedServicePeriod_DB = value.Value; OnPropertyChanged(nameof(SignedServicePeriod));
        }
    }

    private void SignedServicePeriodValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            SignedServicePeriod_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool SignedServicePeriod_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        try
        {
            if (Convert.ToSingle(value.Value) <= 0)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
        }
        catch
        {

        }
        return true;
    }
    #endregion

    #region PropertyCode
    public byte? PropertyCode_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Право собственности на ИОУ","код формы собственности", "11")]
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
        } set
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
        //if (value.Value == 255)//ok
        //{
        //    value.AddError( "Поле не заполнено");
        //}
        if (!(value.Value >= 1 && value.Value <= 9))
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    #endregion

    #region Owner
    public string Owner_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Право собственности на ИОУ", "код ОКПО правообладателя", "12")]
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
        } set
        {
            Owner_DB = value.Value; OnPropertyChanged(nameof(Owner));
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
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
            return true;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((OwnerNote.Value == null) || (OwnerNote.Value == ""))
            //    value.AddError( "Заполните примечание");
            return true;
        }
        if (value.Value.Length != 8 && value.Value.Length != 14)
        {
            value.AddError("Недопустимое значение"); return false;

        }
        var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
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
    [FormProperty(true,"Код ОКПО","поставщика или получателя", "16")]
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
        } set
        {
            ProviderOrRecieverOKPO_DB = value.Value; OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
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
        if (value.Value.Equals("Минобороны"))
        {
            return true;
        }
        if (value.Value.Equals("прим."))
        {
            //if ((ProviderOrRecieverOKPONote.Value == null) || ProviderOrRecieverOKPONote.Value.Equals(""))
            //    value.AddError( "Заполните примечания");
            return true;
        }
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
            return true;
        }
        var a = int.Parse(OperationCode.Value) >= 10 && int.Parse(OperationCode.Value) <= 12;
        var b = int.Parse(OperationCode.Value) >= 41 && int.Parse(OperationCode.Value) <= 43;
        var c = int.Parse(OperationCode.Value) >= 71 && int.Parse(OperationCode.Value) <= 73;
        var d = OperationCode.Value is "15" or "17" or "18" or "46" or "47" or "48" or "53" or "54" or "58" or "61" or "62" or "65" or "67" or "68" or "75" or "76";
        if (a || b || c || d)
        {
            //ProviderOrRecieverOKPO.Value = "ОКПО ОТЧИТЫВАЮЩЕЙСЯ ОРГ";
            //return false;
        }
        if (value.Value.Length != 8 && value.Value.Length != 14)
        {
            value.AddError("Недопустимое значение"); return false;
        }
        var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
        if (!mask.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    #endregion

    #region TransporterOKPO
    public string TransporterOKPO_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Код ОКПО", "перевозчика", "17")]
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
        } set
        {
            TransporterOKPO_DB = value.Value; OnPropertyChanged(nameof(TransporterOKPO));
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
    private bool TransporterOKPO_Validation(RamAccess<string> value)//TODO
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
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
            return true;
        }
        if (value.Value.Equals("Минобороны"))
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
            value.AddError("Недопустимое значение"); return false;

        }
        var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
        if (!mask.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    #endregion

    #region PackName
    public string PackName_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Прибор (установка), УКТ или иная упаковка","наименование", "18")]
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
        } set
        {
            PackName_DB = value.Value; OnPropertyChanged(nameof(PackName));
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
    [FormProperty(true,"Прибор (установка), УКТ или иная упаковка", "тип", "19")]
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
        } set
        {
            PackType_DB = value.Value; OnPropertyChanged(nameof(PackType));
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
            //    value.AddError( "Заполните примечание");//to do note handling
            return true;
        }
        return true;
    }
    #endregion

    #region PackNumber
    public string PackNumber_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Прибор (установка), УКТ или иная упаковка", "номер", "20")]
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
        } set
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
            //    value.AddError( "Заполните примечание");//to do note handling
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
        if(value.Value is "01" or "13" or "14" or "16" or "26" or "36" or "44" or "45" or "49" or "51" or "52" or "55" or "56" or "57" or "59" or "76")
        {
            value.AddError("Код операции не может быть использован для РВ");
            return false;
        }
        return true;
    }

    #region IExcel
    public void ExcelGetRow(ExcelWorksheet worksheet, int Row) 
    {
        base.ExcelGetRow(worksheet, Row);
        PassportNumber_DB = Convert.ToString(worksheet.Cells[Row, 4].Value);
        NameIOU_DB = Convert.ToString(worksheet.Cells[Row, 5].Value);
        FactoryNumber_DB = Convert.ToString(worksheet.Cells[Row, 6].Value);
        Mass_DB = Convert.ToString(worksheet.Cells[Row, 7].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 7].Value), out var val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 7].Value);
        CreatorOKPO_DB = Convert.ToString(worksheet.Cells[Row, 8].Value);
        CreationDate_DB = Convert.ToString(worksheet.Cells[Row, 9].Value);
        SignedServicePeriod_DB = Convert.ToString(worksheet.Cells[Row, 10].Value).Equals("0") ? "-" : Convert.ToString(worksheet.Cells[Row, 10].Value);
        PropertyCode_DB = Convert.ToByte(worksheet.Cells[Row, 11].Value);
        Owner_DB = Convert.ToString(worksheet.Cells[Row, 12].Value);
        DocumentVid_DB = Convert.ToByte(worksheet.Cells[Row, 13].Value);
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[Row, 14].Value);
        DocumentDate_DB = Convert.ToString(worksheet.Cells[Row, 15].Value);
        ProviderOrRecieverOKPO_DB = Convert.ToString(worksheet.Cells[Row, 16].Value);
        TransporterOKPO_DB = Convert.ToString(worksheet.Cells[Row, 17].Value);
        PackName_DB = Convert.ToString(worksheet.Cells[Row, 18].Value);
        PackType_DB = Convert.ToString(worksheet.Cells[Row, 19].Value);
        PackNumber_DB = Convert.ToString(worksheet.Cells[Row, 20].Value);
    }
    public int ExcelRow(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true, string SumNumber = "")
    {
        var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
        Column += Transpon == true ? cnt : 0;
        Row += Transpon == false ? cnt : 0;

        worksheet.Cells[Row + (!Transpon ? 0 : 0), Column + (Transpon ? 0 : 0)].Value = PassportNumber_DB;
        worksheet.Cells[Row + (!Transpon ? 1 : 0), Column + (Transpon ? 1 : 0)].Value = NameIOU_DB;
        worksheet.Cells[Row + (!Transpon ? 2 : 0), Column + (Transpon ? 2 : 0)].Value = FactoryNumber_DB;
        worksheet.Cells[Row + (!Transpon ? 3 : 0), Column + (Transpon ? 3 : 0)].Value = string.IsNullOrEmpty(Mass_DB) || Mass_DB == "-" ? 0  : double.TryParse(Mass_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out var val) ? val : Mass_DB;
        worksheet.Cells[Row + (!Transpon ? 4 : 0), Column + (Transpon ? 4 : 0)].Value = CreatorOKPO_DB;
        worksheet.Cells[Row + (!Transpon ? 5 : 0), Column + (Transpon ? 5 : 0)].Value = CreationDate_DB;
        worksheet.Cells[Row + (!Transpon ? 6 : 0), Column + (Transpon ? 6 : 0)].Value = string.IsNullOrEmpty(SignedServicePeriod_DB) || SignedServicePeriod_DB == "-" ? 0  : int.TryParse(SignedServicePeriod_DB.Replace("(", "").Replace(")", "").Replace(".", ","), out var valInt) ? valInt : SignedServicePeriod_DB;
        worksheet.Cells[Row + (!Transpon ? 7 : 0), Column + (Transpon ? 7 : 0)].Value = PropertyCode_DB;
        worksheet.Cells[Row + (!Transpon ? 8 : 0), Column + (Transpon ? 8 : 0)].Value = Owner_DB;
        worksheet.Cells[Row + (!Transpon ? 9 : 0), Column + (Transpon ? 9 : 0)].Value = DocumentVid_DB;
        worksheet.Cells[Row + (!Transpon ? 10 : 0), Column + (Transpon ? 10 : 0)].Value = DocumentNumber_DB;
        worksheet.Cells[Row + (!Transpon ? 11 : 0), Column + (Transpon ? 11 : 0)].Value = DocumentDate_DB;
        worksheet.Cells[Row + (!Transpon ? 12 : 0), Column + (Transpon ? 12 : 0)].Value = ProviderOrRecieverOKPO_DB;
        worksheet.Cells[Row + (!Transpon ? 13 : 0), Column + (Transpon ? 13 : 0)].Value = TransporterOKPO_DB;
        worksheet.Cells[Row + (!Transpon ? 14 : 0), Column + (Transpon ? 14 : 0)].Value = PackName_DB;
        worksheet.Cells[Row + (!Transpon ? 15 : 0), Column + (Transpon ? 15 : 0)].Value = PackType_DB;
        worksheet.Cells[Row + (!Transpon ? 16 : 0), Column + (Transpon ? 16 : 0)].Value = PackNumber_DB;

        return 17;
    }
    public static int ExcelHeader(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true)
    {
        var cnt = Form1.ExcelHeader(worksheet, Row, Column, Transpon);
        Column += +(Transpon == true ? cnt : 0);
        Row += Transpon == false ? cnt : 0;

        worksheet.Cells[Row + (!Transpon ? 0 : 0), Column + (Transpon ? 0 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(PassportNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 1 : 0), Column + (Transpon ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(NameIOU)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 2 : 0), Column + (Transpon ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(FactoryNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 3 : 0), Column + (Transpon ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(Mass)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 4 : 0), Column + (Transpon ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(CreatorOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 5 : 0), Column + (Transpon ? 5 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(CreationDate)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 6 : 0), Column + (Transpon ? 6 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(SignedServicePeriod)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 7 : 0), Column + (Transpon ? 7 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(PropertyCode)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 8 : 0), Column + (Transpon ? 8 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(Owner)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 9 : 0), Column + (Transpon ? 9 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(DocumentVid)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 10 : 0), Column + (Transpon ? 10 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(DocumentNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 11 : 0), Column + (Transpon ? 11 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(DocumentDate)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 12 : 0), Column + (Transpon ? 12 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(ProviderOrRecieverOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 13 : 0), Column + (Transpon ? 13 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(TransporterOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 14 : 0), Column + (Transpon ? 14 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(PackName)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 15 : 0), Column + (Transpon ? 15 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(PackType)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 16 : 0), Column + (Transpon ? 16 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty(nameof(PackNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        return 17;
    }
    #endregion

    #region IDataGridColumn
    public override DataGridColumns GetColumnStructure(string param)
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
        var PassportNumberR = ((FormPropertyAttribute)typeof(Form12).GetProperty(nameof(PassportNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        PassportNumberR.SetSizeColToAllLevels(125);
        PassportNumberR.Binding = nameof(PassportNumber);
        NumberInOrderR += PassportNumberR;
        #endregion

        #region NameIOU (5)
        var NameIOUR = ((FormPropertyAttribute)typeof(Form12).GetProperty(nameof(NameIOU)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        NameIOUR.SetSizeColToAllLevels(110);
        NameIOUR.Binding = nameof(NameIOU);
        NumberInOrderR += NameIOUR;
        #endregion

        #region FactoryNumber (6)
        var FactoryNumberR = ((FormPropertyAttribute)typeof(Form12).GetProperty(nameof(FactoryNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        FactoryNumberR.SetSizeColToAllLevels(80);
        FactoryNumberR.Binding = nameof(FactoryNumber);
        NumberInOrderR += FactoryNumberR;
        #endregion

        #region Mass (7)
        var MassR = ((FormPropertyAttribute)typeof(Form12).GetProperty(nameof(Mass)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        MassR.SetSizeColToAllLevels(125);
        MassR.Binding = nameof(Mass);
        NumberInOrderR += MassR;
        #endregion

        #region CreatorOKPO (8)
        var CreatorOKPOR = ((FormPropertyAttribute)typeof(Form12).GetProperty(nameof(CreatorOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        CreatorOKPOR.SetSizeColToAllLevels(90);
        CreatorOKPOR.Binding = nameof(CreatorOKPO);
        NumberInOrderR += CreatorOKPOR;
        #endregion

        #region CreationDate (9)
        var CreationDateR = ((FormPropertyAttribute)typeof(Form12).GetProperty(nameof(CreationDate)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        CreationDateR.SetSizeColToAllLevels(90);
        CreationDateR.Binding = nameof(CreationDate);
        NumberInOrderR += CreationDateR;
        #endregion

        #region SignedServicePeriod (10)
        var SignedServicePeriodR = ((FormPropertyAttribute)typeof(Form12).GetProperty(nameof(SignedServicePeriod)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        SignedServicePeriodR.SetSizeColToAllLevels(80);
        SignedServicePeriodR.Binding = nameof(SignedServicePeriod);
        NumberInOrderR += SignedServicePeriodR;
        #endregion

        #region PropertyCode (11)
        var PropertyCodeR = ((FormPropertyAttribute)typeof(Form12).GetProperty(nameof(PropertyCode)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        PropertyCodeR.SetSizeColToAllLevels(100);
        PropertyCodeR.Binding = nameof(PropertyCode);
        NumberInOrderR += PropertyCodeR;
        #endregion

        #region Owner (12)
        var OwnerR = ((FormPropertyAttribute)typeof(Form12).GetProperty(nameof(Owner)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        OwnerR.SetSizeColToAllLevels(110);
        OwnerR.Binding = nameof(Owner);
        NumberInOrderR += OwnerR;
        #endregion

        #region DocumentVid (13)
        var DocumentVidR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentVid)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        DocumentVidR.SetSizeColToAllLevels(60);
        DocumentVidR.Binding = nameof(DocumentVid);
        NumberInOrderR += DocumentVidR;
        #endregion

        #region DocumentNumber (14)
        var DocumentNumberR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        DocumentNumberR.SetSizeColToAllLevels(80);
        DocumentNumberR.Binding = nameof(DocumentNumber);
        NumberInOrderR += DocumentNumberR;
        #endregion

        #region DocumentDate (15)
        var DocumentDateR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentDate)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        DocumentDateR.SetSizeColToAllLevels(80);
        DocumentDateR.Binding = nameof(DocumentDate);
        NumberInOrderR += DocumentDateR;
        #endregion

        #region ProviderOrReciverOKPO (16)
        var ProviderOrRecieverOKPOR = ((FormPropertyAttribute)typeof(Form12).GetProperty(nameof(ProviderOrRecieverOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        ProviderOrRecieverOKPOR.SetSizeColToAllLevels(100);
        ProviderOrRecieverOKPOR.Binding = nameof(ProviderOrRecieverOKPO);
        NumberInOrderR += ProviderOrRecieverOKPOR;
        #endregion

        #region TransporterOKPO (17)
        var TransporterOKPOR = ((FormPropertyAttribute)typeof(Form12).GetProperty(nameof(TransporterOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        TransporterOKPOR.SetSizeColToAllLevels(130);
        TransporterOKPOR.Binding = nameof(TransporterOKPO);
        NumberInOrderR += TransporterOKPOR;
        #endregion

        #region PackName (18)
        var PackNameR = ((FormPropertyAttribute)typeof(Form12).GetProperty(nameof(PackName)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        PackNameR.SetSizeColToAllLevels(130);
        PackNameR.Binding = nameof(PackName);
        NumberInOrderR += PackNameR;
        #endregion

        #region PackType (19)
        var PackTypeR = ((FormPropertyAttribute)typeof(Form12).GetProperty(nameof(PackType)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        PackTypeR.SetSizeColToAllLevels(80);
        PackTypeR.Binding = nameof(PackType);
        NumberInOrderR += PackTypeR;
        #endregion

        #region PackNumber (20)
        var PackNumberR = ((FormPropertyAttribute)typeof(Form12).GetProperty(nameof(PackNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        PackNumberR.SetSizeColToAllLevels(80);
        PackNumberR.Binding = nameof(PackNumber);
        NumberInOrderR += PackNumberR;
        #endregion

        return NumberInOrderR;
    }
    #endregion
}