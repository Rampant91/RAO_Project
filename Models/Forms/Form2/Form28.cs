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

        [Attributes.Form_Property("Форма")]
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
        public string WasteSourceName_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Наименование, номер выпуска сточных вод")]
        public RamAccess<string> WasteSourceName
        {
            get
            {
                    var tmp = new RamAccess<string>(WasteSourceName_Validation, WasteSourceName_DB);
                    tmp.PropertyChanged += WasteSourceNameValueChanged;
                    return tmp;
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
        public string WasteRecieverName_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("наименование")]
        public RamAccess<string> WasteRecieverName
        {
            get
            {
                    var tmp = new RamAccess<string>(WasteRecieverName_Validation, WasteRecieverName_DB);
                    tmp.PropertyChanged += WasteRecieverNameValueChanged;
                    return tmp;
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
        [Attributes.Form_Property("код типа приемника")]
        public RamAccess<string> RecieverTypeCode
        {
            get
            {
                    var tmp = new RamAccess<string>(RecieverTypeCode_Validation, RecieverTypeCode_DB);
                    tmp.PropertyChanged += RecieverTypeCodeValueChanged;
                    return tmp;
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
        [Attributes.Form_Property("наименование бассейнового округа")]
        public RamAccess<string> PoolDistrictName
        {
            get
            {
                    var tmp = new RamAccess<string>(PoolDistrictName_Validation, PoolDistrictName_DB);
                    tmp.PropertyChanged += PoolDistrictNameValueChanged;
                    return tmp;
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
        [Attributes.Form_Property("Допустимый объем водоотведения за год, тыс. куб. м")]
        public RamAccess<string> AllowedWasteRemovalVolume
        {
            get
            {
                    var tmp = new RamAccess<string>(AllowedWasteRemovalVolume_Validation, AllowedWasteRemovalVolume_DB);
                    tmp.PropertyChanged += AllowedWasteRemovalVolumeValueChanged;
                    return tmp;
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    AllowedWasteRemovalVolume_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
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
        [Attributes.Form_Property("Отведено за отчетный период, тыс. куб. м")]
        public RamAccess<string> RemovedWasteVolume
        {
            get
            {
                    var tmp = new RamAccess<string>(RemovedWasteVolume_Validation, RemovedWasteVolume_DB);
                    tmp.PropertyChanged += RemovedWasteVolumeValueChanged;
                    return tmp;
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    RemovedWasteVolume_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
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

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = WasteSourceName_DB;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = WasteRecieverName_DB;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = RecieverTypeCode_DB;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = PoolDistrictName_DB;
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = AllowedWasteRemovalVolume_DB;
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = RemovedWasteVolume_DB;
            return 6;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = Form2.ExcelHeader(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(WasteSourceName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(WasteRecieverName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(RecieverTypeCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(PoolDistrictName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(AllowedWasteRemovalVolume)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form28,Models").GetProperty(nameof(RemovedWasteVolume)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;

            return 6;
        }
        #endregion
    }
}
