using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 3.0: Титульный лист отчета организации-экспортера радиоактивных источников 1 и 2 категории")]
    public class Form30 : Abstracts.Form
    {
        public static string SQLCommandParams()
        {
            string strNotNullDeclaration = " varchar(255) not null, ";
            string intNotNullDeclaration = " int not null, ";
            string shortNotNullDeclaration = " smallint not null, ";
            string byteNotNullDeclaration = " tinyint not null, ";
            string dateNotNullDeclaration = " ????, ";
            return
                nameof(RegNo) + strNotNullDeclaration +
                nameof(OrganUprav) + strNotNullDeclaration +
                nameof(SubjectRF) + strNotNullDeclaration +
                nameof(JurLico) + strNotNullDeclaration +
                nameof(ShortJurLico) + strNotNullDeclaration +
                nameof(JurLicoAddress) + strNotNullDeclaration +
                nameof(JurLicoFactAddress) + strNotNullDeclaration +
                nameof(GradeFIO) + strNotNullDeclaration +
                nameof(Telephone) + strNotNullDeclaration +
                nameof(Fax) + strNotNullDeclaration +
                nameof(Email) + strNotNullDeclaration +
                nameof(Okpo) + strNotNullDeclaration +
                nameof(Okved) + strNotNullDeclaration +
                nameof(Okogu) + strNotNullDeclaration +
                nameof(Oktmo) + strNotNullDeclaration +
                nameof(Inn) + strNotNullDeclaration +
                nameof(Kpp) + strNotNullDeclaration +
                nameof(Okopf) + strNotNullDeclaration +
                nameof(Okfs) + " varchar(255) not null";
        }
        public Form30(IDataAccess Access) : base(Access)
        {
            FormNum = "30";
            NumberOfFields = 19;
        }

        [Attributes.Form_Property("Форма")]
        public override void Object_Validation()
        {

        }

        //RegNo property
        [Attributes.Form_Property("Рег. №")]
        public string RegNo
        {
            get
            {
                if (GetErrors(nameof(RegNo)) != null)
                {
                    return (string)_dataAccess.Get(nameof(RegNo));
                }
                else
                {
                    return _RegNo_Not_Valid;
                }
            }
            set
            {
                _RegNo_Not_Valid = value;
                if (GetErrors(nameof(RegNo)) != null)
                {
                    _dataAccess.Set(nameof(RegNo), _RegNo_Not_Valid);
                }
                OnPropertyChanged(nameof(RegNo));
            }
        }
        
        private string _RegNo_Not_Valid = "";
        //RegNo property

        //OrganUprav property
        [Attributes.Form_Property("Орган управления")]
        public string OrganUprav
        {
            get
            {
                if (GetErrors(nameof(OrganUprav)) != null)
                {
                    return (string)_dataAccess.Get(nameof(OrganUprav));
                }
                else
                {
                    return _OrganUprav_Not_Valid;
                }
            }
            set
            {
                _OrganUprav_Not_Valid = value;
                if (GetErrors(nameof(OrganUprav)) != null)
                {
                    _dataAccess.Set(nameof(OrganUprav), _OrganUprav_Not_Valid);
                }
                OnPropertyChanged(nameof(OrganUprav));
            }
        }
        
        private string _OrganUprav_Not_Valid = "";
        //OrganUprav property

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

        //JurLico property
        [Attributes.Form_Property("Юр. лицо")]
        public string JurLico
        {
            get
            {
                if (GetErrors(nameof(JurLico)) != null)
                {
                    return (string)_dataAccess.Get(nameof(JurLico));
                }
                else
                {
                    return _JurLico_Not_Valid;
                }
            }
            set
            {
                _JurLico_Not_Valid = value;
                if (GetErrors(nameof(JurLico)) != null)
                {
                    _dataAccess.Set(nameof(JurLico), _JurLico_Not_Valid);
                }
                OnPropertyChanged(nameof(JurLico));
            }
        }
        
        private string _JurLico_Not_Valid = "";
        //JurLico property

        //ShortJurLico property
        [Attributes.Form_Property("Краткое наименование юр. лица")]
        public string ShortJurLico
        {
            get
            {
                if (GetErrors(nameof(ShortJurLico)) != null)
                {
                    return (string)_dataAccess.Get(nameof(ShortJurLico));
                }
                else
                {
                    return _ShortJurLico_Not_Valid;
                }
            }
            set
            {
                _ShortJurLico_Not_Valid = value;
                if (GetErrors(nameof(ShortJurLico)) != null)
                {
                    _dataAccess.Set(nameof(ShortJurLico), _ShortJurLico_Not_Valid);
                }
                OnPropertyChanged(nameof(ShortJurLico));
            }
        }
        
        private string _ShortJurLico_Not_Valid = "";
        //ShortJurLico property

        //JurLicoAddress property
        [Attributes.Form_Property("Адрес юр. лица")]
        public string JurLicoAddress
        {
            get
            {
                if (GetErrors(nameof(JurLicoAddress)) != null)
                {
                    return (string)_dataAccess.Get(nameof(JurLicoAddress));
                }
                else
                {
                    return _JurLicoAddress_Not_Valid;
                }
            }
            set
            {
                _JurLicoAddress_Not_Valid = value;
                if (GetErrors(nameof(JurLicoAddress)) != null)
                {
                    _dataAccess.Set(nameof(JurLicoAddress), _JurLicoAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(JurLicoAddress));
            }
        }
        
        private string _JurLicoAddress_Not_Valid = "";
        //JurLicoAddress property

        //JurLicoFactAddress property
        [Attributes.Form_Property("Фактический адрес юр. лица")]
        public string JurLicoFactAddress
        {
            get
            {
                if (GetErrors(nameof(JurLicoFactAddress)) != null)
                {
                    return (string)_dataAccess.Get(nameof(JurLicoFactAddress));
                }
                else
                {
                    return _JurLicoFactAddress_Not_Valid;
                }
            }
            set
            {
                _JurLicoFactAddress_Not_Valid = value;
                if (GetErrors(nameof(JurLicoFactAddress)) != null)
                {
                    _dataAccess.Set(nameof(JurLicoFactAddress), _JurLicoFactAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(JurLicoFactAddress));
            }
        }
        
        private string _JurLicoFactAddress_Not_Valid = "";
        //JurLicoFactAddress property

        //GradeFIO property
        [Attributes.Form_Property("ФИО, должность")]
        public string GradeFIO
        {
            get
            {
                if (GetErrors(nameof(GradeFIO)) != null)
                {
                    return (string)_dataAccess.Get(nameof(GradeFIO));
                }
                else
                {
                    return _GradeFIO_Not_Valid;
                }
            }
            set
            {
                _GradeFIO_Not_Valid = value;
                if (GetErrors(nameof(GradeFIO)) != null)
                {
                    _dataAccess.Set(nameof(GradeFIO), _GradeFIO_Not_Valid);
                }
                OnPropertyChanged(nameof(GradeFIO));
            }
        }
        
        private string _GradeFIO_Not_Valid = "";
        //GradeFIO property

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

        //Okpo property
        [Attributes.Form_Property("ОКПО")]
        public string Okpo
        {
            get
            {
                if (GetErrors(nameof(Okpo)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Okpo));
                }
                else
                {
                    return _Okpo_Not_Valid;
                }
            }
            set
            {
                _Okpo_Not_Valid = value;
                if (GetErrors(nameof(Okpo)) != null)
                {
                    _dataAccess.Set(nameof(Okpo), _Okpo_Not_Valid);
                }
                OnPropertyChanged(nameof(Okpo));
            }
        }
        
        private string _Okpo_Not_Valid = "";
        //Okpo property

        //Okved property
        [Attributes.Form_Property("ОКВЭД")]
        public string Okved
        {
            get
            {
                if (GetErrors(nameof(Okved)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Okved));
                }
                else
                {
                    return _Okved_Not_Valid;
                }
            }
            set
            {
                _Okved_Not_Valid = value;
                if (GetErrors(nameof(Okved)) != null)
                {
                    _dataAccess.Set(nameof(Okved), _Okved_Not_Valid);
                }
                OnPropertyChanged(nameof(Okved));
            }
        }
        
        private string _Okved_Not_Valid = "";
        //Okved property

        //Okogu property
        [Attributes.Form_Property("ОКОГУ")]
        public string Okogu
        {
            get
            {
                if (GetErrors(nameof(Okogu)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Okogu));
                }
                else
                {
                    return _Okogu_Not_Valid;
                }
            }
            set
            {
                _Okogu_Not_Valid = value;
                if (GetErrors(nameof(Okved)) != null)
                {
                    _dataAccess.Set(nameof(Okogu), _Okogu_Not_Valid);
                }
                OnPropertyChanged(nameof(Okogu));
            }
        }
        
        private string _Okogu_Not_Valid = "";
        //Okogu property

        //Oktmo property
        [Attributes.Form_Property("ОКТМО")]
        public string Oktmo
        {
            get
            {
                if (GetErrors(nameof(Oktmo)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Oktmo));
                }
                else
                {
                    return _Oktmo_Not_Valid;
                }
            }
            set
            {
                _Oktmo_Not_Valid = value;
                if (GetErrors(nameof(Oktmo)) != null)
                {
                    _dataAccess.Set(nameof(Oktmo), _Oktmo_Not_Valid);
                }
                OnPropertyChanged(nameof(Oktmo));
            }
        }
        
        private string _Oktmo_Not_Valid = "";
        //Oktmo property

        //Inn property
        [Attributes.Form_Property("ИНН")]
        public string Inn
        {
            get
            {
                if (GetErrors(nameof(Inn)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Inn));
                }
                else
                {
                    return _Inn_Not_Valid;
                }
            }
            set
            {
                _Inn_Not_Valid = value;
                if (GetErrors(nameof(Inn)) != null)
                {
                    _dataAccess.Set(nameof(Inn), _Inn_Not_Valid);
                }
                OnPropertyChanged(nameof(Inn));
            }
        }
        
        private string _Inn_Not_Valid = "";
        //Inn property

        //Kpp property
        [Attributes.Form_Property("КПП")]
        public string Kpp
        {
            get
            {
                if (GetErrors(nameof(Kpp)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Kpp));
                }
                else
                {
                    return _Kpp_Not_Valid;
                }
            }
            set
            {
                _Kpp_Not_Valid = value;
                if (GetErrors(nameof(Kpp)) != null)
                {
                    _dataAccess.Set(nameof(Kpp), _Kpp_Not_Valid);
                }
                OnPropertyChanged(nameof(Kpp));
            }
        }
        
        private string _Kpp_Not_Valid = "";
        //Kpp property

        //Okopf property
        [Attributes.Form_Property("ОКОПФ")]
        public string Okopf
        {
            get
            {
                if (GetErrors(nameof(Okopf)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Okopf));
                }
                else
                {
                    return _Okopf_Not_Valid;
                }
            }
            set
            {
                _Okopf_Not_Valid = value;
                if (GetErrors(nameof(Okopf)) != null)
                {
                    _dataAccess.Set(nameof(Okopf), _Okopf_Not_Valid);
                }
                OnPropertyChanged(nameof(Okopf));
            }
        }
        
        private string _Okopf_Not_Valid = "";
        //Okopf property

        //Okfs property
        [Attributes.Form_Property("ОКФС")]
        public string Okfs
        {
            get
            {
                if (GetErrors(nameof(Okfs)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Okfs));
                }
                else
                {
                    return _Okfs_Not_Valid;
                }
            }
            set
            {
                _Okfs_Not_Valid = value;
                if (GetErrors(nameof(Okfs)) != null)
                {
                    _dataAccess.Set(nameof(Okfs), _Okfs_Not_Valid);
                }
                OnPropertyChanged(nameof(Okfs));
            }
        }
        
        private string _Okfs_Not_Valid = "";
        //Okfs property
    }
}
