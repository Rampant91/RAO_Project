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
    [Serializable]
    [Attributes.Form_Class("Форма 1.3: Сведения об ОРИ в виде отдельных изделий")]
    public class Form13 : Abstracts.Form1
    {
        public Form13() : base()
        {
            FormNum.Value = "1.3";
            Validate_all();
        }

        public bool _autoRN = false;
        public override bool Object_Validation()
        {
            return !(CreationDate.HasErrors||
            CreatorOKPO.HasErrors||
            Owner.HasErrors||
            PackName.HasErrors||
            PackNumber.HasErrors||
            PackType.HasErrors||
            PassportNumber.HasErrors||
            PropertyCode.HasErrors||
            ProviderOrRecieverOKPO.HasErrors||
            TransporterOKPO.HasErrors||
            FactoryNumber.HasErrors||
            AggregateState.HasErrors||
            Activity.HasErrors||
            Radionuclids.HasErrors||
            Type.HasErrors);
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
            TransporterOKPO_Validation(TransporterOKPO);
            FactoryNumber_Validation(FactoryNumber);
            AggregateState_Validation(AggregateState);
            Activity_Validation(Activity);
            Radionuclids_Validation(Radionuclids);
            Type_Validation(Type);
        }

        #region PassportNumber
        public string PassportNumber_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("номер паспорта")]
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

        protected bool PassportNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if(string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PassportNumberNote.Value == null)||(PassportNumberNote.Value == ""))
                //    value.AddError( "Заполните примечание");//to do note handling
                return true;
            }
            return true;
        }
        #endregion

        #region Type
        public string Type_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("тип")]
        public RamAccess<string> Type
        {
            get
            {
                var tmp = new RamAccess<string>(Type_Validation, Type_DB);
                tmp.PropertyChanged += TypeValueChanged;
                return tmp;
            }
            set
            {
                Type_DB = value.Value;
            }
        }
        private void TypeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Type_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool Type_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var a = from item in Spravochniks.SprTypesToRadionuclids where item.Item1 == value.Value select item.Item2;
            if (a.Count() == 1)
            {
                _autoRN = true;
                Radionuclids.Value = a.First();
            }
            return true;
        }
        #endregion

        #region Radionuclids
        public string Radionuclids_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("радионуклиды")]
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
            if (_autoRN)
            {
                _autoRN = false;
                return true;
            }
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
        #endregion

        #region FactoryNumber
        public string FactoryNumber_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("номер")]
        public RamAccess<string> FactoryNumber
        {
            get
            {
                var tmp = new RamAccess<string>(FactoryNumber_Validation, FactoryNumber_DB);
                tmp.PropertyChanged += FactoryNumberValueChanged;
                return tmp;
            }
            set
            {
                FactoryNumber_DB = value.Value;
            }
        }
        private void FactoryNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                FactoryNumber_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool FactoryNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if(string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        #endregion

        #region Activity
        public string Activity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("активность, Бк")]
        public RamAccess<string> Activity
        {
            get
            {
                var tmp = new RamAccess<string>(Activity_Validation, Activity_DB);
                tmp.PropertyChanged += ActivityValueChanged;
                return tmp;
            }
            set
            {
                Activity_DB = value.Value;
            }
        }
        private void ActivityValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                Activity_DB = value1;
            }
        }
        private bool Activity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((ActivityNote == null) || ActivityNote.Equals(""))
                //    value.AddError( "Заполните примечание");
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
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region CreationDate
        public string CreationDate_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("дата выпуска")]
        public RamAccess<string> CreationDate
        {
            get
            {
                var tmp = new RamAccess<string>(CreationDate_Validation, CreationDate_DB);
                tmp.PropertyChanged += CreationDateValueChanged;
                return tmp;
            }
            set
            {
                CreationDate_DB = value.Value;
            }
        }//If change this change validation

        private void CreationDateValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                CreationDate_DB = tmp;
            }
        }
        private bool CreationDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if(string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((CreationDateNote == null) || CreationDateNote.Equals(""))
                //    value.AddError( "Заполните примечание");
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

        #region CreatorOKPO
        public string CreatorOKPO_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("ОКПО изготовителя")]
        public RamAccess<string> CreatorOKPO
        {
            get
            {
                var tmp = new RamAccess<string>(CreatorOKPO_Validation, CreatorOKPO_DB);
                tmp.PropertyChanged += CreatorOKPOValueChanged;
                return tmp;
            }
            set
            {
                CreatorOKPO_DB = value.Value;
            }
        }//If change this change validation

        private void CreatorOKPOValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                CreatorOKPO_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool CreatorOKPO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((CreatorOKPONote.Value == null) || CreatorOKPONote.Value.Equals(""))
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
        #endregion

        #region AggregateState
        public byte? AggregateState_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property("агрегатное состояние")]
        public RamAccess<byte?> AggregateState//1 2 3
        {
            get
            {
                var tmp = new RamAccess<byte?>(AggregateState_Validation, AggregateState_DB);
                tmp.PropertyChanged += AggregateStateValueChanged;
                return tmp;
            }
            set
            {
                AggregateState_DB = value.Value;
            }
        }
        private void AggregateStateValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                AggregateState_DB = ((RamAccess<byte?>)Value).Value;
            }
        }
        private bool AggregateState_Validation(RamAccess<byte?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            if ((value.Value != 1) && (value.Value != 2) && (value.Value != 3))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region PropertyCode
        public byte? PropertyCode_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property("код формы собственности")]
        public RamAccess<byte?> PropertyCode
        {
            get
            {
                var tmp = new RamAccess<byte?>(PropertyCode_Validation, PropertyCode_DB);
                tmp.PropertyChanged += PropertyCodeValueChanged;
                return tmp;
            }
            set
            {
                PropertyCode_DB = value.Value;
            }
        }
        private void PropertyCodeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PropertyCode_DB = ((RamAccess<byte?>)Value).Value;
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
        #endregion

        #region Owner
        public string Owner_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("правообладатель")]
        public RamAccess<string> Owner
        {
            get
            {
                var tmp = new RamAccess<string>(Owner_Validation, Owner_DB);
                tmp.PropertyChanged += OwnerValueChanged;
                return tmp;
            }
            set
            {
                Owner_DB = value.Value;
            }
        }//if change this change validation

        private void OwnerValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Owner_DB = ((RamAccess<string>)Value).Value;
            }
        }
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
                //if ((OwnerNote.Value == null) || OwnerNote.Value.Equals(""))
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
        #endregion

        #region ProviderOrRecieverOKPO
        public string ProviderOrRecieverOKPO_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("поставщика или получателя")]
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
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((ProviderOrRecieverOKPONote.Value == null) || ProviderOrRecieverOKPONote.Value.Equals(""))
                //    value.AddError( "Заполните примечание");
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
                //ProviderOrRecieverOKPO.Value = "ОКПО ОТЧИТЫВАЮЩЕЙСЯ ОРГ";
                //return true;
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
        [Attributes.Form_Property("перевозчика")]
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
        private bool TransporterOKPO_Validation(RamAccess<string> value)//TODO
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
            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region PackName
        public string PackName_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("наименование")]
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
        #endregion

        #region PackType
        public string PackType_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("тип")]
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
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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
        #endregion

        #region PackNumber
        public string PackNumber_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("номер упаковки")]
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
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))//ok
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
        #endregion

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

        #region IExcel
        public void ExcelRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelRow(worksheet, Row);
            worksheet.Cells[Row, 4].Value = PassportNumber_DB;
            worksheet.Cells[Row, 5].Value = Type_DB;
            worksheet.Cells[Row, 6].Value = Radionuclids_DB;
            worksheet.Cells[Row, 7].Value = FactoryNumber_DB;
            worksheet.Cells[Row, 8].Value = Activity_DB;
            worksheet.Cells[Row, 9].Value = CreatorOKPO_DB;
            worksheet.Cells[Row, 10].Value = CreationDate_DB;
            worksheet.Cells[Row, 11].Value = AggregateState_DB;
            worksheet.Cells[Row, 12].Value = PropertyCode_DB;
            worksheet.Cells[Row, 13].Value = Owner_DB;
            worksheet.Cells[Row, 14].Value = DocumentVid_DB;
            worksheet.Cells[Row, 15].Value = DocumentNumber_DB;
            worksheet.Cells[Row, 16].Value = DocumentDate_DB;
            worksheet.Cells[Row, 17].Value = ProviderOrRecieverOKPO_DB;
            worksheet.Cells[Row, 18].Value = TransporterOKPO_DB;
            worksheet.Cells[Row, 19].Value = PackName_DB;
            worksheet.Cells[Row, 20].Value = PackType_DB;
            worksheet.Cells[Row, 21].Value = PackNumber_DB;
        }

        public static void ExcelHeader(ExcelWorksheet worksheet)
        {
            Form1.ExcelHeader(worksheet);
            worksheet.Cells[1, 4].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(PassportNumber))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 5].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(Type))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 6].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(Radionuclids))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 7].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(FactoryNumber))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 8].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(Activity))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 9].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(CreatorOKPO))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 10].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(CreationDate))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 11].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(AggregateState))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 12].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(PropertyCode))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 13].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(Owner))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 14].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(DocumentVid))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 15].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(DocumentNumber))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 16].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(DocumentDate))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 17].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(ProviderOrRecieverOKPO))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 18].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(TransporterOKPO))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 19].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(PackName))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 20].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(PackType))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 21].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form13,Models").GetProperty(nameof(PackNumber))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
        }
        #endregion
    }
}
