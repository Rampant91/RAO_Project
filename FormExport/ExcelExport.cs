using System;
using System.IO;
using Collections;
using OfficeOpenXml;

namespace FormExport
{
    public class Export:IExport
    {
        public void DoExport(Report report, string Path)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //Set some properties of the Excel document
                excelPackage.Workbook.Properties.Author = "APP";
                excelPackage.Workbook.Properties.Title = "Report";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                //Create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Отчет");

                //Add some text to cell A1
                worksheet.Cells[1, 1].Value = "Табельный номер";
                worksheet.Cells[1, 2].Value = "ФИО";



                FileInfo fi = new FileInfo(Path + @"\Report.xlsx");
                excelPackage.SaveAs(fi);
            }
        }
    }
}
