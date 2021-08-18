using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

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

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //RefineMachineName property
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Наименование установки переработки")]
        public RamAccess<string> RefineMachineName
        {
            get => new RamAccess<string>(RefineMachineName_Validation, _DB);
            set
            {
                RefineMachineName_DB = value.Value;
                OnPropertyChanged(nameof(RefineMachineName));
            }
        }

        private bool RefineMachineName_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //RefineMachineName property
#endregion

        //MachineCode property
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Код установки переработки")]
        public RamAccess<byte?> MachineCode
        {
            get => new RamAccess<byte?>(MachineCode_Validation, _DB);
            set
            {
                MachineCode_DB = value.Value;
                OnPropertyChanged(nameof(MachineCode));
            }
        }

        private bool MachineCode_Validation(RamAccess<byte?> value)//TODO
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
            bool h = (value.Value == 19) || (value.Value == 29) || (value.Value == 39) || (value.Value == 49) || (value.Value == 99) || (value.Value == 79);
            if (!(a || b || c || d || e || f || g || h))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //MachineCode property
#endregion

        //MachinePower property
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Мощность, куб. м/год")]
        public RamAccess<string> MachinePower
        {
            get => new RamAccess<string>(MachinePower_Validation, _DB);
            set
            {
                MachinePower_DB = value.Value;
                OnPropertyChanged(nameof(MachinePower));
            }
        }

        private bool MachinePower_Validation(RamAccess<string> value)//TODO
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
            string tmp = value.Value;
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
        //MachinePower property
#endregion

        //NumberOfHoursPerYear property
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Количество часов работы за год")]
        public RamAccess<string> NumberOfHoursPerYear
        {
            get => new RamAccess<string>(NumberOfHoursPerYear_Validation, _DB);
            set
            {
                NumberOfHoursPerYear_DB = value.Value;
                OnPropertyChanged(nameof(NumberOfHoursPerYear));
            }
        }

        private bool NumberOfHoursPerYear_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return false;
            }
            if (value.Value.Equals("прим.") || value.Value.Equals("0"))
            {

            }
            string tmp = value.Value;
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
        //NumberOfHoursPerYear property
