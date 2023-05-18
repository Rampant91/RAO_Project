using Client_App.Views;
using Models.Collections;
using Models.DBRealization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client_App.ViewModels;
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands.Delete;

//  Удалить выбранную форму у выбранной организации
internal class DeleteFormAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        {
            var answer = await MainWindowVM.ShowMessage.Handle(new List<string>
                { "Вы действительно хотите удалить отчет?", "Уведомление", "Да", "Нет" });
            if (answer is "Да")
            {
                var mainWindow = Desktop.MainWindow as MainWindow;
                if (mainWindow.SelectedReports.Count() != 0)
                {
                    var selectedReports = new ObservableCollectionWithItemPropertyChanged<IKey>(mainWindow.SelectedReports);
                    var selectedReportsFirst = mainWindow.SelectedReports.First() as Reports;
                    if (parameter is IEnumerable param)
                    {

                        foreach (var item in param)
                        {
                            selectedReportsFirst.Report_Collection.Remove((Report)item);
                        }
                    }

                    mainWindow.SelectedReports = selectedReports;
                }

                await StaticConfiguration.DBModel.SaveChangesAsync();
            }
        }
    }
}