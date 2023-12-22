using OfficeOpenXml;

namespace Models.Interfaces;

public interface IExcel
{
    int ExcelRow(ExcelWorksheet worksheet,int row, int column, bool transpose = true, string sumNumber = "");
    void ExcelGetRow(ExcelWorksheet worksheet, int row);
}