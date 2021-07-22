using System;
using Collections;
using Models;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var t1 = new Reports();
            var t2 = new Reports();

            var y1 = t1.GetHashCode();
            var y2 = t2.GetHashCode();
        }
    }
}
