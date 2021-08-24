using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace Spravochniki
{
    public static class Spravochniks
    {
        public static List<string> SprPlotCodes = new List<string>
        {

        };
        public static List<short> SprOpCodes = new List<short>
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
        private static Task<List<Tuple<string, long, long>>> SprRadionuclidsTask = ReadCsvAsync(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"))+ "data\\Spravochniki\\RadionuclidsActivities.csv");
        private static Task<List<Tuple<string, string>>> SprTypesToRadionuclidsTask = ReadCsvAsync1(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")) + "data\\Spravochniki\\TypeToRadionuclids.csv");
#else
        private static Task<List<Tuple<string, long, long>>> SprRadionuclidsTask = ReadCsvAsync(Path.GetFullPath(AppContext.BaseDirectory)+"data\\Spravochniki\\RadionuclidsActivities.csv");
        private static Task<List<Tuple<string, string>>> SprTypesToRadionuclidsTask = ReadCsvAsync1(Path.GetFullPath(AppContext.BaseDirectory)+"data\\Spravochniki\\TypeToRadionuclids.csv");
#endif

        private static Task<List<Tuple<string, long, long>>> ReadCsvAsync(string path)
        {
            return Task.Run(() => ReadCsv(path));
        }

        private static Task<List<Tuple<string, string>>> ReadCsvAsync1(string path)
        {
            return Task.Run(() => ReadCsv1(path));
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
                var tmp = rows[k].Split(";");
                string i1 = tmp[0];
                string i2 = tmp[1];
                res.Add(new Tuple<string, string>(i1, i2));
            }
            return res;
        }
    }
}
