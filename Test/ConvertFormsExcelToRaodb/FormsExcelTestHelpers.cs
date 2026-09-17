using System;
using OfficeOpenXml;

namespace Test.ConvertFormsExcelToRaodb;

internal static class FormsExcelTestHelpers
{
    internal static void WriteHeaders(ExcelWorksheet ws, string[] headers)
    {
        for (var i = 0; i < headers.Length; i++)
        {
            ws.Cells[1, i + 1].Value = headers[i];
        }
    }

    internal static void WriteMinimalForm1Row(
        ExcelWorksheet ws,
        int row,
        string regNo,
        string okpo,
        byte correction,
        DateTime start,
        DateTime end)
    {
        ws.Cells[row, 1].Value = 1;
        ws.Cells[row, 2].Value = okpo;
        ws.Cells[row, 3].Value = "Org";
        ws.Cells[row, 4].Value = regNo;
        ws.Cells[row, 5].Value = correction;
        ws.Cells[row, 6].Value = start;
        ws.Cells[row, 7].Value = end;
        ws.Cells[row, 8].Value = 1;
        ws.Cells[row, 9].Value = "11";
        ws.Cells[row, 10].Value = start;
        for (var col = 11; col <= 30; col++)
        {
            ws.Cells[row, col].Value = "-";
        }
    }

    internal static void WriteMinimalForm2Row(
        ExcelWorksheet ws,
        int row,
        string regNo,
        string okpo,
        byte correction,
        int year)
    {
        ws.Cells[row, 2].Value = okpo;
        ws.Cells[row, 4].Value = regNo;
        ws.Cells[row, 5].Value = correction;
        ws.Cells[row, 6].Value = year;
        ws.Cells[row, 7].Value = 1;
        ws.Cells[row, 8].Value = "11";
    }
}
