using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Client_App.ViewModels.Calculator;
using Client_App.Views.Calculator;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.Calculator;

public class OpenCalculatorAsyncCommand : BaseAsyncCommand
{
    private protected static List<Radionuclid> R = [];

    public override Task AsyncExecute(object? parameter)
    {
        if (R.Count == 0) R_Populate_From_File();
        
        var dialogWindow = new ActivityCalculator
        {
            DataContext = new ActivityCalculatorVM(R)
        };
        dialogWindow.Show();

        return Task.CompletedTask;
    }

    #region RFromFile

    private static void R_Populate_From_File()
    {

#if DEBUG
        var filePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "R.xlsx");
#else
        var filePath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"R.xlsx");
#endif

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        if (!File.Exists(filePath)) return;
        FileInfo excelImportFile = new(filePath);
        var xls = new ExcelPackage(excelImportFile);
        var worksheet = xls.Workbook.Worksheets["Лист1"];
        var i = 2;
        R.Clear();
        while (worksheet.Cells[i, 1].Text != string.Empty)
        {
            var name = worksheet.Cells[i, 1].Text;
            var abbreviation = worksheet.Cells[i, 4].Text;
            var halfLifeString = worksheet.Cells[i, 5].Text;
            var unit = worksheet.Cells[i, 6].Text;
            var code = worksheet.Cells[i, 8].Text;

            if (double.TryParse(halfLifeString, out var halfLife)
                && !string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(abbreviation)
                && !string.IsNullOrWhiteSpace(unit)
                && !string.IsNullOrWhiteSpace(code))
            {
                R.Add(new Radionuclid
                {
                    Name = name,
                    Abbreviation = abbreviation,
                    Halflife = halfLife,
                    Unit = unit,
                    Code = code
                });
            }
            i++;
        }
        R = [..R.OrderBy(x => x.Name)];
    }

    #endregion
}