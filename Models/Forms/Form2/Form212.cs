using Models.DataAccess;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.12: Суммарные сведения о РВ не в составе ЗРИ")]
    public class Form212 : Abstracts.Form2
    {
        public Form212() : base()
        {
            FormNum = "212";
            NumberOfFields = 8;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //OperationCode property
        [Attributes.Form_Property("Код")]
        public short OperationCode
        {
            get
            {
                
                {
                    string tmp = _dataAccess.Get<string>(nameof(OperationCode));
                    return tmp != null ? short.Parse(tmp) : (short)-1;
                }
                else
                {
                    
                }
            }
            set
            {
                var tmp1 = value.ToString();
                if (tmp1.Length == 1) tmp1 = "0" + tmp1;

                OperationCode_Validation(tmp1);


                
                {
                    var tmp = _OperationCode_Not_Valid.ToString();
                    if (tmp.Length == 1) tmp = "0" + tmp;
                    _dataAccess.Set(nameof(OperationCode), tmp);
                }
                OnPropertyChanged(nameof(OperationCode));
            }
        }

        
        private void OperationCode_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //OperationCode property

        //ObjectTypeCode property
        [Attributes.Form_Property("Код типа объектов учета")]
        public IDataAccess<string> ObjectTypeCode
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ObjectTypeCode));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(ObjectTypeCode), value);
                }
                OnPropertyChanged(nameof(ObjectTypeCode));
            }
        }
        //2 digit code
        
        private void ObjectTypeCode_Validation(IDataAccess<string> value)//TODO
        {
            ClearErrors(ObjectTypeCode);
        }
        //ObjectTypeCode property

        //Radionuclids property
        [Attributes.Form_Property("Радионуклиды")]
        public IDataAccess<string> Radionuclids
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(Radionuclids));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
                }
            }
            set
            {
                Radionuclids_Validation(value);

                
                {
                    _dataAccess.Set(nameof(Radionuclids), value);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation
        
        private void Radionuclids_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            List<Tuple<string, string>> spr = new List<Tuple<string, string>>();//Here binds spravochnik
            foreach (var item in spr)
            {
                if (item.Item2.Equals(value))
                {
                    Radionuclids = item.Item2;
                    return;
                }
            }
        }
        //Radionuclids property

        //Activity property
        [Attributes.Form_Property("Активность, Бк")]
        public double Activity
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Activity));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Activity), value);
                }
                OnPropertyChanged(nameof(Activity));
            }
        }

        
        private void Activity_Validation(double value)//Ready
        {
            value.ClearErrors();
            if (value.Value == -1)
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (!(value > 0))
                value.AddError( "Число должно быть больше нуля");
        }
        //Activity property

        //ProviderOrRecieverOKPO property
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
        public IDataAccess<string> ProviderOrRecieverOKPO
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(ProviderOrRecieverOKPO));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
                }
            }
            set
            {
                ProviderOrRecieverOKPO_Validation(value);

                
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPO), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }

        
        private void ProviderOrRecieverOKPO_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("Минобороны") || value.Equals("прим.")) return;
            if (OKSM.Contains(value)) return;
            if ((value.Length != 8) && (value.Length != 14))
                value.AddError( "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
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
        public IDataAccess<string> ProviderOrRecieverOKPONote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(ProviderOrRecieverOKPONote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
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

        
        private void ProviderOrRecieverOKPONote_Validation()
        {
            value.ClearErrors();
        }
        //ProviderOrRecieverOKPONote property
    }
}
