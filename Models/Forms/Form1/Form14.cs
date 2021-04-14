using System;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.4: Сведения об ОРИ, кроме отдельных изделий")]
    public class Form14: Abstracts.Form1
    {
        public static string SQLCommandParams()
        {
            string strNotNullDeclaration = " varchar(255) not null, ";
            string intNotNullDeclaration = " int not null, ";
            string shortNotNullDeclaration = " smallint not null, ";
            string byteNotNullDeclaration = " tinyint not null, ";
            string dateNotNullDeclaration = " ????, ";
            string doubleNotNullDeclaration = " float(53) not null, ";
            return
                Abstracts.Form1.SQLCommandParamsBase() +
                nameof(PassportNumber) + strNotNullDeclaration +
                nameof(PassportNumberNote) + strNotNullDeclaration +
                nameof(PassportNumberRecoded) + strNotNullDeclaration +
                nameof(Name) + strNotNullDeclaration +
                nameof(Sort) + byteNotNullDeclaration +
                nameof(Type) + strNotNullDeclaration +
                nameof(ActivityMeasurementDate) + dateNotNullDeclaration +
                nameof(Volume) + doubleNotNullDeclaration +
                nameof(Mass) + doubleNotNullDeclaration +
                nameof(AggregateState) + byteNotNullDeclaration +
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
                nameof(PackNumberRecoded) + strNotNullDeclaration +
                nameof(Radionuclids) + strNotNullDeclaration +
                nameof(Activity) + " varchar(255) not null";
        }
        public Form14(int RowID) : base(RowID)
        {
            FormNum = "14";
            NumberOfFields = 32;
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

        //Name property
        [Attributes.Form_Property("Наименование")]
        public string Name
        {
            get
            {
                if (GetErrors(nameof(Name)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Name));
                }
                else
                {
                    return _Name_Not_Valid;
                }
            }
            set
            {
                _Name_Not_Valid = value;
                if (GetErrors(nameof(Name)) != null)
                {
                    _dataAccess.Set(nameof(Name), _Name_Not_Valid);
                }
                OnPropertyChanged(nameof(Name));
            }
        }
        
        private string _Name_Not_Valid = "";
        private void Name_Validation(string value)//TODO
        {
            ClearErrors(nameof(Name));
        }
        //Name property

        //Sort property
        [Attributes.Form_Property("Вид")]
        public byte Sort
        {
            get
            {
                if (GetErrors(nameof(Sort)) != null)
                {
                    return (byte)_dataAccess.Get(nameof(Sort));
                }
                else
                {
                    return _Sort_Not_Valid;
                }
            }
            set
            {
                _Sort_Not_Valid = value;
                if (GetErrors(nameof(Sort)) != null)
                {
                    _dataAccess.Set(nameof(Sort), _Sort_Not_Valid);
                }
                OnPropertyChanged(nameof(Sort));
            }
        }
        //If change this change validation
        private byte _Sort_Not_Valid = 255;
        private void Sort_Validation(byte value)//TODO
        {
            ClearErrors(nameof(Sort));
        }
        //Sort property

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

        //ActivityMeasurementDate property
        [Attributes.Form_Property("Дата измерения активности")]
        public DateTimeOffset ActivityMeasurementDate
        {
            get
            {
                if (GetErrors(nameof(ActivityMeasurementDate)) != null)
                {
                    return (DateTime)_dataAccess.Get(nameof(ActivityMeasurementDate));
                }
                else
                {
                    return _ActivityMeasurementDate_Not_Valid;
                }
            }
            set
            {
                _ActivityMeasurementDate_Not_Valid = value;
                if (GetErrors(nameof(ActivityMeasurementDate)) != null)
                {
                    _dataAccess.Set(nameof(ActivityMeasurementDate), _ActivityMeasurementDate_Not_Valid);
                }
                OnPropertyChanged(nameof(ActivityMeasurementDate));
            }
        }
        //if change this change validation
        private DateTimeOffset _ActivityMeasurementDate_Not_Valid = DateTimeOffset.MinValue;
        private void ActivityMeasurementDate_Validation(DateTimeOffset value)//Ready
        {
            ClearErrors(nameof(ActivityMeasurementDate));
        }
        //ActivityMeasurementDate property

        //Volume property
        [Attributes.Form_Property("Объем, куб. м")]
        public double Volume
        {
            get
            {
                if (GetErrors(nameof(Volume)) != null)
                {
                    return (double)_dataAccess.Get(nameof(Volume));
                }
                else
                {
                    return _Volume_Not_Valid;
                }
            }
            set
            {
                _Volume_Not_Valid = value;
                if (GetErrors(nameof(Volume)) != null)
                {
                    _dataAccess.Set(nameof(Volume), _Volume_Not_Valid);
                }
                OnPropertyChanged(nameof(Volume));
            }
        }
        
        private double _Volume_Not_Valid = -1;
        private void Volume_Validation(double value)//TODO
        {
            ClearErrors(nameof(Volume));
        }
        //Volume property

        //Mass Property
        [Attributes.Form_Property("Масса, кг")]
        public double Mass
        {
            get
            {
                if (GetErrors(nameof(Mass)) != null)
                {
                    return (double)_dataAccess.Get(nameof(Mass));
                }
                else
                {
                    return _Mass_Not_Valid;
                }
            }
            set
            {
                _Mass_Not_Valid = value;
                if (GetErrors(nameof(Mass)) != null)
                {
                    _dataAccess.Set(nameof(Mass), _Mass_Not_Valid);
                }
                OnPropertyChanged(nameof(Mass));
            }
        }
        
        private double _Mass_Not_Valid = -1;
        private void Mass_Validation()//TODO
        {
            ClearErrors(nameof(Mass));
        }
        //Mass Property

        //AggregateState property
        [Attributes.Form_Property("Агрегатное состояние")]
        public byte AggregateState//1 2 3
        {
            get
            {
                if (GetErrors(nameof(AggregateState)) != null)
                {
                    return (byte)_dataAccess.Get(nameof(AggregateState));
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
                    _dataAccess.Set(nameof(AggregateState), _AggregateState_Not_Valid);
                }
                OnPropertyChanged(nameof(AggregateState));
            }
        }
        
        private byte _AggregateState_Not_Valid = 0;
        private void AggregateState_Validation(byte value)//Ready
        {
            ClearErrors(nameof(AggregateState));
            if ((value != 1) && (value != 2) && (value != 3))
                AddError(nameof(AggregateState), "Недопустимое значение");
        }
        //AggregateState property

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
