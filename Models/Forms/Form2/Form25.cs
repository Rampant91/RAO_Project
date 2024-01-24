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
public partial class Form25 : Form2
{
    #region Constructor
    
    public Form25()
    {
        FormNum.Value = "2.5";
        //NumberOfFields.Value = 12;
        Validate_all();
    }

    #endregion

    #region Validation
    
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

    [FormProperty(true, "Форма")]
    public override bool Object_Validation()
    {
        return !(CodeOYAT.HasErrors ||
                 FcpNumber.HasErrors ||
                 StoragePlaceCode.HasErrors ||
                 StoragePlaceName.HasErrors ||
                 FuelMass.HasErrors ||
                 CellMass.HasErrors ||
                 Quantity.HasErrors ||
                 BetaGammaActivity.HasErrors ||
                 AlphaActivity.HasErrors);
    }

    #endregion

    #region Properties

    #region  StoragePlaceName (2)

    public string StoragePlaceName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Пункт хранения ОЯТ", "наименование, номер", "2")]
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
            rm.PropertyChanged += StoragePlaceNameValueChanged;
            Dictionary.Add(nameof(StoragePlaceName), rm);
            return (RamAccess<string>)Dictionary[nameof(StoragePlaceName)];
        }
        set
        {
            StoragePlaceName_DB = value.Value;
            OnPropertyChanged(nameof(StoragePlaceName));
        }
    }
    //If change this change validation

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
        return true;
    }

    #endregion

    #region  StoragePlaceCode (3)

    public string StoragePlaceCode_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Пункт хранения ОЯТ", "код", "3")]
    public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(StoragePlaceCode), out var value))
            {
                ((RamAccess<string>)value).Value = StoragePlaceCode_DB;
                return (RamAccess<string>)value;
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
    }

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
        if (value.Value == "-")
        {
            return true;
        }
        if (!EightNumRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  CodeOYAT (4)

    public string CodeOYAT_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Наличие на конец отчетного года", "код ОЯТ", "4")]
    public RamAccess<string> CodeOYAT
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(CodeOYAT), out var value))
            {
                ((RamAccess<string>)value).Value = CodeOYAT_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(CodeOYAT_Validation, CodeOYAT_DB);
            rm.PropertyChanged += CodeOYATValueChanged;
            Dictionary.Add(nameof(CodeOYAT), rm);
            return (RamAccess<string>)Dictionary[nameof(CodeOYAT)];
        }
        set
        {
            CodeOYAT_DB = value.Value;
            OnPropertyChanged(nameof(CodeOYAT));
        }
    }

    private void CodeOYATValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            CodeOYAT_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool CodeOYAT_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено"); return false;
        }
        if (!FiveNumRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }

    #endregion

    #region  FcpNumber (5)
    public string FcpNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Наличие на конец отчетного года", "номер мероприятия ФЦП", "5")]
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

    #region  FuelMass (6)

    public string FuelMass_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Наличие на конец отчетного года", "топлива (нетто)", "6")]
    public RamAccess<string> FuelMass
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(FuelMass), out var value))
            {
                ((RamAccess<string>)value).Value = FuelMass_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(FuelMass_Validation, FuelMass_DB);
            rm.PropertyChanged += FuelMassValueChanged;
            Dictionary.Add(nameof(FuelMass), rm);
            return (RamAccess<string>)Dictionary[nameof(FuelMass)];
        }
        set
        {
            FuelMass_DB = value.Value;
            OnPropertyChanged(nameof(FuelMass));
        }
    }

    private void FuelMassValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
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
        if (value1[0] == '(' && value1[^1] == ')')
        {
            value1 = value1.Remove(value1.Length - 1, 1).Remove(0, 1);
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

    #region  CellMass (7)

    public string CellMass_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Наличие на конец отчетного года", "ОТВС(ТВЭЛ, выемной части реактора) брутто", "7")]
    public RamAccess<string> CellMass
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(CellMass), out var value))
            {
                ((RamAccess<string>)value).Value = CellMass_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(CellMass_Validation, CellMass_DB);
            rm.PropertyChanged += CellMassValueChanged;
            Dictionary.Add(nameof(CellMass), rm);
            return (RamAccess<string>)Dictionary[nameof(CellMass)];
        }
        set
        {
            CellMass_DB = value.Value;
            OnPropertyChanged(nameof(CellMass));
        }
    }

    private void CellMassValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
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
        if (value1[0] == '(' && value1[^1] == ')')
        {
            value1 = value1.Remove(value1.Length - 1, 1).Remove(0, 1);
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

    #region  Quantity (8)

    public int? Quantity_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Наличие на конец отчетного года", "количество, шт", "8")]
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
    // positive int.

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
            return true;
        }
        if (value.Value <= 0)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  AlphaActivity (9)

    public string AlphaActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Наличие на конец отчетного года", "альфа-излучающих нуклидов", "9")]
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
            rm.PropertyChanged += AlphaActivityValueChanged;
            Dictionary.Add(nameof(AlphaActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(AlphaActivity)];
        }
        set
        {
            AlphaActivity_DB = value.Value;
            OnPropertyChanged(nameof(AlphaActivity));
        }
    }

    private void AlphaActivityValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
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
        if (value1[0] == '(' && value1[^1] == ')')
        {
            value1 = value1.Remove(value1.Length - 1, 1).Remove(0, 1);
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

    #region  BetaGammaActivity (10)

    public string BetaGammaActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Наличие на конец отчетного года", "бета-, гамма-излучающих нуклидов", "10")]
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
            rm.PropertyChanged += BetaGammaActivityValueChanged;
            Dictionary.Add(nameof(BetaGammaActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(BetaGammaActivity)];
        }
        set
        {
            BetaGammaActivity_DB = value.Value;
            OnPropertyChanged(nameof(BetaGammaActivity));
        }
    }

    private void BetaGammaActivityValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
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

    private bool BetaGammaActivity_Validation(RamAccess<string> value)//TODO
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
        if (value1[0] == '(' && value1[^1] == ')')
        {
            value1 = value1.Remove(value1.Length - 1, 1).Remove(0, 1);
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

        #region StoragePlaceName (2)

        var storagePlaceNameR = ((FormPropertyAttribute)typeof(Form25)
                .GetProperty(nameof(StoragePlaceName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (storagePlaceNameR != null)
        {
            storagePlaceNameR.SetSizeColToAllLevels(163);
            storagePlaceNameR.Binding = nameof(StoragePlaceName);
            numberInOrderR += storagePlaceNameR;
        }

        #endregion

        #region StoragePlaceCode (3)

        var storagePlaceCodeR = ((FormPropertyAttribute)typeof(Form25)
                .GetProperty(nameof(StoragePlaceCode))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (storagePlaceCodeR != null)
        {
            storagePlaceCodeR.SetSizeColToAllLevels(88);
            storagePlaceCodeR.Binding = nameof(StoragePlaceCode);
            numberInOrderR += storagePlaceCodeR;
        }

        #endregion

        #region CodeOYAT (4)

        var codeOyatR = ((FormPropertyAttribute)typeof(Form25)
                .GetProperty(nameof(CodeOYAT))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (codeOyatR != null)
        {
            codeOyatR.SetSizeColToAllLevels(88);
            codeOyatR.Binding = nameof(CodeOYAT);
            numberInOrderR += codeOyatR;
        }

        #endregion

        #region FcpNumber (5)

        var fcpNumberR = ((FormPropertyAttribute)typeof(Form25)
                .GetProperty(nameof(FcpNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (fcpNumberR != null)
        {
            fcpNumberR.SetSizeColToAllLevels(163);
            fcpNumberR.Binding = nameof(FcpNumber);
            numberInOrderR += fcpNumberR;
        }

        #endregion

        #region FuelMass (6)

        var fuelMassR = ((FormPropertyAttribute)typeof(Form25)
                .GetProperty(nameof(FuelMass))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (fuelMassR != null)
        {
            fuelMassR.SetSizeColToAllLevels(103);
            fuelMassR.Binding = nameof(FuelMass);
            numberInOrderR += fuelMassR;
        }

        #endregion

        #region CellMass (7)

        var cellMassR = ((FormPropertyAttribute)typeof(Form25)
                .GetProperty(nameof(CellMass))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (cellMassR != null)
        {
            cellMassR.SetSizeColToAllLevels(288);
            cellMassR.Binding = nameof(CellMass);
            numberInOrderR += cellMassR;
        }

        #endregion

        #region Quantity (8)

        var quantityR = ((FormPropertyAttribute)typeof(Form25)
                .GetProperty(nameof(Quantity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        quantityR.SetSizeColToAllLevels(100);
        quantityR.Binding = nameof(Quantity);
        numberInOrderR += quantityR;

        #endregion

        #region AlphaActivity (9)

        var alphaActivityR = ((FormPropertyAttribute)typeof(Form25)
                .GetProperty(nameof(AlphaActivity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        alphaActivityR.SetSizeColToAllLevels(185);
        alphaActivityR.Binding = nameof(AlphaActivity);
        numberInOrderR += alphaActivityR;

        #endregion

        #region BetaGammaActivity (10)

        var betaGammaActivityR = ((FormPropertyAttribute)typeof(Form25)
                .GetProperty(nameof(BetaGammaActivity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        betaGammaActivityR.SetSizeColToAllLevels(185);
        betaGammaActivityR.Binding = nameof(BetaGammaActivity);
        numberInOrderR += betaGammaActivityR;

        #endregion

        _DataGridColumns = numberInOrderR;

        return _DataGridColumns;
    }

    #endregion

    #region GeneratedRegex
    
    [GeneratedRegex("^[0-9]{8}$")]
    private static partial Regex EightNumRegex();

    [GeneratedRegex("^[0-9]{5}$")]
    private static partial Regex FiveNumRegex();

    #endregion
}