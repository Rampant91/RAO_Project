using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DBRealization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 4.0: Титульный лист годового отчета СГУК РВ и РАО")]
    public class Form40 : Abstracts.Form
    {
        public static string SQLCommandParams()
        {
            string strNotNullDeclaration = " varchar(255) not null, ";
            string intNotNullDeclaration = " int not null, ";
            string shortNotNullDeclaration = " smallint not null, ";
            string byteNotNullDeclaration = " tinyint not null, ";
            string dateNotNullDeclaration = " ????, ";
            return
                nameof(SubjectRF) + strNotNullDeclaration +
                nameof(Year) + intNotNullDeclaration +
                nameof(SubjectAuthorityName) + intNotNullDeclaration +
                nameof(ShortSubjectAuthorityName) + intNotNullDeclaration +
                nameof(FactAddress) + strNotNullDeclaration +
                nameof(GradeFIOchef) + strNotNullDeclaration +
                nameof(GradeFIOresponsibleExecutor) + strNotNullDeclaration +
                nameof(Telephone) + strNotNullDeclaration +
                nameof(Fax) + strNotNullDeclaration +
                nameof(Email) + strNotNullDeclaration +
                nameof(Telephone1) + strNotNullDeclaration +
                nameof(Fax1) + strNotNullDeclaration +
                nameof(Email1) + strNotNullDeclaration +
                nameof(OrgName) + strNotNullDeclaration +
                nameof(ShortOrgName) + strNotNullDeclaration +
                nameof(FactAddress1) + strNotNullDeclaration +
                nameof(GradeFIOchef1) + strNotNullDeclaration +
                nameof(GradeFIOresponsibleExecutor1) + " varchar(255) not null";
        }
        public Form40(IDataAccess Access) : base(Access)
        {
            FormNum = "40";
            NumberOfFields = 18;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //SubjectRF property
        [Attributes.Form_Property("Субъект РФ")]
        public string SubjectRF
        {
            get
            {
                if (GetErrors(nameof(SubjectRF)) != null)
                {
                    return (string)_dataAccess.Get(nameof(SubjectRF));
                }
                else
                {
                    return _SubjectRF_Not_Valid;
                }
            }
            set
            {
                _SubjectRF_Not_Valid = value;
                if (GetErrors(nameof(SubjectRF)) != null)
                {
                    _dataAccess.Set(nameof(SubjectRF), _SubjectRF_Not_Valid);
                }
                OnPropertyChanged(nameof(SubjectRF));
            }
        }
        
        private string _SubjectRF_Not_Valid = "";
        //SubjectRF property

        //Year property
        [Attributes.Form_Property("Год")]
        public int Year
        {
            get
            {
                if (GetErrors(nameof(Year)) != null)
                {
                    return (int)_dataAccess.Get(nameof(Year));
                }
                else
                {
                    return _Year_Not_Valid;
                }
            }
            set
            {
                _Year_Not_Valid = value;
                if (GetErrors(nameof(Year)) != null)
                {
                    _dataAccess.Set(nameof(Year), _Year_Not_Valid);
                }
                OnPropertyChanged(nameof(Year));
            }
        }
        
        private int _Year_Not_Valid = -1;
        //Year property

        //SubjectAuthorityName property
        [Attributes.Form_Property("Наименование органа исполнительной власти")]
        public int SubjectAuthorityName
        {
            get
            {
                if (GetErrors(nameof(SubjectAuthorityName)) != null)
                {
                    return (int)_dataAccess.Get(nameof(SubjectAuthorityName));
                }
                else
                {
                    return _SubjectAuthorityName_Not_Valid;
                }
            }
            set
            {
                _SubjectAuthorityName_Not_Valid = value;
                if (GetErrors(nameof(SubjectAuthorityName)) != null)
                {
                    _dataAccess.Set(nameof(SubjectAuthorityName), _SubjectAuthorityName_Not_Valid);
                }
                OnPropertyChanged(nameof(SubjectAuthorityName));
            }
        }
        
        private int _SubjectAuthorityName_Not_Valid = -1;
        //SubjectAuthorityName property

        //ShortSubjectAuthorityName property
        [Attributes.Form_Property("Краткое наименование органа исполнительной власти")]
        public int ShortSubjectAuthorityName
        {
            get
            {
                if (GetErrors(nameof(ShortSubjectAuthorityName)) != null)
                {
                    return (int)_dataAccess.Get(nameof(ShortSubjectAuthorityName));
                }
                else
                {
                    return _ShortSubjectAuthorityName_Not_Valid;
                }
            }
            set
            {
                _ShortSubjectAuthorityName_Not_Valid = value;
                if (GetErrors(nameof(ShortSubjectAuthorityName)) != null)
                {
                    _dataAccess.Set(nameof(ShortSubjectAuthorityName), _ShortSubjectAuthorityName_Not_Valid);
                }
                OnPropertyChanged(nameof(ShortSubjectAuthorityName));
            }
        }
        
        private int _ShortSubjectAuthorityName_Not_Valid = -1;
        //ShortSubjectAuthorityName property

        //FactAddress property
        [Attributes.Form_Property("Фактический адрес")]
        public string FactAddress
        {
            get
            {
                if (GetErrors(nameof(FactAddress)) != null)
                {
                    return (string)_dataAccess.Get(nameof(FactAddress));
                }
                else
                {
                    return _FactAddress_Not_Valid;
                }
            }
            set
            {
                _FactAddress_Not_Valid = value;
                if (GetErrors(nameof(FactAddress)) != null)
                {
                    _dataAccess.Set(nameof(FactAddress), _FactAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(FactAddress));
            }
        }
        
        private string _FactAddress_Not_Valid = "";
        //FactAddress property

        //GradeFIOchef property
        [Attributes.Form_Property("ФИО, должность руководителя")]
        public string GradeFIOchef
        {
            get
            {
                if (GetErrors(nameof(GradeFIOchef)) != null)
                {
                    return (string)_dataAccess.Get(nameof(GradeFIOchef));
                }
                else
                {
                    return _GradeFIOchef_Not_Valid;
                }
            }
            set
            {
                _GradeFIOchef_Not_Valid = value;
                if (GetErrors(nameof(GradeFIOchef)) != null)
                {
                    _dataAccess.Set(nameof(GradeFIOchef), _GradeFIOchef_Not_Valid);
                }
                OnPropertyChanged(nameof(GradeFIOchef));
            }
        }
        
        private string _GradeFIOchef_Not_Valid = "";
        //GradeFIOchef property

        //GradeFIOresponsibleExecutor property
        [Attributes.Form_Property("ФИО, должность ответственного исполнителя")]
        public string GradeFIOresponsibleExecutor
        {
            get
            {
                if (GetErrors(nameof(GradeFIOresponsibleExecutor)) != null)
                {
                    return (string)_dataAccess.Get(nameof(GradeFIOresponsibleExecutor));
                }
                else
                {
                    return _GradeFIOresponsibleExecutor_Not_Valid;
                }
            }
            set
            {
                _GradeFIOresponsibleExecutor_Not_Valid = value;
                if (GetErrors(nameof(GradeFIOresponsibleExecutor)) != null)
                {
                    _dataAccess.Set(nameof(GradeFIOresponsibleExecutor), _GradeFIOresponsibleExecutor_Not_Valid);
                }
                OnPropertyChanged(nameof(GradeFIOresponsibleExecutor));
            }
        }
        
        private string _GradeFIOresponsibleExecutor_Not_Valid = "";
        //GradeFIOresponsibleExecutor property

        //Telephone property
        [Attributes.Form_Property("Телефон")]
        public string Telephone
        {
            get
            {
                if (GetErrors(nameof(Telephone)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Telephone));
                }
                else
                {
                    return _Telephone_Not_Valid;
                }
            }
            set
            {
                _Telephone_Not_Valid = value;
                if (GetErrors(nameof(Telephone)) != null)
                {
                    _dataAccess.Set(nameof(Telephone), _Telephone_Not_Valid);
                }
                OnPropertyChanged(nameof(Telephone));
            }
        }
        
        private string _Telephone_Not_Valid = "";
        //Telephone property

        //Fax property
        [Attributes.Form_Property("Факс")]
        public string Fax
        {
            get
            {
                if (GetErrors(nameof(Fax)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Fax));
                }
                else
                {
                    return _Fax_Not_Valid;
                }
            }
            set
            {
                _Fax_Not_Valid = value;
                if (GetErrors(nameof(Fax)) != null)
                {
                    _dataAccess.Set(nameof(Fax), _Fax_Not_Valid);
                }
                OnPropertyChanged(nameof(Fax));
            }
        }
        
        private string _Fax_Not_Valid = "";
        //Fax property

        //Email property
        [Attributes.Form_Property("Эл. почта")]
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

        //Telephone1 property
        [Attributes.Form_Property("Телефон")]
        public string Telephone1
        {
            get
            {
                if (GetErrors(nameof(Telephone1)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Telephone1));
                }
                else
                {
                    return _Telephone1_Not_Valid;
                }
            }
            set
            {
                _Telephone1_Not_Valid = value;
                if (GetErrors(nameof(Telephone1)) != null)
                {
                    _dataAccess.Set(nameof(Telephone1), _Telephone1_Not_Valid);
                }
                OnPropertyChanged(nameof(Telephone1));
            }
        }
        
        private string _Telephone1_Not_Valid = "";
        //Telephone1 property

        //Fax1 property
        [Attributes.Form_Property("Факс")]
        public string Fax1
        {
            get
            {
                if (GetErrors(nameof(Fax1)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Fax1));
                }
                else
                {
                    return _Fax1_Not_Valid;
                }
            }
            set
            {
                _Fax1_Not_Valid = value;
                if (GetErrors(nameof(Fax1)) != null)
                {
                    _dataAccess.Set(nameof(Fax1), _Fax1_Not_Valid);
                }
                OnPropertyChanged(nameof(Fax1));
            }
        }
        
        private string _Fax1_Not_Valid = "";
        //Fax1 property

        //Email1 property
        [Attributes.Form_Property("Эл. почта")]
        public string Email1
        {
            get
            {
                if (GetErrors(nameof(Email1)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Email1));
                }
                else
                {
                    return _Email1_Not_Valid;
                }
            }
            set
            {
                _Email1_Not_Valid = value;
                if (GetErrors(nameof(Email1)) != null)
                {
                    _dataAccess.Set(nameof(Email1), _Email1_Not_Valid);
                }
                OnPropertyChanged(nameof(Email1));
            }
        }
        
        private string _Email1_Not_Valid = "";
        //Email1 property

        //OrgName property
        [Attributes.Form_Property("Название организации")]
        public string OrgName
        {
            get
            {
                if (GetErrors(nameof(OrgName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(OrgName));
                }
                else
                {
                    return _OrgName_Not_Valid;
                }
            }
            set
            {
                _OrgName_Not_Valid = value;
                if (GetErrors(nameof(OrgName)) != null)
                {
                    _dataAccess.Set(nameof(OrgName), _OrgName_Not_Valid);
                }
                OnPropertyChanged(nameof(OrgName));
            }
        }
        
        private string _OrgName_Not_Valid = "";
        //OrgName property

        //ShortOrgName property
        [Attributes.Form_Property("Краткое название организации")]
        public string ShortOrgName
        {
            get
            {
                if (GetErrors(nameof(ShortOrgName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(ShortOrgName));
                }
                else
                {
                    return _ShortOrgName_Not_Valid;
                }
            }
            set
            {
                _ShortOrgName_Not_Valid = value;
                if (GetErrors(nameof(ShortOrgName)) != null)
                {
                    _dataAccess.Set(nameof(ShortOrgName), _ShortOrgName_Not_Valid);
                }
                OnPropertyChanged(nameof(ShortOrgName));
            }
        }
        
        private string _ShortOrgName_Not_Valid = "";
        //ShortOrgName property

        //FactAddress1 property
        [Attributes.Form_Property("Фактический адрес")]
        public string FactAddress1
        {
            get
            {
                if (GetErrors(nameof(FactAddress1)) != null)
                {
                    return (string)_dataAccess.Get(nameof(FactAddress1));
                }
                else
                {
                    return _FactAddress1_Not_Valid;
                }
            }
            set
            {
                _FactAddress1_Not_Valid = value;
                if (GetErrors(nameof(FactAddress1)) != null)
                {
                    _dataAccess.Set(nameof(FactAddress1), _FactAddress1_Not_Valid);
                }
                OnPropertyChanged(nameof(FactAddress1));
            }
        }
        
        private string _FactAddress1_Not_Valid = "";
        //FactAddress1 property

        //GradeFIOchef1 property
        [Attributes.Form_Property("ФИО, должность руководителя")]
        public string GradeFIOchef1
        {
            get
            {
                if (GetErrors(nameof(GradeFIOchef1)) != null)
                {
                    return (string)_dataAccess.Get(nameof(GradeFIOchef1));
                }
                else
                {
                    return _GradeFIOchef1_Not_Valid;
                }
            }
            set
            {
                _GradeFIOchef1_Not_Valid = value;
                if (GetErrors(nameof(GradeFIOchef1)) != null)
                {
                    _dataAccess.Set(nameof(GradeFIOchef1), _GradeFIOchef1_Not_Valid);
                }
                OnPropertyChanged(nameof(GradeFIOchef1));
            }
        }
        
        private string _GradeFIOchef1_Not_Valid = "";
        //GradeFIOchef1 property

        //GradeFIOresponsibleExecutor1 property
        [Attributes.Form_Property("ФИО, должность ответственного исполнителя")]
        public string GradeFIOresponsibleExecutor1
        {
            get
            {
                if (GetErrors(nameof(GradeFIOresponsibleExecutor1)) != null)
                {
                    return (string)_dataAccess.Get(nameof(GradeFIOresponsibleExecutor1));
                }
                else
                {
                    return _GradeFIOresponsibleExecutor1_Not_Valid;
                }
            }
            set
            {
                _GradeFIOresponsibleExecutor1_Not_Valid = value;
                if (GetErrors(nameof(GradeFIOresponsibleExecutor1)) != null)
                {
                    _dataAccess.Set(nameof(GradeFIOresponsibleExecutor1), _GradeFIOresponsibleExecutor1_Not_Valid);
                }
                OnPropertyChanged(nameof(GradeFIOresponsibleExecutor1));
            }
        }
        
        private string _GradeFIOresponsibleExecutor1_Not_Valid = "";
        //GradeFIOresponsibleExecutor1 property
    }
}
