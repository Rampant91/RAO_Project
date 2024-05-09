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

namespace Models.Forms.Form2;

[Serializable]
[Form_Class("Форма 2.7: Поступление радионуклидов в атмосферный воздух")]
[Table (name: "form_27")]
public class Form27 : Form2
{
    #region Constructor
    
    public Form27()
    {
        FormNum.Value = "2.7";
        //NumberOfFields.Value = 13;
        Validate_all();
    }

    #endregion

    #region Validation
    
    private void Validate_all()
    {
        ObservedSourceNumber_Validation(ObservedSourceNumber);
        RadionuclidName_Validation(RadionuclidName);
        AllowedWasteValue_Validation(AllowedWasteValue);
        FactedWasteValue_Validation(FactedWasteValue);
        WasteOutbreakPreviousYear_Validation(WasteOutbreakPreviousYear);
    }

    [FormProperty(true, "Форма")]
    public override bool Object_Validation()
    {
        return !(ObservedSourceNumber.HasErrors ||
                 RadionuclidName.HasErrors ||
                 AllowedWasteValue.HasErrors ||
                 FactedWasteValue.HasErrors ||
                 WasteOutbreakPreviousYear.HasErrors);
    }

    #endregion

    #region Properties
    
    #region  ObservedSourceNumber (2)

