using Models;
using Models.Collections;
using System;

namespace Test
{
    class Program
    {
        class B
        {
        }

        class D : B
        {

        }

        class G<T>
        {
            public static UInt64 Id;
        }


        static void Main(string[] args)
        {
            //Reports testRep = new();
            //var t = testRep.GetColumnStructure();

            //Note testNo = new();
            //var t = testNo.GetColumnStructure();

            //Report testR = new();
            //var t = testR.GetColumnStructure();

            //Form11 test11 = new();
            //var t = test11.GetColumnStructure();


            G<D>.Id = 1;
            G<B>.Id = 2;
            Console.WriteLine("{0}{1}{2}", G<ulong>.Id, G<D>.Id, G<B>.Id);








            Console.ReadKey();

        }
    }
}
