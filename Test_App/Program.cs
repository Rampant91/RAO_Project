using DBRealization;
using System;

namespace Test_App
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //RedDataBaseCreation.CreateDB("C:\\Database\\local.raodb");
            //var strm=File.Create("C:\\DATABASE\\local.raodb");
            //strm.Close();
            DBModel mdl = new DBModel("C:\\DATABASE\\local.raodb");
            bool sm = mdl.Database.EnsureCreated();

            mdl.SaveChanges();
            Console.ReadKey();
        }
    }
}
