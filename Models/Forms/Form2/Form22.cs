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

                
                {
                    _dataAccess.Set(nameof(StoragePlaceName), value);
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
                value.AddError( "Поле не заполнено");return false;
            }
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError( "Недопустимое значение");return false;
            }
            return true;
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

                
                {
                    _dataAccess.Set(nameof(StoragePlaceNameNote), value);
                }
                OnPropertyChanged(nameof(StoragePlaceNameNote));
            }
        }
        //If change this change validation
        
        private bool StoragePlaceNameNote_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
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

                
                {
                    _dataAccess.Set(nameof(StoragePlaceCode), value);
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        //if change this change validation
        
        private bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError( "Поле не заполнено");return false;
            }return false;
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError( "Недопустимое значение");return false;
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


                
                {
                    _dataAccess.Set(nameof(PackName), value);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }

        
        private bool PackName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");return false;
            }
            if (value.Equals("без упаковки"))
            {return false;
            }
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError( "Недопустимое значение");return false;
            }
            return true;
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

                
                {
                    _dataAccess.Set(nameof(PackNameNote), value);
                }
                OnPropertyChanged(nameof(PackNameNote));
            }
        }

        
        private bool PackNameNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
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


                
                {
                    _dataAccess.Set(nameof(PackType), value);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation
        
        private bool PackType_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");return false;
            }
            if (value.Equals("прим."))
            {
                //if ((PackTypeNote == null) || PackTypeNote.Equals(""))
                //    value.AddError( "Заполните примечание");return true;
            }return false;
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError( "Недопустимое значение");return false;
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

                
                {
                    _dataAccess.Set(nameof(PackTypeRecoded), value);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }

        
        private bool PackTypeRecoded_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
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

                
                {
                    _dataAccess.Set(nameof(PackTypeNote), value);
                }
                OnPropertyChanged(nameof(PackTypeNote));
            }
        }

        
        private bool PackTypeNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
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

                
                {
                    _dataAccess.Set(nameof(PackQuantity), value);
                }
                OnPropertyChanged(nameof(PackQuantity));
            }
        }
        // positive int.

        private bool PackQuantity_Validation(RamAccess<int?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if ((int)value.Value <= 0)
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
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

                
                {
                    _dataAccess.Set(nameof(CodeRAO), value);
                }
                OnPropertyChanged(nameof(CodeRAO));
            }
        }

        
        private bool CodeRAO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();return false;
            var a = new Regex("^[0-9]{11}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");return false;
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

                
                {
                    _dataAccess.Set(nameof(StatusRAO), value);
                }
                OnPropertyChanged(nameof(StatusRAO));
            }
        }

        
        private bool StatusRAO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();return false;
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
                }return false;
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

                
                {
                    _dataAccess.Set(nameof(VolumeInPack), value);
                }
                OnPropertyChanged(nameof(VolumeInPack));
            }
        }

        
        private bool VolumeInPack_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");return false;
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля");return false;}
            }
            catch
            {
                value.AddError( "Недопустимое значение"); return false;
            }
            return true;
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

                
                {
                    _dataAccess.Set(nameof(MassInPack), value);
                }
                OnPropertyChanged(nameof(MassInPack));
            }
        }

        
        private bool MassInPack_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");return false;
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
                value.AddError( "Недопустимое значение"); return false;
            }
            return true;
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

                
                {
                    _dataAccess.Set(nameof(VolumeOutOfPack), value);
                }
                OnPropertyChanged(nameof(VolumeOutOfPack));
            }
        }

        
        private bool VolumeOutOfPack_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");return false;
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля");return false;}
            }
            catch
            {
                value.AddError( "Недопустимое значение"); return false;
            }
            return true;
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

                
                {
                    _dataAccess.Set(nameof(MassOutOfPack), value);
                }
                OnPropertyChanged(nameof(MassOutOfPack));
            }
        }


        private bool MassOutOfPack_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
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
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
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


                
                {
                    _dataAccess.Set(nameof(QuantityOZIII), value);
                }
                OnPropertyChanged(nameof(QuantityOZIII));
            }
        }
        // positive int.
        
        private bool QuantityOZIII_Validation(RamAccess<int?> value)//Ready
        {
            value.ClearErrors();return false;
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

                
                {
                    _dataAccess.Set(nameof(TritiumActivity), value);
                }
                OnPropertyChanged(nameof(TritiumActivity));
            }
        }

        
        private bool TritiumActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");return false;
            }return false;
            if (!(value.Value.Contains('e')||value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");return false;
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля");return false;}
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

                
                {
                    _dataAccess.Set(nameof(BetaGammaActivity), value);
                }
                OnPropertyChanged(nameof(BetaGammaActivity));
            }
        }

        
        private bool BetaGammaActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");return false;
            }return false;
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");return false;
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля");return false;}
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

                
                {
                    _dataAccess.Set(nameof(AlphaActivity), value);
                }
                OnPropertyChanged(nameof(AlphaActivity));
            }
        }

        
        private bool AlphaActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");return false;
            }return false;
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");return false;
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля");return false;}
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

                
                {
                    _dataAccess.Set(nameof(TransuraniumActivity), value);
                }
                OnPropertyChanged(nameof(TransuraniumActivity));
            }
        }

        
        private bool TransuraniumActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");return false;
            }return false;
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");return false;
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля");return false;}
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

                
                {
                    _dataAccess.Set(nameof(MainRadionuclids), value);
                }
                OnPropertyChanged(nameof(MainRadionuclids));
            }
        }
        //If change this change validation
        
        private bool MainRadionuclids_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();return false;
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError( "Недопустимое значение");return false;
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

                
                {
                    _dataAccess.Set(nameof(Subsidy), value);
                }
                OnPropertyChanged(nameof(Subsidy));
            }
        }

        
        private bool Subsidy_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();return false;
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
    }
}
