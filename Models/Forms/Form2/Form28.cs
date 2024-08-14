using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;
using Spravochniki;

namespace Models.Forms.Form2;

[Serializable]
[Form_Class("Форма 2.8: Отведение сточных вод, содержащих радионуклиды")]
[Table (name: "form_28")]
public class Form28 : Form2
{
    #region Constructor

    public Form28()
    {
        FormNum.Value = "2.8";
        //NumberOfFields.Value = 24;
        Validate_all();
    }

    #endregion

    #region Validation
    
    private void Validate_all()
    {
        WasteSourceName_Validation(WasteSourceName);
        WasteRecieverName_Validation(WasteRecieverName);
        RecieverTypeCode_Validation(RecieverTypeCode);
        AllowedWasteRemovalVolume_Validation(AllowedWasteRemovalVolume);
        RemovedWasteVolume_Validation(RemovedWasteVolume);
        PoolDistrictName_Validation(PoolDistrictName);
    }

    [FormProperty(true, "Форма")]
    public override bool Object_Validation()
    {
        return !(WasteSourceName.HasErrors ||
                 WasteRecieverName.HasErrors ||
                 RecieverTypeCode.HasErrors ||
                 AllowedWasteRemovalVolume.HasErrors ||
                 RemovedWasteVolume.HasErrors ||
                 PoolDistrictName.HasErrors);
    }

    #endregion

    #region Properties

    #region WasteSourceName (2)

