using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Spravochniki;
using System.Linq;
using System.ComponentModel;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using Models.Collections;

namespace Models
{
    [Attributes.Form_Class("Форма 1.7: Сведения о твердых кондиционированных РАО")]
    public class Form17 : Abstracts.Form1
    {
        public Form17() : base()
        {
            FormNum.Value = "1.7";
            Validate_all();
        }
        private void Validate_all()
        {
            CodeRAO_Validation(CodeRAO);
            PackName_Validation(PackName);
            PackNumber_Validation(PackNumber);
            PackFactoryNumber_Validation(PackFactoryNumber);
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
        public override bool Object_Validation()
        {
            return !(CodeRAO.HasErrors||
            PackName.HasErrors||
            PackNumber.HasErrors||
            PackFactoryNumber.HasErrors||
            PackType.HasErrors||
            Volume.HasErrors||
            Mass.HasErrors||
            Radionuclids.HasErrors||
            ProviderOrRecieverOKPO.HasErrors||
            TransporterOKPO.HasErrors||
            TritiumActivity.HasErrors||
            BetaGammaActivity.HasErrors||
            AlphaActivity.HasErrors||
            TransuraniumActivity.HasErrors||
            FormingDate.HasErrors||
            PassportNumber.HasErrors||
            RefineOrSortRAOCode.HasErrors||
            Subsidy.HasErrors||
            FcpNumber.HasErrors||
            StatusRAO.HasErrors||
            VolumeOutOfPack.HasErrors||
            MassOutOfPack.HasErrors||
            StoragePlaceName.HasErrors||
            StoragePlaceCode.HasErrors||
            SpecificActivity.HasErrors||
            Quantity.HasErrors);
        }

        #region  Sum
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

        #region PackName
        public string PackName_DB { get; set; } = "";
        public bool PackName_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool PackName_Hidden
        {
            get => PackName_Hidden_Priv;
            set
            {
                PackName_Hidden_Priv = value;
            }
        }
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения об упаковке", "наименование","4")]
        public RamAccess<string> PackName
        {
            get
            {
                if (!PackName_Hidden_Priv)
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
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!PackName_Hidden_Priv)
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
            //List<string> spr = new List<string>();
            //if (!spr.Contains(value.Value))
            //{
            //    value.AddError("Недопустимое значение");
            //    return false;
            //}
            return true;
        }
        #endregion

        #region PackType
        public string PackType_DB { get; set; } = "";
        public bool PackType_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool PackType_Hidden
        {
            get => PackType_Hidden_Priv;
            set
            {
                PackType_Hidden_Priv = value;
            }
        }
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения об упаковке","тип","5")]
        public RamAccess<string> PackType
        {
            get
            {
                if (!PackType_Hidden_Priv)
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
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!PackType_Hidden_Priv)
                {
                    PackType_DB = value.Value;
                    OnPropertyChanged(nameof(PackType));
                }
            }
        }

        private void PackTypeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PackType_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PackType_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        #endregion

        #region PackNumber
        public string PackNumber_DB { get; set; } = "";
        public bool PackNumber_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool PackNumber_Hidden
        {
            get => PackNumber_Hidden_Priv;
            set
            {
                PackNumber_Hidden_Priv = value;
            }
        }
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения об упаковке","номер упаковки (идентификационный код)","7")]
        public RamAccess<string> PackNumber
        {
            get
            {
                if (!PackNumber_Hidden_Priv)
                {
                    if (Dictionary.ContainsKey(nameof(PackNumber)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(PackNumber)]).Value = PackNumber_DB;
                        return (RamAccess<string>)Dictionary[nameof(PackNumber)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(PackNumber_Validation, PackNumber_DB);
                        rm.PropertyChanged += PackNumberValueChanged;
                        Dictionary.Add(nameof(PackNumber), rm);
                        return (RamAccess<string>)Dictionary[nameof(PackNumber)];
                    }
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!PackNumber_Hidden_Priv)
                {
                    PackNumber_DB = value.Value;
                    OnPropertyChanged(nameof(PackNumber));
                }
            }
        }

        private void PackNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PackNumber_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PackNumber_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        #endregion

        #region PackFactoryNumber
        public string PackFactoryNumber_DB { get; set; } = "";
        public bool PackFactoryNumber_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool PackFactoryNumber_Hidden
        {
            get => PackFactoryNumber_Hidden_Priv;
            set
            {
                PackFactoryNumber_Hidden_Priv = value;
            }
        }
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения об упаковке","заводской номер","6")]
        public RamAccess<string> PackFactoryNumber
        {
            get
            {
                if (!PackFactoryNumber_Hidden_Priv)
                {
                    if (Dictionary.ContainsKey(nameof(PackFactoryNumber)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(PackFactoryNumber)]).Value = PackFactoryNumber_DB;
                        return (RamAccess<string>)Dictionary[nameof(PackFactoryNumber)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(PackFactoryNumber_Validation, PackFactoryNumber_DB);
                        rm.PropertyChanged += PackFactoryNumberValueChanged;
                        Dictionary.Add(nameof(PackFactoryNumber), rm);
                        return (RamAccess<string>)Dictionary[nameof(PackFactoryNumber)];
                    }
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!PackFactoryNumber_Hidden_Priv)
                {
                    PackFactoryNumber_DB = value.Value;
                    OnPropertyChanged(nameof(PackFactoryNumber));
                }
            }
        }
        private void PackFactoryNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PackFactoryNumber_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PackFactoryNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors(); return true;
        }
        #endregion

