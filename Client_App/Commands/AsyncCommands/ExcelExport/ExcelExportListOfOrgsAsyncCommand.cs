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

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Excel -> Список организаций.
/// </summary>
public class ExcelExportListOfOrgsAsyncCommand : ExcelBaseAsyncCommand
{
    private DateOnly _form1Start = DateOnly.MinValue;
    private DateOnly _form1End = DateOnly.MaxValue;
    private int _form2YearStart = int.MinValue;
    private int _form2YearEnd = int.MaxValue;

    private bool MatchesExportPeriodFilter(Report report)
    {
        if (string.IsNullOrEmpty(report.FormNum_DB))
            return false;

        if (report.FormNum_DB.StartsWith("1.", StringComparison.Ordinal))
        {
            if (_form1Start == DateOnly.MinValue && _form1End == DateOnly.MaxValue)
                return true;

            if (!DateOnly.TryParse(report.EndPeriod_DB, out var repEnd))
                return false;

            var repStart = DateOnly.TryParse(report.StartPeriod_DB, out var rs) ? rs : DateOnly.MinValue;
            return _form1Start <= repEnd && _form1End >= repStart;
        }

        if (report.FormNum_DB.StartsWith("2.", StringComparison.Ordinal))
        {
            if (_form2YearStart == int.MinValue && _form2YearEnd == int.MaxValue)
                return true;

            return int.TryParse(report.Year_DB, out var year)
                   && year >= _form2YearStart
                   && year <= _form2YearEnd;
        }

        return true;
    }

    private int CountFormReports(IEnumerable<Report> collection, string formNum) =>
        collection.Count(x => x.FormNum_DB.Equals(formNum) && MatchesExportPeriodFilter(x));

    private static readonly string[] ExportFormNumbers =
    [
        "1.1", "1.2", "1.3", "1.4", "1.5", "1.6", "1.7", "1.8", "1.9",
        "2.1", "2.2", "2.3", "2.4", "2.5", "2.6", "2.7", "2.8", "2.9", "2.10", "2.11", "2.12"
    ];

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
        var isFullExport = parameter?.ToString() == "full";
        var formCountsStartColumn = isFullExport ? 42 : 8;
        var checkedLst = new List<Reports>();
        var row = 2;
        double progressBarDoubleValue = progressBarVM.ValueBar;

        foreach (var reps in repsList
                     .Where(reps => reps.Master.FormNum_DB is "1.0" or "2.0")
                     .OrderBy(x => x.Master_DB.RegNoRep?.Value)
                     .ThenBy(x => x.Master_DB.OkpoRep?.Value))
        {
            var isDuplicate = checkedLst.Any(x => x.Master_DB.RegNoRep == reps.Master_DB.RegNoRep
                                                  && x.Master_DB.OkpoRep == reps.Master_DB.OkpoRep);

            if (isDuplicate)
            {
                row--;
                AccumulateFormCounts(row, formCountsStartColumn, reps);
                row++;
            }
            else
            {
                WriteOrgRow(row, reps, isFullExport);
                row++;
                checkedLst.Add(reps);
            }

            progressBarDoubleValue += (double)65 / repsList.Count;
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
        Worksheet.Cells[Worksheet.Dimension.Address].AutoFilter = true;
        Worksheet.View.FreezePanes(2, 1);

        return Task.CompletedTask;
    }

    private void WriteOrgRow(int row, Reports reps, bool isFullExport)
    {
        var master = reps.Master;
        var regNo = master.RegNoRep.Value;

        Worksheet.Cells[row, 1].Value = regNo;
        Worksheet.Cells[row, 2].Value = regNo.Length >= 2 ? regNo[..2] : regNo;
        Worksheet.Cells[row, 3].Value = GetOrganUprav(master);
        Worksheet.Cells[row, 4].Value = master.OkpoRep.Value;
        Worksheet.Cells[row, 5].Value = master.ShortJurLicoRep.Value;
        Worksheet.Cells[row, 6].Value = GetJurAddress(master);
        Worksheet.Cells[row, 7].Value = GetInn(master);

        if (isFullExport)
        {
            WriteTitleRowFields(row, master, startColumn: 8, rowIndex: 0);
            WriteTitleRowFields(row, master, startColumn: 25, rowIndex: 1);
        }

        SetFormCounts(row, isFullExport ? 42 : 8, reps);
    }

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

    private void SetFormCounts(int row, int startColumn, Reports reps)
    {
        for (var i = 0; i < ExportFormNumbers.Length; i++)
        {
            Worksheet.Cells[row, startColumn + i].Value =
                CountFormReports(reps.Report_Collection, ExportFormNumbers[i]);
        }
    }

    private void AccumulateFormCounts(int row, int startColumn, Reports reps)
    {
        for (var i = 0; i < ExportFormNumbers.Length; i++)
        {
            var column = startColumn + i;
            Worksheet.Cells[row, column].Value = (int)Worksheet.Cells[row, column].Value
                                                   + CountFormReports(reps.Report_Collection, ExportFormNumbers[i]);
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
        Worksheet.Cells[Worksheet.Dimension.Address].AutoFilter = true;

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
                    ContentTitle = "Выгрузка в .xlsx",
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
}