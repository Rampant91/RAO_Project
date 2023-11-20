using Client_App.Views;
using Models.Collections;
using Models.DBRealization;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands.Delete;

//  Удалить выбранную форму у выбранной организации
internal class DeleteFormAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        {
            #region MessageDeleteReport

            var answer = await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions = new[]
                        {
                        new ButtonDefinition { Name = "Да", IsDefault = true},
                        new ButtonDefinition { Name = "Нет", IsCancel = false}
                        },
                        ContentTitle = "Уведомление",
                        ContentHeader = "Уведомление",
                        ContentMessage = "Вы действительно хотите удалить отчет?",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow);

            #endregion

            if (answer is not "Да") return;

            await using var db = StaticConfiguration.DBModel;
            var mainWindow = Desktop.MainWindow as MainWindow;
            if (mainWindow!.SelectedReports.Any())
            {
                var selectedReports = new ObservableCollectionWithItemPropertyChanged<IKey>(mainWindow.SelectedReports);
                var selectedReportsFirst = mainWindow.SelectedReports.First() as Reports;
                var param = parameter as System.Collections.Generic.IEnumerable<Report>;
                var repToRemove = param.First();
                foreach (var form in repToRemove.Rows)
                {
                    db.Remove(form);
                }
                db.Remove(repToRemove);
                selectedReportsFirst.Report_Collection.Remove(repToRemove);
                mainWindow.SelectedReports = selectedReports;
            }
            await StaticConfiguration.DBModel.SaveChangesAsync();
        }
    }
}