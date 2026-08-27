using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Services.DataAccess;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Messages;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_App.Views.Messages;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Добавить N строк в форму.
/// </summary>
public class NewAddRowsAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private Report Storage => formVM.Report;
    private string FormType => formVM.FormType;

    public override async Task AsyncExecute(object? parameter)
    {
        var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                .FirstOrDefault(w => w.IsActive);

        if (owner == null) return;

        var dialog = new AskIntMessageWindow(new AskIntMessageVM("Введите количество строк"));
        var rowCount = await dialog.ShowDialog<int?>(owner);

        if (rowCount > 0)
        {
            var number = formVM.GetNextNumberInOrder();
            var lst = new List<Form?>();
            for (var i = 0; i < rowCount; i++)
            {
                var frm = FormCreator.Create(FormType);
                frm.NumberInOrder_DB = number;
                frm.Report = Storage;
                frm.ReportId = Storage.Id;
                lst.Add(frm);
                number++;
            }
            formVM.Report.Rows.AddRange(lst);
            foreach (var frm in lst)
            {
                if (frm != null)
                    FormRowsPageLoader.TrackNewFormRow(StaticConfiguration.DBModel, Storage, frm);
            }

            formVM.NotifyRowMutation();
            await formVM.RevealLastPageAsync();
        }
    }
}
