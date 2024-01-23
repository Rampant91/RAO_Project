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
[Form_Class("Форма 2.9: Активность радионуклидов, отведенных со сточными водами")]
public class Form29 : Form2
{
    #region Constructor

    public Form29()
    {
        FormNum.Value = "2.9";
        //NumberOfFields.Value = 8;
        Validate_all();
    }

    #endregion

    #region Validation
    
    private void Validate_all()
    {
        WasteSourceName_Validation(WasteSourceName);
        RadionuclidName_Validation(RadionuclidName);
        AllowedActivity_Validation(AllowedActivity);
        FactedActivity_Validation(FactedActivity);
    }

    [FormProperty(true, "Форма")]
    public override bool Object_Validation()
    {
        return !(WasteSourceName.HasErrors ||
                 RadionuclidName.HasErrors ||
                 AllowedActivity.HasErrors ||
                 FactedActivity.HasErrors);
    }

    #endregion

    #region Properties

    #region WasteSourceName (2)

    public string WasteSourceName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-2", "Наименование, номер выпуска сточных вод", "2")]
    public RamAccess<string> WasteSourceName
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(WasteSourceName), out var value))
            {
                ((RamAccess<string>)value).Value = WasteSourceName_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(WasteSourceName_Validation, WasteSourceName_DB);
            rm.PropertyChanged += WasteSourceNameValueChanged;
            Dictionary.Add(nameof(WasteSourceName), rm);
            return (RamAccess<string>)Dictionary[nameof(WasteSourceName)];
        }
        set
        {
            WasteSourceName_DB = value.Value;
            OnPropertyChanged(nameof(WasteSourceName));
        }
    }

    private void WasteSourceNameValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            WasteSourceName_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool WasteSourceName_Validation(RamAccess<string> value)
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

    #region RadionuclidName (3)

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
    //If change this change validation

    private void RadionuclidNameValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            RadionuclidName_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool RadionuclidName_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var tmpstr = value.Value.ToLower().Replace(" ", "");
        if (!Spravochniks.SprRadionuclids
                .Where(item => tmpstr == item.Item1)
                .Select(item => item.Item1)
                .Any())
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region AllowedActivity (4)

    public string AllowedActivity_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Активность радионуклида, Бк", "допустимая", "4")]
    public RamAccess<string> AllowedActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(AllowedActivity), out var value))
            {
                ((RamAccess<string>)value).Value = AllowedActivity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(AllowedActivity_Validation, AllowedActivity_DB);
            rm.PropertyChanged += AllowedActivityValueChanged;
            Dictionary.Add(nameof(AllowedActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(AllowedActivity)];
        }
        set
        {
            AllowedActivity_DB = value.Value;
            OnPropertyChanged(nameof(AllowedActivity));
        }
    }

    private void AllowedActivityValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                AllowedActivity_DB = value1;
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
        AllowedActivity_DB = value1;
    }

    private bool AllowedActivity_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value == "прим.")
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
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

    #region FactedActivity (5)

    public string FactedActivity_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Активность радионуклида, Бк", "фактическая", "5")]
    public RamAccess<string> FactedActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(FactedActivity), out var value))
            {
                ((RamAccess<string>)value).Value = FactedActivity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(FactedActivity_Validation, FactedActivity_DB);
            rm.PropertyChanged += FactedActivityValueChanged;
            Dictionary.Add(nameof(FactedActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(FactedActivity)];
        }
        set
        {
            FactedActivity_DB = value.Value;
            OnPropertyChanged(nameof(FactedActivity));
        }
    }

    private void FactedActivityValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)Value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                FactedActivity_DB = value1;
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
        FactedActivity_DB = value1;
    }

    private bool FactedActivity_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
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
        WasteSourceName_DB = Convert.ToString(worksheet.Cells[row, 2].Value);
        RadionuclidName_DB = Convert.ToString(worksheet.Cells[row, 3].Value);
        AllowedActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 4].Value);
        FactedActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 5].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ConvertToExcelString(WasteSourceName_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(RadionuclidName_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelDouble(AllowedActivity_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelDouble(FactedActivity_DB);

        return 4;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form2.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form29,Models")?.GetProperty(nameof(WasteSourceName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form29,Models")?.GetProperty(nameof(RadionuclidName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form29,Models")?.GetProperty(nameof(AllowedActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form29,Models")?.GetProperty(nameof(FactedActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        
        return 4;
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

        #region WasteSourceName (2)

        var wasteSourceNameR = ((FormPropertyAttribute)typeof(Form29)
                .GetProperty(nameof(WasteSourceName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (wasteSourceNameR != null)
        {
            wasteSourceNameR.SetSizeColToAllLevels(268);
            wasteSourceNameR.Binding = nameof(WasteSourceName);
            numberInOrderR += wasteSourceNameR;
        }

        #endregion

        #region RadionuclidName (3)

        var radionuclidNameR = ((FormPropertyAttribute)typeof(Form29)
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

        #region AllowedActivity (4)

        var allowedActivityR = ((FormPropertyAttribute)typeof(Form29)
                .GetProperty(nameof(AllowedActivity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (allowedActivityR != null)
        {
            allowedActivityR.SetSizeColToAllLevels(94);
            allowedActivityR.Binding = nameof(AllowedActivity);
            numberInOrderR += allowedActivityR;
        }

        #endregion

        #region FactedActivity (5)

        var factedActivityR = ((FormPropertyAttribute)typeof(Form29)
                .GetProperty(nameof(FactedActivity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (factedActivityR != null)
        {
            factedActivityR.SetSizeColToAllLevels(94);
            factedActivityR.Binding = nameof(FactedActivity);
            numberInOrderR += factedActivityR;
        }

        #endregion

        _DataGridColumns = numberInOrderR;

        return _DataGridColumns;
    }

    #endregion
}