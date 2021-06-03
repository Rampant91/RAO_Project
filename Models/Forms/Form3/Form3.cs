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
                    _dataAccess.Set(nameof(CorrectionNumber), _CorrectionNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }

        
        private void CorrectionNumber_Validation()
        {
            value.ClearErrors();
        }
        //CorrectionNumber property

        //NotificationDate property
        [Attributes.Form_Property("Дата уведомления")]
        public DateTimeOffset NotificationDate
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(NotificationDate));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(NotificationDate), _NotificationDate_Not_Valid);
                }
                OnPropertyChanged(nameof(NotificationDate));
            }
        }

        
        //NotificationDate property
    }
}
