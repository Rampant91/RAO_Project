﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;

namespace Models.Forms.Form2;

[Serializable]
[Form_Class("Форма 2.6: Контроль загрязнения подземных вод РВ")]
[Table (name: "form_26")]
public class Form26 : Form2
{
    #region Constructor
    
    public Form26()
    {
        FormNum.Value = "2.6";
        //NumberOfFields.Value = 11;
        Validate_all();
    }

    #endregion

    #region Validation

    private void Validate_all()
    {
        ObservedSourceNumber_Validation(ObservedSourceNumber);
        ControlledAreaName_Validation(ControlledAreaName);
        SupposedWasteSource_Validation(SupposedWasteSource);
        DistanceToWasteSource_Validation(DistanceToWasteSource);
        TestDepth_Validation(TestDepth);
        RadionuclidName_Validation(RadionuclidName);
        AverageYearConcentration_Validation(AverageYearConcentration);
    }

    [FormProperty(true, "Форма")]
    public override bool Object_Validation()
    {
        return !(ObservedSourceNumber.HasErrors ||
                 ControlledAreaName.HasErrors ||
                 SupposedWasteSource.HasErrors ||
                 DistanceToWasteSource.HasErrors ||
                 TestDepth.HasErrors ||
                 RadionuclidName.HasErrors ||
                 AverageYearConcentration.HasErrors);
    }

    #endregion

    #region Properties

    #region ObservedSourceNumber (2)

