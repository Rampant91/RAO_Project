using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Linq;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.1: Сортировка, переработка и кондиционирование РАО на установках")]
    public class Form21 : Abstracts.Form2
    {
        public Form21() : base()
        {
            FormNum.Value = "2.1";
            //NumberOfFields.Value = 24;
            Validate_all();
        }

        private void Validate_all()
        {
            MachinePower_Validation(MachinePower);
            MachineCode_Validation(MachineCode);
            RefineMachineName_Validation(RefineMachineName);
            NumberOfHoursPerYear_Validation(NumberOfHoursPerYear);
            CodeRAOIn_Validation(CodeRAOIn);
            StatusRAOIn_Validation(StatusRAOIn);
            VolumeIn_Validation(VolumeIn);
            MassIn_Validation(MassIn);
            QuantityIn_Validation(QuantityIn);
            TritiumActivityIn_Validation(TritiumActivityIn);
            TritiumActivityOut_Validation(TritiumActivityOut);
            BetaGammaActivityIn_Validation(BetaGammaActivityIn);
            BetaGammaActivityOut_Validation(BetaGammaActivityOut);
            TransuraniumActivityIn_Validation(TransuraniumActivityIn);
            TransuraniumActivityOut_Validation(TransuraniumActivityOut);
            AlphaActivityIn_Validation(AlphaActivityIn);
            AlphaActivityOut_Validation(AlphaActivityOut);
            VolumeOut_Validation(VolumeOut);
            MassOut_Validation(MassOut);
            QuantityOZIIIout_Validation(QuantityOZIIIout);
            CodeRAOout_Validation(CodeRAOout);
            StatusRAOout_Validation(StatusRAOout);
        }
        public override bool Object_Validation()
        {
            return !(MachinePower.HasErrors ||
            MachineCode.HasErrors ||
            RefineMachineName.HasErrors ||
            NumberOfHoursPerYear.HasErrors ||
            CodeRAOIn.HasErrors ||
            StatusRAOIn.HasErrors ||
            VolumeIn.HasErrors ||
            MassIn.HasErrors ||
            QuantityIn.HasErrors ||
            TritiumActivityIn.HasErrors ||
            TritiumActivityOut.HasErrors ||
            BetaGammaActivityIn.HasErrors ||
            BetaGammaActivityOut.HasErrors ||
            TransuraniumActivityIn.HasErrors ||
            TransuraniumActivityOut.HasErrors ||
            AlphaActivityIn.HasErrors ||
            AlphaActivityOut.HasErrors ||
            VolumeOut.HasErrors ||
            MassOut.HasErrors ||
            QuantityOZIIIout.HasErrors ||
            CodeRAOout.HasErrors ||
            StatusRAOout.HasErrors);
        }

        #region  Sum
        public bool Sum_DB { get; set; } = false;

        [NotMapped]
        public RamAccess<bool> Sum
        {
            get
            {
                var tmp = new RamAccess<bool>(Sum_Validation, Sum_DB);
                tmp.PropertyChanged += SumValueChanged;
                return tmp;
            }
            set
            {
                Sum_DB = value.Value;
                OnPropertyChanged(nameof(Sum));
            }
        }

        private void SumValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Sum_DB = ((RamAccess<bool>)Value).Value;
            }
        }

        private bool Sum_Validation(RamAccess<bool> value)
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        //RefineMachineName property
        #region  RefineMachineName
        public string RefineMachineName_DB { get; set; } = "";
        public bool RefineMachineName_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool RefineMachineName_Hidden
        {
            get => RefineMachineName_Hidden_Priv;
            set
            {
                RefineMachineName_Hidden_Priv = value;
            }
        }

        [NotMapped]
        [Attributes.Form_Property("наименование")]
        public RamAccess<string> RefineMachineName
        {
            get
            {
                if (!RefineMachineName_Hidden)
                {
                    var tmp = new RamAccess<string>(RefineMachineName_Validation, RefineMachineName_DB);
                    tmp.PropertyChanged += RefineMachineNameValueChanged;
                    return tmp;
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!RefineMachineName_Hidden)
                {
                    RefineMachineName_DB = value.Value;
                    OnPropertyChanged(nameof(RefineMachineName));
                }
            }
        }

        private void RefineMachineNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                RefineMachineName_DB = ((RamAccess<string>)Value).Value;
                OnPropertyChanged(nameof(RefineMachineName));
            }
        }

        private bool RefineMachineName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        //RefineMachineName property
        #endregion

        //MachineCode property
        #region MachineCode 
        public byte? MachineCode_DB { get; set; } = null;
        public bool MachineCode_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool MachineCode_Hidden
        {
            get => MachineCode_Hidden_Priv;
            set
            {
                MachineCode_Hidden_Priv = value;
            }
        }

        [NotMapped]
        [Attributes.Form_Property("код")]
        public RamAccess<byte?> MachineCode
        {
            get
            {
                if (!MachineCode_Hidden)
                {
                    var tmp = new RamAccess<byte?>(MachineCode_Validation, MachineCode_DB);
                    tmp.PropertyChanged += MachineCodeValueChanged;
                    return tmp;
                }
                else
                {
                    var tmp = new RamAccess<byte?>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!MachineCode_Hidden)
                {
                    MachineCode_DB = value.Value;
                    OnPropertyChanged(nameof(MachineCode));
                }
            }
        }

        private void MachineCodeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                MachineCode_DB = ((RamAccess<byte?>)Value).Value;
                OnPropertyChanged(nameof(MachineCode));
            }
        }

        private bool MachineCode_Validation(RamAccess<byte?> value) //TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }

            bool a = (value.Value >= 11) && (value.Value <= 17);
            bool b = (value.Value >= 21) && (value.Value <= 24);
            bool c = (value.Value >= 31) && (value.Value <= 32);
            bool d = (value.Value >= 41) && (value.Value <= 43);
            bool e = (value.Value >= 51) && (value.Value <= 56);
            bool f = (value.Value >= 61) && (value.Value <= 63);
            bool g = (value.Value >= 71) && (value.Value <= 73);
            bool h = (value.Value == 19) || (value.Value == 29) || (value.Value == 39) || (value.Value == 49) ||
                     (value.Value == 99) || (value.Value == 79);
            if (!(a || b || c || d || e || f || g || h))
            {
                value.AddError("Недопустимое значение");
                return false;
            }

            return true;
        }

        //MachineCode property
        #endregion

        //MachinePower property
        #region  MachinePower
        public string MachinePower_DB { get; set; } = "";
        public bool MachinePower_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool MachinePower_Hidden
        {
            get => MachinePower_Hidden_Priv;
            set
            {
                MachinePower_Hidden_Priv = value;
            }
        }

        [NotMapped]
        [Attributes.Form_Property("мощность, куб. м/год")]
        public RamAccess<string> MachinePower
        {
            get
            {
                if (!MachinePower_Hidden)
                {
                    var tmp = new RamAccess<string>(MachinePower_Validation, MachinePower_DB);
                    tmp.PropertyChanged += MachinePowerValueChanged;
                    return tmp;
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!MachinePower_Hidden)
                {
                    MachinePower_DB = value.Value;
                    OnPropertyChanged(nameof(MachinePower));
                }
            }
        }

        private void MachinePowerValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    MachinePower_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                MachinePower_DB = value1;
                OnPropertyChanged(nameof(MachinePower)); //Why here
            }
        }

        private bool MachinePower_Validation(RamAccess<string> value) //TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }

            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                                  NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                {
                    value.AddError("Число должно быть больше нуля");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }

            return true;
        }

        //MachinePower property
        #endregion

        //NumberOfHoursPerYear property
        #region  NumberOfHoursPerYear
        public string NumberOfHoursPerYear_DB { get; set; } = "";
        public bool NumberOfHoursPerYear_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool NumberOfHoursPerYear_Hidden
        {
            get => NumberOfHoursPerYear_Hidden_Priv;
            set
            {
                NumberOfHoursPerYear_Hidden_Priv = value;
            }
        }

        [NotMapped]
        [Attributes.Form_Property("количество часов работы за год")]
        public RamAccess<string> NumberOfHoursPerYear
        {
            get
            {
                if (!NumberOfHoursPerYear_Hidden)
                {
                    var tmp = new RamAccess<string>(NumberOfHoursPerYear_Validation, NumberOfHoursPerYear_DB);
                    tmp.PropertyChanged += NumberOfHoursPerYearValueChanged;
                    return tmp;
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!NumberOfHoursPerYear_Hidden)
                {
                    NumberOfHoursPerYear_DB = value.Value;
                    OnPropertyChanged(nameof(NumberOfHoursPerYear));
                }
            }
        }

        private void NumberOfHoursPerYearValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    NumberOfHoursPerYear_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                NumberOfHoursPerYear_DB = value1;
                OnPropertyChanged(nameof(NumberOfHoursPerYear)); //Why here
            }
        }

        private bool NumberOfHoursPerYear_Validation(RamAccess<string> value) //TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return false;
            }
            if (value.Value.Equals("прим.") || value.Value.Equals("0"))
            {
                return true;
                //TODO
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                {
                    value.AddError("Число должно быть больше нуля");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }

            return true;
        }

        //NumberOfHoursPerYear property
        #endregion

        //CodeRAOIn property
        #region  CodeRAOIn
        public string CodeRAOIn_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property("код РАО")]
        public RamAccess<string> CodeRAOIn
        {
            get
            {
                var tmp = new RamAccess<string>(CodeRAOIn_Validation, CodeRAOIn_DB);
                tmp.PropertyChanged += CodeRAOInValueChanged;
                return tmp;
            } set
            {
                CodeRAOIn_DB = value.Value;
                OnPropertyChanged(nameof(CodeRAOIn));
            }
        }

        private void CodeRAOInValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value.ToLower();
                tmp = tmp.Replace("х", "x");
                CodeRAOIn_DB = tmp;
            }
        }
        private bool CodeRAOIn_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return false;
            }
            var tmp = value.Value.ToLower();
            tmp = tmp.Replace("х", "x");
            Regex a = new Regex("^[0-9x+]{11}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //CodeRAOIn property
        #endregion

        //StatusRAOIn property
        #region  StatusRAOIn
        public string StatusRAOIn_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property("статус РАО")]
        public RamAccess<string> StatusRAOIn  //1 cyfer or OKPO.
        {
            get
            {
                var tmp = new RamAccess<string>(StatusRAOIn_Validation, StatusRAOIn_DB);
                tmp.PropertyChanged += StatusRAOInValueChanged;
                return tmp;
            } set
            {
                StatusRAOIn_DB = value.Value;
                OnPropertyChanged(nameof(StatusRAOIn));
            }
        }

        private void StatusRAOInValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                StatusRAOIn_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool StatusRAOIn_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value == "-")
            {
                return true;
            }
            if (value.Value.Length == 1)
            {
                int tmp;
                try
                {
                    tmp = int.Parse(value.Value);
                    if ((tmp < 1) || ((tmp > 4) && (tmp != 6) && (tmp != 9)))
                    {
                        value.AddError("Недопустимое значение");
                        return false;
                    }
                }
                catch (Exception)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
                return true;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //StatusRAOIn property
        #endregion

        //VolumeIn property
        #region  VolumeIn
        public string VolumeIn_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property("куб. м")]
        public RamAccess<string> VolumeIn//SUMMARIZABLE
        {
            get
            {
                var tmp = new RamAccess<string>(VolumeIn_Validation, VolumeIn_DB);
                tmp.PropertyChanged += VolumeInValueChanged;
                return tmp;
            }
            set
            {
                VolumeIn_DB = value.Value;
                OnPropertyChanged(nameof(VolumeIn));
            }
        }

        private void VolumeInValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    VolumeIn_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                VolumeIn_DB = value1;
            }
        }

        private bool VolumeIn_Validation(RamAccess<string> value) //TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }

            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                {
                    value.AddError("Число должно быть больше нуля");
                    return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }

            return true;
        }

        //VolumeIn property
        #endregion

        //MassIn Property
        #region  MassIn
        public string MassIn_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property("т")]
        public RamAccess<string> MassIn//SUMMARIZABLE
        {
            get
            {
                var tmp = new RamAccess<string>(MassIn_Validation, MassIn_DB);
                tmp.PropertyChanged += MassInValueChanged;
                return tmp;
            } set
            {
                MassIn_DB = value.Value;
                OnPropertyChanged(nameof(MassIn));
            }
        }

        private void MassInValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    MassIn_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                MassIn_DB = value1;
            }
        }
        private bool MassIn_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //MassIn property
        #endregion

        //QuantityIn property
        #region  QuantityIn
        public string QuantityIn_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property("ОЗИИИ, шт.")]
        public RamAccess<string> QuantityIn//SUMMARIZABLE
        {
            get
            {
                var tmp = new RamAccess<string>(QuantityIn_Validation, QuantityIn_DB);//OK
                tmp.PropertyChanged += QuantityInValueChanged;
                return tmp;
            }
            set
            {
                QuantityIn_DB = value.Value;
                OnPropertyChanged(nameof(QuantityIn));
            }
        }
        // positive int.
        private void QuantityInValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                QuantityIn_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool QuantityIn_Validation(RamAccess<string> value1)//Ready
        {
            value1.ClearErrors();
            if (string.IsNullOrEmpty(value1.Value))
            {
                return true;
            }
            if (value1.Value.Equals("прим.")||value1.Value.Equals("-"))
            {
                return true;
            }
            string tmp = value1.Value;
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
                {
                    value1.AddError("Число должно быть больше нуля"); return false;
                }
            }
            catch
            {
                value1.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //QuantityIn property
        #endregion

        //TritiumActivityIn property
        #region  TritiumActivityIn
        public string TritiumActivityIn_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("тритий")]
        public RamAccess<string> TritiumActivityIn//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(TritiumActivityIn_Validation, TritiumActivityIn_DB);
                    tmp.PropertyChanged += TritiumActivityInValueChanged;
                    return tmp;
            }
            set
            {
                    TritiumActivityIn_DB = value.Value;
                OnPropertyChanged(nameof(TritiumActivityIn));
            }
        }

        private void TritiumActivityInValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    TritiumActivityIn_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                TritiumActivityIn_DB = value1;
            }
        }
        private bool TritiumActivityIn_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            if (value.Value == "-")
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //TritiumActivityIn property
        #endregion

        //BetaGammaActivityIn property
        #region  BetaGammaActivityIn
        public string BetaGammaActivityIn_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("бета-, гамма-излучающие радионуклиды (исключая тритий)")]
        public RamAccess<string> BetaGammaActivityIn//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(BetaGammaActivityIn_Validation, BetaGammaActivityIn_DB);
                    tmp.PropertyChanged += BetaGammaActivityInValueChanged;
                    return tmp;
            }
            set
            {
                    BetaGammaActivityIn_DB = value.Value;
                OnPropertyChanged(nameof(BetaGammaActivityIn));
            }
        }

        private void BetaGammaActivityInValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    BetaGammaActivityIn_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                BetaGammaActivityIn_DB = value1;
            }
        }
        private bool BetaGammaActivityIn_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            if (value.Value == "-")
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //BetaGammaActivity property
        #endregion

        //AlphaActivityIn property
        #region  AlphaActivityIn
        public string AlphaActivityIn_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("альфа-излучающие радионуклиды (исключая трансурановые)")]
        public RamAccess<string> AlphaActivityIn//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(AlphaActivityIn_Validation, AlphaActivityIn_DB);
                    tmp.PropertyChanged += AlphaActivityInValueChanged;
                    return tmp;
            }
            set
            {
                    AlphaActivityIn_DB = value.Value;
                OnPropertyChanged(nameof(AlphaActivityIn));
            }
        }

        private void AlphaActivityInValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    AlphaActivityIn_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                AlphaActivityIn_DB = value1;
            }
        }
        private bool AlphaActivityIn_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (value.Value == "-")
            {
                return true;
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //AlphaActivityIn property
        #endregion

        //TransuraniumActivityIn property
        #region  TransuraniumActivityIn
        public string TransuraniumActivityIn_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("трансурановые радионуклиды")]
        public RamAccess<string> TransuraniumActivityIn//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(TransuraniumActivityIn_Validation, TransuraniumActivityIn_DB);
                    tmp.PropertyChanged += TransuraniumActivityInValueChanged;
                    return tmp;
            }
            set
            {
                    TransuraniumActivityIn_DB = value.Value;
                OnPropertyChanged(nameof(TransuraniumActivityIn));
            }
        }

        private void TransuraniumActivityInValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    TransuraniumActivityIn_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                TransuraniumActivityIn_DB = value1;
            }
        }
        private bool TransuraniumActivityIn_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            if (value.Value == "-")
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //TransuraniumActivityIn property
        #endregion

        //CodeRAOout property
        #region  CodeRAOout
        public string CodeRAOout_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("код РАО")]
        public RamAccess<string> CodeRAOout
        {
            get
            {
                    var tmp = new RamAccess<string>(CodeRAOout_Validation, CodeRAOout_DB);
                    tmp.PropertyChanged += CodeRAOoutValueChanged;
                    return tmp;
            }
            set
            {
                    CodeRAOout_DB = value.Value;
                OnPropertyChanged(nameof(CodeRAOout));
            }
        }

        private void CodeRAOoutValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value.ToLower();
                tmp = tmp.Replace("х", "x");
                CodeRAOout_DB = tmp;
            }
        }
        private bool CodeRAOout_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return false;
            }
            var tmp = value.Value.ToLower();
            tmp = tmp.Replace("х", "x");
            Regex a = new Regex("^[0-9x+]{11}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //CodeRAOout property
        #endregion

        //StatusRAOout property
        #region  StatusRAOout
        public string StatusRAOout_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("статус РАО")]
        public RamAccess<string> StatusRAOout  //1 cyfer or OKPO.
        {
            get
            {
                    var tmp = new RamAccess<string>(StatusRAOout_Validation, StatusRAOout_DB);
                    tmp.PropertyChanged += StatusRAOoutValueChanged;
                    return tmp;
            }
            set
            {
                    StatusRAOout_DB = value.Value;
                OnPropertyChanged(nameof(StatusRAOout));
            }
        }

        private void StatusRAOoutValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                StatusRAOout_DB = ((RamAccess<string>)Value).Value;
}
}
private bool StatusRAOout_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            if (value.Value.Length == 1)
            {
                int tmp;
                try
                {
                    tmp = int.Parse(value.Value);
                    if ((tmp < 1) || ((tmp > 4) && (tmp != 6) && (tmp != 9)))
                    {
                        value.AddError("Недопустимое значение");
                        return false;
                    }
                }
                catch (Exception)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
                return true;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //StatusRAOout property
        #endregion

        //VolumeOut property
        #region  VolumeOut
        public string VolumeOut_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("куб. м")]
        public RamAccess<string> VolumeOut//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(VolumeOut_Validation, VolumeOut_DB);
                    tmp.PropertyChanged += VolumeOutValueChanged;
                    return tmp;
            }
            set
            {
                    VolumeOut_DB = value.Value;
                OnPropertyChanged(nameof(VolumeOut));
            }
        }

        private void VolumeOutValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    VolumeOut_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                VolumeOut_DB = value1;
            }
        }
        private bool VolumeOut_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //VolumeOut property
        #endregion

        //MassOut Property
        #region  MassOut
        public string MassOut_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("т")]
        public RamAccess<string> MassOut//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(MassOut_Validation, MassOut_DB);
                    tmp.PropertyChanged += MassOutValueChanged;
                    return tmp;
            }
            set
            {
                    MassOut_DB = value.Value;
                OnPropertyChanged(nameof(MassOut));
            }
        }

        private void MassOutValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    MassOut_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                MassOut_DB = value1;
            }
        }
        private bool MassOut_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //MassOut Property
        #endregion

        //QuantityOZIIIout property
        #region  QuantityOZIIIout
        public string QuantityOZIIIout_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("ОЗИИИ, шт.")]
        public RamAccess<string> QuantityOZIIIout//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(QuantityOZIIIout_Validation, QuantityOZIIIout_DB);//OK
                    tmp.PropertyChanged += QuantityOZIIIoutValueChanged;
                    return tmp;
            }
            set
            {
                    QuantityOZIIIout_DB = value.Value;
                OnPropertyChanged(nameof(QuantityOZIIIout));
            }
        }
        // positive int.
        private void QuantityOZIIIoutValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                QuantityOZIIIout_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool QuantityOZIIIout_Validation(RamAccess<string> value1)//Ready
        {
            value1.ClearErrors();
            if (string.IsNullOrEmpty(value1.Value))
            {
                value1.AddError("Поле не заполнено");
                return false;
            }
            if (value1.Value == "-")
            {
                return true;
            }
            if (value1.Equals("прим."))
            {
                return true;
            }
            string tmp = value1.Value;
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
                {
                    value1.AddError("Число должно быть больше нуля"); return false;
                }
            }
            catch
            {
                value1.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //QuantityOZIIIout property
        #endregion

        //TritiumActivityOut property
        #region  TritiumActivityOut
        public string TritiumActivityOut_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("тритий")]
        public RamAccess<string> TritiumActivityOut//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(TritiumActivityOut_Validation, TritiumActivityOut_DB);
                    tmp.PropertyChanged += TritiumActivityOutValueChanged;
                    return tmp;
            }
            set
            {
                    TritiumActivityOut_DB = value.Value;
                OnPropertyChanged(nameof(TritiumActivityOut));
            }
        }

        private void TritiumActivityOutValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    TritiumActivityOut_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                TritiumActivityOut_DB = value1;
            }
        }
        private bool TritiumActivityOut_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value == "-")
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //TritiumActivityOut property
        #endregion

        //BetaGammaActivityOut property
        #region  BetaGammaActivityOut
        public string BetaGammaActivityOut_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("бета-, гамма-излучающие (исключая тритий)")]
        public RamAccess<string> BetaGammaActivityOut//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(BetaGammaActivityOut_Validation, BetaGammaActivityOut_DB);
                    tmp.PropertyChanged += BetaGammaActivityOutValueChanged;
                    return tmp;
            }
            set
            {
                    BetaGammaActivityOut_DB = value.Value;
                OnPropertyChanged(nameof(BetaGammaActivityOut));
            }
        }
        private void BetaGammaActivityOutValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    BetaGammaActivityOut_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                BetaGammaActivityOut_DB = value1;
            }
        }
        private bool BetaGammaActivityOut_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value == "-")
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //BetaGammaActivityOut property
        #endregion

        //AlphaActivityOut property
        #region  AlphaActivityOut
        public string AlphaActivityOut_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("альфа-излучающие радионуклиды (исключая трансурановые)")]
        public RamAccess<string> AlphaActivityOut//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(AlphaActivityOut_Validation, AlphaActivityOut_DB);
                    tmp.PropertyChanged += AlphaActivityOutValueChanged;
                    return tmp;
            }
            set
            {
                    AlphaActivityOut_DB = value.Value;
                OnPropertyChanged(nameof(AlphaActivityOut));
            }
        }

        private void AlphaActivityOutValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    AlphaActivityOut_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                AlphaActivityOut_DB = value1;
            }
        }
        private bool AlphaActivityOut_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value == "-")
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //AlphaActivityOut property
        #endregion

        //TransuraniumActivityOut property
        #region  TransuraniumActivityOut
        public string TransuraniumActivityOut_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("трансурановые радионуклиды")]
        public RamAccess<string> TransuraniumActivityOut//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(TransuraniumActivityOut_Validation, TransuraniumActivityOut_DB);
                    tmp.PropertyChanged += TransuraniumActivityOutValueChanged;
                    return tmp;
            }
            set
            {
                    TransuraniumActivityOut_DB = value.Value;
                OnPropertyChanged(nameof(TransuraniumActivityOut));
            }
        }

        private void TransuraniumActivityOutValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    TransuraniumActivityOut_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                TransuraniumActivityOut_DB = value1;
            }
        }
        private bool TransuraniumActivityOut_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value == "-")
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //TransuraniumActivityOut property
        #endregion

        #region IExcel
        public void ExcelRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelRow(worksheet, Row);
            worksheet.Cells[Row, 2].Value = RefineMachineName_DB;
            worksheet.Cells[Row, 3].Value = MachineCode_DB;
            worksheet.Cells[Row, 4].Value = MachinePower_DB;
            worksheet.Cells[Row, 5].Value = NumberOfHoursPerYear_DB;
            worksheet.Cells[Row, 6].Value = CodeRAOIn_DB;
            worksheet.Cells[Row, 7].Value = StatusRAOIn_DB;
            worksheet.Cells[Row, 8].Value = VolumeIn_DB;
            worksheet.Cells[Row, 9].Value = MassIn_DB;
            worksheet.Cells[Row, 10].Value = QuantityIn_DB;
            worksheet.Cells[Row, 11].Value = TritiumActivityIn_DB;
            worksheet.Cells[Row, 12].Value = BetaGammaActivityIn_DB;
            worksheet.Cells[Row, 13].Value = AlphaActivityIn_DB;
            worksheet.Cells[Row, 14].Value = TritiumActivityIn_DB;
            worksheet.Cells[Row, 15].Value = CodeRAOout_DB;
            worksheet.Cells[Row, 16].Value = StatusRAOout_DB;
            worksheet.Cells[Row, 17].Value = VolumeOut_DB;
            worksheet.Cells[Row, 18].Value = MassOut_DB;
            worksheet.Cells[Row, 19].Value = QuantityOZIIIout_DB;
            worksheet.Cells[Row, 20].Value = TritiumActivityOut_DB;
            worksheet.Cells[Row, 21].Value = BetaGammaActivityOut_DB;
            worksheet.Cells[Row, 22].Value = AlphaActivityOut_DB;
            worksheet.Cells[Row, 23].Value = TritiumActivityOut_DB;
        }

        public static void ExcelHeader(ExcelWorksheet worksheet)
        {
            Form2.ExcelHeader(worksheet);
            worksheet.Cells[1, 2].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(RefineMachineName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 3].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(MachineCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 4].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(MachinePower)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 5].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(NumberOfHoursPerYear)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 6].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(CodeRAOIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 7].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(StatusRAOIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 8].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(VolumeIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 9].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(MassIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 10].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(QuantityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 11].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(TritiumActivityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 12].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(BetaGammaActivityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 13].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(AlphaActivityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 14].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(TritiumActivityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 15].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(CodeRAOout)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 16].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(StatusRAOout)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 17].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(VolumeOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 18].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(MassOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 19].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(QuantityOZIIIout)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 20].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(TritiumActivityOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 21].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(BetaGammaActivityOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 22].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(AlphaActivityOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 23].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(TritiumActivityOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
        }
        #endregion
    }
}
