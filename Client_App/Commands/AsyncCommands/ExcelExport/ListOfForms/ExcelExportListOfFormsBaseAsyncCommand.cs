using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Collections;

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
}