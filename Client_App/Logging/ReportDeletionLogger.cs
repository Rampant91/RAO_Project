using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Client_App.Services;
using Client_App.ViewModels;
using Models.Collections;

namespace Client_App.Logging;

public static class ReportDeletionLogger
{
    public static async Task LogDeletionAsync(Report report)
    {
        try
        {
            var logsDirectory = string.IsNullOrWhiteSpace(BaseVM.LogsDirectory)
                ? "C:/RAO/logs"
                : BaseVM.LogsDirectory;
            if (!Directory.Exists(logsDirectory)) Directory.CreateDirectory(logsDirectory);
            var logFile = Path.Combine(logsDirectory, "Deleted_Reports.log");

            var logEntry = new
            {
                DeletionTime = DateTime.Now,
                RegNum = report.FormNum_DB is "1.0" or "2.0" 
                    ? report.RegNoRep.Value
                    : report.Reports.Master_DB.RegNoRep.Value,
                Okpo = report.FormNum_DB is "1.0" or "2.0"
                    ? report.OkpoRep.Value
                    : report.Reports.Master_DB.OkpoRep.Value,
                FormNum = report.FormNum_DB,
                StartPeriod = report.StartPeriod_DB,
                EndPeriod = report.EndPeriod_DB,
                RowsCount = report.FormNum_DB is "1.0" or "2.0" 
                    ? 2 
                    : await ReportsStorage.GetReportRowsCount(report)
            };

            // Краткая запись об удалении в общий FirebirdLogger
            try
            {
                var shortInfo =
                    $"RegNum={logEntry.RegNum}; Okpo={logEntry.Okpo}; Form={logEntry.FormNum}; " +
                    $"Period={logEntry.StartPeriod}-{logEntry.EndPeriod}; Rows={logEntry.RowsCount}";

                FirebirdLogger.Log("Report deleted", shortInfo);
            }
            catch
            {
                // Ошибки вторичного логгера здесь глушим, чтобы не мешать основному LogDeletionAsync
            }

            var entries = new List<JsonElement>();
            if (File.Exists(logFile))
            {
                var content = await File.ReadAllTextAsync(logFile);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var loaded = false;
                    try
                    {
                        using var doc = JsonDocument.Parse(content);
                        if (doc.RootElement.ValueKind == JsonValueKind.Array)
                        {
                            entries.AddRange(doc.RootElement.EnumerateArray().Select(el => el.Clone()));
                            loaded = true;
                        }
                    }
                    catch { /* fall back to NDJSON */ }

                    if (!loaded)
                    {
                        foreach (var line in File.ReadLines(logFile))
                        {
                            if (string.IsNullOrWhiteSpace(line)) continue;
                            try
                            {
                                using var ldoc = JsonDocument.Parse(line);
                                entries.Add(ldoc.RootElement.Clone());
                            }
                            catch { /* skip bad line */ }
                        }
                    }
                }
            }

            using (var newDoc = JsonDocument.Parse(JsonSerializer.Serialize(logEntry)))
            {
                entries.Add(newDoc.RootElement.Clone());
            }

            var output = JsonSerializer.Serialize(entries);
            await File.WriteAllTextAsync(logFile, output);
        }
        catch
        {
            // ignore logging failures
        }
    }
}