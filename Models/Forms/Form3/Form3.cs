using Models.DataAccess;
using System;

namespace Models.Abstracts
{
    public abstract class Form3 : Form
    {
        [Attributes.Form_Property("Форма")]
        public Form3() : base()
        {
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

        
        private bool CorrectionNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //CorrectionNumber property

        //NotificationDate property
        [Attributes.Form_Property("Дата уведомления")]
        public RamAccess<string> NotificationDate
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(NotificationDate));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(NotificationDate), value);
                }
                OnPropertyChanged(nameof(NotificationDate));
            }
        }

        
        //NotificationDate property
    }
}
