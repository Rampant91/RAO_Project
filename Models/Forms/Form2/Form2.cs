using Models.DataAccess;
using System.ComponentModel;

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

        protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }

        private void Init_base()
        {
            DataAccess.Init<int>(nameof(NumberInOrder), NumberInOrder_Validation, -1);
            NumberInOrder.PropertyChanged += InPropertyChanged;
            //DataAccess.Init<string>(nameof(), _Validation, null);
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
                    return DataAccess.Get<byte>(nameof(CorrectionNumber));

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(CorrectionNumber), value);
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
                    return DataAccess.Get<int>(nameof(NumberInOrder));

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(NumberInOrder), value);
                }
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }

        private bool NumberInOrder_Validation(RamAccess<int> value)
        {
            value.ClearErrors(); return true;
        }
        //NumberInOrder property
    }
}
