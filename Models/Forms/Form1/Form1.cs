using Models.DataAccess;
using System;

namespace Models.Abstracts
{
    public abstract class Form1 : Form
    {
        [Attributes.Form_Property("Форма")]
        public Form1() : base()
        {

        }

        //NumberInOrder property
        [Attributes.Form_Property("№ п/п")]
        public int NumberInOrder
        {
            get
            {
                if (GetErrors(nameof(NumberInOrder)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(NumberInOrder));
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _NumberInOrder_Not_Valid;
                }
            }
            set
            {
                _NumberInOrder_Not_Valid = value;
                if (GetErrors(nameof(NumberInOrder)) == null)
                {
                    _dataAccess.Set(nameof(NumberInOrder), _NumberInOrder_Not_Valid);
                }
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        private int _NumberInOrder_Not_Valid = -1;
        //private void NumberInOrder_Validation()
        //{
        //    ClearErrors(nameof(NumberInOrder));
        //}
        //NumberInOrder property

        //CorrectionNumber property
        [Attributes.Form_Property("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(CorrectionNumber));
                    return tmp != null ? (byte)tmp : (byte)0;
                }
                else
                {
                    return _CorrectionNumber_Not_Valid;
                }
            }
            set
            {
                _CorrectionNumber_Not_Valid = value;
                if (GetErrors(nameof(CorrectionNumber)) == null)
                {
                    _dataAccess.Set(nameof(CorrectionNumber), _CorrectionNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }

        private byte _CorrectionNumber_Not_Valid = 255;
        //private void CorrectionNumber_Validation()
        //{
        //    ClearErrors(nameof(CorrectionNumber));
        //}
        //CorrectionNumber property

        //OperationCode property
        [Attributes.Form_Property("Код")]
        public short OperationCode
        {
            get
            {
                if (GetErrors(nameof(OperationCode)) == null)
                {
                    string tmp = (string)_dataAccess.Get(nameof(OperationCode));
                    return tmp != null ? short.Parse(tmp) : (short)-1;
                }
                else
                {
                    return _OperationCode_Not_Valid;
                }
            }
            set
            {
                var tmp1 = value.ToString();
                if (tmp1.Length == 1) tmp1 = "0" + tmp1;

                OperationCode_Validation(tmp1);
                //_OperationCode_Not_Valid = value;

                if (GetErrors(nameof(OperationCode)) == null)
                {
                    var tmp = _OperationCode_Not_Valid.ToString();
                    if (tmp.Length == 1) tmp = "0" + tmp;
                    _dataAccess.Set(nameof(OperationCode), tmp);
                }
                OnPropertyChanged(nameof(OperationCode));
            }
        }
        protected virtual void OperationCode_Validation(string arg) { }

        protected short _OperationCode_Not_Valid = -1;
        //OprationCode property

        //OperationDate property
        [Attributes.Form_Property("Дата операции")]
        public DateTimeOffset OperationDate
        {
            get
            {
                if (GetErrors(nameof(OperationDate)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(OperationDate));
                    return tmp != null ? (DateTimeOffset)tmp : DateTimeOffset.Parse("01/01/1921");
                }
                else
                {
                    return _OperationDate_Not_Valid;
                }
            }
            set
            {
                _OperationDate_Not_Valid = value;
                if (GetErrors(nameof(OperationDate)) == null)
                {
                    _dataAccess.Set(nameof(OperationDate), _OperationDate_Not_Valid);
                }
                OnPropertyChanged(nameof(OperationDate));
            }
        }

        private DateTimeOffset _OperationDate_Not_Valid = DateTimeOffset.Parse("01/01/1921");
        private void OperationDate_Validation()
        {
            ClearErrors(nameof(OperationDate));
        }
        //OperationDate property

        //DocumentVid property
        [Attributes.Form_Property("Вид документа")]
        public byte DocumentVid
        {
            get
            {
                if (GetErrors(nameof(DocumentVid)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(DocumentVid));//Ok
                    return tmp != null ? (byte)tmp : (byte)0;
                }
                else
                {
                    return _DocumentVid_Not_Valid;
                }
            }
            set
            {
                DocumentVid_Validation(value);

                if (GetErrors(nameof(DocumentVid)) == null)
                {
                    _dataAccess.Set(nameof(DocumentVid), _DocumentVid_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentVid));
            }
        }

        private byte _DocumentVid_Not_Valid = 255;
        protected virtual void DocumentVid_Validation(byte value)//Ok
        {
            ClearErrors(nameof(DocumentVid));
        }
        //DocumentVid property

        //DocumentNumber property
        [Attributes.Form_Property("Номер документа")]
        public string DocumentNumber
        {
            get
            {
                if (GetErrors(nameof(DocumentNumber)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(DocumentNumber));//Ok
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _DocumentNumber_Not_Valid;
                }
            }
            set
            {
                DocumentNumber_Validation(value);

                if (GetErrors(nameof(DocumentNumber)) == null)
                {
                    _dataAccess.Set(nameof(DocumentNumber), _DocumentNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentNumber));
            }
        }

        private string _DocumentNumber_Not_Valid = "";
        private void DocumentNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(DocumentNumber));
            if ((value == null) || value=="")//ok
            {
                AddError(nameof(DocumentNumber), "Поле не заполнено");
                return;
            }
        }
        //DocumentNumber property

        //DocumentNumberRecoded property
        public string DocumentNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(DocumentNumberRecoded)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(DocumentNumberRecoded));//ok
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _DocumentNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _DocumentNumberRecoded_Not_Valid = value;
                if (GetErrors(nameof(DocumentNumberRecoded)) == null)
                {
                    _dataAccess.Set(nameof(DocumentNumberRecoded), _DocumentNumberRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentNumberRecoded));
            }
        }

        private string _DocumentNumberRecoded_Not_Valid = "";
        //private void DocumentNumberRecoded_Validation(string value)//Ready
        //{
        //    ClearErrors(nameof(DocumentNumberRecoded));
        //}
        //DocumentNumberRecoded property

        //DocumentDate property
        [Attributes.Form_Property("Дата документа")]
        public DateTimeOffset DocumentDate
        {
            get
            {
                if (GetErrors(nameof(DocumentDate)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(DocumentDate));//OK
                    return tmp != null ? (DateTimeOffset)tmp : DateTimeOffset.MinValue;
                }
                else
                {
                    return _DocumentDate_Not_Valid;
                }
            }
            set
            {
                _DocumentDate_Not_Valid = value;
                if (GetErrors(nameof(DocumentDate)) == null)
                {
                    _dataAccess.Set(nameof(DocumentDate), _DocumentDate_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentDate));
            }
        }
        //if change this change validation
        private DateTimeOffset _DocumentDate_Not_Valid = DateTimeOffset.Parse("01/01/1921");
        //DocumentDate property

        //DocumentDateNote property
        [Attributes.Form_Property("Дата документа")]
        public string DocumentDateNote
        {
            get
            {
                if (GetErrors(nameof(DocumentDateNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(DocumentDateNote));
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _DocumentDateNote_Not_Valid;
                }
            }
            set
            {
                _DocumentDateNote_Not_Valid = value;
                if (GetErrors(nameof(DocumentDateNote)) == null)
                {
                    _dataAccess.Set(nameof(DocumentDateNote), _DocumentDateNote_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentDateNote));
            }
        }
        //if change this change validation
        private string _DocumentDateNote_Not_Valid = "-";

        private void DocumentDateNote_Validation(string value)
        {
            ClearErrors(nameof(DocumentDate));
        }
        //DocumentDateNote property
    }
}
