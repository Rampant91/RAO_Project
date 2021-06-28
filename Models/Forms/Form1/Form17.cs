using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.7: Сведения о твердых кондиционированных РАО")]
    public class Form17 : Abstracts.Form1
    {
        public Form17() : base()
        {
            //FormNum.Value = "17";
            //NumberOfFields.Value = 43;
            Init();
            Validate_all();
        }

        private void Init()
        {
            _dataAccess.Init<string>(nameof(CodeRAO), CodeRAO_Validation, null);
            CodeRAO.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PackName), PackName_Validation, null);
            PackName.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PackNumberRecoded), PackNumberRecoded_Validation, null);
            PackNumberRecoded.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PackNumber), PackNumber_Validation, null);
            PackNumber.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PackFactoryNumber), PackFactoryNumber_Validation, null);
            PackFactoryNumber.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PackTypeRecoded), PackTypeRecoded_Validation, null);
            PackTypeRecoded.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PackType), PackType_Validation, null);
            PackType.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Volume), Volume_Validation, null);
            Volume.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Mass), Mass_Validation, null);
            Mass.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(ProviderOrRecieverOKPO), ProviderOrRecieverOKPO_Validation, null);
            ProviderOrRecieverOKPO.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(TransporterOKPO), TransporterOKPO_Validation, null);
            TransporterOKPO.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(TritiumActivity), TritiumActivity_Validation, null);
            TritiumActivity.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(BetaGammaActivity), BetaGammaActivity_Validation, null);
            BetaGammaActivity.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(AlphaActivity), AlphaActivity_Validation, null);
            AlphaActivity.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(TransuraniumActivity), TransuraniumActivity_Validation, null);
            TransuraniumActivity.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(FormingDate), FormingDate_Validation, null);
            FormingDate.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Radionuclids), Radionuclids_Validation, null);
            Radionuclids.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PassportNumber), PassportNumber_Validation, null);
            PassportNumber.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(RefineOrSortRAOCode), RefineOrSortRAOCode_Validation, null);
            RefineOrSortRAOCode.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(StatusRAO), StatusRAO_Validation, null);
            StatusRAO.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Subsidy), Subsidy_Validation, null);
            Subsidy.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<int?>(nameof(Quantity), Quantity_Validation, null);
            Quantity.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(FcpNumber), FcpNumber_Validation, null);
            FcpNumber.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(VolumeOutOfPack), VolumeOutOfPack_Validation, null);
            VolumeOutOfPack.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(MassOutOfPack), MassOutOfPack_Validation, null);
            MassOutOfPack.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(StoragePlaceName), StoragePlaceName_Validation, null);
            StoragePlaceName.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(StoragePlaceCode), StoragePlaceCode_Validation, null);
            StoragePlaceCode.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(SpecificActivity), SpecificActivity_Validation, null);
            SpecificActivity.PropertyChanged += InPropertyChanged;
        }

        private void Validate_all()
        {
            CodeRAO_Validation(CodeRAO);
            PackName_Validation(PackName);
            PackNumberRecoded_Validation(PackNumberRecoded);
            PackNumber_Validation(PackNumber);
            PackFactoryNumber_Validation(PackFactoryNumber);
            PackTypeRecoded_Validation(PackTypeRecoded);
            PackType_Validation(PackType);
            Volume_Validation(Volume);
            Mass_Validation(Mass);
            Radionuclids_Validation(Radionuclids);
            ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
            TransporterOKPO_Validation(TransporterOKPO);
            TritiumActivity_Validation(TritiumActivity);
            BetaGammaActivity_Validation(BetaGammaActivity);
            AlphaActivity_Validation(AlphaActivity);
            TransuraniumActivity_Validation(TransuraniumActivity);
            FormingDate_Validation(FormingDate);
            PassportNumber_Validation(PassportNumber);
            RefineOrSortRAOCode_Validation(RefineOrSortRAOCode);
            Subsidy_Validation(Subsidy);
            FcpNumber_Validation(FcpNumber);
            StatusRAO_Validation(StatusRAO);
            VolumeOutOfPack_Validation(VolumeOutOfPack);
            MassOutOfPack_Validation(MassOutOfPack);
            StoragePlaceName_Validation(StoragePlaceName);
            StoragePlaceCode_Validation(StoragePlaceCode);
            SpecificActivity_Validation(SpecificActivity);
            Quantity_Validation(Quantity);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //PackName property
        [Attributes.Form_Property("Наименование упаковки")]public int? PackNameId { get; set; }
        public virtual RamAccess<string> PackName
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
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            return true;
        }
        //PackName property

        //PackNameNote property
        public virtual RamAccess<string> PackNameNote
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
        [Attributes.Form_Property("Тип упаковки")]public int? PackTypeId { get; set; }
        public virtual RamAccess<string> PackType
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
            value.ClearErrors(); return true;}
        //PackType property

        //PackTypeRecoded property
        public virtual RamAccess<string> PackTypeRecoded
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
        public virtual RamAccess<string> PackTypeNote
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

        //PackNumber property
        [Attributes.Form_Property("Номер упаковки")]public int? PackNumberId { get; set; }
        public virtual RamAccess<string> PackNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackNumber));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(PackNumber), value);
                }
                OnPropertyChanged(nameof(PackNumber));
            }
        }
        //If change this change validation

        private bool PackNumber_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
        //PackNumber property

        //PackNumberNote property
        public virtual RamAccess<string> PackNumberNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackNumberNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(PackNumberNote), value);
                }
                OnPropertyChanged(nameof(PackNumberNote));
            }
        }


        private bool PackNumberNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            return true;
        }
        //PackNumberNote property

        //PackNumberRecoded property
        public int? PackNumberRecodedId { get; set; }
        public virtual RamAccess<string> PackNumberRecoded
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackNumberRecoded));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PackNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        //If change this change validation

        private bool PackNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
        //PackNumberRecoded property

        //PackFactoryNumber property
        [Attributes.Form_Property("Заводской номер упаковки")]public int? PackFactoryNumberId { get; set; }
        public virtual RamAccess<string> PackFactoryNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackFactoryNumber));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PackFactoryNumber), value);
                }
                OnPropertyChanged(nameof(PackFactoryNumber));
            }
        }


        private bool PackFactoryNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors(); return true;}
        //PackFactoryNumber property

        //FormingDate property
        [Attributes.Form_Property("Дата формирования")]public int? FormingDateId { get; set; }
        public virtual RamAccess<string> FormingDate
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FormingDate));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(FormingDate), value);
                }
                OnPropertyChanged(nameof(FormingDate));
            }
        }


        private bool FormingDate_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            return true;
        }
        //FormingDate property

        //Volume property
        [Attributes.Form_Property("Объем, куб. м")]public int? VolumeId { get; set; }
        public virtual RamAccess<string> Volume
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Volume));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Volume), value);
                }
                OnPropertyChanged(nameof(Volume));
            }
        }


        private bool Volume_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        //Volume property

        //Mass Property
        [Attributes.Form_Property("Масса брутто, т")]public int? MassId { get; set; }
        public virtual RamAccess<string> Mass
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Mass));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Mass), value);
                }
                OnPropertyChanged(nameof(Mass));
            }
        }


        private bool Mass_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        //Mass Property

        //PassportNumber property
        [Attributes.Form_Property("Номер паспорта")]public int? PassportNumberId { get; set; }
        public virtual RamAccess<string> PassportNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PassportNumber));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(PassportNumber), value);
                }
                OnPropertyChanged(nameof(PassportNumber));
            }
        }


        private bool PassportNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //PassportNumber property

        //Radionuclids property
        [Attributes.Form_Property("Наименования радионуклидов")]public int? RadionuclidsId { get; set; }
        public virtual RamAccess<string> Radionuclids
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Radionuclids));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(Radionuclids), value);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation

        private bool Radionuclids_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            List<Tuple<string, string>> spr = new List<Tuple<string, string>>();//Here binds spravochnik
            foreach (var item in spr)
            {
                if (item.Item2.Equals(value.Value))
                {
                    Radionuclids.Value = item.Item2;
return true;
                }
            }
            value.AddError( "Недопустимое значение");
            return false;
        }
        //Radionuclids property

        //SpecificActivity property
        [Attributes.Form_Property("Удельная активность, Бк/г")]public int? SpecificActivityId { get; set; }
        public virtual RamAccess<string> SpecificActivity
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(SpecificActivity));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SpecificActivity), value);
                }
                OnPropertyChanged(nameof(SpecificActivity));
            }
        }


        private bool SpecificActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (!(value.Value.Contains('e')||value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля");return false;}
            }
            catch
            {
                value.AddError( "Недопустимое значение"); return false;
            }
            return true;
        }
        //SpecificActivity property

        //ProviderOrRecieverOKPO property
        [Attributes.Form_Property("ОКПО поставщика/получателя")]public int? ProviderOrRecieverOKPOId { get; set; }
        public virtual RamAccess<string> ProviderOrRecieverOKPO
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ProviderOrRecieverOKPO));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPO), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }


        private bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return false;
            }
            if (value.Value.Equals("прим."))
            {
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
        //ProviderOrRecieverOKPO property

        //ProviderOrRecieverOKPONote property
        public virtual RamAccess<string> ProviderOrRecieverOKPONote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ProviderOrRecieverOKPONote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPONote), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
            }
        }


        private bool ProviderOrRecieverOKPONote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //ProviderOrRecieverOKPONote property

        //TransporterOKPO property
        [Attributes.Form_Property("ОКПО перевозчика")]public int? TransporterOKPOId { get; set; }
        public virtual RamAccess<string> TransporterOKPO
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(TransporterOKPO));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(TransporterOKPO), value);
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
                //if ((TransporterOKPONote.Value == null) || TransporterOKPONote.Value.Equals(""))
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
        //TransporterOKPO property

        //TransporterOKPONote property
        public virtual RamAccess<string> TransporterOKPONote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(TransporterOKPONote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(TransporterOKPONote), value);
                }
                OnPropertyChanged(nameof(TransporterOKPONote));
            }
        }


        private bool TransporterOKPONote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //TransporterOKPONote property

        //StoragePlaceName property
        [Attributes.Form_Property("Наименование ПХ")]public int? StoragePlaceNameId { get; set; }
        public virtual RamAccess<string> StoragePlaceName
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
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //StoragePlaceName property

        //StoragePlaceNameNote property
        public virtual RamAccess<string> StoragePlaceNameNote
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
        [Attributes.Form_Property("Код ПХ")]public int? StoragePlaceCodeId { get; set; }
        public virtual RamAccess<string> StoragePlaceCode //8 cyfer code or - .
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
            var lst = new List<string>();//HERE binds spr
            if (!lst.Contains(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //StoragePlaceCode property

        //Subsidy property
        [Attributes.Form_Property("Субсидия, %")]public int? SubsidyId { get; set; }
        public virtual RamAccess<string> Subsidy // 0<number<=100 or empty.
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
            value.ClearErrors();
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
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Subsidy property

        //FcpNumber property
        [Attributes.Form_Property("Номер мероприятия ФЦП")]public int? FcpNumberId { get; set; }
        public virtual RamAccess<string> FcpNumber
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

        //CodeRAO property
        [Attributes.Form_Property("Код РАО")]public int? CodeRAOId { get; set; }
        public virtual RamAccess<string> CodeRAO
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
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            var a = new Regex("^[0-9]{11}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            return true;
        }
        //CodeRAO property

        //StatusRAO property
        [Attributes.Form_Property("Статус РАО")]public int? StatusRAOId { get; set; }
        public virtual RamAccess<string> StatusRAO  //1 cyfer or OKPO.
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
            value.ClearErrors();
            if (value.Value.Length == 1)
            {
                int tmp;
                try
                {
                    tmp = int.Parse(value.Value);
                    if ((tmp < 1) || ((tmp > 4) && (tmp != 6) && (tmp != 9)))
                    {
                        value.AddError( "Недопустимое значение"); return false;
                    }
                }
                catch (Exception)
                {
                    value.AddError( "Недопустимое значение"); return false;
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

        //VolumeOutOfPack property
        [Attributes.Form_Property("Объем без упаковки, куб. м")]public int? VolumeOutOfPackId { get; set; }
        public virtual RamAccess<string> VolumeOutOfPack
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
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля"); return false;}
            }
            catch
            {
                value.AddError( "Недопустимое значение"); return false;
            }
            return true;
        }
        //VolumeOutOfPack property

        //MassOutOfPack Property
        [Attributes.Form_Property("Масса без упаковки, т")]public int? MassOutOfPackId { get; set; }
        public virtual RamAccess<string> MassOutOfPack
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
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (!(value.Value.Contains('e')||value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError( "Недопустимое значение");
                return false;
            }
            return true;
        }
        //MassOutOfPack Property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]public int? QuantityId { get; set; }
        public virtual RamAccess<int?> Quantity
        {
            get
            {
                
                {
                    return _dataAccess.Get<int?>(nameof(Quantity));//OK;
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(Quantity), value);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.

        private bool Quantity_Validation(RamAccess<int?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Quantity property

        //TritiumActivity property
        [Attributes.Form_Property("Активность трития, Бк")]public int? TritiumActivityId { get; set; }
        public virtual RamAccess<string> TritiumActivity
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
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]public int? BetaGammaActivityId { get; set; }
        public virtual RamAccess<string> BetaGammaActivity
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
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]public int? AlphaActivityId { get; set; }
        public virtual RamAccess<string> AlphaActivity
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
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        [Attributes.Form_Property("Активность трансурановых, Бк")]public int? TransuraniumActivityId { get; set; }
        public virtual RamAccess<string> TransuraniumActivity
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
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        [Attributes.Form_Property("Код переработки/сортировки РАО")]public int? RefineOrSortRAOCodeId { get; set; }
        public virtual RamAccess<string> RefineOrSortRAOCode //2 cyfer code or empty.
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RefineOrSortRAOCode));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(RefineOrSortRAOCode), value);
                }
                OnPropertyChanged(nameof(RefineOrSortRAOCode));
            }
        }
        //If change this change validation

        private bool RefineOrSortRAOCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
