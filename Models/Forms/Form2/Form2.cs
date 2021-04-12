using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Abstracts
{
    public abstract class Form2:Form
    {
        [Attributes.Form_Property("Форма")]

        public Form2(int RowID):base(RowID)
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

        //NumberInOrder property
        [Attributes.Form_Property("№ п/п")]
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
    }
}
