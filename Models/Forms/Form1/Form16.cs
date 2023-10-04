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

[Form_Class("Форма 1.6: Сведения о некондиционированных РАО")]
public class Form16 : Form1
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
        if (!new Regex(@"^\d{2}$").IsMatch(value.Value)
            || !byte.TryParse(value.Value, out var byteValue)
            || byteValue is (15 or 17 or 46 or 47 or 53 or 54 or 58 or 61 or 62 or 65 or 66 or 67 or 81
                or 82 or 83 or 85 or 86 or 87))
        {
            value.AddError("Код операции не может быть использован в форме 1.6");
            return false;
        }

        return true;
    }

    #endregion

    #region Properties

    #region CodeRAO
    public string CodeRAO_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "null-4", "Код РАО", "4")]
    public RamAccess<string> CodeRAO
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(CodeRAO)))
            {
                ((RamAccess<string>)Dictionary[nameof(CodeRAO)]).Value = CodeRAO_DB;
                return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
            }
            else
            {
                var rm = new RamAccess<string>(CodeRAO_Validation, CodeRAO_DB);
                rm.PropertyChanged += CodeRAOValueChanged;
                Dictionary.Add(nameof(CodeRAO), rm);
                return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
            }
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
            value.AddError("Поле не заполнено");
            return false;
        }
        var tmp = value.Value.ToLower();
        tmp = tmp.Replace("х", "x");
        Regex a = new("^[0-9x+]{11}$");
        if (!a.IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmp.Length == 11)
        {
            Regex a0 = new("^[1-3x+]");
            if (!a0.IsMatch(tmp.Substring(0, 1)))
            {
                value.AddError($"Недопустимое агрегатное состояние - {tmp.Substring(0, 1)}");
            }
            Regex a1 = new("^[0-49x+]");
            if (!a1.IsMatch(tmp.Substring(1, 1)))
            {
                value.AddError($"Недопустимое категория РАО - {tmp.Substring(1, 1)}");
            }
            Regex a2 = new("^[0-6x+]");
            if (!a2.IsMatch(tmp.Substring(2, 1)))
            {
                value.AddError($"Недопустимый радионуклидный состав РАО - {tmp.Substring(2, 1)}");
            }
            Regex a3 = new("^[12x+]");
            if (!a3.IsMatch(tmp.Substring(3, 1)))
            {
                value.AddError($"Недопустимое содержание ядерных материалов - {tmp.Substring(3, 1)}");
            }
            Regex a4 = new("^[12x+]");
            if (!a4.IsMatch(tmp.Substring(4, 1)))
            {
                value.AddError($"Недопустимоый период полураспада - {tmp.Substring(4, 1)}");
            }
            Regex a5 = new("^[0-3x+]");
            if (!a5.IsMatch(tmp.Substring(5, 1)))
            {
                value.AddError($"Недопустимоый период потенциальной опасности РАО - {tmp.Substring(5, 1)}");
            }
            Regex a6 = new("^[0-49x+]");
            if (!a6.IsMatch(tmp.Substring(6, 1)))
            {
                value.AddError($"Недопустимоый способ переработки - {tmp.Substring(6, 1)}");
            }
            Regex a7 = new("^[0-79x+]");
            if (!a7.IsMatch(tmp.Substring(7, 1)))
            {
                value.AddError($"Недопустимоый класс РАО - {tmp.Substring(7, 1)}");
            }
            Regex a89 = new("^[1]{1}[1-9]{1}|^[0]{1}[1]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1-9]{1}|^[4]{1}[1-6]{1}|^[5]{1}[1-9]{1}|^[6]{1}[1-9]{1}|^[7]{1}[1-9]{1}|^[8]{1}[1-9]{1}|^[9]{1}[1-9]{1}");
            if (!a89.IsMatch(tmp.Substring(8, 2)))
            {
                value.AddError($"Недопустимоый код типа РАО - {tmp.Substring(8, 2)}");
            }
            Regex a10 = new("^[12x+]");
            if (!a7.IsMatch(tmp.Substring(10, 1)))
            {
                value.AddError($"Недопустимая горючесть - {tmp.Substring(10, 1)}");
            }
            if (value.HasErrors)
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region StatusRAO
    public string StatusRAO_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "null-5", "Статус РАО", "5")]
    public RamAccess<string> StatusRAO  //1 cyfer or OKPO.
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(StatusRAO)))
            {
                ((RamAccess<string>)Dictionary[nameof(StatusRAO)]).Value = StatusRAO_DB;
                return (RamAccess<string>)Dictionary[nameof(StatusRAO)];
            }
            else
            {
                var rm = new RamAccess<string>(StatusRAO_Validation, StatusRAO_DB);
                rm.PropertyChanged += StatusRAOValueChanged;
                Dictionary.Add(nameof(StatusRAO), rm);
                return (RamAccess<string>)Dictionary[nameof(StatusRAO)];
            }
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
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("-"))
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
                    value.AddError("Недопустимое значение");
                }
            }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
            }
            return false;
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

    #region Volume
    public string Volume_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Количество", "объем без упаковки, куб. м", "6")]
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
                if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
                {
                    value1 = $"{doubleValue:0.######################################################e+00}";
                }
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

    #region Mass
    public string Mass_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Количество", "масса без упаковки (нетто), т", "7")]
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
                if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
                {
                    value1 = $"{doubleValue:0.######################################################e+00}";
                }
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
            { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch (Exception)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    #endregion

    #region QuantityOZIII
    public string QuantityOZIII_DB { get; set; }
    [NotMapped]
    [FormProperty(true, "null-8", "Количество ОЗИИИ, шт", "8")]
    public RamAccess<string> QuantityOZIII
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(QuantityOZIII)))
            {
                ((RamAccess<string>)Dictionary[nameof(QuantityOZIII)]).Value = QuantityOZIII_DB;
                return (RamAccess<string>)Dictionary[nameof(QuantityOZIII)];
            }
            else
            {
                var rm = new RamAccess<string>(QuantityOZIII_Validation, QuantityOZIII_DB);
                rm.PropertyChanged += QuantityOZIIIValueChanged;
                Dictionary.Add(nameof(QuantityOZIII), rm);
                return (RamAccess<string>)Dictionary[nameof(QuantityOZIII)];
            }
        }
        set
        {
            QuantityOZIII_DB = value.Value;
            OnPropertyChanged(nameof(QuantityOZIII));
        }
    }// positive int.

    private void QuantityOZIIIValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            QuantityOZIII_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool QuantityOZIII_Validation(RamAccess<string> value)//Ready
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

    #region MainRadionuclids
    public string MainRadionuclids_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "null-9", "Основные радионуклиды", "9")]
    public RamAccess<string> MainRadionuclids
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(MainRadionuclids)))
            {
                ((RamAccess<string>)Dictionary[nameof(MainRadionuclids)]).Value = MainRadionuclids_DB;
                return (RamAccess<string>)Dictionary[nameof(MainRadionuclids)];
            }
            else
            {
                var rm = new RamAccess<string>(MainRadionuclids_Validation, MainRadionuclids_DB);
                rm.PropertyChanged += MainRadionuclidsValueChanged;
                Dictionary.Add(nameof(MainRadionuclids), rm);
                return (RamAccess<string>)Dictionary[nameof(MainRadionuclids)];
            }
        }
        set
        {
            MainRadionuclids_DB = value.Value;
            OnPropertyChanged(nameof(MainRadionuclids));
        }
    }//If change this change validation

    private void MainRadionuclidsValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            MainRadionuclids_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool MainRadionuclids_Validation(RamAccess<string> value)//TODO
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

    #region TritiumActivity
    public string TritiumActivity_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Суммарная активность, Бк", "тритий", "10")]
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
                if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
                {
                    value1 = $"{doubleValue:0.######################################################e+00}";
                }
            }
            TritiumActivity_DB = value1;
        }
    }
    private bool TritiumActivity_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
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

    #region BetaGammaActivity
    public string BetaGammaActivity_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Суммарная активность, Бк", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "11")]
    public RamAccess<string> BetaGammaActivity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(BetaGammaActivity)))
            {
                ((RamAccess<string>)Dictionary[nameof(BetaGammaActivity)]).Value = BetaGammaActivity_DB;
                return (RamAccess<string>)Dictionary[nameof(BetaGammaActivity)];
            }
            else
            {
                var rm = new RamAccess<string>(BetaGammaActivity_Validation, BetaGammaActivity_DB);
                rm.PropertyChanged += BetaGammaActivityValueChanged;
                Dictionary.Add(nameof(BetaGammaActivity), rm);
                return (RamAccess<string>)Dictionary[nameof(BetaGammaActivity)];
            }
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
                if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
                {
                    value1 = $"{doubleValue:0.######################################################e+00}";
                }
            }
            BetaGammaActivity_DB = value1;
        }
    }
    private bool BetaGammaActivity_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
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

    #region AlphaActivity
    public string AlphaActivity_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Суммарная активность, Бк", "альфа-излучающие радионуклиды (исключая трансурановые)", "12")]
    public RamAccess<string> AlphaActivity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(AlphaActivity)))
            {
                ((RamAccess<string>)Dictionary[nameof(AlphaActivity)]).Value = AlphaActivity_DB;
                return (RamAccess<string>)Dictionary[nameof(AlphaActivity)];
            }
            else
            {
                var rm = new RamAccess<string>(AlphaActivity_Validation, AlphaActivity_DB);
                rm.PropertyChanged += AlphaActivityValueChanged;
                Dictionary.Add(nameof(AlphaActivity), rm);
                return (RamAccess<string>)Dictionary[nameof(AlphaActivity)];
            }
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
                if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
                {
                    value1 = $"{doubleValue:0.######################################################e+00}";
                }
            }
            AlphaActivity_DB = value1;
        }
    }
    private bool AlphaActivity_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
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

    #region TransuraniumActivity
    public string TransuraniumActivity_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "Суммарная активность, Бк", "трансурановые радионуклиды", "13")]
    public RamAccess<string> TransuraniumActivity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(TransuraniumActivity)))
            {
                ((RamAccess<string>)Dictionary[nameof(TransuraniumActivity)]).Value = TransuraniumActivity_DB;
                return (RamAccess<string>)Dictionary[nameof(TransuraniumActivity)];
            }
            else
            {
                var rm = new RamAccess<string>(TransuraniumActivity_Validation, TransuraniumActivity_DB);
                rm.PropertyChanged += TransuraniumActivityValueChanged;
                Dictionary.Add(nameof(TransuraniumActivity), rm);
                return (RamAccess<string>)Dictionary[nameof(TransuraniumActivity)];
            }
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
                if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
                {
                    value1 = $"{doubleValue:0.######################################################e+00}";
                }
            }
            TransuraniumActivity_DB = value1;
        }
    }
    private bool TransuraniumActivity_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
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
    [FormProperty(true, "null-14", "Дата измерения активности", "14")]

    public virtual RamAccess<string> ActivityMeasurementDate
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
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
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

    #region ProviderOrRecieverOKPO
    public string ProviderOrRecieverOKPO_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "ОКПО", "поставщика или получателя", "18")]
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
            OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
        }
    }
    [NotMapped]
    public string OKPOofFormFiller { get; set; } = null;
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
            try
            {
                var a = int.Parse(OperationCode.Value) >= 10 && int.Parse(OperationCode.Value) <= 14;
                var b = int.Parse(OperationCode.Value) >= 41 && int.Parse(OperationCode.Value) <= 45;
                var c = int.Parse(OperationCode.Value) >= 71 && int.Parse(OperationCode.Value) <= 73;
                var e = int.Parse(OperationCode.Value) >= 55 && int.Parse(OperationCode.Value) <= 57;
                var d = OperationCode.Value is "01" or "16" or "18" or "48" or "49" or "51" or "52" or "59" or "68" or "75" or "76";
                if (a || b || c || d || e)
                {
                    //ProviderOrRecieverOKPO_DB = OKPOofFormFiller;
                }
            }
            catch
            {

            }
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
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
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

    #region TransporterOKPO
    public string TransporterOKPO_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "ОКПО", "перевозчика", "19")]
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
            TransporterOKPO_DB = ((RamAccess<string>)Value).Value;
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
        if (value.Value.Equals("прим."))
        {
            //if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
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

    #region StoragePlaceName
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
            else
            {
                var rm = new RamAccess<string>(StoragePlaceName_Validation, StoragePlaceName_DB);
                rm.PropertyChanged += StoragePlaceNameValueChanged;
                Dictionary.Add(nameof(StoragePlaceName), rm);
                return (RamAccess<string>)Dictionary[nameof(StoragePlaceName)];
            }
        }
        set
        {
            StoragePlaceName_DB = value.Value;
            OnPropertyChanged(nameof(StoragePlaceName));
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

    #region StoragePlaceCode
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
            else
            {
                var rm = new RamAccess<string>(StoragePlaceCode_Validation, StoragePlaceCode_DB);
                rm.PropertyChanged += StoragePlaceCodeValueChanged;
                Dictionary.Add(nameof(StoragePlaceCode), rm);
                return (RamAccess<string>)Dictionary[nameof(StoragePlaceCode)];
            }
        }
        set
        {
            StoragePlaceCode_DB = value.Value;
            OnPropertyChanged(nameof(StoragePlaceCode));
        }
    }//if change this change validation

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
        Regex a = new("^[0-9]{8}$");
        if (!a.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); return false;
        }
        var tmp = value.Value;
        if (tmp.Length == 8)
        {
            Regex a0 = new("^[1-9]");
            if (!a0.IsMatch(tmp.Substring(0, 1)))
            {
                value.AddError($"Недопустимый вид пункта - {tmp.Substring(0, 1)}");
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
                value.AddError($"Недопустимоый код типа РАО - {tmp.Substring(6, 2)}");
            }
            if (value.HasErrors)
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region RefineOrSortRAOCode
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

    #region PackName
    public string PackName_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "УКТ, упаковка или иная учетная единица", "наименование", "23")]
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
            //    value.AddError( "Заполните примечание");
            return true;
        }
        return true;
    }
    #endregion

    #region PackType
    public string PackType_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "УКТ, упаковка или иная учетная единица", "тип", "24")]
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
            OnPropertyChanged(nameof(PackType));
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
            //    value.AddError( "Заполните примечание");//to do note handling
            return true;
        }
        return true;
    }
    #endregion

    #region PackNumber
    public string PackNumber_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "УКТ, упаковка или иная учетная единица", "номер упаковки", "25")]
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
            OnPropertyChanged(nameof(PackNumber));
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
            //    value.AddError( "Заполните примечание");
            return true;
        }
        return true;
    }
    #endregion

    #region Subsidy
    public string Subsidy_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true, "null-26", "Субсидия, %", "26")]
    public RamAccess<string> Subsidy // 0<number<=100 or empty.
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Subsidy)))
            {
                ((RamAccess<string>)Dictionary[nameof(Subsidy)]).Value = Subsidy_DB;
                return (RamAccess<string>)Dictionary[nameof(Subsidy)];
            }
            else
            {
                var rm = new RamAccess<string>(Subsidy_Validation, Subsidy_DB);
                rm.PropertyChanged += SubsidyValueChanged;
                Dictionary.Add(nameof(Subsidy), rm);
                return (RamAccess<string>)Dictionary[nameof(Subsidy)];
            }
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
    [FormProperty(true, "null-27", "Номер мероприятия ФЦП", "27")]
    public RamAccess<string> FcpNumber
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(FcpNumber)))
            {
                ((RamAccess<string>)Dictionary[nameof(FcpNumber)]).Value = FcpNumber_DB;
                return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
            }
            else
            {
                var rm = new RamAccess<string>(FcpNumber_Validation, FcpNumber_DB);
                rm.PropertyChanged += FcpNumberValueChanged;
                Dictionary.Add(nameof(FcpNumber), rm);
                return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
            }
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
        value.ClearErrors(); return true;
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
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ConvertToExcelDate(ActivityMeasurementDate_DB);
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = DocumentVid_DB is null ? "-" : DocumentVid_DB;
        worksheet.Cells[row + (!transpose ? 12 : 0), column + (transpose ? 12 : 0)].Value = ConvertToExcelString(DocumentNumber_DB);
        worksheet.Cells[row + (!transpose ? 13 : 0), column + (transpose ? 13 : 0)].Value = ConvertToExcelDate(DocumentDate_DB);
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

            #region CodeRAO (4)
            var CodeRAOR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(CodeRAO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            CodeRAOR.SetSizeColToAllLevels(88);
            CodeRAOR.Binding = nameof(CodeRAO);
            NumberInOrderR += CodeRAOR;
            #endregion

            #region StatusRAO (5)
            var StatusRAOR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(StatusRAO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            StatusRAOR.SetSizeColToAllLevels(88);
            StatusRAOR.Binding = nameof(StatusRAO);
            NumberInOrderR += StatusRAOR;
            #endregion

            #region Volume (6)
            var VolumeR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(Volume)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            VolumeR.SetSizeColToAllLevels(90);
            VolumeR.Binding = nameof(Volume);
            NumberInOrderR += VolumeR;
            #endregion

            #region Mass (7)
            var MassR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(Mass)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            MassR.SetSizeColToAllLevels(90);
            MassR.Binding = nameof(Mass);
            NumberInOrderR += MassR;
            #endregion

            #region QuantityOZIII (8)
            var QuantityOZIIIR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(QuantityOZIII)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            QuantityOZIIIR.SetSizeColToAllLevels(70);
            QuantityOZIIIR.Binding = nameof(QuantityOZIII);
            NumberInOrderR += QuantityOZIIIR;
            #endregion

            #region MainRadionuclids (9)
            var MainRadionuclidsR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(MainRadionuclids)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            MainRadionuclidsR.SetSizeColToAllLevels(150);
            MainRadionuclidsR.Binding = nameof(MainRadionuclids);
            NumberInOrderR += MainRadionuclidsR;
            #endregion

            #region TritiumActivity (10)
            var TritiumActivityR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(TritiumActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            TritiumActivityR.SetSizeColToAllLevels(163);
            TritiumActivityR.Binding = nameof(TritiumActivity);
            NumberInOrderR += TritiumActivityR;
            #endregion

            #region BetaGammaActivity (11)
            var BetaGammaActivityR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(BetaGammaActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            BetaGammaActivityR.SetSizeColToAllLevels(175);
            BetaGammaActivityR.Binding = nameof(BetaGammaActivity);
            NumberInOrderR += BetaGammaActivityR;
            #endregion

            #region AlphaActivity (12)
            var AlphaActivityR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(AlphaActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            AlphaActivityR.SetSizeColToAllLevels(185);
            AlphaActivityR.Binding = nameof(AlphaActivity);
            NumberInOrderR += AlphaActivityR;
            #endregion

            #region TransuraniumActivity (13)
            var TransuraniumActivityR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(TransuraniumActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            TransuraniumActivityR.SetSizeColToAllLevels(200);
            TransuraniumActivityR.Binding = nameof(TransuraniumActivity);
            NumberInOrderR += TransuraniumActivityR;
            #endregion

            #region ActivityMeasurementDate (14)
            var ActivityMeasurementDateR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(ActivityMeasurementDate)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            ActivityMeasurementDateR.SetSizeColToAllLevels(100);
            ActivityMeasurementDateR.Binding = nameof(ActivityMeasurementDate);
            NumberInOrderR += ActivityMeasurementDateR;
            #endregion

            #region DocumentVid (15)
            var DocumentVidR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentVid)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentVidR.SetSizeColToAllLevels(88);
            DocumentVidR.Binding = nameof(DocumentVid);
            NumberInOrderR += DocumentVidR;
            #endregion

            #region DocumentNumber (16)
            var DocumentNumberR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentNumberR.SetSizeColToAllLevels(103);
            DocumentNumberR.Binding = nameof(DocumentNumber);
            NumberInOrderR += DocumentNumberR;
            #endregion

            #region DocumentDate (17)
            var DocumentDateR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentDate)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentDateR.SetSizeColToAllLevels(88);
            DocumentDateR.Binding = nameof(DocumentDate);
            NumberInOrderR += DocumentDateR;
            #endregion

            #region ProviderOrRecieverOKPO (18)
            var ProviderOrRecieverOKPOR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(ProviderOrRecieverOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            ProviderOrRecieverOKPOR.SetSizeColToAllLevels(100);
            ProviderOrRecieverOKPOR.Binding = nameof(ProviderOrRecieverOKPO);
            NumberInOrderR += ProviderOrRecieverOKPOR;
            #endregion

            #region TransporterOKPO (19)
            var TransporterOKPOR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(TransporterOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            TransporterOKPOR.SetSizeColToAllLevels(163);
            TransporterOKPOR.Binding = nameof(TransporterOKPO);
            NumberInOrderR += TransporterOKPOR;
            #endregion

            #region StoragePlaceName (20)
            var StoragePlaceNameR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(StoragePlaceName)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            StoragePlaceNameR.SetSizeColToAllLevels(103);
            StoragePlaceNameR.Binding = nameof(StoragePlaceName);
            NumberInOrderR += StoragePlaceNameR;
            #endregion

            #region StoragePlaceCode (21)
            var StoragePlaceCodeR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(StoragePlaceCode)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            StoragePlaceCodeR.SetSizeColToAllLevels(88);
            StoragePlaceCodeR.Binding = nameof(StoragePlaceCode);
            NumberInOrderR += StoragePlaceCodeR;
            #endregion

            #region RefineOrSortRAOCode (22)
            var RefineOrSortRAOCodeR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(RefineOrSortRAOCode)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            RefineOrSortRAOCodeR.SetSizeColToAllLevels(110);
            RefineOrSortRAOCodeR.Binding = nameof(RefineOrSortRAOCode);
            NumberInOrderR += RefineOrSortRAOCodeR;
            #endregion

            #region PackName (23)
            var PackNameR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(PackName)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PackNameR.SetSizeColToAllLevels(163);
            PackNameR.Binding = nameof(PackName);
            NumberInOrderR += PackNameR;
            #endregion

            #region PackType (24)
            var PackTypeR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(PackType)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PackTypeR.SetSizeColToAllLevels(88);
            PackTypeR.Binding = nameof(PackType);
            NumberInOrderR += PackTypeR;
            #endregion

            #region PackNumber (25)
            var PackNumberR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(PackNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            PackNumberR.SetSizeColToAllLevels(163);
            PackNumberR.Binding = nameof(PackNumber);
            NumberInOrderR += PackNumberR;
            #endregion

            #region Subsidy (26)
            var SubsidyR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(Subsidy)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            SubsidyR.SetSizeColToAllLevels(88);
            SubsidyR.Binding = nameof(Subsidy);
            NumberInOrderR += SubsidyR;
            #endregion

            #region FcpNumber (27)
            var FcpNumberR = ((FormPropertyAttribute)typeof(Form16).GetProperty(nameof(FcpNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
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