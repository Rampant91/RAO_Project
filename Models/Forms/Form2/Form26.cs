using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Linq;
using Spravochniki;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using Models.Collections;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.6: Контроль загрязнения подземных вод РВ")]
    public class Form26 : Abstracts.Form2
    {
        public Form26() : base()
        {
            FormNum.Value = "2.6";
            //NumberOfFields.Value = 11;
            Validate_all();
        }
        private void Validate_all()
        {
            ObservedSourceNumber_Validation(ObservedSourceNumber);
            ControlledAreaName_Validation(ControlledAreaName);
            SupposedWasteSource_Validation(SupposedWasteSource);
            DistanceToWasteSource_Validation(DistanceToWasteSource);
            TestDepth_Validation(TestDepth);
            RadionuclidName_Validation(RadionuclidName);
            AverageYearConcentration_Validation(AverageYearConcentration);
        }

        [Attributes.Form_Property(true,"Форма")]
        public override bool Object_Validation()
        {
            return !(ObservedSourceNumber.HasErrors||
            ControlledAreaName.HasErrors||
            SupposedWasteSource.HasErrors||
            DistanceToWasteSource.HasErrors||
            TestDepth.HasErrors||
            RadionuclidName.HasErrors||
            AverageYearConcentration.HasErrors);
        }

        //ObservedSourceNumber property
        #region ObservedSourceNumber
        public string ObservedSourceNumber_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property(true,"null-1","Номер наблюдательной скважины","2")]
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
            value.ClearErrors(); return true;
        }
        //ObservedSourceNumber property
        #endregion

        //ControlledAreaName property
        #region ControlledAreaName
        public string ControlledAreaName_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property(true,"null-1", "Наименование зоны контроля","3")]
        public RamAccess<string> ControlledAreaName
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ControlledAreaName)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ControlledAreaName)]).Value = ControlledAreaName_DB;
                    return (RamAccess<string>)Dictionary[nameof(ControlledAreaName)];
                }
                else
                {
                    var rm = new RamAccess<string>(ControlledAreaName_Validation, ControlledAreaName_DB);
                    rm.PropertyChanged += ControlledAreaNameValueChanged;
                    Dictionary.Add(nameof(ControlledAreaName), rm);
                    return (RamAccess<string>)Dictionary[nameof(ControlledAreaName)];
                }
            }
            set
            {
                ControlledAreaName_DB = value.Value;
                OnPropertyChanged(nameof(ControlledAreaName));
            }
        }
        //If change this change validation
        private void ControlledAreaNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ControlledAreaName_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool ControlledAreaName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value == "-")
            {
                return true;
            }
            List<string> spr = new List<string>()
            {
                "ПП",
                "СЗЗ",
                "ЗН",
                "прим."
            };
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //ControlledAreaName property
        #endregion

        //SupposedWasteSource property
        #region SupposedWasteSource
        public string SupposedWasteSource_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"null-1", "Предполагаемый источник поступления радиоактивных веществ","4")]
        public RamAccess<string> SupposedWasteSource
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(SupposedWasteSource)))
                {
                    ((RamAccess<string>)Dictionary[nameof(SupposedWasteSource)]).Value = SupposedWasteSource_DB;
                    return (RamAccess<string>)Dictionary[nameof(SupposedWasteSource)];
                }
                else
                {
                    var rm = new RamAccess<string>(SupposedWasteSource_Validation, SupposedWasteSource_DB);
                    rm.PropertyChanged += SupposedWasteSourceValueChanged;
                    Dictionary.Add(nameof(SupposedWasteSource), rm);
                    return (RamAccess<string>)Dictionary[nameof(SupposedWasteSource)];
                }
            }
            set
            {
                SupposedWasteSource_DB = value.Value;
                OnPropertyChanged(nameof(SupposedWasteSource));
            }
        }

        private void SupposedWasteSourceValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                SupposedWasteSource_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool SupposedWasteSource_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();//done
            return true;
        }
        //SupposedWasteSource property
        #endregion

        //DistanceToWasteSource property
        #region DistanceToWasteSource
        public string DistanceToWasteSource_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"null-1", "Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м","5")]
        public RamAccess<string> DistanceToWasteSource
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(DistanceToWasteSource)))
                {
                    ((RamAccess<string>)Dictionary[nameof(DistanceToWasteSource)]).Value = DistanceToWasteSource_DB;
                    return (RamAccess<string>)Dictionary[nameof(DistanceToWasteSource)];
                }
                else
                {
                    var rm = new RamAccess<string>(DistanceToWasteSource_Validation, DistanceToWasteSource_DB);
                    rm.PropertyChanged += DistanceToWasteSourceValueChanged;
                    Dictionary.Add(nameof(DistanceToWasteSource), rm);
                    return (RamAccess<string>)Dictionary[nameof(DistanceToWasteSource)];
                }
            }
            set
            {
                DistanceToWasteSource_DB = value.Value;
                OnPropertyChanged(nameof(DistanceToWasteSource));
            }
        }

        private void DistanceToWasteSourceValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        DistanceToWasteSource_DB = value1;
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
                DistanceToWasteSource_DB = value1;
            }
        }
        private bool DistanceToWasteSource_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value == "-")
            {
                return true;
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
        //DistanceToWasteSource property
        #endregion

        //TestDepth property
        #region TestDepth
        public string TestDepth_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"null-1", "Глубина отбора проб, м","6")]
        public RamAccess<string> TestDepth
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(TestDepth)))
                {
                    ((RamAccess<string>)Dictionary[nameof(TestDepth)]).Value = TestDepth_DB;
                    return (RamAccess<string>)Dictionary[nameof(TestDepth)];
                }
                else
                {
                    var rm = new RamAccess<string>(TestDepth_Validation, TestDepth_DB);
                    rm.PropertyChanged += TestDepthValueChanged;
                    Dictionary.Add(nameof(TestDepth), rm);
                    return (RamAccess<string>)Dictionary[nameof(TestDepth)];
                }
            }
            set
            {
                TestDepth_DB = value.Value;
                OnPropertyChanged(nameof(TestDepth));
            }
        }

        private void TestDepthValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        TestDepth_DB = value1;
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
                TestDepth_DB = value1;
            }
        }
        private bool TestDepth_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value == "-")
            {
                return true;
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
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                {
                    value.AddError("Число должно быть больше нуля"); return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //TestDepth property
        #endregion

        //RadionuclidName property
        #region RadionuclidName
        public string RadionuclidName_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property(true,"null-1", "Наименование радионуклида","7")]
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
        //If change this change validation
        private void RadionuclidNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                RadionuclidName_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool RadionuclidName_Validation(RamAccess<string> value)//TODO
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

        //AverageYearConcentration property
        #region AverageYearConcentration 
        public string AverageYearConcentration_DB { get; set; } = null; [NotMapped]
        [Attributes.Form_Property(true,"null-1", "Среднегодовое содержание радионуклида, Бк/кг","8")]
        public RamAccess<string> AverageYearConcentration
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(AverageYearConcentration)))
                {
                    ((RamAccess<string>)Dictionary[nameof(AverageYearConcentration)]).Value = AverageYearConcentration_DB;
                    return (RamAccess<string>)Dictionary[nameof(AverageYearConcentration)];
                }
                else
                {
                    var rm = new RamAccess<string>(AverageYearConcentration_Validation, AverageYearConcentration_DB);
                    rm.PropertyChanged += AverageYearConcentrationValueChanged;
                    Dictionary.Add(nameof(AverageYearConcentration), rm);
                    return (RamAccess<string>)Dictionary[nameof(AverageYearConcentration)];
                }
            }
            set
            {
                AverageYearConcentration_DB = value.Value;
                OnPropertyChanged(nameof(AverageYearConcentration));
            }
        }

        private void AverageYearConcentrationValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        AverageYearConcentration_DB = value1;
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
                AverageYearConcentration_DB = value1;
            }
        }
        private bool AverageYearConcentration_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                return true;
            }
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
        //AverageYearConcentration property
        #endregion

        #region IExcel
        public void ExcelGetRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelGetRow(worksheet, Row);
            ObservedSourceNumber_DB = Convert.ToString(worksheet.Cells[Row, 1].Value);
            ControlledAreaName_DB = Convert.ToString(worksheet.Cells[Row, 2].Value);
            SupposedWasteSource_DB = Convert.ToString(worksheet.Cells[Row, 3].Value);
            DistanceToWasteSource_DB = Convert.ToString(worksheet.Cells[Row, 4].Value);
            TestDepth_DB = Convert.ToString(worksheet.Cells[Row, 5].Value);
            RadionuclidName_DB = Convert.ToString(worksheet.Cells[Row, 6].Value);
            AverageYearConcentration_DB = Convert.ToString(worksheet.Cells[Row, 7].Value);

        }
        public int ExcelRow(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);
            double val;

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ObservedSourceNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ControlledAreaName_DB;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = SupposedWasteSource_DB;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = DistanceToWasteSource_DB== "" || DistanceToWasteSource_DB == "-" || DistanceToWasteSource_DB == null ? 0  : double.TryParse(DistanceToWasteSource_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : DistanceToWasteSource_DB;
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = TestDepth_DB== "" || TestDepth_DB == "-" || TestDepth_DB == null ? 0  : double.TryParse(TestDepth_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : TestDepth_DB;
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = RadionuclidName_DB;
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = AverageYearConcentration_DB== "" || AverageYearConcentration_DB == "-" || AverageYearConcentration_DB == null ? 0  : double.TryParse(AverageYearConcentration_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : AverageYearConcentration_DB;
            return 7;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = Form2.ExcelHeader(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form26,Models").GetProperty(nameof(ObservedSourceNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form26,Models").GetProperty(nameof(ControlledAreaName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form26,Models").GetProperty(nameof(SupposedWasteSource)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form26,Models").GetProperty(nameof(DistanceToWasteSource)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form26,Models").GetProperty(nameof(TestDepth)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form26,Models").GetProperty(nameof(RadionuclidName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form26,Models").GetProperty(nameof(AverageYearConcentration)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            return 7;
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
                #region ObservedSourceNumber (2)
                DataGridColumns ObservedSourceNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form26).GetProperty(nameof(Form26.ObservedSourceNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                ObservedSourceNumberR.SetSizeColToAllLevels(164);
                ObservedSourceNumberR.Binding = nameof(Form26.ObservedSourceNumber);
                NumberInOrderR += ObservedSourceNumberR;
                #endregion
                #region ControlledAreaName (3)
                DataGridColumns ControlledAreaNameR = ((Attributes.Form_PropertyAttribute)typeof(Form26).GetProperty(nameof(Form26.ControlledAreaName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                ControlledAreaNameR.SetSizeColToAllLevels(166);
                ControlledAreaNameR.Binding = nameof(Form26.ControlledAreaName);
                NumberInOrderR += ControlledAreaNameR;
                #endregion
                #region SupposedWasteSource (4)
                DataGridColumns SupposedWasteSourceR = ((Attributes.Form_PropertyAttribute)typeof(Form26).GetProperty(nameof(Form26.SupposedWasteSource)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                SupposedWasteSourceR.SetSizeColToAllLevels(238);
                SupposedWasteSourceR.Binding = nameof(Form26.SupposedWasteSource);
                NumberInOrderR += SupposedWasteSourceR;
                #endregion
                #region DistanceToWasteSource (5)
                DataGridColumns DistanceToWasteSourceR = ((Attributes.Form_PropertyAttribute)typeof(Form26).GetProperty(nameof(Form26.DistanceToWasteSource)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                DistanceToWasteSourceR.SetSizeColToAllLevels(337);
                DistanceToWasteSourceR.Binding = nameof(Form26.DistanceToWasteSource);
                NumberInOrderR += DistanceToWasteSourceR;
                #endregion
                #region TestDepth (6)
                DataGridColumns TestDepthR = ((Attributes.Form_PropertyAttribute)typeof(Form26).GetProperty(nameof(Form26.TestDepth)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                TestDepthR.SetSizeColToAllLevels(180);
                TestDepthR.Binding = nameof(Form26.TestDepth);
                NumberInOrderR += TestDepthR;
                #endregion
                #region RadionuclidName (7)
                DataGridColumns RadionuclidNameR = ((Attributes.Form_PropertyAttribute)typeof(Form26).GetProperty(nameof(Form26.RadionuclidName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                RadionuclidNameR.SetSizeColToAllLevels(230);
                RadionuclidNameR.Binding = nameof(Form26.RadionuclidName);
                NumberInOrderR += RadionuclidNameR;
                #endregion
                #region AverageYearConcentration (8)
                DataGridColumns AverageYearConcentrationR = ((Attributes.Form_PropertyAttribute)typeof(Form26).GetProperty(nameof(Form26.AverageYearConcentration)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                AverageYearConcentrationR.SetSizeColToAllLevels(200);
                AverageYearConcentrationR.Binding = nameof(Form26.AverageYearConcentration);
                NumberInOrderR += AverageYearConcentrationR;
                #endregion
                _DataGridColumns = NumberInOrderR;
            }
            return _DataGridColumns;
        }
        #endregion
    }
}
