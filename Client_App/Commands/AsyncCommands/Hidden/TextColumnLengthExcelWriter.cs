using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Commands.AsyncCommands.Hidden;

internal static class TextColumnLengthExcelWriter
{
    private const string SheetSummary = "Summary";
    private const string SheetAll = "AllColumns";
    private const string SheetBlob = "BlobOnly";
    private const string SheetWarnings = "Warnings";

    public static void Write(
        ExcelPackage package,
        IReadOnlyList<Models.DBRealization.SchemaAnalysis.TextColumnDescriptor> columns,
        string databasePath,
        string appVersion)
    {
        WriteSummary(package, columns, databasePath, appVersion);
        WriteDataSheet(package, SheetAll, columns);
        WriteDataSheet(
            package,
            SheetBlob,
            columns.Where(c => c.IsMigrationTarget).ToList());
        WriteWarnings(package, columns);
    }

    private static void WriteSummary(
        ExcelPackage package,
        IReadOnlyList<Models.DBRealization.SchemaAnalysis.TextColumnDescriptor> columns,
        string databasePath,
        string appVersion)
    {
        var sheet = package.Workbook.Worksheets.Add(SheetSummary);
        var blobCount = columns.Count(c => c.StorageKind == Models.DBRealization.SchemaAnalysis.TextColumnStorageKind.BlobText);
        var varcharCount = columns.Count(c => c.StorageKind == Models.DBRealization.SchemaAnalysis.TextColumnStorageKind.Varchar);
        var migrationTargets = columns.Count(c => c.IsMigrationTarget);
        var warnings = columns.Count(c => c.WarningKind != Models.DBRealization.SchemaAnalysis.TextColumnWarningKind.None
            || !string.IsNullOrEmpty(c.MeasureError));

        sheet.Cells[1, 1].Value = "Database";
        sheet.Cells[1, 2].Value = databasePath;
        sheet.Cells[2, 1].Value = "App version";
        sheet.Cells[2, 2].Value = appVersion;
        sheet.Cells[3, 1].Value = "Generated (UTC)";
        sheet.Cells[3, 2].Value = DateTime.UtcNow.ToString("O");
        sheet.Cells[5, 1].Value = "Total columns";
        sheet.Cells[5, 2].Value = columns.Count;
        sheet.Cells[6, 1].Value = "BLOB TEXT";
        sheet.Cells[6, 2].Value = blobCount;
        sheet.Cells[7, 1].Value = "VARCHAR";
        sheet.Cells[7, 2].Value = varcharCount;
        sheet.Cells[8, 1].Value = "Migration targets (blob, not keep)";
        sheet.Cells[8, 2].Value = migrationTargets;
        sheet.Cells[9, 1].Value = "Warnings / errors";
        sheet.Cells[9, 2].Value = warnings;
        sheet.Column(1).AutoFit();
        sheet.Column(2).AutoFit();
    }

    private static void WriteDataSheet(
        ExcelPackage package,
        string sheetName,
        IReadOnlyList<Models.DBRealization.SchemaAnalysis.TextColumnDescriptor> columns)
    {
        var sheet = package.Workbook.Worksheets.Add(sheetName);
        var headers = new[]
        {
            "Table", "Column", "StorageKind", "DeclaredLength", "MaxCharLength", "NonNullCount",
            "ProposedVarchar", "KeepBlob", "SuggestedWave", "EntityType", "PropertyName",
            "WarningKind", "WarningMessage", "MeasureError"
        };

        for (var c = 0; c < headers.Length; c++)
            sheet.Cells[1, c + 1].Value = headers[c];

        for (var r = 0; r < columns.Count; r++)
        {
            var row = columns[r];
            var line = r + 2;
            sheet.Cells[line, 1].Value = row.Table;
            sheet.Cells[line, 2].Value = row.Column;
            sheet.Cells[line, 3].Value = row.StorageKind.ToString();
            sheet.Cells[line, 4].Value = row.DeclaredLength;
            sheet.Cells[line, 5].Value = row.MaxCharLength;
            sheet.Cells[line, 6].Value = row.NonNullCount;
            sheet.Cells[line, 7].Value = row.ProposedVarchar;
            sheet.Cells[line, 8].Value = row.KeepBlob;
            sheet.Cells[line, 9].Value = row.SuggestedWave;
            sheet.Cells[line, 10].Value = row.EntityTypeName;
            sheet.Cells[line, 11].Value = row.PropertyName;
            sheet.Cells[line, 12].Value = row.WarningKind.ToString();
            sheet.Cells[line, 13].Value = row.WarningMessage;
            sheet.Cells[line, 14].Value = row.MeasureError;
        }

        if (columns.Count > 0)
        {
            var range = sheet.Cells[1, 1, columns.Count + 1, headers.Length];
            range.AutoFilter = true;
            sheet.View.FreezePanes(2, 1);
            sheet.Row(1).Style.Font.Bold = true;
        }

        sheet.Cells.AutoFitColumns(0, 60);
    }

    private static void WriteWarnings(
        ExcelPackage package,
        IReadOnlyList<Models.DBRealization.SchemaAnalysis.TextColumnDescriptor> columns)
    {
        var warnings = columns
            .Where(c => c.WarningKind != Models.DBRealization.SchemaAnalysis.TextColumnWarningKind.None
                || !string.IsNullOrEmpty(c.MeasureError))
            .ToList();

        WriteDataSheet(package, SheetWarnings, warnings);
    }
}
