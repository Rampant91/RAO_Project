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
/// Групповая выгрузка отчётов форм 1.1–1.9 и 2.1–2.12 по списку организаций из .xlsx в отдельные файлы .RAODB.
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

        var dbReadOnlyPath = await CreateTempDataBase(progressBar, cts);
        await using var dbReadOnly = new DBModel(dbReadOnlyPath);

        var organizationsForm10 = await LoadOrganizationsAsync(dbReadOnly, "1.0", cts.Token);
        var organizationsForm20 = await LoadOrganizationsAsync(dbReadOnly, "2.0", cts.Token);

        var orgIds = new HashSet<int>();
        foreach (var (regNo, okpo) in orgPairs)
        {
            foreach (var org in organizationsForm10)
            {
                if (OrganizationMatchesRegOkpo(org, regNo, okpo))
                {
                    orgIds.Add(org.Id);
                }
            }

            foreach (var org in organizationsForm20)
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
        var form1Numbers = formNumbers.Where(f => f.StartsWith("1.")).ToArray();
        var form2Numbers = formNumbers.Where(f => f.StartsWith("2.")).ToArray();
        var periodStart = dialogResult.PeriodStart;
        var periodEnd = dialogResult.PeriodEnd;

        var orgIdsForm10 = organizationsForm10
            .Where(o => orgIds.Contains(o.Id))
            .Select(o => o.Id)
            .ToHashSet();
        var orgIdsForm20 = organizationsForm20
            .Where(o => orgIds.Contains(o.Id))
            .Select(o => o.Id)
            .ToHashSet();

        var reportCandidates = new List<(int Id, int OrgId, string FormNum_DB, string? StartPeriod_DB, string? EndPeriod_DB, string? Year_DB)>();

        if (form1Numbers.Length > 0 && orgIdsForm10.Count > 0)
        {
            var reportsForm1 = await dbReadOnly.ReportCollectionDbSet
                .AsNoTracking()
                .Where(rep => rep.Reports != null && orgIdsForm10.Contains(rep.Reports.Id))
                .Where(rep => form1Numbers.Contains(rep.FormNum_DB))
                .Select(rep => new { rep.Id, OrgId = rep.Reports!.Id, rep.FormNum_DB, rep.StartPeriod_DB, rep.EndPeriod_DB, rep.Year_DB })
                .ToListAsync(cancellationToken: cts.Token);
            reportCandidates.AddRange(reportsForm1.Select(r =>
                (r.Id, r.OrgId, r.FormNum_DB, r.StartPeriod_DB, r.EndPeriod_DB, r.Year_DB)));
        }

        if (form2Numbers.Length > 0 && orgIdsForm20.Count > 0)
        {
            var reportsForm2 = await dbReadOnly.ReportCollectionDbSet
                .AsNoTracking()
                .Where(rep => rep.Reports != null && orgIdsForm20.Contains(rep.Reports.Id))
                .Where(rep => form2Numbers.Contains(rep.FormNum_DB))
                .Select(rep => new { rep.Id, OrgId = rep.Reports!.Id, rep.FormNum_DB, rep.StartPeriod_DB, rep.EndPeriod_DB, rep.Year_DB })
                .ToListAsync(cancellationToken: cts.Token);
            reportCandidates.AddRange(reportsForm2.Select(r =>
                (r.Id, r.OrgId, r.FormNum_DB, r.StartPeriod_DB, r.EndPeriod_DB, r.Year_DB)));
        }

        var reportIdArray = reportCandidates
            .Where(rep => ReportMatchesPeriod(rep.FormNum_DB, rep.StartPeriod_DB, rep.EndPeriod_DB, rep.Year_DB, periodStart, periodEnd))
            .Select(rep => rep.Id)
            .ToArray();

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
        var exportedOrgPairs = new HashSet<(string RegNo, string Okpo)>();
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

                        "2.0" =>
                            StaticStringMethods.RemoveForbiddenChars(repFull.Reports.Master.RegNoRep.Value) +
                            $"_{StaticStringMethods.RemoveForbiddenChars(repFull.Reports.Master.OkpoRep.Value)}" +
                            $"_{repFull.FormNum_DB}" +
                            $"_{StaticStringMethods.RemoveForbiddenChars(repFull.Year_DB)}" +
                            $"_{repFull.CorrectionNumber_DB}" +
                            $"_{Assembly.GetExecutingAssembly().GetName().Version}",

                        _ => $"{repFull.FormNum_DB}_{repFull.Id}"
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
                    var regNo = repFull.Reports.Master.RegNoRep.Value?.Trim() ?? "";
                    var okpo = repFull.Reports.Master.OkpoRep.Value?.Trim() ?? "";
                    if (!string.IsNullOrEmpty(regNo) && !string.IsNullOrEmpty(okpo))
                    {
                        lock (exportedOrgPairs)
                        {
                            exportedOrgPairs.Add((regNo, okpo));
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
            var exportedOrgCount = exportedOrgPairs.Count;
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

    private static async Task<List<Reports>> LoadOrganizationsAsync(DBModel db, string masterFormNum, CancellationToken ct)
    {
        var query = db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Master_DB.FormNum_DB == masterFormNum);

        query = masterFormNum switch
        {
            "1.0" => query.Include(x => x.Master_DB).ThenInclude(m => m.Rows10.OrderBy(r => r.NumberInOrder_DB)),
            "2.0" => query.Include(x => x.Master_DB).ThenInclude(m => m.Rows20.OrderBy(r => r.NumberInOrder_DB)),
            _ => query
        };

        return await query.ToListAsync(ct);
    }

    /// <summary>
    /// Рег. № и ОКПО отчитывающейся организации (логика RegNoRep / OkpoRep).
    /// ОКПО: сначала строка [1], если Okpo_DB пустой или «-», то строка [0].
    /// </summary>
    private static (string RegNo, string Okpo) GetReportingRegNoOkpo(Report master) =>
        master.FormNum_DB switch
        {
            "1.0" => GetForm10ReportingRegNoOkpo(master),
            "2.0" => GetForm20ReportingRegNoOkpo(master),
            _ => ("", "")
        };

    private static (string RegNo, string Okpo) GetForm10ReportingRegNoOkpo(Report master)
    {
        var rows = master.Rows10.OrderBy(r => r.NumberInOrder_DB).ToArray();
        if (rows.Length < 2) return ("", "");

        var row0 = rows[0];
        var row1 = rows[1];
        var okpo = row1.Okpo_DB is "" or "-"
            ? GetForm10RowOkpo(row0)
            : GetForm10RowOkpo(row1);
        var regNo = (GetForm10RowRegNo(row1) != "" || row1.Okpo_DB == "-") && GetForm10RowOkpo(row1) != ""
            ? GetForm10RowRegNo(row1)
            : GetForm10RowRegNo(row0);
        return (regNo, okpo);
    }

    private static (string RegNo, string Okpo) GetForm20ReportingRegNoOkpo(Report master)
    {
        var rows = master.Rows20.OrderBy(r => r.NumberInOrder_DB).ToArray();
        if (rows.Length < 2) return ("", "");

        var row0 = rows[0];
        var row1 = rows[1];
        var okpo = row1.Okpo_DB is "" or "-"
            ? GetForm20RowOkpo(row0)
            : GetForm20RowOkpo(row1);
        var regNo = (GetForm20RowRegNo(row1) != "" || row1.Okpo_DB == "-") && GetForm20RowOkpo(row1) != ""
            ? GetForm20RowRegNo(row1)
            : GetForm20RowRegNo(row0);
        return (regNo, okpo);
    }

    private static string GetForm10RowOkpo(Models.Forms.Form1.Form10 row) =>
        string.IsNullOrWhiteSpace(row.Okpo_DB) ? (row.Okpo?.Value ?? "").Trim() : row.Okpo_DB.Trim();

    private static string GetForm10RowRegNo(Models.Forms.Form1.Form10 row) =>
        string.IsNullOrWhiteSpace(row.RegNo_DB) ? (row.RegNo?.Value ?? "").Trim() : row.RegNo_DB.Trim();

    private static string GetForm20RowOkpo(Models.Forms.Form2.Form20 row) =>
        string.IsNullOrWhiteSpace(row.Okpo_DB) ? (row.Okpo?.Value ?? "").Trim() : row.Okpo_DB.Trim();

    private static string GetForm20RowRegNo(Models.Forms.Form2.Form20 row) =>
        string.IsNullOrWhiteSpace(row.RegNo_DB) ? (row.RegNo?.Value ?? "").Trim() : row.RegNo_DB.Trim();

    private static bool OrganizationMatchesRegOkpo(Reports org, string regNo, string okpo)
    {
        if (org.Master_DB is not { } master
            || (master.FormNum_DB != "1.0" && master.FormNum_DB != "2.0"))
        {
            return false;
        }

        if (master.FormNum_DB == "1.0" && master.Rows10.Count < 2
            || master.FormNum_DB == "2.0" && master.Rows20.Count < 2)
        {
            return false;
        }

        var (orgRegNo, orgOkpo) = GetReportingRegNoOkpo(master);
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

    private static bool ReportMatchesPeriod(
        string formNum, string? startDb, string? endDb, string? yearDb, DateOnly periodStart, DateOnly periodEnd)
    {
        if (formNum.StartsWith("2.", StringComparison.Ordinal))
        {
            return int.TryParse(yearDb, out var year)
                   && periodStart.Year <= year
                   && year <= periodEnd.Year;
        }

        return ReportOverlapsPeriod(startDb, endDb, periodStart, periodEnd);
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
            .AsSplitQuery();

        if (formNum.StartsWith("1.", StringComparison.Ordinal))
        {
            query = query.Include(rep => rep.Reports)
                .ThenInclude(x => x.Master_DB)
                .ThenInclude(x => x.Rows10.OrderBy(r => r.NumberInOrder_DB));
        }
        else if (formNum.StartsWith("2.", StringComparison.Ordinal))
        {
            query = query.Include(rep => rep.Reports)
                .ThenInclude(x => x.Master_DB)
                .ThenInclude(x => x.Rows20.OrderBy(r => r.NumberInOrder_DB));
        }
        else
        {
            query = query.Include(rep => rep.Reports).ThenInclude(x => x.Master_DB);
        }

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
            "2.1" => query.Include(x => x.Rows21.OrderBy(r => r.NumberInOrder_DB)),
            "2.2" => query.Include(x => x.Rows22.OrderBy(r => r.NumberInOrder_DB)),
            "2.3" => query.Include(x => x.Rows23.OrderBy(r => r.NumberInOrder_DB)),
            "2.4" => query.Include(x => x.Rows24.OrderBy(r => r.NumberInOrder_DB)),
            "2.5" => query.Include(x => x.Rows25.OrderBy(r => r.NumberInOrder_DB)),
            "2.6" => query.Include(x => x.Rows26.OrderBy(r => r.NumberInOrder_DB)),
            "2.7" => query.Include(x => x.Rows27.OrderBy(r => r.NumberInOrder_DB)),
            "2.8" => query.Include(x => x.Rows28.OrderBy(r => r.NumberInOrder_DB)),
            "2.9" => query.Include(x => x.Rows29.OrderBy(r => r.NumberInOrder_DB)),
            "2.10" => query.Include(x => x.Rows210.OrderBy(r => r.NumberInOrder_DB)),
            "2.11" => query.Include(x => x.Rows211.OrderBy(r => r.NumberInOrder_DB)),
            "2.12" => query.Include(x => x.Rows212.OrderBy(r => r.NumberInOrder_DB)),
            _ => query
        };

        if (formNum.StartsWith("1.", StringComparison.Ordinal))
        {
            query = query.Include(x => x.Notes.OrderBy(n => n.Order));
        }

        return await query.FirstAsync(x => x.Id == reportId, cancellationToken: ct);
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
