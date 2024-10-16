using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client_App.Views.ProgressBar;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Interfaces;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Выбранная форма -> Выгрузка Excel -> Для анализа.
/// </summary>
public class ExcelExportFormAnalysisAsyncCommand : ExcelBaseAsyncCommand
{
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not ObservableCollectionWithItemPropertyChanged<IKey> forms) return;
        var repParam = (Report)forms.First();
        var repId = repParam.Id;

        var cts = new CancellationTokenSource();
        ExportType = "Для_анализа";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(5, "Создание временной БД", "Выгрузка отчёта для анализа", ExportType);
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);

        progressBarVM.SetProgressBar(15, "Загрузка отчёта");
        var rep = await GetReportWithRows(repId, tmpDbPath, cts);

        progressBarVM.SetProgressBar(30, "Определение имени файла");
        var fileName = await GetFileName(rep, progressBar, cts);

        progressBarVM.SetProgressBar(35, "Запрос пути сохранения");
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(40, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(50, "Создание страниц и заполнение заголовков");
        var startColumn =  await CreateWorksheetsAndFillHeaders(excelPackage, rep.FormNum_DB);

        progressBarVM.SetProgressBar(60, "Выгрузка строчек форм");
        await ExcelExportRows(rep, startColumn);

        progressBarVM.SetProgressBar(80, "Выгрузка строчек примечаний");
        await ExcelExportNotes(rep, startColumn);

        progressBarVM.SetProgressBar(90, "Сохранение");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);

        progressBarVM.SetProgressBar(95, "Очистка временных данных");
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

    #region CreateWorksheetsAndFillHeaders

    /// <summary>
    /// Создаёт страницы и заполняет заголовки в excel пакете.
    /// </summary>
    /// <param name="excelPackage">Пакет Excel.</param>
    /// <param name="formNum">Номер формы отчётности.</param>
    /// <returns>Успешно выполненная Task.</returns>
    private Task<int> CreateWorksheetsAndFillHeaders(ExcelPackage excelPackage, string formNum)
    {
        Worksheet = excelPackage.Workbook.Worksheets.Add($"Отчеты {formNum}");
        WorksheetPrim = excelPackage.Workbook.Worksheets.Add($"Примечания {formNum}");
        int masterHeaderLength;
        if (formNum.Split('.')[0] == "1")
        {
            Form10.ExcelHeader(Worksheet, 1, 1);
            masterHeaderLength = Form10.ExcelHeader(WorksheetPrim, 1, 1);
        }
        else
        {
            Form20.ExcelHeader(Worksheet, 1, 1);
            masterHeaderLength = Form20.ExcelHeader(WorksheetPrim, 1, 1);
        }

        var tmpLength = Report.ExcelHeader(Worksheet, formNum, 1, masterHeaderLength + 1);
        Report.ExcelHeader(WorksheetPrim, formNum, 1, masterHeaderLength + 1);
        masterHeaderLength += tmpLength;

        #region ExcelHeaders

        switch (formNum)
        {
            case "1.1":
                Form11.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.2":
                Form12.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.3":
                Form13.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.4":
                Form14.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.5":
                Form15.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.6":
                Form16.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.7":
                Form17.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.8":
                Form18.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.9":
                Form19.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.1":
                Form21.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.2":
                Form22.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.3":
                Form23.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.4":
                Form24.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.5":
                Form25.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.6":
                Form26.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.7":
                Form27.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.8":
                Form28.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.9":
                Form29.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.10":
                Form210.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.11":
                Form211.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.12":
                Form212.ExcelHeader(Worksheet, 1, masterHeaderLength + 1);
                break;
        }
        Note.ExcelHeader(WorksheetPrim, 1, masterHeaderLength + 1);

        #endregion

        if (OperatingSystem.IsWindows())
        {
            Worksheet.Cells.AutoFitColumns();
            WorksheetPrim.Cells.AutoFitColumns();
        }
        Worksheet.View.FreezePanes(2, 1);
        WorksheetPrim.View.FreezePanes(2, 1);

        return Task.FromResult(masterHeaderLength);
    }

    #endregion

    #region ExcelExportNotes

    /// <summary>
    /// Выгрузка примечаний в .xlsx.
    /// </summary>
    /// <param name="rep">Отчёт.</param>
    /// <param name="startRow">Номер начального ряда.</param>
    /// <param name="startColumn">Номер начальной колонки.</param>
    /// <returns>Успешно выполненная Task.</returns>
    private Task ExcelExportNotes(Report rep, int startColumn)
    {
        var curRow = 2;
        foreach (var i in rep.Notes)
        {
            var mstRep = rep.Reports.Master_DB;
            i.ExcelRow(WorksheetPrim, curRow, startColumn + 1);
            var yu = rep.FormNum_DB.Split('.')[0] == "1"
                ? mstRep.Rows10[1].RegNo_DB != "" && mstRep.Rows10[1].Okpo_DB != ""
                    ? rep.Reports.Master_DB.Rows10[1].ExcelRow(WorksheetPrim, curRow, 1) + 1
                    : rep.Reports.Master_DB.Rows10[0].ExcelRow(WorksheetPrim, curRow, 1) + 1
                : mstRep.Rows20[1].RegNo_DB != "" && mstRep.Rows20[1].Okpo_DB != ""
                    ? rep.Reports.Master_DB.Rows20[1].ExcelRow(WorksheetPrim, curRow, 1) + 1
                    : rep.Reports.Master_DB.Rows20[0].ExcelRow(WorksheetPrim, curRow, 1) + 1;

            rep.ExcelRow(WorksheetPrim, curRow, yu);
            curRow++;
        }
        return Task.CompletedTask;
    }

    #endregion

    #region ExcelExportRows

    /// <summary>
    /// Выгружает строчки отчёта в .xlsx.
    /// </summary>
    /// <param name="rep">Отчёт.</param>
    /// <param name="startColumn">Номер начальной графы.</param>
    /// <returns>Успешно выполненная Task.</returns>
    private Task ExcelExportRows(Report rep, int startColumn)
    {
        var formNum = rep.FormNum_DB;
        var startRow = 2;

        IEnumerable<IKey> t;
        switch (rep.FormNum_DB)
        {
            case "2.1":
                t = rep[rep.FormNum_DB].ToList<IKey>().Where(x => ((Form21)x).Sum_DB || ((Form21)x).SumGroup_DB);
                if (rep[rep.FormNum_DB].ToList<IKey>().Any() && !t.Any())
                {
                    t = rep[rep.FormNum_DB].ToList<IKey>();
                }
                break;
            case "2.2":
                t = rep[rep.FormNum_DB].ToList<IKey>().Where(x => ((Form22)x).Sum_DB || ((Form22)x).SumGroup_DB);
                if (rep[rep.FormNum_DB].ToList<IKey>().Any() && !t.Any())
                {
                    t = rep[rep.FormNum_DB].ToList<IKey>();
                }
                break;
            default:
                t = rep[rep.FormNum_DB].ToList<IKey>();
                break;
        }

        var lst = t.Any()
            ? rep[rep.FormNum_DB].ToList<IKey>().ToList()
            : rep[rep.FormNum_DB]
                .ToList<IKey>()
                .OrderBy(x => ((Form)x).NumberInOrder_DB)
                .ToList();
        var count = startRow;
        startRow--;
        lst = lst
            .Where(it => it != null)
            .OrderBy(x => x.Order)
            .ToList();
        foreach (var it in lst)
        {
            switch (it)
            {
                case Form11 form11:
                    form11.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form12 form12:
                    form12.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form13 form13:
                    form13.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form14 form14:
                    form14.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form15 form15:
                    form15.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form16 form16:
                    form16.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form17 form17:
                    form17.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form18 form18:
                    form18.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form19 form19:
                    form19.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form21 form21:
                    form21.ExcelRow(Worksheet, count, startColumn + 1, sumNumber: form21.NumberInOrderSum_DB);
                    break;
                case Form22 form22:
                    form22.ExcelRow(Worksheet, count, startColumn + 1, sumNumber: form22.NumberInOrderSum_DB);
                    break;
                case Form23 form23:
                    form23.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form24 form24:
                    form24.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form25 form25:
                    form25.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form26 form26:
                    form26.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form27 form27:
                    form27.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form28 form28:
                    form28.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form29 form29:
                    form29.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form210 form210:
                    form210.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form211 form211:
                    form211.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
                case Form212 form212:
                    form212.ExcelRow(Worksheet, count, startColumn + 1);
                    break;
            }

            var masterRep = rep.Reports.Master_DB;

            var yu = rep.FormNum_DB.Split('.')[0] == "1"
                ? masterRep.Rows10[1].RegNo_DB != "" && masterRep.Rows10[1].Okpo_DB != ""
                    ? rep.Reports.Master_DB.Rows10[1].ExcelRow(Worksheet, count, 1) + 1
                    : rep.Reports.Master_DB.Rows10[0].ExcelRow(Worksheet, count, 1) + 1
                : masterRep.Rows20[1].RegNo_DB != "" && masterRep.Rows20[1].Okpo_DB != ""
                    ? rep.Reports.Master_DB.Rows20[1].ExcelRow(Worksheet, count, 1) + 1
                    : rep.Reports.Master_DB.Rows20[0].ExcelRow(Worksheet, count, 1) + 1;

            rep.ExcelRow(Worksheet, count, yu);
            count++;
        }

        if (formNum is "2.2")
        {
            for (var col = Worksheet.Dimension.Start.Column; col <= Worksheet.Dimension.End.Column; col++)
            {
                if (Worksheet.Cells[1, col].Text != "№ п/п") continue;
                using var excelRange = Worksheet.Cells[2, 1, Worksheet.Dimension.End.Row, Worksheet.Dimension.End.Column];
                excelRange.Sort(col - 1);
                break;
            }
        }
        return Task.CompletedTask;
    }

    #endregion

    #region GetFileName

    /// <summary>
    /// Определение имени файла.
    /// </summary>
    /// <param name="rep">Отчёт.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Имя файла.</returns>
    private async Task<string> GetFileName(Report rep, AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        string fileName;
        var formNum = rep.FormNum_DB;
        var regNum = RemoveForbiddenChars(rep.Reports.Master.RegNoRep.Value);
        var okpo = RemoveForbiddenChars(rep.Reports.Master.OkpoRep.Value);
        var corNum = Convert.ToString(rep.CorrectionNumber_DB);
        switch (formNum[0])
        {
            case '1':
                var startPeriod = RemoveForbiddenChars(rep.StartPeriod_DB);
                var endPeriod = RemoveForbiddenChars(rep.EndPeriod_DB);
                fileName = $"{ExportType}_{regNum}_{okpo}_{formNum}_{startPeriod}_{endPeriod}_{corNum}_{Assembly.GetExecutingAssembly().GetName().Version}";
                break;
            case '2':
                var year = RemoveForbiddenChars(rep.Year_DB);
                fileName = $"{ExportType}_{regNum}_{okpo}_{formNum}_{year}_{corNum}_{Assembly.GetExecutingAssembly().GetName().Version}";
                break;
            default:
                await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                fileName = "";
                break;
        }
        return fileName;
    }

    #endregion

    #region GetReportWithRows

    /// <summary>
    /// Получение отчёта вместе со строчками из БД.
    /// </summary>
    /// <param name="repId">Id отчёта.</param>
    /// <param name="dbPath">Полный путь к временной БД.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Отчёт вместе со строчками.</returns>
    private static async Task<Report> GetReportWithRows(int repId, string dbPath, CancellationTokenSource cts)
    {
        await using var db = new DBModel(dbPath);
        return await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(rep => rep.Reports).ThenInclude(reps => reps.Master_DB).ThenInclude(x => x.Rows10)
                .Include(rep => rep.Reports).ThenInclude(reps => reps.Master_DB).ThenInclude(x => x.Rows20)
                .Include(rep => rep.Rows11.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows12.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows13.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows14.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows15.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows16.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows17.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows18.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows19.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows21.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows22.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows23.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows24.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows25.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows26.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows27.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows28.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows29.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows210.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows211.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Rows212.OrderBy(form => form.NumberInOrder_DB))
                .Include(rep => rep.Notes.OrderBy(note => note.Order))
                .FirstAsync(rep => rep.Id == repId, cts.Token);
    }

    #endregion
}