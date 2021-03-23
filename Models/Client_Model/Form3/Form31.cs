using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 3.1: Отчет о намерении экспортировать радиоактивные источники 1 и 2 категории")]
    public class Form31 : Form3
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 19;

        [Attributes.FormVisual("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    return _correctionNumber;
                }
                else
                {
                    return _correctionNumber_Not_Valid;
                }
            }
            set
            {
                _correctionNumber_Not_Valid = value;
                if (CorrectionNumber_Validation())
                {
                    _correctionNumber = _correctionNumber_Not_Valid;
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private byte _correctionNumber = 255;
        private byte _correctionNumber_Not_Valid = 255;
        private bool CorrectionNumber_Validation()
        {
            return true;
            //ClearErrors(nameof(CorrectionNumber));
            ////Пример
            //if (value < 10)
            //    AddError(nameof(CorrectionNumber), "Значение должно быть больше 10.");
        }

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

        private string _recieverName = "";
        public string RecieverName
        {
            get { return _recieverName; }
            set
            {
                _recieverName = value;
                OnPropertyChanged("RecieverName");
            }
        }

        private string _recieverAddress = "";
        public string RecieverAddress
        {
            get { return _recieverAddress; }
            set
            {
                _recieverAddress = value;
                OnPropertyChanged("RecieverAddress");
            }
        }

        private string _recieverFactAddress = "";
        public string RecieverFactAddress
        {
            get { return _recieverFactAddress; }
            set
            {
                _recieverFactAddress = value;
                OnPropertyChanged("RecieverFactAddress");
            }
        }

        private string _licenseId="";
        public string LicenseId
        {
            get { return _licenseId; }
            set
            {
                _licenseId = value;
                OnPropertyChanged("LicenseId");
            }
        }

        private DateTime _validThru = DateTime.MinValue;
        public DateTime ValidThru
        {
            get { return _validThru; }
            set
            {
                _validThru = value;
                OnPropertyChanged("ValidThru");
            }
        }

        private DateTime _suggestedSolutionDate = DateTime.MinValue;
        public DateTime SuggestedSolutionDate
        {
            get { return _suggestedSolutionDate; }
            set
            {
                _suggestedSolutionDate = value;
                OnPropertyChanged("SuggestedSolutionDate");
            }
        }

        private string _userName = "";
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        private string _userAddress = "";
        public string UserAddress
        {
            get { return _userAddress; }
            set
            {
                _userAddress = value;
                OnPropertyChanged("UserAddress");
            }
        }

        private string _userFactAddress = "";
        public string UserFactAddress
        {
            get { return _userFactAddress; }
            set
            {
                _userFactAddress = value;
                OnPropertyChanged("UserFactAddress");
            }
        }

        private string _userTelephone = "";
        public string UserTelephone
        {
            get { return _userTelephone; }
            set
            {
                _userTelephone = value;
                OnPropertyChanged("UserTelephone");
            }
        }

        private string _userFax = "";
        public string UserFax
        {
            get { return _userFax; }
            set
            {
                _userFax = value;
                OnPropertyChanged("UserFax");
            }
        }

        private string _email = "";
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        private string _zriUsageScope = "";
        public string ZriUsageScope
        {
            get { return _zriUsageScope; }
            set
            {
                _zriUsageScope = value;
                OnPropertyChanged("ZriUsageScope");
            }
        }

        private string _contractId = "";
        public string ContractId
        {
            get { return _contractId; }
            set
            {
                _contractId = value;
                OnPropertyChanged("ContractId");
            }
        }

        private DateTime _contractDate = DateTime.MinValue;
        public DateTime ContractDate
        {
            get { return _contractDate; }
            set
            {
                _contractDate = value;
                OnPropertyChanged("ContractDate");
            }
        }

        private string _countryCreator = "";
        public string CountryCreator
        {
            get { return _countryCreator; }
            set
            {
                _countryCreator = value;
                OnPropertyChanged("CountryCreator");
            }
        }

        private List<Form31_1> _zriList = new List<Form31_1>();
        public List<Form31_1> ZriList
        {
            get { return _zriList; }
            set
            {
                _zriList = value;
                OnPropertyChanged("ZriList");
            }
        }
    }
}
