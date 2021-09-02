using Models.DataAccess;
using System;
using Spravochniki;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        [Attributes.Form_Property("Код")]
        public RamAccess<short?> OperationCode
        {
            get
            {
                var tmp = new RamAccess<short?>(OperationCode_Validation, OperationCode_DB);
                tmp.PropertyChanged += OperationCodeValueChanged;
                return tmp;
            }
            set
            {
                OperationCode_DB = value.Value;
                OnPropertyChanged(nameof(OperationCode));
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
        [NotMapped]
        [Attributes.Form_Property("Дата операции")]
        public RamAccess<string> OperationDate
        {
            get
            {
                var tmp = new RamAccess<string>(OperationDate_Validation, OperationDate_DB);
                tmp.PropertyChanged += OperationDateValueChanged;
                return tmp;
            }
            set
            {
                OperationDate_DB = value.Value;
                OnPropertyChanged(nameof(OperationDate));
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
        [NotMapped]
        [Attributes.Form_Property("Вид документа")]
        public RamAccess<byte?> DocumentVid
        {
            get
            {
                var tmp = new RamAccess<byte?>(DocumentVid_Validation, DocumentVid_DB);
                tmp.PropertyChanged += DocumentVidValueChanged;
                return tmp;
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
        [NotMapped]
        [Attributes.Form_Property("Номер документа")]
        public RamAccess<string> DocumentNumber
        {
            get
            {
                var tmp = new RamAccess<string>(DocumentNumber_Validation, DocumentNumber_DB);
                tmp.PropertyChanged += DocumentNumberValueChanged;
                return tmp;
            }
            set
            {
                DocumentNumber_DB = value.Value;
                OnPropertyChanged(nameof(DocumentNumber));
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
        [NotMapped]
        [Attributes.Form_Property("Дата документа")]
        public RamAccess<string> DocumentDate
        {
            get
            {
                var tmp = new RamAccess<string>(DocumentDate_Validation, DocumentDate_DB);
                tmp.PropertyChanged += DocumentDateValueChanged;
                return tmp;
            }
            set
            {
                DocumentDate_DB = value.Value;
                OnPropertyChanged(nameof(DocumentDate));
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
            //bool ab = (OperationCode.Value >= 11) && (OperationCode.Value <= 18);
            //bool b = (OperationCode.Value >= 41) && (OperationCode.Value <= 49);
            //bool c = (OperationCode.Value >= 51) && (OperationCode.Value <= 59);
            //bool d = (OperationCode.Value == 65) || (OperationCode.Value == 68);
            //if (ab || b || c || d)
            //{
            //    if (!value.Value.Equals(OperationDate))
            //    {
            //        value.AddError("Заполните примечание");//to do note handling
            //    }
            //}
            return true;
        }
        #endregion
    }
}
