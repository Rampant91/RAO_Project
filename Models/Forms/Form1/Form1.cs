using Models.DataAccess;
using System;
using Spravochniki;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using Models.Attributes;
using OfficeOpenXml;

namespace Models.Abstracts
{
    public abstract class Form1 : Form
    {
        [Attributes.Form_Property("Форма")]

        public Form1():base()
        {

        }
        protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }

        #region NumberInOrder
        public int NumberInOrder_DB { get; set; } = 0;
        [NotMapped]
        [Attributes.Form_Property("№ п/п")]
        public RamAccess<int> NumberInOrder
        {
            get
            {
                var tmp = new RamAccess<int>(NumberInOrder_Validation, NumberInOrder_DB);
                tmp.PropertyChanged += NumberInOrderValueChanged;
                return tmp;
            }
            set
            {
                NumberInOrder_DB = value.Value;
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        private void NumberInOrderValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                NumberInOrder_DB = ((RamAccess<int>)Value).Value;
            }
        }
        private bool NumberInOrder_Validation(RamAccess<int> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region OperationCode
        public short? OperationCode_DB { get; set; } = null;
        public bool OperationCode_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool OperationCode_Hidden
        {
            get => OperationCode_Hidden_Priv;
            set
            {
                OperationCode_Hidden_Priv = value;
            }
        }

        [NotMapped]
        [Attributes.Form_Property("Код операции")]
        public RamAccess<short?> OperationCode
        {
            get
            {
                if (!OperationCode_Hidden_Priv)
                {
                    var tmp = new RamAccess<short?>(OperationCode_Validation, OperationCode_DB);
                    tmp.PropertyChanged += OperationCodeValueChanged;
                    return tmp;
                }
                else
                {
                    var tmp = new RamAccess<short?>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!OperationCode_Hidden_Priv)
                {
                    OperationCode_DB = value.Value;
                    OnPropertyChanged(nameof(OperationCode));
                }
            }
        }
        private void OperationCodeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                OperationCode_DB = ((RamAccess<short?>)Value).Value;
            }
        }
        protected virtual bool OperationCode_Validation(RamAccess<short?> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region OperationDate
        public string OperationDate_DB { get; set; } = "";
        public bool OperationDate_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool OperationDate_Hidden
        {
            get => OperationDate_Hidden_Priv;
            set
            {
                OperationDate_Hidden_Priv = value;
            }
        }

        [NotMapped]
        [Attributes.Form_Property("Дата операции")]
        public RamAccess<string> OperationDate
        {
            get
            {
                if (!OperationDate_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(OperationDate_Validation, OperationDate_DB);
                    tmp.PropertyChanged += OperationDateValueChanged;
                    return tmp;
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!OperationDate_Hidden_Priv)
                {
                    OperationDate_DB = value.Value;
                    OnPropertyChanged(nameof(OperationDate));
                }
            }
        }
        private void OperationDateValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                OperationDate_DB = ((RamAccess<string>)Value).Value;
            }
        }
        protected virtual bool OperationDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region DocumentVid
        
        public byte? DocumentVid_DB { get; set; } = null;
        public bool DocumentVid_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool DocumentVid_Hidden
        {
            get => DocumentVid_Hidden_Priv;
            set
            {
                DocumentVid_Hidden_Priv = value;
            }
        }

        [NotMapped]
        [Attributes.Form_Property("Вид документа")]
        public RamAccess<byte?> DocumentVid
        {
            get
            {
                if (!DocumentVid_Hidden_Priv)
                {
                    var tmp = new RamAccess<byte?>(DocumentVid_Validation, DocumentVid_DB);
                    tmp.PropertyChanged += DocumentVidValueChanged;
                    return tmp;
                }
                else
                {
                    var tmp = new RamAccess<byte?>(null, null);
                    return tmp;
                }
            }
            set
            {
                DocumentVid_DB = value.Value;
                OnPropertyChanged(nameof(DocumentVid));
            }
        }

        private void DocumentVidValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                DocumentVid_DB = ((RamAccess<byte?>)Value).Value;
            }
        }
        protected virtual bool DocumentVid_Validation(RamAccess<byte?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            foreach (Tuple<byte?, string> item in Spravochniks.SprDocumentVidName)
            {
                if (value.Value == item.Item1)
                {
                    return true;
                }
            }
            value.AddError("Недопустимое значение");
            return false;
        }
        #endregion

        #region DocumentNumber
        public string DocumentNumber_DB { get; set; } = "";
        public bool DocumentNumber_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool DocumentNumber_Hidden
        {
            get => DocumentNumber_Hidden_Priv;
            set
            {
                DocumentNumber_Hidden_Priv = value;
            }
        }

        [NotMapped]
        [Attributes.Form_Property("Номер документа")]
        public RamAccess<string> DocumentNumber
        {
            get
            {
                if (!DocumentNumber_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(DocumentNumber_Validation, DocumentNumber_DB);
                    tmp.PropertyChanged += DocumentNumberValueChanged;
                    return tmp;
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!DocumentNumber_Hidden_Priv)
                {
                    DocumentNumber_DB = value.Value;
                    OnPropertyChanged(nameof(DocumentNumber));
                }
            }
        }
        private void DocumentNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                DocumentNumber_DB = ((RamAccess<string>)Value).Value;
            }
        }
        protected virtual bool DocumentNumber_Validation(RamAccess<string> value)//Ready
        { return true; }
        #endregion

        #region DocumentDate
        public string DocumentDate_DB { get; set; } = "";
        public bool DocumentDate_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool DocumentDate_Hidden
        {
            get => DocumentDate_Hidden_Priv;
            set
            {
                DocumentDate_Hidden_Priv = value;
            }
        }

        [NotMapped]
        [Attributes.Form_Property("Дата документа")]
        public RamAccess<string> DocumentDate
        {
            get
            {
                if (!DocumentDate_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(DocumentDate_Validation, DocumentDate_DB);
                    tmp.PropertyChanged += DocumentDateValueChanged;
                    return tmp;
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!DocumentDate_Hidden_Priv)
                {
                    DocumentDate_DB = value.Value;
                    OnPropertyChanged(nameof(DocumentDate));
                }
            }
        }
        private void DocumentDateValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                DocumentDate_DB = ((RamAccess<string>)Value).Value;
            }
        }
        protected virtual bool DocumentDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region IExcel
        public void ExcelRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelRow(worksheet, Row);
            worksheet.Cells[Row, 1].Value = NumberInOrder_DB;
            worksheet.Cells[Row, 2].Value = OperationCode_DB;
            worksheet.Cells[Row, 3].Value = OperationDate_DB;
        }

        public static void ExcelHeader(ExcelWorksheet worksheet)
        {
            Form.ExcelHeader(worksheet);
            worksheet.Cells[1, 1].Value = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").GetProperty(nameof(NumberInOrder))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 2].Value = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").GetProperty(nameof(OperationCode))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 3].Value = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").GetProperty(nameof(OperationDate))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
        }
        #endregion
    }
}
