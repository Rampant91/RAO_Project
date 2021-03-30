using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Client_Model
{
    public abstract class Form3:Form
    {
        [Attributes.FormVisual("Форма")]
        public abstract string FormNum { get; }
        public abstract int NumberOfFields { get; }

        //CorrectionNumber property
        [Attributes.FormVisual("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    return (byte)_CorrectionNumber.Get();
                }
                else
                {
                    return _CorrectionNumber_Not_Valid;
                }
            }
            set
            {
                _CorrectionNumber_Not_Valid = value;
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    _CorrectionNumber.Set(_CorrectionNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private IDataLoadEngine _CorrectionNumber;
        private byte _CorrectionNumber_Not_Valid = 255;
        private void CorrectionNumber_Validation()
        {
            ClearErrors(nameof(CorrectionNumber));
        }
        //CorrectionNumber property

        //NotificationDate property
        [Attributes.FormVisual("Дата уведомления")]
        public DateTime NotificationDate
        {
            get
            {
                if (GetErrors(nameof(NotificationDate)) != null)
                {
                    return (DateTime)_NotificationDate.Get();
                }
                else
                {
                    return _NotificationDate_Not_Valid;
                }
            }
            set
            {
                _NotificationDate_Not_Valid = value;
                if (GetErrors(nameof(NotificationDate)) != null)
                {
                    _NotificationDate.Set(_NotificationDate_Not_Valid);
                }
                OnPropertyChanged(nameof(NotificationDate));
            }
        }
        private IDataLoadEngine _NotificationDate;
        private DateTime _NotificationDate_Not_Valid = DateTime.MinValue;
        //NotificationDate property
    }
}
