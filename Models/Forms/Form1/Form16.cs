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
[Form_Class("Форма 1.6: Сведения о некондиционированных РАО")]
[Table (name: "form_16")]
public partial class Form16 : Form1
{
    #region Contructor
    
    public Form16()
    {
        FormNum.Value = "1.6";
        Validate_all();
    }

    #endregion

    #region Validation
    
    private void Validate_all()
    {
        CodeRAO_Validation(CodeRAO);
        PackName_Validation(PackName);
        PackNumber_Validation(PackNumber);
        PackType_Validation(PackType);
        Volume_Validation(Volume);
        Mass_Validation(Mass);
        ActivityMeasurementDate_Validation(ActivityMeasurementDate);
        ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
        TransporterOKPO_Validation(TransporterOKPO);
        TritiumActivity_Validation(TritiumActivity);
        BetaGammaActivity_Validation(BetaGammaActivity);
        AlphaActivity_Validation(AlphaActivity);
        TransuraniumActivity_Validation(TransuraniumActivity);
        MainRadionuclids_Validation(MainRadionuclids);
        QuantityOZIII_Validation(QuantityOZIII);
        RefineOrSortRAOCode_Validation(RefineOrSortRAOCode);
        Subsidy_Validation(Subsidy);
        FcpNumber_Validation(FcpNumber);
        StatusRAO_Validation(StatusRAO);
        StoragePlaceName_Validation(StoragePlaceName);
        StoragePlaceCode_Validation(StoragePlaceCode);
    }