#endregion

        //CodeRAOIn property
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Код РАО")]
        public RamAccess<string> CodeRAOIn
        {
            get => new RamAccess<string>(CodeRAOIn_Validation, _DB);
            set
            {
                CodeRAOIn_DB = value.Value;
                OnPropertyChanged(nameof(CodeRAOIn));
            }
        }

        private bool CodeRAOIn_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return false;
            }
            Regex a = new Regex("^[0-9]{11}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //CodeRAOIn property
#endregion

        //StatusRAOIn property
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Статус РАО")]
        public RamAccess<string> StatusRAOIn  //1 cyfer or OKPO.
        {
            get => new RamAccess<string>(StatusRAOIn_Validation, _DB);
            set
            {
                StatusRAOIn_DB = value.Value;
                OnPropertyChanged(nameof(StatusRAOIn));
            }
        }

        private bool StatusRAOIn_Validation(RamAccess<string> value)//TODO
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
        //StatusRAOIn property
#endregion

        //VolumeIn property
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Объем, куб. м")]
        public RamAccess<string> VolumeIn//SUMMARIZABLE
        {
            get => new RamAccess<string>(VolumeIn_Validation, _DB);
            set
            {
                VolumeIn_DB = value.Value;
                OnPropertyChanged(nameof(VolumeIn));
            }
        }

        private bool VolumeIn_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
        //VolumeIn property
#endregion

        //MassIn Property
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Масса, т")]
        public RamAccess<string> MassIn//SUMMARIZABLE
        {
            get => new RamAccess<string>(MassIn_Validation, _DB);
            set
            {
                MassIn_DB = value.Value;
                OnPropertyChanged(nameof(MassIn));
            }
        }

        private bool MassIn_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
        //MassIn property
#endregion

        //QuantityIn property
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public RamAccess<string> QuantityIn//SUMMARIZABLE
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(QuantityIn_Validation, _DB);//OK
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {



                {
                    QuantityIn_DB = value.Value;
                }
                OnPropertyChanged(nameof(QuantityIn));
            }
        }
        // positive int.
        private bool QuantityIn_Validation(RamAccess<string> value1)//Ready
        {
            value1.ClearErrors();
            if ((value1.Value == null)||value1.Value.Equals("прим."))
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
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Активность трития, Бк")]
        public RamAccess<string> TritiumActivityIn//SUMMARIZABLE
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(TritiumActivityIn_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    TritiumActivityIn_DB = value.Value;
                }
                OnPropertyChanged(nameof(TritiumActivityIn));
            }
        }

        private bool TritiumActivityIn_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == "-")
            {
                return true;
            }
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public RamAccess<string> BetaGammaActivityIn//SUMMARIZABLE
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(BetaGammaActivityIn_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    BetaGammaActivityIn_DB = value.Value;
                }
                OnPropertyChanged(nameof(BetaGammaActivityIn));
            }
        }

        private bool BetaGammaActivityIn_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == "-")
            {
                return true;
            }
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public RamAccess<string> AlphaActivityIn//SUMMARIZABLE
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(AlphaActivityIn_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    AlphaActivityIn_DB = value.Value;
                }
                OnPropertyChanged(nameof(AlphaActivityIn));
            }
        }

        private bool AlphaActivityIn_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == "-")
            {
                return true;
            }
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Активность трансурановых, Бк")]
        public RamAccess<string> TransuraniumActivityIn//SUMMARIZABLE
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(TransuraniumActivityIn_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    TransuraniumActivityIn_DB = value.Value;
                }
                OnPropertyChanged(nameof(TransuraniumActivityIn));
            }
        }

        private bool TransuraniumActivityIn_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == "-")
            {
                return true;
            }
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Код РАО")]
        public RamAccess<string> CodeRAOout
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(CodeRAOout_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    CodeRAOout_DB = value.Value;
                }
                OnPropertyChanged(nameof(CodeRAOout));
            }
        }

        private bool CodeRAOout_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return false;
            }
            Regex a = new Regex("^[0-9]{11}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //CodeRAOout property
#endregion

        //StatusRAOout property
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Статус РАО")]
        public RamAccess<string> StatusRAOout  //1 cyfer or OKPO.
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(StatusRAOout_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    StatusRAOout_DB = value.Value;
                }
                OnPropertyChanged(nameof(StatusRAOout));
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
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Объем, куб. м")]
        public RamAccess<string> VolumeOut//SUMMARIZABLE
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(VolumeOut_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    VolumeOut_DB = value.Value;
                }
                OnPropertyChanged(nameof(VolumeOut));
            }
        }

        private bool VolumeOut_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Масса, т")]
        public RamAccess<string> MassOut//SUMMARIZABLE
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(MassOut_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    MassOut_DB = value.Value;
                }
                OnPropertyChanged(nameof(MassOut));
            }
        }

        private bool MassOut_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public RamAccess<string> QuantityOZIIIout//SUMMARIZABLE
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(QuantityOZIIIout_Validation, _DB);//OK
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;

                }

                {

                }
            }
            set
            {



                {
                    QuantityOZIIIout_DB = value.Value;
                }
                OnPropertyChanged(nameof(QuantityOZIIIout));
            }
        }
        // positive int.
        private bool QuantityOZIIIout_Validation(RamAccess<string> value1)//Ready
        {
            value1.ClearErrors();
            if (value1.Equals("прим.") || string.IsNullOrEmpty(value1.Value))
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
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Активность трития, Бк")]
        public RamAccess<string> TritiumActivityOut//SUMMARIZABLE
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(TritiumActivityOut_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    TritiumActivityOut_DB = value.Value;
                }
                OnPropertyChanged(nameof(TritiumActivityOut));
            }
        }

        private bool TritiumActivityOut_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == "-")
            {
                return true;
            }
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public RamAccess<string> BetaGammaActivityOut//SUMMARIZABLE
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(BetaGammaActivityOut_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    BetaGammaActivityOut_DB = value.Value;
                }
                OnPropertyChanged(nameof(BetaGammaActivityOut));
            }
        }

        private bool BetaGammaActivityOut_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == "-")
            {
                return true;
            }
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public RamAccess<string> AlphaActivityOut//SUMMARIZABLE
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(AlphaActivityOut_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {
                {
                    AlphaActivityOut_DB = value.Value;
                }
                OnPropertyChanged(nameof(AlphaActivityOut));
            }
        }

        private bool AlphaActivityOut_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == "-")
            {
                return true;
            }
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Активность трансурановых, Бк")]
        public RamAccess<string> TransuraniumActivityOut//SUMMARIZABLE
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(TransuraniumActivityOut_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    TransuraniumActivityOut_DB = value.Value;
                }
                OnPropertyChanged(nameof(TransuraniumActivityOut));
            }
        }

        private bool TransuraniumActivityOut_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == "-")
            {
                return true;
            }
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
    }
}
