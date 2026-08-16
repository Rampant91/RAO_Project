using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Services.DataAccess;
using Client_App.ViewModels.Forms;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Удалить выбранные строчки из формы.
/// </summary>
/// <param name="formVM">ViewModel отчёта.</param>
public class NewDeleteRowsAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private Report Storage => formVM.Report;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not IEnumerable<IKey> enumerable)
            return;

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
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        if (answer is not "Да") return;

        // Сначала полный набор строк: иначе Sort→EnsureAll подтянет удалённые обратно из БД,
        // а Remove с page-only коллекцией не перенумерует остальные страницы.
        await formVM.EnsureAllRowsForMutationAsync();

        var minItem = param.Min(x => x.Order);
        var db = StaticConfiguration.DBModel;
        foreach (var item in param)
        {
            FormRowsPageLoader.TrackDeletedFormRow(db, Storage, item);
        }

        if (formVM.DbTotalRows.HasValue)
            formVM.DbTotalRows = Math.Max(0, formVM.DbTotalRows.Value - param.Length);

        formVM.SortForm.Execute(minItem);
        formVM.UpdateFormList();
        formVM.UpdatePageInfo();
        formVM.IsCanSaveReportEnabled = true;
    }
}
