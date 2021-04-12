using System;
using System.Globalization;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("")]
    public class Form32_1: Abstracts.Form3
    {
        public Form32_1(int RowID) : base(RowID)
        {
            FormNum = "32_1";
            NumberOfFields = 15;
        }

        [Attributes.Form_Property("Форма")]
        public override void Object_Validation()
        {

        }

        //PassportNumber property
        [Attributes.Form_Property("Номер паспорта")]
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

        //PassportNumberNote property
        public string PassportNumberNote
        {
            get
            {
                if (GetErrors(nameof(PassportNumberNote)) != null)
                {
                    return (string)_PassportNumberNote.Get();
                }
                else
                {
                    return _PassportNumberNote_Not_Valid;
                }
            }
            set
            {
                _PassportNumberNote_Not_Valid = value;
                if (GetErrors(nameof(PassportNumberNote)) != null)
                {
                    _PassportNumberNote.Set(_PassportNumberNote_Not_Valid);
                }
                OnPropertyChanged(nameof(PassportNumberNote));
            }
        }
        private IDataLoadEngine _PassportNumberNote;
        private string _PassportNumberNote_Not_Valid = "";
        private void PassportNumberNote_Validation()
        {
            ClearErrors(nameof(PassportNumberNote));
        }
        //PassportNumberNote property

        //CreatorOKPO property
        [Attributes.Form_Property("ОКПО изготовителя")]
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
        [Attributes.Form_Property("Тип")]
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

        //TypeRecoded property
        public string TypeRecoded
        {
            get
            {
                if (GetErrors(nameof(TypeRecoded)) != null)
                {
                    return (string)_TypeRecoded.Get();
                }
                else
                {
                    return _TypeRecoded_Not_Valid;
                }
            }
            set
            {
                _TypeRecoded_Not_Valid = value;
                if (GetErrors(nameof(TypeRecoded)) != null)
                {
                    _TypeRecoded.Set(_TypeRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(TypeRecoded));
            }
        }
        private IDataLoadEngine _TypeRecoded;
        private string _TypeRecoded_Not_Valid = "";
        private void TypeRecoded_Validation()
        {
            ClearErrors(nameof(TypeRecoded));
        }
        //TypeRecoded property

        //Radionuclids property
        [Attributes.Form_Property("Радионуклиды")]
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
        [Attributes.Form_Property("Заводской номер")]
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

        //FactoryNumberRecoded property
        public string FactoryNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(FactoryNumberRecoded)) != null)
                {
                    return (string)_FactoryNumberRecoded.Get();
                }
                else
                {
                    return _FactoryNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _FactoryNumberRecoded_Not_Valid = value;
                if (GetErrors(nameof(FactoryNumberRecoded)) != null)
                {
                    _FactoryNumberRecoded.Set(_FactoryNumberRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(FactoryNumberRecoded));
            }
        }
        private IDataLoadEngine _FactoryNumberRecoded;//If change this change validation
        private string _FactoryNumberRecoded_Not_Valid = "";
        private void FactoryNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(FactoryNumberRecoded));
        }
        //FactoryNumberRecoded property

        //ActivityOnCreation property
        [Attributes.Form_Property("Активность на дату создания, Бк")]
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
        [Attributes.Form_Property("Дата изготовления")]
        public DateTimeOffset CreationDate
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
        private DateTimeOffset _CreationDate_Not_Valid = DateTimeOffset.MinValue;
        private void CreationDate_Validation(DateTimeOffset value)//Ready
        {
            ClearErrors(nameof(CreationDate));
        }
        //CreationDate property

        //CreatorOKPONote property
        public string CreatorOKPONote
        {
            get
            {
                if (GetErrors(nameof(CreatorOKPONote)) != null)
                {
                    return (string)_CreatorOKPONote.Get();
                }
                else
                {
                    return _CreatorOKPONote_Not_Valid;
                }
            }
            set
            {
                _CreatorOKPONote_Not_Valid = value;
                if (GetErrors(nameof(CreatorOKPONote)) != null)
                {
                    _CreatorOKPONote.Set(_CreatorOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(CreatorOKPONote));
            }
        }
        private IDataLoadEngine _CreatorOKPONote;
        private string _CreatorOKPONote_Not_Valid = "";
        private void CreatorOKPONote_Validation()
        {
            ClearErrors(nameof(CreatorOKPONote));
        }
        //CreatorOKPONote property

        //Kategory property
        [Attributes.Form_Property("Категория")]
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
        [Attributes.Form_Property("Содержание ядерных материалов")]
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
        [Attributes.Form_Property("Номер сертификата")]
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
        [Attributes.Form_Property("Действует по")]
        public DateTimeOffset ValidThru
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
        private DateTimeOffset _ValidThru_Not_Valid = DateTimeOffset.MinValue;
        //ValidThru property
    }
}
