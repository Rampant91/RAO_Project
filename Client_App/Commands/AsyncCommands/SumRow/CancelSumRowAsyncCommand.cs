using Client_App.ViewModels;
using DynamicData;
using Models.Collections;
using Models.Forms.Form2;
using Models.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.SumRow;

//  Отменяет группировку по наименованию в формах 2.1 и 2.2
internal class CancelSumRowAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    private Report Storage => changeOrCreateViewModel.Storage;

    public override async Task AsyncExecute(object? parameter)
    {
        if (Storage.FormNum_DB == "2.1")
        {
            await UnSum21();
            Storage.Rows21.Sorted = false;
            await Storage.Rows21.QuickSortAsync();
        }
        if (Storage.FormNum_DB == "2.2")
        {
            await UnSum22();
            Storage.Rows21.Sorted = false;
            await Storage.Rows22.QuickSortAsync();
        }
    }

    public async void UnSumRow(object sender, Avalonia.Interactivity.RoutedEventArgs args)
    {
        await AsyncExecute(null);
    }

    private Task UnSum21()
    {
        var sumRows = Storage.Rows21
            .Where(x => x.Sum_DB)
            .ToList();

        Storage.Rows21.RemoveMany(sumRows);

        var sumRowsGroup = Storage.Rows21
            .Where(x => x.SumGroup_DB)
            .ToList();
        foreach (var row in sumRowsGroup)
        {
            row.RefineMachineName_Hidden_Set.Set(true);
            row.MachineCode_Hidden_Set.Set(true);
            row.MachinePower_Hidden_Set.Set(true);
            row.NumberOfHoursPerYear_Hidden_Set.Set(true);
            row.RefineMachineName_Hidden_Get.Set(true);
            row.MachineCode_Hidden_Get.Set(true);
            row.MachinePower_Hidden_Get.Set(true);
            row.NumberOfHoursPerYear_Hidden_Get.Set(true);
            row.SumGroup_DB = false;
            row._BaseColor = ColorType.None;
        }
        var rows = Storage.Rows21
            .GetEnumerable()
            .ToList();
        var count = 1;
        foreach (var key in rows)
        {
            var row = (Form21)key;
            row.NumberInOrder_DB = count;
            count++;
            row.NumberInOrderSum_DB = "";
        }

        return Task.CompletedTask;
    }

    private Task UnSum22()
    {
        var sumRows = Storage.Rows22
            .Where(x => x.Sum_DB)
            .ToList();

        Storage.Rows22.RemoveMany(sumRows);

        var sumRowsGroup = Storage.Rows22
            .Where(x => x.SumGroup_DB)
            .ToList();
        foreach (var row in sumRowsGroup)
        {
            row.StoragePlaceName_Hidden_Set.Set(true);
            row.StoragePlaceCode_Hidden_Set.Set(true);
            row.PackName_Hidden_Set.Set(true);
            row.PackType_Hidden_Set.Set(true);
            row.StoragePlaceName_Hidden_Get.Set(true);
            row.StoragePlaceCode_Hidden_Get.Set(true);
            row.PackName_Hidden_Get.Set(true);
            row.PackType_Hidden_Get.Set(true);
            row.SumGroup_DB = false;
            row._BaseColor = ColorType.None;
        }
        var rows = Storage.Rows22
            .GetEnumerable()
            .ToList();
        var count = 1;
        foreach (var item in rows)
        {
            var row = (Form22)item;
            row.NumberInOrder_DB = count;
            count++;
            row.NumberInOrderSum_DB = "";
        }

        return Task.CompletedTask;
    }
}