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
                if (GetErrors(nameof(CorrectionNumber)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(CorrectionNumber));
                    return tmp != null ? (byte)tmp : (byte)0;
                }
                else
                {
                    return _CorrectionNumber_Not_Valid;
                }
            }
            set
            {
                _CorrectionNumber_Not_Valid = value;
                if (GetErrors(nameof(CorrectionNumber)) == null)
                {
                    _dataAccess.Set(nameof(CorrectionNumber), _CorrectionNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }

        private byte _CorrectionNumber_Not_Valid = 255;
        private void CorrectionNumber_Validation()
        {
            ClearErrors(nameof(CorrectionNumber));
        }
        //CorrectionNumber property

        //NotificationDate property
        [Attributes.Form_Property("Дата уведомления")]
        public DateTimeOffset NotificationDate
        {
            get
            {
                if (GetErrors(nameof(NotificationDate)) == null)
                {
                    return (DateTimeOffset)_dataAccess.Get(nameof(NotificationDate));
                }
                else
                {
                    return _NotificationDate_Not_Valid;
                }
            }
            set
            {
                _NotificationDate_Not_Valid = value;
                if (GetErrors(nameof(NotificationDate)) == null)
                {
                    _dataAccess.Set(nameof(NotificationDate), _NotificationDate_Not_Valid);
                }
                OnPropertyChanged(nameof(NotificationDate));
            }
        }

        private DateTimeOffset _NotificationDate_Not_Valid = DateTimeOffset.Parse("01/01/1753");
        //NotificationDate property
    }
}
