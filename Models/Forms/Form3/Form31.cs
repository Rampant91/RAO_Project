using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 3.1: Отчет о намерении экспортировать радиоактивные источники 1 и 2 категории")]
    public class Form31 : Abstracts.Form3
    {
        public static string SQLCommandParams()
        {
            string strNotNullDeclaration = " varchar(255) not null, ";
            string intNotNullDeclaration = " int not null, ";
            string shortNotNullDeclaration = " smallint not null, ";
            string byteNotNullDeclaration = " tinyint not null, ";
            string dateNotNullDeclaration = " ????, ";
            string doubleNotNullDeclaration = " float(53) not null, ";
            return
                Abstracts.Form3.SQLCommandParamsBase() +
            nameof(RecieverName) + strNotNullDeclaration +
            nameof(RecieverAddress) + strNotNullDeclaration +
            nameof(RecieverFactAddress) + strNotNullDeclaration +
            nameof(LicenseId) + strNotNullDeclaration +
            nameof(SuggestedSolutionDate) + dateNotNullDeclaration +
            nameof(UserName) + strNotNullDeclaration +
            nameof(UserAddress) + strNotNullDeclaration +
            nameof(UserFactAddress) + strNotNullDeclaration +
            nameof(UserTelephone) + strNotNullDeclaration +
            nameof(UserFax) + strNotNullDeclaration +
            nameof(ZriUsageScope) + strNotNullDeclaration +
            nameof(ContractId) + strNotNullDeclaration +
            nameof(ContractDate) + dateNotNullDeclaration +
            nameof(CountryCreator) + strNotNullDeclaration +
            nameof(ValidThru) + dateNotNullDeclaration +
            nameof(Email) + " varchar(255) not null";
        }
        public Form31(IDataAccess Access) : base(Access)
        {
            FormNum = "31";
            NumberOfFields = 19;
        }

        [Attributes.Form_Property("Форма")]

        public override void Object_Validation()
        {

        }

        //RecieverName property
        [Attributes.Form_Property("Получатель")]
        public string RecieverName
        {
            get
            {
                if (GetErrors(nameof(RecieverName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(RecieverName));
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
                    _dataAccess.Set(nameof(RecieverName), _RecieverName_Not_Valid);
                }
                OnPropertyChanged(nameof(RecieverName));
            }
        }
        
        private string _RecieverName_Not_Valid = "";
        //RecieverName property

        //RecieverAddress property
        [Attributes.Form_Property("Адрес получателя")]
        public string RecieverAddress
        {
            get
            {
                if (GetErrors(nameof(RecieverAddress)) != null)
                {
                    return (string)_dataAccess.Get(nameof(RecieverAddress));
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
                    _dataAccess.Set(nameof(RecieverAddress), _RecieverAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(RecieverAddress));
            }
        }
        
        private string _RecieverAddress_Not_Valid = "";
        //RecieverAddress property

        //RecieverFactAddress property
        [Attributes.Form_Property("Фактический адрес получателя")]
        public string RecieverFactAddress
        {
            get
            {
                if (GetErrors(nameof(RecieverFactAddress)) != null)
                {
                    return (string)_dataAccess.Get(nameof(RecieverFactAddress));
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
                    _dataAccess.Set(nameof(RecieverFactAddress), _RecieverFactAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(RecieverFactAddress));
            }
        }
        
        private string _RecieverFactAddress_Not_Valid = "";
        //RecieverFactAddress property

        //LicenseId property
        [Attributes.Form_Property("Номер лицензии")]
        public string LicenseId
        {
            get
            {
                if (GetErrors(nameof(LicenseId)) != null)
                {
                    return (string)_dataAccess.Get(nameof(LicenseId));
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
                    _dataAccess.Set(nameof(LicenseId), _LicenseId_Not_Valid);
                }
                OnPropertyChanged(nameof(LicenseId));
            }
        }
        
        private string _LicenseId_Not_Valid = "";
        //LicenseId property

        //ValidThru property
        [Attributes.Form_Property("Действует по")]
        public DateTimeOffset ValidThru
        {
            get
            {
                if (GetErrors(nameof(ValidThru)) != null)
                {
                    return (DateTime)_dataAccess.Get(nameof(ValidThru));
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
                    _dataAccess.Set(nameof(ValidThru), _ValidThru_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidThru));
            }
        }
        
        private DateTimeOffset _ValidThru_Not_Valid = DateTimeOffset.MinValue;
        //ValidThru property

        //SuggestedSolutionDate property
        [Attributes.Form_Property("")]
        public DateTimeOffset SuggestedSolutionDate
        {
            get
            {
                if (GetErrors(nameof(SuggestedSolutionDate)) != null)
                {
                    return (DateTime)_dataAccess.Get(nameof(SuggestedSolutionDate));
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
                    _dataAccess.Set(nameof(SuggestedSolutionDate), _SuggestedSolutionDate_Not_Valid);
                }
                OnPropertyChanged(nameof(SuggestedSolutionDate));
            }
        }
        
        private DateTimeOffset _SuggestedSolutionDate_Not_Valid = DateTimeOffset.MinValue;
        //SuggestedSolutionDate property

        //UserName property
        [Attributes.Form_Property("Наименование пользователя")]
        public string UserName
        {
            get
            {
                if (GetErrors(nameof(UserName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(UserName));
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
                    _dataAccess.Set(nameof(UserName), _UserName_Not_Valid);
                }
                OnPropertyChanged(nameof(UserName));
            }
        }
        
        private string _UserName_Not_Valid = "";
        //UserName property

        //UserAddress property
        [Attributes.Form_Property("Адрес")]
        public string UserAddress
        {
            get
            {
                if (GetErrors(nameof(UserAddress)) != null)
                {
                    return (string)_dataAccess.Get(nameof(UserAddress));
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
                    _dataAccess.Set(nameof(UserAddress), _UserAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(UserAddress));
            }
        }
        
        private string _UserAddress_Not_Valid = "";
        //UserAddress property

        //UserFactAddress property
        [Attributes.Form_Property("Фактический адрес")]
        public string UserFactAddress
        {
            get
            {
                if (GetErrors(nameof(UserFactAddress)) != null)
                {
                    return (string)_dataAccess.Get(nameof(UserFactAddress));
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
                    _dataAccess.Set(nameof(UserFactAddress), _UserFactAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(UserFactAddress));
            }
        }
        
        private string _UserFactAddress_Not_Valid = "";
        //UserFactAddress property

        //UserTelephone property
        [Attributes.Form_Property("Телефон")]
        public string UserTelephone
        {
            get
            {
                if (GetErrors(nameof(UserTelephone)) != null)
                {
                    return (string)_dataAccess.Get(nameof(UserTelephone));
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
                    _dataAccess.Set(nameof(UserTelephone), _UserTelephone_Not_Valid);
                }
                OnPropertyChanged(nameof(UserTelephone));
            }
        }
        
        private string _UserTelephone_Not_Valid = "";
        //UserTelephone property

        //UserFax property
        [Attributes.Form_Property("Факс")]
        public string UserFax
        {
            get
            {
                if (GetErrors(nameof(UserFax)) != null)
                {
                    return (string)_dataAccess.Get(nameof(UserFax));
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
                    _dataAccess.Set(nameof(UserFax), _UserFax_Not_Valid);
                }
                OnPropertyChanged(nameof(UserFax));
            }
        }
        
        private string _UserFax_Not_Valid = "";
        //UserFax property

        //Email property
        [Attributes.Form_Property("Электронная почта")]
        public string Email
        {
            get
            {
                if (GetErrors(nameof(Email)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Email));
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
                    _dataAccess.Set(nameof(Email), _Email_Not_Valid);
                }
                OnPropertyChanged(nameof(Email));
            }
        }
        
        private string _Email_Not_Valid = "";
        //Email property

        //ZriUsageScope property
        [Attributes.Form_Property("Область применения ЗРИ")]
        public string ZriUsageScope
        {
            get
            {
                if (GetErrors(nameof(ZriUsageScope)) != null)
                {
                    return (string)_dataAccess.Get(nameof(ZriUsageScope));
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
                    _dataAccess.Set(nameof(ZriUsageScope), _ZriUsageScope_Not_Valid);
                }
                OnPropertyChanged(nameof(ZriUsageScope));
            }
        }
        
        private string _ZriUsageScope_Not_Valid = "";
        //ZriUsageScope property

        //ContractId property
        [Attributes.Form_Property("Номер контракта")]
        public string ContractId
        {
            get
            {
                if (GetErrors(nameof(ContractId)) != null)
                {
                    return (string)_dataAccess.Get(nameof(ContractId));
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
                    _dataAccess.Set(nameof(ContractId), _ContractId_Not_Valid);
                }
                OnPropertyChanged(nameof(ContractId));
            }
        }
        
        private string _ContractId_Not_Valid = "";
        //ContractId property

        //ContractDate property
        [Attributes.Form_Property("Дата контракта")]
        public DateTimeOffset ContractDate
        {
            get
            {
                if (GetErrors(nameof(ContractDate)) != null)
                {
                    return (DateTime)_dataAccess.Get(nameof(ContractDate));
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
                    _dataAccess.Set(nameof(ContractDate), _ContractDate_Not_Valid);
                }
                OnPropertyChanged(nameof(ContractDate));
            }
        }
        
        private DateTimeOffset _ContractDate_Not_Valid = DateTimeOffset.MinValue;
        //ContractDate property

        //CountryCreator property
        [Attributes.Form_Property("Страна-изготовитель")]
        public string CountryCreator
        {
            get
            {
                if (GetErrors(nameof(CountryCreator)) != null)
                {
                    return (string)_dataAccess.Get(nameof(CountryCreator));
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
                    _dataAccess.Set(nameof(CountryCreator), _CountryCreator_Not_Valid);
                }
                OnPropertyChanged(nameof(CountryCreator));
            }
        }
        
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
