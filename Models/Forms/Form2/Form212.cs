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
            FormNum.Value = "212";
            NumberOfFields.Value = 8;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //OperationCode property
        [Attributes.Form_Property("Код")]
        public RamAccess<short> OperationCode
        {
            get
            {
                    return _dataAccess.Get<short>(nameof(OperationCode));
            }
            set
            {
                _dataAccess.Set(nameof(OperationCode), value);
                OnPropertyChanged(nameof(OperationCode));
            }
        }

        
        private bool OperationCode_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //OperationCode property

        //ObjectTypeCode property
        [Attributes.Form_Property("Код типа объектов учета")]
        public RamAccess<string> ObjectTypeCode
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ObjectTypeCode));
                }
                
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
        
        private bool ObjectTypeCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors(); return true;}
        //ObjectTypeCode property

        //Radionuclids property
        [Attributes.Form_Property("Радионуклиды")]
        public RamAccess<string> Radionuclids
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
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            List<Tuple<string, string>> spr = new List<Tuple<string, string>>();//Here binds spravochnik
            foreach (var item in spr)
            {
                if (item.Item2.Equals(value))
                {
                    Radionuclids.Value = item.Item2;
return false;
                }
            }
            return true;
        }
        //Radionuclids property

        //Activity property
        [Attributes.Form_Property("Активность, Бк")]
        public RamAccess<double> Activity
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(Activity));
                }
                
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

        
        private bool Activity_Validation(RamAccess<double?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (!(value.Value > 0))
            {
                value.AddError("Число должно быть больше нуля"); return false;
            }
            return true;
        }
        //Activity property

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
                value.AddError("Поле не заполнено");
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
    }
}