    public string ObservedSourceNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-1", "Номер наблюдательной скважины", "2")]
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
            rm.PropertyChanged += ObservedSourceNumber_ValueChanged;
            Dictionary.Add(nameof(ObservedSourceNumber), rm);
            return (RamAccess<string>)Dictionary[nameof(ObservedSourceNumber)];
        }
        set
        {
            ObservedSourceNumber_DB = value.Value;
            OnPropertyChanged();
        }
    }
    //If change this change validation

    private void ObservedSourceNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        ObservedSourceNumber_DB = ((RamAccess<string>)value).Value;
    }

    private bool ObservedSourceNumber_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors(); return true;
    }

    #endregion

    #region ControlledAreaName (3)

    public string ControlledAreaName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-1", "Наименование зоны контроля", "3")]
    public RamAccess<string> ControlledAreaName
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ControlledAreaName), out var value))
            {
                ((RamAccess<string>)value).Value = ControlledAreaName_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(ControlledAreaName_Validation, ControlledAreaName_DB);
            rm.PropertyChanged += ControlledAreaName_ValueChanged;
            Dictionary.Add(nameof(ControlledAreaName), rm);
            return (RamAccess<string>)Dictionary[nameof(ControlledAreaName)];
        }
        set
        {
            ControlledAreaName_DB = value.Value;
            OnPropertyChanged();
        }
    }
    //If change this change validation

    private void ControlledAreaName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        ControlledAreaName_DB = ((RamAccess<string>)value).Value;
    }

    private bool ControlledAreaName_Validation(RamAccess<string> value)//Ready
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
        if (!ControlledAreaNameArray.Contains(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region SupposedWasteSource (4)

    public string SupposedWasteSource_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-1", "Предполагаемый источник поступления радиоактивных веществ", "4")]
    public RamAccess<string> SupposedWasteSource
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(SupposedWasteSource), out var value))
            {
                ((RamAccess<string>)value).Value = SupposedWasteSource_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(SupposedWasteSource_Validation, SupposedWasteSource_DB);
            rm.PropertyChanged += SupposedWasteSource_ValueChanged;
            Dictionary.Add(nameof(SupposedWasteSource), rm);
            return (RamAccess<string>)Dictionary[nameof(SupposedWasteSource)];
        }
        set
        {
            SupposedWasteSource_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void SupposedWasteSource_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            SupposedWasteSource_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool SupposedWasteSource_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();//done
        return true;
    }

    #endregion

    #region DistanceToWasteSource (5)

    public string DistanceToWasteSource_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-1", "Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м", "5")]
    public RamAccess<string> DistanceToWasteSource
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(DistanceToWasteSource), out var value))
            {
                ((RamAccess<string>)value).Value = DistanceToWasteSource_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(DistanceToWasteSource_Validation, DistanceToWasteSource_DB);
            rm.PropertyChanged += DistanceToWasteSource_ValueChanged;
            Dictionary.Add(nameof(DistanceToWasteSource), rm);
            return (RamAccess<string>)Dictionary[nameof(DistanceToWasteSource)];
        }
        set
        {
            DistanceToWasteSource_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void DistanceToWasteSource_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        DistanceToWasteSource_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool DistanceToWasteSource_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region TestDepth (6)

    public string TestDepth_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-1", "Глубина отбора проб, м", "6")]
    public RamAccess<string> TestDepth
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(TestDepth), out var value))
            {
                ((RamAccess<string>)value).Value = TestDepth_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(TestDepth_Validation, TestDepth_DB);
            rm.PropertyChanged += TestDepth_ValueChanged;
            Dictionary.Add(nameof(TestDepth), rm);
            return (RamAccess<string>)Dictionary[nameof(TestDepth)];
        }
        set
        {
            TestDepth_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void TestDepth_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        TestDepth_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool TestDepth_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region RadionuclidName (7)

    public string RadionuclidName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-1", "Наименование радионуклида", "7")]
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
            rm.PropertyChanged += RadionuclidName_ValueChanged;
            Dictionary.Add(nameof(RadionuclidName), rm);
            return (RamAccess<string>)Dictionary[nameof(RadionuclidName)];
        }
        set
        {
            RadionuclidName_DB = value.Value;
            OnPropertyChanged();
        }
    }
    //If change this change validation

    private void RadionuclidName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        RadionuclidName_DB = ((RamAccess<string>)value).Value;
    }

    private bool RadionuclidName_Validation(RamAccess<string> value) => NuclidString_Validation(value);

    #endregion

    #region AverageYearConcentration (8)

    public string AverageYearConcentration_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "null-1", "Среднегодовое содержание радионуклида, Бк/кг", "8")]
    public RamAccess<string> AverageYearConcentration
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(AverageYearConcentration), out var value))
            {
                ((RamAccess<string>)value).Value = AverageYearConcentration_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(AverageYearConcentration_Validation, AverageYearConcentration_DB);
            rm.PropertyChanged += AverageYearConcentration_ValueChanged;
            Dictionary.Add(nameof(AverageYearConcentration), rm);
            return (RamAccess<string>)Dictionary[nameof(AverageYearConcentration)];
        }
        set
        {
            AverageYearConcentration_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void AverageYearConcentration_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        AverageYearConcentration_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool AverageYearConcentration_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #endregion

    #region IExcel

    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        base.ExcelGetRow(worksheet, row);
        ObservedSourceNumber_DB = Convert.ToString(worksheet.Cells[row, 2].Value);
        ControlledAreaName_DB = Convert.ToString(worksheet.Cells[row, 3].Value);
        SupposedWasteSource_DB = Convert.ToString(worksheet.Cells[row, 4].Value);
        DistanceToWasteSource_DB = ConvertFromExcelDouble(worksheet.Cells[row, 5].Value);
        TestDepth_DB = ConvertFromExcelDouble(worksheet.Cells[row, 6].Value);
        RadionuclidName_DB = Convert.ToString(worksheet.Cells[row, 7].Value);
        AverageYearConcentration_DB = ConvertFromExcelDouble(worksheet.Cells[row, 8].Value);

    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ConvertToExcelString(ObservedSourceNumber_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(ControlledAreaName_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelString(SupposedWasteSource_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelDouble(DistanceToWasteSource_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelDouble(TestDepth_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelString(RadionuclidName_DB);
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDouble(AverageYearConcentration_DB);
        
        return 7;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form2.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form2.Form26,Models")?.GetProperty(nameof(ObservedSourceNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form2.Form26,Models")?.GetProperty(nameof(ControlledAreaName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form2.Form26,Models")?.GetProperty(nameof(SupposedWasteSource))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form2.Form26,Models")?.GetProperty(nameof(DistanceToWasteSource))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form2.Form26,Models")?.GetProperty(nameof(TestDepth))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form2.Form26,Models")?.GetProperty(nameof(RadionuclidName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute)Type.GetType("Models.Forms.Form2.Form26,Models")?.GetProperty(nameof(AverageYearConcentration))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        
        return 7;
    }

    #endregion

    #region IDataGridColumn

    private static DataGridColumns _DataGridColumns { get; set; }

    private static readonly string[] ControlledAreaNameArray = ["ПП", "СЗЗ", "ЗН", "прим."];

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

        var observedSourceNumberR = ((FormPropertyAttribute)typeof(Form26)
                .GetProperty(nameof(ObservedSourceNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (observedSourceNumberR != null)
        {
            observedSourceNumberR.SetSizeColToAllLevels(164);
            observedSourceNumberR.Binding = nameof(ObservedSourceNumber);
            numberInOrderR += observedSourceNumberR;
        }

        #endregion

        #region ControlledAreaName (3)

        var controlledAreaNameR = ((FormPropertyAttribute)typeof(Form26)
                .GetProperty(nameof(ControlledAreaName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (controlledAreaNameR != null)
        {
            controlledAreaNameR.SetSizeColToAllLevels(166);
            controlledAreaNameR.Binding = nameof(ControlledAreaName);
            numberInOrderR += controlledAreaNameR;
        }

        #endregion

        #region SupposedWasteSource (4)

        var supposedWasteSourceR = ((FormPropertyAttribute)typeof(Form26)
                .GetProperty(nameof(SupposedWasteSource))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (supposedWasteSourceR != null)
        {
            supposedWasteSourceR.SetSizeColToAllLevels(238);
            supposedWasteSourceR.Binding = nameof(SupposedWasteSource);
            numberInOrderR += supposedWasteSourceR;
        }

        #endregion

        #region DistanceToWasteSource (5)

        var distanceToWasteSourceR = ((FormPropertyAttribute)typeof(Form26)
                .GetProperty(nameof(DistanceToWasteSource))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (distanceToWasteSourceR != null)
        {
            distanceToWasteSourceR.SetSizeColToAllLevels(337);
            distanceToWasteSourceR.Binding = nameof(DistanceToWasteSource);
            numberInOrderR += distanceToWasteSourceR;
        }

        #endregion

        #region TestDepth (6)

        var testDepthR = ((FormPropertyAttribute)typeof(Form26)
                .GetProperty(nameof(TestDepth))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (testDepthR != null)
        {
            testDepthR.SetSizeColToAllLevels(180);
            testDepthR.Binding = nameof(TestDepth);
            numberInOrderR += testDepthR;
        }

        #endregion

        #region RadionuclidName (7)

        var radionuclidNameR = ((FormPropertyAttribute)typeof(Form26)
                .GetProperty(nameof(RadionuclidName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (radionuclidNameR != null)
        {
            radionuclidNameR.SetSizeColToAllLevels(230);
            radionuclidNameR.Binding = nameof(RadionuclidName);
            numberInOrderR += radionuclidNameR;
        }

        #endregion

        #region AverageYearConcentration (8)

        var averageYearConcentrationR = ((FormPropertyAttribute)typeof(Form26)
                .GetProperty(nameof(AverageYearConcentration))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        averageYearConcentrationR.SetSizeColToAllLevels(200);
        averageYearConcentrationR.Binding = nameof(AverageYearConcentration);
        numberInOrderR += averageYearConcentrationR;
        
        #endregion

        _DataGridColumns = numberInOrderR;

        return _DataGridColumns;
    }

    #endregion
}