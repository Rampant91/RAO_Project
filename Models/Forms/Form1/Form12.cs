using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.2: Сведения об изделиях из обедненного урана")]
    public class Form12 : Abstracts.Form1
    {
        public Form12() : base()
        {
            //FormNum.Value = "12";
            //NumberOfFields.Value = 35;
            Init();
            Validate_all();
        }
        private void Init()
        {
            DataAccess.Init<string>(nameof(NameIOU), NameIOU_Validation, null);
            NameIOU.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Mass), Mass_Validation, null);
            Mass.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(CreationDate), CreationDate_Validation, null);
            CreationDate.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(CreatorOKPO), CreatorOKPO_Validation, null);
            CreatorOKPO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(FactoryNumber), FactoryNumber_Validation, null);
            FactoryNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Owner), Owner_Validation, null);
            Owner.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackName), PackName_Validation, null);
            PackName.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackNumber), PackNumber_Validation, null);
            PackNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackType), PackType_Validation, null);
            PackType.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PassportNumber), PassportNumber_Validation, null);
            PassportNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<byte?>(nameof(PropertyCode), PropertyCode_Validation, null);
            PropertyCode.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(ProviderOrRecieverOKPO), ProviderOrRecieverOKPO_Validation, null);
            ProviderOrRecieverOKPO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<float>(nameof(SignedServicePeriod), SignedServicePeriod_Validation, 0);
            SignedServicePeriod.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(TransporterOKPO), TransporterOKPO_Validation, null);
            TransporterOKPO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(FactoryNumberRecoded), FactoryNumberRecoded_Validation, null);
            FactoryNumberRecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackNumberRecoded), PackNumberRecoded_Validation, null);
            PackNumberRecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackTypeRecoded), PackTypeRecoded_Validation, null);
            PackTypeRecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PassportNumberRecoded), PassportNumberRecoded_Validation, null);
            PassportNumberRecoded.PropertyChanged += InPropertyChanged;
        }
        private void Validate_all()
        {
            CreationDate_Validation(CreationDate);
            CreatorOKPO_Validation(CreatorOKPO);
            Owner_Validation(Owner);
            PackName_Validation(PackName);
            PackNumber_Validation(PackNumber);
            PackType_Validation(PackType);
            PassportNumber_Validation(PassportNumber);
            PropertyCode_Validation(PropertyCode);
            ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
            SignedServicePeriod_Validation(SignedServicePeriod);
            TransporterOKPO_Validation(TransporterOKPO);
            FactoryNumber_Validation(FactoryNumber);
            Mass_Validation(Mass);
            NameIOU_Validation(NameIOU);
            FactoryNumberRecoded_Validation(FactoryNumberRecoded);
            PassportNumberRecoded_Validation(PassportNumberRecoded);
            PackTypeRecoded_Validation(PackTypeRecoded);
            PackNumberRecoded_Validation(PackNumberRecoded);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        ////DocumentNumberNote property
        //public virtual RamAccess<string> DocumentNumberNote
        //{
        //    get
        //    {
                
        //        {
        //            return DataAccess.Get<string>(nameof(DocumentNumberNote));//OK
                    
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {


                
        //        {
        //            DataAccess.Set(nameof(DocumentNumberNote), value);
        //        }
        //        OnPropertyChanged(nameof(DocumentNumberNote));
        //    }
        //}


        //private bool DocumentNumberNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////DocumentNumberNote property

        //PassportNumber property
        public int? PassportNumberId { get; set; }
        [Attributes.Form_Property("Номер паспорта")]
        public virtual RamAccess<string> PassportNumber
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(PassportNumber));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    DataAccess.Set(nameof(PassportNumber), value);
                }
                OnPropertyChanged(nameof(PassportNumber));
            }
        }


        private bool PassportNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PassportNumberNote.Value == null) || (PassportNumberNote.Value == ""))
                //    value.AddError( "Заполните примечание");//to do note handling
return true;
            }
            return true;
        }
        //PassportNumber property

        ////OwnerNote property
        //public virtual RamAccess<string> OwnerNote
        //{
        //    get
        //    {
                
        //        {
        //            return DataAccess.Get<string>(nameof(OwnerNote));//OK
                    
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {


                
        //        {
        //            DataAccess.Set(nameof(OwnerNote), value);
        //        }
        //        OnPropertyChanged(nameof(OwnerNote));
        //    }
        //}
        ////if change this change validation

        //private bool OwnerNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////OwnerNote property

        protected override bool OperationCode_Validation(RamAccess<short?> value)//OK
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            List<short> spr = new List<short>();    //HERE BINDS SPRAVOCHNIK
            if (!spr.Contains((short)value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            if ((value.Value == 01) || (value.Value == 13) ||
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

        ////PassportNumberNote property
        //public virtual RamAccess<string> PassportNumberNote
        //{
        //    get
        //    {
                
        //        {
        //            return DataAccess.Get<string>(nameof(PassportNumberNote));//OK
                    
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {


                
        //        {
        //            DataAccess.Set(nameof(PassportNumberNote), value);
        //        }
        //        OnPropertyChanged(nameof(PassportNumberNote));
        //    }
        //}


        //private bool PassportNumberNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////PassportNumberNote property

        //PassportNumberRecoded property
        public int? PassportNumberRecodedId { get; set; }
        public virtual RamAccess<string> PassportNumberRecoded
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(PassportNumberRecoded));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(PassportNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PassportNumberRecoded));
            }
        }
        //If change this change validation

        private bool PassportNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
        //PassportNumberRecoded property

        //NameIOU property
        public int? NameIOUId { get; set; }
        [Attributes.Form_Property("Наименование ИОУ")]
        public virtual RamAccess<string> NameIOU
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(NameIOU));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    DataAccess.Set(nameof(NameIOU), value);
                }
                OnPropertyChanged(nameof(NameIOU));
            }
        }


        private bool NameIOU_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return false;
            }
            return true;
        }
        //NameIOU property

        //FactoryNumber property
        public int? FactoryNumberId { get; set; }
        [Attributes.Form_Property("Заводской номер")]
        public virtual RamAccess<string> FactoryNumber
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(FactoryNumber));//OK
                    
                }
                
                {
                    
                }
            }
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
                value.AddError( "Поле не заполнено");
