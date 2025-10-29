using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Views.ProgressBar;
using Models.Collections;
using FirebirdSql.Data.FirebirdClient;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

/// <summary>
/// Экспорт всех организаций в один файл .RAODB (отключён)
/// </summary>
public partial class ExportAllReportsOneFileAsyncCommand : ExportRaodbBaseAsyncCommand
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

        var dbReadOnlyPath = CreateTempDataBase();

        #region Progress = 7

        loadStatus = "Загрузка данных организаций";
        progressBarVM.ValueBar = 7;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        #endregion

        await using var dbReadOnly = new DBModel(dbReadOnlyPath);
        var countReports = await dbReadOnly.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Where(x => x.DBObservableId != null)
            .CountAsync(cancellationToken: cts.Token);

        if (countReports > 10)
        {
            #region ExportDoneMessage

            answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Да", IsDefault = true },
                        new ButtonDefinition { Name = "Отменить выгрузку", IsCancel = true }
                    ],
                    ContentTitle = "Выгрузка",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        $"Текущая база содержит {ReportsStorage.LocalReports.Reports_Collection.Count} форм организаций," +
                        $"{Environment.NewLine}выгрузка может занять длительный период времени. Продолжить операцию?",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                }).ShowDialog(Desktop.MainWindow));

            #endregion

            if (answer is "Отменить выгрузку") return;
        }
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

        var countExportedReports = 0;
        double progressBarDoubleValue = progressBarVM.ValueBar;
        foreach (var repsId in reportsIdArray)
        {
            try
            {
                #region GetReportsWithRows

                var repsFull = await dbReadOnly.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows11.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows12.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows13.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows14.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows15.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows16.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows17.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows18.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows19.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows21.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows22.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows23.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows24.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows25.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows26.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows27.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows28.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows29.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows210.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows211.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows212.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Rows41.OrderBy(x => x.NumberInOrder_DB))
                    .Include(reports => reports.Report_Collection).ThenInclude(x => x.Notes.OrderBy(x => x.Order))
                    .FirstAsync(x => x.Id == repsId, cancellationToken: cts.Token);

                #endregion

                await using var db = new DBModel(newDbPath);
                await db.Database.MigrateAsync(cancellationToken: cts.Token);

                await db.ReportsCollectionDbSet.AddAsync(repsFull, cts.Token);
                if (!db.DBObservableDbSet.Any())
                {
                    db.DBObservableDbSet.Add(new DBObservable());
                    db.DBObservableDbSet.Local.First().Reports_Collection.AddRange(db.ReportsCollectionDbSet.Local);
                }

                await db.SaveChangesAsync(cts.Token);

                var t = db.Database.GetDbConnection() as FbConnection;
                await t.CloseAsync();
                await t.DisposeAsync();
                await db.Database.CloseConnectionAsync();

                countExportedReports++;
                progressBarDoubleValue += (double)90 / countReports;
                progressBarVM.ValueBar = (int)Math.Floor(progressBarDoubleValue);
                loadStatus = $"выгружено {countExportedReports} из {countReports} организаций";
                progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";
            }
            catch (Exception ex)
            {
                var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                          $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
                ServiceExtension.LoggerManager.Error(msg);
            }
        }

        #region Progress = 100

        loadStatus = "Завершение выгрузки";
        progressBarVM.ValueBar = 100;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        #endregion

        if (!cts.IsCancellationRequested)
        {
            #region ExportDoneMessage

            answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Ок", IsDefault = true },
                        new ButtonDefinition { Name = "Открыть расположение файла" }
                    ],
                    ContentTitle = "Выгрузка",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Выгрузка всех организаций в отдельный" +
                                     $"{Environment.NewLine}файл .raodb завершена.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            if (answer is "Открыть расположение файлов")
            {
                Process.Start("explorer", newDbFolder);
            }
        }
        await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
    }

    #region RestoreReportsOrders

    private static async Task RestoreReportsOrders(Reports item)
    {
        if (item.Master_DB.FormNum_DB == "1.0")
        {
            if (item.Master_DB.Rows10[0].Id > item.Master_DB.Rows10[1].Id)
            {
                if (item.Master_DB.Rows10[0].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows10[0].NumberInOrder_DB = 2;
                }

                if (item.Master_DB.Rows10[1].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows10[1].NumberInOrder_DB = item.Master_DB.Rows10[1].NumberInOrder_DB == 2
                        ? 1
                        : 2;
                }

                item.Master_DB.Rows10.Sorted = false;
                await item.Master_DB.Rows10.QuickSortAsync();
            }
            else
            {
                if (item.Master_DB.Rows10[0].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows10[0].NumberInOrder_DB = 1;
                }

                if (item.Master_DB.Rows10[1].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows10[1].NumberInOrder_DB = item.Master_DB.Rows10[1].NumberInOrder_DB == 2
                        ? 1
                        : 2;
                }

                item.Master_DB.Rows10.Sorted = false;
                await item.Master_DB.Rows10.QuickSortAsync();
            }
        }

        if (item.Master_DB.FormNum_DB == "2.0")
        {
            if (item.Master_DB.Rows20[0].Id > item.Master_DB.Rows20[1].Id)
            {
                if (item.Master_DB.Rows20[0].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows20[0].NumberInOrder_DB = 2;
                }

                if (item.Master_DB.Rows20[1].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows20[1].NumberInOrder_DB = item.Master_DB.Rows20[1].NumberInOrder_DB == 2
                        ? 1
                        : 2;
                }

                item.Master_DB.Rows20.Sorted = false;
                await item.Master_DB.Rows20.QuickSortAsync();
            }
            else
            {
                if (item.Master_DB.Rows20[0].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows20[0].NumberInOrder_DB = 1;
                }

                if (item.Master_DB.Rows20[1].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows20[1].NumberInOrder_DB = item.Master_DB.Rows20[1].NumberInOrder_DB == 2
                        ? 1
                        : 2;
                }

                item.Master_DB.Rows20.Sorted = false;
                await item.Master_DB.Rows20.QuickSortAsync();
            }
        }
    }

    #endregion
}