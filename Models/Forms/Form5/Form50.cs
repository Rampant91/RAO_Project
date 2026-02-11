using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace Models.Forms.Form5;
[Serializable]
[Form_Class(name: "Форма 5.0: Титульный лист организации")]
[Table(name: "form_50")]
public partial class Form50 : Form
{
    #region Constructor

    public Form50()
    {
        FormNum.Value = "5.0";
    }

    #endregion

    #region Properties

    #region VIAC (1)

    #region ExecutiveAuthority

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    public string ExecutiveAuthority_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> ExecutiveAuthority
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ExecutiveAuthority), out var value))
            {
                ((RamAccess<string>)value).Value = ExecutiveAuthority_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(ExecutiveAuthority_Validation, ExecutiveAuthority_DB);
            rm.PropertyChanged += ExecutiveAuthority_ValueChanged;
            Dictionary.Add(nameof(ExecutiveAuthority), rm);
            return (RamAccess<string>)Dictionary[nameof(ExecutiveAuthority)];
        }
        set
        {
            ExecutiveAuthority_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void ExecutiveAuthority_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (ExecutiveAuthority_DB != value1)
        {
            ExecutiveAuthority_DB = value1;

            if (!string.IsNullOrEmpty(value1))
            {
                Rosatom.Value = false;
                MinObr.Value = false;
            }
        }
    }

    private bool ExecutiveAuthority_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region Rosatom

    public bool Rosatom_DB { get; set; } = false;

    [NotMapped]
    public RamAccess<bool> Rosatom
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Rosatom), out var value))
            {
                ((RamAccess<bool>)value).Value = Rosatom_DB;
                return (RamAccess<bool>)value;
            }
            var rm = new RamAccess<bool>(Rosatom_Validation, Rosatom_DB);
            rm.PropertyChanged += Rosatom_ValueChanged;
            Dictionary.Add(nameof(Rosatom), rm);
            return (RamAccess<bool>)Dictionary[nameof(Rosatom)];
        }
        set
        {
            Rosatom_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void Rosatom_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<bool>)value).Value;

        if (Rosatom_DB != value1)
        {
            Rosatom_DB = value1;
            if (value1 == true)
            {
                ExecutiveAuthority.Value = "";
                MinObr.Value = false;
            }
        }
    }

    private bool Rosatom_Validation(RamAccess<bool> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region MinObr

    public bool MinObr_DB { get; set; } = false;

    [NotMapped]
    public RamAccess<bool> MinObr
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(MinObr), out var value))
            {
                ((RamAccess<bool>)value).Value = MinObr_DB;
                return (RamAccess<bool>)value;
            }
            var rm = new RamAccess<bool>(MinObr_Validation, MinObr_DB);
            rm.PropertyChanged += MinObr_ValueChanged;
            Dictionary.Add(nameof(MinObr), rm);
            return (RamAccess<bool>)Dictionary[nameof(MinObr)];
        }
        set
        {
            MinObr_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void MinObr_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<bool>)value).Value;

        if (MinObr_DB != value1)
        {
            MinObr_DB = value1;

            if (value1 == true)
            {
                ExecutiveAuthority.Value = "";
                Rosatom.Value = false;
            }
        }
    }

    private bool MinObr_Validation(RamAccess<bool> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #endregion


    // Form5X.Year (2)

    #region Name (3)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    public string Name_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> Name
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Name), out var value))
            {
                ((RamAccess<string>)value).Value = Name_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Name_Validation, Name_DB);
            rm.PropertyChanged += Name_ValueChanged;
            Dictionary.Add(nameof(Name), rm);
            return (RamAccess<string>)Dictionary[nameof(Name)];
        }
        set
        {
            Name_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void Name_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (Name_DB != value1)
        {
            Name_DB = value1;
        }
    }

    private bool Name_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region ShortName (4)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    [AllowNull]
    public string? ShortName_DB { get; set; }

    [NotMapped]
    public RamAccess<string?> ShortName
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ShortName), out var value))
            {
                ((RamAccess<string?>)value).Value = ShortName_DB;
                return (RamAccess<string?>)value;
            }
            var rm = new RamAccess<string?>(ShortName_Validation, ShortName_DB);
            rm.PropertyChanged += ShortName_ValueChanged;
            Dictionary.Add(nameof(ShortName), rm);
            return (RamAccess<string?>)Dictionary[nameof(ShortName)];
        }
        set
        {
            ShortName_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void ShortName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string?>)value).Value;

        if (value1 is null) return;


        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (ShortName_DB != value1)
        {
            ShortName_DB = value1;
        }
    }

    private bool ShortName_Validation(RamAccess<string?> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region Address (5)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    public string Address_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> Address
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Address), out var value))
            {
                ((RamAccess<string>)value).Value = Address_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Address_Validation, Address_DB);
            rm.PropertyChanged += Address_ValueChanged;
            Dictionary.Add(nameof(Address), rm);
            return (RamAccess<string>)Dictionary[nameof(Address)];
        }
        set
        {
            Address_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void Address_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (Address_DB != value1)
        {
            Address_DB = value1;
        }
    }
    private bool Address_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region GradeFioDirector (8)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    [AllowNull]
    public string? GradeFioDirector_DB { get; set; }

    [NotMapped]
    public RamAccess<string?> GradeFioDirector
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(GradeFioDirector), out var value))
            {
                ((RamAccess<string?>)value).Value = GradeFioDirector_DB;
                return (RamAccess<string?>)value;
            }
            var rm = new RamAccess<string?>(GradeFioDirector_Validation, GradeFioDirector_DB);
            rm.PropertyChanged += GradeFioDirector_ValueChanged;
            Dictionary.Add(nameof(GradeFioDirector), rm);
            return (RamAccess<string?>)Dictionary[nameof(GradeFioDirector)];
        }
        set
        {
            GradeFioDirector_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void GradeFioDirector_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string?>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (GradeFioDirector_DB != value1)
        {
            GradeFioDirector_DB = value1;
        }
    }

    private bool GradeFioDirector_Validation(RamAccess<string?> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region GradeFioExecutor (9)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    [AllowNull]
    public string? GradeFioExecutor_DB { get; set; }

    [NotMapped]
    public RamAccess<string?> GradeFioExecutor
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(GradeFioExecutor), out var value))
            {
                ((RamAccess<string?>)value).Value = GradeFioExecutor_DB;
                return (RamAccess<string?>)value;
            }
            var rm = new RamAccess<string?>(GradeFioExecutor_Validation, GradeFioExecutor_DB);
            rm.PropertyChanged += GradeFioExecutor_ValueChanged;
            Dictionary.Add(nameof(GradeFioExecutor), rm);
            return (RamAccess<string?>)Dictionary[nameof(GradeFioExecutor)];
        }
        set
        {
            GradeFioExecutor_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void GradeFioExecutor_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string?>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (GradeFioExecutor_DB != value1)
        {
            GradeFioExecutor_DB = value1;
        }
    }

    private bool GradeFioExecutor_Validation(RamAccess<string?> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region Telephone (10)

    [MaxLength(64)]
    [Column(TypeName = "varchar(64)")]
    public string Telephone_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> Telephone
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Telephone), out var value))
            {
                ((RamAccess<string>)value).Value = Telephone_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Telephone_Validation, Telephone_DB);
            rm.PropertyChanged += Telephone_ValueChanged;
            Dictionary.Add(nameof(Telephone), rm);
            return (RamAccess<string>)Dictionary[nameof(Telephone)];
        }
        set
        {
            Telephone_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void Telephone_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 64
            ? value1[..64]
            : value1;
        if (Telephone_DB != value1)
        {
            Telephone_DB = value1;
        }
    }

    private bool Telephone_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region Fax (11)

    [MaxLength(64)]
    [Column(TypeName = "varchar(64)")]
    public string Fax_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> Fax
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Fax), out var value))
            {
                ((RamAccess<string>)value).Value = Fax_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Fax_Validation, Fax_DB);
            rm.PropertyChanged += Fax_ValueChanged;
            Dictionary.Add(nameof(Fax), rm);
            return (RamAccess<string>)Dictionary[nameof(Fax)];
        }
        set
        {
            Fax_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void Fax_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 64
            ? value1[..64]
            : value1;
        if (Fax_DB != value1)
        {
            Fax_DB = value1;
        }
    }

    private bool Fax_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region Email (12)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    [AllowNull]
    public string? Email_DB { get; set; }

    [NotMapped]
    public RamAccess<string?> Email
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Email), out var value))
            {
                ((RamAccess<string?>)value).Value = Email_DB;
                return (RamAccess<string?>)value;
            }
            var rm = new RamAccess<string?>(Email_Validation, Email_DB);
            rm.PropertyChanged += Email_ValueChanged;
            Dictionary.Add(nameof(Email), rm);
            return (RamAccess<string?>)Dictionary[nameof(Email)];
        }
        set
        {
            Email_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void Email_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string?>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (Email_DB != value1)
        {
            Email_DB = value1;
        }
    }

    private bool Email_Validation(RamAccess<string?> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #endregion

    #region Validation

    public override bool Object_Validation()
    {
        return !(Name.HasErrors ||
                 ShortName.HasErrors ||
                 Address.HasErrors ||
                 GradeFioDirector.HasErrors ||
                 GradeFioExecutor.HasErrors ||
                 Telephone.HasErrors ||
                 Fax.HasErrors ||
                 Email.HasErrors);
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
        // Заглушка
        var str = "Форма 5.0";
        return str;
    }

    #endregion
}