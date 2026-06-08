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
using Client_App.ViewModels;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Excel -> Список организаций.
/// </summary>
public class ExcelExportListOfOrgsAsyncCommand : ExcelBaseAsyncCommand
{
    /// <summary>
    /// Номера форм, количество которых подсчитывается при выгрузке.
    /// </summary>
    private static readonly string[] ExportFormNumbers =
    [
        "1.1", "1.2", "1.3", "1.4", "1.5", "1.6", "1.7", "1.8", "1.9",
        "2.1", "2.2", "2.3", "2.4", "2.5", "2.6", "2.7", "2.8", "2.9", "2.10", "2.11", "2.12"
    ];

    /// <summary>
    /// Firebird ограничивает список значений в IN (...) ~1500 элементами.
    /// </summary>
    private const int FirebirdInClauseBatchSize = 1000;

    /// <summary>
    /// Максимальная ширина колонки после AutoFit (в символах стандартного шрифта Excel).
    /// </summary>
    private const double MaxAutoFitColumnWidth = 35;

    /// <summary>
    /// Интервал обновления прогрессбара при записи строк в Excel (в организациях).
    /// </summary>
    private const int ProgressUpdateRowInterval = 50;

    /// <summary>
    /// Ширина колонок счётчиков форм (в символах стандартного шрифта Excel).
    /// </summary>
    private const double FormCountColumnWidth = 10;

    /// <summary>
    /// Высота строки заголовков (в пунктах).
    /// </summary>
    private const double HeaderRowHeight = 40;

    /// <summary>
    /// Начало диапазона прогрессбара при загрузке данных из БД.
    /// </summary>
    private const int ProgressDbLoadStart = 18;

    /// <summary>
    /// Прогрессбар после загрузки организаций формы 1.0.
    /// </summary>
    private const int ProgressForm10Loaded = 28;

    /// <summary>
    /// Прогрессбар после загрузки организаций формы 2.0.
    /// </summary>
    private const int ProgressForm20Loaded = 38;

    /// <summary>
    /// Прогрессбар после загрузки отчётов для подсчёта форм.
    /// </summary>
    private const int ProgressFormReportsLoadEnd = 72;

    /// <summary>
    /// Прогрессбар после завершения загрузки данных из БД.
    /// </summary>
    private const int ProgressDbLoadEnd = 74;

    /// <summary>
    /// Прогрессбар после записи данных в Excel.
    /// </summary>
    private const int ProgressExcelFillEnd = 92;

    /// <summary>
    /// Цвет фона чередующихся строк данных.
    /// </summary>
    private static readonly Color AlternatingRowFill = Color.FromArgb(221, 235, 247); // #DDEBF7

    /// <summary>
    /// Начало периода фильтрации для форм 1 (MinValue — без нижней границы).
    /// </summary>
    private DateOnly _form1Start = DateOnly.MinValue;

    /// <summary>
    /// Конец периода фильтрации для форм 1 (MaxValue — без верхней границы).
    /// </summary>
    private DateOnly _form1End = DateOnly.MaxValue;

    /// <summary>
    /// Начальный год фильтрации для форм 2 (MinValue — без нижней границы).
    /// </summary>
    private int _form2YearStart = int.MinValue;

    /// <summary>
    /// Конечный год фильтрации для форм 2 (MaxValue — без верхней границы).
    /// </summary>
    private int _form2YearEnd = int.MaxValue;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();

        ExportType = parameter is "full"
            ? "Список_организаций_с_доп_полями"
            : "Список_организаций";

        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(2, "Проверка параметров", "Выгрузка в .xlsx", ExportType);
        var folderPath = await CheckAppParameter();
        var isBackgroundCommand = folderPath != string.Empty;

        progressBarVM.SetProgressBar(5, "Запрос периода фильтрации");
        if (!isBackgroundCommand)
        {
            await InputPeriodFilter(progressBar, cts);
        }

