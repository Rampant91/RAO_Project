using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.8: Сведения о жидких кондиционированных РАО")]
    public class Form18 : Abstracts.Form1
    {
        public Form18() : base()
        {
            //FormNum.Value = "18";
            //NumberOfFields.Value = 37;
            Init();
            Validate_all();
        }

        private void Init()
        {
            DataAccess.Init<string>(nameof(CodeRAO), CodeRAO_Validation, null);
            CodeRAO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(SpecificActivity), SpecificActivity_Validation, null);
            SpecificActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<double?>(nameof(SaltConcentration), SaltConcentration_Validation, null);
            SaltConcentration.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(ProviderOrRecieverOKPO), ProviderOrRecieverOKPO_Validation, null);
            ProviderOrRecieverOKPO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(TransporterOKPO), TransporterOKPO_Validation, null);
            TransporterOKPO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(TritiumActivity), TritiumActivity_Validation, null);
            TritiumActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(BetaGammaActivity), BetaGammaActivity_Validation, null);
            BetaGammaActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(AlphaActivity), AlphaActivity_Validation, null);
            AlphaActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(TransuraniumActivity), TransuraniumActivity_Validation, null);
            TransuraniumActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(IndividualNumberZHRO), IndividualNumberZHRO_Validation, null);
            IndividualNumberZHRO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Radionuclids), Radionuclids_Validation, null);
            Radionuclids.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PassportNumber), PassportNumber_Validation, null);
            PassportNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(RefineOrSortRAOCode), RefineOrSortRAOCode_Validation, null);
            RefineOrSortRAOCode.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(StatusRAO), StatusRAO_Validation, null);
            StatusRAO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Subsidy), Subsidy_Validation, null);
            Subsidy.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(IndividualNumberZHROrecoded), IndividualNumberZHROrecoded_Validation, null);
            IndividualNumberZHROrecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(FcpNumber), FcpNumber_Validation, null);
            FcpNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Volume6), Volume6_Validation, null);
            Volume6.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Mass7), Mass7_Validation, null);
            Mass7.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Volume20), Volume6_Validation, null);
            Volume20.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Mass21), Mass7_Validation, null);
            Mass21.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(StoragePlaceName), StoragePlaceName_Validation, null);
            StoragePlaceName.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(StoragePlaceCode), StoragePlaceCode_Validation, null);
            StoragePlaceCode.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PassportNumberRecoded), PassportNumberRecoded_Validation, null);
            PassportNumberRecoded.PropertyChanged += InPropertyChanged;
        }

        private void Validate_all()
        {
            CodeRAO_Validation(CodeRAO);
            IndividualNumberZHRO_Validation(IndividualNumberZHRO);
            IndividualNumberZHROrecoded_Validation(IndividualNumberZHROrecoded);
            SpecificActivity_Validation(SpecificActivity);
            SaltConcentration_Validation(SaltConcentration);
            Radionuclids_Validation(Radionuclids);
            ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
            TransporterOKPO_Validation(TransporterOKPO);
            TritiumActivity_Validation(TritiumActivity);
            BetaGammaActivity_Validation(BetaGammaActivity);
            AlphaActivity_Validation(AlphaActivity);
            TransuraniumActivity_Validation(TransuraniumActivity);
            PassportNumber_Validation(PassportNumber);
            RefineOrSortRAOCode_Validation(RefineOrSortRAOCode);
            Subsidy_Validation(Subsidy);
            FcpNumber_Validation(FcpNumber);
            StatusRAO_Validation(StatusRAO);
            Volume6_Validation(Volume6);
            Mass7_Validation(Mass7);
            Volume20_Validation(Volume20);
            Mass21_Validation(Mass21);
            StoragePlaceName_Validation(StoragePlaceName);
            StoragePlaceCode_Validation(StoragePlaceCode);
            PassportNumberRecoded_Validation(PassportNumberRecoded);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //IndividualNumberZHRO property
        public int? IndividualNumberZHROId { get; set; }
        [Attributes.Form_Property("Индивидуальный номер ЖРО")]
        public virtual RamAccess<string> IndividualNumberZHRO
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(IndividualNumberZHRO));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(IndividualNumberZHRO), value);
                }
                OnPropertyChanged(nameof(IndividualNumberZHRO));
            }
        }


        private bool IndividualNumberZHRO_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //IndividualNumberZHRO property

        //IndividualNumberZHROrecoded property
        public virtual RamAccess<string> IndividualNumberZHROrecoded
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(IndividualNumberZHROrecoded));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(IndividualNumberZHROrecoded), value);
                }
                OnPropertyChanged(nameof(IndividualNumberZHROrecoded));
            }
        }


        private bool IndividualNumberZHROrecoded_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //IndividualNumberZHROrecoded property

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
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PassportNumberNote.Value == null) || (PassportNumberNote.Value == ""))
                //{
                //    value.AddError("Поле не может быть пустым");//to do note handling
                //}
                return true;
            }
            return true;
        }
        //PassportNumber property

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
        //    value.ClearErrors(); return true;
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

        //Volume6 property
        public int? Volume6Id { get; set; }
        [Attributes.Form_Property("Объем, куб. м")]
        public virtual RamAccess<string> Volume6
        {
            get
            {
                {
                    return DataAccess.Get<string>(nameof(Volume6));
                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(Volume6), value);
                OnPropertyChanged(nameof(Volume6));
            }
        }


        private bool Volume6_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
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
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Volume6 property

        //Mass7 Property
        public int? Mass7Id { get; set; }
        [Attributes.Form_Property("Масса, т")]
        public virtual RamAccess<string> Mass7
        {
            get
            {
                {
                    return DataAccess.Get<string>(nameof(Mass7));
                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(Mass7), value);
                OnPropertyChanged(nameof(Mass7));
            }
        }


        private bool Mass7_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
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
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Mass7 Property

        //SaltConcentration property
        public int? SaltConcentrationId { get; set; }
        [Attributes.Form_Property("Солесодержание, г/л")]
        public virtual RamAccess<double?> SaltConcentration
        {
            get
            {

                {
                    return DataAccess.Get<double?>(nameof(SaltConcentration));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(SaltConcentration), value);
                }
                OnPropertyChanged(nameof(SaltConcentration));
            }
        }


        private bool SaltConcentration_Validation(RamAccess<double?> value)
        {
            value.ClearErrors();
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //SaltConcentration property

        //Radionuclids property
        public int? RadionuclidsId { get; set; }
        [Attributes.Form_Property("Наименования радионуклидов")]
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
            List<Tuple<string, string>> spr = new List<Tuple<string, string>>();//Here binds spravochnik
            foreach (Tuple<string, string> item in spr)
            {
                if (item.Item2.Equals(value.Value))
                {
                    Radionuclids.Value = item.Item2;
                    return false;
                }
            }
            return true;
        }
        //Radionuclids property

        //SpecificActivity property
        public int? SpecificActivityId { get; set; }
        [Attributes.Form_Property("Удельная активность, Бк/г")]
        public virtual RamAccess<string> SpecificActivity
        {
            get => DataAccess.Get<string>(nameof(SpecificActivity));
            set
            {


                {
                    DataAccess.Set(nameof(SpecificActivity), value);
                }
                OnPropertyChanged(nameof(SpecificActivity));
            }
        }


        private bool SpecificActivity_Validation(RamAccess<string> value)//TODO
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
        //SpecificActivity property

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
        //    value.ClearErrors(); return true;
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
            List<string> spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
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
        //    value.ClearErrors(); return true;
        //}
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
            List<string> lst = new List<string>();//HERE binds spr
            if (!lst.Contains(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //StoragePlaceCode property

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
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex a = new Regex("^[0-9]{11}$");
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
            if (value.Value.Length == 1)
            {
                int tmp;
                try
                {
                    tmp = int.Parse(value.Value);
                    if ((tmp < 1) || ((tmp > 4) && (tmp != 6) && (tmp != 9)))
                    {
                        value.AddError("Недопустимое значение"); return false;
                    }
                    return true;
                }
                catch (Exception)
                {
                    value.AddError("Недопустимое значение"); return false;
                }
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
        //StatusRAO property

        //Volume20 property
        public int? Volume20Id { get; set; }
        [Attributes.Form_Property("Объем, куб. м")]
        public virtual RamAccess<string> Volume20
        {
            get
            {
                {
                    return DataAccess.Get<string>(nameof(Volume20));
                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(Volume20), value);
                OnPropertyChanged(nameof(Volume20));
            }
        }


        private bool Volume20_Validation(RamAccess<string> value)//TODO
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
        //Volume20 property

        //Mass21 Property
        public int? Mass21Id { get; set; }
        [Attributes.Form_Property("Масса, т")]
        public virtual RamAccess<string> Mass21
        {
            get
            {
                {
                    return DataAccess.Get<string>(nameof(Mass21));
                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(Mass21), value);
                OnPropertyChanged(nameof(Mass21));
            }
        }


        private bool Mass21_Validation(RamAccess<string> value)//TODO
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
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Mass21 Property

        //TritiumActivity property
        public int? TritiumActivityId { get; set; }
        [Attributes.Form_Property("Активность трития, Бк")]
        public virtual RamAccess<string> TritiumActivity
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
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //TritiumActivity property

        //BetaGammaActivity property
        public int? BetaGammaActivityId { get; set; }
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public virtual RamAccess<string> BetaGammaActivity
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
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //BetaGammaActivity property

        //AlphaActivity property
        public int? AlphaActivityId { get; set; }
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public virtual RamAccess<string> AlphaActivity
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
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //AlphaActivity property

        //TransuraniumActivity property
        public int? TransuraniumActivityId { get; set; }
        [Attributes.Form_Property("Активность трансурановых, Бк")]
        public virtual RamAccess<string> TransuraniumActivity
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
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //TransuraniumActivity property

        //RefineOrSortRAOCode property
        public int? RefineOrSortRAOCodeId { get; set; }
        [Attributes.Form_Property("Код переработки/сортировки РАО")]
        public virtual RamAccess<string> RefineOrSortRAOCode //2 cyfer code or empty.
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(RefineOrSortRAOCode));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(RefineOrSortRAOCode), value);
                }
                OnPropertyChanged(nameof(RefineOrSortRAOCode));
            }
        }
        //If change this change validation

        private bool RefineOrSortRAOCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (OperationCode.Value == 55)
            {
                if (string.IsNullOrEmpty(value.Value))
                {
                    value.AddError("Поле не заполнено");
                    return false;
                }
                Regex a = new Regex("^[0-9][0-9]$");
                if (!a.IsMatch(value.Value))
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }

            {
                if (!string.IsNullOrEmpty(value.Value))
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            return true;
        }
        //RefineOrSortRAOCode property

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
            try
            {
                int tmp = int.Parse(value.Value);
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
            value.ClearErrors(); return true;
        }
        //FcpNumber property

        protected override bool OperationCode_Validation(RamAccess<short?> value)//OK
        {
            value.ClearErrors();
            List<short> spr = new List<short>();    //HERE BINDS SPRAVOCHNIK
            if (!spr.Contains((short)value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            bool a0 = value.Value == 1;
            bool a2 = value.Value == 18;
            bool a3 = value.Value == 55;
            bool a4 = value.Value == 63;
            bool a5 = value.Value == 64;
            bool a6 = value.Value == 68;
            bool a7 = value.Value == 97;
            bool a8 = value.Value == 98;
            bool a9 = value.Value == 99;
            bool a10 = (value.Value >= 21) && (value.Value <= 29);
            bool a11 = (value.Value >= 31) && (value.Value <= 39);
            if (!(a0 || a2 || a3 || a4 || a5 || a6 || a7 || a8 || a9 || a10 || a11))
            {
                value.AddError("Код операции не может быть использован в форме 1.7");
                return false;
            }
            return true;
        }

        protected override bool OperationDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
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

        protected override bool DocumentNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }

        protected override bool DocumentVid_Validation(RamAccess<byte?> value)
        {
            value.ClearErrors();
            List<Tuple<byte?, string>> spr = new List<Tuple<byte?, string>>
            {
                new Tuple<byte?, string>(0,""),
                new Tuple<byte?, string>(1,""),
                new Tuple<byte?, string>(2,""),
                new Tuple<byte?, string>(3,""),
                new Tuple<byte?, string>(4,""),
                new Tuple<byte?, string>(5,""),
                new Tuple<byte?, string>(6,""),
                new Tuple<byte?, string>(7,""),
                new Tuple<byte?, string>(8,""),
                new Tuple<byte?, string>(9,""),
                new Tuple<byte?, string>(10,""),
                new Tuple<byte?, string>(11,""),
                new Tuple<byte?, string>(12,""),
                new Tuple<byte?, string>(13,""),
                new Tuple<byte?, string>(14,""),
                new Tuple<byte?, string>(15,""),
                new Tuple<byte?, string>(19,""),
                new Tuple<byte?, string>(null,"")
            };   //HERE BINDS SPRAVOCHNICK
            foreach (Tuple<byte?, string> item in spr)
            {
                if (value.Value == item.Item1)
                {
                    return true;
                }
            }
            value.AddError("Недопустимое значение");
            return false;
        }

        protected override bool DocumentDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
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
            bool b = (OperationCode.Value == 68);
            bool c = (OperationCode.Value == 52) || (OperationCode.Value == 55);
            bool d = (OperationCode.Value == 18) || (OperationCode.Value == 51);
            if (b || c || d)
            {
                if (!value.Value.Equals(OperationDate))
                {
                    value.AddError("Заполните примечание");//to do note handling
                    return false;
                }
            }

            return true;
        }
    }
}
