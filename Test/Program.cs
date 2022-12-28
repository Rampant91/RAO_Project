using Spravochniki;
using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Threading;

namespace Test;

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<Test>();
    }
}

public class Test
{
    public Test()
    {
        ArrayTest();
        ListTest();
    }

    [Benchmark]
    public void ArrayTest()
    {
        var a = Spravochniks.SprTypesToRadionuclids
            .Where(item => item.Item1 == "азот-13")
            .Select(item => item.Item2)
            .ToArray();
        var b = a.Length;
        Thread.Sleep(500);
    }

    [Benchmark]
    public void ListTest()
    {
        var a = Spravochniks.SprTypesToRadionuclids
            .Where(item => item.Item1 == "азот-13")
            .Select(item => item.Item2)
            .ToList();
        var b = a.Count;
        Thread.Sleep(500);
    }
}