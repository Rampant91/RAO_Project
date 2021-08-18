using Models.DataAccess;
using System;
using System.Collections.Generic;

namespace Models
{
    //[Serializable]
    //[Attributes.Form_Class("Форма 3.1: Отчет о намерении экспортировать радиоактивные источники 1 и 2 категории")]
    //public class Form31 : Abstracts.Form3
    //{
    //    public Form31() : base()
    //    {
    //        //FormNum.Value = "31";
    //        //NumberOfFields.Value = 19;
    //    }

    //    [Attributes.Form_Property("Форма")]

    //    public override bool Object_Validation()
    //    {
    //        return false;
    //    }

    //    //RecieverName property
    //    public int? RecieverNameId { get; set; }
    //    [Attributes.Form_Property("Получатель")]
    //    public virtual RamAccess<string> RecieverName
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(RecieverName));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(RecieverName), value);
    //            }
    //            OnPropertyChanged(nameof(RecieverName));
    //        }
    //    }


    //    private bool RecieverName_Validation(RamAccess<string> value)
    //    { return true; }
    //    //RecieverName property

    //    //RecieverAddress property
    //    public int? RecieverAddressId { get; set; }
    //    [Attributes.Form_Property("Адрес получателя")]
    //    public virtual RamAccess<string> RecieverAddress
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(RecieverAddress));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(RecieverAddress), value);
    //            }
    //            OnPropertyChanged(nameof(RecieverAddress));
    //        }
    //    }
    //    private bool RecieverAddress_Validation(RamAccess<string> value)
    //    { return true; }


    //    //RecieverAddress property

    //    //RecieverFactAddress property
    //    public int? RecieverFactAddressId { get; set; }
    //    [Attributes.Form_Property("Фактический адрес получателя")]
    //    public virtual RamAccess<string> RecieverFactAddress
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(RecieverFactAddress));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(RecieverFactAddress), value);
    //            }
    //            OnPropertyChanged(nameof(RecieverFactAddress));
    //        }
    //    }
    //    private bool RecieverFactAddress_Validation(RamAccess<string> value)
    //    { return true; }

    //    //RecieverFactAddress property

    //    //Licenseid property
    //    public int? LicenseidId { get; set; }
    //    [Attributes.Form_Property("Номер лицензии")]
    //    public virtual RamAccess<string> Licenseid
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(Licenseid));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(Licenseid), value);
    //            }
    //            OnPropertyChanged(nameof(Licenseid));
    //        }
    //    }
    //    private bool Licenseid_Validation(RamAccess<string> value)
    //    { return true; }

    //    //Licenseid property

    //    //ValidThru property
    //    public int? ValidThruId { get; set; }
    //    [Attributes.Form_Property("Действует по")]
    //    public virtual RamAccess<string> ValidThru
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(ValidThru));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(ValidThru), value);
    //            }
    //            OnPropertyChanged(nameof(ValidThru));
    //        }
    //    }
    //    private bool ValidThru_Validation(RamAccess<string> value)
    //    { return true; }

    //    //ValidThru property

    //    //SuggestedSolutionDate property
    //    public int? SuggestedSolutionDateId { get; set; }
    //    [Attributes.Form_Property("")]
    //    public virtual RamAccess<string> SuggestedSolutionDate
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(SuggestedSolutionDate));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(SuggestedSolutionDate), value);
    //            }
    //            OnPropertyChanged(nameof(SuggestedSolutionDate));
    //        }
    //    }
    //    private bool SuggestedSolutionDate_Validation(RamAccess<string> value)
    //    { return true; }

    //    //SuggestedSolutionDate property

    //    //UserName property
    //    public int? UserNameId { get; set; }
    //    [Attributes.Form_Property("Наименование пользователя")]
    //    public virtual RamAccess<string> UserName
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(UserName));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(UserName), value);
    //            }
    //            OnPropertyChanged(nameof(UserName));
    //        }
    //    }
    //    private bool UserName_Validation(RamAccess<string> value)
    //    { return true; }

    //    //UserName property

    //    //UserAddress property
    //    public int? UserAddressId { get; set; }
    //    [Attributes.Form_Property("Адрес")]
    //    public virtual RamAccess<string> UserAddress
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(UserAddress));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(UserAddress), value);
    //            }
    //            OnPropertyChanged(nameof(UserAddress));
    //        }
    //    }
    //    private bool UserAddress_Validation(RamAccess<string> value)
    //    { return true; }

    //    //UserAddress property

    //    //UserFactAddress property
    //    public int? UserFactAddressId { get; set; }
    //    [Attributes.Form_Property("Фактический адрес")]
    //    public virtual RamAccess<string> UserFactAddress
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(UserFactAddress));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(UserFactAddress), value);
    //            }
    //            OnPropertyChanged(nameof(UserFactAddress));
    //        }
    //    }
    //    private bool UserFactAddress_Validation(RamAccess<string> value)
    //    { return true; }

    //    //UserFactAddress property

    //    //UserTelephone property
    //    public int? UserTelephoneId { get; set; }
    //    [Attributes.Form_Property("Телефон")]
    //    public virtual RamAccess<string> UserTelephone
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(UserTelephone));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(UserTelephone), value);
    //            }
    //            OnPropertyChanged(nameof(UserTelephone));
    //        }
    //    }

    //    private bool UserTelephone_Validation(RamAccess<string> value)
    //    { return true; }

    //    //UserTelephone property

    //    //UserFax property
    //    public int? UserFaxId { get; set; }
    //    [Attributes.Form_Property("Факс")]
    //    public virtual RamAccess<string> UserFax
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(UserFax));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(UserFax), value);
    //            }
    //            OnPropertyChanged(nameof(UserFax));
    //        }
    //    }
    //    private bool UserFax_Validation(RamAccess<string> value)
    //    { return true; }

    //    //UserFax property

    //    //Email property
    //    public int? EmailId { get; set; }
    //    [Attributes.Form_Property("Электронная почта")]
    //    public virtual RamAccess<string> Email
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(Email));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(Email), value);
    //            }
    //            OnPropertyChanged(nameof(Email));
    //        }
    //    }
    //    private bool Email_Validation(RamAccess<string> value)
    //    { return true; }

    //    //Email property

    //    //ZriUsageScope property
    //    public int? ZriUsageScopeId { get; set; }
    //    [Attributes.Form_Property("Область применения ЗРИ")]
    //    public virtual RamAccess<string> ZriUsageScope
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(ZriUsageScope));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(ZriUsageScope), value);
    //            }
    //            OnPropertyChanged(nameof(ZriUsageScope));
    //        }
    //    }
    //    private bool ZriUsageScope_Validation(RamAccess<string> value)
    //    { return true; }

    //    //ZriUsageScope property

    //    //Contractid property
    //    public int? ContractidId { get; set; }
    //    [Attributes.Form_Property("Номер контракта")]
    //    public virtual RamAccess<string> Contractid
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(Contractid));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(Contractid), value);
    //            }
    //            OnPropertyChanged(nameof(Contractid));
    //        }
    //    }
    //    private bool Contractid_Validation(RamAccess<string> value)
    //    { return true; }

    //    //Contractid property

    //    //ContractDate property
    //    public int? ContractDateId { get; set; }
    //    [Attributes.Form_Property("Дата контракта")]
    //    public virtual RamAccess<string> ContractDate
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(ContractDate));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(ContractDate), value);
    //            }
    //            OnPropertyChanged(nameof(ContractDate));
    //        }
    //    }
    //    private bool ContractDate_Validation(RamAccess<string> value)
    //    { return true; }

    //    //ContractDate property

    //    //CountryCreator property
    //    public int? CountryCreatorId { get; set; }
    //    [Attributes.Form_Property("Страна-изготовитель")]
    //    public virtual RamAccess<string> CountryCreator
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(CountryCreator));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(CountryCreator), value);
    //            }
    //            OnPropertyChanged(nameof(CountryCreator));
    //        }
    //    }
    //    private bool CountryCreator_Validation(RamAccess<string> value)
    //    { return true; }

    //    //CountryCreator property

    //    private List<Form31_1> _zriList = new List<Form31_1>();
    //    public int? ZriListId { get; set; }
    //    public List<Form31_1> ZriList
    //    {
    //        get { return _zriList; }
    //        set
    //        {
    //            _zriList = value;
    //            OnPropertyChanged("ZriList");
    //        }
    //    }
    //}
}
