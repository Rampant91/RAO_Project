using Models.DataAccess;

namespace Models.Abstracts
{
    public abstract class Form2 : Form
    {
        [Attributes.Form_Property("Форма")]

        public Form2() : base()
        {

        }
        //CorrectionNumber property
        [Attributes.Form_Property("Номер корректировки")]
        public IDataAccess<byte> CorrectionNumber
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

                private void CorrectionNumber_Validation(IDataAccess<byte> value)
        {
            value.ClearErrors();
        }
        //CorrectionNumber property

        //NumberInOrder property
        [Attributes.Form_Property("№ п/п")]
        public IDataAccess<int> NumberInOrder
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

                private void NumberInOrder_Validation(IDataAccess<int> value)
        {
            value.ClearErrors();
        }
        //NumberInOrder property
    }
}
