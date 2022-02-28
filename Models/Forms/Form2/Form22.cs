using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using Spravochniki;
using System.Linq;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using Models.Collections;
using Models.Interfaces;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.2: Наличие РАО в пунктах хранения, местах сбора и/или временного хранения")]
    public class Form22 : Abstracts.Form2, IBaseColor
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

        [Attributes.Form_Property(true,"Форма")]
        public override bool Object_Validation()
        {
            return !(StoragePlaceName.HasErrors ||
                     StoragePlaceCode.HasErrors ||
                     PackName.HasErrors ||
                     PackType.HasErrors ||
                     CodeRAO.HasErrors ||
                     StatusRAO.HasErrors ||
                     VolumeOutOfPack.HasErrors ||
                     MassInPack.HasErrors ||
                     QuantityOZIII.HasErrors ||
                     TritiumActivity.HasErrors ||
                     BetaGammaActivity.HasErrors ||
                     TransuraniumActivity.HasErrors ||
                     AlphaActivity.HasErrors ||
                     VolumeInPack.HasErrors ||
                     MassOutOfPack.HasErrors ||
                     MainRadionuclids.HasErrors ||
                     Subsidy.HasErrors ||
                     FcpNumber.HasErrors ||
                     PackQuantity.HasErrors);
        }

        #region BaseColor
        [NotMapped]
        public ColorType _BaseColor { get; set; } = ColorType.None;
        [NotMapped]
        public ColorType BaseColor
        {

            get => _BaseColor;
            set
            {
                if (_BaseColor != value)
                {
                    _BaseColor = value;
                    OnPropertyChanged(nameof(BaseColor));
                }
            }
        }
        #endregion

        #region Sum

        public bool Sum_DB { get; set; } = false;

        [NotMapped]
        public RamAccess<bool> Sum
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(Sum)))
                {
                    ((RamAccess<bool>)Dictionary[nameof(Sum)]).Value = Sum_DB;
                    return (RamAccess<bool>)Dictionary[nameof(Sum)];
                }
                else
                {
                    var rm = new RamAccess<bool>(Sum_Validation, Sum_DB);
                    rm.PropertyChanged += SumValueChanged;
                    Dictionary.Add(nameof(Sum), rm);
                    return (RamAccess<bool>)Dictionary[nameof(Sum)];
                }
            }
            set
            {
                Sum_DB = value.Value;
                OnPropertyChanged(nameof(Sum));
            }
        }

        private void SumValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Sum_DB = ((RamAccess<bool>)Value).Value;
            }
        }

        private bool Sum_Validation(RamAccess<bool> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region StoragePlaceName

        public string StoragePlaceName_DB { get; set; } = "";
        public bool StoragePlaceName_Hidden_Priv { get; set; } = false;

        [NotMapped]
        public bool StoragePlaceName_Hidden
        {
            get => StoragePlaceName_Hidden_Priv;
            set { StoragePlaceName_Hidden_Priv = value; }
        }
        public bool StoragePlaceName_Hidden_Priv2 { get; set; } = false;

        [NotMapped]
        public bool StoragePlaceName_Hidden2
        {
            get => StoragePlaceName_Hidden_Priv2;
            set { StoragePlaceName_Hidden_Priv2 = value; }
        }

        [NotMapped]
        [Attributes.Form_Property(true,"Пункт хранения","наименование","2")]
        public RamAccess<string> StoragePlaceName
        {
            get
            {
                if (!StoragePlaceName_Hidden || StoragePlaceName_Hidden2)
                {
                    if (Dictionary.ContainsKey(nameof(StoragePlaceName)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(StoragePlaceName)]).Value = StoragePlaceName_DB;
                        return (RamAccess<string>)Dictionary[nameof(StoragePlaceName)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(StoragePlaceName_Validation, StoragePlaceName_DB);
                        rm.PropertyChanged += StoragePlaceNameValueChanged;
                        Dictionary.Add(nameof(StoragePlaceName), rm);
                        return (RamAccess<string>)Dictionary[nameof(StoragePlaceName)];
                    }
                }
                else
                {
                    if (Dictionary.ContainsKey(nameof(StoragePlaceName)))
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary[nameof(StoragePlaceName)] = rm;
                        return (RamAccess<string>)Dictionary[nameof(StoragePlaceName)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary.Add(nameof(StoragePlaceName), rm);
                        return (RamAccess<string>)Dictionary[nameof(StoragePlaceName)];
                    }
                }
            }
            set
            {
                if (!StoragePlaceName_Hidden)
                {
                    StoragePlaceName_DB = value.Value;
                    OnPropertyChanged(nameof(StoragePlaceName));
                }
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

        private bool StoragePlaceName_Validation(RamAccess<string> value) //Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
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

        #region StoragePlaceCode

        public string StoragePlaceCode_DB { get; set; } = "";
        public bool StoragePlaceCode_Hidden_Priv { get; set; } = false;

        [NotMapped]
        public bool StoragePlaceCode_Hidden
        {
            get => StoragePlaceCode_Hidden_Priv;
            set { StoragePlaceCode_Hidden_Priv = value; }
        }
        public bool StoragePlaceCode_Hidden_Priv2 { get; set; } = false;

        [NotMapped]
        public bool StoragePlaceCode_Hidden2
        {
            get => StoragePlaceCode_Hidden_Priv2;
            set { StoragePlaceCode_Hidden_Priv2 = value; }
        }

        [NotMapped]
        [Attributes.Form_Property(true,"Пункт хранения","код","3")]
        public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                if (!StoragePlaceCode_Hidden || StoragePlaceName_Hidden2)
                {
                    if (Dictionary.ContainsKey(nameof(StoragePlaceCode)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(StoragePlaceCode)]).Value = StoragePlaceCode_DB;
                        return (RamAccess<string>)Dictionary[nameof(StoragePlaceCode)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(StoragePlaceCode_Validation, StoragePlaceCode_DB);
                        rm.PropertyChanged += StoragePlaceCodeValueChanged;
                        Dictionary.Add(nameof(StoragePlaceCode), rm);
                        return (RamAccess<string>)Dictionary[nameof(StoragePlaceCode)];
                    }
                }
                else
                {
                    if (Dictionary.ContainsKey(nameof(StoragePlaceCode)))
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary[nameof(StoragePlaceCode)] = rm;
                        return (RamAccess<string>)Dictionary[nameof(StoragePlaceCode)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary.Add(nameof(StoragePlaceCode), rm);
                        return (RamAccess<string>)Dictionary[nameof(StoragePlaceCode)];
                    }
                }
            }
            set
            {
                if (!StoragePlaceCode_Hidden)
                {
                    StoragePlaceCode_DB = value.Value;
                    OnPropertyChanged(nameof(StoragePlaceCode));
                }
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

        private bool StoragePlaceCode_Validation(RamAccess<string> value) //TODO
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
                value.AddError("Недопустимое значение");
                return false;
            }

            return true;
        }

        //StoragePlaceCode property

        #endregion

        #region PackName

        public string PackName_DB { get; set; } = "";
        public bool PackName_Hidden_Priv { get; set; } = false;

        [NotMapped]
        public bool PackName_Hidden
        {
            get => PackName_Hidden_Priv;
            set { PackName_Hidden_Priv = value; }
        }
        public bool PackName_Hidden_Priv2 { get; set; } = false;

        [NotMapped]
        public bool PackName_Hidden2
        {
            get => PackName_Hidden_Priv2;
            set { PackName_Hidden_Priv2 = value; }
        }

        [NotMapped]
        [Attributes.Form_Property(true,"УКТ, упаковка ли иная учетная единица","наименование","4")]
        public RamAccess<string> PackName
        {
            get
            {
                if (!PackName_Hidden || PackName_Hidden2)
                {
                    if (Dictionary.ContainsKey(nameof(PackName)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(PackName)]).Value = PackName_DB;
                        return (RamAccess<string>)Dictionary[nameof(PackName)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(PackName_Validation, PackName_DB);
                        rm.PropertyChanged += PackNameValueChanged;
                        Dictionary.Add(nameof(PackName), rm);
                        return (RamAccess<string>)Dictionary[nameof(PackName)];
                    }
                }
                else
                {
                    if (Dictionary.ContainsKey(nameof(PackName)))
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary[nameof(PackName)] = rm;
                        return (RamAccess<string>)Dictionary[nameof(PackName)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary.Add(nameof(PackName), rm);
                        return (RamAccess<string>)Dictionary[nameof(PackName)];
                    }
                }
            }
            set
            {
                if (!PackName_Hidden)
                {
                    PackName_DB = value.Value;
                    OnPropertyChanged(nameof(PackName));
                }
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
                value.AddError("Поле не заполнено");
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

        #region PackType

        public string PackType_DB { get; set; } = "";
        public bool PackType_Hidden_Priv { get; set; } = false;

        [NotMapped]
        public bool PackType_Hidden
        {
            get => PackType_Hidden_Priv;
            set { PackType_Hidden_Priv = value; }
        }
        public bool PackType_Hidden_Priv2 { get; set; } = false;

        [NotMapped]
        public bool PackType_Hidden2
        {
            get => PackType_Hidden_Priv2;
            set { PackType_Hidden_Priv2 = value; }
        }

        [NotMapped]
        [Attributes.Form_Property(true,"УКТ, упаковка ли иная учетная единица","тип","5")]
        public RamAccess<string> PackType
        {
            get
            {
                if (!PackType_Hidden || PackType_Hidden2)
                {
                    if (Dictionary.ContainsKey(nameof(PackType)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(PackType)]).Value = PackType_DB;
                        return (RamAccess<string>)Dictionary[nameof(PackType)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(PackType_Validation, PackType_DB);
                        rm.PropertyChanged += PackTypeValueChanged;
                        Dictionary.Add(nameof(PackType), rm);
                        return (RamAccess<string>)Dictionary[nameof(PackType)];
                    }
                }
                else
                {
                    if (Dictionary.ContainsKey(nameof(PackType)))
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary[nameof(PackType)] = rm;
                        return (RamAccess<string>)Dictionary[nameof(PackType)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary.Add(nameof(PackType), rm);
                        return (RamAccess<string>)Dictionary[nameof(PackType)];
                    }
                }
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

        private bool PackType_Validation(RamAccess<string> value) //Ready
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

            //var spr = new List<string>();
            //if (!spr.Contains(value.Value))
            //{
            //    value.AddError("Недопустимое значение");
            //    return false;
            //}

            return true;
        }

        //PackType property

        #endregion

        #region PackQuantity

        public string PackQuantity_DB { get; set; } = null;

        [NotMapped]
        [Attributes.Form_Property(true,"УКТ, упаковка ли иная учетная единица","количество, шт.","6")]
        public RamAccess<string> PackQuantity
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PackQuantity)))
                {
                    ((RamAccess<string>)Dictionary[nameof(PackQuantity)]).Value = PackQuantity_DB;
                    return (RamAccess<string>)Dictionary[nameof(PackQuantity)];
                }
                else
                {
                    var rm = new RamAccess<string>(PackQuantity_Validation, PackQuantity_DB);
                    rm.PropertyChanged += PackQuantityValueChanged;
                    Dictionary.Add(nameof(PackQuantity), rm);
                    return (RamAccess<string>)Dictionary[nameof(PackQuantity)];
                }
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
                PackQuantity_DB = ((RamAccess<string>)Value).Value;
            }
        }

        private bool PackQuantity_Validation(RamAccess<string> value) //Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if(value.Value == "-")
            {
                return true;
            }
            try
            {
                if (int.Parse(value.Value) <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }

        //PackQuantity property

        #endregion

        #region CodeRAO

        public string CodeRAO_DB { get; set; } = "";
        public bool CodeRAO_Hidden_Priv { get; set; } = false;

        [NotMapped]
        public bool CodeRAO_Hidden
        {
            get => CodeRAO_Hidden_Priv;
            set { CodeRAO_Hidden_Priv = value; }
        }

        [NotMapped]
        [Attributes.Form_Property(true,"null-7","Код РАО","7")]
        public RamAccess<string> CodeRAO
        {
            get
            {
                if (!CodeRAO_Hidden)
                {
                    if (Dictionary.ContainsKey(nameof(CodeRAO)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(CodeRAO)]).Value = CodeRAO_DB;
                        return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(CodeRAO_Validation, CodeRAO_DB);
                        rm.PropertyChanged += CodeRAOValueChanged;
                        Dictionary.Add(nameof(CodeRAO), rm);
                        return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
                    }
                }
                else
                {
                    if (Dictionary.ContainsKey(nameof(CodeRAO)))
                    {
                        return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary.Add(nameof(CodeRAO), rm);
                        return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
                    }
                }
            }

            set
            {
                if (!CodeRAO_Hidden)
                {
                    CodeRAO_DB = value.Value;
                    OnPropertyChanged(nameof(CodeRAO));
                }
            }
        }
        private void CodeRAOValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value.ToLower();
                tmp = tmp.Replace("х", "x");
                CodeRAO_DB = tmp;
            }
        }
        private bool CodeRAO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value.ToLower();
            tmp = tmp.Replace("х", "x");
            Regex a = new Regex("^[0-9x+]{11}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //CodeRAO property
        #endregion

        #region StatusRAO
        public string StatusRAO_DB { get; set; } = "";
        public bool StatusRAO_Hidden_Priv { get; set; } = false;

        [NotMapped]
        public bool StatusRAO_Hidden
        {
            get => StatusRAO_Hidden_Priv;
            set { StatusRAO_Hidden_Priv = value; }
        }

        [NotMapped]
        [Attributes.Form_Property(true,"null-8","Статус РАО","8")]
        public RamAccess<string> StatusRAO  //1 cyfer or OKPO.
        {
            get
            {
                if (!StatusRAO_Hidden)
                {
                    if (Dictionary.ContainsKey(nameof(StatusRAO)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(StatusRAO)]).Value = StatusRAO_DB;
                        return (RamAccess<string>)Dictionary[nameof(StatusRAO)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(StatusRAO_Validation, StatusRAO_DB);
                        rm.PropertyChanged += StatusRAOValueChanged;
                        Dictionary.Add(nameof(StatusRAO), rm);
                        return (RamAccess<string>)Dictionary[nameof(StatusRAO)];
                    }
                }
                else
                {
                    if (Dictionary.ContainsKey(nameof(StatusRAO)))
                    {
                        return (RamAccess<string>)Dictionary[nameof(StatusRAO)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary.Add(nameof(StatusRAO), rm);
                        return (RamAccess<string>)Dictionary[nameof(StatusRAO)];
                    }
                }
            }
            set
            {
                if (!StatusRAO_Hidden)
                {
                    StatusRAO_DB = value.Value;
                    OnPropertyChanged(nameof(StatusRAO));
                }
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

        #region VolumeInPack
        public string VolumeInPack_DB { get; set; } = "";
        public bool VolumeInPack_Hidden_Priv { get; set; } = false;

        [NotMapped]
        public bool VolumeInPack_Hidden
        {
            get => VolumeInPack_Hidden_Priv;
            set { VolumeInPack_Hidden_Priv = value; }
        }
        public bool VolumeInPack_Hidden_Priv2 { get; set; } = false;

        [NotMapped]
        public bool VolumeInPack_Hidden2
        {
            get => VolumeInPack_Hidden_Priv2;
            set { VolumeInPack_Hidden_Priv2 = value; }
        }
        [NotMapped]
        [Attributes.Form_Property(true,"Объем, куб. м","РАО с упаковкой","10")]
        public RamAccess<string> VolumeInPack
        {
            get
            {
                if (!VolumeInPack_Hidden || VolumeInPack_Hidden2)
                {
                    if (Dictionary.ContainsKey(nameof(VolumeInPack)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(VolumeInPack)]).Value = VolumeInPack_DB;
                        return (RamAccess<string>)Dictionary[nameof(VolumeInPack)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(VolumeInPack_Validation, VolumeInPack_DB);
                        rm.PropertyChanged += VolumeInPackValueChanged;
                        Dictionary.Add(nameof(VolumeInPack), rm);
                        return (RamAccess<string>)Dictionary[nameof(VolumeInPack)];
                    }
                }
                else
                {
                    if (Dictionary.ContainsKey(nameof(VolumeInPack)))
                    {
                        return (RamAccess<string>)Dictionary[nameof(VolumeInPack)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary.Add(nameof(VolumeInPack), rm);
                        return (RamAccess<string>)Dictionary[nameof(VolumeInPack)];
                    }
                }
            }
            set
            {
                if (!VolumeInPack_Hidden)
                {
                    VolumeInPack_DB = value.Value;
                    OnPropertyChanged(nameof(VolumeInPack));
                }
            }
        }


        private void VolumeInPackValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        VolumeInPack_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                VolumeInPack_DB = value1;
            }
        }
        private bool VolumeInPack_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                return true;
            }
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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
        //VolumeInPack property
        #endregion

        #region MassInPack
        public string MassInPack_DB { get; set; } = "";
        public bool MassInPack_Hidden_Priv { get; set; } = false;

        [NotMapped]
        public bool MassInPack_Hidden
        {
            get => MassInPack_Hidden_Priv;
            set { MassInPack_Hidden_Priv = value; }
        }
        public bool MassInPack_Hidden_Priv2 { get; set; } = false;

        [NotMapped]
        public bool MassInPack_Hidden2
        {
            get => MassInPack_Hidden_Priv2;
            set { MassInPack_Hidden_Priv2 = value; }
        }

        [NotMapped]
        [Attributes.Form_Property(true,"Масса, т","РАО с упаковкой (брутто)","12")]
        public RamAccess<string> MassInPack
        {
            get
            {
                if (!MassInPack_Hidden || MassInPack_Hidden2)
                {
                    if (Dictionary.ContainsKey(nameof(MassInPack)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(MassInPack)]).Value = MassInPack_DB;
                        return (RamAccess<string>)Dictionary[nameof(MassInPack)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(MassInPack_Validation, MassInPack_DB);
                        rm.PropertyChanged += MassInPackValueChanged;
                        Dictionary.Add(nameof(MassInPack), rm);
                        return (RamAccess<string>)Dictionary[nameof(MassInPack)];
                    }
                }
                else
                {
                    if (Dictionary.ContainsKey(nameof(MassInPack)))
                    {
                        return (RamAccess<string>)Dictionary[nameof(MassInPack)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary.Add(nameof(MassInPack), rm);
                        return (RamAccess<string>)Dictionary[nameof(MassInPack)];
                    }
                }
            }
            set
            {
                if (!MassInPack_Hidden)
                {
                    MassInPack_DB = value.Value;
                    OnPropertyChanged(nameof(MassInPack));
                }
            }
        }


        private void MassInPackValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        MassInPack_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                MassInPack_DB = value1;
            }
        }
        private bool MassInPack_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                return true;
            }
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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
        //MassInPack Property
        #endregion

        #region VolumeOutOfPack 
        public string VolumeOutOfPack_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true,"Объем, куб. м","РАО без упаковки","9")]
        public RamAccess<string> VolumeOutOfPack//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(VolumeOutOfPack)))
                {
                    ((RamAccess<string>)Dictionary[nameof(VolumeOutOfPack)]).Value = VolumeOutOfPack_DB;
                    return (RamAccess<string>)Dictionary[nameof(VolumeOutOfPack)];
                }
                else
                {
                    var rm = new RamAccess<string>(VolumeOutOfPack_Validation, VolumeOutOfPack_DB);
                    rm.PropertyChanged += VolumeOutOfPackValueChanged;
                    Dictionary.Add(nameof(VolumeOutOfPack), rm);
                    return (RamAccess<string>)Dictionary[nameof(VolumeOutOfPack)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        VolumeOutOfPack_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                VolumeOutOfPack_DB = value1;
            }
        }
        private bool VolumeOutOfPack_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("-"))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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
        //VolumeOutOfPack property
        #endregion

        #region MassOutOfPack
        public string MassOutOfPack_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true,"Масса, т","РАО без упаковки (нетто)","11")]
        public RamAccess<string> MassOutOfPack//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(MassOutOfPack)))
                {
                    ((RamAccess<string>)Dictionary[nameof(MassOutOfPack)]).Value = MassOutOfPack_DB;
                    return (RamAccess<string>)Dictionary[nameof(MassOutOfPack)];
                }
                else
                {
                    var rm = new RamAccess<string>(MassOutOfPack_Validation, MassOutOfPack_DB);
                    rm.PropertyChanged += MassOutOfPackValueChanged;
                    Dictionary.Add(nameof(MassOutOfPack), rm);
                    return (RamAccess<string>)Dictionary[nameof(MassOutOfPack)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        MassOutOfPack_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                MassOutOfPack_DB = value1;
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
            if (value.Value.Equals("-"))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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

        #region QuantityOZIII_DB
        public string QuantityOZIII_DB { get; set; } = null;[NotMapped]
        [Attributes.Form_Property(true,"null-13","Количество ОЗИИИ, шт.","13")]
        public RamAccess<string> QuantityOZIII//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(QuantityOZIII)))
                {
                    ((RamAccess<string>)Dictionary[nameof(QuantityOZIII)]).Value = QuantityOZIII_DB;
                    return (RamAccess<string>)Dictionary[nameof(QuantityOZIII)];
                }
                else
                {
                    var rm = new RamAccess<string>(QuantityOZIII_Validation, QuantityOZIII_DB);
                    rm.PropertyChanged += QuantityOZIIIValueChanged;
                    Dictionary.Add(nameof(QuantityOZIII), rm);
                    return (RamAccess<string>)Dictionary[nameof(QuantityOZIII)];
                }
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
                QuantityOZIII_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool QuantityOZIII_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                if (int.Parse(value.Value) <= 0)
                {
                    value.AddError("Число должно быть больше нуля");
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
        //QuantityOZIII property
        #endregion

        #region TritiumActivity
        public string TritiumActivity_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true,"Суммарная активность, Бк","тритий","14")]
        public RamAccess<string> TritiumActivity//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(TritiumActivity)))
                {
                    ((RamAccess<string>)Dictionary[nameof(TritiumActivity)]).Value = TritiumActivity_DB;
                    return (RamAccess<string>)Dictionary[nameof(TritiumActivity)];
                }
                else
                {
                    var rm = new RamAccess<string>(TritiumActivity_Validation, TritiumActivity_DB);
                    rm.PropertyChanged += TritiumActivityValueChanged;
                    Dictionary.Add(nameof(TritiumActivity), rm);
                    return (RamAccess<string>)Dictionary[nameof(TritiumActivity)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        TritiumActivity_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                TritiumActivity_DB = value1;
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
            string tmp = value.Value;
            tmp = tmp.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!tmp.Contains('e')) && (tmp.Contains('+') ^ tmp.Contains('-')))
            {
                tmp = tmp.Replace("+", "e+").Replace("-", "e-");
            }
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

        #region BetaGammaActivity
        public string BetaGammaActivity_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true,"Суммарная активность, Бк","бета-, гамма-излучающие радионуклиды (исключая тритий)","15")]
        public RamAccess<string> BetaGammaActivity//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(BetaGammaActivity)))
                {
                    ((RamAccess<string>)Dictionary[nameof(BetaGammaActivity)]).Value = BetaGammaActivity_DB;
                    return (RamAccess<string>)Dictionary[nameof(BetaGammaActivity)];
                }
                else
                {
                    var rm = new RamAccess<string>(BetaGammaActivity_Validation, BetaGammaActivity_DB);
                    rm.PropertyChanged += BetaGammaActivityValueChanged;
                    Dictionary.Add(nameof(BetaGammaActivity), rm);
                    return (RamAccess<string>)Dictionary[nameof(BetaGammaActivity)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        BetaGammaActivity_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                BetaGammaActivity_DB = value1;
            }
        }
        private bool BetaGammaActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (value.Value == "-")
            {
                return true;
            }
            string tmp = value1;
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

        #region AlphaActivity 
        public string AlphaActivity_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true,"Суммарная активность, Бк","альфа-излучающие радионуклиды (исключая трансурановые)","16")]
        public RamAccess<string> AlphaActivity//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(AlphaActivity)))
                {
                    ((RamAccess<string>)Dictionary[nameof(AlphaActivity)]).Value = AlphaActivity_DB;
                    return (RamAccess<string>)Dictionary[nameof(AlphaActivity)];
                }
                else
                {
                    var rm = new RamAccess<string>(AlphaActivity_Validation, AlphaActivity_DB);
                    rm.PropertyChanged += AlphaActivityValueChanged;
                    Dictionary.Add(nameof(AlphaActivity), rm);
                    return (RamAccess<string>)Dictionary[nameof(AlphaActivity)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        AlphaActivity_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                AlphaActivity_DB = value1;
            }
        }
        private bool AlphaActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            if (value.Value == "-")
            {
                return true;
            }
            string tmp = value1;
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
        #endregion

        #region TransuraniumActivity 
        public string TransuraniumActivity_DB { get; set; } = "";[NotMapped]
        [Attributes.Form_Property(true,"Суммарная активность, Бк","трансурановые радионуклиды","17")]
        public RamAccess<string> TransuraniumActivity//SUMMARIZABLE
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(TransuraniumActivity)))
                {
                    ((RamAccess<string>)Dictionary[nameof(TransuraniumActivity)]).Value = TransuraniumActivity_DB;
                    return (RamAccess<string>)Dictionary[nameof(TransuraniumActivity)];
                }
                else
                {
                    var rm = new RamAccess<string>(TransuraniumActivity_Validation, TransuraniumActivity_DB);
                    rm.PropertyChanged += TransuraniumActivityValueChanged;
                    Dictionary.Add(nameof(TransuraniumActivity), rm);
                    return (RamAccess<string>)Dictionary[nameof(TransuraniumActivity)];
                }
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
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        TransuraniumActivity_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                TransuraniumActivity_DB = value1;
            }
        }
        private bool TransuraniumActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value == "-")
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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

        #region MainRadionuclids 
        public string MainRadionuclids_DB { get; set; } = "";
        public bool MainRadionuclids_Hidden_Priv { get; set; } = false;

        [NotMapped]
        public bool MainRadionuclids_Hidden
        {
            get => MainRadionuclids_Hidden_Priv;
            set { MainRadionuclids_Hidden_Priv = value; }
        }

        [NotMapped]
        [Attributes.Form_Property(true,"null-18","Основные радионуклиды","18")]
        public RamAccess<string> MainRadionuclids
        {
            get
            {
                if (!MainRadionuclids_Hidden)
                {
                    if (Dictionary.ContainsKey(nameof(MainRadionuclids)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(MainRadionuclids)]).Value = MainRadionuclids_DB;
                        return (RamAccess<string>)Dictionary[nameof(MainRadionuclids)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(MainRadionuclids_Validation, MainRadionuclids_DB);
                        rm.PropertyChanged += MainRadionuclidsValueChanged;
                        Dictionary.Add(nameof(MainRadionuclids), rm);
                        return (RamAccess<string>)Dictionary[nameof(MainRadionuclids)];
                    }
                }
                else
                {
                    if (Dictionary.ContainsKey(nameof(MainRadionuclids)))
                    {
                        return (RamAccess<string>)Dictionary[nameof(MainRadionuclids)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary.Add(nameof(MainRadionuclids), rm);
                        return (RamAccess<string>)Dictionary[nameof(MainRadionuclids)];
                    }
                }
            }
            set
            {
                if (!MainRadionuclids_Hidden)
                {
                    MainRadionuclids_DB = value.Value;
                    OnPropertyChanged(nameof(MainRadionuclids));
                }
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
            string[] nuclids = value.Value.Split(";");
            for (int k = 0; k < nuclids.Length; k++)
            {
                nuclids[k] = nuclids[k].ToLower().Replace(" ", "");
            }
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

        #region Subsidy
        public string Subsidy_DB { get; set; } = "";
        public bool Subsidy_Hidden_Priv { get; set; } = false;

        [NotMapped]
        public bool Subsidy_Hidden
        {
            get => Subsidy_Hidden_Priv;
            set { Subsidy_Hidden_Priv = value; }
        }

        [NotMapped]
        [Attributes.Form_Property(true,"null-19","Субсидия, %","19")]
        public RamAccess<string> Subsidy
        {
            get
            {
                if (!Subsidy_Hidden)
                {
                    if (Dictionary.ContainsKey(nameof(Subsidy)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(Subsidy)]).Value = Subsidy_DB;
                        return (RamAccess<string>)Dictionary[nameof(Subsidy)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(Subsidy_Validation, Subsidy_DB);
                        rm.PropertyChanged += SubsidyValueChanged;
                        Dictionary.Add(nameof(Subsidy), rm);
                        return (RamAccess<string>)Dictionary[nameof(Subsidy)];
                    }
                }
                else
                {
                    if (Dictionary.ContainsKey(nameof(Subsidy)))
                    {
                        return (RamAccess<string>)Dictionary[nameof(Subsidy)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary.Add(nameof(Subsidy), rm);
                        return (RamAccess<string>)Dictionary[nameof(Subsidy)];
                    }
                }
            }
            set
            {
                if (!Subsidy_Hidden)
                {
                    Subsidy_DB = value.Value;
                    OnPropertyChanged(nameof(Subsidy));
                }
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

        #region FcpNumber
        public string FcpNumber_DB { get; set; } = "";
        public bool FcpNumber_Hidden_Priv { get; set; } = false;

        [NotMapped]
        public bool FcpNumber_Hidden
        {
            get => FcpNumber_Hidden_Priv;
            set { FcpNumber_Hidden_Priv = value; }
        }

        [NotMapped]
        [Attributes.Form_Property(true,"null-20","Номер мероприятия ФЦП","20")]
        public RamAccess<string> FcpNumber
        {
            get
            {
                if (!FcpNumber_Hidden)
                {
                    if (Dictionary.ContainsKey(nameof(FcpNumber)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(FcpNumber)]).Value = FcpNumber_DB;
                        return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(FcpNumber_Validation, FcpNumber_DB);
                        rm.PropertyChanged += FcpNumberValueChanged;
                        Dictionary.Add(nameof(FcpNumber), rm);
                        return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
                    }
                }
                else
                {
                    if (Dictionary.ContainsKey(nameof(FcpNumber)))
                    {
                        return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(null, null);
                        Dictionary.Add(nameof(FcpNumber), rm);
                        return (RamAccess<string>)Dictionary[nameof(FcpNumber)];
                    }
                }
            }
            set
            {
                if (!FcpNumber_Hidden)
                {
                    FcpNumber_DB = value.Value;
                    OnPropertyChanged(nameof(FcpNumber));
                }
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
            value.ClearErrors(); return true;
        }
        //FcpNumber property
        #endregion

        #region IExcel
        public int ExcelRow(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true)
        {
            var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = StoragePlaceName.Value;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = StoragePlaceCode.Value;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = PackName.Value;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = PackType.Value;
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = PackQuantity_DB;
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = CodeRAO_DB;
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = StatusRAO_DB;
            worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = VolumeOutOfPack_DB;
            worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = VolumeInPack_DB;
            worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = MassOutOfPack_DB;
            worksheet.Cells[Row + (Transpon == false ? 10 : 0), Column + (Transpon == true ? 10 : 0)].Value = MassInPack_DB;
            worksheet.Cells[Row + (Transpon == false ? 11 : 0), Column + (Transpon == true ? 11 : 0)].Value = QuantityOZIII_DB;
            worksheet.Cells[Row + (Transpon == false ? 12 : 0), Column + (Transpon == true ? 12 : 0)].Value = TritiumActivity_DB;
            worksheet.Cells[Row + (Transpon == false ? 13 : 0), Column + (Transpon == true ? 13 : 0)].Value = BetaGammaActivity_DB;
            worksheet.Cells[Row + (Transpon == false ? 14 : 0), Column + (Transpon == true ? 14 : 0)].Value = AlphaActivity_DB;
            worksheet.Cells[Row + (Transpon == false ? 15 : 0), Column + (Transpon == true ? 15 : 0)].Value = TransuraniumActivity_DB;
            worksheet.Cells[Row + (Transpon == false ? 16 : 0), Column + (Transpon == true ? 16 : 0)].Value = MainRadionuclids_DB;
            worksheet.Cells[Row + (Transpon == false ? 17 : 0), Column + (Transpon == true ? 17 : 0)].Value = Subsidy_DB;
            worksheet.Cells[Row + (Transpon == false ? 18 : 0), Column + (Transpon == true ? 18 : 0)].Value = FcpNumber_DB;
            return 19;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true)
        {
            var cnt = Form2.ExcelHeader(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(StoragePlaceName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(StoragePlaceCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(PackName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(PackType)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(PackQuantity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(CodeRAO)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(StatusRAO)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(VolumeOutOfPack)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(VolumeInPack)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(MassOutOfPack)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 10 : 0), Column + (Transpon == true ? 10 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(MassInPack)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 11 : 0), Column + (Transpon == true ? 11 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(QuantityOZIII)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 12 : 0), Column + (Transpon == true ? 12 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(TritiumActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 13 : 0), Column + (Transpon == true ? 13 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(BetaGammaActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 14 : 0), Column + (Transpon == true ? 14 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(AlphaActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 15 : 0), Column + (Transpon == true ? 15 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(TransuraniumActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 16 : 0), Column + (Transpon == true ? 16 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(MainRadionuclids)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 17 : 0), Column + (Transpon == true ? 17 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(Subsidy)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 18 : 0), Column + (Transpon == true ? 18 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form22,Models").GetProperty(nameof(FcpNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            return 19;
        }
        #endregion
        #region IDataGridColumn
        private static DataGridColumns _DataGridColumns { get; set; } = null;
        public override DataGridColumns GetColumnStructure(string param = "")
        {
            if (_DataGridColumns == null)
            {
                #region NumberInOrder (1)
                DataGridColumns NumberInOrderR = ((Attributes.Form_PropertyAttribute)typeof(Form).GetProperty(nameof(Form.NumberInOrder)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                NumberInOrderR.SetSizeColToAllLevels(88);
                NumberInOrderR.Binding = nameof(Form.NumberInOrder);
                NumberInOrderR.Blocked = true;
                NumberInOrderR.ChooseLine = true;
                #endregion
                #region StoragePlaceName (2)
                DataGridColumns StoragePlaceNameR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.StoragePlaceName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                StoragePlaceNameR.SetSizeColToAllLevels(163);
                StoragePlaceNameR.Binding = nameof(Form22.StoragePlaceName);
                NumberInOrderR += StoragePlaceNameR;
                #endregion
                #region StoragePlaceCode (3)
                DataGridColumns StoragePlaceCodeR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.StoragePlaceCode)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                StoragePlaceCodeR.SetSizeColToAllLevels(88);
                StoragePlaceCodeR.Binding = nameof(Form22.StoragePlaceCode);
                NumberInOrderR += StoragePlaceCodeR;
                #endregion
                #region PackName (4)
                DataGridColumns PackNameR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.PackName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                PackNameR.SetSizeColToAllLevels(163);
                PackNameR.Binding = nameof(Form22.PackName);
                NumberInOrderR += PackNameR;
                #endregion
                #region PackType (5)
                DataGridColumns PackTypeR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.PackType)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                PackTypeR.SetSizeColToAllLevels(88);
                PackTypeR.Binding = nameof(Form22.PackType);
                NumberInOrderR += PackTypeR;
                #endregion
                #region PackQuantity (6)
                DataGridColumns PackQuantityR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.PackQuantity)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                PackQuantityR.SetSizeColToAllLevels(163);
                PackQuantityR.Binding = nameof(Form22.PackQuantity);
                NumberInOrderR += PackQuantityR;
                #endregion
                #region CodeRAO (7)
                DataGridColumns CodeRAOR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.CodeRAO)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                CodeRAOR.SetSizeColToAllLevels(88);
                CodeRAOR.Binding = nameof(Form22.CodeRAO);
                NumberInOrderR += CodeRAOR;
                #endregion
                #region StatusRAO (8)
                DataGridColumns StatusRAOR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.StatusRAO)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                StatusRAOR.SetSizeColToAllLevels(88);
                StatusRAOR.Binding = nameof(Form22.StatusRAO);
                NumberInOrderR += StatusRAOR;
                #endregion
                #region VolumeOutOfPack (9)
                DataGridColumns VolumeOutOfPackR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.VolumeOutOfPack)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                VolumeOutOfPackR.SetSizeColToAllLevels(163);
                VolumeOutOfPackR.Binding = nameof(Form22.VolumeOutOfPack);
                NumberInOrderR += VolumeOutOfPackR;
                #endregion
                #region VolumeInPack (10)
                DataGridColumns VolumeInPackR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.VolumeInPack)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                VolumeInPackR.SetSizeColToAllLevels(163);
                VolumeInPackR.Binding = nameof(Form22.VolumeInPack);
                NumberInOrderR += VolumeInPackR;
                #endregion
                #region MassOutOfPack (11)
                DataGridColumns MassOutOfPackR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.MassOutOfPack)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                MassOutOfPackR.SetSizeColToAllLevels(163);
                MassOutOfPackR.Binding = nameof(Form22.MassOutOfPack);
                NumberInOrderR += MassOutOfPackR;
                #endregion
                #region MassInPack (12)
                DataGridColumns MassInPackR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.MassInPack)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                MassInPackR.SetSizeColToAllLevels(163);
                MassInPackR.Binding = nameof(Form22.MassInPack);
                NumberInOrderR += MassInPackR;
                #endregion
                #region QuantityOZIII (13)
                DataGridColumns QuantityOZIIIR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.QuantityOZIII)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                QuantityOZIIIR.SetSizeColToAllLevels(163);
                QuantityOZIIIR.Binding = nameof(Form22.QuantityOZIII);
                NumberInOrderR += QuantityOZIIIR;
                #endregion
                #region TritiumActivity (14)
                DataGridColumns TritiumActivityR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.TritiumActivity)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                TritiumActivityR.SetSizeColToAllLevels(163);
                TritiumActivityR.Binding = nameof(Form22.TritiumActivity);
                NumberInOrderR += TritiumActivityR;
                #endregion
                #region BetaGammaActivity (15)
                DataGridColumns BetaGammaActivityR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.BetaGammaActivity)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                BetaGammaActivityR.SetSizeColToAllLevels(350);
                BetaGammaActivityR.Binding = nameof(Form22.BetaGammaActivity);
                NumberInOrderR += BetaGammaActivityR;
                #endregion
                #region AlphaActivity (16)
                DataGridColumns AlphaActivityR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.AlphaActivity)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                AlphaActivityR.SetSizeColToAllLevels(365);
                AlphaActivityR.Binding = nameof(Form22.AlphaActivity);
                NumberInOrderR += AlphaActivityR;
                #endregion
                #region TransuraniumActivity (17)
                DataGridColumns TransuraniumActivityR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.TransuraniumActivity)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                TransuraniumActivityR.SetSizeColToAllLevels(200);
                TransuraniumActivityR.Binding = nameof(Form22.TransuraniumActivity);
                NumberInOrderR += TransuraniumActivityR;
                #endregion
                #region MainRadionuclids (18)
                DataGridColumns MainRadionuclidsR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.MainRadionuclids)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                MainRadionuclidsR.SetSizeColToAllLevels(153);
                MainRadionuclidsR.Binding = nameof(Form22.MainRadionuclids);
                NumberInOrderR += MainRadionuclidsR;
                #endregion
                #region Subsidy (19)
                DataGridColumns SubsidyR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.Subsidy)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                SubsidyR.SetSizeColToAllLevels(88);
                SubsidyR.Binding = nameof(Form22.Subsidy);
                NumberInOrderR += SubsidyR;
                #endregion
                #region FcpNumber (20)
                DataGridColumns FcpNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form22).GetProperty(nameof(Form22.FcpNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                FcpNumberR.SetSizeColToAllLevels(163);
                FcpNumberR.Binding = nameof(Form22.FcpNumber);
                NumberInOrderR += FcpNumberR;
                #endregion
                _DataGridColumns = NumberInOrderR;
            }
            return _DataGridColumns;
        }
        #endregion
    }
}
