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

    #region CodeSubjectRF (2)

    [MaxLength(2)]
    [Column(TypeName = "varchar(2)")]
    public string CodeSubjectRF_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> CodeSubjectRF
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(CodeSubjectRF), out var value))
            {
                ((RamAccess<string>)value).Value = CodeSubjectRF_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(CodeSubjectRF_Validation, CodeSubjectRF_DB);
            rm.PropertyChanged += CodeSubjectRF_ValueChanged;
            Dictionary.Add(nameof(CodeSubjectRF), rm);
            return (RamAccess<string>)Dictionary[nameof(CodeSubjectRF)];
        }
        set
        {
            CodeSubjectRF_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void CodeSubjectRF_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 64
            ? value1[..64]
            : value1;

        if (CodeSubjectRF_DB != value1)
        {
            CodeSubjectRF_DB = value1;
        }
    }

    private bool CodeSubjectRF_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region SubjectRF (3)

    [MaxLength(64)]
    [Column(TypeName = "varchar(64)")]
    public string SubjectRF_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> SubjectRF
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(SubjectRF), out var value))
            {
                ((RamAccess<string>)value).Value = SubjectRF_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(SubjectRF_Validation, SubjectRF_DB);
            rm.PropertyChanged += SubjectRF_ValueChanged;
            Dictionary.Add(nameof(SubjectRF), rm);
            return (RamAccess<string>)Dictionary[nameof(SubjectRF)];
        }
        set
        {
            SubjectRF_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void SubjectRF_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 64
            ? value1[..64]
            : value1;

        if (SubjectRF_DB != value1)
        {
            SubjectRF_DB = value1;
        }
    }

    private bool SubjectRF_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    // Form41.Year (4)

    #region NameOrganUprav (5)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    public string NameOrganUprav_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> NameOrganUprav
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(NameOrganUprav), out var value))
            {
                ((RamAccess<string>)value).Value = NameOrganUprav_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(NameOrganUprav_Validation, NameOrganUprav_DB);
            rm.PropertyChanged += NameOrganUprav_ValueChanged;
            Dictionary.Add(nameof(NameOrganUprav), rm);
            return (RamAccess<string>)Dictionary[nameof(NameOrganUprav)];
        }
        set
        {
            NameOrganUprav_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void NameOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (NameOrganUprav_DB != value1)
        {
            NameOrganUprav_DB = value1;
        }
    }

    private bool NameOrganUprav_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region ShortNameOrganUprav (6)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    [AllowNull]
    public string? ShortNameOrganUprav_DB { get; set; }

    [NotMapped]
    public RamAccess<string?> ShortNameOrganUprav
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ShortNameOrganUprav), out var value))
            {
                ((RamAccess<string?>)value).Value = ShortNameOrganUprav_DB;
                return (RamAccess<string?>)value;
            }
            var rm = new RamAccess<string?>(ShortNameOrganUprav_Validation, ShortNameOrganUprav_DB);
            rm.PropertyChanged += ShortNameOrganUprav_ValueChanged;
            Dictionary.Add(nameof(ShortNameOrganUprav), rm);
            return (RamAccess<string?>)Dictionary[nameof(ShortNameOrganUprav)];
        }
        set
        {
            ShortNameOrganUprav_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void ShortNameOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string?>)value).Value;

        if (value1 is null) return;

        
        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (ShortNameOrganUprav_DB != value1)
        {
            ShortNameOrganUprav_DB = value1;
        }
    }

    private bool ShortNameOrganUprav_Validation(RamAccess<string?> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region AddressOrganUprav (7)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    public string AddressOrganUprav_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> AddressOrganUprav
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(AddressOrganUprav), out var value))
            {
                ((RamAccess<string>)value).Value = AddressOrganUprav_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(AddressOrganUprav_Validation, AddressOrganUprav_DB);
            rm.PropertyChanged += AddressOrganUprav_ValueChanged;
            Dictionary.Add(nameof(AddressOrganUprav), rm);
            return (RamAccess<string>)Dictionary[nameof(AddressOrganUprav)];
        }
        set
        {
            AddressOrganUprav_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void AddressOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (AddressOrganUprav_DB != value1)
        {
            AddressOrganUprav_DB = value1;
        }
    }
    private bool AddressOrganUprav_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region GradeFioDirectorOrganUprav (8)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    [AllowNull]
    public string? GradeFioDirectorOrganUprav_DB { get; set; }

    [NotMapped]
    public RamAccess<string?> GradeFioDirectorOrganUprav
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(GradeFioDirectorOrganUprav), out var value))
            {
                ((RamAccess<string?>)value).Value = GradeFioDirectorOrganUprav_DB;
                return (RamAccess<string?>)value;
            }
            var rm = new RamAccess<string?>(GradeFioDirectorOrganUprav_Validation, GradeFioDirectorOrganUprav_DB);
            rm.PropertyChanged += GradeFioDirectorOrganUprav_ValueChanged;
            Dictionary.Add(nameof(GradeFioDirectorOrganUprav), rm);
            return (RamAccess<string?>)Dictionary[nameof(GradeFioDirectorOrganUprav)];
        }
        set
        {
            GradeFioDirectorOrganUprav_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void GradeFioDirectorOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string?>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (GradeFioDirectorOrganUprav_DB != value1)
        {
            GradeFioDirectorOrganUprav_DB = value1;
        }
    }

    private bool GradeFioDirectorOrganUprav_Validation(RamAccess<string?> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region GradeFioExecutorOrganUprav (9)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    [AllowNull]
    public string? GradeFioExecutorOrganUprav_DB { get; set; }

    [NotMapped]
    public RamAccess<string?> GradeFioExecutorOrganUprav
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(GradeFioExecutorOrganUprav), out var value))
            {
                ((RamAccess<string?>)value).Value = GradeFioExecutorOrganUprav_DB;
                return (RamAccess<string?>)value;
            }
            var rm = new RamAccess<string?>(GradeFioExecutorOrganUprav_Validation, GradeFioExecutorOrganUprav_DB);
            rm.PropertyChanged += GradeFioExecutorOrganUprav_ValueChanged;
            Dictionary.Add(nameof(GradeFioExecutorOrganUprav), rm);
            return (RamAccess<string?>)Dictionary[nameof(GradeFioExecutorOrganUprav)];
        }
        set
        {
            GradeFioExecutorOrganUprav_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void GradeFioExecutorOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string?>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (GradeFioExecutorOrganUprav_DB != value1)
        {
            GradeFioExecutorOrganUprav_DB = value1;
        }
    }

    private bool GradeFioExecutorOrganUprav_Validation(RamAccess<string?> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region TelephoneOrganUprav (10)

    [MaxLength(64)]
    [Column(TypeName = "varchar(64)")]
    public string TelephoneOrganUprav_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> TelephoneOrganUprav
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(TelephoneOrganUprav), out var value))
            {
                ((RamAccess<string>)value).Value = TelephoneOrganUprav_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(TelephoneOrganUprav_Validation, TelephoneOrganUprav_DB);
            rm.PropertyChanged += TelephoneOrganUprav_ValueChanged;
            Dictionary.Add(nameof(TelephoneOrganUprav), rm);
            return (RamAccess<string>)Dictionary[nameof(TelephoneOrganUprav)];
        }
        set
        {
            TelephoneOrganUprav_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void TelephoneOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 64
            ? value1[..64]
            : value1;
        if (TelephoneOrganUprav_DB != value1)
        {
            TelephoneOrganUprav_DB = value1;
        }
    }

    private bool TelephoneOrganUprav_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region FaxOrganUprav (11)

    [MaxLength(64)]
    [Column(TypeName = "varchar(64)")]
    public string FaxOrganUprav_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> FaxOrganUprav
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(FaxOrganUprav), out var value))
            {
                ((RamAccess<string>)value).Value = FaxOrganUprav_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(FaxOrganUprav_Validation, FaxOrganUprav_DB);
            rm.PropertyChanged += FaxOrganUprav_ValueChanged;
            Dictionary.Add(nameof(FaxOrganUprav), rm);
            return (RamAccess<string>)Dictionary[nameof(FaxOrganUprav)];
        }
        set
        {
            FaxOrganUprav_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void FaxOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 64
            ? value1[..64]
            : value1;
        if (FaxOrganUprav_DB != value1)
        {
            FaxOrganUprav_DB = value1;
        }
    }

    private bool FaxOrganUprav_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region EmailOrganUprav (12)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    [AllowNull]
    public string? EmailOrganUprav_DB { get; set; }

    [NotMapped]
    public RamAccess<string?> EmailOrganUprav
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(EmailOrganUprav), out var value))
            {
                ((RamAccess<string?>)value).Value = EmailOrganUprav_DB;
                return (RamAccess<string?>)value;
            }
            var rm = new RamAccess<string?>(EmailOrganUprav_Validation, EmailOrganUprav_DB);
            rm.PropertyChanged += EmailOrganUprav_ValueChanged;
            Dictionary.Add(nameof(EmailOrganUprav), rm);
            return (RamAccess<string?>)Dictionary[nameof(EmailOrganUprav)];
        }
        set
        {
            EmailOrganUprav_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void EmailOrganUprav_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string?>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (EmailOrganUprav_DB != value1)
        {
            EmailOrganUprav_DB = value1;
        }
    }

    private bool EmailOrganUprav_Validation(RamAccess<string?> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region NameRiac (13)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    public string NameRiac_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> NameRiac
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(NameRiac), out var value))
            {
                ((RamAccess<string>)value).Value = NameRiac_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(NameRiac_Validation, NameRiac_DB);
            rm.PropertyChanged += NameRiac_ValueChanged;
            Dictionary.Add(nameof(NameRiac), rm);
            return (RamAccess<string>)Dictionary[nameof(NameRiac)];
        }
        set
        {
            NameRiac_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void NameRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (NameRiac_DB != value1)
        {
            NameRiac_DB = value1;
        }
    }

    private bool NameRiac_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region ShortNameRiac (14)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    [AllowNull]
    public string? ShortNameRiac_DB { get; set; }

    [NotMapped]
    public RamAccess<string?> ShortNameRiac
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ShortNameRiac), out var value))
            {
                ((RamAccess<string?>)value).Value = ShortNameRiac_DB;
                return (RamAccess<string?>)value;
            }
            var rm = new RamAccess<string?>(ShortNameRiac_Validation, ShortNameRiac_DB);
            rm.PropertyChanged += ShortNameRiac_ValueChanged;
            Dictionary.Add(nameof(ShortNameRiac), rm);
            return (RamAccess<string?>)Dictionary[nameof(ShortNameRiac)];
        }
        set
        {
            ShortNameRiac_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void ShortNameRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string?>)value).Value;

        if (value1 is null) return;

        if (value1 is null) return;
        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (ShortNameRiac_DB != value1)
        {
            ShortNameRiac_DB = value1;
        }
    }

    private bool ShortNameRiac_Validation(RamAccess<string?> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region AddressRiac (15)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    public string AddressRiac_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> AddressRiac
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(AddressRiac), out var value))
            {
                ((RamAccess<string>)value).Value = AddressRiac_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(AddressRiac_Validation, AddressRiac_DB);
            rm.PropertyChanged += AddressRiac_ValueChanged;
            Dictionary.Add(nameof(AddressRiac), rm);
            return (RamAccess<string>)Dictionary[nameof(AddressRiac)];
        }
        set
        {
            AddressRiac_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void AddressRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (AddressRiac_DB != value1)
        {
            AddressRiac_DB = value1;
        }
    }

    private bool AddressRiac_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region GradeFioDirectorRiac (16)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    [AllowNull]
    public string? GradeFioDirectorRiac_DB { get; set; }

    [NotMapped]
    public RamAccess<string?> GradeFioDirectorRiac
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(GradeFioDirectorRiac), out var value))
            {
                ((RamAccess<string?>)value).Value = GradeFioDirectorRiac_DB;
                return (RamAccess<string?>)value;
            }
            var rm = new RamAccess<string?>(GradeFioDirectorRiac_Validation, GradeFioDirectorRiac_DB);
            rm.PropertyChanged += GradeFioDirectorRiac_ValueChanged;
            Dictionary.Add(nameof(GradeFioDirectorRiac), rm);
            return (RamAccess<string?>)Dictionary[nameof(GradeFioDirectorRiac)];
        }
        set
        {
            SubjectRF_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void GradeFioDirectorRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string?>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (GradeFioDirectorRiac_DB != value1)
        {
            GradeFioDirectorRiac_DB = value1;
        }
    }

    private bool GradeFioDirectorRiac_Validation(RamAccess<string?> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region GradeFioExecutorRiac (17)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    [AllowNull]
    public string GradeFioExecutorRiac_DB { get; set; }

    [NotMapped]
    public RamAccess<string?> GradeFioExecutorRiac
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(GradeFioExecutorRiac), out var value))
            {
                ((RamAccess<string?>)value).Value = GradeFioExecutorRiac_DB;
                return (RamAccess<string?>)value;
            }
            var rm = new RamAccess<string?>(GradeFioExecutorRiac_Validation, GradeFioExecutorRiac_DB);
            rm.PropertyChanged += GradeFioExecutorRiac_ValueChanged;
            Dictionary.Add(nameof(GradeFioExecutorRiac), rm);
            return (RamAccess<string?>)Dictionary[nameof(GradeFioExecutorRiac)];
        }
        set
        {
            GradeFioExecutorRiac_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void GradeFioExecutorRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string?>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (GradeFioExecutorRiac_DB != value1)
        {
            GradeFioExecutorRiac_DB = value1;
        }
    }

    private bool GradeFioExecutorRiac_Validation(RamAccess<string?> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region TelephoneRiac (18)

    [MaxLength(64)]
    [Column(TypeName = "varchar(64)")]
    public string TelephoneRiac_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> TelephoneRiac
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(TelephoneRiac), out var value))
            {
                ((RamAccess<string>)value).Value = TelephoneRiac_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(TelephoneRiac_Validation, TelephoneRiac_DB);
            rm.PropertyChanged += TelephoneRiac_ValueChanged;
            Dictionary.Add(nameof(TelephoneRiac), rm);
            return (RamAccess<string>)Dictionary[nameof(TelephoneRiac)];
        }
        set
        {
            TelephoneRiac_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void TelephoneRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 64
            ? value1[..64]
            : value1;
        if (TelephoneRiac_DB != value1)
        {
            TelephoneRiac_DB = value1;
        }
    }

    private bool TelephoneRiac_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region FaxRiac (19)

    [MaxLength(64)]
    [Column(TypeName = "varchar(64)")]
    public string FaxRiac_DB { get; set; } = "";

    [NotMapped]
    public RamAccess<string> FaxRiac
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(FaxRiac), out var value))
            {
                ((RamAccess<string>)value).Value = FaxRiac_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(FaxRiac_Validation, FaxRiac_DB);
            rm.PropertyChanged += FaxRiac_ValueChanged;
            Dictionary.Add(nameof(FaxRiac), rm);
            return (RamAccess<string>)Dictionary[nameof(FaxRiac)];
        }
        set
        {
            FaxRiac_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void FaxRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;

        if (value1 is null) return;

        value1 = value1.Length > 64
            ? value1[..64]
            : value1;
        if (FaxRiac_DB != value1)
        {
            FaxRiac_DB = value1;
        }
    }

    private bool FaxRiac_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region EmailRiac (20)

    [MaxLength(256)]
    [Column(TypeName = "varchar(256)")]
    [AllowNull]
    public string? EmailRiac_DB { get; set; }

    [NotMapped]
    public RamAccess<string?> EmailRiac
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(EmailRiac), out var value))
            {
                ((RamAccess<string?>)value).Value = EmailRiac_DB;
                return (RamAccess<string?>)value;
            }
            var rm = new RamAccess<string?>(EmailRiac_Validation, EmailRiac_DB);
            rm.PropertyChanged += EmailRiac_ValueChanged;
            Dictionary.Add(nameof(EmailRiac), rm);
            return (RamAccess<string?>)Dictionary[nameof(EmailRiac)];
        }
        set
        {
            EmailRiac_DB = ParseInnerText(value.Value);
            OnPropertyChanged();
        }
    }

    private void EmailRiac_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 is null) return;
        value1 = value1.Length > 256
            ? value1[..256]
            : value1;
        if (EmailRiac_DB != value1)
        {
            EmailRiac_DB = value1;
        }
    }

    private bool EmailRiac_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #endregion

    #region Validation

    public override bool Object_Validation()
    {
        return !(CodeSubjectRF.HasErrors || 
                 SubjectRF.HasErrors ||
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
        var str = "Форма 4.0";
        return str;
    }

    #endregion
}