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
using Spravochniki;

namespace Models.Forms.Form2;

[Serializable]
[Form_Class("Форма 2.12: Суммарные сведения о РВ не в составе ЗРИ")]
public class Form212 : Form2
{
    #region Constrctor
    
    public Form212()
    {
        FormNum.Value = "2.12";
        //NumberOfFields.Value = 8;
        Validate_all();
    }

    #endregion

    #region Validation
    
    private void Validate_all()
    {
        Radionuclids_Validation(Radionuclids);
        OperationCode_Validation(OperationCode);
        ObjectTypeCode_Validation(ObjectTypeCode);
        Activity_Validation(Activity);
        ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
    }

    [FormProperty(true, "Форма")]
    public override bool Object_Validation()
    {
        return !(Radionuclids.HasErrors ||
                 OperationCode.HasErrors ||
                 ObjectTypeCode.HasErrors ||
                 Activity.HasErrors ||
                 ProviderOrRecieverOKPO.HasErrors);
    }

    #endregion

    #region Properties
    
    #region  OperationCode (2)

    public short? OperationCode_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "null-2", "Код операции", "2")]
    public RamAccess<short?> OperationCode
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(OperationCode), out var value))
            {
                ((RamAccess<short?>)value).Value = OperationCode_DB;
                return (RamAccess<short?>)value;
            }
            var rm = new RamAccess<short?>(OperationCode_Validation, OperationCode_DB);
            rm.PropertyChanged += OperationCodeValueChanged;
            Dictionary.Add(nameof(OperationCode), rm);
            return (RamAccess<short?>)Dictionary[nameof(OperationCode)];
        }
        set
        {
            OperationCode_DB = value.Value;
            OnPropertyChanged(nameof(OperationCode));
        }
    }

    private void OperationCodeValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            OperationCode_DB = ((RamAccess<short?>)value).Value;
        }
    }

    private bool OperationCode_Validation(RamAccess<short?> value)
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (!Spravochniks.SprOpCodes1.Contains((short)value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region ObjectTypeCode (3)

    public short? ObjectTypeCode_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "null-3", "Код типа объектов учета", "3")]
    public RamAccess<short?> ObjectTypeCode
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ObjectTypeCode), out var value))
            {
                ((RamAccess<short?>)value).Value = ObjectTypeCode_DB;
                return (RamAccess<short?>)value;
            }
            var rm = new RamAccess<short?>(ObjectTypeCode_Validation, ObjectTypeCode_DB);
            rm.PropertyChanged += ObjectTypeCodeValueChanged;
            Dictionary.Add(nameof(ObjectTypeCode), rm);
            return (RamAccess<short?>)Dictionary[nameof(ObjectTypeCode)];
        }
        set
        {
            ObjectTypeCode_DB = value.Value;
            OnPropertyChanged(nameof(ObjectTypeCode));
        }
    }
    //2 digit code

    private void ObjectTypeCodeValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            ObjectTypeCode_DB = ((RamAccess<short?>)value).Value;
        }
    }

    private bool ObjectTypeCode_Validation(RamAccess<short?> value)//TODO
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (!Spravochniks.SprAccObjCodes.Contains((short)value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  Radionuclids (4)

    public string Radionuclids_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сведения о радионуклидных источниках", "радионуклиды", "4")]
    public RamAccess<string> Radionuclids
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Radionuclids), out var value))
            {
                ((RamAccess<string>)value).Value = Radionuclids_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Radionuclids_Validation, Radionuclids_DB);
            rm.PropertyChanged += RadionuclidsValueChanged;
            Dictionary.Add(nameof(Radionuclids), rm);
            return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
        }
        set
        {
            Radionuclids_DB = value.Value;
            OnPropertyChanged(nameof(Radionuclids));
        }
    }
    //If change this change validation

    private void RadionuclidsValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Radionuclids_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool Radionuclids_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var nuclids = value.Value.Split(";");
        for (var k = 0; k < nuclids.Length; k++)
        {
            nuclids[k] = nuclids[k].ToLower().Replace(" ", "");
        }
        var flag = true;
        foreach (var nucl in nuclids)
        {
            if (!Spravochniks.SprRadionuclids
                    .Where(item => nucl == item.Item1)
                    .Select(item => item.Item1)
                    .Any())
                flag = false;
        }
        if (!flag)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  Activity (5)

    public string Activity_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Сведения о радионуклидных источниках", "активность, Бк", "5")]
    public RamAccess<string> Activity
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(Activity), out var value))
            {
                ((RamAccess<string>)value).Value = Activity_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(Activity_Validation, Activity_DB);
            rm.PropertyChanged += ActivityValueChanged;
            Dictionary.Add(nameof(Activity), rm);
            return (RamAccess<string>)Dictionary[nameof(Activity)];
        }
        set
        {
            Activity_DB = value.Value;
            OnPropertyChanged(nameof(Activity));
        }
    }

    private void ActivityValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
        {
            value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                Activity_DB = value1;
                return;
            }
            if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (double.TryParse(value1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
            {
                value1 = $"{doubleValue:0.######################################################e+00}";
            }
        }
        Activity_DB = value1;
    }

    private bool Activity_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        const NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent;
        if (!double.TryParse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB"), out var doubleValue))
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

    #region  ProviderOrRecieverOKPO (6)

    public string ProviderOrRecieverOKPO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-4", "ОКПО поставщика/получателя", "6")]
    public RamAccess<string> ProviderOrRecieverOKPO
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ProviderOrRecieverOKPO), out var value))
            {
                ((RamAccess<string>)value).Value = ProviderOrRecieverOKPO_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(ProviderOrRecieverOKPO_Validation, ProviderOrRecieverOKPO_DB);
            rm.PropertyChanged += ProviderOrRecieverOKPOValueChanged;
            Dictionary.Add(nameof(ProviderOrRecieverOKPO), rm);
            return (RamAccess<string>)Dictionary[nameof(ProviderOrRecieverOKPO)];
        }
        set
        {
            ProviderOrRecieverOKPO_DB = value.Value;
            OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
        }
    }

    private void ProviderOrRecieverOKPOValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var value1 = ((RamAccess<string>)value).Value;
        if (value1 != null)
            if (Spravochniks.OKSM.Contains(value1.ToUpper()))
            {
                value1 = value1.ToUpper();
            }
        ProviderOrRecieverOKPO_DB = value1;
    }

    private bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()) || value.Value.Equals("Минобороны"))
        {
            return true;
        }
        if (value.Value.Length != 8 && value.Value.Length != 14 || !OkpoRegex().IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
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
        OperationCode_DB = short.TryParse(Convert.ToString(worksheet.Cells[row, 2].Value), out var shortValue) ? shortValue : null;
        ObjectTypeCode_DB = short.TryParse(Convert.ToString(worksheet.Cells[row, 3].Value), out shortValue) ? shortValue : null;
        Radionuclids_DB = Convert.ToString(worksheet.Cells[row, 4].Value);
        Activity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 5].Value);
        ProviderOrRecieverOKPO_DB = Convert.ToString(worksheet.Cells[row, 6].Value);

    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = OperationCode_DB is null ? "-" : OperationCode_DB;
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ObjectTypeCode_DB is null ? "-" : ObjectTypeCode_DB;
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelString(Radionuclids_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelDouble(Activity_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelString(ProviderOrRecieverOKPO_DB);
        
        return 5;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form2.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form212,Models")?.GetProperty(nameof(OperationCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form212,Models")?.GetProperty(nameof(ObjectTypeCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form212,Models")?.GetProperty(nameof(Radionuclids))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form212,Models")?.GetProperty(nameof(Activity))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form212,Models")?.GetProperty(nameof(ProviderOrRecieverOKPO))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        
        return 5;
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

        #region OperationCode (2)

        var operationCodeR = ((FormPropertyAttribute)typeof(Form212)
                .GetProperty(nameof(OperationCode))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (operationCodeR != null)
        {
            operationCodeR.SetSizeColToAllLevels(100);
            operationCodeR.Binding = nameof(OperationCode);
            numberInOrderR += operationCodeR;
        }

        #endregion

        #region ObjectTypeCode (3)

        var objectTypeCodeR = ((FormPropertyAttribute)typeof(Form212)
                .GetProperty(nameof(ObjectTypeCode))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (objectTypeCodeR != null)
        {
            objectTypeCodeR.SetSizeColToAllLevels(193);
            objectTypeCodeR.Binding = nameof(ObjectTypeCode);
            numberInOrderR += objectTypeCodeR;
        }

        #endregion

        #region Radionuclids (4)

        var radionuclidsR = ((FormPropertyAttribute)typeof(Form212)
                .GetProperty(nameof(Radionuclids))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (radionuclidsR != null)
        {
            radionuclidsR.SetSizeColToAllLevels(145);
            radionuclidsR.Binding = nameof(Radionuclids);
            numberInOrderR += radionuclidsR;
        }

        #endregion

        #region Activity (5)

        var activityR = ((FormPropertyAttribute)typeof(Form212)
                .GetProperty(nameof(Activity))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (activityR != null)
        {
            activityR.SetSizeColToAllLevels(108);
            activityR.Binding = nameof(Activity);
            numberInOrderR += activityR;
        }

        #endregion

        #region ProviderOrRecieverOKPO (6)

        var providerOrRecieverOkpoR = ((FormPropertyAttribute)typeof(Form212)
                .GetProperty(nameof(ProviderOrRecieverOKPO))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (providerOrRecieverOkpoR != null)
        {
            providerOrRecieverOkpoR.SetSizeColToAllLevels(193);
            providerOrRecieverOkpoR.Binding = nameof(ProviderOrRecieverOKPO);
            numberInOrderR += providerOrRecieverOkpoR;
        }

        #endregion

        _DataGridColumns = numberInOrderR;

        return _DataGridColumns;
    }

    #endregion
}