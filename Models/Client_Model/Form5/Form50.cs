using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 5.0: Титульный лист годового отчета СГУК РВ и РАО")]
    public class Form50 : Form
    {
        public Form50() : base()
        {
        }

        [Attributes.FormVisual("Форма")]
        public override string FormNum { get { return "50"; } }
        public override int NumberOfFields { get; } = 11;
        public override void Object_Validation()
        {

        }
        public enum Authority
        {
            FederalAuthority,
            CorporationRosatom,
            DepartmentOfDefense,
            None
        }

        //Authority1 property
        [Attributes.FormVisual("ВИАЦ")]
        public Authority Authority1
        {
            get
            {
                if (GetErrors(nameof(Authority1)) != null)
                {
                    return (Authority)_Authority1.Get();
                }
                else
                {
                    return _Authority1_Not_Valid;
                }
            }
            set
            {
                _Authority1_Not_Valid = value;
                if (GetErrors(nameof(Authority1)) != null)
                {
                    _Authority1.Set(_Authority1_Not_Valid);
                }
                OnPropertyChanged(nameof(Authority1));
            }
        }
        private IDataLoadEngine _Authority1;
        private Authority _Authority1_Not_Valid = Authority.None;
        //Authority1 property

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
    }
}
