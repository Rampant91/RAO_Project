using System;
using DBRealization;

namespace Test_App
{
    class Program
    {
        static void Main(string[] args)
        {
            DBModel mdl = new DBModel();
            mdl.SaveChanges();

            Console.ReadKey();
        }
    }
}
