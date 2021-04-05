using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 1.3: Сведения об ОРИ в виде отдельных изделий")]
    public class Form13: Form1
    {
        public Form13() : base()
        {
        }

        [Attributes.FormVisual("Форма")]
        public override string FormNum { get { return "13"; } }
        public override int NumberOfFields { get; } = 35;
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

        private string _passportNumberRecoded = "";
        public string PassportNumberRecoded
        {
            get { return _passportNumberRecoded; }
            set
            {
                _passportNumberRecoded = value;
                OnPropertyChanged("PassportNumberRecoded");
            }
        }

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

        //Activity property
        [Attributes.FormVisual("Активность, Бк")]
        public string Activity
        {
            get
            {
                if (GetErrors(nameof(Activity)) != null)
                {
                    return (string)_Activity.Get();
                }
                else
                {
                    return _Activity_Not_Valid;
                }
            }
            set
            {
                _Activity_Not_Valid = value;
                if (GetErrors(nameof(Activity)) != null)
                {
                    _Activity.Set(_Activity_Not_Valid);
                }
                OnPropertyChanged(nameof(Activity));
            }
        }
        private IDataLoadEngine _Activity;
        private string _Activity_Not_Valid = "";
        private void Activity_Validation(string value)//Ready
        {
            ClearErrors(nameof(Activity));
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(Activity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(Activity), "Недопустимое значение");
            }
        }
        //Activity property

        private string _activityNote = "";
        public string ActivityNote
        {
            get { return _activityNote; }
            set
            {
                _activityNote = value;
                OnPropertyChanged("ActivityNote");
            }
        }

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

        //AggregateState property
        [Attributes.FormVisual("Агрегатное состояние")]
        public byte AggregateState//1 2 3
        {
            get
            {
                if (GetErrors(nameof(AggregateState)) != null)
                {
                    return (byte)_AggregateState.Get();
                }
                else
                {
                    return _AggregateState_Not_Valid;
                }
            }
            set
            {
                _AggregateState_Not_Valid = value;
                if (GetErrors(nameof(AggregateState)) != null)
                {
                    _AggregateState.Set(_AggregateState_Not_Valid);
                }
                OnPropertyChanged(nameof(AggregateState));
            }
        }
        private IDataLoadEngine _AggregateState;
        private byte _AggregateState_Not_Valid = 0;
        private void AggregateState_Validation(byte value)//Ready
        {
            ClearErrors(nameof(AggregateState));
            if ((value != 1) && (value != 2) && (value != 3))
                AddError(nameof(AggregateState), "Недопустимое значение");
        }
        //AggregateState property

        //PropertyCode property
        [Attributes.FormVisual("Код собственности")]
        public byte PropertyCode
        {
            get
            {
                if (GetErrors(nameof(PropertyCode)) != null)
                {
                    return (byte)_PropertyCode.Get();
                }
                else
                {
                    return _PropertyCode_Not_Valid;
                }
            }
            set
            {
                _PropertyCode_Not_Valid = value;
                if (GetErrors(nameof(PropertyCode)) != null)
                {
                    _PropertyCode.Set(_PropertyCode_Not_Valid);
                }
                OnPropertyChanged(nameof(PropertyCode));
            }
        }
        private IDataLoadEngine _PropertyCode;
        private byte _PropertyCode_Not_Valid = 255;
        private void PropertyCode_Validation(byte value)//Ready
        {
            ClearErrors(nameof(PropertyCode));
            if (!((value >= 1) && (value <= 9)))
                AddError(nameof(PropertyCode), "Недопустимое значение");
        }
        //PropertyCode property

        //Owner property
        [Attributes.FormVisual("Владелец")]
        public string Owner
        {
            get
            {
                if (GetErrors(nameof(Owner)) != null)
                {
                    return (string)_Owner.Get();
                }
                else
                {
                    return _Owner_Not_Valid;
                }
            }
            set
            {
                _Owner_Not_Valid = value;
                if (GetErrors(nameof(Owner)) != null)
                {
                    _Owner.Set(_Owner_Not_Valid);
                }
                OnPropertyChanged(nameof(Owner));
            }
        }
        private IDataLoadEngine _Owner;//if change this change validation
        private string _Owner_Not_Valid = "";
        private void Owner_Validation(string value)//Ready
        {
            ClearErrors(nameof(Owner));
        }
        //Owner property

        //ProviderOrRecieverOKPO property
        [Attributes.FormVisual("ОКПО поставщика/получателя")]
        public string ProviderOrRecieverOKPO
        {
            get
            {
                if (GetErrors(nameof(ProviderOrRecieverOKPO)) != null)
                {
                    return (string)_ProviderOrRecieverOKPO.Get();
                }
                else
                {
                    return _ProviderOrRecieverOKPO_Not_Valid;
                }
            }
            set
            {
                _ProviderOrRecieverOKPO_Not_Valid = value;
                if (GetErrors(nameof(ProviderOrRecieverOKPO)) != null)
                {
                    _ProviderOrRecieverOKPO.Set(_ProviderOrRecieverOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }
        private IDataLoadEngine _ProviderOrRecieverOKPO;
        private string _ProviderOrRecieverOKPO_Not_Valid = "";
        private void ProviderOrRecieverOKPO_Validation()//TODO
        {
            ClearErrors(nameof(ProviderOrRecieverOKPO));
        }
        //ProviderOrRecieverOKPO property

        private string _providerOrRecieverOKPONote = "";
        public string ProviderOrRecieverOKPONote
        {
            get { return _providerOrRecieverOKPONote; }
            set
            {
                _providerOrRecieverOKPONote = value;
                OnPropertyChanged("ProviderOrRecieverOKPONote");
            }
        }

        //TransporterOKPO property
        [Attributes.FormVisual("ОКПО перевозчика")]
        public string TransporterOKPO
        {
            get
            {
                if (GetErrors(nameof(TransporterOKPO)) != null)
                {
                    return (string)_TransporterOKPO.Get();
                }
                else
                {
                    return _TransporterOKPO_Not_Valid;
                }
            }
            set
            {
                _TransporterOKPO_Not_Valid = value;
                if (GetErrors(nameof(TransporterOKPO)) != null)
                {
                    _TransporterOKPO.Set(_TransporterOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }
        private IDataLoadEngine _TransporterOKPO;
        private string _TransporterOKPO_Not_Valid = "";
        private void TransporterOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(TransporterOKPO));
        }
        //TransporterOKPO property

        private string _transporterOKPONote = "";
        public string TransporterOKPONote
        {
            get { return _transporterOKPONote; }
            set
            {
                _transporterOKPONote = value;
                OnPropertyChanged("TransporterOKPONote");
            }
        }

        //PackName property
        [Attributes.FormVisual("Наименование упаковки")]
        public string PackName
        {
            get
            {
                if (GetErrors(nameof(PackName)) != null)
                {
                    return (string)_PackName.Get();
                }
                else
                {
                    return _PackName_Not_Valid;
                }
            }
            set
            {
                _PackName_Not_Valid = value;
                if (GetErrors(nameof(PackName)) != null)
                {
                    _PackName.Set(_PackName_Not_Valid);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }
        private IDataLoadEngine _PackName;
        private string _PackName_Not_Valid = "";
        private void PackName_Validation()
        {
            ClearErrors(nameof(PackName));
        }
        //PackName property

        private string _packNameNote = "";
        public string PackNameNote
        {
            get { return _packNameNote; }
            set
            {
                _packNameNote = value;
                OnPropertyChanged("PackNameNote");
            }
        }

        //PackType property
        [Attributes.FormVisual("Тип упаковки")]
        public string PackType
        {
            get
            {
                if (GetErrors(nameof(PackType)) != null)
                {
                    return (string)_PackType.Get();
                }
                else
                {
                    return _PackType_Not_Valid;
                }
            }
            set
            {
                _PackType_Not_Valid = value;
                if (GetErrors(nameof(PackType)) != null)
                {
                    _PackType.Set(_PackType_Not_Valid);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        private IDataLoadEngine _PackType;//If change this change validation
        private string _PackType_Not_Valid = "";
        private void PackType_Validation()//Ready
        {
            ClearErrors(nameof(PackType));
        }
        //PackType property

        private string _packTypeRecoded = "";
        public string PackTypeRecoded
        {
            get { return _packTypeRecoded; }
            set
            {
                _packTypeRecoded = value;
                OnPropertyChanged("PackTypeRecoded");
            }
        }

        private string _packTypeNote = "";
        public string PackTypeNote
        {
            get { return _packTypeNote; }
            set
            {
                _packTypeNote = value;
                OnPropertyChanged("PackTypeNote");
            }
        }

        //PackNumber property
        [Attributes.FormVisual("Номер упаковки")]
        public string PackNumber
        {
            get
            {
                if (GetErrors(nameof(PackNumber)) != null)
                {
                    return (string)_PackNumber.Get();
                }
                else
                {
                    return _PackNumber_Not_Valid;
                }
            }
            set
            {
                _PackNumber_Not_Valid = value;
                if (GetErrors(nameof(PackNumber)) != null)
                {
                    _PackNumber.Set(_PackNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PackNumber));
            }
        }
        private IDataLoadEngine _PackNumber;//If change this change validation
        private string _PackNumber_Not_Valid = "";
        private void PackNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackNumber));
        }
        //PackNumber property

        private string _packNumberRecoded = "";
        public string PackNumberRecoded
        {
            get { return _packNumberRecoded; }
            set
            {
                _packNumberRecoded = value;
                OnPropertyChanged("PackNumberRecoded");
            }
        }
    }
}
