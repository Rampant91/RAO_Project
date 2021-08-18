using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.5: Наличие РВ, содержащихся в отработавшем ядерном топливе, в пунктах хранения")]
    public class Form25 : Abstracts.Form2
    {
        public Form25() : base()
        {
            FormNum.Value = "2.5";
            //NumberOfFields.Value = 12;
            Validate_all();
        }

        private void Validate_all()
        {
            CodeOYAT_Validation(CodeOYAT);
            FcpNumber_Validation(FcpNumber);
            StoragePlaceCode_Validation(StoragePlaceCode);
            StoragePlaceName_Validation(StoragePlaceName);
            FuelMass_Validation(FuelMass);
            CellMass_Validation(CellMass);
            Quantity_Validation(Quantity);
            BetaGammaActivity_Validation(BetaGammaActivity);
            AlphaActivity_Validation(AlphaActivity);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //StoragePlaceName property
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Наименование ПХ")]
        public RamAccess<string> StoragePlaceName
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(StoragePlaceName_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    StoragePlaceName_DB = value.Value;
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation
        private bool StoragePlaceName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено"); return false;
            }
            return true;
        }
        //StoragePlaceName property
        #endregion

        //CodeOYAT property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Код ОЯТ")]
        public RamAccess<string> CodeOYAT
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(CodeOYAT_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    CodeOYAT_DB = value.Value;
                }
                OnPropertyChanged(nameof(CodeOYAT));
            }
        }

        private bool CodeOYAT_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено"); return false;
            }
            Regex a = new Regex("^[0-9]{5}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //CodeOYAT property
        #endregion

        ////CodeOYATnote property
        //public RamAccess<string> CodeOYATnote
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<string>(CodeOYATnote_Validation, _DB);
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            CodeOYATnote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(CodeOYATnote));
        //    }
        //}
        //private bool CodeOYATnote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////CodeOYATnote property

        //StoragePlaceCode property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Код ПХ")]
        public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(StoragePlaceCode_Validation, _DB);
                }

                {

                }
            }
            set
            {


                {
                    StoragePlaceCode_DB = value.Value;
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        private bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено"); return false;
            }
            if (!(value.Value == "-"))
            {
                if (value.Value.Length != 8)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }

            Regex a = new Regex("^[0-9]{8}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //StoragePlaceCode property
        #endregion

        //FcpNumber property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public RamAccess<string> FcpNumber
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(FcpNumber_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    FcpNumber_DB = value.Value;
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

        private bool FcpNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors(); return true;
        }
        //FcpNumber property
        #endregion

        //FuelMass property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Масса топлива, т")]
        public RamAccess<string> FuelMass
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(FuelMass_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    FuelMass_DB = value.Value;
                }
                OnPropertyChanged(nameof(FuelMass));
            }
        }

        private bool FuelMass_Validation(RamAccess<string> value)//TODO
        {
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
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
        //FuelMass property
        #endregion

        //CellMass property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Масса ОТВС(ТВЭЛ, выемной части реактора), т")]
        public RamAccess<string> CellMass
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(CellMass_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    CellMass_DB = value.Value;
                }
                OnPropertyChanged(nameof(CellMass));
            }
        }

        private bool CellMass_Validation(RamAccess<string> value)//TODO
        {
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
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
        //CellMass property
        #endregion

        //Quantity property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Количество, шт.")]
        public RamAccess<int?> Quantity
        {
            get
            {

                {
                    var tmp = new RamAccess<int?>(Quantity_Validation, _DB);//OK
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;

                }

                {

                }
            }
            set
            {



                {
                    Quantity_DB = value.Value;
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.
        private bool Quantity_Validation(RamAccess<int?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                return true;
            }
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Quantity property
        #endregion

        //BetaGammaActivity property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public RamAccess<string> BetaGammaActivity
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(BetaGammaActivity_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    BetaGammaActivity_DB = value.Value;
                }
                OnPropertyChanged(nameof(BetaGammaActivity));
            }
        }

        private bool BetaGammaActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors(); if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e')))
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                {
                    value.AddError("Число должно быть больше нуля"); return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //BetaGammaActivity property
        #endregion

        //AlphaActivity property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public RamAccess<string> AlphaActivity
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(AlphaActivity_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    AlphaActivity_DB = value.Value;
                }
                OnPropertyChanged(nameof(AlphaActivity));
            }
        }

        private bool AlphaActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e')))
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                {
                    value.AddError("Число должно быть больше нуля"); return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //AlphaActivity property
        #endregion
    }
}
