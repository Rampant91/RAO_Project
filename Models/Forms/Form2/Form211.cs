using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System; using System.Collections.Generic;
using System.Globalization;
using Spravochniki;
using System.Linq;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using Models.Collections;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.11: Радионуклидный состав загрязненных участков территорий")]
    public class Form211 : Abstracts.Form2
    {
        public Form211() : base()
        {
            FormNum.Value = "2.11";
            //NumberOfFields.Value = 11;
            Validate_all();
        }
        private void Validate_all()
        {
            Radionuclids_Validation(Radionuclids);
            PlotName_Validation(PlotName);
            PlotKadastrNumber_Validation(PlotKadastrNumber);
            PlotCode_Validation(PlotCode);
            InfectedArea_Validation(InfectedArea);
            SpecificActivityOfPlot_Validation(SpecificActivityOfPlot);
            SpecificActivityOfLiquidPart_Validation(SpecificActivityOfLiquidPart);
            SpecificActivityOfDensePart_Validation(SpecificActivityOfDensePart);
        }

        [Attributes.Form_Property(true,"Форма")]
        public override bool Object_Validation()
        {
            return !(Radionuclids.HasErrors||
            PlotName.HasErrors||
            PlotKadastrNumber.HasErrors||
            PlotCode.HasErrors||
            InfectedArea.HasErrors||
            SpecificActivityOfPlot.HasErrors||
            SpecificActivityOfLiquidPart.HasErrors||
            SpecificActivityOfDensePart.HasErrors);
        }

        //PlotName property
        #region  PlotName
        public string PlotName_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"null-1","Наименование участка","2")]
        public RamAccess<string> PlotName
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PlotName)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PlotName)]).Value = PlotName_DB;
                    return (RamAccess<string>)Dictionary[nameof(PlotName)];
                }
                else
                {
                    var rm = new RamAccess<string>(PlotName_Validation, PlotName_DB);
                    rm.PropertyChanged += PlotNameValueChanged;
                    Dictionary.Add(nameof(PlotName), rm);
                    return (RamAccess<string>)Dictionary[nameof(PlotName)];
                }
            }
            set
            {
                PlotName_DB = value.Value;
                OnPropertyChanged(nameof(PlotName));
            }
        }

       private void PlotNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PlotName_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PlotName_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //PlotName property
        #endregion

        //PlotKadastrNumber property
        #region  PlotKadastrNumber
        public string PlotKadastrNumber_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"null-1","Кадастровый номер участка","3")]
        public RamAccess<string> PlotKadastrNumber
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PlotKadastrNumber)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PlotKadastrNumber)]).Value = PlotKadastrNumber_DB;
                    return (RamAccess<string>)Dictionary[nameof(PlotKadastrNumber)];
                }
                else
                {
                    var rm = new RamAccess<string>(PlotKadastrNumber_Validation, PlotKadastrNumber_DB);
                    rm.PropertyChanged += PlotKadastrNumberValueChanged;
                    Dictionary.Add(nameof(PlotKadastrNumber), rm);
                    return (RamAccess<string>)Dictionary[nameof(PlotKadastrNumber)];
                }
            }
            set
            {
                PlotKadastrNumber_DB = value.Value;
                OnPropertyChanged(nameof(PlotKadastrNumber));
            }
        }

       private void PlotKadastrNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PlotKadastrNumber_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PlotKadastrNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //PlotKadastrNumber property
        #endregion

        //PlotCode property
        #region  PlotCode
        public string PlotCode_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"null-1","Код участка","4")]
        public RamAccess<string> PlotCode
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PlotCode)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PlotCode)]).Value = PlotCode_DB;
                    return (RamAccess<string>)Dictionary[nameof(PlotCode)];
                }
                else
                {
                    var rm = new RamAccess<string>(PlotCode_Validation, PlotCode_DB);
                    rm.PropertyChanged += PlotCodeValueChanged;
                    Dictionary.Add(nameof(PlotCode), rm);
                    return (RamAccess<string>)Dictionary[nameof(PlotCode)];
                }
            }
            set
            {
                PlotCode_DB = value.Value;
                OnPropertyChanged(nameof(PlotCode));
            }
        }
        //6 symbols code
       private void PlotCodeValueChanged(object Value, PropertyChangedEventArgs args)
       {
            if (args.PropertyName == "Value")
            {
                PlotCode_DB = ((RamAccess<string>)Value).Value;
            }
       }
        private bool PlotCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex a = new Regex("^[0-9]{6}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //PlotCode property
        #endregion

        //InfectedArea property
        #region  InfectedArea
        public string InfectedArea_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property(true,"null-1","Площадь загрязненной территории, кв. м","5")]
        public RamAccess<string> InfectedArea
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(InfectedArea)))
                {
                    ((RamAccess<string>)Dictionary[nameof(InfectedArea)]).Value = InfectedArea_DB;
                    return (RamAccess<string>)Dictionary[nameof(InfectedArea)];
                }
                else
                {
                    var rm = new RamAccess<string>(InfectedArea_Validation, InfectedArea_DB);
                    rm.PropertyChanged += InfectedAreaValueChanged;
                    Dictionary.Add(nameof(InfectedArea), rm);
                    return (RamAccess<string>)Dictionary[nameof(InfectedArea)];
                }
            }
            set
            {
                InfectedArea_DB = value.Value;
                OnPropertyChanged(nameof(InfectedArea));
            }
        }

        private void InfectedAreaValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        InfectedArea_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                InfectedArea_DB = value1;
            }
        }
        private bool InfectedArea_Validation(RamAccess<string> value)//TODO
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
        //InfectedArea property
        #endregion

        //Radionuclids property
        #region  Radionuclids
        public string Radionuclids_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"null-1","Наименования радионуклидов","6")]
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
            value.Value.Replace(" ", "");
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            string[] nuclids = value.Value.Split("; ");
            bool flag = true;
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

        //SpecificActivityOfPlot property
        #region  SpecificActivityOfPlot
        public string SpecificActivityOfPlot_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"null-1","Удельная активность, Бк/г","7")]
        public RamAccess<string> SpecificActivityOfPlot
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(SpecificActivityOfPlot)))
                {
                    ((RamAccess<string>)Dictionary[nameof(SpecificActivityOfPlot)]).Value = SpecificActivityOfPlot_DB;
                    return (RamAccess<string>)Dictionary[nameof(SpecificActivityOfPlot)];
                }
                else
                {
                    var rm = new RamAccess<string>(SpecificActivityOfPlot_Validation, SpecificActivityOfPlot_DB);
                    rm.PropertyChanged += SpecificActivityOfPlotValueChanged;
                    Dictionary.Add(nameof(SpecificActivityOfPlot), rm);
                    return (RamAccess<string>)Dictionary[nameof(SpecificActivityOfPlot)];
                }
            }
            set
            {
                SpecificActivityOfPlot_DB = value.Value;
                OnPropertyChanged(nameof(SpecificActivityOfPlot));
            }
        }

        private void SpecificActivityOfPlotValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        SpecificActivityOfPlot_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                SpecificActivityOfPlot_DB = value1;
            }
        }
        private bool SpecificActivityOfPlot_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value.Value.Equals("-"))
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
        //SpecificActivityOfPlot property
        #endregion

        //SpecificActivityOfLiquidPart property
        #region  SpecificActivityOfLiquidPart
        public string SpecificActivityOfLiquidPart_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"null-1","Удельная активность жидкой части, Бк/г","8")]
        public RamAccess<string> SpecificActivityOfLiquidPart
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(SpecificActivityOfLiquidPart)))
                {
                    ((RamAccess<string>)Dictionary[nameof(SpecificActivityOfLiquidPart)]).Value = SpecificActivityOfLiquidPart_DB;
                    return (RamAccess<string>)Dictionary[nameof(SpecificActivityOfLiquidPart)];
                }
                else
                {
                    var rm = new RamAccess<string>(SpecificActivityOfLiquidPart_Validation, SpecificActivityOfLiquidPart_DB);
                    rm.PropertyChanged += SpecificActivityOfLiquidPartValueChanged;
                    Dictionary.Add(nameof(SpecificActivityOfLiquidPart), rm);
                    return (RamAccess<string>)Dictionary[nameof(SpecificActivityOfLiquidPart)];
                }
            }
            set
            {
                SpecificActivityOfLiquidPart_DB = value.Value;
                OnPropertyChanged(nameof(SpecificActivityOfLiquidPart));
            }
        }

        private void SpecificActivityOfLiquidPartValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        SpecificActivityOfLiquidPart_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                SpecificActivityOfLiquidPart_DB = value1;
            }
        }
        private bool SpecificActivityOfLiquidPart_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value.Value.Equals("-"))
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
        //SpecificActivityOfLiquidPart property
        #endregion

        //SpecificActivityOfDensePart property
        #region SpecificActivityOfDensePart 
        public string SpecificActivityOfDensePart_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"null-1","Удельная активность твердой части, Бк/г","9")]
        public RamAccess<string> SpecificActivityOfDensePart
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(SpecificActivityOfDensePart)))
                {
                    ((RamAccess<string>)Dictionary[nameof(SpecificActivityOfDensePart)]).Value = SpecificActivityOfDensePart_DB;
                    return (RamAccess<string>)Dictionary[nameof(SpecificActivityOfDensePart)];
                }
                else
                {
                    var rm = new RamAccess<string>(SpecificActivityOfDensePart_Validation, SpecificActivityOfDensePart_DB);
                    rm.PropertyChanged += SpecificActivityOfDensePartValueChanged;
                    Dictionary.Add(nameof(SpecificActivityOfDensePart), rm);
                    return (RamAccess<string>)Dictionary[nameof(SpecificActivityOfDensePart)];
                }
            }
            set
            {
                SpecificActivityOfDensePart_DB = value.Value;
                OnPropertyChanged(nameof(SpecificActivityOfDensePart));
            }
        }

        private void SpecificActivityOfDensePartValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        SpecificActivityOfDensePart_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                SpecificActivityOfDensePart_DB = value1;
            }
        }
        private bool SpecificActivityOfDensePart_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value.Value.Equals("-"))
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
        //SpecificActivityOfDensePart property
        #endregion

        #region IExcel
        public int ExcelRow(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = PlotName_DB;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = PlotKadastrNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = PlotCode_DB;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = InfectedArea_DB;
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = Radionuclids_DB;
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = SpecificActivityOfPlot_DB;
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = SpecificActivityOfLiquidPart_DB;
            worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = SpecificActivityOfDensePart_DB;
            return 8;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = Form2.ExcelHeader(worksheet, Row, Column, Transpon);
            Column = Column +(Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

           worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(PlotName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(PlotKadastrNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(PlotCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(InfectedArea)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(Radionuclids)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(SpecificActivityOfPlot)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(SpecificActivityOfLiquidPart)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(SpecificActivityOfDensePart)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];

            return 8;
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
                NumberInOrderR.SetSizeColToAllLevels(88);
                NumberInOrderR.Binding = nameof(Form.NumberInOrder);
                NumberInOrderR.Blocked = true;
                NumberInOrderR.ChooseLine = true;
                #endregion
                #region PlotName (2)
                DataGridColumns PlotNameR = ((Attributes.Form_PropertyAttribute)typeof(Form211).GetProperty(nameof(Form211.PlotName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                PlotNameR.SetSizeColToAllLevels(163);
                PlotNameR.Binding = nameof(Form211.PlotName);
                NumberInOrderR += PlotNameR;
                #endregion
                #region PlotKadastrNumber (3)
                DataGridColumns PlotKadastrNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form211).GetProperty(nameof(Form211.PlotKadastrNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                PlotKadastrNumberR.SetSizeColToAllLevels(173);
                PlotKadastrNumberR.Binding = nameof(Form211.PlotKadastrNumber);
                NumberInOrderR += PlotKadastrNumberR;
                #endregion
                #region PlotCode (4)
                DataGridColumns PlotCodeR = ((Attributes.Form_PropertyAttribute)typeof(Form211).GetProperty(nameof(Form211.PlotCode)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                PlotCodeR.SetSizeColToAllLevels(88);
                PlotCodeR.Binding = nameof(Form211.PlotCode);
                NumberInOrderR += PlotCodeR;
                #endregion
                #region InfectedArea (5)
                DataGridColumns InfectedAreaR = ((Attributes.Form_PropertyAttribute)typeof(Form211).GetProperty(nameof(Form211.InfectedArea)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                InfectedAreaR.SetSizeColToAllLevels(248);
                InfectedAreaR.Binding = nameof(Form211.InfectedArea);
                NumberInOrderR += InfectedAreaR;
                #endregion
                #region Radionuclids (6)
                DataGridColumns RadionuclidsR = ((Attributes.Form_PropertyAttribute)typeof(Form211).GetProperty(nameof(Form211.Radionuclids)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                RadionuclidsR.SetSizeColToAllLevels(188);
                RadionuclidsR.Binding = nameof(Form211.Radionuclids);
                NumberInOrderR += RadionuclidsR;
                #endregion
                #region SpecificActivityOfPlot (7)
                DataGridColumns SpecificActivityOfPlotR = ((Attributes.Form_PropertyAttribute)typeof(Form211).GetProperty(nameof(Form211.SpecificActivityOfPlot)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                SpecificActivityOfPlotR.SetSizeColToAllLevels(155);
                SpecificActivityOfPlotR.Binding = nameof(Form211.SpecificActivityOfPlot);
                NumberInOrderR += SpecificActivityOfPlotR;
                #endregion
                #region SpecificActivityOfLiquidPart (8)
                DataGridColumns SpecificActivityOfLiquidPartR = ((Attributes.Form_PropertyAttribute)typeof(Form211).GetProperty(nameof(Form211.SpecificActivityOfLiquidPart)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                SpecificActivityOfLiquidPartR.SetSizeColToAllLevels(236);
                SpecificActivityOfLiquidPartR.Binding = nameof(Form211.SpecificActivityOfLiquidPart);
                NumberInOrderR += SpecificActivityOfLiquidPartR;
                #endregion
                #region SpecificActivityOfDensePart (9)
                DataGridColumns SpecificActivityOfDensePartR = ((Attributes.Form_PropertyAttribute)typeof(Form211).GetProperty(nameof(Form211.SpecificActivityOfDensePart)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                SpecificActivityOfDensePartR.SetSizeColToAllLevels(246);
                SpecificActivityOfDensePartR.Binding = nameof(Form211.SpecificActivityOfDensePart);
                NumberInOrderR += SpecificActivityOfDensePartR;
                #endregion
                _DataGridColumns = NumberInOrderR;
            }
            return _DataGridColumns;
        }
        #endregion
    }
}
