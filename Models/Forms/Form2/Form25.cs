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

namespace Models.Forms.Form2;

[Serializable]
[Form_Class("Форма 2.5: Наличие РВ, содержащихся в отработавшем ядерном топливе, в пунктах хранения")]
public class Form25 : Form2
{
    public Form25()
    {
        FormNum.Value = "2.5";
        //NumberOfFields.Value = 12;
        Validate_all();
    }
    private void Validate_all()
    {
        CodeOYAT_Validation(CodeOYAT);
        FcpNumber_Validation(FcpNumber);
        StoragePlaceCode_Validation(StoragePlaceCode);
        StoragePlaceName_Validation(StoragePlaceName);
        FuelMass_Validation(FuelMass);
        CellMass_Validation(CellMass);
        Quantity_Validation(Quantity);
        BetaGammaActivity_Validation(BetaGammaActivity);
        AlphaActivity_Validation(AlphaActivity);
    }

    [FormProperty(true,"Форма")]
    public override bool Object_Validation()
    {
        return !(CodeOYAT.HasErrors||
                 FcpNumber.HasErrors||
                 StoragePlaceCode.HasErrors||
                 StoragePlaceName.HasErrors||
                 FuelMass.HasErrors||
                 CellMass.HasErrors||
                 Quantity.HasErrors||
                 BetaGammaActivity.HasErrors||
                 AlphaActivity.HasErrors);
    }

