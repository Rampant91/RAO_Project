using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;
using Client_App.ViewModels;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Excel -> Список исполнителей.
/// </summary>
public class ExcelExportExecutorsAsyncCommand : ExcelExportListOfFormsBaseAsyncCommand
{
    /// <summary>
    /// Firebird ограничивает список значений в IN (...) ~1500 элементами.
    /// </summary>
    private const int FirebirdInClauseBatchSize = 1000;

    /// <summary>
    /// Максимальная ширина колонки после AutoFit (в символах стандартного шрифта Excel).
    /// </summary>
    private const double MaxAutoFitColumnWidth = 35;

    /// <summary>
    /// Ширина колонок с датами (в символах стандартного шрифта Excel).
    /// </summary>
    private const double DateColumnWidth = 10;

    /// <summary>
    /// Ширина колонки «Отчетный год».
    /// </summary>
    private const double YearColumnWidth = 8;

    /// <summary>
    /// Ширина колонки «Номер корректировки».
    /// </summary>
    private const double CorrectionColumnWidth = 8;

    /// <summary>
    /// Высота строки заголовков (в пунктах).
    /// </summary>
    private const double HeaderRowHeight = 55;

    /// <summary>
    /// Суффикс переноса строки в заголовке узкой колонки — стрелка AutoFilter остаётся на пустой строке.
    /// </summary>
    private const string HeaderFilterLineBreak = "\n";

    /// <summary>
    /// Интервал обновления прогрессбара при записи строк в Excel (в строках).
    /// </summary>
    private const int ProgressUpdateRowInterval = 50;

    /// <summary>
    /// Начало диапазона прогрессбара при загрузке листа форм 1 из БД.
    /// </summary>
    private const int ProgressForm1DbStart = 15;
    /// <summary>
    /// Прогрессбар после загрузки организаций форм 1.
    /// </summary>
    private const int ProgressForm1OrgsLoaded = 18;

    /// <summary>
    /// Прогрессбар после загрузки отчётов форм 1 из БД.
    /// </summary>
    private const int ProgressForm1DbEnd = 28;

    /// <summary>
    /// Прогрессбар после записи листа форм 1 в Excel.
    /// </summary>
    private const int ProgressForm1ExcelEnd = 32;

    /// <summary>
    /// Начало диапазона прогрессбара при загрузке листа форм 2 из БД.
    /// </summary>
    private const int ProgressForm2DbStart = 32;

    /// <summary>
    /// Прогрессбар после загрузки организаций форм 2.
    /// </summary>
    private const int ProgressForm2OrgsLoaded = 35;

    /// <summary>
    /// Прогрессбар после загрузки отчётов форм 2 из БД.
    /// </summary>
    private const int ProgressForm2DbEnd = 45;

    /// <summary>
    /// Прогрессбар после записи листа форм 2 в Excel.
    /// </summary>
    private const int ProgressForm2ExcelEnd = 49;

    /// <summary>
    /// Начало диапазона прогрессбара при загрузке листа форм 4 из БД.
    /// </summary>
    private const int ProgressForm4DbStart = 49;

    /// <summary>
    /// Прогрессбар после загрузки организаций форм 4.
    /// </summary>
    private const int ProgressForm4OrgsLoaded = 52;

    /// <summary>
    /// Прогрессбар после загрузки отчётов форм 4 из БД.
    /// </summary>
    private const int ProgressForm4DbEnd = 62;

    /// <summary>
    /// Прогрессбар после записи листа форм 4 в Excel.
    /// </summary>
    private const int ProgressForm4ExcelEnd = 66;

    /// <summary>
    /// Цвет фона чередующихся строк данных.
    /// </summary>
    private static readonly Color AlternatingRowFill = Color.FromArgb(221, 235, 247); // #DDEBF7

    /// <summary>
    /// Текущая организация при заполнении листа Excel.
    /// </summary>
    private Reports _currentReports = null!;

