using System.Threading.Tasks;
using Client_App.ViewModels;
using Models.DBRealization;

namespace Client_App.Commands.AsyncCommands.Save;

//  Сохраняет текущую базу, используется только для сохранения комментария формы
internal class SaveReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        await StaticConfiguration.DBModel.SaveChangesAsync();
        await ReportsStorage.LocalReports.Reports_Collection.QuickSortAsync();
    }
}