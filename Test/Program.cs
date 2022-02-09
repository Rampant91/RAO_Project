using Models;
using Models.Collections;
using Models.DBRealization;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Models.DBRealization.DBAPIFactory;

namespace Test
{
    class Program
    {
        private static async Task<string> ProcessRaoDirectory(string systemDirectory)
        {
            var tmp = "";
            var pty = "";
            try
            {
                string path = Path.GetPathRoot(systemDirectory);
                tmp = Path.Combine(path, "RAO");
                pty = tmp;
                tmp = Path.Combine(tmp, "temp");
                Directory.CreateDirectory(tmp);
            }
            catch (Exception e)
            {
                return null;
            }
            try
            {
                var fl = Directory.GetFiles(tmp);
                foreach (var file in fl)
                {
                    File.Delete(file);
                }
                return pty;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private static async Task<string> GetSystemDirectory()
        {
            try
            {
                string system = "";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    system = Environment.GetFolderPath(Environment.SpecialFolder.System);
                }
                else
                {
                    system = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                return system;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private static async Task ProcessDataBaseCreate(string tempDirectory)
        {
            var i = 0;
            bool flag = false;
            DBModel dbm = null;
            foreach (var file in Directory.GetFiles(tempDirectory))
            {
                try
                {
                    StaticConfiguration.DBPath = file;
                    StaticConfiguration.DBModel = new DBModel(StaticConfiguration.DBPath);

                    dbm = StaticConfiguration.DBModel;

                    //await dbm.Database.MigrateAsync();
                    flag = true;
                    break;
                }
                catch (Exception e)
                {
                    i++;
                }
            }
            if (!flag)
            {
                StaticConfiguration.DBPath = Path.Combine(tempDirectory, "Local" + "_" + i + ".raodb");
                StaticConfiguration.DBModel = new DBModel(StaticConfiguration.DBPath);
                dbm = StaticConfiguration.DBModel;
                //await dbm.Database.MigrateAsync();
            }
        }
        static async Task Main(string[] args)
        {
            //var systemDirectory = await GetSystemDirectory();
            //var raoDirectory = await ProcessRaoDirectory(systemDirectory);
            //await ProcessDataBaseCreate(raoDirectory);

            //var rep = DBUse.GetData10Main();

            //var t = new EssanceMethods.APIFactory<Report>();
            //var tre=t.Post(new Report());

            //var t2 = new EssanceMethods.APIFactory(typeof(Report));
            //var tre2 = t.Post(new Report());

            //var t3 = new EssanceMethods.APIFactory<Report>();
            //var tre3 = await t.PostAsync(new Report());

            //var t4 = new EssanceMethods.APIFactory(typeof(Report));
            //var tre4 = await t.PostAsync(new Report());

            var t = new EssanceMethods.APIFactory<Reports>();
            var tre = t.Post(new Reports());

            Console.ReadKey();

        }
    }
}
