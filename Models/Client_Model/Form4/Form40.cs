using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 4.0: Титульный лист годового отчета СГУК РВ и РАО")]
    public class Form40 : Form
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 18;

        //SubjectRF property
        [Attributes.FormVisual("Субъект РФ")]
        public string SubjectRF
        {
            get
            {
                if (GetErrors(nameof(SubjectRF)) != null)
                {
                    return (string)_SubjectRF.Get();
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
                    _SubjectRF.Set(_SubjectRF_Not_Valid);
                }
                OnPropertyChanged(nameof(SubjectRF));
            }
        }
        private IDataLoadEngine _SubjectRF;
        private string _SubjectRF_Not_Valid = "";
        //SubjectRF property

        //Year property
        [Attributes.FormVisual("Год")]
        public int Year
        {
            get
            {
                if (GetErrors(nameof(Year)) != null)
                {
                    return (int)_Year.Get();
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
                    _Year.Set(_Year_Not_Valid);
                }
                OnPropertyChanged(nameof(Year));
            }
        }
        private IDataLoadEngine _Year;
        private int _Year_Not_Valid = -1;
        //Year property

        //SubjectAuthorityName property
        [Attributes.FormVisual("Наименование органа исполнительной власти")]
        public int SubjectAuthorityName
        {
            get
            {
                if (GetErrors(nameof(SubjectAuthorityName)) != null)
                {
                    return (int)_SubjectAuthorityName.Get();
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
                    _SubjectAuthorityName.Set(_SubjectAuthorityName_Not_Valid);
                }
                OnPropertyChanged(nameof(SubjectAuthorityName));
            }
        }
        private IDataLoadEngine _SubjectAuthorityName;
        private int _SubjectAuthorityName_Not_Valid = -1;
        //SubjectAuthorityName property

        //SubjectAuthorityName property
        [Attributes.FormVisual("Краткое наименование органа исполнительной власти")]
        public int ShortSubjectAuthorityName
        {
            get
            {
                if (GetErrors(nameof(ShortSubjectAuthorityName)) != null)
                {
                    return (int)_ShortSubjectAuthorityName.Get();
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
                    _ShortSubjectAuthorityName.Set(_ShortSubjectAuthorityName_Not_Valid);
                }
                OnPropertyChanged(nameof(ShortSubjectAuthorityName));
            }
        }
        private IDataLoadEngine _ShortSubjectAuthorityName;
        private int _ShortSubjectAuthorityName_Not_Valid = -1;
        //ShortSubjectAuthorityName property

        //FactAddress property
        [Attributes.FormVisual("Фактический адрес")]
        public string FactAddress
        {
            get
            {
                if (GetErrors(nameof(FactAddress)) != null)
                {
                    return (string)_FactAddress.Get();
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
                    _FactAddress.Set(_FactAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(FactAddress));
            }
        }
        private IDataLoadEngine _FactAddress;
        private string _FactAddress_Not_Valid = "";
        //FactAddress property

        //GradeFIOchef property
        [Attributes.FormVisual("ФИО, должность руководителя")]
        public string GradeFIOchef
        {
            get
            {
                if (GetErrors(nameof(GradeFIOchef)) != null)
                {
                    return (string)_GradeFIOchef.Get();
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
                    _GradeFIOchef.Set(_GradeFIOchef_Not_Valid);
                }
                OnPropertyChanged(nameof(GradeFIOchef));
            }
        }
        private IDataLoadEngine _GradeFIOchef;
        private string _GradeFIOchef_Not_Valid = "";
        //GradeFIOchef property

        //GradeFIOresponsibleExecutor property
        [Attributes.FormVisual("ФИО, должность ответственного исполнителя")]
        public string GradeFIOresponsibleExecutor
        {
            get
            {
                if (GetErrors(nameof(GradeFIOresponsibleExecutor)) != null)
                {
                    return (string)_GradeFIOresponsibleExecutor.Get();
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
                    _GradeFIOresponsibleExecutor.Set(_GradeFIOresponsibleExecutor_Not_Valid);
                }
                OnPropertyChanged(nameof(GradeFIOresponsibleExecutor));
            }
        }
        private IDataLoadEngine _GradeFIOresponsibleExecutor;
        private string _GradeFIOresponsibleExecutor_Not_Valid = "";
        //GradeFIOresponsibleExecutor property

        //Telephone property
        [Attributes.FormVisual("Телефон")]
        public string Telephone
        {
            get
            {
                if (GetErrors(nameof(Telephone)) != null)
                {
                    return (string)_Telephone.Get();
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
                    _Telephone.Set(_Telephone_Not_Valid);
                }
                OnPropertyChanged(nameof(Telephone));
            }
        }
        private IDataLoadEngine _Telephone;
        private string _Telephone_Not_Valid = "";
        //Telephone property

        //Fax property
        [Attributes.FormVisual("Факс")]
        public string Fax
        {
            get
            {
                if (GetErrors(nameof(Fax)) != null)
                {
                    return (string)_Fax.Get();
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
                    _Fax.Set(_Fax_Not_Valid);
                }
                OnPropertyChanged(nameof(Fax));
            }
        }
        private IDataLoadEngine _Fax;
        private string _Fax_Not_Valid = "";
        //Fax property

        //Email property
        [Attributes.FormVisual("Эл. почта")]
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

        //Telephone1 property
        [Attributes.FormVisual("Телефон")]
        public string Telephone1
        {
            get
            {
                if (GetErrors(nameof(Telephone1)) != null)
                {
                    return (string)_Telephone1.Get();
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
                    _Telephone1.Set(_Telephone1_Not_Valid);
                }
                OnPropertyChanged(nameof(Telephone1));
            }
        }
        private IDataLoadEngine _Telephone1;
        private string _Telephone1_Not_Valid = "";
        //Telephone1 property

        //Fax1 property
        [Attributes.FormVisual("Факс")]
        public string Fax1
        {
            get
            {
                if (GetErrors(nameof(Fax1)) != null)
                {
                    return (string)_Fax1.Get();
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
                    _Fax1.Set(_Fax1_Not_Valid);
                }
                OnPropertyChanged(nameof(Fax1));
            }
        }
        private IDataLoadEngine _Fax1;
        private string _Fax1_Not_Valid = "";
        //Fax1 property

        //Email1 property
        [Attributes.FormVisual("Эл. почта")]
        public string Email1
        {
            get
            {
                if (GetErrors(nameof(Email1)) != null)
                {
                    return (string)_Email1.Get();
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
                    _Email1.Set(_Email1_Not_Valid);
                }
                OnPropertyChanged(nameof(Email1));
            }
        }
        private IDataLoadEngine _Email1;
        private string _Email1_Not_Valid = "";
        //Email1 property

        //OrgName property
        [Attributes.FormVisual("Название организации")]
        public string OrgName
        {
            get
            {
                if (GetErrors(nameof(OrgName)) != null)
                {
                    return (string)_OrgName.Get();
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
                    _OrgName.Set(_OrgName_Not_Valid);
                }
                OnPropertyChanged(nameof(OrgName));
            }
        }
        private IDataLoadEngine _OrgName;
        private string _OrgName_Not_Valid = "";
        //OrgName property

        //ShortOrgName property
        [Attributes.FormVisual("Краткое название организации")]
        public string ShortOrgName
        {
            get
            {
                if (GetErrors(nameof(ShortOrgName)) != null)
                {
                    return (string)_ShortOrgName.Get();
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
                    _ShortOrgName.Set(_ShortOrgName_Not_Valid);
                }
                OnPropertyChanged(nameof(ShortOrgName));
            }
        }
        private IDataLoadEngine _ShortOrgName;
        private string _ShortOrgName_Not_Valid = "";
        //ShortOrgName property

        //FactAddress1 property
        [Attributes.FormVisual("Фактический адрес")]
        public string FactAddress1
        {
            get
            {
                if (GetErrors(nameof(FactAddress1)) != null)
                {
                    return (string)_FactAddress1.Get();
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
                    _FactAddress1.Set(_FactAddress1_Not_Valid);
                }
                OnPropertyChanged(nameof(FactAddress1));
            }
        }
        private IDataLoadEngine _FactAddress1;
        private string _FactAddress1_Not_Valid = "";
        //FactAddress1 property

        //GradeFIOchef1 property
        [Attributes.FormVisual("ФИО, должность руководителя")]
        public string GradeFIOchef1
        {
            get
            {
                if (GetErrors(nameof(GradeFIOchef1)) != null)
                {
                    return (string)_GradeFIOchef1.Get();
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
                    _GradeFIOchef1.Set(_GradeFIOchef1_Not_Valid);
                }
                OnPropertyChanged(nameof(GradeFIOchef1));
            }
        }
        private IDataLoadEngine _GradeFIOchef1;
        private string _GradeFIOchef1_Not_Valid = "";
        //GradeFIOchef1 property

        //GradeFIOresponsibleExecutor1 property
        [Attributes.FormVisual("ФИО, должность ответственного исполнителя")]
        public string GradeFIOresponsibleExecutor1
        {
            get
            {
                if (GetErrors(nameof(GradeFIOresponsibleExecutor1)) != null)
                {
                    return (string)_GradeFIOresponsibleExecutor1.Get();
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
                    _GradeFIOresponsibleExecutor1.Set(_GradeFIOresponsibleExecutor1_Not_Valid);
                }
                OnPropertyChanged(nameof(GradeFIOresponsibleExecutor1));
            }
        }
        private IDataLoadEngine _GradeFIOresponsibleExecutor1;
        private string _GradeFIOresponsibleExecutor1_Not_Valid = "";
        //GradeFIOresponsibleExecutor1 property
    }
}
