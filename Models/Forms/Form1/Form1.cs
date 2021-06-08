using Models.DataAccess;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Models.Abstracts
{
    public abstract class Form1 : Form
    {
        [Attributes.Form_Property("Форма")]
        public Form1() : base()
        {
            Init_base();
            Validate_base();
        }

        private void Init_base()
        {
            _dataAccess.Init<short?>(nameof(OperationCode), OperationCode_Validation, null);
            _dataAccess.Init<string>(nameof(OperationDate), OperationDate_Validation, null);
            _dataAccess.Init<string>(nameof(DocumentNumber), DocumentNumber_Validation, null);
            _dataAccess.Init<byte?>(nameof(DocumentVid), DocumentVid_Validation, null);
            _dataAccess.Init<string>(nameof(DocumentDate), DocumentDate_Validation, null);
            _dataAccess.Init<int>(nameof(NumberInOrder), NumberInOrder_Validation, -1);
            _dataAccess.Init<string>(nameof(DocumentNumberRecoded), DocumentNumberRecoded_Validation, null);
            //_dataAccess.Init<string>(nameof(), _Validation, null);
        }
        protected void Validate_base()
        {
            OperationCode_Validation(OperationCode);
            OperationDate_Validation(OperationDate);
            DocumentNumber_Validation(DocumentNumber);
            DocumentVid_Validation(DocumentVid);
            DocumentDate_Validation(DocumentDate);
            NumberInOrder_Validation(NumberInOrder);
            DocumentNumberRecoded_Validation(DocumentNumberRecoded);
        }

        //NumberInOrder property
        [Attributes.Form_Property("№ п/п")]
        public RamAccess<int> NumberInOrder
        {
            get
            {
                    return _dataAccess.Get<int>(nameof(NumberInOrder));
            }
            set
            {
                    _dataAccess.Set(nameof(NumberInOrder), value);
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        private bool NumberInOrder_Validation(RamAccess<int> value)
        {
            value.ClearErrors(); return true;
        }
        //NumberInOrder property

        //CorrectionNumber property
        [Attributes.Form_Property("Номер корректировки")]
        public RamAccess<byte> CorrectionNumber
        {
            get
            {
                    return _dataAccess.Get<byte>(nameof(CorrectionNumber));
            }
            set
            {
                    _dataAccess.Set(nameof(CorrectionNumber), value);
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }

        //private bool CorrectionNumber_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;}
        //CorrectionNumber property

        //OperationCode property
        [Attributes.Form_Property("Код")]
        public RamAccess<short?> OperationCode
        {
            get
            {
                    return _dataAccess.Get<short?>(nameof(OperationCode));
            }
            set
            {
                    _dataAccess.Set(nameof(OperationCode), value);
                OnPropertyChanged(nameof(OperationCode));
            }
        }
        protected virtual bool OperationCode_Validation(RamAccess<short?> arg) { return true; }

        //OprationCode property
        
        //OperationDate property
        [Attributes.Form_Property("Дата операции")]
        public RamAccess<string> OperationDate
        {
            get
            {
                return _dataAccess.Get<string>(nameof(OperationDate));
            }
            set
            {
                    _dataAccess.Set(nameof(OperationDate), value);
                OnPropertyChanged(nameof(OperationDate));
            }
        }

        protected virtual bool OperationDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            return true;
        }
        //OperationDate property

        //DocumentVid property
        [Attributes.Form_Property("Вид документа")]
        public RamAccess<byte?> DocumentVid
        {
            get
            {
                    return _dataAccess.Get<byte?>(nameof(DocumentVid));//Ok
            }
            set
            {
                    _dataAccess.Set(nameof(DocumentVid), value);
                OnPropertyChanged(nameof(DocumentVid));
            }
        }

        protected virtual bool DocumentVid_Validation(RamAccess<byte?> value)// TO DO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            List<Tuple<byte, string>> spr = new List<Tuple<byte, string>>
            {
                new Tuple<byte, string>(0,""),
                new Tuple<byte, string>(1,""),
                new Tuple<byte, string>(2,""),
                new Tuple<byte, string>(3,""),
                new Tuple<byte, string>(4,""),
                new Tuple<byte, string>(5,""),
                new Tuple<byte, string>(6,""),
                new Tuple<byte, string>(7,""),
                new Tuple<byte, string>(8,""),
                new Tuple<byte, string>(9,""),
                new Tuple<byte, string>(10,""),
                new Tuple<byte, string>(11,""),
                new Tuple<byte, string>(12,""),
                new Tuple<byte, string>(13,""),
                new Tuple<byte, string>(14,""),
                new Tuple<byte, string>(15,""),
                new Tuple<byte, string>(19,"")
            };   //HERE BINDS SPRAVOCHNICK
            foreach (var item in spr)
            {
                if (value.Value == item.Item1)
                    return true;
            }
            value.AddError("Недопустимое значение");
            return false;
        }
        //DocumentVid property

        //DocumentNumber property
        [Attributes.Form_Property("Номер документа")]
        public RamAccess<string> DocumentNumber
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(DocumentNumber));//Ok
            }
            set
            {
                    _dataAccess.Set(nameof(DocumentNumber), value);
                OnPropertyChanged(nameof(DocumentNumber));
            }
        }

        protected virtual bool DocumentNumber_Validation(RamAccess<string> value)//Ready
        { return true; }
        //DocumentNumber property

        //DocumentNumberRecoded property
        public RamAccess<string> DocumentNumberRecoded
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(DocumentNumberRecoded));//ok
            }
            set
            {
                    _dataAccess.Set(nameof(DocumentNumberRecoded), value);
                OnPropertyChanged(nameof(DocumentNumberRecoded));
            }
        }

        private bool DocumentNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
        //DocumentNumberRecoded property

        //DocumentDate property
        [Attributes.Form_Property("Дата документа")]
        public RamAccess<string> DocumentDate
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(DocumentDate));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(DocumentDate), value);
                OnPropertyChanged(nameof(DocumentDate));
            }
        }
        //if change this change validation

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

        protected virtual bool DocumentDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            bool ab = (OperationCode.Value >= 11) && (OperationCode.Value <= 18);
            bool b = (OperationCode.Value >= 41) && (OperationCode.Value <= 49);
            bool c = (OperationCode.Value >= 51) && (OperationCode.Value <= 59);
            bool d = (OperationCode.Value == 65) || (OperationCode.Value == 68);
            if (ab || b || c || d)
            {
                if (!value.Value.Equals(OperationDate))
                    value.AddError("Заполните примечание");//to do note handling
            }
            return true;
        }
        //DocumentDate property

        //DocumentDateNote property
        [Attributes.Form_Property("Дата документа")]
        public RamAccess<string> DocumentDateNote
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(DocumentDateNote));
            }
            set
            {
                    _dataAccess.Set(nameof(DocumentDateNote), value);
                OnPropertyChanged(nameof(DocumentDateNote));
            }
        }
        //if change this change validation

        private bool DocumentDateNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //DocumentDateNote property
    }
}
