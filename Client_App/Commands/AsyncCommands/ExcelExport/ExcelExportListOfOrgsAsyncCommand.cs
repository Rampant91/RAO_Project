using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.ViewModels;
using Models.Collections;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

//  Excel -> Список организаций
public class ExcelExportListOfOrgsAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (ReportsStorage.LocalReports.Reports_Collection.Count == 0) return;
        var cts = new CancellationTokenSource();
        ExportType = "Список организаций";
        
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{BaseVM.Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        finally
        {
            cts.Dispose();
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;

        if (ReportsStorage.LocalReports.Reports_Collection.Count == 0) return;
        Worksheet = excelPackage.Workbook.Worksheets.Add("Список всех организаций");

        #region Headers

        Worksheet.Cells[1, 1].Value = "Рег.№";
        Worksheet.Cells[1, 2].Value = "Регион";
        Worksheet.Cells[1, 3].Value = "Орган управления";
        Worksheet.Cells[1, 4].Value = "ОКПО";
        Worksheet.Cells[1, 5].Value = "Сокращенное наименование";
        Worksheet.Cells[1, 6].Value = "Адрес";
        Worksheet.Cells[1, 7].Value = "ИНН";
        Worksheet.Cells[1, 8].Value = "Форма 1.1";
        Worksheet.Cells[1, 9].Value = "Форма 1.2";
        Worksheet.Cells[1, 10].Value = "Форма 1.3";
        Worksheet.Cells[1, 11].Value = "Форма 1.4";
        Worksheet.Cells[1, 12].Value = "Форма 1.5";
        Worksheet.Cells[1, 13].Value = "Форма 1.6";
        Worksheet.Cells[1, 14].Value = "Форма 1.7";
        Worksheet.Cells[1, 15].Value = "Форма 1.8";
        Worksheet.Cells[1, 16].Value = "Форма 1.9";
        Worksheet.Cells[1, 17].Value = "Форма 2.1";
        Worksheet.Cells[1, 18].Value = "Форма 2.2";
        Worksheet.Cells[1, 19].Value = "Форма 2.3";
        Worksheet.Cells[1, 20].Value = "Форма 2.4";
        Worksheet.Cells[1, 21].Value = "Форма 2.5";
        Worksheet.Cells[1, 22].Value = "Форма 2.6";
        Worksheet.Cells[1, 23].Value = "Форма 2.7";
        Worksheet.Cells[1, 24].Value = "Форма 2.8";
        Worksheet.Cells[1, 25].Value = "Форма 2.9";
        Worksheet.Cells[1, 26].Value = "Форма 2.10";
        Worksheet.Cells[1, 27].Value = "Форма 2.11";
        Worksheet.Cells[1, 28].Value = "Форма 2.12";

        #endregion

        if (OperatingSystem.IsWindows())    // Под Astra Linux эта команда крашит программу без GDI дров
        {
            Worksheet.Column(3).AutoFit();
            Worksheet.Column(5).AutoFit();
            Worksheet.Column(6).AutoFit();
        }

        var lst = new List<Reports>();
        var checkedLst = new List<Reports>();
        foreach (var key in ReportsStorage.LocalReports.Reports_Collection)
        {
            var item = (Reports)key;
            lst.Add(item);
        }

        var row = 2;
        foreach (var reps in lst)
        {
            if (checkedLst.FirstOrDefault(x => x.Master_DB.RegNoRep == reps.Master_DB.RegNoRep && x.Master_DB.OkpoRep == reps.Master_DB.OkpoRep) != null)
            {
                row--;

                #region BindingCells

                Worksheet.Cells[row, 8].Value =
                    (int)Worksheet.Cells[row, 8].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.1"));
                Worksheet.Cells[row, 9].Value =
                    (int)Worksheet.Cells[row, 9].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.2"));
                Worksheet.Cells[row, 10].Value =
                    (int)Worksheet.Cells[row, 10].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.3"));
                Worksheet.Cells[row, 11].Value =
                    (int)Worksheet.Cells[row, 11].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.4"));
                Worksheet.Cells[row, 12].Value =
                    (int)Worksheet.Cells[row, 12].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.5"));
                Worksheet.Cells[row, 13].Value =
                    (int)Worksheet.Cells[row, 13].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.6"));
                Worksheet.Cells[row, 14].Value =
                    (int)Worksheet.Cells[row, 14].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.7"));
                Worksheet.Cells[row, 15].Value =
                    (int)Worksheet.Cells[row, 15].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.8"));
                Worksheet.Cells[row, 16].Value =
                    (int)Worksheet.Cells[row, 16].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.9"));
                Worksheet.Cells[row, 17].Value =
                    (int)Worksheet.Cells[row, 17].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.1"));
                Worksheet.Cells[row, 18].Value =
                    (int)Worksheet.Cells[row, 18].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.2"));
                Worksheet.Cells[row, 19].Value =
                    (int)Worksheet.Cells[row, 19].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.3"));
                Worksheet.Cells[row, 20].Value =
                    (int)Worksheet.Cells[row, 20].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.4"));
                Worksheet.Cells[row, 21].Value =
                    (int)Worksheet.Cells[row, 21].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.5"));
                Worksheet.Cells[row, 22].Value =
                    (int)Worksheet.Cells[row, 22].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.6"));
                Worksheet.Cells[row, 23].Value =
                    (int)Worksheet.Cells[row, 23].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.7"));
                Worksheet.Cells[row, 24].Value =
                    (int)Worksheet.Cells[row, 24].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.8"));
                Worksheet.Cells[row, 25].Value =
                    (int)Worksheet.Cells[row, 25].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.9"));
                Worksheet.Cells[row, 26].Value =
                    (int)Worksheet.Cells[row, 26].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.10"));
                Worksheet.Cells[row, 27].Value =
                    (int)Worksheet.Cells[row, 27].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.11"));
                Worksheet.Cells[row, 28].Value =
                    (int)Worksheet.Cells[row, 28].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.12"));

                #endregion

                row++;
            }
            else
            {
                #region BindingCells

                Worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                Worksheet.Cells[row, 2].Value = reps.Master.RegNoRep.Value.Length >= 2
                    ? reps.Master.RegNoRep.Value[..2]
                    : reps.Master.RegNoRep.Value;
                Worksheet.Cells[row, 3].Value = !string.IsNullOrEmpty(reps.Master.Rows10[0].OrganUprav_DB)
                    ? reps.Master.Rows10[0].OrganUprav_DB
                    : !string.IsNullOrEmpty(reps.Master.Rows10[1].OrganUprav_DB)
                        ? reps.Master.Rows10[1].OrganUprav_DB
                        : !string.IsNullOrEmpty(reps.Master.Rows20[0].OrganUprav_DB)
                            ? reps.Master.Rows20[0].OrganUprav_DB
                            : !string.IsNullOrEmpty(reps.Master.Rows20[1].OrganUprav_DB)
                                ? reps.Master.Rows20[1].OrganUprav_DB
                                : "";
                Worksheet.Cells[row, 4].Value = reps.Master.OkpoRep.Value;
                Worksheet.Cells[row, 5].Value = reps.Master.ShortJurLicoRep.Value;
                Worksheet.Cells[row, 6].Value =
                    !string.IsNullOrEmpty(reps.Master.Rows10[1].JurLicoFactAddress_DB) &&
                    !reps.Master.Rows10[1].JurLicoFactAddress_DB.Equals("-")
                        ? reps.Master.Rows10[1].JurLicoFactAddress_DB
                        : !string.IsNullOrEmpty(reps.Master.Rows20[1].JurLicoFactAddress_DB) &&
                          !reps.Master.Rows20[1].JurLicoFactAddress_DB.Equals("-")
                            ? reps.Master.Rows20[1].JurLicoFactAddress_DB
                            : !string.IsNullOrEmpty(reps.Master.Rows10[1].JurLicoAddress_DB) &&
                              !reps.Master.Rows10[1].JurLicoAddress_DB.Equals("-")
                                ? reps.Master.Rows10[1].JurLicoAddress_DB
                                : !string.IsNullOrEmpty(reps.Master.Rows20[1].JurLicoAddress_DB) &&
                                  !reps.Master.Rows20[1].JurLicoAddress_DB.Equals("-")
                                    ? reps.Master.Rows20[1].JurLicoAddress_DB
                                    : !string.IsNullOrEmpty(reps.Master.Rows10[0].JurLicoFactAddress_DB) &&
                                      !reps.Master.Rows10[0].JurLicoFactAddress_DB.Equals("-")
                                        ? reps.Master.Rows10[0].JurLicoFactAddress_DB
                                        : !string.IsNullOrEmpty(reps.Master.Rows20[0].JurLicoFactAddress_DB) &&
                                          !reps.Master.Rows20[0].JurLicoFactAddress_DB.Equals("-")
                                            ? reps.Master.Rows20[0].JurLicoFactAddress_DB
                                            : !string.IsNullOrEmpty(reps.Master.Rows10[0].JurLicoAddress_DB) &&
                                              !reps.Master.Rows10[0].JurLicoAddress_DB.Equals("-")
                                                ? reps.Master.Rows10[0].JurLicoAddress_DB
                                                : reps.Master.Rows20[0].JurLicoAddress_DB;
                Worksheet.Cells[row, 7].Value = !string.IsNullOrEmpty(reps.Master.Rows10[0].Inn_DB)
                    ? reps.Master.Rows10[0].Inn_DB
                    : !string.IsNullOrEmpty(reps.Master.Rows10[1].Inn_DB)
                        ? reps.Master.Rows10[1].Inn_DB
                        : !string.IsNullOrEmpty(reps.Master.Rows20[0].Inn_DB)
                            ? reps.Master.Rows20[0].Inn_DB
                            : reps.Master.Rows20[1].Inn_DB;
                Worksheet.Cells[row, 8].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.1"));
                Worksheet.Cells[row, 9].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.2"));
                Worksheet.Cells[row, 10].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.3"));
                Worksheet.Cells[row, 11].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.4"));
                Worksheet.Cells[row, 12].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.5"));
                Worksheet.Cells[row, 13].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.6"));
                Worksheet.Cells[row, 14].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.7"));
                Worksheet.Cells[row, 15].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.8"));
                Worksheet.Cells[row, 16].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.9"));
                Worksheet.Cells[row, 17].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.1"));
                Worksheet.Cells[row, 18].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.2"));
                Worksheet.Cells[row, 19].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.3"));
                Worksheet.Cells[row, 20].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.4"));
                Worksheet.Cells[row, 21].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.5"));
                Worksheet.Cells[row, 22].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.6"));
                Worksheet.Cells[row, 23].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.7"));
                Worksheet.Cells[row, 24].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.8"));
                Worksheet.Cells[row, 25].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.9"));
                Worksheet.Cells[row, 26].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.10"));
                Worksheet.Cells[row, 27].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.11"));
                Worksheet.Cells[row, 28].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.12"));

                #endregion

                row++;
                checkedLst.Add(reps);
            }
        }

        for (var col = 1; col <= Worksheet.Dimension.End.Column; col++)
        {
            if (Worksheet.Cells[1, col].Value is "Сокращенное наименование" or "Адрес" or "Орган управления") continue;
            if (OperatingSystem.IsWindows()) // Под Astra Linux эта команда крашит программу без GDI дров
            {
                Worksheet.Column(col).AutoFit();
            }
        }
        Worksheet.View.FreezePanes(2, 1);

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }
}