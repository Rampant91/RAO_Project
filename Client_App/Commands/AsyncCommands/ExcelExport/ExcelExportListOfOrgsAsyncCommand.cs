using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources.CustomComparers;
using Client_App.ViewModels;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Excel -> Список организаций.
/// </summary>
public class ExcelExportListOfOrgsAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Список_организаций";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(2, "Проверка параметров", "Выгрузка в .xlsx", ExportType);
        var folderPath = await CheckAppParameter();
        var isBackgroundCommand = folderPath != string.Empty;

        progressBarVM.SetProgressBar(5, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(10, "Подсчёт количества организаций");
        await ReportsCountCheck(db, progressBar, cts);

        progressBarVM.SetProgressBar(13, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";

        var (fullPath, openTemp) = !isBackgroundCommand
            ? await ExcelGetFullPath(fileName, cts, progressBar)
            : (Path.Combine(folderPath, $"{fileName}.xlsx"), true);

        var count = 0;
        while (File.Exists(fullPath))
        {
            fullPath = Path.Combine(folderPath, fileName + $"_{++count}.xlsx");
        }

        progressBarVM.SetProgressBar(15, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(18, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage, parameter);

        progressBarVM.SetProgressBar(20, "Получение списка организаций");
        var repsList = await GetReportsList(db, cts);

        progressBarVM.SetProgressBar(30, "Заполнение строчек в .xlsx");
        await FillExcel(repsList, parameter, progressBarVM);

        progressBarVM.SetProgressBar(95, "Сохранение");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar, isBackgroundCommand);

        progressBarVM.SetProgressBar(98, "Очистка временных данных");
        try
        {
            File.Delete(tmpDbPath);
        }
        catch
        {
            // ignored
        }

        progressBarVM.SetProgressBar(100, "Завершение выгрузки");
        await progressBar.CloseAsync();
    }

    #region FillExcel

    /// <summary>
    /// Выгружает в .xlsx требуемые значения.
    /// </summary>
    /// <param name="repsList">Список организаций.</param>
    /// <param name="parameter">Параметр команды (full - выгрузка с дополнительными полями)</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    private Task FillExcel(IReadOnlyCollection<Reports> repsList, object? parameter, AnyTaskProgressBarVM progressBarVM)
    {
        var checkedLst = new List<Reports>();
        var row = 2;
        double progressBarDoubleValue = progressBarVM.ValueBar;

        var comparator = new CustomReportsComparer();
        foreach (var reps in repsList
                     .OrderBy(x => x.Master_DB?.RegNoRep?.Value, comparator)
                     .ThenBy(x => x.Master_DB?.OkpoRep?.Value, comparator))
        {
            if (checkedLst.Any(x => x.Master_DB.RegNoRep == reps.Master_DB.RegNoRep
                                    && x.Master_DB.OkpoRep == reps.Master_DB.OkpoRep))
            {
                row--;

                if (parameter?.ToString() != "full")
                {
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
                }
                else
                {
                    #region BindingCells

                    Worksheet.Cells[row, 42].Value =
                        (int)Worksheet.Cells[row, 42].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.1"));
                    Worksheet.Cells[row, 43].Value =
                        (int)Worksheet.Cells[row, 43].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.2"));
                    Worksheet.Cells[row, 44].Value =
                        (int)Worksheet.Cells[row, 44].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.3"));
                    Worksheet.Cells[row, 45].Value =
                        (int)Worksheet.Cells[row, 45].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.4"));
                    Worksheet.Cells[row, 46].Value =
                        (int)Worksheet.Cells[row, 46].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.5"));
                    Worksheet.Cells[row, 47].Value =
                        (int)Worksheet.Cells[row, 47].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.6"));
                    Worksheet.Cells[row, 48].Value =
                        (int)Worksheet.Cells[row, 48].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.7"));
                    Worksheet.Cells[row, 49].Value =
                        (int)Worksheet.Cells[row, 49].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.8"));
                    Worksheet.Cells[row, 50].Value =
                        (int)Worksheet.Cells[row, 50].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.9"));
                    Worksheet.Cells[row, 51].Value =
                        (int)Worksheet.Cells[row, 51].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.1"));
                    Worksheet.Cells[row, 52].Value =
                        (int)Worksheet.Cells[row, 52].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.2"));
                    Worksheet.Cells[row, 53].Value =
                        (int)Worksheet.Cells[row, 53].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.3"));
                    Worksheet.Cells[row, 54].Value =
                        (int)Worksheet.Cells[row, 54].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.4"));
                    Worksheet.Cells[row, 55].Value =
                        (int)Worksheet.Cells[row, 55].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.5"));
                    Worksheet.Cells[row, 56].Value =
                        (int)Worksheet.Cells[row, 56].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.6"));
                    Worksheet.Cells[row, 57].Value =
                        (int)Worksheet.Cells[row, 57].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.7"));
                    Worksheet.Cells[row, 58].Value =
                        (int)Worksheet.Cells[row, 58].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.8"));
                    Worksheet.Cells[row, 59].Value =
                        (int)Worksheet.Cells[row, 59].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.9"));
                    Worksheet.Cells[row, 60].Value =
                        (int)Worksheet.Cells[row, 60].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.10"));
                    Worksheet.Cells[row, 61].Value =
                        (int)Worksheet.Cells[row, 61].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.11"));
                    Worksheet.Cells[row, 62].Value =
                        (int)Worksheet.Cells[row, 62].Value
                        + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.12"));

                    #endregion
                }

                row++;
            }
            else
            {
                #region BindingCells

                Worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                Worksheet.Cells[row, 2].Value = reps.Master.RegNoRep.Value.Length >= 2
                    ? reps.Master.RegNoRep.Value[..2]
                    : reps.Master.RegNoRep.Value;
                Worksheet.Cells[row, 3].Value = !string.IsNullOrEmpty(reps.Master.Rows10[0]?.OrganUprav_DB)
                    ? reps.Master.Rows10[0].OrganUprav_DB
                    : !string.IsNullOrEmpty(reps.Master.Rows10[1]?.OrganUprav_DB)
                        ? reps.Master.Rows10[1].OrganUprav_DB
                        : !string.IsNullOrEmpty(reps.Master.Rows20[0]?.OrganUprav_DB)
                            ? reps.Master.Rows20[0]?.OrganUprav_DB
                            : !string.IsNullOrEmpty(reps.Master.Rows20[1]?.OrganUprav_DB)
                                ? reps.Master.Rows20[1]?.OrganUprav_DB
                                : string.Empty;
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
                if (parameter?.ToString() != "full")
                {
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
                }
                else
                {
                    Worksheet.Cells[row, 8].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].SubjectRF_DB
                        : reps.Master.Rows20[0].SubjectRF_DB;
                    Worksheet.Cells[row, 9].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].JurLico_DB
                        : reps.Master.Rows20[0].JurLico_DB;
                    Worksheet.Cells[row, 10].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].ShortJurLico_DB
                        : reps.Master.Rows20[0].ShortJurLico_DB;
                    Worksheet.Cells[row, 11].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].JurLicoAddress_DB
                        : reps.Master.Rows20[0].JurLicoAddress_DB;
                    Worksheet.Cells[row, 12].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].JurLicoFactAddress_DB
                        : reps.Master.Rows20[0].JurLicoFactAddress_DB;
                    Worksheet.Cells[row, 13].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].GradeFIO_DB
                        : reps.Master.Rows20[0].GradeFIO_DB;
                    Worksheet.Cells[row, 14].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].Telephone_DB
                        : reps.Master.Rows20[0].Telephone_DB;
                    Worksheet.Cells[row, 15].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].Fax_DB
                        : reps.Master.Rows20[0].Fax_DB;
                    Worksheet.Cells[row, 16].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].Email_DB
                        : reps.Master.Rows20[0].Email_DB;
                    Worksheet.Cells[row, 17].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].Okpo_DB
                        : reps.Master.Rows20[0].Okpo_DB;
                    Worksheet.Cells[row, 18].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].Okved_DB
                        : reps.Master.Rows20[0].Okved_DB;
                    Worksheet.Cells[row, 19].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].Okogu_DB
                        : reps.Master.Rows20[0].Okogu_DB;
                    Worksheet.Cells[row, 20].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].Oktmo_DB
                        : reps.Master.Rows20[0].Oktmo_DB;
                    Worksheet.Cells[row, 21].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].Inn_DB
                        : reps.Master.Rows20[0].Inn_DB;
                    Worksheet.Cells[row, 22].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].Kpp_DB
                        : reps.Master.Rows20[0].Kpp_DB;
                    Worksheet.Cells[row, 23].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].Okopf_DB
                        : reps.Master.Rows20[0].Okopf_DB;
                    Worksheet.Cells[row, 24].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[0].Okfs_DB
                        : reps.Master.Rows20[0].Okfs_DB;
                    Worksheet.Cells[row, 25].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].SubjectRF_DB
                        : reps.Master.Rows20[1].SubjectRF_DB;
                    Worksheet.Cells[row, 26].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].JurLico_DB
                        : reps.Master.Rows20[1].JurLico_DB;
                    Worksheet.Cells[row, 27].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].ShortJurLico_DB
                        : reps.Master.Rows20[1].ShortJurLico_DB;
                    Worksheet.Cells[row, 28].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].JurLicoAddress_DB
                        : reps.Master.Rows20[1].JurLicoAddress_DB;
                    Worksheet.Cells[row, 29].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].JurLicoFactAddress_DB
                        : reps.Master.Rows20[1].JurLicoFactAddress_DB;
                    Worksheet.Cells[row, 30].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].GradeFIO_DB
                        : reps.Master.Rows20[1].GradeFIO_DB;
                    Worksheet.Cells[row, 31].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].Telephone_DB
                        : reps.Master.Rows20[1].Telephone_DB;
                    Worksheet.Cells[row, 32].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].Fax_DB
                        : reps.Master.Rows20[1].Fax_DB;
                    Worksheet.Cells[row, 33].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].Email_DB
                        : reps.Master.Rows20[1].Email_DB;
                    Worksheet.Cells[row, 34].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].Okpo_DB
                        : reps.Master.Rows20[1].Okpo_DB;
                    Worksheet.Cells[row, 35].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].Okved_DB
                        : reps.Master.Rows20[1].Okved_DB;
                    Worksheet.Cells[row, 36].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].Okogu_DB
                        : reps.Master.Rows20[1].Okogu_DB;
                    Worksheet.Cells[row, 37].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].Oktmo_DB
                        : reps.Master.Rows20[1].Oktmo_DB;
                    Worksheet.Cells[row, 38].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].Inn_DB
                        : reps.Master.Rows20[1].Inn_DB;
                    Worksheet.Cells[row, 39].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].Kpp_DB
                        : reps.Master.Rows20[1].Kpp_DB;
                    Worksheet.Cells[row, 40].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].Okopf_DB
                        : reps.Master.Rows20[1].Okopf_DB;
                    Worksheet.Cells[row, 41].Value = reps.Master.FormNum_DB == "1.0"
                        ? reps.Master.Rows10[1].Okfs_DB
                        : reps.Master.Rows20[1].Okfs_DB;
                    Worksheet.Cells[row, 42].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("1.1"));
                    Worksheet.Cells[row, 43].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("1.2"));
                    Worksheet.Cells[row, 44].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("1.3"));
                    Worksheet.Cells[row, 45].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("1.4"));
                    Worksheet.Cells[row, 46].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("1.5"));
                    Worksheet.Cells[row, 47].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("1.6"));
                    Worksheet.Cells[row, 48].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("1.7"));
                    Worksheet.Cells[row, 49].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("1.8"));
                    Worksheet.Cells[row, 50].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("1.9"));
                    Worksheet.Cells[row, 51].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("2.1"));
                    Worksheet.Cells[row, 52].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("2.2"));
                    Worksheet.Cells[row, 53].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("2.3"));
                    Worksheet.Cells[row, 54].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("2.4"));
                    Worksheet.Cells[row, 55].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("2.5"));
                    Worksheet.Cells[row, 56].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("2.6"));
                    Worksheet.Cells[row, 57].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("2.7"));
                    Worksheet.Cells[row, 58].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("2.8"));
                    Worksheet.Cells[row, 59].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("2.9"));
                    Worksheet.Cells[row, 60].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("2.10"));
                    Worksheet.Cells[row, 61].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("2.11"));
                    Worksheet.Cells[row, 62].Value = reps.Report_Collection
                        .Count(x => x.FormNum_DB.Equals("2.12"));
                }

                #endregion

                row++;
                checkedLst.Add(reps);
            }
            progressBarDoubleValue += (double)65 / (repsList.Count);
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Выгрузка {reps.Master_DB.RegNoRep.Value}_{reps.Master_DB.OkpoRep.Value}");
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

        return Task.CompletedTask;
    }

    #endregion

    #region FillExcelHeaders

    /// <summary>
    /// Заполнение заголовков в .xlsx.
    /// </summary>
    /// <param name="excelPackage">Excel пакет.</param>
    /// <param name="parameter">Параметр команды (full - выгрузка с дополнительными полями)</param>
    private Task FillExcelHeaders(ExcelPackage excelPackage, object? parameter)
    {
        Worksheet = excelPackage.Workbook.Worksheets.Add("Список всех организаций");

        #region Headers

        if (parameter?.ToString() != "full")
        {
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
        }
        else
        {
            Worksheet.Cells[1, 1].Value = "Рег.№";
            Worksheet.Cells[1, 2].Value = "Регион";
            Worksheet.Cells[1, 3].Value = "Орган управления";
            Worksheet.Cells[1, 4].Value = "ОКПО";
            Worksheet.Cells[1, 5].Value = "Сокращенное наименование";
            Worksheet.Cells[1, 6].Value = "Адрес";
            Worksheet.Cells[1, 7].Value = "ИНН";
            Worksheet.Cells[1, 8].Value = "Субъект Российской Федерации";
            Worksheet.Cells[1, 9].Value = "Наименование юр. лица";
            Worksheet.Cells[1, 10].Value = "Сокращенное наименование";
            Worksheet.Cells[1, 11].Value = "Адрес места нахождения юр. лица";
            Worksheet.Cells[1, 12].Value = "Фактический адрес юр. лица";
            Worksheet.Cells[1, 13].Value = "ФИО, должность руководителя";
            Worksheet.Cells[1, 14].Value = "Телефон организации";
            Worksheet.Cells[1, 15].Value = "Факс организации";
            Worksheet.Cells[1, 16].Value = "Эл. почта организации";
            Worksheet.Cells[1, 17].Value = "ОКПО";
            Worksheet.Cells[1, 18].Value = "ОКВЭД";
            Worksheet.Cells[1, 19].Value = "ОКОГУ";
            Worksheet.Cells[1, 20].Value = "ОКТМО";
            Worksheet.Cells[1, 21].Value = "ИНН";
            Worksheet.Cells[1, 22].Value = "КПП";
            Worksheet.Cells[1, 23].Value = "ОКОПФ";
            Worksheet.Cells[1, 24].Value = "ОКФС";
            Worksheet.Cells[1, 25].Value = "Субъект Российской Федерации";
            Worksheet.Cells[1, 26].Value = "Наименование юр. лица";
            Worksheet.Cells[1, 27].Value = "Сокращенное наименование";
            Worksheet.Cells[1, 28].Value = "Адрес места нахождения юр. лица";
            Worksheet.Cells[1, 29].Value = "Фактический адрес юр. лица";
            Worksheet.Cells[1, 30].Value = "ФИО, должность руководителя";
            Worksheet.Cells[1, 31].Value = "Телефон организации";
            Worksheet.Cells[1, 32].Value = "Факс организации";
            Worksheet.Cells[1, 33].Value = "Эл. почта организации";
            Worksheet.Cells[1, 34].Value = "ОКПО";
            Worksheet.Cells[1, 35].Value = "ОКВЭД";
            Worksheet.Cells[1, 36].Value = "ОКОГУ";
            Worksheet.Cells[1, 37].Value = "ОКТМО";
            Worksheet.Cells[1, 38].Value = "ИНН";
            Worksheet.Cells[1, 39].Value = "КПП";
            Worksheet.Cells[1, 40].Value = "ОКОПФ";
            Worksheet.Cells[1, 41].Value = "ОКФС";
            Worksheet.Cells[1, 42].Value = "Форма 1.1";
            Worksheet.Cells[1, 43].Value = "Форма 1.2";
            Worksheet.Cells[1, 44].Value = "Форма 1.3";
            Worksheet.Cells[1, 45].Value = "Форма 1.4";
            Worksheet.Cells[1, 46].Value = "Форма 1.5";
            Worksheet.Cells[1, 47].Value = "Форма 1.6";
            Worksheet.Cells[1, 48].Value = "Форма 1.7";
            Worksheet.Cells[1, 49].Value = "Форма 1.8";
            Worksheet.Cells[1, 50].Value = "Форма 1.9";
            Worksheet.Cells[1, 51].Value = "Форма 2.1";
            Worksheet.Cells[1, 52].Value = "Форма 2.2";
            Worksheet.Cells[1, 53].Value = "Форма 2.3";
            Worksheet.Cells[1, 54].Value = "Форма 2.4";
            Worksheet.Cells[1, 55].Value = "Форма 2.5";
            Worksheet.Cells[1, 56].Value = "Форма 2.6";
            Worksheet.Cells[1, 57].Value = "Форма 2.7";
            Worksheet.Cells[1, 58].Value = "Форма 2.8";
            Worksheet.Cells[1, 59].Value = "Форма 2.9";
            Worksheet.Cells[1, 60].Value = "Форма 2.10";
            Worksheet.Cells[1, 61].Value = "Форма 2.11";
            Worksheet.Cells[1, 62].Value = "Форма 2.12";
        }

        #endregion

        if (OperatingSystem.IsWindows())    // Под Astra Linux эта команда крашит программу без GDI дров
        {
            Worksheet.Column(3).AutoFit();
            Worksheet.Column(5).AutoFit();
            Worksheet.Column(6).AutoFit();
        }

        return Task.CompletedTask;
    }

    #endregion

    #region GetReportsList

    /// <summary>
    /// Получение списка организаций.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Коллекция организаций.</returns>
    private static async Task<IReadOnlyCollection<Reports>> GetReportsList(DBModel db, CancellationTokenSource cts)
    {
        return await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.DBObservable)
            .Include(reps => reps.Master_DB).ThenInclude(x => x.Rows10)
            .Include(reps => reps.Master_DB).ThenInclude(x => x.Rows20)
            .Include(reps => reps.Report_Collection)
            .Where(reps => reps.DBObservable != null)
            .ToListAsync(cts.Token);
    }

    #endregion

    #region ReportsCountCheck

    /// <summary>
    /// Подсчёт количества организаций. При количестве равном 0, выводится сообщение, операция завершается.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task ReportsCountCheck(DBModel db, AnyTaskProgressBar? progressBar, CancellationTokenSource cts)
    {
        var countReports = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Where(x => x.DBObservable != null)
            .CountAsync(cts.Token);

        if (countReports == 0)
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        "Не удалось совершить выгрузку списка всех отчетов по форме 1 с указанием количества строк," +
                        $"{Environment.NewLine}поскольку в текущей базе отсутствуют формы организаций./",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
    }

    #endregion
}