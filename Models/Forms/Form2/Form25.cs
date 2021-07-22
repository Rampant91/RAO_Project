using Models.DataAccess;
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
            //FormNum.Value = "25";
            //NumberOfFields.Value = 12;
            Init();
            Validate_all();
        }

        private void Init()
        {
            DataAccess.Init<string>(nameof(CodeOYAT), CodeOYAT_Validation, null);
            CodeOYAT.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(FcpNumber), FcpNumber_Validation, null);
            FcpNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(StoragePlaceCode), StoragePlaceCode_Validation, null);
            StoragePlaceCode.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(StoragePlaceName), StoragePlaceName_Validation, null);
            StoragePlaceName.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(FuelMass), FuelMass_Validation, null);
            FuelMass.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(CellMass), CellMass_Validation, null);
            CellMass.PropertyChanged += InPropertyChanged;
            DataAccess.Init<int?>(nameof(Quantity), Quantity_Validation, null);
            Quantity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(BetaGammaActivity), BetaGammaActivity_Validation, null);
            BetaGammaActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(AlphaActivity), AlphaActivity_Validation, null);
            AlphaActivity.PropertyChanged += InPropertyChanged;
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
        public int? StoragePlaceNameId { get; set; }
        [Attributes.Form_Property("Наименование ПХ")]
        public virtual RamAccess<string> StoragePlaceName
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(StoragePlaceName));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(StoragePlaceName), value);
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

        //CodeOYAT property
        public int? CodeOYATId { get; set; }
        [Attributes.Form_Property("Код ОЯТ")]
        public virtual RamAccess<string> CodeOYAT
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(CodeOYAT));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(CodeOYAT), value);
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
            return true;
        }
        //CodeOYAT property

        ////CodeOYATnote property
        //public virtual RamAccess<string> CodeOYATnote
        //{
        //    get
        //    {

        //        {
        //            return DataAccess.Get<string>(nameof(CodeOYATnote));
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            DataAccess.Set(nameof(CodeOYATnote), value);
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
        public int? StoragePlaceCodeId { get; set; }
        [Attributes.Form_Property("Код ПХ")]
        public virtual RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(StoragePlaceCode));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(StoragePlaceCode), value);
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

        //FcpNumber property
        public int? FcpNumberId { get; set; }
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public virtual RamAccess<string> FcpNumber
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(FcpNumber));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(FcpNumber), value);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

        private bool FcpNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors(); return true;
        }
        //FcpNumber property

        //FuelMass property
        public int? FuelMassId { get; set; }
        [Attributes.Form_Property("Масса топлива, т")]
        public virtual RamAccess<string> FuelMass
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(FuelMass));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(FuelMass), value);
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

        //CellMass property
        public int? CellMassId { get; set; }
        [Attributes.Form_Property("Масса ОТВС(ТВЭЛ, выемной части реактора), т")]
        public virtual RamAccess<string> CellMass
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(CellMass));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(CellMass), value);
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

        //Quantity property
        public int? QuantityId { get; set; }
        [Attributes.Form_Property("Количество, шт.")]
        public virtual RamAccess<int?> Quantity
        {
            get
            {

                {
                    return DataAccess.Get<int?>(nameof(Quantity));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(Quantity), value);
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

        //BetaGammaActivity property
        public int? BetaGammaActivityId { get; set; }
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public virtual RamAccess<string> BetaGammaActivity
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(BetaGammaActivity));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(BetaGammaActivity), value);
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

        //AlphaActivity property
        public int? AlphaActivityId { get; set; }
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public virtual RamAccess<string> AlphaActivity
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(AlphaActivity));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(AlphaActivity), value);
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
    }
}