    /// <summary>
    /// Номер текущей строки при заполнении листа Excel.
    /// </summary>
    private int _currentRow;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Список_исполнителей";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(2, "Проверка параметров", 
            "Выгрузка списка исполнителей", "Выгрузка в .xlsx");
        var folderPath = await CheckAppParameter();
        var isBackgroundCommand = folderPath != string.Empty;

        progressBarVM.SetProgressBar(3, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = !isBackgroundCommand
            ? await ExcelGetFullPath(fileName, cts, progressBar)
            : (Path.Combine(folderPath, $"{fileName}.xlsx"), true);

        progressBarVM.SetProgressBar(5, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        try
        {
            await using var db = new DBModel(tmpDbPath);

            progressBarVM.SetProgressBar(10, "Подсчёт количества организаций");
            await CountReports(db, progressBar, cts);

            fullPath = ResolveUniqueFilePath(fullPath, isBackgroundCommand ? folderPath : null);

            progressBarVM.SetProgressBar(12, "Инициализация Excel пакета");
            using var excelPackage = await InitializeExcelPackage(fullPath);

            await ExportExecutorsSheet(
                db, excelPackage, progressBarVM, cts,
                formNum: '1', sheetName: "Формы 1", masterFormNum: "1.0", reportFormPrefix: "1.",
                includeMaster: q => q
                    .Include(reps => reps.Master_DB)
                    .ThenInclude(rep => rep.Rows10),
                progressDbStart: ProgressForm1DbStart,
                progressOrgsLoaded: ProgressForm1OrgsLoaded,
                progressDbEnd: ProgressForm1DbEnd,
                progressExcelEnd: ProgressForm1ExcelEnd);

            await ExportExecutorsSheet(
                db, excelPackage, progressBarVM, cts,
                formNum: '2', sheetName: "Формы 2", masterFormNum: "2.0", reportFormPrefix: "2.",
                includeMaster: q => q
                    .Include(reps => reps.Master_DB)
                    .ThenInclude(rep => rep.Rows20),
                progressDbStart: ProgressForm2DbStart,
                progressOrgsLoaded: ProgressForm2OrgsLoaded,
                progressDbEnd: ProgressForm2DbEnd,
                progressExcelEnd: ProgressForm2ExcelEnd);

            await ExportExecutorsSheet(
                db, excelPackage, progressBarVM, cts,
                formNum: '4', sheetName: "Формы 4", masterFormNum: "4.0", reportFormPrefix: "4.",
                includeMaster: q => q
                    .Include(reps => reps.Master_DB)
                    .ThenInclude(rep => rep.Rows40),
                progressDbStart: ProgressForm4DbStart,
                progressOrgsLoaded: ProgressForm4OrgsLoaded,
                progressDbEnd: ProgressForm4DbEnd,
                progressExcelEnd: ProgressForm4ExcelEnd);

            progressBarVM.SetProgressBar(95, "Сохранение");
            await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar, isBackgroundCommand);

            progressBarVM.SetProgressBar(100, "Завершение выгрузки");
            await progressBar.CloseAsync();
        }
        finally
        {
            TryDeleteTempDataBase(tmpDbPath);
        }
    }

    #region CountReports

    /// <summary>
    /// Подсчёт количества организаций. При = 0 выводит сообщение и завершает операцию.
    /// </summary>
    /// <param name="db">Модель временной БД.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    private static async Task CountReports(DBModel db, AnyTaskProgressBar? progressBar, CancellationTokenSource cts)
    {
        var countReports = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(x => x.DBObservableId != null)
            .CountAsync(cts.Token);

        if (countReports == 0)
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Выгрузка не выполнена, поскольку в базе отсутствуют формы отчетности организаций.",
                    MinHeight = 150,
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
    }

    #endregion

    #region FillExecutorsHeaders

    /// <summary>
    /// Заполняет заголовки в выгрузке в .xlsx.
    /// </summary>
    /// <param name="formNum">Номер формы (1 - оперативная, 2 - годовая).</param>
    private Task FillExecutorsHeaders(char formNum)
    {
        switch (formNum)
        {
            case '1':
            {
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Форма";
                Worksheet.Cells[1, 5].Value = "Дата начала периода" + HeaderFilterLineBreak;
                Worksheet.Cells[1, 6].Value = "Дата конца периода" + HeaderFilterLineBreak;
                Worksheet.Cells[1, 7].Value = "Номер корректировки" + HeaderFilterLineBreak;
                Worksheet.Cells[1, 8].Value = "ФИО исполнителя";
                Worksheet.Cells[1, 9].Value = "Должность";
                Worksheet.Cells[1, 10].Value = "Телефон";
                Worksheet.Cells[1, 11].Value = "Электронная почта";
                break;
            }
            case '2':
            {
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Форма";
                Worksheet.Cells[1, 5].Value = "Отчетный год";
                Worksheet.Cells[1, 6].Value = "Номер корректировки" + HeaderFilterLineBreak;
                Worksheet.Cells[1, 7].Value = "ФИО исполнителя";
                Worksheet.Cells[1, 8].Value = "Должность";
                Worksheet.Cells[1, 9].Value = "Телефон";
                Worksheet.Cells[1, 10].Value = "Электронная почта";
                break;
            }
            case '4':
            {
                Worksheet.Cells[1, 1].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 2].Value = "Форма";
                Worksheet.Cells[1, 3].Value = "Отчетный год";
                Worksheet.Cells[1, 4].Value = "Номер корректировки" + HeaderFilterLineBreak;
                Worksheet.Cells[1, 5].Value = "ФИО исполнителя";
                Worksheet.Cells[1, 6].Value = "Должность";
                Worksheet.Cells[1, 7].Value = "Телефон";
                Worksheet.Cells[1, 8].Value = "Электронная почта";
                break;
            }
        }
        return Task.CompletedTask;
    }

    #endregion

    #region FillExecutors

    /// <summary>
    /// Выгрузка строчек данных в .xlsx.
    /// </summary>
    /// <param name="rep">Данные отчёта.</param>
    /// <param name="formNum">Группа форм (1, 2 или 4).</param>
    private void FillExecutors(ExecutorReportInfo rep, char formNum)
    {
        switch (formNum)
        {
            case '1':
            {
                Worksheet.Cells[_currentRow, 1].Value = GetRegNoRep(_currentReports.Master);
                Worksheet.Cells[_currentRow, 2].Value = GetShortJurLicoRep(_currentReports.Master);
                Worksheet.Cells[_currentRow, 3].Value = GetOkpoRep(_currentReports.Master);
                Worksheet.Cells[_currentRow, 4].Value = rep.FormNum;
                Worksheet.Cells[_currentRow, 5].Value = ConvertToExcelDate(rep.StartPeriod ?? string.Empty, Worksheet, _currentRow, 5);
                Worksheet.Cells[_currentRow, 6].Value = ConvertToExcelDate(rep.EndPeriod ?? string.Empty, Worksheet, _currentRow, 6);
                Worksheet.Cells[_currentRow, 7].Value = rep.CorrectionNumber;
                Worksheet.Cells[_currentRow, 8].Value = rep.FioExecutor;
                Worksheet.Cells[_currentRow, 9].Value = rep.GradeExecutor;
                Worksheet.Cells[_currentRow, 10].Value = rep.ExecPhone;
                Worksheet.Cells[_currentRow, 11].Value = rep.ExecEmail;
                break;
            }
            case '2':
            {
                Worksheet.Cells[_currentRow, 1].Value = GetRegNoRep(_currentReports.Master);
                Worksheet.Cells[_currentRow, 2].Value = GetShortJurLicoRep(_currentReports.Master);
                Worksheet.Cells[_currentRow, 3].Value = GetOkpoRep(_currentReports.Master);
                Worksheet.Cells[_currentRow, 4].Value = rep.FormNum;
                Worksheet.Cells[_currentRow, 5].Value = rep.Year;
                Worksheet.Cells[_currentRow, 6].Value = rep.CorrectionNumber;
                Worksheet.Cells[_currentRow, 7].Value = rep.FioExecutor;
                Worksheet.Cells[_currentRow, 8].Value = rep.GradeExecutor;
                Worksheet.Cells[_currentRow, 9].Value = rep.ExecPhone;
                Worksheet.Cells[_currentRow, 10].Value = rep.ExecEmail;
                break;
            }
            case '4':
            {
                Worksheet.Cells[_currentRow, 1].Value = _currentReports.Master.Rows40
                    .OrderBy(r => r.NumberInOrder_DB)
                    .FirstOrDefault()?.ShortNameRiac.Value;
                Worksheet.Cells[_currentRow, 2].Value = rep.FormNum;
                Worksheet.Cells[_currentRow, 3].Value = rep.Year;
                Worksheet.Cells[_currentRow, 4].Value = rep.CorrectionNumber;
                Worksheet.Cells[_currentRow, 5].Value = rep.FioExecutor;
                Worksheet.Cells[_currentRow, 6].Value = rep.GradeExecutor;
                Worksheet.Cells[_currentRow, 7].Value = rep.ExecPhone;
                Worksheet.Cells[_currentRow, 8].Value = rep.ExecEmail;
                break;
            }
        }
    }

    #endregion

    #region ExportExecutorsSheet

    /// <summary>
    /// Загружает данные и формирует лист Excel для указанной группы форм.
    /// </summary>
    private async Task ExportExecutorsSheet(
        DBModel db,
        ExcelPackage excelPackage,
        AnyTaskProgressBarVM progressBarVM,
        CancellationTokenSource cts,
        char formNum,
        string sheetName,
        string masterFormNum,
        string reportFormPrefix,
        Func<IQueryable<Reports>, IQueryable<Reports>> includeMaster,
        int progressDbStart,
        int progressOrgsLoaded,
        int progressDbEnd,
        int progressExcelEnd)
    {
        var token = cts.Token;

        progressBarVM.SetProgressBar(progressDbStart, $"Загрузка организаций по форме {formNum}");
        var data = await LoadExecutorsData(
            db,
            masterFormNum,
            reportFormPrefix,
            includeMaster,
            progressBarVM,
            progressOrgsLoaded,
            progressDbEnd,
            token);

        _currentRow = 2;
        var firstDataRow = _currentRow;
        Worksheet = excelPackage.Workbook.Worksheets.Add(sheetName);
        await FillExecutorsHeaders(formNum);

        var totalRows = data.Sum(o => o.ExecutorReports.Count);
        var excelFillRange = progressExcelEnd - progressDbEnd;
        double progressValue = progressDbEnd;

        progressBarVM.SetProgressBar(progressDbEnd, $"Запись в Excel: лист «{sheetName}»");

        var rowsWritten = 0;
        foreach (var org in data)
        {
            _currentReports = org.Reports;
            foreach (var rep in OrderExecutorReports(org.ExecutorReports, formNum))
            {
                FillExecutors(rep, formNum);
                _currentRow++;

                if (totalRows > 0)
                {
                    progressValue += (double)excelFillRange / totalRows;
                    rowsWritten++;
                    if (rowsWritten % ProgressUpdateRowInterval == 0 || rowsWritten == totalRows)
                    {
                        progressBarVM.SetProgressBar(
                            Math.Min(progressExcelEnd, (int)Math.Floor(progressValue)),
                            $"Запись в Excel: {rep.FormNum}");
                    }
                }
            }
        }

        if (_currentRow > firstDataRow)
            ApplyAlternatingRowColors(firstDataRow, _currentRow - 1);

        ApplyColumnWidths();
        ApplyExcelHeaderRowStyle(Worksheet.Dimension.End.Column, headerRowHeight: HeaderRowHeight);
    }

    /// <summary>
    /// Сортирует отчёты организации для выгрузки на лист Excel.
    /// </summary>
    /// <param name="reports">Отчёты одной организации.</param>
    /// <param name="formNum">Группа форм (1, 2 или 4).</param>
    private static IEnumerable<ExecutorReportInfo> OrderExecutorReports(
        IReadOnlyList<ExecutorReportInfo> reports,
        char formNum) =>
        formNum switch
        {
            '1' => reports
                .OrderBy(x => GetFormSubNumberOrder(x.FormNum))
                .ThenByDescending(x => DateOnly.TryParse(x.StartPeriod, out var stPer)
                    ? stPer
                    : DateOnly.MaxValue)
                .ThenByDescending(x => DateOnly.TryParse(x.EndPeriod, out var endPer)
                    ? endPer
                    : DateOnly.MaxValue),
            '2' or '4' => reports
                .OrderBy(x => GetFormSubNumberOrder(x.FormNum))
                .ThenByDescending(x => int.TryParse(x.Year, out var year)
                    ? year
                    : int.MinValue),
            _ => reports
        };

    /// <summary>
    /// Чередует белый и светло-голубой фон строк данных.
    /// </summary>
    private void ApplyAlternatingRowColors(int firstRow, int lastRow)
    {
        var lastColumn = Worksheet.Dimension.End.Column;

        for (var row = firstRow; row <= lastRow; row++)
        {
            if ((row - firstRow) % 2 != 0)
            {
                Worksheet.Cells[row, 1, row, lastColumn].Style.Fill.SetBackground(
                    AlternatingRowFill,
                    ExcelFillStyle.Solid);
            }
        }
    }

    /// <summary>
    /// Подбирает ширину колонок: узкие колонки (даты, год, корректировка) — фиксированная ширина;
    /// текстовые — AutoFit с ограничением сверху (только Windows; на Linux — ширина по умолчанию) и перенос строк.
    /// </summary>
    private void ApplyColumnWidths()
    {
        var canAutoFit = OperatingSystem.IsWindows();

        for (var col = 1; col <= Worksheet.Dimension.End.Column; col++)
        {
            var header = NormalizeHeaderText(Worksheet.Cells[1, col].Value?.ToString());
            var column = Worksheet.Column(col);

            if (TryGetFixedColumnWidth(header, out var fixedWidth))
            {
                column.Width = fixedWidth;
                continue;
            }

            if (canAutoFit)
            {
                column.AutoFit();
                if (column.Width > MaxAutoFitColumnWidth)
                    column.Width = MaxAutoFitColumnWidth;
            }
            else if (IsTextColumn(header))
            {
                column.Width = MaxAutoFitColumnWidth;
            }

            if (IsTextColumn(header))
                column.Style.WrapText = true;
        }
    }

    /// <summary>
    /// Убирает завершающий перенос строки из текста заголовка для сопоставления с шаблонами.
    /// </summary>
    private static string NormalizeHeaderText(string? header) =>
        header?.TrimEnd('\n', '\r') ?? string.Empty;

    /// <summary>
    /// Определяет фиксированную ширину колонки по заголовку.
    /// </summary>
    private static bool TryGetFixedColumnWidth(string? header, out double width)
    {
        switch (header)
        {
            case "Дата начала периода":
            case "Дата конца периода":
                width = DateColumnWidth;
                return true;
            case "Отчетный год":
                width = YearColumnWidth;
                return true;
            case "Номер корректировки":
                width = CorrectionColumnWidth;
                return true;
            default:
                width = 0;
                return false;
        }
    }

    /// <summary>
    /// Определяет, является ли колонка текстовой (с переносом длинного содержимого).
    /// </summary>
    private static bool IsTextColumn(string? header) =>
        header is "Сокращенное наименование" or "ФИО исполнителя" or "Должность" or "Электронная почта";

    #endregion

    #region LoadExecutorsData

    /// <summary>
    /// Загружает организации и отчёты для выгрузки исполнителей по группе форм.
    /// </summary>
    private static async Task<IReadOnlyList<OrgExecutorsData>> LoadExecutorsData(
        DBModel db,
        string masterFormNum,
        string reportFormPrefix,
        Func<IQueryable<Reports>, IQueryable<Reports>> includeMaster,
        AnyTaskProgressBarVM progressBarVM,
        int progressOrgsLoaded,
        int progressDbEnd,
        CancellationToken token)
    {
        var query = db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.DBObservableId != null)
            .Where(x => x.Master_DB.FormNum_DB == masterFormNum);

        query = includeMaster(query);

        var orgs = await query.ToListAsync(token);

        progressBarVM.SetProgressBar(progressOrgsLoaded, $"Загрузка отчётов форм {reportFormPrefix.TrimEnd('.')}.x");

        var orgIds = orgs.Select(o => o.Id).ToHashSet();
        var reportsByOrgId = await LoadExecutorReports(
            db, orgIds, reportFormPrefix, progressBarVM, progressOrgsLoaded, progressDbEnd, token);

        return orgs
            .Select(org =>
            {
                reportsByOrgId.TryGetValue(org.Id, out var reports);
                return new OrgExecutorsData(org, reports ?? []);
            })
            .ToList();
    }

    /// <summary>
    /// Загружает только поля отчётов, необходимые для выгрузки исполнителей.
    /// </summary>
    private static async Task<Dictionary<int, List<ExecutorReportInfo>>> LoadExecutorReports(
        DBModel db,
        HashSet<int> orgIds,
        string formPrefix,
        AnyTaskProgressBarVM progressBarVM,
        int progressReportsLoadStart,
        int progressReportsLoadEnd,
        CancellationToken token)
    {
        if (orgIds.Count == 0)
            return [];

        var orgIdList = orgIds.ToList();
        var batchCount = (orgIdList.Count + FirebirdInClauseBatchSize - 1) / FirebirdInClauseBatchSize;
        var masterFormNum = formPrefix.TrimEnd('.') + ".0";
        var result = new Dictionary<int, List<ExecutorReportInfo>>(orgIds.Count);
        var formGroup = formPrefix.TrimEnd('.');

        for (var batchIndex = 0; batchIndex < batchCount; batchIndex++)
        {
            var batch = orgIdList
                .Skip(batchIndex * FirebirdInClauseBatchSize)
                .Take(FirebirdInClauseBatchSize)
                .ToList();

            var status = batchCount == 1
                ? $"Загрузка отчётов форм {formGroup}.x"
                : $"Загрузка отчётов форм {formGroup}.x (пакет {batchIndex + 1}/{batchCount})";
            var batchRange = progressReportsLoadEnd - progressReportsLoadStart;
            var percent = progressReportsLoadStart
                          + (int)Math.Floor(batchRange * (batchIndex + 1) / (double)batchCount);
            progressBarVM.SetProgressBar(percent, status);

            var rows = await db.ReportCollectionDbSet
                .AsNoTracking()
                .Where(r => r.Reports != null && batch.Contains(r.Reports.Id))
                .Where(r => r.FormNum_DB.StartsWith(formPrefix) && r.FormNum_DB != masterFormNum)
                .Select(r => new ExecutorReportInfo(
                    r.Reports!.Id,
                    r.FormNum_DB,
                    r.StartPeriod_DB,
                    r.EndPeriod_DB,
                    r.Year_DB,
                    r.CorrectionNumber_DB,
                    r.FIOexecutor_DB,
                    r.GradeExecutor_DB,
                    r.ExecPhone_DB,
                    r.ExecEmail_DB))
                .ToListAsync(token);

            foreach (var row in rows)
            {
                if (!result.TryGetValue(row.OrgId, out var list))
                {
                    list = [];
                    result[row.OrgId] = list;
                }

                list.Add(row);
            }
        }

        return result;
    }

    /// <summary>
    /// Возвращает числовой подномер формы для сортировки (например, 10 для «1.10»).
    /// </summary>
    private static int GetFormSubNumberOrder(string formNum)
    {
        var parts = formNum.Split('.');
        return parts.Length > 1 && int.TryParse(parts[1], out var order) ? order : int.MaxValue;
    }

    #endregion

    #region MasterTitleFields

    /// <summary>
    /// Безопасное чтение регистрационного номера представительной формы организации.
    /// </summary>
    private static string GetRegNoRep(Report master) => master.FormNum_DB switch
    {
        "1.0" => GetRegNoRepForm10(master),
        "2.0" => GetRegNoRepForm20(master),
        _ => string.Empty
    };

    private static string GetOkpoRep(Report master) => master.FormNum_DB switch
    {
        "1.0" => GetOkpoRepForm10(master),
        "2.0" => GetOkpoRepForm20(master),
        _ => string.Empty
    };

    private static string GetShortJurLicoRep(Report master) => master.FormNum_DB switch
    {
        "1.0" => GetShortJurLicoRepForm10(master),
        "2.0" => GetShortJurLicoRepForm20(master),
        _ => string.Empty
    };

    private static Form10? GetForm10Row(Report master, int index) =>
        master.Rows10.OrderBy(r => r.NumberInOrder_DB).ElementAtOrDefault(index);

    private static Form20? GetForm20Row(Report master, int index) =>
        master.Rows20.OrderBy(r => r.NumberInOrder_DB).ElementAtOrDefault(index);

    private static string GetRegNoRepForm10(Report master)
    {
        var branch = GetForm10Row(master, 1);
        var head = GetForm10Row(master, 0);
        if (branch is not null
            && (GetForm10RowRegNo(branch) != "" || branch.Okpo_DB == "-")
            && GetForm10RowOkpo(branch) != "")
        {
            return GetForm10RowRegNo(branch);
        }

        return head is not null ? GetForm10RowRegNo(head) : string.Empty;
    }

    private static string GetRegNoRepForm20(Report master)
    {
        var branch = GetForm20Row(master, 1);
        var head = GetForm20Row(master, 0);
        if (branch is not null
            && (GetForm20RowRegNo(branch) != "" || branch.Okpo_DB == "-")
            && GetForm20RowOkpo(branch) != "")
        {
            return GetForm20RowRegNo(branch);
        }

        return head is not null ? GetForm20RowRegNo(head) : string.Empty;
    }

    private static string GetOkpoRepForm10(Report master)
    {
        var branch = GetForm10Row(master, 1);
        var head = GetForm10Row(master, 0);
        if (branch is not null && branch.Okpo_DB is not ("" or "-"))
            return GetForm10RowOkpo(branch);

        return head is not null ? GetForm10RowOkpo(head) : string.Empty;
    }

    private static string GetOkpoRepForm20(Report master)
    {
        var branch = GetForm20Row(master, 1);
        var head = GetForm20Row(master, 0);
        if (branch is not null && branch.Okpo_DB is not ("" or "-"))
            return GetForm20RowOkpo(branch);

        return head is not null ? GetForm20RowOkpo(head) : string.Empty;
    }

    private static string GetShortJurLicoRepForm10(Report master)
    {
        var branch = GetForm10Row(master, 1);
        var head = GetForm10Row(master, 0);
        if (branch is not null && branch.Okpo_DB is not ("" or "-"))
            return GetForm10ShortJurLico(branch);

        return head is not null ? GetForm10ShortJurLico(head) : string.Empty;
    }

    private static string GetShortJurLicoRepForm20(Report master)
    {
        var branch = GetForm20Row(master, 1);
        var head = GetForm20Row(master, 0);
        if (branch is not null && branch.Okpo_DB is not ("" or "-"))
            return GetForm20ShortJurLico(branch);

        return head is not null ? GetForm20ShortJurLico(head) : string.Empty;
    }

    private static string GetForm10RowOkpo(Form10 row) =>
        string.IsNullOrWhiteSpace(row.Okpo_DB) ? (row.Okpo?.Value ?? "").Trim() : row.Okpo_DB.Trim();

    private static string GetForm10RowRegNo(Form10 row) =>
        string.IsNullOrWhiteSpace(row.RegNo_DB) ? (row.RegNo?.Value ?? "").Trim() : row.RegNo_DB.Trim();

    private static string GetForm20RowOkpo(Form20 row) =>
        string.IsNullOrWhiteSpace(row.Okpo_DB) ? (row.Okpo?.Value ?? "").Trim() : row.Okpo_DB.Trim();

    private static string GetForm20RowRegNo(Form20 row) =>
        string.IsNullOrWhiteSpace(row.RegNo_DB) ? (row.RegNo?.Value ?? "").Trim() : row.RegNo_DB.Trim();

    private static string GetForm10ShortJurLico(Form10 row) =>
        string.IsNullOrWhiteSpace(row.ShortJurLico_DB)
            ? (row.ShortJurLico?.Value ?? "").Trim()
            : row.ShortJurLico_DB.Trim();

    private static string GetForm20ShortJurLico(Form20 row) =>
        string.IsNullOrWhiteSpace(row.ShortJurLico_DB)
            ? (row.ShortJurLico?.Value ?? "").Trim()
            : row.ShortJurLico_DB.Trim();

    #endregion

    #region ExportData

    /// <summary>
    /// Данные организации и её отчётов для выгрузки исполнителей.
    /// </summary>
    private sealed class OrgExecutorsData(Reports reports, IReadOnlyList<ExecutorReportInfo> executorReports)
    {
        /// <summary>
        /// Организация.
        /// </summary>
        public Reports Reports { get; } = reports;

        /// <summary>
        /// Отчёты организации с полями исполнителей.
        /// </summary>
        public IReadOnlyList<ExecutorReportInfo> ExecutorReports { get; } = executorReports;
    }

    /// <summary>
    /// Облегчённая проекция отчёта с полями исполнителя.
    /// </summary>
    private readonly record struct ExecutorReportInfo(
        int OrgId,
        string FormNum,
        string? StartPeriod,
        string? EndPeriod,
        string? Year,
        byte CorrectionNumber,
        string? FioExecutor,
        string? GradeExecutor,
        string? ExecPhone,
        string? ExecEmail);

    #endregion
}
