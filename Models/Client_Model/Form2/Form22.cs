using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 2.2: Наличие РАО в пунктах хранения, местах сбора и/или временного хранения")]
    public class Form22 : Form2
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 25;

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

        private int _packQuantity = -1;
        private void PackQuantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(PackQuantity));
            if (value <= 0)
                AddError(nameof(PackQuantity), "Недопустимое значение");
        }

        [Attributes.FormVisual("Количество упаковок, шт.")]
        public int PackQuantity
        {
            get { return _packQuantity; }
            set
            {
                _packQuantity = value;
                PackQuantity_Validation(value);
                OnPropertyChanged("PackQuantity");
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
        private double _volumeInPack = -1;
        private void VolumeInPack_Validation(double value)
        {

        }

        [Attributes.FormVisual("Объем с упаковкой, куб. м")]
        public double VolumeInPack
        {
            get { return _volumeInPack; }
            set
            {
                _volumeInPack = value;
                VolumeInPack_Validation(value);
                OnPropertyChanged("VolumeInPack");
            }
        }

        private double _massInPack = -1;
        private void MassInPack_Validation(double value)
        {

        }

        [Attributes.FormVisual("Масса с упаковкой, т")]
        public double MassInPack
        {
            get { return _massInPack; }
            set
            {
                _massInPack = value;
                MassInPack_Validation(value);
                OnPropertyChanged("MassInPack");
            }
        }

        private double _volumeOutOfPack = -1;
        private void VolumeOutOfPack_Validation(double value)
        {

        }

        [Attributes.FormVisual("Объем без упаковки, куб. м")]
        public double VolumeOutOfPack
        {
            get { return _volumeOutOfPack; }
            set
            {
                _volumeOutOfPack = value;
                VolumeOutOfPack_Validation(value);
                OnPropertyChanged("VolumeOutOfPack");
            }
        }

        private double _massOutOfPack = -1;
        private void MassOutOfPack_Validation(double value)
        {

        }

        [Attributes.FormVisual("Масса без упаковки, куб. м")]
        public double MassOutOfPack
        {
            get { return _massOutOfPack; }
            set
            {
                _massOutOfPack = value;
                MassOutOfPack_Validation(value);
                OnPropertyChanged("MassOutOfPack");
            }
        }

        private int _quantityOZIII = -1;
        private void QuantityOZIII_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityOZIII));
            if (value <= 0)
                AddError(nameof(QuantityOZIII), "Недопустимое значение");
        }

        [Attributes.FormVisual("Количество ОЗИИИ, шт.")]
        public int QuantityOZIII
        {
            get { return _quantityOZIII; }
            set
            {
                _quantityOZIII = value;
                QuantityOZIII_Validation(value);
                OnPropertyChanged("QuantityOZIII");
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

        private string _mainRadionuclids = "";//if change this change validation

        private void MainRadionuclids_Validation(string value)//TODO
        {
            ClearErrors(nameof(MainRadionuclids));
        }

        [Attributes.FormVisual("Основные адионуклиды")]
        public string MainRadionuclids
        {
            get { return _mainRadionuclids; }
            set
            {
                _mainRadionuclids = value;
                MainRadionuclids_Validation(value);
                OnPropertyChanged("MainRadionuclids");
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
