using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace Models.Forms.Form4;

[Serializable]
[Form_Class(name: "Форма 4.1: Титульный лист организации")]
[Table(name: "form_41")]
public partial class Form41 : Form
{
    #region Constructor

    public Form41()
    {
        var x = this;
        FormNum.Value = "4.1";
    }

    #endregion

    #region Properties

    #region RegNo (2)

    public string RegNo_DB { get; set; } = "";


    [MaxLength(5)]
    [Column(TypeName = "varchar(5)")]
    [NotMapped]
    public RamAccess<string> RegNo
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(RegNo), out var value))
            {
                ((RamAccess<string>)value).Value = RegNo_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(RegNo_Validation, RegNo_DB);
            rm.PropertyChanged += RegNo_ValueChanged;
            Dictionary.Add(nameof(RegNo), rm);
            return (RamAccess<string>)Dictionary[nameof(RegNo)];
        }
        set
        {
            RegNo_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void RegNo_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Length > 5
                ? value1[..5]
                : value1;
        }
        if (RegNo_DB != value1)
        {
            RegNo_DB = value1;
            OnPropertyChanged(nameof(RowColor));
        }
    }

    private bool RegNo_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region Okpo (3)

    [MaxLength(14)]
    [Column(TypeName = "varchar(14)")]
    public string Okpo_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> Okpo
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Okpo), out var value))
            {
                ((RamAccess<string>)value).Value = Okpo_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Okpo_Validation, Okpo_DB);
            rm.PropertyChanged += Okpo_ValueChanged;
            Dictionary.Add(nameof(Okpo), rm);
            return (RamAccess<string>)Dictionary[nameof(Okpo)];
        }
        set
        {
            Okpo_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void Okpo_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Length > 14
            ? value1[..14]
            : value1;
        }
        if (Okpo_DB != value1)
        {
            Okpo_DB = value1;
            OnPropertyChanged(nameof(RowColor));
        }
    }

    private bool Okpo_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region OrganizationName (4)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    public string OrganizationName_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> OrganizationName
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(OrganizationName), out var value))
            {
                ((RamAccess<string>)value).Value = OrganizationName_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(OrganizationName_Validation, OrganizationName_DB);
            rm.PropertyChanged += OrganizationName_ValueChanged;
            Dictionary.Add(nameof(OrganizationName), rm);
            return (RamAccess<string>)Dictionary[nameof(OrganizationName)];
        }
        set
        {
            OrganizationName_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void OrganizationName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        }
        if (OrganizationName_DB != value1)
        {
            OrganizationName_DB = value1;
            OnPropertyChanged(nameof(RowColor));
        }
    }

    private bool OrganizationName_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region LicenseOrRegistrationInfo (5)

    public string LicenseOrRegistrationInfo_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> LicenseOrRegistrationInfo
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(LicenseOrRegistrationInfo), out var value))
            {
                ((RamAccess<string>)value).Value = LicenseOrRegistrationInfo_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(LicenseOrRegistrationInfo_Validation, LicenseOrRegistrationInfo_DB);
            rm.PropertyChanged += LicenseOrRegistrationInfo_ValueChanged;
            Dictionary.Add(nameof(LicenseOrRegistrationInfo), rm);
            return (RamAccess<string>)Dictionary[nameof(LicenseOrRegistrationInfo)];
        }
        set
        {
            LicenseOrRegistrationInfo_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void LicenseOrRegistrationInfo_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (LicenseOrRegistrationInfo_DB != value1)
        {
            LicenseOrRegistrationInfo_DB = value1;
            OnPropertyChanged(nameof(RowColor));
        }
    }

    private bool LicenseOrRegistrationInfo_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region NumOfFormsWithInventarizationInfo (6)

    public int NumOfFormsWithInventarizationInfo_DB { get; set; } = 0;

    [NotMapped]
    public RamAccess<int> NumOfFormsWithInventarizationInfo
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(NumOfFormsWithInventarizationInfo), out var value))
            {
                ((RamAccess<int>)value).Value = NumOfFormsWithInventarizationInfo_DB;
                return (RamAccess<int>)value;
            }
            var rm = new RamAccess<int>(NumOfFormsWithInventarizationInfo_Validation, NumOfFormsWithInventarizationInfo_DB);
            rm.PropertyChanged += NumOfFormsWithInventarizationInfo_ValueChanged;
            Dictionary.Add(nameof(NumOfFormsWithInventarizationInfo), rm);
            return (RamAccess<int>)Dictionary[nameof(NumOfFormsWithInventarizationInfo)];
        }
        set
        {
            NumOfFormsWithInventarizationInfo_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void NumOfFormsWithInventarizationInfo_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<int>)value).Value;
        if (NumOfFormsWithInventarizationInfo_DB != value1)
        {
            NumOfFormsWithInventarizationInfo_DB = value1;
            OnPropertyChanged(nameof(RowColor));
        }
    }

    private bool NumOfFormsWithInventarizationInfo_Validation(RamAccess<int> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region NumOfFormsWithoutInventarizationInfo (7)

    public int NumOfFormsWithoutInventarizationInfo_DB { get; set; } = 0;

    [NotMapped]
    public RamAccess<int> NumOfFormsWithoutInventarizationInfo
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(NumOfFormsWithoutInventarizationInfo), out var value))
            {
                ((RamAccess<int>)value).Value = NumOfFormsWithoutInventarizationInfo_DB;
                return (RamAccess<int>)value;
            }
            var rm = new RamAccess<int>(NumOfFormsWithoutInventarizationInfo_Validation, NumOfFormsWithoutInventarizationInfo_DB);
            rm.PropertyChanged += NumOfFormsWithoutInventarizationInfo_ValueChanged;
            Dictionary.Add(nameof(NumOfFormsWithoutInventarizationInfo), rm);
            return (RamAccess<int>)Dictionary[nameof(NumOfFormsWithoutInventarizationInfo)];
        }
        set
        {
            NumOfFormsWithoutInventarizationInfo_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void NumOfFormsWithoutInventarizationInfo_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<int>)value).Value;
        if (NumOfFormsWithoutInventarizationInfo_DB != value1)
        {
            NumOfFormsWithoutInventarizationInfo_DB = value1;
            OnPropertyChanged(nameof(RowColor));
        }
    }

    private bool NumOfFormsWithoutInventarizationInfo_Validation(RamAccess<int> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region NumOfForms212 (8)

    public int NumOfForms212_DB { get; set; } = 0;

    [NotMapped]
    public RamAccess<int> NumOfForms212
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(NumOfForms212), out var value))
            {
                ((RamAccess<int>)value).Value = NumOfForms212_DB;
                return (RamAccess<int>)value;
            }
            var rm = new RamAccess<int>(NumOfForms212_Validation, NumOfForms212_DB);
            rm.PropertyChanged += NumOfForms212_ValueChanged;
            Dictionary.Add(nameof(NumOfForms212), rm);
            return (RamAccess<int>)Dictionary[nameof(NumOfForms212)];
        }
        set
        {
            NumOfForms212_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void NumOfForms212_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<int>)value).Value;
        if (NumOfForms212_DB != value1)
        {
            NumOfForms212_DB = value1;
            OnPropertyChanged(nameof(RowColor));
        }
    }

    private bool NumOfForms212_Validation(RamAccess<int> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region Note (9)

    public string Note_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> Note
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Note), out var value))
            {
                ((RamAccess<string>)value).Value = Note_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Note_Validation, Note_DB);
            rm.PropertyChanged += Note_ValueChanged;
            Dictionary.Add(nameof(Note), rm);
            return (RamAccess<string>)Dictionary[nameof(Note)];
        }
        set
        {
            Note_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void Note_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (Note_DB != value1)
        {
            Note_DB = value1;
            OnPropertyChanged(nameof(RowColor));
        }
    }

    private bool Note_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region RowColor

    [NotMapped]
    public Color RowColor
    {
        get
        {
            if (LicenseOrRegistrationInfo.Value is "" or null
                && NumOfFormsWithInventarizationInfo.Value <= 0
                && NumOfFormsWithoutInventarizationInfo.Value <= 0
                && NumOfForms212.Value <= 0
                && Note.Value is "" or null)
            {
                ToolTipText = "Если у организации нет отчетов и сведений о лицензии, необходимо заполнить ячейку \"Примечение\"";
                return Color.FromArgb(50, 255, 255, 0);
            }

            //if (NumOfFormsWithInventarizationInfo.Value <= 0)
            //{
            //    ToolTipText = "Нет инвентаризационного отчета";
            //    return Color.FromArgb(50, 255, 0, 0);
            //}

            if ((NumOfFormsWithInventarizationInfo.Value > 0
                || NumOfFormsWithoutInventarizationInfo.Value > 0
                || NumOfForms212.Value > 0)
                && LicenseOrRegistrationInfo.Value is "" or null)
            {
                ToolTipText = "Если у организации есть, хотя бы 1 отчет, то необходимо заполнить \"Сведения о лицензии\"";
                return Color.FromArgb(50, 139, 0, 255);
            }



            ToolTipText = "";
            return Color.FromArgb(0,255,255,255); // Значение по умолчанию
        }
    }

    [NotMapped]
    private string _toolTipText = "";

    [NotMapped]
    public string ToolTipText
    {
        get
        {
            return _toolTipText;
        }
        set
        {
            _toolTipText = value;
            OnPropertyChanged();
        }
    }
    #endregion 
    #endregion

    #region Validation

    public override bool Object_Validation()
    {
        return !(RegNo.HasErrors ||
                 Okpo.HasErrors ||
                 OrganizationName.HasErrors ||
                 LicenseOrRegistrationInfo.HasErrors ||
                 NumOfFormsWithInventarizationInfo.HasErrors ||
                 NumOfFormsWithoutInventarizationInfo.HasErrors ||
                 NumOfForms212.HasErrors ||
                 Note.HasErrors);
    }

    #endregion

    #region ParseInnerText
    private static string ParseInnerText(string text)
    {
        return text.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
    }
    #endregion

    #region IExcel

    public override void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
            NumberInOrder_DB = int.TryParse(Convert.ToString(worksheet.Cells[row, 1].Value), out var intValue)
                ? intValue
                : 0;
        

        RegNo_DB = Convert.ToString(worksheet.Cells[row, 2].Value).Trim();
        if(RegNo_DB.Count() > 5)
            RegNo_DB = RegNo_DB[..5];

        Okpo_DB = Convert.ToString(worksheet.Cells[row, 3].Value).Trim();
        if (Okpo_DB.Count() > 14)
            Okpo_DB = Okpo_DB[..14];

        OrganizationName_DB = Convert.ToString(worksheet.Cells[row, 4].Value).Trim();
        if (OrganizationName_DB.Count() > 256)
            OrganizationName_DB = OrganizationName_DB[..256];

        LicenseOrRegistrationInfo_DB = Convert.ToString(worksheet.Cells[row, 5].Value).Trim();

        NumOfFormsWithInventarizationInfo_DB = int.TryParse(Convert.ToString(worksheet.Cells[row, 6].Value), out intValue) ? intValue : 0;

        NumOfFormsWithoutInventarizationInfo_DB = int.TryParse(Convert.ToString(worksheet.Cells[row, 7].Value), out intValue) ? intValue : 0;

        NumOfForms212_DB = int.TryParse(Convert.ToString(worksheet.Cells[row, 8].Value), out intValue) ? intValue : 0;

        Note_DB = Convert.ToString(worksheet.Cells[row, 9].Value).Trim();
    }

    public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
            worksheet.Cells[row + 0, column + 0].Value = NumberInOrder_DB;
            worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(RegNo_DB);
            worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelString(Okpo_DB);
            worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelString(OrganizationName_DB);
            worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelString(LicenseOrRegistrationInfo_DB);
            worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = NumOfFormsWithInventarizationInfo_DB == 0 ? "": NumOfFormsWithInventarizationInfo_DB;
            worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = NumOfFormsWithoutInventarizationInfo_DB == 0 ? "" : NumOfFormsWithoutInventarizationInfo_DB;
            worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = NumOfForms212_DB == 0 ? "" : NumOfForms212_DB;
            worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ConvertToExcelString(Note_DB);

            return 9;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string id = "")
    {
        throw new NotImplementedException();
    }

    #endregion

    #region IDataGridColumn

    // Заглушка
    public override DataGridColumns GetColumnStructure(string param)
    {
        return null;
    }

    #endregion

    #region ConvertToTSVstring

    /// <summary>
    /// </summary>
    /// <returns>Возвращает строку с записанными данными в формате TSV(Tab-Separated Values) </returns>
    public override string ConvertToTSVstring()
    {
        // Создаем текстовое представление (TSV - tab-separated values)
        var str =
            $"{NumberInOrder.Value}\t" +
            $"{RegNo.Value}\t" +
            $"{Okpo.Value}\t" +
            $"{OrganizationName.Value}\t" +
            $"{LicenseOrRegistrationInfo.Value}\t" +
            $"{NumOfFormsWithInventarizationInfo.Value}\t" +
            $"{NumOfFormsWithoutInventarizationInfo.Value}\t" +
            $"{NumOfForms212.Value}\t" +
            $"{Note.Value}";
        return str;
    }

    #endregion
}