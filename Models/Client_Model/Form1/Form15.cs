using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 1.5: Сведения о РАО в виде отработавших ЗРИ")]
    public class Form15: Form
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 37;

        private byte _correctionNumber = 255;

        //Data Validation
        private void CorrectionNumber_Validation(byte value)//TODO
        {
            ClearErrors(nameof(CorrectionNumber));
            //Пример
            if (value < 10)
                AddError(nameof(CorrectionNumber), "Значение должно быть больше 10.");
        }
        //Data Validation

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

        private string _type = "";

        private void Type_Validation(string value)//Ready
        {
            ClearErrors(nameof(Type));
        }

        [Attributes.FormVisual("Тип")]
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                Type_Validation(value);
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

        private int _quantity = -1;  // positive int.

        private void Quantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(Quantity));
            if (value <= 0)
                AddError(nameof(Quantity), "Недопустимое значение");
        }

        [Attributes.FormVisual("Количество, шт.")]
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                Quantity_Validation(value);
                OnPropertyChanged("Quantity");
            }
        }

        private string _activity = "";

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

        [Attributes.FormVisual("Активность, Бк")]
        public string Activity
        {
            get { return _activity; }
            set
            {
                _activity = value;
                Activity_Validation(value);
                OnPropertyChanged("Activity");
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

        private string _statusRAO = "";

        private void StatusRAO_Validation(string value)//TODO
        {
        }

        [Attributes.FormVisual("Статус РАО")]
        public string StatusRAO  //1 cyfer or OKPO.
        {
            get { return _statusRAO; }
            set
            {
                _statusRAO = value;
                StatusRAO_Validation(value);
                OnPropertyChanged("StatusRAO");
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

        private string _storagePlaceName = "";//If change this change validation

        private void StoragePlaceName_Validation(string value)//Ready
        {
            ClearErrors(nameof(StoragePlaceName));
        }

        [Attributes.FormVisual("Наименование ПХ")]
        public string StoragePlaceName
        {
            get { return _storagePlaceName; }
            set
            {
                _storagePlaceName = value;
                StoragePlaceName_Validation(value);
                OnPropertyChanged("StoragePlaceName");
            }
        }

        private string _storagePlaceNameNote = "";
        public string StoragePlaceNameNote
        {
            get { return _storagePlaceNameNote; }
            set
            {
                _storagePlaceNameNote = value;
                OnPropertyChanged("StoragePlaceNameNote");
            }
        }

        private string _storagePlaceCode = "";//if change this change validation

        private void StoragePlaceCode_Validation(string value)//TODO
        {
            ClearErrors(nameof(StoragePlaceCode));
            if (!(value == "-"))
                if (value.Length != 8)
                    AddError(nameof(StoragePlaceCode), "Недопустимое значение");
                else
                    for(int i = 0; i < 8; i++)
                    {
                        if (!((value[i] >= '0') && (value[i] <= '9')))
                        {
                            AddError(nameof(StoragePlaceCode), "Недопустимое значение");
                            return;
                        }
                    }
        }

        [Attributes.FormVisual("Код ПХ")]
        public string StoragePlaceCode //8 cyfer code or - .
        {
            get { return _storagePlaceCode; }
            set
            {
                _storagePlaceCode = value;
                StoragePlaceCode_Validation(value);
                OnPropertyChanged("StoragePlaceCode");
            }
        }

        private string _refineOrSortRAOCode = "";//If change this change validation

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

        [Attributes.FormVisual("Код переработки/сортировки РАО")]
        public string RefineOrSortRAOCode //2 cyfer code or empty.
        {
            get { return _refineOrSortRAOCode; }
            set
            {
                _refineOrSortRAOCode = value;
                RefineOrSortRAOCode_Validation(value);
                OnPropertyChanged("RefineOrSortRAOCode");
            }
        }

        private string _subsidy = "";

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

        [Attributes.FormVisual("Субсидия, %")]
        public string Subsidy // 0<number<=100 or empty.
        {
            get { return _subsidy; }
            set
            {
                _subsidy = value;
                Subsidy_Validation(value);
                OnPropertyChanged("Subsidy");
            }
        }

        private string _fcpNumber = "";

        private void FcpNuber_Validation(string value)//TODO
        {
            ClearErrors(nameof(FcpNumber));
        }

        [Attributes.FormVisual("Номер мероприятия ФЦП")]
        public string FcpNumber
        {
            get { return _fcpNumber; }
            set
            {
                _fcpNumber = value;
                FcpNuber_Validation(value);
                OnPropertyChanged("FcpNumber");
            }
        }
    }
}
