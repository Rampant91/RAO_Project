using Models.DataAccess;
using System;
using System.Text.RegularExpressions;
using System.Globalization;

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
            _dataAccess.Init<string>(nameof(CodeOYAT), CodeOYAT_Validation, null);
            CodeOYAT.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(FcpNumber), FcpNumber_Validation, null);
            FcpNumber.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(StoragePlaceCode), StoragePlaceCode_Validation, null);
            StoragePlaceCode.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(StoragePlaceName), StoragePlaceName_Validation, null);
            StoragePlaceName.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(FuelMass), FuelMass_Validation, null);
            FuelMass.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(CellMass), CellMass_Validation, null);
            CellMass.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<int?>(nameof(Quantity), Quantity_Validation, null);
            Quantity.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(BetaGammaActivity), BetaGammaActivity_Validation, null);
            BetaGammaActivity.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(AlphaActivity), AlphaActivity_Validation, null);
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
        [Attributes.Form_Property("Наименование ПХ")]public int? StoragePlaceNameId { get; set; }
        public virtual RamAccess<string> StoragePlaceName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(StoragePlaceName));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(StoragePlaceName), value);
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation
                private bool StoragePlaceName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
        //StoragePlaceName property

        //CodeOYAT property
        [Attributes.Form_Property("Код ОЯТ")]public int? CodeOYATId { get; set; }
        public virtual RamAccess<string> CodeOYAT
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CodeOYAT));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(CodeOYAT), value);
                }
                OnPropertyChanged(nameof(CodeOYAT));
            }
        }

                private bool CodeOYAT_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //CodeOYAT property

        //CodeOYATnote property
        public virtual RamAccess<string> CodeOYATnote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CodeOYATnote));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(CodeOYATnote), value);
                }
                OnPropertyChanged(nameof(CodeOYATnote));
            }
        }
                private bool CodeOYATnote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //CodeOYATnote property

        //StoragePlaceCode property
        [Attributes.Form_Property("Код ПХ")]public int? StoragePlaceCodeId { get; set; }
        public virtual RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(StoragePlaceCode));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(StoragePlaceCode), value);
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        private bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (!(value.Value == "-"))
                if (value.Value.Length != 8)
                {
                    value.AddError("Недопустимое значение");
                    return false;
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
        [Attributes.Form_Property("Номер мероприятия ФЦП")]public int? FcpNumberId { get; set; }
        public virtual RamAccess<string> FcpNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FcpNumber));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(FcpNumber), value);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

                private bool FcpNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors(); return true;}
        //FcpNumber property

        //FuelMass property
        [Attributes.Form_Property("Масса топлива, т")]public int? FuelMassId { get; set; }
        public virtual RamAccess<string> FuelMass
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FuelMass));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(FuelMass), value);
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
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        [Attributes.Form_Property("Масса ОТВС(ТВЭЛ, выемной части реактора), т")]public int? CellMassId { get; set; }
        public virtual RamAccess<string> CellMass
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CellMass));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(CellMass), value);
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
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        [Attributes.Form_Property("Количество, шт.")]public int? QuantityId { get; set; }
        public virtual RamAccess<int?> Quantity
        {
            get
            {
                
                {
                    return _dataAccess.Get<int?>(nameof(Quantity));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(Quantity), value);
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
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]public int? BetaGammaActivityId { get; set; }
        public virtual RamAccess<string> BetaGammaActivity
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(BetaGammaActivity));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(BetaGammaActivity), value);
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
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]public int? AlphaActivityId { get; set; }
        public virtual RamAccess<string> AlphaActivity
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(AlphaActivity));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(AlphaActivity), value);
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
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
