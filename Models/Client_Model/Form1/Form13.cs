using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 1.3: Сведения об ОРИ в виде отдельных изделий")]
    public class Form13: Form1
    {
        public Form13(bool isSQL) : base()
        {
            FormNum = "13";
            NumberOfFields = 35;
            if (isSQL)
            {
                _AggregateState = new SQLite("AggregateState", FormNum, 0);
                _PassportNumber = new SQLite("PassportNumber", FormNum, 0);
                _PassportNumberRecoded = new SQLite("PassportNumberRecoded", FormNum, 0);
                _PassportNumberNote = new SQLite("PassportNumberNote", FormNum, 0);
                _Type = new SQLite("Type", FormNum, 0);
                _TypeRecoded = new SQLite("TypeRecoded", FormNum, 0);
                _Radionuclids = new SQLite("Radionuclids", FormNum, 0);
                _FactoryNumber = new SQLite("FactoryNumber", FormNum, 0);
                _FactoryNumberRecoded = new SQLite("FactoryNumberRecoded", FormNum, 0);
                _Activity = new SQLite("Activity", FormNum, 0);
                _ActivityNote = new SQLite("ActivityNote", FormNum, 0);
                _CreationDate = new SQLite("CreationDate", FormNum, 0);
                _CreatorOKPO = new SQLite("CreatorOKPO", FormNum, 0);
                _CreatorOKPONote = new SQLite("CreatorOKPONote", FormNum, 0);
                _PropertyCode = new SQLite("PropertyCode", FormNum, 0);
                _Owner = new SQLite("Owner", FormNum, 0);
                _ProviderOrRecieverOKPO = new SQLite("ProviderOrRecieverOKPO", FormNum, 0);
                _ProviderOrRecieverOKPONote = new SQLite("ProviderOrRecieverOKPONote", FormNum, 0);
                _TransporterOKPO = new SQLite("TransporterOKPO", FormNum, 0);
                _TransporterOKPONote = new SQLite("TransporterOKPONote", FormNum, 0);
                _PackName = new SQLite("PackName", FormNum, 0);
                _PackNameNote = new SQLite("PackNameNote", FormNum, 0);
                _PackType = new SQLite("PackType", FormNum, 0);
                _PackTypeRecoded = new SQLite("PackTypeRecoded", FormNum, 0);
                _PackTypeNote = new SQLite("PackTypeNote", FormNum, 0);
                _PackNumber = new SQLite("PackNumber", FormNum, 0);
                _PackNumberRecoded = new SQLite("PackNumberRecoded", FormNum, 0);
            }
            else
            {
                _AggregateState = new File();
                _PassportNumber = new File();
                _PassportNumberRecoded = new File();
                _PassportNumberNote = new File();
                _Type = new File();
                _TypeRecoded = new File();
                _Radionuclids = new File();
                _FactoryNumber = new File();
                _FactoryNumberRecoded = new File();
                _Activity = new File();
                _ActivityNote = new File();
                _CreationDate = new File();
                _CreatorOKPO = new File();
                _CreatorOKPONote = new File();
                _PropertyCode = new File();
                _Owner = new File();
                _ProviderOrRecieverOKPO = new File();
                _ProviderOrRecieverOKPONote = new File();
                _TransporterOKPO = new File();
                _TransporterOKPONote = new File();
                _PackName = new File();
                _PackNameNote = new File();
                _PackType = new File();
                _PackTypeRecoded = new File();
                _PackTypeNote = new File();
                _PackNumber = new File();
                _PackNumberRecoded = new File();
            }
        }

        [Attributes.FormVisual("Форма")]
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

        //PassportNumberRecoded property
        [Attributes.FormVisual("Номер упаковки")]
        public string PassportNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(PassportNumberRecoded)) != null)
                {
                    return (string)_PassportNumberRecoded.Get();
                }
                else
                {
                    return _PassportNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _PassportNumberRecoded_Not_Valid = value;
                if (GetErrors(nameof(PassportNumberRecoded)) != null)
                {
                    _PassportNumberRecoded.Set(_PassportNumberRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(PassportNumberRecoded));
            }
        }
        private IDataLoadEngine _PassportNumberRecoded;//If change this change validation
        private string _PassportNumberRecoded_Not_Valid = "";
        private void PassportNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(PassportNumberRecoded));
        }
        //PassportNumberRecoded property

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

        //ActivityNote property
        public string ActivityNote
        {
            get
            {
                if (GetErrors(nameof(ActivityNote)) != null)
                {
                    return (string)_ActivityNote.Get();
                }
                else
                {
                    return _ActivityNote_Not_Valid;
                }
            }
            set
            {
                _ActivityNote_Not_Valid = value;
                if (GetErrors(nameof(ActivityNote)) != null)
                {
                    _ActivityNote.Set(_ActivityNote_Not_Valid);
                }
                OnPropertyChanged(nameof(ActivityNote));
            }
        }
        private IDataLoadEngine _ActivityNote;//If change this change validation
        private string _ActivityNote_Not_Valid = "";
        private void ActivityNote_Validation(string value)//Ready
        {
            ClearErrors(nameof(ActivityNote));
        }
        //ActivityNote property

        //CreationDate property
        [Attributes.FormVisual("Дата изготовления")]
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

        //ProviderOrRecieverOKPONote property
        public string ProviderOrRecieverOKPONote
        {
            get
            {
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) != null)
                {
                    return (string)_ProviderOrRecieverOKPONote.Get();
                }
                else
                {
                    return _ProviderOrRecieverOKPONote_Not_Valid;
                }
            }
            set
            {
                _ProviderOrRecieverOKPONote_Not_Valid = value;
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) != null)
                {
                    _ProviderOrRecieverOKPONote.Set(_ProviderOrRecieverOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
            }
        }
        private IDataLoadEngine _ProviderOrRecieverOKPONote;
        private string _ProviderOrRecieverOKPONote_Not_Valid = "";
        private void ProviderOrRecieverOKPONote_Validation()
        {
            ClearErrors(nameof(ProviderOrRecieverOKPONote));
        }
        //ProviderOrRecieverOKPONote property

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

        //TransporterOKPONote property
        public string TransporterOKPONote
        {
            get
            {
                if (GetErrors(nameof(TransporterOKPONote)) != null)
                {
                    return (string)_TransporterOKPONote.Get();
                }
                else
                {
                    return _TransporterOKPONote_Not_Valid;
                }
            }
            set
            {
                _TransporterOKPONote_Not_Valid = value;
                if (GetErrors(nameof(TransporterOKPONote)) != null)
                {
                    _TransporterOKPONote.Set(_TransporterOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(TransporterOKPONote));
            }
        }
        private IDataLoadEngine _TransporterOKPONote;
        private string _TransporterOKPONote_Not_Valid = "";
        private void TransporterOKPONote_Validation()
        {
            ClearErrors(nameof(TransporterOKPONote));
        }
        //TransporterOKPONote property

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

        //PackNameNote property
        public string PackNameNote
        {
            get
            {
                if (GetErrors(nameof(PackNameNote)) != null)
                {
                    return (string)_PackNameNote.Get();
                }
                else
                {
                    return _PackNameNote_Not_Valid;
                }
            }
            set
            {
                _PackNameNote_Not_Valid = value;
                if (GetErrors(nameof(PackNameNote)) != null)
                {
                    _PackNameNote.Set(_PackNameNote_Not_Valid);
                }
                OnPropertyChanged(nameof(PackNameNote));
            }
        }
        private IDataLoadEngine _PackNameNote;
        private string _PackNameNote_Not_Valid = "";
        private void PackNameNote_Validation()
        {
            ClearErrors(nameof(PackNameNote));
        }
        //PackNameNote property

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

        //PackTypeRecoded property
        public string PackTypeRecoded
        {
            get
            {
                if (GetErrors(nameof(PackTypeRecoded)) != null)
                {
                    return (string)_PackTypeRecoded.Get();
                }
                else
                {
                    return _PackTypeRecoded_Not_Valid;
                }
            }
            set
            {
                _PackTypeRecoded_Not_Valid = value;
                if (GetErrors(nameof(PackTypeRecoded)) != null)
                {
                    _PackTypeRecoded.Set(_PackTypeRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }
        private IDataLoadEngine _PackTypeRecoded;
        private string _PackTypeRecoded_Not_Valid = "";
        private void PackTypeRecoded_Validation()
        {
            ClearErrors(nameof(PackTypeRecoded));
        }
        //PackTypeRecoded property

        //PackTypeNote property
        public string PackTypeNote
        {
            get
            {
                if (GetErrors(nameof(PackTypeNote)) != null)
                {
                    return (string)_PackTypeNote.Get();
                }
                else
                {
                    return _PackTypeNote_Not_Valid;
                }
            }
            set
            {
                _PackTypeNote_Not_Valid = value;
                if (GetErrors(nameof(PackTypeNote)) != null)
                {
                    _PackTypeNote.Set(_PackTypeNote_Not_Valid);
                }
                OnPropertyChanged(nameof(PackTypeNote));
            }
        }
        private IDataLoadEngine _PackTypeNote;
        private string _PackTypeNote_Not_Valid = "";
        private void PackTypeNote_Validation()
        {
            ClearErrors(nameof(PackTypeNote));
        }
        //PackTypeNote property

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

        //PackNumberRecoded property
        [Attributes.FormVisual("Номер упаковки")]
        public string PackNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(PackNumberRecoded)) != null)
                {
                    return (string)_PackNumberRecoded.Get();
                }
                else
                {
                    return _PackNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _PackNumberRecoded_Not_Valid = value;
                if (GetErrors(nameof(PackNumberRecoded)) != null)
                {
                    _PackNumberRecoded.Set(_PackNumberRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        private IDataLoadEngine _PackNumberRecoded;//If change this change validation
        private string _PackNumberRecoded_Not_Valid = "";
        private void PackNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackNumberRecoded));
        }
        //PackNumberRecoded property
    }
}