        progressBarVM.SetProgressBar(8, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(10, "Подсчёт количества организаций");
        await ReportsCountCheck(db, progressBar, cts);

        progressBarVM.SetProgressBar(13, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";

        var (fullPath, openTemp) = !isBackgroundCommand
            ? await ExcelGetFullPath(fileName, cts, progressBar)
            : (Path.Combine(folderPath, $"{fileName}.xlsx"), true);

        fullPath = ResolveUniqueFilePath(fullPath, isBackgroundCommand ? folderPath : null);

        progressBarVM.SetProgressBar(15, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(18, "Заполнение заголовков");
        await FillExcelHeaders(excelPackage, parameter);

        progressBarVM.SetProgressBar(ProgressDbLoadStart, "Получение списка организаций");
        var repsList = await GetReportsList(db, progressBarVM, cts);

        progressBarVM.SetProgressBar(ProgressDbLoadEnd, "Заполнение строчек в .xlsx");
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
            .Where(x => x.DBObservableId != null)
            .Where(x => x.Master_DB.FormNum_DB == "1.0" || x.Master_DB.FormNum_DB == "2.0")
            .CountAsync(cts.Token);

        if (countReports == 0)
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        "Не удалось совершить выгрузку списка всех отчетов по форме 1 с указанием количества строк," +
                        $"{Environment.NewLine}поскольку в текущей базе отсутствуют формы организаций.",
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

    #region InputPeriodFilter

    /// <summary>
    /// Запрашивает у пользователя период фильтрации в одном окне:
    /// даты начала/конца для форм 1 и годы начала/конца для форм 2.
    /// Пустые поля означают отсутствие соответствующей границы.
    /// </summary>
    private async Task InputPeriodFilter(AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        var res = await Dispatcher.UIThread.InvokeAsync(() =>
        {
            var window = new AskListOfOrgsPeriodMessageWindow();
            return window.ShowDialog<(string command, DateOnly form1Start, DateOnly form1End, int form2Start, int form2End)>(Desktop.MainWindow);
        });

        if (res.command is not "Ок")
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
            return;
        }

        _form1Start = res.form1Start;
        _form1End = res.form1End;
        _form2YearStart = res.form2Start;
        _form2YearEnd = res.form2End;
    }

    #endregion

    #region GetReportsList

    /// <summary>
    /// Получение списка организаций: титульные данные и облегчённый набор полей дочерних отчётов для подсчёта форм.
    /// </summary>
    private async Task<IReadOnlyCollection<OrgExportData>> GetReportsList(
        DBModel db,
        AnyTaskProgressBarVM progressBarVM,
        CancellationTokenSource cts)
    {
        var token = cts.Token;

        progressBarVM.SetProgressBar(ProgressDbLoadStart, "Загрузка организаций по форме 1");
        var form10Orgs = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Where(reps => reps.DBObservableId != null)
            .Where(reps => reps.Master_DB.FormNum_DB == "1.0")
            .Include(reps => reps.Master_DB)
                .ThenInclude(x => x.Rows10)
            .ToListAsync(token);

        progressBarVM.SetProgressBar(ProgressForm10Loaded, "Загрузка организаций по форме 2");
        var form20Orgs = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Where(reps => reps.DBObservableId != null)
            .Where(reps => reps.Master_DB.FormNum_DB == "2.0")
            .Include(reps => reps.Master_DB)
                .ThenInclude(x => x.Rows20)
            .ToListAsync(token);

        progressBarVM.SetProgressBar(ProgressForm20Loaded, "Загрузка отчётов форм 1 и 2");

        var orgIds = new HashSet<int>(form10Orgs.Count + form20Orgs.Count);
        foreach (var org in form10Orgs)
            orgIds.Add(org.Id);
        foreach (var org in form20Orgs)
            orgIds.Add(org.Id);

        var formReportsByOrgId = await LoadFormReportsForCount(db, orgIds, progressBarVM, token);

        progressBarVM.SetProgressBar(ProgressFormReportsLoadEnd, "Подготовка списка организаций");
        var result = new List<OrgExportData>(form10Orgs.Count + form20Orgs.Count);
        foreach (var org in form10Orgs)
        {
            formReportsByOrgId.TryGetValue(org.Id, out var formReports);
            result.Add(new OrgExportData(org, formReports ?? []));
        }

        foreach (var org in form20Orgs)
        {
            formReportsByOrgId.TryGetValue(org.Id, out var formReports);
            result.Add(new OrgExportData(org, formReports ?? []));
        }

        return result;
    }

    /// <summary>
    /// Загружает только поля, необходимые для подсчёта количества форм по организации.
    /// Запрос выполняется пакетами из-за ограничения Firebird на размер IN (...).
    /// </summary>
    private static async Task<Dictionary<int, List<FormReportCountInfo>>> LoadFormReportsForCount(
        DBModel db,
        HashSet<int> orgIds,
        AnyTaskProgressBarVM progressBarVM,
        CancellationToken token)
    {
        if (orgIds.Count == 0)
            return [];

        var orgIdList = orgIds.ToList();
        var batchCount = (orgIdList.Count + FirebirdInClauseBatchSize - 1) / FirebirdInClauseBatchSize;
        var result = new Dictionary<int, List<FormReportCountInfo>>(orgIds.Count);

        for (var batchIndex = 0; batchIndex < batchCount; batchIndex++)
        {
            var batch = orgIdList
                .Skip(batchIndex * FirebirdInClauseBatchSize)
                .Take(FirebirdInClauseBatchSize)
                .ToList();

            var status = batchCount == 1
                ? "Загрузка отчётов форм 1 и 2 для подсчёта"
                : $"Загрузка отчётов форм 1 и 2 для подсчёта (пакет {batchIndex + 1}/{batchCount})";
            var batchRange = ProgressFormReportsLoadEnd - ProgressForm20Loaded;
            var percent = ProgressForm20Loaded
                            + (int)Math.Floor(batchRange * (batchIndex + 1) / (double)batchCount);
            progressBarVM.SetProgressBar(percent, status);

            var rows = await db.ReportCollectionDbSet
                .AsNoTracking()
                .Where(r => r.Reports != null && batch.Contains(r.Reports.Id))
                .Where(r => ExportFormNumbers.Contains(r.FormNum_DB))
                .Select(r => new FormReportCountInfo(
                    r.Reports!.Id,
                    r.FormNum_DB,
                    r.StartPeriod_DB,
                    r.EndPeriod_DB,
                    r.Year_DB))
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

    #endregion

    #region FillExcelHeaders

    /// <summary>
    /// Заполнение заголовков в .xlsx.
    /// </summary>
    /// <param name="excelPackage">Excel пакет.</param>
    /// <param name="parameter">Параметр команды (full — выгрузка с дополнительными полями).</param>
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

        return Task.CompletedTask;
    }

    #endregion

    #region FillExcel

    /// <summary>
    /// Выгружает в .xlsx требуемые значения.
    /// </summary>
    /// <param name="repsList">Список организаций.</param>
    /// <param name="parameter">Параметр команды (full - выгрузка с дополнительными полями)</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    private Task FillExcel(IReadOnlyCollection<OrgExportData> repsList, object? parameter, AnyTaskProgressBarVM progressBarVM)
    {
        var isFullExport = parameter?.ToString() == "full";
        var formCountsStartColumn = isFullExport ? 42 : 8;
        var checkedLst = new List<OrgExportData>();
        var row = 2;
        var firstDataRow = row;
        var excelFillRange = ProgressExcelFillEnd - ProgressDbLoadEnd;
        double progressBarDoubleValue = ProgressDbLoadEnd;
        var rowsProcessed = 0;

        foreach (var org in repsList
                     .OrderBy(x => GetRegNoRep(x.Reps.Master))
                     .ThenBy(x => GetOkpoRep(x.Reps.Master)))
        {
            var regNo = GetRegNoRep(org.Reps.Master);
            var okpo = GetOkpoRep(org.Reps.Master);
            var isDuplicate = checkedLst.Any(x =>
                GetRegNoRep(x.Reps.Master) == regNo && GetOkpoRep(x.Reps.Master) == okpo);

            if (isDuplicate)
            {
                row--;
                AccumulateFormCounts(row, formCountsStartColumn, org);
                row++;
            }
            else
            {
                WriteOrgRow(row, org, isFullExport);
                row++;
                checkedLst.Add(org);
            }

            if (repsList.Count > 0)
            {
                progressBarDoubleValue += (double)excelFillRange / repsList.Count;
                rowsProcessed++;
                if (rowsProcessed % ProgressUpdateRowInterval == 0 || rowsProcessed == repsList.Count)
                {
                    progressBarVM.SetProgressBar(
                        Math.Min(ProgressExcelFillEnd, (int)Math.Floor(progressBarDoubleValue)),
                        $"Запись в Excel: {regNo}_{okpo}");
                }
            }
        }

        if (row > firstDataRow)
            ApplyAlternatingRowColors(firstDataRow, row - 1);

        ApplyColumnWidths();
        ApplyExcelHeaderRowStyle(Worksheet.Dimension.End.Column, headerRowHeight: HeaderRowHeight);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Записывает строку организации в лист Excel.
    /// </summary>
    private void WriteOrgRow(int row, OrgExportData org, bool isFullExport)
    {
        var master = org.Reps.Master;
        var regNo = GetRegNoRep(master);

        Worksheet.Cells[row, 1].Value = regNo;
        Worksheet.Cells[row, 2].Value = regNo.Length >= 2 ? regNo[..2] : regNo;
        Worksheet.Cells[row, 3].Value = GetOrganUprav(master);
        Worksheet.Cells[row, 4].Value = GetOkpoRep(master);
        Worksheet.Cells[row, 5].Value = GetShortJurLicoRep(master);
        Worksheet.Cells[row, 6].Value = GetJurAddress(master);
        Worksheet.Cells[row, 7].Value = GetInn(master);

        if (isFullExport)
        {
            WriteTitleRowFields(row, master, startColumn: 8, rowIndex: 0);
            WriteTitleRowFields(row, master, startColumn: 25, rowIndex: 1);
        }

        SetFormCounts(row, isFullExport ? 42 : 8, org);
    }

    /// <summary>
    /// Записывает поля титульной строки формы 1.0 или 2.0.
    /// </summary>
    private void WriteTitleRowFields(int row, Report master, int startColumn, int rowIndex)
    {
        Worksheet.Cells[row, startColumn].Value = GetTitleField(master, rowIndex, r => r.SubjectRF_DB, r => r.SubjectRF_DB);
        Worksheet.Cells[row, startColumn + 1].Value = GetTitleField(master, rowIndex, r => r.JurLico_DB, r => r.JurLico_DB);
        Worksheet.Cells[row, startColumn + 2].Value = GetTitleField(master, rowIndex, r => r.ShortJurLico_DB, r => r.ShortJurLico_DB);
        Worksheet.Cells[row, startColumn + 3].Value = GetTitleField(master, rowIndex, r => r.JurLicoAddress_DB, r => r.JurLicoAddress_DB);
        Worksheet.Cells[row, startColumn + 4].Value = GetTitleField(master, rowIndex, r => r.JurLicoFactAddress_DB, r => r.JurLicoFactAddress_DB);
        Worksheet.Cells[row, startColumn + 5].Value = GetTitleField(master, rowIndex, r => r.GradeFIO_DB, r => r.GradeFIO_DB);
        Worksheet.Cells[row, startColumn + 6].Value = GetTitleField(master, rowIndex, r => r.Telephone_DB, r => r.Telephone_DB);
        Worksheet.Cells[row, startColumn + 7].Value = GetTitleField(master, rowIndex, r => r.Fax_DB, r => r.Fax_DB);
        Worksheet.Cells[row, startColumn + 8].Value = GetTitleField(master, rowIndex, r => r.Email_DB, r => r.Email_DB);
        Worksheet.Cells[row, startColumn + 9].Value = GetTitleField(master, rowIndex, r => r.Okpo_DB, r => r.Okpo_DB);
        Worksheet.Cells[row, startColumn + 10].Value = GetTitleField(master, rowIndex, r => r.Okved_DB, r => r.Okved_DB);
        Worksheet.Cells[row, startColumn + 11].Value = GetTitleField(master, rowIndex, r => r.Okogu_DB, r => r.Okogu_DB);
        Worksheet.Cells[row, startColumn + 12].Value = GetTitleField(master, rowIndex, r => r.Oktmo_DB, r => r.Oktmo_DB);
        Worksheet.Cells[row, startColumn + 13].Value = GetTitleField(master, rowIndex, r => r.Inn_DB, r => r.Inn_DB);
        Worksheet.Cells[row, startColumn + 14].Value = GetTitleField(master, rowIndex, r => r.Kpp_DB, r => r.Kpp_DB);
        Worksheet.Cells[row, startColumn + 15].Value = GetTitleField(master, rowIndex, r => r.Okopf_DB, r => r.Okopf_DB);
        Worksheet.Cells[row, startColumn + 16].Value = GetTitleField(master, rowIndex, r => r.Okfs_DB, r => r.Okfs_DB);
    }

    /// <summary>
    /// Записывает счётчики форм по организации в строку Excel.
    /// </summary>
    private void SetFormCounts(int row, int startColumn, OrgExportData org)
    {
        for (var i = 0; i < ExportFormNumbers.Length; i++)
        {
            Worksheet.Cells[row, startColumn + i].Value =
                CountFormReports(org.FormReports, ExportFormNumbers[i]);
        }
    }

    /// <summary>
    /// Суммирует счётчики форм при объединении дубликатов организации.
    /// </summary>
    private void AccumulateFormCounts(int row, int startColumn, OrgExportData org)
    {
        for (var i = 0; i < ExportFormNumbers.Length; i++)
        {
            var column = startColumn + i;
            Worksheet.Cells[row, column].Value = GetCellInt(Worksheet.Cells[row, column].Value)
                                                   + CountFormReports(org.FormReports, ExportFormNumbers[i]);
        }
    }

    private static Form10? GetForm10Row(Report master, int index) =>
        master.Rows10.OrderBy(r => r.NumberInOrder_DB).ElementAtOrDefault(index);

    private static Form20? GetForm20Row(Report master, int index) =>
        master.Rows20.OrderBy(r => r.NumberInOrder_DB).ElementAtOrDefault(index);

    private static string GetOrganUprav(Report master) => master.FormNum_DB switch
    {
        "1.0" => master.Rows10
            .OrderBy(r => r.NumberInOrder_DB)
            .Select(r => r.OrganUprav_DB)
            .FirstOrDefault(v => !string.IsNullOrEmpty(v)) ?? string.Empty,
        "2.0" => master.Rows20
            .OrderBy(r => r.NumberInOrder_DB)
            .Select(r => r.OrganUprav_DB)
            .FirstOrDefault(v => !string.IsNullOrEmpty(v)) ?? string.Empty,
        _ => string.Empty
    };

    private static string GetInn(Report master) => master.FormNum_DB switch
    {
        "1.0" => master.Rows10
            .OrderBy(r => r.NumberInOrder_DB)
            .Select(r => r.Inn_DB)
            .FirstOrDefault(v => !string.IsNullOrEmpty(v)) ?? string.Empty,
        "2.0" => master.Rows20
            .OrderBy(r => r.NumberInOrder_DB)
            .Select(r => r.Inn_DB)
            .FirstOrDefault(v => !string.IsNullOrEmpty(v)) ?? string.Empty,
        _ => string.Empty
    };

    private static string GetJurAddress(Report master) => master.FormNum_DB switch
    {
        "1.0" => GetJurAddress(
            GetForm10Row(master, 1),
            GetForm10Row(master, 0),
            r => r.JurLicoFactAddress_DB,
            r => r.JurLicoAddress_DB),
        "2.0" => GetJurAddress(
            GetForm20Row(master, 1),
            GetForm20Row(master, 0),
            r => r.JurLicoFactAddress_DB,
            r => r.JurLicoAddress_DB),
        _ => string.Empty
    };

    private static string GetJurAddress<T>(
        T? branch,
        T? head,
        Func<T, string?> factAddress,
        Func<T, string?> jurAddress)
    {
        if (branch is not null)
        {
            if (IsValidAddress(factAddress(branch))) return factAddress(branch)!;
            if (IsValidAddress(jurAddress(branch))) return jurAddress(branch)!;
        }

        if (head is not null)
        {
            if (IsValidAddress(factAddress(head))) return factAddress(head)!;
            if (IsValidAddress(jurAddress(head))) return jurAddress(head)!;
            return jurAddress(head) ?? string.Empty;
        }

        return string.Empty;
    }

    private static bool IsValidAddress(string? value) =>
        !string.IsNullOrEmpty(value) && !value.Equals("-");

    private static string GetTitleField(
        Report master,
        int rowIndex,
        Func<Form10, string?> form10Selector,
        Func<Form20, string?> form20Selector) =>
        master.FormNum_DB switch
        {
            "1.0" when GetForm10Row(master, rowIndex) is { } row => form10Selector(row) ?? string.Empty,
            "2.0" when GetForm20Row(master, rowIndex) is { } row => form20Selector(row) ?? string.Empty,
            _ => string.Empty
        };

    /// <summary>
    /// Возвращает целочисленное значение ячейки Excel или 0, если значение отсутствует.
    /// </summary>
    private static int GetCellInt(object? value) => value switch
    {
        int i => i,
        long l => (int)l,
        double d => (int)d,
        float f => (int)f,
        decimal m => (int)m,
        null => 0,
        _ => int.TryParse(value.ToString(), out var n) ? n : 0
    };

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
    /// Подбирает ширину колонок: счётчики форм — фиксированная ширина;
    /// текстовые — AutoFit с ограничением сверху (только Windows; на Linux — ширина по умолчанию) и перенос строк.
    /// </summary>
    private void ApplyColumnWidths()
    {
        var canAutoFit = OperatingSystem.IsWindows();

        for (var col = 1; col <= Worksheet.Dimension.End.Column; col++)
        {
            var header = Worksheet.Cells[1, col].Value?.ToString();
            var column = Worksheet.Column(col);

            if (header?.StartsWith("Форма ", StringComparison.Ordinal) == true)
            {
                column.Width = FormCountColumnWidth;
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
    /// Определяет, является ли колонка текстовой (с переносом длинного содержимого).
    /// </summary>
    private static bool IsTextColumn(string? header) =>
        !string.IsNullOrEmpty(header) && !header.StartsWith("Форма ", StringComparison.Ordinal);

    #endregion

    #region PeriodFilter

    /// <summary>
    /// Проверяет, попадает ли отчёт в заданный пользователем период фильтрации.
    /// </summary>
    private bool MatchesExportPeriodFilter(FormReportCountInfo report) =>
        MatchesExportPeriodFilter(report.FormNum_DB, report.StartPeriod_DB, report.EndPeriod_DB, report.Year_DB);

    /// <summary>
    /// Проверяет, попадает ли отчёт в заданный пользователем период фильтрации.
    /// </summary>
    private bool MatchesExportPeriodFilter(string formNum, string? startPeriod, string? endPeriod, string? year)
    {
        if (string.IsNullOrEmpty(formNum))
            return false;

        if (formNum.StartsWith("1.", StringComparison.Ordinal))
        {
            if (_form1Start == DateOnly.MinValue && _form1End == DateOnly.MaxValue)
                return true;

            if (!DateOnly.TryParse(endPeriod, out var repEnd))
                return false;

            var repStart = DateOnly.TryParse(startPeriod, out var rs) ? rs : DateOnly.MinValue;
            return _form1Start <= repEnd && _form1End >= repStart;
        }

        if (formNum.StartsWith("2.", StringComparison.Ordinal))
        {
            if (_form2YearStart == int.MinValue && _form2YearEnd == int.MaxValue)
                return true;

            return int.TryParse(year, out var reportYear)
                   && reportYear >= _form2YearStart
                   && reportYear <= _form2YearEnd;
        }

        return true;
    }

    /// <summary>
    /// Подсчитывает количество отчётов указанной формы с учётом фильтра периода.
    /// </summary>
    private int CountFormReports(IReadOnlyList<FormReportCountInfo> collection, string formNum) =>
        collection.Count(x => x.FormNum_DB.Equals(formNum) && MatchesExportPeriodFilter(x));

    #endregion

    #region ExportData

    /// <summary>
    /// Данные организации и облегчённый список её отчётов для подсчёта форм.
    /// </summary>
    private sealed class OrgExportData(Reports reps, IReadOnlyList<FormReportCountInfo> formReports)
    {
        /// <summary>
        /// Организация.
        /// </summary>
        public Reports Reps { get; } = reps;

        /// <summary>
        /// Отчёты организации с полями, необходимыми для подсчёта форм.
        /// </summary>
        public IReadOnlyList<FormReportCountInfo> FormReports { get; } = formReports;
    }

    /// <summary>
    /// Облегчённая проекция отчёта с полями, необходимыми для подсчёта форм.
    /// </summary>
    private readonly record struct FormReportCountInfo(
        int OrgId,
        string FormNum_DB,
        string? StartPeriod_DB,
        string? EndPeriod_DB,
        string? Year_DB);

    #endregion
}
