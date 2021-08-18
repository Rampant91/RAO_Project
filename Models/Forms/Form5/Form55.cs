using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Models
{
//    [Serializable]
//    [Attributes.Form_Class("Форма 5.5: Сведения о поступлении/передаче в подведомственные организации от сторонних организаций и переводе в РАО изделий из обедненного урана")]
//    public class Form55 : Abstracts.Form5
//    {
//        public Form55() : base()
//        {
//            //FormNum.Value = "55";
//            //NumberOfFields.Value = 8;
//        }

//        [Attributes.Form_Property("Форма")]
//        public override bool Object_Validation()
//        {
//            return false;
//        }

//        //Name property
//        public int? NameId { get; set; }
//        [Attributes.Form_Property("Наименование")]
//        public virtual RamAccess<string> Name
//        {
//            get
//            {
                
//                {
//                    return DataAccess.Get<string>(nameof(Name));//OK
                    
//                }
                
//                {
                    
//                }
//            }
//            set
//            {

                
//                {
//                    DataAccess.Set(nameof(Name), value);
//                }
//                OnPropertyChanged(nameof(Name));
//            }
//        }


//        private bool Name_Validation(RamAccess<string> value)//TODO
//        {
//            value.ClearErrors(); return true;}
//        //Name property

//        //OperationCode property
//        public int? OperationCodeId { get; set; }
//        [Attributes.Form_Property("Код")]
//        public virtual RamAccess<short> OperationCode
//        {
//            get
//            {
//                    return DataAccess.Get<short>(nameof(OperationCode));
//            }
//            set
//            {
//                DataAccess.Set(nameof(OperationCode), value);
//                OnPropertyChanged(nameof(OperationCode));
//            }
//        }


//        private bool OperationCode_Validation(RamAccess<short> value)
//        {
//            value.ClearErrors(); return true;}
//        //OperationCode property

//        //Quantity property
//        public int? QuantityId { get; set; }
//        [Attributes.Form_Property("Количество, шт.")]
//        public virtual RamAccess<int> Quantity
//        {
//            get
//            {
                
//                {
//                    return DataAccess.Get<int>(nameof(Quantity));//OK
                    
//                }
                
//                {
                    
//                }
//            }
//            set
//            {



                
//                {
//                    DataAccess.Set(nameof(Quantity), value);
//                }
//                OnPropertyChanged(nameof(Quantity));
//            }
//        }
//        // positive int.

//        private bool Quantity_Validation(RamAccess<int> value)//Ready
//        {
//            value.ClearErrors();
//            if (value.Value <= 0)
//            {
//                value.AddError( "Недопустимое значение");
//return false;
//            }
//            return true;
//        }
//        //Quantity property

//        //ProviderOrRecieverOKPO property
//        public int? ProviderOrRecieverOKPOId { get; set; }
//        [Attributes.Form_Property("ОКПО поставщика/получателя")]
//        public virtual RamAccess<string> ProviderOrRecieverOKPO
//        {
//            get
//            {
                
//                {
//                    return DataAccess.Get<string>(nameof(ProviderOrRecieverOKPO));//OK
                    
//                }
                
//                {
                    
//                }
//            }
//            set
//            {


                
//                {
//                    DataAccess.Set(nameof(ProviderOrRecieverOKPO), value);
//                }
//                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
//            }
//        }


//        private bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
//        {
//            value.ClearErrors();
//            if (string.IsNullOrEmpty(value.Value))
//            {
//                value.AddError("Поле не заполнено");
//                return false;
//            }
//            if ((value.Value.Length != 8) && (value.Value.Length != 14))
//            {
//                value.AddError("Недопустимое значение"); return false;

//            }
//            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
//            if (!mask.IsMatch(value.Value))
//            {
//                value.AddError("Недопустимое значение"); return false;
//            }
//            return true;
//        }
//        //ProviderOrRecieverOKPO property
//        protected List<string> OKSM = new List<string>
//            {
//                "АФГАНИСТАН","АЛБАНИЯ","АНТАРКТИДА","АЛЖИР","АМЕРИКАНСКОЕ САМОА","АНДОРРА","АНГОЛА","АНТИГУА И БАРБУДА","АЗЕРБАЙДЖАН","АРГЕНТИНА","АВСТРАЛИЯ","АВСТРИЯ","БАГАМЫ","БАХРЕЙН",
//                "БАНГЛАДЕШ","АРМЕНИЯ","БАРБАДОС","БЕЛЬГИЯ","БЕРМУДЫ","БУТАН","БОЛИВИЯ, МНОГОНАЦИОНАЛЬНОЕ ГОСУДАРСТВО","БОСНИЯ И ГЕРЦЕГОВИНА","БОТСВАНА","ОСТРОВ БУВЕ","БРАЗИЛИЯ","БЕЛИЗ",
//                "БРИТАНСКАЯ ТЕРРИТОРИЯ В ИНДИЙСКОМ ОКЕАНЕ","СОЛОМОНОВЫ ОСТРОВА","ВИРГИНСКИЕ ОСТРОВА (БРИТАНСКИЕ)","БРУНЕЙ-ДАРУССАЛАМ","БОЛГАРИЯ","МЬЯНМА","БУРУНДИ","БЕЛАРУСЬ","КАМБОДЖА",
//                "КАМЕРУН","КАНАДА","КАБО-ВЕРДЕ","ОСТРОВА КАЙМАН","ЦЕНТРАЛЬНО-АФРИКАНСКАЯ РЕСПУБЛИКА","ШРИ-ЛАНКА","ЧАД","ЧИЛИ","КИТАЙ","ТАЙВАНЬ (КИТАЙ)","ОСТРОВ РОЖДЕСТВА","КОКОСОВЫЕ (КИЛИНГ) ОСТРОВА",
//                "КОЛУМБИЯ","КОМОРЫ","МАЙОТТА","КОНГО","КОНГО, ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","ОСТРОВА КУКА","КОСТА-РИКА","ХОРВАТИЯ","КУБА","КИПР","ЧЕХИЯ","БЕНИН","ДАНИЯ","ДОМИНИКА","ДОМИНИКАНСКАЯ РЕСПУБЛИКА",
//                "ЭКВАДОР","ЭЛЬ-САЛЬВАДОР","ЭКВАТОРИАЛЬНАЯ ГВИНЕЯ","ЭФИОПИЯ","ЭРИТРЕЯ","ЭСТОНИЯ","ФАРЕРСКИЕ ОСТРОВА","ФОЛКЛЕНДСКИЕ ОСТРОВА (МАЛЬВИНСКИЕ)","ЮЖНАЯ ДЖОРДЖИЯ И ЮЖНЫЕ САНДВИЧЕВЫ ОСТРОВА",
//                "ФИНЛЯНДИЯ","ЭЛАНДСКИЕ ОСТРОВА","ФРАНЦИЯ","ФРАНЦУЗСКАЯ ГВИАНА","БОНЭЙР, СИНТ-ЭСТАТИУС И САБА","НОВАЯ КАЛЕДОНИЯ","ВАНУАТУ","НОВАЯ ЗЕЛАНДИЯ","НИКАРАГУА","НИГЕР","ФИДЖИ",
//                "ФРАНЦУЗСКАЯ ПОЛИНЕЗИЯ","ФРАНЦУЗСКИЕ ЮЖНЫЕ ТЕРРИТОРИИ","ДЖИБУТИ","ГАБОН","ГРУЗИЯ","ГАМБИЯ","ПАЛЕСТИНА, ГОСУДАРСТВО","ГЕРМАНИЯ","ГАНА","ГИБРАЛТАР","КИРИБАТИ","МАЛИ","МАЛЬТА",
//                "ГРЕЦИЯ","ГРЕНЛАНДИЯ","ГРЕНАДА","ГВАДЕЛУПА","ГУАМ","ГВАТЕМАЛА","ГВИНЕЯ","ГАЙАНА","ГАИТИ","ОСТРОВ ХЕРД И ОСТРОВА МАКДОНАЛЬД","ПАПСКИЙ ПРЕСТОЛ (ГОСУДАРСТВО - ГОРОД ВАТИКАН)",
//                "ГОНДУРАС","ГОНКОНГ","ВЕНГРИЯ","ИСЛАНДИЯ","ИНДИЯ","ИНДОНЕЗИЯ","ИРАН (ИСЛАМСКАЯ РЕСПУБЛИКА)","ИРАК","ИРЛАНДИЯ","ИЗРАИЛЬ","ИТАЛИЯ","КОТ Д'ИВУАР","ЯМАЙКА","ЯПОНИЯ","МАЛЬДИВЫ",
//                "КАЗАХСТАН","ИОРДАНИЯ","КЕНИЯ","КОРЕЯ, НАРОДНО-ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","КОРЕЯ, РЕСПУБЛИКА","КУВЕЙТ","КИРГИЗИЯ","НИГЕРИЯ","НИУЭ","ОСТРОВ НОРФОЛК","НОРВЕГИЯ","СЕВЕРНЫЕ МАРИАНСКИЕ ОСТРОВА",
//                "ЛАОССКАЯ НАРОДНО-ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","ЛИВАН","ЛЕСОТО","ЛАТВИЯ","ЛИБЕРИЯ","ЛИВИЯ","ЛИХТЕНШТЕЙН","ЛИТВА","ЛЮКСЕМБУРГ","МАКАО","МАДАГАСКАР","МАЛАВИ","МАЛАЙЗИЯ",
//                "МАРТИНИКА","МАВРИТАНИЯ","МАВРИКИЙ","МЕКСИКА","МОНАКО","МОНГОЛИЯ","МОЛДОВА, РЕСПУБЛИКА","ЧЕРНОГОРИЯ","МОНТСЕРРАТ","МАРОККО","МОЗАМБИК","ОМАН","НАМИБИЯ","НАУРУ","НЕПАЛ",
//                "АРУБА","СЕН-МАРТЕН (нидерландская часть)","МАЛЫЕ ТИХООКЕАНСКИЕ ОТДАЛЕННЫЕ ОСТРОВА СОЕДИНЕННЫХ ШТАТОВ","МИКРОНЕЗИЯ, ФЕДЕРАТИВНЫЕ ШТАТЫ","МАРШАЛЛОВЫ ОСТРОВА","КЮРАСАО",
//                "ПАЛАУ","ПАКИСТАН","ПАНАМА","ПАПУА-НОВАЯ ГВИНЕЯ","ПАРАГВАЙ","ПЕРУ","ФИЛИППИНЫ","ПИТКЕРН","ПОЛЬША","ПОРТУГАЛИЯ","ГВИНЕЯ-БИСАУ","ТИМОР-ЛЕСТЕ","ШВЕЦИЯ","ШВЕЙЦАРИЯ","НИДЕРЛАНДЫ",
//                "ПУЭРТО-РИКО","КАТАР","РЕЮНЬОН","РУМЫНИЯ","РОССИЯ","РУАНДА","СЕН-БАРТЕЛЕМИ","СВЯТАЯ ЕЛЕНА, ОСТРОВ ВОЗНЕСЕНИЯ, ТРИСТАН-ДА-КУНЬЯ","СЕНТ-КИТС И НЕВИС","АНГИЛЬЯ","СЕНТ-ЛЮСИЯ",
//                "СЕН-МАРТЕН (французская часть)","СЕН-ПЬЕР И МИКЕЛОН","СЕНТ-ВИНСЕНТ И ГРЕНАДИНЫ","САН-МАРИНО","САН-ТОМЕ И ПРИНСИПИ","САУДОВСКАЯ АРАВИЯ","СЕНЕГАЛ","СЕРБИЯ","СЕЙШЕЛЫ","ЮЖНЫЙ СУДАН",
//                "СЬЕРРА-ЛЕОНЕ","СИНГАПУР","СЛОВАКИЯ","ВЬЕТНАМ","СЛОВЕНИЯ","СОМАЛИ","ЮЖНАЯ АФРИКА","ЗИМБАБВЕ","ИСПАНИЯ","ЗАПАДНАЯ САХАРА","СУДАН","СУРИНАМ","ШПИЦБЕРГЕН И ЯН МАЙЕН","ЭСВАТИНИ",
//                "СИРИЙСКАЯ АРАБСКАЯ РЕСПУБЛИКА","ТАДЖИКИСТАН","ТАИЛАНД","ТОГО","ТОКЕЛАУ","ТОНГА","ТРИНИДАД И ТОБАГО","ОБЪЕДИНЕННЫЕ АРАБСКИЕ ЭМИРАТЫ","ТУНИС","ТУРЦИЯ","ТУРКМЕНИСТАН","ОСТРОВА ТЕРКС И КАЙКОС",
//                "ТУВАЛУ","УГАНДА","УКРАИНА","СЕВЕРНАЯ МАКЕДОНИЯ","ЕГИПЕТ","СОЕДИНЕННОЕ КОРОЛЕВСТВО","ГЕРНСИ","ДЖЕРСИ","ОСТРОВ МЭН","ТАНЗАНИЯ, ОБЪЕДИНЕННАЯ РЕСПУБЛИКА","СОЕДИНЕННЫЕ ШТАТЫ",
//                "ВИРГИНСКИЕ ОСТРОВА (США)","БУРКИНА-ФАСО","УРУГВАЙ","УЗБЕКИСТАН","ВЕНЕСУЭЛА (БОЛИВАРИАНСКАЯ РЕСПУБЛИКА)","УОЛЛИС И ФУТУНА","САМОА","ЙЕМЕН","ЗАМБИЯ","АБХАЗИЯ","ЮЖНАЯ ОСЕТИЯ"
//            };
//        ////ProviderOrRecieverOKPONote property
//        //public virtual RamAccess<string> ProviderOrRecieverOKPONote
//        //{
//        //    get
//        //    {
                
//        //        {
//        //            return DataAccess.Get<string>(nameof(ProviderOrRecieverOKPONote));//OK
                    
//        //        }
                
//        //        {
                    
//        //        }
//        //    }
//        //    set
//        //    {

                
//        //        {
//        //            DataAccess.Set(nameof(ProviderOrRecieverOKPONote), value);
//        //        }
//        //        OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
//        //    }
//        //}


//        //private bool ProviderOrRecieverOKPONote_Validation(RamAccess<string> value)
//        //{
//        //    value.ClearErrors(); return true;}
//        ////ProviderOrRecieverOKPONote property

//        //Mass Property
//        public int? MassId { get; set; }
//        [Attributes.Form_Property("Масса, кг")]
//        public virtual RamAccess<double> Mass
//        {
//            get
//            {
                
//                {
//                    return DataAccess.Get<double>(nameof(Mass));
//                }
                
//                {
                    
//                }
//            }
//            set
//            {

                
//                {
//                    DataAccess.Set(nameof(Mass), value);
//                }
//                OnPropertyChanged(nameof(Mass));
//            }
//        }


//        private bool Mass_Validation(RamAccess<double> value)//TODO
//        {
//            value.ClearErrors();
//            if (value.Value <= 0)
//            {
//                value.AddError( "Недопустимое значение");
//return false;
//            }
//            return true;
//        }
//        //Mass Property
//    }
}
