using Models.DataAccess; 
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel;
using System.Globalization;
using Spravochniki;
using System.Linq;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using Models.Collections;
using Models.Forms;

namespace Models;

[Serializable]
[Form_Class("Форма 2.9: Активность радионуклидов, отведенных со сточными водами")]
public class Form29 : Form2
{
    public Form29() : base()
    {
        FormNum.Value = "2.9";
        //NumberOfFields.Value = 8;
        Validate_all();
    }
    private void Validate_all()
    {
        WasteSourceName_Validation(WasteSourceName);
        RadionuclidName_Validation(RadionuclidName);
        AllowedActivity_Validation(AllowedActivity);
        FactedActivity_Validation(FactedActivity);
    }

    [FormProperty(true,"Форма")]
    public override bool Object_Validation()
    {
        return !(WasteSourceName.HasErrors||
                 RadionuclidName.HasErrors||
                 AllowedActivity.HasErrors||
                 FactedActivity.HasErrors);
    }

    //WasteSourceName property
    #region WasteSourceName 
    public string WasteSourceName_DB { get; set; } = ""; [NotMapped]
    [FormProperty(true,"null-2","Наименование, номер выпуска сточных вод","2")]
    public RamAccess<string> WasteSourceName
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(WasteSourceName)))
            {
                ((RamAccess<string>)Dictionary[nameof(WasteSourceName)]).Value = WasteSourceName_DB;
                return (RamAccess<string>)Dictionary[nameof(WasteSourceName)];
            }
            else
            {
                var rm = new RamAccess<string>(WasteSourceName_Validation, WasteSourceName_DB);
                rm.PropertyChanged += WasteSourceNameValueChanged;
                Dictionary.Add(nameof(WasteSourceName), rm);
                return (RamAccess<string>)Dictionary[nameof(WasteSourceName)];
            }
        }
        set
        {
            WasteSourceName_DB = value.Value;
            OnPropertyChanged(nameof(WasteSourceName));
        }
    }
    private void WasteSourceNameValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            WasteSourceName_DB = ((RamAccess<string>)Value).Value;
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
    //WasteSourceName property
    #endregion

    //RadionuclidName property
    #region RadionuclidName
    public string RadionuclidName_DB { get; set; } = ""; 
    [NotMapped]        
    [FormProperty(true,"null-3","Наименование радионуклида","3")]
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
        var tmp = from item in Spravochniks.SprRadionuclids where tmpstr == item.Item1 select item.Item1;
        if (!tmp.Any())
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    //RadionuclidName property
    #endregion

    //AllowedActivity property
    #region AllowedActivity
    public string AllowedActivity_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Активность радионуклида, Бк", "допустимая","4")]
    public RamAccess<string> AllowedActivity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(AllowedActivity)))
            {
                ((RamAccess<string>)Dictionary[nameof(AllowedActivity)]).Value = AllowedActivity_DB;
                return (RamAccess<string>)Dictionary[nameof(AllowedActivity)];
            }
            else
            {
                var rm = new RamAccess<string>(AllowedActivity_Validation, AllowedActivity_DB);
                rm.PropertyChanged += AllowedActivityValueChanged;
                Dictionary.Add(nameof(AllowedActivity), rm);
                return (RamAccess<string>)Dictionary[nameof(AllowedActivity)];
            }
        }
        set
        {
            AllowedActivity_DB = value.Value;
            OnPropertyChanged(nameof(AllowedActivity));
        }
    }

    private void AllowedActivityValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
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
                try
                {
                    var value2 = Convert.ToDouble(value1);
                    value1 = $"{value2:0.######################################################e+00}";
                }
                catch (Exception ex)
                { }
            }
            AllowedActivity_DB = value1;
        }
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
        if (!value1.Contains('e')&& value1.Contains('+') ^ value1.Contains('-'))
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
    //AllowedActivity property
    #endregion
    //FactedActivity property
    #region FactedActivity
    public string FactedActivity_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Активность радионуклида, Бк", "фактическая","5")]
    public RamAccess<string> FactedActivity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(FactedActivity)))
            {
                ((RamAccess<string>)Dictionary[nameof(FactedActivity)]).Value = FactedActivity_DB;
                return (RamAccess<string>)Dictionary[nameof(FactedActivity)];
            }
            else
            {
                var rm = new RamAccess<string>(FactedActivity_Validation, FactedActivity_DB);
                rm.PropertyChanged += FactedActivityValueChanged;
                Dictionary.Add(nameof(FactedActivity), rm);
                return (RamAccess<string>)Dictionary[nameof(FactedActivity)];
            }
        }
        set
        {
            FactedActivity_DB = value.Value;
            OnPropertyChanged(nameof(FactedActivity));
        }
    }
    private void FactedActivityValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
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
                try
                {
                    var value2 = Convert.ToDouble(value1);
                    value1 = $"{value2:0.######################################################e+00}";
                }
                catch (Exception ex)
                { }
            }
            FactedActivity_DB = value1;
        }
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
    //FactedActivity property
    #endregion

    #region IExcel
    public void ExcelGetRow(ExcelWorksheet worksheet, int Row)
    {
        double val;
        base.ExcelGetRow(worksheet, Row);
        WasteSourceName_DB = Convert.ToString(worksheet.Cells[Row, 1].Value);
        RadionuclidName_DB = Convert.ToString(worksheet.Cells[Row, 2].Value);
        AllowedActivity_DB = Convert.ToString(worksheet.Cells[Row, 3].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 3].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 3].Value);
        FactedActivity_DB = Convert.ToString(worksheet.Cells[Row, 4].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 4].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 4].Value);

    }
    public int ExcelRow(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true)
    {
        var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
        Column += Transpon ? cnt : 0;
        Row += !Transpon ? cnt : 0;
        double val = 0;
        worksheet.Cells[Row + (!Transpon ? 0 : 0), Column + (Transpon ? 0 : 0)].Value = WasteSourceName_DB;
        worksheet.Cells[Row + (!Transpon ? 1 : 0), Column + (Transpon ? 1 : 0)].Value = RadionuclidName_DB;
        worksheet.Cells[Row + (!Transpon ? 2 : 0), Column + (Transpon ? 2 : 0)].Value = string.IsNullOrEmpty(AllowedActivity_DB) || AllowedActivity_DB == null ? 0 : double.TryParse(AllowedActivity_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : AllowedActivity_DB;
        worksheet.Cells[Row + (!Transpon ? 3 : 0), Column + (Transpon ? 3 : 0)].Value = string.IsNullOrEmpty(FactedActivity_DB) || FactedActivity_DB == null ? 0 : double.TryParse(FactedActivity_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : FactedActivity_DB;

        return 4;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true)
    {
        var cnt = Form2.ExcelHeader(worksheet, Row, Column, Transpon);
        Column += Transpon ? cnt : 0;
        Row += !Transpon ? cnt : 0;

        worksheet.Cells[Row + (!Transpon ? 0 : 0), Column + (Transpon ? 0 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form29,Models").GetProperty(nameof(WasteSourceName)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 1 : 0), Column + (Transpon ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form29,Models").GetProperty(nameof(RadionuclidName)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 2 : 0), Column + (Transpon ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form29,Models").GetProperty(nameof(AllowedActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 3 : 0), Column + (Transpon ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form29,Models").GetProperty(nameof(FactedActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        return 4;
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
            #region WasteSourceName (2)
            var WasteSourceNameR = ((FormPropertyAttribute)typeof(Form29).GetProperty(nameof(WasteSourceName)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            WasteSourceNameR.SetSizeColToAllLevels(268);
            WasteSourceNameR.Binding = nameof(WasteSourceName);
            NumberInOrderR += WasteSourceNameR;
            #endregion
            #region RadionuclidName (3)
            var RadionuclidNameR = ((FormPropertyAttribute)typeof(Form29).GetProperty(nameof(RadionuclidName)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            RadionuclidNameR.SetSizeColToAllLevels(183);
            RadionuclidNameR.Binding = nameof(RadionuclidName);
            NumberInOrderR += RadionuclidNameR;
            #endregion
            #region AllowedActivity (4)
            var AllowedActivityR = ((FormPropertyAttribute)typeof(Form29).GetProperty(nameof(AllowedActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            AllowedActivityR.SetSizeColToAllLevels(94);
            AllowedActivityR.Binding = nameof(AllowedActivity);
            NumberInOrderR += AllowedActivityR;
            #endregion
            #region FactedActivity (5)
            var FactedActivityR = ((FormPropertyAttribute)typeof(Form29).GetProperty(nameof(FactedActivity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            FactedActivityR.SetSizeColToAllLevels(94);
            FactedActivityR.Binding = nameof(FactedActivity);
            NumberInOrderR += FactedActivityR;
            #endregion
            _DataGridColumns = NumberInOrderR;
        }
        return _DataGridColumns;
    }
    #endregion
}