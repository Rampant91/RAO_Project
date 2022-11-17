using Models.DataAccess; 
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Text.RegularExpressions;
using System.Globalization;
using Spravochniki;
using System.ComponentModel;
using System.Linq;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using Models.Collections;

namespace Models;

[Serializable]
[Form_Class("Форма 2.12: Суммарные сведения о РВ не в составе ЗРИ")]
public class Form212 : Form2
{
    public Form212() : base()
    {
        FormNum.Value = "2.12";
        //NumberOfFields.Value = 8;
        Validate_all();
    }
    private void Validate_all()
    {
        Radionuclids_Validation(Radionuclids);
        OperationCode_Validation(OperationCode);
        ObjectTypeCode_Validation(ObjectTypeCode);
        Activity_Validation(Activity);
        ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
    }

    [FormProperty(true,"Форма")]
    public override bool Object_Validation()
    {
        return !(Radionuclids.HasErrors||
                 OperationCode.HasErrors||
                 ObjectTypeCode.HasErrors||
                 Activity.HasErrors||
                 ProviderOrRecieverOKPO.HasErrors);
    }

    //OperationCode property
    #region  OperationCode
    public short? OperationCode_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"null-2","Код операции","2")]
    public RamAccess<short?> OperationCode
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(OperationCode)))
            {
                ((RamAccess<short?>)Dictionary[nameof(OperationCode)]).Value = OperationCode_DB;
                return (RamAccess<short?>)Dictionary[nameof(OperationCode)];
            }
            else
            {
                var rm = new RamAccess<short?>(OperationCode_Validation, OperationCode_DB);
                rm.PropertyChanged += OperationCodeValueChanged;
                Dictionary.Add(nameof(OperationCode), rm);
                return (RamAccess<short?>)Dictionary[nameof(OperationCode)];
            }
        }
        set
        {
            OperationCode_DB = value.Value;
            OnPropertyChanged(nameof(OperationCode));
        }
    }

    private void OperationCodeValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            OperationCode_DB = ((RamAccess<short?>)Value).Value;
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
    //OperationCode property
    #endregion

    //ObjectTypeCode property
    #region 
    public short? ObjectTypeCode_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"null-3","Код типа объектов учета","3")]
    public RamAccess<short?> ObjectTypeCode
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(ObjectTypeCode)))
            {
                ((RamAccess<short?>)Dictionary[nameof(ObjectTypeCode)]).Value = ObjectTypeCode_DB;
                return (RamAccess<short?>)Dictionary[nameof(ObjectTypeCode)];
            }
            else
            {
                var rm = new RamAccess<short?>(ObjectTypeCode_Validation, ObjectTypeCode_DB);
                rm.PropertyChanged += ObjectTypeCodeValueChanged;
                Dictionary.Add(nameof(ObjectTypeCode), rm);
                return (RamAccess<short?>)Dictionary[nameof(ObjectTypeCode)];
            }
        }
        set
        {
            ObjectTypeCode_DB = value.Value;
            OnPropertyChanged(nameof(ObjectTypeCode));
        }
    }
    //2 digit code

    private void ObjectTypeCodeValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            ObjectTypeCode_DB = ((RamAccess<short?>)Value).Value;
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
    //ObjectTypeCode property
    #endregion

    //Radionuclids property
    #region  Radionuclids
    public string Radionuclids_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"Сведения о радионуклидных источниках", "радионуклиды","4")]
    public RamAccess<string> Radionuclids
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Radionuclids)))
            {
                ((RamAccess<string>)Dictionary[nameof(Radionuclids)]).Value = Radionuclids_DB;
                return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
            }
            else
            {
                var rm = new RamAccess<string>(Radionuclids_Validation, Radionuclids_DB);
                rm.PropertyChanged += RadionuclidsValueChanged;
                Dictionary.Add(nameof(Radionuclids), rm);
                return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
            }
        }
        set
        {
            Radionuclids_DB = value.Value;
            OnPropertyChanged(nameof(Radionuclids));
        }
    }
    //If change this change validation

    private void RadionuclidsValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Radionuclids_DB = ((RamAccess<string>)Value).Value;
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
            var tmp = from item in Spravochniks.SprRadionuclids where nucl == item.Item1 select item.Item1;
            if (!tmp.Any())
                flag = false;
        }
        if (!flag)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    //Radionuclids property
    #endregion

    //Activity property
    #region  Activity
    public string Activity_DB { get; set; }
    [NotMapped]
    [FormProperty(true,"Сведения о радионуклидных источниках", "активность, Бк","5")]
    public RamAccess<string> Activity
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Activity)))
            {
                ((RamAccess<string>)Dictionary[nameof(Activity)]).Value = Activity_DB;
                return (RamAccess<string>)Dictionary[nameof(Activity)];
            }
            else
            {
                var rm = new RamAccess<string>(Activity_Validation, Activity_DB);
                rm.PropertyChanged += ActivityValueChanged;
                Dictionary.Add(nameof(Activity), rm);
                return (RamAccess<string>)Dictionary[nameof(Activity)];
            }
        }
        set
        {
            Activity_DB = value.Value;
            OnPropertyChanged(nameof(Activity));
        }
    }


    private void ActivityValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
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
                try
                {
                    var value2 = Convert.ToDouble(value1);
                    value1 = $"{value2:0.######################################################e+00}";
                }
                catch (Exception ex)
                { }
            }
            Activity_DB = value1;
        }
    }
    private bool Activity_Validation(RamAccess<string> value)//Ready
    {
        value.ClearErrors();
        if (value.Value==null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
        if (!value1.Contains('e') && value1.Contains('+') ^ value1.Contains('-'))
        {
            value1 = value1.Replace("+", "e+").Replace("-", "e-");
        }
        var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
        try
        {
            if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
        }
        catch
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }
    //Activity property
    #endregion

    //ProviderOrRecieverOKPO property
    #region  ProviderOrRecieverOKPO
    public string ProviderOrRecieverOKPO_DB { get; set; } = "";
    [NotMapped]
    [FormProperty(true,"null-4","ОКПО поставщика/получателя","6")]
    public RamAccess<string> ProviderOrRecieverOKPO
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(ProviderOrRecieverOKPO)))
            {
                ((RamAccess<string>)Dictionary[nameof(ProviderOrRecieverOKPO)]).Value = ProviderOrRecieverOKPO_DB;
                return (RamAccess<string>)Dictionary[nameof(ProviderOrRecieverOKPO)];
            }
            else
            {
                var rm = new RamAccess<string>(ProviderOrRecieverOKPO_Validation, ProviderOrRecieverOKPO_DB);
                rm.PropertyChanged += ProviderOrRecieverOKPOValueChanged;
                Dictionary.Add(nameof(ProviderOrRecieverOKPO), rm);
                return (RamAccess<string>)Dictionary[nameof(ProviderOrRecieverOKPO)];
            }
        }
        set
        {
            ProviderOrRecieverOKPO_DB = value.Value;
            OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
        }
    }


    private void ProviderOrRecieverOKPOValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var value1 = ((RamAccess<string>)Value).Value;
            if (value1 != null)
                if (Spravochniks.OKSM.Contains(value1.ToUpper()))
                {
                    value1 = value1.ToUpper();
                }
            ProviderOrRecieverOKPO_DB = value1;
        }
    }
    private bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (Spravochniks.OKSM.Contains(value.Value.ToUpper()))
        {
            return true;
        }
        if (value.Value.Equals("Минобороны"))
        {
            return true;
        }
        if (value.Value.Length != 8 && value.Value.Length != 14)
        {
            value.AddError("Недопустимое значение"); return false;
        }
        Regex mask = new("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
        if (!mask.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение"); return false;
        }
        return true;
    }
    //ProviderOrRecieverOKPO property
    #endregion

    #region IExcel
    public void ExcelGetRow(ExcelWorksheet worksheet, int Row)
    {
        base.ExcelGetRow(worksheet, Row);
        OperationCode_DB = Convert.ToInt16(worksheet.Cells[Row, 2].Value);
        ObjectTypeCode_DB = Convert.ToInt16(worksheet.Cells[Row, 3].Value);
        Radionuclids_DB = Convert.ToString(worksheet.Cells[Row, 4].Value);
        Activity_DB = Convert.ToString(worksheet.Cells[Row, 5].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 5].Value), out var val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 5].Value);
        ProviderOrRecieverOKPO_DB = Convert.ToString(worksheet.Cells[Row, 6].Value);

    }
    public int ExcelRow(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true)
    {
        var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
        Column += Transpon ? cnt : 0;
        Row += !Transpon ? cnt : 0;

        worksheet.Cells[Row + (!Transpon ? 0 : 0), Column + (Transpon ? 0 : 0)].Value = OperationCode_DB;
        worksheet.Cells[Row + (!Transpon ? 1 : 0), Column + (Transpon ? 1 : 0)].Value = ObjectTypeCode_DB;
        worksheet.Cells[Row + (!Transpon ? 2 : 0), Column + (Transpon ? 2 : 0)].Value = Radionuclids_DB;
        worksheet.Cells[Row + (!Transpon ? 3 : 0), Column + (Transpon ? 3 : 0)].Value = string.IsNullOrEmpty(Activity_DB) || Activity_DB == null ? 0 : double.TryParse(Activity_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out var val) ? val : Activity_DB;
        worksheet.Cells[Row + (!Transpon ? 4 : 0), Column + (Transpon ? 4 : 0)].Value = ProviderOrRecieverOKPO_DB;
        return 5;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true)
    {
        var cnt = Form2.ExcelHeader(worksheet, Row, Column, Transpon);
        Column += Transpon ? cnt : 0;
        Row += !Transpon ? cnt : 0;

        worksheet.Cells[Row + (!Transpon ? 0 : 0), Column + (Transpon ? 0 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty(nameof(OperationCode)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 1 : 0), Column + (Transpon ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty(nameof(ObjectTypeCode)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 2 : 0), Column + (Transpon ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty(nameof(Radionuclids)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 3 : 0), Column + (Transpon ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty(nameof(Activity)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        worksheet.Cells[Row + (!Transpon ? 4 : 0), Column + (Transpon ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty(nameof(ProviderOrRecieverOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[1];
        return 5;
    }
    #endregion
    #region IDataGridColumn
    private static DataGridColumns _DataGridColumns { get; set; }
    public override DataGridColumns GetColumnStructure(string param = "")
    {
        if (_DataGridColumns == null)
        {
            #region NumberInOrder (1)
            var NumberInOrderR = ((FormPropertyAttribute)typeof(Form).GetProperty(nameof(NumberInOrder)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            NumberInOrderR.SetSizeColToAllLevels(50);
            NumberInOrderR.Binding = nameof(NumberInOrder);
            NumberInOrderR.Blocked = true;
            NumberInOrderR.ChooseLine = true;
            #endregion
            #region OperationCode (2)
            var OperationCodeR = ((FormPropertyAttribute)typeof(Form212).GetProperty(nameof(OperationCode)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            OperationCodeR.SetSizeColToAllLevels(100);
            OperationCodeR.Binding = nameof(OperationCode);
            NumberInOrderR += OperationCodeR;
            #endregion
            #region ObjectTypeCode (3)
            var ObjectTypeCodeR = ((FormPropertyAttribute)typeof(Form212).GetProperty(nameof(ObjectTypeCode)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            ObjectTypeCodeR.SetSizeColToAllLevels(193);
            ObjectTypeCodeR.Binding = nameof(ObjectTypeCode);
            NumberInOrderR += ObjectTypeCodeR;
            #endregion
            #region Radionuclids (4)
            var RadionuclidsR = ((FormPropertyAttribute)typeof(Form212).GetProperty(nameof(Radionuclids)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            RadionuclidsR.SetSizeColToAllLevels(145);
            RadionuclidsR.Binding = nameof(Radionuclids);
            NumberInOrderR += RadionuclidsR;
            #endregion
            #region Activity (5)
            var ActivityR = ((FormPropertyAttribute)typeof(Form212).GetProperty(nameof(Activity)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            ActivityR.SetSizeColToAllLevels(108);
            ActivityR.Binding = nameof(Activity);
            NumberInOrderR += ActivityR;
            #endregion
            #region ProviderOrRecieverOKPO (6)
            var ProviderOrRecieverOKPOR = ((FormPropertyAttribute)typeof(Form212).GetProperty(nameof(ProviderOrRecieverOKPO)).GetCustomAttributes(typeof(FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
            ProviderOrRecieverOKPOR.SetSizeColToAllLevels(193);
            ProviderOrRecieverOKPOR.Binding = nameof(ProviderOrRecieverOKPO);
            NumberInOrderR += ProviderOrRecieverOKPOR;
            #endregion
            _DataGridColumns = NumberInOrderR;
        }
        return _DataGridColumns;
    }
    #endregion
}