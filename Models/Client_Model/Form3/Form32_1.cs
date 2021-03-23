using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("")]
    public class Form32_1: Form3
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 15;

        //PassportNumber property
        [Attributes.FormVisual("Номер паспорта")]
        public string PassportNumber
        {
            get
            {
                if (GetErrors(nameof(PassportNumber)) != null)
                {
                    return _PassportNumber;
                }
                else
                {
                    return _PassportNumber_Not_Valid;
                }
            }
            set
            {
                _PassportNumber_Not_Valid = value;
                if (PassportNumber_Validation())
                {
                    _PassportNumber = _PassportNumber_Not_Valid;
                }
                OnPropertyChanged(nameof(PassportNumber));
            }
        }
        private string _PassportNumber = "";
        private string _PassportNumber_Not_Valid = "";
        private bool PassportNumber_Validation()
        {
            return true;
        }

        private string _passportNumberNote = "";
        public string PassportNumberNote
        {
            get { return _passportNumberNote; }
            set
            {
                _passportNumberNote = value;
                OnPropertyChanged("PassportNumberNote");
            }
        }

        private string _creatorOKPO = "";  //If change this change validation

        private void CreatorOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(CreatorOKPO));
        }

        [Attributes.FormVisual("ОКПО изготовителя")]
        public string CreatorOKPO
        {
            get { return _creatorOKPO; }
            set
            {
                _creatorOKPO = value;
                CreatorOKPO_Validation(value);
                OnPropertyChanged("CreatorOKPO");
            }
        }

        private string _type = "";

        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        private string _typeRecoded = "";
        public string TypeRecoded
        {
            get { return _typeRecoded; }
            set
            {
                _typeRecoded = value;
                OnPropertyChanged("TypeRecoded");
            }
        }

        private string _radionuclids = "";//If change this change validation

        private void Radionuclids_Validation(string value)//TODO
        {
            ClearErrors(nameof(Radionuclids));
        }

        [Attributes.FormVisual("Радионуклиды")]
        public string Radionuclids
        {
            get { return _radionuclids; }
            set
            {
                _radionuclids = value;
                Radionuclids_Validation(value);
                OnPropertyChanged("Radionuclids");
            }
        }

        private string _factoryNumber = "";//If change this change validation
        private void FactoryNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(FactoryNumber));
        }

        [Attributes.FormVisual("Заводской номер")]
        public string FactoryNumber
        {
            get { return _factoryNumber; }
            set
            {
                _factoryNumber = value;
                FactoryNumber_Validation(value);
                OnPropertyChanged("FactoryNumber");
            }
        }

        private string _factoryNumberRecoded = "";
        public string FactoryNumberRecoded
        {
            get { return _factoryNumberRecoded; }
            set
            {
                _factoryNumberRecoded = value;
                OnPropertyChanged("FactoryNumberRecoded");
            }
        }

        private string _activityOnCreation = "";
        public string ActivityOnCreation
        {
            get { return _activityOnCreation; }
            set
            {
                _activityOnCreation = value;
                OnPropertyChanged("ActivityOnCreation");
            }
        }

        private DateTime _creationDate = DateTime.MinValue;//If change this change validation

        private void CreationDate_Validation(DateTime value)//Ready
        {
            ClearErrors(nameof(CreationDate));
        }

        public DateTime CreationDate
        {
            get { return _creationDate; }
            set
            {
                _creationDate = value;
                CreationDate_Validation(value);
                OnPropertyChanged("CreationDate");
            }
        }

        private string _creatorOKPONote = "";
        public string CreatorOKPONote
        {
            get { return _creatorOKPONote; }
            set
            {
                _creatorOKPONote = value;
                OnPropertyChanged("CreatorOKPONote");
            }
        }

        private short _kategory = -1;
        public short Kategory
        {
            get { return _kategory; }
            set
            {
                _kategory = value;
                OnPropertyChanged("Kategory");
            }
        }

        private double _nuclearMaterialPresence = -1;
        public double NuclearMaterialPresence
        {
            get { return _nuclearMaterialPresence; }
            set
            {
                _nuclearMaterialPresence = value;
                OnPropertyChanged("NuclearMaterialPresence");
            }
        }

        private string _certificateId = "";
        public string CertificateId
        {
            get { return _certificateId; }
            set
            {
                _certificateId = value;
                OnPropertyChanged("CertificateId");
            }
        }

        private DateTime _validThru = DateTime.MinValue;
        public DateTime ValidThru
        {
            get { return _validThru; }
            set
            {
                _validThru = value;
                OnPropertyChanged("ValidThru");
            }
        }
    }
}
