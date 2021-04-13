using System;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.1: Сведения о ЗРИ")]
    public class Form11 : Abstracts.Form1
    {
        public static string SQLCommandParams()
        {
            string strNotNullDeclaration = " varchar(255) not null, ";
            string intNotNullDeclaration = " int not null, ";
            string shortNotNullDeclaration = " smallint not null, ";
            string byteNotNullDeclaration = " tinyint not null, ";
            string dateNotNullDeclaration = " ????, ";
            return
                Abstracts.Form1.SQLCommandParamsBase() +
                nameof(PassportNumber) + strNotNullDeclaration +
                nameof(PassportNumberNote) + strNotNullDeclaration +
                nameof(PassportNumberRecoded) + strNotNullDeclaration +
                nameof(Type) + strNotNullDeclaration +
                nameof(TypeRecoded) + strNotNullDeclaration +
                nameof(Radionuclids) + strNotNullDeclaration +
                nameof(FactoryNumber) + strNotNullDeclaration +
                nameof(FactoryNumberRecoded) + strNotNullDeclaration +
                nameof(Quantity) + intNotNullDeclaration +
                nameof(Activity) + strNotNullDeclaration +
                nameof(ActivityNote) + strNotNullDeclaration +
                nameof(CreationDate) + dateNotNullDeclaration +
                nameof(CreatorOKPO) + strNotNullDeclaration +
                nameof(CreatorOKPONote) + strNotNullDeclaration +
                nameof(Category) + shortNotNullDeclaration +
                nameof(SignedServicePeriod) + intNotNullDeclaration +
                nameof(PropertyCode) + byteNotNullDeclaration +
                nameof(Owner) + strNotNullDeclaration +
                nameof(ProviderOrRecieverOKPO) + strNotNullDeclaration +
                nameof(ProviderOrRecieverOKPONote) + strNotNullDeclaration +
                nameof(TransporterOKPO) + strNotNullDeclaration +
                nameof(TransporterOKPONote) + strNotNullDeclaration +
                nameof(PackName) + strNotNullDeclaration +
                nameof(PackNameNote) + strNotNullDeclaration +
                nameof(PackType) + strNotNullDeclaration +
                nameof(PackTypeRecoded) + strNotNullDeclaration +
                nameof(PackTypeNote) + strNotNullDeclaration +
                nameof(PackNumber) + strNotNullDeclaration +
                nameof(PackNumberRecoded) + " varchar(255) not null";
        }
        public Form11(int RowID) : base(RowID)
        {
            FormNum = "11";
            NumberOfFields = 37;
        }

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
        //If change this change validation
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
                    return (string)_dataAccess.Get(nameof(ActivityNote));
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
                    _dataAccess.Set(nameof(ActivityNote), _ActivityNote_Not_Valid);
                }
                OnPropertyChanged(nameof(ActivityNote));
            }
        }
        //If change this change validation
        private string _ActivityNote_Not_Valid = "";
        private void ActivityNote_Validation(string value)//Ready
        {
            ClearErrors(nameof(ActivityNote));
        }
        //ActivityNote property

        //CreationDate property
        [Attributes.Form_Property("Дата изготовления")]
        public DateTimeOffset CreationDate
        {
            get
            {
                if (GetErrors(nameof(CreationDate)) != null)
                {
                    return (DateTimeOffset)_dataAccess.Get(nameof(CreationDate));
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
        //If change this change validation
        private DateTimeOffset _CreationDate_Not_Valid = DateTimeOffset.MinValue;
        private void CreationDate_Validation(DateTimeOffset value)//Ready
        {
            ClearErrors(nameof(CreationDate));
        }
        //CreationDate property

        //CreatorOKPO property
        [Attributes.Form_Property("ОКПО изготовителя")]
        public string CreatorOKPO
        {
            get
            {
                if (GetErrors(nameof(CreatorOKPO)) != null)
                {
                    return (string)_dataAccess.Get(nameof(CreatorOKPO));
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
                    _dataAccess.Set(nameof(CreatorOKPO), _CreatorOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(CreatorOKPO));
            }
        }
          //If change this change validation
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
                    return (string)_dataAccess.Get(nameof(CreatorOKPONote));
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
                    _dataAccess.Set(nameof(CreatorOKPONote), _CreatorOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(CreatorOKPONote));
            }
        }
        
        private string _CreatorOKPONote_Not_Valid = "";
        private void CreatorOKPONote_Validation()
        {
            ClearErrors(nameof(CreatorOKPONote));
        }
        //CreatorOKPONote property

        //Category property
        [Attributes.Form_Property("Категория")]
        public short Category
        {
            get
            {
                if (GetErrors(nameof(Category)) != null)
                {
                    return (short)_dataAccess.Get(nameof(Category));
                }
                else
                {
                    return _Category_Not_Valid;
                }
            }
            set
            {
                _Category_Not_Valid = value;
                if (GetErrors(nameof(Category)) != null)
                {
                    _dataAccess.Set(nameof(Category), _Category_Not_Valid);
                }
                OnPropertyChanged(nameof(Category));
            }
        }
        
        private short _Category_Not_Valid = -1;
        private void Сategory_Validation(short value)//TODO
        {
            ClearErrors(nameof(Category));
        }
        //Category property

        //SignedServicePeriod property
        [Attributes.Form_Property("НСС, мес.")]
        public int SignedServicePeriod
        {
            get
            {
                if (GetErrors(nameof(SignedServicePeriod)) != null)
                {
                    return (int)_dataAccess.Get(nameof(SignedServicePeriod));
                }
                else
                {
                    return _SignedServicePeriod_Not_Valid;
                }
            }
            set
            {
                _SignedServicePeriod_Not_Valid = value;
                if (GetErrors(nameof(SignedServicePeriod)) != null)
                {
                    _dataAccess.Set(nameof(SignedServicePeriod), _SignedServicePeriod_Not_Valid);
                }
                OnPropertyChanged(nameof(SignedServicePeriod));
            }
        }
        
        private int _SignedServicePeriod_Not_Valid = -1;
        private void SignedServicePeriod_Validation(int value)//Ready
        {
            ClearErrors(nameof(SignedServicePeriod));
            if (value <= 0)
                AddError(nameof(SignedServicePeriod), "Недопустимое значение");
        }
        //SignedServicePeriod property

        //PropertyCode property
        [Attributes.Form_Property("Код собственности")]
        public byte PropertyCode
        {
            get
            {
                if (GetErrors(nameof(PropertyCode)) != null)
                {
                    return (byte)_dataAccess.Get(nameof(PropertyCode));
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
                    _dataAccess.Set(nameof(PropertyCode), _PropertyCode_Not_Valid);
                }
                OnPropertyChanged(nameof(PropertyCode));
            }
        }
        
        private byte _PropertyCode_Not_Valid = 255;
        private void PropertyCode_Validation(byte value)//Ready
        {
            ClearErrors(nameof(PropertyCode));
            if (!((value >= 1) && (value <= 9)))
                AddError(nameof(PropertyCode), "Недопустимое значение");
        }
        //PropertyCode property

        //Owner property
        [Attributes.Form_Property("Владелец")]
        public string Owner
        {
            get
            {
                if (GetErrors(nameof(Owner)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Owner));
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
                    _dataAccess.Set(nameof(Owner), _Owner_Not_Valid);
                }
                OnPropertyChanged(nameof(Owner));
            }
        }
        //if change this change validation
        private string _Owner_Not_Valid = "";
        private void Owner_Validation(string value)//Ready
        {
            ClearErrors(nameof(Owner));
        }
        //Owner property

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
    }
}
