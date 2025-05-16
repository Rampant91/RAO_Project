using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Avalonia.Threading;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;

public abstract class ExcelExportListOfFormsBaseAsyncCommand : ExcelBaseAsyncCommand
{
    #region GetReportsList

    /// <summary>
    /// Получение списка организаций.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="formNum">Номер головной формы организации.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список организаций.</returns>
    private protected static async Task<List<Reports>> GetReportsList(DBModel db, string formNum, CancellationTokenSource cts)
    {
        return formNum switch
        {
            "1.0" => await db.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(reps => reps.DBObservable)
                .Include(reps => reps.Master_DB).ThenInclude(x => x.Rows10)
                .Include(reps => reps.Report_Collection)
                .Where(reps => reps.DBObservable != null && reps.Master_DB.FormNum_DB == formNum)
                .ToListAsync(cts.Token),

            "2.0" => await db.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(reps => reps.DBObservable)
                .Include(reps => reps.Master_DB).ThenInclude(x => x.Rows20)
                .Include(reps => reps.Report_Collection)
                .Where(reps => reps.DBObservable != null && reps.Master_DB.FormNum_DB == formNum)
                .ToListAsync(cts.Token),

            _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
        };
    }

    #endregion

    #region ReportsCountCheck

    /// <summary>
    /// Подсчёт количества организаций. При количестве равном 0, выводится сообщение, операция завершается.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private protected static async Task ReportsCountCheck(DBModel db, string formNum, AnyTaskProgressBar? progressBar, CancellationTokenSource cts)
    {
        var countReports = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(x => x.Master_DB)
            .Where(x => x.DBObservable != null && x.Master_DB.FormNum_DB == formNum)
            .CountAsync(cts.Token);

        if (countReports == 0)
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        $"Не удалось совершить выгрузку списка всех отчетов по форме {formNum[0]} с указанием количества строк," +
                        $"{Environment.NewLine}поскольку в текущей базе отсутствует отчетность по формам {formNum[0]}./",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
    }

    #endregion
}