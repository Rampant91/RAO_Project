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
            FormNum.Value = "25";
            NumberOfFields.Value = 12;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //StoragePlaceName property
        [Attributes.Form_Property("Наименование ПХ")]
        public RamAccess<string> StoragePlaceName
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
        [Attributes.Form_Property("Код ОЯТ")]
        public RamAccess<string> CodeOYAT
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
        public RamAccess<string> CodeOYATnote
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
        [Attributes.Form_Property("Код ПХ")]
        public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
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
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public RamAccess<string> FcpNumber
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
        [Attributes.Form_Property("Масса топлива, т")]
        public RamAccess<double> FuelMass
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(FuelMass));
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

                private bool FuelMass_Validation(RamAccess<double?> value)//TODO
        {
            value.ClearErrors(); return true;}
        //FuelMass property

        //CellMass property
        [Attributes.Form_Property("Масса ОТВС(ТВЭЛ, выемной части реактора), т")]
        public RamAccess<double> CellMass
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(CellMass));
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

                private bool CellMass_Validation(RamAccess<double?> value)//TODO
        {
            value.ClearErrors(); return true;}
        //CellMass property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public RamAccess<int> Quantity
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(Quantity));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                Quantity_Validation(value);

                
                {
                    _dataAccess.Set(nameof(Quantity), value);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.
        private bool Quantity_Validation(RamAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Quantity property

        //BetaGammaActivity property
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public RamAccess<string> BetaGammaActivity
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
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public RamAccess<string> AlphaActivity
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
                value.AddError( "Поле не заполнено");return false;
            }
            if (!(value.Value.Contains('e')))
            {
                value.AddError( "Недопустимое значение");return false;
            }
            string tmp=value.Value;
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
                value.AddError( "Недопустимое значение"); return false;
            }
            return true;
        }
        //AlphaActivity property
    }
}
