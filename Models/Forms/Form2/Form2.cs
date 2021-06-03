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
        public byte CorrectionNumber
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(CorrectionNumber));
                    return tmp != null ? (byte)tmp : (byte)0;
                }
                else
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

                private void CorrectionNumber_Validation()
        {
            value.ClearErrors();
        }
        //CorrectionNumber property

        //NumberInOrder property
        [Attributes.Form_Property("№ п/п")]
        public int NumberInOrder
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(NumberInOrder));
                    return tmp != null ? (int)tmp : -1;
                }
                else
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

                private void NumberInOrder_Validation()
        {
            value.ClearErrors();
        }
        //NumberInOrder property
    }
}
