using Models.Collections;
using Models.DBRealization;
using System.Collections;
using System.Threading.Tasks;
using Avalonia.Controls;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;

namespace Client_App.Commands.AsyncCommands.Delete;

//  Удалить выбранную организацию
internal class DeleteReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        #region MessageExcelExportComplete

        var answer = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" }
                },
                ContentTitle = "Уведомление",
                ContentHeader = "Уведомление",
                ContentMessage = "Вы действительно хотите удалить организацию?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow);

        #endregion

        if (answer is not "Да") return;

        if (parameter is IEnumerable param)
        {
            foreach (var item in param)
            {
                MainWindowVM.LocalReports.Reports_Collection.Remove((Reports)item);
            }
        }
        await StaticConfiguration.DBModel.SaveChangesAsync();
        //await Local_Reports.Reports_Collection.QuickSortAsync();
    }
}