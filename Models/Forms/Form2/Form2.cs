using System;
using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using Models.Attributes;
using OfficeOpenXml;

namespace Models.Abstracts
{
    public abstract class Form2 : Form
    {
        [Attributes.Form_Property("Форма")]
        public Form2()
        {

        }

        protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }

        #region CorrectionNumber

        public byte CorrectionNumber_DB { get; set; } = 0;

        [NotMapped]
        [Attributes.Form_Property("Номер корректировки")]
        public RamAccess<byte> CorrectionNumber
        {
            get
            {
                var tmp = new RamAccess<byte>(CorrectionNumber_Validation, CorrectionNumber_DB);
                tmp.PropertyChanged += CorrectionNumberValueChanged;
                return tmp;
            }
            set
            {
                CorrectionNumber_DB = value.Value;
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }

        private void CorrectionNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                CorrectionNumber_DB = ((RamAccess<byte>) Value).Value;
            }
        }

        private bool CorrectionNumber_Validation(RamAccess<byte> value)
        {
            value.ClearErrors();
            return true;
        }

        //CorrectionNumber property

        #endregion

        #region IExcel
        public void ExcelRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelRow(worksheet, Row);
            worksheet.Cells[Row, 1].Value = NumberInOrder_DB;
        }

        public static void ExcelHeader(ExcelWorksheet worksheet)
        {
            Form.ExcelHeader(worksheet);
            worksheet.Cells[1, 1].Value = ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").GetProperty(nameof(NumberInOrder))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
        }
        #endregion
    }
}
