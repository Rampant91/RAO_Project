using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Services.DataAccess;
using Client_App.ViewModels.ProgressBar;
using Models.CheckForm;
using Models.Collections;
using Models.DBRealization;

namespace Client_App.Commands.AsyncCommands.CheckForm;

/// <summary>
/// Единая точка загрузки read-only снимка и запуска Check_Total (формы 1.1–1.8 и др.).
/// </summary>
public static class ReportCheckRunner
{
    public sealed class Options
    {
        public int ReportId { get; init; }
        public string FormNum { get; init; } = "";

        /// <summary>Уже загруженный снимок (выгрузка). Без повторного запроса к БД.</summary>
        public Report? PreloadedReport { get; init; }

        public ReportCheckProgress? Progress { get; init; }
        public CancellationToken CancellationToken { get; init; }
        public string? DbPath { get; init; }
    }

    public sealed class CheckRunResult
    {
        public List<CheckError> Errors { get; init; } = [];
        public Report? Report { get; init; }
    }

    public static async Task<CheckRunResult> RunAsync(Options options)
    {
        options.CancellationToken.ThrowIfCancellationRequested();

        Report? rep;
        if (options.PreloadedReport is not null)
        {
            rep = options.PreloadedReport;
            options.Progress?.SetOrgHeader(
                GetRegNo(rep),
                GetOkpo(rep),
                rep.FormNum_DB,
                GetPeriodHint(rep));
            options.Progress?.OnLoadComplete(ReportCheckSnapshotLoader.CountLoadedRows(rep));
        }
        else
        {
            options.Progress?.OnLoadStarted();
            options.Progress?.SetOrgHeader(
                string.Empty,
                string.Empty,
                options.FormNum);

            await using var db = CreateDb(options.DbPath);
            rep = await ReportCheckSnapshotLoader.LoadAsync(
                db,
                options.ReportId,
                options.FormNum,
                options.CancellationToken);

            if (rep is null)
            {
                return new CheckRunResult();
            }

            options.Progress?.SetOrgHeader(
                GetRegNo(rep),
                GetOkpo(rep),
                rep.FormNum_DB,
                GetPeriodHint(rep));
            options.Progress?.OnLoadComplete(ReportCheckSnapshotLoader.CountLoadedRows(rep));
        }

        if (rep.Reports?.Master_DB == null)
        {
            throw new InvalidOperationException(
                $"Для проверки отчёта {rep.FormNum_DB} (Id={rep.Id}) не загружен титул организации (Master_DB).");
        }

        options.CancellationToken.ThrowIfCancellationRequested();

        var errors = await Task.Run(
            () => ExecuteCheck(rep.Reports, rep, options.Progress),
            options.CancellationToken);
        return new CheckRunResult { Errors = errors, Report = rep };
    }

    /// <summary>Проверка по уже загруженному снимку (орг. проверка всех форм, temp DB).</summary>
    public static List<CheckError> ExecuteCheck(Reports reps, Report rep, ReportCheckProgress? progress = null)
    {
        using var ctx = new CheckRunContext(progress);
        using (ctx.EnterScope())
        {
            return rep.FormNum_DB switch
            {
                "1.1" => CheckF11.Check_Total(reps, rep),
                "1.2" => CheckF12.Check_Total(reps, rep),
                "1.3" => CheckF13.Check_Total(reps, rep),
                "1.4" => CheckF14.Check_Total(reps, rep),
                "1.5" => CheckF15.Check_Total(reps, rep),
                "1.6" => CheckF16.Check_Total(reps, rep),
                "1.7" => CheckF17.Check_Total(reps, rep),
                "1.8" => CheckF18.Check_Total(reps, rep),
                _ => []
            };
        }
    }

    public static async Task<List<CheckError>> RunForm2OrLegacyAsync(
        Report rep,
        DBModel db,
        CancellationToken cancellationToken)
    {
        return rep.FormNum_DB switch
        {
            "2.1" => await new CheckF21().AsyncExecute(rep),
            "2.2" => await new CheckF22().AsyncExecute(rep),
            "2.3" => await CheckF23.Check_Total(rep),
            "2.6" => await CheckF26.Check_Total(rep),
            "2.7" => await CheckF27.Check_Total(rep),
            "2.8" => await CheckF28.Check_Total(rep),
            "2.9" => await CheckF29.Check_Total(rep),
            "2.10" => await CheckF210.Check_Total(rep),
            "2.11" => await CheckF211.Check_Total(rep),
            "4.1" => await CheckF41.Check_Total(rep),
            _ => throw new NotImplementedException()
        };
    }

    private static DBModel CreateDb(string? dbPath) =>
        string.IsNullOrEmpty(dbPath) ? new DBModel(StaticConfiguration.DBPath) : new DBModel(dbPath);

    private static string GetRegNo(Report rep)
    {
        try
        {
            return rep.Reports?.Master_DB?.RegNoRep?.Value ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    private static string GetOkpo(Report rep)
    {
        try
        {
            return rep.Reports?.Master_DB?.OkpoRep?.Value ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    private static string GetPeriodHint(Report rep) =>
        rep.FormNum_DB.StartsWith('2') || rep.FormNum_DB == "4.1"
            ? rep.Year_DB ?? string.Empty
            : $"{rep.StartPeriod_DB}-{rep.EndPeriod_DB}";
}
