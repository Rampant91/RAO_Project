using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace ClassLibrary1
{
    public static class Spravochniki
    {
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
        private static Task<List<Tuple<string, long, long>>> SprRadionuclidsTask = ReadCsvAsync(@"C:\Databases\RadionuclidsActivities.csv");
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
