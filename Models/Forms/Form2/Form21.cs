using Models.DataAccess;
using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.1: Сортировка, переработка и кондиционирование РАО на установках")]
    public class Form21 : Abstracts.Form2
    {
        public Form21() : base()
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
                    return (string)_dataAccess.Get(nameof(RefineMachineName));
                }
                else
                {
                    return _RefineMachineName_Not_Valid;
                }
            }
            set
            {
                RefineMachineName_Validation(value);
                if (GetErrors(nameof(RefineMachineName)) == null)
                {
                    _dataAccess.Set(nameof(RefineMachineName), value);
                }
                OnPropertyChanged(nameof(RefineMachineName));
            }
        }

        private string _RefineMachineName_Not_Valid = "";
        private void RefineMachineName_Validation(string value)
        {
            ClearErrors(nameof(RefineMachineName));
        }
        //RefineMachineName property

        //MachineCode property
        [Attributes.Form_Property("Код установки переработки")]
        public byte? MachineCode
        {
            get
            {
                if (GetErrors(nameof(MachineCode)) == null)
                {
                    return (byte?)_dataAccess.Get(nameof(MachineCode));
                }
                else
                {
                    return _MachineCode_Not_Valid;
                }
            }
            set
            {
                MachineCode_Validation(value);
                if (GetErrors(nameof(MachineCode)) == null)
                {
                    _dataAccess.Set(nameof(MachineCode), value);
                }
                OnPropertyChanged(nameof(MachineCode));
            }
        }

        private byte? _MachineCode_Not_Valid = 0;
        private void MachineCode_Validation(byte? value)//TODO
        {
            ClearErrors(nameof(MachineCode));
            if (value == null) return;
            bool a = (value >= 11) && (value <= 17);
            bool b = (value >= 21) && (value <= 24);
            bool c = (value >= 31) && (value <= 32);
            bool d = (value >= 41) && (value <= 43);
            bool e = (value >= 51) && (value <= 56);
            bool f = (value >= 61) && (value <= 63);
            bool g = (value >= 71) && (value <= 73);
            bool h = (value == 19) || (value == 29) || (value == 39) || (value == 49) || (value == 99) || (value == 79);
            if (!(a || b || c || d || e || f || g || h))
                AddError(nameof(MachineCode), "Недопустимое значение");
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
                    return (string)_dataAccess.Get(nameof(MachinePower));
                }
                else
                {
                    return _MachinePower_Not_Valid;
                }
            }
            set
            {
                MachinePower_Validation(value);
                if (GetErrors(nameof(MachinePower)) == null)
                {
                    _dataAccess.Set(nameof(MachinePower), value);
                }
                OnPropertyChanged(nameof(MachinePower));
            }
        }

        private string _MachinePower_Not_Valid = "";
        private void MachinePower_Validation(string value)//TODO
        {
            ClearErrors(nameof(MachinePower));
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            if (value.Equals("прим."))
            {
                
            }
            string tmp = value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(MachinePower), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(MachinePower), "Недопустимое значение");
            }
        }
        //MachinePower property

        //NumberOfHoursPerYear property
        [Attributes.Form_Property("Количество часов работы за год")]
        public string NumberOfHoursPerYear
        {
            get
            {
                if (GetErrors(nameof(NumberOfHoursPerYear)) == null)
                {
                    return (string)_dataAccess.Get(nameof(NumberOfHoursPerYear));
                }
                else
                {
                    return _NumberOfHoursPerYear_Not_Valid;
                }
            }
            set
            {
                NumberOfHoursPerYear_Validation(value);
                if (GetErrors(nameof(NumberOfHoursPerYear)) == null)
                {
                    _dataAccess.Set(nameof(NumberOfHoursPerYear), value);
                }
                OnPropertyChanged(nameof(NumberOfHoursPerYear));
            }
        }

        private string _NumberOfHoursPerYear_Not_Valid = "";
        private void NumberOfHoursPerYear_Validation(string value)//TODO
        {
            ClearErrors(nameof(NumberOfHoursPerYear));
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            if (value.Equals("прим.")||value.Equals("0"))
            {

            }
            string tmp = value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(NumberOfHoursPerYear), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(NumberOfHoursPerYear), "Недопустимое значение");
            }
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
                    return (string)_dataAccess.Get(nameof(CodeRAOIn));
                }
                else
                {
                    return _CodeRAOIn_Not_Valid;
                }
            }
            set
            {
                CodeRAOIn_Validation(value);
                if (GetErrors(nameof(CodeRAOIn)) == null)
                {
                    _dataAccess.Set(nameof(CodeRAOIn), value);
                }
                OnPropertyChanged(nameof(CodeRAOIn));
            }
        }

        private string _CodeRAOIn_Not_Valid = "";
        private void CodeRAOIn_Validation(string value)//TODO
        {
            ClearErrors(nameof(CodeRAOIn));
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            var a = new Regex("^[0-9]{11}$");
            if (!a.IsMatch(value))
            {
                AddError(nameof(CodeRAOIn), "Недопустимое значение");
                return;
            }
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
                    return (string)_dataAccess.Get(nameof(StatusRAOIn));
                }
                else
                {
                    return _StatusRAOIn_Not_Valid;
                }
            }
            set
            {
                StatusRAOIn_Validation(value);
                if (GetErrors(nameof(StatusRAOIn)) == null)
                {
                    _dataAccess.Set(nameof(StatusRAOIn), value);
                }
                OnPropertyChanged(nameof(StatusRAOIn));
            }
        }

        private string _StatusRAOIn_Not_Valid = "";
        private void StatusRAOIn_Validation(string value)//TODO
        {
            ClearErrors(nameof(StatusRAOIn));
            if (string.IsNullOrEmpty(value)) return;
            if (value.Length == 1)
            {
                int tmp;
                try
                {
                    tmp = int.Parse(value);
                    if ((tmp < 1) || ((tmp > 4) && (tmp != 6) && (tmp != 9)))
                    {
                        AddError(nameof(StatusRAOIn), "Недопустимое значение");
                    }
                }
                catch (Exception)
                {
                    AddError(nameof(StatusRAOIn), "Недопустимое значение");
                }
                return;
            }
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(StatusRAOIn), "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value))
                    AddError(nameof(StatusRAOIn), "Недопустимое значение");
            }
        }
        //StatusRAOIn property

        //VolumeIn property
        [Attributes.Form_Property("Объем, куб. м")]
        public string VolumeIn//SUMMARIZABLE
        {
            get
            {
                if (GetErrors(nameof(VolumeIn)) == null)
                {
                    return (string)_dataAccess.Get(nameof(VolumeIn));
                }
                else
                {
                    return _VolumeIn_Not_Valid;
                }
            }
            set
            {
                VolumeIn_Validation(value);
                if (GetErrors(nameof(VolumeIn)) == null)
                {
                    _dataAccess.Set(nameof(VolumeIn), value);
                }
                OnPropertyChanged(nameof(VolumeIn));
            }
        }

        private string _VolumeIn_Not_Valid = "";
        private void VolumeIn_Validation(string value)//TODO
        {
            ClearErrors(nameof(VolumeIn));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(VolumeIn), "Поле не заполнено");
                return;
            }
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(VolumeIn), "Недопустимое значение");
                return;
            }
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
                    AddError(nameof(VolumeIn), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(VolumeIn), "Недопустимое значение");
            }
        }
        //VolumeIn property

        //MassIn Property
        [Attributes.Form_Property("Масса, т")]
        public string MassIn//SUMMARIZABLE
        {
            get
            {
                if (GetErrors(nameof(MassIn)) == null)
                {
                    return (string)_dataAccess.Get(nameof(MassIn));
                }
                else
                {
                    return _MassIn_Not_Valid;
                }
            }
            set
            {
                MassIn_Validation(value);
                if (GetErrors(nameof(MassIn)) == null)
                {
                    _dataAccess.Set(nameof(MassIn), value);
                }
                OnPropertyChanged(nameof(MassIn));
            }
        }

        private string _MassIn_Not_Valid = "";
        private void MassIn_Validation(string value)//TODO
        {
            ClearErrors(nameof(MassIn));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(MassIn), "Поле не заполнено");
                return;
            }
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(MassIn), "Недопустимое значение");
                return;
            }
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
                    AddError(nameof(MassIn), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(MassIn), "Недопустимое значение");
            }
        }
        //MassIn Property

        //QuantityIn property
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public string QuantityIn//SUMMARIZABLE
        {
            get
            {
                if (GetErrors(nameof(QuantityIn)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityIn));//OK
                    return tmp != null ? (string)tmp : "";
                }
                else
                {
                    return _QuantityIn_Not_Valid;
                }
            }
            set
            {
                QuantityIn_Validation(value);

                if (GetErrors(nameof(QuantityIn)) == null)
                {
                    _dataAccess.Set(nameof(QuantityIn), value);
                }
                OnPropertyChanged(nameof(QuantityIn));
            }
        }
        // positive int.
        private string _QuantityIn_Not_Valid = "";
        private void QuantityIn_Validation(string value1)//Ready
        {
            ClearErrors(nameof(QuantityIn));
            if (string.IsNullOrEmpty(value1)) return;
            if (value1.Equals("-")) return;
            if (value1.Equals("прим."))
            {

            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            int value;
            try
            {
                value = int.Parse(tmp);
                if (value <= 0)
                    AddError(nameof(QuantityIn), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(QuantityIn), "Недопустимое значение");
            }
        }
        //QuantityIn property

        //TritiumActivityIn property
        [Attributes.Form_Property("Активность трития, Бк")]
        public string TritiumActivityIn//SUMMARIZABLE
        {
            get
            {
                if (GetErrors(nameof(TritiumActivityIn)) == null)
                {
                    return (string)_dataAccess.Get(nameof(TritiumActivityIn));
                }
                else
                {
                    return _TritiumActivityIn_Not_Valid;
                }
            }
            set
            {
                TritiumActivityIn_Validation(value);
                if (GetErrors(nameof(TritiumActivityIn)) == null)
                {
                    _dataAccess.Set(nameof(TritiumActivityIn), value);
                }
                OnPropertyChanged(nameof(TritiumActivityIn));
            }
        }

        private string _TritiumActivityIn_Not_Valid = "";
        private void TritiumActivityIn_Validation(string value)//TODO
        {
            ClearErrors(nameof(TritiumActivityIn));
            if (string.IsNullOrEmpty(value))
            {
                AddError(nameof(TritiumActivityIn), "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(TritiumActivityIn), "Недопустимое значение");
                return;
            }
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
        public string BetaGammaActivityIn//SUMMARIZABLE
        {
            get
            {
                if (GetErrors(nameof(BetaGammaActivityIn)) == null)
                {
                    return (string)_dataAccess.Get(nameof(BetaGammaActivityIn));
                }
                else
                {
                    return _BetaGammaActivityIn_Not_Valid;
                }
            }
            set
            {
                BetaGammaActivityIn_Validation(value);
                if (GetErrors(nameof(BetaGammaActivityIn)) == null)
                {
                    _dataAccess.Set(nameof(BetaGammaActivityIn), value);
                }
                OnPropertyChanged(nameof(BetaGammaActivityIn));
            }
        }

        private string _BetaGammaActivityIn_Not_Valid = "";
        private void BetaGammaActivityIn_Validation(string value)//TODO
        {
            ClearErrors(nameof(BetaGammaActivityIn));
            if (string.IsNullOrEmpty(value))
            {
                AddError(nameof(BetaGammaActivityIn), "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(BetaGammaActivityIn), "Недопустимое значение");
                return;
            }
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
        public string AlphaActivityIn//SUMMARIZABLE
        {
            get
            {
                if (GetErrors(nameof(AlphaActivityIn)) == null)
                {
                    return (string)_dataAccess.Get(nameof(AlphaActivityIn));
                }
                else
                {
                    return _AlphaActivityIn_Not_Valid;
                }
            }
            set
            {
                AlphaActivityIn_Validation(value);
                if (GetErrors(nameof(AlphaActivityIn)) == null)
                {
                    _dataAccess.Set(nameof(AlphaActivityIn), value);
                }
                OnPropertyChanged(nameof(AlphaActivityIn));
            }
        }

        private string _AlphaActivityIn_Not_Valid = "";
        private void AlphaActivityIn_Validation(string value)//TODO
        {
            ClearErrors(nameof(AlphaActivityIn));
            if (string.IsNullOrEmpty(value))
            {
                AddError(nameof(AlphaActivityIn), "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(AlphaActivityIn), "Недопустимое значение");
                return;
            }
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
        public string TransuraniumActivityIn//SUMMARIZABLE
        {
            get
            {
                if (GetErrors(nameof(TransuraniumActivityIn)) == null)
                {
                    return (string)_dataAccess.Get(nameof(TransuraniumActivityIn));
                }
                else
                {
                    return _TransuraniumActivityIn_Not_Valid;
                }
            }
            set
            {
                TransuraniumActivityIn_Validation(value);
                if (GetErrors(nameof(TransuraniumActivityIn)) == null)
                {
                    _dataAccess.Set(nameof(TransuraniumActivityIn), value);
                }
                OnPropertyChanged(nameof(TransuraniumActivityIn));
            }
        }

        private string _TransuraniumActivityIn_Not_Valid = "";
        private void TransuraniumActivityIn_Validation(string value)//TODO
        {
            ClearErrors(nameof(TransuraniumActivityIn));
            if (string.IsNullOrEmpty(value))
            {
                AddError(nameof(TransuraniumActivityIn), "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(TransuraniumActivityIn), "Недопустимое значение");
                return;
            }
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
                    return (string)_dataAccess.Get(nameof(CodeRAOout));
                }
                else
                {
                    return _CodeRAOout_Not_Valid;
                }
            }
            set
            {
                CodeRAOout_Validation(value);
                if (GetErrors(nameof(CodeRAOout)) == null)
                {
                    _dataAccess.Set(nameof(CodeRAOout), value);
                }
                OnPropertyChanged(nameof(CodeRAOout));
            }
        }

        private string _CodeRAOout_Not_Valid = "";
        private void CodeRAOout_Validation(string value)//TODO
        {
            ClearErrors(nameof(CodeRAOout));
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            var a = new Regex("^[0-9]{11}$");
            if (!a.IsMatch(value))
            {
                AddError(nameof(CodeRAOout), "Недопустимое значение");
                return;
            }
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
                    return (string)_dataAccess.Get(nameof(StatusRAOout));
                }
                else
                {
                    return _StatusRAOout_Not_Valid;
                }
            }
            set
            {
                StatusRAOout_Validation(value);
                if (GetErrors(nameof(StatusRAOout)) == null)
                {
                    _dataAccess.Set(nameof(StatusRAOout), value);
                }
                OnPropertyChanged(nameof(StatusRAOout));
            }
        }

        private string _StatusRAOout_Not_Valid = "";
        private void StatusRAOout_Validation(string value)//TODO
        {
            ClearErrors(nameof(StatusRAOout));
            if (string.IsNullOrEmpty(value)) return;
            if (value.Length == 1)
            {
                int tmp;
                try
                {
                    tmp = int.Parse(value);
                    if ((tmp < 1) || ((tmp > 4) && (tmp != 6) && (tmp != 9)))
                    {
                        AddError(nameof(StatusRAOout), "Недопустимое значение");
                    }
                }
                catch (Exception)
                {
                    AddError(nameof(StatusRAOout), "Недопустимое значение");
                }
                return;
            }
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(StatusRAOout), "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value))
                    AddError(nameof(StatusRAOout), "Недопустимое значение");
            }
        }
        //StatusRAOout property

        //VolumeOut property
        [Attributes.Form_Property("Объем, куб. м")]
        public string VolumeOut//SUMMARIZABLE
        {
            get
            {
                if (GetErrors(nameof(VolumeOut)) == null)
                {
                    return (string)_dataAccess.Get(nameof(VolumeOut));
                }
                else
                {
                    return _VolumeOut_Not_Valid;
                }
            }
            set
            {
                VolumeOut_Validation(value);
                if (GetErrors(nameof(VolumeOut)) == null)
                {
                    _dataAccess.Set(nameof(VolumeOut), value);
                }
                OnPropertyChanged(nameof(VolumeOut));
            }
        }

        private string _VolumeOut_Not_Valid = "";
        private void VolumeOut_Validation(string value)//TODO
        {
            ClearErrors(nameof(VolumeOut));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(VolumeOut), "Поле не заполнено");
                return;
            }
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(VolumeOut), "Недопустимое значение");
                return;
            }
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
                    AddError(nameof(VolumeOut), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(VolumeOut), "Недопустимое значение");
            }
        }
        //VolumeOut property

        //MassOut Property
        [Attributes.Form_Property("Масса, т")]
        public string MassOut//SUMMARIZABLE
        {
            get
            {
                if (GetErrors(nameof(MassOut)) == null)
                {
                    return (string)_dataAccess.Get(nameof(MassOut));
                }
                else
                {
                    return _MassOut_Not_Valid;
                }
            }
            set
            {
                MassOut_Validation(value);
                if (GetErrors(nameof(MassOut)) == null)
                {
                    _dataAccess.Set(nameof(MassOut), value);
                }
                OnPropertyChanged(nameof(MassOut));
            }
        }

        private string _MassOut_Not_Valid = "";
        private void MassOut_Validation(string value)//TODO
        {
            ClearErrors(nameof(MassOut));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(MassOut), "Поле не заполнено");
                return;
            }
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(MassOut), "Недопустимое значение");
                return;
            }
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
                    AddError(nameof(MassOut), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(MassOut), "Недопустимое значение");
            }
        }
        //MassOut Property

        //QuantityOZIIIout property
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public string QuantityOZIIIout//SUMMARIZABLE
        {
            get
            {
                if (GetErrors(nameof(QuantityOZIIIout)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(QuantityOZIIIout));//OK
                    return tmp != null ? (string)tmp : "";
                }
                else
                {
                    return _QuantityOZIIIout_Not_Valid;
                }
            }
            set
            {
                QuantityOZIIIout_Validation(value);

                if (GetErrors(nameof(QuantityOZIIIout)) == null)
                {
                    _dataAccess.Set(nameof(QuantityOZIIIout), value);
                }
                OnPropertyChanged(nameof(QuantityOZIIIout));
            }
        }
        // positive int.
        private string _QuantityOZIIIout_Not_Valid = "";
        private void QuantityOZIIIout_Validation(string value1)//Ready
        {
            ClearErrors(nameof(QuantityOZIIIout));
            if (string.IsNullOrEmpty(value1)) return;
            if (value1.Equals("-")) return;
            if (value1.Equals("прим."))
            {

            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            int value;
            try
            {
                value = int.Parse(tmp);
                if (value <= 0)
                    AddError(nameof(QuantityOZIIIout), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(QuantityOZIIIout), "Недопустимое значение");
            }
        }
        //QuantityOZIIIout property

        //TritiumActivityOut property
        [Attributes.Form_Property("Активность трития, Бк")]
        public string TritiumActivityOut//SUMMARIZABLE
        {
            get
            {
                if (GetErrors(nameof(TritiumActivityOut)) == null)
                {
                    return (string)_dataAccess.Get(nameof(TritiumActivityOut));
                }
                else
                {
                    return _TritiumActivityOut_Not_Valid;
                }
            }
            set
            {
                TritiumActivityOut_Validation(value);
                if (GetErrors(nameof(TritiumActivityOut)) == null)
                {
                    _dataAccess.Set(nameof(TritiumActivityOut), value);
                }
                OnPropertyChanged(nameof(TritiumActivityOut));
            }
        }

        private string _TritiumActivityOut_Not_Valid = "";
        private void TritiumActivityOut_Validation(string value)//TODO
        {
            ClearErrors(nameof(TritiumActivityOut));
            if (string.IsNullOrEmpty(value))
            {
                AddError(nameof(TritiumActivityOut), "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(TritiumActivityOut), "Недопустимое значение");
                return;
            }
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
        public string BetaGammaActivityOut//SUMMARIZABLE
        {
            get
            {
                if (GetErrors(nameof(BetaGammaActivityOut)) == null)
                {
                    return (string)_dataAccess.Get(nameof(BetaGammaActivityOut));
                }
                else
                {
                    return _BetaGammaActivityOut_Not_Valid;
                }
            }
            set
            {
                BetaGammaActivityOut_Validation(value);
                if (GetErrors(nameof(BetaGammaActivityOut)) == null)
                {
                    _dataAccess.Set(nameof(BetaGammaActivityOut), value);
                }
                OnPropertyChanged(nameof(BetaGammaActivityOut));
            }
        }

        private string _BetaGammaActivityOut_Not_Valid = "";
        private void BetaGammaActivityOut_Validation(string value)//TODO
        {
            ClearErrors(nameof(BetaGammaActivityOut));
            if (string.IsNullOrEmpty(value))
            {
                AddError(nameof(BetaGammaActivityOut), "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(BetaGammaActivityOut), "Недопустимое значение");
                return;
            }
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
        public string AlphaActivityOut//SUMMARIZABLE
        {
            get
            {
                if (GetErrors(nameof(AlphaActivityOut)) == null)
                {
                    return (string)_dataAccess.Get(nameof(AlphaActivityOut));
                }
                else
                {
                    return _AlphaActivityOut_Not_Valid;
                }
            }
            set
            {
                AlphaActivityOut_Validation(value);
                if (GetErrors(nameof(AlphaActivityOut)) == null)
                {
                    _dataAccess.Set(nameof(AlphaActivityOut), value);
                }
                OnPropertyChanged(nameof(AlphaActivityOut));
            }
        }

        private string _AlphaActivityOut_Not_Valid = "";
        private void AlphaActivityOut_Validation(string value)//TODO
        {
            ClearErrors(nameof(AlphaActivityOut));
            if (string.IsNullOrEmpty(value))
            {
                AddError(nameof(AlphaActivityOut), "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(AlphaActivityOut), "Недопустимое значение");
                return;
            }
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
        public string TransuraniumActivityOut//SUMMARIZABLE
        {
            get
            {
                if (GetErrors(nameof(TransuraniumActivityOut)) == null)
                {
                    return (string)_dataAccess.Get(nameof(TransuraniumActivityOut));
                }
                else
                {
                    return _TransuraniumActivityOut_Not_Valid;
                }
            }
            set
            {
                TransuraniumActivityOut_Validation(value);
                if (GetErrors(nameof(TransuraniumActivityOut)) == null)
                {
                    _dataAccess.Set(nameof(TransuraniumActivityOut), value);
                }
                OnPropertyChanged(nameof(TransuraniumActivityOut));
            }
        }

        private string _TransuraniumActivityOut_Not_Valid = "";
        private void TransuraniumActivityOut_Validation(string value)//TODO
        {
            ClearErrors(nameof(TransuraniumActivityOut));
            if (string.IsNullOrEmpty(value))
            {
                AddError(nameof(TransuraniumActivityOut), "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(TransuraniumActivityOut), "Недопустимое значение");
                return;
            }
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
