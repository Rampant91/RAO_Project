using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Views.ProgressBar;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.DBRealization.SchemaAnalysis;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.Hidden;

/// <summary>
/// Excel: max CHAR_LENGTH for all text columns in the open RAODB (sizing for BLOB to VARCHAR).
/// </summary>
public class MaxGraphsLengthAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "\u041c\u0430\u043a\u0441\u0438\u043c\u0430\u043b\u044c\u043d\u0430\u044f_\u0434\u043b\u0438\u043d\u0430_\u043a\u043e\u043b\u043e\u043d\u043e\u043a";
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";

        AnyTaskProgressBar? progressBar = null;
        try
        {
            progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
            var progressBarVm = progressBar.AnyTaskProgressBarVM;
            progressBarVm.SetProgressBar(2, "\u041f\u043e\u0434\u0433\u043e\u0442\u043e\u0432\u043a\u0430", ExportType);

            (string fullPath, bool openTemp) result;
            try
            {
                result = await ExcelGetFullPath(fileName, cts, progressBar);
            }
            catch
            {
                return;
            }

            var fullPath = result.fullPath;
            var openTemp = result.openTemp;
            if (string.IsNullOrEmpty(fullPath))
                return;

            progressBarVm.SetProgressBar(5, "\u0427\u0442\u0435\u043d\u0438\u0435 \u043c\u0435\u0442\u0430\u0434\u0430\u043d\u043d\u044b\u0445 \u0411\u0414");

            await using var db = new DBModel(StaticConfiguration.DBPath);
            var dbColumns = await FirebirdTextColumnCatalog.LoadAsync(db.Database, cts.Token)
                .ConfigureAwait(false);
            var modelColumns = EfModelTextColumnIndex.Load(db.Model);
            var merged = TextColumnDescriptor.Merge(dbColumns, modelColumns);

            progressBarVm.SetProgressBar(10, "\u0418\u0437\u043c\u0435\u0440\u0435\u043d\u0438\u0435 \u0434\u043b\u0438\u043d \u043a\u043e\u043b\u043e\u043d\u043e\u043a");

            var measurer = new TextColumnLengthMeasurer(db.Database);
            var progress = new Progress<(int current, int total, string message)>(p =>
            {
                if (p.total <= 0)
                    return;

                var percent = 10 + (int)(80.0 * p.current / p.total);
                progressBarVm.SetProgressBar(percent, p.message);
            });

            await measurer.MeasureAsync(merged, progress, cts.Token).ConfigureAwait(false);
            VarcharSizeProposal.Apply(merged);

            progressBarVm.SetProgressBar(92, "\u0424\u043e\u0440\u043c\u0438\u0440\u043e\u0432\u0430\u043d\u0438\u0435 Excel");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var excelPackage = new ExcelPackage(new FileInfo(fullPath));
            excelPackage.Workbook.Properties.Author = "RAO_APP";
            excelPackage.Workbook.Properties.Title = "Column length report";
            excelPackage.Workbook.Properties.Created = DateTime.Now;

            TextColumnLengthExcelWriter.Write(
                excelPackage,
                merged,
                StaticConfiguration.DBPath,
                Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "");

            progressBarVm.SetProgressBar(98, "\u0421\u043e\u0445\u0440\u0430\u043d\u0435\u043d\u0438\u0435");
            await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts).ConfigureAwait(false);
            progressBarVm.SetProgressBar(100, "\u0413\u043e\u0442\u043e\u0432\u043e");
        }
        finally
        {
            if (progressBar is not null)
                await progressBar.CloseAsync().ConfigureAwait(false);
        }
    }
}
