using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Client_Model
{
    public abstract class Form1:Form
    {
        [Attributes.FormVisual("Форма")]
        public abstract string FormNum { get;}
        public abstract int NumberOfFields { get; }

        //NumberInOrder property
        [Attributes.FormVisual("№ п/п")]
        public int NumberInOrder
        {
            get
            {
                if (GetErrors(nameof(NumberInOrder)) != null)
                {
                    return (int)_NumberInOrder.Get();
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
                    _NumberInOrder.Set(_NumberInOrder_Not_Valid);
                }
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        private IDataLoadEngine _NumberInOrder;
        private int _NumberInOrder_Not_Valid = -1;
        private void NumberInOrder_Validation()
        {
            ClearErrors(nameof(NumberInOrder));
        }
        //NumberInOrder property

        //CorrectionNumber property
        [Attributes.FormVisual("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    return (byte)_CorrectionNumber.Get();
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
                    _CorrectionNumber.Set(_CorrectionNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private IDataLoadEngine _CorrectionNumber;
        private byte _CorrectionNumber_Not_Valid = 255;
        private void CorrectionNumber_Validation()
        {
            ClearErrors(nameof(CorrectionNumber));
        }
        //CorrectionNumber property

        //OperationCode property
        [Attributes.FormVisual("Код")]
        public short OperationCode
        {
            get
            {
                if (GetErrors(nameof(OperationCode)) != null)
                {
                    return (short)_OperationCode.Get();
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
                    _OperationCode.Set(_OperationCode_Not_Valid);
                }
                OnPropertyChanged(nameof(OperationCode));
            }
        }
        private IDataLoadEngine _OperationCode;
        private short _OperationCode_Not_Valid = -1;
        private void OperationCode_Validation()
        {
            ClearErrors(nameof(OperationCode));
        }
        //OprationCode property

        //OperationDate property
        [Attributes.FormVisual("Дата операции")]
        public DateTime OperationDate
        {
            get
            {
                if (GetErrors(nameof(OperationDate)) != null)
                {
                    return (DateTime)_OperationDate.Get();
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
                    _OperationDate.Set(_OperationDate_Not_Valid);
                }
                OnPropertyChanged(nameof(OperationDate));
            }
        }
        private IDataLoadEngine _OperationDate;
        private DateTime _OperationDate_Not_Valid = DateTime.MinValue;
        private void OperationDate_Validation()
        {
            ClearErrors(nameof(OperationDate));
        }
        //OperationDate property

        //DocumentVid property
        [Attributes.FormVisual("Вид документа")]
        public byte DocumentVid
        {
            get
            {
                if (GetErrors(nameof(DocumentVid)) != null)
                {
                    return (byte)_DocumentVid.Get();
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
                    _DocumentVid.Set(_DocumentVid_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentVid));
            }
        }
        private IDataLoadEngine _DocumentVid;
        private byte _DocumentVid_Not_Valid = 255;
        private void DocumentVid_Validation(byte value)//TODO
        {
            ClearErrors(nameof(DocumentVid));
        }
        //DocumentVid property

        //DocumentNumber property
        [Attributes.FormVisual("Номер документа")]
        public string DocumentNumber
        {
            get
            {
                if (GetErrors(nameof(DocumentNumber)) != null)
                {
                    return (string)_DocumentNumber.Get();
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
                    _DocumentNumber.Set(_DocumentNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentNumber));
            }
        }
        private IDataLoadEngine _DocumentNumber;
        private string _DocumentNumber_Not_Valid = "";
        private void DocumentNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(DocumentNumber));
        }
        //DocumentNumber property

        private string _documentNumberRecoded = "";
        public string DocumentNumberRecoded
        {
            get { return _documentNumberRecoded; }
            set
            {
                _documentNumberRecoded = value;
                OnPropertyChanged("DocumentNumberRecoded");
            }
        }

        //DocumentDate property
        [Attributes.FormVisual("Дата документа")]
        public DateTime DocumentDate
        {
            get
            {
                if (GetErrors(nameof(DocumentDate)) != null)
                {
                    return (DateTime)_DocumentDate.Get();
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
                    _DocumentDate.Set(_DocumentDate_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentDate));
            }
        }
        private IDataLoadEngine _DocumentDate;//if change this change validation
        private DateTime _DocumentDate_Not_Valid = DateTime.MinValue;
        private void DocumentDate_Validation(DateTime value)//Ready
        {
            ClearErrors(nameof(DocumentDate));
        }
        //DocumentDate property
    }
}
