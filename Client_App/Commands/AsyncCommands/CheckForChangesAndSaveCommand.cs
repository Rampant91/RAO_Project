using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Forms;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

public class CheckForChangesAndSaveCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    public override async Task<bool> AsyncExecute(object? parameter)
    {
        var desktop = (IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current?.ApplicationLifetime!;
        var dbm = StaticConfiguration.DBModel;

        try
        {
            if (!dbm.ChangeTracker.HasChanges()) return true;
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
            return true;
        }

        var formNum = formVM.FormType;
        var window = Desktop.Windows.FirstOrDefault(x => x.Name == formNum);

        #region MessageSaveChanges

        var res = await Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" },
                    new ButtonDefinition { Name = "Отмена" }
                ],
                ContentTitle = "Сохранение изменений",
                ContentHeader = "Уведомление",
                ContentMessage = $"Сохранить форму {formNum}?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(window ?? desktop.MainWindow));

        #endregion

        switch (res)
        {
            case "Да":
                await dbm.SaveChangesAsync();
                await new SaveReportAsyncCommand(formVM).AsyncExecute(null);
                return true;

            case "Нет":
                dbm.Restore();
                await dbm.SaveChangesAsync();
                return true;

            case "Отмена":
            default:
                return false; // Отмена
        }
    }
}
