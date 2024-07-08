using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;

namespace Models.Forms.Form2;

[Serializable]
[Form_Class("Форма 2.3: Разрешение на размещение РАО в пунктах хранения, местах сбора и/или временного хранения")]
[Table (name: "form_23")]
public partial class Form23 : Form2
{
    #region Constructor

    public Form23()
    {
        FormNum.Value = "2.3";
        //NumberOfFields.Value = 17;
        Validate_all();
    }

    #endregion

    #region Validation
    
    private void Validate_all()
    {
        StoragePlaceName_Validation(StoragePlaceName);
        StoragePlaceCode_Validation(StoragePlaceCode);
        ProjectVolume_Validation(ProjectVolume);
        CodeRAO_Validation(CodeRAO);
        Volume_Validation(Volume);
        Mass_Validation(Mass);
        SummaryActivity_Validation(SummaryActivity);
        QuantityOZIII_Validation(QuantityOZIII);
        DocumentNumber_Validation(DocumentNumber);
        ExpirationDate_Validation(ExpirationDate);
        DocumentName_Validation(DocumentName);
        DocumentDate_Validation(DocumentDate);
    }

    [FormProperty(true, "Форма")]
    public override bool Object_Validation()
    {
        return !(StoragePlaceName.HasErrors ||
                 StoragePlaceCode.HasErrors ||
                 ProjectVolume.HasErrors ||
                 CodeRAO.HasErrors ||
                 Volume.HasErrors ||
                 Mass.HasErrors ||
                 SummaryActivity.HasErrors ||
                 QuantityOZIII.HasErrors ||
                 DocumentNumber.HasErrors ||
                 ExpirationDate.HasErrors ||
                 DocumentName.HasErrors ||
                 DocumentDate.HasErrors);
    }

    #endregion

    #region Properties
    
    #region  StoragePlaceName (2)

