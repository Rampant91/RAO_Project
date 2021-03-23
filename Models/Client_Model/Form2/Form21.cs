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

        //CorrectionNumber property
        [Attributes.FormVisual("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    return (byte)_CorrectionNumber.Get();
                }
                else
                {
                    return _CorrectionNumber_Not_Valid;
                }
            }
            set
            {
                _CorrectionNumber_Not_Valid = value;
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    _CorrectionNumber.Set(_CorrectionNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private IDataLoadEngine _CorrectionNumber;
        private byte _CorrectionNumber_Not_Valid = 255;
        private void CorrectionNumber_Validation()
        {
            ClearErrors(nameof(CorrectionNumber));
        }
        //CorrectionNumber property

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

        //TritiumActivityIn property
        [Attributes.FormVisual("Активность трития, Бк")]
        public string TritiumActivityIn
        {
            get
            {
                if (GetErrors(nameof(TritiumActivityIn)) != null)
                {
                    return (string)_TritiumActivityIn.Get();
                }
                else
                {
                    return _TritiumActivityIn_Not_Valid;
                }
            }
            set
            {
                _TritiumActivityIn_Not_Valid = value;
                if (GetErrors(nameof(TritiumActivityIn)) != null)
                {
                    _TritiumActivityIn.Set(_TritiumActivityIn_Not_Valid);
                }
                OnPropertyChanged(nameof(TritiumActivityIn));
            }
        }
        private IDataLoadEngine _TritiumActivityIn;
        private string _TritiumActivityIn_Not_Valid = "";
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
        //TritiumActivityIn property

        //BetaGammaActivityIn property
        [Attributes.FormVisual("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public string BetaGammaActivityIn
        {
            get
            {
                if (GetErrors(nameof(BetaGammaActivityIn)) != null)
                {
                    return (string)_BetaGammaActivityIn.Get();
                }
                else
                {
                    return _BetaGammaActivityIn_Not_Valid;
                }
            }
            set
            {
                _BetaGammaActivityIn_Not_Valid = value;
                if (GetErrors(nameof(BetaGammaActivityIn)) != null)
                {
                    _BetaGammaActivityIn.Set(_BetaGammaActivityIn_Not_Valid);
                }
                OnPropertyChanged(nameof(BetaGammaActivityIn));
            }
        }
        private IDataLoadEngine _BetaGammaActivityIn;
        private string _BetaGammaActivityIn_Not_Valid = "";
        private void BetaGammaActivity_Validation(string value)//TODO
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
        //BetaGammaActivity property

        //AlphaActivityIn property
        [Attributes.FormVisual("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public string AlphaActivityIn
        {
            get
            {
                if (GetErrors(nameof(AlphaActivityIn)) != null)
                {
                    return (string)_AlphaActivityIn.Get();
                }
                else
                {
                    return _AlphaActivityIn_Not_Valid;
                }
            }
            set
            {
                _AlphaActivityIn_Not_Valid = value;
                if (GetErrors(nameof(AlphaActivityIn)) != null)
                {
                    _AlphaActivityIn.Set(_AlphaActivityIn_Not_Valid);
                }
                OnPropertyChanged(nameof(AlphaActivityIn));
            }
        }
        private IDataLoadEngine _AlphaActivityIn;
        private string _AlphaActivityIn_Not_Valid = "";
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
        //AlphaActivityIn property

        //TransuraniumActivityIn property
        [Attributes.FormVisual("Активность трансурановых, Бк")]
        public string TransuraniumActivityIn
        {
            get
            {
                if (GetErrors(nameof(TransuraniumActivityIn)) != null)
                {
                    return (string)_TransuraniumActivityIn.Get();
                }
                else
                {
                    return _TransuraniumActivityIn_Not_Valid;
                }
            }
            set
            {
                _TransuraniumActivityIn_Not_Valid = value;
                if (GetErrors(nameof(TransuraniumActivityIn)) != null)
                {
                    _TransuraniumActivityIn.Set(_TransuraniumActivityIn_Not_Valid);
                }
                OnPropertyChanged(nameof(TransuraniumActivityIn));
            }
        }
        private IDataLoadEngine _TransuraniumActivityIn;
        private string _TransuraniumActivityIn_Not_Valid = "";
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
        //TransuraniumActivityIn property

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

        //TritiumActivityOut property
        [Attributes.FormVisual("Активность трития, Бк")]
        public string TritiumActivityOut
        {
            get
            {
                if (GetErrors(nameof(TritiumActivityOut)) != null)
                {
                    return (string)_TritiumActivityOut.Get();
                }
                else
                {
                    return _TritiumActivityOut_Not_Valid;
                }
            }
            set
            {
                _TritiumActivityOut_Not_Valid = value;
                if (GetErrors(nameof(TritiumActivityOut)) != null)
                {
                    _TritiumActivityOut.Set(_TritiumActivityOut_Not_Valid);
                }
                OnPropertyChanged(nameof(TritiumActivityOut));
            }
        }
        private IDataLoadEngine _TritiumActivityOut;
        private string _TritiumActivityOut_Not_Valid = "";
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
        //TritiumActivityOut property

        //BetaGammaActivityOut property
        [Attributes.FormVisual("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public string BetaGammaActivityOut
        {
            get
            {
                if (GetErrors(nameof(BetaGammaActivityOut)) != null)
                {
                    return (string)_BetaGammaActivityOut.Get();
                }
                else
                {
                    return _BetaGammaActivityOut_Not_Valid;
                }
            }
            set
            {
                _BetaGammaActivityOut_Not_Valid = value;
                if (GetErrors(nameof(BetaGammaActivityOut)) != null)
                {
                    _BetaGammaActivityOut.Set(_BetaGammaActivityOut_Not_Valid);
                }
                OnPropertyChanged(nameof(BetaGammaActivityOut));
            }
        }
        private IDataLoadEngine _BetaGammaActivityOut;
        private string _BetaGammaActivityOut_Not_Valid = "";
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
        //BetaGammaActivityOut property

        //AlphaActivityOut property
        [Attributes.FormVisual("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public string AlphaActivityOut
        {
            get
            {
                if (GetErrors(nameof(AlphaActivityOut)) != null)
                {
                    return (string)_AlphaActivityOut.Get();
                }
                else
                {
                    return _AlphaActivityOut_Not_Valid;
                }
            }
            set
            {
                _AlphaActivityOut_Not_Valid = value;
                if (GetErrors(nameof(AlphaActivityOut)) != null)
                {
                    _AlphaActivityOut.Set(_AlphaActivityOut_Not_Valid);
                }
                OnPropertyChanged(nameof(AlphaActivityOut));
            }
        }
        private IDataLoadEngine _AlphaActivityOut;
        private string _AlphaActivityOut_Not_Valid = "";
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
        //AlphaActivityOut property

        //TransuraniumActivityOut property
        [Attributes.FormVisual("Активность трансурановых, Бк")]
        public string TransuraniumActivityOut
        {
            get
            {
                if (GetErrors(nameof(TransuraniumActivityOut)) != null)
                {
                    return (string)_TransuraniumActivityOut.Get();
                }
                else
                {
                    return _TransuraniumActivityOut_Not_Valid;
                }
            }
            set
            {
                _TransuraniumActivityOut_Not_Valid = value;
                if (GetErrors(nameof(TransuraniumActivityOut)) != null)
                {
                    _TransuraniumActivityOut.Set(_TransuraniumActivityOut_Not_Valid);
                }
                OnPropertyChanged(nameof(TransuraniumActivityOut));
            }
        }
        private IDataLoadEngine _TransuraniumActivityOut;
        private string _TransuraniumActivityOut_Not_Valid = "";
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
        //TransuraniumActivityOut property
    }
}
