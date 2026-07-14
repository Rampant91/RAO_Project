using Avalonia.Controls;
using Avalonia.Threading;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.RaodbExport.tempExportCommands
{
    private class ExportAllFormsFromListAsyncCommand : ExportRaodbBaseAsyncCommand
    {
        public override async Task AsyncExecute(object? parameter)
        {
            var cts = new CancellationTokenSource();
            string answer;

            #region ProgressBarInitialization

            await Dispatcher.UIThread.InvokeAsync(() => ProgressBar = new AnyTaskProgressBar(cts));
            var progressBar = ProgressBar;
            var progressBarVM = progressBar.AnyTaskProgressBarVM;

            progressBarVM.ExportType = "Экспорт_RAODB";
            progressBarVM.ExportName = "Выгрузка организаций в отдельный файл";
            progressBarVM.ValueBar = 5;
            var loadStatus = "Создание временной БД";
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

            #endregion

            var dbReadOnlyPath = await CreateTempDataBase(progressBar, cts);

            #region Progress = 7

            loadStatus = "Загрузка данных организаций";
            progressBarVM.ValueBar = 7;
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

            #endregion

            await using var dbReadOnly = new DBModel(dbReadOnlyPath);
           
            var newDbFolder = await new OpenFolderDialog().ShowAsync(Desktop.MainWindow);
            if (string.IsNullOrEmpty(newDbFolder)) return;
            var newDbPath = Path.Combine(newDbFolder, "Local_0.RAODB");

            var reportsIdArray = await dbReadOnly.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.DBObservable)
                .Where(x => x.DBObservableId != null)
                .Select(x => x.Id)
                .ToArrayAsync(cancellationToken: cts.Token);

            #region Progress = 8

            loadStatus = "Создание новой БД";
            progressBarVM.ValueBar = 8;
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

            #endregion

            #region Progress = 10

            loadStatus = "Загрузка организаций";
            progressBarVM.ValueBar = 10;
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

            #endregion
        }
    }
}
