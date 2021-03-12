using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 5.0: Титульный лист годового отчета СГУК РВ и РАО")]
    public class Form50 : Form5
    {
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
        public int NumberOfFields { get; } = 11;

        private Authority _authority1 = Authority.None;
        public Authority Authority1
        {
            get { return _authority1; }
            set
            {
                _authority1 = value;
                OnPropertyChanged("Authority1");
            }
        }

        private int _year = -1;
        public int Year
        {
            get { return _year; }
            set
            {
                _year = value;
                OnPropertyChanged("Year");
            }
        }

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

        private string _gradeFIOresponsibleExecutor = "";
        public string GradeFIOresponsibleExecutor
        {
            get { return _gradeFIOresponsibleExecutor; }
            set
            {
                _gradeFIOresponsibleExecutor = value;
                OnPropertyChanged("GradeFIOresponsibleExecutor");
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
    }
}
