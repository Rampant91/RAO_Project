using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Services.DataAccess;
using Client_App.ViewModels.Forms;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Удалить выбранные строчки из формы (новое окно BaseFormVM, не legacy Form 2).
/// </summary>
public class NewDeleteRowsAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private Report Storage => formVM.Report;

    public override async Task AsyncExecute(object? parameter)
    {
        var candidates = FormRowMutationService.EnumerateForms(parameter);
        if (candidates.Length == 0)
            candidates = formVM.SelectedForms?.ToArray() ?? [];
        if (candidates.Length == 0 && formVM.SelectedForm != null)
            candidates = [formVM.SelectedForm];
        if (candidates.Length == 0)
            return;

        #region MessageDeleteLine

        var suffix = candidates.Length == 1 ? 'у' : 'и';
        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да", IsDefault = true },
                    new ButtonDefinition { Name = "Нет", IsCancel = true }
                ],
                CanResize = true,
                ContentTitle = "Удаление",
                ContentHeader = "Уведомление",
                ContentMessage = $"Вы действительно хотите удалить строчк{suffix}?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        if (answer is not "Да") return;

        var live = Storage.Rows.ToList<Form>();
        var param = FormRowMutationService.FilterLiveForms(candidates, live);
        if (param.Count == 0)
            return;

        var db = StaticConfiguration.DBModel;
        foreach (var item in param)
        {
            if (item.Id > 0)
                FormRowMutationService.RemoveFormRowById(db, Storage, item.Id);
            else
                FormRowMutationService.RemoveFormRowInstance(db, Storage, item);
        }

        formVM.NotifyRowMutation();
        if (formVM.CurrentPage > formVM.TotalPages && formVM.TotalPages > 0)
            await formVM.RevealLastPageAsync();
        else
            await formVM.RefreshVisibleRowsAfterMutationAsync();

        formVM.ClearFormRowSelection();
    }
}
