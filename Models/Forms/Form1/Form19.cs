using Models.DataAccess;
using System;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Spravochniki;
using System.Linq;
using System.ComponentModel;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using Models.Collections;

namespace Models;

[Form_Class("Форма 1.9: Сведения о результатах инвентаризации РВ не в составе ЗРИ")]
public class Form19 : Form1
{
    public Form19() : base()
    {
        FormNum.Value = "1.9";
        OperationCode.Value = "10";
        Validate_all();
    }
    private void Validate_all()
    {
        //Quantity_Validation(Quantity);
        CodeTypeAccObject_Validation(CodeTypeAccObject);
        Activity_Validation(Activity);
        Radionuclids_Validation(Radionuclids);
    }
    public override bool Object_Validation()
    {
        return !(CodeTypeAccObject.HasErrors||
                 Activity.HasErrors||
                 Radionuclids.HasErrors);
    }

    #region CodeTypeAccObject
    public short? CodeTypeAccObject_DB { get; set; }
    [NotMapped]
    [FormProperty(true, "null-7","Код типа объектов учета","7")]
    public RamAccess<short?> CodeTypeAccObject
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(CodeTypeAccObject)))
            {
                ((RamAccess<short?>)Dictionary[nameof(CodeTypeAccObject)]).Value = CodeTypeAccObject_DB;
                return (RamAccess<short?>)Dictionary[nameof(CodeTypeAccObject)];
            }
            else
            {
                var rm = new RamAccess<short?>(CodeTypeAccObject_Validation, CodeTypeAccObject_DB);
                rm.PropertyChanged += CodeTypeAccObjectValueChanged;
                Dictionary.Add(nameof(CodeTypeAccObject), rm);
                return (RamAccess<short?>)Dictionary[nameof(CodeTypeAccObject)];
            }
        }
        set
        {
            CodeTypeAccObject_DB = value.Value;
            OnPropertyChanged(nameof(CodeTypeAccObject));
        }
    }
    private void CodeTypeAccObjectValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            CodeTypeAccObject_DB = ((RamAccess<short?>)Value).Value;
        }
    }
    private bool CodeTypeAccObject_Validation(RamAccess<short?> value)//TODO
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

    #region Radionuclids
    public string Radionuclids_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения о радиоактивных веществах","радионуклиды","8")]
    public RamAccess<string> Radionuclids
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Radionuclids)))
            {
                ((RamAccess<string>)Dictionary[nameof(Radionuclids)]).Value = Radionuclids_DB;
                return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
            }
            else
            {
                var rm = new RamAccess<string>(Radionuclids_Validation, Radionuclids_DB);
                rm.PropertyChanged += RadionuclidsValueChanged;
                Dictionary.Add(nameof(Radionuclids), rm);
                return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
            }
        }
        set
        {
            Radionuclids_DB = value.Value;
            OnPropertyChanged(nameof(Radionuclids));
        }
    }//If change this change validation

    private void RadionuclidsValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Radionuclids_DB = ((RamAccess<string>)Value).Value;
        }
    }
    private bool Radionuclids_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var nuclids = value.Value.Split("; ");
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

    #region Activity
    public string Activity_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Сведения о радиоактивных веществах","активность, Бк","9")]
    public RamAccess<string> Activity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Activity)))
            {
                ((RamAccess<string>)Dictionary[nameof(Activity)]).Value = Activity_DB;
                return (RamAccess<string>)Dictionary[nameof(Activity)];
            }
            else
            {
                var rm = new RamAccess<string>(Activity_Validation, Activity_DB);
                rm.PropertyChanged += ActivityValueChanged;
                Dictionary.Add(nameof(Activity), rm);
                return (RamAccess<string>)Dictionary[nameof(Activity)];
            }
        }
        set
        {
            Activity_DB = value.Value;
            OnPropertyChanged(nameof(Activity));
        }
    }
    private void ActivityValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
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
                try
                {
                    var value2 = Convert.ToDouble(value1);
                    value1 = $"{value2:0.######################################################e+00}";
                }
                catch (Exception ex)
                { }
            }
            Activity_DB = value1;
        }
    }
    private bool Activity_Validation(RamAccess<string> value)//Ready
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
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    #endregion

    protected override bool OperationCode_Validation(RamAccess<string> value)//OK
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value != "10")
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
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
        //DateTimeOffset date = DateTimeOffset.Parse(tmp);
        //if (date.Date > DateTimeOffset.Now.Date)
        //{
        //    value.AddError("Недопустимое значение");
        //    return false;
        //}
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
        //DateTimeOffset date = DateTimeOffset.Parse(tmp);
        //if (date.Date > DateTimeOffset.Now.Date)
        //{
        //    value.AddError("Недопустимое значение");
        //    return false;
        //}
        return true;
    }
    protected override bool DocumentNumber_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))//ok
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            return true;
        }
        return true;
    }
    #region IExcel
    public void ExcelGetRow(ExcelWorksheet worksheet, int Row)
    {
        base.ExcelGetRow(worksheet, Row);
        DocumentVid_DB = Convert.ToByte(worksheet.Cells[Row, 4].Value);
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[Row, 5].Value);
        DocumentDate_DB = Convert.ToString(worksheet.Cells[Row, 6].Value);
        CodeTypeAccObject_DB = Convert.ToInt16(worksheet.Cells[Row, 7].Value);
        Radionuclids_DB = Convert.ToString(worksheet.Cells[Row, 8].Value);
        Activity_DB = Convert.ToString(worksheet.Cells[Row, 9].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 9].Value), out var val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 9].Value);
    }
    public int ExcelRow(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true, string SumNumber = "")
    {
        var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
        Column += Transpon == true ? cnt : 0;
        Row += Transpon == false ? cnt : 0;

        worksheet.Cells[Row + (!Transpon ? 0 : 0), Column + (Transpon ? 0 : 0)].Value = DocumentVid_DB;
        worksheet.Cells[Row + (!Transpon ? 1 : 0), Column + (Transpon ? 1 : 0)].Value = DocumentNumber_DB;
        worksheet.Cells[Row + (!Transpon ? 2 : 0), Column + (Transpon ? 2 : 0)].Value = DocumentDate_DB;
        worksheet.Cells[Row + (!Transpon ? 3 : 0), Column + (Transpon ? 3 : 0)].Value = CodeTypeAccObject_DB;
        worksheet.Cells[Row + (!Transpon ? 4 : 0), Column + (Transpon ? 4 : 0)].Value = Radionuclids_DB;
        worksheet.Cells[Row + (!Transpon ? 5 : 0), Column + (Transpon ? 5 : 0)].Value = string.IsNullOrEmpty(Activity_DB) || Activity_DB == "-" ? 0  : double.TryParse(Activity_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out var val) ? val : Activity_DB;
        return 6;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
    {
        var cnt = Form1.ExcelHeader(worksheet, Row, Column, Transpon);
        Column += +(Transpon == true ? cnt : 0);
        Row += Transpon == false ? cnt : 0;

        worksheet.Cells[Row + (!Transpon? 0 : 0), Column + (Transpon ? 0 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty(nameof(DocumentVid)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon? 1 : 0), Column + (Transpon ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty(nameof(DocumentNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon? 2 : 0), Column + (Transpon ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty(nameof(DocumentDate)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon? 3 : 0), Column + (Transpon ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty(nameof(CodeTypeAccObject)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon? 4 : 0), Column + (Transpon ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty(nameof(Radionuclids)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon? 5 : 0), Column + (Transpon ? 5 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty(nameof(Activity)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];

        return 6;
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
            #region DocumentVid (4)
            var DocumentVidR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentVid)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentVidR.SetSizeColToAllLevels(88);
            DocumentVidR.Binding = nameof(DocumentVid);
            NumberInOrderR += DocumentVidR;
            #endregion
            #region DocumentNumber (5)
            var DocumentNumberR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentNumber)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentNumberR.SetSizeColToAllLevels(103);
            DocumentNumberR.Binding = nameof(DocumentNumber);
            NumberInOrderR += DocumentNumberR;
            #endregion
            #region DocumentDate (6)
            var DocumentDateR = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(DocumentDate)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            DocumentDateR.SetSizeColToAllLevels(88);
            DocumentDateR.Binding = nameof(DocumentDate);
            NumberInOrderR += DocumentDateR;
            #endregion
            #region CodeTypeAccObject (7)
            var CodeTypeAccObjectR = ((FormPropertyAttribute)typeof(Form19).GetProperty(nameof(CodeTypeAccObject)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            CodeTypeAccObjectR.SetSizeColToAllLevels(163);
            CodeTypeAccObjectR.Binding = nameof(CodeTypeAccObject);
            NumberInOrderR += CodeTypeAccObjectR;
            #endregion
            #region Radionuclids (8)
            var RadionuclidsR = ((FormPropertyAttribute)typeof(Form19).GetProperty(nameof(Radionuclids)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            RadionuclidsR.SetSizeColToAllLevels(125);
            RadionuclidsR.Binding = nameof(Radionuclids);
            NumberInOrderR += RadionuclidsR;
            #endregion
            #region Activity (9)
            var ActivityR = ((FormPropertyAttribute)typeof(Form19).GetProperty(nameof(Activity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            ActivityR.SetSizeColToAllLevels(125);
            ActivityR.Binding = nameof(Activity);
            NumberInOrderR += ActivityR;
            #endregion
            _DataGridColumns = NumberInOrderR;
        }
        return _DataGridColumns;
    }
    #endregion
}