using Models.Collections;
using Models.DBRealization;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Forms;
using Models.Interfaces;

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

        if (answer is not "Да" || parameter is null) return;

        await using var db = StaticConfiguration.DBModel;
        var param = ((ObservableCollectionWithItemPropertyChanged<IKey>)parameter).First();
        var repsToRemove = (Reports)param;
        foreach (var key1 in repsToRemove.Report_Collection)
        {
            var repToRemove = (Report)key1;
            foreach (var key2 in repToRemove.Rows)
            {
                var formToRemove = (Form)key2;
                db.Remove(formToRemove);
            }
            db.Remove(repToRemove);
        }
        db.Remove(repsToRemove);
        repsToRemove?.Report_Collection.Clear();
        ReportsStorage.LocalReports.Reports_Collection.Remove(repsToRemove);
        await StaticConfiguration.DBModel.SaveChangesAsync().ConfigureAwait(false);
    }
}