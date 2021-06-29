using Models.DataAccess;
using System;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 4.0: Титульный лист годового отчета СГУК РВ и РАО")]
    public class Form40 : Abstracts.Form
    {
        public Form40() : base()
        {
            //FormNum.Value = "40";
            //NumberOfFields.Value = 18;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //SubjectRF property
        public int? SubjectRFId { get; set; }
        [Attributes.Form_Property("Субъект РФ")]
        public virtual RamAccess<string> SubjectRF
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(SubjectRF));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SubjectRF), value);
                }
                OnPropertyChanged(nameof(SubjectRF));
            }
        }


        //SubjectRF property

        //Yyear property
        public int? YyearId { get; set; }
        [Attributes.Form_Property("Год")]
        public virtual RamAccess<int> Yyear
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(Yyear));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Yyear), value);
                }
                OnPropertyChanged(nameof(Yyear));
            }
        }


        //Yyear property

        //SubjectAuthorityName property
        public int? SubjectAuthorityNameId { get; set; }
        [Attributes.Form_Property("Наименование органа исполнительной власти")]
        public virtual RamAccess<int> SubjectAuthorityName
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(SubjectAuthorityName));
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(SubjectAuthorityName), value);
                OnPropertyChanged(nameof(SubjectAuthorityName));
            }
        }


        //SubjectAuthorityName property

        //ShortSubjectAuthorityName property
        public int? ShortSubjectAuthorityNameId { get; set; }
        [Attributes.Form_Property("Краткое наименование органа исполнительной власти")]
        public virtual RamAccess<int> ShortSubjectAuthorityName
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(ShortSubjectAuthorityName));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(ShortSubjectAuthorityName), value);
                }
                OnPropertyChanged(nameof(ShortSubjectAuthorityName));
            }
        }


        //ShortSubjectAuthorityName property

        //FactAddress property
        public int? FactAddressId { get; set; }
        [Attributes.Form_Property("Фактический адрес")]
        public virtual RamAccess<string> FactAddress
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FactAddress));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(FactAddress), value);
                }
                OnPropertyChanged(nameof(FactAddress));
            }
        }


        //FactAddress property

        //GradeFIOchef property
        public int? GradeFIOchefId { get; set; }
        [Attributes.Form_Property("ФИО, должность руководителя")]
        public virtual RamAccess<string> GradeFIOchef
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(GradeFIOchef));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(GradeFIOchef), value);
                }
                OnPropertyChanged(nameof(GradeFIOchef));
            }
        }


        //GradeFIOchef property

        //GradeFIOresponsibleExecutor property
        public int? GradeFIOresponsibleExecutorId { get; set; }
        [Attributes.Form_Property("ФИО, должность ответственного исполнителя")]
        public virtual RamAccess<string> GradeFIOresponsibleExecutor
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(GradeFIOresponsibleExecutor));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(GradeFIOresponsibleExecutor), value);
                }
                OnPropertyChanged(nameof(GradeFIOresponsibleExecutor));
            }
        }


        //GradeFIOresponsibleExecutor property

        //Telephone property
        public int? TelephoneId { get; set; }
        [Attributes.Form_Property("Телефон")]
        public virtual RamAccess<string> Telephone
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Telephone));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Telephone), value);
                }
                OnPropertyChanged(nameof(Telephone));
            }
        }


        //Telephone property

        //Fax property
        public int? FaxId { get; set; }
        [Attributes.Form_Property("Факс")]
        public virtual RamAccess<string> Fax
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Fax));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Fax), value);
                }
                OnPropertyChanged(nameof(Fax));
            }
        }


        //Fax property

        //Email property
        public int? EmailId { get; set; }
        [Attributes.Form_Property("Эл. почта")]
        public virtual RamAccess<string> Email
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

                
                {
                    _dataAccess.Set(nameof(Email), value);
                }
                OnPropertyChanged(nameof(Email));
            }
        }


        //Email property

        //Telephone1 property
        public int? Telephone1Id { get; set; }
        [Attributes.Form_Property("Телефон")]
        public virtual RamAccess<string> Telephone1
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Telephone1));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Telephone1), value);
                }
                OnPropertyChanged(nameof(Telephone1));
            }
        }


        //Telephone1 property

        //Fax1 property
        public int? Fax1Id { get; set; }
        [Attributes.Form_Property("Факс")]
        public virtual RamAccess<string> Fax1
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Fax1));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Fax1), value);
                }
                OnPropertyChanged(nameof(Fax1));
            }
        }


        //Fax1 property

        //Email1 property
        public int? Email1Id { get; set; }
        [Attributes.Form_Property("Эл. почта")]
        public virtual RamAccess<string> Email1
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Email1));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Email1), value);
                }
                OnPropertyChanged(nameof(Email1));
            }
        }


        //Email1 property

        //OrgName property
        public int? OrgNameId { get; set; }
        [Attributes.Form_Property("Название организации")]
        public virtual RamAccess<string> OrgName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(OrgName));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(OrgName), value);
                }
                OnPropertyChanged(nameof(OrgName));
            }
        }


        //OrgName property

        //ShortOrgName property
        public int? ShortOrgNameId { get; set; }
        [Attributes.Form_Property("Краткое название организации")]
        public virtual RamAccess<string> ShortOrgName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ShortOrgName));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(ShortOrgName), value);
                }
                OnPropertyChanged(nameof(ShortOrgName));
            }
        }


        //ShortOrgName property

        //FactAddress1 property
        public int? FactAddress1Id { get; set; }
        [Attributes.Form_Property("Фактический адрес")]
        public virtual RamAccess<string> FactAddress1
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FactAddress1));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(FactAddress1), value);
                }
                OnPropertyChanged(nameof(FactAddress1));
            }
        }


        //FactAddress1 property

        //GradeFIOchef1 property
        public int? GradeFIOchef1Id { get; set; }
        [Attributes.Form_Property("ФИО, должность руководителя")]
        public virtual RamAccess<string> GradeFIOchef1
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(GradeFIOchef1));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(GradeFIOchef1), value);
                }
                OnPropertyChanged(nameof(GradeFIOchef1));
            }
        }


        //GradeFIOchef1 property

        //GradeFIOresponsibleExecutor1 property
        public int? GradeFIOresponsibleExecutor1Id { get; set; }
        [Attributes.Form_Property("ФИО, должность ответственного исполнителя")]
        public virtual RamAccess<string> GradeFIOresponsibleExecutor1
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(GradeFIOresponsibleExecutor1));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(GradeFIOresponsibleExecutor1), value);
                }
                OnPropertyChanged(nameof(GradeFIOresponsibleExecutor1));
            }
        }


        //GradeFIOresponsibleExecutor1 property
    }
}
