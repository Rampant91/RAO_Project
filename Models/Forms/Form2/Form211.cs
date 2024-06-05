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

namespace Models.Forms.Form2;

[Serializable]
[Form_Class("Форма 2.11: Радионуклидный состав загрязненных участков территорий")]
[Table (name: "form_211")]
public partial class Form211 : Form2
{
    #region Cobstructor
    
    public Form211()
    {
        FormNum.Value = "2.11";
        //NumberOfFields.Value = 11;
        Validate_all();
    }

    #endregion

    #region Validation
    
    private void Validate_all()
    {
        Radionuclids_Validation(Radionuclids);
        PlotName_Validation(PlotName);
        PlotKadastrNumber_Validation(PlotKadastrNumber);
        PlotCode_Validation(PlotCode);
        InfectedArea_Validation(InfectedArea);
        SpecificActivityOfPlot_Validation(SpecificActivityOfPlot);
        SpecificActivityOfLiquidPart_Validation(SpecificActivityOfLiquidPart);
        SpecificActivityOfDensePart_Validation(SpecificActivityOfDensePart);
    }

    [FormProperty(true, "Форма")]
    public override bool Object_Validation()
    {
        return !(Radionuclids.HasErrors ||
                 PlotName.HasErrors ||
                 PlotKadastrNumber.HasErrors ||
                 PlotCode.HasErrors ||
                 InfectedArea.HasErrors ||
                 SpecificActivityOfPlot.HasErrors ||
                 SpecificActivityOfLiquidPart.HasErrors ||
                 SpecificActivityOfDensePart.HasErrors);
    }

    #endregion

    #region Properties
    
    #region  PlotName (2)

