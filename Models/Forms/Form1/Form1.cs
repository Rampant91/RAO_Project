using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Abstracts
{
    public abstract class Form1:Form
    {
        [Attributes.Form_Property("Форма")]
        public Form1(IDataAccess Access) : base(Access)
        {

        }

        protected static string SQLCommandParamsBase()
        {
            string strNotNullDeclaration = " varchar(255) not null, ";
            string intNotNullDeclaration = " int not null, ";
            string shortNotNullDeclaration = " smallint not null, ";
            string byteNotNullDeclaration = " tinyint not null, ";
            //string dateNotNullDeclaration = " ????, ";
            return
                nameof(NumberInOrder) + intNotNullDeclaration +
                nameof(CorrectionNumber) + byteNotNullDeclaration +
                nameof(OperationCode) + shortNotNullDeclaration +
                //nameof(OperationDate) + dateNotNullDeclaration +
                nameof(DocumentVid) + byteNotNullDeclaration +
                nameof(DocumentNumber) + strNotNullDeclaration +
                nameof(DocumentNumberRecoded) + strNotNullDeclaration;

                //nameof(DocumentDate) + dateNotNullDeclaration;
        }

            //NumberInOrder property
        [Attributes.Form_Property("№ п/п")]
        public int NumberInOrder
        {
            get
            {
                if (GetErrors(nameof(NumberInOrder)) != null)
                {
                    return (int)_dataAccess.Get(nameof(NumberInOrder));
                }
                else
                {
                    return _NumberInOrder_Not_Valid;
                }
            }
            set
            {
                _NumberInOrder_Not_Valid = value;
                if (GetErrors(nameof(NumberInOrder)) != null)
                {
                    _dataAccess.Set(nameof(NumberInOrder), _NumberInOrder_Not_Valid);
                }
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        private int _NumberInOrder_Not_Valid = -1;
        private void NumberInOrder_Validation()
        {
            ClearErrors(nameof(NumberInOrder));
        }
        //NumberInOrder property

        //CorrectionNumber property
        [Attributes.Form_Property("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    return (byte)_dataAccess.Get(nameof(CorrectionNumber));
                }
                else
                {
                    return _CorrectionNumber_Not_Valid;
                }
            }
            set
            {
                _CorrectionNumber_Not_Valid = value;
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    _dataAccess.Set(nameof(CorrectionNumber), _CorrectionNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        
        private byte _CorrectionNumber_Not_Valid = 255;
        private void CorrectionNumber_Validation()
        {
            ClearErrors(nameof(CorrectionNumber));
        }
        //CorrectionNumber property

        //OperationCode property
        [Attributes.Form_Property("Код")]
        public short OperationCode
        {
            get
            {
                if (GetErrors(nameof(OperationCode)) != null)
                {
                    return (short)_dataAccess.Get(nameof(OperationCode));
                }
                else
                {
                    return _OperationCode_Not_Valid;
                }
            }
            set
            {
                _OperationCode_Not_Valid = value;
                if (GetErrors(nameof(OperationCode)) != null)
                {
                    _dataAccess.Set(nameof(OperationCode), _OperationCode_Not_Valid);
                }
                OnPropertyChanged(nameof(OperationCode));
            }
        }
        
        private short _OperationCode_Not_Valid = -1;
        private void OperationCode_Validation()
        {
            ClearErrors(nameof(OperationCode));
        }
        //OprationCode property

        //OperationDate property
        [Attributes.Form_Property("Дата операции")]
        public DateTimeOffset OperationDate
        {
            get
            {
                if (GetErrors(nameof(OperationDate)) != null)
                {
                    return (DateTimeOffset)_dataAccess.Get(nameof(OperationDate));
                }
                else
                {
                    return _OperationDate_Not_Valid;
                }
            }
            set
            {
                _OperationDate_Not_Valid = value;
                if (GetErrors(nameof(OperationDate)) != null)
                {
                    _dataAccess.Set(nameof(OperationDate), _OperationDate_Not_Valid);
                }
                OnPropertyChanged(nameof(OperationDate));
            }
        }
        
        private DateTimeOffset _OperationDate_Not_Valid = DateTimeOffset.MinValue;
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
                if (GetErrors(nameof(DocumentVid)) != null)
                {
                    return (byte)_dataAccess.Get(nameof(DocumentVid));
                }
                else
                {
                    return _DocumentVid_Not_Valid;
                }
            }
            set
            {
                _DocumentVid_Not_Valid = value;
                if (GetErrors(nameof(DocumentVid)) != null)
                {
                    _dataAccess.Set(nameof(DocumentVid), _DocumentVid_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentVid));
            }
        }
        
        private byte _DocumentVid_Not_Valid = 255;
        private void DocumentVid_Validation(byte value)//TODO
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
                if (GetErrors(nameof(DocumentNumber)) != null)
                {
                    return (string)_dataAccess.Get(nameof(DocumentNumber));
                }
                else
                {
                    return _DocumentNumber_Not_Valid;
                }
            }
            set
            {
                _DocumentNumber_Not_Valid = value;
                if (GetErrors(nameof(DocumentNumber)) != null)
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
        }
        //DocumentNumber property

        //DocumentNumberRecoded property
        public string DocumentNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(DocumentNumberRecoded)) != null)
                {
                    return (string)_dataAccess.Get(nameof(DocumentNumberRecoded));
                }
                else
                {
                    return _DocumentNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _DocumentNumberRecoded_Not_Valid = value;
                if (GetErrors(nameof(DocumentNumberRecoded)) != null)
                {
                    _dataAccess.Set(nameof(DocumentNumberRecoded), _DocumentNumberRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentNumberRecoded));
            }
        }
        
        private string _DocumentNumberRecoded_Not_Valid = "";
        private void DocumentNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(DocumentNumberRecoded));
        }
        //DocumentNumberRecoded property

        //DocumentDate property
        [Attributes.Form_Property("Дата документа")]
        public DateTimeOffset DocumentDate
        {
            get
            {
                if (GetErrors(nameof(DocumentDate)) != null)
                {
                    return (DateTimeOffset)_dataAccess.Get(nameof(DocumentDate));
                }
                else
                {
                    return _DocumentDate_Not_Valid;
                }
            }
            set
            {
                _DocumentDate_Not_Valid = value;
                if (GetErrors(nameof(DocumentDate)) != null)
                {
                    _dataAccess.Set(nameof(DocumentDate), _DocumentDate_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentDate));
            }
        }
        //if change this change validation
        private DateTimeOffset _DocumentDate_Not_Valid = DateTimeOffset.MinValue;
        private void DocumentDate_Validation(DateTimeOffset value)//Ready
        {
            ClearErrors(nameof(DocumentDate));
        }
        //DocumentDate property
    }
}
