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
        
        private string _subjectRf = "";
        public string SubjectRf
        {
            get { return _subjectRf; }
            set
            {
                _subjectRf = value;
                OnPropertyChanged("SubjectRf");
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

        private string _subjectAuthorityName = "";
        public string SubjectAuthorityName
        {
            get { return _subjectAuthorityName; }
            set
            {
                _subjectAuthorityName = value;
                OnPropertyChanged("SubjectAuthorityName");
            }
        }

        private string _shortSubjectAuthorityName = "";
        public string ShortSubjectAuthorityName
        {
            get { return _shortSubjectAuthorityName; }
            set
            {
                _shortSubjectAuthorityName = value;
                OnPropertyChanged("ShortSubjectAuthorityName");
            }
        }

        private string _factAddress = "";
        public string FactAddress
        {
            get { return _factAddress; }
            set
            {
                _factAddress = value;
                OnPropertyChanged("FactAddress");
            }
        }

        private string _gradeFIOchef = "";
        public string GradeFIOchef
        {
            get { return _gradeFIOchef; }
            set
            {
                _gradeFIOchef = value;
                OnPropertyChanged("GradeFIOchef");
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

        private string _telephone1 = "";
        public string Telephone1
        {
            get { return _telephone1; }
            set
            {
                _telephone1 = value;
                OnPropertyChanged("Telephone1");
            }
        }

        private string _fax1 = "";
        public string Fax1
        {
            get { return _fax1; }
            set
            {
                _fax1 = value;
                OnPropertyChanged("Fax1");
            }
        }

        private string _email1 = "";
        public string Email1
        {
            get { return _email1; }
            set
            {
                _email1 = value;
                OnPropertyChanged("Email1");
            }
        }

        private string _orgName = "";
        public string OrgName
        {
            get { return _orgName; }
            set
            {
                _orgName = value;
                OnPropertyChanged("OrgName");
            }
        }

        private string _shortOrgName = "";
        public string ShortOrgName
        {
            get { return _shortOrgName; }
            set
            {
                _shortOrgName = value;
                OnPropertyChanged("ShortOrgName");
            }
        }

        private string _factAddress1 = "";
        public string FactAddress1
        {
            get { return _factAddress1; }
            set
            {
                _factAddress1 = value;
                OnPropertyChanged("FactAddress1");
            }
        }

        private string _gradeFIOchef1 = "";
        public string GradeFIOchef1
        {
            get { return _gradeFIOchef1; }
            set
            {
                _gradeFIOchef1 = value;
                OnPropertyChanged("GradeFIOchef1");
            }
        }

        private string _gradeFIOresponsibleExecutor1 = "";
        public string GradeFIOresponsibleExecutor1
        {
            get { return _gradeFIOresponsibleExecutor1; }
            set
            {
                _gradeFIOresponsibleExecutor1 = value;
                OnPropertyChanged("GradeFIOresponsibleExecutor1");
            }
        }
    }
}
