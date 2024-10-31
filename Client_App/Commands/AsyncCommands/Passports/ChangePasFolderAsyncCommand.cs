using Avalonia.Controls;
using System.Threading.Tasks;
using Client_App.Properties;
using ReactiveUI;
using OfficeOpenXml.Style;

namespace Client_App.Commands.AsyncCommands.Passports;

/// <summary>
/// Excel -> Паспорта -> Изменить расположение паспортов по умолчанию.
/// </summary>
public class ChangePasFolderAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        OpenFolderDialog openFolderDialog = new() { Directory = Settings.Default.PasFolderDefaultPath };
        var selectedFolderPath = await openFolderDialog.ShowAsync(Desktop.MainWindow);
        if (selectedFolderPath != null)
        {
            Settings.Default.PasFolderDefaultPath = selectedFolderPath;
            Settings.Default.Save();
        }
    }
}