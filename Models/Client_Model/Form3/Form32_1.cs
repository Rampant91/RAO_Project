using System;
using System.Globalization;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("")]
    public class Form32_1: Form3
    {
        public Form32_1() : base()
        {
        }

        [Attributes.FormVisual("Форма")]
        public override string FormNum { get { return "32_1"; } }
        public override int NumberOfFields { get; } = 15;
        public override void Object_Validation()
        {

        }

        //PassportNumber property
        [Attributes.FormVisual("Номер паспорта")]
        public string PassportNumber
        {
            get
            {
                if (GetErrors(nameof(PassportNumber)) != null)
                {
                    return (string)_PassportNumber.Get();
                }
                else
                {
                    return _PassportNumber_Not_Valid;
                }
            }
            set
            {
                _PassportNumber_Not_Valid = value;
                if (GetErrors(nameof(PassportNumber)) != null)
                {
                    _PassportNumber.Set(_PassportNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PassportNumber));
            }
        }
        private IDataLoadEngine _PassportNumber;
        private string _PassportNumber_Not_Valid = "";
        private void PassportNumber_Validation()
        {
            ClearErrors(nameof(PassportNumber));
        }
        //PassportNumber property

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

        //CreatorOKPO property
        [Attributes.FormVisual("ОКПО изготовителя")]
        public string CreatorOKPO
        {
            get
            {
                if (GetErrors(nameof(CreatorOKPO)) != null)
                {
                    return (string)_CreatorOKPO.Get();
                }
                else
                {
                    return _CreatorOKPO_Not_Valid;
                }
            }
            set
            {
                _CreatorOKPO_Not_Valid = value;
                if (GetErrors(nameof(CreatorOKPO)) != null)
                {
                    _CreatorOKPO.Set(_CreatorOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(CreatorOKPO));
            }
        }
        private IDataLoadEngine _CreatorOKPO;  //If change this change validation
        private string _CreatorOKPO_Not_Valid = "";
        private void CreatorOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(CreatorOKPO));
        }
        //CreatorOKPO property

        //Type property
        [Attributes.FormVisual("Тип")]
        public string Type
        {
            get
            {
                if (GetErrors(nameof(Type)) != null)
                {
                    return (string)_Type.Get();
                }
                else
                {
                    return _Type_Not_Valid;
                }
            }
            set
            {
                _Type_Not_Valid = value;
                if (GetErrors(nameof(Type)) != null)
                {
                    _Type.Set(_Type_Not_Valid);
                }
                OnPropertyChanged(nameof(Type));
            }
        }
        private IDataLoadEngine _Type;
        private string _Type_Not_Valid = "";
        private void Type_Validation()
        {
            ClearErrors(nameof(Type));
        }
        //Type property

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

        //Radionuclids property
        [Attributes.FormVisual("Радионуклиды")]
        public string Radionuclids
        {
            get
            {
                if (GetErrors(nameof(Radionuclids)) != null)
                {
                    return (string)_Radionuclids.Get();
                }
                else
                {
                    return _Radionuclids_Not_Valid;
                }
            }
            set
            {
                _Radionuclids_Not_Valid = value;
                if (GetErrors(nameof(Radionuclids)) != null)
                {
                    _Radionuclids.Set(_Radionuclids_Not_Valid);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        private IDataLoadEngine _Radionuclids;//If change this change validation
        private string _Radionuclids_Not_Valid = "";
        private void Radionuclids_Validation()//TODO
        {
            ClearErrors(nameof(Radionuclids));
        }
        //Radionuclids property

        //FactoryNumber property
        [Attributes.FormVisual("Заводской номер")]
        public string FactoryNumber
        {
            get
            {
                if (GetErrors(nameof(FactoryNumber)) != null)
                {
                    return (string)_FactoryNumber.Get();
                }
                else
                {
                    return _FactoryNumber_Not_Valid;
                }
            }
            set
            {
                _FactoryNumber_Not_Valid = value;
                if (GetErrors(nameof(FactoryNumber)) != null)
                {
                    _FactoryNumber.Set(_FactoryNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(FactoryNumber));
            }
        }
        private IDataLoadEngine _FactoryNumber;
        private string _FactoryNumber_Not_Valid = "";
        private void FactoryNumber_Validation()
        {
            ClearErrors(nameof(FactoryNumber));
        }
        //FactoryNumber property

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

        //ActivityOnCreation property
        [Attributes.FormVisual("Активность на дату создания, Бк")]
        public string ActivityOnCreation
        {
            get
            {
                if (GetErrors(nameof(ActivityOnCreation)) != null)
                {
                    return (string)_ActivityOnCreation.Get();
                }
                else
                {
                    return _ActivityOnCreation_Not_Valid;
                }
            }
            set
            {
                _ActivityOnCreation_Not_Valid = value;
                if (GetErrors(nameof(ActivityOnCreation)) != null)
                {
                    _ActivityOnCreation.Set(_ActivityOnCreation_Not_Valid);
                }
                OnPropertyChanged(nameof(ActivityOnCreation));
            }
        }
        private IDataLoadEngine _ActivityOnCreation;
        private string _ActivityOnCreation_Not_Valid = "";
        private void ActivityOnCreation_Validation(string value)//Ready
        {
            ClearErrors(nameof(ActivityOnCreation));
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(ActivityOnCreation), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(ActivityOnCreation), "Недопустимое значение");
            }
        }
        //ActivityOnCreation property

        //CreationDate property
        [Attributes.FormVisual("Дата изготовления")]
        public DateTime CreationDate
        {
            get
            {
                if (GetErrors(nameof(CreationDate)) != null)
                {
                    return (DateTime)_CreationDate.Get();
                }
                else
                {
                    return _CreationDate_Not_Valid;
                }
            }
            set
            {
                _CreationDate_Not_Valid = value;
                if (GetErrors(nameof(CreationDate)) != null)
                {
                    _CreationDate.Set(_CreationDate_Not_Valid);
                }
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        private IDataLoadEngine _CreationDate;//If change this change validation
        private DateTime _CreationDate_Not_Valid = DateTime.MinValue;
        private void CreationDate_Validation(DateTime value)//Ready
        {
            ClearErrors(nameof(CreationDate));
        }
        //CreationDate property

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

        //Kategory property
        [Attributes.FormVisual("Категория")]
        public short Kategory
        {
            get
            {
                if (GetErrors(nameof(Kategory)) != null)
                {
                    return (short)_Kategory.Get();
                }
                else
                {
                    return _Kategory_Not_Valid;
                }
            }
            set
            {
                _Kategory_Not_Valid = value;
                if (GetErrors(nameof(Kategory)) != null)
                {
                    _Kategory.Set(_Kategory_Not_Valid);
                }
                OnPropertyChanged(nameof(Kategory));
            }
        }
        private IDataLoadEngine _Kategory;
        private short _Kategory_Not_Valid = -1;
        private void Kategory_Validation(short value)//TODO
        {
            ClearErrors(nameof(Kategory));
        }
        //Kategory property

        //NuclearMaterialPresence property
        [Attributes.FormVisual("Содержание ядерных материалов")]
        public double NuclearMaterialPresence
        {
            get
            {
                if (GetErrors(nameof(NuclearMaterialPresence)) != null)
                {
                    return (double)_NuclearMaterialPresence.Get();
                }
                else
                {
                    return _NuclearMaterialPresence_Not_Valid;
                }
            }
            set
            {
                _NuclearMaterialPresence_Not_Valid = value;
                if (GetErrors(nameof(NuclearMaterialPresence)) != null)
                {
                    _NuclearMaterialPresence.Set(_NuclearMaterialPresence_Not_Valid);
                }
                OnPropertyChanged(nameof(NuclearMaterialPresence));
            }
        }
        private IDataLoadEngine _NuclearMaterialPresence;
        private double _NuclearMaterialPresence_Not_Valid = -1;
        //NuclearMaterialPresence property

        //CertificateId property
        [Attributes.FormVisual("Номер сертификата")]
        public string CertificateId
        {
            get
            {
                if (GetErrors(nameof(CertificateId)) != null)
                {
                    return (string)_CertificateId.Get();
                }
                else
                {
                    return _CertificateId_Not_Valid;
                }
            }
            set
            {
                _CertificateId_Not_Valid = value;
                if (GetErrors(nameof(CertificateId)) != null)
                {
                    _CertificateId.Set(_CertificateId_Not_Valid);
                }
                OnPropertyChanged(nameof(CertificateId));
            }
        }
        private IDataLoadEngine _CertificateId;
        private string _CertificateId_Not_Valid = "";
        //CertificateId property

        //ValidThru property
        [Attributes.FormVisual("Действует по")]
        public DateTime ValidThru
        {
            get
            {
                if (GetErrors(nameof(ValidThru)) != null)
                {
                    return (DateTime)_ValidThru.Get();
                }
                else
                {
                    return _ValidThru_Not_Valid;
                }
            }
            set
            {
                _ValidThru_Not_Valid = value;
                if (GetErrors(nameof(ValidThru)) != null)
                {
                    _ValidThru.Set(_ValidThru_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidThru));
            }
        }
        private IDataLoadEngine _ValidThru;
        private DateTime _ValidThru_Not_Valid = DateTime.MinValue;
        //ValidThru property
    }
}