    #region  StoragePlaceName
    public string StoragePlaceName_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Пункт хранения ОЯТ", "наименование, номер","2")]
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
    //If change this change validation
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
            value.AddError("Поле не заполнено"); return false;
        }
        return true;
    }
    //StoragePlaceName property
    #endregion

    #region  CodeOYAT
    public string CodeOYAT_DB { get; set; } = ""; [NotMapped]
    [FormProperty(true,"Наличие на конец отчетного года", "код ОЯТ","4")]
    public RamAccess<string> CodeOYAT
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(CodeOYAT)))
            {
                ((RamAccess<string>)Dictionary[nameof(CodeOYAT)]).Value = CodeOYAT_DB;
                return (RamAccess<string>)Dictionary[nameof(CodeOYAT)];
            }
            else
            {
                var rm = new RamAccess<string>(CodeOYAT_Validation, CodeOYAT_DB);
                rm.PropertyChanged += CodeOYATValueChanged;
                Dictionary.Add(nameof(CodeOYAT), rm);
                return (RamAccess<string>)Dictionary[nameof(CodeOYAT)];
            }
        }
        set
        {
            CodeOYAT_DB = value.Value;
            OnPropertyChanged(nameof(CodeOYAT));
        }
    }

    private void CodeOYATValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            CodeOYAT_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool CodeOYAT_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено"); return false;
        }
        Regex a = new("^[0-9]{5}$");
        if (!a.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    //CodeOYAT property
    #endregion

    #region  StoragePlaceCode
    public string StoragePlaceCode_DB { get; set; } = ""; [NotMapped]
    [FormProperty(true,"Пункт хранения ОЯТ", "код","3")]
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
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено"); return false;
        }
        if (value.Value == "-")
        {
            return true;
        }
        Regex a = new("^[0-9]{8}$");
        if (!a.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    //StoragePlaceCode property
    #endregion

    #region  FcpNumber
    public string FcpNumber_DB { get; set; } = ""; [NotMapped]
    [FormProperty(true,"Наличие на конец отчетного года", "номер мероприятия ФЦП","5")]
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
        value.ClearErrors();
        return true;
    }
    //FcpNumber property
    #endregion

    #region  FuelMass
    public string FuelMass_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Наличие на конец отчетного года", "топлива (нетто)","6")]
    public RamAccess<string> FuelMass
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(FuelMass)))
            {
                ((RamAccess<string>)Dictionary[nameof(FuelMass)]).Value = FuelMass_DB;
                return (RamAccess<string>)Dictionary[nameof(FuelMass)];
            }
            else
            {
                var rm = new RamAccess<string>(FuelMass_Validation, FuelMass_DB);
                rm.PropertyChanged += FuelMassValueChanged;
                Dictionary.Add(nameof(FuelMass), rm);
                return (RamAccess<string>)Dictionary[nameof(FuelMass)];
            }
        }
        set
        {
            FuelMass_DB = value.Value;
            OnPropertyChanged(nameof(FuelMass));
        }
    }

    private void FuelMassValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
            if (value1 != null)
            {
                value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    FuelMass_DB = value1;
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
            FuelMass_DB = value1;
        }
    }
    private bool FuelMass_Validation(RamAccess<string> value)//TODO
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
    //FuelMass property
    #endregion

    #region  CellMass
    public string CellMass_DB { get; set; } = ""; [NotMapped]
    [FormProperty(true,"Наличие на конец отчетного года", "ОТВС(ТВЭЛ, выемной части реактора) брутто","7")]
    public RamAccess<string> CellMass
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(CellMass)))
            {
                ((RamAccess<string>)Dictionary[nameof(CellMass)]).Value = CellMass_DB;
                return (RamAccess<string>)Dictionary[nameof(CellMass)];
            }
            else
            {
                var rm = new RamAccess<string>(CellMass_Validation, CellMass_DB);
                rm.PropertyChanged += CellMassValueChanged;
                Dictionary.Add(nameof(CellMass), rm);
                return (RamAccess<string>)Dictionary[nameof(CellMass)];
            }
        }
        set
        {
            CellMass_DB = value.Value;
            OnPropertyChanged(nameof(CellMass));
        }
    }

    private void CellMassValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
            if (value1 != null)
            {
                value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    CellMass_DB = value1;
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
            CellMass_DB = value1;
        }
    }
    private bool CellMass_Validation(RamAccess<string> value)//TODO
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
    //CellMass property
    #endregion

    #region  Quantity
    public int? Quantity_DB { get; set; } [NotMapped]
    [FormProperty(true,"Наличие на конец отчетного года", "количество, шт","8")]
    public RamAccess<int?> Quantity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Quantity)))
            {
                ((RamAccess<int?>)Dictionary[nameof(Quantity)]).Value = Quantity_DB;
                return (RamAccess<int?>)Dictionary[nameof(Quantity)];
            }
            else
            {
                var rm = new RamAccess<int?>(Quantity_Validation, Quantity_DB);
                rm.PropertyChanged += QuantityValueChanged;
                Dictionary.Add(nameof(Quantity), rm);
                return (RamAccess<int?>)Dictionary[nameof(Quantity)];
            }
        }
        set
        {
            Quantity_DB = value.Value;
            OnPropertyChanged(nameof(Quantity));
        }
    }
    // positive int.
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
        if (value.Value == null)
        {
            return true;
        }
        if (value.Value <= 0)
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    //Quantity property
    #endregion

    #region  BetaGammaActivity
    public string BetaGammaActivity_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Наличие на конец отчетного года", "бета-, гамма-излучающих нуклидов","10")]
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
        if(string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
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
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    //BetaGammaActivity property
    #endregion

    #region  AlphaActivity
    public string AlphaActivity_DB { get; set; } = ""; [NotMapped]
    [FormProperty(true,"Наличие на конец отчетного года", "альфа-излучающих нуклидов","9")]
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
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    //AlphaActivity property
    #endregion

    #region IExcel

    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        base.ExcelGetRow(worksheet, row);
        StoragePlaceCode_DB = Convert.ToString(worksheet.Cells[row, 2].Value);
        StoragePlaceName_DB = Convert.ToString(worksheet.Cells[row, 3].Value);
        CodeOYAT_DB = Convert.ToString(worksheet.Cells[row, 4].Value);
        FcpNumber_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        FuelMass_DB = ConvertFromExcelDouble(worksheet.Cells[row, 6].Value);
        CellMass_DB = ConvertFromExcelDouble(worksheet.Cells[row, 7].Value);
        Quantity_DB = int.TryParse(Convert.ToString(worksheet.Cells[row, 8].Value), out var intValue) ? intValue : null;
        AlphaActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 9].Value);
        BetaGammaActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 10].Value);
    }
    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ConvertToExcelString(StoragePlaceCode_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(StoragePlaceName_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelString(CodeOYAT_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelString(FcpNumber_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelDouble(FuelMass_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelDouble(CellMass_DB);
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = Quantity_DB is null ? "-" : Quantity_DB;
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelDouble(AlphaActivity_DB);
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ConvertToExcelDouble(BetaGammaActivity_DB);
       
        return 9;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form2.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form25,Models")?.GetProperty(nameof(StoragePlaceName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form25,Models")?.GetProperty(nameof(StoragePlaceCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form25,Models")?.GetProperty(nameof(CodeOYAT))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form25,Models")?.GetProperty(nameof(FcpNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form25,Models")?.GetProperty(nameof(FuelMass))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form25,Models")?.GetProperty(nameof(CellMass))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form25,Models")?.GetProperty(nameof(Quantity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form25,Models")?.GetProperty(nameof(AlphaActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form25,Models")?.GetProperty(nameof(BetaGammaActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        
        return 9;
    }
    
    #endregion

    #region IDataGridColumn

    private static DataGridColumns _DataGridColumns { get; set; }
    public override DataGridColumns GetColumnStructure(string param = "")
    {
        if (_DataGridColumns != null) return _DataGridColumns;

        #region NumberInOrder (1)
        var NumberInOrderR = ((FormPropertyAttribute)typeof(Form).GetProperty(nameof(NumberInOrder)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
        NumberInOrderR.SetSizeColToAllLevels(50);
        NumberInOrderR.Binding = nameof(NumberInOrder);
        NumberInOrderR.Blocked = true;
        NumberInOrderR.ChooseLine = true;
        #endregion

        #region StoragePlaceName (2)
        var StoragePlaceNameR = ((FormPropertyAttribute)typeof(Form25).GetProperty(nameof(StoragePlaceName)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        StoragePlaceNameR.SetSizeColToAllLevels(163);
        StoragePlaceNameR.Binding = nameof(StoragePlaceName);
        NumberInOrderR += StoragePlaceNameR;
        #endregion

        #region StoragePlaceCode (3)
        var StoragePlaceCodeR = ((FormPropertyAttribute)typeof(Form25).GetProperty(nameof(StoragePlaceCode)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        StoragePlaceCodeR.SetSizeColToAllLevels(88);
        StoragePlaceCodeR.Binding = nameof(StoragePlaceCode);
        NumberInOrderR += StoragePlaceCodeR;
        #endregion

        #region CodeOYAT (4)
        var CodeOYATR = ((FormPropertyAttribute)typeof(Form25).GetProperty(nameof(CodeOYAT)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        CodeOYATR.SetSizeColToAllLevels(88);
        CodeOYATR.Binding = nameof(CodeOYAT);
        NumberInOrderR += CodeOYATR;
        #endregion

        #region FcpNumber (5)
        var FcpNumberR = ((FormPropertyAttribute)typeof(Form25).GetProperty(nameof(FcpNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        FcpNumberR.SetSizeColToAllLevels(163);
        FcpNumberR.Binding = nameof(FcpNumber);
        NumberInOrderR += FcpNumberR;
        #endregion

        #region FuelMass (6)
        var FuelMassR = ((FormPropertyAttribute)typeof(Form25).GetProperty(nameof(FuelMass)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        FuelMassR.SetSizeColToAllLevels(103);
        FuelMassR.Binding = nameof(FuelMass);
        NumberInOrderR += FuelMassR;
        #endregion

        #region CellMass (7)
        var CellMassR = ((FormPropertyAttribute)typeof(Form25).GetProperty(nameof(CellMass)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        CellMassR.SetSizeColToAllLevels(288);
        CellMassR.Binding = nameof(CellMass);
        NumberInOrderR += CellMassR;
        #endregion

        #region Quantity (8)
        var QuantityR = ((FormPropertyAttribute)typeof(Form25).GetProperty(nameof(Quantity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        QuantityR.SetSizeColToAllLevels(100);
        QuantityR.Binding = nameof(Quantity);
        NumberInOrderR += QuantityR;
        #endregion

        #region AlphaActivity (9)
        var AlphaActivityR = ((FormPropertyAttribute)typeof(Form25).GetProperty(nameof(AlphaActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        AlphaActivityR.SetSizeColToAllLevels(185);
        AlphaActivityR.Binding = nameof(AlphaActivity);
        NumberInOrderR += AlphaActivityR;
        #endregion

        #region BetaGammaActivity (10)
        var BetaGammaActivityR = ((FormPropertyAttribute)typeof(Form25).GetProperty(nameof(BetaGammaActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        BetaGammaActivityR.SetSizeColToAllLevels(185);
        BetaGammaActivityR.Binding = nameof(BetaGammaActivity);
        NumberInOrderR += BetaGammaActivityR;
        #endregion

        _DataGridColumns = NumberInOrderR;

        return _DataGridColumns;
    }

    #endregion
}