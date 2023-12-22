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

[Form_Class("Форма 1.9: Сведения о результатах инвентаризации РВ не в составе ЗРИ")]
public class Form19 : Form1
{
    #region Constructor

    public Form19()
    {
        FormNum.Value = "1.9";
        OperationCode.Value = "10";
        Validate_all();
    }

    #endregion

    #region Validation

    private void Validate_all()
    {
        //Quantity_Validation(Quantity);
        CodeTypeAccObject_Validation(CodeTypeAccObject);
        Activity_Validation(Activity);
        Radionuclids_Validation(Radionuclids);
    }

    public override bool Object_Validation()
    {
        return !(CodeTypeAccObject.HasErrors ||
                 Activity.HasErrors ||
                 Radionuclids.HasErrors);
    }

    protected override bool OperationCode_Validation(RamAccess<string> value)//OK
    {
        value.ClearErrors();
        switch (value.Value)
        {
            case null:
                value.AddError("Поле не заполнено");
                return false;
            case "10":
                return true;
            default:
                value.AddError("Недопустимое значение");
                return false;
        }
    }

    protected override bool OperationDate_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp) || !DateTimeOffset.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    protected override bool DocumentDate_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp)  || !DateTimeOffset.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    protected override bool DocumentNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (!string.IsNullOrEmpty(value.Value))
        {
            return true; //ok
        }
        value.AddError("Поле не заполнено");
        return false;
        //if (value.Value.Equals("прим."))
        //{
        //    return true;
        //}
    }

    #endregion

    #region Properties

    #region CodeTypeAccObject (7)

    public short? CodeTypeAccObject_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "null-7", "Код типа объектов учета", "7")]
    public RamAccess<short?> CodeTypeAccObject
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(CodeTypeAccObject)))
            {
                ((RamAccess<short?>)Dictionary[nameof(CodeTypeAccObject)]).Value = CodeTypeAccObject_DB;
                return (RamAccess<short?>)Dictionary[nameof(CodeTypeAccObject)];
            }
            var rm = new RamAccess<short?>(CodeTypeAccObject_Validation, CodeTypeAccObject_DB);
            rm.PropertyChanged += CodeTypeAccObjectValueChanged;
            Dictionary.Add(nameof(CodeTypeAccObject), rm);
            return (RamAccess<short?>)Dictionary[nameof(CodeTypeAccObject)];
        }
        set
        {
            CodeTypeAccObject_DB = value.Value;
            OnPropertyChanged(nameof(CodeTypeAccObject));
        }
    }

    private void CodeTypeAccObjectValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            CodeTypeAccObject_DB = ((RamAccess<short?>)value).Value;
        }
    }

    private static bool CodeTypeAccObject_Validation(RamAccess<short?> value)//TODO
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (!Spravochniks.SprCodeTypesAccObjects.Contains((short)value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region Radionuclids (8)

    public string Radionuclids_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения о радиоактивных веществах", "радионуклиды", "8")]
    public RamAccess<string> Radionuclids
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Radionuclids)))
            {
                ((RamAccess<string>)Dictionary[nameof(Radionuclids)]).Value = Radionuclids_DB;
                return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
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
    }//If change this change validation

    private void RadionuclidsValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Radionuclids_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool Radionuclids_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var nuclids = value.Value.Split("; ");
        var flag = true;
        foreach (var nuc in nuclids)
        {
            if (!Spravochniks.SprRadionuclids
                    .Where(item => nuc == item.Item1)
                    .Select(item => item.Item1)
                    .Any())
                flag = false;
        }
        if (flag)
        {
            return true;
        }
        value.AddError("Недопустимое значение");
        return false;
    }

    #endregion

    #region Activity (9)

    public string Activity_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения о радиоактивных веществах", "активность, Бк", "9")]
    public RamAccess<string> Activity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Activity)))
            {
                ((RamAccess<string>)Dictionary[nameof(Activity)]).Value = Activity_DB;
                return (RamAccess<string>)Dictionary[nameof(Activity)];
            }
            var rm = new RamAccess<string>(Activity_Validation, Activity_DB);
            rm.PropertyChanged += ActivityValueChanged;
            Dictionary.Add(nameof(Activity), rm);
            return (RamAccess<string>)Dictionary[nameof(Activity)];
        }
        set
        {
            Activity_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void ActivityValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                Activity_DB = value1;
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
        Activity_DB = value1;
    }

    private static bool Activity_Validation(RamAccess<string> value)//Ready
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
        DocumentVid_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 4].Value), out var byteValue) ? byteValue : null;
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        DocumentDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 6].Text);
        CodeTypeAccObject_DB = short.TryParse(Convert.ToString(worksheet.Cells[row, 7].Value), out var shortValue) ? shortValue : null;
        Radionuclids_DB = Convert.ToString(worksheet.Cells[row, 8].Value);
        Activity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 9].Value);
    }

    public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = DocumentVid_DB is null ? "-" : DocumentVid_DB;
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(DocumentNumber_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelDate(DocumentDate_DB, worksheet, row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0));
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = CodeTypeAccObject_DB is null ? "-" : CodeTypeAccObject_DB;
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelString(Radionuclids_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelDouble(Activity_DB);

        return 6;
    }

    public static void ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form1.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form19,Models")?.GetProperty(nameof(DocumentVid))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form19,Models")?.GetProperty(nameof(DocumentNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form19,Models")?.GetProperty(nameof(DocumentDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form19,Models")?.GetProperty(nameof(CodeTypeAccObject))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form19,Models")?.GetProperty(nameof(Radionuclids))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form1.Form19,Models")?.GetProperty(nameof(Activity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
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

        #region DocumentVid (4)

        var documentVidR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentVid))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (documentVidR != null)
        {
            documentVidR.SetSizeColToAllLevels(88);
            documentVidR.Binding = nameof(DocumentVid);
            numberInOrderR += documentVidR;
        }

        #endregion

        #region DocumentNumber (5)

        var documentNumberR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (documentNumberR != null)
        {
            documentNumberR.SetSizeColToAllLevels(103);
            documentNumberR.Binding = nameof(DocumentNumber);
            numberInOrderR += documentNumberR;
        }

        #endregion

        #region DocumentDate (6)

        var documentDateR = ((FormPropertyAttribute)typeof(Form1)
                .GetProperty(nameof(DocumentDate))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (documentDateR != null)
        {
            documentDateR.SetSizeColToAllLevels(88);
            documentDateR.Binding = nameof(DocumentDate);
            numberInOrderR += documentDateR;
        }

        #endregion

        #region CodeTypeAccObject (7)

        var codeTypeAccObjectR = ((FormPropertyAttribute)typeof(Form19)
                .GetProperty(nameof(CodeTypeAccObject))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (codeTypeAccObjectR != null)
        {
            codeTypeAccObjectR.SetSizeColToAllLevels(163);
            codeTypeAccObjectR.Binding = nameof(CodeTypeAccObject);
            numberInOrderR += codeTypeAccObjectR;
        }

        #endregion

        #region Radionuclids (8)

        var radionuclidsR = ((FormPropertyAttribute)typeof(Form19)
                .GetProperty(nameof(Radionuclids))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        radionuclidsR.SetSizeColToAllLevels(125);
        radionuclidsR.Binding = nameof(Radionuclids);
        numberInOrderR += radionuclidsR;

        #endregion

        #region Activity (9)

        var activityR = ((FormPropertyAttribute)typeof(Form19)
                .GetProperty(nameof(Activity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        activityR.SetSizeColToAllLevels(125);
        activityR.Binding = nameof(Activity);
        numberInOrderR += activityR;

        #endregion

        _DataGridColumns = numberInOrderR;
        return _DataGridColumns;
    }

    #endregion
}