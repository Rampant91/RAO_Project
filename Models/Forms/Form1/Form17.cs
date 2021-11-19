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
                var tmp = new RamAccess<bool>(Sum_Validation, Sum_DB);
                tmp.PropertyChanged += SumValueChanged;
                return tmp;
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
        [Attributes.Form_Property("наименование")]
        public RamAccess<string> PackName
        {
            get
            {
                if (!PackName_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(PackName_Validation, PackName_DB);
                    tmp.PropertyChanged += PackNameValueChanged;
                    return tmp;
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
        [Attributes.Form_Property("тип")]
        public RamAccess<string> PackType
        {
            get
            {
                if (!PackType_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(PackType_Validation, PackType_DB);
                    tmp.PropertyChanged += PackTypeValueChanged;
                    return tmp;
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
        [Attributes.Form_Property("номер упаковки (идентификационный код)")]
        public RamAccess<string> PackNumber
        {
            get
            {
                if (!PackNumber_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(PackNumber_Validation, PackNumber_DB);
                    tmp.PropertyChanged += PackNumberValueChanged;
                    return tmp;
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
        [Attributes.Form_Property("заводской номер")]
        public RamAccess<string> PackFactoryNumber
        {
            get
            {
                if (!PackFactoryNumber_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(PackFactoryNumber_Validation, PackFactoryNumber_DB);
                    tmp.PropertyChanged += PackFactoryNumberValueChanged;
                    return tmp;
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
        [Attributes.Form_Property("дата формирования")]
        public RamAccess<string> FormingDate
        {
            get
            {
                if (!FormingDate_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(FormingDate_Validation, FormingDate_DB);
                    tmp.PropertyChanged += FormingDateValueChanged;
                    return tmp;
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
        [Attributes.Form_Property("объем, куб. м")]
        public RamAccess<string> Volume
        {
            get
            {
                if (!Volume_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(Volume_Validation, Volume_DB);
                    tmp.PropertyChanged += VolumeValueChanged;
                    return tmp;
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    Volume_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
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
        [Attributes.Form_Property("масса брутто, т")]
        public RamAccess<string> Mass
        {
            get
            {
                if (!Mass_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(Mass_Validation, Mass_DB);
                    tmp.PropertyChanged += MassValueChanged;
                    return tmp;
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    Mass_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
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
        [Attributes.Form_Property("номер паспорта")]
        public RamAccess<string> PassportNumber
        {
            get
            {
                if (!PassportNumber_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(PassportNumber_Validation, PassportNumber_DB);
                    tmp.PropertyChanged += PassportNumberValueChanged;
                    return tmp;
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
        [Attributes.Form_Property("наименование радионуклида")]
        public RamAccess<string> Radionuclids
        {
            get
            {
                var tmp = new RamAccess<string>(Radionuclids_Validation, Radionuclids_DB);
                tmp.PropertyChanged += RadionuclidsValueChanged;
                return tmp;
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
        [Attributes.Form_Property("удельная активность, Бк/г")]
        public RamAccess<string> SpecificActivity
        {
            get
            {
                var tmp = new RamAccess<string>(SpecificActivity_Validation, SpecificActivity_DB);
                tmp.PropertyChanged += SpecificActivityValueChanged;
                return tmp;
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    SpecificActivity_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
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
        [Attributes.Form_Property("поставщика или получателя")]
        public RamAccess<string> ProviderOrRecieverOKPO
        {
            get
            {
                if (!ProviderOrRecieverOKPO_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(ProviderOrRecieverOKPO_Validation, ProviderOrRecieverOKPO_DB);
                    tmp.PropertyChanged += ProviderOrRecieverOKPOValueChanged;
                    return tmp;
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
                ProviderOrRecieverOKPO_DB = ((RamAccess<string>)Value).Value;
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

            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

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
        [Attributes.Form_Property("перевозчика")]
        public RamAccess<string> TransporterOKPO
        {
            get
            {
                if (!TransporterOKPO_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(TransporterOKPO_Validation, TransporterOKPO_DB);
                    tmp.PropertyChanged += TransporterOKPOValueChanged;
                    return tmp;
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
        [Attributes.Form_Property("наименование")]
        public RamAccess<string> StoragePlaceName
        {
            get
            {
                if (!StoragePlaceName_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(StoragePlaceName_Validation, StoragePlaceName_DB);
                    tmp.PropertyChanged += StoragePlaceNameValueChanged;
                    return tmp;
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
        [Attributes.Form_Property("код")]
        public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                if (!StoragePlaceCode_Hidden_Priv)
                {
                    var tmp = new RamAccess<string>(StoragePlaceCode_Validation, StoragePlaceCode_DB);
                    tmp.PropertyChanged += StoragePlaceCodeValueChanged;
                    return tmp;
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
            value.ClearErrors(); return true;
        }
        #endregion

        #region CodeRAO
        public string CodeRAO_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("код")]
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
        [Attributes.Form_Property("статус")]
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
        [Attributes.Form_Property("объем без упаковки, куб. м")]
        public RamAccess<string> VolumeOutOfPack
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    VolumeOutOfPack_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
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
        [Attributes.Form_Property("масса без упаковки (нетто), т")]
        public RamAccess<string> MassOutOfPack
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    MassOutOfPack_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
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
        public int? Quantity_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property("количество ОЗИИИ, шт.")]
        public RamAccess<int?> Quantity
        {
            get
            {
                var tmp = new RamAccess<int?>(Quantity_Validation, Quantity_DB);
                tmp.PropertyChanged += QuantityValueChanged;
                return tmp;
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
                Quantity_DB = ((RamAccess<int?>)Value).Value;
            }
        }
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
        #endregion

        #region TritiumActivity
        public string TritiumActivity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("тритий")]
        public RamAccess<string> TritiumActivity
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    TritiumActivity_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
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
        [Attributes.Form_Property("бета-, гамма-излучающие радионуклиды (исключая тритий)")]
        public RamAccess<string> BetaGammaActivity
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    BetaGammaActivity_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
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
        [Attributes.Form_Property("альфа-излучающие радионуклиды (исключая трансурановые)")]
        public RamAccess<string> AlphaActivity
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    AlphaActivity_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
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
        [Attributes.Form_Property("трансурановые радионуклиды")]
        public RamAccess<string> TransuraniumActivity
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
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    TransuraniumActivity_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
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
        [Attributes.Form_Property("Код переработки/сортировки РАО")]
        public RamAccess<string> RefineOrSortRAOCode //2 cyfer code or empty.
        {
            get
            {
                var tmp = new RamAccess<string>(RefineOrSortRAOCode_Validation, RefineOrSortRAOCode_DB);
                tmp.PropertyChanged += RefineOrSortRAOCodeValueChanged;
                return tmp;
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

        protected override bool OperationCode_Validation(RamAccess<short?> value)//OK
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!Spravochniks.SprOpCodes.Contains((short)value.Value))
            {
                value.AddError("Недопустимое значение");
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
            bool ab = (OperationCode.Value == 51) || (OperationCode.Value == 52);
            bool c = (OperationCode.Value == 68);
            bool d = (OperationCode.Value == 18) || (OperationCode.Value == 55);
            if (ab || c || d)
            {
                if (!tmp.Equals(OperationDate))
                {
                    value.AddError("Заполните примечание");// to do note handling
                    return true;
                }
            }
            return true;
        }

        #region IExcel
        public void ExcelRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelRow(worksheet, Row);
            worksheet.Cells[Row, 4].Value = PackName_DB;
            worksheet.Cells[Row, 5].Value = PackType_DB;
            worksheet.Cells[Row, 6].Value = PackFactoryNumber_DB;
            worksheet.Cells[Row, 7].Value = PackNumber_DB;
            worksheet.Cells[Row, 8].Value = FormingDate_DB;
            worksheet.Cells[Row, 9].Value = PassportNumber_DB;
            worksheet.Cells[Row, 10].Value = Volume_DB;
            worksheet.Cells[Row, 11].Value = Mass_DB;
            worksheet.Cells[Row, 12].Value = Radionuclids_DB;
            worksheet.Cells[Row, 13].Value = SpecificActivity_DB;
            worksheet.Cells[Row, 14].Value = DocumentVid_DB;
            worksheet.Cells[Row, 15].Value = DocumentNumber_DB;
            worksheet.Cells[Row, 16].Value = DocumentDate_DB;
            worksheet.Cells[Row, 17].Value = ProviderOrRecieverOKPO_DB;
            worksheet.Cells[Row, 18].Value = TransporterOKPO_DB;
            worksheet.Cells[Row, 19].Value = StoragePlaceName_DB;
            worksheet.Cells[Row, 20].Value = StoragePlaceCode_DB;

            worksheet.Cells[Row, 21].Value = CodeRAO_DB;
            worksheet.Cells[Row, 22].Value = StatusRAO_DB;
            worksheet.Cells[Row, 23].Value = VolumeOutOfPack_DB;
            worksheet.Cells[Row, 24].Value = MassOutOfPack_DB;
            worksheet.Cells[Row, 25].Value = Quantity_DB;
            worksheet.Cells[Row, 26].Value = TritiumActivity_DB;
            worksheet.Cells[Row, 27].Value = BetaGammaActivity_DB;
            worksheet.Cells[Row, 28].Value = AlphaActivity_DB;
            worksheet.Cells[Row, 29].Value = TransuraniumActivity_DB;
            worksheet.Cells[Row, 30].Value = RefineOrSortRAOCode_DB;

            worksheet.Cells[Row, 31].Value = Subsidy_DB;
            worksheet.Cells[Row, 32].Value = FcpNumber_DB;
        }

        public static void ExcelHeader(ExcelWorksheet worksheet)
        {
            Form1.ExcelHeader(worksheet);
            worksheet.Cells[1, 4].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(PackName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 5].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(PackType)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 6].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(PackFactoryNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 7].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(PackNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 8].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(FormingDate)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 9].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(PassportNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 10].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(Volume)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 11].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(Mass)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 12].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(Radionuclids)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 13].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(SpecificActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 14].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(DocumentVid)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 15].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(DocumentNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 16].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(DocumentDate)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 17].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(ProviderOrRecieverOKPO)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 18].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(TransporterOKPO)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 19].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(StoragePlaceName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 20].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(StoragePlaceCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;

            worksheet.Cells[1, 21].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(CodeRAO)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 22].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(StatusRAO)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 23].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(VolumeOutOfPack)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 24].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(MassOutOfPack)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 25].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(Quantity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 26].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(TritiumActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 27].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(BetaGammaActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 28].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(AlphaActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 29].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(TransuraniumActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 30].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(RefineOrSortRAOCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;

            worksheet.Cells[1, 31].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(Subsidy)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 32].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form17,Models").GetProperty(nameof(FcpNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
        }
        #endregion
    }
}
