#r "C:/Users/rampa/.nuget/packages/messagebox.avalonia/3.1.6/lib/netstandard2.0/MsBox.Avalonia.dll"
using System;
using System.Linq;
using System.Reflection;
var asm = Assembly.LoadFrom(@"C:\Users\rampa\.nuget\packages\messagebox.avalonia\3.1.6\lib\netstandard2.0\MsBox.Avalonia.dll");
var t = asm.GetType("MsBox.Avalonia.MessageBoxManager");
foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.Static).OrderBy(x => x.Name))
    Console.WriteLine(m);
