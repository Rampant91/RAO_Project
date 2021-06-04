using Models.DataAccess;

namespace Models.Abstracts
{
    public abstract class Form5 : Form
    {
        [Attributes.Form_Property("Форма")]
        public Form5() : base()
        {

        }

        //NumberInOrder property
        [Attributes.Form_Property("№ п/п")]
        public RamAccess<int> NumberInOrder
        {
            get
            {
                    return _dataAccess.Get<int>(nameof(NumberInOrder));
            }
            set
            {
                    _dataAccess.Set(nameof(NumberInOrder), value);
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }

        private int _NumberInOrder_Not_Valid = -1;
        private void NumberInOrder_Validation(RamAccess<int> value)
        {
            value.ClearErrors();
        }
        //NumberInOrder property

        //CorrectionNumber property
        [Attributes.Form_Property("Номер корректировки")]
        public RamAccess<byte> CorrectionNumber
        {
            get
            {
                    return _dataAccess.Get<byte>(nameof(CorrectionNumber));
            }
            set
            {
                    _dataAccess.Set(nameof(CorrectionNumber), value);
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }

        private byte _CorrectionNumber_Not_Valid = 255;
        private void CorrectionNumber_Validation(RamAccess<byte> value)
        {
            value.ClearErrors();
        }
        //CorrectionNumber property
    }
}
