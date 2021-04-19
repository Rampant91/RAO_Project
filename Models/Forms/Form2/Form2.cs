using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Abstracts
{
    public abstract class Form2:Form
    {
        [Attributes.Form_Property("Форма")]
        protected static string SQLCommandParamsBase()
        {
            string strNotNullDeclaration = " varchar(255) not null, ";
            string intNotNullDeclaration = " int not null, ";
            string shortNotNullDeclaration = " smallint not null, ";
            string byteNotNullDeclaration = " tinyint not null, ";
            string dateNotNullDeclaration = " ????, ";
            return
                nameof(NumberInOrder) + intNotNullDeclaration +
                nameof(CorrectionNumber) + byteNotNullDeclaration;
        }
        public Form2(IDataAccess Access) : base(Access)
        {

        }
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
    }
}
