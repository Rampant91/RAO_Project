using Models.DataAccess;

namespace Models.Abstracts
{
    public abstract class Form2 : Form
    {
        [Attributes.Form_Property("Форма")]

        public Form2() : base()
        {
            Init_base();
            Validate_base();
        }

        private void Init_base()
        {
            _dataAccess.Init<int>(nameof(NumberInOrder), NumberInOrder_Validation, -1);
            //_dataAccess.Init<string>(nameof(), _Validation, null);
        }
        protected void Validate_base()
        {
            NumberInOrder_Validation(NumberInOrder);
        }

        //CorrectionNumber property
        [Attributes.Form_Property("Номер корректировки")]
        public RamAccess<byte> CorrectionNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<byte>(nameof(CorrectionNumber));
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(CorrectionNumber), value);
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }

        private bool CorrectionNumber_Validation(RamAccess<byte> value)
        {
            value.ClearErrors(); return true;
        }
        //CorrectionNumber property

        //NumberInOrder property
        [Attributes.Form_Property("№ п/п")]
        public RamAccess<int> NumberInOrder
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(NumberInOrder));
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(NumberInOrder), value);
                }
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }

                private bool NumberInOrder_Validation(RamAccess<int> value)
        {
            value.ClearErrors(); return true;}
        //NumberInOrder property
    }
}
