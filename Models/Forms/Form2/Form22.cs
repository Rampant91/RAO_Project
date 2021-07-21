using Models.DataAccess;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using Spravochniki;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.2: Наличие РАО в пунктах хранения, местах сбора и/или временного хранения")]
    public class Form22 : Abstracts.Form2
    {
        public Form22() : base()
        {
            //FormNum.Value = "22";
            //NumberOfFields.Value = 25;
            Init();
            Validate_all();
        }

        private void Init()
        {
            DataAccess.Init<string>(nameof(StoragePlaceName), StoragePlaceName_Validation, null);
            StoragePlaceName.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(StoragePlaceCode), StoragePlaceCode_Validation, null);
            StoragePlaceCode.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackName), PackName_Validation, null);
            PackName.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackType), PackType_Validation, null);
            PackType.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(CodeRAO), CodeRAO_Validation, null);
            CodeRAO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(StatusRAO), StatusRAO_Validation, null);
            StatusRAO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(VolumeOutOfPack), VolumeOutOfPack_Validation, null);
            VolumeOutOfPack.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(MassInPack), MassInPack_Validation, null);
            MassInPack.PropertyChanged += InPropertyChanged;
            DataAccess.Init<int?>(nameof(QuantityOZIII), QuantityOZIII_Validation, null);
            QuantityOZIII.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(TritiumActivity), TritiumActivity_Validation, null);
            TritiumActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(BetaGammaActivity), BetaGammaActivity_Validation, null);
            BetaGammaActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(TransuraniumActivity), TransuraniumActivity_Validation, null);
            TransuraniumActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(AlphaActivity), AlphaActivity_Validation, null);
            AlphaActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(VolumeInPack), VolumeInPack_Validation, null);
            VolumeInPack.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(MassOutOfPack), MassOutOfPack_Validation, null);
            MassOutOfPack.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(MainRadionuclids), MainRadionuclids_Validation, null);
            MainRadionuclids.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Subsidy), Subsidy_Validation, null);
            Subsidy.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(FcpNumber), FcpNumber_Validation, null);
            FcpNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<int?>(nameof(PackQuantity), PackQuantity_Validation, null);
            PackQuantity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackTypeRecoded), PackTypeRecoded_Validation, null);
            PackTypeRecoded.PropertyChanged += InPropertyChanged;
        }

        private void Validate_all()
        {
            StoragePlaceName_Validation(StoragePlaceName);
            StoragePlaceCode_Validation(StoragePlaceCode);
            PackName_Validation(PackName);
            PackType_Validation(PackType);
            CodeRAO_Validation(CodeRAO);
            StatusRAO_Validation(StatusRAO);
            VolumeOutOfPack_Validation(VolumeOutOfPack);
            MassInPack_Validation(MassInPack);
            QuantityOZIII_Validation(QuantityOZIII);
            TritiumActivity_Validation(TritiumActivity);
            BetaGammaActivity_Validation(BetaGammaActivity);
            TransuraniumActivity_Validation(TransuraniumActivity);
            AlphaActivity_Validation(AlphaActivity);
            VolumeInPack_Validation(VolumeInPack);
            MassOutOfPack_Validation(MassOutOfPack);
            MainRadionuclids_Validation(MainRadionuclids);
            Subsidy_Validation(Subsidy);
            FcpNumber_Validation(FcpNumber);
            PackQuantity_Validation(PackQuantity);
            PackTypeRecoded_Validation(PackTypeRecoded);
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
                value.AddError( "Поле не заполнено");
return false;
            }
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            return true;
        }
        //StoragePlaceName property

        ////StoragePlaceNameNote property
        //public virtual RamAccess<string> StoragePlaceNameNote
        //{
        //    get
        //    {
                
        //        {
        //            return DataAccess.Get<string>(nameof(StoragePlaceNameNote));
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {

                
        //        {
        //            DataAccess.Set(nameof(StoragePlaceNameNote), value);
        //        }
        //        OnPropertyChanged(nameof(StoragePlaceNameNote));
        //    }
        //}
        ////If change this change validation
        
        //private bool StoragePlaceNameNote_Validation(RamAccess<string> value)//Ready
        //{
        //    value.ClearErrors(); return true;}
        ////StoragePlaceNameNote property

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
        //if change this change validation

        private bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //StoragePlaceCode property

        //PackName property
        public int? PackNameId { get; set; }
        [Attributes.Form_Property("Наименование упаковки")]
        public virtual RamAccess<string> PackName
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(PackName));//OK
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    DataAccess.Set(nameof(PackName), value);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }

        
        private bool PackName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (value.Equals("без упаковки"))
            {
return false;
            }
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            return true;
        }
        //PackName property

        ////PackNameNote property
        //public virtual RamAccess<string> PackNameNote
        //{
        //    get
        //    {
                
        //        {
        //            return DataAccess.Get<string>(nameof(PackNameNote));//OK
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {

                
        //        {
        //            DataAccess.Set(nameof(PackNameNote), value);
        //        }
        //        OnPropertyChanged(nameof(PackNameNote));
        //    }
        //}

        
        //private bool PackNameNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////PackNameNote property

        //PackType property
        public int? PackTypeId { get; set; }
        [Attributes.Form_Property("Тип упаковки")]
        public virtual RamAccess<string> PackType
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(PackType));//OK
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    DataAccess.Set(nameof(PackType), value);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation

        private bool PackType_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Equals("прим."))
            {
                //if ((PackTypeNote == null) || PackTypeNote.Equals(""))
                //    value.AddError( "Заполните примечание");
                return true;
            }
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //PackType property

        //PackTypeRecoded property
        public virtual RamAccess<string> PackTypeRecoded
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(PackTypeRecoded));//OK
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(PackTypeRecoded), value);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }

        
        private bool PackTypeRecoded_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PackTypeRecoded property

        ////PackTypeNote property
        //public virtual RamAccess<string> PackTypeNote
        //{
        //    get
        //    {
                
        //        {
        //            return DataAccess.Get<string>(nameof(PackTypeNote));//OK
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {

                
        //        {
        //            DataAccess.Set(nameof(PackTypeNote), value);
        //        }
        //        OnPropertyChanged(nameof(PackTypeNote));
        //    }
        //}

        
        //private bool PackTypeNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////PackTypeNote property

        //PackQuantity property
        public int? PackQuantityId { get; set; }
        [Attributes.Form_Property("Количество упаковок, шт.")]
        public virtual RamAccess<int?> PackQuantity
        {
            get
            {
                
                {
                    return DataAccess.Get<int?>(nameof(PackQuantity));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(PackQuantity), value);
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
        public int? CodeRAOId { get; set; }
        [Attributes.Form_Property("Код РАО")]
        public virtual RamAccess<string> CodeRAO
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(CodeRAO));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(CodeRAO), value);
                }
                OnPropertyChanged(nameof(CodeRAO));
            }
        }


        private bool CodeRAO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var a = new Regex("^[0-9]{11}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //CodeRAO property

        //StatusRAO property
        public int? StatusRAOId { get; set; }
        [Attributes.Form_Property("Статус РАО")]
        public virtual RamAccess<string> StatusRAO  //1 cyfer or OKPO.
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(StatusRAO));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(StatusRAO), value);
                }
                OnPropertyChanged(nameof(StatusRAO));
            }
        }


        private bool StatusRAO_Validation(RamAccess<string> value)//TODO
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
            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //StatusRAO property

        //VolumeInPack property
        public int? VolumeInPackId { get; set; }
        [Attributes.Form_Property("Объем с упаковкой, куб. м")]
        public virtual RamAccess<string> VolumeInPack
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(VolumeInPack));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(VolumeInPack), value);
                }
                OnPropertyChanged(nameof(VolumeInPack));
            }
        }

        
        private bool VolumeInPack_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
        public int? MassInPackId { get; set; }
        [Attributes.Form_Property("Масса с упаковкой, т")]
        public virtual RamAccess<string> MassInPack
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(MassInPack));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(MassInPack), value);
                }
                OnPropertyChanged(nameof(MassInPack));
            }
        }

        
        private bool MassInPack_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
                value.AddError( "Недопустимое значение"); return false;
            }
            return true;
        }
        //MassInPack Property

        //VolumeOutOfPack property
        public int? VolumeOutOfPackId { get; set; }
        [Attributes.Form_Property("Объем без упаковки, куб. м")]
        public virtual RamAccess<string> VolumeOutOfPack//SUMMARIZABLE
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(VolumeOutOfPack));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(VolumeOutOfPack), value);
                }
                OnPropertyChanged(nameof(VolumeOutOfPack));
            }
        }

        
        private bool VolumeOutOfPack_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
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
        public int? MassOutOfPackId { get; set; }
        [Attributes.Form_Property("Масса без упаковки, т")]
        public virtual RamAccess<string> MassOutOfPack//SUMMARIZABLE
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(MassOutOfPack));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(MassOutOfPack), value);
                }
                OnPropertyChanged(nameof(MassOutOfPack));
            }
        }


        private bool MassOutOfPack_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
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
        public int? QuantityOZIIIId { get; set; }
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public virtual RamAccess<int?> QuantityOZIII//SUMMARIZABLE
        {
            get
            {
                
                {
                    return DataAccess.Get<int?>(nameof(QuantityOZIII));//OK
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    DataAccess.Set(nameof(QuantityOZIII), value);
                }
                OnPropertyChanged(nameof(QuantityOZIII));
            }
        }
        // positive int.

        private bool QuantityOZIII_Validation(RamAccess<int?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                return true;
            }
            if ((int)value.Value <= 0)
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //QuantityOZIII property

        //TritiumActivity property
        public int? TritiumActivityId { get; set; }
        [Attributes.Form_Property("Активность трития, Бк")]
        public virtual RamAccess<string> TritiumActivity//SUMMARIZABLE
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(TritiumActivity));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(TritiumActivity), value);
                }
                OnPropertyChanged(nameof(TritiumActivity));
            }
        }


        private bool TritiumActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
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
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        //TritiumActivity property

        //BetaGammaActivity property
        public int? BetaGammaActivityId { get; set; }
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public virtual RamAccess<string> BetaGammaActivity//SUMMARIZABLE
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
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
        public virtual RamAccess<string> AlphaActivity//SUMMARIZABLE
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //AlphaActivity property

        //TransuraniumActivity property
        public int? TransuraniumActivityId { get; set; }
        [Attributes.Form_Property("Активность трансурановых, Бк")]
        public virtual RamAccess<string> TransuraniumActivity//SUMMARIZABLE
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(TransuraniumActivity));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(TransuraniumActivity), value);
                }
                OnPropertyChanged(nameof(TransuraniumActivity));
            }
        }


        private bool TransuraniumActivity_Validation(RamAccess<string> value)//TODO
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //TransuraniumActivity property

        //MainRadionuclids property
        public int? MainRadionuclidsId { get; set; }
        [Attributes.Form_Property("Радионуклиды")]
        public virtual RamAccess<string> MainRadionuclids
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(MainRadionuclids));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(MainRadionuclids), value);
                }
                OnPropertyChanged(nameof(MainRadionuclids));
            }
        }
        //If change this change validation

        private bool MainRadionuclids_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            foreach (var item in Spravochniks.SprRadionuclids)
            {
                if (item.Item1.Equals(value.Value))
                {
                    return true;
                }
            }
            value.AddError("Недопустимое значение");
            return false;
        }
        //MainRadionuclids property

        //Subsidy property
        public int? SubsidyId { get; set; }
        [Attributes.Form_Property("Субсидия, %")]
        public virtual RamAccess<string> Subsidy // 0<number<=100 or empty.
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(Subsidy));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(Subsidy), value);
                }
                OnPropertyChanged(nameof(Subsidy));
            }
        }


        private bool Subsidy_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            try
            {
                int tmp = Int32.Parse(value.Value);
                if (!((tmp > 0) && (tmp <= 100)))
                {
                    value.AddError("Недопустимое значение"); return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Subsidy property

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
            value.ClearErrors(); return true;}
        //FcpNumber property
    }
}
