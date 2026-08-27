using Client_App.Services.DataAccess;
using Client_App.ViewModels.Forms;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Добавить строку в форму.
/// </summary>
/// <param name="formVM">ViewModel отчёта.</param>
public class NewAddRowAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private Report Storage => formVM.Report;
    private string FormType => formVM.FormType;

    public override async Task AsyncExecute(object? parameter)
    {
        var frm = FormCreator.Create(FormType);
        frm.NumberInOrder_DB = formVM.GetNextNumberInOrder();
        frm.Report = Storage;
        frm.ReportId = Storage.Id;

        Storage[Storage.FormNum_DB].Add(frm);
        FormRowsPageLoader.TrackNewFormRow(StaticConfiguration.DBModel, Storage, frm);
        formVM.NotifyRowMutation();

        await formVM.RevealLastPageAsync();
        Debug.WriteLine(this);
    }
}
