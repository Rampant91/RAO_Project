using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Globalization;
using Spravochniki;
using System.Linq;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;

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

        [Attributes.Form_Property("Форма")]
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
        public string PlotName_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Наименование участка")]
        public RamAccess<string> PlotName
        {
            get
            {
                    var tmp = new RamAccess<string>(PlotName_Validation, PlotName_DB);
                    tmp.PropertyChanged += PlotNameValueChanged;
                    return tmp;
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
        public string PlotKadastrNumber_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Кадастровый номер участка")]
        public RamAccess<string> PlotKadastrNumber
        {
            get
            {
                    var tmp = new RamAccess<string>(PlotKadastrNumber_Validation, PlotKadastrNumber_DB);
                    tmp.PropertyChanged += PlotKadastrNumberValueChanged;
                    return tmp;
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
        public string PlotCode_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Код участка")]
        public RamAccess<string> PlotCode
        {
            get
            {
                    var tmp = new RamAccess<string>(PlotCode_Validation, PlotCode_DB);
                    tmp.PropertyChanged += PlotCodeValueChanged;
                    return tmp;
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
        public string InfectedArea_DB { get; set; } = null; [NotMapped]        [Attributes.Form_Property("Площадь загрязненной территории, кв. м")]
        public RamAccess<string> InfectedArea
        {
            get
            {
                    var tmp = new RamAccess<string>(InfectedArea_Validation, InfectedArea_DB);
                    tmp.PropertyChanged += InfectedAreaValueChanged;
                    return tmp;
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    InfectedArea_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
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
        public string Radionuclids_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Наименования радионуклидов")]
        public RamAccess<string> Radionuclids
        {
            get
            {
                    var tmp = new RamAccess<string>(Radionuclids_Validation, Radionuclids_DB);//OK
                    tmp.PropertyChanged += RadionuclidsValueChanged;
                    return tmp;
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
        public string SpecificActivityOfPlot_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Удельная активность, Бк/г")]
        public RamAccess<string> SpecificActivityOfPlot
        {
            get
            {
                    var tmp = new RamAccess<string>(SpecificActivityOfPlot_Validation, SpecificActivityOfPlot_DB);
                    tmp.PropertyChanged += SpecificActivityOfPlotValueChanged;
                    return tmp;
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    SpecificActivityOfPlot_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                SpecificActivityOfPlot_DB = value1;
            }
        }
        private bool SpecificActivityOfPlot_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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
        //SpecificActivityOfPlot property
        #endregion

        //SpecificActivityOfLiquidPart property
        #region  SpecificActivityOfLiquidPart
        public string SpecificActivityOfLiquidPart_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Удельная активность жидкой части, Бк/г")]
        public RamAccess<string> SpecificActivityOfLiquidPart
        {
            get
            {
                    var tmp = new RamAccess<string>(SpecificActivityOfLiquidPart_Validation, SpecificActivityOfLiquidPart_DB);
                    tmp.PropertyChanged += SpecificActivityOfLiquidPartValueChanged;
                    return tmp;
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    SpecificActivityOfLiquidPart_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                SpecificActivityOfLiquidPart_DB = value1;
            }
        }
        private bool SpecificActivityOfLiquidPart_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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
        //SpecificActivityOfLiquidPart property
        #endregion

        //SpecificActivityOfDensePart property
        #region SpecificActivityOfDensePart 
        public string SpecificActivityOfDensePart_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Удельная активность твердой части, Бк/г")]
        public RamAccess<string> SpecificActivityOfDensePart
        {
            get
            {
                var tmp = new RamAccess<string>(SpecificActivityOfDensePart_Validation, SpecificActivityOfDensePart_DB);
                tmp.PropertyChanged += SpecificActivityOfDensePartValueChanged;
                return tmp;
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    SpecificActivityOfDensePart_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                SpecificActivityOfDensePart_DB = value1;
            }
        }
        private bool SpecificActivityOfDensePart_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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
        //SpecificActivityOfDensePart property
        #endregion

        #region IExcel
        public void ExcelRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelRow(worksheet, Row);
            worksheet.Cells[Row, 2].Value = PlotName_DB;
            worksheet.Cells[Row, 3].Value = PlotKadastrNumber_DB;
            worksheet.Cells[Row, 4].Value = PlotCode_DB;
            worksheet.Cells[Row, 5].Value = InfectedArea_DB;
            worksheet.Cells[Row, 6].Value = Radionuclids_DB;
            worksheet.Cells[Row, 7].Value = SpecificActivityOfPlot_DB;
            worksheet.Cells[Row, 8].Value = SpecificActivityOfLiquidPart_DB;
            worksheet.Cells[Row, 9].Value = SpecificActivityOfDensePart_DB;
        }

        public static void ExcelHeader(ExcelWorksheet worksheet)
        {
            Form2.ExcelHeader(worksheet);
            worksheet.Cells[1, 2].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(PlotName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 3].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(PlotKadastrNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 4].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(PlotCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 5].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(InfectedArea)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 6].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(Radionuclids)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 7].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(SpecificActivityOfPlot)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 8].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(SpecificActivityOfLiquidPart)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 9].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form211,Models").GetProperty(nameof(SpecificActivityOfDensePart)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
        }
        #endregion
    }
}
