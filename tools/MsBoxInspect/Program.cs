using System.Reflection;
var asm = Assembly.LoadFrom(@"C:\Users\rampa\.nuget\packages\messagebox.avalonia\3.1.6\lib\netstandard2.0\MsBox.Avalonia.dll");
foreach (var t in asm.GetExportedTypes().Where(x => x.Name.Contains("Params") || x.Name.Contains("Input")))
    Console.WriteLine(t.FullName);
var mgr = asm.GetType("MsBox.Avalonia.MessageBoxManager")!;
foreach (var m in mgr.GetMethods(BindingFlags.Public | BindingFlags.Static))
    Console.WriteLine(m.Name);