    public string PlotName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-1", "null-2", "Наименование участка", "2")]
    public RamAccess<string> PlotName
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(PlotName), out var value))
            {
                ((RamAccess<string>)value).Value = PlotName_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(PlotName_Validation, PlotName_DB);
            rm.PropertyChanged += PlotNameValueChanged;
            Dictionary.Add(nameof(PlotName), rm);
            return (RamAccess<string>)Dictionary[nameof(PlotName)];
        }
        set
        {
            PlotName_DB = value.Value;
            OnPropertyChanged(nameof(PlotName));
        }
    }

    private void PlotNameValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PlotName_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool PlotName_Validation(RamAccess<string> value)//TODO
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

    #region  PlotKadastrNumber (3)

    public string PlotKadastrNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-1", "null-2", "Кадастровый номер участка", "3")]
    public RamAccess<string> PlotKadastrNumber
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(PlotKadastrNumber), out var value))
            {
                ((RamAccess<string>)value).Value = PlotKadastrNumber_DB;
                return (RamAccess<string>)value;
            }

            var rm = new RamAccess<string>(PlotKadastrNumber_Validation, PlotKadastrNumber_DB);
            rm.PropertyChanged += PlotKadastrNumberValueChanged;
            Dictionary.Add(nameof(PlotKadastrNumber), rm);
            return (RamAccess<string>)Dictionary[nameof(PlotKadastrNumber)];
        }
        set
        {
            PlotKadastrNumber_DB = value.Value;
            OnPropertyChanged(nameof(PlotKadastrNumber));
        }
    }

    private void PlotKadastrNumberValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PlotKadastrNumber_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool PlotKadastrNumber_Validation(RamAccess<string> value)//TODO
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

    #region  PlotCode (4)

    public string PlotCode_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-1", "null-2", "Код участка", "4")]
    public RamAccess<string> PlotCode
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(PlotCode), out var value))
            {
                ((RamAccess<string>)value).Value = PlotCode_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(PlotCode_Validation, PlotCode_DB);
            rm.PropertyChanged += PlotCodeValueChanged;
            Dictionary.Add(nameof(PlotCode), rm);
            return (RamAccess<string>)Dictionary[nameof(PlotCode)];
        }
        set
        {
            PlotCode_DB = value.Value;
            OnPropertyChanged(nameof(PlotCode));
        }
    }
    //6 symbols code

    private void PlotCodeValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PlotCode_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool PlotCode_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (!SixNumRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  InfectedArea (5)

    public string InfectedArea_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "null-1", "null-2", "Площадь загрязненной территории, кв. м", "5")]
    public RamAccess<string> InfectedArea
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(InfectedArea), out var value))
            {
                ((RamAccess<string>)value).Value = InfectedArea_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(InfectedArea_Validation, InfectedArea_DB);
            rm.PropertyChanged += InfectedAreaValueChanged;
            Dictionary.Add(nameof(InfectedArea), rm);
            return (RamAccess<string>)Dictionary[nameof(InfectedArea)];
        }
        set
        {
            InfectedArea_DB = value.Value;
            OnPropertyChanged(nameof(InfectedArea));
        }
    }

    private void InfectedAreaValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1
                .Trim()
                .TrimStart('(')
                .TrimEnd(')')
                .ToLower()
                .Replace('.', ',')
                .Replace('е', 'e');
            if (value1.Equals("-"))
            {
                InfectedArea_DB = value1;
                return;
            }
            if (double.TryParse(value1, 
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                    CultureInfo.CreateSpecificCulture("ru-RU"), 
                    out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        InfectedArea_DB = value1;
    }

    private bool InfectedArea_Validation(RamAccess<string> value)//TODO
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

    #region  Radionuclids (6)

    public string Radionuclids_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-1", "null-2", "Наименования радионуклидов", "6")]
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
            rm.PropertyChanged += RadionuclidsValueChanged;
            Dictionary.Add(nameof(Radionuclids), rm);
            return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
        }
        set
        {
            Radionuclids_DB = value.Value;
            OnPropertyChanged(nameof(Radionuclids));
        }
    }
    //If change this change validation

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
        value.Value.Replace(" ", "");
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var nuclids = value.Value.Split("; ");
        var flag = true;
        foreach (var nucl in nuclids)
        {
            if (!Spravochniks.SprRadionuclids
                    .Where(item => nucl == item.Item1)
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

    #region  SpecificActivityOfPlot (7)

    public string SpecificActivityOfPlot_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Удельная активность радионуклида, Бк/г", "null-3", "земельный участок", "7")]
    public RamAccess<string> SpecificActivityOfPlot
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(SpecificActivityOfPlot), out var value))
            {
                ((RamAccess<string>)value).Value = SpecificActivityOfPlot_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(SpecificActivityOfPlot_Validation, SpecificActivityOfPlot_DB);
            rm.PropertyChanged += SpecificActivityOfPlotValueChanged;
            Dictionary.Add(nameof(SpecificActivityOfPlot), rm);
            return (RamAccess<string>)Dictionary[nameof(SpecificActivityOfPlot)];
        }
        set
        {
            SpecificActivityOfPlot_DB = value.Value;
            OnPropertyChanged(nameof(SpecificActivityOfPlot));
        }
    }

    private void SpecificActivityOfPlotValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1
                .Trim()
                .TrimStart('(')
                .TrimEnd(')')
                .ToLower()
                .Replace('.', ',')
                .Replace('е', 'e');
            if (value1.Equals("-"))
            {
                SpecificActivityOfPlot_DB = value1;
                return;
            }
            if (double.TryParse(value1, 
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                    CultureInfo.CreateSpecificCulture("ru-RU"), 
                    out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        SpecificActivityOfPlot_DB = value1;
    }

    private bool SpecificActivityOfPlot_Validation(RamAccess<string> value)//TODO
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

    #region  SpecificActivityOfLiquidPart (8)

    public string SpecificActivityOfLiquidPart_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Удельная активность радионуклида, Бк/г", "водный объект", "жидкая фаза", "8")]
    public RamAccess<string> SpecificActivityOfLiquidPart
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(SpecificActivityOfLiquidPart), out var value))
            {
                ((RamAccess<string>)value).Value = SpecificActivityOfLiquidPart_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(SpecificActivityOfLiquidPart_Validation, SpecificActivityOfLiquidPart_DB);
            rm.PropertyChanged += SpecificActivityOfLiquidPartValueChanged;
            Dictionary.Add(nameof(SpecificActivityOfLiquidPart), rm);
            return (RamAccess<string>)Dictionary[nameof(SpecificActivityOfLiquidPart)];
        }
        set
        {
            SpecificActivityOfLiquidPart_DB = value.Value;
            OnPropertyChanged(nameof(SpecificActivityOfLiquidPart));
        }
    }

    private void SpecificActivityOfLiquidPartValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1
                .Trim()
                .TrimStart('(')
                .TrimEnd(')')
                .ToLower()
                .Replace('.', ',')
                .Replace('е', 'e');
            if (value1.Equals("-"))
            {
                SpecificActivityOfLiquidPart_DB = value1;
                return;
            }
            if (double.TryParse(value1, 
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                    CultureInfo.CreateSpecificCulture("ru-RU"), 
                    out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        SpecificActivityOfLiquidPart_DB = value1;
    }

    private bool SpecificActivityOfLiquidPart_Validation(RamAccess<string> value)//TODO
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

    #region SpecificActivityOfDensePart (9)

    public string SpecificActivityOfDensePart_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Удельная активность радионуклида, Бк/г", "водный объект", "донные отложения", "9")]
    public RamAccess<string> SpecificActivityOfDensePart
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(SpecificActivityOfDensePart), out var value))
            {
                ((RamAccess<string>)value).Value = SpecificActivityOfDensePart_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(SpecificActivityOfDensePart_Validation, SpecificActivityOfDensePart_DB);
            rm.PropertyChanged += SpecificActivityOfDensePartValueChanged;
            Dictionary.Add(nameof(SpecificActivityOfDensePart), rm);
            return (RamAccess<string>)Dictionary[nameof(SpecificActivityOfDensePart)];
        }
        set
        {
            SpecificActivityOfDensePart_DB = value.Value;
            OnPropertyChanged(nameof(SpecificActivityOfDensePart));
        }
    }

    private void SpecificActivityOfDensePartValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1
                .Trim()
                .TrimStart('(')
                .TrimEnd(')')
                .ToLower()
                .Replace('.', ',')
                .Replace('е', 'e');
            if (value1.Equals("-"))
            {
                SpecificActivityOfDensePart_DB = value1;
                return;
            }
            if (double.TryParse(value1, 
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                    CultureInfo.CreateSpecificCulture("ru-RU"), 
                    out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        SpecificActivityOfDensePart_DB = value1;
    }

    private bool SpecificActivityOfDensePart_Validation(RamAccess<string> value)//TODO
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
    
    #endregion

    #region IExcel

    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        base.ExcelGetRow(worksheet, row);
        PlotName_DB = Convert.ToString(worksheet.Cells[row, 2].Value);
        PlotKadastrNumber_DB = Convert.ToString(worksheet.Cells[row, 3].Value);
        PlotCode_DB = Convert.ToString(worksheet.Cells[row, 4].Value);
        InfectedArea_DB = ConvertFromExcelDouble(worksheet.Cells[row, 5].Value);
        Radionuclids_DB = Convert.ToString(worksheet.Cells[row, 6].Value);
        SpecificActivityOfPlot_DB = ConvertFromExcelDouble(worksheet.Cells[row, 7].Value);
        SpecificActivityOfLiquidPart_DB = ConvertFromExcelDouble(worksheet.Cells[row, 8].Value);
        SpecificActivityOfDensePart_DB = ConvertFromExcelDouble(worksheet.Cells[row, 9].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ConvertToExcelString(PlotName_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(PlotKadastrNumber_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelString(PlotCode_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelDouble(InfectedArea_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelString(Radionuclids_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelDouble(SpecificActivityOfPlot_DB);
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDouble(SpecificActivityOfLiquidPart_DB);
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelDouble(SpecificActivityOfDensePart_DB);
        
        return 8;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form2.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form211,Models")?.GetProperty(nameof(PlotName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form211,Models")?.GetProperty(nameof(PlotKadastrNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form211,Models")?.GetProperty(nameof(PlotCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form211,Models")?.GetProperty(nameof(InfectedArea))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form211,Models")?.GetProperty(nameof(Radionuclids))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form211,Models")?.GetProperty(nameof(SpecificActivityOfPlot))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form211,Models")?.GetProperty(nameof(SpecificActivityOfLiquidPart))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form211,Models")?.GetProperty(nameof(SpecificActivityOfDensePart))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];

        return 8;
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

        #region PlotName (2)

        var plotNameR = ((FormPropertyAttribute)typeof(Form211)
                .GetProperty(nameof(PlotName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (plotNameR != null)
        {
            plotNameR.SetSizeColToAllLevels(163);
            plotNameR.Binding = nameof(PlotName);
            numberInOrderR += plotNameR;
        }

        #endregion

        #region PlotKadastrNumber (3)

        var plotKadastrNumberR = ((FormPropertyAttribute)typeof(Form211)
                .GetProperty(nameof(PlotKadastrNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (plotKadastrNumberR != null)
        {
            plotKadastrNumberR.SetSizeColToAllLevels(173);
            plotKadastrNumberR.Binding = nameof(PlotKadastrNumber);
            numberInOrderR += plotKadastrNumberR;
        }

        #endregion

        #region PlotCode (4)

        var plotCodeR = ((FormPropertyAttribute)typeof(Form211)
                .GetProperty(nameof(PlotCode))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (plotCodeR != null)
        {
            plotCodeR.SetSizeColToAllLevels(88);
            plotCodeR.Binding = nameof(PlotCode);
            numberInOrderR += plotCodeR;
        }

        #endregion

        #region InfectedArea (5)

        var infectedAreaR = ((FormPropertyAttribute)typeof(Form211)
                .GetProperty(nameof(InfectedArea))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (infectedAreaR != null)
        {
            infectedAreaR.SetSizeColToAllLevels(248);
            infectedAreaR.Binding = nameof(InfectedArea);
            numberInOrderR += infectedAreaR;
        }

        #endregion

        #region Radionuclids (6)

        var radionuclidsR = ((FormPropertyAttribute)typeof(Form211)
                .GetProperty(nameof(Radionuclids))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (radionuclidsR != null)
        {
            radionuclidsR.SetSizeColToAllLevels(188);
            radionuclidsR.Binding = nameof(Radionuclids);
            numberInOrderR += radionuclidsR;
        }

        #endregion

        #region SpecificActivityOfPlot (7)

        var specificActivityOfPlotR = ((FormPropertyAttribute)typeof(Form211)
                .GetProperty(nameof(SpecificActivityOfPlot))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (specificActivityOfPlotR != null)
        {
            specificActivityOfPlotR.SetSizeColToAllLevels(155);
            specificActivityOfPlotR.Binding = nameof(SpecificActivityOfPlot);
            numberInOrderR += specificActivityOfPlotR;
        }

        #endregion

        #region SpecificActivityOfLiquidPart (8)

        var specificActivityOfLiquidPartR = ((FormPropertyAttribute)typeof(Form211)
                .GetProperty(nameof(SpecificActivityOfLiquidPart))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        specificActivityOfLiquidPartR.SetSizeColToAllLevels(176);
        specificActivityOfLiquidPartR.Binding = nameof(SpecificActivityOfLiquidPart);
        numberInOrderR += specificActivityOfLiquidPartR;
        
        #endregion

        #region SpecificActivityOfDensePart (9)

        var specificActivityOfDensePartR = ((FormPropertyAttribute)typeof(Form211)
                .GetProperty(nameof(SpecificActivityOfDensePart))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        specificActivityOfDensePartR.SetSizeColToAllLevels(176);
        specificActivityOfDensePartR.Binding = nameof(SpecificActivityOfDensePart);
        numberInOrderR += specificActivityOfDensePartR;
        
        #endregion

        _DataGridColumns = numberInOrderR;

        return _DataGridColumns;
    }

    #endregion

    #region GeneratedRegex
    
    [GeneratedRegex("^[0-9]{6}$")]
    private static partial Regex SixNumRegex();
    
    #endregion
}