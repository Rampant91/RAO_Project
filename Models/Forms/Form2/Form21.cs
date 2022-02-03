using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System; using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Linq;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using Models.Collections;

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
                OnPropertyChanged(nameof(RefineMachineName));
            }
        }
        public bool RefineMachineName_Hidden_Priv2 { get; set; } = false;
        [NotMapped]
        public bool RefineMachineName_Hidden2
        {
            get => RefineMachineName_Hidden_Priv2;
            set
            {
                RefineMachineName_Hidden_Priv2 = value;
                OnPropertyChanged(nameof(RefineMachineName));
            }
        }

        [NotMapped]
        [Attributes.Form_Property(true, "Установки переработки","наименование","2")]
        public RamAccess<string> RefineMachineName
        {
            get
            {
                if (!RefineMachineName_Hidden|| RefineMachineName_Hidden2)
                {
                    if(RefineMachineName_Hidden2)
                    {
                        var tmp = new RamAccess<string>(null, RefineMachineName_DB);
                        tmp.PropertyChanged += RefineMachineNameValueChanged;
                        return tmp;
                    }
                    else
                    {
                        var tmp = new RamAccess<string>(RefineMachineName_Validation, RefineMachineName_DB);
                        tmp.PropertyChanged += RefineMachineNameValueChanged;
                        return tmp;
                    }
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
                RefineMachineName_DB = ((RamAccess<string>)Value).Value;            }
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
                OnPropertyChanged(nameof(MachineCode));
            }
        }
        public bool MachineCode_Hidden_Priv2 { get; set; } = false;
        [NotMapped]
        public bool MachineCode_Hidden2
        {
            get => MachineCode_Hidden_Priv2;
            set
            {
                MachineCode_Hidden_Priv2 = value;
                OnPropertyChanged(nameof(MachineCode));
            }
        }

        [NotMapped]
        [Attributes.Form_Property(true, "Установки переработки", "код","3")]
        public RamAccess<byte?> MachineCode
        {
            get
            {
                if (!MachineCode_Hidden|| MachineCode_Hidden2)
                {
                    if (MachineCode_Hidden2)
                    {
                        if (Dictionary.ContainsKey(nameof(MachineCode)))
                        {
                            ((RamAccess<byte?>)Dictionary[nameof(MachineCode)]).Value = MachineCode_DB;
                            return (RamAccess<byte?>)Dictionary[nameof(MachineCode)];
                        }
                        else
                        {
                            var rm = new RamAccess<byte?>(MachineCode_Validation, MachineCode_DB);
                            rm.PropertyChanged += MachineCodeValueChanged;
                            Dictionary.Add(nameof(MachineCode), rm);
                            return (RamAccess<byte?>)Dictionary[nameof(MachineCode)];
                        }
                    }
                    else
                    {
                        var tmp = new RamAccess<byte?>(MachineCode_Validation, MachineCode_DB);
                        tmp.PropertyChanged += MachineCodeValueChanged;
                        return tmp;
                    }
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
                OnPropertyChanged(nameof(MachinePower));
            }
        }
        public bool MachinePower_Hidden_Priv2 { get; set; } = false;
        [NotMapped]
        public bool MachinePower_Hidden2
        {
            get => MachinePower_Hidden_Priv2;
            set
            {
                MachinePower_Hidden_Priv2 = value;
                OnPropertyChanged(nameof(MachinePower));
            }
        }

        [NotMapped]
        [Attributes.Form_Property(true, "Установки переработки", "мощность, куб. м/год","4")]
        public RamAccess<string> MachinePower
        {
            get
            {
                if (!MachinePower_Hidden|| MachinePower_Hidden2)
                {
                    if (MachinePower_Hidden2)
                    {
                        if (Dictionary.ContainsKey(nameof(MachinePower)))
                        {
                            ((RamAccess<string>)Dictionary[nameof(MachinePower)]).Value = MachinePower_DB;
                            ((RamAccess<string>)Dictionary[nameof(MachinePower)]).PropertyChanged -= MachinePowerValueChanged;
                            return (RamAccess<string>)Dictionary[nameof(MachinePower)];
                        }
                        else
                        {
                            var rm = new RamAccess<string>(MachinePower_Validation, MachinePower_DB);
                            Dictionary.Add(nameof(MachinePower), rm);
                            return (RamAccess<string>)Dictionary[nameof(MachinePower)];
                        }
                        //var tmp = new RamAccess<string>(null, MachinePower_DB);
                        //tmp.PropertyChanged += MachinePowerValueChanged;
                        //return tmp;
                    }
                    else
                    {
                        if (Dictionary.ContainsKey(nameof(MachinePower)))
                        {
                            ((RamAccess<string>)Dictionary[nameof(MachinePower)]).Value = MachinePower_DB;
                            return (RamAccess<string>)Dictionary[nameof(MachinePower)];
                        }
                        else
                        {
                            var rm = new RamAccess<string>(MachinePower_Validation, MachinePower_DB);
                            rm.PropertyChanged += MachinePowerValueChanged;
                            Dictionary.Add(nameof(MachinePower), rm);
                            return (RamAccess<string>)Dictionary[nameof(MachinePower)];
                        }
                    }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        MachinePower_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                MachinePower_DB = value1;
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
                OnPropertyChanged(nameof(NumberOfHoursPerYear));
            }
        }

        public bool NumberOfHoursPerYear_Hidden_Priv2 { get; set; } = false;
        [NotMapped]
        public bool NumberOfHoursPerYear_Hidden2
        {
            get => NumberOfHoursPerYear_Hidden_Priv2;
            set
            {
                NumberOfHoursPerYear_Hidden_Priv2 = value;
                OnPropertyChanged(nameof(NumberOfHoursPerYear));
            }
        }

        [NotMapped]
        [Attributes.Form_Property(true, "Установки переработки", "количество часов работы за год","5")]
        public RamAccess<string> NumberOfHoursPerYear
        {
            get
            {
                if (!NumberOfHoursPerYear_Hidden|| NumberOfHoursPerYear_Hidden2)
                {
                    if (NumberOfHoursPerYear_Hidden2)
                    {
                        if (Dictionary.ContainsKey(nameof(NumberOfHoursPerYear)))
                        {
                            ((RamAccess<string>)Dictionary[nameof(NumberOfHoursPerYear)]).Value = NumberOfHoursPerYear_DB;
                            ((RamAccess<string>)Dictionary[nameof(NumberOfHoursPerYear)]).PropertyChanged -= NumberOfHoursPerYearValueChanged;
                            return (RamAccess<string>)Dictionary[nameof(NumberOfHoursPerYear)];
                        }
                        else
                        {
                            var rm = new RamAccess<string>(NumberOfHoursPerYear_Validation, NumberOfHoursPerYear_DB);
                            Dictionary.Add(nameof(NumberOfHoursPerYear), rm);
                            return (RamAccess<string>)Dictionary[nameof(NumberOfHoursPerYear)];
                        }
                        //var tmp = new RamAccess<string>(null, NumberOfHoursPerYear_DB);
                        //tmp.PropertyChanged += NumberOfHoursPerYearValueChanged;
                        //return tmp;
                    }
                    else
                    {
                        if (Dictionary.ContainsKey(nameof(NumberOfHoursPerYear)))
                        {
                            ((RamAccess<string>)Dictionary[nameof(NumberOfHoursPerYear)]).Value = NumberOfHoursPerYear_DB;
                            return (RamAccess<string>)Dictionary[nameof(NumberOfHoursPerYear)];
                        }
                        else
                        {
                            var rm = new RamAccess<string>(NumberOfHoursPerYear_Validation, NumberOfHoursPerYear_DB);
                            rm.PropertyChanged += NumberOfHoursPerYearValueChanged;
                            Dictionary.Add(nameof(NumberOfHoursPerYear), rm);
                            return (RamAccess<string>)Dictionary[nameof(NumberOfHoursPerYear)];
                        }
                    }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        NumberOfHoursPerYear_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                NumberOfHoursPerYear_DB = value1;
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
        public string CodeRAOIn_DB { get; set; } = ""; 
        public bool CodeRAOIn_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool CodeRAOIn_Hidden
        {
            get => CodeRAOIn_Hidden_Priv;
            set
            {
                CodeRAOIn_Hidden_Priv = value;
            }
        }

        [NotMapped]
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "код РАО","6")]
        public RamAccess<string> CodeRAOIn
        {
            get
            {
                if (!CodeRAOIn_Hidden)
                {
                    if (Dictionary.ContainsKey(nameof(CodeRAOIn)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(CodeRAOIn)]).Value = CodeRAOIn_DB;
                        return (RamAccess<string>)Dictionary[nameof(CodeRAOIn)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(CodeRAOIn_Validation, CodeRAOIn_DB);
                        rm.PropertyChanged += CodeRAOInValueChanged;
                        Dictionary.Add(nameof(CodeRAOIn), rm);
                        return (RamAccess<string>)Dictionary[nameof(CodeRAOIn)];
                    }
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!CodeRAOIn_Hidden)
                {
                    CodeRAOIn_DB = value.Value;
                    OnPropertyChanged(nameof(CodeRAOIn));
                }
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
        public string StatusRAOIn_DB { get; set; } = "";
        public bool StatusRAOIn_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool StatusRAOIn_Hidden
        {
            get => StatusRAOIn_Hidden_Priv;
            set
            {
                StatusRAOIn_Hidden_Priv = value;
            }
        }
        [NotMapped]
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "статус РАО","7")]
        public RamAccess<string> StatusRAOIn  //1 cyfer or OKPO.
        {
            get
            {
                if (!StatusRAOIn_Hidden)
                {
                    if (Dictionary.ContainsKey(nameof(StatusRAOIn)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(StatusRAOIn)]).Value = StatusRAOIn_DB;
                        return (RamAccess<string>)Dictionary[nameof(StatusRAOIn)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(StatusRAOIn_Validation, StatusRAOIn_DB);
                        rm.PropertyChanged += StatusRAOInValueChanged;
                        Dictionary.Add(nameof(StatusRAOIn), rm);
                        return (RamAccess<string>)Dictionary[nameof(StatusRAOIn)];
                    }
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!StatusRAOIn_Hidden)
                {
                    StatusRAOIn_DB = value.Value;
                    OnPropertyChanged(nameof(StatusRAOIn));
                }
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
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "куб. м","8")]
        public RamAccess<string> VolumeIn//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(VolumeIn)))
                {
                    ((RamAccess<string>)Dictionary[nameof(VolumeIn)]).Value = VolumeIn_DB;
                    return (RamAccess<string>)Dictionary[nameof(VolumeIn)];
                }
                else
                {
                    var rm = new RamAccess<string>(VolumeIn_Validation, VolumeIn_DB);
                    rm.PropertyChanged += VolumeInValueChanged;
                    Dictionary.Add(nameof(VolumeIn), rm);
                    return (RamAccess<string>)Dictionary[nameof(VolumeIn)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        VolumeIn_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
            if (value.Value.Equals("-"))
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
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "т","9")]
        public RamAccess<string> MassIn//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MassIn)))
                {
                    ((RamAccess<string>)Dictionary[nameof(MassIn)]).Value = MassIn_DB;
                    return (RamAccess<string>)Dictionary[nameof(MassIn)];
                }
                else
                {
                    var rm = new RamAccess<string>(MassIn_Validation, MassIn_DB);
                    rm.PropertyChanged += MassInValueChanged;
                    Dictionary.Add(nameof(MassIn), rm);
                    return (RamAccess<string>)Dictionary[nameof(MassIn)];
                }
            }
            set
            {
                MassIn_DB = value.Value;
                OnPropertyChanged(nameof(MassIn));
            }
        }

        private void MassInValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        MassIn_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
            if (value.Value.Equals("-"))
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
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "ОЗИИИ, шт.","10")]
        public RamAccess<string> QuantityIn//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(QuantityIn)))
                {
                    ((RamAccess<string>)Dictionary[nameof(QuantityIn)]).Value = QuantityIn_DB;
                    return (RamAccess<string>)Dictionary[nameof(QuantityIn)];
                }
                else
                {
                    var rm = new RamAccess<string>(QuantityIn_Validation, QuantityIn_DB);
                    rm.PropertyChanged += QuantityInValueChanged;
                    Dictionary.Add(nameof(QuantityIn), rm);
                    return (RamAccess<string>)Dictionary[nameof(QuantityIn)];
                }
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
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "тритий","11")]
        public RamAccess<string> TritiumActivityIn//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(TritiumActivityIn)))
                {
                    ((RamAccess<string>)Dictionary[nameof(TritiumActivityIn)]).Value = TritiumActivityIn_DB;
                    return (RamAccess<string>)Dictionary[nameof(TritiumActivityIn)];
                }
                else
                {
                    var rm = new RamAccess<string>(TritiumActivityIn_Validation, TritiumActivityIn_DB);
                    rm.PropertyChanged += TritiumActivityInValueChanged;
                    Dictionary.Add(nameof(TritiumActivityIn), rm);
                    return (RamAccess<string>)Dictionary[nameof(TritiumActivityIn)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        TritiumActivityIn_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "бета-, гамма-излучающие радионуклиды (исключая тритий)","12")]
        public RamAccess<string> BetaGammaActivityIn//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(BetaGammaActivityIn)))
                {
                    ((RamAccess<string>)Dictionary[nameof(BetaGammaActivityIn)]).Value = BetaGammaActivityIn_DB;
                    return (RamAccess<string>)Dictionary[nameof(BetaGammaActivityIn)];
                }
                else
                {
                    var rm = new RamAccess<string>(BetaGammaActivityIn_Validation, BetaGammaActivityIn_DB);
                    rm.PropertyChanged += BetaGammaActivityInValueChanged;
                    Dictionary.Add(nameof(BetaGammaActivityIn), rm);
                    return (RamAccess<string>)Dictionary[nameof(BetaGammaActivityIn)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        BetaGammaActivityIn_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "альфа-излучающие радионуклиды (исключая трансурановые)","13")]
        public RamAccess<string> AlphaActivityIn//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(AlphaActivityIn)))
                {
                    ((RamAccess<string>)Dictionary[nameof(AlphaActivityIn)]).Value = AlphaActivityIn_DB;
                    return (RamAccess<string>)Dictionary[nameof(AlphaActivityIn)];
                }
                else
                {
                    var rm = new RamAccess<string>(AlphaActivityIn_Validation, AlphaActivityIn_DB);
                    rm.PropertyChanged += AlphaActivityInValueChanged;
                    Dictionary.Add(nameof(AlphaActivityIn), rm);
                    return (RamAccess<string>)Dictionary[nameof(AlphaActivityIn)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        AlphaActivityIn_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "трансурановые радионуклиды","14")]
        public RamAccess<string> TransuraniumActivityIn//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(TransuraniumActivityIn)))
                {
                    ((RamAccess<string>)Dictionary[nameof(TransuraniumActivityIn)]).Value = TransuraniumActivityIn_DB;
                    return (RamAccess<string>)Dictionary[nameof(TransuraniumActivityIn)];
                }
                else
                {
                    var rm = new RamAccess<string>(TransuraniumActivityIn_Validation, TransuraniumActivityIn_DB);
                    rm.PropertyChanged += TransuraniumActivityInValueChanged;
                    Dictionary.Add(nameof(TransuraniumActivityIn), rm);
                    return (RamAccess<string>)Dictionary[nameof(TransuraniumActivityIn)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        TransuraniumActivityIn_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
        public string CodeRAOout_DB { get; set; } = "";
        public bool CodeRAOout_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool CodeRAOout_Hidden
    {
            get => CodeRAOout_Hidden_Priv;
            set
            {
                CodeRAOout_Hidden_Priv = value;
            }
        }

        [NotMapped]
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "код РАО","15")]
        public RamAccess<string> CodeRAOout
        {
            get
            {
                if (!CodeRAOout_Hidden)
                {
                    if (Dictionary.ContainsKey(nameof(CodeRAOout)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(CodeRAOout)]).Value = CodeRAOout_DB;
                        return (RamAccess<string>)Dictionary[nameof(CodeRAOout)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(CodeRAOout_Validation, CodeRAOout_DB);
                        rm.PropertyChanged += CodeRAOoutValueChanged;
                        Dictionary.Add(nameof(CodeRAOout), rm);
                        return (RamAccess<string>)Dictionary[nameof(CodeRAOout)];
                    }
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!CodeRAOout_Hidden)
                {
                    CodeRAOout_DB = value.Value;
                    OnPropertyChanged(nameof(CodeRAOout));
                }
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
        public string StatusRAOout_DB { get; set; } = "";
        public bool StatusRAOout_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool StatusRAOout_Hidden
        {
            get => StatusRAOout_Hidden_Priv;
            set
            {
                StatusRAOout_Hidden_Priv = value;
            }
        }

        [NotMapped]
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "статус РАО","16")]
        public RamAccess<string> StatusRAOout  //1 cyfer or OKPO.
        {
            get
            {
                if (!StatusRAOout_Hidden)
                {
                    if (Dictionary.ContainsKey(nameof(StatusRAOout)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(StatusRAOout)]).Value = StatusRAOout_DB;
                        return (RamAccess<string>)Dictionary[nameof(StatusRAOout)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(StatusRAOout_Validation, StatusRAOout_DB);
                        rm.PropertyChanged += StatusRAOoutValueChanged;
                        Dictionary.Add(nameof(StatusRAOout), rm);
                        return (RamAccess<string>)Dictionary[nameof(StatusRAOout)];
                    }
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!StatusRAOout_Hidden)
                {
                    StatusRAOout_DB = value.Value;
                    OnPropertyChanged(nameof(StatusRAOout));
                }
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
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "куб. м","17")]
        public RamAccess<string> VolumeOut//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(VolumeOut)))
                {
                    ((RamAccess<string>)Dictionary[nameof(VolumeOut)]).Value = VolumeOut_DB;
                    return (RamAccess<string>)Dictionary[nameof(VolumeOut)];
                }
                else
                {
                    var rm = new RamAccess<string>(VolumeOut_Validation, VolumeOut_DB);
                    rm.PropertyChanged += VolumeOutValueChanged;
                    Dictionary.Add(nameof(VolumeOut), rm);
                    return (RamAccess<string>)Dictionary[nameof(VolumeOut)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        VolumeOut_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
            if (value.Value.Equals("-"))
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
        //VolumeOut property
        #endregion

        //MassOut Property
        #region  MassOut
        public string MassOut_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "т","18")]
        public RamAccess<string> MassOut//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MassOut)))
                {
                    ((RamAccess<string>)Dictionary[nameof(MassOut)]).Value = MassOut_DB;
                    return (RamAccess<string>)Dictionary[nameof(MassOut)];
                }
                else
                {
                    var rm = new RamAccess<string>(MassOut_Validation, MassOut_DB);
                    rm.PropertyChanged += MassOutValueChanged;
                    Dictionary.Add(nameof(MassOut), rm);
                    return (RamAccess<string>)Dictionary[nameof(MassOut)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        MassOut_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
            if (value.Value.Equals("-"))
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
        //MassOut Property
        #endregion

        //QuantityOZIIIout property
        #region  QuantityOZIIIout
        public string QuantityOZIIIout_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "ОЗИИИ, шт.","19")]
        public RamAccess<string> QuantityOZIIIout//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(QuantityOZIIIout)))
                {
                    ((RamAccess<string>)Dictionary[nameof(QuantityOZIIIout)]).Value = QuantityOZIIIout_DB;
                    return (RamAccess<string>)Dictionary[nameof(QuantityOZIIIout)];
                }
                else
                {
                    var rm = new RamAccess<string>(QuantityOZIIIout_Validation, QuantityOZIIIout_DB);
                    rm.PropertyChanged += QuantityOZIIIoutValueChanged;
                    Dictionary.Add(nameof(QuantityOZIIIout), rm);
                    return (RamAccess<string>)Dictionary[nameof(QuantityOZIIIout)];
                }
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
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "тритий","20")]
        public RamAccess<string> TritiumActivityOut//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(TritiumActivityOut)))
                {
                    ((RamAccess<string>)Dictionary[nameof(TritiumActivityOut)]).Value = TritiumActivityOut_DB;
                    return (RamAccess<string>)Dictionary[nameof(TritiumActivityOut)];
                }
                else
                {
                    var rm = new RamAccess<string>(TritiumActivityOut_Validation, TritiumActivityOut_DB);
                    rm.PropertyChanged += TritiumActivityOutValueChanged;
                    Dictionary.Add(nameof(TritiumActivityOut), rm);
                    return (RamAccess<string>)Dictionary[nameof(TritiumActivityOut)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        TritiumActivityOut_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "бета-, гамма-излучающие радионуклиды (исключая тритий)","21")]
        public RamAccess<string> BetaGammaActivityOut//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(BetaGammaActivityOut)))
                {
                    ((RamAccess<string>)Dictionary[nameof(BetaGammaActivityOut)]).Value = BetaGammaActivityOut_DB;
                    return (RamAccess<string>)Dictionary[nameof(BetaGammaActivityOut)];
                }
                else
                {
                    var rm = new RamAccess<string>(BetaGammaActivityOut_Validation, BetaGammaActivityOut_DB);
                    rm.PropertyChanged += BetaGammaActivityOutValueChanged;
                    Dictionary.Add(nameof(BetaGammaActivityOut), rm);
                    return (RamAccess<string>)Dictionary[nameof(BetaGammaActivityOut)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        BetaGammaActivityOut_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "альфа-излучающие радионуклиды (исключая трансурановые)","22")]
        public RamAccess<string> AlphaActivityOut//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(AlphaActivityOut)))
                {
                    ((RamAccess<string>)Dictionary[nameof(AlphaActivityOut)]).Value = AlphaActivityOut_DB;
                    return (RamAccess<string>)Dictionary[nameof(AlphaActivityOut)];
                }
                else
                {
                    var rm = new RamAccess<string>(AlphaActivityOut_Validation, AlphaActivityOut_DB);
                    rm.PropertyChanged += AlphaActivityOutValueChanged;
                    Dictionary.Add(nameof(AlphaActivityOut), rm);
                    return (RamAccess<string>)Dictionary[nameof(AlphaActivityOut)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        AlphaActivityOut_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "трансурановые радионуклиды","23")]
        public RamAccess<string> TransuraniumActivityOut//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(TransuraniumActivityOut)))
                {
                    ((RamAccess<string>)Dictionary[nameof(TransuraniumActivityOut)]).Value = TransuraniumActivityOut_DB;
                    return (RamAccess<string>)Dictionary[nameof(TransuraniumActivityOut)];
                }
                else
                {
                    var rm = new RamAccess<string>(TransuraniumActivityOut_Validation, TransuraniumActivityOut_DB);
                    rm.PropertyChanged += TransuraniumActivityOutValueChanged;
                    Dictionary.Add(nameof(TransuraniumActivityOut), rm);
                    return (RamAccess<string>)Dictionary[nameof(TransuraniumActivityOut)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    value1 = value1.Replace(" ", "");
                    value1 = value1.Replace("\n", "");
                    value1 = value1.Replace("\t", "");
                    value1 = value1.Replace("\r", "");
                    if (value1.Equals("-"))
                    {
                        TransuraniumActivityOut_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
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
            value1 = value1.Replace(" ", "");
            value1 = value1.Replace("\n", "");
            value1 = value1.Replace("\t", "");
            value1 = value1.Replace("\r", "");
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
        public int ExcelRow(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);


            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = RefineMachineName.Value;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = MachineCode.Value;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = MachinePower.Value;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = NumberOfHoursPerYear.Value;
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = CodeRAOIn_DB;
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = StatusRAOIn_DB;
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = VolumeIn_DB;
            worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = MassIn_DB;
            worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = QuantityIn_DB;
            worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = TritiumActivityIn_DB;
            worksheet.Cells[Row + (Transpon == false ? 10 : 0), Column + (Transpon == true ? 10 : 0)].Value = BetaGammaActivityIn_DB;
            worksheet.Cells[Row + (Transpon == false ? 11 : 0), Column + (Transpon == true ? 11 : 0)].Value = AlphaActivityIn_DB;
            worksheet.Cells[Row + (Transpon == false ? 12 : 0), Column + (Transpon == true ? 12 : 0)].Value = TritiumActivityIn_DB;
            worksheet.Cells[Row + (Transpon == false ? 13 : 0), Column + (Transpon == true ? 13 : 0)].Value = CodeRAOout_DB;
            worksheet.Cells[Row + (Transpon == false ? 14 : 0), Column + (Transpon == true ? 14 : 0)].Value = StatusRAOout_DB;
            worksheet.Cells[Row + (Transpon == false ? 15 : 0), Column + (Transpon == true ? 15 : 0)].Value = VolumeOut_DB;
            worksheet.Cells[Row + (Transpon == false ? 16 : 0), Column + (Transpon == true ? 16 : 0)].Value = MassOut_DB;
            worksheet.Cells[Row + (Transpon == false ? 17 : 0), Column + (Transpon == true ? 17 : 0)].Value = QuantityOZIIIout_DB;
            worksheet.Cells[Row + (Transpon == false ? 18 : 0), Column + (Transpon == true ? 18 : 0)].Value = TritiumActivityOut_DB;
            worksheet.Cells[Row + (Transpon == false ? 19 : 0), Column + (Transpon == true ? 19 : 0)].Value = BetaGammaActivityOut_DB;
            worksheet.Cells[Row + (Transpon == false ? 20 : 0), Column + (Transpon == true ? 20 : 0)].Value = AlphaActivityOut_DB;
            worksheet.Cells[Row + (Transpon == false ? 21 : 0), Column + (Transpon == true ? 21 : 0)].Value = TritiumActivityOut_DB;

            return 22;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = Form2.ExcelHeader(worksheet, Row, Column, Transpon);
            Column = Column +(Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

           worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(RefineMachineName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(MachineCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(MachinePower)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(NumberOfHoursPerYear)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(CodeRAOIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(StatusRAOIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(VolumeIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(MassIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(QuantityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(TritiumActivityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 10 : 0), Column + (Transpon == true ? 10 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(BetaGammaActivityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 11 : 0), Column + (Transpon == true ? 11 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(AlphaActivityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 12 : 0), Column + (Transpon == true ? 12 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(TritiumActivityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 13 : 0), Column + (Transpon == true ? 13 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(CodeRAOout)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 14 : 0), Column + (Transpon == true ? 14 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(StatusRAOout)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 15 : 0), Column + (Transpon == true ? 15 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(VolumeOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 16 : 0), Column + (Transpon == true ? 16 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(MassOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 17 : 0), Column + (Transpon == true ? 17 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(QuantityOZIIIout)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 18 : 0), Column + (Transpon == true ? 18 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(TritiumActivityOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 19 : 0), Column + (Transpon == true ? 19 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(BetaGammaActivityOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 20 : 0), Column + (Transpon == true ? 20 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(AlphaActivityOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 21 : 0), Column + (Transpon == true ? 21 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form21,Models").GetProperty(nameof(TritiumActivityOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];

            return 22;
        }
        #endregion
        #region IDataGridColumn
        private static DataGridColumns _DataGridColumns { get; set; } = null;
        public override DataGridColumns GetColumnStructure(string param = "")
        {
            if (_DataGridColumns == null)
            {
                #region NumberInOrder (1)
                DataGridColumns NumberInOrderR = ((Attributes.Form_PropertyAttribute)typeof(Form).GetProperty(nameof(Form.NumberInOrder)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                NumberInOrderR.SetSizeColToAllLevels(88);
                NumberInOrderR.Binding = nameof(Form.NumberInOrder);
                #endregion

                #region RefineMachineName (2)
                DataGridColumns RefineMachineNameR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.RefineMachineName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                RefineMachineNameR.SetSizeColToAllLevels(200);
                RefineMachineNameR.Binding = nameof(Form21.RefineMachineName);
                NumberInOrderR += RefineMachineNameR;
                #endregion

                #region MachineCode (3)
                DataGridColumns MachineCodeR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.MachineCode)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                MachineCodeR.SetSizeColToAllLevels(163);
                MachineCodeR.Binding = nameof(Form21.MachineCode);
                NumberInOrderR += MachineCodeR;
                #endregion

                #region MachinePower (4)
                DataGridColumns MachinePowerR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.MachinePower)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                MachinePowerR.SetSizeColToAllLevels(163);
                MachinePowerR.Binding = nameof(Form21.MachinePower);
                NumberInOrderR += MachinePowerR;
                #endregion

                #region NumberOfHoursPerYear (5)
                DataGridColumns NumberOfHoursPerYearR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.NumberOfHoursPerYear)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                NumberOfHoursPerYearR.SetSizeColToAllLevels(190);
                NumberOfHoursPerYearR.Binding = nameof(Form21.NumberOfHoursPerYear);
                NumberInOrderR += NumberOfHoursPerYearR;
                #endregion

                #region CodeRAOIn (6)
                DataGridColumns CodeRAOInR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.CodeRAOIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                CodeRAOInR.SetSizeColToAllLevels(88);
                CodeRAOInR.Binding = nameof(Form21.CodeRAOIn);
                NumberInOrderR += CodeRAOInR;
                #endregion

                #region StatusRAOIn (7)
                DataGridColumns StatusRAOInR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.StatusRAOIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                StatusRAOInR.SetSizeColToAllLevels(88);
                StatusRAOInR.Binding = nameof(Form21.StatusRAOIn);
                NumberInOrderR += StatusRAOInR;
                #endregion

                #region VolumeIn (8)
                DataGridColumns VolumeInR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.VolumeIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                VolumeInR.SetSizeColToAllLevels(88);
                VolumeInR.Binding = nameof(Form21.VolumeIn);
                NumberInOrderR += VolumeInR;
                #endregion

                #region MassIn (9)
                DataGridColumns MassInR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.MassIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                MassInR.SetSizeColToAllLevels(88);
                MassInR.Binding = nameof(Form21.MassIn);
                NumberInOrderR += MassInR;
                #endregion

                #region QuantityIn (10)
                DataGridColumns QuantityInR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.QuantityIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                QuantityInR.SetSizeColToAllLevels(88);
                QuantityInR.Binding = nameof(Form21.QuantityIn);
                NumberInOrderR += QuantityInR;
                #endregion

                #region TritiumActivityIn (11)
                DataGridColumns TritiumActivityInR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.TritiumActivityIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                TritiumActivityInR.SetSizeColToAllLevels(163);
                TritiumActivityInR.Binding = nameof(Form21.TritiumActivityIn);
                NumberInOrderR += TritiumActivityInR;
                #endregion

                #region BetaGammaActivityIn (12)
                DataGridColumns BetaGammaActivityInR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.BetaGammaActivityIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                BetaGammaActivityInR.SetSizeColToAllLevels(350);
                BetaGammaActivityInR.Binding = nameof(Form21.BetaGammaActivityIn);
                NumberInOrderR += BetaGammaActivityInR;
                #endregion

                #region AlphaActivity (13)
                DataGridColumns AlphaActivityR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.AlphaActivityIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                AlphaActivityR.SetSizeColToAllLevels(365);
                AlphaActivityR.Binding = nameof(Form21.AlphaActivityIn);
                NumberInOrderR += AlphaActivityR;
                #endregion

                #region TransuraniumActivity (14)
                DataGridColumns TransuraniumActivityR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.TransuraniumActivityIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                TransuraniumActivityR.SetSizeColToAllLevels(200);
                TransuraniumActivityR.Binding = nameof(Form21.TransuraniumActivityIn);
                NumberInOrderR += TransuraniumActivityR;
                #endregion

                #region CodeRAOout (15)
                DataGridColumns CodeRAOoutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.CodeRAOout)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                CodeRAOoutR.SetSizeColToAllLevels(88);
                CodeRAOoutR.Binding = nameof(Form21.CodeRAOout);
                NumberInOrderR += CodeRAOoutR;
                #endregion

                #region StatusRAOout (16)
                DataGridColumns StatusRAOoutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.StatusRAOout)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                StatusRAOoutR.SetSizeColToAllLevels(88);
                StatusRAOoutR.Binding = nameof(Form21.StatusRAOout);
                NumberInOrderR += StatusRAOoutR;
                #endregion

                #region VolumeOut (17)
                DataGridColumns VolumeOutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.VolumeOut)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                VolumeOutR.SetSizeColToAllLevels(88);
                VolumeOutR.Binding = nameof(Form21.VolumeOut);
                NumberInOrderR += VolumeOutR;
                #endregion

                #region MassOut (18)
                DataGridColumns MassOutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.MassOut)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                MassOutR.SetSizeColToAllLevels(88);
                MassOutR.Binding = nameof(Form21.MassOut);
                NumberInOrderR += MassOutR;
                #endregion

                #region QuantityOZIIIout (19)
                DataGridColumns QuantityOZIIIoutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.QuantityOZIIIout)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                QuantityOZIIIoutR.SetSizeColToAllLevels(88);
                QuantityOZIIIoutR.Binding = nameof(Form21.QuantityOZIIIout);
                NumberInOrderR += QuantityOZIIIoutR;
                #endregion

                #region TritiumActivityOut (20)
                DataGridColumns TritiumActivityOutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.TritiumActivityOut)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                TritiumActivityOutR.SetSizeColToAllLevels(163);
                TritiumActivityOutR.Binding = nameof(Form21.TritiumActivityOut);
                NumberInOrderR += TritiumActivityOutR;
                #endregion

                #region BetaGammaActivityOut (21)
                DataGridColumns BetaGammaActivityOutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.BetaGammaActivityOut)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                BetaGammaActivityOutR.SetSizeColToAllLevels(365);
                BetaGammaActivityOutR.Binding = nameof(Form21.BetaGammaActivityOut);
                NumberInOrderR += BetaGammaActivityOutR;
                #endregion

                #region AlphaActivityOut (22)
                DataGridColumns AlphaActivityOutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.AlphaActivityOut)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                AlphaActivityOutR.SetSizeColToAllLevels(365);
                AlphaActivityOutR.Binding = nameof(Form21.AlphaActivityOut);
                NumberInOrderR += AlphaActivityOutR;
                #endregion

                #region TransuraniumActivityOut (23)
                DataGridColumns TransuraniumActivityOutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.TransuraniumActivityOut)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                TransuraniumActivityOutR.SetSizeColToAllLevels(163);
                TransuraniumActivityOutR.Binding = nameof(Form21.TransuraniumActivityOut);
                NumberInOrderR += TransuraniumActivityR;
                #endregion

                _DataGridColumns = NumberInOrderR;
            }
            return _DataGridColumns;
        }
        #endregion
    }
}
