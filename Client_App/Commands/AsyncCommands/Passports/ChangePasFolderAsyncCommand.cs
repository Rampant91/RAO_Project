using Avalonia.Controls;
using System.Threading.Tasks;
using Client_App.Properties;
using ReactiveUI;

namespace Client_App.Commands.AsyncCommands.Passports;

/// <summary>
/// Excel -> Паспорта -> Изменить расположение паспортов по умолчанию.
/// </summary>
public class ChangePasFolderAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        OpenFolderDialog openFolderDialog = new() { Directory = Settings.Default.Properties["PasFolderDefaultPath"].DefaultValue.ToString() };
        Settings.Default.PasFolderDefaultPath = await openFolderDialog.ShowAsync(Desktop.MainWindow);
        Settings.Default.Save();
    }
}