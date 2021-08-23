using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
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
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //StoragePlaceName property
        #region StoragePlaceNameNumberInOrder 
        public string StoragePlaceName_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Наименование ПХ")]
        public RamAccess<string> StoragePlaceName
        {
            get
            {
                    var tmp = new RamAccess<string>(StoragePlaceName_Validation, StoragePlaceName_DB);
                    tmp.PropertyChanged += StoragePlaceNameValueChanged;
                    return tmp;
            }
            set
            {
                    StoragePlaceName_DB = value.Value;
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation
        
        private void StoragePlaceNameValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                StoragePlaceName_DB = ((RamAccess<string>)Value).Value;
}
}
private bool StoragePlaceName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
//            var spr = new List<string>();//here binds spr
//            if (!spr.Contains(value.Value))
//            {
//                value.AddError( "Недопустимое значение");
//return false;
//            }
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

        //private void ValueChanged(object Value, PropertyChangedEventArgs args)
        //{
        //if (args.PropertyName == "Value")
        //{
        //_DB = ((RamAccess<string>)Value).Value;
        //}
        //}
        //private bool StoragePlaceNameNote_Validation(RamAccess<string> value)//Ready
        //{
        //    value.ClearErrors(); return true;}
        ////StoragePlaceNameNote property

        //StoragePlaceCode property
        #region StoragePlaceCode
        public string StoragePlaceCode_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Код ПХ")]
        public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                    var tmp = new RamAccess<string>(StoragePlaceCode_Validation, StoragePlaceCode_DB);
                    tmp.PropertyChanged += StoragePlaceCodeValueChanged;
                    return tmp;
            }
            set
            {
                    StoragePlaceCode_DB = value.Value;
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        //if change this change validation

        private void StoragePlaceCodeValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                StoragePlaceCode_DB = ((RamAccess<string>)Value).Value;
}
}
private bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            //var spr = new List<string>();//here binds spr
            //if (!spr.Contains(value.Value))
            //{
            //    value.AddError("Недопустимое значение");
            //    return false;
            //}
            //return true;
            if (value.Value == "-") return true;
            Regex a = new Regex("^[0-9]{8}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //StoragePlaceCode property
        #endregion

        //PackName property
        #region PackName
        public string PackName_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Наименование упаковки")]
        public RamAccess<string> PackName
        {
            get
            {
                    var tmp = new RamAccess<string>(PackName_Validation, PackName_DB);//OK
                    tmp.PropertyChanged += PackNameValueChanged;
                    return tmp;
            }
            set
            {
                    PackName_DB = value.Value;
                OnPropertyChanged(nameof(PackName));
            }
        }

        
        private void PackNameValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                PackName_DB = ((RamAccess<string>)Value).Value;
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
return true;
            }
//            var spr = new List<string>(); //here binds spr
//            if (!spr.Contains(value.Value))
//            {
//                value.AddError( "Недопустимое значение");
//return false;
//            }
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

        
        //private void ValueChanged(object Value, PropertyChangedEventArgs args)
//{
//if (args.PropertyName == "Value")
//{
//_DB = ((RamAccess<string>)Value).Value;
//}
//}
//private bool PackNameNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////PackNameNote property

        //PackType property
