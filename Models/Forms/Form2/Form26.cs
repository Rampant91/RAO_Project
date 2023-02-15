using System;
using System.Collections.Generic;
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
[Form_Class("Форма 2.6: Контроль загрязнения подземных вод РВ")]
public class Form26 : Form2
{
    public Form26() : base()
    {
        FormNum.Value = "2.6";
        //NumberOfFields.Value = 11;
        Validate_all();
    }
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

    [FormProperty(true,"Форма")]
    public override bool Object_Validation()
    {
        return !(ObservedSourceNumber.HasErrors||
                 ControlledAreaName.HasErrors||
                 SupposedWasteSource.HasErrors||
                 DistanceToWasteSource.HasErrors||
                 TestDepth.HasErrors||
                 RadionuclidName.HasErrors||
                 AverageYearConcentration.HasErrors);
    }

    #region ObservedSourceNumber
    public string ObservedSourceNumber_DB { get; set; } = ""; [NotMapped]
    [FormProperty(true,"null-1","Номер наблюдательной скважины","2")]
    public RamAccess<string> ObservedSourceNumber
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(ObservedSourceNumber)))
            {
                ((RamAccess<string>)Dictionary[nameof(ObservedSourceNumber)]).Value = ObservedSourceNumber_DB;
                return (RamAccess<string>)Dictionary[nameof(ObservedSourceNumber)];
            }
            else
            {
                var rm = new RamAccess<string>(ObservedSourceNumber_Validation, ObservedSourceNumber_DB);
                rm.PropertyChanged += ObservedSourceNumberValueChanged;
                Dictionary.Add(nameof(ObservedSourceNumber), rm);
                return (RamAccess<string>)Dictionary[nameof(ObservedSourceNumber)];
            }
        }
        set
        {
            ObservedSourceNumber_DB = value.Value;
            OnPropertyChanged(nameof(ObservedSourceNumber));
        }
    }
    //If change this change validation
    private void ObservedSourceNumberValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            ObservedSourceNumber_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool ObservedSourceNumber_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors(); return true;
    }
    //ObservedSourceNumber property
    #endregion

    #region ControlledAreaName
    public string ControlledAreaName_DB { get; set; } = ""; [NotMapped]
    [FormProperty(true,"null-1", "Наименование зоны контроля","3")]
    public RamAccess<string> ControlledAreaName
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(ControlledAreaName)))
            {
                ((RamAccess<string>)Dictionary[nameof(ControlledAreaName)]).Value = ControlledAreaName_DB;
                return (RamAccess<string>)Dictionary[nameof(ControlledAreaName)];
            }
            else
            {
                var rm = new RamAccess<string>(ControlledAreaName_Validation, ControlledAreaName_DB);
                rm.PropertyChanged += ControlledAreaNameValueChanged;
                Dictionary.Add(nameof(ControlledAreaName), rm);
                return (RamAccess<string>)Dictionary[nameof(ControlledAreaName)];
            }
        }
        set
        {
            ControlledAreaName_DB = value.Value;
            OnPropertyChanged(nameof(ControlledAreaName));
        }
    }
    //If change this change validation
    private void ControlledAreaNameValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            ControlledAreaName_DB = ((RamAccess<string>)Value).Value;
        }
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
        List<string> spr = new()
        {
            "ПП",
            "СЗЗ",
            "ЗН",
            "прим."
        };
        if (!spr.Contains(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    //ControlledAreaName property
    #endregion

    #region SupposedWasteSource
    public string SupposedWasteSource_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"null-1", "Предполагаемый источник поступления радиоактивных веществ","4")]
    public RamAccess<string> SupposedWasteSource
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(SupposedWasteSource)))
            {
                ((RamAccess<string>)Dictionary[nameof(SupposedWasteSource)]).Value = SupposedWasteSource_DB;
                return (RamAccess<string>)Dictionary[nameof(SupposedWasteSource)];
            }
            else
            {
                var rm = new RamAccess<string>(SupposedWasteSource_Validation, SupposedWasteSource_DB);
                rm.PropertyChanged += SupposedWasteSourceValueChanged;
                Dictionary.Add(nameof(SupposedWasteSource), rm);
                return (RamAccess<string>)Dictionary[nameof(SupposedWasteSource)];
            }
        }
        set
        {
            SupposedWasteSource_DB = value.Value;
            OnPropertyChanged(nameof(SupposedWasteSource));
        }
    }

    private void SupposedWasteSourceValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            SupposedWasteSource_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool SupposedWasteSource_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();//done
        return true;
    }
    //SupposedWasteSource property
    #endregion

    #region DistanceToWasteSource
    public string DistanceToWasteSource_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"null-1", "Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м","5")]
    public RamAccess<string> DistanceToWasteSource
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(DistanceToWasteSource)))
            {
                ((RamAccess<string>)Dictionary[nameof(DistanceToWasteSource)]).Value = DistanceToWasteSource_DB;
                return (RamAccess<string>)Dictionary[nameof(DistanceToWasteSource)];
            }
            else
            {
                var rm = new RamAccess<string>(DistanceToWasteSource_Validation, DistanceToWasteSource_DB);
                rm.PropertyChanged += DistanceToWasteSourceValueChanged;
                Dictionary.Add(nameof(DistanceToWasteSource), rm);
                return (RamAccess<string>)Dictionary[nameof(DistanceToWasteSource)];
            }
        }
        set
        {
            DistanceToWasteSource_DB = value.Value;
            OnPropertyChanged(nameof(DistanceToWasteSource));
        }
    }

    private void DistanceToWasteSourceValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
            if (value1 != null)
            {
                value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    DistanceToWasteSource_DB = value1;
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
            DistanceToWasteSource_DB = value1;
        }
    }
    private bool DistanceToWasteSource_Validation(RamAccess<string> value)//Ready
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
        if (value.Value.Equals("прим."))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    //DistanceToWasteSource property
    #endregion

    #region TestDepth
    public string TestDepth_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"null-1", "Глубина отбора проб, м","6")]
    public RamAccess<string> TestDepth
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(TestDepth)))
            {
                ((RamAccess<string>)Dictionary[nameof(TestDepth)]).Value = TestDepth_DB;
                return (RamAccess<string>)Dictionary[nameof(TestDepth)];
            }
            else
            {
                var rm = new RamAccess<string>(TestDepth_Validation, TestDepth_DB);
                rm.PropertyChanged += TestDepthValueChanged;
                Dictionary.Add(nameof(TestDepth), rm);
                return (RamAccess<string>)Dictionary[nameof(TestDepth)];
            }
        }
        set
        {
            TestDepth_DB = value.Value;
            OnPropertyChanged(nameof(TestDepth));
        }
    }

    private void TestDepthValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
            if (value1 != null)
            {
                value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    TestDepth_DB = value1;
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
            TestDepth_DB = value1;
        }
    }
    private bool TestDepth_Validation(RamAccess<string> value)//Ready
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
        if (value.Value.Equals("прим."))
        {
            return true;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
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
    //TestDepth property
    #endregion

    #region RadionuclidName
    public string RadionuclidName_DB { get; set; } = ""; [NotMapped]
    [FormProperty(true,"null-1", "Наименование радионуклида","7")]
    public RamAccess<string> RadionuclidName
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(RadionuclidName)))
            {
                ((RamAccess<string>)Dictionary[nameof(RadionuclidName)]).Value = RadionuclidName_DB;
                return (RamAccess<string>)Dictionary[nameof(RadionuclidName)];
            }
            else
            {
                var rm = new RamAccess<string>(RadionuclidName_Validation, RadionuclidName_DB);
                rm.PropertyChanged += RadionuclidNameValueChanged;
                Dictionary.Add(nameof(RadionuclidName), rm);
                return (RamAccess<string>)Dictionary[nameof(RadionuclidName)];
            }
        }
        set
        {
            RadionuclidName_DB = value.Value;
            OnPropertyChanged(nameof(RadionuclidName));
        }
    }
    //If change this change validation
    private void RadionuclidNameValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            RadionuclidName_DB = ((RamAccess<string>)Value).Value;
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
        var query = from item in Spravochniks.SprRadionuclids where item.Item1 == tmpstr select item.Item1;
        if (!query.Any())
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    //RadionuclidName property
    #endregion

    #region AverageYearConcentration 
    public string AverageYearConcentration_DB { get; set; } [NotMapped]
    [FormProperty(true,"null-1", "Среднегодовое содержание радионуклида, Бк/кг","8")]
    public RamAccess<string> AverageYearConcentration
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(AverageYearConcentration)))
            {
                ((RamAccess<string>)Dictionary[nameof(AverageYearConcentration)]).Value = AverageYearConcentration_DB;
                return (RamAccess<string>)Dictionary[nameof(AverageYearConcentration)];
            }
            else
            {
                var rm = new RamAccess<string>(AverageYearConcentration_Validation, AverageYearConcentration_DB);
                rm.PropertyChanged += AverageYearConcentrationValueChanged;
                Dictionary.Add(nameof(AverageYearConcentration), rm);
                return (RamAccess<string>)Dictionary[nameof(AverageYearConcentration)];
            }
        }
        set
        {
            AverageYearConcentration_DB = value.Value;
            OnPropertyChanged(nameof(AverageYearConcentration));
        }
    }

    private void AverageYearConcentrationValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
            if (value1 != null)
            {
                value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    AverageYearConcentration_DB = value1;
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
            AverageYearConcentration_DB = value1;
        }
    }
    private bool AverageYearConcentration_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (value1.Equals("-"))
        {
            return true;
        }
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    //AverageYearConcentration property
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

        #region ObservedSourceNumber (2)
        var ObservedSourceNumberR = ((FormPropertyAttribute)typeof(Form26).GetProperty(nameof(ObservedSourceNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        ObservedSourceNumberR.SetSizeColToAllLevels(164);
        ObservedSourceNumberR.Binding = nameof(ObservedSourceNumber);
        NumberInOrderR += ObservedSourceNumberR;
        #endregion

        #region ControlledAreaName (3)
        var ControlledAreaNameR = ((FormPropertyAttribute)typeof(Form26).GetProperty(nameof(ControlledAreaName)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        ControlledAreaNameR.SetSizeColToAllLevels(166);
        ControlledAreaNameR.Binding = nameof(ControlledAreaName);
        NumberInOrderR += ControlledAreaNameR;
        #endregion

        #region SupposedWasteSource (4)
        var SupposedWasteSourceR = ((FormPropertyAttribute)typeof(Form26).GetProperty(nameof(SupposedWasteSource)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        SupposedWasteSourceR.SetSizeColToAllLevels(238);
        SupposedWasteSourceR.Binding = nameof(SupposedWasteSource);
        NumberInOrderR += SupposedWasteSourceR;
        #endregion

        #region DistanceToWasteSource (5)
        var DistanceToWasteSourceR = ((FormPropertyAttribute)typeof(Form26).GetProperty(nameof(DistanceToWasteSource)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        DistanceToWasteSourceR.SetSizeColToAllLevels(337);
        DistanceToWasteSourceR.Binding = nameof(DistanceToWasteSource);
        NumberInOrderR += DistanceToWasteSourceR;
        #endregion

        #region TestDepth (6)
        var TestDepthR = ((FormPropertyAttribute)typeof(Form26).GetProperty(nameof(TestDepth)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        TestDepthR.SetSizeColToAllLevels(180);
        TestDepthR.Binding = nameof(TestDepth);
        NumberInOrderR += TestDepthR;
        #endregion

        #region RadionuclidName (7)
        var RadionuclidNameR = ((FormPropertyAttribute)typeof(Form26).GetProperty(nameof(RadionuclidName)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        RadionuclidNameR.SetSizeColToAllLevels(230);
        RadionuclidNameR.Binding = nameof(RadionuclidName);
        NumberInOrderR += RadionuclidNameR;
        #endregion

        #region AverageYearConcentration (8)
        var AverageYearConcentrationR = ((FormPropertyAttribute)typeof(Form26).GetProperty(nameof(AverageYearConcentration)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
        AverageYearConcentrationR.SetSizeColToAllLevels(200);
        AverageYearConcentrationR.Binding = nameof(AverageYearConcentration);
        NumberInOrderR += AverageYearConcentrationR;
        #endregion

        _DataGridColumns = NumberInOrderR;

        return _DataGridColumns;
    }

    #endregion
}