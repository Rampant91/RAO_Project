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
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

//  Excel -> Список форм 2
public class ExcelExportListOfForms2AsyncCommand : ExcelBaseAsyncCommand
{
    private ExcelExportProgressBar progressBar;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Список_форм_2";

        #region ReportsCountCheck

        var findRep = 0;
        foreach (var key in ReportsStorage.LocalReports.Reports_Collection)
        {
            var reps = (Reports)key;
            foreach (var key1 in reps.Report_Collection)
            {
                var rep = (Report)key1;
                if (rep.FormNum_DB.Split('.')[0] == "2")
                {
                    findRep += 1;
                }
            }
        }
        if (findRep == 0)
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        "Не удалось совершить выгрузку списка всех отчетов по форме 2 с указанием количества строк," +
                        $"{Environment.NewLine}поскольку в текущей базе отсутствуют отчеты по форме 2",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        } 

        #endregion

        #region MessageInputYearRange

        var res =
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxInputWindow(new MessageBoxInputParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Отмена", IsCancel = true }
                },
                ContentTitle = "Задать период",
                ContentMessage = "Введите год или период лет через дефис (прим: 2021-2023)." +
                                 $"{Environment.NewLine}Если поле незаполнено или года введены некорректно," +
                                 $"{Environment.NewLine}то выгрузка будет осуществляться без фильтра по годам.",
                MinWidth = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        if (res.Button is null or "Отмена") return;
        var minYear = 0;
        var maxYear = 9999;
        if (res.Message != null)
        {
            if (!res.Message.Contains('-'))
            {
                if (int.TryParse(res.Message, out var parseYear) && parseYear.ToString().Length == 4)
                {
                    minYear = parseYear;
                    maxYear = parseYear;
                }
            }
            else if (res.Message.Length > 4)
            {
                var firstResHalf = res.Message.Split('-')[0].Trim();
                var secondResHalf = res.Message.Split('-')[1].Trim();
                if(int.TryParse(firstResHalf, out var minYearParse) && minYearParse.ToString().Length == 4)
                {
                    minYear = minYearParse;
                }
                if(int.TryParse(secondResHalf, out var maxYearParse) && maxYearParse.ToString().Length == 4)
                {
                    maxYear = maxYearParse;
                }
            }
        }
        
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

        await Dispatcher.UIThread.InvokeAsync(() => progressBar = new ExcelExportProgressBar(cts));
        var progressBarVM = progressBar.ExcelExportProgressBarVM;
        progressBarVM.ExportType = ExportType;
        progressBarVM.ExportName = "Выгрузка списка форм 2";
        progressBarVM.ValueBar = 2;
        var loadStatus = "Создание временной БД";
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var dbReadOnlyPath = Path.Combine(BaseVM.TmpDirectory, BaseVM.DbFileName + ".RAODB");
        try
        {
            if (!StaticConfiguration.IsFileLocked(dbReadOnlyPath))
            {
                File.Delete(dbReadOnlyPath);
                File.Copy(Path.Combine(BaseVM.RaoDirectory, BaseVM.DbFileName + ".RAODB"), dbReadOnlyPath);
            }
        }
        catch
        {
            return;
        }

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        if (ReportsStorage.LocalReports.Reports_Collection.Count == 0) return;

        Worksheet = excelPackage.Workbook.Worksheets.Add("Список всех форм 2");

        #region Headers

        Worksheet.Cells[1, 1].Value = "Рег.№";
        Worksheet.Cells[1, 2].Value = "ОКПО";
        Worksheet.Cells[1, 3].Value = "Форма";
        Worksheet.Cells[1, 4].Value = "Отчетный год";
        Worksheet.Cells[1, 5].Value = "Номер кор.";
        Worksheet.Cells[1, 6].Value = "Количество строк"; 

        #endregion

        var lst = new List<Reports>();
        foreach (var key in ReportsStorage.LocalReports.Reports_Collection)
        {
            var item = (Reports)key;
            if (item.Master_DB.FormNum_DB.Split('.')[0] == "2")
            {
                lst.Add(item);
            }
        }

        var row = 2;
        var repsList = lst
            .OrderBy(x => x.Master_DB.RegNoRep.Value)
            .ToList();

        await using var dbReadOnly = new DBModel(dbReadOnlyPath);

        #region GetDataFormDB

        #region Tuple21

        loadStatus = "Загрузка списка форм 2.1";
        progressBarVM.ValueBar = 10;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var tuple21 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "2.1")
            .Include(x => x.Rows21)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows21.Count))
            .ToList();

        #endregion

        #region Tuple22

        loadStatus = "Загрузка списка форм 2.2";
        progressBarVM.ValueBar = 17;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var tuple22 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "2.2")
            .Include(x => x.Rows22)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows22.Count))
            .ToList();

        #endregion

        #region Tuple23

        loadStatus = "Загрузка списка форм 2.3";
        progressBarVM.ValueBar = 24;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var tuple23 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "2.3")
            .Include(x => x.Rows23)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows23.Count))
            .ToList();

        #endregion

        #region Tuple24

        loadStatus = "Загрузка списка форм 2.4";
        progressBarVM.ValueBar = 31;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var tuple24 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "2.4")
            .Include(x => x.Rows24)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows24.Count))
            .ToList();

        #endregion

        #region Tuple25

        loadStatus = "Загрузка списка форм 2.5";
        progressBarVM.ValueBar = 38;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var tuple25 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "2.5")
            .Include(x => x.Rows25)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows25.Count))
            .ToList();

        #endregion

        #region Tuple26

        loadStatus = "Загрузка списка форм 2.6";
        progressBarVM.ValueBar = 45;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var tuple26 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "2.6")
            .Include(x => x.Rows26)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows26.Count))
            .ToList();

        #endregion

        #region Tuple27

        loadStatus = "Загрузка списка форм 2.7";
        progressBarVM.ValueBar = 52;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var tuple27 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "2.7")
            .Include(x => x.Rows27)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows27.Count))
            .ToList();

        #endregion

        #region Tuple28

        loadStatus = "Загрузка списка форм 2.8";
        progressBarVM.ValueBar = 59;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var tuple28 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "2.8")
            .Include(x => x.Rows28)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows28.Count))
            .ToList();

        #endregion

        #region Tuple29

        loadStatus = "Загрузка списка форм 2.9";
        progressBarVM.ValueBar = 66;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var tuple29 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "2.9")
            .Include(x => x.Rows29)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows29.Count))
            .ToList();

        #endregion

        #region Tuple210

        loadStatus = "Загрузка списка форм 2.10";
        progressBarVM.ValueBar = 73;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var tuple210 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "2.10")
            .Include(x => x.Rows210)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows210.Count))
            .ToList();

        #endregion

        #region Tuple211

        loadStatus = "Загрузка списка форм 2.11";
        progressBarVM.ValueBar = 80;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var tuple211 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "2.11")
            .Include(x => x.Rows211)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows211.Count))
            .ToList();

        #endregion

        #region Tuple212

        loadStatus = "Загрузка списка форм 2.12";
        progressBarVM.ValueBar = 87;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var tuple212 = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "2.12")
            .Include(x => x.Rows212)
            .Select(rep => new Tuple<int, int>(rep.Id, rep.Rows212.Count))
            .ToList();

        #endregion

        #endregion

        foreach (var reps in repsList)
        {
            var repList = reps.Report_Collection
                .Where(x =>
                {
                    if (minYear == 0 && maxYear == 9999) return true;
                    if (x.Year_DB?.Length != 4 || !int.TryParse(x.Year_DB, out var currentRepsYear)) return false;
                    return currentRepsYear >= minYear && currentRepsYear <= maxYear;
                })
                .OrderBy(x => x.FormNum_DB)
                .ThenBy(x => x.Year_DB)
                .ToList();
            foreach (var rep in repList)
            {
                var tupleList = rep.FormNum_DB switch
                {
                    "2.1" => tuple21,
                    "2.2" => tuple22,
                    "2.3" => tuple23,
                    "2.4" => tuple24,
                    "2.5" => tuple25,
                    "2.6" => tuple26,
                    "2.7" => tuple27,
                    "2.8" => tuple28,
                    "2.9" => tuple29,
                    "2.10" => tuple210,
                    "2.11" => tuple211,
                    "2.12" => tuple212,
                };
                var tuple = tupleList.Find(x => x.Item1 == rep.Id) ?? new Tuple<int,int>(rep.Id, 0);
                Worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                Worksheet.Cells[row, 2].Value = reps.Master.OkpoRep.Value;
                Worksheet.Cells[row, 3].Value = rep.FormNum_DB;
                Worksheet.Cells[row, 4].Value = rep.Year_DB;
                Worksheet.Cells[row, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[row, 6].Value = tuple.Item2;
                row++;
            }
        }
        if (OperatingSystem.IsWindows()) 
        {
            Worksheet.Cells.AutoFitColumns();  // Под Astra Linux эта команда крашит программу без GDI дров
        }
        Worksheet.View.FreezePanes(2, 1);

        loadStatus = "Сохранение";
        progressBarVM.ValueBar = 95;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts);

        loadStatus = "Завершение выгрузки";
        progressBarVM.ValueBar = 100;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
    }
}