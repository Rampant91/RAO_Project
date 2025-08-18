using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;

namespace Models.Forms.Form2;

[Serializable]
[Form_Class("Форма 2.10: Территории, загрязненные радионуклидами")]
[Table (name: "form_210")]
public partial class Form210 : Form2
{
    #region Constructor
    
    public Form210()
    {
        FormNum.Value = "2.10";
        //NumberOfFields.Value = 12;
        Validate_all();
    }

    #endregion

    #region Validation
    
    private void Validate_all()
    {
        IndicatorName_Validation(IndicatorName);
        PlotName_Validation(PlotName);
        PlotKadastrNumber_Validation(PlotKadastrNumber);
        PlotCode_Validation(PlotCode);
        InfectedArea_Validation(InfectedArea);
        AvgGammaRaysDosePower_Validation(AvgGammaRaysDosePower);
        MaxGammaRaysDosePower_Validation(MaxGammaRaysDosePower);
        WasteDensityAlpha_Validation(WasteDensityAlpha);
        WasteDensityBeta_Validation(WasteDensityBeta);
        FcpNumber_Validation(FcpNumber);
    }

    [FormProperty(true, "Форма")]
    public override bool Object_Validation()
    {
        return !(IndicatorName.HasErrors ||
                 PlotName.HasErrors ||
                 PlotKadastrNumber.HasErrors ||
                 PlotCode.HasErrors ||
                 InfectedArea.HasErrors ||
                 AvgGammaRaysDosePower.HasErrors ||
                 MaxGammaRaysDosePower.HasErrors ||
                 WasteDensityAlpha.HasErrors ||
                 WasteDensityBeta.HasErrors ||
                 FcpNumber.HasErrors);
    }

    #endregion

    #region Properties
    
    #region  IndicatorName (2)