return false;
            }
            var a = new Regex("^[0-9][0-9]$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            return true;
        }
        //RefineOrSortRAOCode property

        protected override bool OperationCode_Validation(RamAccess<short?> value)//OK
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            List<short> spr = new List<short>();    //HERE BINDS SPRAVOCHNIK
            if (!spr.Contains((short)value.Value))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            bool a0 = value.Value == 1;
            bool a1 = value.Value == 10;
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
            if (!(a0 || a1 || a2 || a3 || a4 || a5 || a6 || a7 || a8 || a9 || a10 || a11))
            {
                value.AddError("Код операции не может быть использован в форме 1.7");
                return false;
            }
            return true;
        }
        protected override bool DocumentNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
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
            foreach (var item in spr)
            {
return false;
            }
            value.AddError( "Недопустимое значение");
            return true;
        }

        protected override bool DocumentDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError( "Недопустимое значение");
                return false;
            }
            bool ab = (OperationCode.Value == 51) || (OperationCode.Value == 52);
            bool c = (OperationCode.Value == 68);
            bool d = (OperationCode.Value == 18) || (OperationCode.Value == 55);
            if (ab || c || d)
                if (!value.Value.Equals(OperationDate))
                {
                    value.AddError("Заполните примечание");// to do note handling
                    return true;
                }
            return true;
        }
    }
}
