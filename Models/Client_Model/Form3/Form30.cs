using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 3.0: Титульный лист отчета организации-экспортера радиоактивных источников 1 и 2 категории")]
    public class Form30 : Form
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 19;

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

        private string _jurLico = "";
        public string JurLico
        {
            get { return _jurLico; }
            set
            {
                _jurLico = value;
                OnPropertyChanged("JurLico");
            }
        }

        private string _shortJurLico = "";
        public string ShortJurLico
        {
            get { return _shortJurLico; }
            set
            {
                _shortJurLico = value;
                OnPropertyChanged("ShortJurLico");
            }
        }

        private string _jurLicoAddress = "";
        public string JurLicoAddress
        {
            get { return _jurLicoAddress; }
            set
            {
                _jurLicoAddress = value;
                OnPropertyChanged("JurLicoAddress");
            }
        }

        private string _jurLicoFactAddress = "";
        public string JurLicoFactAddress
        {
            get { return _jurLicoFactAddress; }
            set
            {
                _jurLicoFactAddress = value;
                OnPropertyChanged("JurLicoFactAddress");
            }
        }

        private string _gradeFIO = "";
        public string GradeFIO
        {
            get { return _gradeFIO; }
            set
            {
                _gradeFIO = value;
                OnPropertyChanged("GradeFIO");
            }
        }

        private string _telephone = "";
        public string Telephone
        {
            get { return _telephone; }
            set
            {
                _telephone = value;
                OnPropertyChanged("Telephone");
            }
        }

        private string _fax = "";
        public string Fax
        {
            get { return _fax; }
            set
            {
                _fax = value;
                OnPropertyChanged("Fax");
            }
        }

        private string _email = "";
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        private string _okpo = "";
        public string Okpo
        {
            get { return _okpo; }
            set
            {
                _okpo = value;
                OnPropertyChanged("Okpo");
            }
        }

        private string _okved = "";
        public string Okved
        {
            get { return _okved; }
            set
            {
                _okved = value;
                OnPropertyChanged("Okved");
            }
        }

        private string _okogu = "";
        public string Okogu
        {
            get { return _okogu; }
            set
            {
                _okogu = value;
                OnPropertyChanged("Okogu");
            }
        }

        private string _oktmo = "";
        public string Oktmo
        {
            get { return _oktmo; }
            set
            {
                _oktmo = value;
                OnPropertyChanged("Oktmo");
            }
        }

        private string _inn = "";
        public string Inn
        {
            get { return _inn; }
            set
            {
                _inn = value;
                OnPropertyChanged("Inn");
            }
        }

        private string _kpp = "";
        public string Kpp
        {
            get { return _kpp; }
            set
            {
                _kpp = value;
                OnPropertyChanged("Kpp");
            }
        }

        private string _okopf = "";
        public string Okopf
        {
            get { return _okopf; }
            set
            {
                _okopf = value;
                OnPropertyChanged("Okopf");
            }
        }

        private string _okfs = "";
        public string Okfs
        {
            get { return _okfs; }
            set
            {
                _okfs = value;
                OnPropertyChanged("Okfs");
            }
        }
    }
}