    public string IndicatorName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-2", "Наименование показателя", "2")]
    public RamAccess<string> IndicatorName
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(IndicatorName), out var value))
            {
                ((RamAccess<string>)value).Value = IndicatorName_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(IndicatorName_Validation, IndicatorName_DB);
            rm.PropertyChanged += IndicatorName_ValueChanged;
            Dictionary.Add(nameof(IndicatorName), rm);
            return (RamAccess<string>)Dictionary[nameof(IndicatorName)];
        }
        set
        {
            IndicatorName_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void IndicatorName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        IndicatorName_DB = ((RamAccess<string>)value).Value;
    }

    private bool IndicatorName_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (!new[] { "З", "Р", "Н" }.Contains(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  PlotName (3)

    public string PlotName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-3", "Наименование участка", "3")]
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
            rm.PropertyChanged += PlotName_ValueChanged;
            Dictionary.Add(nameof(PlotName), rm);
            return (RamAccess<string>)Dictionary[nameof(PlotName)];
        }
        set
        {
            PlotName_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void PlotName_ValueChanged(object value, PropertyChangedEventArgs args)
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

    #region PlotKadastrNumber (4)

    public string PlotKadastrNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-4", "Кадастровый номер участка", "4")]
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
            rm.PropertyChanged += PlotKadastrNumber_ValueChanged;
            Dictionary.Add(nameof(PlotKadastrNumber), rm);
            return (RamAccess<string>)Dictionary[nameof(PlotKadastrNumber)];
        }
        set
        {
            PlotKadastrNumber_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void PlotKadastrNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        PlotKadastrNumber_DB = ((RamAccess<string>)value).Value;
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

    #region  PlotCode (5)

    public string PlotCode_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-5", "Код участка", "5")]
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
            rm.PropertyChanged += PlotCode_ValueChanged;
            Dictionary.Add(nameof(PlotCode), rm);
            return (RamAccess<string>)Dictionary[nameof(PlotCode)];
        }
        set
        {
            PlotCode_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void PlotCode_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        PlotCode_DB = ((RamAccess<string>)value).Value;
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

    #region  InfectedArea (6)

    public string InfectedArea_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "null-6", "Площадь загрязненной территории, кв. м", "6")]
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
            rm.PropertyChanged += InfectedArea_ValueChanged;
            Dictionary.Add(nameof(InfectedArea), rm);
            return (RamAccess<string>)Dictionary[nameof(InfectedArea)];
        }
        set
        {
            InfectedArea_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void InfectedArea_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        InfectedArea_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool InfectedArea_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region  AvgGammaRaysDosePower (7)

    public string AvgGammaRaysDosePower_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Мощность дозы гамма-излучения, мкЗв/час", "средняя", "7")]
    public RamAccess<string> AvgGammaRaysDosePower
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(AvgGammaRaysDosePower), out var value))
            {
                ((RamAccess<string>)value).Value = AvgGammaRaysDosePower_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(AvgGammaRaysDosePower_Validation, AvgGammaRaysDosePower_DB);
            rm.PropertyChanged += AvgGammaRaysDosePower_ValueChanged;
            Dictionary.Add(nameof(AvgGammaRaysDosePower), rm);
            return (RamAccess<string>)Dictionary[nameof(AvgGammaRaysDosePower)];
        }
        set
        {
            AvgGammaRaysDosePower_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void AvgGammaRaysDosePower_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        AvgGammaRaysDosePower_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool AvgGammaRaysDosePower_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region  MaxGammaRaysDosePower (8)

    public string MaxGammaRaysDosePower_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Мощность дозы гамма-излучения, мкЗв/час", "максимальная", "8")]
    public RamAccess<string> MaxGammaRaysDosePower
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MaxGammaRaysDosePower), out var value))
            {
                ((RamAccess<string>)value).Value = MaxGammaRaysDosePower_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(MaxGammaRaysDosePower_Validation, MaxGammaRaysDosePower_DB);
            rm.PropertyChanged += MaxGammaRaysDosePower_ValueChanged;
            Dictionary.Add(nameof(MaxGammaRaysDosePower), rm);
            return (RamAccess<string>)Dictionary[nameof(MaxGammaRaysDosePower)];
        }
        set
        {
            MaxGammaRaysDosePower_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void MaxGammaRaysDosePower_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        MaxGammaRaysDosePower_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool MaxGammaRaysDosePower_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region  WasteDensityAlpha (9)

    public string WasteDensityAlpha_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Плотность загрязнения (средняя), Бк/кв.м", "альфа-излучающие радионуклиды", "9")]
    public RamAccess<string> WasteDensityAlpha
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(WasteDensityAlpha), out var value))
            {
                ((RamAccess<string>)value).Value = WasteDensityAlpha_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(WasteDensityAlpha_Validation, WasteDensityAlpha_DB);
            rm.PropertyChanged += WasteDensityAlpha_ValueChanged;
            Dictionary.Add(nameof(WasteDensityAlpha), rm);
            return (RamAccess<string>)Dictionary[nameof(WasteDensityAlpha)];
        }
        set
        {
            WasteDensityAlpha_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void WasteDensityAlpha_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        WasteDensityAlpha_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool WasteDensityAlpha_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region  WasteDensityBeta (10)

    public string WasteDensityBeta_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Плотность загрязнения (средняя), Бк/кв.м", "бета-излучающие радионуклиды", "10")]
    public RamAccess<string> WasteDensityBeta
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(WasteDensityBeta), out var value))
            {
                ((RamAccess<string>)value).Value = WasteDensityBeta_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(WasteDensityBeta_Validation, WasteDensityBeta_DB);
            rm.PropertyChanged += WasteDensityBeta_ValueChanged;
            Dictionary.Add(nameof(WasteDensityBeta), rm);
            return (RamAccess<string>)Dictionary[nameof(WasteDensityBeta)];
        }
        set
        {
            WasteDensityBeta_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void WasteDensityBeta_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        WasteDensityBeta_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool WasteDensityBeta_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region  FcpNumber (11)

    public string FcpNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-7", "Номер мероприятия ФЦП", "11")]
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
        FcpNumber_DB = ((RamAccess<string>)value).Value;
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
        IndicatorName_DB = Convert.ToString(worksheet.Cells[row, 2].Value);
        PlotName_DB = Convert.ToString(worksheet.Cells[row, 3].Value);
        PlotKadastrNumber_DB = Convert.ToString(worksheet.Cells[row, 4].Value);
        PlotCode_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        InfectedArea_DB = ConvertFromExcelDouble(worksheet.Cells[row, 6].Value);
        AvgGammaRaysDosePower_DB = ConvertFromExcelDouble(worksheet.Cells[row, 7].Value);
        MaxGammaRaysDosePower_DB = ConvertFromExcelDouble(worksheet.Cells[row, 8].Value);
        WasteDensityAlpha_DB = ConvertFromExcelDouble(worksheet.Cells[row, 9].Value);
        WasteDensityBeta_DB = ConvertFromExcelDouble(worksheet.Cells[row, 10].Value);
        FcpNumber_DB = Convert.ToString(worksheet.Cells[row, 11].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ConvertToExcelString(IndicatorName_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(PlotName_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelString(PlotKadastrNumber_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelString(PlotCode_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelDouble(InfectedArea_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelDouble(AvgGammaRaysDosePower_DB);
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDouble(MaxGammaRaysDosePower_DB);
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelDouble(WasteDensityAlpha_DB);
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ConvertToExcelDouble(WasteDensityBeta_DB);
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ConvertToExcelString(FcpNumber_DB);
        
        return 10;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form2.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form210,Models")?.GetProperty(nameof(IndicatorName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form210,Models")?.GetProperty(nameof(PlotName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form210,Models")?.GetProperty(nameof(PlotKadastrNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form210,Models")?.GetProperty(nameof(PlotCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form210,Models")?.GetProperty(nameof(InfectedArea))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form210,Models")?.GetProperty(nameof(AvgGammaRaysDosePower))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form210,Models")?.GetProperty(nameof(MaxGammaRaysDosePower))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form210,Models")?.GetProperty(nameof(WasteDensityAlpha))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form210,Models")?.GetProperty(nameof(WasteDensityBeta))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form210,Models")?.GetProperty(nameof(FcpNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        
        return 10;
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

        #region IndicatorName (2)

        var indicatorNameR = ((FormPropertyAttribute)typeof(Form210)
                .GetProperty(nameof(IndicatorName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (indicatorNameR != null)
        {
            indicatorNameR.SetSizeColToAllLevels(163);
            indicatorNameR.Binding = nameof(IndicatorName);
            numberInOrderR += indicatorNameR;
        }

        #endregion

        #region PlotName (3)

        var plotNameR = ((FormPropertyAttribute)typeof(Form210)
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

        #region PlotKadastrNumber (4)

        var plotKadastrNumberR = ((FormPropertyAttribute)typeof(Form210)
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

        #region PlotCode (5)

        var plotCodeR = ((FormPropertyAttribute)typeof(Form210)
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

        #region InfectedArea (6)

        var infectedAreaR = ((FormPropertyAttribute)typeof(Form210)
                .GetProperty(nameof(InfectedArea))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (infectedAreaR != null)
        {
            infectedAreaR.SetSizeColToAllLevels(188);
            infectedAreaR.Binding = nameof(InfectedArea);
            numberInOrderR += infectedAreaR;
        }

        #endregion

        #region AvgGammaRaysDosePower (7)

        var avgGammaRaysDosePowerR = ((FormPropertyAttribute)typeof(Form210)
                .GetProperty(nameof(AvgGammaRaysDosePower))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (avgGammaRaysDosePowerR != null)
        {
            avgGammaRaysDosePowerR.SetSizeColToAllLevels(113);
            avgGammaRaysDosePowerR.Binding = nameof(AvgGammaRaysDosePower);
            numberInOrderR += avgGammaRaysDosePowerR;
        }

        #endregion

        #region MaxGammaRaysDosePower (8)

        var maxGammaRaysDosePowerR = ((FormPropertyAttribute)typeof(Form210)
                .GetProperty(nameof(MaxGammaRaysDosePower))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        maxGammaRaysDosePowerR.SetSizeColToAllLevels(143);
        maxGammaRaysDosePowerR.Binding = nameof(MaxGammaRaysDosePower);
        numberInOrderR += maxGammaRaysDosePowerR;
        
        #endregion

        #region WasteDensityAlpha (9)

        var wasteDensityAlphaR = ((FormPropertyAttribute)typeof(Form210)
                .GetProperty(nameof(WasteDensityAlpha))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        wasteDensityAlphaR.SetSizeColToAllLevels(162);
        wasteDensityAlphaR.Binding = nameof(WasteDensityAlpha);
        numberInOrderR += wasteDensityAlphaR;
        
        #endregion

        #region WasteDensityBeta (10)

        var wasteDensityBetaR = ((FormPropertyAttribute)typeof(Form210)
                .GetProperty(nameof(WasteDensityBeta))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        wasteDensityBetaR.SetSizeColToAllLevels(154);
        wasteDensityBetaR.Binding = nameof(WasteDensityBeta);
        numberInOrderR += wasteDensityBetaR;
        
        #endregion

        #region FcpNumber (11)

        var fcpNumberR = ((FormPropertyAttribute)typeof(Form210)
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
    
    [GeneratedRegex("^[0-9]{6}$")]
    private static partial Regex SixNumRegex();

    #endregion

    #region ConvertToTSVstring
    /// <summary>
    /// </summary>
    /// <returns>Возвращает строку с записанными данными в формате TSV(Tab-Separated Values) </returns>
    public override string ConvertToTSVstring()
    {
        // Создаем текстовое представление (TSV - tab-separated values)
        string str =
            $"{this.NumberInOrder.Value}\t" +
            $"{this.IndicatorName.Value}\t" +
            $"{this.PlotName.Value}\t" +
            $"{this.PlotKadastrNumber.Value}\t" +
            $"{this.PlotCode.Value}\t" +
            $"{this.InfectedArea.Value}\t" +
            $"{this.AvgGammaRaysDosePower.Value}\t" +
            $"{this.MaxGammaRaysDosePower.Value}\t" +
            $"{this.WasteDensityAlpha.Value}\t" +
            $"{this.WasteDensityBeta.Value}\t" +
            $"{this.FcpNumber.Value}";
        return str;
    }
    #endregion
}