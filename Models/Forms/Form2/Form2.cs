using System;
using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using Models.Attributes;
using Models.Forms;
using OfficeOpenXml;

namespace Models.Abstracts;

public abstract class Form2 : Form
{
    [FormProperty(true,"Форма")]
    public Form2()
    {

    }
    protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        OnPropertyChanged(args.PropertyName);
    }

    #region CorrectionNumber

    public byte CorrectionNumber_DB { get; set; }

    [NotMapped]
    [FormProperty(true,"Номер корректировки")]
    public RamAccess<byte> CorrectionNumber
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(CorrectionNumber)))
            {
                ((RamAccess<byte>)Dictionary[nameof(CorrectionNumber)]).Value = CorrectionNumber_DB;
                return (RamAccess<byte>)Dictionary[nameof(CorrectionNumber)];
            }
            else
            {
                var rm = new RamAccess<byte>(CorrectionNumber_Validation, CorrectionNumber_DB);
                rm.PropertyChanged += CorrectionNumberValueChanged;
                Dictionary.Add(nameof(CorrectionNumber), rm);
                return (RamAccess<byte>)Dictionary[nameof(CorrectionNumber)];
            }
        }
        set
        {
            CorrectionNumber_DB = value.Value;
            OnPropertyChanged(nameof(CorrectionNumber));
        }
    }

    private void CorrectionNumberValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            CorrectionNumber_DB = ((RamAccess<byte>) Value).Value;
        }
    }

    private bool CorrectionNumber_Validation(RamAccess<byte> value)
    {
        value.ClearErrors();
        return true;
    }

    //CorrectionNumber property

    #endregion

    #region IExcel
    public override void ExcelGetRow(ExcelWorksheet worksheet, int Row)
    {
        NumberInOrder_DB = Convert.ToInt32(worksheet.Cells[Row, 1].Value);
    }
    public override int ExcelRow(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true, string SumNumber = "")
    {
        if (NumberInOrder_DB == 0)
        {
            worksheet.Cells[Row, Column].Value = SumNumber;
        }
        else
        {
            worksheet.Cells[Row, Column].Value = NumberInOrder_DB;
        }

        return 1;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int Row, int Column,bool Transpon=true)
    {
        worksheet.Cells[Row, Column].Value = ((FormPropertyAttribute)typeof(Form).GetProperty(nameof(NumberInOrder))
            .GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[2];

        return 1;
    }
    #endregion
}