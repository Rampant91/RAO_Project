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

        private DateTime _notificationDate = DateTime.MinValue;
        public DateTime NotificationDate
        {
            get { return _notificationDate; }
            set
            {
                _notificationDate = value;
                OnPropertyChanged("NotificationDate");
            }
        }
    }
}