return false;
            }
            return true;
        }
        //FactoryNumber property

        //FactoryNumberRecoded property
        public virtual RamAccess<string> FactoryNumberRecoded
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(FactoryNumberRecoded));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(FactoryNumberRecoded), value);
                }
                OnPropertyChanged(nameof(FactoryNumberRecoded));
            }
        }
        //If change this change validation

        private bool FactoryNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
        //FactoryNumberRecoded property

        //Mass Property
        public int? MassId { get; set; }
        [Attributes.Form_Property("Масса, кг")]
        public virtual RamAccess<string> Mass
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(Mass));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    DataAccess.Set(nameof(Mass), value);
                }
                OnPropertyChanged(nameof(Mass));
            }
        }


        private bool Mass_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            string tmp=value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            try
            {
                if (!(double.Parse(tmp) > 0)){value.AddError("Число должно быть больше нуля");return false;}
            }
            catch (Exception)
            {
                value.AddError( "Недопустимое значение");
                return false;
            }
            return true;
        }
        //Mass Property

        //CreatorOKPO property
        public int? CreatorOKPOId { get; set; }
        [Attributes.Form_Property("ОКПО изготовителя")]
        public virtual RamAccess<string> CreatorOKPO
        {
            get
            {
                {
                    return DataAccess.Get<string>(nameof(CreatorOKPO));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    DataAccess.Set(nameof(CreatorOKPO), value);
                }
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
                //if ((CreatorOKPONote.Value == null) || (CreatorOKPONote.Value == ""))
                //    value.AddError( "Заполните примечание");
                return true;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //CreatorOKPO property

        ////CreatorOKPONote property
        //public virtual RamAccess<string> CreatorOKPONote
        //{
        //    get
        //    {
                
        //        {
        //            return DataAccess.Get<string>(nameof(CreatorOKPONote));//OK
                    
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {



                
        //        {
        //            DataAccess.Set(nameof(CreatorOKPONote), value);
        //        }
        //        OnPropertyChanged(nameof(CreatorOKPONote));
        //    }
        //}


        //private bool CreatorOKPONote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////CreatorOKPONote property

        //CreationDate property
        public int? CreationDateId { get; set; }
        [Attributes.Form_Property("Дата изготовления")]
        public virtual RamAccess<string> CreationDate
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(CreationDate));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                
                {
                    DataAccess.Set(nameof(CreationDate), value);
                }
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
                //    if ((CreationDateNote.Value == null) || (CreationDateNote.Value == ""))
                //        value.AddError( "Заполните примечание");
                return true;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
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
                
        //        {
        //            return DataAccess.Get<string>(nameof(CreationDateNote));//OK
                    
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {


                
        //        {
        //            DataAccess.Set(nameof(CreationDateNote), value);
        //        }
        //        OnPropertyChanged(nameof(CreationDateNote));
        //    }
        //}
        ////If change this change validation


        //private bool CreationDateNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        ////CreationDateNote property

        //SignedServicePeriod property
        public int? SignedServicePeriodId { get; set; }
        [Attributes.Form_Property("НСС, мес.")]
        public virtual RamAccess<float> SignedServicePeriod
        {
            get
            {
                    return DataAccess.Get<float>(nameof(SignedServicePeriod));//OK
            }
            set
            {


                
                {
                    DataAccess.Set(nameof(SignedServicePeriod), value);
                }
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
            get
            {
                    return DataAccess.Get<byte?>(nameof(PropertyCode));//OK
            }
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
            //if (value.Value == 255)//ok
            //{
            //    value.AddError( "Поле не заполнено");
            //}
            if (!((value.Value >= 1) && (value.Value <= 9)))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //PropertyCode property

        //Owner property
        public int? OwnerId { get; set; }
        [Attributes.Form_Property("Владелец")]
        public virtual RamAccess<string> Owner
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(Owner));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    DataAccess.Set(nameof(Owner), value);
                }
                OnPropertyChanged(nameof(Owner));
            }
        }
        //if change this change validation

        private bool Owner_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((OwnerNote.Value == null) || (OwnerNote.Value == ""))
                //    value.AddError( "Заполните примечание");
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
        //Owner property

        //ProviderOrRecieverOKPO property
        public int? ProviderOrRecieverOKPOId { get; set; }
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
        public virtual RamAccess<string> ProviderOrRecieverOKPO
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(ProviderOrRecieverOKPO));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    DataAccess.Set(nameof(ProviderOrRecieverOKPO), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }


        private bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((ProviderOrRecieverOKPONote.Value == null) || ProviderOrRecieverOKPONote.Value.Equals(""))
                //    value.AddError( "Заполните примечания");
