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
            FormNum.Value = "31";
            NumberOfFields.Value = 19;
        }

        [Attributes.Form_Property("Форма")]

        public override bool Object_Validation()
        {
            return false;
        }

        //RecieverName property
        [Attributes.Form_Property("Получатель")]
        public RamAccess<string> RecieverName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RecieverName));
                }
                
                {
                    
                }
            }
            set
            {
                RecieverName_Validation(value);
                
                {
                    _dataAccess.Set(nameof(RecieverName), value);
                }
                OnPropertyChanged(nameof(RecieverName));
            }
        }

        
        private bool RecieverName_Validation(RamAccess<string> value)
        { return true; }
        //RecieverName property

        //RecieverAddress property
        [Attributes.Form_Property("Адрес получателя")]
        public RamAccess<string> RecieverAddress
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RecieverAddress));
                }
                
                {
                    
                }
            }
            set
            {
                RecieverAddress_Validation(value);
                
                {
                    _dataAccess.Set(nameof(RecieverAddress), value);
                }
                OnPropertyChanged(nameof(RecieverAddress));
            }
        }
        private bool RecieverAddress_Validation(RamAccess<string> value)
        { return true; }

        
        //RecieverAddress property

        //RecieverFactAddress property
        [Attributes.Form_Property("Фактический адрес получателя")]
        public RamAccess<string> RecieverFactAddress
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RecieverFactAddress));
                }
                
                {
                    
                }
            }
            set
            {
                RecieverFactAddress_Validation(value);
                
                {
                    _dataAccess.Set(nameof(RecieverFactAddress), value);
                }
                OnPropertyChanged(nameof(RecieverFactAddress));
            }
        }
        private bool RecieverFactAddress_Validation(RamAccess<string> value)
        { return true; }
        
        //RecieverFactAddress property

        //LicenseId property
        [Attributes.Form_Property("Номер лицензии")]
        public RamAccess<string> LicenseId
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(LicenseId));
                }
                
                {
                    
                }
            }
            set
            {
                LicenseId_Validation(value);
                
                {
                    _dataAccess.Set(nameof(LicenseId), value);
                }
                OnPropertyChanged(nameof(LicenseId));
            }
        }
        private bool LicenseId_Validation(RamAccess<string> value)
        { return true; }
        
        //LicenseId property

        //ValidThru property
        [Attributes.Form_Property("Действует по")]
        public RamAccess<string> ValidThru
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ValidThru));
                }
                
                {
                    
                }
            }
            set
            {
                ValidThru_Validation(value);
                
                {
                    _dataAccess.Set(nameof(ValidThru), value);
                }
                OnPropertyChanged(nameof(ValidThru));
            }
        }
        private bool ValidThru_Validation(RamAccess<string> value)
        { return true; }
        
        //ValidThru property

        //SuggestedSolutionDate property
        [Attributes.Form_Property("")]
        public RamAccess<string> SuggestedSolutionDate
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(SuggestedSolutionDate));
                }
                
                {
                    
                }
            }
            set
            {
                SuggestedSolutionDate_Validation(value);
                
                {
                    _dataAccess.Set(nameof(SuggestedSolutionDate), value);
                }
                OnPropertyChanged(nameof(SuggestedSolutionDate));
            }
        }
        private bool SuggestedSolutionDate_Validation(RamAccess<string> value)
        { return true; }
        
        //SuggestedSolutionDate property

        //UserName property
        [Attributes.Form_Property("Наименование пользователя")]
        public RamAccess<string> UserName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(UserName));
                }
                
                {
                    
                }
            }
            set
            {
                UserName_Validation(value);
                
                {
                    _dataAccess.Set(nameof(UserName), value);
                }
                OnPropertyChanged(nameof(UserName));
            }
        }
        private bool UserName_Validation(RamAccess<string> value)
        { return true; }
        
        //UserName property

        //UserAddress property
        [Attributes.Form_Property("Адрес")]
        public RamAccess<string> UserAddress
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(UserAddress));
                }
                
                {
                    
                }
            }
            set
            {
                UserAddress_Validation(value);
                
                {
                    _dataAccess.Set(nameof(UserAddress), value);
                }
                OnPropertyChanged(nameof(UserAddress));
            }
        }
        private bool UserAddress_Validation(RamAccess<string> value)
        { return true; }
        
        //UserAddress property

        //UserFactAddress property
        [Attributes.Form_Property("Фактический адрес")]
        public RamAccess<string> UserFactAddress
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(UserFactAddress));
                }
                
                {
                    
                }
            }
            set
            {
                UserFactAddress_Validation(value);
                
                {
                    _dataAccess.Set(nameof(UserFactAddress), value);
                }
                OnPropertyChanged(nameof(UserFactAddress));
            }
        }
        private bool UserFactAddress_Validation(RamAccess<string> value)
        { return true; }
        
        //UserFactAddress property

        //UserTelephone property
        [Attributes.Form_Property("Телефон")]
        public RamAccess<string> UserTelephone
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(UserTelephone));
                }
                
                {
                    
                }
            }
            set
            {
                UserTelephone_Validation(value);
                
                {
                    _dataAccess.Set(nameof(UserTelephone), value);
                }
                OnPropertyChanged(nameof(UserTelephone));
            }
        }

        private bool UserTelephone_Validation(RamAccess<string> value)
        { return true; }
        
        //UserTelephone property

        //UserFax property
        [Attributes.Form_Property("Факс")]
        public RamAccess<string> UserFax
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(UserFax));
                }
                
                {
                    
                }
            }
            set
            {
                UserFax_Validation(value);
                
                {
                    _dataAccess.Set(nameof(UserFax), value);
                }
                OnPropertyChanged(nameof(UserFax));
            }
        }
        private bool UserFax_Validation(RamAccess<string> value)
        { return true; }
        
        //UserFax property

        //Email property
        [Attributes.Form_Property("Электронная почта")]
        public RamAccess<string> Email
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Email));
                }
                
                {
                    
                }
            }
            set
            {
                Email_Validation(value);
                
                {
                    _dataAccess.Set(nameof(Email), value);
                }
                OnPropertyChanged(nameof(Email));
            }
        }
        private bool Email_Validation(RamAccess<string> value)
        { return true; }
        
        //Email property

        //ZriUsageScope property
        [Attributes.Form_Property("Область применения ЗРИ")]
        public RamAccess<string> ZriUsageScope
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ZriUsageScope));
                }
                
                {
                    
                }
            }
            set
            {
                ZriUsageScope_Validation(value);
                
                {
                    _dataAccess.Set(nameof(ZriUsageScope), value);
                }
                OnPropertyChanged(nameof(ZriUsageScope));
            }
        }
        private bool ZriUsageScope_Validation(RamAccess<string> value)
        { return true; }
        
        //ZriUsageScope property

        //ContractId property
        [Attributes.Form_Property("Номер контракта")]
        public RamAccess<string> ContractId
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ContractId));
                }
                
                {
                    
                }
            }
            set
            {
                ContractId_Validation(value);
                
                {
                    _dataAccess.Set(nameof(ContractId), value);
                }
                OnPropertyChanged(nameof(ContractId));
            }
        }
        private bool ContractId_Validation(RamAccess<string> value)
        { return true; }
        
        //ContractId property

        //ContractDate property
        [Attributes.Form_Property("Дата контракта")]
        public RamAccess<string> ContractDate
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ContractDate));
                }
                
                {
                    
                }
            }
            set
            {
                ContractDate_Validation(value);
                
                {
                    _dataAccess.Set(nameof(ContractDate), value);
                }
                OnPropertyChanged(nameof(ContractDate));
            }
        }
        private bool ContractDate_Validation(RamAccess<string> value)
        { return true; }
        
        //ContractDate property

        //CountryCreator property
        [Attributes.Form_Property("Страна-изготовитель")]
        public RamAccess<string> CountryCreator
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CountryCreator));
                }
                
                {
                    
                }
            }
            set
            {
                CountryCreator_Validation(value);
                
                {
                    _dataAccess.Set(nameof(CountryCreator), value);
                }
                OnPropertyChanged(nameof(CountryCreator));
            }
        }
        private bool CountryCreator_Validation(RamAccess<string> value)
        { return true; }
        
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
