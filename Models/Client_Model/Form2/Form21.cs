using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 2.1: Сортировка, переработка и кондиционирование РАО на установках")]
    public class Form21: Form2
    {
        public override string FormNum { get { return "2.1"; } }
        public override void Object_Validation()
        {

        }

        public override int NumberOfFields { get; } = 22;

        //RefineMachineName property
        [Attributes.FormVisual("Наименование установки переработки")]
        public string RefineMachineName
        {
            get
            {
                if (GetErrors(nameof(RefineMachineName)) != null)
                {
                    return (string)_RefineMachineName.Get();
                }
                else
                {
                    return _RefineMachineName_Not_Valid;
                }
            }
            set
            {
                _RefineMachineName_Not_Valid = value;
                if (GetErrors(nameof(RefineMachineName)) != null)
                {
                    _RefineMachineName.Set(_RefineMachineName_Not_Valid);
                }
                OnPropertyChanged(nameof(RefineMachineName));
            }
        }
        private IDataLoadEngine _RefineMachineName;
        private string _RefineMachineName_Not_Valid = "";
        private void RefineMachineName_Validation()
        {
            ClearErrors(nameof(RefineMachineName));
        }
        //RefineMachineName property

        //MachineCode property
        [Attributes.FormVisual("Код установки переработки")]
        public short MachineCode
        {
            get
            {
                if (GetErrors(nameof(MachineCode)) != null)
                {
                    return (short)_MachineCode.Get();
                }
                else
                {
                    return _MachineCode_Not_Valid;
                }
            }
            set
            {
                _MachineCode_Not_Valid = value;
                if (GetErrors(nameof(MachineCode)) != null)
                {
                    _MachineCode.Set(_MachineCode_Not_Valid);
                }
                OnPropertyChanged(nameof(MachineCode));
            }
        }
        private IDataLoadEngine _MachineCode;
        private short _MachineCode_Not_Valid = 0;
        private void MachineCode_Validation(short value)//TODO
        {
            ClearErrors(nameof(MachineCode));
        }
        //MachineCode property

        //MachinePower property
        [Attributes.FormVisual("Мощность, куб. м/год")]
        public string MachinePower
        {
            get
            {
                if (GetErrors(nameof(MachinePower)) != null)
                {
                    return (string)_MachinePower.Get();
                }
                else
                {
                    return _MachinePower_Not_Valid;
                }
            }
            set
            {
                _MachinePower_Not_Valid = value;
                if (GetErrors(nameof(MachinePower)) != null)
                {
                    _MachinePower.Set(_MachinePower_Not_Valid);
                }
                OnPropertyChanged(nameof(MachinePower));
            }
        }
        private IDataLoadEngine _MachinePower;
        private string _MachinePower_Not_Valid = "";
        private void MachinePower_Validation(string value)//TODO
        {
            ClearErrors(nameof(MachinePower));
        }
        //MachinePower property

        //NumberOfHoursPerYear property
        [Attributes.FormVisual("Количество часов работы за год")]
        public int NumberOfHoursPerYear
        {
            get
            {
                if (GetErrors(nameof(NumberOfHoursPerYear)) != null)
                {
                    return (int)_NumberOfHoursPerYear.Get();
                }
                else
                {
                    return _NumberOfHoursPerYear_Not_Valid;
                }
            }
            set
            {
                _NumberOfHoursPerYear_Not_Valid = value;
                if (GetErrors(nameof(NumberOfHoursPerYear)) != null)
                {
                    _NumberOfHoursPerYear.Set(_NumberOfHoursPerYear_Not_Valid);
                }
                OnPropertyChanged(nameof(NumberOfHoursPerYear));
            }
        }
        private IDataLoadEngine _NumberOfHoursPerYear;
        private int _NumberOfHoursPerYear_Not_Valid = -1;
        private void NumberOfHoursPerYear_Validation(int value)//TODO
        {
            ClearErrors(nameof(NumberOfHoursPerYear));
        }
        //NumberOfHoursPerYear property

        //CodeRAOIn property
        [Attributes.FormVisual("Код РАО")]
        public string CodeRAOIn
        {
            get
            {
                if (GetErrors(nameof(CodeRAOIn)) != null)
                {
                    return (string)_CodeRAOIn.Get();
                }
                else
                {
                    return _CodeRAOIn_Not_Valid;
                }
            }
            set
            {
                _CodeRAOIn_Not_Valid = value;
                if (GetErrors(nameof(CodeRAOIn)) != null)
                {
                    _CodeRAOIn.Set(_CodeRAOIn_Not_Valid);
                }
                OnPropertyChanged(nameof(CodeRAOIn));
            }
        }
        private IDataLoadEngine _CodeRAOIn;
        private string _CodeRAOIn_Not_Valid = "";
        private void CodeRAOIn_Validation(string value)//TODO
        {
            ClearErrors(nameof(CodeRAOIn));
        }
        //CodeRAOIn property

        //StatusRAOIn property
        [Attributes.FormVisual("Статус РАО")]
        public string StatusRAOIn  //1 cyfer or OKPO.
        {
            get
            {
                if (GetErrors(nameof(StatusRAOIn)) != null)
                {
                    return (string)_StatusRAOIn.Get();
                }
                else
                {
                    return _StatusRAOIn_Not_Valid;
                }
            }
            set
            {
                _StatusRAOIn_Not_Valid = value;
                if (GetErrors(nameof(StatusRAOIn)) != null)
                {
                    _StatusRAOIn.Set(_StatusRAOIn_Not_Valid);
                }
                OnPropertyChanged(nameof(StatusRAOIn));
            }
        }
        private IDataLoadEngine _StatusRAOIn;
        private string _StatusRAOIn_Not_Valid = "";
        private void StatusRAO_Validation(string value)//TODO
        {
            ClearErrors(nameof(StatusRAOIn));
        }
        //StatusRAOIn property

        //VolumeIn property
        [Attributes.FormVisual("Объем, куб. м")]
        public double VolumeIn
        {
            get
            {
                if (GetErrors(nameof(VolumeIn)) != null)
                {
                    return (double)_VolumeIn.Get();
                }
                else
                {
                    return _VolumeIn_Not_Valid;
                }
            }
            set
            {
                _VolumeIn_Not_Valid = value;
                if (GetErrors(nameof(VolumeIn)) != null)
                {
                    _VolumeIn.Set(_VolumeIn_Not_Valid);
                }
                OnPropertyChanged(nameof(VolumeIn));
            }
        }
        private IDataLoadEngine _VolumeIn;
        private double _VolumeIn_Not_Valid = -1;
        private void VolumeIn_Validation(double value)//TODO
        {
            ClearErrors(nameof(VolumeIn));
        }
        //VolumeIn property

        //MassIn Property
        [Attributes.FormVisual("Масса, т")]
        public double MassIn
        {
            get
            {
                if (GetErrors(nameof(MassIn)) != null)
                {
                    return (double)_MassIn.Get();
                }
                else
                {
                    return _MassIn_Not_Valid;
                }
            }
            set
            {
                _MassIn_Not_Valid = value;
                if (GetErrors(nameof(MassIn)) != null)
                {
                    _MassIn.Set(_MassIn_Not_Valid);
                }
                OnPropertyChanged(nameof(MassIn));
            }
        }
        private IDataLoadEngine _MassIn;
        private double _MassIn_Not_Valid = -1;
        private void MassIn_Validation()//TODO
        {
            ClearErrors(nameof(MassIn));
        }
        //MassIn Property

        //QuantityIn property
        [Attributes.FormVisual("Количество ОЗИИИ, шт.")]
        public int QuantityIn
        {
            get
            {
                if (GetErrors(nameof(QuantityIn)) != null)
                {
                    return (int)_QuantityIn.Get();
                }
                else
                {
                    return _QuantityIn_Not_Valid;
                }
            }
            set
            {
                _QuantityIn_Not_Valid = value;
                if (GetErrors(nameof(QuantityIn)) != null)
                {
                    _QuantityIn.Set(_QuantityIn_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityIn));
            }
        }
        private IDataLoadEngine _QuantityIn;  // positive int.
        private int _QuantityIn_Not_Valid = -1;
        private void QuantityIn_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityIn));
            if (value <= 0)
                AddError(nameof(QuantityIn), "Недопустимое значение");
        }
        //QuantityIn property

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

        //CodeRAOout property
        [Attributes.FormVisual("Код РАО")]
        public string CodeRAOout
        {
            get
            {
                if (GetErrors(nameof(CodeRAOout)) != null)
                {
                    return (string)_CodeRAOout.Get();
                }
                else
                {
                    return _CodeRAOout_Not_Valid;
                }
            }
            set
            {
                _CodeRAOout_Not_Valid = value;
                if (GetErrors(nameof(CodeRAOout)) != null)
                {
                    _CodeRAOout.Set(_CodeRAOout_Not_Valid);
                }
                OnPropertyChanged(nameof(CodeRAOout));
            }
        }
        private IDataLoadEngine _CodeRAOout;
        private string _CodeRAOout_Not_Valid = "";
        private void CodeRAOout_Validation(string value)//TODO
        {
            ClearErrors(nameof(CodeRAOout));
        }
        //CodeRAOout property

        //StatusRAOout property
        [Attributes.FormVisual("Статус РАО")]
        public string StatusRAOout  //1 cyfer or OKPO.
        {
            get
            {
                if (GetErrors(nameof(StatusRAOout)) != null)
                {
                    return (string)_StatusRAOout.Get();
                }
                else
                {
                    return _StatusRAOout_Not_Valid;
                }
            }
            set
            {
                _StatusRAOout_Not_Valid = value;
                if (GetErrors(nameof(StatusRAOout)) != null)
                {
                    _StatusRAOout.Set(_StatusRAOout_Not_Valid);
                }
                OnPropertyChanged(nameof(StatusRAOout));
            }
        }
        private IDataLoadEngine _StatusRAOout;
        private string _StatusRAOout_Not_Valid = "";
        private void StatusRAOout_Validation(string value)//TODO
        {
            ClearErrors(nameof(StatusRAOout));
        }
        //StatusRAOout property

        //VolumeOut property
        [Attributes.FormVisual("Объем, куб. м")]
        public double VolumeOut
        {
            get
            {
                if (GetErrors(nameof(VolumeOut)) != null)
                {
                    return (double)_VolumeOut.Get();
                }
                else
                {
                    return _VolumeOut_Not_Valid;
                }
            }
            set
            {
                _VolumeOut_Not_Valid = value;
                if (GetErrors(nameof(VolumeOut)) != null)
                {
                    _VolumeOut.Set(_VolumeOut_Not_Valid);
                }
                OnPropertyChanged(nameof(VolumeOut));
            }
        }
        private IDataLoadEngine _VolumeOut;
        private double _VolumeOut_Not_Valid = -1;
        private void VolumeOut_Validation(double value)//TODO
        {
            ClearErrors(nameof(VolumeOut));
        }
        //VolumeOut property

        //MassOut Property
        [Attributes.FormVisual("Масса, т")]
        public double MassOut
        {
            get
            {
                if (GetErrors(nameof(MassOut)) != null)
                {
                    return (double)_MassOut.Get();
                }
                else
                {
                    return _MassOut_Not_Valid;
                }
            }
            set
            {
                _MassOut_Not_Valid = value;
                if (GetErrors(nameof(MassOut)) != null)
                {
                    _MassOut.Set(_MassOut_Not_Valid);
                }
                OnPropertyChanged(nameof(MassOut));
            }
        }
        private IDataLoadEngine _MassOut;
        private double _MassOut_Not_Valid = -1;
        private void MassOut_Validation()//TODO
        {
            ClearErrors(nameof(MassOut));
        }
        //MassOut Property

        //QuantityOZIIIout property
        [Attributes.FormVisual("Количество ОЗИИИ, шт.")]
        public int QuantityOZIIIout
        {
            get
            {
                if (GetErrors(nameof(QuantityOZIIIout)) != null)
                {
                    return (int)_QuantityOZIIIout.Get();
                }
                else
                {
                    return _QuantityOZIIIout_Not_Valid;
                }
            }
            set
            {
                _QuantityOZIIIout_Not_Valid = value;
                if (GetErrors(nameof(QuantityOZIIIout)) != null)
                {
                    _QuantityOZIIIout.Set(_QuantityOZIIIout_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityOZIIIout));
            }
        }
        private IDataLoadEngine _QuantityOZIIIout;  // positive int.
        private int _QuantityOZIIIout_Not_Valid = -1;
        private void QuantityOZIIIout_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityOZIIIout));
            if (value <= 0)
                AddError(nameof(QuantityOZIIIout), "Недопустимое значение");
        }
        //QuantityOZIIIout property

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
