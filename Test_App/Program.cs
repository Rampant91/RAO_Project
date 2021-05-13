using System;
using DBRealization;
using System.IO;

namespace Test_App
{
    class Program
    {
        static void Main(string[] args)
        {
            //RedDataBaseCreation.CreateDB("C:\\Database\\local.raodb");
            //var strm=File.Create("C:\\DATABASE\\local.raodb");
            //strm.Close();
            DBModel mdl = new DBModel("C:\\DATABASE\\local.raodb");
            var sm=mdl.Database.EnsureCreated();

            mdl.reports.Add(new Collections.Reports());
            mdl.SaveChanges();
            Console.ReadKey();
        }
    }
}
