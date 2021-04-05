using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 1.0: Титульный лист оперативного отчета организации")]
    public class Form10:Form
    {
        public Form10() : base()
        {

        }

        [Attributes.FormVisual("Форма")]
        public override string FormNum { get { return "10"; } }
        public override int NumberOfFields { get; } = 19;
        public override void Object_Validation()
        {

        }

        //RegNo property
        [Attributes.FormVisual("Рег. №")]
        public string RegNo
        {
            get
            {
                if (GetErrors(nameof(RegNo)) != null)
                {
                    return (string)_RegNo.Get();
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
                    _RegNo.Set(_RegNo_Not_Valid);
                }
                OnPropertyChanged(nameof(RegNo));
            }
        }
        private IDataLoadEngine _RegNo;
        private string _RegNo_Not_Valid = "";
        //RegNo property

        //OrganUprav property
        [Attributes.FormVisual("Орган управления")]
        public string OrganUprav
        {
            get
            {
                if (GetErrors(nameof(OrganUprav)) != null)
                {
                    return (string)_OrganUprav.Get();
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
                    _OrganUprav.Set(_OrganUprav_Not_Valid);
                }
                OnPropertyChanged(nameof(OrganUprav));
            }
        }
        private IDataLoadEngine _OrganUprav;
        private string _OrganUprav_Not_Valid = "";
        //OrganUprav property

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

        //JurLico property
        [Attributes.FormVisual("Юр. лицо")]
        public string JurLico
        {
            get
            {
                if (GetErrors(nameof(JurLico)) != null)
                {
                    return (string)_JurLico.Get();
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
                    _JurLico.Set(_JurLico_Not_Valid);
                }
                OnPropertyChanged(nameof(JurLico));
            }
        }
        private IDataLoadEngine _JurLico;
        private string _JurLico_Not_Valid = "";
        //JurLico property

        //ShortJurLico property
        [Attributes.FormVisual("Краткое наименование юр. лица")]
        public string ShortJurLico
        {
            get
            {
                if (GetErrors(nameof(ShortJurLico)) != null)
                {
                    return (string)_ShortJurLico.Get();
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
                    _ShortJurLico.Set(_ShortJurLico_Not_Valid);
                }
                OnPropertyChanged(nameof(ShortJurLico));
            }
        }
        private IDataLoadEngine _ShortJurLico;
        private string _ShortJurLico_Not_Valid = "";
        //ShortJurLico property

        //JurLicoAddress property
        [Attributes.FormVisual("Адрес юр. лица")]
        public string JurLicoAddress
        {
            get
            {
                if (GetErrors(nameof(JurLicoAddress)) != null)
                {
                    return (string)_JurLicoAddress.Get();
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
                    _JurLicoAddress.Set(_JurLicoAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(JurLicoAddress));
            }
        }
        private IDataLoadEngine _JurLicoAddress;
        private string _JurLicoAddress_Not_Valid = "";
        //JurLicoAddress property

        //JurLicoFactAddress property
        [Attributes.FormVisual("Фактический адрес юр. лица")]
        public string JurLicoFactAddress
        {
            get
            {
                if (GetErrors(nameof(JurLicoFactAddress)) != null)
                {
                    return (string)_JurLicoFactAddress.Get();
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
                    _JurLicoFactAddress.Set(_JurLicoFactAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(JurLicoFactAddress));
            }
        }
        private IDataLoadEngine _JurLicoFactAddress;
        private string _JurLicoFactAddress_Not_Valid = "";
        //JurLicoFactAddress property

        //GradeFIO property
        [Attributes.FormVisual("ФИО, должность")]
        public string GradeFIO
        {
            get
            {
                if (GetErrors(nameof(GradeFIO)) != null)
                {
                    return (string)_GradeFIO.Get();
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
                    _GradeFIO.Set(_GradeFIO_Not_Valid);
                }
                OnPropertyChanged(nameof(GradeFIO));
            }
        }
        private IDataLoadEngine _GradeFIO;
        private string _GradeFIO_Not_Valid = "";
        //GradeFIO property

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

        //Okpo property
        [Attributes.FormVisual("ОКПО")]
        public string Okpo
        {
            get
            {
                if (GetErrors(nameof(Okpo)) != null)
                {
                    return (string)_Okpo.Get();
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
                    _Okpo.Set(_Okpo_Not_Valid);
                }
                OnPropertyChanged(nameof(Okpo));
            }
        }
        private IDataLoadEngine _Okpo;
        private string _Okpo_Not_Valid = "";
        //Okpo property

        //Okved property
        [Attributes.FormVisual("ОКВЭД")]
        public string Okved
        {
            get
            {
                if (GetErrors(nameof(Okved)) != null)
                {
                    return (string)_Okved.Get();
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
                    _Okved.Set(_Okved_Not_Valid);
                }
                OnPropertyChanged(nameof(Okved));
            }
        }
        private IDataLoadEngine _Okved;
        private string _Okved_Not_Valid = "";
        //Okved property

        //Okogu property
        [Attributes.FormVisual("ОКОГУ")]
        public string Okogu
        {
            get
            {
                if (GetErrors(nameof(Okogu)) != null)
                {
                    return (string)_Okogu.Get();
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
                    _Okogu.Set(_Okogu_Not_Valid);
                }
                OnPropertyChanged(nameof(Okogu));
            }
        }
        private IDataLoadEngine _Okogu;
        private string _Okogu_Not_Valid = "";
        //Okogu property

        //Oktmo property
        [Attributes.FormVisual("ОКТМО")]
        public string Oktmo
        {
            get
            {
                if (GetErrors(nameof(Oktmo)) != null)
                {
                    return (string)_Oktmo.Get();
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
                    _Oktmo.Set(_Oktmo_Not_Valid);
                }
                OnPropertyChanged(nameof(Oktmo));
            }
        }
        private IDataLoadEngine _Oktmo;
        private string _Oktmo_Not_Valid = "";
        //Oktmo property

        //Inn property
        [Attributes.FormVisual("ИНН")]
        public string Inn
        {
            get
            {
                if (GetErrors(nameof(Inn)) != null)
                {
                    return (string)_Inn.Get();
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
                    _Inn.Set(_Inn_Not_Valid);
                }
                OnPropertyChanged(nameof(Inn));
            }
        }
        private IDataLoadEngine _Inn;
        private string _Inn_Not_Valid = "";
        //Inn property

        //Kpp property
        [Attributes.FormVisual("КПП")]
        public string Kpp
        {
            get
            {
                if (GetErrors(nameof(Kpp)) != null)
                {
                    return (string)_Kpp.Get();
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
                    _Kpp.Set(_Kpp_Not_Valid);
                }
                OnPropertyChanged(nameof(Kpp));
            }
        }
        private IDataLoadEngine _Kpp;
        private string _Kpp_Not_Valid = "";
        //Kpp property

        //Okopf property
        [Attributes.FormVisual("ОКОПФ")]
        public string Okopf
        {
            get
            {
                if (GetErrors(nameof(Okopf)) != null)
                {
                    return (string)_Okopf.Get();
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
                    _Okopf.Set(_Okopf_Not_Valid);
                }
                OnPropertyChanged(nameof(Okopf));
            }
        }
        private IDataLoadEngine _Okopf;
        private string _Okopf_Not_Valid = "";
        //Okopf property

        //Okfs property
        [Attributes.FormVisual("ОКФС")]
        public string Okfs
        {
            get
            {
                if (GetErrors(nameof(Okfs)) != null)
                {
                    return (string)_Okfs.Get();
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
                    _Okfs.Set(_Okfs_Not_Valid);
                }
                OnPropertyChanged(nameof(Okfs));
            }
        }
        private IDataLoadEngine _Okfs;
        private string _Okfs_Not_Valid = "";
        //Okfs property
    }
}
