using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
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

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> RegNo
    {
        get;
        set;
    }

    private void RegNo_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool RegNo_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region Okpo (3)

    public string Okpo_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> Okpo
    {
        get;
        set;
    }

    private void Okpo_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool Okpo_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region OrganizationName (4)

    public string OrganizationName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> OrganizationName
    {
        get;
        set;
    }

    private void OrganizationName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool OrganizationName_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region LicenseOrRegistrationInfo (5)

    public string LicenseOrRegistrationInfo_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> LicenseOrRegistrationInfo
    {
        get;
        set;
    }

    private void LicenseOrRegistrationInfo_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool LicenseOrRegistrationInfo_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region NumOfFormsWithInventarizationInfo (6)

    public string NumOfFormsWithInventarizationInfo_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> NumOfFormsWithInventarizationInfo
    {
        get;
        set;
    }

    private void NumOfFormsWithInventarizationInfo_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool NumOfFormsWithInventarizationInfo_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region NumOfFormsWithoutInventarizationInfo (7)

    public string NumOfFormsWithoutInventarizationInfo_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> NumOfFormsWithoutInventarizationInfo
    {
        get;
        set;
    }

    private void NumOfFormsWithoutInventarizationInfoUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool NumOfFormsWithoutInventarizationInfoUprav_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region NumOfForms212 (8)

    public string NumOfForms212_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> NumOfForms212
    {
        get;
        set;
    }

    private void NumOfForms212_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool NumOfForms212_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region Note (9)

    public string Note_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> Note
    {
        get;
        set;
    }

    private void NoteOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool Note_Validation(RamAccess<string> value)
    {
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
                 NumOfForms212.HasErrors);
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
        // Заглушка
        var str = "Форма 4.0";
        return str;
    }

    #endregion
}