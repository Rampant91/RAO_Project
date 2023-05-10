using System.Threading.Tasks;
using Client_App.ViewModels;
using Models.Collections;

namespace Client_App.Commands.AsyncCommands;

//  Поменять местами юр. лицо и обособленное подразделение
internal class ChangeReportOrderAsyncCommand : BaseAsyncCommand
{
    private readonly ChangeOrCreateVM _ChangeOrCreateViewModel;
    private Report Storage => _ChangeOrCreateViewModel.Storage;

    public ChangeReportOrderAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel)
    {
        _ChangeOrCreateViewModel = changeOrCreateViewModel;
    }

    public override async Task AsyncExecute(object? parameter)
    {
        if (Storage.FormNum.Value == "1.0")
        {
            Storage.Rows10.Sorted = false;
            var tmp = Storage.Rows10[0].Order;
            Storage.Rows10[0].SetOrder(Storage.Rows10[1].Order);
            Storage.Rows10[1].SetOrder(tmp);
            await Storage.Rows10.QuickSortAsync();
        }
        if (Storage.FormNum.Value == "2.0")
        {
            Storage.Rows20.Sorted = false;
            var tmp = Storage.Rows20[0].Order;
            Storage.Rows20[0].SetOrder(Storage.Rows20[1].Order);
            Storage.Rows20[1].SetOrder(tmp);
            await Storage.Rows20.QuickSortAsync();
        }
    }
}