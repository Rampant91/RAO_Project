using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Models.Abstracts
{
    public abstract class Form2 : Form
    {
        [Attributes.Form_Property("Форма")]
        public Form2()
        {

        }

        protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }

        #region CorrectionNumber

        public byte CorrectionNumber_DB { get; set; } = 0;

        [NotMapped]
        [Attributes.Form_Property("Номер корректировки")]
        public RamAccess<byte> CorrectionNumber
        {
            get
            {
                var tmp = new RamAccess<byte>(CorrectionNumber_Validation, CorrectionNumber_DB);
                tmp.PropertyChanged += CorrectionNumberValueChanged;
                return tmp;
            }
            set
            {
                CorrectionNumber_DB = value.Value;
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }

        private void CorrectionNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                CorrectionNumber_DB = ((RamAccess<byte>) Value).Value;
            }
        }

        private bool CorrectionNumber_Validation(RamAccess<byte> value)
        {
            value.ClearErrors();
            return true;
        }

        //CorrectionNumber property

        #endregion

        #region NumberInOrder

        public int NumberInOrder_DB { get; set; } = 0;

        [NotMapped]
        [Attributes.Form_Property("№ п/п")]
        public RamAccess<int> NumberInOrder
        {
            get
            {
                var tmp = new RamAccess<int>(NumberInOrder_Validation, NumberInOrder_DB);
                tmp.PropertyChanged += NumberInOrderValueChanged;
                return tmp;
            }
            set
            {
                NumberInOrder_DB = value.Value;
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }

        private void NumberInOrderValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                NumberInOrder_DB = ((RamAccess<int>) Value).Value;
            }
        }

        private bool NumberInOrder_Validation(RamAccess<int> value)
        {
            value.ClearErrors();
            return true;
        }

        //NumberInOrder property

        #endregion
    }
}