    public string ObservedSourceNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-2", "Наименование, номер источника выбросов", "2")]
    public RamAccess<string> ObservedSourceNumber
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ObservedSourceNumber), out var value))
            {
                ((RamAccess<string>)value).Value = ObservedSourceNumber_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(ObservedSourceNumber_Validation, ObservedSourceNumber_DB);
            rm.PropertyChanged += ObservedSourceNumberValueChanged;
            Dictionary.Add(nameof(ObservedSourceNumber), rm);
            return (RamAccess<string>)Dictionary[nameof(ObservedSourceNumber)];
        }
        set
        {
            ObservedSourceNumber_DB = value.Value;
            OnPropertyChanged(nameof(ObservedSourceNumber));
        }
    }
    //If change this change validation

    private void ObservedSourceNumberValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            ObservedSourceNumber_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool ObservedSourceNumber_Validation(RamAccess<string> value)//Ready
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

    #region  RadionuclidName (3)

    public string RadionuclidName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-3", "Наименование радионуклида", "3")]
    public RamAccess<string> RadionuclidName
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(RadionuclidName), out var value))
            {
                ((RamAccess<string>)value).Value = RadionuclidName_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(RadionuclidName_Validation, RadionuclidName_DB);
            rm.PropertyChanged += RadionuclidNameValueChanged;
            Dictionary.Add(nameof(RadionuclidName), rm);
            return (RamAccess<string>)Dictionary[nameof(RadionuclidName)];
        }
        set
        {
            RadionuclidName_DB = value.Value;
            OnPropertyChanged(nameof(RadionuclidName));
        }
    }

    private void RadionuclidNameValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            RadionuclidName_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool RadionuclidName_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var tmpstr = value.Value.ToLower().Replace(" ", "");
        if (!Spravochniks.SprRadionuclids
                .Where(item => item.Item1 == tmpstr)
                .Select(item => item.Item1)
                .Any())
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  AllowedWasteValue (4)

    public string AllowedWasteValue_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Выброс радионуклида в атмосферу за отчетный год, Бк", "разрешенный", "4")]
    public RamAccess<string> AllowedWasteValue
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(AllowedWasteValue), out var value))
            {
                ((RamAccess<string>)value).Value = AllowedWasteValue_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(AllowedWasteValue_Validation, AllowedWasteValue_DB);
            rm.PropertyChanged += AllowedWasteValueValueChanged;
            Dictionary.Add(nameof(AllowedWasteValue), rm);
            return (RamAccess<string>)Dictionary[nameof(AllowedWasteValue)];
        }
        set
        {
            AllowedWasteValue_DB = value.Value;
            OnPropertyChanged(nameof(AllowedWasteValue));
        }
    }

    private void AllowedWasteValueValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                AllowedWasteValue_DB = value1;
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
        AllowedWasteValue_DB = value1;
    }

    private bool AllowedWasteValue_Validation(RamAccess<string> value)
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
        if (!double.TryParse(value1, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, 
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

    #region  FactedWasteValue (5)

    public string FactedWasteValue_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Выброс радионуклида в атмосферу за отчетный год, Бк", "фактический", "5")]
    public RamAccess<string> FactedWasteValue
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(FactedWasteValue), out var value))
            {
                ((RamAccess<string>)value).Value = FactedWasteValue_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(FactedWasteValue_Validation, FactedWasteValue_DB);
            rm.PropertyChanged += FactedWasteValueValueChanged;
            Dictionary.Add(nameof(FactedWasteValue), rm);
            return (RamAccess<string>)Dictionary[nameof(FactedWasteValue)];
        }
        set
        {
            FactedWasteValue_DB = value.Value;
            OnPropertyChanged(nameof(FactedWasteValue));
        }
    }

    private void FactedWasteValueValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)Value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                FactedWasteValue_DB = value1;
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
        FactedWasteValue_DB = value1;
    }

    private bool FactedWasteValue_Validation(RamAccess<string> value)
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
        if (!double.TryParse(value1, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, 
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

    #region  WasteOutbreakPreviousYear (6)

    public string WasteOutbreakPreviousYear_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Выброс радионуклида в атмосферу за предыдущий год, Бк", "фактический", "6")]
    public RamAccess<string> WasteOutbreakPreviousYear
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(WasteOutbreakPreviousYear), out var value))
            {
                ((RamAccess<string>)value).Value = WasteOutbreakPreviousYear_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(WasteOutbreakPreviousYear_Validation, WasteOutbreakPreviousYear_DB);
            rm.PropertyChanged += WasteOutbreakPreviousYearValueChanged;
            Dictionary.Add(nameof(WasteOutbreakPreviousYear), rm);
            return (RamAccess<string>)Dictionary[nameof(WasteOutbreakPreviousYear)];
        }
        set
        {
            WasteOutbreakPreviousYear_DB = value.Value;
            OnPropertyChanged(nameof(WasteOutbreakPreviousYear));
        }
    }

    private void WasteOutbreakPreviousYearValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                WasteOutbreakPreviousYear_DB = value1;
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
        WasteOutbreakPreviousYear_DB = value1;
    }

    private bool WasteOutbreakPreviousYear_Validation(RamAccess<string> value)
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
        if (!double.TryParse(value1, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, 
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
    
    #endregion

    #region IExcel

    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        base.ExcelGetRow(worksheet, row);
        ObservedSourceNumber_DB = Convert.ToString(worksheet.Cells[row, 2].Value);
        RadionuclidName_DB = Convert.ToString(worksheet.Cells[row, 3].Value);
        AllowedWasteValue_DB = ConvertFromExcelDouble(worksheet.Cells[row, 4].Value);
        FactedWasteValue_DB = ConvertFromExcelDouble(worksheet.Cells[row, 5].Value);
        WasteOutbreakPreviousYear_DB = ConvertFromExcelDouble(worksheet.Cells[row, 6].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ConvertToExcelString(ObservedSourceNumber_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(RadionuclidName_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelDouble(AllowedWasteValue_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelDouble(FactedWasteValue_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelDouble(WasteOutbreakPreviousYear_DB);
        return 5;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form2.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form27,Models")?.GetProperty(nameof(ObservedSourceNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form27,Models")?.GetProperty(nameof(RadionuclidName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form27,Models")?.GetProperty(nameof(AllowedWasteValue))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form27,Models")?.GetProperty(nameof(FactedWasteValue))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form27,Models")?.GetProperty(nameof(WasteOutbreakPreviousYear))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        
        return 5;
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

        #region ObservedSourceNumber (2)

        var observedSourceNumberR = ((FormPropertyAttribute)typeof(Form27)
                .GetProperty(nameof(ObservedSourceNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (observedSourceNumberR != null)
        {
            observedSourceNumberR.SetSizeColToAllLevels(228);
            observedSourceNumberR.Binding = nameof(ObservedSourceNumber);
            numberInOrderR += observedSourceNumberR;
        }

        #endregion

        #region RadionuclidName (3)

        var radionuclidNameR = ((FormPropertyAttribute)typeof(Form27)
                .GetProperty(nameof(RadionuclidName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (radionuclidNameR != null)
        {
            radionuclidNameR.SetSizeColToAllLevels(183);
            radionuclidNameR.Binding = nameof(RadionuclidName);
            numberInOrderR += radionuclidNameR;
        }

        #endregion

        #region AllowedWasteValue (4)

        var allowedWasteValueR = ((FormPropertyAttribute)typeof(Form27)
                .GetProperty(nameof(AllowedWasteValue))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (allowedWasteValueR != null)
        {
            allowedWasteValueR.SetSizeColToAllLevels(170);
            allowedWasteValueR.Binding = nameof(AllowedWasteValue);
            numberInOrderR += allowedWasteValueR;
        }

        #endregion

        #region FactedWasteValue (5)

        var factedWasteValueR = ((FormPropertyAttribute)typeof(Form27)
                .GetProperty(nameof(FactedWasteValue))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (factedWasteValueR != null)
        {
            factedWasteValueR.SetSizeColToAllLevels(170);
            factedWasteValueR.Binding = nameof(FactedWasteValue);
            numberInOrderR += factedWasteValueR;
        }

        #endregion

        #region WasteOutbreakPreviousYear (6)

        var wasteOutbreakPreviousYearR = ((FormPropertyAttribute)typeof(Form27)
                .GetProperty(nameof(WasteOutbreakPreviousYear))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (wasteOutbreakPreviousYearR != null)
        {
            wasteOutbreakPreviousYearR.SetSizeColToAllLevels(363);
            wasteOutbreakPreviousYearR.Binding = nameof(WasteOutbreakPreviousYear);
            numberInOrderR += wasteOutbreakPreviousYearR;
        }

        #endregion

        _DataGridColumns = numberInOrderR;

        return _DataGridColumns;
    }

    #endregion
}