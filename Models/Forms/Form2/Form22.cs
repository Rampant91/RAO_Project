using Models.DataAccess;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.2: Наличие РАО в пунктах хранения, местах сбора и/или временного хранения")]
    public class Form22 : Abstracts.Form2
    {
        public Form22() : base()
        {
            FormNum.Value = "22";
            NumberOfFields.Value = 25;
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
                StoragePlaceName_Validation(value);
                
                {
                    _dataAccess.Set(nameof(StoragePlaceName), value);
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation
        
        private void StoragePlaceName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //StoragePlaceName property

        //StoragePlaceNameNote property
        public RamAccess<string> StoragePlaceNameNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(StoragePlaceNameNote));
                }
                
                {
                    
                }
            }
            set
            {
                StoragePlaceNameNote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(StoragePlaceNameNote), value);
                }
                OnPropertyChanged(nameof(StoragePlaceNameNote));
            }
        }
        //If change this change validation
        
        private void StoragePlaceNameNote_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //StoragePlaceNameNote property

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
                StoragePlaceCode_Validation(value);
                
                {
                    _dataAccess.Set(nameof(StoragePlaceCode), value);
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        //if change this change validation
        
        private void StoragePlaceCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //StoragePlaceCode property

        //PackName property
        [Attributes.Form_Property("Наименование упаковки")]
        public RamAccess<string> PackName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackName));//OK
                }
                
                {
                    
                }
            }
            set
            {
                PackName_Validation(value);

                
                {
                    _dataAccess.Set(nameof(PackName), value);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }

        
        private void PackName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("без упаковки"))
            {
                return;
            }
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //PackName property

        //PackNameNote property
        public RamAccess<string> PackNameNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackNameNote));//OK
                }
                
                {
                    
                }
            }
            set
            {
                PackNameNote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(PackNameNote), value);
                }
                OnPropertyChanged(nameof(PackNameNote));
            }
        }

        
        private void PackNameNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
        }
        //PackNameNote property

        //PackType property
        [Attributes.Form_Property("Тип упаковки")]
        public RamAccess<string> PackType
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackType));//OK
                }
                
                {
                    
                }
            }
            set
            {
                PackType_Validation(value);

                
                {
                    _dataAccess.Set(nameof(PackType), value);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation
        
        private void PackType_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
            {
                if ((PackTypeNote == null) || PackTypeNote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if (value.Equals("-")) return;
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //PackType property

        //PackTypeRecoded property
        public RamAccess<string> PackTypeRecoded
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackTypeRecoded));//OK
                }
                
                {
                    
                }
            }
            set
            {
                PackTypeRecoded_Validation(value);
                
                {
                    _dataAccess.Set(nameof(PackTypeRecoded), value);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }

        
        private void PackTypeRecoded_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
        }
        //PackTypeRecoded property

        //PackTypeNote property
        public RamAccess<string> PackTypeNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackTypeNote));//OK
                }
                
                {
                    
                }
            }
            set
            {
                PackTypeNote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(PackTypeNote), value);
                }
                OnPropertyChanged(nameof(PackTypeNote));
            }
        }

        
        private void PackTypeNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
        }
        //PackTypeNote property

        //PackQuantity property
        [Attributes.Form_Property("Количество упаковок, шт.")]
        public RamAccess<int?> PackQuantity
        {
            get
            {
                
                {
                    return _dataAccess.Get<int?>(nameof(PackQuantity));
                }
                
                {
                    
                }
            }
            set
            {
                PackQuantity_Validation(value);
                
                {
                    _dataAccess.Set(nameof(PackQuantity), value);
                }
                OnPropertyChanged(nameof(PackQuantity));
            }
        }
        // positive int.
        
        private void PackQuantity_Validation(RamAccess<int?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError( "Поле не заполнено");
            }
            if ((int)value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //PackQuantity property

        //CodeRAO property
        [Attributes.Form_Property("Код РАО")]
        public RamAccess<string> CodeRAO
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CodeRAO));
                }
                
                {
                    
                }
            }
            set
            {
                CodeRAO_Validation(value);
                
                {
                    _dataAccess.Set(nameof(CodeRAO), value);
                }
                OnPropertyChanged(nameof(CodeRAO));
            }
        }

        
        private void CodeRAO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value)) return;
            var a = new Regex("^[0-9]{11}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //CodeRAO property

        //StatusRAO property
        [Attributes.Form_Property("Статус РАО")]
        public RamAccess<string> StatusRAO  //1 cyfer or OKPO.
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(StatusRAO));
                }
                
                {
                    
                }
            }
            set
            {
                StatusRAO_Validation(value);
                
                {
                    _dataAccess.Set(nameof(StatusRAO), value);
                }
                OnPropertyChanged(nameof(StatusRAO));
            }
        }

        
        private void StatusRAO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value)) return;
            if (value.Value.Length == 1)
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
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
                value.AddError( "Недопустимое значение");
            
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //StatusRAO property

        //VolumeInPack property
        [Attributes.Form_Property("Объем с упаковкой, куб. м")]
        public RamAccess<string> VolumeInPack
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(VolumeInPack));
                }
                
                {
                    
                }
            }
            set
            {
                VolumeInPack_Validation(value);
                
                {
                    _dataAccess.Set(nameof(VolumeInPack), value);
                }
                OnPropertyChanged(nameof(VolumeInPack));
            }
        }

        
        private void VolumeInPack_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                return;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
                return;
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //VolumeInPack property

        //MassInPack Property
        [Attributes.Form_Property("Масса с упаковкой, т")]
        public RamAccess<string> MassInPack
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassInPack));
                }
                
                {
                    
                }
            }
            set
            {
                MassInPack_Validation(value);
                
                {
                    _dataAccess.Set(nameof(MassInPack), value);
                }
                OnPropertyChanged(nameof(MassInPack));
            }
        }

        
        private void MassInPack_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                return;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
                return;
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //MassInPack Property

        //VolumeOutOfPack property
        [Attributes.Form_Property("Объем без упаковки, куб. м")]
        public RamAccess<string> VolumeOutOfPack//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(VolumeOutOfPack));
                }
                
                {
                    
                }
            }
            set
            {
                VolumeOutOfPack_Validation(value);
                
                {
                    _dataAccess.Set(nameof(VolumeOutOfPack), value);
                }
                OnPropertyChanged(nameof(VolumeOutOfPack));
            }
        }

        
        private void VolumeOutOfPack_Validation(RamAccess<string> value)//TODO
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //VolumeOutOfPack property

        //MassOutOfPack Property
        [Attributes.Form_Property("Масса без упаковки, т")]
        public RamAccess<string> MassOutOfPack//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MassOutOfPack));
                }
                
                {
                    
                }
            }
            set
            {
                MassOutOfPack_Validation(value);
                
                {
                    _dataAccess.Set(nameof(MassOutOfPack), value);
                }
                OnPropertyChanged(nameof(MassOutOfPack));
            }
        }

        
        private void MassOutOfPack_Validation(RamAccess<string> value)//TODO
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //MassOutOfPack Property

        //QuantityOZIII property
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public RamAccess<int?> QuantityOZIII//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<int?>(nameof(QuantityOZIII));//OK
                }
                
                {
                    
                }
            }
            set
            {
                QuantityOZIII_Validation(value);

                
                {
                    _dataAccess.Set(nameof(QuantityOZIII), value);
                }
                OnPropertyChanged(nameof(QuantityOZIII));
            }
        }
        // positive int.
        
        private void QuantityOZIII_Validation(RamAccess<int?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null) return;
            if ((int)value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityOZIII property

        //TritiumActivity property
        [Attributes.Form_Property("Активность трития, Бк")]
        public RamAccess<string> TritiumActivity//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(TritiumActivity));
                }
                
                {
                    
                }
            }
            set
            {
                TritiumActivity_Validation(value);
                
                {
                    _dataAccess.Set(nameof(TritiumActivity), value);
                }
                OnPropertyChanged(nameof(TritiumActivity));
            }
        }

        
        private void TritiumActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (!(value.Value.Contains('e')||value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
                return;
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //TritiumActivity property

        //BetaGammaActivity property
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public RamAccess<string> BetaGammaActivity//SUMMARIZABLE
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
                BetaGammaActivity_Validation(value);
                
                {
                    _dataAccess.Set(nameof(BetaGammaActivity), value);
                }
                OnPropertyChanged(nameof(BetaGammaActivity));
            }
        }

        
        private void BetaGammaActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //BetaGammaActivity property

        //AlphaActivity property
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public RamAccess<string> AlphaActivity//SUMMARIZABLE
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
                AlphaActivity_Validation(value);
                
                {
                    _dataAccess.Set(nameof(AlphaActivity), value);
                }
                OnPropertyChanged(nameof(AlphaActivity));
            }
        }

        
        private void AlphaActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //AlphaActivity property

        //TransuraniumActivity property
        [Attributes.Form_Property("Активность трансурановых, Бк")]
        public RamAccess<string> TransuraniumActivity//SUMMARIZABLE
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(TransuraniumActivity));
                }
                
                {
                    
                }
            }
            set
            {
                TransuraniumActivity_Validation(value);
                
                {
                    _dataAccess.Set(nameof(TransuraniumActivity), value);
                }
                OnPropertyChanged(nameof(TransuraniumActivity));
            }
        }

        
        private void TransuraniumActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //TransuraniumActivity property

        //MainRadionuclids property
        [Attributes.Form_Property("Радионуклиды")]
        public RamAccess<string> MainRadionuclids
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(MainRadionuclids));
                }
                
                {
                    
                }
            }
            set
            {
                MainRadionuclids_Validation(value);
                
                {
                    _dataAccess.Set(nameof(MainRadionuclids), value);
                }
                OnPropertyChanged(nameof(MainRadionuclids));
            }
        }
        //If change this change validation
        
        private void MainRadionuclids_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value)) return;
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //MainRadionuclids property

        //Subsidy property
        [Attributes.Form_Property("Субсидия, %")]
        public RamAccess<string> Subsidy // 0<number<=100 or empty.
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Subsidy));
                }
                
                {
                    
                }
            }
            set
            {
                Subsidy_Validation(value);
                
                {
                    _dataAccess.Set(nameof(Subsidy), value);
                }
                OnPropertyChanged(nameof(Subsidy));
            }
        }

        
        private void Subsidy_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value)) return;
            try
            {
                int tmp = Int32.Parse(value.Value);
                if (!((tmp > 0) && (tmp <= 100)))
                    value.AddError( "Недопустимое значение");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //Subsidy property

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
                FcpNumber_Validation(value);
                
                {
                    _dataAccess.Set(nameof(FcpNumber), value);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

        
        private void FcpNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //FcpNumber property
    }
}
