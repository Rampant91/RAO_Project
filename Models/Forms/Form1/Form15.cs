using System;
using System.Text.RegularExpressions;
using System.Globalization;
using DBRealization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.5: Сведения о РАО в виде отработавших ЗРИ")]
    public class Form15: Abstracts.Form1
    {
        public static string SQLCommandParams()
        {
            return
                Abstracts.Form1.SQLCommandParamsBase() +
                nameof(PassportNumber) + SQLconsts.strNotNullDeclaration +
                nameof(PassportNumberNote) + SQLconsts.strNotNullDeclaration +
                nameof(PassportNumberRecoded) + SQLconsts.strNotNullDeclaration +
                nameof(StatusRAO) + SQLconsts.strNotNullDeclaration +
                nameof(StoragePlaceName) + SQLconsts.strNotNullDeclaration +
                nameof(StoragePlaceNameNote) + SQLconsts.strNotNullDeclaration +
                nameof(StoragePlaceCode) + SQLconsts.strNotNullDeclaration +
                nameof(RefineOrSortRAOCode) + SQLconsts.strNotNullDeclaration +
                nameof(Subsidy) + SQLconsts.strNotNullDeclaration +
                nameof(FcpNumber) + SQLconsts.strNotNullDeclaration +
                nameof(Quantity) + SQLconsts.intNotNullDeclaration +
                nameof(Type) + SQLconsts.strNotNullDeclaration +
                nameof(FactoryNumber) + SQLconsts.strNotNullDeclaration +
                nameof(FactoryNumberRecoded) + SQLconsts.strNotNullDeclaration +
                nameof(CreationDate) + SQLconsts.dateNotNullDeclaration +
                nameof(TypeRecoded) + SQLconsts.strNotNullDeclaration +
                nameof(ProviderOrRecieverOKPO) + SQLconsts.strNotNullDeclaration +
                nameof(ProviderOrRecieverOKPONote) + SQLconsts.strNotNullDeclaration +
                nameof(TransporterOKPO) + SQLconsts.strNotNullDeclaration +
                nameof(TransporterOKPONote) + SQLconsts.strNotNullDeclaration +
                nameof(PackName) + SQLconsts.strNotNullDeclaration +
                nameof(PackNameNote) + SQLconsts.strNotNullDeclaration +
                nameof(PackType) + SQLconsts.strNotNullDeclaration +
                nameof(PackTypeRecoded) + SQLconsts.strNotNullDeclaration +
                nameof(PackTypeNote) + SQLconsts.strNotNullDeclaration +
                nameof(PackNumber) + SQLconsts.strNotNullDeclaration +
                nameof(PackNumberRecoded) + SQLconsts.strNotNullDeclaration +
                nameof(Radionuclids) + SQLconsts.strNotNullDeclaration +
                nameof(Activity) + " varchar(255) not null";
        }
        public Form15(IDataAccess Access) : base(Access)
        {
            FormNum = "15";
            NumberOfFields = 37;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //PassportNumber property
        [Attributes.Form_Property("Номер паспорта")]
        public string PassportNumber
        {
            get
            {
                if (GetErrors(nameof(PassportNumber)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PassportNumber));
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
                    _dataAccess.Set(nameof(PassportNumber), _PassportNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PassportNumber));
            }
        }
        
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
                    return (string)_dataAccess.Get(nameof(PassportNumberNote));
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
                    _dataAccess.Set(nameof(PassportNumberNote), _PassportNumberNote_Not_Valid);                }
                OnPropertyChanged(nameof(PassportNumberNote));
            }
        }
        
        private string _PassportNumberNote_Not_Valid = "";
        private void PassportNumberNote_Validation()
        {
            ClearErrors(nameof(PassportNumberNote));
        }
        //PassportNumberNote property

        //PassportNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public string PassportNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(PassportNumberRecoded)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PassportNumberRecoded));                }
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
                    _dataAccess.Set(nameof(PassportNumberRecoded), _PassportNumberRecoded_Not_Valid);                }
                OnPropertyChanged(nameof(PassportNumberRecoded));
            }
        }
        //If change this change validation
        private string _PassportNumberRecoded_Not_Valid = "";
        private void PassportNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(PassportNumberRecoded));
        }
        //PassportNumberRecoded property

        //Type property
        [Attributes.Form_Property("Тип")]
        public string Type
        {
            get
            {
                if (GetErrors(nameof(Type)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Type));                }
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
                    _dataAccess.Set(nameof(Type), _Type_Not_Valid);                }
                OnPropertyChanged(nameof(Type));
            }
        }
        
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
                    return (string)_dataAccess.Get(nameof(TypeRecoded));                }
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
                    _dataAccess.Set(nameof(TypeRecoded), _TypeRecoded_Not_Valid);                }
                OnPropertyChanged(nameof(TypeRecoded));
            }
        }
        
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
                    return (string)_dataAccess.Get(nameof(Radionuclids));                }
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
                    _dataAccess.Set(nameof(Radionuclids), _Radionuclids_Not_Valid);                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation
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
                    return (string)_dataAccess.Get(nameof(FactoryNumber));
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
                    _dataAccess.Set(nameof(FactoryNumber), _FactoryNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(FactoryNumber));
            }
        }
        
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
                    return (string)_dataAccess.Get(nameof(FactoryNumberRecoded));
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
                    _dataAccess.Set(nameof(FactoryNumberRecoded), _FactoryNumberRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(FactoryNumberRecoded));
            }
        }
        private string _FactoryNumberRecoded_Not_Valid = "";
        private void FactoryNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(FactoryNumberRecoded));
        }
        //FactoryNumberRecoded property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public int Quantity
        {
            get
            {
                if (GetErrors(nameof(Quantity)) != null)
                {
                    return (int)_dataAccess.Get(nameof(Quantity));
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
                    _dataAccess.Set(nameof(Quantity), _Quantity_Not_Valid);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
          // positive int.
        private int _Quantity_Not_Valid = -1;
        private void Quantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(Quantity));
            if (value <= 0)
                AddError(nameof(Quantity), "Недопустимое значение");
        }
        //Quantity property

        //Activity property
        [Attributes.Form_Property("Активность, Бк")]
        public string Activity
        {
            get
            {
                if (GetErrors(nameof(Activity)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Activity));
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
                    _dataAccess.Set(nameof(Activity), _Activity_Not_Valid);
                }
                OnPropertyChanged(nameof(Activity));
            }
        }
        
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
        [Attributes.Form_Property("Дата изготовления")]
        public DateTimeOffset CreationDate
        {
            get
            {
                if (GetErrors(nameof(CreationDate)) != null)
                {
                    return (DateTime)_dataAccess.Get(nameof(CreationDate));
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
                    _dataAccess.Set(nameof(CreationDate), _CreationDate_Not_Valid);
                }
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        private DateTimeOffset _CreationDate_Not_Valid = DateTimeOffset.MinValue;
        private void CreationDate_Validation(DateTimeOffset value)//Ready
        {
            ClearErrors(nameof(CreationDate));
        }
        //CreationDate property

        //StatusRAO property
        [Attributes.Form_Property("Статус РАО")]
        public string StatusRAO  //1 cyfer or OKPO.
        {
            get
            {
                if (GetErrors(nameof(StatusRAO)) != null)
                {
                    return (string)_dataAccess.Get(nameof(StatusRAO));
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
                    _dataAccess.Set(nameof(StatusRAO), _StatusRAO_Not_Valid);
                }
                OnPropertyChanged(nameof(StatusRAO));
            }
        }
        
        private string _StatusRAO_Not_Valid = "";
        private void StatusRAO_Validation(string value)//TODO
        {
            ClearErrors(nameof(StatusRAO));
        }
        //StatusRAO property

        //ProviderOrRecieverOKPO property
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
        public string ProviderOrRecieverOKPO
        {
            get
            {
                if (GetErrors(nameof(ProviderOrRecieverOKPO)) != null)
                {
                    return (string)_dataAccess.Get(nameof(ProviderOrRecieverOKPO));
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
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPO), _ProviderOrRecieverOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }
        
        private string _ProviderOrRecieverOKPO_Not_Valid = "";
        private void ProviderOrRecieverOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(ProviderOrRecieverOKPO));
            if (value.Equals("Минобороны") || value.Equals("прим.")) return;
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(ProviderOrRecieverOKPO), "Недопустимое значение");
            else
            {
                var mask = new Regex("[0123456789_]*");
                if (!mask.IsMatch(value))
                    AddError(nameof(ProviderOrRecieverOKPO), "Недопустимое значение");
            }
        }
        //ProviderOrRecieverOKPO property

        //ProviderOrRecieverOKPONote property
        public string ProviderOrRecieverOKPONote
        {
            get
            {
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) != null)
                {
                    return (string)_dataAccess.Get(nameof(ProviderOrRecieverOKPONote));
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
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPONote), _ProviderOrRecieverOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
            }
        }
        
        private string _ProviderOrRecieverOKPONote_Not_Valid = "";
        private void ProviderOrRecieverOKPONote_Validation()
        {
            ClearErrors(nameof(ProviderOrRecieverOKPONote));
        }
        //ProviderOrRecieverOKPONote property

        //TransporterOKPO property
        [Attributes.Form_Property("ОКПО перевозчика")]
        public string TransporterOKPO
        {
            get
            {
                if (GetErrors(nameof(TransporterOKPO)) != null)
                {
                    return (string)_dataAccess.Get(nameof(TransporterOKPO));
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
                    _dataAccess.Set(nameof(TransporterOKPO), _TransporterOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }
        
        private string _TransporterOKPO_Not_Valid = "";
        private void TransporterOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(TransporterOKPO));
            if (value.Equals("прим.") || value.Equals("-")) return;
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(TransporterOKPO), "Недопустимое значение");
            else
            {
                var mask = new Regex("[0123456789_]*");
                if (!mask.IsMatch(value))
                    AddError(nameof(TransporterOKPO), "Недопустимое значение");
            }
        }
        //TransporterOKPO property

        //TransporterOKPONote property
        public string TransporterOKPONote
        {
            get
            {
                if (GetErrors(nameof(TransporterOKPONote)) != null)
                {
                    return (string)_dataAccess.Get(nameof(TransporterOKPONote));
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
                    _dataAccess.Set(nameof(TransporterOKPONote), _TransporterOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(TransporterOKPONote));
            }
        }
        
        private string _TransporterOKPONote_Not_Valid = "";
        private void TransporterOKPONote_Validation()
        {
            ClearErrors(nameof(TransporterOKPONote));
        }
        //TransporterOKPONote property

        //PackName property
        [Attributes.Form_Property("Наименование упаковки")]
        public string PackName
        {
            get
            {
                if (GetErrors(nameof(PackName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PackName));
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
                    _dataAccess.Set(nameof(PackName), _PackName_Not_Valid);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }
        
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
                    return (string)_dataAccess.Get(nameof(PackNameNote));
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
                    _dataAccess.Set(nameof(PackNameNote), _PackNameNote_Not_Valid);
                }
                OnPropertyChanged(nameof(PackNameNote));
            }
        }
        
        private string _PackNameNote_Not_Valid = "";
        private void PackNameNote_Validation()
        {
            ClearErrors(nameof(PackNameNote));
        }
        //PackNameNote property

        //PackType property
        [Attributes.Form_Property("Тип упаковки")]
        public string PackType
        {
            get
            {
                if (GetErrors(nameof(PackType)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PackType));
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
                    _dataAccess.Set(nameof(PackType), _PackType_Not_Valid);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation
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
                    return (string)_dataAccess.Get(nameof(PackTypeRecoded));
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
                    _dataAccess.Set(nameof(PackTypeRecoded), _PackTypeRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }
        
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
                    return (string)_dataAccess.Get(nameof(PackTypeNote));
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
                    _dataAccess.Set(nameof(PackTypeNote), _PackTypeNote_Not_Valid);
                }
                OnPropertyChanged(nameof(PackTypeNote));
            }
        }
        
        private string _PackTypeNote_Not_Valid = "";
        private void PackTypeNote_Validation()
        {
            ClearErrors(nameof(PackTypeNote));
        }
        //PackTypeNote property

        //PackNumber property
        [Attributes.Form_Property("Номер упаковки")]
        public string PackNumber
        {
            get
            {
                if (GetErrors(nameof(PackNumber)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PackNumber));
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
                    _dataAccess.Set(nameof(PackNumber), _PackNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PackNumber));
            }
        }
        //If change this change validation
        private string _PackNumber_Not_Valid = "";
        private void PackNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackNumber));
        }
        //PackNumber property

        //PackNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public string PackNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(PackNumberRecoded)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PackNumberRecoded));
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
                    _dataAccess.Set(nameof(PackNumberRecoded), _PackNumberRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        //If change this change validation
        private string _PackNumberRecoded_Not_Valid = "";
        private void PackNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackNumberRecoded));
        }
        //PackNumberRecoded property

        //StoragePlaceName property
        [Attributes.Form_Property("Наименование ПХ")]
        public string StoragePlaceName
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(StoragePlaceName));
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
                    _dataAccess.Set(nameof(StoragePlaceName), _StoragePlaceName_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation
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
                    return (string)_dataAccess.Get(nameof(StoragePlaceNameNote));
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
                    _dataAccess.Set(nameof(StoragePlaceNameNote), _StoragePlaceNameNote_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceNameNote));
            }
        }
        //If change this change validation
        private string _StoragePlaceNameNote_Not_Valid = "";
        private void StoragePlaceNameNote_Validation(string value)//Ready
        {
            ClearErrors(nameof(StoragePlaceNameNote));
        }
        //StoragePlaceNameNote property

        //StoragePlaceCode property
        [Attributes.Form_Property("Код ПХ")]
        public string StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceCode)) != null)
                {
                    return (string)_dataAccess.Get(nameof(StoragePlaceCode));
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
                    _dataAccess.Set(nameof(StoragePlaceCode), _StoragePlaceCode_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        //if change this change validation
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
        [Attributes.Form_Property("Код переработки/сортировки РАО")]
        public string RefineOrSortRAOCode //2 cyfer code or empty.
        {
            get
            {
                if (GetErrors(nameof(RefineOrSortRAOCode)) != null)
                {
                    return (string)_dataAccess.Get(nameof(RefineOrSortRAOCode));
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
                    _dataAccess.Set(nameof(RefineOrSortRAOCode), _RefineOrSortRAOCode_Not_Valid);
                }
                OnPropertyChanged(nameof(RefineOrSortRAOCode));
            }
        }
        //If change this change validation
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
        [Attributes.Form_Property("Субсидия, %")]
        public string Subsidy // 0<number<=100 or empty.
        {
            get
            {
                if (GetErrors(nameof(Subsidy)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Subsidy));
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
                    _dataAccess.Set(nameof(Subsidy), _Subsidy_Not_Valid);
                }
                OnPropertyChanged(nameof(Subsidy));
            }
        }
        
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
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public string FcpNumber
        {
            get
            {
                if (GetErrors(nameof(FcpNumber)) != null)
                {
                    return (string)_dataAccess.Get(nameof(FcpNumber));
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
                    _dataAccess.Set(nameof(FcpNumber), _FcpNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }
        
        private string _FcpNumber_Not_Valid = "";
        private void FcpNuber_Validation(string value)//TODO
        {
            ClearErrors(nameof(FcpNumber));
        }
        //FcpNumber property
    }
}
