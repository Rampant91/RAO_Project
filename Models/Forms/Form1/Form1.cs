using Models.DataAccess;
using System;
using System.Collections.Generic;
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
            Validate_base();
        }
        protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }

        protected void Validate_base()
        {
            OperationCode_Validation(OperationCode);
            OperationDate_Validation(OperationDate);
            DocumentNumber_Validation(DocumentNumber);
            DocumentVid_Validation(DocumentVid);
            DocumentDate_Validation(DocumentDate);
            NumberInOrder_Validation(NumberInOrder);
        }

        #region NumberInOrder
        public int NumberInOrder_DB { get; set; } = 0;
        [NotMapped]
        [Attributes.Form_Property("№ п/п")]
        public RamAccess<int> NumberInOrder
        {
            get => new RamAccess<int>(NumberInOrder_Validation, NumberInOrder_DB);
            set
            {
                NumberInOrder_DB = value.Value;
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        private bool NumberInOrder_Validation(RamAccess<int> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region OperationCode
        public short? OperationCode_DB { get; set; } = 0;
        [NotMapped]
        [Attributes.Form_Property("Код")]
        public RamAccess<short?> OperationCode
        {
            get => new RamAccess<short?>(OperationCode_Validation, OperationCode_DB);
            set
            {
                OperationCode_DB = value.Value;
                OnPropertyChanged(nameof(OperationCode));
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
            get => new RamAccess<string>(OperationDate_Validation, OperationDate_DB);
            set
            {
                OperationDate_DB = value.Value;
                OnPropertyChanged(nameof(OperationCode));
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
        public byte? DocumentVid_DB { get; set; } = 0;
        [NotMapped]
        [Attributes.Form_Property("Дата операции")]
        public RamAccess<byte?> DocumentVid
        {
            get => new RamAccess<byte?>(DocumentVid_Validation, DocumentVid_DB);
            set
            {
                DocumentVid_DB = value.Value;
                OnPropertyChanged(nameof(DocumentVid));
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
            List<Tuple<byte, string>> spr = new List<Tuple<byte, string>>
            {
                new Tuple<byte, string>(0,""),
                new Tuple<byte, string>(1,""),
                new Tuple<byte, string>(2,""),
                new Tuple<byte, string>(3,""),
                new Tuple<byte, string>(4,""),
                new Tuple<byte, string>(5,""),
                new Tuple<byte, string>(6,""),
                new Tuple<byte, string>(7,""),
                new Tuple<byte, string>(8,""),
                new Tuple<byte, string>(9,""),
                new Tuple<byte, string>(10,""),
                new Tuple<byte, string>(11,""),
                new Tuple<byte, string>(12,""),
                new Tuple<byte, string>(13,""),
                new Tuple<byte, string>(14,""),
                new Tuple<byte, string>(15,""),
                new Tuple<byte, string>(19,"")
            };   //HERE BINDS SPRAVOCHNICK
            foreach (Tuple<byte, string> item in spr)
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
        public int? DocumentNumber_DB { get; set; } = 0;
        [NotMapped]
        [Attributes.Form_Property("Дата операции")]
        public RamAccess<int?> DocumentNumber
        {
            get => new RamAccess<int?>(DocumentNumber_Validation, DocumentNumber_DB);
            set
            {
                DocumentNumber_DB = value.Value;
                OnPropertyChanged(nameof(DocumentNumber));
            }
        }
        protected virtual bool DocumentNumber_Validation(RamAccess<int?> value)//Ready
        { return true; }
        #endregion

        #region DocumentDate
        public string DocumentDate_DB { get; set; } = 0;
        [NotMapped]
        [Attributes.Form_Property("Дата операции")]
        public RamAccess<string> DocumentDate
        {
            get => new RamAccess<string>(DocumentDate_Validation, DocumentDate_DB);
            set
            {
                DocumentDate_DB = value.Value;
                OnPropertyChanged(nameof(DocumentDate));
            }
        }
        protected bool DocumentDate_Validation(RamAccess<string> value)//Ready
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
            bool ab = (OperationCode.Value >= 11) && (OperationCode.Value <= 18);
            bool b = (OperationCode.Value >= 41) && (OperationCode.Value <= 49);
            bool c = (OperationCode.Value >= 51) && (OperationCode.Value <= 59);
            bool d = (OperationCode.Value == 65) || (OperationCode.Value == 68);
            if (ab || b || c || d)
            {
                if (!value.Value.Equals(OperationDate))
                {
                    value.AddError("Заполните примечание");//to do note handling
                }
            }
            return true;
        }
        #endregion
    }
}
