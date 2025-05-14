using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Client_App.ViewModels.Calculator;
using Client_App.Views.Calculator;
using Models.DTO;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.Calculator;

public class OpenCalculatorAsyncCommand : BaseAsyncCommand
{
    private protected static List<CalculatorRadionuclidDTO> R = [];

    public override Task AsyncExecute(object? parameter)
    {
        R_Populate_From_File(parameter);

        Window dialogWindow = parameter switch
        {
            "activity" => new ActivityCalculator { DataContext = new ActivityCalculatorVM(R) },
            "category" => new CategoryCalculator { DataContext = new CategoryCalculatorVM(R) },
            _ => throw new ArgumentOutOfRangeException(nameof(parameter), parameter, null)
        };
        dialogWindow.Show();

        return Task.CompletedTask;
    }

    #region RFromFile

    private static void R_Populate_From_File(object? parameter)
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
            var d = worksheet.Cells[i, 15].Text;
            var mza = worksheet.Cells[i, 17].Text;

            if (double.TryParse(halfLifeString, out var halfLife)
                && !string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(abbreviation)
                && !string.IsNullOrWhiteSpace(unit)
                && (parameter is not "category" 
                    || (double.TryParse(mza, out _) 
                        && (double.TryParse(d, out _) || string.Equals(d, "неограничено", StringComparison.OrdinalIgnoreCase)))))
            {
                R.Add(new CalculatorRadionuclidDTO
                {
                    Name = name,
                    Abbreviation = abbreviation,
                    D = d,
                    Halflife = halfLife,
                    Unit = unit,
                    Mza = mza
                });
            }
            i++;
        }
        R = [..R.OrderBy(x => x.Name)];
    }

    #endregion
}