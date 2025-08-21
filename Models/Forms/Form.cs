using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text.RegularExpressions;
using Models.Collections;
using Models.Forms.DataAccess;
using Models.Interfaces;
using OfficeOpenXml;
using Spravochniki;

namespace Models.Forms;

public abstract partial class Form : IKey, IDataGridColumn
{
    #region Properties

    #region Id
    
    [Key]
    public int Id { get; set; }

    #endregion

    #region Report
    
    [ForeignKey(nameof(Report))]
    public int? ReportId { get; set; }

    public virtual Report? Report { get; set; }

    #endregion

    #region Dictionary
    
    [NotMapped]
    protected Dictionary<string, RamAccess> Dictionary { get; set; } = []; 
    
    #endregion

    #region FormNum
    public string FormNum_DB { get; set; } = "";

    [NotMapped]
    [Attributes.FormProperty(true, "Форма")]
    public RamAccess<string> FormNum
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(FormNum), out var value))
            {
                ((RamAccess<string>)value).Value = FormNum_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(FormNum_Validation, FormNum_DB);
            rm.PropertyChanged += FormNum_ValueChanged;
            Dictionary.Add(nameof(FormNum), rm);
            return (RamAccess<string>)Dictionary[nameof(FormNum)];
        }
        set
        {
            FormNum_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void FormNum_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        FormNum_DB = ((RamAccess<string>)value).Value;
    }

    private static bool FormNum_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region Order
    
    [NotMapped]
    public long Order => NumberInOrder_DB;

    public void SetOrder(long index)
    {
        if (NumberInOrder_DB != (int)index)
        {
            NumberInOrder_DB = (int)index;
            OnPropertyChanged(nameof(NumberInOrder_DB));
            OnPropertyChanged(nameof(Order));
        }
    } 

    #endregion

    #region NumberInOrder (1)

    public int NumberInOrder_DB { get; set; }

    [NotMapped]
    [Attributes.FormProperty(true, "null-1-1", "null-1", "№ п/п", "1")]
    public RamAccess<int> NumberInOrder
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(NumberInOrder), out var value))
            {
                ((RamAccess<int>)value).Value = NumberInOrder_DB;
                return (RamAccess<int>)value;
            }
            var rm = new RamAccess<int>(NumberInOrder_Validation, NumberInOrder_DB);
            rm.PropertyChanged += NumberInOrderValueChanged;
            Dictionary.Add(nameof(NumberInOrder), rm);
            return (RamAccess<int>)Dictionary[nameof(NumberInOrder)];
        }
        set
        {
            NumberInOrder_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void NumberInOrderValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        NumberInOrder_DB = ((RamAccess<int>)value).Value;
    }

    private static bool NumberInOrder_Validation(RamAccess<int> value)//Ready
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region NumberOfFields

    public int NumberOfFields_DB { get; set; }

    [NotMapped]
    [Attributes.FormProperty(true, "Число полей")]
    public RamAccess<int> NumberOfFields
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(NumberOfFields), out RamAccess value))
            {
                ((RamAccess<int>)value).Value = NumberOfFields_DB;
                return (RamAccess<int>)value;
            }
            var rm = new RamAccess<int>(NumberOfFields_Validation, NumberOfFields_DB);
            rm.PropertyChanged += NumberOfFieldsValueChanged;
            Dictionary.Add(nameof(NumberOfFields), rm);
            return (RamAccess<int>)Dictionary[nameof(NumberOfFields)];
        }
        set
        {
            NumberOfFields_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void NumberOfFieldsValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            NumberOfFields_DB = ((RamAccess<int>)value).Value;
        }
    }

    private static bool NumberOfFields_Validation(RamAccess<int> value)
    {
        value.ClearErrors();
        return true;

    }

    #endregion 
    
    #endregion

    #region Validation

    public abstract bool Object_Validation();

    private protected const NumberStyles StyleDecimalThousandExp =
        NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent;

    #region ExponentialString
    
    private protected static bool ExponentialString_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        var tmp = value.Value;
        tmp = ReplaceDashes(tmp);
        if (string.IsNullOrWhiteSpace(tmp))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (tmp is "прим." or "-")
        {
            return true;
        }
        tmp = tmp
            .Trim()
            .TrimStart('(')
            .TrimEnd(')')
            .ToLower()
            .Replace('.', ',')
            .Replace('е', 'e');
        if (!double.TryParse(tmp,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var doubleValue)
            || tmp.StartsWith('(') ^ tmp.EndsWith(')'))
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

    #region DateString
    
    private protected static bool DateString_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        var tmp = (value.Value ?? string.Empty).Trim();
        tmp = ReplaceDashes(tmp);
        if (string.IsNullOrWhiteSpace(tmp))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (tmp.Equals("прим.") || tmp.Equals("-"))
        {
            return true;
        }
        if (!DateOnly.TryParse(tmp, CultureInfo.CreateSpecificCulture("ru-RU"), out var date)
            || date.Year < 1945)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region NuclidString

    private protected static bool NuclidString_Validation(RamAccess<string> value)
   {
        value.ClearErrors();
        var tmp = value.Value;
        tmp = ReplaceDashes(tmp);
        if (string.IsNullOrEmpty(tmp))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (tmp.Contains(','))
        {
            value.AddError("При перечислении необходимо использовать \";\"");
            return false;
        }
        if (tmp.Equals("прим.") || tmp.Equals("-"))
        {
            return true;
        }
        var nuclids = tmp
            .ToLower()
            .Split(";")
            .Select(x => x.Trim())
            .ToHashSet();
        var allNuclidsInSpr = nuclids
            .All(nuclid => Spravochniks.SprRadionuclids
                .Any(nameInSpr => nameInSpr.name == nuclid));
        if (!allNuclidsInSpr)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #endregion

    #region ValueChanged

    private protected static string ExponentialString_ValueChanged(string value)
    {
        var tmp = (value ?? string.Empty)
            .Trim()
            .ToLower()
            .Replace('е', 'e');
        tmp = ReplaceDashes(tmp);
        if (tmp != "прим.")
        {
            tmp = tmp.Replace('.', ',');
        }
        if (tmp is "прим." or "-")
        {
            return tmp;
        }
        var doubleStartsWithBrackets = false;
        if (tmp.StartsWith('(') && tmp.EndsWith(')'))
        {
            doubleStartsWithBrackets = true;
            tmp = tmp
                .TrimStart('(')
                .TrimEnd(')');
        }
        var tmpNumWithoutSign = tmp.StartsWith('+') || tmp.StartsWith('-')
            ? tmp[1..]
            : tmp;
        var sign = tmp.StartsWith('-')
            ? "-"
            : string.Empty;
        if (!tmp.Contains('e')
            && tmpNumWithoutSign.Count(x => x is '+' or '-') == 1)
        {
            tmp = sign + tmpNumWithoutSign.Replace("+", "e+").Replace("-", "e-");
        }
        if (double.TryParse(tmp,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var doubleValue))
        {
            tmp = $"{doubleValue:0.######################################################e+00}";
        }
        return doubleStartsWithBrackets 
            ? $"({tmp})" 
            : tmp;
    }

    private protected static string DateString_ValueChanged(string value)
    {
        var tmp = (value ?? string.Empty).Trim();

        if (DateTime.TryParse(tmp, CultureInfo.CreateSpecificCulture("ru-RU"), out var dateTime))
        {
            return dateTime.ToShortDateString();
        }
        return DateOnly.TryParse(tmp, CultureInfo.CreateSpecificCulture("ru-RU"), out var date)
            ? date.ToShortDateString()
            : tmp is not ("" or "-" or "прим.")
                ? ""
                : tmp;
    }

    private protected static string ReplaceDashes(string value) =>
        value switch
        {
            null => string.Empty,
            _ => DashesRegex().Replace(value, "-")
        };

    #endregion

    #region INotifyPropertyChanged

    protected void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region IExcel
    public abstract void ExcelGetRow(ExcelWorksheet worksheet, int row);

    public abstract int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "");

    protected static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        return 0;
    }

    #region Convert

    #region ConvertFromExcel

    private protected static string ConvertFromExcelDate(object value)
    {
        var strValue = Convert.ToString(value);
        return strValue is null or "" or "-"
            ? "-"
            : DateTime.TryParse(strValue, out var dateTime)
                ? dateTime.ToShortDateString()
                : strValue;
    }

    private protected static string ConvertFromExcelDouble(object value)
    {
        var strValue = Convert.ToString(value);
        return double.TryParse(strValue, out var doubleValue)
            ? doubleValue.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture)
            : strValue;
    }

    private protected static string ConvertFromExcelInt(object value)
    {
        var strValue = Convert.ToString(value);
        return int.TryParse(strValue, out var intValue)
            ? intValue.ToString()
            : strValue;
    }

    #endregion

    #region ConvertToExcel
    
    private protected static object ConvertToExcelDate(string value, ExcelWorksheet worksheet, int row, int column)
    {
        if (DateTime.TryParse(value, out var dateTime))
        {
            worksheet.Cells[row, column].Style.Numberformat.Format = "dd.mm.yyyy";
        }
        return value is null or "" or "-"
            ? "-"
            : DateTime.TryParse(value, out _)
                ? dateTime.Date
                : value;
    }

    private protected static object ConvertToExcelDouble(string value)
    {
        return value is null or "" or "-"
            ? "-"
            : double.TryParse(ReplaceE(value), out var doubleValue)
                ? doubleValue
                : value;
    }

    private protected static object ConvertToExcelInt(string value)
    {
        return value is null or "" or "-"
            ? "-"
            : int.TryParse(ReplaceE(value), out var intValue)
                ? intValue
                : value;
    }

    private protected static object ConvertToExcelString(string value)
    {
        return value is null or "" or "-"
            ? "-"
            : value;
    }

    #endregion

    private static string ReplaceE(string numberE)
    {
        return numberE.Replace("е", "E").Replace("Е", "E").Replace("e", "E")
            .Replace("(", "").Replace(")", "").Replace(".", ",");
    } 

    #endregion

    #endregion

    #region IDataGridColumn

    public virtual DataGridColumns GetColumnStructure(string param)
    {
        return null;
    }

    #endregion

    #region GeneratedRegex

    [GeneratedRegex("[-᠆‐‑‒–—―⸺⸻－﹘﹣－]")]
    protected static partial Regex DashesRegex();

    [GeneratedRegex(@"^\d{8}([\d_][Мм\d]\d{4})?$")]
    protected static partial Regex OkpoRegex();

    #endregion

    #region ConvertToTSVstring

    /// <summary>
    /// </summary>
    /// <returns>Возвращает строку с записанными данными в формате TSV(Tab-Separated Values) </returns>
    public abstract string ConvertToTSVstring();

    #endregion
}