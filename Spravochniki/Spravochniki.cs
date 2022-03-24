using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace Spravochniki
{
    public static class Spravochniks
    {
        public static List<string> OKSM = new List<string>
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
        public static List<short> SprAccObjCodes = new List<short>
        {
            11,12,13,21,22,23,31,32,33,99
        };
        public static List<string> SprPlotCodes = new List<string>
        {

        };
        public static List<short> SprCodeTypesAccObjects = new List<short>
        {
            11,12,13,21,22,23,31,32,33,99
        };
        public static List<string> SprOpCodes = new List<string>
        {
            "01","10","11","12","13","14","15","16","17","18","21","22","25","26","27","28","29","31","32","35","36","37","38","39","41","42","43","44","45",
            "46","47","48","49","51","52","53","54","55","56","57","58","59","61","62","63","64","65","66","67","68","71","72","73","74","75","76","81","82",
            "83","84","85","86","87","88","97","98","99"
        };
        public static List<short> SprOpCodes1 = new List<short>
        {
            1,10,11,12,13,14,15,16,17,18,21,22,25,26,27,28,29,31,32,35,36,37,38,39,41,42,43,44,45,
            46,47,48,49,51,52,53,54,55,56,57,58,59,61,62,63,64,65,66,67,68,71,72,73,74,75,76,81,82,
            83,84,85,86,87,88,97,98,99
        };
        public static List<string> SprRecieverTypeCode = new List<string>
        {
            "101","201","211","301","311","401","411","501","601","611","811","821","831","911","921","991","102","202","212","302","312","402","412","502","602","612","812","822","832","912","922","992","109","209","219","309","319","409","419","509","609","619","819","829","839","919","929","999"
        };
        public static List<string> SprRifineOrSortCodes = new List<string>
        {
            "11","12","13","14","15","16","17","19","21","22","23","24","29","31","32","39","41","42","43","49","51","52","53","54","55","61","62","63","71","72","73","74","79","99"
        };
        public static List<Tuple<string, long, long>> SprRadionuclids
        {
            get
            {
                return SprRadionuclidsTask.Result;
            }
            private set
            {

            }
        }
        public static List<Tuple<byte?, string>> SprDocumentVidName = new List<Tuple<byte?, string>>
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
            };
        public static List<Tuple<string,string>> SprTypesToRadionuclidsFor11
        {
            get
            {
                return SprTypesToRadionuclidsIn11Task.Result;
            }
            private set
            {

            }
        }
        public static List<Tuple<string, string>> SprTypesToRadionuclids
        {
            get
            {
                return SprTypesToRadionuclidsTask.Result;
            }
            private set
            {

            }
        }
#if DEBUG
        private static Task<List<Tuple<string, long, long>>> SprRadionuclidsTask
        {
            get
            {
                var tmp = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")),
                    "data", "Spravochniki", "RadionuclidsActivities.csv");
                return ReadCsvAsync(tmp);
            }
        }
        private static Task<List<Tuple<string, string>>> SprTypesToRadionuclidsTask
        {
            get
            {
                var tmp = Path.Combine(
                    Path.GetFullPath(
                        Path.Combine(
                            AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data");
                tmp = Path.Combine(tmp, "Spravochniki", "TypeToRadionuclids.csv");
                return ReadCsvAsync1(tmp);
            }
        }
        private static Task<List<Tuple<string, string>>> SprTypesToRadionuclidsIn11Task
        {
            get
            {
                var tmp = Path.Combine(
                    Path.GetFullPath(
                        Path.Combine(
                            AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data");
                tmp = Path.Combine(tmp, "Spravochniki", "TypeToNuclids.csv");
                return ReadCsvAsync11(tmp);
            }
        }
#else
        private static Task<List<Tuple<string, long, long>>> SprRadionuclidsTask
        {
            get
            {
                var tmp = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory),
                    "data","Spravochniki","RadionuclidsActivities.csv");
                return ReadCsvAsync(tmp);
            }
        }
        private static Task<List<Tuple<string, string>>> SprTypesToRadionuclidsTask
        {
            get
            {
                var tmp = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory),
                    "data", "Spravochniki", "TypeToRadionuclids.csv");
                return ReadCsvAsync1(tmp);
            }
        }
        private static Task<List<Tuple<string, string>>> SprTypesToRadionuclidsIn11Task
        {
            get
            {
                var tmp = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory),
                    "data", "Spravochniki", "TypeToNuclids.csv");
                return ReadCsvAsync11(tmp);
            }
        }
#endif

        private static Task<List<Tuple<string, long, long>>> ReadCsvAsync(string path)
        {
            return Task.Run(() => ReadCsv(path));
        }

        private static Task<List<Tuple<string, string>>> ReadCsvAsync1(string path)
        {
            return Task.Run(() => ReadCsv1(path));
        }

        private static Task<List<Tuple<string, string>>> ReadCsvAsync11(string path)
        {
            return Task.Run(() => ReadCsvFor11(path));
        }

        private static List<Tuple<string, long, long>> ReadCsv(string path)
        {
            var res = new List<Tuple<string, long, long>>();
            string[] rows = File.ReadAllLines(path);
            for (int k = 1; k < rows.Count(); k++)
            {
                var tmp = rows[k].Split(";");
                string i1 = tmp[0];
                long i2 = long.Parse(tmp[1]);
                long i3 = long.Parse(tmp[2]);
                res.Add(new Tuple<string, long, long>(i1, i2, i3));
            }
            return res;
        }

        private static List<Tuple<string, string>> ReadCsv1(string path)
        {
            var res = new List<Tuple<string, string>>();
            string[] rows = File.ReadAllLines(path);
            for (int k = 1; k < rows.Count(); k++)
            {
                var tmp = rows[k].Split(";", 2);
                string i1 = tmp[0];
                string i2 = tmp[1];
                res.Add(new Tuple<string, string>(i1, i2));
            }
            return res;
        }
        private static List<Tuple<string, string>> ReadCsvFor11(string path)
        {
            var res = new List<Tuple<string, string>>();
            string[] rows = File.ReadAllLines(path);
            for (int k = 3; k < rows.Count(); k++)
            {
                var tmp = rows[k].Split(";", 7);
                string i1 = tmp[1];
                string i2 = tmp[5];
                res.Add(new Tuple<string, string>(i1, i2));
            }
            return res;
        }
    }
}
