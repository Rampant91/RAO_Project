using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Spravochniki;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.4: Сведения об ОРИ, кроме отдельных изделий")]
    public class Form14 : Abstracts.Form1
    {
        public Form14() : base()
        {
            //FormNum.Value = "14";
            //NumberOfFields.Value = 35;
            Init();
            Validate_all();
        }

        private void Init()
        {
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
            DataAccess.Init<string>(nameof(ProviderOrRecieverOKPO), ProviderOrRecieverOKPO_Validation, null);
            ProviderOrRecieverOKPO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(TransporterOKPO), TransporterOKPO_Validation, null);
            TransporterOKPO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Activity), Activity_Validation, null);
            Activity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Radionuclids), Radionuclids_Validation, null);
            Radionuclids.PropertyChanged += InPropertyChanged;
            DataAccess.Init<byte?>(nameof(PropertyCode), PropertyCode_Validation, null);
            PropertyCode.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Name), Name_Validation, null);
            Name.PropertyChanged += InPropertyChanged;
            DataAccess.Init<byte?>(nameof(Sort), Sort_Validation, null);
            Sort.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(ActivityMeasurementDate), ActivityMeasurementDate_Validation, null);
            ActivityMeasurementDate.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Volume), Volume_Validation, null);
            Volume.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Mass), Mass_Validation, null);
            Mass.PropertyChanged += InPropertyChanged;
            DataAccess.Init<byte?>(nameof(AggregateState), AggregateState_Validation, null);
            AggregateState.PropertyChanged += InPropertyChanged;
        }

        private void Validate_all()
        {
            Owner_Validation(Owner);
            PackName_Validation(PackName);
            PackNumberRecoded_Validation(PackNumberRecoded);
            PackNumber_Validation(PackNumber);
            PackTypeRecoded_Validation(PackTypeRecoded);
            PackType_Validation(PackType);
            PassportNumberRecoded_Validation(PassportNumberRecoded);
            PassportNumber_Validation(PassportNumber);
            PropertyCode_Validation(PropertyCode);
            ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
            TransporterOKPO_Validation(TransporterOKPO);
            Activity_Validation(Activity);
            Radionuclids_Validation(Radionuclids);
            Name_Validation(Name);
            Sort_Validation(Sort);
            Volume_Validation(Volume);
            Mass_Validation(Mass);
            ActivityMeasurementDate_Validation(ActivityMeasurementDate);
            AggregateState_Validation(AggregateState);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

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
                //if ((PassportNumberNote.Value == null) || (PassportNumberNote.Value == ""))
                //    value.AddError( "Заполните примечание");

                return true;
            }
            return true;
        }
        //PassportNumber property

        protected override bool OperationCode_Validation(RamAccess<short?> value)//OK
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            List<short> spr = new List<short>();    //HERE BINDS SPRAVOCHNIK
            bool flag = false;
            foreach (short item in spr)
            {
                if (item == value.Value)
                {
                    flag = true;
                }
            }
            if (!flag)
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
            }

            return false;
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
        //    value.ClearErrors();
        //    if ((value.Value == null) || value.Value.Equals(""))
        //    {
        //        value.AddError("Поле не заполнено");
        //        return false;
        //    }
        //    return true;
        //}
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
            value.ClearErrors(); return true;
        }
        //PassportNumberRecoded property

        //Name property
        public int? NameId { get; set; }
        [Attributes.Form_Property("Наименование")]
        public virtual RamAccess<string> Name
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Name));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Name), value);
                }
                OnPropertyChanged(nameof(Name));
            }
        }


        private bool Name_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //Name property

        //Sort property
        public int? SortId { get; set; }
        [Attributes.Form_Property("Вид")]
        public virtual RamAccess<byte?> Sort
        {
            get
            {

                {
                    return DataAccess.Get<byte?>(nameof(Sort));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Sort), value);
                }
                OnPropertyChanged(nameof(Sort));
            }
        }
        //If change this change validation

        private bool Sort_Validation(RamAccess<byte?> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!((value.Value >= 4) && (value.Value <= 12)))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Sort property

        //Radionuclids property
        public int? RadionuclidsId { get; set; }
        [Attributes.Form_Property("Радионуклиды")]
        public virtual RamAccess<string> Radionuclids
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Radionuclids));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(Radionuclids), value);
                }
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
                if (item.Item1.Equals(value.Value))
                {
                    return true;
                }
            }
            value.AddError("Недопустимое значение");
            return false;
        }
        //Radionuclids property

        //Activity property
        public int? ActivityId { get; set; }
        [Attributes.Form_Property("Активность, Бк")]
        public virtual RamAccess<string> Activity
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Activity));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Activity), value);
                }
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
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
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

        //ActivityMeasurementDate property
        public int? ActivityMeasurementDateId { get; set; }
        [Attributes.Form_Property("Дата измерения активности")]
        public virtual RamAccess<string> ActivityMeasurementDate
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(ActivityMeasurementDate));
                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(ActivityMeasurementDate), value);
                OnPropertyChanged(nameof(ActivityMeasurementDate));
            }
        }
        //if change this change validation

        private bool ActivityMeasurementDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
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
        //ActivityMeasurementDate property

        //Volume property
        public int? VolumeId { get; set; }
        [Attributes.Form_Property("Объем, куб. м")]
        public virtual RamAccess<string> Volume
        {
            get => DataAccess.Get<string>(nameof(Volume));
            set
            {
                {
                    DataAccess.Set(nameof(Volume), value);
                }
                OnPropertyChanged(nameof(Volume));
            }
        }


        private bool Volume_Validation(RamAccess<string> value)//TODO
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Volume property

        //Mass Property
        public int? MassId { get; set; }
        [Attributes.Form_Property("Масса, кг")]
        public virtual RamAccess<string> Mass
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Mass));
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
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!((value.Value.Contains('e') || value.Value.Contains('E'))))
            {
                value.AddError("Недопустимое значение");
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
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Mass Property

        //AggregateState property
public int? AggregateStateId { get; set; }
        [Attributes.Form_Property("Агрегатное состояние")]
        public virtual RamAccess<byte?> AggregateState//1 2 3
        {
            get
            {

                {
                    return DataAccess.Get<byte?>(nameof(AggregateState));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(AggregateState), value);
                }
                OnPropertyChanged(nameof(AggregateState));
            }
        }


        private bool AggregateState_Validation(RamAccess<byte?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if ((value.Value != 1) && (value.Value != 2) && (value.Value != 3))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //AggregateState property

        //PropertyCode property
        public int? PropertyCodeId { get; set; }
        [Attributes.Form_Property("Код собственности")]
        public virtual RamAccess<byte?> PropertyCode
        {
            get => DataAccess.Get<byte?>(nameof(PropertyCode));//OK
            set
            {



                {
                    DataAccess.Set(nameof(PropertyCode), value);
                }
                OnPropertyChanged(nameof(PropertyCode));
            }
        }


        private bool PropertyCode_Validation(RamAccess<byte?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!((value.Value >= 1) && (value.Value <= 9)))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //PropertyCode property

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
        //    value.ClearErrors();
        //    if ((value.Value == null) || value.Value.Equals(""))
        //    {
        //        value.AddError("Поле не заполнено");
        //        return false;
        //    }
        //    return true;
        //}
        ////OwnerNote property

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
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((OwnerNote == null) || OwnerNote.Equals(""))
                //    value.AddError( "Заполните примечание");
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
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            short tmp = (short)OperationCode.Value;
            bool a = (tmp >= 10) && (tmp <= 12);
            bool b = (tmp >= 41) && (tmp <= 43);
            bool c = (tmp >= 71) && (tmp <= 73);
            bool d = (tmp == 15) || (tmp == 17) || (tmp == 18) || (tmp == 46) ||
                (tmp == 47) || (tmp == 48) || (tmp == 53) || (tmp == 54) ||
                (tmp == 58) || (tmp == 61) || (tmp == 62) || (tmp == 65) ||
                (tmp == 67) || (tmp == 68) || (tmp == 75) || (tmp == 76);
            if (a || b || c || d)
            {
                ProviderOrRecieverOKPO.Value = "ОКПО ОТЧИТЫВАЮЩЕЙСЯ ОРГ";
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((ProviderOrRecieverOKPONote.Value == null) || ProviderOrRecieverOKPONote.Value.Equals(""))
                //    value.AddError( "Заполните примечание");
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
        //    value.ClearErrors();
        //    if ((value.Value == null) || value.Value.Equals(""))
        //    {
        //        value.AddError("Поле не заполнено");
        //        return false;
        //    }
        //    return true;
        //}
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


        private bool TransporterOKPO_Validation(RamAccess<string> value)//Done
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
                //    value.AddError( "Заполните примечание");
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
        //    value.ClearErrors(); return true;
        //}
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
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
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
        //    value.ClearErrors(); return true;
        //}
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
            if (value.Value.Equals("прим."))
            {
                //if ((PackTypeNote == null) || PackTypeNote.Equals(""))
                //    value.AddError( "Заполните примечание");// to do note handling
                return true;
            }
            return true;
        }
        //PackType property

        //PackTypeRecoded property
        public int? PackTypeRecodedId { get; set; }
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
        //    value.ClearErrors(); return true;
        //}
        ////PackTypeNote property

        //PackNumber property
        public int? PackNumberId { get; set; }
        [Attributes.Form_Property("Номер упаковки")]
        public virtual RamAccess<string> PackNumber
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackNumber));
                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(PackNumber), value);
                }
                OnPropertyChanged(nameof(PackNumber));
            }
        }
        //If change this change validation

        private bool PackNumber_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PackNumberNote == null) || PackNumberNote.Equals(""))
                //    value.AddError( "Заполните примечание");// to do note handling
                return true;
            }
            return true;
        }
        //PackNumber property

        ////PackNumberNote property
        //public virtual RamAccess<string> PackNumberNote
        //{
        //    get
        //    {

        //        {
        //            return DataAccess.Get<string>(nameof(PackNumberNote));//OK

        //        }

        //        {

        //        }
        //    }
        //    set
        //    {



        //        {
        //            DataAccess.Set(nameof(PackNumberNote), value);
        //        }
        //        OnPropertyChanged(nameof(PackNumberNote));
        //    }
        //}


        //private bool PackNumberNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors();
        //    if ((value.Value == null) || value.Value.Equals(""))
        //    {
        //        value.AddError("Поле не заполнено");
        //        return false;
        //    }
        //    return true;
        //}
        ////PackNumberNote property

        //PackNumberRecoded property
        public int? PackNumberRecodedId { get; set; }
        public virtual RamAccess<string> PackNumberRecoded
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackNumberRecoded));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(PackNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        //If change this change validation

        private bool PackNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        //PackNumberRecoded property

        protected override bool DocumentNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (value.Value == "прим.")
            {
                //if ((DocumentNumberNote == null) || DocumentNumberNote.Equals(""))
                //    value.AddError( "Заполните примечание");
                return true;
            }
            if (string.IsNullOrEmpty(value.Value))//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
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
        //    value.ClearErrors();
        //    if ((value.Value == null) || value.Value.Equals(""))
        //    {
        //        value.AddError("Поле не заполнено");
        //        return false;
        //    }
        //    return true;
        //}
        ////DocumentNumberNote property
    }
}
