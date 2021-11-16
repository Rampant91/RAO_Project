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

        [Attributes.Form_Property("Форма")]
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
        [Attributes.Form_Property("номер паспорта")]
        public RamAccess<string> PassportNumber
        {
            get
            {
                var tmp = new RamAccess<string>(PassportNumber_Validation, PassportNumber_DB);
                tmp.PropertyChanged += PassportNumberValueChanged;
                return tmp;
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
        } private bool PassportNumber_Validation(RamAccess<string> value)
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
        [Attributes.Form_Property("наименование")]
        public RamAccess<string> NameIOU
        {
            get
            {
                var tmp = new RamAccess<string>(NameIOU_Validation, NameIOU_DB);
                tmp.PropertyChanged += NameIOUValueChanged;
                return tmp;
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
        } private bool NameIOU_Validation(RamAccess<string> value)//TODO
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
        [Attributes.Form_Property("номер")]
        public RamAccess<string> FactoryNumber
        {
            get
            {
                var tmp = new RamAccess<string>(FactoryNumber_Validation, FactoryNumber_DB);
                tmp.PropertyChanged += FactoryNumberValueChanged;
                return tmp;
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
        } private bool FactoryNumber_Validation(RamAccess<string> value)
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
        [Attributes.Form_Property("масса, кг")]
        public RamAccess<string> Mass
        {
            get
            {
                var tmp = new RamAccess<string>(Mass_Validation, Mass_DB);
                tmp.PropertyChanged += MassValueChanged;
                return tmp;
            } set
            {
                Mass_DB = value.Value; OnPropertyChanged(nameof(Mass));
            }
        }


        private void MassValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Mass_DB = ((RamAccess<string>)Value).Value;
            }
        } private bool Mass_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            try
            {
                if (!(double.Parse(tmp) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
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
                CreatorOKPO_DB = ((RamAccess<string>)Value).Value;
            }
        } private bool CreatorOKPO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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
                CreationDate_DB = ((RamAccess<string>)Value).Value;
            }
        } private bool CreationDate_Validation(RamAccess<string> value)//Ready
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
        #endregion

        #region SignedServicePeriod
        public float? SignedServicePeriod_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property("НСС, мес.")]
        public RamAccess<float?> SignedServicePeriod
        {
            get
            {
                var tmp = new RamAccess<float?>(SignedServicePeriod_Validation, SignedServicePeriod_DB);
                tmp.PropertyChanged += SignedServicePeriodValueChanged;
                return tmp;
            } set
            {
                SignedServicePeriod_DB = value.Value; OnPropertyChanged(nameof(SignedServicePeriod));
            }
        }


        private void SignedServicePeriodValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                SignedServicePeriod_DB = ((RamAccess<float?>)Value).Value;
            }
        } private bool SignedServicePeriod_Validation(RamAccess<float?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value <= 0)
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
        } private bool PropertyCode_Validation(RamAccess<byte?> value)//Ready
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
        [Attributes.Form_Property("правообладатель")]
        public RamAccess<string> Owner
        {
            get
            {
                var tmp = new RamAccess<string>(Owner_Validation, Owner_DB);
                tmp.PropertyChanged += OwnerValueChanged;
                return tmp;
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
                Owner_DB = ((RamAccess<string>)Value).Value;
            }
        } private bool Owner_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
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
            } set
            {
                ProviderOrRecieverOKPO_DB = value.Value; OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }


        private void ProviderOrRecieverOKPOValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                ProviderOrRecieverOKPO_DB = ((RamAccess<string>)Value).Value;
            }
        } private bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
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
                //    value.AddError( "Заполните примечания");
                return true;
            }
            if (OKSM.Contains(value.Value))
            {
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
        [Attributes.Form_Property("перевозчика")]
        public RamAccess<string> TransporterOKPO
        {
            get
            {
                var tmp = new RamAccess<string>(TransporterOKPO_Validation, TransporterOKPO_DB);
                tmp.PropertyChanged += TransporterOKPOValueChanged;
                return tmp;
            } set
            {
                TransporterOKPO_DB = value.Value; OnPropertyChanged(nameof(TransporterOKPO));
            }
        }


        private void TransporterOKPOValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                TransporterOKPO_DB = ((RamAccess<string>)Value).Value;
            }
        } private bool TransporterOKPO_Validation(RamAccess<string> value)//TODO
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
        [Attributes.Form_Property("наименование")]
        public RamAccess<string> PackName
        {
            get
            {
                var tmp = new RamAccess<string>(PackName_Validation, PackName_DB);
                tmp.PropertyChanged += PackNameValueChanged;
                return tmp;
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
        } private bool PackName_Validation(RamAccess<string> value)
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
        } private bool PackType_Validation(RamAccess<string> value)//Ready
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
        } private bool PackNumber_Validation(RamAccess<string> value)//Ready
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
            worksheet.Cells[Row, 5].Value = NameIOU_DB;
            worksheet.Cells[Row, 6].Value = FactoryNumber_DB;
            worksheet.Cells[Row, 7].Value = Mass_DB;
            worksheet.Cells[Row, 8].Value = CreatorOKPO_DB;
            worksheet.Cells[Row, 9].Value = CreationDate_DB;
            worksheet.Cells[Row, 10].Value = SignedServicePeriod_DB;
            worksheet.Cells[Row, 11].Value = PropertyCode_DB;
            worksheet.Cells[Row, 12].Value = Owner_DB;
            worksheet.Cells[Row, 13].Value = DocumentVid_DB;
            worksheet.Cells[Row, 14].Value = DocumentNumber_DB;
            worksheet.Cells[Row, 15].Value = DocumentDate_DB;
            worksheet.Cells[Row, 16].Value = ProviderOrRecieverOKPO_DB;
            worksheet.Cells[Row, 17].Value = TransporterOKPO_DB;
            worksheet.Cells[Row, 18].Value = PackName_DB;
            worksheet.Cells[Row, 19].Value = PackType_DB;
            worksheet.Cells[Row, 20].Value = PackNumber_DB;
        }

        public static void ExcelHeader(ExcelWorksheet worksheet)
        {
            Form1.ExcelHeader(worksheet);
            worksheet.Cells[1, 4].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(PassportNumber))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 5].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(NameIOU))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 6].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(FactoryNumber))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 7].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(Mass))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 8].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(CreatorOKPO))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 9].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(CreationDate))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 10].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(SignedServicePeriod))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 11].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(PropertyCode))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 12].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(Owner))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 13].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(DocumentVid))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 14].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(DocumentNumber))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 15].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(DocumentDate))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 16].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(ProviderOrRecieverOKPO))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 17].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(TransporterOKPO))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 18].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(PackName))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 19].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(PackType))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 20].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form12,Models").GetProperty(nameof(PackNumber))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
        }
        #endregion
    }
}
