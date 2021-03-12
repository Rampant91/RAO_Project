using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 2.1: Сортировка, переработка и кондиционирование РАО на установках")]
    public class Form21: Form2
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 24;

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

        private string _refineMachineName = "";

        [Attributes.FormVisual("Наименование установки переработки")]
        public string RefineMachineName
        {
            get { return _refineMachineName; }
            set
            {
                _refineMachineName = value;
                OnPropertyChanged("RefineMachineName");
            }
        }

        private short _machineCode = 0;
        private void MachineCode_Validation(short value)//TODO
        {

        }

        [Attributes.FormVisual("Код установки переработки")]
        public short MachineCode
        {
            get { return _machineCode; }
            set
            {
                _machineCode = value;
                MachineCode_Validation(value);
                OnPropertyChanged("MachineCode");
            }
        }

        private string _machinePower = "";
        private void MachinePower_Validation(string value)//TODO
        {

        }

        [Attributes.FormVisual("Мощность, куб. м/год")]
        public string MachinePower
        {
            get { return _machinePower; }
            set
            {
                _machinePower = value;
                MachinePower_Validation(value);
                OnPropertyChanged("MachinePower");
            }
        }

        private int _numberOfHoursPerYear = -1;
        private void NumberOfHoursPerYear_Validation(int value)//TODO
        {

        }

        [Attributes.FormVisual("Количество часов работы за год")]
        public int NumberOfHoursPerYear
        {
            get { return _numberOfHoursPerYear; }
            set
            {
                _numberOfHoursPerYear = value;
                NumberOfHoursPerYear_Validation(value);
                OnPropertyChanged("NumberOfHoursPerYear");
            }
        }

        private string _codeRAOin = "";
        private void CodeRAOin_Validation(string value)//TODO
        {

        }

        [Attributes.FormVisual("Код РАО")]
        public string CodeRAOin
        {
            get { return _codeRAOin; }
            set
            {
                _codeRAOin = value;
                CodeRAOin_Validation(value);
                OnPropertyChanged("CodeRAOin");
            }
        }

        private string _statusRAOin = "";
        private void StatusRAOin_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("Статус РАО")]
        public string StatusRAOin  //1 cyfer or OKPO.
        {
            get { return _statusRAOin; }
            set
            {
                _statusRAOin = value;
                StatusRAOin_Validation(VolumeIn);
                OnPropertyChanged("StatusRAOin");
            }
        }

        private double _volumeIn = -1;
        private void VolumeIn_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("куб. м")]
        public double VolumeIn
        {
            get { return _volumeIn; }
            set
            {
                _volumeIn = value;
                VolumeIn_Validation(value);
                OnPropertyChanged("VolumeIn");
            }
        }

        private double _massIn = -1;
        private void MassIn_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("т")]
        public double MassIn
        {
            get { return _massIn; }
            set
            {
                _massIn = value;
                MassIn_Validation(value);
                OnPropertyChanged("MassIn");
            }
        }

        private int _quantityOZIIIin = -1;
        private void QuantityOZIIIin_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityOZIIIin));
            if (value <= 0)
                AddError(nameof(QuantityOZIIIin), "Недопустимое значение");
        }

        [Attributes.FormVisual("Количество ОЗИИИ, шт.")]
        public int QuantityOZIIIin
        {
            get { return _quantityOZIIIin; }
            set
            {
                _quantityOZIIIin = value;
                QuantityOZIIIin_Validation(value);
                OnPropertyChanged("QuantityOZIIIin");
            }
        }

        private string _tritiumActivityIn = "";
        private void TritiumActivityIn_Validation(string value)//TODO
        {
            ClearErrors(nameof(TritiumActivityIn));
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
                    AddError(nameof(TritiumActivityIn), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(TritiumActivityIn), "Недопустимое значение");
            }
        }

        [Attributes.FormVisual("Активность трития, Бк")]
        public string TritiumActivityIn
        {
            get { return _tritiumActivityIn; }
            set
            {
                _tritiumActivityIn = value;
                TritiumActivityIn_Validation(value);
                OnPropertyChanged("TritiumActivityIn");
            }
        }

        private string _betaGammaActivityIn = "";
        private void BetaGammaActivityIn_Validation(string value)//TODO
        {
            ClearErrors(nameof(BetaGammaActivityIn));
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
                    AddError(nameof(BetaGammaActivityIn), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(BetaGammaActivityIn), "Недопустимое значение");
            }
        }

        [Attributes.FormVisual("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public string BetaGammaActivityIn
        {
            get { return _betaGammaActivityIn; }
            set
            {
                _betaGammaActivityIn = value;
                BetaGammaActivityIn_Validation(value);
                OnPropertyChanged("BetaGammaActivityIn");
            }
        }

        private string _alphaActivityIn = "";
        private void AlphaActivityIn_Validation(string value)//TODO
        {
            ClearErrors(nameof(AlphaActivityIn));
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
                    AddError(nameof(AlphaActivityIn), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(AlphaActivityIn), "Недопустимое значение");
            }
        }

        [Attributes.FormVisual("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public string AlphaActivityIn
        {
            get { return _alphaActivityIn; }
            set
            {
                _alphaActivityIn = value;
                AlphaActivityIn_Validation(value);
                OnPropertyChanged("AlphaActivityIn");
            }
        }

        private string _transuraniumActivityIn = "";
        private void TransuraniumActivityIn_Validation(string value)//TODO
        {
            ClearErrors(nameof(TransuraniumActivityIn));
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
                    AddError(nameof(TransuraniumActivityIn), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(TransuraniumActivityIn), "Недопустимое значение");
            }
        }

        [Attributes.FormVisual("Активность трансурановых, Бк")]
        public string TransuraniumActivityIn
        {
            get { return _transuraniumActivityIn; }
            set
            {
                _transuraniumActivityIn = value;
                TransuraniumActivityIn_Validation(value);
                OnPropertyChanged("TransuraniumActivityIn");
            }
        }

        private string _codeRAOout = "";
        private void CodeRAOout_Validation(string value)//TODO
        {

        }

        [Attributes.FormVisual("Код РАО")]
        public string CodeRAOout
        {
            get { return _codeRAOout; }
            set
            {
                _codeRAOout = value;
                CodeRAOout_Validation(value);
                OnPropertyChanged("CodeRAOout");
            }
        }

        private string _statusRAOout = "";
        private void StatusRAOout_Validation(string value)//TODO
        {

        }

        [Attributes.FormVisual("Статус РАО")]
        public string StatusRAOout  //1 cyfer or OKPO.
        {
            get { return _statusRAOout; }
            set
            {
                _statusRAOout = value;
                StatusRAOout_Validation(value);
                OnPropertyChanged("StatusRAOout");
            }
        }

        private double _volumeOut = -1;
        private void VolumeOut_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("куб. м")]
        public double VolumeOut
        {
            get { return _volumeOut; }
            set
            {
                _volumeOut = value;
                VolumeOut_Validation(value);
                OnPropertyChanged("VolumeOut");
            }
        }

        private double _massOut = -1;
        private void MassOut_Validation(double value)//TODO
        {

        }

        [Attributes.FormVisual("т")]
        public double MassOut
        {
            get { return _massOut; }
            set
            {
                _massOut = value;
                MassOut_Validation(value);
                OnPropertyChanged("MassOut");
            }
        }

        private int _quantityOZIIIout = -1;
        private void QuantityOZIIIout_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityOZIIIout));
            if (value <= 0)
                AddError(nameof(QuantityOZIIIout), "Недопустимое значение");
        }

        [Attributes.FormVisual("Количество ОЗИИИ, шт.")]
        public int QuantityOZIIIout
        {
            get { return _quantityOZIIIout; }
            set
            {
                _quantityOZIIIout = value;
                QuantityOZIIIout_Validation(value);
                OnPropertyChanged("QuantityOZIIIout");
            }
        }

        private string _tritiumActivityOut = "";
        private void TritiumActivityOut_Validation(string value)//TODO
        {
            ClearErrors(nameof(TritiumActivityOut));
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
                    AddError(nameof(TritiumActivityOut), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(TritiumActivityOut), "Недопустимое значение");
            }
        }

        [Attributes.FormVisual("Активность трития, Бк")]
        public string TritiumActivityOut
        {
            get { return _tritiumActivityOut; }
            set
            {
                _tritiumActivityOut = value;
                TritiumActivityOut_Validation(value);
                OnPropertyChanged("TritiumActivityOut");
            }
        }

        private string _betaGammaActivityOut = "";
        private void BetaGammaActivityOut_Validation(string value)//TODO
        {
            ClearErrors(nameof(BetaGammaActivityOut));
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
                    AddError(nameof(BetaGammaActivityOut), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(BetaGammaActivityOut), "Недопустимое значение");
            }
        }

        [Attributes.FormVisual("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public string BetaGammaActivityOut
        {
            get { return _betaGammaActivityOut; }
            set
            {
                _betaGammaActivityOut = value;
                BetaGammaActivityOut_Validation(value);
                OnPropertyChanged("BetaGammaActivityOut");
            }
        }

        private string _alphaActivityOut = "";
        private void AlphaActivityOut_Validation(string value)//TODO
        {
            ClearErrors(nameof(AlphaActivityOut));
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
                    AddError(nameof(AlphaActivityOut), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(AlphaActivityOut), "Недопустимое значение");
            }
        }

        [Attributes.FormVisual("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public string AlphaActivityOut
        {
            get { return _alphaActivityOut; }
            set
            {
                _alphaActivityOut = value;
                AlphaActivityOut_Validation(value);
                OnPropertyChanged("AlphaActivityOut");
            }
        }

        private string _transuraniumActivityOut = "";
        private void TransuraniumActivityOut_Validation(string value)//TODO
        {
            ClearErrors(nameof(TransuraniumActivityOut));
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
                    AddError(nameof(TransuraniumActivityOut), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(TransuraniumActivityOut), "Недопустимое значение");
            }
        }

        [Attributes.FormVisual("Активность трансурановых, Бк")]
        public string TransuraniumActivityOut
        {
            get { return _transuraniumActivityOut; }
            set
            {
                _transuraniumActivityOut = value;
                TransuraniumActivityOut_Validation(value);
                OnPropertyChanged("TransuraniumActivityOut");
            }
        }
    }
}
