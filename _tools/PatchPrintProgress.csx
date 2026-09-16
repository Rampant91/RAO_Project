using System.Text;

var path = @"c:\Projects\RAO_Project\Client_App\Commands\AsyncCommands\ExcelExport\ExcelExportFormPrintAsyncCommand.cs";
var t = File.ReadAllText(path, Encoding.UTF8).Replace("\r\n", "\n");

static string MustInsertAfter(string text, string anchor, string insert, string label)
{
    var idx = text.IndexOf(anchor, StringComparison.Ordinal);
    if (idx < 0) throw new InvalidOperationException(label);
    return text.Insert(idx + anchor.Length, insert);
}

// After first progressBarVM assignment (single export)
t = MustInsertAfter(t,
    "var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));\n        var progressBarVM = progressBar.AnyTaskProgressBarVM;",
    "\n        progressBarVM.SetProgressBar(1, \"Подготовка...\", \"Выгрузка отчёта для печати\");",
    "first create");

// After second (batch overload) — only when not suppressDialogs
t = MustInsertAfter(t,
    "progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));\n            progressBarVM = progressBar.AnyTaskProgressBarVM;\n            progressBarVM.SetProgressBar(5, \"Определение имени файла\");",
    "",
    "second already has status — retitle via replace");

// Ensure early title on second path: replace the first SetProgressBar(5) in overload to include name
t = t.Replace(
    "progressBarVM = progressBar.AnyTaskProgressBarVM;\n            progressBarVM.SetProgressBar(5, \"Определение имени файла\");",
    "progressBarVM = progressBar.AnyTaskProgressBarVM;\n            progressBarVM.SetProgressBar(1, \"Подготовка...\", \"Выгрузка отчёта для печати\");\n            progressBarVM.SetProgressBar(5, \"Определение имени файла\");");

// Keep human title; do not pass filename ExportType into WindowTitle path as 4th arg when name already set
t = t.Replace(
    "progressBarVM.SetProgressBar(15, \"Загрузка отчёта\", \"Выгрузка отчёта для печати\", ExportType);",
    "progressBarVM.SetProgressBar(15, \"Загрузка отчёта\", \"Выгрузка отчёта для печати\");");

File.WriteAllText(path, t.Replace("\n", "\r\n"), new UTF8Encoding(false));
Console.WriteLine("Patched ExcelExportFormPrintAsyncCommand.cs");
