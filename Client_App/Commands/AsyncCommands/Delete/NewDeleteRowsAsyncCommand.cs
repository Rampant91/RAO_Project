using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels.Forms;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.Forms;
using Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Удалить выбранные строчки из формы.
/// </summary>
/// <param name="changeOrCreateViewModel">ViewModel отчёта.</param>
public class NewDeleteRowsAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private Report Storage => formVM.Report;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is IEnumerable<IKey> enumerable)
        {
            var param = enumerable.Cast<Form>().ToArray();

            #region MessageDeleteLine

            var suffix = param.Length == 1 ? 'у' : 'и';
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
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            if (answer is not "Да") return;

            var minItem = param.Min(x => x.Order);
            foreach (var item in param)
            {
                Storage.Rows.Remove(item);
            }
            formVM.SortForm.Execute(minItem);
            formVM.UpdateFormList();
            formVM.UpdatePageInfo();

        }
        //if (parameter is null) return;

        //var param = ((IEnumerable<IKey>)parameter).ToArray();

        //#region MessageDeleteLine

        //var suffix = param.Length == 1 ? 'у' : 'и';
        //var answer = await MessageBox.Avalonia.MessageBoxManager
        //    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
        //    {
        //        ButtonDefinitions =
        //        [
        //            new ButtonDefinition { Name = "Да", IsDefault = true },
        //            new ButtonDefinition { Name = "Нет", IsCancel = true }
        //        ],
        //        ContentTitle = "Удаление строки",
        //        ContentHeader = "Уведомление",
        //        ContentMessage = $"Вы действительно хотите удалить строчк{suffix}?",
        //        MinWidth = 400,
        //        WindowStartupLocation = WindowStartupLocation.CenterOwner
        //    })
        //    .ShowDialog(Desktop.MainWindow);

        //#endregion

        //if (answer is "Да" && parameter is IEnumerable)
        //{
        //    var minItem = param.Min(x => x.Order);
        //    foreach (var item in param)
        //    {
        //        if (item != null)
        //        {
        //            Storage.Rows.Remove(item);
        //        }
        //    }
        //    changeOrCreateViewModel.SortForm.Execute(minItem);
        //}
    }
}