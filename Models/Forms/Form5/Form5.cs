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
        public int? NumberInOrderId { get; set; }
        [Attributes.Form_Property("№ п/п")]
        public virtual RamAccess<int> NumberInOrder
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
        private bool NumberInOrder_Validation(RamAccess<int> value)
        {
            value.ClearErrors(); return true;}
        //NumberInOrder property

        //CorrectionNumber property
        public int? CorrectionNumberId { get; set; }
        [Attributes.Form_Property("Номер корректировки")]
        public virtual RamAccess<byte> CorrectionNumber
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

        private bool CorrectionNumber_Validation(RamAccess<byte> value)
        {
            value.ClearErrors(); return true;}
        //CorrectionNumber property
    }
}
