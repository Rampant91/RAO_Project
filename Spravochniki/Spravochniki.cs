using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace Spravochniki
{
    public static class Spravochniks
    {
        public static List<string> SprRifineOrSortCodes = new List<string> {
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
#if DEBUG
        private static Task<List<Tuple<string, long, long>>> SprRadionuclidsTask = ReadCsvAsync(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"))+ "data\\Spravochniki\\RadionuclidsActivities.csv");
#else
        private static Task<List<Tuple<string, long, long>>> SprRadionuclidsTask = ReadCsvAsync(Path.GetFullPath(AppContext.BaseDirectory)+"data\\Spravochniki\\RadionuclidsActivities.csv");
#endif

        private static Task<List<Tuple<string, long, long>>> ReadCsvAsync(string path)
        {
            return Task.Run(() => ReadCsv(path));
        }

        private static List<Tuple<string, long, long>> ReadCsv(string path)
        {
            var res = new List<Tuple<string, long, long>>();
            string[] radionuclids = File.ReadAllLines(path);
            for (int k = 1; k < radionuclids.Count(); k++)
            {
                var tmp = radionuclids[k].Split(";");
                string i1 = tmp[0];
                long i2 = long.Parse(tmp[1]);
                long i3 = long.Parse(tmp[2]);
                res.Add(new Tuple<string, long, long>(i1, i2, i3));
            }
            return res;
        }
    }
}
