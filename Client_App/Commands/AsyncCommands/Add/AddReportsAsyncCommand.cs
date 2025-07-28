using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources.CustomComparers;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.Save;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Создать и открыть новое окно формы организации (1.0 и 2.0).
/// </summary>
public class AddReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is string par)
        {
            var mainWindow = (Desktop.MainWindow as MainWindow)!;

            var form10VM = new Form_10VM(ReportsStorage.LocalReports);

            #region AskIfTheOrganizationIsSeparateDivision

            var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Юридическое лицо" },
                        new ButtonDefinition { Name = "Обособленное подразделение" },
                        new ButtonDefinition { Name = "Отмена", IsCancel = true }
                    ],
                    CanResize = true,
                    ContentTitle = "Создание формы организации",
                    ContentMessage = "Ваша организация является юридическим лицом или территориальным обособленным подразделением?",
                    MinWidth = 300,
                    MinHeight = 125,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(mainWindow));

            switch (answer)
            {
                case "Юридическое лицо":
                {
                    form10VM.IsSeparateDivision = false;
                    break;
                }
                case "Обособленное подразделение":
                {
                    form10VM.IsSeparateDivision = true;
                    break;
                }
                case null or "Отмена":
                {
                    return;
                }
            }

            #endregion

            var window = new Form_10(form10VM) { DataContext = form10VM };
            await new SaveReportAsyncCommand(form10VM).AsyncExecute(null);
            await window.ShowDialog(mainWindow);

            mainWindow.SelectedReports = mainWindow.SelectedReports is null
                ? []
                : new ObservableCollectionWithItemPropertyChanged<IKey>(mainWindow.SelectedReports);

            var comparator = new CustomReportsComparer();
            var tmpReportsList = new List<Reports>(ReportsStorage.LocalReports.Reports_Collection);
            ReportsStorage.LocalReports.Reports_Collection.Clear();
            ReportsStorage.LocalReports.Reports_Collection
                .AddRange(tmpReportsList
                    .OrderBy(x => x.Master_DB.RegNoRep.Value, comparator)
                    .ThenBy(x => x.Master_DB.OkpoRep.Value, comparator));

            //await ReportsStorage.LocalReports.Reports_Collection.QuickSortAsync(); не нужно
        }
    }
}