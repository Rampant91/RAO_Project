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
[Form_Class(name: "Форма 4.0: Титульный лист организации")]
[Table(name: "form_40")]
public partial class Form40 : Form
{
    #region Constructor

    public Form40()
    {
        FormNum.Value = "4.0";
    }

    #endregion

    #region Properties

    #region SubjectRF (2)

    public string SubjectRF_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> SubjectRF
    {
        get;
        set;
    }

    private void SubjectRF_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool SubjectRF_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region Year (3)

    public string Year_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> Year
    {
        get;
        set;
    }

    private void Year_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool Year_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region NameOrganUprav (4)

    public string NameOrganUprav_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> NameOrganUprav
    {
        get;
        set;
    }

    private void NameOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool NameOrganUprav_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region ShortNameOrganUprav (5)

    public string ShortNameOrganUprav_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> ShortNameOrganUprav
    {
        get;
        set;
    }

    private void ShortNameOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool ShortNameOrganUprav_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region AddressOrganUprav (6)

    public string AddressOrganUprav_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> AddressOrganUprav
    {
        get;
        set;
    }

    private void AddressOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool AddressOrganUprav_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region GradeFioDirectorOrganUprav (7)

    public string GradeFioDirectorOrganUprav_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> GradeFioDirectorOrganUprav
    {
        get;
        set;
    }

    private void GradeFioDirectorOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool GradeFioDirectorOrganUprav_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region GradeFioExecutorOrganUprav (8)

    public string GradeFioExecutorOrganUprav_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> GradeFioExecutorOrganUprav
    {
        get;
        set;
    }

    private void GradeFioExecutorOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool GradeFioExecutorOrganUprav_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region TelephoneOrganUprav (9)

    public string TelephoneOrganUprav_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> TelephoneOrganUprav
    {
        get;
        set;
    }

    private void TelephoneOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool TelephoneOrganUprav_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region FaxOrganUprav (10)

    public string FaxOrganUprav_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> FaxOrganUprav
    {
        get;
        set;
    }

    private void FaxOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool FaxOrganUprav_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region EmailOrganUprav (11)

    public string EmailOrganUprav_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> EmailOrganUprav
    {
        get;
        set;
    }

    private void EmailOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool EmailOrganUprav_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region NameRiac (12)

    public string NameRiac_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> NameRiac
    {
        get;
        set;
    }

    private void NameRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool NameRiac_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region ShortNameRiac (13)

    public string ShortNameRiac_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> ShortNameRiac
    {
        get;
        set;
    }

    private void ShortNameRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool ShortNameRiac_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region AddressRiac (14)

    public string AddressRiac_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> AddressRiac
    {
        get;
        set;
    }

    private void AddressRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool AddressRiac_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region GradeFioDirectorRiac (15)

    public string GradeFioDirectorRiac_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> GradeFioDirectorRiac
    {
        get;
        set;
    }

    private void GradeFioDirectorRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool GradeFioDirectorRiac_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region GradeFioExecutorRiac (16)

    public string GradeFioExecutorRiac_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> GradeFioExecutorRiac
    {
        get;
        set;
    }

    private void GradeFioExecutorRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool GradeFioExecutorRiac_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region TelephoneRiac (17)

    public string TelephoneRiac_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> TelephoneRiac
    {
        get;
        set;
    }

    private void TelephoneRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool TelephoneRiac_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region FaxRiac (18)

    public string FaxRiac_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> FaxRiac
    {
        get;
        set;
    }

    private void FaxRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool FaxRiac_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region EmailRiac (19)

    public string EmailRiac_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации")]
    public RamAccess<string> EmailRiac
    {
        get;
        set;
    }

    private void EmailRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
    }

    private bool EmailRiac_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #endregion

    #region Validation

    public override bool Object_Validation()
    {
        return !(SubjectRF.HasErrors ||
                 Year.HasErrors ||
                 NameOrganUprav.HasErrors ||
                 ShortNameOrganUprav.HasErrors ||
                 AddressOrganUprav.HasErrors ||
                 GradeFioDirectorOrganUprav.HasErrors ||
                 GradeFioExecutorOrganUprav.HasErrors ||
                 TelephoneOrganUprav.HasErrors ||
                 FaxOrganUprav.HasErrors ||
                 EmailOrganUprav.HasErrors ||
                 NameRiac.HasErrors ||
                 ShortNameRiac.HasErrors ||
                 AddressRiac.HasErrors ||
                 GradeFioDirectorOrganUprav.HasErrors ||
                 GradeFioExecutorRiac.HasErrors ||
                 TelephoneRiac.HasErrors ||
                 FaxRiac.HasErrors ||
                 EmailRiac.HasErrors);
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