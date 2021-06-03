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
        public IDataAccess<string> RefineMachineName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RefineMachineName));
                }
                else
                {
                    
                }
            }
            set
            {
                RefineMachineName_Validation(value);
                
                {
                    _dataAccess.Set(nameof(RefineMachineName), value);
                }
                OnPropertyChanged(nameof(RefineMachineName));
            }
        }

                private void RefineMachineName_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //RefineMachineName property

        //MachineCode property
        [Attributes.Form_Property("Код установки переработки")]
        public byte? MachineCode
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MachineCode));
                }
                else
                {
                    
                }
            }
            set
            {
                MachineCode_Validation(value);
                
                {
                    _dataAccess.Set(nameof(MachineCode), value);
                }
                OnPropertyChanged(nameof(MachineCode));
            }
        }

                private void MachineCode_Validation(byte? value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null) return;
            bool a = (value >= 11) && (value <= 17);
            bool b = (value >= 21) && (value <= 24);
            bool c = (value >= 31) && (value <= 32);
            bool d = (value >= 41) && (value <= 43);
            bool e = (value >= 51) && (value <= 56);
            bool f = (value >= 61) && (value <= 63);
            bool g = (value >= 71) && (value <= 73);
            bool h = (value.Value == 19) || (value.Value == 29) || (value.Value == 39) || (value.Value == 49) || (value.Value == 99) || (value.Value == 79);
            if (!(a || b || c || d || e || f || g || h))
                value.AddError( "Недопустимое значение");
        }
        //MachineCode property

        //MachinePower property
        [Attributes.Form_Property("Мощность, куб. м/год")]
        public IDataAccess<string> MachinePower
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MachinePower));
                }
                else
                {
                    
                }
            }
            set
            {
                MachinePower_Validation(value);
                
                {
                    _dataAccess.Set(nameof(MachinePower), value);
                }
                OnPropertyChanged(nameof(MachinePower));
            }
        }

                private void MachinePower_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //MachinePower property

        //NumberOfHoursPerYear property
        [Attributes.Form_Property("Количество часов работы за год")]
        public IDataAccess<string> NumberOfHoursPerYear
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(NumberOfHoursPerYear));
                }
                else
                {
                    
                }
            }
            set
            {
                NumberOfHoursPerYear_Validation(value);
                
                {
                    _dataAccess.Set(nameof(NumberOfHoursPerYear), value);
                }
                OnPropertyChanged(nameof(NumberOfHoursPerYear));
            }
        }

                private void NumberOfHoursPerYear_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //NumberOfHoursPerYear property

        //CodeRAOIn property
        [Attributes.Form_Property("Код РАО")]
        public IDataAccess<string> CodeRAOIn
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CodeRAOIn));
                }
                else
                {
                    
                }
            }
            set
            {
                CodeRAOIn_Validation(value);
                
                {
                    _dataAccess.Set(nameof(CodeRAOIn), value);
                }
                OnPropertyChanged(nameof(CodeRAOIn));
            }
        }

                private void CodeRAOIn_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            var a = new Regex("^[0-9]{11}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //CodeRAOIn property

        //StatusRAOIn property
        [Attributes.Form_Property("Статус РАО")]
        public IDataAccess<string> StatusRAOIn  //1 cyfer or OKPO.
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(StatusRAOIn));
                }
                else
                {
                    
                }
            }
            set
            {
                StatusRAOIn_Validation(value);
                
                {
                    _dataAccess.Set(nameof(StatusRAOIn), value);
                }
                OnPropertyChanged(nameof(StatusRAOIn));
            }
        }

                private void StatusRAOIn_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value)) return;
            if (value.Length == 1)
            {
                int tmp;
                try
                {
                    tmp = int.Parse(value.Value);
                    if ((tmp < 1) || ((tmp > 4) && (tmp != 6) && (tmp != 9)))
                    {
                        value.AddError( "Недопустимое значение");
                    }
                }
                catch (Exception)
                {
                    value.AddError( "Недопустимое значение");
                }
                return;
            }
            if ((value.Length != 8) && (value.Length != 14))
                value.AddError( "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //StatusRAOIn property

        //VolumeIn property
        [Attributes.Form_Property("Объем, куб. м")]
        public IDataAccess<string> VolumeIn//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(VolumeIn));
                }
                else
                {
                    
                }
            }
            set
            {
                VolumeIn_Validation(value);
                
                {
                    _dataAccess.Set(nameof(VolumeIn), value);
                }
                OnPropertyChanged(nameof(VolumeIn));
            }
        }

                private void VolumeIn_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //VolumeIn property

        //MassIn Property
        [Attributes.Form_Property("Масса, т")]
        public IDataAccess<string> MassIn//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassIn));
                }
                else
                {
                    
                }
            }
            set
            {
                MassIn_Validation(value);
                
                {
                    _dataAccess.Set(nameof(MassIn), value);
                }
                OnPropertyChanged(nameof(MassIn));
            }
        }

                private void MassIn_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //MassIn Property

        //QuantityIn property
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public IDataAccess<string> QuantityIn//SUMMARIZABLE
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityIn));//OK
                    return tmp != null ? (string)tmp : "";
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityIn_Validation(value);

                
                {
                    _dataAccess.Set(nameof(QuantityIn), value);
                }
                OnPropertyChanged(nameof(QuantityIn));
            }
        }
        // positive int.
                private void QuantityIn_Validation(IDataAccess<string> value1)//Ready
        {
            value.ClearErrors();
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //QuantityIn property

        //TritiumActivityIn property
        [Attributes.Form_Property("Активность трития, Бк")]
        public IDataAccess<string> TritiumActivityIn//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(TritiumActivityIn));
                }
                else
                {
                    
                }
            }
            set
            {
                TritiumActivityIn_Validation(value);
                
                {
                    _dataAccess.Set(nameof(TritiumActivityIn), value);
                }
                OnPropertyChanged(nameof(TritiumActivityIn));
            }
        }

                private void TritiumActivityIn_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //TritiumActivityIn property

        //BetaGammaActivityIn property
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public IDataAccess<string> BetaGammaActivityIn//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(BetaGammaActivityIn));
                }
                else
                {
                    
                }
            }
            set
            {
                BetaGammaActivityIn_Validation(value);
                
                {
                    _dataAccess.Set(nameof(BetaGammaActivityIn), value);
                }
                OnPropertyChanged(nameof(BetaGammaActivityIn));
            }
        }

                private void BetaGammaActivityIn_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //BetaGammaActivity property

        //AlphaActivityIn property
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public IDataAccess<string> AlphaActivityIn//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(AlphaActivityIn));
                }
                else
                {
                    
                }
            }
            set
            {
                AlphaActivityIn_Validation(value);
                
                {
                    _dataAccess.Set(nameof(AlphaActivityIn), value);
                }
                OnPropertyChanged(nameof(AlphaActivityIn));
            }
        }

                private void AlphaActivityIn_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //AlphaActivityIn property

        //TransuraniumActivityIn property
        [Attributes.Form_Property("Активность трансурановых, Бк")]
        public IDataAccess<string> TransuraniumActivityIn//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(TransuraniumActivityIn));
                }
                else
                {
                    
                }
            }
            set
            {
                TransuraniumActivityIn_Validation(value);
                
                {
                    _dataAccess.Set(nameof(TransuraniumActivityIn), value);
                }
                OnPropertyChanged(nameof(TransuraniumActivityIn));
            }
        }

                private void TransuraniumActivityIn_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //TransuraniumActivityIn property

        //CodeRAOout property
        [Attributes.Form_Property("Код РАО")]
        public IDataAccess<string> CodeRAOout
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CodeRAOout));
                }
                else
                {
                    
                }
            }
            set
            {
                CodeRAOout_Validation(value);
                
                {
                    _dataAccess.Set(nameof(CodeRAOout), value);
                }
                OnPropertyChanged(nameof(CodeRAOout));
            }
        }

                private void CodeRAOout_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            var a = new Regex("^[0-9]{11}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //CodeRAOout property

        //StatusRAOout property
        [Attributes.Form_Property("Статус РАО")]
        public IDataAccess<string> StatusRAOout  //1 cyfer or OKPO.
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(StatusRAOout));
                }
                else
                {
                    
                }
            }
            set
            {
                StatusRAOout_Validation(value);
                
                {
                    _dataAccess.Set(nameof(StatusRAOout), value);
                }
                OnPropertyChanged(nameof(StatusRAOout));
            }
        }

                private void StatusRAOout_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value)) return;
            if (value.Length == 1)
            {
                int tmp;
                try
                {
                    tmp = int.Parse(value.Value);
                    if ((tmp < 1) || ((tmp > 4) && (tmp != 6) && (tmp != 9)))
                    {
                        value.AddError( "Недопустимое значение");
                    }
                }
                catch (Exception)
                {
                    value.AddError( "Недопустимое значение");
                }
                return;
            }
            if ((value.Length != 8) && (value.Length != 14))
                value.AddError( "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //StatusRAOout property

        //VolumeOut property
        [Attributes.Form_Property("Объем, куб. м")]
        public IDataAccess<string> VolumeOut//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(VolumeOut));
                }
                else
                {
                    
                }
            }
            set
            {
                VolumeOut_Validation(value);
                
                {
                    _dataAccess.Set(nameof(VolumeOut), value);
                }
                OnPropertyChanged(nameof(VolumeOut));
            }
        }

                private void VolumeOut_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //VolumeOut property

        //MassOut Property
        [Attributes.Form_Property("Масса, т")]
        public IDataAccess<string> MassOut//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassOut));
                }
                else
                {
                    
                }
            }
            set
            {
                MassOut_Validation(value);
                
                {
                    _dataAccess.Set(nameof(MassOut), value);
                }
                OnPropertyChanged(nameof(MassOut));
            }
        }

                private void MassOut_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //MassOut Property

        //QuantityOZIIIout property
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public IDataAccess<string> QuantityOZIIIout//SUMMARIZABLE
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityOZIIIout));//OK
                    return tmp != null ? (string)tmp : "";
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityOZIIIout_Validation(value);

                
                {
                    _dataAccess.Set(nameof(QuantityOZIIIout), value);
                }
                OnPropertyChanged(nameof(QuantityOZIIIout));
            }
        }
        // positive int.
                private void QuantityOZIIIout_Validation(IDataAccess<string> value1)//Ready
        {
            value.ClearErrors();
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //QuantityOZIIIout property

        //TritiumActivityOut property
        [Attributes.Form_Property("Активность трития, Бк")]
        public IDataAccess<string> TritiumActivityOut//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(TritiumActivityOut));
                }
                else
                {
                    
                }
            }
            set
            {
                TritiumActivityOut_Validation(value);
                
                {
                    _dataAccess.Set(nameof(TritiumActivityOut), value);
                }
                OnPropertyChanged(nameof(TritiumActivityOut));
            }
        }

                private void TritiumActivityOut_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //TritiumActivityOut property

        //BetaGammaActivityOut property
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public IDataAccess<string> BetaGammaActivityOut//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(BetaGammaActivityOut));
                }
                else
                {
                    
                }
            }
            set
            {
                BetaGammaActivityOut_Validation(value);
                
                {
                    _dataAccess.Set(nameof(BetaGammaActivityOut), value);
                }
                OnPropertyChanged(nameof(BetaGammaActivityOut));
            }
        }

                private void BetaGammaActivityOut_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //BetaGammaActivityOut property

        //AlphaActivityOut property
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public IDataAccess<string> AlphaActivityOut//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(AlphaActivityOut));
                }
                else
                {
                    
                }
            }
            set
            {
                AlphaActivityOut_Validation(value);
                
                {
                    _dataAccess.Set(nameof(AlphaActivityOut), value);
                }
                OnPropertyChanged(nameof(AlphaActivityOut));
            }
        }

                private void AlphaActivityOut_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //AlphaActivityOut property

        //TransuraniumActivityOut property
        [Attributes.Form_Property("Активность трансурановых, Бк")]
        public IDataAccess<string> TransuraniumActivityOut//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(TransuraniumActivityOut));
                }
                else
                {
                    
                }
            }
            set
            {
                TransuraniumActivityOut_Validation(value);
                
                {
                    _dataAccess.Set(nameof(TransuraniumActivityOut), value);
                }
                OnPropertyChanged(nameof(TransuraniumActivityOut));
            }
        }

                private void TransuraniumActivityOut_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //TransuraniumActivityOut property
    }
}
