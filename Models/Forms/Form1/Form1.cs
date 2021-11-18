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

        [NotMapped]
        public bool flag = false;
        public Form1():base()
        {

        }
        protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }

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
        [Attributes.Form_Property("код")]
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
        [Attributes.Form_Property("дата")]
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
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                OperationDate_DB = tmp;
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
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
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
        [Attributes.Form_Property("вид")]
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
        [Attributes.Form_Property("номер")]
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
        [Attributes.Form_Property("дата")]
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
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                DocumentDate_DB = tmp;
            }
        }
        protected virtual bool DocumentDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if(string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
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
