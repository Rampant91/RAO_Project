using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Linq;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using Spravochniki;
using Models.Collections;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.8: Отведение сточных вод, содержащих радионуклиды")]
    public class Form28 : Abstracts.Form2
    {
        public Form28() : base()
        {
            FormNum.Value = "2.8";
            //NumberOfFields.Value = 24;
            Validate_all();
        }
        private void Validate_all()
        {
            WasteSourceName_Validation(WasteSourceName);
            WasteRecieverName_Validation(WasteRecieverName);
            RecieverTypeCode_Validation(RecieverTypeCode);
            AllowedWasteRemovalVolume_Validation(AllowedWasteRemovalVolume);
            RemovedWasteVolume_Validation(RemovedWasteVolume);
            PoolDistrictName_Validation(PoolDistrictName);
        }

        [Attributes.Form_Property(true,"Форма")]
        public override bool Object_Validation()
        {
            return !(WasteSourceName.HasErrors||
            WasteRecieverName.HasErrors||
            RecieverTypeCode.HasErrors||
            AllowedWasteRemovalVolume.HasErrors||
            RemovedWasteVolume.HasErrors||
            PoolDistrictName.HasErrors);
        }

        //WasteSourceName property
        #region WasteSourceName
        public string WasteSourceName_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"null-2","Наименование, номер выпуска сточных вод","2")]
        public RamAccess<string> WasteSourceName
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(WasteSourceName)))
                {
                    ((RamAccess<string>)Dictionary[nameof(WasteSourceName)]).Value = WasteSourceName_DB;
                    return (RamAccess<string>)Dictionary[nameof(WasteSourceName)];
                }
                else
                {
                    var rm = new RamAccess<string>(WasteSourceName_Validation, WasteSourceName_DB);
                    rm.PropertyChanged += WasteSourceNameValueChanged;
                    Dictionary.Add(nameof(WasteSourceName), rm);
                    return (RamAccess<string>)Dictionary[nameof(WasteSourceName)];
                }
            }
            set
            {
                WasteSourceName_DB = value.Value;
                OnPropertyChanged(nameof(WasteSourceName));
            }
        }

        private void WasteSourceNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                WasteSourceName_DB = ((RamAccess<string>)Value).Value;
            }
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
        //WasteSourceName property
        #endregion

        //WasteRecieverName property
        #region WasteRecieverName
        public string WasteRecieverName_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Приемник отведенных вод", "наименование","3")]
        public RamAccess<string> WasteRecieverName
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(WasteRecieverName)))
                {
                    ((RamAccess<string>)Dictionary[nameof(WasteRecieverName)]).Value = WasteRecieverName_DB;
                    return (RamAccess<string>)Dictionary[nameof(WasteRecieverName)];
                }
                else
                {
                    var rm = new RamAccess<string>(WasteRecieverName_Validation, WasteRecieverName_DB);
                    rm.PropertyChanged += WasteRecieverNameValueChanged;
                    Dictionary.Add(nameof(WasteRecieverName), rm);
                    return (RamAccess<string>)Dictionary[nameof(WasteRecieverName)];
                }
            }
            set
            {
                WasteRecieverName_DB = value.Value;
                OnPropertyChanged(nameof(WasteRecieverName));
            }
        }

        private void WasteRecieverNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                WasteRecieverName_DB = ((RamAccess<string>)Value).Value;
            }
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
        //WasteRecieverName property
        #endregion

        //RecieverTypeCode property
        #region RecieverTypeCode
        public string RecieverTypeCode_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property(true,"Приемник отведенных вод", "код типа приемника","4")]
        public RamAccess<string> RecieverTypeCode
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(RecieverTypeCode)))
                {
                    ((RamAccess<string>)Dictionary[nameof(RecieverTypeCode)]).Value = RecieverTypeCode_DB;
                    return (RamAccess<string>)Dictionary[nameof(RecieverTypeCode)];
                }
                else
                {
                    var rm = new RamAccess<string>(RecieverTypeCode_Validation, RecieverTypeCode_DB);
                    rm.PropertyChanged += RecieverTypeCodeValueChanged;
                    Dictionary.Add(nameof(RecieverTypeCode), rm);
                    return (RamAccess<string>)Dictionary[nameof(RecieverTypeCode)];
                }
            }
            set
            {
                RecieverTypeCode_DB = value.Value;
                OnPropertyChanged(nameof(RecieverTypeCode));
            }
        }

        private void RecieverTypeCodeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                RecieverTypeCode_DB = ((RamAccess<string>)Value).Value;
            }
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
        //RecieverTypeCode property
        #endregion

        //PoolDistrictName property
        #region PoolDistrictName
        public string PoolDistrictName_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property(true,"Приемник отведенных вод", "наименование бассейнового округа","5")]
        public RamAccess<string> PoolDistrictName
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PoolDistrictName)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PoolDistrictName)]).Value = PoolDistrictName_DB;
                    return (RamAccess<string>)Dictionary[nameof(PoolDistrictName)];
                }
                else
                {
                    var rm = new RamAccess<string>(PoolDistrictName_Validation, PoolDistrictName_DB);
                    rm.PropertyChanged += PoolDistrictNameValueChanged;
                    Dictionary.Add(nameof(PoolDistrictName), rm);
                    return (RamAccess<string>)Dictionary[nameof(PoolDistrictName)];
                }
            }
            set
            {
                PoolDistrictName_DB = value.Value;
                OnPropertyChanged(nameof(PoolDistrictName));
            }
        }

        private void PoolDistrictNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PoolDistrictName_DB = ((RamAccess<string>)Value).Value;
            }
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
        //PoolDistrictName property
        #endregion

        //AllowedWasteRemovalVolume property
        #region AllowedWasteRemovalVolume
        public string AllowedWasteRemovalVolume_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property(true,"null-3","Допустимый объем водоотведения за год, тыс. куб. м", "6")]
        public RamAccess<string> AllowedWasteRemovalVolume
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(AllowedWasteRemovalVolume)))
                {
                    ((RamAccess<string>)Dictionary[nameof(AllowedWasteRemovalVolume)]).Value = AllowedWasteRemovalVolume_DB;
                    return (RamAccess<string>)Dictionary[nameof(AllowedWasteRemovalVolume)];
                }
                else
                {
                    var rm = new RamAccess<string>(AllowedWasteRemovalVolume_Validation, AllowedWasteRemovalVolume_DB);
                    rm.PropertyChanged += AllowedWasteRemovalVolumeValueChanged;
                    Dictionary.Add(nameof(AllowedWasteRemovalVolume), rm);
                    return (RamAccess<string>)Dictionary[nameof(AllowedWasteRemovalVolume)];
                }
            }
            set
            {
                AllowedWasteRemovalVolume_DB = value.Value;
                OnPropertyChanged(nameof(AllowedWasteRemovalVolume));
            }
        }

        private void AllowedWasteRemovalVolumeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        AllowedWasteRemovalVolume_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
                }
                AllowedWasteRemovalVolume_DB = value1;
            }
        }
        private bool AllowedWasteRemovalVolume_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        //AllowedWasteRemovalVolume property
        #endregion

        //RemovedWasteVolume property
        #region RemovedWasteVolume
        public string RemovedWasteVolume_DB { get; set; } = null; [NotMapped]
        [Attributes.Form_Property(true,"null-4","Отведено за отчетный период, тыс. куб. м","7")]
        public RamAccess<string> RemovedWasteVolume
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(RemovedWasteVolume)))
                {
                    ((RamAccess<string>)Dictionary[nameof(RemovedWasteVolume)]).Value = RemovedWasteVolume_DB;
                    return (RamAccess<string>)Dictionary[nameof(RemovedWasteVolume)];
                }
                else
                {
                    var rm = new RamAccess<string>(RemovedWasteVolume_Validation, RemovedWasteVolume_DB);
                    rm.PropertyChanged += RemovedWasteVolumeValueChanged;
                    Dictionary.Add(nameof(RemovedWasteVolume), rm);
                    return (RamAccess<string>)Dictionary[nameof(RemovedWasteVolume)];
                }
            }
            set
            {
                RemovedWasteVolume_DB = value.Value;
                OnPropertyChanged(nameof(RemovedWasteVolume));
            }
        }

        private void RemovedWasteVolumeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        RemovedWasteVolume_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
                }
                RemovedWasteVolume_DB = value1;
            }
        }
        private bool RemovedWasteVolume_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        //RemovedWasteVolume property
        #endregion

        #region IExcel
        public int ExcelRow(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);
            double val;

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = WasteSourceName_DB;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = WasteRecieverName_DB;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = RecieverTypeCode_DB;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = PoolDistrictName_DB;
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = AllowedWasteRemovalVolume_DB== "" || AllowedWasteRemovalVolume_DB == "-" || AllowedWasteRemovalVolume_DB == null ? 0  : double.TryParse(AllowedWasteRemovalVolume_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : AllowedWasteRemovalVolume_DB;
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value =  RemovedWasteVolume_DB== "" || RemovedWasteVolume_DB == "-" || RemovedWasteVolume_DB == null ? 0  : double.TryParse(RemovedWasteVolume_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : RemovedWasteVolume_DB;
            return 6;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = Form2.ExcelHeader(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

           worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(WasteSourceName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(WasteRecieverName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(RecieverTypeCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(PoolDistrictName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(AllowedWasteRemovalVolume)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(RemovedWasteVolume)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];

            return 6;
        }
        #endregion
        #region IDataGridColumn
        private static DataGridColumns _DataGridColumns { get; set; } = null;
        public override DataGridColumns GetColumnStructure(string param = "")
        {
            if (_DataGridColumns == null)
            {
                #region NumberInOrder (1)
                DataGridColumns NumberInOrderR = ((Attributes.Form_PropertyAttribute)typeof(Form).GetProperty(nameof(Form.NumberInOrder)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                NumberInOrderR.SetSizeColToAllLevels(50);
                NumberInOrderR.Binding = nameof(Form.NumberInOrder);
                NumberInOrderR.Blocked = true;
                NumberInOrderR.ChooseLine = true;
                #endregion
                #region WasteSourceName (2)
                DataGridColumns WasteSourceNameR = ((Attributes.Form_PropertyAttribute)typeof(Form28).GetProperty(nameof(Form28.WasteSourceName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                WasteSourceNameR.SetSizeColToAllLevels(258);
                WasteSourceNameR.Binding = nameof(Form28.WasteSourceName);
                NumberInOrderR += WasteSourceNameR;
                #endregion
                #region WasteRecieverName (3)
                DataGridColumns WasteRecieverNameR = ((Attributes.Form_PropertyAttribute)typeof(Form28).GetProperty(nameof(Form28.WasteRecieverName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                WasteRecieverNameR.SetSizeColToAllLevels(238);
                WasteRecieverNameR.Binding = nameof(Form28.WasteRecieverName);
                NumberInOrderR += WasteRecieverNameR;
                #endregion
                #region RecieverTypeCode (4)
                DataGridColumns RecieverTypeCodeR = ((Attributes.Form_PropertyAttribute)typeof(Form28).GetProperty(nameof(Form28.RecieverTypeCode)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                RecieverTypeCodeR.SetSizeColToAllLevels(200);
                RecieverTypeCodeR.Binding = nameof(Form28.RecieverTypeCode);
                NumberInOrderR += RecieverTypeCodeR;
                #endregion
                #region PoolDistrictName (5)
                DataGridColumns PoolDistrictNameR = ((Attributes.Form_PropertyAttribute)typeof(Form28).GetProperty(nameof(Form28.PoolDistrictName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                PoolDistrictNameR.SetSizeColToAllLevels(213);
                PoolDistrictNameR.Binding = nameof(Form28.PoolDistrictName);
                NumberInOrderR += PoolDistrictNameR;
                #endregion
                #region AllowedWasteRemovalVolume (6)
                DataGridColumns AllowedWasteRemovalVolumeR = ((Attributes.Form_PropertyAttribute)typeof(Form28).GetProperty(nameof(Form28.AllowedWasteRemovalVolume)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                AllowedWasteRemovalVolumeR.SetSizeColToAllLevels(213);
                AllowedWasteRemovalVolumeR.Binding = nameof(Form28.AllowedWasteRemovalVolume);
                NumberInOrderR += AllowedWasteRemovalVolumeR;
                #endregion
                #region RemovedWasteVolume (7)
                DataGridColumns RemovedWasteVolumeR = ((Attributes.Form_PropertyAttribute)typeof(Form28).GetProperty(nameof(Form28.RemovedWasteVolume)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                RemovedWasteVolumeR.SetSizeColToAllLevels(208);
                RemovedWasteVolumeR.Binding = nameof(Form28.RemovedWasteVolume);
                NumberInOrderR += RemovedWasteVolumeR;
                #endregion
                _DataGridColumns = NumberInOrderR;
            }
            return _DataGridColumns;
        }
        #endregion
    }
}
