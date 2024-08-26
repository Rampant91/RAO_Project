using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using Models.Interfaces;
using OfficeOpenXml;

namespace Models.Forms;

[Table("notes")]
public class Note : IKey, IDataGridColumn
{
    #region Constructor
    
    public Note()
    {
        Init();
    }

    public Note(string rowNumber, string graphNumber, string comment)
    {
        RowNumber.Value = rowNumber;
        GraphNumber.Value = graphNumber;
        Comment.Value = comment;
        Init();
    }

    public void Init()
    {
        RowNumber_Validation(RowNumber);
        GraphNumber_Validation(GraphNumber);
        Comment_Validation(Comment);
    }

    #endregion

    #region Id

    [Key]
    public int Id { get; set; }

    #endregion

    #region Report
    
    [ForeignKey(nameof(Report))]
    public int? ReportId { get; set; }

    public virtual Report Report { get; set; }

    #endregion

    [NotMapped] 
    private Dictionary<string, RamAccess> Dictionary { get; set; } = new();

    #region Order
    
    public long Order { get; set; }

    public void SetOrder(long index) { }

    #endregion

    #region RowNumber

    public string? RowNumber_DB { get; set; }

    [NotMapped]
    [FormProperty(false, "№ строки", "1")]
#nullable enable
    public RamAccess<string?> RowNumber
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(RowNumber), out var value))
            {
                ((RamAccess<string?>)value).Value = RowNumber_DB;
                return (RamAccess<string?>)value;
            }
            var rm = new RamAccess<string?>(RowNumber_Validation, RowNumber_DB);
            rm.PropertyChanged += RowNumber_ValueChanged;
            Dictionary.Add(nameof(RowNumber), rm);
            return (RamAccess<string?>)Dictionary[nameof(RowNumber)];
        }
        set
        {
            RowNumber_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void RowNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        RowNumber_DB = ((RamAccess<string?>)value).Value;
    }

    private static bool RowNumber_Validation(RamAccess<string?> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region GraphNumber

    public string? GraphNumber_DB { get; set; }

    [NotMapped]
    [FormProperty(false, "№ графы", "2")]
#nullable enable
    public RamAccess<string?> GraphNumber
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(GraphNumber), out var value))
            {
                ((RamAccess<string?>)value).Value = GraphNumber_DB;
                return (RamAccess<string?>)value;
            }
            var rm = new RamAccess<string?>(GraphNumber_Validation, GraphNumber_DB);
            rm.PropertyChanged += GraphNumber_ValueChanged;
            Dictionary.Add(nameof(GraphNumber), rm);
            return (RamAccess<string?>)Dictionary[nameof(GraphNumber)];
        }
        set
        {
            GraphNumber_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void GraphNumber_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        GraphNumber_DB = ((RamAccess<string?>)value).Value;
    }

    private static bool GraphNumber_Validation(RamAccess<string?> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region Comment

    public string? Comment_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(false, "Пояснение", "3")]
    public RamAccess<string> Comment
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Comment), out RamAccess? value))
            {
                ((RamAccess<string>)value).Value = Comment_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Comment_Validation, Comment_DB);
            rm.PropertyChanged += Comment_ValueChanged;
            Dictionary.Add(nameof(Comment), rm);
            return (RamAccess<string>)Dictionary[nameof(Comment)];
        }
        set
        {
            Comment_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void Comment_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        Comment_DB = ((RamAccess<string>)value).Value;
    }

    private static bool Comment_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    //Для валидации
    //public bool Object_Validation() => true;
    //Для валидации

    #region PropertyChanged
    
    protected void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        OnPropertyChanged(args.PropertyName);
    }

    #endregion

    #region IExcel

    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        RowNumber_DB = Convert.ToString(worksheet.Cells[row, 1].Value);
        GraphNumber_DB = Convert.ToString(worksheet.Cells[row, 2].Value);
        Comment_DB = Convert.ToString(worksheet.Cells[row, 3].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        worksheet.Cells[row + 0, column + 0].Value = RowNumber_DB;
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = GraphNumber_DB;
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = Comment_DB;
        return 3;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        worksheet.Cells[row + 0, column + 0].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Note,Models")?.GetProperty(nameof(RowNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First()!).Names[0];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Note,Models")?.GetProperty(nameof(GraphNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First()!).Names[0];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Note,Models")?.GetProperty(nameof(Comment))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First()!).Names[0];
        return 3;
    }

    #endregion

    #region IDataGridColumn

    public DataGridColumns GetColumnStructure(string param = "")
    {
        var rowNumberN = ((FormPropertyAttribute)typeof(Note)
                .GetProperty(nameof(RowNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault()!)
            .GetDataColumnStructureD();
        rowNumberN.SizeCol = 100;
        rowNumberN.Binding = nameof(RowNumber);
            
        var graphNumberN = ((FormPropertyAttribute)typeof(Note)
                .GetProperty(nameof(GraphNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault()!)
            .GetDataColumnStructureD();
        graphNumberN.SizeCol = 100;
        graphNumberN.Binding = nameof(GraphNumber);
        rowNumberN += graphNumberN;

        var commentN = ((FormPropertyAttribute)typeof(Note)
                .GetProperty(nameof(Comment))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault()!)
            .GetDataColumnStructureD();
        commentN.SizeCol = 660;
        commentN.Binding = nameof(Comment);
        commentN.IsTextWrapping = true;
        rowNumberN += commentN;

        return rowNumberN;
    }

    #endregion
}