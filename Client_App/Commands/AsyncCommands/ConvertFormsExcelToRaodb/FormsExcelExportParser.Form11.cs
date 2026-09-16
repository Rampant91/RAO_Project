using Models.Forms;
using Models.Forms.Form1;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Парсер/маппер формы 1.1. Общий разбор книги — <see cref="FormsExcelExportParserForm1"/>.
/// </summary>
public static class FormsExcelExportParserForm11
{
    public static bool HeadersMatch(ExcelWorksheet worksheet, System.Collections.Generic.IReadOnlyList<string> expected, out string? error) =>
        FormsExcelHeaders.HeadersMatch(worksheet, expected, out error);

    public static FormsExcelForm1ParseResult ParseWorkbook(ExcelPackage package) =>
        FormsExcelExportParserForm1.ParseWorkbook(package);

    public static FormsExcelForm1ParseResult ParseDetectedSheet(FormsExcelDetectedSheet sheet) =>
        FormsExcelExportParserForm1.ParseDetectedSheet(sheet);

    public static bool TryReadReportKey(
        ExcelWorksheet worksheet,
        int row,
        out FormsExcelReportKey key,
        out string? error) =>
        FormsExcelExportParserForm1.TryReadReportKey(worksheet, row, out key, out error);

    public static Form11 MapForm11Row(ExcelWorksheet worksheet, int row)
    {
        var form = (Form11)FormCreator.Create(FormsExcelExportHeadersForm11.FormNum);
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(worksheet.Cells[row, 8].Value) ?? 0;
        form.OperationCode_DB = FormsExcelCellConverters.CellStringOrDash(worksheet.Cells[row, 9].Value);
        form.OperationDate_DB = FormsExcelCellConverters.NormalizeDate(
            worksheet.Cells[row, 10].Value,
            worksheet.Cells[row, 10].Text);
        form.PassportNumber_DB = FormsExcelCellConverters.CellStringOrDash(worksheet.Cells[row, 11].Value);
        form.Type_DB = FormsExcelCellConverters.CellStringOrDash(worksheet.Cells[row, 12].Value);
        form.Radionuclids_DB = FormsExcelCellConverters.CellStringOrDash(worksheet.Cells[row, 13].Value);
        form.FactoryNumber_DB = FormsExcelCellConverters.CellStringOrDash(worksheet.Cells[row, 14].Value);
        form.Quantity_DB = FormsExcelCellConverters.ParseNullableInt(worksheet.Cells[row, 15].Value);
        form.Activity_DB = FormsExcelCellConverters.ParseActivity(worksheet.Cells[row, 16].Value);
        form.CreatorOKPO_DB = FormsExcelCellConverters.CellStringOrDash(worksheet.Cells[row, 17].Value);
        form.CreationDate_DB = FormsExcelCellConverters.NormalizeDate(
            worksheet.Cells[row, 18].Value,
            worksheet.Cells[row, 18].Text);
        form.Category_DB = FormsExcelCellConverters.ParseNullableShort(worksheet.Cells[row, 19].Value);
        form.SignedServicePeriod_DB = FormsExcelCellConverters.ParseNullableFloat(worksheet.Cells[row, 20].Value);
        form.PropertyCode_DB = FormsExcelCellConverters.ParseNullableByte(worksheet.Cells[row, 21].Value);
        form.Owner_DB = FormsExcelCellConverters.CellStringOrDash(worksheet.Cells[row, 22].Value);
        form.DocumentVid_DB = FormsExcelCellConverters.ParseNullableByte(worksheet.Cells[row, 23].Value);
        form.DocumentNumber_DB = FormsExcelCellConverters.CellStringOrDash(worksheet.Cells[row, 24].Value);
        form.DocumentDate_DB = FormsExcelCellConverters.NormalizeDate(
            worksheet.Cells[row, 25].Value,
            worksheet.Cells[row, 25].Text);
        form.ProviderOrRecieverOKPO_DB = FormsExcelCellConverters.CellStringOrDash(worksheet.Cells[row, 26].Value);
        form.TransporterOKPO_DB = FormsExcelCellConverters.CellStringOrDash(worksheet.Cells[row, 27].Value);
        form.PackName_DB = FormsExcelCellConverters.CellStringOrDash(worksheet.Cells[row, 28].Value);
        form.PackType_DB = FormsExcelCellConverters.CellStringOrDash(worksheet.Cells[row, 29].Value);
        form.PackNumber_DB = FormsExcelCellConverters.CellStringOrDash(worksheet.Cells[row, 30].Value);
        return form;
    }
}
