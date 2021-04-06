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
        public Form31() : base()
        {
            FormNum = "31";
            NumberOfFields = 19;
        }

        [Attributes.FormVisual("Форма")]

        public override void Object_Validation()
        {

        }

        //RecieverName property
        [Attributes.FormVisual("Получатель")]
        public string RecieverName
        {
            get
            {
                if (GetErrors(nameof(RecieverName)) != null)
                {
                    return (string)_RecieverName.Get();
                }
                else
                {
                    return _RecieverName_Not_Valid;
                }
            }
            set
            {
                _RecieverName_Not_Valid = value;
                if (GetErrors(nameof(RecieverName)) != null)
                {
                    _RecieverName.Set(_RecieverName_Not_Valid);
                }
                OnPropertyChanged(nameof(RecieverName));
            }
        }
        private IDataLoadEngine _RecieverName;
        private string _RecieverName_Not_Valid = "";
        //RecieverName property

        //RecieverAddress property
        [Attributes.FormVisual("Адрес получателя")]
        public string RecieverAddress
        {
            get
            {
                if (GetErrors(nameof(RecieverAddress)) != null)
                {
                    return (string)_RecieverAddress.Get();
                }
                else
                {
                    return _RecieverAddress_Not_Valid;
                }
            }
            set
            {
                _RecieverAddress_Not_Valid = value;
                if (GetErrors(nameof(RecieverAddress)) != null)
                {
                    _RecieverAddress.Set(_RecieverAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(RecieverAddress));
            }
        }
        private IDataLoadEngine _RecieverAddress;
        private string _RecieverAddress_Not_Valid = "";
        //RecieverAddress property

        //RecieverFactAddress property
        [Attributes.FormVisual("Фактический адрес получателя")]
        public string RecieverFactAddress
        {
            get
            {
                if (GetErrors(nameof(RecieverFactAddress)) != null)
                {
                    return (string)_RecieverFactAddress.Get();
                }
                else
                {
                    return _RecieverFactAddress_Not_Valid;
                }
            }
            set
            {
                _RecieverFactAddress_Not_Valid = value;
                if (GetErrors(nameof(RecieverFactAddress)) != null)
                {
                    _RecieverFactAddress.Set(_RecieverFactAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(RecieverFactAddress));
            }
        }
        private IDataLoadEngine _RecieverFactAddress;
        private string _RecieverFactAddress_Not_Valid = "";
        //RecieverFactAddress property

        //LicenseId property
        [Attributes.FormVisual("Номер лицензии")]
        public string LicenseId
        {
            get
            {
                if (GetErrors(nameof(LicenseId)) != null)
                {
                    return (string)_LicenseId.Get();
                }
                else
                {
                    return _LicenseId_Not_Valid;
                }
            }
            set
            {
                _LicenseId_Not_Valid = value;
                if (GetErrors(nameof(LicenseId)) != null)
                {
                    _LicenseId.Set(_LicenseId_Not_Valid);
                }
                OnPropertyChanged(nameof(LicenseId));
            }
        }
        private IDataLoadEngine _LicenseId;
        private string _LicenseId_Not_Valid = "";
        //LicenseId property

        //ValidThru property
        [Attributes.FormVisual("Действует по")]
        public DateTimeOffset ValidThru
        {
            get
            {
                if (GetErrors(nameof(ValidThru)) != null)
                {
                    return (DateTime)_ValidThru.Get();
                }
                else
                {
                    return _ValidThru_Not_Valid;
                }
            }
            set
            {
                _ValidThru_Not_Valid = value;
                if (GetErrors(nameof(ValidThru)) != null)
                {
                    _ValidThru.Set(_ValidThru_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidThru));
            }
        }
        private IDataLoadEngine _ValidThru;
        private DateTimeOffset _ValidThru_Not_Valid = DateTimeOffset.MinValue;
        //ValidThru property

        //SuggestedSolutionDate property
        [Attributes.FormVisual("")]
        public DateTimeOffset SuggestedSolutionDate
        {
            get
            {
                if (GetErrors(nameof(SuggestedSolutionDate)) != null)
                {
                    return (DateTime)_SuggestedSolutionDate.Get();
                }
                else
                {
                    return _SuggestedSolutionDate_Not_Valid;
                }
            }
            set
            {
                _SuggestedSolutionDate_Not_Valid = value;
                if (GetErrors(nameof(SuggestedSolutionDate)) != null)
                {
                    _SuggestedSolutionDate.Set(_SuggestedSolutionDate_Not_Valid);
                }
                OnPropertyChanged(nameof(SuggestedSolutionDate));
            }
        }
        private IDataLoadEngine _SuggestedSolutionDate;
        private DateTimeOffset _SuggestedSolutionDate_Not_Valid = DateTimeOffset.MinValue;
        //SuggestedSolutionDate property

        //UserName property
        [Attributes.FormVisual("Наименование пользователя")]
        public string UserName
        {
            get
            {
                if (GetErrors(nameof(UserName)) != null)
                {
                    return (string)_UserName.Get();
                }
                else
                {
                    return _UserName_Not_Valid;
                }
            }
            set
            {
                _UserName_Not_Valid = value;
                if (GetErrors(nameof(UserName)) != null)
                {
                    _UserName.Set(_UserName_Not_Valid);
                }
                OnPropertyChanged(nameof(UserName));
            }
        }
        private IDataLoadEngine _UserName;
        private string _UserName_Not_Valid = "";
        //UserName property

        //UserAddress property
        [Attributes.FormVisual("Адрес")]
        public string UserAddress
        {
            get
            {
                if (GetErrors(nameof(UserAddress)) != null)
                {
                    return (string)_UserAddress.Get();
                }
                else
                {
                    return _UserAddress_Not_Valid;
                }
            }
            set
            {
                _UserAddress_Not_Valid = value;
                if (GetErrors(nameof(UserAddress)) != null)
                {
                    _UserAddress.Set(_UserAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(UserAddress));
            }
        }
        private IDataLoadEngine _UserAddress;
        private string _UserAddress_Not_Valid = "";
        //UserAddress property

        //UserFactAddress property
        [Attributes.FormVisual("Фактический адрес")]
        public string UserFactAddress
        {
            get
            {
                if (GetErrors(nameof(UserFactAddress)) != null)
                {
                    return (string)_UserFactAddress.Get();
                }
                else
                {
                    return _UserFactAddress_Not_Valid;
                }
            }
            set
            {
                _UserFactAddress_Not_Valid = value;
                if (GetErrors(nameof(UserFactAddress)) != null)
                {
                    _UserFactAddress.Set(_UserFactAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(UserFactAddress));
            }
        }
        private IDataLoadEngine _UserFactAddress;
        private string _UserFactAddress_Not_Valid = "";
        //UserFactAddress property

        //UserTelephone property
        [Attributes.FormVisual("Телефон")]
        public string UserTelephone
        {
            get
            {
                if (GetErrors(nameof(UserTelephone)) != null)
                {
                    return (string)_UserTelephone.Get();
                }
                else
                {
                    return _UserTelephone_Not_Valid;
                }
            }
            set
            {
                _UserTelephone_Not_Valid = value;
                if (GetErrors(nameof(UserTelephone)) != null)
                {
                    _UserTelephone.Set(_UserTelephone_Not_Valid);
                }
                OnPropertyChanged(nameof(UserTelephone));
            }
        }
        private IDataLoadEngine _UserTelephone;
        private string _UserTelephone_Not_Valid = "";
        //UserTelephone property

        //UserFax property
        [Attributes.FormVisual("Факс")]
        public string UserFax
        {
            get
            {
                if (GetErrors(nameof(UserFax)) != null)
                {
                    return (string)_UserFax.Get();
                }
                else
                {
                    return _UserFax_Not_Valid;
                }
            }
            set
            {
                _UserFax_Not_Valid = value;
                if (GetErrors(nameof(UserFax)) != null)
                {
                    _UserFax.Set(_UserFax_Not_Valid);
                }
                OnPropertyChanged(nameof(UserFax));
            }
        }
        private IDataLoadEngine _UserFax;
        private string _UserFax_Not_Valid = "";
        //UserFax property

        //Email property
        [Attributes.FormVisual("Электронная почта")]
        public string Email
        {
            get
            {
                if (GetErrors(nameof(Email)) != null)
                {
                    return (string)_Email.Get();
                }
                else
                {
                    return _Email_Not_Valid;
                }
            }
            set
            {
                _Email_Not_Valid = value;
                if (GetErrors(nameof(Email)) != null)
                {
                    _Email.Set(_Email_Not_Valid);
                }
                OnPropertyChanged(nameof(Email));
            }
        }
        private IDataLoadEngine _Email;
        private string _Email_Not_Valid = "";
        //Email property

        //ZriUsageScope property
        [Attributes.FormVisual("Область применения ЗРИ")]
        public string ZriUsageScope
        {
            get
            {
                if (GetErrors(nameof(ZriUsageScope)) != null)
                {
                    return (string)_ZriUsageScope.Get();
                }
                else
                {
                    return _ZriUsageScope_Not_Valid;
                }
            }
            set
            {
                _ZriUsageScope_Not_Valid = value;
                if (GetErrors(nameof(ZriUsageScope)) != null)
                {
                    _ZriUsageScope.Set(_ZriUsageScope_Not_Valid);
                }
                OnPropertyChanged(nameof(ZriUsageScope));
            }
        }
        private IDataLoadEngine _ZriUsageScope;
        private string _ZriUsageScope_Not_Valid = "";
        //ZriUsageScope property

        //ContractId property
        [Attributes.FormVisual("Номер контракта")]
        public string ContractId
        {
            get
            {
                if (GetErrors(nameof(ContractId)) != null)
                {
                    return (string)_ContractId.Get();
                }
                else
                {
                    return _ContractId_Not_Valid;
                }
            }
            set
            {
                _ContractId_Not_Valid = value;
                if (GetErrors(nameof(ContractId)) != null)
                {
                    _UserFactAddress.Set(_ContractId_Not_Valid);
                }
                OnPropertyChanged(nameof(ContractId));
            }
        }
        private IDataLoadEngine _ContractId;
        private string _ContractId_Not_Valid = "";
        //ContractId property

        //ContractDate property
        [Attributes.FormVisual("Дата контракта")]
        public DateTimeOffset ContractDate
        {
            get
            {
                if (GetErrors(nameof(ContractDate)) != null)
                {
                    return (DateTime)_ContractDate.Get();
                }
                else
                {
                    return _ContractDate_Not_Valid;
                }
            }
            set
            {
                _ContractDate_Not_Valid = value;
                if (GetErrors(nameof(ContractDate)) != null)
                {
                    _ContractDate.Set(_ContractDate_Not_Valid);
                }
                OnPropertyChanged(nameof(ContractDate));
            }
        }
        private IDataLoadEngine _ContractDate;
        private DateTimeOffset _ContractDate_Not_Valid = DateTimeOffset.MinValue;
        //ContractDate property

        //CountryCreator property
        [Attributes.FormVisual("Страна-изготовитель")]
        public string CountryCreator
        {
            get
            {
                if (GetErrors(nameof(CountryCreator)) != null)
                {
                    return (string)_CountryCreator.Get();
                }
                else
                {
                    return _CountryCreator_Not_Valid;
                }
            }
            set
            {
                _CountryCreator_Not_Valid = value;
                if (GetErrors(nameof(CountryCreator)) != null)
                {
                    _CountryCreator.Set(_CountryCreator_Not_Valid);
                }
                OnPropertyChanged(nameof(CountryCreator));
            }
        }
        private IDataLoadEngine _CountryCreator;
        private string _CountryCreator_Not_Valid = "";
        //CountryCreator property

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