#region NumberInOrder 
public string PackType_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Тип упаковки")]
        public RamAccess<string> PackType
        {
            get
            {
                var tmp = new RamAccess<string>(PackType_Validation, PackType_DB);//OK
                tmp.PropertyChanged += PackTypeValueChanged;
                return tmp;
            }
            set
            {
                    PackType_DB = value.Value;
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation

        private void PackTypeValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                PackType_DB = ((RamAccess<string>)Value).Value;
}
}
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


        //private void ValueChanged(object Value, PropertyChangedEventArgs args)
        //{
        //if (args.PropertyName == "Value")
        //{
        //_DB = ((RamAccess<string>)Value).Value;
        //}
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


        //        //private void ValueChanged(object Value, PropertyChangedEventArgs args)
        //{
        //if (args.PropertyName == "Value")
        //{
        //_DB = ((RamAccess<string>)Value).Value;
        //}
        //}
        //private bool PackTypeNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////PackTypeNote property

        //PackQuantity property
        #region PackQuantity
        public int? PackQuantity_DB { get; set; } = null; [NotMapped]
        [Attributes.Form_Property("Количество упаковок, шт.")]
        public RamAccess<int?> PackQuantity
        {
            get
            {
                    var tmp = new RamAccess<int?>(PackQuantity_Validation, PackQuantity_DB);
                    tmp.PropertyChanged += PackQuantityValueChanged;
                    return tmp;
            }
            set
            {
                    PackQuantity_DB = value.Value;
                OnPropertyChanged(nameof(PackQuantity));
            }
        }
        // positive int.

        private void PackQuantityValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                PackQuantity_DB = ((RamAccess<int?>)Value).Value;
}
}
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
        #region CodeRAO 
        public string CodeRAO_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Код РАО")]
        public RamAccess<string> CodeRAO
        {
            get
            {
                    var tmp = new RamAccess<string>(CodeRAO_Validation, CodeRAO_DB);
                    tmp.PropertyChanged += CodeRAOValueChanged;
                    return tmp;
            }
            set
            {
                    CodeRAO_DB = value.Value;
                OnPropertyChanged(nameof(CodeRAO));
            }
        }


        private void CodeRAOValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                CodeRAO_DB = ((RamAccess<string>)Value).Value;
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
        #region StatusRAO
        public string StatusRAO_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Статус РАО")]
        public RamAccess<string> StatusRAO  //1 cyfer or OKPO.
        {
            get
            {
                    var tmp = new RamAccess<string>(StatusRAO_Validation, StatusRAO_DB);
                    tmp.PropertyChanged += StatusRAOValueChanged;
                    return tmp;
            }
            set
            {
                    StatusRAO_DB = value.Value;
                OnPropertyChanged(nameof(StatusRAO));
            }
        }


        private void StatusRAOValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                StatusRAO_DB = ((RamAccess<string>)Value).Value;
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
        #region VolumeInPack
        public string VolumeInPack_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Объем с упаковкой, куб. м")]
        public RamAccess<string> VolumeInPack
        {
            get
            {
                    var tmp = new RamAccess<string>(VolumeInPack_Validation, VolumeInPack_DB);
                    tmp.PropertyChanged += VolumeInPackValueChanged;
                    return tmp;
            }
            set
            {
                    VolumeInPack_DB = value.Value;
                OnPropertyChanged(nameof(VolumeInPack));
            }
        }

        
        private void VolumeInPackValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                VolumeInPack_DB = ((RamAccess<string>)Value).Value;
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
        #region MassInPack
        public string MassInPack_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Масса с упаковкой, т")]
        public RamAccess<string> MassInPack
        {
            get
            {
                    var tmp = new RamAccess<string>(MassInPack_Validation, MassInPack_DB);
                    tmp.PropertyChanged += MassInPackValueChanged;
                    return tmp;
            }
            set
            {
                    MassInPack_DB = value.Value;
                OnPropertyChanged(nameof(MassInPack));
            }
        }

        
        private void MassInPackValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                MassInPack_DB = ((RamAccess<string>)Value).Value;
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
        #region VolumeOutOfPack 
        public string VolumeOutOfPack_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Объем без упаковки, куб. м")]
        public RamAccess<string> VolumeOutOfPack//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(VolumeOutOfPack_Validation, VolumeOutOfPack_DB);
                    tmp.PropertyChanged += VolumeOutOfPackValueChanged;
                    return tmp;
            }
            set
            {
                    VolumeOutOfPack_DB = value.Value;
                OnPropertyChanged(nameof(VolumeOutOfPack));
            }
        }

        
        private void VolumeOutOfPackValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                VolumeOutOfPack_DB = ((RamAccess<string>)Value).Value;
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
        #region MassOutOfPack
        public string MassOutOfPack_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Масса без упаковки, т")]
        public RamAccess<string> MassOutOfPack//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(MassOutOfPack_Validation, MassOutOfPack_DB);
                    tmp.PropertyChanged += MassOutOfPackValueChanged;
                    return tmp;
            }
            set
            {
                    MassOutOfPack_DB = value.Value;
                OnPropertyChanged(nameof(MassOutOfPack));
            }
        }


        private void MassOutOfPackValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                MassOutOfPack_DB = ((RamAccess<string>)Value).Value;
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
        #region QuantityOZIII_DB
        public int? QuantityOZIII_DB { get; set; } = null; [NotMapped]
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public RamAccess<int?> QuantityOZIII//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<int?>(QuantityOZIII_Validation, QuantityOZIII_DB);//OK
                    tmp.PropertyChanged += QuantityOZIIIValueChanged;
                    return tmp;
            }
            set
            {
                    QuantityOZIII_DB = value.Value;
                OnPropertyChanged(nameof(QuantityOZIII));
            }
        }
        // positive int.

        private void QuantityOZIIIValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                QuantityOZIII_DB = ((RamAccess<int?>)Value).Value;
}
}
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
        #region TritiumActivity
        public string TritiumActivity_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Активность трития, Бк")]
        public RamAccess<string> TritiumActivity//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(TritiumActivity_Validation, TritiumActivity_DB);
                    tmp.PropertyChanged += TritiumActivityValueChanged;
                    return tmp;
            }
            set
            {
                    TritiumActivity_DB = value.Value;
                OnPropertyChanged(nameof(TritiumActivity));
            }
        }


        private void TritiumActivityValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                TritiumActivity_DB = ((RamAccess<string>)Value).Value;
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
        #region BetaGammaActivity
        public string BetaGammaActivity_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public RamAccess<string> BetaGammaActivity//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(BetaGammaActivity_Validation, BetaGammaActivity_DB);
                    tmp.PropertyChanged += BetaGammaActivityValueChanged;
                    return tmp;
            }
            set
            {
                    BetaGammaActivity_DB = value.Value;
                OnPropertyChanged(nameof(BetaGammaActivity));
            }
        }


        private void BetaGammaActivityValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                BetaGammaActivity_DB = ((RamAccess<string>)Value).Value;
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
public string AlphaActivity_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public RamAccess<string> AlphaActivity//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(AlphaActivity_Validation, AlphaActivity_DB);
                    tmp.PropertyChanged += AlphaActivityValueChanged;
                    return tmp;
            }
            set
            {
                    AlphaActivity_DB = value.Value;
                OnPropertyChanged(nameof(AlphaActivity));
            }
        }


        private void AlphaActivityValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                AlphaActivity_DB = ((RamAccess<string>)Value).Value;
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
        #region TransuraniumActivity 
        public string TransuraniumActivity_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Активность трансурановых, Бк")]
        public RamAccess<string> TransuraniumActivity//SUMMARIZABLE
        {
            get
            {
                    var tmp = new RamAccess<string>(TransuraniumActivity_Validation, TransuraniumActivity_DB);
                    tmp.PropertyChanged += TransuraniumActivityValueChanged;
                    return tmp;
            }
            set
            {
                    TransuraniumActivity_DB = value.Value;
                OnPropertyChanged(nameof(TransuraniumActivity));
            }
        }


        private void TransuraniumActivityValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                TransuraniumActivity_DB = ((RamAccess<string>)Value).Value;
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
        #region MainRadionuclids 
        public string MainRadionuclids_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Радионуклиды")]
        public RamAccess<string> MainRadionuclids
        {
            get
            {
                    var tmp = new RamAccess<string>(MainRadionuclids_Validation, MainRadionuclids_DB);
                    tmp.PropertyChanged += MainRadionuclidsValueChanged;
                    return tmp;
            }
            set
            {
                    MainRadionuclids_DB = value.Value;
                OnPropertyChanged(nameof(MainRadionuclids));
            }
        }
        //If change this change validation

        private void MainRadionuclidsValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                MainRadionuclids_DB = ((RamAccess<string>)Value).Value;
}
}
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
                if (!tmp.Any())
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
        #region Subsidy
        public string Subsidy_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Субсидия, %")]
        public RamAccess<string> Subsidy // 0<number<=100 or empty.
        {
            get
            {
                    var tmp = new RamAccess<string>(Subsidy_Validation, Subsidy_DB);
                    tmp.PropertyChanged += SubsidyValueChanged;
                    return tmp;
            }
            set
            {
                    Subsidy_DB = value.Value;
                OnPropertyChanged(nameof(Subsidy));
            }
        }


        private void SubsidyValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                Subsidy_DB = ((RamAccess<string>)Value).Value;
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
        #region FcpNumber
        public string FcpNumber_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public RamAccess<string> FcpNumber
        {
            get
            {
                    var tmp = new RamAccess<string>(FcpNumber_Validation, FcpNumber_DB);
                    tmp.PropertyChanged += FcpNumberValueChanged;
                    return tmp;
            }
            set
            {
                    FcpNumber_DB = value.Value;
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

        
        private void FcpNumberValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                FcpNumber_DB = ((RamAccess<string>)Value).Value;
}
}
private bool FcpNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors(); return true;}
        //FcpNumber property
#endregion
    }
}
