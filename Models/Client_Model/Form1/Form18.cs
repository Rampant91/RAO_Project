using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 1.8: Сведения о жидких кондиционированных РАО")]
    public class Form18: Form1
    {        
        public override string FormNum { get { return "1.9"; } }
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 36;

        [Attributes.FormVisual("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    return _correctionNumber;
                }
                else
                {
                    return _correctionNumber_Not_Valid;
                }
            }
            set
            {
                _correctionNumber_Not_Valid = value;
                if (CorrectionNumber_Validation())
                {
                    _correctionNumber = _correctionNumber_Not_Valid;
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private byte _correctionNumber = 255;
        private byte _correctionNumber_Not_Valid = 255;
        private bool CorrectionNumber_Validation()
        {
            return true;
            //ClearErrors(nameof(CorrectionNumber));
            ////Пример
            //if (value < 10)
            //    AddError(nameof(CorrectionNumber), "Значение должно быть больше 10.");
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

        [Attributes.FormVisual("Код")]
        public short OperationCode
        {
            get
            {
                if (GetErrors(nameof(OperationCode)) != null)
                {
                    return _OperationCode;
                }
                else
                {
                    return _OperationCode_Not_Valid;
                }
            }
            set
            {
                _OperationCode_Not_Valid = value;
                if (OperationCode_Validation())
                {
                    _OperationCode = _OperationCode_Not_Valid;
                }
                OnPropertyChanged(nameof(OperationCode));
            }
        }
        private short _OperationCode = -1;
        private short _OperationCode_Not_Valid = -1;
        private bool OperationCode_Validation()
        {
            return true;
        }

        //OperationDate property
        [Attributes.FormVisual("Дата операции")]
        public DateTime OperationDate
        {
            get
            {
                if (GetErrors(nameof(OperationDate)) != null)
                {
                    return _OperationDate;
                }
                else
                {
                    return _OperationDate_Not_Valid;
                }
            }
            set
            {
                _OperationDate_Not_Valid = value;
                if (OperationDate_Validation())
                {
                    _OperationDate = _OperationDate_Not_Valid;
                }
                OnPropertyChanged(nameof(OperationDate));
            }
        }
        private DateTime _OperationDate = DateTime.MinValue;
        private DateTime _OperationDate_Not_Valid = DateTime.MinValue;
        private bool OperationDate_Validation()
        {
            return true;
        }

        private string _individualNumberZHRO = "";
        private void IndividualNumberZHRO_Validation(string value)
        {

        }

        [Attributes.FormVisual("Индивидуальный номер ЖРО")]
        public string IndividualNumberZHRO
        {
            get { return _individualNumberZHRO; }
            set
            {
                _individualNumberZHRO = value;
                IndividualNumberZHRO_Validation(value);
                OnPropertyChanged("IndividualNumberZHRO");
            }
        }

        private string _individualNumberZHRORecoded = "";
        public string IndividualNumberZHRORecoded
        {
            get { return _individualNumberZHRORecoded; }
            set
            {
                _individualNumberZHRORecoded = value;
                OnPropertyChanged("IndividualNumberZHRORecoded");
            }
        }

        //PassportNumber property
        [Attributes.FormVisual("Номер паспорта")]
        public string PassportNumber
        {
            get
            {
                if (GetErrors(nameof(PassportNumber)) != null)
                {
                    return _PassportNumber;
                }
                else
                {
                    return _PassportNumber_Not_Valid;
                }
            }
            set
            {
                _PassportNumber_Not_Valid = value;
                if (PassportNumber_Validation())
                {
                    _PassportNumber = _PassportNumber_Not_Valid;
                }
                OnPropertyChanged(nameof(PassportNumber));
            }
        }
        private string _PassportNumber = "";
        private string _PassportNumber_Not_Valid = "";
        private bool PassportNumber_Validation()
        {
            return true;
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

        private double _volume6 = -1;
        private void Volume6_Validation(double value)
        {

        }

        [Attributes.FormVisual("Объем, куб. м")]
        public double Volume6
        {
            get { return _volume6; }
            set
            {
                _volume6 = value;
                Volume6_Validation(value);
                OnPropertyChanged("Volume");
            }
        }

        private double _mass7 = -1;
        private void Mass7_Validation(double value)
        {

        }

        [Attributes.FormVisual("Масса, т")]
        public double Mass7
        {
            get { return _mass7; }
            set
            {
                _mass7 = value;
                Mass7_Validation(value);
                OnPropertyChanged("Mass7");
            }
        }

        private double _saltConcentration = -1;
        private void SaltConcentration_Validation(double value)
        {

        }

        [Attributes.FormVisual("Солесодержание, г/л")]
        public double SaltConcentration
        {
            get { return _saltConcentration; }
            set
            {
                _saltConcentration = value;
                SaltConcentration_Validation(value);
                OnPropertyChanged("SaltConcentration");
            }
        }

        private string _nuclidName = "";
        private void NuclidName_Validation(string value)
        {

        }

        [Attributes.FormVisual("Наименования радионуклидов")]
        public string NuclidName
        {
            get { return _nuclidName; }
            set
            {
                _nuclidName = value;
                NuclidName_Validation(value);
                OnPropertyChanged("NuclidName");
            }
        }

        private string _specificActivity = "";
        private void SpecificActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(SpecificActivity));
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(SpecificActivity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(SpecificActivity), "Недопустимое значение");
            }
        }

        [Attributes.FormVisual("Удельная активность, Бк/г")]
        public string SpecificActivity
        {
            get { return _specificActivity; }
            set
            {
                _specificActivity = value;
                SpecificActivity_Validation(value);
                OnPropertyChanged("SpecificActivity");
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
                ProviderOrRecieverOKPO_Validation(value);
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
                    for (int i = 0; i < 8; i++)
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

        private string _codeRAO = "";
        private void CodeRAO_Validation(string value)//TODO
        {
        }

        [Attributes.FormVisual("Код РАО")]
        public string CodeRAO
        {
            get { return _codeRAO; }
            set
            {
                _codeRAO = value;
                CodeRAO_Validation(value);
                OnPropertyChanged("CodeRAO");
            }
        }

        private string _statusRAO = "";
        private void StatusRAO_Validation(string value)
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

        private double _volume20 = -1;
        private void Volume20_Validation(double value)
        {

        }

        [Attributes.FormVisual("Объем, куб. м")]
        public double Volume20
        {
            get { return _volume20; }
            set
            {
                _volume20 = value;
                Volume20_Validation(value);
                OnPropertyChanged("Volume20");
            }
        }

        private double _mass21 = -1;
        private void Mass21_Validation(double value)
        {
        }

        [Attributes.FormVisual("Масса, т")]
        public double Mass21
        {
            get { return _mass21; }
            set
            {
                _mass21 = value;
                Mass21_Validation(value);
                OnPropertyChanged("Mass21");
            }
        }

        private string _tritiumActivity = "";

        private void TritiumActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(TritiumActivity));
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
                    AddError(nameof(TritiumActivity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(TritiumActivity), "Недопустимое значение");
            }
        }

        [Attributes.FormVisual("Активность трития, Бк")]
        public string TritiumActivity
        {
            get { return _tritiumActivity; }
            set
            {
                _tritiumActivity = value;
                TritiumActivity_Validation(value);
                OnPropertyChanged("TritiumActivity");
            }
        }

        private string _betaGammaActivity = "";
        private void BetaGammaActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(BetaGammaActivity));
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
                    AddError(nameof(BetaGammaActivity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(BetaGammaActivity), "Недопустимое значение");
            }
        }

        [Attributes.FormVisual("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public string BetaGammaActivity
        {
            get { return _betaGammaActivity; }
            set
            {
                _betaGammaActivity = value;
                BetaGammaActivity_Validation(value);
                OnPropertyChanged("BetaGammaActivity");
            }
        }

        private string _alphaActivity = "";
        private void AlphaActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(AlphaActivity));
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
                    AddError(nameof(AlphaActivity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(AlphaActivity), "Недопустимое значение");
            }
        }

        [Attributes.FormVisual("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public string AlphaActivity
        {
            get { return _alphaActivity; }
            set
            {
                _alphaActivity = value;
                AlphaActivity_Validation(value);
                OnPropertyChanged("AlphaActivity");
            }
        }

        private string _transuraniumActivity = "";
        private void TransuraniumActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(TransuraniumActivity));
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
                    AddError(nameof(TransuraniumActivity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(TransuraniumActivity), "Недопустимое значение");
            }
        }

        [Attributes.FormVisual("Активность трансурановых, Бк")]
        public string TransuraniumActivity
        {
            get { return _transuraniumActivity; }
            set
            {
                _transuraniumActivity = value;
                TransuraniumActivity_Validation(value);
                OnPropertyChanged("TransuraniumActivity");
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
