using Models.DataAccess;
using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 3.1: Отчет о намерении экспортировать радиоактивные источники 1 и 2 категории")]
    public class Form31 : Abstracts.Form3
    {
        public Form31() : base()
        {
            FormNum = "31";
            NumberOfFields = 19;
        }

        [Attributes.Form_Property("Форма")]

        public override bool Object_Validation()
        {
            return false;
        }

        //RecieverName property
        [Attributes.Form_Property("Получатель")]
        public string RecieverName
        {
            get
            {
                if (GetErrors(nameof(RecieverName)) == null)
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
                RecieverName_Validation(value);
                if (GetErrors(nameof(RecieverName)) == null)
                {
                    _dataAccess.Set(nameof(RecieverName), value);
                }
                OnPropertyChanged(nameof(RecieverName));
            }
        }

        private string _RecieverName_Not_Valid = "";
        private void RecieverName_Validation(string value)
        { }
        //RecieverName property

        //RecieverAddress property
        [Attributes.Form_Property("Адрес получателя")]
        public string RecieverAddress
        {
            get
            {
                if (GetErrors(nameof(RecieverAddress)) == null)
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
                RecieverAddress_Validation(value);
                if (GetErrors(nameof(RecieverAddress)) == null)
                {
                    _dataAccess.Set(nameof(RecieverAddress), value);
                }
                OnPropertyChanged(nameof(RecieverAddress));
            }
        }
        private void RecieverAddress_Validation(string value)
        { }

        private string _RecieverAddress_Not_Valid = "";
        //RecieverAddress property

        //RecieverFactAddress property
        [Attributes.Form_Property("Фактический адрес получателя")]
        public string RecieverFactAddress
        {
            get
            {
                if (GetErrors(nameof(RecieverFactAddress)) == null)
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
                RecieverFactAddress_Validation(value);
                if (GetErrors(nameof(RecieverFactAddress)) == null)
                {
                    _dataAccess.Set(nameof(RecieverFactAddress), value);
                }
                OnPropertyChanged(nameof(RecieverFactAddress));
            }
        }
        private void RecieverFactAddress_Validation(string value)
        { }
        private string _RecieverFactAddress_Not_Valid = "";
        //RecieverFactAddress property

        //LicenseId property
        [Attributes.Form_Property("Номер лицензии")]
        public string LicenseId
        {
            get
            {
                if (GetErrors(nameof(LicenseId)) == null)
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
                LicenseId_Validation(value);
                if (GetErrors(nameof(LicenseId)) == null)
                {
                    _dataAccess.Set(nameof(LicenseId), value);
                }
                OnPropertyChanged(nameof(LicenseId));
            }
        }
        private void LicenseId_Validation(string value)
        { }
        private string _LicenseId_Not_Valid = "";
        //LicenseId property

        //ValidThru property
        [Attributes.Form_Property("Действует по")]
        public DateTimeOffset ValidThru
        {
            get
            {
                if (GetErrors(nameof(ValidThru)) == null)
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
                ValidThru_Validation(value);
                if (GetErrors(nameof(ValidThru)) == null)
                {
                    _dataAccess.Set(nameof(ValidThru), value);
                }
                OnPropertyChanged(nameof(ValidThru));
            }
        }
        private void ValidThru_Validation(DateTimeOffset value)
        { }
        private DateTimeOffset _ValidThru_Not_Valid = DateTimeOffset.Parse("01/01/1921");
        //ValidThru property

        //SuggestedSolutionDate property
        [Attributes.Form_Property("")]
        public DateTimeOffset SuggestedSolutionDate
        {
            get
            {
                if (GetErrors(nameof(SuggestedSolutionDate)) == null)
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
                SuggestedSolutionDate_Validation(value);
                if (GetErrors(nameof(SuggestedSolutionDate)) == null)
                {
                    _dataAccess.Set(nameof(SuggestedSolutionDate), value);
                }
                OnPropertyChanged(nameof(SuggestedSolutionDate));
            }
        }
        private void SuggestedSolutionDate_Validation(DateTimeOffset value)
        { }
        private DateTimeOffset _SuggestedSolutionDate_Not_Valid = DateTimeOffset.Parse("01/01/1921");
        //SuggestedSolutionDate property

        //UserName property
        [Attributes.Form_Property("Наименование пользователя")]
        public string UserName
        {
            get
            {
                if (GetErrors(nameof(UserName)) == null)
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
                UserName_Validation(value);
                if (GetErrors(nameof(UserName)) == null)
                {
                    _dataAccess.Set(nameof(UserName), value);
                }
                OnPropertyChanged(nameof(UserName));
            }
        }
        private void UserName_Validation(string value)
        { }
        private string _UserName_Not_Valid = "";
        //UserName property

        //UserAddress property
        [Attributes.Form_Property("Адрес")]
        public string UserAddress
        {
            get
            {
                if (GetErrors(nameof(UserAddress)) == null)
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
                UserAddress_Validation(value);
                if (GetErrors(nameof(UserAddress)) == null)
                {
                    _dataAccess.Set(nameof(UserAddress), value);
                }
                OnPropertyChanged(nameof(UserAddress));
            }
        }
        private void UserAddress_Validation(string value)
        { }
        private string _UserAddress_Not_Valid = "";
        //UserAddress property

        //UserFactAddress property
        [Attributes.Form_Property("Фактический адрес")]
        public string UserFactAddress
        {
            get
            {
                if (GetErrors(nameof(UserFactAddress)) == null)
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
                UserFactAddress_Validation(value);
                if (GetErrors(nameof(UserFactAddress)) == null)
                {
                    _dataAccess.Set(nameof(UserFactAddress), value);
                }
                OnPropertyChanged(nameof(UserFactAddress));
            }
        }
        private void UserFactAddress_Validation(string value)
        { }
        private string _UserFactAddress_Not_Valid = "";
        //UserFactAddress property

        //UserTelephone property
        [Attributes.Form_Property("Телефон")]
        public string UserTelephone
        {
            get
            {
                if (GetErrors(nameof(UserTelephone)) == null)
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
                UserTelephone_Validation(value);
                if (GetErrors(nameof(UserTelephone)) == null)
                {
                    _dataAccess.Set(nameof(UserTelephone), value);
                }
                OnPropertyChanged(nameof(UserTelephone));
            }
        }

        private void UserTelephone_Validation(string value)
        { }
        private string _UserTelephone_Not_Valid = "";
        //UserTelephone property

        //UserFax property
        [Attributes.Form_Property("Факс")]
        public string UserFax
        {
            get
            {
                if (GetErrors(nameof(UserFax)) == null)
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
                UserFax_Validation(value);
                if (GetErrors(nameof(UserFax)) == null)
                {
                    _dataAccess.Set(nameof(UserFax), value);
                }
                OnPropertyChanged(nameof(UserFax));
            }
        }
        private void UserFax_Validation(string value)
        { }
        private string _UserFax_Not_Valid = "";
        //UserFax property

        //Email property
        [Attributes.Form_Property("Электронная почта")]
        public string Email
        {
            get
            {
                if (GetErrors(nameof(Email)) == null)
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
                Email_Validation(value);
                if (GetErrors(nameof(Email)) == null)
                {
                    _dataAccess.Set(nameof(Email), value);
                }
                OnPropertyChanged(nameof(Email));
            }
        }
        private void Email_Validation(string value)
        { }
        private string _Email_Not_Valid = "";
        //Email property

        //ZriUsageScope property
        [Attributes.Form_Property("Область применения ЗРИ")]
        public string ZriUsageScope
        {
            get
            {
                if (GetErrors(nameof(ZriUsageScope)) == null)
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
                ZriUsageScope_Validation(value);
                if (GetErrors(nameof(ZriUsageScope)) == null)
                {
                    _dataAccess.Set(nameof(ZriUsageScope), value);
                }
                OnPropertyChanged(nameof(ZriUsageScope));
            }
        }
        private void ZriUsageScope_Validation(string value)
        { }
        private string _ZriUsageScope_Not_Valid = "";
        //ZriUsageScope property

        //ContractId property
        [Attributes.Form_Property("Номер контракта")]
        public string ContractId
        {
            get
            {
                if (GetErrors(nameof(ContractId)) == null)
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
                ContractId_Validation(value);
                if (GetErrors(nameof(ContractId)) == null)
                {
                    _dataAccess.Set(nameof(ContractId), value);
                }
                OnPropertyChanged(nameof(ContractId));
            }
        }
        private void ContractId_Validation(string value)
        { }
        private string _ContractId_Not_Valid = "";
        //ContractId property

        //ContractDate property
        [Attributes.Form_Property("Дата контракта")]
        public DateTimeOffset ContractDate
        {
            get
            {
                if (GetErrors(nameof(ContractDate)) == null)
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
                ContractDate_Validation(value);
                if (GetErrors(nameof(ContractDate)) == null)
                {
                    _dataAccess.Set(nameof(ContractDate), value);
                }
                OnPropertyChanged(nameof(ContractDate));
            }
        }
        private void ContractDate_Validation(DateTimeOffset value)
        { }
        private DateTimeOffset _ContractDate_Not_Valid = DateTimeOffset.Parse("01/01/1921");
        //ContractDate property

        //CountryCreator property
        [Attributes.Form_Property("Страна-изготовитель")]
        public string CountryCreator
        {
            get
            {
                if (GetErrors(nameof(CountryCreator)) == null)
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
                CountryCreator_Validation(value);
                if (GetErrors(nameof(CountryCreator)) == null)
                {
                    _dataAccess.Set(nameof(CountryCreator), value);
                }
                OnPropertyChanged(nameof(CountryCreator));
            }
        }
        private void CountryCreator_Validation(string value)
        { }
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
