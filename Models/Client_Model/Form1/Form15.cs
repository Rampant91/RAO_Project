using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 1.5: Сведения о РАО в виде отработавших ЗРИ")]
    public class Form15: Form1
    {
        public Form15(bool isSQL) : base()
        {
            FormNum = "15";
            NumberOfFields = 38;
            if (isSQL)
            {
                _StoragePlaceCode = new SQLite("StoragePlaceCode", FormNum, 0);
                _StoragePlaceName = new SQLite("StoragePlaceName", FormNum, 0);
                _Subsidy = new SQLite("Subsidy", FormNum, 0);
                _StoragePlaceNameNote = new SQLite("StoragePlaceNameNote", FormNum, 0);
                _StatusRAO = new SQLite("StatusRAO", FormNum, 0);
                _RefineOrSortRAOCode = new SQLite("RefineOrSortRAOCode", FormNum, 0);
                _FcpNumber = new SQLite("FcpNumber", FormNum, 0);
                _Radionuclids = new SQLite("Radionuclids", FormNum, 0);
                _PassportNumber = new SQLite("PassportNumber", FormNum, 0);
                _PassportNumberRecoded = new SQLite("PassportNumberRecoded", FormNum, 0);
                _PassportNumberNote = new SQLite("PassportNumberNote", FormNum, 0);
                _Type = new SQLite("Type", FormNum, 0);
                _TypeRecoded = new SQLite("TypeRecoded", FormNum, 0);
                _Radionuclids = new SQLite("Radionuclids", FormNum, 0);
                _FactoryNumber = new SQLite("FactoryNumber", FormNum, 0);
                _FactoryNumberRecoded = new SQLite("FactoryNumberRecoded", FormNum, 0);
                _Quantity = new SQLite("Quantity", FormNum, 0);
                _Activity = new SQLite("Activity", FormNum, 0);
                _CreationDate = new SQLite("CreationDate", FormNum, 0);
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
                _StoragePlaceCode = new File();
                _StoragePlaceName = new File();
                _Subsidy = new File();
                _StoragePlaceNameNote = new File();
                _StatusRAO = new File();
                _RefineOrSortRAOCode = new File();
                _FcpNumber = new File();
                _Radionuclids = new File();
                _PassportNumber = new File();
                _PassportNumberRecoded = new File();
                _PassportNumberNote = new File();
                _Type = new File();
                _TypeRecoded = new File();
                _Radionuclids = new File();
                _FactoryNumber = new File();
                _FactoryNumberRecoded = new File();
                _Quantity = new File();
                _Activity = new File();
                _CreationDate = new File();
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

        //Quantity property
        [Attributes.FormVisual("Количество, шт.")]
        public int Quantity
        {
            get
            {
                if (GetErrors(nameof(Quantity)) != null)
                {
                    return (int)_Quantity.Get();
                }
                else
                {
                    return _Quantity_Not_Valid;
                }
            }
            set
            {
                _Quantity_Not_Valid = value;
                if (GetErrors(nameof(Quantity)) != null)
                {
                    _Quantity.Set(_Quantity_Not_Valid);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        private IDataLoadEngine _Quantity;  // positive int.
        private int _Quantity_Not_Valid = -1;
        private void Quantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(Quantity));
            if (value <= 0)
                AddError(nameof(Quantity), "Недопустимое значение");
        }
        //Quantity property

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
            string tmp = value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(Activity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(Activity), "Недопустимое значение");
            }
        }
        //Activity property

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

        //StatusRAO property
        [Attributes.FormVisual("Статус РАО")]
        public string StatusRAO  //1 cyfer or OKPO.
        {
            get
            {
                if (GetErrors(nameof(StatusRAO)) != null)
                {
                    return (string)_StatusRAO.Get();
                }
                else
                {
                    return _StatusRAO_Not_Valid;
                }
            }
            set
            {
                _StatusRAO_Not_Valid = value;
                if (GetErrors(nameof(StatusRAO)) != null)
                {
                    _StatusRAO.Set(_StatusRAO_Not_Valid);
                }
                OnPropertyChanged(nameof(StatusRAO));
            }
        }
        private IDataLoadEngine _StatusRAO;
        private string _StatusRAO_Not_Valid = "";
        private void StatusRAO_Validation(string value)//TODO
        {
            ClearErrors(nameof(StatusRAO));
        }
        //StatusRAO property

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

        //StoragePlaceName property
        [Attributes.FormVisual("Наименование ПХ")]
        public string StoragePlaceName
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceName)) != null)
                {
                    return (string)_StoragePlaceName.Get();
                }
                else
                {
                    return _StoragePlaceName_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceName_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceName)) != null)
                {
                    _StoragePlaceName.Set(_StoragePlaceName_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        private IDataLoadEngine _StoragePlaceName;//If change this change validation
        private string _StoragePlaceName_Not_Valid = "";
        private void StoragePlaceName_Validation(string value)//Ready
        {
            ClearErrors(nameof(StoragePlaceName));
        }
        //StoragePlaceName property

        //StoragePlaceNameNote property
        public string StoragePlaceNameNote
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceNameNote)) != null)
                {
                    return (string)_StoragePlaceNameNote.Get();
                }
                else
                {
                    return _StoragePlaceNameNote_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceNameNote_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceNameNote)) != null)
                {
                    _StoragePlaceNameNote.Set(_StoragePlaceNameNote_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceNameNote));
            }
        }
        private IDataLoadEngine _StoragePlaceNameNote;//If change this change validation
        private string _StoragePlaceNameNote_Not_Valid = "";
        private void StoragePlaceNameNote_Validation(string value)//Ready
        {
            ClearErrors(nameof(StoragePlaceNameNote));
        }
        //StoragePlaceNameNote property

        //StoragePlaceCode property
        [Attributes.FormVisual("Код ПХ")]
        public string StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceCode)) != null)
                {
                    return (string)_StoragePlaceCode.Get();
                }
                else
                {
                    return _StoragePlaceCode_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceCode_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceCode)) != null)
                {
                    _StoragePlaceCode.Set(_StoragePlaceCode_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        private IDataLoadEngine _StoragePlaceCode;//if change this change validation
        private string _StoragePlaceCode_Not_Valid = "";
        private void StoragePlaceCode_Validation(string value)//TODO
        {
            ClearErrors(nameof(StoragePlaceCode));
            if (!(value == "-"))
                if (value.Length != 8)
                    AddError(nameof(StoragePlaceCode), "Недопустимое значение");
                else
                    for (int i = 0; i < 8; i++)
                    {
                        if (!((value[i] >= '0') && (value[i] <= '9')))
                        {
                            AddError(nameof(StoragePlaceCode), "Недопустимое значение");
                            return;
                        }
                    }
        }
        //StoragePlaceCode property

        //RefineOrSortRAOCode property
        [Attributes.FormVisual("Код переработки/сортировки РАО")]
        public string RefineOrSortRAOCode //2 cyfer code or empty.
        {
            get
            {
                if (GetErrors(nameof(RefineOrSortRAOCode)) != null)
                {
                    return (string)_RefineOrSortRAOCode.Get();
                }
                else
                {
                    return _RefineOrSortRAOCode_Not_Valid;
                }
            }
            set
            {
                _RefineOrSortRAOCode_Not_Valid = value;
                if (GetErrors(nameof(RefineOrSortRAOCode)) != null)
                {
                    _RefineOrSortRAOCode.Set(_RefineOrSortRAOCode_Not_Valid);
                }
                OnPropertyChanged(nameof(RefineOrSortRAOCode));
            }
        }
        private IDataLoadEngine _RefineOrSortRAOCode;//If change this change validation
        private string _RefineOrSortRAOCode_Not_Valid = "";
        private void RefineOrSortRAOCode_Validation(string value)//TODO
        {
            ClearErrors(nameof(RefineOrSortRAOCode));
            if (value.Length != 2)
                AddError(nameof(RefineOrSortRAOCode), "Недопустимое значение");
            else
                for (int i = 0; i < 2; i++)
                {
                    if (!((value[i] >= '0') && (value[i] <= '9')))
                    {
                        AddError(nameof(RefineOrSortRAOCode), "Недопустимое значение");
                        return;
                    }
                }
        }
        //RefineOrSortRAOCode property

        //Subsidy property
        [Attributes.FormVisual("Субсидия, %")]
        public string Subsidy // 0<number<=100 or empty.
        {
            get
            {
                if (GetErrors(nameof(Subsidy)) != null)
                {
                    return (string)_Subsidy.Get();
                }
                else
                {
                    return _Subsidy_Not_Valid;
                }
            }
            set
            {
                _Subsidy_Not_Valid = value;
                if (GetErrors(nameof(Subsidy)) != null)
                {
                    _Subsidy.Set(_Subsidy_Not_Valid);
                }
                OnPropertyChanged(nameof(Subsidy));
            }
        }
        private IDataLoadEngine _Subsidy;
        private string _Subsidy_Not_Valid = "";
        private void Subsidy_Validation(string value)//Ready
        {
            ClearErrors(nameof(Subsidy));
            try
            {
                int tmp = Int32.Parse(value);
                if (!((tmp > 0) && (tmp <= 100)))
                    AddError(nameof(Subsidy), "Недопустимое значение");
            }
            catch
            {
                AddError(nameof(Subsidy), "Недопустимое значение");
            }
        }
        //Subsidy property

        //FcpNumber property
        [Attributes.FormVisual("Номер мероприятия ФЦП")]
        public string FcpNumber
        {
            get
            {
                if (GetErrors(nameof(FcpNumber)) != null)
                {
                    return (string)_FcpNumber.Get();
                }
                else
                {
                    return _FcpNumber_Not_Valid;
                }
            }
            set
            {
                _FcpNumber_Not_Valid = value;
                if (GetErrors(nameof(FcpNumber)) != null)
                {
                    _FcpNumber.Set(_FcpNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }
        private IDataLoadEngine _FcpNumber;
        private string _FcpNumber_Not_Valid = "";
        private void FcpNuber_Validation(string value)//TODO
        {
            ClearErrors(nameof(FcpNumber));
        }
        //FcpNumber property
    }
}
