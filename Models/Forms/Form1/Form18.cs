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
            FormNum.Value = "18";
            NumberOfFields.Value = 37;
            Init();
            Validate_all();
        }

        private void Init()
        {
            _dataAccess.Init<string>(nameof(CodeRAO), CodeRAO_Validation, null);
            CodeRAO.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(SpecificActivity), SpecificActivity_Validation, null);
            SpecificActivity.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<double?>(nameof(SaltConcentration), SaltConcentration_Validation, null);
            SaltConcentration.PropertyChanged += InPropertyChanged;
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
            _dataAccess.Init<string>(nameof(IndividualNumberZHRO), IndividualNumberZHRO_Validation, null);
            IndividualNumberZHRO.PropertyChanged += InPropertyChanged;
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
            _dataAccess.Init<string>(nameof(IndividualNumberZHROrecoded), IndividualNumberZHROrecoded_Validation, null);
            IndividualNumberZHROrecoded.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(FcpNumber), FcpNumber_Validation, null);
            FcpNumber.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Volume6), Volume6_Validation, null);
            Volume6.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Mass7), Mass7_Validation, null);
            Mass7.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Volume20), Volume6_Validation, null);
            Volume20.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Mass21), Mass7_Validation, null);
            Mass21.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(StoragePlaceName), StoragePlaceName_Validation, null);
            StoragePlaceName.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(StoragePlaceCode), StoragePlaceCode_Validation, null);
            StoragePlaceCode.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PassportNumberRecoded), PassportNumberRecoded_Validation, null);
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
        [Attributes.Form_Property("Индивидуальный номер ЖРО")]
        public RamAccess<string> IndividualNumberZHRO
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(IndividualNumberZHRO));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(IndividualNumberZHRO), value);
                }
                OnPropertyChanged(nameof(IndividualNumberZHRO));
            }
        }


        private bool IndividualNumberZHRO_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //IndividualNumberZHRO property

        //IndividualNumberZHROrecoded property
        public RamAccess<string> IndividualNumberZHROrecoded
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(IndividualNumberZHROrecoded));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(IndividualNumberZHROrecoded), value);
                }
                OnPropertyChanged(nameof(IndividualNumberZHROrecoded));
            }
        }


        private bool IndividualNumberZHROrecoded_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //IndividualNumberZHROrecoded property

        //PassportNumber property
        [Attributes.Form_Property("Номер паспорта")]
        public RamAccess<string> PassportNumber
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
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
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

        //PassportNumberNote property
        public RamAccess<string> PassportNumberNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PassportNumberNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PassportNumberNote), value);
                }
                OnPropertyChanged(nameof(PassportNumberNote));
            }
        }


        private bool PassportNumberNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //PassportNumberNote property

        //PassportNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public RamAccess<string> PassportNumberRecoded
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PassportNumberRecoded));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PassportNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PassportNumberRecoded));
            }
        }
        //If change this change validation

        private bool PassportNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
        //PassportNumberRecoded property

        //Volume6 property
        [Attributes.Form_Property("Объем, куб. м")]
        public RamAccess<string> Volume6
        {
            get
            {
                {
                    return _dataAccess.Get<string>(nameof(Volume6));
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(Volume6), value);
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
        //Volume6 property

        //Mass7 Property
        [Attributes.Form_Property("Масса, т")]
        public RamAccess<string> Mass7
        {
            get
            {
                {
                    return _dataAccess.Get<string>(nameof(Mass7));
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(Mass7), value);
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
        //Mass7 Property

        //SaltConcentration property
        [Attributes.Form_Property("Солесодержание, г/л")]
        public RamAccess<double?> SaltConcentration
        {
            get
            {
                
                {
                    return _dataAccess.Get<double?>(nameof(SaltConcentration));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SaltConcentration), value);
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
        [Attributes.Form_Property("Наименования радионуклидов")]
        public RamAccess<string> Radionuclids
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(Radionuclids));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(Radionuclids), value);
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation

        private bool Radionuclids_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
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
return false;
                }
            }
            return true;
        }
        //Radionuclids property

        //SpecificActivity property
        [Attributes.Form_Property("Удельная активность, Бк/г")]
        public RamAccess<string> SpecificActivity
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(SpecificActivity));
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
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля"); return false;}
            }
            catch
            {
                value.AddError( "Недопустимое значение");
                return false;
            }
            return true;
        }
        //SpecificActivity property

        //ProviderOrRecieverOKPO property
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
        public RamAccess<string> ProviderOrRecieverOKPO
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
            if ((value.Value == null))
            {
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

        //ProviderOrRecieverOKPONote property
        public RamAccess<string> ProviderOrRecieverOKPONote
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
        [Attributes.Form_Property("ОКПО перевозчика")]
        public RamAccess<string> TransporterOKPO
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
            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //TransporterOKPO property

        //TransporterOKPONote property
        public RamAccess<string> TransporterOKPONote
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
            var lst = new List<string>();//HERE binds spr
            if (!lst.Contains(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //StoragePlaceCode property

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
            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //StatusRAO property

        //Volume20 property
        [Attributes.Form_Property("Объем, куб. м")]
        public RamAccess<string> Volume20
        {
            get
            {
                {
                    return _dataAccess.Get<string>(nameof(Volume20));
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(Volume20), value);
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
        //Volume20 property

        //Mass21 Property
        [Attributes.Form_Property("Масса, т")]
        public RamAccess<string> Mass21
        {
            get
            {
                {
                    return _dataAccess.Get<string>(nameof(Mass21));
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(Mass21), value);
                OnPropertyChanged(nameof(Mass21));
            }
        }


        private bool Mass21_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (!((value.Value.Contains('e') || value.Value.Contains('E'))))
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
                value.AddError( "Недопустимое значение");
                return false;
            }
            return true;
        }
        //Mass21 Property

        //TritiumActivity property
        [Attributes.Form_Property("Активность трития, Бк")]
        public RamAccess<string> TritiumActivity
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
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля");return false;}
            }
            catch
            {
                value.AddError( "Недопустимое значение"); return false;
            }
            return true;
        }
        //TritiumActivity property

        //BetaGammaActivity property
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public RamAccess<string> BetaGammaActivity
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
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля");return false;}
            }
            catch
            {
                value.AddError( "Недопустимое значение"); return false;
            }
            return true;
        }
        //BetaGammaActivity property

        //AlphaActivity property
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public RamAccess<string> AlphaActivity
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
        //AlphaActivity property

        //TransuraniumActivity property
        [Attributes.Form_Property("Активность трансурановых, Бк")]
        public RamAccess<string> TransuraniumActivity
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
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля");return false;}
            }
            catch
            {
                value.AddError( "Недопустимое значение"); return false;
            }
            return true;
        }
        //TransuraniumActivity property

        //RefineOrSortRAOCode property
        [Attributes.Form_Property("Код переработки/сортировки РАО")]
        public RamAccess<string> RefineOrSortRAOCode //2 cyfer code or empty.
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
            if (OperationCode.Value == 55)
            {
                if (string.IsNullOrEmpty(value.Value))
                {
                    value.AddError( "Поле не заполнено");
return false;
                }
                var a = new Regex("^[0-9][0-9]$");
                if (!a.IsMatch(value.Value))
                {
                    value.AddError( "Недопустимое значение");
return false;
                }
            }
            
            {
                if (!string.IsNullOrEmpty(value.Value))
                {
                    value.AddError( "Недопустимое значение");
return false;
                }
            }
            return true;
        }
        //RefineOrSortRAOCode property

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
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
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
                if (value.Value == item.Item1)
                    return true;
            }
            value.AddError( "Недопустимое значение");
            return false;
        }

        protected override bool DocumentDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
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
            bool b = (OperationCode.Value == 68);
            bool c = (OperationCode.Value == 52) || (OperationCode.Value == 55);
            bool d = (OperationCode.Value == 18) || (OperationCode.Value == 51);
            if (b || c || d)
                if (!value.Value.Equals(OperationDate))
                {
                    value.AddError("Заполните примечание");//to do note handling
                    return false;
                }
            return true;
        }
    }
}
