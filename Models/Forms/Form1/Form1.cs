using Models.DataAccess;
using System;
using Spravochniki;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Models.Attributes;
using OfficeOpenXml;
using Models.Collections;
using Models.Forms;

namespace Models.Abstracts;

public abstract class Form1 : Form
{
    [FormProperty(true,"Форма")]

    [NotMapped]
    public bool flag = false;
    public Form1():base()
    {

    }
    protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        OnPropertyChanged(args.PropertyName);
    }

    public DataGridColumns GetDataGridColumn() 
    {
        return null;
    } 

    #region OperationCode
    public string OperationCode_DB { get; set; } = "";
    public bool OperationCode_Hidden_Priv { get; set; }
    [NotMapped]
    public bool OperationCode_Hidden
    {
        get => OperationCode_Hidden_Priv;
        set
        {
            OperationCode_Hidden_Priv = value;
        }
    }

    [NotMapped]
    [FormProperty(true, "Сведения об операции","код","2")]
    public RamAccess<string> OperationCode
    {
        get
        {
            if (!OperationCode_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(OperationCode)))
                {
                    ((RamAccess<string>)Dictionary[nameof(OperationCode)]).Value = OperationCode_DB;
                    return (RamAccess<string>)Dictionary[nameof(OperationCode)];
                }
                else
                {
                    var rm = new RamAccess<string>(OperationCode_Validation, OperationCode_DB);
                    rm.PropertyChanged += OperationCodeValueChanged;
                    Dictionary.Add(nameof(OperationCode), rm);
                    return (RamAccess<string>)Dictionary[nameof(OperationCode)];
                }
            }
            else
            {
                var tmp = new RamAccess<string>(null, null);
                return tmp;
            }
        }
        set
        {
            if (!OperationCode_Hidden_Priv)
            {
                OperationCode_DB = value.Value;
                OnPropertyChanged(nameof(OperationCode));
            }
        }
    }
    private void OperationCodeValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            OperationCode_DB = ((RamAccess<string>)Value).Value;
        }
    }
    protected virtual bool OperationCode_Validation(RamAccess<string> value)//Ready
    {

        value.ClearErrors();
        return true;
    }
    #endregion

    #region OperationDate
    public string OperationDate_DB { get; set; } = "";
    public bool OperationDate_Hidden_Priv { get; set; }
    [NotMapped]
    public bool OperationDate_Hidden
    {
        get => OperationDate_Hidden_Priv;
        set
        {
            OperationDate_Hidden_Priv = value;
        }
    }

    [NotMapped]
    [FormProperty(true, "Сведения об операции", "дата", "3")]
    public RamAccess<string> OperationDate
    {
        get
        {
            if (!OperationDate_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(OperationDate)))
                {
                    ((RamAccess<string>)Dictionary[nameof(OperationDate)]).Value = OperationDate_DB;
                    return (RamAccess<string>)Dictionary[nameof(OperationDate)];
                }
                else
                {
                    var rm = new RamAccess<string>(OperationDate_Validation, OperationDate_DB);
                    rm.PropertyChanged += OperationDateValueChanged;
                    Dictionary.Add(nameof(OperationDate), rm);
                    return (RamAccess<string>)Dictionary[nameof(OperationDate)];
                }
            }
            else
            {
                var tmp = new RamAccess<string>(null, null);
                return tmp;
            }
        }
        set
        {
            if (!OperationDate_Hidden_Priv)
            {
                OperationDate_DB = value.Value;
                OnPropertyChanged(nameof(OperationDate));
            }
        }
    }
    private void OperationDateValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var tmp = ((RamAccess<string>)Value).Value;
            Regex b = new("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            OperationDate_DB = tmp;
        }
    }
    protected virtual bool OperationDate_Validation(RamAccess<string> value)//Ready
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
        return true;
    }
    #endregion

    #region DocumentVid
        
    public byte? DocumentVid_DB { get; set; }
    public bool DocumentVid_Hidden_Priv { get; set; }
    [NotMapped]
    public bool DocumentVid_Hidden
    {
        get => DocumentVid_Hidden_Priv;
        set
        {
            DocumentVid_Hidden_Priv = value;
        }
    }

    [NotMapped]
    [FormProperty(true,"Документ","вид", "16")]
    public RamAccess<byte?> DocumentVid
    {
        get
        {
            if (!DocumentVid_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(DocumentVid)))
                {
                    ((RamAccess<byte?>)Dictionary[nameof(DocumentVid)]).Value = DocumentVid_DB;
                    return (RamAccess<byte?>)Dictionary[nameof(DocumentVid)];
                }
                else
                {
                    var rm = new RamAccess<byte?>(DocumentVid_Validation, DocumentVid_DB);
                    rm.PropertyChanged += DocumentVidValueChanged;
                    Dictionary.Add(nameof(DocumentVid), rm);
                    return (RamAccess<byte?>)Dictionary[nameof(DocumentVid)];
                }
            }
            else
            {
                var tmp = new RamAccess<byte?>(null, null);
                return tmp;
            }
        }
        set
        {
            DocumentVid_DB = value.Value;
            OnPropertyChanged(nameof(DocumentVid));
        }
    }

    private void DocumentVidValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            DocumentVid_DB = ((RamAccess<byte?>)Value).Value;
        }
    }
    protected virtual bool DocumentVid_Validation(RamAccess<byte?> value)//Ready
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        foreach (var item in Spravochniks.SprDocumentVidName)
        {
            if (value.Value == item.Item1)
            {
                return true;
            }
        }
        value.AddError("Недопустимое значение");
        return false;
    }
    #endregion

    #region DocumentNumber
    public string DocumentNumber_DB { get; set; } = "";
    public bool DocumentNumber_Hidden_Priv { get; set; }
    [NotMapped]
    public bool DocumentNumber_Hidden
    {
        get => DocumentNumber_Hidden_Priv;
        set
        {
            DocumentNumber_Hidden_Priv = value;
        }
    }

    [NotMapped]
    [FormProperty(true,"Документ", "номер", "17")]
    public RamAccess<string> DocumentNumber
    {
        get
        {
            if (!DocumentNumber_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(DocumentNumber)))
                {
                    ((RamAccess<string>)Dictionary[nameof(DocumentNumber)]).Value = DocumentNumber_DB;
                    return (RamAccess<string>)Dictionary[nameof(DocumentNumber)];
                }
                else
                {
                    var rm = new RamAccess<string>(DocumentNumber_Validation, DocumentNumber_DB);
                    rm.PropertyChanged += DocumentNumberValueChanged;
                    Dictionary.Add(nameof(DocumentNumber), rm);
                    return (RamAccess<string>)Dictionary[nameof(DocumentNumber)];
                }
            }
            else
            {
                var tmp = new RamAccess<string>(null, null);
                return tmp;
            }
        }
        set
        {
            if (!DocumentNumber_Hidden_Priv)
            {
                DocumentNumber_DB = value.Value;
                OnPropertyChanged(nameof(DocumentNumber));
            }
        }
    }
    private void DocumentNumberValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            DocumentNumber_DB = ((RamAccess<string>)Value).Value;
        }
    }
    protected virtual bool DocumentNumber_Validation(RamAccess<string> value)//Ready
    { return true; }
    #endregion

    #region DocumentDate
    public string DocumentDate_DB { get; set; } = "";
    public bool DocumentDate_Hidden_Priv { get; set; }
    [NotMapped]
    public bool DocumentDate_Hidden
    {
        get => DocumentDate_Hidden_Priv;
        set
        {
            DocumentDate_Hidden_Priv = value;
        }
    }

    [NotMapped]
    [FormProperty(true,"Документ", "дата", "18")]
    public RamAccess<string> DocumentDate
    {
        get
        {
            if (!DocumentDate_Hidden_Priv)
            {
                if (Dictionary.ContainsKey(nameof(DocumentDate)))
                {
                    ((RamAccess<string>)Dictionary[nameof(DocumentDate)]).Value = DocumentDate_DB;
                    return (RamAccess<string>)Dictionary[nameof(DocumentDate)];
                }
                else
                {
                    var rm = new RamAccess<string>(DocumentDate_Validation, DocumentDate_DB);
                    rm.PropertyChanged += DocumentDateValueChanged;
                    Dictionary.Add(nameof(DocumentDate), rm);
                    return (RamAccess<string>)Dictionary[nameof(DocumentDate)];
                }
            }
            else
            {
                var tmp = new RamAccess<string>(null, null);
                return tmp;
            }
        }
        set
        {
            if (!DocumentDate_Hidden_Priv)
            {
                DocumentDate_DB = value.Value;
                OnPropertyChanged(nameof(DocumentDate));
            }
        }
    }
    private void DocumentDateValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var tmp = ((RamAccess<string>)Value).Value;
            Regex b = new("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            DocumentDate_DB = tmp;
        }
    }
    protected virtual bool DocumentDate_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if(string.IsNullOrEmpty(value.Value))
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
        return true;
    }
    #endregion

    #region IExcel
    public override void ExcelGetRow(ExcelWorksheet worksheet, int Row) 
    {
        NumberInOrder_DB = Convert.ToInt32(worksheet.Cells[Row, 1].Value);
        OperationCode_DB = Convert.ToString(worksheet.Cells[Row, 2].Value);
        OperationDate_DB = Convert.ToString(worksheet.Cells[Row, 3].Value);
    }
    public override int ExcelRow(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true, string SumNumber = "")
    {
        worksheet.Cells[Row + (!Transpon ? 0 : 0), Column + (Transpon ? 0 : 0)].Value = NumberInOrder_DB;
        worksheet.Cells[Row + (!Transpon ? 1 : 0), Column + (Transpon ? 1 : 0)].Value = OperationCode_DB;
        worksheet.Cells[Row + (!Transpon ? 2 : 0), Column + (Transpon ? 2 : 0)].Value = OperationDate_DB;

        return 3;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true)
    {
        worksheet.Cells[Row + (!Transpon ? 0 : 0), Column + (Transpon ? 0 : 0)].Value = ((FormPropertyAttribute)typeof(Form).GetProperty(nameof(NumberInOrder))
            .GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[2];     
        worksheet.Cells[Row + (!Transpon ? 1 : 0), Column + (Transpon ? 1 : 0)].Value = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(OperationCode))
            .GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];     
        worksheet.Cells[Row + (!Transpon ? 2 : 0), Column + (Transpon ? 2 : 0)].Value = ((FormPropertyAttribute)typeof(Form1).GetProperty(nameof(OperationDate))
            .GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];

        return 3;
    }
    #endregion
}