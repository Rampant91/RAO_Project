using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using Spravochniki;
using System.Linq;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.2: Наличие РАО в пунктах хранения, местах сбора и/или временного хранения")]
    public class Form22 : Abstracts.Form2
    {
        public Form22() : base()
        {
            FormNum.Value = "2.2";
            //NumberOfFields.Value = 25;
            Validate_all();
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
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
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
#endregion

        ////StoragePlaceNameNote property
        //public RamAccess<string> StoragePlaceNameNote
        //{
        //    get
        //    {
                
        //        {
        //            var tmp = new RamAccess<string>(StoragePlaceNameNote_Validation, _DB);
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {

                
        //        {
        //            StoragePlaceNameNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(StoragePlaceNameNote_Validation, _DB);
        //    }
        //}
        ////If change this change validation
        
        //private bool StoragePlaceNameNote_Validation(RamAccess<string> value)//Ready
        //{
        //    value.ClearErrors(); return true;}
        ////StoragePlaceNameNote property

        //StoragePlaceCode property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Код ПХ")]
        public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                
                {
                    var tmp = new RamAccess<string>(StoragePlaceCode_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
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
#endregion

        //PackName property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Наименование упаковки")]
        public RamAccess<string> PackName
        {
            get
            {
                
                {
                    var tmp = new RamAccess<string>(PackName_Validation, _DB);//OK
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    PackName_DB = value.Value;
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
#endregion

        ////PackNameNote property
        //public RamAccess<string> PackNameNote
        //{
        //    get
        //    {
                
        //        {
        //            var tmp = new RamAccess<string>(PackNameNote_Validation, _DB);//OK
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {

                
        //        {
        //            PackNameNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(PackNameNote));
        //    }
        //}

        
        //private bool PackNameNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////PackNameNote property

        //PackType property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Тип упаковки")]
        public RamAccess<string> PackType
        {
            get
            {
                
                {
                    var tmp = new RamAccess<string>(PackType_Validation, _DB);//OK
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    PackType_DB = value.Value;
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
#endregion

        //PackTypeRecoded property
        //public RamAccess<string> PackTypeRecoded
        //{
        //    get
        //    {
                
        //        {
        //            var tmp = new RamAccess<string>(PackTypeRecoded_Validation, _DB);//OK
        //            tmp.PropertyChanged += ValueChanged;
        //            return tmp;
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {

                
        //        {
        //            PackTypeRecoded_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(PackTypeRecoded));
        //    }
        //}

        
        //private bool PackTypeRecoded_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        //PackTypeRecoded property

        ////PackTypeNote property
        //public RamAccess<string> PackTypeNote
        //{
        //    get
        //    {
                
        //        {
        //            var tmp = new RamAccess<string>(PackTypeNote_Validation, _DB);//OK
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {

                
        //        {
        //            PackTypeNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(PackTypeNote));
        //    }
        //}

        
        //private bool PackTypeNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////PackTypeNote property

        //PackQuantity property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Количество упаковок, шт.")]
        public RamAccess<int?> PackQuantity
        {
            get
            {
                
                {
                    var tmp = new RamAccess<int?>(PackQuantity_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    PackQuantity_DB = value.Value;
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
#endregion

            //CodeRAO property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Код РАО")]
        public RamAccess<string> CodeRAO
        {
            get
            {
                
                {
                    var tmp = new RamAccess<string>(CodeRAO_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    CodeRAO_DB = value.Value;
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
#endregion

        //StatusRAO property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Статус РАО")]
        public RamAccess<string> StatusRAO  //1 cyfer or OKPO.
        {
            get
            {
                
                {
                    var tmp = new RamAccess<string>(StatusRAO_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    StatusRAO_DB = value.Value;
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
#endregion

        //VolumeInPack property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Объем с упаковкой, куб. м")]
        public RamAccess<string> VolumeInPack
        {
            get
            {
                
                {
                    var tmp = new RamAccess<string>(VolumeInPack_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    VolumeInPack_DB = value.Value;
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
#endregion

        //MassInPack Property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Масса с упаковкой, т")]
        public RamAccess<string> MassInPack
        {
            get
            {
                
                {
                    var tmp = new RamAccess<string>(MassInPack_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    MassInPack_DB = value.Value;
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
        #endregion

        //VolumeOutOfPack property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Объем без упаковки, куб. м")]
        public RamAccess<string> VolumeOutOfPack//SUMMARIZABLE
        {
            get
            {
                
                {
                    var tmp = new RamAccess<string>(VolumeOutOfPack_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    VolumeOutOfPack_DB = value.Value;
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
#endregion

        //MassOutOfPack Property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Масса без упаковки, т")]
        public RamAccess<string> MassOutOfPack//SUMMARIZABLE
        {
            get
            {
                
                {
                    var tmp = new RamAccess<string>(MassOutOfPack_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    MassOutOfPack_DB = value.Value;
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
        #endregion

        //QuantityOZIII property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public RamAccess<int?> QuantityOZIII//SUMMARIZABLE
        {
            get
            {
                
                {
                    var tmp = new RamAccess<int?>(QuantityOZIII_Validation, _DB);//OK
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    QuantityOZIII_DB = value.Value;
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
#endregion

        //TritiumActivity property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Активность трития, Бк")]
        public RamAccess<string> TritiumActivity//SUMMARIZABLE
        {
            get
            {
                
                {
                    var tmp = new RamAccess<string>(TritiumActivity_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    TritiumActivity_DB = value.Value;
                }
                OnPropertyChanged(nameof(TritiumActivity));
            }
        }


        private bool TritiumActivity_Validation(RamAccess<string> value)//TODO
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
#endregion

        //BetaGammaActivity property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public RamAccess<string> BetaGammaActivity//SUMMARIZABLE
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
            value.ClearErrors();
            if (value.Value == "-")
            {
                return true;
            }
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
#endregion

        //AlphaActivity property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public RamAccess<string> AlphaActivity//SUMMARIZABLE
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
            if (value.Value == "-")
            {
                return true;
            }
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
#endregion

        //TransuraniumActivity property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Активность трансурановых, Бк")]
        public RamAccess<string> TransuraniumActivity//SUMMARIZABLE
        {
            get
            {
                
                {
                    var tmp = new RamAccess<string>(TransuraniumActivity_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    TransuraniumActivity_DB = value.Value;
                }
                OnPropertyChanged(nameof(TransuraniumActivity));
            }
        }


        private bool TransuraniumActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == "-")
            {
                return true;
            }
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
#endregion

        //MainRadionuclids property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Радионуклиды")]
        public RamAccess<string> MainRadionuclids
        {
            get
            {
                
                {
                    var tmp = new RamAccess<string>(MainRadionuclids_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    MainRadionuclids_DB = value.Value;
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
                value.AddError("Поле не заполнено");
                return false;
            }
            string[] nuclids = value.Value.Split("; ");
            bool flag = true;
            foreach (var nucl in nuclids)
            {
                var tmp = from item in Spravochniks.SprRadionuclids where nucl == item.Item1 select item.Item1;
                if (tmp.Count() == 0)
                    flag = false;
            }
            if (!flag)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //MainRadionuclids property
#endregion

        //Subsidy property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Субсидия, %")]
        public RamAccess<string> Subsidy // 0<number<=100 or empty.
        {
            get
            {
                
                {
                    var tmp = new RamAccess<string>(Subsidy_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    Subsidy_DB = value.Value;
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
#endregion

        //FcpNumber property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
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
            value.ClearErrors(); return true;}
        //FcpNumber property
#endregion
    }
}
