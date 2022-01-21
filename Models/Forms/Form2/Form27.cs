using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System; using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Spravochniki;
using System.Globalization;
using System.ComponentModel;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using Models.Collections;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.7: Поступление радионуклидов в атмосферный воздух")]
    public class Form27 : Abstracts.Form2
    {
        public Form27() : base()
        {
            FormNum.Value = "2.7";
            //NumberOfFields.Value = 13;
            Validate_all();
        }
        private void Validate_all()
        {
            ObservedSourceNumber_Validation(ObservedSourceNumber);
            RadionuclidName_Validation(RadionuclidName);
            AllowedWasteValue_Validation(AllowedWasteValue);
            FactedWasteValue_Validation(FactedWasteValue);
            WasteOutbreakPreviousYear_Validation(WasteOutbreakPreviousYear);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return !(ObservedSourceNumber.HasErrors||
            RadionuclidName.HasErrors||
            AllowedWasteValue.HasErrors||
            FactedWasteValue.HasErrors||
            WasteOutbreakPreviousYear.HasErrors);
        }

        //ObservedSourceNumber property
        #region  ObservedSourceNumber
        public string ObservedSourceNumber_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Наименование, номер источника выбросов")]
        public RamAccess<string> ObservedSourceNumber
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ObservedSourceNumber)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ObservedSourceNumber)]).Value = ObservedSourceNumber_DB;
                    return (RamAccess<string>)Dictionary[nameof(ObservedSourceNumber)];
                }
                else
                {
                    var rm = new RamAccess<string>(ObservedSourceNumber_Validation, ObservedSourceNumber_DB);
                    rm.PropertyChanged += ObservedSourceNumberValueChanged;
                    Dictionary.Add(nameof(ObservedSourceNumber), rm);
                    return (RamAccess<string>)Dictionary[nameof(ObservedSourceNumber)];
                }
            }
            set
            {
                ObservedSourceNumber_DB = value.Value;
                OnPropertyChanged(nameof(ObservedSourceNumber));
            }
        }
        //If change this change validation
        private void ObservedSourceNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ObservedSourceNumber_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool ObservedSourceNumber_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //ObservedSourceNumber property
        #endregion

        //RadionuclidName property
        #region  RadionuclidName
        public string RadionuclidName_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Наименование радионуклида")]
        public RamAccess<string> RadionuclidName
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(RadionuclidName)))
                {
                    ((RamAccess<string>)Dictionary[nameof(RadionuclidName)]).Value = RadionuclidName_DB;
                    return (RamAccess<string>)Dictionary[nameof(RadionuclidName)];
                }
                else
                {
                    var rm = new RamAccess<string>(RadionuclidName_Validation, RadionuclidName_DB);
                    rm.PropertyChanged += RadionuclidNameValueChanged;
                    Dictionary.Add(nameof(RadionuclidName), rm);
                    return (RamAccess<string>)Dictionary[nameof(RadionuclidName)];
                }
            }
            set
            {
                RadionuclidName_DB = value.Value;
                OnPropertyChanged(nameof(RadionuclidName));
            }
        }


        private void RadionuclidNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                RadionuclidName_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool RadionuclidName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var tmpstr = value.Value.ToLower().Replace(" ", "");
            var query = from item in Spravochniks.SprRadionuclids where item.Item1 == tmpstr select item.Item1;
            if (!query.Any())
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //RadionuclidName property
        #endregion

        //AllowedWasteValue property
        #region  AllowedWasteValue
        public string AllowedWasteValue_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("разрешенный")]
        public RamAccess<string> AllowedWasteValue
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(AllowedWasteValue)))
                {
                    ((RamAccess<string>)Dictionary[nameof(AllowedWasteValue)]).Value = AllowedWasteValue_DB;
                    return (RamAccess<string>)Dictionary[nameof(AllowedWasteValue)];
                }
                else
                {
                    var rm = new RamAccess<string>(AllowedWasteValue_Validation, AllowedWasteValue_DB);
                    rm.PropertyChanged += AllowedWasteValueValueChanged;
                    Dictionary.Add(nameof(AllowedWasteValue), rm);
                    return (RamAccess<string>)Dictionary[nameof(AllowedWasteValue)];
                }
            }
            set
            {
                AllowedWasteValue_DB = value.Value;
                OnPropertyChanged(nameof(AllowedWasteValue));
            }
        }


        private void AllowedWasteValueValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        AllowedWasteValue_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                AllowedWasteValue_DB = value1;
            }
        }
        private bool AllowedWasteValue_Validation(RamAccess<string> value)
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
        //AllowedWasteValue property
        #endregion

        //FactedWasteValue property
        #region  FactedWasteValue
        public string FactedWasteValue_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("фактический")]
        public RamAccess<string> FactedWasteValue
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(FactedWasteValue)))
                {
                    ((RamAccess<string>)Dictionary[nameof(FactedWasteValue)]).Value = FactedWasteValue_DB;
                    return (RamAccess<string>)Dictionary[nameof(FactedWasteValue)];
                }
                else
                {
                    var rm = new RamAccess<string>(FactedWasteValue_Validation, FactedWasteValue_DB);
                    rm.PropertyChanged += FactedWasteValueValueChanged;
                    Dictionary.Add(nameof(FactedWasteValue), rm);
                    return (RamAccess<string>)Dictionary[nameof(FactedWasteValue)];
                }
            }
            set
            {
                FactedWasteValue_DB = value.Value;
                OnPropertyChanged(nameof(FactedWasteValue));
            }
        }


        private void FactedWasteValueValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        FactedWasteValue_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                FactedWasteValue_DB = value1;
            }
        }
        private bool FactedWasteValue_Validation(RamAccess<string> value)
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
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //FactedWasteValue property
        #endregion

        //WasteOutbreakPreviousYear property
        #region  WasteOutbreakPreviousYear
        public string WasteOutbreakPreviousYear_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("фактический")]
        public RamAccess<string> WasteOutbreakPreviousYear
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(WasteOutbreakPreviousYear)))
                {
                    ((RamAccess<string>)Dictionary[nameof(WasteOutbreakPreviousYear)]).Value = WasteOutbreakPreviousYear_DB;
                    return (RamAccess<string>)Dictionary[nameof(WasteOutbreakPreviousYear)];
                }
                else
                {
                    var rm = new RamAccess<string>(WasteOutbreakPreviousYear_Validation, WasteOutbreakPreviousYear_DB);
                    rm.PropertyChanged += WasteOutbreakPreviousYearValueChanged;
                    Dictionary.Add(nameof(WasteOutbreakPreviousYear), rm);
                    return (RamAccess<string>)Dictionary[nameof(WasteOutbreakPreviousYear)];
                }
            }
            set
            {
                WasteOutbreakPreviousYear_DB = value.Value;
                OnPropertyChanged(nameof(WasteOutbreakPreviousYear));
            }
        }


        private void WasteOutbreakPreviousYearValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        WasteOutbreakPreviousYear_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                WasteOutbreakPreviousYear_DB = value1;
            }
        }
        private bool WasteOutbreakPreviousYear_Validation(RamAccess<string> value)
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
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //WasteOutbreakPreviousYear property
        #endregion

        #region IExcel
        public int ExcelRow(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ObservedSourceNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = RadionuclidName_DB;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = AllowedWasteValue_DB;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = FactedWasteValue_DB;
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = WasteOutbreakPreviousYear_DB;
            return 5;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = Form2.ExcelHeader(worksheet, Row, Column, Transpon);
            Column = Column +(Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

            //worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form27,Models").GetProperty(nameof(ObservedSourceNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            //worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form27,Models").GetProperty(nameof(RadionuclidName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            //worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form27,Models").GetProperty(nameof(AllowedWasteValue)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            //worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form27,Models").GetProperty(nameof(FactedWasteValue)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            //worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form27,Models").GetProperty(nameof(WasteOutbreakPreviousYear)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            return 5;
        }
        #endregion
        #region IDataGridColumn
        public override DataGridColumns GetColumnStructure(string param)
        {
            return null;
        }
        #endregion
    }
}
