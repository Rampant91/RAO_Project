using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Resources;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using FirebirdSql.Data.FirebirdClient;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

/// <summary>
/// Экспорт всех организаций организации в отдельные файлы .RAODB
/// </summary>
public partial class ExportAllReportAsyncCommand : ExportRaodbBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        string? answer;

        #region ProgressBarInitialization

        await Dispatcher.UIThread.InvokeAsync(() => ProgressBar = new AnyTaskProgressBar(cts));
        var progressBar = ProgressBar;
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        progressBarVM.ExportType = "Экспорт_RAODB";
        progressBarVM.ExportName = "Выгрузка организаций в отдельные файлы";
        progressBarVM.ValueBar = 5;
        var loadStatus = "Создание временной БД";
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        #endregion

        var dbReadOnlyPath = CreateTempDataBase();
        await using var dbReadOnly = new DBModel(dbReadOnlyPath);
        var countReport = await dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(rep => rep.Reports)
            .ThenInclude(x => x.DBObservable)
            .Where(x => x.Reports.DBObservableId != null)
            .Where(rep => rep.FormNum_DB != "1.0"
                && rep.FormNum_DB != "2.0"
                && rep.FormNum_DB != "4.0"
                && rep.FormNum_DB != "5.0")
            .CountAsync(cancellationToken: cts.Token);
        if (countReport > 10)
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
                        $"Текущая база содержит {countReport} отчетов," +
                        $"{Environment.NewLine}выгрузка может занять длительный период времени. Продолжить операцию?",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                }).ShowDialog(Desktop.MainWindow));

            #endregion

            if (answer is not "Да") return;
        }
        var folderPath = await new OpenFolderDialog().ShowAsync(Desktop.MainWindow);
        if (string.IsNullOrEmpty(folderPath)) return;

        var reportIdArray = await dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(rep => rep.Reports)
            .ThenInclude(x => x.DBObservable)
            .Where(x => x.Reports.DBObservableId != null)
            .Where(rep => rep.FormNum_DB != "1.0"
                && rep.FormNum_DB != "2.0"
                && rep.FormNum_DB != "4.0"
                && rep.FormNum_DB != "5.0")
            .Select(x => x.Id)
            .ToArrayAsync(cancellationToken: cts.Token);

        #region Progress = 10

        loadStatus = "Загрузка данных отчетов";
        progressBarVM.ValueBar = 10;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        #endregion

        var countExportedReport = 0;
        double progressBarDoubleValue = progressBarVM.ValueBar;
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 20 };
        await Task.Run(async () =>
        {
            await Parallel.ForEachAsync(reportIdArray, parallelOptions, async (repsId, parallelCts) =>
            {
                try
                {
                    await using var dbReadOnly2 = new DBModel(dbReadOnlyPath);

                    #region GetReportWithRows

                    var repFull = await dbReadOnly2.ReportCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Include(rep => rep.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows10.OrderBy(x => x.NumberInOrder_DB))
                        .Include(rep => rep.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows20.OrderBy(x => x.NumberInOrder_DB))
                        .Include(rep => rep.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows40.OrderBy(x => x.NumberInOrder_DB))
                        .Include(rep => rep.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows50.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows11.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows12.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows13.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows14.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows15.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows16.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows17.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows18.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows19.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows21.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows22.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows23.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows24.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows25.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows26.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows27.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows28.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows29.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows210.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows211.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows212.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows41.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows51.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows52.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows53.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows54.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows55.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows56.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Rows57.OrderBy(x => x.NumberInOrder_DB))
                        .Include(x => x.Notes.OrderBy(x => x.Order))
                        .FirstAsync(x => x.Id == repsId, cancellationToken: parallelCts);

                    #endregion

                    var dt = DateTime.Now;
                    var fileNameTmp = $"Report_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}_{dt.Millisecond}_{dt.Microsecond}";
                    var fullPathTmp = Path.Combine(BaseVM.TmpDirectory, $"{fileNameTmp}.RAODB");
                    var filename = repFull.Reports.Master_DB.FormNum_DB switch
                    {
                        "1.0" =>
                            StaticStringMethods.RemoveForbiddenChars(repFull.Reports.Master.RegNoRep.Value) +
                            $"_{StaticStringMethods.RemoveForbiddenChars(repFull.Reports.Master.OkpoRep.Value)}" +
                            $"_{repFull.FormNum_DB}" +
                            $"_{StaticStringMethods.RemoveForbiddenChars(repFull.StartPeriod_DB)}" +
                            $"_{StaticStringMethods.RemoveForbiddenChars(repFull.EndPeriod_DB)}" +
                            $"_{repFull.CorrectionNumber_DB}" +
                            $"_{Assembly.GetExecutingAssembly().GetName().Version}",

                        "2.0" when repFull.Reports.Master.Rows20.Count > 0 =>
                            StaticStringMethods.RemoveForbiddenChars(repFull.Reports.Master.RegNoRep.Value) +
                            $"_{StaticStringMethods.RemoveForbiddenChars(repFull.Reports.Master.OkpoRep.Value)}" +
                            $"_{repFull.FormNum_DB}" +
                            $"_{StaticStringMethods.RemoveForbiddenChars(repFull.Year_DB)}" +
                            $"_{repFull.CorrectionNumber_DB}" +
                            $"_{Assembly.GetExecutingAssembly().GetName().Version}",

                        "4.0" when repFull.Reports.Master.Rows40.Count > 0 =>
                            $"{repFull.Reports.Master.Rows40.OrderBy(r => r.NumberInOrder_DB).ToList()[0].CodeSubjectRF_DB}" +
                            $"_{repFull.FormNum_DB}" +
                            $"_{StaticStringMethods.RemoveForbiddenChars(repFull.Year_DB)}" +
                            $"_{repFull.CorrectionNumber_DB}" +
                            $"_{Assembly.GetExecutingAssembly().GetName().Version}",

                        "5.0" when repFull.Reports.Master.Rows50.Count > 0 =>
                            $"{repFull.FormNum_DB}" +
                            $"_{StaticStringMethods.RemoveForbiddenChars(repFull.Year_DB)}" +
                            $"_{repFull.CorrectionNumber_DB}" +
                            $"_{Assembly.GetExecutingAssembly().GetName().Version}",

                        _ => throw new ArgumentOutOfRangeException()
                    };
                    var fullPath = Path.Combine(folderPath, $"{filename}.RAODB");

                    fullPathTmp = InsertIndexInFilePath(fullPathTmp);
                    var db = new DBModel(fullPathTmp);
                    await db.Database.MigrateAsync(cancellationToken: parallelCts);
                    await db.ReportCollectionDbSet.AddAsync(repFull, parallelCts);
                    if (!db.DBObservableDbSet.Any())
                    {
                        db.DBObservableDbSet.Add(new DBObservable());
                        db.DBObservableDbSet.Local.First().Reports_Collection.AddRange(db.ReportsCollectionDbSet.Local);
                    }
                    await db.SaveChangesAsync(parallelCts);

                    var t = db.Database.GetDbConnection() as FbConnection;
                    await t.CloseAsync();
                    await t.DisposeAsync();
                    await db.Database.CloseConnectionAsync();

                    fullPath = InsertIndexInFilePath(fullPath);
                    try
                    {
                        File.Copy(fullPathTmp, fullPath);
                        File.Delete(fullPathTmp);
                    }
                    catch (Exception e)
                    {
                        #region FailedCopyFromTempMessage

                        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                            {
                                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                                ContentTitle = "Выгрузка",
                                ContentHeader = "Ошибка",
                                ContentMessage =
                                    "При копировании файла базы данных из временной папки возникла ошибка." +
                                    $"{Environment.NewLine}Экспорт не выполнен." +
                                    $"{Environment.NewLine}{e.Message}",
                                MinWidth = 400,
                                MinHeight = 150,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen
                            })
                            .ShowDialog(Desktop.MainWindow));

                        #endregion

                        return;
                    }

                    countExportedReport++;
                    progressBarDoubleValue += (double)90 / countReport;
                    progressBarVM.ValueBar = (int)Math.Floor(progressBarDoubleValue);
                    loadStatus = $"выгружено {countExportedReport} из {countReport} отчетов";
                    progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";
                }
                catch (Exception ex)
                {
                    var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                              $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
                    ServiceExtension.LoggerManager.Error(msg);
                }
            });
        }, cts.Token);

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
                        new ButtonDefinition { Name = "Открыть расположение файлов" }
                    ],
                    ContentTitle = "Выгрузка",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Выгрузка всех организаций в отдельные" +
                                     $"{Environment.NewLine}файлы .raodb завершена.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            if (answer is "Открыть расположение файлов")
            {
                Process.Start("explorer", folderPath);
            }
        }
        await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
    }

    #region InsertIndexInFilePath

    /// <summary>
    /// Проверяет, существует ли файл по пути fullPath и возвращает имя файла с добавлением индекса
    /// </summary>
    /// <param name="fullPath">Путь к файлу назначения</param>
    /// <returns>Полный путь до файла с добавлением индекса при необходимости</returns>
    private static string InsertIndexInFilePath(string fullPath)
    {
        while (File.Exists(fullPath)) // insert index if file already exist
        {
            var matches = RaodbFileNameRegex().Matches(fullPath);
            if (matches.Count > 0)
            {
                foreach (var match in matches.Cast<Match>())
                {
                    if (!int.TryParse(match.Groups[2].Value, out var index)) return fullPath;
                    fullPath = match.Groups[1].Value + $"#{index + 1}.RAODB";
                }
            }
            else
            {
                fullPath = fullPath.TrimEnd(".RAODB".ToCharArray()) + "#1.RAODB";
            }
        }
        return fullPath;
    }

    #endregion

    [GeneratedRegex(@"(.+)#(\d+)(?=\.RAODB)")]
    private static partial Regex RaodbFileNameRegex();
}