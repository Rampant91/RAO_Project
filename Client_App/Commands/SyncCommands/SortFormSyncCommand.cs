using System.Linq;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form2;

namespace Client_App.Commands.SyncCommands;

public class SortFormSyncCommand : BaseCommand
{
    private readonly BaseFormVM? _vm;

    private readonly ChangeOrCreateVM? _changeOrCreateViewModel;

    public SortFormSyncCommand(BaseFormVM vm)
    {
        _vm = vm;
    }

    public SortFormSyncCommand(ChangeOrCreateVM changeOrCreateViewModel)
    {
        _changeOrCreateViewModel = changeOrCreateViewModel;
    }

    private Report Report => _changeOrCreateViewModel is null ? _vm.Report : _changeOrCreateViewModel.Storage;

    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter)
    {
        var formNum = Report.FormNum_DB;
        var minItem = (long)(parameter ?? Report[formNum].GetEnumerable().Min(x => x.Order));
        switch (formNum)
        {
            case "2.1":
            {
                var count = 1;
                var rows = Report[formNum]
                    .GetEnumerable()
                    .Cast<Form21>()
                    .ToArray();
                foreach (var row in rows)
                {
                    row.NumberInOrder_DB = count;
                    count++;
                    row.NumberInOrderSum_DB = string.Empty;
                }
                break;
            }
            case "2.2":
            {
                var count = 1;
                var rows = Report[formNum]
                    .GetEnumerable()
                    .Cast<Form22>()
                    .ToArray();
                foreach (var row in rows)
                {
                    row.NumberInOrder_DB = count;
                    count++;
                    row.NumberInOrderSum_DB = string.Empty;
                }
                break;
            }
            default:
            {
                Report.Sort();
                var itemQ = Report.Rows
                    .GetEnumerable()
                    .Where(x => x.Order >= minItem)
                    .Select(x => (Form)x)
                    .ToArray();
                foreach (var form in itemQ)
                {
                    form.NumberInOrder_DB = (int)minItem;
                    form.NumberInOrder.OnPropertyChanged();
                    minItem++;
                }
                break;
            }
        }
    }
}