    public string WasteSourceName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-2", "Наименование, номер выпуска сточных вод", "2")]
    public RamAccess<string> WasteSourceName
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(WasteSourceName), out var value))
            {
                ((RamAccess<string>)value).Value = WasteSourceName_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(WasteSourceName_Validation, WasteSourceName_DB);
            rm.PropertyChanged += WasteSourceName_ValueChanged;
            Dictionary.Add(nameof(WasteSourceName), rm);
            return (RamAccess<string>)Dictionary[nameof(WasteSourceName)];
        }
        set
        {
            WasteSourceName_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void WasteSourceName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        WasteSourceName_DB = ((RamAccess<string>)value).Value;
    }

    private bool WasteSourceName_Validation(RamAccess<string> value)
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

    #region WasteRecieverName (3)

    public string WasteRecieverName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Приемник отведенных вод", "наименование", "3")]
    public RamAccess<string> WasteRecieverName
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(WasteRecieverName), out var value))
            {
                ((RamAccess<string>)value).Value = WasteRecieverName_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(WasteRecieverName_Validation, WasteRecieverName_DB);
            rm.PropertyChanged += WasteRecieverName_ValueChanged;
            Dictionary.Add(nameof(WasteRecieverName), rm);
            return (RamAccess<string>)Dictionary[nameof(WasteRecieverName)];
        }
        set
        {
            WasteRecieverName_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void WasteRecieverName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        WasteRecieverName_DB = ((RamAccess<string>)value).Value;
    }

    private bool WasteRecieverName_Validation(RamAccess<string> value)
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

    #region RecieverTypeCode (4)

    public string RecieverTypeCode_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Приемник отведенных вод", "код типа приемника", "4")]
    public RamAccess<string> RecieverTypeCode
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(RecieverTypeCode), out var value))
            {
                ((RamAccess<string>)value).Value = RecieverTypeCode_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(RecieverTypeCode_Validation, RecieverTypeCode_DB);
            rm.PropertyChanged += RecieverTypeCode_ValueChanged;
            Dictionary.Add(nameof(RecieverTypeCode), rm);
            return (RamAccess<string>)Dictionary[nameof(RecieverTypeCode)];
        }
        set
        {
            RecieverTypeCode_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void RecieverTypeCode_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        RecieverTypeCode_DB = ((RamAccess<string>)value).Value;
    }

    private bool RecieverTypeCode_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (Spravochniks.SprRecieverTypeCode.Contains(value.Value))
        {
            return true;
        }
        value.AddError("Недопустимое значение");
        return false;
    }

    #endregion

    #region PoolDistrictName (5)

    public string PoolDistrictName_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Приемник отведенных вод", "наименование бассейнового округа", "5")]
    public RamAccess<string> PoolDistrictName
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(PoolDistrictName), out var value))
            {
                ((RamAccess<string>)value).Value = PoolDistrictName_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(PoolDistrictName_Validation, PoolDistrictName_DB);
            rm.PropertyChanged += PoolDistrictName_ValueChanged;
            Dictionary.Add(nameof(PoolDistrictName), rm);
            return (RamAccess<string>)Dictionary[nameof(PoolDistrictName)];
        }
        set
        {
            PoolDistrictName_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void PoolDistrictName_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        PoolDistrictName_DB = ((RamAccess<string>)value).Value;
    }

    private bool PoolDistrictName_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        return true;
        //List<string> spr = new List<string>();
        //if (spr.Contains(value.Value))
        //{
        //    return true;
        //}
        //value.AddError("Недопустимое значение");
        //return false;
    }

    #endregion

    #region AllowedWasteRemovalVolume (6)

    public string AllowedWasteRemovalVolume_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "null-3", "Допустимый объем водоотведения за год, тыс. куб. м", "6")]
    public RamAccess<string> AllowedWasteRemovalVolume
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(AllowedWasteRemovalVolume), out var value))
            {
                ((RamAccess<string>)value).Value = AllowedWasteRemovalVolume_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(AllowedWasteRemovalVolume_Validation, AllowedWasteRemovalVolume_DB);
            rm.PropertyChanged += AllowedWasteRemovalVolume_ValueChanged;
            Dictionary.Add(nameof(AllowedWasteRemovalVolume), rm);
            return (RamAccess<string>)Dictionary[nameof(AllowedWasteRemovalVolume)];
        }
        set
        {
            AllowedWasteRemovalVolume_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void AllowedWasteRemovalVolume_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        AllowedWasteRemovalVolume_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool AllowedWasteRemovalVolume_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #region RemovedWasteVolume (7)

    public string RemovedWasteVolume_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "null-4", "Отведено за отчетный период, тыс. куб. м", "7")]
    public RamAccess<string> RemovedWasteVolume
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(RemovedWasteVolume), out var value))
            {
                ((RamAccess<string>)value).Value = RemovedWasteVolume_DB;
                return (RamAccess<string>)value;
            }
            var rm = new RamAccess<string>(RemovedWasteVolume_Validation, RemovedWasteVolume_DB);
            rm.PropertyChanged += RemovedWasteVolume_ValueChanged;
            Dictionary.Add(nameof(RemovedWasteVolume), rm);
            return (RamAccess<string>)Dictionary[nameof(RemovedWasteVolume)];
        }
        set
        {
            RemovedWasteVolume_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void RemovedWasteVolume_ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        RemovedWasteVolume_DB = ExponentialString_ValueChanged(((RamAccess<string>)value).Value);
    }

    private bool RemovedWasteVolume_Validation(RamAccess<string> value) => ExponentialString_Validation(value);

    #endregion

    #endregion

    #region IExcel

    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        base.ExcelGetRow(worksheet, row);
        WasteSourceName_DB = Convert.ToString(worksheet.Cells[row, 2].Value);
        WasteRecieverName_DB = Convert.ToString(worksheet.Cells[row, 3].Value);
        RecieverTypeCode_DB = Convert.ToString(worksheet.Cells[row, 4].Value);
        PoolDistrictName_DB = Convert.ToString(worksheet.Cells[row, 5].Value);
        AllowedWasteRemovalVolume_DB = ConvertFromExcelDouble(worksheet.Cells[row, 6].Value);
        RemovedWasteVolume_DB = ConvertFromExcelDouble(worksheet.Cells[row, 7].Value);
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = base.ExcelRow(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ConvertToExcelString(WasteSourceName_DB);
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(WasteRecieverName_DB);
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelString(RecieverTypeCode_DB);
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelString(PoolDistrictName_DB);
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelDouble(AllowedWasteRemovalVolume_DB);
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelDouble(RemovedWasteVolume_DB);
        
        return 6;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        var cnt = Form2.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        worksheet.Cells[row, column].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form28,Models")?.GetProperty(nameof(WasteSourceName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form28,Models")?.GetProperty(nameof(WasteRecieverName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form28,Models")?.GetProperty(nameof(RecieverTypeCode))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form28,Models")?.GetProperty(nameof(PoolDistrictName))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form28,Models")?.GetProperty(nameof(AllowedWasteRemovalVolume))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];
        worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form28,Models")?.GetProperty(nameof(RemovedWasteVolume))?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[1];

        return 6;
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

        #region WasteSourceName (2)

        var wasteSourceNameR = ((FormPropertyAttribute)typeof(Form28)
                .GetProperty(nameof(WasteSourceName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (wasteSourceNameR != null)
        {
            wasteSourceNameR.SetSizeColToAllLevels(258);
            wasteSourceNameR.Binding = nameof(WasteSourceName);
            numberInOrderR += wasteSourceNameR;
        }

        #endregion

        #region WasteRecieverName (3)

        var wasteRecieverNameR = ((FormPropertyAttribute)typeof(Form28)
                .GetProperty(nameof(WasteRecieverName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (wasteRecieverNameR != null)
        {
            wasteRecieverNameR.SetSizeColToAllLevels(238);
            wasteRecieverNameR.Binding = nameof(WasteRecieverName);
            numberInOrderR += wasteRecieverNameR;
        }

        #endregion

        #region RecieverTypeCode (4)

        var recieverTypeCodeR = ((FormPropertyAttribute)typeof(Form28)
                .GetProperty(nameof(RecieverTypeCode))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (recieverTypeCodeR != null)
        {
            recieverTypeCodeR.SetSizeColToAllLevels(200);
            recieverTypeCodeR.Binding = nameof(RecieverTypeCode);
            numberInOrderR += recieverTypeCodeR;
        }

        #endregion

        #region PoolDistrictName (5)

        var poolDistrictNameR = ((FormPropertyAttribute)typeof(Form28)
                .GetProperty(nameof(PoolDistrictName))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (poolDistrictNameR != null)
        {
            poolDistrictNameR.SetSizeColToAllLevels(213);
            poolDistrictNameR.Binding = nameof(PoolDistrictName);
            numberInOrderR += poolDistrictNameR;
        }

        #endregion

        #region AllowedWasteRemovalVolume (6)

        var allowedWasteRemovalVolumeR = ((FormPropertyAttribute)typeof(Form28)
                .GetProperty(nameof(AllowedWasteRemovalVolume))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (allowedWasteRemovalVolumeR != null)
        {
            allowedWasteRemovalVolumeR.SetSizeColToAllLevels(213);
            allowedWasteRemovalVolumeR.Binding = nameof(AllowedWasteRemovalVolume);
            numberInOrderR += allowedWasteRemovalVolumeR;
        }

        #endregion

        #region RemovedWasteVolume (7)

        var removedWasteVolumeR = ((FormPropertyAttribute)typeof(Form28)
                .GetProperty(nameof(RemovedWasteVolume))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD(numberInOrderR);
        if (removedWasteVolumeR != null)
        {
            removedWasteVolumeR.SetSizeColToAllLevels(208);
            removedWasteVolumeR.Binding = nameof(RemovedWasteVolume);
            numberInOrderR += removedWasteVolumeR;
        }

        #endregion

        _DataGridColumns = numberInOrderR;

        return _DataGridColumns;
    }

    #endregion
}