    public string StoragePlaceName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Пункт хранения РАО", "наименование", "2")]
    public RamAccess<string> StoragePlaceName
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(StoragePlaceName), out var value))
            {
                ((RamAccess<string>)value).Value = StoragePlaceName_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(StoragePlaceName_Validation, StoragePlaceName_DB);
            rm.PropertyChanged += StoragePlaceNameValueChanged;
            Dictionary.Add(nameof(StoragePlaceName), rm);
            return (RamAccess<string>)Dictionary[nameof(StoragePlaceName)];
        }
        set
        {
            StoragePlaceName_DB = value.Value;
            OnPropertyChanged(nameof(StoragePlaceName));
        }
    }
    //If change this change validation

    private void StoragePlaceNameValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            StoragePlaceName_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool StoragePlaceName_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        //List<string> spr = new List<string>();//here binds spr
        //if (!spr.Contains(value.Value))
        //{
        //    value.AddError("Недопустиое значение");
        //    return false;
        //}
        return true;
    }
    //StoragePlaceName property

    #endregion

    #region  StoragePlaceCode (3)

    public string StoragePlaceCode_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Пункт хранения РАО", "код", "3")]
    public RamAccess<string> StoragePlaceCode //8 digits code or - .
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(StoragePlaceCode), out var value))
            {
                ((RamAccess<string>)value).Value = StoragePlaceCode_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(StoragePlaceCode_Validation, StoragePlaceCode_DB);
            rm.PropertyChanged += StoragePlaceCodeValueChanged;
            Dictionary.Add(nameof(StoragePlaceCode), rm);
            return (RamAccess<string>)Dictionary[nameof(StoragePlaceCode)];
        }
        set
        {
            StoragePlaceCode_DB = value.Value;
            OnPropertyChanged();
        }
    }
    //if change this change validation

    private void StoragePlaceCodeValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            StoragePlaceCode_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("-"))
        {
            return true;
        }
        //List<string> spr = new List<string>();//here binds spr
        //if (!spr.Contains(value.Value))
        //{
        //    value.AddError("Недопустимое значение");
        //    return false;
        //}
        //return true;
        if (!StoragePlaceCodeRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); return false;
        }
        var tmp = value.Value;
        if (tmp.Length != 8) return true;
        if (!StoragePlaceCodeRegex1().IsMatch(tmp[..1]))
        {
            value.AddError($"Недопустимый вид пункта - {tmp[..1]}");
        }
        if (!StoragePlaceCodeRegex2().IsMatch(tmp.AsSpan(1, 1)))
        {
            value.AddError($"Недопустимое состояние пункта - {tmp.Substring(1, 1)}");
        }
        if (!StoragePlaceCodeRegex3().IsMatch(tmp.AsSpan(2, 1)))
        {
            value.AddError($"Недопустимая изоляция от окружающей среды - {tmp.Substring(2, 1)}");
        }
        if (!StoragePlaceCodeRegex4().IsMatch(tmp.AsSpan(3, 1)))
        {
            value.AddError($"Недопустимая зона нахождения пункта - {tmp.Substring(3, 1)}");
        }
        if (!StoragePlaceCodeRegex5().IsMatch(tmp.AsSpan(4, 1)))
        {
            value.AddError($"Недопустимое значение пункта - {tmp.Substring(4, 1)}");
        }
        if (!StoragePlaceCodeRegex6().IsMatch(tmp.AsSpan(5, 1)))
        {
            value.AddError($"Недопустимое размещение пункта хранения относительно поверхности земли - {tmp.Substring(5, 1)}");
        }
        if (!StoragePlaceCodeRegex7().IsMatch(tmp.AsSpan(6, 2)))
        {
            value.AddError($"Недопустимый код типа РАО - {tmp.Substring(6, 2)}");
        }
        return !value.HasErrors;
    }

    #endregion

    #region  ProjectVolume (4)

    public string ProjectVolume_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Пункт хранения РАО", "проектный объем, куб. м", "4")]
    public RamAccess<string> ProjectVolume
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ProjectVolume), out var value))
            {
                ((RamAccess<string>)value).Value = ProjectVolume_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(ProjectVolume_Validation, ProjectVolume_DB);
            rm.PropertyChanged += ProjectVolumeValueChanged;
            Dictionary.Add(nameof(ProjectVolume), rm);
            return (RamAccess<string>)Dictionary[nameof(ProjectVolume)];
        }
        set
        {
            ProjectVolume_DB = value.Value;
            OnPropertyChanged(nameof(ProjectVolume));
        }
    }

    private void ProjectVolumeValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1
                .Trim()
                .TrimStart('(')
                .TrimEnd(')')
                .ToLower()
                .Replace('.', ',')
                .Replace('е', 'e');
            if (value1.Equals("-"))
            {
                ProjectVolume_DB = value1;
                return;
            }
            if (double.TryParse(value1, 
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                    CultureInfo.CreateSpecificCulture("ru-RU"), 
                    out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        ProjectVolume_DB = value1;
    }

    private bool ProjectVolume_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            return false;
        }
        var value1 = value.Value
            .Trim()
            .TrimStart('(')
            .TrimEnd(')')
            .ToLower()
            .Replace('.', ',')
            .Replace('е', 'e');
        if (!double.TryParse(value1, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (doubleValue <= 0)
        {
            value.AddError("Число должно быть больше нуля"); 
            return false;
        }
        return true;
    }

    #endregion

    #region CodeRAO (5)

    public string CodeRAO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Разрешено к размещению", "код РАО", "5")]
    public RamAccess<string> CodeRAO
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(CodeRAO), out var value))
            {
                ((RamAccess<string>)value).Value = CodeRAO_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(CodeRAO_Validation, CodeRAO_DB);
            rm.PropertyChanged += CodeRAOValueChanged;
            Dictionary.Add(nameof(CodeRAO), rm);
            return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
        }
        set
        {
            CodeRAO_DB = value.Value;
            OnPropertyChanged(nameof(CodeRAO));
        }
    }

    private void CodeRAOValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value.ToLower();
        tmp = tmp.Replace("х", "x");
        CodeRAO_DB = tmp;
    }

    private bool CodeRAO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var tmp = value.Value.ToLower();
        tmp = tmp.Replace("х", "x");
        if (!CodeRaoRegex().IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (tmp.Length != 11) return true;
        if (!CodeRaoRegex1().IsMatch(tmp[..1]))
        {
            value.AddError($"Недопустимое агрегатное состояние - {tmp[..1]}");
        }
        if (!CodeRaoRegex2().IsMatch(tmp.AsSpan(1, 1)))
        {
            value.AddError($"Недопустимое категория РАО - {tmp.Substring(1, 1)}");
        }
        if (!CodeRaoRegex3().IsMatch(tmp.AsSpan(2, 1)))
        {
            value.AddError($"Недопустимый радионуклидный состав РАО - {tmp.Substring(2, 1)}");
        }
        if (!CodeRaoRegex4().IsMatch(tmp.AsSpan(3, 1)))
        {
            value.AddError($"Недопустимое содержание ядерных материалов - {tmp.Substring(3, 1)}");
        }
        if (!CodeRaoRegex4().IsMatch(tmp.AsSpan(4, 1)))
        {
            value.AddError($"Недопустимый период полураспада - {tmp.Substring(4, 1)}");
        }
        if (!CodeRaoRegex5().IsMatch(tmp.AsSpan(5, 1)))
        {
            value.AddError($"Недопустимый период потенциальной опасности РАО - {tmp.Substring(5, 1)}");
        }
        if (!CodeRaoRegex2().IsMatch(tmp.AsSpan(6, 1)))
        {
            value.AddError($"Недопустимый способ переработки - {tmp.Substring(6, 1)}");
        }
        if (!CodeRaoRegex6().IsMatch(tmp.AsSpan(7, 1)))
        {
            value.AddError($"Недопустимый класс РАО - {tmp.Substring(7, 1)}");
        }
        if (!CodeRaoRegex7().IsMatch(tmp.AsSpan(8, 2)))
        {
            value.AddError($"Недопустимый код типа РАО - {tmp.Substring(8, 2)}");
        }
        if (!CodeRaoRegex4().IsMatch(tmp.AsSpan(10, 1)))
        {
            value.AddError($"Недопустимая горючесть - {tmp.Substring(10, 1)}");
        }
        return !value.HasErrors;
    }

    #endregion

    #region  Volume (6)

    public string Volume_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Разрешено к размещению", "объем, куб. м", "6")]
    public RamAccess<string> Volume
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Volume), out var value))
            {
                ((RamAccess<string>)value).Value = Volume_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Volume_Validation, Volume_DB);
            rm.PropertyChanged += VolumeValueChanged;
            Dictionary.Add(nameof(Volume), rm);
            return (RamAccess<string>)Dictionary[nameof(Volume)];
        }
        set
        {
            Volume_DB = value.Value;
            OnPropertyChanged(nameof(Volume));
        }
    }

    private void VolumeValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1
                .Trim()
                .TrimStart('(')
                .TrimEnd(')')
                .ToLower()
                .Replace('.', ',')
                .Replace('е', 'e');
            if (value1.Equals("-"))
            {
                Volume_DB = value1;
                return;
            }
            if (double.TryParse(value1, 
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                    CultureInfo.CreateSpecificCulture("ru-RU"), 
                    out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        Volume_DB = value1;
    }

    private bool Volume_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            return false;
        }
        var value1 = value.Value
            .Trim()
            .TrimStart('(')
            .TrimEnd(')')
            .ToLower()
            .Replace('.', ',')
            .Replace('е', 'e');
        if (!double.TryParse(value1, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (doubleValue <= 0)
        {
            value.AddError("Число должно быть больше нуля"); 
            return false;
        }
        return true;
    }

    #endregion

    #region  Mass (7)

    public string Mass_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Разрешено к размещению", "масса, т", "7")]
    public RamAccess<string> Mass
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Mass), out var value))
            {
                ((RamAccess<string>)value).Value = Mass_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Mass_Validation, Mass_DB);
            rm.PropertyChanged += MassValueChanged;
            Dictionary.Add(nameof(Mass), rm);
            return (RamAccess<string>)Dictionary[nameof(Mass)];
        }
        set
        {
            Mass_DB = value.Value;
            OnPropertyChanged(nameof(Mass));
        }
    }

    private void MassValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1
                .Trim()
                .TrimStart('(')
                .TrimEnd(')')
                .ToLower()
                .Replace('.', ',')
                .Replace('е', 'e');
            if (value1.Equals("-"))
            {
                Mass_DB = value1;
                return;
            }
            if (double.TryParse(value1, 
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                    CultureInfo.CreateSpecificCulture("ru-RU"), 
                    out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        Mass_DB = value1;
    }

    private bool Mass_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            return false;
        }
        var value1 = value.Value
            .Trim()
            .TrimStart('(')
            .TrimEnd(')')
            .ToLower()
            .Replace('.', ',')
            .Replace('е', 'e');
        if (!double.TryParse(value1, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (doubleValue <= 0)
        {
            value.AddError("Число должно быть больше нуля"); 
            return false;
        }
        return true;
    }

    #endregion

    #region  QuantityOZIII (8)

    public string QuantityOZIII_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Разрешено к размещению", "количество ОЗИИИ, шт", "8")]
    public RamAccess<string> QuantityOZIII
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(QuantityOZIII), out var value))
            {
                ((RamAccess<string>)value).Value = QuantityOZIII_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(QuantityOZIII_Validation, QuantityOZIII_DB);
            rm.PropertyChanged += QuantityOZIIIValueChanged;
            Dictionary.Add(nameof(QuantityOZIII), rm);
            return (RamAccess<string>)Dictionary[nameof(QuantityOZIII)];
        }
        set
        {
            QuantityOZIII_DB = value.Value;
            OnPropertyChanged(nameof(QuantityOZIII));
        }
    }
    // positive int.

    private void QuantityOZIIIValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            QuantityOZIII_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool QuantityOZIII_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("-"))
        {
            return true;
        }
        if (!int.TryParse(value.Value, out var intValue))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (intValue <= 0)
        {
            value.AddError("Число должно быть больше нуля"); 
            return false;
        }
        return true;
    }

    #endregion

    #region  SummaryActivity (9)

    public string SummaryActivity_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Разрешено к размещению", "суммарная активность, Бк", "9")]
    public RamAccess<string> SummaryActivity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(SummaryActivity), out var value))
            {
                ((RamAccess<string>)value).Value = SummaryActivity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(SummaryActivity_Validation, SummaryActivity_DB);
            rm.PropertyChanged += SummaryActivityValueChanged;
            Dictionary.Add(nameof(SummaryActivity), rm);
            return (RamAccess<string>)Dictionary[nameof(SummaryActivity)];
        }
        set
        {
            SummaryActivity_DB = value.Value;
            OnPropertyChanged(nameof(SummaryActivity));
        }
    }

    private void SummaryActivityValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1
                .Trim()
                .TrimStart('(')
                .TrimEnd(')')
                .ToLower()
                .Replace('.', ',')
                .Replace('е', 'e');
            if (value1.Equals("-"))
            {
                SummaryActivity_DB = value1;
                return;
            }
            if (double.TryParse(value1, 
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                    CultureInfo.CreateSpecificCulture("ru-RU"), 
                    out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        SummaryActivity_DB = value1;
    }

    private bool SummaryActivity_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value.Equals("прим."))
        {
            return false;
        }
        var value1 = value.Value
            .Trim()
            .TrimStart('(')
            .TrimEnd(')')
            .ToLower()
            .Replace('.', ',')
            .Replace('е', 'e');
        if (!double.TryParse(value1, 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, 
                CultureInfo.CreateSpecificCulture("ru-RU"), 
                out var doubleValue))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (doubleValue <= 0)
        {
            value.AddError("Число должно быть больше нуля"); 
            return false;
        }
        return true;
    }

    #endregion

    #region  DocumentNumber (10)

    public string DocumentNumber_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Наименование и реквизиты документа на размещение РАО", "номер", "10")]
    public RamAccess<string> DocumentNumber
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(DocumentNumber), out var value))
            {
                ((RamAccess<string>)value).Value = DocumentNumber_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(DocumentNumber_Validation, DocumentNumber_DB);
            rm.PropertyChanged += DocumentNumberValueChanged;
            Dictionary.Add(nameof(DocumentNumber), rm);
            return (RamAccess<string>)Dictionary[nameof(DocumentNumber)];
        }
        set
        {
            DocumentNumber_DB = value.Value;
            OnPropertyChanged(nameof(DocumentNumber));
        }
    }

    private void DocumentNumberValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            DocumentNumber_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool DocumentNumber_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))//ok
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        return true;
    }

    #endregion

    #region DocumentDate (11)

    public string DocumentDate_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Наименование и реквизиты документа на размещение РАО", "дата", "11")]
    public RamAccess<string> DocumentDate
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(DocumentDate), out var value))
            {
                ((RamAccess<string>)value).Value = DocumentDate_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(DocumentDate_Validation, DocumentDate_DB);
            rm.PropertyChanged += DocumentDateValueChanged;
            Dictionary.Add(nameof(DocumentDate), rm);
            return (RamAccess<string>)Dictionary[nameof(DocumentDate)];
        }
        set
        {
            DocumentDate_DB = value.Value;
            OnPropertyChanged(nameof(DocumentDate));
        }
    }
    //if change this change validation

    private void DocumentDateValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var tmp = ((RamAccess<string>)value).Value;
            if (Date6NumRegex().IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            DocumentDate_DB = tmp;
        }
    }

    private bool DocumentDate_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var tmp = value.Value;
        if (Date6NumRegex().IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!Date8NumRegex().IsMatch(tmp) || !DateTimeOffset.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  ExpirationDate (12)

    public string ExpirationDate_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Наименование и реквизиты документа на размещение РАО", "срок действия", "12")]
    public RamAccess<string> ExpirationDate
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ExpirationDate), out var value))
            {
                ((RamAccess<string>)value).Value = ExpirationDate_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(ExpirationDate_Validation, ExpirationDate_DB);
            rm.PropertyChanged += ExpirationDateValueChanged;
            Dictionary.Add(nameof(ExpirationDate), rm);
            return (RamAccess<string>)Dictionary[nameof(ExpirationDate)];
        }
        set
        {
            ExpirationDate_DB = value.Value;
            OnPropertyChanged(nameof(ExpirationDate));
        }
    }

    private void ExpirationDateValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value;
        if (Date6NumRegex().IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        ExpirationDate_DB = tmp;
    }

    private bool ExpirationDate_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var tmp = value.Value;
        if (Date6NumRegex().IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!Date8NumRegex().IsMatch(tmp) || !DateTimeOffset.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  DocumentName (13)

    public string DocumentName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Наименование и реквизиты документа на размещение РАО", "наименование документа", "13")]
    public RamAccess<string> DocumentName
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(DocumentName), out var value))
            {
                ((RamAccess<string>)value).Value = DocumentName_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(DocumentName_Validation, DocumentName_DB);
            rm.PropertyChanged += DocumentNameValueChanged;
            Dictionary.Add(nameof(DocumentName), rm);
            return (RamAccess<string>)Dictionary[nameof(DocumentName)];
        }
        set
        {
            DocumentName_DB = value.Value;
            OnPropertyChanged(nameof(DocumentName));
        }
    }

    private void DocumentNameValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            DocumentName_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool DocumentName_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        return true;
    }

    #endregion 
    
    #endregion

    #region IExcel

    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        base.ExcelGetRow(worksheet, row);
        StoragePlaceName_DB = Convert.ToString(worksheet.Cells[row, 2].Value);
        StoragePlaceCode_DB = Convert.ToString(worksheet.Cells[row, 3].Value);
        ProjectVolume_DB = ConvertFromExcelDouble(worksheet.Cells[row, 4].Value);
        CodeRAO_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        Volume_DB = ConvertFromExcelDouble(worksheet.Cells[row, 6].Value);
        Mass_DB = ConvertFromExcelDouble(worksheet.Cells[row, 7].Value);
        QuantityOZIII_DB = ConvertFromExcelInt(worksheet.Cells[row, 8].Value);
        SummaryActivity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 9].Value);
        DocumentNumber_DB = Convert.ToString(worksheet.Cells[row, 10].Value);
        DocumentDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 11].Text);
        ExpirationDate_DB = ConvertFromExcelDate(worksheet.Cells[row, 12].Text);
        DocumentName_DB = Convert.ToString(worksheet.Cells[row, 13].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ConvertToExcelString(StoragePlaceName_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(StoragePlaceCode_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelDouble(ProjectVolume_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelString(CodeRAO_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelDouble(Volume_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelDouble(Mass_DB);
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelInt(QuantityOZIII_DB);
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelDouble(SummaryActivity_DB);
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ConvertToExcelString(DocumentNumber_DB);
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ConvertToExcelDate(DocumentDate_DB, worksheet, row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0));
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ConvertToExcelDate(ExpirationDate_DB, worksheet, row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0));
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ConvertToExcelString(DocumentName_DB);
        
        return 12;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form2.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form23,Models")?.GetProperty(nameof(StoragePlaceName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form23,Models")?.GetProperty(nameof(StoragePlaceCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form23,Models")?.GetProperty(nameof(ProjectVolume))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form23,Models")?.GetProperty(nameof(CodeRAO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form23,Models")?.GetProperty(nameof(Volume))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form23,Models")?.GetProperty(nameof(Mass))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form23,Models")?.GetProperty(nameof(QuantityOZIII))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form23,Models")?.GetProperty(nameof(SummaryActivity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form23,Models")?.GetProperty(nameof(DocumentNumber))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form23,Models")?.GetProperty(nameof(DocumentDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form23,Models")?.GetProperty(nameof(ExpirationDate))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 11 : 0), column + (transpose ? 11 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form23,Models")?.GetProperty(nameof(DocumentName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        
        return 12;
    }
    
    #endregion

    #region IDataGridColumn

    private static DataGridColumns _DataGridColumns { get; set; }

    public override DataGridColumns GetColumnStructure(string param = "")
    {
        if (_DataGridColumns != null) return _DataGridColumns;

        #region NumberInOrder (1)

        var numberInOrderR = ((FormPropertyAttribute)typeof(Form)
                .GetProperty(nameof(NumberInOrder))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD();
        if (numberInOrderR != null)
        {
            numberInOrderR.SetSizeColToAllLevels(50);
            numberInOrderR.Binding = nameof(NumberInOrder);
            numberInOrderR.Blocked = true;
            numberInOrderR.ChooseLine = true;
        }
        
        #endregion

        #region StoragePlaceName (2)

        var storagePlaceNameR = ((FormPropertyAttribute)typeof(Form23)
                .GetProperty(nameof(StoragePlaceName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (storagePlaceNameR != null)
        {
            storagePlaceNameR.SetSizeColToAllLevels(163);
            storagePlaceNameR.Binding = nameof(StoragePlaceName);
            numberInOrderR += storagePlaceNameR;
        }

        #endregion

        #region StoragePlaceCode (3)

        var storagePlaceCodeR = ((FormPropertyAttribute)typeof(Form23)
                .GetProperty(nameof(StoragePlaceCode))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (storagePlaceCodeR != null)
        {
            storagePlaceCodeR.SetSizeColToAllLevels(88);
            storagePlaceCodeR.Binding = nameof(StoragePlaceCode);
            numberInOrderR += storagePlaceCodeR;
        }

        #endregion

        #region ProjectVolume (4)

        var projectVolumeR = ((FormPropertyAttribute)typeof(Form23)
                .GetProperty(nameof(ProjectVolume))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (projectVolumeR != null)
        {
            projectVolumeR.SetSizeColToAllLevels(133);
            projectVolumeR.Binding = nameof(ProjectVolume);
            numberInOrderR += projectVolumeR;
        }

        #endregion

        #region CodeRAO (5)

        var codeRaoR = ((FormPropertyAttribute)typeof(Form23)
                .GetProperty(nameof(CodeRAO))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (codeRaoR != null)
        {
            codeRaoR.SetSizeColToAllLevels(88);
            codeRaoR.Binding = nameof(CodeRAO);
            numberInOrderR += codeRaoR;
        }

        #endregion

        #region Volume (6)

        var volumeR = ((FormPropertyAttribute)typeof(Form23)
                .GetProperty(nameof(Volume))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (volumeR != null)
        {
            volumeR.SetSizeColToAllLevels(163);
            volumeR.Binding = nameof(Volume);
            numberInOrderR += volumeR;
        }

        #endregion

        #region Mass (7)

        var massR = ((FormPropertyAttribute)typeof(Form23)
                .GetProperty(nameof(Mass))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (massR != null)
        {
            massR.SetSizeColToAllLevels(123);
            massR.Binding = nameof(Mass);
            numberInOrderR += massR;
        }

        #endregion

        #region QuantityOZIII (8)

        var quantityOziiiR = ((FormPropertyAttribute)typeof(Form23)
                .GetProperty(nameof(QuantityOZIII))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        quantityOziiiR.SetSizeColToAllLevels(123);
        quantityOziiiR.Binding = nameof(QuantityOZIII);
        numberInOrderR += quantityOziiiR;
        
        #endregion

        #region SummaryActivity (9)

        var summaryActivityR = ((FormPropertyAttribute)typeof(Form23)
                .GetProperty(nameof(SummaryActivity))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        summaryActivityR.SetSizeColToAllLevels(163);
        summaryActivityR.Binding = nameof(SummaryActivity);
        numberInOrderR += summaryActivityR;
        
        #endregion

        #region DocumentNumber (10)

        var documentNumberR = ((FormPropertyAttribute)typeof(Form23)
                .GetProperty(nameof(DocumentNumber))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentNumberR.SetSizeColToAllLevels(103);
        documentNumberR.Binding = nameof(DocumentNumber);
        numberInOrderR += documentNumberR;
        
        #endregion

        #region DocumentDate (11)

        var documentDateR = ((FormPropertyAttribute)typeof(Form23)
                .GetProperty(nameof(DocumentDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentDateR.SetSizeColToAllLevels(88);
        documentDateR.Binding = nameof(DocumentDate);
        numberInOrderR += documentDateR;
        
        #endregion

        #region ExpirationDate (12)
        var expirationDateR = ((FormPropertyAttribute)typeof(Form23)
                .GetProperty(nameof(ExpirationDate))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        expirationDateR.SetSizeColToAllLevels(163);
        expirationDateR.Binding = nameof(ExpirationDate);
        numberInOrderR += expirationDateR;
        #endregion

        #region DocumentName (13)

        var documentNameR = ((FormPropertyAttribute)typeof(Form23)
                .GetProperty(nameof(DocumentName))
                .GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD(numberInOrderR);
        documentNameR.SetSizeColToAllLevels(163);
        documentNameR.Binding = nameof(DocumentName);
        numberInOrderR += documentNameR;
        
        #endregion

        _DataGridColumns = numberInOrderR;

        return _DataGridColumns;
    }

    #endregion

    #region GeneratedRegex
    
    #region StoragePlaceCode

    [GeneratedRegex("^[0-9]{8}$")]
    private static partial Regex StoragePlaceCodeRegex();

    [GeneratedRegex("^[1-9]")]
    private static partial Regex StoragePlaceCodeRegex1();

    [GeneratedRegex("^[1-3]")]
    private static partial Regex StoragePlaceCodeRegex2();

    [GeneratedRegex("^[1-2]")]
    private static partial Regex StoragePlaceCodeRegex3();

    [GeneratedRegex("^[1-59]")]
    private static partial Regex StoragePlaceCodeRegex4();

    [GeneratedRegex("^[0-4]")]
    private static partial Regex StoragePlaceCodeRegex5();

    [GeneratedRegex("^[1-49]")]
    private static partial Regex StoragePlaceCodeRegex6();

    [GeneratedRegex("^[1]{1}[1-9]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1]{1}|^[4]{1}[1-49]{1}|^[5]{1}[1-69]{1}|^[6]{1}[1]{1}|^[7]{1}[1349]{1}|^[8]{1}[1-69]{1}|^[9]{1}[9]{1}")]
    private static partial Regex StoragePlaceCodeRegex7();

    #endregion

    #region CodeRao

    [GeneratedRegex("^[0-9x+]{11}$")]
    private static partial Regex CodeRaoRegex();

    [GeneratedRegex("^[1-3x+]")]
    private static partial Regex CodeRaoRegex1();

    [GeneratedRegex("^[0-49x+]")]
    private static partial Regex CodeRaoRegex2();

    [GeneratedRegex("^[0-6x+]")]
    private static partial Regex CodeRaoRegex3();

    [GeneratedRegex("^[12x+]")]
    private static partial Regex CodeRaoRegex4();

    [GeneratedRegex("^[0-3x+]")]
    private static partial Regex CodeRaoRegex5();

    [GeneratedRegex("^[0-79x+]")]
    private static partial Regex CodeRaoRegex6();

    [GeneratedRegex("^[1]{1}[1-9]{1}|^[0]{1}[1]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1-9]{1}|^[4]{1}[1-6]{1}|^[5]{1}[1-9]{1}|^[6]{1}[1-9]{1}|^[7]{1}[1-9]{1}|^[8]{1}[1-9]{1}|^[9]{1}[1-9]{1}")]
    private static partial Regex CodeRaoRegex7();

    #endregion 

    #endregion
}