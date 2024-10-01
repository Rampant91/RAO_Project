using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Excel -> Список организаций.
/// </summary>
public class ExcelExportListOfOrgsAsyncCommand : ExcelBaseAsyncCommand
{
    private AnyTaskProgressBar progressBar;

    public override async Task AsyncExecute(object? parameter)
    {
        if (ReportsStorage.LocalReports.Reports_Collection.Count == 0) return;
        var cts = new CancellationTokenSource();
        ExportType = "Список_организаций";

        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        await Dispatcher.UIThread.InvokeAsync(() => progressBar = new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        progressBarVM.ExportType = ExportType;
        progressBarVM.ExportName = "Выгрузка списка организаций";
        progressBarVM.ValueBar = 2;
        var loadStatus = "Выгрузка данных";
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;

        if (ReportsStorage.LocalReports.Reports_Collection.Count == 0) 
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        "Не удалось совершить выгрузку списка организаций," +
                        $"{Environment.NewLine}поскольку в текущей базе данных отсутствуют отчеты.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;  
        }
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

        var lst = new List<Reports>();
        var checkedLst = new List<Reports>();
        lst.AddRange(ReportsStorage.LocalReports.Reports_Collection);

        var row = 2;
        foreach (var reps in lst)
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

        progressBarVM.LoadStatus = "Сохранение";
        progressBarVM.ValueBar = 95;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts);

        progressBarVM.LoadStatus = "Завершение выгрузки";
        progressBarVM.ValueBar = 100;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
    }
}