return true;
            }
            bool a = (OperationCode.Value >= 10) && (OperationCode.Value <= 12);
            bool b = (OperationCode.Value >= 41) && (OperationCode.Value <= 43);
            bool c = (OperationCode.Value >= 71) && (OperationCode.Value <= 73);
            bool d = (OperationCode.Value == 15) || (OperationCode.Value == 17) || (OperationCode.Value == 18) || (OperationCode.Value == 46) ||
                (OperationCode.Value == 47) || (OperationCode.Value == 48) || (OperationCode.Value == 53) || (OperationCode.Value == 54) ||
                (OperationCode.Value == 58) || (OperationCode.Value == 61) || (OperationCode.Value == 62) || (OperationCode.Value == 65) ||
                (OperationCode.Value == 67) || (OperationCode.Value == 68) || (OperationCode.Value == 75) || (OperationCode.Value == 76);
            if (a || b || c || d)
            {
                ProviderOrRecieverOKPO.Value = "ОКПО ОТЧИТЫВАЮЩЕЙСЯ ОРГ";
return false;
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
        //ProviderOrRecieverOKPO property

        ////ProviderOrRecieverOKPONote property
        //public virtual RamAccess<string> ProviderOrRecieverOKPONote
        //{
        //    get
        //    {
                
        //        {
        //            return DataAccess.Get<string>(nameof(ProviderOrRecieverOKPONote));//OK
                    
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {

                
        //        {
        //            DataAccess.Set(nameof(ProviderOrRecieverOKPONote), value);
        //        }
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
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(TransporterOKPO));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    DataAccess.Set(nameof(TransporterOKPO), value);
                }
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }


        private bool TransporterOKPO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((TransporterOKPONote.Value == null) || TransporterOKPONote.Value.Equals(""))
                //    value.AddError( "Заполните примечание");
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
        //TransporterOKPO property

        ////TransporterOKPONote property
        //public virtual RamAccess<string> TransporterOKPONote
        //{
        //    get
        //    {
                
        //        {
        //            return DataAccess.Get<string>(nameof(TransporterOKPONote));//OK
                    
        //        }
                
        //        {
                    
        //        }
        //    }
        //    set
        //    {

                
        //        {
        //            DataAccess.Set(nameof(TransporterOKPONote), value);
        //        }
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
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PackNameNote == null) || PackNameNote.Equals(""))
                //    value.AddError( "Заполните примечание");//to do note handling
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
            if ((value.Value == null))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PackTypeNote == null) || PackTypeNote.Equals(""))
                //    value.AddError( "Заполните примечание");//to do note handling
                return true;
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
            value.ClearErrors(); return true;}
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
        //            DataAccess.Set(nameof(PackTypeNote), value);
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
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(PackNumber));//OK
                    
                }
                
                {
                    
                }
            }
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
            if ((value.Value == null))//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PackNumberNote == null) || PackNumberNote.Equals(""))
                //    value.AddError( "Заполните примечание");//to do note handling
                return true;
            }
            return true;
        }
        //PackNumber property

////PackNumberNote property
//        public virtual RamAccess<string> PackNumberNote
//        {
//            get
//            {
                
//                {
//                    return DataAccess.Get<string>(nameof(PackNumberNote));//OK
                    
//                }
                
//                {
                    
//                }
//            }
//            set
//            {


                
//                {
//                    DataAccess.Set(nameof(PackNumberNote), value);
//                }
//                OnPropertyChanged(nameof(PackNumberNote));
//            }
//        }


//        private bool PackNumberNote_Validation(RamAccess<string> value)
//        {
//            value.ClearErrors();
//            if ((value.Value == null) || value.Value.Equals(""))
//            {
//                value.AddError("Поле не заполнено");
//return false;
//            }
//            return true;
//        }
////PackNumberNote property

        //PackNumberRecoded property
        public int? PackNumberRecodedId { get; set; }
        public virtual RamAccess<string> PackNumberRecoded
        {
            get
            {
                
                {
                    return DataAccess.Get<string>(nameof(PackNumberRecoded));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                    DataAccess.Set(nameof(PackNumberRecoded), value);
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        //If change this change validation

        private bool PackNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
        //PackNumberRecoded property

        protected override bool DocumentNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (value.Value == "прим.")
            {
                //if ((DocumentNumberNote == null) || DocumentNumberNote.Equals(""))
                //    value.AddError( "Заполните примечание");//to do note handling
return true;
            }
            if ((value.Value == null))//ok
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            return true;
        }
    }
}
