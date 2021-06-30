using Models.DataAccess;

namespace Models.Abstracts
{
    public abstract class Form3 : Form
    {
        [Attributes.Form_Property("Форма")]
        public Form3() : base()
        {
        }

        //CorrectionNumber property
        public int? CorrectionNumberId { get; set; }
        [Attributes.Form_Property("Номер корректировки")]
        public virtual RamAccess<byte> CorrectionNumber
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


        private bool CorrectionNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //CorrectionNumber property

        //NotificationDate property
        public int? NotificationDateId { get; set; }
        [Attributes.Form_Property("Дата уведомления")]
        public virtual RamAccess<string> NotificationDate
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(NotificationDate));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(NotificationDate), value);
                }
                OnPropertyChanged(nameof(NotificationDate));
            }
        }


        //NotificationDate property
    }
}
