using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Spravochniki;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.1: Сведения о ЗРИ")]
    public class Form11 : Abstracts.Form1
    {
        public Form11() : base()
        {
            ////FormNum.Value = "11";
            ////NumberOfFields.Value = 42;
            Init();
            Validate_all();
        }

        private void Init()
        {
            DataAccess.Init<string>(nameof(Activity), Activity_Validation, null);
            Activity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<short?>(nameof(Category), Category_Validation, null);
            Category.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(CreationDate), CreationDate_Validation, null);
            CreationDate.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(CreatorOKPO), CreatorOKPO_Validation, null);
            CreatorOKPO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(FactoryNumberRecoded), FactoryNumberRecoded_Validation, null);
            FactoryNumberRecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(FactoryNumber), FactoryNumber_Validation, null);
            FactoryNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Owner), Owner_Validation, null);
            Owner.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackName), PackName_Validation, null);
            PackName.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackNumberRecoded), PackNumberRecoded_Validation, null);
            PackNumberRecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackNumber), PackNumber_Validation, null);
            PackNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackTypeRecoded), PackTypeRecoded_Validation, null);
            PackTypeRecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackType), PackType_Validation, null);
            PackType.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PassportNumberRecoded), PassportNumberRecoded_Validation, null);
            PassportNumberRecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PassportNumber), PassportNumber_Validation, null);
            PassportNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<byte?>(nameof(PropertyCode), PropertyCode_Validation, null);
            PropertyCode.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(ProviderOrRecieverOKPO), ProviderOrRecieverOKPO_Validation, null);
            ProviderOrRecieverOKPO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<int?>(nameof(Quantity), Quantity_Validation, null);
            Quantity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Radionuclids), Radionuclids_Validation, null);
            Radionuclids.PropertyChanged += InPropertyChanged;
            DataAccess.Init<float>(nameof(SignedServicePeriod), SignedServicePeriod_Validation, 0);
            SignedServicePeriod.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(TransporterOKPO), TransporterOKPO_Validation, null);
            TransporterOKPO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(TypeRecoded), TypeRecoded_Validation, null);
            TypeRecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Type), Type_Validation, null);
            Type.PropertyChanged += InPropertyChanged;
        }

        private void Validate_all()
        {
            Activity_Validation(Activity);
            Category_Validation(Category);
            CreationDate_Validation(CreationDate);
            CreatorOKPO_Validation(CreatorOKPO);
            FactoryNumber_Validation(FactoryNumber);
            Owner_Validation(Owner);
            PackName_Validation(PackName);
            PackNumber_Validation(PackNumber);
            PackTypeRecoded_Validation(PackTypeRecoded);
            PackType_Validation(PackType);
            PassportNumber_Validation(PassportNumber);
            PropertyCode_Validation(PropertyCode);
            ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
            Quantity_Validation(Quantity);
            Radionuclids_Validation(Radionuclids);
            SignedServicePeriod_Validation(SignedServicePeriod);
            TransporterOKPO_Validation(TransporterOKPO);
            Type_Validation(Type);
            FactoryNumberRecoded_Validation(FactoryNumberRecoded);
            PassportNumberRecoded_Validation(PassportNumberRecoded);
            PackNumberRecoded_Validation(PackNumberRecoded);
            TypeRecoded_Validation(TypeRecoded);
        }

        public override bool Object_Validation()
        {
            return false;
        }

        //PassportNumber property
        public int? PassportNumberId { get; set; }
        [Attributes.Form_Property("Номер паспорта")]
        public virtual RamAccess<string> PassportNumber
        {
            get => DataAccess.Get<string>(nameof(PassportNumber));//OK
            set
            {
                DataAccess.Set(nameof(PassportNumber), value);
                OnPropertyChanged(nameof(PassportNumber));
            }
        }
        protected bool PassportNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PassportNumberNote.Value==null)||(PassportNumberNote.Value == ""))
                //    value.AddError("Заполните примечание"); //to do note handling
                return true;
            }
            return true;
        }
        //PassportNumber property


        //PassportNumberRecoded property
        public int? PassportNumberRecodedId { get; set; }
        public virtual RamAccess<string> PassportNumberRecoded
        {
            get => DataAccess.Get<string>(nameof(PassportNumberRecoded));//OK 
            set
            {
                DataAccess.Set(nameof(PassportNumberRecoded), value);
                OnPropertyChanged(nameof(PassportNumberRecoded));
            }
        }
        //If change this change validation
        private bool PassportNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        //PassportNumberRecoded property

        //Type property
        public int? TypeId { get; set; }
        [Attributes.Form_Property("Тип")]
        public virtual RamAccess<string> Type
        {
            get => DataAccess.Get<string>(nameof(Type));//OK
            set
            {



                {
                    DataAccess.Set(nameof(Type), value);
                }
                OnPropertyChanged(nameof(Type));
            }
        }

        private bool Type_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            List<string> spr = new List<string>
            {
                "ГИИД-6",
                "ГИИД-5"
            };    //HERE BINDS SPRAVOCHNIK
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Type property

        //TypeRecoded property
        public virtual RamAccess<string> TypeRecoded
        {
            get => DataAccess.Get<string>(nameof(TypeRecoded));//OK
            set
            {
                DataAccess.Set(nameof(TypeRecoded), value);
                OnPropertyChanged(nameof(TypeRecoded));
            }
        }

        private bool TypeRecoded_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //TypeRecoded property

        //Radionuclids property
        public int? RadionuclidsId { get; set; }
        [Attributes.Form_Property("Радионуклиды")]
        public virtual RamAccess<string> Radionuclids
        {
            get => DataAccess.Get<string>(nameof(Radionuclids));//OK
            set
            {
                DataAccess.Set(nameof(Radionuclids), value);
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation
        private bool Radionuclids_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            foreach (var item in Spravochniks.SprRadionuclids)
            {
                if (item.Item1.Equals(value.Value.ToLower()))
                {
                    return true;
                }
            }
            value.AddError("Недопустимое значение");
            return false;
        }
        //Radionuclids property

        //FactoryNumber property
        public int? FactoryNumberId { get; set; }
        [Attributes.Form_Property("Заводской номер")]
        public virtual RamAccess<string> FactoryNumber
        {
            get => DataAccess.Get<string>(nameof(FactoryNumber));//OK
            set
            {
                DataAccess.Set(nameof(FactoryNumber), value);
                OnPropertyChanged(nameof(FactoryNumber));
            }
        }

        private bool FactoryNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //FactoryNumber property

        //FactoryNumberRecoded property
        public virtual RamAccess<string> FactoryNumberRecoded
        {
            get => DataAccess.Get<string>(nameof(FactoryNumberRecoded));//OK
            set
            {
                DataAccess.Set(nameof(FactoryNumberRecoded), value);
                OnPropertyChanged(nameof(FactoryNumberRecoded));
            }
        }
        //If change this change validation
        private bool FactoryNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        //FactoryNumberRecoded property

        //Quantity property
        public int? QuantityId { get; set; }
        [Attributes.Form_Property("Количество, шт.")]
        public virtual RamAccess<int?> Quantity
        {
            get => DataAccess.Get<int?>(nameof(Quantity));//OK;
            set
            {
                DataAccess.Set(nameof(Quantity), value);
                OnPropertyChanged(nameof(Quantity));
            }
        }
        private bool Quantity_Validation(RamAccess<int?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Quantity property

        //Activity property
        public int? ActivityId { get; set; }
        [Attributes.Form_Property("Активность, Бк")]
        public virtual RamAccess<string> Activity
        {
            get => DataAccess.Get<string>(nameof(Activity));//OK
            set
            {
                DataAccess.Set(nameof(Activity), value);
                OnPropertyChanged(nameof(Activity));
            }
        }

        private bool Activity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!((value.Value.Contains('e') || value.Value.Contains('E'))))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((Activity.Value == null) || (ActivityNote.Value == ""))
                //    value.AddError("Заполните примечание");
                return false;
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Activity property

        ////ActivityNote property
        //public virtual RamAccess<string> ActivityNote
        //{
        //    get
        //    {
        //        return DataAccess.Get<string>(nameof(ActivityNote));//OK
        //    }
        //    set
        //    {
        //        DataAccess.Set(nameof(ActivityNote), value);
        //        OnPropertyChanged(nameof(ActivityNote));
        //    }
        //}
        ////If change this change validation
        //private bool ActivityNote_Validation(RamAccess<string> value)//Ready
        //{
        //    value.ClearErrors(); return true;}
        ////ActivityNote property

        //CreationDate property
        public int? CreationDateId { get; set; }
        [Attributes.Form_Property("Дата изготовления")]
        public virtual RamAccess<string> CreationDate
        {
            get => DataAccess.Get<string>(nameof(CreationDate));//OK
            set
            {
                DataAccess.Set(nameof(CreationDate), value);
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        //If change this change validation
        private bool CreationDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((CreationDateNote.Value == null) || (CreationDateNote.Value == ""))
                //    value.AddError("Заполните примечание");
                return false;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //CreationDate property

        ////CreationDateNote property
        //public virtual RamAccess<string> CreationDateNote
        //{
        //    get
        //    {
        //        return DataAccess.Get<string>(nameof(CreationDateNote));//OK
        //    }
        //    set
        //    {
        //        DataAccess.Set(nameof(CreationDateNote), value);
        //        OnPropertyChanged(nameof(CreationDateNote));
        //    }
        //}
        ////If change this change validation

        //private bool CreationDateNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////CreationDateNote property

        //CreatorOKPO property
        public int? CreatorOKPOId { get; set; }
        [Attributes.Form_Property("ОКПО изготовителя")]
        public virtual RamAccess<string> CreatorOKPO
        {
            get => DataAccess.Get<string>(nameof(CreatorOKPO));//OK
            set
            {
                DataAccess.Set(nameof(CreatorOKPO), value);
                OnPropertyChanged(nameof(CreatorOKPO));
            }
        }
        //If change this change validation
        private bool CreatorOKPO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || (value.Value.Equals("")))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PassportNumberNote.Value == null) || (PassportNumberNote.Value == ""))
                //    value.AddError("Заполните примечание");
                return false;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //CreatorOKPO property

        ////CreatorOKPONote property
        //public virtual RamAccess<string> CreatorOKPONote
        //{
        //    get
        //    {
        //        return DataAccess.Get<string>(nameof(CreatorOKPONote));//OK
        //    }
        //    set
        //    {
        //        DataAccess.Set(nameof(CreatorOKPONote), value);
        //        OnPropertyChanged(nameof(CreatorOKPONote));
        //    }
        //}

        //private bool CreatorOKPONote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////CreatorOKPONote property

        //Category property
        public int? CategoryId { get; set; }
        [Attributes.Form_Property("Категория")]
        public virtual RamAccess<short?> Category
        {
            get => DataAccess.Get<short?>(nameof(Category));//OK
            set
            {
                DataAccess.Set(nameof(Category), value);
                OnPropertyChanged(nameof(Category));
            }
        }

        private bool Category_Validation(RamAccess<short?> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if ((value.Value < 1) || (value.Value > 5))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Category property

        //SignedServicePeriod property
        public int? SignedServicePeriodId { get; set; }
        [Attributes.Form_Property("НСС, мес.")]
        public virtual RamAccess<float> SignedServicePeriod
        {
            get => DataAccess.Get<float>(nameof(SignedServicePeriod));//OK
            set
            {
                DataAccess.Set(nameof(SignedServicePeriod), value);
                OnPropertyChanged(nameof(SignedServicePeriod));
            }
        }

        private bool SignedServicePeriod_Validation(RamAccess<float> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //SignedServicePeriod property

        //PropertyCode property
        public int? PropertyCodeId { get; set; }
        [Attributes.Form_Property("Код собственности")]
        public virtual RamAccess<byte?> PropertyCode
        {
            get => DataAccess.Get<byte?>(nameof(PropertyCode));//OK
            set
            {
                DataAccess.Set(nameof(PropertyCode), value);
                OnPropertyChanged(nameof(PropertyCode));
            }
        }

        private bool PropertyCode_Validation(RamAccess<byte?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!((value.Value >= 1) && (value.Value <= 9)))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //PropertyCode property

        ////OwnerNote property
        //public virtual RamAccess<string> OwnerNote
        //{
        //    get
        //    {
        //        return DataAccess.Get<string>(nameof(OwnerNote));//OK
        //    }
        //    set
        //    {
        //        DataAccess.Set(nameof(OwnerNote), value);
        //        OnPropertyChanged(nameof(OwnerNote));
        //    }
        //}
        ////if change this change validation
        //private bool OwnerNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////OwnerNote property

        //Owner property
        public int? OwnerId { get; set; }
        [Attributes.Form_Property("Владелец")]
        public virtual RamAccess<string> Owner
        {
            get => DataAccess.Get<string>(nameof(Owner));//OK
            set
            {
                DataAccess.Set(nameof(Owner), value);
                OnPropertyChanged(nameof(Owner));
            }
        }
        //if change this change validation
        private bool Owner_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((OwnerNote == null) || OwnerNote.Equals(""))
                //    value.AddError("Заполните примечание");
                return true;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;

            }
            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Owner property

        //ProviderOrRecieverOKPO property
        public int? ProviderOrRecieverOKPOId { get; set; }
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
        public virtual RamAccess<string> ProviderOrRecieverOKPO
        {
            get => DataAccess.Get<string>(nameof(ProviderOrRecieverOKPO));//OK
            set
            {
                DataAccess.Set(nameof(ProviderOrRecieverOKPO), value);
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }

        private bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((ProviderOrRecieverOKPONote == null) || ProviderOrRecieverOKPONote.Equals(""))
                //    value.AddError("Заполните примечание");
                return true;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение");
                return false;

            }
            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //ProviderOrRecieverOKPO property

        ////ProviderOrRecieverOKPONote property
        //public virtual RamAccess<string> ProviderOrRecieverOKPONote
        //{
        //    get
        //    {
        //        return DataAccess.Get<string>(nameof(ProviderOrRecieverOKPONote));//OK
        //    }
        //    set
        //    {
        //        DataAccess.Set(nameof(ProviderOrRecieverOKPONote), value);
        //        OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
        //    }
        //}

        //private bool ProviderOrRecieverOKPONote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////ProviderOrRecieverOKPONote property

        //TransporterOKPO property
        public int? TransporterOKPOId { get; set; }
        [Attributes.Form_Property("ОКПО перевозчика")]
        public virtual RamAccess<string> TransporterOKPO
        {
            get => DataAccess.Get<string>(nameof(TransporterOKPO));//OK
            set
            {
                DataAccess.Set(nameof(TransporterOKPO), value);
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }

        private bool TransporterOKPO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
                //    value.AddError("Заполните примечание");
                return true;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //TransporterOKPO property

        ////TransporterOKPONote property
        //public virtual RamAccess<string> TransporterOKPONote
        //{
        //    get
        //    {
        //        return DataAccess.Get<string>(nameof(TransporterOKPONote));//OK
        //    }
        //    set
        //    {
        //        DataAccess.Set(nameof(TransporterOKPONote), value);
        //        OnPropertyChanged(nameof(TransporterOKPONote));
        //    }
        //}

        //private bool TransporterOKPONote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////TransporterOKPONote property

        //PackName property
        public int? PackNameId { get; set; }
        [Attributes.Form_Property("Наименование упаковки")]
        public virtual RamAccess<string> PackName
        {
            get => DataAccess.Get<string>(nameof(PackName));//OK
            set
            {
                DataAccess.Set(nameof(PackName), value);
                OnPropertyChanged(nameof(PackName));
            }
        }

        private bool PackName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PackNameNote == null) || PackNameNote.Equals(""))
                //    value.AddError("Заполните примечание");//to do note handling
                return true;
            }
            return true;
        }
        //PackName property

        ////PackNameNote property
        //public virtual RamAccess<string> PackNameNote
        //{
        //    get
        //    {
        //        return DataAccess.Get<string>(nameof(PackNameNote));//OK
        //    }
        //    set
        //    {
        //        DataAccess.Set(nameof(PackNameNote), value);
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
            get => DataAccess.Get<string>(nameof(PackType));//OK
            set
            {
                DataAccess.Set(nameof(PackType), value);
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation
        private bool PackType_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PackTypeNote == null) || PackTypeNote.Equals(""))
                //    value.AddError("Заполните примечание");//to do note handling
                return true;
            }
            return true;
        }
        //PackType property

        //PackTypeRecoded property
        public virtual RamAccess<string> PackTypeRecoded
        {
            get => DataAccess.Get<string>(nameof(PackTypeRecoded));//OK
            set
            {
                DataAccess.Set(nameof(PackTypeRecoded), value);
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }

        private bool PackTypeRecoded_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PackTypeRecoded property

        ////DocumentNumberNote property
        //public virtual RamAccess<string> DocumentNumberNote
        //{
        //    get
        //    {
        //        return DataAccess.Get<string>(nameof(DocumentNumberNote));//OK
        //    }
        //    set
        //    {
        //        DataAccess.Set(nameof(DocumentNumberNote), value);
        //        OnPropertyChanged(nameof(DocumentNumberNote));
        //    }
        //}

        //private bool DocumentNumberNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////DocumentNumberNote property

        ////PackTypeNote property
        //public virtual RamAccess<string> PackTypeNote
        //{
        //    get
        //    {
        //        return DataAccess.Get<string>(nameof(PackTypeNote));//OK
        //    }
        //    set
        //    {
        //        DataAccess.Set(nameof(PackTypeNote), value);
        //        OnPropertyChanged(nameof(PackTypeNote));
        //    }
        //}

        //private bool PackTypeNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////PackTypeNote property

        //PackNumber property
        public int? PackNumberId { get; set; }
        [Attributes.Form_Property("Номер упаковки")]
        public virtual RamAccess<string> PackNumber
        {
            get => DataAccess.Get<string>(nameof(PackNumber));//OK
            set
            {
                DataAccess.Set(nameof(PackNumber), value);
                OnPropertyChanged(nameof(PackNumber));
            }
        }
        //If change this change validation
        private bool PackNumber_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PackNumberNote == null) || PackNumberNote.Equals(""))
                //    value.AddError("Заполните примечание");//to do note handling
                return true;
            }
            return true;
        }
        //PackNumber property

        //PackNumberRecoded property
        public int? PackNumberRecodedId { get; set; }
        public virtual RamAccess<string> PackNumberRecoded
        {
            get => DataAccess.Get<string>(nameof(PackNumberRecoded));
            set
            {
                DataAccess.Set(nameof(PackNumberRecoded), value);
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        //If change this change validation
        private bool PackNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        //PackNumberRecoded property

        ////PackNumberNote property
        //public virtual RamAccess<string> PackNumberNote
        //{
        //    get
        //    {
        //        return DataAccess.Get<string>(nameof(PackNumberNote));//OK
        //    }
        //    set
        //    {
        //        DataAccess.Set(nameof(PackNumberNote), value);
        //        OnPropertyChanged(nameof(PackNumberNote));
        //    }
        //}

        //private bool PackNumberNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors();
        //    return true;
        //}
        ////PackNumberNote property

        protected override bool DocumentNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (value.Value == "прим.")
            {
                //if ((DocumentNumberNote.Value == null) || DocumentNumberNote.Value.Equals(""))
                //    value.AddError("Заполните примечание");
                return true;
            }
            if (value.Value == null)//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        protected override bool OperationCode_Validation(RamAccess<short?> value)//OK
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            List<short> spr = new List<short>()
            {
                1,10,11,12,13,14,15,16,17,18,21,22,25,26,27,28,29,31,32,35,36,37,38,39,41,42,43,44,45,
                46,47,48,49,51,52,53,54,55,56,57,58,59,61,62,63,64,65,66,67,68,71,72,73,74,75,76,81,82,
                83,84,85,86,87,88,97,98,99
            };    //HERE BINDS SPRAVOCHNIK
            if (!spr.Contains((short)value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            if ((value.Value == 1) || (value.Value == 13) ||
                (value.Value == 14) || (value.Value == 16) ||
                (value.Value == 26) || (value.Value == 36) ||
                (value.Value == 44) || (value.Value == 45) ||
                (value.Value == 49) || (value.Value == 51) ||
                (value.Value == 52) || (value.Value == 55) ||
                (value.Value == 56) || (value.Value == 57) ||
                (value.Value == 59) || (value.Value == 76))
            {
                value.AddError("Код операции не может быть использован для РВ");
                return false;
            }
            return true;
        }
    }
}
