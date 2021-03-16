using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 1.2: Сведения об изделиях из обедненного урана")]
    public class Form12: Form1
    {
        public override string FormNum { get { return "1.2"; } }
        public override void Object_Validation()
        {

        }

        public int NumberOfFields { get; } = 32;

        private byte _correctionNumber = 255;

        private void CorrectionNumber_Validation(byte value)//TODO
        {
            ClearErrors(nameof(CorrectionNumber));
            //Пример
            if (value < 10)
                AddError(nameof(CorrectionNumber), "Значение должно быть больше 10.");
        }

        [Attributes.FormVisual("Номер корректировки")]
        public byte CorrectionNumber
        {
            get { return _correctionNumber; }
            set
            {
                _correctionNumber = value;
                CorrectionNumber_Validation(value);
                OnPropertyChanged("CorrectionNumber");
            }
        }

        private int _numberInOrder = -1;
        [Attributes.FormVisual("№ п/п")]
        public int NumberInOrder
        {
            get { return _numberInOrder; }
            set
            {
                _numberInOrder = value;
                OnPropertyChanged("NumberInOrder");
            }
        }

        private short _operationCode = -1;

        private void OperationCode_Validation(short value)//TODO
        {
        }

        [Attributes.FormVisual("Код операции")]
        public short OperationCode
        {
            get { return _operationCode; }
            set
            {
                _operationCode = value;
                OperationCode_Validation(value);
                OnPropertyChanged("OperationCode");
            }
        }

        private DateTime _operationDate = DateTime.MinValue;

        private void OperationDate_Validation(DateTime value)//Ready
        {
        }

        [Attributes.FormVisual("Дата операции")]
        public DateTime OperationDate
        {
            get { return _operationDate; }
            set
            {
                _operationDate = value;
                OperationDate_Validation(value);
                OnPropertyChanged("OperationDate");
            }
        }

        private string _passportNumber = "";

        private void PassportNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(PassportNumber));
        }

        [Attributes.FormVisual("Номер паспорта")]
        public string PassportNumber
        {
            get { return _passportNumber; }
            set
            {
                _passportNumber = value;
                PassportNumber_Validation(value);
                OnPropertyChanged("PassportNumber");
            }
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

        private string _nameIOU = "";

        private void NameIOU_Validation(string value)//TODO
        {
        }

        [Attributes.FormVisual("Наименование ИОУ")]
        public string NameIOU
        {
            get { return _nameIOU; }
            set
            {
                _nameIOU = value;
                NameIOU_Validation(value);
                OnPropertyChanged("NameIOU");
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

        private string _mass = "";

        private void Mass_Validation(string value)//TODO
        {
        }

        [Attributes.FormVisual("Масса, кг")]
        public string Mass
        {
            get { return _mass; }
            set
            {
                _mass = value;
                Mass_Validation(value);
                OnPropertyChanged("Mass");
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

        private DateTime _creationDate = DateTime.MinValue;//If change this change validation

        private void CreationDate_Validation(DateTime value)//Ready
        {
            ClearErrors(nameof(CreationDate));
        }

        [Attributes.FormVisual("Дата изготовления")]
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

        private int _signedServicePeriod = -1;

        private void SignedServicePeriod_Validation(int value)//Ready
        {
            ClearErrors(nameof(SignedServicePeriod));
            if (value <= 0)
                AddError(nameof(SignedServicePeriod), "Недопустимое значение");
        }

        [Attributes.FormVisual("НСС, мес.")]
        public int SignedServicePeriod
        {
            get { return _signedServicePeriod; }
            set
            {
                _signedServicePeriod = value;
                SignedServicePeriod_Validation(value);
                OnPropertyChanged("SignedServicePeriod");
            }
        }

        private byte _propertyCode = 255;

        private void PropertyCode_Validation(byte value)//Ready
        {
            ClearErrors(nameof(PropertyCode));
            if (!((value >= 1) && (value <= 9)))
                AddError(nameof(PropertyCode), "Недопустимое значение");
        }

        [Attributes.FormVisual("Код собственности")]
        public byte PropertyCode
        {
            get { return _propertyCode; }
            set
            {
                _propertyCode = value;
                PropertyCode_Validation(value);
                OnPropertyChanged("PropertyCode");
            }
        }

        private string _owner = "";//if change this change validation

        private void Owner_Validation(string value)//Ready
        {
            ClearErrors(nameof(Owner));
        }

        [Attributes.FormVisual("Владелец")]
        public string Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                Owner_Validation(value);
                OnPropertyChanged("Owner");
            }
        }

        private byte _documentVid = 255;

        private void DocumentVid_Validation(byte value)//TODO
        {
        }

        [Attributes.FormVisual("Вид документа")]
        public byte DocumentVid
        {
            get { return _documentVid; }
            set
            {
                _documentVid = value;
                DocumentVid_Validation(value);
                OnPropertyChanged("DocumentVid");
            }
        }

        private string _documentNumber = "";

        private void DocumentNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(DocumentNumber));
        }

        [Attributes.FormVisual("Номер документа")]
        public string DocumentNumber
        {
            get { return _documentNumber; }
            set
            {
                _documentNumber = value;
                DocumentNumber_Validation(value);
                OnPropertyChanged("DocumentNumber");
            }
        }

        private string _documentNumberRecoded = "";
        public string DocumentNumberRecoded
        {
            get { return _documentNumberRecoded; }
            set
            {
                _documentNumberRecoded = value;
                OnPropertyChanged("DocumentNumberRecoded");
            }
        }

        private DateTime _documentDate = DateTime.MinValue;//if change this change validation

        private void DocumentDate_Validation(DateTime value)//Ready
        {
            ClearErrors(nameof(DocumentDate));
        }

        [Attributes.FormVisual("Дата документа")]
        public DateTime DocumentDate
        {
            get { return _documentDate; }
            set
            {
                _documentDate = value;
                DocumentDate_Validation(value);
                OnPropertyChanged("DocumentDate");
            }
        }

        private string _providerOrRecieverOKPO = "";

        private void ProviderOrRecieverOKPO_Validation(string value)//TODO
        {
        }

        [Attributes.FormVisual("ОКПО поставщика/получателя")]
        public string ProviderOrRecieverOKPO
        {
            get { return _providerOrRecieverOKPO; }
            set
            {
                _providerOrRecieverOKPO = value;
                ProviderOrRecieverOKPO_Validation(value);
                OnPropertyChanged("ProviderOrRecieverOKPO");
            }
        }

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

        private string _transporterOKPO = "";

        private void TransporterOKPO_Validation(string value)//TODO
        {
        }

        [Attributes.FormVisual("ОКПО перевозчика")]
        public string TransporterOKPO
        {
            get { return _transporterOKPO; }
            set
            {
                _transporterOKPO = value;
                TransporterOKPO_Validation(value);
                OnPropertyChanged("TransporterOKPO");
            }
        }

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

        private string _packName = "";

        [Attributes.FormVisual("Наименование упаковки")]
        public string PackName
        {
            get { return _packName; }
            set
            {
                _packName = value;
                OnPropertyChanged("PackName");
            }
        }

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

        private string _packType = "";//If change this change validation
        private void PackType_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackType));
        }

        [Attributes.FormVisual("Тип упаковки")]
        public string PackType
        {
            get { return _packType; }
            set
            {
                _packType = value;
                PackType_Validation(value);
                OnPropertyChanged("PackType");
            }
        }

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

        private string _packNumber = "";//If change this change validation

        private void PackNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackNumber));
        }

        [Attributes.FormVisual("Номер упаковки")]
        public string PackNumber
        {
            get { return _packNumber; }
            set
            {
                _packNumber = value;
                PackNumber_Validation(value);
                OnPropertyChanged("PackNumber");
            }
        }

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
