using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Resources;
using Client_App.ViewModels;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Сборка организации+отчёта и запись отдельного файла .RAODB.
/// </summary>
public static partial class FormsExcelRaodbWriter
{
    public static Reports BuildOrganizationWithReport(
        Report clonedMaster,
        FormsExcelForm1ReportGroup group)
    {
        var report = new Report
        {
            FormNum_DB = group.FormNum,
            StartPeriod_DB = group.Key.StartPeriod,
            EndPeriod_DB = group.Key.EndPeriod,
            CorrectionNumber_DB = group.Key.CorrectionNumber
        };

        AttachForm1Rows(report, group);
        return FinalizeOrganization(clonedMaster, report, group.Notes);
    }

    public static Reports BuildOrganizationWithReport(
        Report clonedMaster,
        FormsExcelForm2ReportGroup group)
    {
        var report = new Report
        {
            FormNum_DB = group.FormNum,
            Year_DB = group.Key.Year,
            CorrectionNumber_DB = group.Key.CorrectionNumber
        };

        AttachForm2Rows(report, group);
        return FinalizeOrganization(clonedMaster, report, group.Notes);
    }

    private static Reports FinalizeOrganization(
        Report clonedMaster,
        Report report,
        System.Collections.Generic.List<Note> notes)
    {
        foreach (var note in notes.OrderBy(n => n.Order))
        {
            report.Notes.Add(note);
        }

        var now = DateTime.Now;
        report.ExportDate_DB = $"{now.Day:00}.{now.Month:00}.{now.Year}";
        report.ReportChangedDate = now;

        var org = new Reports
        {
            Master = clonedMaster,
            Report_Collection = new ObservableCollectionWithItemPropertyChanged<Report>([report])
        };
        report.Reports = org;
        return org;
    }

    private static void AttachForm1Rows(Report report, FormsExcelForm1ReportGroup group)
    {
        var ordered = group.Rows.OrderBy(r => r.NumberInOrder_DB).ToList();
        switch (group.FormNum)
        {
            case "1.1":
                foreach (var row in ordered.Cast<Form11>()) report.Rows11.Add(row);
                break;
            case "1.2":
                foreach (var row in ordered.Cast<Form12>()) report.Rows12.Add(row);
                break;
            case "1.3":
                foreach (var row in ordered.Cast<Form13>()) report.Rows13.Add(row);
                break;
            case "1.4":
                foreach (var row in ordered.Cast<Form14>()) report.Rows14.Add(row);
                break;
            case "1.5":
                foreach (var row in ordered.Cast<Form15>()) report.Rows15.Add(row);
                break;
            case "1.6":
                foreach (var row in ordered.Cast<Form16>()) report.Rows16.Add(row);
                break;
            case "1.7":
                foreach (var row in ordered.Cast<Form17>()) report.Rows17.Add(row);
                break;
            case "1.8":
                foreach (var row in ordered.Cast<Form18>()) report.Rows18.Add(row);
                break;
            case "1.9":
                foreach (var row in ordered.Cast<Form19>()) report.Rows19.Add(row);
                break;
            default:
                throw new InvalidOperationException($"Запись Rows для формы {group.FormNum} не реализована.");
        }
    }

    private static void AttachForm2Rows(Report report, FormsExcelForm2ReportGroup group)
    {
        var ordered = group.Rows.OrderBy(r => r.NumberInOrder_DB).ToList();
        switch (group.FormNum)
        {
            case "2.1":
                foreach (var row in ordered.Cast<Form21>()) report.Rows21.Add(row);
                break;
            case "2.2":
                foreach (var row in ordered.Cast<Form22>()) report.Rows22.Add(row);
                break;
            case "2.3":
                foreach (var row in ordered.Cast<Form23>()) report.Rows23.Add(row);
                break;
            case "2.4":
                foreach (var row in ordered.Cast<Form24>()) report.Rows24.Add(row);
                break;
            case "2.5":
                foreach (var row in ordered.Cast<Form25>()) report.Rows25.Add(row);
                break;
            case "2.6":
                foreach (var row in ordered.Cast<Form26>()) report.Rows26.Add(row);
                break;
            case "2.7":
                foreach (var row in ordered.Cast<Form27>()) report.Rows27.Add(row);
                break;
            case "2.8":
                foreach (var row in ordered.Cast<Form28>()) report.Rows28.Add(row);
                break;
            case "2.9":
                foreach (var row in ordered.Cast<Form29>()) report.Rows29.Add(row);
                break;
            case "2.10":
                foreach (var row in ordered.Cast<Form210>()) report.Rows210.Add(row);
                break;
            case "2.11":
                foreach (var row in ordered.Cast<Form211>()) report.Rows211.Add(row);
                break;
            case "2.12":
                foreach (var row in ordered.Cast<Form212>()) report.Rows212.Add(row);
                break;
            default:
                throw new InvalidOperationException($"Запись Rows для формы {group.FormNum} не реализована.");
        }
    }

    public static string BuildFileName(Reports org, Report report)
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        var reg = StaticStringMethods.RemoveForbiddenChars(org.Master.RegNoRep.Value);
        var okpo = StaticStringMethods.RemoveForbiddenChars(org.Master.OkpoRep.Value);
        if (report.FormNum_DB.StartsWith("2.", StringComparison.Ordinal))
        {
            var year = report.Year_DB?.ToString() ?? "";
            return $"{reg}_{okpo}_{report.FormNum_DB}_{year}_{report.CorrectionNumber_DB}_{version}";
        }

        return $"{reg}_{okpo}_{report.FormNum_DB}_" +
               $"{StaticStringMethods.RemoveForbiddenChars(report.StartPeriod_DB)}_" +
               $"{StaticStringMethods.RemoveForbiddenChars(report.EndPeriod_DB)}_" +
               $"{report.CorrectionNumber_DB}_{version}";
    }

    public static string InsertIndexInFilePath(string fullPath)
    {
        while (File.Exists(fullPath))
        {
            var matches = RaodbFileNameRegex().Matches(fullPath);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
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

    public static async Task WriteRaodbAsync(Reports organization, string destinationPath, CancellationToken ct)
    {
        var tmpDbPath = Path.Combine(BaseVM.TmpDirectory, $"{Guid.NewGuid():N}.RAODB");
        try
        {
            await using var tempDb = new DBModel(tmpDbPath);
            await tempDb.MigrateDatabaseAsync(ct);
            await tempDb.ReportsCollectionDbSet.AddAsync(organization, ct);
            if (!tempDb.DBObservableDbSet.Any())
            {
                tempDb.DBObservableDbSet.Add(new DBObservable());
                tempDb.DBObservableDbSet.Local.First().Reports_Collection
                    .AddRange(tempDb.ReportsCollectionDbSet.Local);
            }

            await tempDb.SaveChangesAsync(ct);

            if (tempDb.Database.GetDbConnection() is FbConnection conn)
            {
                await conn.CloseAsync();
                await conn.DisposeAsync();
            }

            await tempDb.Database.CloseConnectionAsync();

            var directory = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.Copy(tmpDbPath, destinationPath, overwrite: false);
        }
        finally
        {
            try
            {
                if (File.Exists(tmpDbPath)) File.Delete(tmpDbPath);
            }
            catch
            {
                // temp cleanup best-effort
            }
        }
    }

    [GeneratedRegex(@"(.+)#(\d+)(?=\.RAODB)")]
    private static partial Regex RaodbFileNameRegex();
}
