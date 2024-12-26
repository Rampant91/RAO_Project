using Models.DTO;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Client_App.Service
{
    public class ExcelParser
    {
        public static List<RadionuclidDTO> ParseRadionuclides(string filePath)
        {
            var radionuclides = new List<RadionuclidDTO>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var name = worksheet.Cells[row, 1].Text;
                        var codeNumber = worksheet.Cells[row, 4].Text;

                        if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(codeNumber))
                        {
                            var halflife = worksheet.Cells[row, 5].Text;
                            var unit = worksheet.Cells[row, 6].Text;

                            radionuclides.Add(new RadionuclidDTO
                            {
                                Name = name,
                                CodeNumber = codeNumber,
                                HalfLife = halflife,
                                Unit = unit
                            });
                        }
                    }
                }
            }
            return radionuclides;
        }
    }
}