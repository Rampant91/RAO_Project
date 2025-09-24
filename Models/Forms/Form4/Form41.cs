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
        value1 = value1.Length > 5
            ? value1[..5]
            : value1;
        if (RegNo_DB != value1)
        {
            RegNo_DB = value1;
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
            rm.PropertyChanged += RegNo_ValueChanged;
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
        value1 = value1.Length > 14
            ? value1[..14]
            : value1;
        if (Okpo_DB != value1)
        {
            Okpo_DB = value1;
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
        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (OrganizationName_DB != value1)
        {
            OrganizationName_DB = value1;
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
            if (Dictionary.TryGetValue(nameof(RegNo), out var value))
            {
                ((RamAccess<string>)value).Value = RegNo_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Note_Validation, RegNo_DB);
            rm.PropertyChanged += Note_ValueChanged;
            Dictionary.Add(nameof(RegNo), rm);
            return (RamAccess<string>)Dictionary[nameof(RegNo)];
        }
        set
        {
            RegNo_DB = ParseInnerText(value.Value);
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
        }
    }

    private bool Note_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
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
        throw new NotImplementedException();
    }

    public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        throw new NotImplementedException();
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
            $"{RegNo.Value}\t" +
            $"{Okpo.Value}\t" +
            $"{OrganizationName.Value}\t" +
            $"{LicenseOrRegistrationInfo.Value}\t" +
            $"{NumOfFormsWithInventarizationInfo.Value}\t" +
            $"{NumOfFormsWithoutInventarizationInfo.Value}\t" +
            $"{NumOfForms212.Value}\t" +
            $"{Note.Value}\t";
        return str;
    }

    #endregion
}