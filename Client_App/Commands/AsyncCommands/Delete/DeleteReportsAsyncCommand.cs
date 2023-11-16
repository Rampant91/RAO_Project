using Models.Collections;
using Models.DBRealization;
using System.Collections;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;

namespace Client_App.Commands.AsyncCommands.Delete;

//  Удалить выбранную организацию
internal class DeleteReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        #region MessageDeleteReports

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
                var rep = item as Reports;
                rep?.Report_Collection.Clear();
                ReportsStorage.LocalReports.Reports_Collection.Remove((Reports)item);
            }
        }
        await StaticConfiguration.DBModel.SaveChangesAsync().ConfigureAwait(false);
        //await Local_Reports.Reports_Collection.QuickSortAsync();
    }
}