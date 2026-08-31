using System.Text;
using System.Text.RegularExpressions;

var clientApp = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "Client_App"));

if (args.Length == 0)
{
    Console.WriteLine("Use: fix-msbox-show | fix-windows-show | fix-phase2 | fix-axaml");
    return;
}

switch (args[0].ToLowerInvariant())
{
    case "fix-msbox-show":
        FixMsBoxShowDialogOnly(clientApp);
        break;
    case "fix-windows-show":
        FixWindowShowDialog(clientApp);
        break;
    case "fix-phase2":
        FixRemainingMsBoxAndClipboard(clientApp);
        FixScrollBindings(clientApp);
        break;
    case "fix-axaml":
        FixAxamlBindings(clientApp);
        break;
    case "fix-selected-items":
        FixSelectedItemsSource(clientApp);
        break;
}

static void FixMsBoxShowDialogOnly(string clientApp)
{
    var changed = 0;
    foreach (var path in Directory.EnumerateFiles(clientApp, "*.cs", SearchOption.AllDirectories))
    {
        if (path.Contains("\\obj\\", StringComparison.Ordinal) || path.Contains("\\bin\\", StringComparison.Ordinal))
            continue;

        var text = File.ReadAllText(path, Encoding.UTF8);
        if (!text.Contains("MessageBoxManager", StringComparison.Ordinal))
            continue;

        var original = text;
        text = Regex.Replace(text, @"(MessageBoxManager[\s\S]*?)\.ShowDialog\(", "$1.ShowWindowDialogAsync(", RegexOptions.Singleline);
        text = Regex.Replace(text, @"(MessageBoxManager[\s\S]*?)\.Show\(\)", "$1.ShowAsync()", RegexOptions.Singleline);

        if (original != text)
        {
            File.WriteAllText(path, text, Encoding.UTF8);
            changed++;
        }
    }

    Console.WriteLine($"Fixed MsBox Show/ShowDialog in {changed} files");
}

static void FixWindowShowDialog(string clientApp)
{
    var replacements = new (string Old, string New)[]
    {
        (".ShowWindowDialogAsync(mainWindow)", ".ShowDialog(mainWindow)"),
        (".ShowWindowDialogAsync(desktop.MainWindow)", ".ShowDialog(desktop.MainWindow)"),
        (".ShowWindowDialogAsync(owner)", ".ShowDialog(owner)"),
        (".ShowWindowDialogAsync(progressBar ?? Desktop.MainWindow)", ".ShowDialog(progressBar ?? Desktop.MainWindow)"),
        (".ShowWindowDialogAsync(window ?? Desktop.MainWindow)", ".ShowDialog(window ?? Desktop.MainWindow)"),
        (".ShowWindowDialogAsync(Desktop.Windows[0])", ".ShowDialog(Desktop.Windows[0])"),
    };

    var changed = 0;
    foreach (var path in Directory.EnumerateFiles(clientApp, "*.cs", SearchOption.AllDirectories))
    {
        if (path.Contains("\\obj\\", StringComparison.Ordinal)) continue;
        var text = File.ReadAllText(path, Encoding.UTF8);
        var original = text;
        foreach (var (old, @new) in replacements)
            text = text.Replace(old, @new);

        if (original != text)
        {
            File.WriteAllText(path, text, Encoding.UTF8);
            changed++;
        }
    }

    Console.WriteLine($"Reverted window ShowWindowDialogAsync in {changed} files");
}

static void FixRemainingMsBoxAndClipboard(string clientApp)
{
    var changed = 0;
    foreach (var path in Directory.EnumerateFiles(clientApp, "*.cs", SearchOption.AllDirectories))
    {
        if (path.Contains("\\obj\\", StringComparison.Ordinal) || path.Contains("\\bin\\", StringComparison.Ordinal))
            continue;

        var text = File.ReadAllText(path, Encoding.UTF8);
        var original = text;

        text = text.Replace("Application.Current?.Clipboard", "Avalonia11Compat.MainClipboard");
        text = text.Replace("Application.Current.Clipboard", "Avalonia11Compat.MainClipboard!");

        if (text.Contains("Avalonia11Compat.", StringComparison.Ordinal) &&
            !text.Contains("using Client_App.Resources;", StringComparison.Ordinal))
        {
            text = InsertUsing(text, "using Client_App.Resources;");
        }

        if (original != text)
        {
            File.WriteAllText(path, text, Encoding.UTF8);
            changed++;
        }
    }

    Console.WriteLine($"Fixed clipboard in {changed} files");
}

static void FixScrollBindings(string clientApp) =>
    Console.WriteLine("Scroll bindings already migrated");

static void FixAxamlBindings(string clientApp)
{
    var changed = 0;
    foreach (var path in Directory.EnumerateFiles(clientApp, "*.axaml", SearchOption.AllDirectories))
    {
        if (path.Contains("\\obj\\", StringComparison.Ordinal)) continue;

        var text = File.ReadAllText(path, Encoding.UTF8);
        var original = text;

        text = text.Replace("SelectedItemsSource", "SelectedItems");
        text = text.Replace("Items=\"{Binding", "ItemsSource=\"{Binding");
        text = text.Replace(" Items=\"", " ItemsSource=\"");
        text = Regex.Replace(text, @"\s*AlternatingRowBackground=""[^""]*""", string.Empty);
        text = Regex.Replace(text, @"<Setter Property=""AlternatingRowBackground"" Value=""[^""]*""\s*/>", string.Empty);
        text = text.Replace(" StaysOpen=\"False\"", string.Empty);
        text = text.Replace("StaysOpen=\"True\"", "IsLightDismissEnabled=\"False\"");

        if (original != text)
        {
            File.WriteAllText(path, text, Encoding.UTF8);
            changed++;
        }
    }

    Console.WriteLine($"Fixed AXAML bindings in {changed} files");
}

static void FixSelectedItemsSource(string clientApp)
{
    var changed = 0;
    foreach (var path in Directory.EnumerateFiles(clientApp, "*.axaml", SearchOption.AllDirectories))
    {
        if (path.Contains("\\obj\\", StringComparison.Ordinal)) continue;
        var text = File.ReadAllText(path, Encoding.UTF8);
        if (!text.Contains("SelectedItemsSource", StringComparison.Ordinal)) continue;
        File.WriteAllText(path, text.Replace("SelectedItemsSource", "SelectedItems"), Encoding.UTF8);
        changed++;
    }
    Console.WriteLine($"Fixed SelectedItemsSource in {changed} files");
}

static string InsertUsing(string text, string usingLine)
{
    if (text.Contains(usingLine, StringComparison.Ordinal))
        return text;

    var idx = 0;
    while (idx < text.Length)
    {
        var lineEnd = text.IndexOf('\n', idx);
        if (lineEnd < 0)
            break;
        var line = text[idx..(lineEnd + 1)];
        if (!line.StartsWith("using ", StringComparison.Ordinal) && !string.IsNullOrWhiteSpace(line))
            return text.Insert(idx, usingLine + Environment.NewLine);

        idx = lineEnd + 1;
    }

    return usingLine + Environment.NewLine + text;
}
