using System;
using System.Collections.Generic;
using System.Text;
using DBRealization;

namespace Models.Abstracts
{
    public abstract class Form3:Form
    {
        [Attributes.Form_Property("Форма")]
        //protected static string SQLCommandParamsBase()
        //{
        //    return
        //        nameof(CorrectionNumber) + SQLconsts.shortNotNullDeclaration +
        //        nameof(NotificationDate) + " datetimeoffset not null";
        //}
        public Form3(IDataAccess Access) : base(Access)
        {
        }

        //CorrectionNumber property
        [Attributes.Form_Property("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    return (byte)_dataAccess.Get(nameof(CorrectionNumber));
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
                if (GetErrors(nameof(NotificationDate)) != null)
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
                if (GetErrors(nameof(NotificationDate)) != null)
                {
                    _dataAccess.Set(nameof(NotificationDate), _NotificationDate_Not_Valid);
                }
                OnPropertyChanged(nameof(NotificationDate));
            }
        }
        
        private DateTimeOffset _NotificationDate_Not_Valid = DateTimeOffset.MinValue;
        //NotificationDate property
    }
}
