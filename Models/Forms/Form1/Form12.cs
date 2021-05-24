using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.2: Сведения об изделиях из обедненного урана")]
    public class Form12 : Abstracts.Form1
    {
        public Form12() : base()
        {
            FormNum = "12";
            NumberOfFields = 33;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //PassportNumber property
        [Attributes.Form_Property("Номер паспорта")]
        public string PassportNumber
        {
            get
            {
                if (GetErrors(nameof(PassportNumber)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PassportNumber));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PassportNumber_Not_Valid;
                }
            }
            set
            {
                _PassportNumber_Not_Valid = value;
                if (GetErrors(nameof(PassportNumber)) == null)
                {
                    _dataAccess.Set(nameof(PassportNumber), _PassportNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PassportNumber));
            }
        }

        private string _PassportNumber_Not_Valid = "";
        private void PassportNumber_Validation()
        {
            ClearErrors(nameof(PassportNumber));
        }
        //PassportNumber property

        protected override void OperationCode_Validation(string value1)//OK
        {
            ClearErrors(nameof(OperationCode));
            if (value1 == null)
            {
                AddError(nameof(OperationCode), "Недопустимое значение");
                return;
            }
            var value = short.Parse(value1);
            if (value == _OperationCode_Not_Valid)
                AddError(nameof(OperationCode), "Поле не заполнено");
            List<short> spr = new List<short>();    //HERE BINDS SPRAVOCHNIK
            bool flag = false;
            foreach (var item in spr)
            {
                if (item == value) flag = true;
            }
            if (!flag)
            {
                AddError(nameof(OperationCode), "Недопустимое значение");
                return;
            }
            if ((value==01) || (value==13) ||
            (value==14) || (value==16) ||
            (value==26) || (value==36) ||
            (value==44) || (value==45) ||
            (value==49) || (value==51) ||
            (value==52) || (value==55) ||
            (value==56) || (value==57) ||
            (value==59) || (value==76))
                AddError(nameof(OperationCode), "Код операции не может быть использован для РВ");
            return;
        }

        //PassportNumberNote property
        public string PassportNumberNote
        {
            get
            {
                if (GetErrors(nameof(PassportNumberNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PassportNumberNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PassportNumberNote_Not_Valid;
                }
            }
            set
            {
                _PassportNumberNote_Not_Valid = value;
                if (GetErrors(nameof(PassportNumberNote)) == null)
                {
                    _dataAccess.Set(nameof(PassportNumberNote), _PassportNumberNote_Not_Valid);
                }
                OnPropertyChanged(nameof(PassportNumberNote));
            }
        }

        private string _PassportNumberNote_Not_Valid = "";
        private void PassportNumberNote_Validation()
        {
            ClearErrors(nameof(PassportNumberNote));
        }
        //PassportNumberNote property

        //PassportNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public string PassportNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(PassportNumberRecoded)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PassportNumberRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PassportNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _PassportNumberRecoded_Not_Valid = value;
                if (GetErrors(nameof(PassportNumberRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PassportNumberRecoded), _PassportNumberRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(PassportNumberRecoded));
            }
        }
        //If change this change validation
        private string _PassportNumberRecoded_Not_Valid = "";
        private void PassportNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(PassportNumberRecoded));
        }
        //PassportNumberRecoded property

        //NameIOU property
        [Attributes.Form_Property("Наименование ИОУ")]
        public string NameIOU
        {
            get
            {
                if (GetErrors(nameof(NameIOU)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(NameIOU));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _NameIOU_Not_Valid;
                }
            }
            set
            {
                _NameIOU_Not_Valid = value;
                if (GetErrors(nameof(NameIOU)) == null)
                {
                    _dataAccess.Set(nameof(NameIOU), _NameIOU_Not_Valid);
                }
                OnPropertyChanged(nameof(NameIOU));
            }
        }

        private string _NameIOU_Not_Valid = "";
        private void NameIOU_Validation(string value)//TODO
        {
            ClearErrors(nameof(NameIOU));
        }
        //NameIOU property

        //FactoryNumber property
        [Attributes.Form_Property("Заводской номер")]
        public string FactoryNumber
        {
            get
            {
                if (GetErrors(nameof(FactoryNumber)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(FactoryNumber));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _FactoryNumber_Not_Valid;
                }
            }
            set
            {
                _FactoryNumber_Not_Valid = value;
                if (GetErrors(nameof(FactoryNumber)) == null)
                {
                    _dataAccess.Set(nameof(FactoryNumber), _FactoryNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(FactoryNumber));
            }
        }

        private string _FactoryNumber_Not_Valid = "";
        private void FactoryNumber_Validation()
        {
            ClearErrors(nameof(FactoryNumber));
        }
        //FactoryNumber property

        //FactoryNumberRecoded property
        public string FactoryNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(FactoryNumberRecoded)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(FactoryNumberRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _FactoryNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _FactoryNumberRecoded_Not_Valid = value;
                if (GetErrors(nameof(FactoryNumberRecoded)) == null)
                {
                    _dataAccess.Set(nameof(FactoryNumberRecoded), _FactoryNumberRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(FactoryNumberRecoded));
            }
        }
        //If change this change validation
        private string _FactoryNumberRecoded_Not_Valid = "";
        private void FactoryNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(FactoryNumberRecoded));
        }
        //FactoryNumberRecoded property

        //Mass Property
        [Attributes.Form_Property("Масса, кг")]
        public string Mass
        {
            get
            {
                if (GetErrors(nameof(Mass)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Mass));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _Mass_Not_Valid;
                }
            }
            set
            {
                _Mass_Not_Valid = value;
                if (GetErrors(nameof(Mass)) == null)
                {
                    _dataAccess.Set(nameof(Mass), _Mass_Not_Valid);
                }
                OnPropertyChanged(nameof(Mass));
            }
        }

        private string _Mass_Not_Valid = "";
        private void Mass_Validation(string value)//TODO
        {
            ClearErrors(nameof(Mass));
            string tmp = value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            try
            {
                if (!(double.Parse(tmp) > 0))
                    AddError(nameof(Mass), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(Mass), "Недопустимое значение");
            }
        }
        //Mass Property

        //CreatorOKPO property
        [Attributes.Form_Property("ОКПО изготовителя")]
        public string CreatorOKPO
        {
            get
            {
                if (GetErrors(nameof(CreatorOKPO)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(CreatorOKPO));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _CreatorOKPO_Not_Valid;
                }
            }
            set
            {
                CreatorOKPO_Validation(value);

                if (GetErrors(nameof(CreatorOKPO)) == null)
                {
                    _dataAccess.Set(nameof(CreatorOKPO), _CreatorOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(CreatorOKPO));
            }
        }
        //If change this change validation
        private string _CreatorOKPO_Not_Valid = "";
        private void CreatorOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(CreatorOKPO));
            if ((value == null) || (value.Equals("")))
            {
                AddError(nameof(CreatorOKPO), "Поле не заполнено");
                return;
            }
            if (value.Equals("прим.")) return;
            foreach (var item in OKSM)
            {
                if (value.Equals(item)) return;
            }
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(CreatorOKPO), "Недопустимое значение");
            else
            {
                var mask = new Regex("[0123456789_]*");
                if (!mask.IsMatch(value))
                    AddError(nameof(CreatorOKPO), "Недопустимое значение");
            }
        }
        //CreatorOKPO property

        //CreatorOKPONote property
        public string CreatorOKPONote
        {
            get
            {
                if (GetErrors(nameof(CreatorOKPONote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(CreatorOKPONote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _CreatorOKPONote_Not_Valid;
                }
            }
            set
            {
                _CreatorOKPONote_Not_Valid = value;
                if (GetErrors(nameof(CreatorOKPONote)) == null)
                {
                    _dataAccess.Set(nameof(CreatorOKPONote), _CreatorOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(CreatorOKPONote));
            }
        }

        private string _CreatorOKPONote_Not_Valid = "";
        private void CreatorOKPONote_Validation()
        {
            ClearErrors(nameof(CreatorOKPONote));
        }
        //CreatorOKPONote property

        //CreationDate property
        [Attributes.Form_Property("Дата изготовления")]
        public string CreationDate
        {
            get
            {
                if (GetErrors(nameof(CreationDate)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(CreationDate));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _CreationDate_Not_Valid;
                }
            }
            set
            {
                _CreationDate_Not_Valid = value;
                if (GetErrors(nameof(CreationDate)) == null)
                {
                    _dataAccess.Set(nameof(CreationDate), _CreationDate_Not_Valid);
                }
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        //If change this change validation
        private string _CreationDate_Not_Valid = "01/01/1921";
        private void CreationDate_Validation(string value)//Ready
        {
            ClearErrors(nameof(CreationDate));
            if (value.Equals("прим.")) return;
            var a = new Regex("[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}");
            if (!a.IsMatch(value))
                AddError(nameof(CreationDate), "Недопустимое значение");
        }
        //CreationDate property

        //SignedServicePeriod property
        [Attributes.Form_Property("НСС, мес.")]
        public float SignedServicePeriod
        {
            get
            {
                if (GetErrors(nameof(SignedServicePeriod)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(SignedServicePeriod));//OK
                    return tmp != null ? (float)tmp : -1;
                }
                else
                {
                    return _SignedServicePeriod_Not_Valid;
                }
            }
            set
            {
                SignedServicePeriod_Validation(value);

                if (GetErrors(nameof(SignedServicePeriod)) == null)
                {
                    _dataAccess.Set(nameof(SignedServicePeriod), _SignedServicePeriod_Not_Valid);
                }
                OnPropertyChanged(nameof(SignedServicePeriod));
            }
        }

        private float _SignedServicePeriod_Not_Valid = -1;
        private void SignedServicePeriod_Validation(float value)//Ready
        {
            ClearErrors(nameof(SignedServicePeriod));
            if (value <= 0)
                AddError(nameof(SignedServicePeriod), "Недопустимое значение");
        }
        //SignedServicePeriod property

        //PropertyCode property
        [Attributes.Form_Property("Код собственности")]
        public byte PropertyCode
        {
            get
            {
                if (GetErrors(nameof(PropertyCode)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PropertyCode));//OK
                    return tmp != null ? (byte)tmp : (byte)0;
                }
                else
                {
                    return _PropertyCode_Not_Valid;
                }
            }
            set
            {
                PropertyCode_Validation(value);

                if (GetErrors(nameof(PropertyCode)) == null)
                {
                    _dataAccess.Set(nameof(PropertyCode), _PropertyCode_Not_Valid);
                }
                OnPropertyChanged(nameof(PropertyCode));
            }
        }

        private byte _PropertyCode_Not_Valid = 255;
        private void PropertyCode_Validation(byte value)//Ready
        {
            ClearErrors(nameof(PropertyCode));
            if (!((value >= 1) && (value <= 9)))
                AddError(nameof(PropertyCode), "Недопустимое значение");
        }
        //PropertyCode property

        //Owner property
        [Attributes.Form_Property("Владелец")]
        public string Owner
        {
            get
            {
                if (GetErrors(nameof(Owner)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Owner));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _Owner_Not_Valid;
                }
            }
            set
            {
                Owner_Validation(value);

                if (GetErrors(nameof(Owner)) == null)
                {
                    _dataAccess.Set(nameof(Owner), _Owner_Not_Valid);
                }
                OnPropertyChanged(nameof(Owner));
            }
        }
        //if change this change validation
        private string _Owner_Not_Valid = "";
        private void Owner_Validation(string value)//Ready
        {
            ClearErrors(nameof(Owner));
            if ((value == null) || value.Equals(_Owner_Not_Valid))
            {
                AddError(nameof(Owner), "Поле не заполнено");
                return;
            }
            if (value.Equals("прим.")) return;
            foreach (var item in OKSM)
            {
                if (value.Equals(item)) return;
            }
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(Owner), "Недопустимое значение");
            else
            {
                var mask = new Regex("[0123456789_]*");
                if (!mask.IsMatch(value))
                    AddError(nameof(Owner), "Недопустимое значение");
            }
        }
        //Owner property

        //ProviderOrRecieverOKPO property
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
        public string ProviderOrRecieverOKPO
        {
            get
            {
                if (GetErrors(nameof(ProviderOrRecieverOKPO)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(ProviderOrRecieverOKPO));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _ProviderOrRecieverOKPO_Not_Valid;
                }
            }
            set
            {
                ProviderOrRecieverOKPO_Validation(value);

                if (GetErrors(nameof(ProviderOrRecieverOKPO)) == null)
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPO), _ProviderOrRecieverOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }

        private string _ProviderOrRecieverOKPO_Not_Valid = "";
        private void ProviderOrRecieverOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(ProviderOrRecieverOKPO));
            if ((value == null) || value.Equals(_ProviderOrRecieverOKPO_Not_Valid))
            {
                AddError(nameof(ProviderOrRecieverOKPO), "Поле не заполнено");
                return;
            }
            bool a = (OperationCode >= 10) && (OperationCode <= 12);
            bool b = (OperationCode >= 41) && (OperationCode <= 43);
            bool c = (OperationCode >= 71) && (OperationCode <= 73);
            bool d = (OperationCode == 15) || (OperationCode == 17) || (OperationCode == 18) || (OperationCode == 46) ||
                (OperationCode == 47) || (OperationCode == 48) || (OperationCode == 53) || (OperationCode == 54) ||
                (OperationCode == 58) || (OperationCode == 61) || (OperationCode == 62) || (OperationCode == 65) ||
                (OperationCode == 67) || (OperationCode == 68) || (OperationCode == 75) || (OperationCode == 76);
            if (a || b || c || d)
            {
                ProviderOrRecieverOKPO = "ОКПО ОТЧИТЫВАЮЩЕЙСЯ ОРГ";
                return;
            }
            if (value.Equals("Минобороны") || value.Equals("прим.")) return;
            foreach (var item in OKSM)
            {
                if (item.Equals(value)) return;
            }
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(ProviderOrRecieverOKPO), "Недопустимое значение");
            else
            {
                var mask = new Regex("[0123456789_]*");
                if (!mask.IsMatch(value))
                    AddError(nameof(ProviderOrRecieverOKPO), "Недопустимое значение");
            }
        }
        //ProviderOrRecieverOKPO property

        private List<string> OKSM = new List<string>
            {
                "АФГАНИСТАН",
                "АЛБАНИЯ",
                "АНТАРКТИДА",
                "АЛЖИР",
                "АМЕРИКАНСКОЕ САМОА",
                "АНДОРРА",
                "АНГОЛА",
                "АНТИГУА И БАРБУДА",
                "АЗЕРБАЙДЖАН",
                "АРГЕНТИНА",
                "АВСТРАЛИЯ",
                "АВСТРИЯ",
                "БАГАМЫ",
                "БАХРЕЙН",
                "БАНГЛАДЕШ",
                "АРМЕНИЯ",
                "БАРБАДОС",
                "БЕЛЬГИЯ",
                "БЕРМУДЫ",
                "БУТАН",
                "БОЛИВИЯ, МНОГОНАЦИОНАЛЬНОЕ ГОСУДАРСТВО",
                "БОСНИЯ И ГЕРЦЕГОВИНА",
                "БОТСВАНА",
                "ОСТРОВ БУВЕ",
                "БРАЗИЛИЯ",
                "БЕЛИЗ",
                "БРИТАНСКАЯ ТЕРРИТОРИЯ В ИНДИЙСКОМ ОКЕАНЕ",
                "СОЛОМОНОВЫ ОСТРОВА",
                "ВИРГИНСКИЕ ОСТРОВА (БРИТАНСКИЕ)",
                "БРУНЕЙ-ДАРУССАЛАМ",
                "БОЛГАРИЯ",
                "МЬЯНМА",
                "БУРУНДИ",
                "БЕЛАРУСЬ",
                "КАМБОДЖА",
                "КАМЕРУН",
                "КАНАДА",
                "КАБО-ВЕРДЕ",
                "ОСТРОВА КАЙМАН",
                "ЦЕНТРАЛЬНО-АФРИКАНСКАЯ РЕСПУБЛИКА",
                "ШРИ-ЛАНКА",
                "ЧАД",
                "ЧИЛИ",
                "КИТАЙ",
                "ТАЙВАНЬ (КИТАЙ)",
                "ОСТРОВ РОЖДЕСТВА",
                "КОКОСОВЫЕ (КИЛИНГ) ОСТРОВА",
                "КОЛУМБИЯ",
                "КОМОРЫ",
                "МАЙОТТА",
                "КОНГО",
                "КОНГО, ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА",
                "ОСТРОВА КУКА",
                "КОСТА-РИКА",
                "ХОРВАТИЯ",
                "КУБА",
                "КИПР",
                "ЧЕХИЯ",
                "БЕНИН",
                "ДАНИЯ",
                "ДОМИНИКА",
                "ДОМИНИКАНСКАЯ РЕСПУБЛИКА",
                "ЭКВАДОР",
                "ЭЛЬ-САЛЬВАДОР",
                "ЭКВАТОРИАЛЬНАЯ ГВИНЕЯ",
                "ЭФИОПИЯ",
                "ЭРИТРЕЯ",
                "ЭСТОНИЯ",
                "ФАРЕРСКИЕ ОСТРОВА",
                "ФОЛКЛЕНДСКИЕ ОСТРОВА (МАЛЬВИНСКИЕ)",
                "ЮЖНАЯ ДЖОРДЖИЯ И ЮЖНЫЕ САНДВИЧЕВЫ ОСТРОВА",
                "ФИДЖИ",
                "ФИНЛЯНДИЯ",
                "ЭЛАНДСКИЕ ОСТРОВА",
                "ФРАНЦИЯ",
                "ФРАНЦУЗСКАЯ ГВИАНА",
                "ФРАНЦУЗСКАЯ ПОЛИНЕЗИЯ",
                "ФРАНЦУЗСКИЕ ЮЖНЫЕ ТЕРРИТОРИИ",
                "ДЖИБУТИ",
                "ГАБОН",
                "ГРУЗИЯ",
                "ГАМБИЯ",
                "ПАЛЕСТИНА, ГОСУДАРСТВО",
                "ГЕРМАНИЯ",
                "ГАНА",
                "ГИБРАЛТАР",
                "КИРИБАТИ",
                "ГРЕЦИЯ",
                "ГРЕНЛАНДИЯ",
                "ГРЕНАДА",
                "ГВАДЕЛУПА",
                "ГУАМ",
                "ГВАТЕМАЛА",
                "ГВИНЕЯ",
                "ГАЙАНА",
                "ГАИТИ",
                "ОСТРОВ ХЕРД И ОСТРОВА МАКДОНАЛЬД",
                "ПАПСКИЙ ПРЕСТОЛ (ГОСУДАРСТВО - ГОРОД ВАТИКАН)",
                "ГОНДУРАС",
                "ГОНКОНГ",
                "ВЕНГРИЯ",
                "ИСЛАНДИЯ",
                "ИНДИЯ",
                "ИНДОНЕЗИЯ",
                "ИРАН (ИСЛАМСКАЯ РЕСПУБЛИКА)",
                "ИРАК","ИРЛАНДИЯ",
                "ИЗРАИЛЬ","ИТАЛИЯ","КОТ Д'ИВУАР","ЯМАЙКА","ЯПОНИЯ",
                "КАЗАХСТАН","ИОРДАНИЯ","КЕНИЯ","КОРЕЯ, НАРОДНО-ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","КОРЕЯ, РЕСПУБЛИКА","КУВЕЙТ","КИРГИЗИЯ",
                "ЛАОССКАЯ НАРОДНО-ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","ЛИВАН","ЛЕСОТО","ЛАТВИЯ","ЛИБЕРИЯ","ЛИВИЯ","ЛИХТЕНШТЕЙН",
                "ЛИТВА",
                "ЛЮКСЕМБУРГ",
                "МАКАО",
                "МАДАГАСКАР","МАЛАВИ",
                "МАЛАЙЗИЯ",
                "МАЛЬДИВЫ",
                "МАЛИ",
                "МАЛЬТА",
                "МАРТИНИКА",
                "МАВРИТАНИЯ",
                "МАВРИКИЙ",
                "МЕКСИКА",
                "МОНАКО",
                "МОНГОЛИЯ",
                "МОЛДОВА, РЕСПУБЛИКА",
                "ЧЕРНОГОРИЯ",
                "МОНТСЕРРАТ",
                "МАРОККО",
                "МОЗАМБИК",
                "ОМАН",
                "НАМИБИЯ",
                "НАУРУ",
                "НЕПАЛ",
                "НИДЕРЛАНДЫ",
                "КЮРАСАО",
                "АРУБА",
                "СЕН-МАРТЕН (нидерландская часть)",
                "БОНЭЙР, СИНТ-ЭСТАТИУС И САБА","НОВАЯ КАЛЕДОНИЯ","ВАНУАТУ","НОВАЯ ЗЕЛАНДИЯ","НИКАРАГУА","НИГЕР",
                "НИГЕРИЯ","НИУЭ","ОСТРОВ НОРФОЛК","НОРВЕГИЯ","СЕВЕРНЫЕ МАРИАНСКИЕ ОСТРОВА",
                "МАЛЫЕ ТИХООКЕАНСКИЕ ОТДАЛЕННЫЕ ОСТРОВА СОЕДИНЕННЫХ ШТАТОВ","МИКРОНЕЗИЯ, ФЕДЕРАТИВНЫЕ ШТАТЫ","МАРШАЛЛОВЫ ОСТРОВА",
                "ПАЛАУ","ПАКИСТАН","ПАНАМА","ПАПУА-НОВАЯ ГВИНЕЯ","ПАРАГВАЙ","ПЕРУ",
                "ФИЛИППИНЫ","ПИТКЕРН","ПОЛЬША","ПОРТУГАЛИЯ","ГВИНЕЯ-БИСАУ","ТИМОР-ЛЕСТЕ",
                "ПУЭРТО-РИКО","КАТАР","РЕЮНЬОН","РУМЫНИЯ","РОССИЯ","РУАНДА","СЕН-БАРТЕЛЕМИ",
                "СВЯТАЯ ЕЛЕНА, ОСТРОВ ВОЗНЕСЕНИЯ, ТРИСТАН-ДА-КУНЬЯ","СЕНТ-КИТС И НЕВИС","АНГИЛЬЯ","СЕНТ-ЛЮСИЯ",
                "СЕН-МАРТЕН (французская часть)","СЕН-ПЬЕР И МИКЕЛОН","СЕНТ-ВИНСЕНТ И ГРЕНАДИНЫ",
                "САН-МАРИНО","САН-ТОМЕ И ПРИНСИПИ","САУДОВСКАЯ АРАВИЯ","СЕНЕГАЛ","СЕРБИЯ","СЕЙШЕЛЫ",
                "СЬЕРРА-ЛЕОНЕ","СИНГАПУР","СЛОВАКИЯ","ВЬЕТНАМ","СЛОВЕНИЯ","СОМАЛИ","ЮЖНАЯ АФРИКА","ЗИМБАБВЕ","ИСПАНИЯ",
                "ЗАПАДНАЯ САХАРА","СУДАН","СУРИНАМ","ШПИЦБЕРГЕН И ЯН МАЙЕН","ЭСВАТИНИ","ШВЕЦИЯ","ШВЕЙЦАРИЯ",
                "СИРИЙСКАЯ АРАБСКАЯ РЕСПУБЛИКА","ТАДЖИКИСТАН","ТАИЛАНД","ТОГО","ТОКЕЛАУ","ТОНГА","ТРИНИДАД И ТОБАГО",
                "ОБЪЕДИНЕННЫЕ АРАБСКИЕ ЭМИРАТЫ","ТУНИС","ТУРЦИЯ","ТУРКМЕНИСТАН","ОСТРОВА ТЕРКС И КАЙКОС",
                "ТУВАЛУ","УГАНДА","УКРАИНА","СЕВЕРНАЯ МАКЕДОНИЯ","ЕГИПЕТ",
                "СОЕДИНЕННОЕ КОРОЛЕВСТВО","ГЕРНСИ","ДЖЕРСИ","ОСТРОВ МЭН","ТАНЗАНИЯ, ОБЪЕДИНЕННАЯ РЕСПУБЛИКА","СОЕДИНЕННЫЕ ШТАТЫ",
                "ВИРГИНСКИЕ ОСТРОВА (США)","БУРКИНА-ФАСО","УРУГВАЙ","УЗБЕКИСТАН",
                "ВЕНЕСУЭЛА (БОЛИВАРИАНСКАЯ РЕСПУБЛИКА)","УОЛЛИС И ФУТУНА","САМОА","ЙЕМЕН",
                "ЗАМБИЯ","АБХАЗИЯ","ЮЖНАЯ ОСЕТИЯ","ЮЖНЫЙ СУДАН"};

        //ProviderOrRecieverOKPONote property
        public string ProviderOrRecieverOKPONote
        {
            get
            {
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(ProviderOrRecieverOKPONote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _ProviderOrRecieverOKPONote_Not_Valid;
                }
            }
            set
            {
                _ProviderOrRecieverOKPONote_Not_Valid = value;
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) == null)
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPONote), _ProviderOrRecieverOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
            }
        }

        private string _ProviderOrRecieverOKPONote_Not_Valid = "";
        private void ProviderOrRecieverOKPONote_Validation()
        {
            ClearErrors(nameof(ProviderOrRecieverOKPONote));
        }
        //ProviderOrRecieverOKPONote property

        //TransporterOKPO property
        [Attributes.Form_Property("ОКПО перевозчика")]
        public string TransporterOKPO
        {
            get
            {
                if (GetErrors(nameof(TransporterOKPO)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(TransporterOKPO));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _TransporterOKPO_Not_Valid;
                }
            }
            set
            {
                TransporterOKPO_Validation(value);

                if (GetErrors(nameof(TransporterOKPO)) == null)
                {
                    _dataAccess.Set(nameof(TransporterOKPO), _TransporterOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }

        private string _TransporterOKPO_Not_Valid = "";
        private void TransporterOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(TransporterOKPO));
            if ((value == null) || value.Equals(_TransporterOKPO_Not_Valid))
            {
                AddError(nameof(TransporterOKPO), "Поле не заполнено");
                return;
            }
            if (value.Equals("прим.") || value.Equals("-")) return;
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(TransporterOKPO), "Недопустимое значение");
            else
            {
                var mask = new Regex("[0123456789_]*");
                if (!mask.IsMatch(value))
                    AddError(nameof(TransporterOKPO), "Недопустимое значение");
            }
        }
        //TransporterOKPO property

        //TransporterOKPONote property
        public string TransporterOKPONote
        {
            get
            {
                if (GetErrors(nameof(TransporterOKPONote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(TransporterOKPONote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _TransporterOKPONote_Not_Valid;
                }
            }
            set
            {
                _TransporterOKPONote_Not_Valid = value;
                if (GetErrors(nameof(TransporterOKPONote)) == null)
                {
                    _dataAccess.Set(nameof(TransporterOKPONote), _TransporterOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(TransporterOKPONote));
            }
        }

        private string _TransporterOKPONote_Not_Valid = "";
        private void TransporterOKPONote_Validation()
        {
            ClearErrors(nameof(TransporterOKPONote));
        }
        //TransporterOKPONote property

        //PackName property
        [Attributes.Form_Property("Наименование упаковки")]
        public string PackName
        {
            get
            {
                if (GetErrors(nameof(PackName)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackName));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackName_Not_Valid;
                }
            }
            set
            {
                PackName_Validation(value);

                if (GetErrors(nameof(PackName)) == null)
                {
                    _dataAccess.Set(nameof(PackName), _PackName_Not_Valid);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }

        private string _PackName_Not_Valid = "";
        private void PackName_Validation(string value)
        {
            ClearErrors(nameof(PackName));
            if ((value == null) || value.Equals(_PackName_Not_Valid))
            {
                AddError(nameof(PackName), "Поле не заполнено");
                return;
            }
        }
        //PackName property

        //PackNameNote property
        public string PackNameNote
        {
            get
            {
                if (GetErrors(nameof(PackNameNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackNameNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackNameNote_Not_Valid;
                }
            }
            set
            {
                _PackNameNote_Not_Valid = value;
                if (GetErrors(nameof(PackNameNote)) == null)
                {
                    _dataAccess.Set(nameof(PackNameNote), _PackNameNote_Not_Valid);
                }
                OnPropertyChanged(nameof(PackNameNote));
            }
        }

        private string _PackNameNote_Not_Valid = "";
        private void PackNameNote_Validation()
        {
            ClearErrors(nameof(PackNameNote));
        }
        //PackNameNote property

        //PackType property
        [Attributes.Form_Property("Тип упаковки")]
        public string PackType
        {
            get
            {
                if (GetErrors(nameof(PackType)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackType));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackType_Not_Valid;
                }
            }
            set
            {
                PackType_Validation(value);

                if (GetErrors(nameof(PackType)) == null)
                {
                    _dataAccess.Set(nameof(PackType), _PackType_Not_Valid);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation
        private string _PackType_Not_Valid = "";
        private void PackType_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackType));
            if ((value == null) || value.Equals(_PackType_Not_Valid))
            {
                AddError(nameof(PackType), "Поле не заполнено");
                return;
            }
        }
        //PackType property

        //PackTypeRecoded property
        public string PackTypeRecoded
        {
            get
            {
                if (GetErrors(nameof(PackTypeRecoded)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackTypeRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackTypeRecoded_Not_Valid;
                }
            }
            set
            {
                _PackTypeRecoded_Not_Valid = value;
                if (GetErrors(nameof(PackTypeRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PackTypeRecoded), _PackTypeRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }

        private string _PackTypeRecoded_Not_Valid = "";
        private void PackTypeRecoded_Validation()
        {
            ClearErrors(nameof(PackTypeRecoded));
        }
        //PackTypeRecoded property

        //PackTypeNote property
        public string PackTypeNote
        {
            get
            {
                if (GetErrors(nameof(PackTypeNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackTypeNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackTypeNote_Not_Valid;
                }
            }
            set
            {
                _PackTypeNote_Not_Valid = value;
                if (GetErrors(nameof(PackTypeNote)) == null)
                {
                    _dataAccess.Set(nameof(PackTypeNote), _PackTypeNote_Not_Valid);
                }
                OnPropertyChanged(nameof(PackTypeNote));
            }
        }

        private string _PackTypeNote_Not_Valid = "";
        private void PackTypeNote_Validation()
        {
            ClearErrors(nameof(PackTypeNote));
        }
        //PackTypeNote property

        //PackNumber property
        [Attributes.Form_Property("Номер упаковки")]
        public string PackNumber
        {
            get
            {
                if (GetErrors(nameof(PackNumber)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackNumber));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackNumber_Not_Valid;
                }
            }
            set
            {
                _PackNumber_Not_Valid = value;
                if (GetErrors(nameof(PackNumber)) == null)
                {
                    _dataAccess.Set(nameof(PackNumber), _PackNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PackNumber));
            }
        }
        //If change this change validation
        private string _PackNumber_Not_Valid = "";
        private void PackNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackNumber));
            if ((value == null) || value.Equals(_PackNumber_Not_Valid))//ok
            {
                AddError(nameof(PackNumber), "Поле не заполнено");
                return;
            }
        }
        //PackNumber property

        //PackNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public string PackNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(PackNumberRecoded)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackNumberRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _PackNumberRecoded_Not_Valid = value;
                if (GetErrors(nameof(PackNumberRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PackNumberRecoded), _PackNumberRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        //If change this change validation
        private string _PackNumberRecoded_Not_Valid = "";
        private void PackNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackNumberRecoded));
        }
        //PackNumberRecoded property

        private void DocumentDate_Validation(DateTimeOffset value)
        {
            ClearErrors(nameof(DocumentDate));
            short tmp = OperationCode;
            bool a = (tmp >= 11) && (tmp <= 18);
            bool b = (tmp >= 41) && (tmp <= 49);
            bool c = (tmp >= 53) && (tmp <= 59);
            bool d = (tmp == 65) || (tmp == 68);
            if (a || b || c || d)
                if (!value.Date.Equals(OperationDate.Date))
                    AddError(nameof(DocumentDate), "Заполните примечание");
        }
    }
}