    [FormProperty(true, "Форма")]
    public override bool Object_Validation()
    {
        return !(CodeRAO.HasErrors ||
                 PackName.HasErrors ||
                 PackNumber.HasErrors ||
                 PackType.HasErrors ||
                 Volume.HasErrors ||
                 Mass.HasErrors ||
                 ActivityMeasurementDate.HasErrors ||
                 ProviderOrRecieverOKPO.HasErrors ||
                 TransporterOKPO.HasErrors ||
                 TritiumActivity.HasErrors ||
                 BetaGammaActivity.HasErrors ||
                 AlphaActivity.HasErrors ||
                 TransuraniumActivity.HasErrors ||
                 MainRadionuclids.HasErrors ||
                 QuantityOZIII.HasErrors ||
                 RefineOrSortRAOCode.HasErrors ||
                 Subsidy.HasErrors ||
                 FcpNumber.HasErrors ||
                 StatusRAO.HasErrors ||
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
        if (!TwoNumRegex().IsMatch(value.Value)
            || !byte.TryParse(value.Value, out var byteValue)
            || byteValue is (15 or 17 or 46 or 47 or 53 or 54 or 58 or 61 or 62 or 65 or 66 or 67 or 81 or 82 or 83 or 85 or 86 or 87))
        {
            value.AddError("Код операции не может быть использован в форме 1.6");
            return false;
        }

        return true;
    }

    #endregion

    #region Properties

    #region CodeRAO (4)

    public string CodeRAO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-4", "Код РАО", "4")]
    public RamAccess<string> CodeRAO
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(CodeRAO), out var value))
            {
                ((RamAccess<string>)value).Value = CodeRAO_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(CodeRAO_Validation, CodeRAO_DB);
            rm.PropertyChanged += CodeRAO_ValueChanged;
            Dictionary.Add(nameof(CodeRAO), rm);
            return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
        }
        set
        {
            CodeRAO_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void CodeRAO_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim().ToLower().Replace("х", "x");
        CodeRAO_DB = tmp;
    }

    private static bool CodeRAO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var tmp = value.Value.ToLower();
        tmp = tmp.Replace("х", "x");
        if (!CodeRaoRegex().IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmp.Length != 11) return true;
        if (!CodeRaoRegex1().IsMatch(tmp[..1]))
        {
            value.AddError($"Недопустимое агрегатное состояние - {tmp[..1]}");
        }
        if (!CodeRaoRegex2().IsMatch(tmp.AsSpan(1, 1)))
        {
            value.AddError($"Недопустимое категория РАО - {tmp.Substring(1, 1)}");
        }
        if (!CodeRaoRegex3().IsMatch(tmp.AsSpan(2, 1)))
        {
            value.AddError($"Недопустимый радионуклидный состав РАО - {tmp.Substring(2, 1)}");
        }
        if (!CodeRaoRegex4().IsMatch(tmp.AsSpan(3, 1)))
        {
            value.AddError($"Недопустимое содержание ядерных материалов - {tmp.Substring(3, 1)}");
        }
        if (!CodeRaoRegex5().IsMatch(tmp.AsSpan(4, 1)))
        {
            value.AddError($"Недопустимый период полураспада - {tmp.Substring(4, 1)}");
        }
        if (!CodeRaoRegex6().IsMatch(tmp.AsSpan(5, 1)))
        {
            value.AddError($"Недопустимый период потенциальной опасности РАО - {tmp.Substring(5, 1)}");
        }
        if (!CodeRaoRegex7().IsMatch(tmp.AsSpan(6, 1)))
        {
            value.AddError($"Недопустимый способ переработки - {tmp.Substring(6, 1)}");
        }
        if (!CodeRaoRegex8().IsMatch(tmp.AsSpan(7, 1)))
        {
            value.AddError($"Недопустимый класс РАО - {tmp.Substring(7, 1)}");
        }
        if (!CodeRaoRegex9().IsMatch(tmp.AsSpan(8, 2)))
        {
            value.AddError($"Недопустимый код типа РАО - {tmp.Substring(8, 2)}");
        }
        if (!CodeRaoRegex10().IsMatch(tmp.AsSpan(10, 1)))
        {
            value.AddError($"Недопустимая горючесть - {tmp.Substring(10, 1)}");
        }
        return !value.HasErrors;
    }

    #endregion

    #region StatusRAO (5)

    public string StatusRAO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-5", "Статус РАО", "5")]
    public RamAccess<string> StatusRAO  //1 digit or OKPO.
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(StatusRAO), out var value))
            {
                ((RamAccess<string>)value).Value = StatusRAO_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(StatusRAO_Validation, StatusRAO_DB);
            rm.PropertyChanged += StatusRAO_ValueChanged;
            Dictionary.Add(nameof(StatusRAO), rm);
            return (RamAccess<string>)Dictionary[nameof(StatusRAO)];
        }
        set
        {
            StatusRAO_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void StatusRAO_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        StatusRAO_DB = tmp;
    }

    private static bool StatusRAO_Validation(RamAccess<string> value)//TODO
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

        if (value.Value.Length == 1
            && int.TryParse(value.Value, out var intValue)
            && intValue is > 0 and not 5 and not 8
            || OkpoRegex().IsMatch(value.Value))
        {
            return true;
        }
        value.AddError("Недопустимое значение");
        return false;
    }

    #endregion

    #region Volume (6)

    public string Volume_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Количество", "объем без упаковки, куб. м", "6")]
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
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        tmp = tmp
            .Trim()
            .ToLower()
            .Replace('.', ',')
            .Replace('е', 'e');
        if (tmp.Equals("-"))
        {
            Volume_DB = tmp;
            return;
        }
        if (double.TryParse(tmp, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            tmp = $"{doubleValue:0.######################################################e+00}";
        }
        Volume_DB = tmp;
    }

    private static bool Volume_Validation(RamAccess<string> value)//TODO
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

    #region Mass (7)

    public string Mass_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Количество", "масса без упаковки (нетто), т", "7")]
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
        tmp = tmp
            .Trim()
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

    private static bool Mass_Validation(RamAccess<string> value)//TODO
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

    #region QuantityOZIII (8)

    public string QuantityOZIII_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "null-8", "Количество ОЗИИИ, шт", "8")]
    public RamAccess<string> QuantityOZIII
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(QuantityOZIII), out var value))
            {
                ((RamAccess<string>)value).Value = QuantityOZIII_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(QuantityOZIII_Validation, QuantityOZIII_DB);
            rm.PropertyChanged += QuantityOZIII_ValueChanged;
            Dictionary.Add(nameof(QuantityOZIII), rm);
            return (RamAccess<string>)Dictionary[nameof(QuantityOZIII)];
        }
        set
        {
            QuantityOZIII_DB = value.Value;
            OnPropertyChanged();
        }
    }// positive int.


    private void QuantityOZIII_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        QuantityOZIII_DB = tmp;
    }

    private static bool QuantityOZIII_Validation(RamAccess<string> value)//Ready
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
        if (!int.TryParse(value.Value, out var intValue))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (intValue <= 0)
        {
            value.AddError("Число должно быть больше нуля");
            return false;
        }
        return true;
    }

    #endregion

    #region MainRadionuclids (9)

    public string MainRadionuclids_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-9", "Основные радионуклиды", "9")]
    public RamAccess<string> MainRadionuclids
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MainRadionuclids), out var value))
            {
                ((RamAccess<string>)value).Value = MainRadionuclids_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(MainRadionuclids_Validation, MainRadionuclids_DB);
            rm.PropertyChanged += MainRadionuclids_ValueChanged;
            Dictionary.Add(nameof(MainRadionuclids), rm);
            return (RamAccess<string>)Dictionary[nameof(MainRadionuclids)];
        }
        set
        {
            MainRadionuclids_DB = value.Value;
            OnPropertyChanged();
        }
    }//If change this change validation

    private void MainRadionuclids_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        MainRadionuclids_DB = tmp;
    }

    private static bool MainRadionuclids_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
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
        foreach (var nuclid in nuclids)
        {
            if (!Spravochniks.SprRadionuclids
                    .Where(item => nuclid == item.Item1)
                    .Select(item => item.Item1)
                    .Any())
            {
                flag = false;
            }
        }
        if (!flag)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region TritiumActivity (10)

    public string TritiumActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Суммарная активность, Бк", "тритий", "10")]
    public RamAccess<string> TritiumActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(TritiumActivity), out var value))
            {
                ((RamAccess<string>)value).Value = TritiumActivity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(TritiumActivity_Validation, TritiumActivity_DB);
            rm.PropertyChanged += TritiumActivity_ValueChanged;
            Dictionary.Add(nameof(TritiumActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(TritiumActivity)];
        }
        set
        {
            TritiumActivity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TritiumActivity_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        tmp = tmp
            .Trim()
            .ToLower()
            .Replace('.', ',')
            .Replace('е', 'e');
        if (tmp.Equals("-"))
        {
            TritiumActivity_DB = tmp;
            return;
        }
        if (double.TryParse(tmp, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            tmp = $"{doubleValue:0.######################################################e+00}";
        }
        TritiumActivity_DB = tmp;
    }

    private static bool TritiumActivity_Validation(RamAccess<string> value)//TODO
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

    #region BetaGammaActivity (11)

    public string BetaGammaActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Суммарная активность, Бк", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "11")]
    public RamAccess<string> BetaGammaActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(BetaGammaActivity), out var value))
            {
                ((RamAccess<string>)value).Value = BetaGammaActivity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(BetaGammaActivity_Validation, BetaGammaActivity_DB);
            rm.PropertyChanged += BetaGammaActivity_ValueChanged;
            Dictionary.Add(nameof(BetaGammaActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(BetaGammaActivity)];
        }
        set
        {
            BetaGammaActivity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void BetaGammaActivity_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        tmp = tmp
            .Trim()
            .ToLower()
            .Replace('.', ',')
            .Replace('е', 'e');
        if (tmp.Equals("-"))
        {
            BetaGammaActivity_DB = tmp;
            return;
        }
        if (double.TryParse(tmp, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            tmp = $"{doubleValue:0.######################################################e+00}";
        }
        BetaGammaActivity_DB = tmp;
    }

    private static bool BetaGammaActivity_Validation(RamAccess<string> value)//TODO
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

    #region AlphaActivity (12)

    public string AlphaActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Суммарная активность, Бк", "альфа-излучающие радионуклиды (исключая трансурановые)", "12")]
    public RamAccess<string> AlphaActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(AlphaActivity), out var value))
            {
                ((RamAccess<string>)value).Value = AlphaActivity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(AlphaActivity_Validation, AlphaActivity_DB);
            rm.PropertyChanged += AlphaActivity_ValueChanged;
            Dictionary.Add(nameof(AlphaActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(AlphaActivity)];
        }
        set
        {
            AlphaActivity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void AlphaActivity_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        tmp = tmp
            .Trim()
            .ToLower()
            .Replace('.', ',')
            .Replace('е', 'e');
        if (tmp.Equals("-"))
        {
            AlphaActivity_DB = tmp;
            return;
        }
        if (double.TryParse(tmp, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            tmp = $"{doubleValue:0.######################################################e+00}";
        }
        AlphaActivity_DB = tmp;
    }

    private static bool AlphaActivity_Validation(RamAccess<string> value)//TODO
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

    #region TransuraniumActivity (13)

    public string TransuraniumActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Суммарная активность, Бк", "трансурановые радионуклиды", "13")]
    public RamAccess<string> TransuraniumActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(TransuraniumActivity), out var value))
            {
                ((RamAccess<string>)value).Value = TransuraniumActivity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(TransuraniumActivity_Validation, TransuraniumActivity_DB);
            rm.PropertyChanged += TransuraniumActivity_ValueChanged;
            Dictionary.Add(nameof(TransuraniumActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(TransuraniumActivity)];
        }
        set
        {
            TransuraniumActivity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TransuraniumActivity_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value ?? string.Empty;
        tmp = tmp
            .Trim()
            .ToLower()
            .Replace('.', ',')
            .Replace('е', 'e');
        if (tmp.Equals("-"))
        {
            TransuraniumActivity_DB = tmp;
            return;
        }
        if (double.TryParse(tmp, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            tmp = $"{doubleValue:0.######################################################e+00}";
        }
        TransuraniumActivity_DB = tmp;
    }

    private static bool TransuraniumActivity_Validation(RamAccess<string> value)//TODO
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

    #region ActivityMeasurementDate (14)

    public string ActivityMeasurementDate_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-14", "Дата измерения активности", "14")]

    public virtual RamAccess<string> ActivityMeasurementDate
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
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        ActivityMeasurementDate_DB = DateOnly.TryParse(tmp, CultureInfo.CreateSpecificCulture("ru-RU"), out var date)
            ? date.ToShortDateString()
            : tmp;
    }

    private static bool ActivityMeasurementDate_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var tmp = value.Value.Trim();
        if (!DateOnly.TryParse(tmp, CultureInfo.CreateSpecificCulture("ru-RU"), out _))
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
    [FormProperty(true, "ОКПО", "поставщика или получателя", "18")]
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

    [NotMapped]
    public string OKPOofFormFiller { get; set; } = null;

    private void ProviderOrRecieverOKPO_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        if (Spravochniks.OKSM.Contains(tmp.ToUpper()))
        {
            tmp = tmp.ToUpper();
        }

        ProviderOrRecieverOKPO_DB = tmp;
        //try
        //{
        //    var a = int.Parse(OperationCode.Value) >= 10 && int.Parse(OperationCode.Value) <= 14;
        //    var b = int.Parse(OperationCode.Value) >= 41 && int.Parse(OperationCode.Value) <= 45;
        //    var c = int.Parse(OperationCode.Value) >= 71 && int.Parse(OperationCode.Value) <= 73;
        //    var e = int.Parse(OperationCode.Value) >= 55 && int.Parse(OperationCode.Value) <= 57;
        //    var d = OperationCode.Value is "01" or "16" or "18" or "48" or "49" or "51" or "52" or "59" or "68" or "75" or "76";
        //    if (a || b || c || d || e)
        //    {
        //        //ProviderOrRecieverOKPO_DB = OKPOofFormFiller;
        //    }
        //}
        //catch
        //{
        //    // ignored
        //}
    }

    private static bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("Минобороны") || Spravochniks.OKSM.Contains(value.Value.ToUpper()))
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

    #region TransporterOKPO (19)

    public string TransporterOKPO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "ОКПО", "перевозчика", "19")]
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
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
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
        var tmp = value.Value.Trim();
        if (tmp.Equals("-") || tmp.Equals("Минобороны"))
        {
            return true;
        }
        if (tmp.Equals("прим."))
        {
            //if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
            //    value.AddError( "Заполните примечание");
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

    #region StoragePlaceName (20)

    public string StoragePlaceName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Пункт хранения", "наименование", "20")]
    public RamAccess<string> StoragePlaceName
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(StoragePlaceName), out var value))
            {
                ((RamAccess<string>)value).Value = StoragePlaceName_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(StoragePlaceName_Validation, StoragePlaceName_DB);
            rm.PropertyChanged += StoragePlaceName_ValueChanged;
            Dictionary.Add(nameof(StoragePlaceName), rm);
            return (RamAccess<string>)Dictionary[nameof(StoragePlaceName)];
        }
        set
        {
            StoragePlaceName_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void StoragePlaceName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        StoragePlaceName_DB = tmp;
    }

    private static bool StoragePlaceName_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        //var a = new List<string>();//here binds spr
        //if (a.Contains(value.Value))
        //    return true;
        //value.AddError("Недопустимое значение");
        //return false;
        return true;
    }

    #endregion

    #region StoragePlaceCode (21)

    public string StoragePlaceCode_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Пункт хранения", "код", "21")]
    public RamAccess<string> StoragePlaceCode //8 digits code or - .
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(StoragePlaceCode), out var value))
            {
                ((RamAccess<string>)value).Value = StoragePlaceCode_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(StoragePlaceCode_Validation, StoragePlaceCode_DB);
            rm.PropertyChanged += StoragePlaceCode_ValueChanged;
            Dictionary.Add(nameof(StoragePlaceCode), rm);
            return (RamAccess<string>)Dictionary[nameof(StoragePlaceCode)];
        }
        set
        {
            StoragePlaceCode_DB = value.Value;
            OnPropertyChanged();
        }
    }//if change this change validation

    private void StoragePlaceCode_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        StoragePlaceCode_DB = tmp;
    }

    private static bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        //var lst = new List<string>();//HERE binds spr
        //if (!lst.Contains(value.Value))
        //{
        //    value.AddError("Недопустимое значение");
        //    return false;
        //}
        //return true;
        if (value.Value == "-") return true;
        if (!StoragePlaceCodeRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); 
            return false;
        }
        var tmp = value.Value;
        if (tmp.Length != 8) return true;
        if (!StoragePlaceCodeRegex1().IsMatch(tmp[..1]))
        {
            value.AddError($"Недопустимый вид пункта - {tmp[..1]}");
        }
        if (!StoragePlaceCodeRegex2().IsMatch(tmp.AsSpan(1, 1)))
        {
            value.AddError($"Недопустимое состояние пункта - {tmp.Substring(1, 1)}");
        }
        if (!StoragePlaceCodeRegex3().IsMatch(tmp.AsSpan(2, 1)))
        {
            value.AddError($"Недопустимая изоляция от окружающей среды - {tmp.Substring(2, 1)}");
        }
        if (!StoragePlaceCodeRegex4().IsMatch(tmp.AsSpan(3, 1)))
        {
            value.AddError($"Недопустимая зона нахождения пункта - {tmp.Substring(3, 1)}");
        }
        if (!StoragePlaceCodeRegex5().IsMatch(tmp.AsSpan(4, 1)))
        {
            value.AddError($"Недопустимое значение пункта - {tmp.Substring(4, 1)}");
        }
        if (!StoragePlaceCodeRegex6().IsMatch(tmp.AsSpan(5, 1)))
        {
            value.AddError($"Недопустимое размещение пункта хранения относительно поверхности земли - {tmp.Substring(5, 1)}");
        }
        if (!StoragePlaceCodeRegex7().IsMatch(tmp.AsSpan(6, 2)))
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
    public RamAccess<string> RefineOrSortRAOCode //2 digits code or empty.
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(RefineOrSortRAOCode), out var value))
            {
                ((RamAccess<string>)value).Value = RefineOrSortRAOCode_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(RefineOrSortRAOCode_Validation, RefineOrSortRAOCode_DB);
            rm.PropertyChanged += RefineOrSortRAOCode_ValueChanged;
            Dictionary.Add(nameof(RefineOrSortRAOCode), rm);
            return (RamAccess<string>)Dictionary[nameof(RefineOrSortRAOCode)];
        }
        set
        {
            RefineOrSortRAOCode_DB = value.Value;
            OnPropertyChanged();
        }
    }//If change this change validation

    private void RefineOrSortRAOCode_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        RefineOrSortRAOCode_DB = tmp;
    }

    private static bool RefineOrSortRAOCode_Validation(RamAccess<string> value)//TODO
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

    #region PackName (23)

    public string PackName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "УКТ, упаковка или иная учетная единица", "наименование", "23")]
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
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
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
            //    value.AddError( "Заполните примечание");
            return true;
        }
        return true;
    }

    #endregion

    #region PackType (24)

    public string PackType_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "УКТ, упаковка или иная учетная единица", "тип", "24")]
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
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
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
            //    value.AddError( "Заполните примечание");//to do note handling
            return true;
        }
        return true;
    }

    #endregion

    #region PackNumber (25)

    public string PackNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "УКТ, упаковка или иная учетная единица", "номер упаковки", "25")]
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
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
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
            //    value.AddError( "Заполните примечание");
            return true;
        }
        return true;
    }

    #endregion

    #region Subsidy (26)

    public string Subsidy_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-26", "Субсидия, %", "26")]
    public RamAccess<string> Subsidy // 0<number<=100 or empty.
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Subsidy), out var value))
            {
                ((RamAccess<string>)value).Value = Subsidy_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Subsidy_Validation, Subsidy_DB);
            rm.PropertyChanged += Subsidy_ValueChanged;
            Dictionary.Add(nameof(Subsidy), rm);
            return (RamAccess<string>)Dictionary[nameof(Subsidy)];
        }
        set
        {
            Subsidy_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void Subsidy_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        Subsidy_DB = tmp;
    }

    private static bool Subsidy_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value) || value.Value.Equals("-"))
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

    #region FcpNumber (27)

    public string FcpNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-27", "Номер мероприятия ФЦП", "27")]
    public RamAccess<string> FcpNumber
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(FcpNumber), out var value))
            {
                ((RamAccess<string>)value).Value = FcpNumber_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(FcpNumber_Validation, FcpNumber_DB);
            rm.PropertyChanged += FcpNumber_ValueChanged;
            Dictionary.Add(nameof(FcpNumber), rm);
            return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
        }
        set
        {
            FcpNumber_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void FcpNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = (((RamAccess<string>)value).Value ?? string.Empty).Trim();
        FcpNumber_DB = tmp;
    }

    private static bool FcpNumber_Validation(RamAccess<string> value)//TODO
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
        CodeRAO_DB = Convert.ToString(worksheet.Cells[row, 4].Value);
        StatusRAO_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        Volume_DB = ConvertFromExcelDouble(worksheet.Cells[row, 6].Value);
        Mass_DB = ConvertFromExcelDouble(worksheet.Cells[row, 7].Value);
        QuantityOZIII_DB = ConvertFromExcelInt(worksheet.Cells[row, 8].Value);
        MainRadionuclids_DB = Convert.ToString(worksheet.Cells[row, 9].Value);
        TritiumActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 10].Value);
        BetaGammaActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 11].Value);
        AlphaActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 12].Value);
        TransuraniumActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 13].Value);
        ActivityMeasurementDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 14].Text);
        DocumentVid_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 15].Value), out var byteValue) ? byteValue : null;
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[row, 16].Value);
        DocumentDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 17].Text);
        ProviderOrRecieverOKPO_DB = Convert.ToString(worksheet.Cells[row, 18].Value);
        TransporterOKPO_DB = Convert.ToString(worksheet.Cells[row, 19].Value);
        StoragePlaceName_DB = Convert.ToString(worksheet.Cells[row, 20].Value);
        StoragePlaceCode_DB = Convert.ToString(worksheet.Cells[row, 21].Value);
        RefineOrSortRAOCode_DB = Convert.ToString(worksheet.Cells[row, 22].Value);
        PackName_DB = Convert.ToString(worksheet.Cells[row, 23].Value);
        PackType_DB = Convert.ToString(worksheet.Cells[row, 24].Value);
        PackNumber_DB = Convert.ToString(worksheet.Cells[row, 25].Value);
        Subsidy_DB = Convert.ToString(worksheet.Cells[row, 26].Value);
        FcpNumber_DB = Convert.ToString(worksheet.Cells[row, 27].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ConvertToExcelString(CodeRAO_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(StatusRAO_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelDouble(Volume_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelDouble(Mass_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelInt(QuantityOZIII_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelString(MainRadionuclids_DB);
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDouble(TritiumActivity_DB);
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelDouble(BetaGammaActivity_DB);
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ConvertToExcelDouble(AlphaActivity_DB);
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ConvertToExcelDouble(TransuraniumActivity_DB);
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ConvertToExcelDate(ActivityMeasurementDate_DB, worksheet, row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0));
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = DocumentVid_DB is null ? "-" : DocumentVid_DB;
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ConvertToExcelString(DocumentNumber_DB);
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ConvertToExcelDate(DocumentDate_DB, worksheet, row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0));
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ConvertToExcelString(ProviderOrRecieverOKPO_DB);
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ConvertToExcelString(TransporterOKPO_DB);
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ConvertToExcelString(StoragePlaceName_DB);
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ConvertToExcelString(StoragePlaceCode_DB);
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ConvertToExcelString(RefineOrSortRAOCode_DB);
        worksheet.Cells[row + (!transpose ? 19 : 0), column + (transpose ? 19 : 0)].Value = ConvertToExcelString(PackName_DB);
        worksheet.Cells[row + (!transpose ? 20 : 0), column + (transpose ? 20 : 0)].Value = ConvertToExcelString(PackType_DB);
        worksheet.Cells[row + (!transpose ? 21 : 0), column + (transpose ? 21 : 0)].Value = ConvertToExcelString(PackNumber_DB);
        worksheet.Cells[row + (!transpose ? 22 : 0), column + (transpose ? 22 : 0)].Value = ConvertToExcelString(Subsidy_DB);
        worksheet.Cells[row + (!transpose ? 23 : 0), column + (transpose ? 23 : 0)].Value = ConvertToExcelString(FcpNumber_DB);

        return 24;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form1.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(CodeRAO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(StatusRAO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(Volume))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(Mass))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(QuantityOZIII))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(MainRadionuclids))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(TritiumActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(BetaGammaActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(AlphaActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(TransuraniumActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(ActivityMeasurementDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(DocumentVid))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(DocumentNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(DocumentDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 14 : 0), column + (transpose ? 14 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(ProviderOrRecieverOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 15 : 0), column + (transpose ? 15 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(TransporterOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 16 : 0), column + (transpose ? 16 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(StoragePlaceName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 17 : 0), column + (transpose ? 17 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(StoragePlaceCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 18 : 0), column + (transpose ? 18 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(RefineOrSortRAOCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 19 : 0), column + (transpose ? 19 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(PackName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 20 : 0), column + (transpose ? 20 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(PackType))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 21 : 0), column + (transpose ? 21 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(PackNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 22 : 0), column + (transpose ? 22 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(Subsidy))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 23 : 0), column + (transpose ? 23 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form16,Models")?.GetProperty(nameof(FcpNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];

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

        #region CodeRAO (4)

        var codeRaoR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(CodeRAO))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (codeRaoR != null)
        {
            codeRaoR.SetSizeColToAllLevels(88);
            codeRaoR.Binding = nameof(CodeRAO);
            numberInOrderR += codeRaoR;
        }

        #endregion

        #region StatusRAO (5)

        var statusRaoR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(StatusRAO))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (statusRaoR != null)
        {
            statusRaoR.SetSizeColToAllLevels(88);
            statusRaoR.Binding = nameof(StatusRAO);
            numberInOrderR += statusRaoR;
        }

        #endregion

        #region Volume (6)

        var volumeR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(Volume))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (volumeR != null)
        {
            volumeR.SetSizeColToAllLevels(90);
            volumeR.Binding = nameof(Volume);
            numberInOrderR += volumeR;
        }

        #endregion

        #region Mass (7)

        var massR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(Mass))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (massR != null)
        {
            massR.SetSizeColToAllLevels(90);
            massR.Binding = nameof(Mass);
            numberInOrderR += massR;
        }

        #endregion

        #region QuantityOZIII (8)

        var quantityOziiiR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(QuantityOZIII))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        quantityOziiiR.SetSizeColToAllLevels(70);
        quantityOziiiR.Binding = nameof(QuantityOZIII);
        numberInOrderR += quantityOziiiR;

        #endregion

        #region MainRadionuclids (9)

        var mainRadionuclidsR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(MainRadionuclids))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        mainRadionuclidsR.SetSizeColToAllLevels(150);
        mainRadionuclidsR.Binding = nameof(MainRadionuclids);
        numberInOrderR += mainRadionuclidsR;

        #endregion

        #region TritiumActivity (10)

        var tritiumActivityR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(TritiumActivity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        tritiumActivityR.SetSizeColToAllLevels(163);
        tritiumActivityR.Binding = nameof(TritiumActivity);
        numberInOrderR += tritiumActivityR;

        #endregion

        #region BetaGammaActivity (11)

        var betaGammaActivityR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(BetaGammaActivity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        betaGammaActivityR.SetSizeColToAllLevels(175);
        betaGammaActivityR.Binding = nameof(BetaGammaActivity);
        numberInOrderR += betaGammaActivityR;

        #endregion

        #region AlphaActivity (12)

        var alphaActivityR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(AlphaActivity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        alphaActivityR.SetSizeColToAllLevels(185);
        alphaActivityR.Binding = nameof(AlphaActivity);
        numberInOrderR += alphaActivityR;

        #endregion

        #region TransuraniumActivity (13)

        var transuraniumActivityR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(TransuraniumActivity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        transuraniumActivityR.SetSizeColToAllLevels(200);
        transuraniumActivityR.Binding = nameof(TransuraniumActivity);
        numberInOrderR += transuraniumActivityR;

        #endregion

        #region ActivityMeasurementDate (14)

        var activityMeasurementDateR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(ActivityMeasurementDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        activityMeasurementDateR.SetSizeColToAllLevels(100);
        activityMeasurementDateR.Binding = nameof(ActivityMeasurementDate);
        numberInOrderR += activityMeasurementDateR;

        #endregion

        #region DocumentVid (15)

        var documentVidR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentVid))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentVidR.SetSizeColToAllLevels(88);
        documentVidR.Binding = nameof(DocumentVid);
        numberInOrderR += documentVidR;

        #endregion

        #region DocumentNumber (16)

        var documentNumberR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentNumberR.SetSizeColToAllLevels(103);
        documentNumberR.Binding = nameof(DocumentNumber);
        numberInOrderR += documentNumberR;

        #endregion

        #region DocumentDate (17)

        var documentDateR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentDateR.SetSizeColToAllLevels(88);
        documentDateR.Binding = nameof(DocumentDate);
        numberInOrderR += documentDateR;

        #endregion

        #region ProviderOrRecieverOKPO (18)

        var providerOrRecieverOkpoR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(ProviderOrRecieverOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        providerOrRecieverOkpoR.SetSizeColToAllLevels(100);
        providerOrRecieverOkpoR.Binding = nameof(ProviderOrRecieverOKPO);
        numberInOrderR += providerOrRecieverOkpoR;

        #endregion

        #region TransporterOKPO (19)

        var transporterOkpoR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(TransporterOKPO))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        transporterOkpoR.SetSizeColToAllLevels(163);
        transporterOkpoR.Binding = nameof(TransporterOKPO);
        numberInOrderR += transporterOkpoR;

        #endregion

        #region StoragePlaceName (20)

        var storagePlaceNameR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(StoragePlaceName))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        storagePlaceNameR.SetSizeColToAllLevels(103);
        storagePlaceNameR.Binding = nameof(StoragePlaceName);
        numberInOrderR += storagePlaceNameR;

        #endregion

        #region StoragePlaceCode (21)

        var storagePlaceCodeR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(StoragePlaceCode))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        storagePlaceCodeR.SetSizeColToAllLevels(88);
        storagePlaceCodeR.Binding = nameof(StoragePlaceCode);
        numberInOrderR += storagePlaceCodeR;

        #endregion

        #region RefineOrSortRAOCode (22)

        var refineOrSortRAOCodeR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(RefineOrSortRAOCode))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        refineOrSortRAOCodeR.SetSizeColToAllLevels(110);
        refineOrSortRAOCodeR.Binding = nameof(RefineOrSortRAOCode);
        numberInOrderR += refineOrSortRAOCodeR;

        #endregion

        #region PackName (23)

        var packNameR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(PackName))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packNameR.SetSizeColToAllLevels(163);
        packNameR.Binding = nameof(PackName);
        numberInOrderR += packNameR;

        #endregion

        #region PackType (24)

        var packTypeR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(PackType))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packTypeR.SetSizeColToAllLevels(88);
        packTypeR.Binding = nameof(PackType);
        numberInOrderR += packTypeR;

        #endregion

        #region PackNumber (25)

        var packNumberR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(PackNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        packNumberR.SetSizeColToAllLevels(163);
        packNumberR.Binding = nameof(PackNumber);
        numberInOrderR += packNumberR;

        #endregion

        #region Subsidy (26)

        var subsidyR = ((FormPropertyAttribute)typeof(Form16)
                .GetProperty(nameof(Subsidy))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        subsidyR.SetSizeColToAllLevels(88);
        subsidyR.Binding = nameof(Subsidy);
        numberInOrderR += subsidyR;

        #endregion

        #region FcpNumber (27)

        var fcpNumberR = ((FormPropertyAttribute)typeof(Form16)
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

    #region GeneratedRegex

    #region StoragePlaceCode
    
    [GeneratedRegex("^[0-9]{8}$")]
    private static partial Regex StoragePlaceCodeRegex();

    [GeneratedRegex("^[1-9]")]
    private static partial Regex StoragePlaceCodeRegex1();

    [GeneratedRegex("^[1-3]")]
    private static partial Regex StoragePlaceCodeRegex2();

    [GeneratedRegex("^[1-2]")]
    private static partial Regex StoragePlaceCodeRegex3();

    [GeneratedRegex("^[1-59]")]
    private static partial Regex StoragePlaceCodeRegex4();

    [GeneratedRegex("^[0-4]")]
    private static partial Regex StoragePlaceCodeRegex5();

    [GeneratedRegex("^[1-49]")]
    private static partial Regex StoragePlaceCodeRegex6();

    [GeneratedRegex("^[1]{1}[1-9]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1]{1}|^[4]{1}[1-49]{1}|^[5]{1}[1-69]{1}|^[6]{1}[1]{1}|^[7]{1}[1349]{1}|^[8]{1}[1-69]{1}|^[9]{1}[9]{1}")]
    private static partial Regex StoragePlaceCodeRegex7();

    #endregion

    #region CodeRao
    
    [GeneratedRegex("^[0-9x+]{11}$")]
    private static partial Regex CodeRaoRegex();

    [GeneratedRegex("^[1-3x+]")]
    private static partial Regex CodeRaoRegex1();

    [GeneratedRegex("^[0-49x+]")]
    private static partial Regex CodeRaoRegex2();

    [GeneratedRegex("^[0-6x+]")]
    private static partial Regex CodeRaoRegex3();

    [GeneratedRegex("^[12x+]")]
    private static partial Regex CodeRaoRegex4();

    [GeneratedRegex("^[12x+]")]
    private static partial Regex CodeRaoRegex5();

    [GeneratedRegex("^[0-3x+]")]
    private static partial Regex CodeRaoRegex6();

    [GeneratedRegex("^[0-49x+]")]
    private static partial Regex CodeRaoRegex7();

    [GeneratedRegex("^[0-79x+]")]
    private static partial Regex CodeRaoRegex8();

    [GeneratedRegex("^[1]{1}[1-9]{1}|^[0]{1}[1]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1-9]{1}|^[4]{1}[1-6]{1}|^[5]{1}[1-9]{1}|^[6]{1}[1-9]{1}|^[7]{1}[1-9]{1}|^[8]{1}[1-9]{1}|^[9]{1}[1-9]{1}")]
    private static partial Regex CodeRaoRegex9();

    [GeneratedRegex("^[12x+]")]
    private static partial Regex CodeRaoRegex10();

    [GeneratedRegex(@"^\d{2}$")]
    private static partial Regex TwoNumRegex();

    #endregion

    #endregion
}