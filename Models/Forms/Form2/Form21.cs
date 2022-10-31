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
using Models.Interfaces;
using Models.DataAccess;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.1: Сортировка, переработка и кондиционирование РАО на установках")]
    public class Form21 : Abstracts.Form2, IBaseColor
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

        #region BaseColor
        public ColorType _BaseColor { get; set; } = ColorType.None;
        [NotMapped]
        public ColorType BaseColor {

            get => _BaseColor;
            set
            {
                if (_BaseColor != value)
                {
                    _BaseColor = value;
                    OnPropertyChanged(nameof(BaseColor));
                }
            }
        }
        #endregion

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

        #region  SumGroup
        public bool SumGroup_DB { get; set; } = false;

        [NotMapped]
        public RamAccess<bool> SumGroup
        {
            get
            {
                var tmp = new RamAccess<bool>(SumGroup_Validation, SumGroup_DB);
                tmp.PropertyChanged += SumGroupValueChanged;
                return tmp;
            }
            set
            {
                SumGroup_DB = value.Value;
                OnPropertyChanged(nameof(SumGroup));
            }
        }

        private void SumGroupValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                SumGroup_DB = ((RamAccess<bool>)Value).Value;
            }
        }

        private bool SumGroup_Validation(RamAccess<bool> value)
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region NumberInOrder
        public string NumberInOrderSum_DB { get; set; } = "";

        [NotMapped]
        [Attributes.Form_Property(true, "null-1-1", "null-1", "№ п/п", "1")]
        public RamAccess<string> NumberInOrderSum
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(NumberInOrderSum)))
                {
                    if (NumberInOrderSum_DB != "" && NumberInOrderSum_DB != null)
                    {
                        ((RamAccess<string>)Dictionary[nameof(NumberInOrderSum)]).Value = NumberInOrderSum_DB;
                    }
                    else
                    {
                        ((RamAccess<string>)Dictionary[nameof(NumberInOrderSum)]).Value = NumberInOrder_DB.ToString();
                    }
                    return (RamAccess<string>)Dictionary[nameof(NumberInOrderSum)];
                }
                else
                {
                    RamAccess<string> rm = null;
                    if (NumberInOrderSum_DB != "" && NumberInOrderSum_DB != null)
                    {
                        rm = new RamAccess<string>(NumberInOrderSum_Validation, NumberInOrderSum_DB);
                    }
                    else
                    {
                        rm = new RamAccess<string>(NumberInOrderSum_Validation, NumberInOrder_DB.ToString());
                    }
                    rm.PropertyChanged += NumberInOrderSumValueChanged;
                    Dictionary.Add(nameof(NumberInOrderSum), rm);
                    return (RamAccess<string>)Dictionary[nameof(NumberInOrderSum)];
                }
            }
            set
            {
                NumberInOrderSum_DB = value.Value;
                OnPropertyChanged(nameof(NumberInOrderSum));
            }
        }
        private void NumberInOrderSumValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                NumberInOrderSum_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool NumberInOrderSum_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        //RefineMachineName property
        #region  RefineMachineName
        public string RefineMachineName_DB { get; set; } = "";
        public bool _RefineMachineName_Hidden_Get { get; set; } = true;
        [NotMapped]
        public RefBool RefineMachineName_Hidden_Get
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(RefineMachineName_Hidden_Get)))
                {
                    ((RefBool)Dictionary[nameof(RefineMachineName_Hidden_Get)]).Set(_RefineMachineName_Hidden_Get);
                    return (RefBool)Dictionary[nameof(RefineMachineName_Hidden_Get)];

                }
                else
                {
                    var rm = new RefBool(_RefineMachineName_Hidden_Get);
                    Dictionary.Add(nameof(RefineMachineName_Hidden_Get), rm);
                    return rm;
                }
            }
            set
            {
                if (_RefineMachineName_Hidden_Get != value.Get())
                {
                    _RefineMachineName_Hidden_Get = value.Get();
                    var tmp = RefineMachineName;
                    OnPropertyChanged(nameof(RefineMachineName_Hidden_Get));
                }
            }
        }
        public bool _RefineMachineName_Hidden_Set { get; set; } = true;
        [NotMapped]
        public RefBool RefineMachineName_Hidden_Set
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(RefineMachineName_Hidden_Set)))
                {
                    ((RefBool)Dictionary[nameof(RefineMachineName_Hidden_Set)]).Set(_RefineMachineName_Hidden_Set);
                    return (RefBool)Dictionary[nameof(RefineMachineName_Hidden_Set)];

                }
                else
                {
                    var rm = new RefBool(_RefineMachineName_Hidden_Set);
                    Dictionary.Add(nameof(RefineMachineName_Hidden_Set), rm);
                    return rm;
                }
            }
            set
            {
                if (_RefineMachineName_Hidden_Set != value.Get())
                {
                    _RefineMachineName_Hidden_Set = value.Get();
                    var tmp = RefineMachineName;
                    OnPropertyChanged(nameof(RefineMachineName_Hidden_Set));
                }
            }
        }

        [NotMapped]
        [Attributes.Form_Property(true, "Установки переработки", "наименование", "2")]
        public RamAccess<string> RefineMachineName
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(RefineMachineName)))
                {
                    ((RamAccess<string>)Dictionary[nameof(RefineMachineName)]).Value = RefineMachineName_DB;
                    return (RamAccess<string>)Dictionary[nameof(RefineMachineName)];
                }
                else
                {
                    var rm = new RamAccess<string>(RefineMachineName_Validation, RefineMachineName_DB, RefineMachineName_Hidden_Get, RefineMachineName_Hidden_Set);
                    rm.PropertyChanged += RefineMachineNameValueChanged;
                    Dictionary.Add(nameof(RefineMachineName), rm);
                    return (RamAccess<string>)Dictionary[nameof(RefineMachineName)];
                }
            }
            set
            {
                if (RefineMachineName.Value != value.Value)
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
                if (_RefineMachineName_Hidden_Set)
                {
                    RefineMachineName_DB = ((RamAccess<string>)Value).Value;
                }
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
        public bool _MachineCode_Hidden_Get { get; set; } = true;
        [NotMapped]
        public RefBool MachineCode_Hidden_Get
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MachineCode_Hidden_Get)))
                {
                    ((RefBool)Dictionary[nameof(MachineCode_Hidden_Get)]).Set(_MachineCode_Hidden_Get);
                    return (RefBool)Dictionary[nameof(MachineCode_Hidden_Get)];

                }
                else
                {
                    var rm = new RefBool(_MachineCode_Hidden_Get);
                    Dictionary.Add(nameof(MachineCode_Hidden_Get), rm);
                    return rm;
                }
            }
            set
            {
                if (_MachineCode_Hidden_Get != value.Get())
                {
                    _MachineCode_Hidden_Get = value.Get();
                    var tmp = MachineCode;
                    OnPropertyChanged(nameof(MachineCode_Hidden_Get));
                }
            }
        }
        public bool _MachineCode_Hidden_Set { get; set; } = true;
        [NotMapped]
        public RefBool MachineCode_Hidden_Set
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MachineCode_Hidden_Set)))
                {
                    ((RefBool)Dictionary[nameof(MachineCode_Hidden_Set)]).Set(_MachineCode_Hidden_Set);
                    return (RefBool)Dictionary[nameof(MachineCode_Hidden_Set)];

                }
                else
                {
                    var rm = new RefBool(_MachineCode_Hidden_Set);
                    Dictionary.Add(nameof(MachineCode_Hidden_Set), rm);
                    return rm;
                }
            }
            set
            {
                if (_MachineCode_Hidden_Set != value.Get())
                {
                    _MachineCode_Hidden_Set = value.Get();
                    var tmp = MachineCode;
                    OnPropertyChanged(nameof(MachineCode_Hidden_Set));
                }
            }
        }

        [NotMapped]
        [Attributes.Form_Property(true, "Установки переработки", "код", "3")]
        public RamAccess<byte?> MachineCode
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MachineCode)))
                {
                    ((RamAccess<byte?>)Dictionary[nameof(MachineCode)]).Value = MachineCode_DB;
                    return (RamAccess<byte?>)Dictionary[nameof(MachineCode)];
                }
                else
                {
                    var rm = new RamAccess<byte?>(MachineCode_Validation, MachineCode_DB, MachineCode_Hidden_Get, MachineCode_Hidden_Set);
                    rm.PropertyChanged += MachineCodeValueChanged;
                    Dictionary.Add(nameof(MachineCode), rm);
                    return (RamAccess<byte?>)Dictionary[nameof(MachineCode)];
                }
            }
            set
            {
                if (MachineCode.Value != value.Value)
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
                if (_MachineCode_Hidden_Set)
                {
                    MachineCode_DB = ((RamAccess<byte?>)Value).Value;
                }
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
        public bool _MachinePower_Hidden_Get { get; set; } = true;
        [NotMapped]
        public RefBool MachinePower_Hidden_Get
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MachinePower_Hidden_Get)))
                {
                    ((RefBool)Dictionary[nameof(MachinePower_Hidden_Get)]).Set(_MachinePower_Hidden_Get);
                    return (RefBool)Dictionary[nameof(MachinePower_Hidden_Get)];

                }
                else
                {
                    var rm = new RefBool(_MachinePower_Hidden_Get);
                    Dictionary.Add(nameof(MachinePower_Hidden_Get), rm);
                    return rm;
                }
            }
            set
            {
                if (_MachinePower_Hidden_Get != value.Get())
                {
                    _MachinePower_Hidden_Get = value.Get();
                    var tmp = MachinePower;
                    OnPropertyChanged(nameof(MachinePower_Hidden_Get));
                }
            }
        }
        public bool _MachinePower_Hidden_Set { get; set; } = true;
        [NotMapped]
        public RefBool MachinePower_Hidden_Set
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MachinePower_Hidden_Set)))
                {
                    ((RefBool)Dictionary[nameof(MachinePower_Hidden_Set)]).Set(_MachinePower_Hidden_Set);
                    return (RefBool)Dictionary[nameof(MachinePower_Hidden_Set)];

                }
                else
                {
                    var rm = new RefBool(_MachinePower_Hidden_Set);
                    Dictionary.Add(nameof(MachinePower_Hidden_Set), rm);
                    return rm;
                }
            }
            set
            {
                if (_MachinePower_Hidden_Set != value.Get())
                {
                    _MachinePower_Hidden_Set = value.Get();
                    var tmp = MachinePower;
                    OnPropertyChanged(nameof(MachinePower_Hidden_Set));
                }
            }
        }

        public string MachinePower_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true, "Установки переработки", "мощность, куб. м/год", "4")]
        public RamAccess<string> MachinePower
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MachinePower)))
                {
                    ((RamAccess<string>)Dictionary[nameof(MachinePower)]).Value = MachinePower_DB;
                    return ((RamAccess<string>)Dictionary[nameof(MachinePower)]);

                }
                else
                {
                    var rm = new RamAccess<string>(MachinePower_Validation, MachinePower_DB, MachinePower_Hidden_Get, MachinePower_Hidden_Set);
                    rm.PropertyChanged += MachinePowerValueChanged;
                    Dictionary.Add(nameof(MachinePower), rm);
                    return rm;
                }
            }
            set
            {
                if (MachinePower.Value != value.Value)
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
                if (_MachinePower_Hidden_Set)
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
                        try
                        {
                            var value2 = Convert.ToDouble(value1);
                            value1 = String.Format("{0:0.######################################################e+00}", value2);
                        }
                        catch (Exception ex)
                        { }
                    }
                    MachinePower_DB = value1;
                }
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
        public bool _NumberOfHoursPerYear_Hidden_Get { get; set; } = true;
        [NotMapped]
        public RefBool NumberOfHoursPerYear_Hidden_Get
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(NumberOfHoursPerYear_Hidden_Get)))
                {
                    ((RefBool)Dictionary[nameof(NumberOfHoursPerYear_Hidden_Get)]).Set(_NumberOfHoursPerYear_Hidden_Get);
                    return (RefBool)Dictionary[nameof(NumberOfHoursPerYear_Hidden_Get)];

                }
                else
                {
                    var rm = new RefBool(_NumberOfHoursPerYear_Hidden_Get);
                    Dictionary.Add(nameof(NumberOfHoursPerYear_Hidden_Get), rm);
                    return rm;
                }
            }
            set
            {
                if (_NumberOfHoursPerYear_Hidden_Get != value.Get())
                {
                    _NumberOfHoursPerYear_Hidden_Get = value.Get();
                    var tmp = NumberOfHoursPerYear;
                    OnPropertyChanged(nameof(NumberOfHoursPerYear_Hidden_Get));
                }
            }
        }
        public bool _NumberOfHoursPerYear_Hidden_Set { get; set; } = true;
        [NotMapped]
        public RefBool NumberOfHoursPerYear_Hidden_Set
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(NumberOfHoursPerYear_Hidden_Set)))
                {
                    ((RefBool)Dictionary[nameof(NumberOfHoursPerYear_Hidden_Set)]).Set(_NumberOfHoursPerYear_Hidden_Set);
                    return (RefBool)Dictionary[nameof(NumberOfHoursPerYear_Hidden_Set)];

                }
                else
                {
                    var rm = new RefBool(_NumberOfHoursPerYear_Hidden_Set);
                    Dictionary.Add(nameof(NumberOfHoursPerYear_Hidden_Set), rm);
                    return rm;
                }
            }
            set
            {
                if (_NumberOfHoursPerYear_Hidden_Set != value.Get())
                {
                    _NumberOfHoursPerYear_Hidden_Set = value.Get();
                    var tmp = NumberOfHoursPerYear;
                    OnPropertyChanged(nameof(NumberOfHoursPerYear_Hidden_Set));
                }
            }
        }

        [NotMapped]
        [Attributes.Form_Property(true, "Установки переработки", "количество часов работы за год", "5")]
        public RamAccess<string> NumberOfHoursPerYear
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(NumberOfHoursPerYear)))
                {
                    ((RamAccess<string>)Dictionary[nameof(NumberOfHoursPerYear)]).Value = NumberOfHoursPerYear_DB;
                    return (RamAccess<string>)Dictionary[nameof(NumberOfHoursPerYear)];
                }
                else
                {
                    var rm = new RamAccess<string>(NumberOfHoursPerYear_Validation, NumberOfHoursPerYear_DB, NumberOfHoursPerYear_Hidden_Get, NumberOfHoursPerYear_Hidden_Set);
                    rm.PropertyChanged += NumberOfHoursPerYearValueChanged;
                    Dictionary.Add(nameof(NumberOfHoursPerYear), rm);
                    return (RamAccess<string>)Dictionary[nameof(NumberOfHoursPerYear)];
                }
            }
            set
            {
                if (NumberOfHoursPerYear.Value != value.Value)
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
                if (_NumberOfHoursPerYear_Hidden_Set)
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
                        try
                        {
                            var value2 = Convert.ToDouble(value1);
                            value1 = String.Format("{0:0.######################################################e+00}", value2);
                        }
                        catch (Exception ex)
                        { }
                    }
                    NumberOfHoursPerYear_DB = value1;
                }
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
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "код РАО", "6")]
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
            if (tmp.Length == 11)
            {
                Regex a0 = new Regex("^[1-3x+]");
                if (!a0.IsMatch(tmp.Substring(0, 1)))
                {
                    value.AddError("Недопустимое агрегатное состояние - " + tmp.Substring(0, 1));
                }
                Regex a1 = new Regex("^[0-49x+]");
                if (!a1.IsMatch(tmp.Substring(1, 1)))
                {
                    value.AddError("Недопустимое категория РАО - " + tmp.Substring(1, 1));
                }
                Regex a2 = new Regex("^[0-6x+]");
                if (!a2.IsMatch(tmp.Substring(2, 1)))
                {
                    value.AddError("Недопустимый радионуклидный состав РАО - " + tmp.Substring(2, 1));
                }
                Regex a3 = new Regex("^[12x+]");
                if (!a3.IsMatch(tmp.Substring(3, 1)))
                {
                    value.AddError("Недопустимое содержание ядерных материалов - " + tmp.Substring(3, 1));
                }
                Regex a4 = new Regex("^[12x+]");
                if (!a4.IsMatch(tmp.Substring(4, 1)))
                {
                    value.AddError("Недопустимоый период полураспада - " + tmp.Substring(4, 1));
                }
                Regex a5 = new Regex("^[0-3x+]");
                if (!a5.IsMatch(tmp.Substring(5, 1)))
                {
                    value.AddError("Недопустимоый период потенциальной опасности РАО - " + tmp.Substring(5, 1));
                }
                Regex a6 = new Regex("^[0-49x+]");
                if (!a6.IsMatch(tmp.Substring(6, 1)))
                {
                    value.AddError("Недопустимоый способ переработки - " + tmp.Substring(6, 1));
                }
                Regex a7 = new Regex("^[0-79x+]");
                if (!a7.IsMatch(tmp.Substring(7, 1)))
                {
                    value.AddError("Недопустимоый класс РАО - " + tmp.Substring(7, 1));
                }
                Regex a89 = new Regex("^[1]{1}[1-9]{1}|^[0]{1}[1]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1-9]{1}|^[4]{1}[1-6]{1}|^[5]{1}[1-9]{1}|^[6]{1}[1-9]{1}|^[7]{1}[1-9]{1}|^[8]{1}[1-9]{1}|^[9]{1}[1-9]{1}");
                if (!a89.IsMatch(tmp.Substring(8, 2)))
                {
                    value.AddError("Недопустимоый код типа РАО - " + tmp.Substring(8, 2));
                }
                Regex a10 = new Regex("^[12x+]");
                if (!a7.IsMatch(tmp.Substring(10, 1)))
                {
                    value.AddError("Недопустимая горючесть - " + tmp.Substring(10, 1));
                }
                if (value.HasErrors)
                {
                    return false;
                }
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
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "статус РАО", "7")]
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
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "куб. м", "8")]
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
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "т", "9")]
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
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "ОЗИИИ, шт", "10")]
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
            if (value1.Value.Equals("прим.") || value1.Value.Equals("-"))
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
        public string TritiumActivityIn_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "тритий", "11")]
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
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        public string BetaGammaActivityIn_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "12")]
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
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        public string AlphaActivityIn_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "альфа-излучающие радионуклиды (исключая трансурановые)", "13")]
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
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        public string TransuraniumActivityIn_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true, "Поступило РАО на переработку, кондиционирование", "трансурановые радионуклиды", "14")]
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
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "код РАО", "15")]
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
            if (tmp.Length == 11)
            {
                Regex a0 = new Regex("^[1-3x+]");
                if (!a0.IsMatch(tmp.Substring(0, 1)))
                {
                    value.AddError("Недопустимое агрегатное состояние - " + tmp.Substring(0, 1));
                }
                Regex a1 = new Regex("^[0-49x+]");
                if (!a1.IsMatch(tmp.Substring(1, 1)))
                {
                    value.AddError("Недопустимое категория РАО - " + tmp.Substring(1, 1));
                }
                Regex a2 = new Regex("^[0-6x+]");
                if (!a2.IsMatch(tmp.Substring(2, 1)))
                {
                    value.AddError("Недопустимый радионуклидный состав РАО - " + tmp.Substring(2, 1));
                }
                Regex a3 = new Regex("^[12x+]");
                if (!a3.IsMatch(tmp.Substring(3, 1)))
                {
                    value.AddError("Недопустимое содержание ядерных материалов - " + tmp.Substring(3, 1));
                }
                Regex a4 = new Regex("^[12x+]");
                if (!a4.IsMatch(tmp.Substring(4, 1)))
                {
                    value.AddError("Недопустимоый период полураспада - " + tmp.Substring(4, 1));
                }
                Regex a5 = new Regex("^[0-3x+]");
                if (!a5.IsMatch(tmp.Substring(5, 1)))
                {
                    value.AddError("Недопустимоый период потенциальной опасности РАО - " + tmp.Substring(5, 1));
                }
                Regex a6 = new Regex("^[0-49x+]");
                if (!a6.IsMatch(tmp.Substring(6, 1)))
                {
                    value.AddError("Недопустимоый способ переработки - " + tmp.Substring(6, 1));
                }
                Regex a7 = new Regex("^[0-79x+]");
                if (!a7.IsMatch(tmp.Substring(7, 1)))
                {
                    value.AddError("Недопустимоый класс РАО - " + tmp.Substring(7, 1));
                }
                Regex a89 = new Regex("^[1]{1}[1-9]{1}|^[0]{1}[1]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1-9]{1}|^[4]{1}[1-6]{1}|^[5]{1}[1-9]{1}|^[6]{1}[1-9]{1}|^[7]{1}[1-9]{1}|^[8]{1}[1-9]{1}|^[9]{1}[1-9]{1}");
                if (!a89.IsMatch(tmp.Substring(8, 2)))
                {
                    value.AddError("Недопустимоый код типа РАО - " + tmp.Substring(8, 2));
                }
                Regex a10 = new Regex("^[12x+]");
                if (!a7.IsMatch(tmp.Substring(10, 1)))
                {
                    value.AddError("Недопустимая горючесть - " + tmp.Substring(10, 1));
                }
                if (value.HasErrors)
                {
                    return false;
                }
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
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "статус РАО", "16")]
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
        public string VolumeOut_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "куб. м", "17")]
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
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        public string MassOut_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "т", "18")]
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
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        public string QuantityOZIIIout_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "ОЗИИИ, шт", "19")]
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
        public string TritiumActivityOut_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "тритий", "20")]
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
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        public string BetaGammaActivityOut_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "21")]
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
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        public string AlphaActivityOut_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "альфа-излучающие радионуклиды (исключая трансурановые)", "22")]
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
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        public string TransuraniumActivityOut_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true, "Образовалось РАО после переработки, кондиционирования", "трансурановые радионуклиды", "23")]
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
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
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
        public void ExcelGetRow(ExcelWorksheet worksheet, int Row)
        {
            double val;
            base.ExcelGetRow(worksheet, Row);
            RefineMachineName.Value = Convert.ToString(worksheet.Cells[Row, 2].Value);
            MachineCode.Value = Convert.ToByte(worksheet.Cells[Row, 3].Value);
            MachinePower.Value = Convert.ToString(worksheet.Cells[Row, 4].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 4].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 4].Value);
            NumberOfHoursPerYear.Value = Convert.ToString(worksheet.Cells[Row, 5].Value).Equals("0") ? "-" : Convert.ToString(worksheet.Cells[Row, 5].Value);
            CodeRAOIn_DB = Convert.ToString(worksheet.Cells[Row, 6].Value);
            StatusRAOIn_DB = Convert.ToString(worksheet.Cells[Row, 7].Value);
            VolumeIn_DB = Convert.ToString(worksheet.Cells[Row, 8].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 8].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 8].Value);
            MassIn_DB = Convert.ToString(worksheet.Cells[Row, 9].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 9].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 9].Value);
            QuantityIn_DB = Convert.ToString(worksheet.Cells[Row, 10].Value);
            TritiumActivityIn_DB = Convert.ToString(worksheet.Cells[Row, 11].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 11].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 11].Value);
            BetaGammaActivityIn_DB = Convert.ToString(worksheet.Cells[Row, 12].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 12].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 12].Value);
            AlphaActivityIn_DB = Convert.ToString(worksheet.Cells[Row, 13].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 13].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 13].Value);
            TritiumActivityIn_DB = Convert.ToString(worksheet.Cells[Row, 14].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 14].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 14].Value);
            CodeRAOout_DB = Convert.ToString(worksheet.Cells[Row, 15].Value);
            StatusRAOout_DB = Convert.ToString(worksheet.Cells[Row, 16].Value);
            VolumeOut_DB = Convert.ToString(worksheet.Cells[Row, 17].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 18].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 17].Value);
            MassOut_DB = Convert.ToString(worksheet.Cells[Row, 18].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 18].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 18].Value);
            QuantityOZIIIout_DB = Convert.ToString(worksheet.Cells[Row, 19].Value);
            TritiumActivityOut_DB = Convert.ToString(worksheet.Cells[Row, 20].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 20].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 20].Value);
            BetaGammaActivityOut_DB = Convert.ToString(worksheet.Cells[Row, 21].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 21].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 21].Value);
            AlphaActivityOut_DB = Convert.ToString(worksheet.Cells[Row, 22].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 22].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 22].Value);
            TransuraniumActivityOut_DB = Convert.ToString(worksheet.Cells[Row, 23].Value).Equals("0") ? "-" : double.TryParse(Convert.ToString(worksheet.Cells[Row, 23].Value), out val) ? val.ToString("0.00######################################################e+00", CultureInfo.InvariantCulture) : Convert.ToString(worksheet.Cells[Row, 23].Value);
        }
        public int ExcelRow(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true, string SumNumber = "")
        {
            var cnt = base.ExcelRow(worksheet, Row, Column, Transpon, SumNumber);
            Column += (Transpon ? cnt : 0);
            Row += (!Transpon ? cnt : 0);
            double val = 0;
            worksheet.Cells[Row + (!Transpon ? 0 : 0), Column + (Transpon ? 0 : 0)].Value = RefineMachineName.Value == null ? "" : RefineMachineName.Value;
            worksheet.Cells[Row + (!Transpon ? 1 : 0), Column + (Transpon ? 1 : 0)].Value = MachineCode.Value == null ? "" : MachineCode.Value;
            worksheet.Cells[Row + (!Transpon ? 2 : 0), Column + (Transpon ? 2 : 0)].Value = string.IsNullOrEmpty(MachinePower.Value) || MachinePower.Value == "-" ? 0 : double.TryParse(MachinePower.Value.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : MachinePower.Value;
            worksheet.Cells[Row + (!Transpon ? 3 : 0), Column + (Transpon ? 3 : 0)].Value = string.IsNullOrEmpty(NumberOfHoursPerYear.Value) || NumberOfHoursPerYear.Value == "-" ? 0 : int.TryParse(NumberOfHoursPerYear.Value.Replace("(", "").Replace(")", "").Replace(".", ","), out int valInt) ? valInt : NumberOfHoursPerYear.Value;
            worksheet.Cells[Row + (!Transpon ? 4 : 0), Column + (Transpon ? 4 : 0)].Value = CodeRAOIn_DB;
            worksheet.Cells[Row + (!Transpon ? 5 : 0), Column + (Transpon ? 5 : 0)].Value = StatusRAOIn_DB;
            worksheet.Cells[Row + (!Transpon ? 6 : 0), Column + (Transpon ? 6 : 0)].Value = string.IsNullOrEmpty(VolumeIn_DB) || VolumeIn_DB == "-" ? 0 : double.TryParse(VolumeIn_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : VolumeIn_DB;
            worksheet.Cells[Row + (!Transpon ? 7 : 0), Column + (Transpon ? 7 : 0)].Value = string.IsNullOrEmpty(MassIn_DB) || MassIn_DB == "-" ? 0 : double.TryParse(MassIn_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : MassIn_DB;
            worksheet.Cells[Row + (!Transpon ? 8 : 0), Column + (Transpon ? 8 : 0)].Value = QuantityIn_DB;
            worksheet.Cells[Row + (!Transpon ? 9 : 0), Column + (Transpon ? 9 : 0)].Value = string.IsNullOrEmpty(TritiumActivityIn_DB) || TritiumActivityIn_DB == "-" ? 0 : double.TryParse(TritiumActivityIn_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : TritiumActivityIn_DB;
            worksheet.Cells[Row + (!Transpon ? 10 : 0), Column + (Transpon ? 10 : 0)].Value = string.IsNullOrEmpty(BetaGammaActivityIn_DB) || BetaGammaActivityIn_DB == "-" ? 0 : double.TryParse(BetaGammaActivityIn_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val)  ?  val : BetaGammaActivityIn_DB;
            worksheet.Cells[Row + (!Transpon ? 11 : 0), Column + (Transpon ? 11 : 0)].Value = string.IsNullOrEmpty(AlphaActivityIn_DB) || AlphaActivityIn_DB == "-" ? 0 : double.TryParse(AlphaActivityIn_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ?  val : AlphaActivityIn_DB;
            worksheet.Cells[Row + (!Transpon ? 12 : 0), Column + (Transpon ? 12 : 0)].Value = string.IsNullOrEmpty(TritiumActivityIn_DB) || TritiumActivityIn_DB == "-" ? 0 : double.TryParse(TritiumActivityIn_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : TritiumActivityIn_DB;
            worksheet.Cells[Row + (!Transpon ? 13 : 0), Column + (Transpon ? 13 : 0)].Value = CodeRAOout_DB;
            worksheet.Cells[Row + (!Transpon ? 14 : 0), Column + (Transpon ? 14 : 0)].Value = StatusRAOout_DB;
            worksheet.Cells[Row + (!Transpon ? 15 : 0), Column + (Transpon ? 15 : 0)].Value = string.IsNullOrEmpty(VolumeOut_DB) || VolumeOut_DB == "-" ? 0 : double.TryParse(VolumeOut_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : VolumeOut_DB;
            worksheet.Cells[Row + (!Transpon ? 16 : 0), Column + (Transpon ? 16 : 0)].Value = string.IsNullOrEmpty(MassOut_DB) || MassOut_DB == "-" ? 0 : double.TryParse(MassOut_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : MassOut_DB;
            worksheet.Cells[Row + (!Transpon ? 17 : 0), Column + (Transpon ? 17 : 0)].Value = QuantityOZIIIout_DB;
            worksheet.Cells[Row + (!Transpon ? 18 : 0), Column + (Transpon ? 18 : 0)].Value = string.IsNullOrEmpty(TritiumActivityOut_DB) || TritiumActivityOut_DB == "-" ? 0 : double.TryParse(TritiumActivityOut_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : TritiumActivityOut_DB;
            worksheet.Cells[Row + (!Transpon ? 19 : 0), Column + (Transpon ? 19 : 0)].Value = string.IsNullOrEmpty(BetaGammaActivityOut_DB) || BetaGammaActivityOut_DB == "-" ? 0 : double.TryParse(BetaGammaActivityOut_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : BetaGammaActivityOut_DB;
            worksheet.Cells[Row + (!Transpon ? 20 : 0), Column + (Transpon ? 20 : 0)].Value = string.IsNullOrEmpty(AlphaActivityOut_DB) || AlphaActivityOut_DB == "-" ? 0 : double.TryParse(AlphaActivityOut_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : AlphaActivityOut_DB;
            worksheet.Cells[Row + (!Transpon ? 21 : 0), Column + (Transpon ? 21 : 0)].Value = string.IsNullOrEmpty(TransuraniumActivityOut_DB) || TransuraniumActivityOut_DB == "-" ? 0 : double.TryParse(TransuraniumActivityOut_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : TransuraniumActivityOut_DB;

            return 22;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row, int Column, bool Transpon=true)
        {
            var cnt = Form2.ExcelHeader(worksheet, Row, Column, Transpon);
            Column += (Transpon ? cnt : 0);
            Row += (!Transpon ? cnt : 0);

           worksheet.Cells[Row + (!Transpon ? 0 : 0), Column + (Transpon ? 0 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(RefineMachineName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 1 : 0), Column + (Transpon ? 1 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(MachineCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 2 : 0), Column + (Transpon ? 2 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(MachinePower)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 3 : 0), Column + (Transpon ? 3 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(NumberOfHoursPerYear)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 4 : 0), Column + (Transpon ? 4 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(CodeRAOIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 5 : 0), Column + (Transpon ? 5 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(StatusRAOIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 6 : 0), Column + (Transpon ? 6 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(VolumeIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 7 : 0), Column + (Transpon ? 7 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(MassIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 8 : 0), Column + (Transpon ? 8 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(QuantityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 9 : 0), Column + (Transpon ? 9 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(TritiumActivityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 10 : 0), Column + (Transpon ? 10 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(BetaGammaActivityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 11 : 0), Column + (Transpon ? 11 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(AlphaActivityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 12 : 0), Column + (Transpon ? 12 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(TransuraniumActivityIn)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 13 : 0), Column + (Transpon ? 13 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(CodeRAOout)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 14 : 0), Column + (Transpon ? 14 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(StatusRAOout)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 15 : 0), Column + (Transpon ? 15 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(VolumeOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 16 : 0), Column + (Transpon ? 16 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(MassOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 17 : 0), Column + (Transpon ? 17 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(QuantityOZIIIout)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 18 : 0), Column + (Transpon ? 18 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(TritiumActivityOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 19 : 0), Column + (Transpon ? 19 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(BetaGammaActivityOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 20 : 0), Column + (Transpon ? 20 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(AlphaActivityOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (!Transpon ? 21 : 0), Column + (Transpon ? 21 : 0)].Value = ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty(nameof(TransuraniumActivityOut)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];

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
                DataGridColumns NumberInOrderR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.NumberInOrderSum)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                NumberInOrderR.SetSizeColToAllLevels(50);
                NumberInOrderR.Binding = nameof(Form21.NumberInOrderSum);
                NumberInOrderR.Blocked = true;
                NumberInOrderR.ChooseLine = true;
                #endregion

                #region RefineMachineName (2)
                DataGridColumns RefineMachineNameR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.RefineMachineName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                RefineMachineNameR.SetSizeColToAllLevels(200);
                RefineMachineNameR.Binding = nameof(Form21.RefineMachineName);
                NumberInOrderR += RefineMachineNameR;
                #endregion

                #region MachineCode (3)
                DataGridColumns MachineCodeR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.MachineCode)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                MachineCodeR.SetSizeColToAllLevels(60);
                MachineCodeR.Binding = nameof(Form21.MachineCode);
                NumberInOrderR += MachineCodeR;
                #endregion

                #region MachinePower (4)
                DataGridColumns MachinePowerR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.MachinePower)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                MachinePowerR.SetSizeColToAllLevels(80);
                MachinePowerR.Binding = nameof(Form21.MachinePower);
                NumberInOrderR += MachinePowerR;
                #endregion

                #region NumberOfHoursPerYear (5)
                DataGridColumns NumberOfHoursPerYearR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.NumberOfHoursPerYear)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                NumberOfHoursPerYearR.SetSizeColToAllLevels(110);
                NumberOfHoursPerYearR.Binding = nameof(Form21.NumberOfHoursPerYear);
                NumberInOrderR += NumberOfHoursPerYearR;
                #endregion

                #region CodeRAOIn (6)
                DataGridColumns CodeRAOInR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.CodeRAOIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                CodeRAOInR.SetSizeColToAllLevels(88);
                CodeRAOInR.Binding = nameof(Form21.CodeRAOIn);
                NumberInOrderR += CodeRAOInR;
                #endregion

                #region StatusRAOIn (7)
                DataGridColumns StatusRAOInR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.StatusRAOIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                StatusRAOInR.SetSizeColToAllLevels(88);
                StatusRAOInR.Binding = nameof(Form21.StatusRAOIn);
                NumberInOrderR += StatusRAOInR;
                #endregion

                #region VolumeIn (8)
                DataGridColumns VolumeInR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.VolumeIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                VolumeInR.SetSizeColToAllLevels(88);
                VolumeInR.Binding = nameof(Form21.VolumeIn);
                NumberInOrderR += VolumeInR;
                #endregion

                #region MassIn (9)
                DataGridColumns MassInR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.MassIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                MassInR.SetSizeColToAllLevels(88);
                MassInR.Binding = nameof(Form21.MassIn);
                NumberInOrderR += MassInR;
                #endregion

                #region QuantityIn (10)
                DataGridColumns QuantityInR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.QuantityIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                QuantityInR.SetSizeColToAllLevels(88);
                QuantityInR.Binding = nameof(Form21.QuantityIn);
                NumberInOrderR += QuantityInR;
                #endregion

                #region TritiumActivityIn (11)
                DataGridColumns TritiumActivityInR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.TritiumActivityIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                TritiumActivityInR.SetSizeColToAllLevels(163);
                TritiumActivityInR.Binding = nameof(Form21.TritiumActivityIn);
                NumberInOrderR += TritiumActivityInR;
                #endregion

                #region BetaGammaActivityIn (12)
                DataGridColumns BetaGammaActivityInR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.BetaGammaActivityIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                BetaGammaActivityInR.SetSizeColToAllLevels(160);
                BetaGammaActivityInR.Binding = nameof(Form21.BetaGammaActivityIn);
                NumberInOrderR += BetaGammaActivityInR;
                #endregion

                #region AlphaActivity (13)
                DataGridColumns AlphaActivityR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.AlphaActivityIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                AlphaActivityR.SetSizeColToAllLevels(170);
                AlphaActivityR.Binding = nameof(Form21.AlphaActivityIn);
                NumberInOrderR += AlphaActivityR;
                #endregion

                #region TransuraniumActivity (14)
                DataGridColumns TransuraniumActivityR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.TransuraniumActivityIn)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                TransuraniumActivityR.SetSizeColToAllLevels(200);
                TransuraniumActivityR.Binding = nameof(Form21.TransuraniumActivityIn);
                NumberInOrderR += TransuraniumActivityR;
                #endregion

                #region CodeRAOout (15)
                DataGridColumns CodeRAOoutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.CodeRAOout)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                CodeRAOoutR.SetSizeColToAllLevels(88);
                CodeRAOoutR.Binding = nameof(Form21.CodeRAOout);
                NumberInOrderR += CodeRAOoutR;
                #endregion

                #region StatusRAOout (16)
                DataGridColumns StatusRAOoutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.StatusRAOout)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                StatusRAOoutR.SetSizeColToAllLevels(88);
                StatusRAOoutR.Binding = nameof(Form21.StatusRAOout);
                NumberInOrderR += StatusRAOoutR;
                #endregion

                #region VolumeOut (17)
                DataGridColumns VolumeOutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.VolumeOut)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                VolumeOutR.SetSizeColToAllLevels(88);
                VolumeOutR.Binding = nameof(Form21.VolumeOut);
                NumberInOrderR += VolumeOutR;
                #endregion

                #region MassOut (18)
                DataGridColumns MassOutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.MassOut)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                MassOutR.SetSizeColToAllLevels(88);
                MassOutR.Binding = nameof(Form21.MassOut);
                NumberInOrderR += MassOutR;
                #endregion

                #region QuantityOZIIIout (19)
                DataGridColumns QuantityOZIIIoutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.QuantityOZIIIout)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                QuantityOZIIIoutR.SetSizeColToAllLevels(88);
                QuantityOZIIIoutR.Binding = nameof(Form21.QuantityOZIIIout);
                NumberInOrderR += QuantityOZIIIoutR;
                #endregion

                #region TritiumActivityOut (20)
                DataGridColumns TritiumActivityOutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.TritiumActivityOut)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                TritiumActivityOutR.SetSizeColToAllLevels(163);
                TritiumActivityOutR.Binding = nameof(Form21.TritiumActivityOut);
                NumberInOrderR += TritiumActivityOutR;
                #endregion

                #region BetaGammaActivityOut (21)
                DataGridColumns BetaGammaActivityOutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.BetaGammaActivityOut)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                BetaGammaActivityOutR.SetSizeColToAllLevels(170);
                BetaGammaActivityOutR.Binding = nameof(Form21.BetaGammaActivityOut);
                NumberInOrderR += BetaGammaActivityOutR;
                #endregion

                #region AlphaActivityOut (22)
                DataGridColumns AlphaActivityOutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.AlphaActivityOut)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                AlphaActivityOutR.SetSizeColToAllLevels(170);
                AlphaActivityOutR.Binding = nameof(Form21.AlphaActivityOut);
                NumberInOrderR += AlphaActivityOutR;
                #endregion

                #region TransuraniumActivityOut (23)
                DataGridColumns TransuraniumActivityOutR = ((Attributes.Form_PropertyAttribute)typeof(Form21).GetProperty(nameof(Form21.TransuraniumActivityOut)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                TransuraniumActivityOutR.SetSizeColToAllLevels(163);
                TransuraniumActivityOutR.Binding = nameof(Form21.TransuraniumActivityOut);
                NumberInOrderR += TransuraniumActivityOutR;
                #endregion

                _DataGridColumns = NumberInOrderR;
            }
            return _DataGridColumns;
        }
        #endregion
    }
}
