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
    [Serializable]
    [Attributes.Form_Class("Форма 1.2: Сведения об изделиях из обедненного урана")]
    public class Form12 : Abstracts.Form1
    {
        public Form12() : base()
        {
            FormNum.Value = "1.2";
            Validate_all();
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
        }

        [Attributes.Form_Property(true,"Форма")]
        public override bool Object_Validation()
        {
            return !(CreationDate.HasErrors ||
            CreatorOKPO.HasErrors ||
            Owner.HasErrors ||
            PackName.HasErrors ||
            PackNumber.HasErrors ||
            PackType.HasErrors ||
            PassportNumber.HasErrors ||
            PropertyCode.HasErrors ||
            ProviderOrRecieverOKPO.HasErrors ||
            SignedServicePeriod.HasErrors ||
            TransporterOKPO.HasErrors ||
            FactoryNumber.HasErrors ||
            Mass.HasErrors ||
            NameIOU.HasErrors);
        }

        #region PassportNumber
        public string PassportNumber_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения из паспорта на изделие из обедненного урана", "номер паспорта", "4")]
        public RamAccess<string> PassportNumber
        {
            get
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
            } set
            {
                PassportNumber_DB = value.Value; OnPropertyChanged(nameof(PassportNumber));
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
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
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
        #endregion

        #region NameIOU
        public string NameIOU_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения из паспорта на изделие из обедненного урана", "наименование", "5")]
        public RamAccess<string> NameIOU
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(NameIOU)))
                {
                    ((RamAccess<string>)Dictionary[nameof(NameIOU)]).Value = NameIOU_DB;
                    return (RamAccess<string>)Dictionary[nameof(NameIOU)];
                }
                else
                {
                    var rm = new RamAccess<string>(NameIOU_Validation, NameIOU_DB);
                    rm.PropertyChanged += NameIOUValueChanged;
                    Dictionary.Add(nameof(NameIOU), rm);
                    return (RamAccess<string>)Dictionary[nameof(NameIOU)];
                }
            } set
            {
                NameIOU_DB = value.Value; OnPropertyChanged(nameof(NameIOU));
            }
        }


        private void NameIOUValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                NameIOU_DB = ((RamAccess<string>)Value).Value;
            }
        } 
        private bool NameIOU_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        #endregion

        #region FactoryNumber
        public string FactoryNumber_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения из паспорта на изделие из обедненного урана", "номер", "6")]
        public RamAccess<string> FactoryNumber
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(FactoryNumber)))
                {
                    ((RamAccess<string>)Dictionary[nameof(FactoryNumber)]).Value = FactoryNumber_DB;
                    return (RamAccess<string>)Dictionary[nameof(FactoryNumber)];
                }
                else
                {
                    var rm = new RamAccess<string>(FactoryNumber_Validation, FactoryNumber_DB);
                    rm.PropertyChanged += FactoryNumberValueChanged;
                    Dictionary.Add(nameof(FactoryNumber), rm);
                    return (RamAccess<string>)Dictionary[nameof(FactoryNumber)];
                }
            } set
            {
                FactoryNumber_DB = value.Value;
                OnPropertyChanged(nameof(FactoryNumber));
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
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        #endregion

        #region Mass
        public string Mass_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения из паспорта на изделие из обедненного урана", "масса обедненного урана, кг", "7")]
        public RamAccess<string> Mass
        {
            get
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
            } set
            {
                Mass_DB = value.Value; OnPropertyChanged(nameof(Mass));
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
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
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
        [Attributes.Form_Property(true,"Сведения из паспорта на изделие из обедненного урана", "код ОКПО изготовителя", "8")]
        public RamAccess<string> CreatorOKPO
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(CreatorOKPO)))
                {
                    ((RamAccess<string>)Dictionary[nameof(CreatorOKPO)]).Value = CreatorOKPO_DB;
                    return (RamAccess<string>)Dictionary[nameof(CreatorOKPO)];
                }
                else
                {
                    var rm = new RamAccess<string>(CreatorOKPO_Validation, CreatorOKPO_DB);
                    rm.PropertyChanged += CreatorOKPOValueChanged;
                    Dictionary.Add(nameof(CreatorOKPO), rm);
                    return (RamAccess<string>)Dictionary[nameof(CreatorOKPO)];
                }
            } set
            {
                CreatorOKPO_DB = value.Value; OnPropertyChanged(nameof(CreatorOKPO));
            }
        }
        //If change this change validation

        private void CreatorOKPOValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                string value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                    if (OKSM.Contains(value1.ToUpper()))
                    {
                        value1 = value1.ToUpper();
                    }
                CreatorOKPO_DB = value1;
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
            if (OKSM.Contains(value.Value.ToUpper()))
            {
                return true;
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
        #endregion

        #region CreationDate
        public string CreationDate_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения из паспорта на изделие из обедненного урана", "дата выпуска", "9")]
        public RamAccess<string> CreationDate
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(CreationDate)))
                {
                    ((RamAccess<string>)Dictionary[nameof(CreationDate)]).Value = CreationDate_DB;
                    return (RamAccess<string>)Dictionary[nameof(CreationDate)];
                }
                else
                {
                    var rm = new RamAccess<string>(CreationDate_Validation, CreationDate_DB);
                    rm.PropertyChanged += CreationDateValueChanged;
                    Dictionary.Add(nameof(CreationDate), rm);
                    return (RamAccess<string>)Dictionary[nameof(CreationDate)];
                }
            } set
            {
                CreationDate_DB = value.Value; OnPropertyChanged(nameof(CreationDate));
            }
        }
        //If change this change validation

        private void CreationDateValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                if (!tmp.Equals("прим."))
                {
                    Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                    if (b.IsMatch(tmp))
                    {
                        tmp = tmp.Insert(6, "20");
                    }
                }
                CreationDate_DB = tmp;
            }
        }
        private bool CreationDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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

        #region SignedServicePeriod
        public string SignedServicePeriod_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Сведения из паспорта на изделие из обедненного урана", "НСС, мес.", "10")]
        public RamAccess<string> SignedServicePeriod
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(SignedServicePeriod)))
                {
                    ((RamAccess<string>)Dictionary[nameof(SignedServicePeriod)]).Value = SignedServicePeriod_DB;
                    return (RamAccess<string>)Dictionary[nameof(SignedServicePeriod)];
                }
                else
                {
                    var rm = new RamAccess<string>(SignedServicePeriod_Validation, SignedServicePeriod_DB);
                    rm.PropertyChanged += SignedServicePeriodValueChanged;
                    Dictionary.Add(nameof(SignedServicePeriod), rm);
                    return (RamAccess<string>)Dictionary[nameof(SignedServicePeriod)];
                }
            }
            set
            {
                SignedServicePeriod_DB = value.Value; OnPropertyChanged(nameof(SignedServicePeriod));
            }
        }

        private void SignedServicePeriodValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                SignedServicePeriod_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool SignedServicePeriod_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            try
            {
                if (Convert.ToSingle(value.Value) <= 0)
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            catch
            {

            }
            return true;
        }
        #endregion

        #region PropertyCode
        public byte? PropertyCode_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property(true,"Право собственности на ИОУ","код формы собственности", "11")]
        public RamAccess<byte?> PropertyCode
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(PropertyCode)))
                {
                    ((RamAccess<byte?>)Dictionary[nameof(PropertyCode)]).Value = PropertyCode_DB;
                    return (RamAccess<byte?>)Dictionary[nameof(PropertyCode)];
                }
                else
                {
                    var rm = new RamAccess<byte?>(PropertyCode_Validation, PropertyCode_DB);
                    rm.PropertyChanged += PropertyCodeValueChanged;
                    Dictionary.Add(nameof(PropertyCode), rm);
                    return (RamAccess<byte?>)Dictionary[nameof(PropertyCode)];
                }
            } set
            {
                PropertyCode_DB = value.Value;
                OnPropertyChanged(nameof(PropertyCode));
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
        #endregion

        #region Owner
        public string Owner_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Право собственности на ИОУ", "код ОКПО правообладателя", "12")]
        public RamAccess<string> Owner
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(Owner)))
                {
                    ((RamAccess<string>)Dictionary[nameof(Owner)]).Value = Owner_DB;
                    return (RamAccess<string>)Dictionary[nameof(Owner)];
                }
                else
                {
                    var rm = new RamAccess<string>(Owner_Validation, Owner_DB);
                    rm.PropertyChanged += OwnerValueChanged;
                    Dictionary.Add(nameof(Owner), rm);
                    return (RamAccess<string>)Dictionary[nameof(Owner)];
                }
            } set
            {
                Owner_DB = value.Value; OnPropertyChanged(nameof(Owner));
            }
        }
        //if change this change validation
        private void OwnerValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                string value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                    if (OKSM.Contains(value1.ToUpper()))
                    {
                        value1 = value1.ToUpper();
                    }
                Owner_DB = value1;
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
            if (OKSM.Contains(value.Value.ToUpper()))
            {
                return true;
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
        #endregion

        #region ProviderOrRecieverOKPO
        public string ProviderOrRecieverOKPO_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Код ОКПО","поставщика или получателя", "16")]
        public RamAccess<string> ProviderOrRecieverOKPO
        {
            get
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
            } set
            {
                ProviderOrRecieverOKPO_DB = value.Value; OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
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
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("Минобороны"))
            {
                return true;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((ProviderOrRecieverOKPONote.Value == null) || ProviderOrRecieverOKPONote.Value.Equals(""))
                //    value.AddError( "Заполните примечания");
                return true;
            }
            if (OKSM.Contains(value.Value.ToUpper()))
            {
                return true;
            }
            bool a = (int.Parse(OperationCode.Value) >= 10) && (int.Parse(OperationCode.Value) <= 12);
            bool b = (int.Parse(OperationCode.Value) >= 41) && (int.Parse(OperationCode.Value) <= 43);
            bool c = (int.Parse(OperationCode.Value) >= 71) && (int.Parse(OperationCode.Value) <= 73);
            bool d = (OperationCode.Value == "15") || (OperationCode.Value == "17") || (OperationCode.Value == "18") || (OperationCode.Value == "46") ||
                (OperationCode.Value == "47") || (OperationCode.Value == "48") || (OperationCode.Value == "53") || (OperationCode.Value == "54") ||
                (OperationCode.Value == "58") || (OperationCode.Value == "61") || (OperationCode.Value == "62") || (OperationCode.Value == "65") ||
                (OperationCode.Value == "67") || (OperationCode.Value == "68") || (OperationCode.Value == "75") || (OperationCode.Value == "76");
            if (a || b || c || d)
            {
                //ProviderOrRecieverOKPO.Value = "ОКПО ОТЧИТЫВАЮЩЕЙСЯ ОРГ";
                //return false;
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
        #endregion

        #region TransporterOKPO
        public string TransporterOKPO_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Код ОКПО", "перевозчика", "17")]
        public RamAccess<string> TransporterOKPO
        {
            get
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
            } set
            {
                TransporterOKPO_DB = value.Value; OnPropertyChanged(nameof(TransporterOKPO));
            }
        }
        private void TransporterOKPOValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                string value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                    if (OKSM.Contains(value1.ToUpper()))
                    {
                        value1 = value1.ToUpper();
                    }
                TransporterOKPO_DB = value1;
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
            if (OKSM.Contains(value.Value.ToUpper()))
            {
                return true;
            }
            if (value.Value.Equals("Минобороны"))
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
            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
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

        #region PackName
        public string PackName_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Прибор (установка), УКТ или иная упаковка","наименование", "18")]
        public RamAccess<string> PackName
        {
            get
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
            } set
            {
                PackName_DB = value.Value; OnPropertyChanged(nameof(PackName));
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
        [Attributes.Form_Property(true,"Прибор (установка), УКТ или иная упаковка", "тип", "19")]
        public RamAccess<string> PackType
        {
            get
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
            } set
            {
                PackType_DB = value.Value; OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation

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
        [Attributes.Form_Property(true,"Прибор (установка), УКТ или иная упаковка", "номер", "20")]
        public RamAccess<string> PackNumber
        {
            get
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
            } set
            {
                PackNumber_DB = value.Value;
                OnPropertyChanged(nameof(PackNumber));
            }
        }
        //If change this change validation
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
            if ((string.IsNullOrEmpty(value.Value)))//ok
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
                //    value.AddError( "Заполните примечание");//to do note handling
                return true;
            }
            if (string.IsNullOrEmpty(value.Value))//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
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
            if((value.Value == "01") || (value.Value == "13") ||
            (value.Value == "14") || (value.Value == "16") ||
            (value.Value == "26") || (value.Value == "36") ||
            (value.Value == "44") || (value.Value == "45") ||
            (value.Value == "49") || (value.Value == "51") ||
            (value.Value == "52") || (value.Value == "55") ||
            (value.Value == "56") || (value.Value == "57") ||
            (value.Value == "59") || (value.Value == "76"))
            {
                value.AddError("Код операции не может быть использован для РВ");
                return false;
            }
            return true;
        }

        #region IExcel
        public int ExcelRow(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true)
        {
            var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
            Column = Column +(Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);


            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = PassportNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = NameIOU_DB;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = FactoryNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = Mass_DB;
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = CreatorOKPO_DB;
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = CreationDate_DB;
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = SignedServicePeriod_DB;
            worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = PropertyCode_DB;
            worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = Owner_DB;
            worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = DocumentVid_DB;
            worksheet.Cells[Row + (Transpon == false ? 10 : 0), Column + (Transpon == true ? 10 : 0)].Value = DocumentNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 11 : 0), Column + (Transpon == true ? 11 : 0)].Value = DocumentDate_DB;
            worksheet.Cells[Row + (Transpon == false ? 12 : 0), Column + (Transpon == true ? 12 : 0)].Value = ProviderOrRecieverOKPO_DB;
            worksheet.Cells[Row + (Transpon == false ? 13 : 0), Column + (Transpon == true ? 13 : 0)].Value = TransporterOKPO_DB;
            worksheet.Cells[Row + (Transpon == false ? 14 : 0), Column + (Transpon == true ? 14 : 0)].Value = PackName_DB;
            worksheet.Cells[Row + (Transpon == false ? 15 : 0), Column + (Transpon == true ? 15 : 0)].Value = PackType_DB;
            worksheet.Cells[Row + (Transpon == false ? 16 : 0), Column + (Transpon == true ? 16 : 0)].Value = PackNumber_DB;

            return 17;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true)
        {
            var cnt = Form1.ExcelHeader(worksheet, Row, Column, Transpon);
            Column = Column + +(Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(PassportNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(NameIOU)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(FactoryNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(Mass)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(CreatorOKPO)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(CreationDate)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(SignedServicePeriod)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(PropertyCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(Owner)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(DocumentVid)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 10 : 0), Column + (Transpon == true ? 10 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(DocumentNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 11 : 0), Column + (Transpon == true ? 11 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(DocumentDate)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 12 : 0), Column + (Transpon == true ? 12 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(ProviderOrRecieverOKPO)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 13 : 0), Column + (Transpon == true ? 13 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(TransporterOKPO)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 14 : 0), Column + (Transpon == true ? 14 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(PackName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 15 : 0), Column + (Transpon == true ? 15 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(PackType)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 16 : 0), Column + (Transpon == true ? 16 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(PackNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            return 17;
        }
        #endregion

        #region IDataGridColumn
        public override DataGridColumns GetColumnStructure(string param)
        {
            #region NumberInOrder (1)
            DataGridColumns NumberInOrderR = ((Attributes.Form_PropertyAttribute)typeof(Form).GetProperty(nameof(Form.NumberInOrder)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            NumberInOrderR.SetSizeColToAllLevels(50);
            NumberInOrderR.Binding = nameof(Form.NumberInOrder);
            NumberInOrderR.Blocked = true;
            NumberInOrderR.ChooseLine = true;
            #endregion

            #region OperationCode (2)
            DataGridColumns OperationCodeR = ((Attributes.Form_PropertyAttribute)typeof(Form1).GetProperty(nameof(Form1.OperationCode)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            OperationCodeR.SetSizeColToAllLevels(80);
            OperationCodeR.Binding = nameof(Form1.OperationCode);
            NumberInOrderR += OperationCodeR;
            #endregion

            #region OperationDate (3)
            DataGridColumns OperationDateR = ((Attributes.Form_PropertyAttribute)typeof(Form1).GetProperty(nameof(Form1.OperationDate)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            OperationDateR.SetSizeColToAllLevels(80);
            OperationDateR.Binding = nameof(Form1.OperationDate);
            NumberInOrderR += OperationDateR;
            #endregion

            #region PassportNumber (4)
            DataGridColumns PassportNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form12).GetProperty(nameof(Form12.PassportNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            PassportNumberR.SetSizeColToAllLevels(260);
            PassportNumberR.Binding = nameof(Form12.PassportNumber);
            NumberInOrderR += PassportNumberR;
            #endregion

            #region NameIOU (5)
            DataGridColumns NameIOUR = ((Attributes.Form_PropertyAttribute)typeof(Form12).GetProperty(nameof(Form12.NameIOU)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            NameIOUR.SetSizeColToAllLevels(130);
            NameIOUR.Binding = nameof(Form12.NameIOU);
            NumberInOrderR += NameIOUR;
            #endregion

            #region FactoryNumber (6)
            DataGridColumns FactoryNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form12).GetProperty(nameof(Form12.FactoryNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            FactoryNumberR.SetSizeColToAllLevels(100);
            FactoryNumberR.Binding = nameof(Form12.FactoryNumber);
            NumberInOrderR += FactoryNumberR;
            #endregion

            #region Mass (7)
            DataGridColumns MassR = ((Attributes.Form_PropertyAttribute)typeof(Form12).GetProperty(nameof(Form12.Mass)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            MassR.SetSizeColToAllLevels(175);
            MassR.Binding = nameof(Form12.Mass);
            NumberInOrderR += MassR;
            #endregion

            #region CreatorOKPO (8)
            DataGridColumns CreatorOKPOR = ((Attributes.Form_PropertyAttribute)typeof(Form12).GetProperty(nameof(Form12.CreatorOKPO)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            CreatorOKPOR.SetSizeColToAllLevels(150);
            CreatorOKPOR.Binding = nameof(Form12.CreatorOKPO);
            NumberInOrderR += CreatorOKPOR;
            #endregion

            #region CreationDate (9)
            DataGridColumns CreationDateR = ((Attributes.Form_PropertyAttribute)typeof(Form12).GetProperty(nameof(Form12.CreationDate)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            CreationDateR.SetSizeColToAllLevels(180);
            CreationDateR.Binding = nameof(Form12.CreationDate);
            NumberInOrderR += CreationDateR;
            #endregion

            #region SignedServicePeriod (10)
            DataGridColumns SignedServicePeriodR = ((Attributes.Form_PropertyAttribute)typeof(Form12).GetProperty(nameof(Form12.SignedServicePeriod)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            SignedServicePeriodR.SetSizeColToAllLevels(180);
            SignedServicePeriodR.Binding = nameof(Form12.SignedServicePeriod);
            NumberInOrderR += SignedServicePeriodR;
            #endregion

            #region PropertyCode (11)
            DataGridColumns PropertyCodeR = ((Attributes.Form_PropertyAttribute)typeof(Form12).GetProperty(nameof(Form12.PropertyCode)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            PropertyCodeR.SetSizeColToAllLevels(160);
            PropertyCodeR.Binding = nameof(Form12.PropertyCode);
            NumberInOrderR += PropertyCodeR;
            #endregion

            #region Owner (12)
            DataGridColumns OwnerR = ((Attributes.Form_PropertyAttribute)typeof(Form12).GetProperty(nameof(Form12.Owner)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            OwnerR.SetSizeColToAllLevels(168);
            OwnerR.Binding = nameof(Form12.Owner);
            NumberInOrderR += OwnerR;
            #endregion

            #region DocumentVid (13)
            DataGridColumns DocumentVidR = ((Attributes.Form_PropertyAttribute)typeof(Form1).GetProperty(nameof(Form1.DocumentVid)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            DocumentVidR.SetSizeColToAllLevels(80);
            DocumentVidR.Binding = nameof(Form1.DocumentVid);
            NumberInOrderR += DocumentVidR;
            #endregion

            #region DocumentNumber (14)
            DataGridColumns DocumentNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form1).GetProperty(nameof(Form1.DocumentNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            DocumentNumberR.SetSizeColToAllLevels(120);
            DocumentNumberR.Binding = nameof(Form1.DocumentNumber);
            NumberInOrderR += DocumentNumberR;
            #endregion

            #region DocumentDate (15)
            DataGridColumns DocumentDateR = ((Attributes.Form_PropertyAttribute)typeof(Form1).GetProperty(nameof(Form1.DocumentDate)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            DocumentDateR.SetSizeColToAllLevels(80);
            DocumentDateR.Binding = nameof(Form1.DocumentDate);
            NumberInOrderR += DocumentDateR;
            #endregion

            #region ProviderOrReciverOKPO (16)
            DataGridColumns ProviderOrRecieverOKPOR = ((Attributes.Form_PropertyAttribute)typeof(Form12).GetProperty(nameof(Form12.ProviderOrRecieverOKPO)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            ProviderOrRecieverOKPOR.SetSizeColToAllLevels(180);
            ProviderOrRecieverOKPOR.Binding = nameof(Form12.ProviderOrRecieverOKPO);
            NumberInOrderR += ProviderOrRecieverOKPOR;
            #endregion

            #region TransporterOKPO (17)
            DataGridColumns TransporterOKPOR = ((Attributes.Form_PropertyAttribute)typeof(Form12).GetProperty(nameof(Form12.TransporterOKPO)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            TransporterOKPOR.SetSizeColToAllLevels(130);
            TransporterOKPOR.Binding = nameof(Form12.TransporterOKPO);
            NumberInOrderR += TransporterOKPOR;
            #endregion

            #region PackName (18)
            DataGridColumns PackNameR = ((Attributes.Form_PropertyAttribute)typeof(Form12).GetProperty(nameof(Form12.PackName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            PackNameR.SetSizeColToAllLevels(130);
            PackNameR.Binding = nameof(Form12.PackName);
            NumberInOrderR += PackNameR;
            #endregion

            #region PackType (19)
            DataGridColumns PackTypeR = ((Attributes.Form_PropertyAttribute)typeof(Form12).GetProperty(nameof(Form12.PackType)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            PackTypeR.SetSizeColToAllLevels(80);
            PackTypeR.Binding = nameof(Form12.PackType);
            NumberInOrderR += PackTypeR;
            #endregion

            #region PackNumber (20)
            DataGridColumns PackNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form12).GetProperty(nameof(Form12.PackNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
            PackNumberR.SetSizeColToAllLevels(100);
            PackNumberR.Binding = nameof(Form12.PackNumber);
            NumberInOrderR += PackNumberR;
            #endregion

            return NumberInOrderR;
        }
        #endregion
    }
}
