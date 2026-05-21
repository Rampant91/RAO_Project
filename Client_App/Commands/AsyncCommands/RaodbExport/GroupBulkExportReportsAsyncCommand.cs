using System;
using System.Collections.Generic;
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
using Client_App.ViewModels.Messages;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using FirebirdSql.Data.FirebirdClient;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

/// <summary>
/// Групповая выгрузка отчётов форм 1.1–1.9 по списку организаций из .xlsx в отдельные файлы .RAODB.
/// </summary>
public partial class GroupBulkExportReportsAsyncCommand : ExportRaodbBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var dialogResult = await Dispatcher.UIThread.InvokeAsync(() =>
        {
            var window = new GroupBulkExportReportsMessageWindow();
            return window.ShowDialog<GroupBulkExportReportsDialogResult?>(Desktop.MainWindow);
        });

        if (dialogResult is null) return;

        List<(string RegNo, string Okpo)> orgPairs;
        try
        {
            orgPairs = ReadOrgPairsFromExcel(dialogResult.ExcelFilePath);
        }
        catch (Exception ex)
        {
            await ShowErrorMessage("Не удалось прочитать файл .xlsx.", ex.Message);
            return;
        }

        if (orgPairs.Count == 0)
        {
            await ShowErrorMessage("Файл .xlsx не содержит пар рег. № и ОКПО.", null);
            return;
        }

        var cts = new CancellationTokenSource();

        #region ProgressBarInitialization

        await Dispatcher.UIThread.InvokeAsync(() => ProgressBar = new AnyTaskProgressBar(cts));
        var progressBar = ProgressBar;
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        progressBarVM.ExportType = "Экспорт_RAODB";
        progressBarVM.ExportName = "Групповая выгрузка отчётов";
        progressBarVM.ValueBar = 5;
        var loadStatus = "Подготовка";
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        #endregion

        var dbReadOnlyPath = CreateTempDataBase();
        await using var dbReadOnly = new DBModel(dbReadOnlyPath);

        try
        {
            var orgLookup2 = await LoadForm10OrganizationsAsync(dbReadOnly, cts.Token);
        }
        catch (Exception ex)
        {

        }

        var organizations = await LoadForm10OrganizationsAsync(dbReadOnly, cts.Token);

        var orgIds = new HashSet<int>();
        foreach (var (regNo, okpo) in orgPairs)
        {
            foreach (var org in organizations)
            {
                if (OrganizationMatchesRegOkpo(org, regNo, okpo))
                {
                    orgIds.Add(org.Id);
                }
            }
        }

        if (orgIds.Count == 0)
        {
            await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
            await ShowErrorMessage("Не найдено ни одной организации по указанным парам рег. № и ОКПО.", null);
            return;
        }

        progressBarVM.ValueBar = 10;
        loadStatus = "Поиск отчётов";
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var formNumbers = dialogResult.FormNumbers.ToArray();
        var periodStart = dialogResult.PeriodStart;
        var periodEnd = dialogResult.PeriodEnd;

        var reportCandidates = await dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .Where(rep => rep.Reports != null && orgIds.Contains(rep.Reports.Id))
            .Where(rep => formNumbers.Contains(rep.FormNum_DB))
            .Select(rep => new { rep.Id, OrgId = rep.Reports.Id, rep.FormNum_DB, rep.StartPeriod_DB, rep.EndPeriod_DB })
            .ToListAsync(cancellationToken: cts.Token);

        var reportIdArray = reportCandidates
            .Where(rep => ReportOverlapsPeriod(rep.StartPeriod_DB, rep.EndPeriod_DB, periodStart, periodEnd))
            .Select(rep => rep.Id)
            .ToArray();

        var reportsIdToOrgId = reportCandidates.ToDictionary(x => x.Id, x => x.OrgId);
        var reportsIdToFormNum = reportCandidates.ToDictionary(x => x.Id, x => x.FormNum_DB);

        if (reportIdArray.Length == 0)
        {
            await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
            await ShowErrorMessage("Не найдено отчётов, соответствующих выбранным формам и периоду.", null);
            return;
        }

        var folderPath = dialogResult.OutputFolderPath;
        var countReport = reportIdArray.Length;
        var countExportedReport = 0;
        var exportedOrgIds = new HashSet<int>();
        double progressBarDoubleValue = progressBarVM.ValueBar;
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 20, CancellationToken = cts.Token };

        await Task.Run(async () =>
        {
            await Parallel.ForEachAsync(reportIdArray, parallelOptions, async (repsId, parallelCts) =>
            {
                try
                {
                    await using var dbReadOnly2 = new DBModel(dbReadOnlyPath);

                    var formNum = reportsIdToFormNum[repsId];
                    var repFull = await LoadReportWithRowsAsync(dbReadOnly2, repsId, formNum, parallelCts);

                    var dt = DateTime.Now;
                    var fileNameTmp = $"Report_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}_{dt.Millisecond}_{dt.Microsecond}";
                    var fullPathTmp = Path.Combine(BaseVM.TmpDirectory, $"{fileNameTmp}.RAODB");
                    var filename =
                        StaticStringMethods.RemoveForbiddenChars(repFull.Reports.Master.RegNoRep.Value) +
                        $"_{StaticStringMethods.RemoveForbiddenChars(repFull.Reports.Master.OkpoRep.Value)}" +
                        $"_{repFull.FormNum_DB}" +
                        $"_{StaticStringMethods.RemoveForbiddenChars(repFull.StartPeriod_DB)}" +
                        $"_{StaticStringMethods.RemoveForbiddenChars(repFull.EndPeriod_DB)}" +
                        $"_{repFull.CorrectionNumber_DB}" +
                        $"_{Assembly.GetExecutingAssembly().GetName().Version}";

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
                    await t!.CloseAsync();
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
                        return;
                    }

                    countExportedReport++;
                    if (reportsIdToOrgId.TryGetValue(repsId, out var orgId))
                    {
                        lock (exportedOrgIds)
                        {
                            exportedOrgIds.Add(orgId);
                        }
                    }

                    progressBarDoubleValue += (double)90 / countReport;
                    progressBarVM.ValueBar = (int)Math.Floor(progressBarDoubleValue);
                    loadStatus = $"выгружено {countExportedReport} из {countReport} отчётов";
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

        progressBarVM.ValueBar = 100;
        loadStatus = "Завершение выгрузки";
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        if (!cts.IsCancellationRequested)
        {
            var exportedOrgCount = exportedOrgIds.Count;
            var orgSuffix = exportedOrgCount.ToString() is [.., '1'] && !exportedOrgCount.ToString().EndsWith("11")
                ? "а"
                : "ий";

            var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Ок", IsDefault = true },
                        new ButtonDefinition { Name = "Открыть расположение файлов" }
                    ],
                    ContentTitle = "Выгрузка",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        $"Групповая выгрузка завершена.{Environment.NewLine}" +
                        $"Выгружено отчётов: {countExportedReport}.{Environment.NewLine}" +
                        $"Выгружено организаций: {exportedOrgCount} организац{orgSuffix}.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                })
                .ShowDialog(Desktop.MainWindow));

            if (answer is "Открыть расположение файлов")
            {
                Process.Start("explorer", folderPath);
            }
        }

        await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
    }

    private static async Task<List<Reports>> LoadForm10OrganizationsAsync(DBModel db, CancellationToken ct)
    {
        return await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Master_DB.FormNum_DB == "1.0")
            .Include(x => x.Master_DB).ThenInclude(m => m.Rows10.OrderBy(r => r.NumberInOrder_DB))
            .ToListAsync(ct);
    }

    /// <summary>
    /// Рег. № и ОКПО отчитывающейся организации формы 1.0 (как RegNoRep / OkpoRep в Report).
    /// ОКПО: сначала Rows10[1], если Okpo_DB пустой или «-», то Rows10[0].
    /// </summary>
    private static (string RegNo, string Okpo) GetForm10ReportingRegNoOkpo(Report master)
    {
        var rows = master.Rows10.OrderBy(r => r.NumberInOrder_DB).ToArray();
        if (rows.Length < 2) return ("", "");

        var row0 = rows[0];
        var row1 = rows[1];

        var okpo = row1.Okpo_DB is "" or "-"
            ? GetRowOkpo(row0)
            : GetRowOkpo(row1);

        var regNo = (GetRowRegNo(row1) != "" || row1.Okpo_DB == "-") && GetRowOkpo(row1) != ""
            ? GetRowRegNo(row1)
            : GetRowRegNo(row0);

        return (regNo, okpo);
    }

    private static string GetRowOkpo(Models.Forms.Form1.Form10 row) =>
        string.IsNullOrWhiteSpace(row.Okpo_DB) ? (row.Okpo?.Value ?? "").Trim() : row.Okpo_DB.Trim();

    private static string GetRowRegNo(Models.Forms.Form1.Form10 row) =>
        string.IsNullOrWhiteSpace(row.RegNo_DB) ? (row.RegNo?.Value ?? "").Trim() : row.RegNo_DB.Trim();

    private static bool OrganizationMatchesRegOkpo(Reports org, string regNo, string okpo)
    {
        if (org.Master_DB is not { FormNum_DB: "1.0" } master || master.Rows10.Count < 2)
        {
            return false;
        }

        var (orgRegNo, orgOkpo) = GetForm10ReportingRegNoOkpo(master);
        if (string.IsNullOrEmpty(orgRegNo) || string.IsNullOrEmpty(orgOkpo))
        {
            return false;
        }

        return string.Equals(orgRegNo, regNo.Trim(), StringComparison.Ordinal)
               && string.Equals(orgOkpo, okpo.Trim(), StringComparison.Ordinal);
    }

    private static List<(string RegNo, string Okpo)> ReadOrgPairsFromExcel(string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var excelPackage = new ExcelPackage(new FileInfo(filePath));
        var worksheet = excelPackage.Workbook.Worksheets[0];
        if (worksheet.Dimension is null) return [];

        var pairs = new List<(string RegNo, string Okpo)>();
        var seen = new HashSet<(string, string)>();

        for (var row = 1; row <= worksheet.Dimension.End.Row; row++)
        {
            var regNo = Convert.ToString(worksheet.Cells[row, 1].Value)?.Trim() ?? string.Empty;
            var okpo = Convert.ToString(worksheet.Cells[row, 2].Value)?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(regNo) && string.IsNullOrEmpty(okpo)) continue;
            if (string.IsNullOrEmpty(regNo) || string.IsNullOrEmpty(okpo)) continue;

            if (!seen.Add((regNo, okpo))) continue;
            pairs.Add((regNo, okpo));
        }

        return pairs;
    }

    private static bool ReportOverlapsPeriod(string? startDb, string? endDb, DateOnly periodStart, DateOnly periodEnd)
    {
        var hasStart = DateOnly.TryParse(startDb, out var repStart);
        var repEnd = DateOnly.MaxValue;
        var hasEnd = !string.IsNullOrWhiteSpace(endDb) && DateOnly.TryParse(endDb, out repEnd);

        if (!hasStart && !hasEnd)
        {
            return false;
        }

        if (!hasStart)
        {
            repStart = DateOnly.MinValue;
        }

        return periodStart <= repEnd && periodEnd >= repStart;
    }

    private static async Task<Report> LoadReportWithRowsAsync(DBModel db, int reportId, string formNum, CancellationToken ct)
    {
        IQueryable<Report> query = db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Include(rep => rep.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows10.OrderBy(r => r.NumberInOrder_DB));

        query = formNum switch
        {
            "1.1" => query.Include(x => x.Rows11.OrderBy(r => r.NumberInOrder_DB)),
            "1.2" => query.Include(x => x.Rows12.OrderBy(r => r.NumberInOrder_DB)),
            "1.3" => query.Include(x => x.Rows13.OrderBy(r => r.NumberInOrder_DB)),
            "1.4" => query.Include(x => x.Rows14.OrderBy(r => r.NumberInOrder_DB)),
            "1.5" => query.Include(x => x.Rows15.OrderBy(r => r.NumberInOrder_DB)),
            "1.6" => query.Include(x => x.Rows16.OrderBy(r => r.NumberInOrder_DB)),
            "1.7" => query.Include(x => x.Rows17.OrderBy(r => r.NumberInOrder_DB)),
            "1.8" => query.Include(x => x.Rows18.OrderBy(r => r.NumberInOrder_DB)),
            "1.9" => query.Include(x => x.Rows19.OrderBy(r => r.NumberInOrder_DB)),
            _ => query
        };

        return await query
            .Include(x => x.Notes.OrderBy(n => n.Order))
            .FirstAsync(x => x.Id == reportId, cancellationToken: ct);
    }

    private static async Task ShowErrorMessage(string message, string? details)
    {
        var content = details is null ? message : $"{message}{Environment.NewLine}{details}";
        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = "Групповая выгрузка",
                ContentHeader = "Уведомление",
                ContentMessage = content,
                MinWidth = 400,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            })
            .ShowDialog(Desktop.MainWindow));
    }

    private static string InsertIndexInFilePath(string fullPath)
    {
        while (File.Exists(fullPath))
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

    [GeneratedRegex(@"(.+)#(\d+)(?=\.RAODB)")]
    private static partial Regex RaodbFileNameRegex();
}