        #region FormingDate
        public string FormingDate_DB { get; set; } = "";
        public bool FormingDate_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool FormingDate_Hidden
        {
            get => FormingDate_Hidden_Priv;
            set
            {
                FormingDate_Hidden_Priv = value;
            }
        }
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения об упаковке","дата формирования","8")]
        public RamAccess<string> FormingDate
        {
            get
            {
                if (!FormingDate_Hidden_Priv)
                {
                    if (Dictionary.ContainsKey(nameof(FormingDate)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(FormingDate)]).Value = FormingDate_DB;
                        return (RamAccess<string>)Dictionary[nameof(FormingDate)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(FormingDate_Validation, FormingDate_DB);
                        rm.PropertyChanged += FormingDateValueChanged;
                        Dictionary.Add(nameof(FormingDate), rm);
                        return (RamAccess<string>)Dictionary[nameof(FormingDate)];
                    }
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!FormingDate_Hidden_Priv)
                {
                    FormingDate_DB = value.Value;
                    OnPropertyChanged(nameof(FormingDate));
                }
            }
        }
        private void FormingDateValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                FormingDate_DB = tmp;
            }
        }
        private bool FormingDate_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region Volume
        public string Volume_DB { get; set; } = "";
        public bool Volume_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool Volume_Hidden
        {
            get => Volume_Hidden_Priv;
            set
            {
                Volume_Hidden_Priv = value;
            }
        }
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО", "объем, куб. м","10")]
        public RamAccess<string> Volume
        {
            get
            {
                if (!Volume_Hidden_Priv)
                {
                    if (Dictionary.ContainsKey(nameof(Volume)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(Volume)]).Value = Volume_DB;
                        return (RamAccess<string>)Dictionary[nameof(Volume)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(Volume_Validation, Volume_DB);
                        rm.PropertyChanged += VolumeValueChanged;
                        Dictionary.Add(nameof(Volume), rm);
                        return (RamAccess<string>)Dictionary[nameof(Volume)];
                    }
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!Volume_Hidden_Priv)
                {
                    Volume_DB = value.Value;
                    OnPropertyChanged(nameof(Volume));
                }
            }
        }
        private void VolumeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        Volume_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                Volume_DB = value1;
            }
        }
        private bool Volume_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value)||(value.Value=="-"))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region Mass
        public string Mass_DB { get; set; } = "";
        public bool Mass_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool Mass_Hidden
        {
            get => Mass_Hidden_Priv;
            set
            {
                Mass_Hidden_Priv = value;
            }
        }
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО","масса брутто, т","11")]
        public RamAccess<string> Mass
        {
            get
            {
                if (!Mass_Hidden_Priv)
                {
                    if (Dictionary.ContainsKey(nameof(Mass)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(Mass)]).Value = Mass_DB;
                        return (RamAccess<string>)Dictionary[nameof(Mass)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(Mass_Validation, Mass_DB);
                        rm.PropertyChanged += MassValueChanged;
                        Dictionary.Add(nameof(Mass), rm);
                        return (RamAccess<string>)Dictionary[nameof(Mass)];
                    }
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!Mass_Hidden_Priv)
                {
                    Mass_DB = value.Value;
                    OnPropertyChanged(nameof(Mass));
                }
            }
        }
        private void MassValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        Mass_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                Mass_DB = value1;
            }
        }
        private bool Mass_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value) || (value.Value == "-"))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region PassportNumber
        public string PassportNumber_DB { get; set; } = "";
        public bool PassportNumber_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool PassportNumber_Hidden
        {
            get => PassportNumber_Hidden_Priv;
            set
            {
                PassportNumber_Hidden_Priv = value;
            }
        }
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО", "номер паспорта","9")]
        public RamAccess<string> PassportNumber
        {
            get
            {
                if (!PassportNumber_Hidden_Priv)
                {
                    if (Dictionary.ContainsKey(nameof(PassportNumber)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(PassportNumber)]).Value = PassportNumber_DB;
                        return (RamAccess<string>)Dictionary[nameof(PassportNumber)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(PassportNumber_Validation, PassportNumber_DB);
                        rm.PropertyChanged += PassportNumberValueChanged;
                        Dictionary.Add(nameof(PassportNumber), rm);
                        return (RamAccess<string>)Dictionary[nameof(PassportNumber)];
                    }
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!PassportNumber_Hidden_Priv)
                {
                    PassportNumber_DB = value.Value;
                    OnPropertyChanged(nameof(PassportNumber));
                }
            }
        }
        private void PassportNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PassportNumber_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PassportNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        #endregion

        #region Radionuclids
        public string Radionuclids_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО","наименование радионуклида","12")]
        public RamAccess<string> Radionuclids
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(Radionuclids)))
                {
                    ((RamAccess<string>)Dictionary[nameof(Radionuclids)]).Value = Radionuclids_DB;
                    return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
                }
                else
                {
                    var rm = new RamAccess<string>(Radionuclids_Validation, Radionuclids_DB);
                    rm.PropertyChanged += RadionuclidsValueChanged;
                    Dictionary.Add(nameof(Radionuclids), rm);
                    return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
                }
            }
            set
            {
                Radionuclids_DB = value.Value;
                OnPropertyChanged(nameof(Radionuclids));
            }
        }//If change this change validation

        private void RadionuclidsValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Radionuclids_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool Radionuclids_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("="))
            {
                return true;
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
        #endregion

        #region SpecificActivity
        public string SpecificActivity_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО","удельная активность, Бк/г","13")]
        public RamAccess<string> SpecificActivity
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(SpecificActivity)))
                {
                    ((RamAccess<string>)Dictionary[nameof(SpecificActivity)]).Value = SpecificActivity_DB;
                    return (RamAccess<string>)Dictionary[nameof(SpecificActivity)];
                }
                else
                {
                    var rm = new RamAccess<string>(SpecificActivity_Validation, SpecificActivity_DB);
                    rm.PropertyChanged += SpecificActivityValueChanged;
                    Dictionary.Add(nameof(SpecificActivity), rm);
                    return (RamAccess<string>)Dictionary[nameof(SpecificActivity)];
                }
            }
            set
            {
                SpecificActivity_DB = value.Value;
                OnPropertyChanged(nameof(SpecificActivity));
            }
        }
        private void SpecificActivityValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        SpecificActivity_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                }
                SpecificActivity_DB = value1;
            }
        }
        private bool SpecificActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("="))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region ProviderOrRecieverOKPO
        public string ProviderOrRecieverOKPO_DB { get; set; } = "";
        public bool ProviderOrRecieverOKPO_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool ProviderOrRecieverOKPO_Hidden
        {
            get => ProviderOrRecieverOKPO_Hidden_Priv;
            set
            {
                ProviderOrRecieverOKPO_Hidden_Priv = value;
            }
        }
        [NotMapped]
        [Attributes.Form_Property(true,"ОКПО","поставщика или получателя","17")]
        public RamAccess<string> ProviderOrRecieverOKPO
        {
            get
            {
                if (!ProviderOrRecieverOKPO_Hidden_Priv)
                {
                    if (Dictionary.ContainsKey(nameof(ProviderOrRecieverOKPO)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(ProviderOrRecieverOKPO)]).Value = ProviderOrRecieverOKPO_DB;
                        return (RamAccess<string>)Dictionary[nameof(ProviderOrRecieverOKPO)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(ProviderOrRecieverOKPO_Validation, ProviderOrRecieverOKPO_DB);
                        rm.PropertyChanged += ProviderOrRecieverOKPOValueChanged;
                        Dictionary.Add(nameof(ProviderOrRecieverOKPO), rm);
                        return (RamAccess<string>)Dictionary[nameof(ProviderOrRecieverOKPO)];
                    }
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!ProviderOrRecieverOKPO_Hidden_Priv)
                {
                    ProviderOrRecieverOKPO_DB = value.Value;
                    OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
                }
            }
        }
        private void ProviderOrRecieverOKPOValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                string value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                    if (OKSM.Contains(value1.ToUpper()))
                    {
                        value1 = value1.ToUpper();
                    }
                ProviderOrRecieverOKPO_DB = value1;
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
            if (value.Value.Equals("Минобороны"))
            {
                return true;
            }
            if (OKSM.Contains(value.Value.ToUpper()))
            {
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
        #endregion
        protected List<string> OKSM = new List<string>
            {
                "АФГАНИСТАН","АЛБАНИЯ","АНТАРКТИДА","АЛЖИР","АМЕРИКАНСКОЕ САМОА","АНДОРРА","АНГОЛА","АНТИГУА И БАРБУДА","АЗЕРБАЙДЖАН","АРГЕНТИНА","АВСТРАЛИЯ","АВСТРИЯ","БАГАМЫ","БАХРЕЙН",
                "БАНГЛАДЕШ","АРМЕНИЯ","БАРБАДОС","БЕЛЬГИЯ","БЕРМУДЫ","БУТАН","БОЛИВИЯ, МНОГОНАЦИОНАЛЬНОЕ ГОСУДАРСТВО","БОСНИЯ И ГЕРЦЕГОВИНА","БОТСВАНА","ОСТРОВ БУВЕ","БРАЗИЛИЯ","БЕЛИЗ",
                "БРИТАНСКАЯ ТЕРРИТОРИЯ В ИНДИЙСКОМ ОКЕАНЕ","СОЛОМОНОВЫ ОСТРОВА","ВИРГИНСКИЕ ОСТРОВА (БРИТАНСКИЕ)","БРУНЕЙ-ДАРУССАЛАМ","БОЛГАРИЯ","МЬЯНМА","БУРУНДИ","БЕЛАРУСЬ","КАМБОДЖА",
                "КАМЕРУН","КАНАДА","КАБО-ВЕРДЕ","ОСТРОВА КАЙМАН","ЦЕНТРАЛЬНО-АФРИКАНСКАЯ РЕСПУБЛИКА","ШРИ-ЛАНКА","ЧАД","ЧИЛИ","КИТАЙ","ТАЙВАНЬ (КИТАЙ)","ОСТРОВ РОЖДЕСТВА","КОКОСОВЫЕ (КИЛИНГ) ОСТРОВА",
                "КОЛУМБИЯ","КОМОРЫ","МАЙОТТА","КОНГО","КОНГО, ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","ОСТРОВА КУКА","КОСТА-РИКА","ХОРВАТИЯ","КУБА","КИПР","ЧЕХИЯ","БЕНИН","ДАНИЯ","ДОМИНИКА","ДОМИНИКАНСКАЯ РЕСПУБЛИКА",
                "ЭКВАДОР","ЭЛЬ-САЛЬВАДОР","ЭКВАТОРИАЛЬНАЯ ГВИНЕЯ","ЭФИОПИЯ","ЭРИТРЕЯ","ЭСТОНИЯ","ФАРЕРСКИЕ ОСТРОВА","ФОЛКЛЕНДСКИЕ ОСТРОВА (МАЛЬВИНСКИЕ)","ЮЖНАЯ ДЖОРДЖИЯ И ЮЖНЫЕ САНДВИЧЕВЫ ОСТРОВА",
                "ФИНЛЯНДИЯ","ЭЛАНДСКИЕ ОСТРОВА","ФРАНЦИЯ","ФРАНЦУЗСКАЯ ГВИАНА","БОНЭЙР, СИНТ-ЭСТАТИУС И САБА","НОВАЯ КАЛЕДОНИЯ","ВАНУАТУ","НОВАЯ ЗЕЛАНДИЯ","НИКАРАГУА","НИГЕР","ФИДЖИ",
                "ФРАНЦУЗСКАЯ ПОЛИНЕЗИЯ","ФРАНЦУЗСКИЕ ЮЖНЫЕ ТЕРРИТОРИИ","ДЖИБУТИ","ГАБОН","ГРУЗИЯ","ГАМБИЯ","ПАЛЕСТИНА, ГОСУДАРСТВО","ГЕРМАНИЯ","ГАНА","ГИБРАЛТАР","КИРИБАТИ","МАЛИ","МАЛЬТА",
                "ГРЕЦИЯ","ГРЕНЛАНДИЯ","ГРЕНАДА","ГВАДЕЛУПА","ГУАМ","ГВАТЕМАЛА","ГВИНЕЯ","ГАЙАНА","ГАИТИ","ОСТРОВ ХЕРД И ОСТРОВА МАКДОНАЛЬД","ПАПСКИЙ ПРЕСТОЛ (ГОСУДАРСТВО - ГОРОД ВАТИКАН)",
                "ГОНДУРАС","ГОНКОНГ","ВЕНГРИЯ","ИСЛАНДИЯ","ИНДИЯ","ИНДОНЕЗИЯ","ИРАН (ИСЛАМСКАЯ РЕСПУБЛИКА)","ИРАК","ИРЛАНДИЯ","ИЗРАИЛЬ","ИТАЛИЯ","КОТ Д'ИВУАР","ЯМАЙКА","ЯПОНИЯ","МАЛЬДИВЫ",
                "КАЗАХСТАН","ИОРДАНИЯ","КЕНИЯ","КОРЕЯ, НАРОДНО-ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","КОРЕЯ, РЕСПУБЛИКА","КУВЕЙТ","КИРГИЗИЯ","НИГЕРИЯ","НИУЭ","ОСТРОВ НОРФОЛК","НОРВЕГИЯ","СЕВЕРНЫЕ МАРИАНСКИЕ ОСТРОВА",
                "ЛАОССКАЯ НАРОДНО-ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","ЛИВАН","ЛЕСОТО","ЛАТВИЯ","ЛИБЕРИЯ","ЛИВИЯ","ЛИХТЕНШТЕЙН","ЛИТВА","ЛЮКСЕМБУРГ","МАКАО","МАДАГАСКАР","МАЛАВИ","МАЛАЙЗИЯ",
                "МАРТИНИКА","МАВРИТАНИЯ","МАВРИКИЙ","МЕКСИКА","МОНАКО","МОНГОЛИЯ","МОЛДОВА, РЕСПУБЛИКА","ЧЕРНОГОРИЯ","МОНТСЕРРАТ","МАРОККО","МОЗАМБИК","ОМАН","НАМИБИЯ","НАУРУ","НЕПАЛ",
                "АРУБА","СЕН-МАРТЕН (нидерландская часть)","МАЛЫЕ ТИХООКЕАНСКИЕ ОТДАЛЕННЫЕ ОСТРОВА СОЕДИНЕННЫХ ШТАТОВ","МИКРОНЕЗИЯ, ФЕДЕРАТИВНЫЕ ШТАТЫ","МАРШАЛЛОВЫ ОСТРОВА","КЮРАСАО",
                "ПАЛАУ","ПАКИСТАН","ПАНАМА","ПАПУА-НОВАЯ ГВИНЕЯ","ПАРАГВАЙ","ПЕРУ","ФИЛИППИНЫ","ПИТКЕРН","ПОЛЬША","ПОРТУГАЛИЯ","ГВИНЕЯ-БИСАУ","ТИМОР-ЛЕСТЕ","ШВЕЦИЯ","ШВЕЙЦАРИЯ","НИДЕРЛАНДЫ",
                "ПУЭРТО-РИКО","КАТАР","РЕЮНЬОН","РУМЫНИЯ","РОССИЯ","РУАНДА","СЕН-БАРТЕЛЕМИ","СВЯТАЯ ЕЛЕНА, ОСТРОВ ВОЗНЕСЕНИЯ, ТРИСТАН-ДА-КУНЬЯ","СЕНТ-КИТС И НЕВИС","АНГИЛЬЯ","СЕНТ-ЛЮСИЯ",
                "СЕН-МАРТЕН (французская часть)","СЕН-ПЬЕР И МИКЕЛОН","СЕНТ-ВИНСЕНТ И ГРЕНАДИНЫ","САН-МАРИНО","САН-ТОМЕ И ПРИНСИПИ","САУДОВСКАЯ АРАВИЯ","СЕНЕГАЛ","СЕРБИЯ","СЕЙШЕЛЫ","ЮЖНЫЙ СУДАН",
                "СЬЕРРА-ЛЕОНЕ","СИНГАПУР","СЛОВАКИЯ","ВЬЕТНАМ","СЛОВЕНИЯ","СОМАЛИ","ЮЖНАЯ АФРИКА","ЗИМБАБВЕ","ИСПАНИЯ","ЗАПАДНАЯ САХАРА","СУДАН","СУРИНАМ","ШПИЦБЕРГЕН И ЯН МАЙЕН","ЭСВАТИНИ",
                "СИРИЙСКАЯ АРАБСКАЯ РЕСПУБЛИКА","ТАДЖИКИСТАН","ТАИЛАНД","ТОГО","ТОКЕЛАУ","ТОНГА","ТРИНИДАД И ТОБАГО","ОБЪЕДИНЕННЫЕ АРАБСКИЕ ЭМИРАТЫ","ТУНИС","ТУРЦИЯ","ТУРКМЕНИСТАН","ОСТРОВА ТЕРКС И КАЙКОС",
                "ТУВАЛУ","УГАНДА","УКРАИНА","СЕВЕРНАЯ МАКЕДОНИЯ","ЕГИПЕТ","СОЕДИНЕННОЕ КОРОЛЕВСТВО","ГЕРНСИ","ДЖЕРСИ","ОСТРОВ МЭН","ТАНЗАНИЯ, ОБЪЕДИНЕННАЯ РЕСПУБЛИКА","СОЕДИНЕННЫЕ ШТАТЫ",
                "ВИРГИНСКИЕ ОСТРОВА (США)","БУРКИНА-ФАСО","УРУГВАЙ","УЗБЕКИСТАН","ВЕНЕСУЭЛА (БОЛИВАРИАНСКАЯ РЕСПУБЛИКА)","УОЛЛИС И ФУТУНА","САМОА","ЙЕМЕН","ЗАМБИЯ","АБХАЗИЯ","ЮЖНАЯ ОСЕТИЯ"
            };
        #region TransporterOKPO
        public string TransporterOKPO_DB { get; set; } = "";
        public bool TransporterOKPO_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool TransporterOKPO_Hidden
        {
            get => TransporterOKPO_Hidden_Priv;
            set
            {
                TransporterOKPO_Hidden_Priv = value;
            }
        }
        [NotMapped]
        [Attributes.Form_Property(true,"ОКПО","перевозчика","18")]
        public RamAccess<string> TransporterOKPO
        {
            get
            {
                if (!TransporterOKPO_Hidden_Priv)
                {
                    if (Dictionary.ContainsKey(nameof(TransporterOKPO)))
                    {
                        ((RamAccess<string>)Dictionary[nameof(TransporterOKPO)]).Value = TransporterOKPO_DB;
                        return (RamAccess<string>)Dictionary[nameof(TransporterOKPO)];
                    }
                    else
                    {
                        var rm = new RamAccess<string>(TransporterOKPO_Validation, TransporterOKPO_DB);
                        rm.PropertyChanged += TransporterOKPOValueChanged;
                        Dictionary.Add(nameof(TransporterOKPO), rm);
                        return (RamAccess<string>)Dictionary[nameof(TransporterOKPO)];
                    }
                }
                else
                {
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!TransporterOKPO_Hidden_Priv)
                {
                    TransporterOKPO_DB = value.Value;
                    OnPropertyChanged(nameof(TransporterOKPO));
                }
            }
        }
        private void TransporterOKPOValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                TransporterOKPO_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool TransporterOKPO_Validation(RamAccess<string> value)//Done
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            if (value.Value.Equals("Минобороны") || value.Value.Equals("-"))
            {
                return true;
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
            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
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
            set
            {
                StoragePlaceName_Hidden_Priv = value;
            }
        }
        [NotMapped]
        [Attributes.Form_Property(true,"Пункт хранения","наименование","19")]
        public RamAccess<string> StoragePlaceName
        {
            get
            {
                if (!StoragePlaceName_Hidden_Priv)
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
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!StoragePlaceName_Hidden_Priv)
                {
                    StoragePlaceName_DB = value.Value;
                    OnPropertyChanged(nameof(StoragePlaceName));
                }
            }
        }

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
                value.AddError("Недопустимое значение");
                return false;
            }
            //List<string> spr = new List<string>();
            //if (!spr.Contains(value.Value))
            //{
            //    value.AddError("Недопустимое значение");
            //    return false;
            //}
            return true;
        }
        #endregion

        #region StoragePlaceCode
        public string StoragePlaceCode_DB { get; set; } = "";
        public bool StoragePlaceCode_Hidden_Priv { get; set; } = false;
        [NotMapped]
        public bool StoragePlaceCode_Hidden
        {
            get => StoragePlaceCode_Hidden_Priv;
            set
            {
                StoragePlaceCode_Hidden_Priv = value;
            }
        }
        [NotMapped]
        [Attributes.Form_Property(true,"Пункт хранения","код","20")]
        public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                if (!StoragePlaceCode_Hidden_Priv)
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
                    var tmp = new RamAccess<string>(null, null);
                    return tmp;
                }
            }
            set
            {
                if (!StoragePlaceCode_Hidden_Priv)
                {
                    StoragePlaceCode_DB = value.Value;
                    OnPropertyChanged(nameof(StoragePlaceCode));
                }
            }
        }

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
            //List<string> lst = new List<string>();//HERE binds spr
            //if (!lst.Contains(value.Value))
            //{
            //    value.AddError("Недопустимое значение"); return false;
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
        #endregion

        #region Subsidy
        public string Subsidy_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true, "null-31","Субсидия, %","31")]
        public RamAccess<string> Subsidy // 0<number<=100 or empty.
        {
            get
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
            if (value.Value.Equals("-"))
            {
                return true;
            }
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
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region FcpNumber
        public string FcpNumber_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true, "null-32","Номер мероприятия ФЦП","32")]
        public RamAccess<string> FcpNumber
        {
            get
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
            value.ClearErrors();
            return true;
        }
        #endregion

        #region CodeRAO
        public string CodeRAO_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО", "код","21")]
        public RamAccess<string> CodeRAO
        {
            get
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
                var tmp = ((RamAccess<string>)Value).Value.ToLower();
                tmp = tmp.Replace("х", "x");
                CodeRAO_DB = tmp;
            }
        }
        private bool CodeRAO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if(string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
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
        #endregion

        #region StatusRAO
        public string StatusRAO_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО","статус","22")]
        public RamAccess<string> StatusRAO  //1 cyfer or OKPO.
        {
            get
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
                value.AddError("Поле не заполнено");
                return false;
            }
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
                }
                catch (Exception)
                {
                    value.AddError("Недопустимое значение"); return false;
                }
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
        #endregion

        #region VolumeOutOfPack
        public string VolumeOutOfPack_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО","объем без упаковки, куб. м","23")]
        public RamAccess<string> VolumeOutOfPack
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
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region MassOutOfPack
        public string MassOutOfPack_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО","масса без упаковки (нетто), т","24")]
        public RamAccess<string> MassOutOfPack
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
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region Quantity
        public string Quantity_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО","количество ОЗИИИ, шт.","25")]
        public RamAccess<string> Quantity
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(Quantity)))
                {
                    ((RamAccess<string>)Dictionary[nameof(Quantity)]).Value = Quantity_DB;
                    return (RamAccess<string>)Dictionary[nameof(Quantity)];
                }
                else
                {
                    var rm = new RamAccess<string>(Quantity_Validation, Quantity_DB);
                    rm.PropertyChanged += QuantityValueChanged;
                    Dictionary.Add(nameof(Quantity), rm);
                    return (RamAccess<string>)Dictionary[nameof(Quantity)];
                }
            }
            set
            {
                Quantity_DB = value.Value;
                OnPropertyChanged(nameof(Quantity));
            }
        }// positive int.

        private void QuantityValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Quantity_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool Quantity_Validation(RamAccess<string> value)//Ready
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
        #endregion

        #region TritiumActivity
        public string TritiumActivity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО","тритий","26")]
        public RamAccess<string> TritiumActivity
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
            if(string.IsNullOrEmpty(value.Value))
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region BetaGammaActivity
        public string BetaGammaActivity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО","бета-, гамма-излучающие радионуклиды (исключая тритий)","27")]
        public RamAccess<string> BetaGammaActivity
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
            if(string.IsNullOrEmpty(value.Value))
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region AlphaActivity
        public string AlphaActivity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО", "альфа-излучающие радионуклиды (исключая трансурановые)","28")]
        public RamAccess<string> AlphaActivity
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
            if(string.IsNullOrEmpty(value.Value))
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region TransuraniumActivity
        public string TransuraniumActivity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО","трансурановые радионуклиды","29")]
        public RamAccess<string> TransuraniumActivity
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
            if(string.IsNullOrEmpty(value.Value))
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region RefineOrSortRAOCode
        public string RefineOrSortRAOCode_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения о РАО","Код переработки/сортировки РАО","30")]
        public RamAccess<string> RefineOrSortRAOCode //2 cyfer code or empty.
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(RefineOrSortRAOCode)))
                {
                    ((RamAccess<string>)Dictionary[nameof(RefineOrSortRAOCode)]).Value = RefineOrSortRAOCode_DB;
                    return (RamAccess<string>)Dictionary[nameof(RefineOrSortRAOCode)];
                }
                else
                {
                    var rm = new RamAccess<string>(RefineOrSortRAOCode_Validation, RefineOrSortRAOCode_DB);
                    rm.PropertyChanged += RefineOrSortRAOCodeValueChanged;
                    Dictionary.Add(nameof(RefineOrSortRAOCode), rm);
                    return (RamAccess<string>)Dictionary[nameof(RefineOrSortRAOCode)];
                }
            }
            set
            {
                RefineOrSortRAOCode_DB = value.Value;
                OnPropertyChanged(nameof(RefineOrSortRAOCode));
            }
        }//If change this change validation

        private void RefineOrSortRAOCodeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                RefineOrSortRAOCode_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool RefineOrSortRAOCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return false;
            }
            if (!Spravochniks.SprRifineOrSortCodes.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        protected override bool OperationCode_Validation(RamAccess<string> value)//OK
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!Spravochniks.SprOpCodes.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            bool a0 = value.Value == "01";
            bool a1 = value.Value == "10";
            bool a2 = value.Value == "18";
            bool a3 = value.Value == "55";
            bool a4 = value.Value == "63";
            bool a5 = value.Value == "64";
            bool a6 = value.Value == "68";
            bool a7 = value.Value == "97";
            bool a8 = value.Value == "98";
            bool a9 = value.Value == "99";
            bool a10 = (int.Parse(value.Value) >= 21) && (int.Parse(value.Value) <= 29);
            bool a11 = (int.Parse(value.Value) >= 31) && (int.Parse(value.Value) <= 39);
            if (!(a0 || a1 || a2 || a3 || a4 || a5 || a6 || a7 || a8 || a9 || a10 || a11))
            {
                value.AddError("Код операции не может быть использован в форме 1.7");
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
            foreach (Tuple<byte?, string> item in Spravochniks.SprDocumentVidName)
            {
                return false;
            }
            value.AddError("Недопустимое значение");
            return true;
        }
        protected override bool DocumentDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if(string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            bool ab = (OperationCode.Value == "51") || (OperationCode.Value == "52");
            bool c =  (OperationCode.Value == "68");
            bool d =  (OperationCode.Value == "18") || (OperationCode.Value == "55");
            if (ab || c || d)
            {
                if (!tmp.Equals(OperationDate))
                {
                    //value.AddError("Заполните примечание");// to do note handling
                    return true;
                }
            }
            return true;
        }

        #region IExcel
        public int ExcelRow(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = PackName_DB;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = PackType_DB;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = PackFactoryNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = PackNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = FormingDate_DB;
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = PassportNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = Volume_DB;
            worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = Mass_DB;
            worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = Radionuclids_DB;
            worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = SpecificActivity_DB;
            worksheet.Cells[Row + (Transpon == false ? 10 : 0), Column + (Transpon == true ? 10 : 0)].Value = DocumentVid_DB;
            worksheet.Cells[Row + (Transpon == false ? 11 : 0), Column + (Transpon == true ? 11 : 0)].Value = DocumentNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 12 : 0), Column + (Transpon == true ? 12 : 0)].Value = DocumentDate_DB;
            worksheet.Cells[Row + (Transpon == false ? 13 : 0), Column + (Transpon == true ? 13 : 0)].Value = ProviderOrRecieverOKPO_DB;
            worksheet.Cells[Row + (Transpon == false ? 14 : 0), Column + (Transpon == true ? 14 : 0)].Value = TransporterOKPO_DB;
            worksheet.Cells[Row + (Transpon == false ? 15 : 0), Column + (Transpon == true ? 15 : 0)].Value = StoragePlaceName_DB;
            worksheet.Cells[Row + (Transpon == false ? 16 : 0), Column + (Transpon == true ? 16 : 0)].Value = StoragePlaceCode_DB;
            worksheet.Cells[Row + (Transpon == false ? 17 : 0), Column + (Transpon == true ? 17 : 0)].Value = CodeRAO_DB;
            worksheet.Cells[Row + (Transpon == false ? 18 : 0), Column + (Transpon == true ? 18 : 0)].Value = StatusRAO_DB;
            worksheet.Cells[Row + (Transpon == false ? 19 : 0), Column + (Transpon == true ? 19 : 0)].Value = VolumeOutOfPack_DB;
            worksheet.Cells[Row + (Transpon == false ? 20 : 0), Column + (Transpon == true ? 20 : 0)].Value = MassOutOfPack_DB;
            worksheet.Cells[Row + (Transpon == false ? 21 : 0), Column + (Transpon == true ? 21 : 0)].Value = Quantity_DB;
            worksheet.Cells[Row + (Transpon == false ? 22 : 0), Column + (Transpon == true ? 22 : 0)].Value = TritiumActivity_DB;
            worksheet.Cells[Row + (Transpon == false ? 23 : 0), Column + (Transpon == true ? 23 : 0)].Value = BetaGammaActivity_DB;
            worksheet.Cells[Row + (Transpon == false ? 24 : 0), Column + (Transpon == true ? 24 : 0)].Value = AlphaActivity_DB;
            worksheet.Cells[Row + (Transpon == false ? 25 : 0), Column + (Transpon == true ? 25 : 0)].Value = TransuraniumActivity_DB;
            worksheet.Cells[Row + (Transpon == false ? 26 : 0), Column + (Transpon == true ? 26 : 0)].Value = RefineOrSortRAOCode_DB;
            worksheet.Cells[Row + (Transpon == false ? 27 : 0), Column + (Transpon == true ? 27 : 0)].Value = Subsidy_DB;
            worksheet.Cells[Row + (Transpon == false ? 28 : 0), Column + (Transpon == true ? 28 : 0)].Value = FcpNumber_DB;

            return 29;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true)
        {
            var cnt = Form1.ExcelHeader(worksheet, Row, Column, Transpon);
            Column = Column + +(Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

           worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(PackName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(PackType)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(PackFactoryNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(PackNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(FormingDate)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(PassportNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(Volume)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(Mass)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(Radionuclids)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(SpecificActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 10 : 0), Column + (Transpon == true ? 10 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(DocumentVid)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 11 : 0), Column + (Transpon == true ? 11 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(DocumentNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 12 : 0), Column + (Transpon == true ? 12 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(DocumentDate)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 13 : 0), Column + (Transpon == true ? 13 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(ProviderOrRecieverOKPO)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 14 : 0), Column + (Transpon == true ? 14 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(TransporterOKPO)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 15 : 0), Column + (Transpon == true ? 15 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(StoragePlaceName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 16 : 0), Column + (Transpon == true ? 16 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(StoragePlaceCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 17 : 0), Column + (Transpon == true ? 17 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(CodeRAO)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 18 : 0), Column + (Transpon == true ? 18 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(StatusRAO)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 19 : 0), Column + (Transpon == true ? 19 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(VolumeOutOfPack)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 20 : 0), Column + (Transpon == true ? 20 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(MassOutOfPack)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 21 : 0), Column + (Transpon == true ? 21 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(Quantity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 22 : 0), Column + (Transpon == true ? 22 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(TritiumActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 23 : 0), Column + (Transpon == true ? 23 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(BetaGammaActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 24 : 0), Column + (Transpon == true ? 24 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(AlphaActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 25 : 0), Column + (Transpon == true ? 25 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(TransuraniumActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 26 : 0), Column + (Transpon == true ? 26 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(RefineOrSortRAOCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 27 : 0), Column + (Transpon == true ? 27 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(Subsidy)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
           worksheet.Cells[Row + (Transpon == false ? 28 : 0), Column + (Transpon == true ? 28 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(FcpNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];

            return 29;
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
                #region OperationCode (2)
                DataGridColumns OperationCodeR = ((Attributes.Form_PropertyAttribute)typeof(Form1).GetProperty(nameof(Form17.OperationCode)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                OperationCodeR.SetSizeColToAllLevels(88);
                OperationCodeR.Binding = nameof(Form17.OperationCode);
                NumberInOrderR += OperationCodeR;
                #endregion
                #region OperationDate (3)
                DataGridColumns OperationDateR = ((Attributes.Form_PropertyAttribute)typeof(Form1).GetProperty(nameof(Form17.OperationDate)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                OperationDateR.SetSizeColToAllLevels(88);
                OperationDateR.Binding = nameof(Form17.OperationDate);
                NumberInOrderR += OperationDateR;
                #endregion
                #region PackName (4)
                DataGridColumns PackNameR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.PackName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                PackNameR.SetSizeColToAllLevels(163);
                PackNameR.Binding = nameof(Form17.PackName);
                NumberInOrderR += PackNameR;
                #endregion
                #region PackType (5)
                DataGridColumns PackTypeR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.PackType)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                PackTypeR.SetSizeColToAllLevels(88);
                PackTypeR.Binding = nameof(Form17.PackType);
                NumberInOrderR += PackTypeR;
                #endregion
                #region PackFactoryNumber (6)
                DataGridColumns PackFactoryNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.PackFactoryNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                PackFactoryNumberR.SetSizeColToAllLevels(238);
                PackFactoryNumberR.Binding = nameof(Form17.PackFactoryNumber);
                NumberInOrderR += PackFactoryNumberR;
                #endregion
                #region PackNumber (7)
                DataGridColumns PackNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.PackNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                PackNumberR.SetSizeColToAllLevels(260);
                PackNumberR.Binding = nameof(Form17.PackNumber);
                NumberInOrderR += PackNumberR;
                #endregion
                #region FormingDate (8)
                DataGridColumns FormingDateR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.FormingDate)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                FormingDateR.SetSizeColToAllLevels(163);
                FormingDateR.Binding = nameof(Form17.FormingDate);
                NumberInOrderR += FormingDateR;
                #endregion
                #region PassportNumber (9)
                DataGridColumns PassportNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.PassportNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                PassportNumberR.SetSizeColToAllLevels(163);
                PassportNumberR.Binding = nameof(Form17.PassportNumber);
                NumberInOrderR += PassportNumberR;
                #endregion
                #region Volume (10)
                DataGridColumns VolumeR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.Volume)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                VolumeR.SetSizeColToAllLevels(88);
                VolumeR.Binding = nameof(Form17.Volume);
                NumberInOrderR += VolumeR;
                #endregion
                #region Mass (11)
                DataGridColumns MassR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.Mass)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                MassR.SetSizeColToAllLevels(95);
                MassR.Binding = nameof(Form17.Mass);
                NumberInOrderR += MassR;
                #endregion
                #region Radionuclids (12)
                DataGridColumns RadionuclidsR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.Radionuclids)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                RadionuclidsR.SetSizeColToAllLevels(170);
                RadionuclidsR.Binding = nameof(Form17.Radionuclids);
                NumberInOrderR += RadionuclidsR;
                #endregion
                #region SpecificActivity (13)
                DataGridColumns SpecificActivityR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.SpecificActivity)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                SpecificActivityR.SetSizeColToAllLevels(163);
                SpecificActivityR.Binding = nameof(Form17.SpecificActivity);
                NumberInOrderR += SpecificActivityR;
                #endregion
                #region DocumentVid (14)
                DataGridColumns DocumentVidR = ((Attributes.Form_PropertyAttribute)typeof(Form1).GetProperty(nameof(Form17.DocumentVid)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                DocumentVidR.SetSizeColToAllLevels(88);
                DocumentVidR.Binding = nameof(Form17.DocumentVid);
                NumberInOrderR += DocumentVidR;
                #endregion
                #region DocumentNumber (15)
                DataGridColumns DocumentNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form1).GetProperty(nameof(Form17.DocumentNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                DocumentNumberR.SetSizeColToAllLevels(103);
                DocumentNumberR.Binding = nameof(Form17.DocumentNumber);
                NumberInOrderR += DocumentNumberR;
                #endregion
                #region DocumentDate (16)
                DataGridColumns DocumentDateR = ((Attributes.Form_PropertyAttribute)typeof(Form1).GetProperty(nameof(Form17.DocumentDate)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                DocumentDateR.SetSizeColToAllLevels(88);
                DocumentDateR.Binding = nameof(Form17.DocumentDate);
                NumberInOrderR += DocumentDateR;
                #endregion
                #region ProviderOrRecieverOKPO (17)
                DataGridColumns ProviderOrRecieverOKPOR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.ProviderOrRecieverOKPO)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                ProviderOrRecieverOKPOR.SetSizeColToAllLevels(163);
                ProviderOrRecieverOKPOR.Binding = nameof(Form17.ProviderOrRecieverOKPO);
                NumberInOrderR += ProviderOrRecieverOKPOR;
                #endregion
                #region TransporterOKPO (18)
                DataGridColumns TransporterOKPOR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.TransporterOKPO)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                TransporterOKPOR.SetSizeColToAllLevels(163);
                TransporterOKPOR.Binding = nameof(Form17.TransporterOKPO);
                NumberInOrderR += TransporterOKPOR;
                #endregion
                #region StoragePlaceName (19)
                DataGridColumns StoragePlaceNameR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.StoragePlaceName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                StoragePlaceNameR.SetSizeColToAllLevels(103);
                StoragePlaceNameR.Binding = nameof(Form17.StoragePlaceName);
                NumberInOrderR += StoragePlaceNameR;
                #endregion
                #region StoragePlaceCode (20)
                DataGridColumns StoragePlaceCodeR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.StoragePlaceCode)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                StoragePlaceCodeR.SetSizeColToAllLevels(88);
                StoragePlaceCodeR.Binding = nameof(Form17.StoragePlaceCode);
                NumberInOrderR += StoragePlaceCodeR;
                #endregion
                #region CodeRAO (21)
                DataGridColumns CodeRAOR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.CodeRAO)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                CodeRAOR.SetSizeColToAllLevels(88);
                CodeRAOR.Binding = nameof(Form17.CodeRAO);
                NumberInOrderR += CodeRAOR;
                #endregion
                #region StatusRAO (22)
                DataGridColumns StatusRAOR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.StatusRAO)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                StatusRAOR.SetSizeColToAllLevels(88);
                StatusRAOR.Binding = nameof(Form17.StatusRAO);
                NumberInOrderR += StatusRAOR;
                #endregion
                #region VolumeOutOfPack (23)
                DataGridColumns VolumeOutOfPackR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.VolumeOutOfPack)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                VolumeOutOfPackR.SetSizeColToAllLevels(163);
                VolumeOutOfPackR.Binding = nameof(Form17.VolumeOutOfPack);
                NumberInOrderR += VolumeOutOfPackR;
                #endregion
                #region MassOutOfPack (24)
                DataGridColumns MassOutOfPackR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.MassOutOfPack)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                MassOutOfPackR.SetSizeColToAllLevels(170);
                MassOutOfPackR.Binding = nameof(Form17.MassOutOfPack);
                NumberInOrderR += MassOutOfPackR;
                #endregion
                #region Quantity (25)
                DataGridColumns QuantityR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.Quantity)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                QuantityR.SetSizeColToAllLevels(140);
                QuantityR.Binding = nameof(Form17.Quantity);
                NumberInOrderR += QuantityR;
                #endregion
                #region TritiumActivity (26)
                DataGridColumns TritiumActivityR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.TritiumActivity)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                TritiumActivityR.SetSizeColToAllLevels(163);
                TritiumActivityR.Binding = nameof(Form17.TritiumActivity);
                NumberInOrderR += TritiumActivityR;
                #endregion
                #region BetaGammaActivity (27)
                DataGridColumns BetaGammaActivityR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.BetaGammaActivity)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                BetaGammaActivityR.SetSizeColToAllLevels(350);
                BetaGammaActivityR.Binding = nameof(Form17.BetaGammaActivity);
                NumberInOrderR += BetaGammaActivityR;
                #endregion
                #region AlphaActivity (28)
                DataGridColumns AlphaActivityR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.AlphaActivity)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                AlphaActivityR.SetSizeColToAllLevels(365);
                AlphaActivityR.Binding = nameof(Form17.AlphaActivity);
                NumberInOrderR += AlphaActivityR;
                #endregion
                #region TransuraniumActivity (29)
                DataGridColumns TransuraniumActivityR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.TransuraniumActivity)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                TransuraniumActivityR.SetSizeColToAllLevels(200);
                TransuraniumActivityR.Binding = nameof(Form17.TransuraniumActivity);
                NumberInOrderR += TransuraniumActivityR;
                #endregion
                #region RefineOrSortRAOCode (30)
                DataGridColumns RefineOrSortRAOCodeR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.RefineOrSortRAOCode)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                RefineOrSortRAOCodeR.SetSizeColToAllLevels(210);
                RefineOrSortRAOCodeR.Binding = nameof(Form17.RefineOrSortRAOCode);
                NumberInOrderR += RefineOrSortRAOCodeR;
                #endregion
                #region Subsidy (31)
                DataGridColumns SubsidyR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.Subsidy)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                SubsidyR.SetSizeColToAllLevels(88);
                SubsidyR.Binding = nameof(Form17.Subsidy);
                NumberInOrderR += SubsidyR;
                #endregion
                #region FcpNumber (32)
                DataGridColumns FcpNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form17).GetProperty(nameof(Form17.FcpNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                FcpNumberR.SetSizeColToAllLevels(163);
                FcpNumberR.Binding = nameof(Form17.FcpNumber);
                NumberInOrderR += FcpNumberR;
                #endregion
                _DataGridColumns = NumberInOrderR;
            }
            return _DataGridColumns;
        }
        #endregion
    }
}
