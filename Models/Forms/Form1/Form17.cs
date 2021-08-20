using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Spravochniki;
using System.Linq;
using System.ComponentModel;

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
            return false;
        }

        #region PackName
        public string PackName_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Наименование упаковки")]
        public RamAccess<string> PackName
        {
            get
            {
                var tmp = new RamAccess<string>(PackName_Validation, PackName_DB);
                tmp.PropertyChanged += PackNameValueChanged;
                return tmp;
            }
            set
            {
                PackName_DB = value.Value;
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
            List<string> spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region PackType
        public string PackType_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Тип упаковки")]
        public RamAccess<string> PackType
        {
            get
            {
                var tmp = new RamAccess<string>(PackType_Validation, PackType_DB);
                tmp.PropertyChanged += PackTypeValueChanged;
                return tmp;
            }
            set
            {
                PackType_DB = value.Value;
            }
        }//If change this change validation

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
        [NotMapped]
        [Attributes.Form_Property("Номер упаковки")]
        public RamAccess<string> PackNumber
        {
            get
            {
                var tmp = new RamAccess<string>(PackNumber_Validation, PackNumber_DB);
                tmp.PropertyChanged += PackNumberValueChanged;
                return tmp;
            }
            set
            {
                PackNumber_DB = value.Value;
            }
        }//If change this change validation

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
        [NotMapped]
        [Attributes.Form_Property("Заводской номер упаковки")]
        public RamAccess<string> PackFactoryNumber
        {
            get
            {
                var tmp = new RamAccess<string>(PackFactoryNumber_Validation, PackFactoryNumber_DB);
                tmp.PropertyChanged += PackFactoryNumberValueChanged;
                return tmp;
            }
            set
            {
                PackFactoryNumber_DB = value.Value;
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
        [NotMapped]
        [Attributes.Form_Property("Дата формирования")]
        public RamAccess<string> FormingDate
        {
            get
            {
                var tmp = new RamAccess<string>(FormingDate_Validation, FormingDate_DB);
                tmp.PropertyChanged += FormingDateValueChanged;
                return tmp;
            }
            set
            {
                FormingDate_DB = value.Value;
            }
        }
        private void FormingDateValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                FormingDate_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool FormingDate_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
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
        #endregion

        #region Volume
        public string Volume_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Объем, куб. м")]
        public RamAccess<string> Volume
        {
            get
            {
                var tmp = new RamAccess<string>(Volume_Validation, Volume_DB);
                tmp.PropertyChanged += VolumeValueChanged;
                return tmp;
            }
            set
            {
                Volume_DB = value.Value;
            }
        }
        private void VolumeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Volume_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool Volume_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
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
        [NotMapped]
        [Attributes.Form_Property("Масса брутто, т")]
        public RamAccess<string> Mass
        {
            get
            {
                var tmp = new RamAccess<string>(Mass_Validation, Mass_DB);
                tmp.PropertyChanged += MassValueChanged;
                return tmp;
            }
            set
            {
                Mass_DB = value.Value;
            }
        }
        private void MassValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Mass_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool Mass_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
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
        [NotMapped]
        [Attributes.Form_Property("Номер паспорта")]
        public RamAccess<string> PassportNumber
        {
            get
            {
                var tmp = new RamAccess<string>(PassportNumber_Validation, PassportNumber_DB);
                tmp.PropertyChanged += PassportNumberValueChanged;
                return tmp;
            }
            set
            {
                PassportNumber_DB = value.Value;
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
        [Attributes.Form_Property("Наименования радионуклидов")]
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
        #endregion

        #region SpecificActivity
        public string SpecificActivity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Удельная активность, Бк/г")]
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
            }
        }
        private void SpecificActivityValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                SpecificActivity_DB = ((RamAccess<string>)Value).Value;
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
        #endregion

        #region ProviderOrRecieverOKPO
        public string ProviderOrRecieverOKPO_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
        public RamAccess<string> ProviderOrRecieverOKPO
        {
            get
            {
                var tmp = new RamAccess<string>(ProviderOrRecieverOKPO_Validation, ProviderOrRecieverOKPO_DB);
                tmp.PropertyChanged += ProviderOrRecieverOKPOValueChanged;
                return tmp;
            }
            set
            {
                ProviderOrRecieverOKPO_DB = value.Value;
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
        [NotMapped]
        [Attributes.Form_Property("ОКПО перевозчика")]
        public RamAccess<string> TransporterOKPO
        {
            get
            {
                var tmp = new RamAccess<string>(TransporterOKPO_Validation, TransporterOKPO_DB);
                tmp.PropertyChanged += TransporterOKPOValueChanged;
                return tmp;
            }
            set
            {
                TransporterOKPO_DB = value.Value;
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
        [NotMapped]
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
            }
        }//If change this change validation

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
            List<string> spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region StoragePlaceCode
        public string StoragePlaceCode_DB { get; set; } = "";
        [NotMapped]
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
            }
        }//if change this change validation

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
            List<string> lst = new List<string>();//HERE binds spr
            if (!lst.Contains(value.Value))
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
        #endregion

        #region StatusRAO
        public string StatusRAO_DB { get; set; } = "";
        [NotMapped]
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
        public string VolumeOutOfPack_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Объем без упаковки, куб. м")]
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
        #endregion

        #region MassOutOfPack
        public string MassOutOfPack_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Масса без упаковки, т")]
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
        #endregion

        #region Quantity
        public int? Quantity_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property("Количество, шт.")]
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
        [Attributes.Form_Property("Активность трития, Бк")]
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
        #endregion

        #region BetaGammaActivity
        public string BetaGammaActivity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
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
        #endregion

        #region AlphaActivity
        public string AlphaActivity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
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
        #endregion

        #region TransuraniumActivity
        public string TransuraniumActivity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Активность трансурановых, Бк")]
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
            List<short> spr = new List<short>();    //HERE BINDS SPRAVOCHNIK
            if (!spr.Contains((short)value.Value))
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
                return false;
            }
            value.AddError("Недопустимое значение");
            return true;
        }
        protected override bool DocumentDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                return true;
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
            bool ab = (OperationCode.Value == 51) || (OperationCode.Value == 52);
            bool c = (OperationCode.Value == 68);
            bool d = (OperationCode.Value == 18) || (OperationCode.Value == 55);
            if (ab || c || d)
            {
                if (!value.Value.Equals(OperationDate))
                {
                    value.AddError("Заполните примечание");// to do note handling
                    return true;
                }
            }

            return true;
        }
    }
}
