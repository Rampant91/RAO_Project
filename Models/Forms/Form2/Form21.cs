using Collections.Rows_Collection;
using System;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.1: Сортировка, переработка и кондиционирование РАО на установках")]
    public class Form21 : Abstracts.Form2
    {
        public Form21(IDataAccess Access) : base(Access)
        {
            FormNum = "21";
            NumberOfFields = 24;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //RefineMachineName property
        [Attributes.Form_Property("Наименование установки переработки")]
        public string RefineMachineName
        {
            get
            {
                if (GetErrors(nameof(RefineMachineName)) == null)
                {
                    return (string)_dataAccess.Get(nameof(RefineMachineName))[0][0];
                }
                else
                {
                    return _RefineMachineName_Not_Valid;
                }
            }
            set
            {
                _RefineMachineName_Not_Valid = value;
                if (GetErrors(nameof(RefineMachineName)) == null)
                {
                    _dataAccess.Set(nameof(RefineMachineName), _RefineMachineName_Not_Valid);
                }
                OnPropertyChanged(nameof(RefineMachineName));
            }
        }

        private string _RefineMachineName_Not_Valid = "";
        private void RefineMachineName_Validation()
        {
            ClearErrors(nameof(RefineMachineName));
        }
        //RefineMachineName property

        //MachineCode property
        [Attributes.Form_Property("Код установки переработки")]
        public short MachineCode
        {
            get
            {
                if (GetErrors(nameof(MachineCode)) == null)
                {
                    return (short)_dataAccess.Get(nameof(MachineCode))[0][0];
                }
                else
                {
                    return _MachineCode_Not_Valid;
                }
            }
            set
            {
                _MachineCode_Not_Valid = value;
                if (GetErrors(nameof(MachineCode)) == null)
                {
                    _dataAccess.Set(nameof(MachineCode), _MachineCode_Not_Valid);
                }
                OnPropertyChanged(nameof(MachineCode));
            }
        }

        private short _MachineCode_Not_Valid = 0;
        private void MachineCode_Validation(short value)//TODO
        {
            ClearErrors(nameof(MachineCode));
        }
        //MachineCode property

        //MachinePower property
        [Attributes.Form_Property("Мощность, куб. м/год")]
        public string MachinePower
        {
            get
            {
                if (GetErrors(nameof(MachinePower)) == null)
                {
                    return (string)_dataAccess.Get(nameof(MachinePower))[0][0];
                }
                else
                {
                    return _MachinePower_Not_Valid;
                }
            }
            set
            {
                _MachinePower_Not_Valid = value;
                if (GetErrors(nameof(MachinePower)) == null)
                {
                    _dataAccess.Set(nameof(MachinePower), _MachinePower_Not_Valid);
                }
                OnPropertyChanged(nameof(MachinePower));
            }
        }

        private string _MachinePower_Not_Valid = "";
        private void MachinePower_Validation(string value)//TODO
        {
            ClearErrors(nameof(MachinePower));
        }
        //MachinePower property

        //NumberOfHoursPerYear property
        [Attributes.Form_Property("Количество часов работы за год")]
        public int NumberOfHoursPerYear
        {
            get
            {
                if (GetErrors(nameof(NumberOfHoursPerYear)) == null)
                {
                    return (int)_dataAccess.Get(nameof(NumberOfHoursPerYear))[0][0];
                }
                else
                {
                    return _NumberOfHoursPerYear_Not_Valid;
                }
            }
            set
            {
                _NumberOfHoursPerYear_Not_Valid = value;
                if (GetErrors(nameof(NumberOfHoursPerYear)) == null)
                {
                    _dataAccess.Set(nameof(NumberOfHoursPerYear), _NumberOfHoursPerYear_Not_Valid);
                }
                OnPropertyChanged(nameof(NumberOfHoursPerYear));
            }
        }

        private int _NumberOfHoursPerYear_Not_Valid = -1;
        private void NumberOfHoursPerYear_Validation(int value)//TODO
        {
            ClearErrors(nameof(NumberOfHoursPerYear));
        }
        //NumberOfHoursPerYear property

        //CodeRAOIn property
        [Attributes.Form_Property("Код РАО")]
        public string CodeRAOIn
        {
            get
            {
                if (GetErrors(nameof(CodeRAOIn)) == null)
                {
                    return (string)_dataAccess.Get(nameof(CodeRAOIn))[0][0];
                }
                else
                {
                    return _CodeRAOIn_Not_Valid;
                }
            }
            set
            {
                _CodeRAOIn_Not_Valid = value;
                if (GetErrors(nameof(CodeRAOIn)) == null)
                {
                    _dataAccess.Set(nameof(CodeRAOIn), _CodeRAOIn_Not_Valid);
                }
                OnPropertyChanged(nameof(CodeRAOIn));
            }
        }

        private string _CodeRAOIn_Not_Valid = "";
        private void CodeRAOIn_Validation(string value)//TODO
        {
            ClearErrors(nameof(CodeRAOIn));
        }
        //CodeRAOIn property

        //StatusRAOIn property
        [Attributes.Form_Property("Статус РАО")]
        public string StatusRAOIn  //1 cyfer or OKPO.
        {
            get
            {
                if (GetErrors(nameof(StatusRAOIn)) == null)
                {
                    return (string)_dataAccess.Get(nameof(StatusRAOIn))[0][0];
                }
                else
                {
                    return _StatusRAOIn_Not_Valid;
                }
            }
            set
            {
                _StatusRAOIn_Not_Valid = value;
                if (GetErrors(nameof(StatusRAOIn)) == null)
                {
                    _dataAccess.Set(nameof(StatusRAOIn), _StatusRAOIn_Not_Valid);
                }
                OnPropertyChanged(nameof(StatusRAOIn));
            }
        }

        private string _StatusRAOIn_Not_Valid = "";
        private void StatusRAO_Validation(string value)//TODO
        {
            ClearErrors(nameof(StatusRAOIn));
        }
        //StatusRAOIn property

        //VolumeIn property
        [Attributes.Form_Property("Объем, куб. м")]
        public double VolumeIn
        {
            get
            {
                if (GetErrors(nameof(VolumeIn)) == null)
                {
                    return (double)_dataAccess.Get(nameof(VolumeIn))[0][0];
                }
                else
                {
                    return _VolumeIn_Not_Valid;
                }
            }
            set
            {
                _VolumeIn_Not_Valid = value;
                if (GetErrors(nameof(VolumeIn)) == null)
                {
                    _dataAccess.Set(nameof(VolumeIn), _VolumeIn_Not_Valid);
                }
                OnPropertyChanged(nameof(VolumeIn));
            }
        }

        private double _VolumeIn_Not_Valid = -1;
        private void VolumeIn_Validation(double value)//TODO
        {
            ClearErrors(nameof(VolumeIn));
        }
        //VolumeIn property

        //MassIn Property
        [Attributes.Form_Property("Масса, т")]
        public double MassIn
        {
            get
            {
                if (GetErrors(nameof(MassIn)) == null)
                {
                    return (double)_dataAccess.Get(nameof(MassIn))[0][0];
                }
                else
                {
                    return _MassIn_Not_Valid;
                }
            }
            set
            {
                _MassIn_Not_Valid = value;
                if (GetErrors(nameof(MassIn)) == null)
                {
                    _dataAccess.Set(nameof(MassIn), _MassIn_Not_Valid);
                }
                OnPropertyChanged(nameof(MassIn));
            }
        }

        private double _MassIn_Not_Valid = -1;
        private void MassIn_Validation()//TODO
        {
            ClearErrors(nameof(MassIn));
        }
        //MassIn Property

        //QuantityIn property
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public int QuantityIn
        {
            get
            {
                if (GetErrors(nameof(QuantityIn)) == null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityIn))[0][0];
                }
                else
                {
                    return _QuantityIn_Not_Valid;
                }
            }
            set
            {
                _QuantityIn_Not_Valid = value;
                if (GetErrors(nameof(QuantityIn)) == null)
                {
                    _dataAccess.Set(nameof(QuantityIn), _QuantityIn_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityIn));
            }
        }
        // positive int.
        private int _QuantityIn_Not_Valid = -1;
        private void QuantityIn_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityIn));
            if (value <= 0)
                AddError(nameof(QuantityIn), "Недопустимое значение");
        }
        //QuantityIn property

        //TritiumActivityIn property
        [Attributes.Form_Property("Активность трития, Бк")]
        public string TritiumActivityIn
        {
            get
            {
                if (GetErrors(nameof(TritiumActivityIn)) == null)
                {
                    return (string)_dataAccess.Get(nameof(TritiumActivityIn))[0][0];
                }
                else
                {
                    return _TritiumActivityIn_Not_Valid;
                }
            }
            set
            {
                _TritiumActivityIn_Not_Valid = value;
                if (GetErrors(nameof(TritiumActivityIn)) == null)
                {
                    _dataAccess.Set(nameof(TritiumActivityIn), _TritiumActivityIn_Not_Valid);
                }
                OnPropertyChanged(nameof(TritiumActivityIn));
            }
        }

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
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public string BetaGammaActivityIn
        {
            get
            {
                if (GetErrors(nameof(BetaGammaActivityIn)) == null)
                {
                    return (string)_dataAccess.Get(nameof(BetaGammaActivityIn))[0][0];
                }
                else
                {
                    return _BetaGammaActivityIn_Not_Valid;
                }
            }
            set
            {
                _BetaGammaActivityIn_Not_Valid = value;
                if (GetErrors(nameof(BetaGammaActivityIn)) == null)
                {
                    _dataAccess.Set(nameof(BetaGammaActivityIn), _BetaGammaActivityIn_Not_Valid);
                }
                OnPropertyChanged(nameof(BetaGammaActivityIn));
            }
        }

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
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public string AlphaActivityIn
        {
            get
            {
                if (GetErrors(nameof(AlphaActivityIn)) == null)
                {
                    return (string)_dataAccess.Get(nameof(AlphaActivityIn))[0][0];
                }
                else
                {
                    return _AlphaActivityIn_Not_Valid;
                }
            }
            set
            {
                _AlphaActivityIn_Not_Valid = value;
                if (GetErrors(nameof(AlphaActivityIn)) == null)
                {
                    _dataAccess.Set(nameof(AlphaActivityIn), _AlphaActivityIn_Not_Valid);
                }
                OnPropertyChanged(nameof(AlphaActivityIn));
            }
        }

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
        [Attributes.Form_Property("Активность трансурановых, Бк")]
        public string TransuraniumActivityIn
        {
            get
            {
                if (GetErrors(nameof(TransuraniumActivityIn)) == null)
                {
                    return (string)_dataAccess.Get(nameof(TransuraniumActivityIn))[0][0];
                }
                else
                {
                    return _TransuraniumActivityIn_Not_Valid;
                }
            }
            set
            {
                _TransuraniumActivityIn_Not_Valid = value;
                if (GetErrors(nameof(TransuraniumActivityIn)) == null)
                {
                    _dataAccess.Set(nameof(TransuraniumActivityIn), _TransuraniumActivityIn_Not_Valid);
                }
                OnPropertyChanged(nameof(TransuraniumActivityIn));
            }
        }

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
        [Attributes.Form_Property("Код РАО")]
        public string CodeRAOout
        {
            get
            {
                if (GetErrors(nameof(CodeRAOout)) == null)
                {
                    return (string)_dataAccess.Get(nameof(CodeRAOout))[0][0];
                }
                else
                {
                    return _CodeRAOout_Not_Valid;
                }
            }
            set
            {
                _CodeRAOout_Not_Valid = value;
                if (GetErrors(nameof(CodeRAOout)) == null)
                {
                    _dataAccess.Set(nameof(CodeRAOout), _CodeRAOout_Not_Valid);
                }
                OnPropertyChanged(nameof(CodeRAOout));
            }
        }

        private string _CodeRAOout_Not_Valid = "";
        private void CodeRAOout_Validation(string value)//TODO
        {
            ClearErrors(nameof(CodeRAOout));
        }
        //CodeRAOout property

        //StatusRAOout property
        [Attributes.Form_Property("Статус РАО")]
        public string StatusRAOout  //1 cyfer or OKPO.
        {
            get
            {
                if (GetErrors(nameof(StatusRAOout)) == null)
                {
                    return (string)_dataAccess.Get(nameof(StatusRAOout))[0][0];
                }
                else
                {
                    return _StatusRAOout_Not_Valid;
                }
            }
            set
            {
                _StatusRAOout_Not_Valid = value;
                if (GetErrors(nameof(StatusRAOout)) == null)
                {
                    _dataAccess.Set(nameof(StatusRAOout), _StatusRAOout_Not_Valid);
                }
                OnPropertyChanged(nameof(StatusRAOout));
            }
        }

        private string _StatusRAOout_Not_Valid = "";
        private void StatusRAOout_Validation(string value)//TODO
        {
            ClearErrors(nameof(StatusRAOout));
        }
        //StatusRAOout property

        //VolumeOut property
        [Attributes.Form_Property("Объем, куб. м")]
        public double VolumeOut
        {
            get
            {
                if (GetErrors(nameof(VolumeOut)) == null)
                {
                    return (double)_dataAccess.Get(nameof(VolumeOut))[0][0];
                }
                else
                {
                    return _VolumeOut_Not_Valid;
                }
            }
            set
            {
                _VolumeOut_Not_Valid = value;
                if (GetErrors(nameof(VolumeOut)) == null)
                {
                    _dataAccess.Set(nameof(VolumeOut), _VolumeOut_Not_Valid);
                }
                OnPropertyChanged(nameof(VolumeOut));
            }
        }

        private double _VolumeOut_Not_Valid = -1;
        private void VolumeOut_Validation(double value)//TODO
        {
            ClearErrors(nameof(VolumeOut));
        }
        //VolumeOut property

        //MassOut Property
        [Attributes.Form_Property("Масса, т")]
        public double MassOut
        {
            get
            {
                if (GetErrors(nameof(MassOut)) == null)
                {
                    return (double)_dataAccess.Get(nameof(MassOut))[0][0];
                }
                else
                {
                    return _MassOut_Not_Valid;
                }
            }
            set
            {
                _MassOut_Not_Valid = value;
                if (GetErrors(nameof(MassOut)) == null)
                {
                    _dataAccess.Set(nameof(MassOut), _MassOut_Not_Valid);
                }
                OnPropertyChanged(nameof(MassOut));
            }
        }

        private double _MassOut_Not_Valid = -1;
        private void MassOut_Validation()//TODO
        {
            ClearErrors(nameof(MassOut));
        }
        //MassOut Property

        //QuantityOZIIIout property
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public int QuantityOZIIIout
        {
            get
            {
                if (GetErrors(nameof(QuantityOZIIIout)) == null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityOZIIIout))[0][0];
                }
                else
                {
                    return _QuantityOZIIIout_Not_Valid;
                }
            }
            set
            {
                _QuantityOZIIIout_Not_Valid = value;
                if (GetErrors(nameof(QuantityOZIIIout)) == null)
                {
                    _dataAccess.Set(nameof(QuantityOZIIIout), _QuantityOZIIIout_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityOZIIIout));
            }
        }
        // positive int.
        private int _QuantityOZIIIout_Not_Valid = -1;
        private void QuantityOZIIIout_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityOZIIIout));
            if (value <= 0)
                AddError(nameof(QuantityOZIIIout), "Недопустимое значение");
        }
        //QuantityOZIIIout property

        //TritiumActivityOut property
        [Attributes.Form_Property("Активность трития, Бк")]
        public string TritiumActivityOut
        {
            get
            {
                if (GetErrors(nameof(TritiumActivityOut)) == null)
                {
                    return (string)_dataAccess.Get(nameof(TritiumActivityOut))[0][0];
                }
                else
                {
                    return _TritiumActivityOut_Not_Valid;
                }
            }
            set
            {
                _TritiumActivityOut_Not_Valid = value;
                if (GetErrors(nameof(TritiumActivityOut)) == null)
                {
                    _dataAccess.Set(nameof(TritiumActivityOut), _TritiumActivityOut_Not_Valid);
                }
                OnPropertyChanged(nameof(TritiumActivityOut));
            }
        }

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
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public string BetaGammaActivityOut
        {
            get
            {
                if (GetErrors(nameof(BetaGammaActivityOut)) == null)
                {
                    return (string)_dataAccess.Get(nameof(BetaGammaActivityOut))[0][0];
                }
                else
                {
                    return _BetaGammaActivityOut_Not_Valid;
                }
            }
            set
            {
                _BetaGammaActivityOut_Not_Valid = value;
                if (GetErrors(nameof(BetaGammaActivityOut)) == null)
                {
                    _dataAccess.Set(nameof(BetaGammaActivityOut), _BetaGammaActivityOut_Not_Valid);
                }
                OnPropertyChanged(nameof(BetaGammaActivityOut));
            }
        }

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
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public string AlphaActivityOut
        {
            get
            {
                if (GetErrors(nameof(AlphaActivityOut)) == null)
                {
                    return (string)_dataAccess.Get(nameof(AlphaActivityOut))[0][0];
                }
                else
                {
                    return _AlphaActivityOut_Not_Valid;
                }
            }
            set
            {
                _AlphaActivityOut_Not_Valid = value;
                if (GetErrors(nameof(AlphaActivityOut)) == null)
                {
                    _dataAccess.Set(nameof(AlphaActivityOut), _AlphaActivityOut_Not_Valid);
                }
                OnPropertyChanged(nameof(AlphaActivityOut));
            }
        }

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
        [Attributes.Form_Property("Активность трансурановых, Бк")]
        public string TransuraniumActivityOut
        {
            get
            {
                if (GetErrors(nameof(TransuraniumActivityOut)) == null)
                {
                    return (string)_dataAccess.Get(nameof(TransuraniumActivityOut))[0][0];
                }
                else
                {
                    return _TransuraniumActivityOut_Not_Valid;
                }
            }
            set
            {
                _TransuraniumActivityOut_Not_Valid = value;
                if (GetErrors(nameof(TransuraniumActivityOut)) == null)
                {
                    _dataAccess.Set(nameof(TransuraniumActivityOut), _TransuraniumActivityOut_Not_Valid);
                }
                OnPropertyChanged(nameof(TransuraniumActivityOut));
            }
        }

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
