using System;
using System.ComponentModel;
using System.Linq;
using Models.Attributes;
using OfficeOpenXml;

namespace Models.Forms.Form2;

public abstract partial class Form2 : Form
{
    [FormProperty(true, "Форма")]
    public Form2() { }

    protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        OnPropertyChanged(args.PropertyName);
    }

    #region IExcel

    public override void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        NumberInOrder_DB = int.TryParse(Convert.ToString(worksheet.Cells[row, 1].Value), out var intValue)
            ? intValue
            : 0;
    }

    public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        worksheet.Cells[row, column].Value = NumberInOrder_DB == 0
            ? sumNumber
            : NumberInOrder_DB;
        return 1;
    }

    protected static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        worksheet.Cells[row, column].Value = ((FormPropertyAttribute)typeof(Form).GetProperty(nameof(NumberInOrder))
            ?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[2];
        return 1;
    }

    #endregion
}