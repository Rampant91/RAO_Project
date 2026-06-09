using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;

public abstract partial class ExcelExportListOfFormsBaseAsyncCommand
{
  private protected enum FormListExportGroup
  {
    Forms1,
    Forms2,
    Forms4,
    Forms5
  }

  private protected readonly record struct FormListExportFilters(
    DateOnly Form1StartDate,
    DateOnly Form1EndDate,
    int MinYear,
    int MaxYear);

  /// <summary>
  /// Диапазоны прогрессбара для одного листа списка форм.
  /// </summary>
  private readonly record struct FormListSheetProgressSegment(
    int LoadStart,
    int LoadEnd,
    int RowCountsStart,
    int RowCountsEnd,
    int ExcelEnd);

  private static readonly FormListExportGroup[] AllFormListExportGroups =
  [
    FormListExportGroup.Forms1,
    FormListExportGroup.Forms2,
    FormListExportGroup.Forms4,
    FormListExportGroup.Forms5
  ];

  #region Orchestration

  /// <summary>
  /// Выгрузка одного списка форм в отдельный файл .xlsx.
  /// </summary>
  private protected async Task ExportSingleFormListAsync(FormListExportGroup group)
  {
    var cts = new CancellationTokenSource();
    ExportType = GetExportTypeName(group);
    var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
    var progressBarVM = progressBar.AnyTaskProgressBarVM;

    progressBarVM.SetProgressBar(2, "Проверка параметров", "Выгрузка в .xlsx", ExportType);
    var folderPath = await CheckAppParameter();
    var isBackgroundCommand = folderPath != string.Empty;

    progressBarVM.SetProgressBar(5, "Запрос периода");
    var filters = await PromptFiltersAsync(group, isBackgroundCommand, progressBar, cts);

    progressBarVM.SetProgressBar(8, "Создание временной БД");
    var tmpDbPath = await CreateTempDataBase(progressBar, cts);
    await using var db = new DBModel(tmpDbPath);

    progressBarVM.SetProgressBar(10, GetReportsCountCheckStatus(group));
    await ReportsCountCheck(db, GetMasterFormNum(group), progressBar, cts);

    progressBarVM.SetProgressBar(13, "Запрос пути сохранения", "Выгрузка в .xlsx", ExportType);
    var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
    var (fullPath, openTemp) = !isBackgroundCommand
      ? await ExcelGetFullPath(fileName, cts, progressBar)
      : (Path.Combine(folderPath, $"{fileName}.xlsx"), true);

    fullPath = ResolveUniqueFilePath(fullPath, isBackgroundCommand ? folderPath : null);

    progressBarVM.SetProgressBar(15, "Инициализация Excel пакета");
    using var excelPackage = await InitializeExcelPackage(fullPath);

    progressBarVM.SetProgressBar(18, "Заполнение листа");
    var worksheet = excelPackage.Workbook.Worksheets.Add(GetSheetName(group));
    WriteFormListHeaders(worksheet, group);

    await FillFormListSheetAsync(
      db,
      worksheet,
      group,
      filters,
      progressBarVM,
      CreateSingleFormProgressSegment(group),
      cts);

    progressBarVM.SetProgressBar(ProgressSaveStart, "Сохранение");
    await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar, isBackgroundCommand);

    await CleanupTempDbAsync(tmpDbPath, progressBarVM, progressBar);
  }

  /// <summary>
  /// Выгрузка списков форм 1, 2, 4 и 5 на отдельные листы одного файла .xlsx.
  /// </summary>
  private protected async Task ExportAllFormListsAsync()
  {
    var cts = new CancellationTokenSource();
    ExportType = "Список_форм_все";
    var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
    var progressBarVM = progressBar.AnyTaskProgressBarVM;

    progressBarVM.SetProgressBar(2, "Проверка параметров", "Выгрузка в .xlsx", ExportType);
    var folderPath = await CheckAppParameter();
    var isBackgroundCommand = folderPath != string.Empty;

    progressBarVM.SetProgressBar(5, "Запрос периодов");
    var filters = await PromptAllFormListFiltersAsync(isBackgroundCommand, progressBar, cts);

    progressBarVM.SetProgressBar(8, "Создание временной БД");
    var tmpDbPath = await CreateTempDataBase(progressBar, cts);
    await using var db = new DBModel(tmpDbPath);

    progressBarVM.SetProgressBar(13, "Запрос пути сохранения", "Выгрузка в .xlsx", ExportType);
    var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
    var (fullPath, openTemp) = !isBackgroundCommand
      ? await ExcelGetFullPath(fileName, cts, progressBar)
      : (Path.Combine(folderPath, $"{fileName}.xlsx"), true);

    fullPath = ResolveUniqueFilePath(fullPath, isBackgroundCommand ? folderPath : null);

    progressBarVM.SetProgressBar(15, "Инициализация Excel пакета");
    using var excelPackage = await InitializeExcelPackage(fullPath);

    const int sheetProgressStart = 18;
    const int sheetProgressEnd = 91;
    var sheetProgressStep = (sheetProgressEnd - sheetProgressStart) / AllFormListExportGroups.Length;

    for (var i = 0; i < AllFormListExportGroups.Length; i++)
    {
      var group = AllFormListExportGroups[i];
      var sheetProgressFrom = sheetProgressStart + sheetProgressStep * i;
      var sheetProgressTo = i == AllFormListExportGroups.Length - 1
        ? sheetProgressEnd
        : sheetProgressStart + sheetProgressStep * (i + 1);

      progressBarVM.SetProgressBar(sheetProgressFrom, $"Лист: {GetSheetName(group)}");

      var worksheet = excelPackage.Workbook.Worksheets.Add(GetSheetName(group));
      WriteFormListHeaders(worksheet, group);

      if (await HasReportsForMasterFormAsync(db, GetMasterFormNum(group), cts.Token))
      {
        await FillFormListSheetAsync(
          db,
          worksheet,
          group,
          filters,
          progressBarVM,
          CreateAllFormsSheetProgressSegment(sheetProgressFrom, sheetProgressTo),
          cts);
      }
      else
      {
        ApplyFormListSheetStyle(worksheet, group);
      }
    }

    progressBarVM.SetProgressBar(ProgressSaveStart, "Сохранение");
    await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar, isBackgroundCommand);

    await CleanupTempDbAsync(tmpDbPath, progressBarVM, progressBar);
  }

  private static async Task CleanupTempDbAsync(
    string tmpDbPath,
    AnyTaskProgressBarVM progressBarVM,
    AnyTaskProgressBar progressBar)
  {
    progressBarVM.SetProgressBar(ProgressCleanup, "Очистка временных данных");
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

  #endregion

  #region SheetFill

  private static async Task FillFormListSheetAsync(
    DBModel db,
    ExcelWorksheet worksheet,
    FormListExportGroup group,
    FormListExportFilters filters,
    AnyTaskProgressBarVM progressBarVM,
    FormListSheetProgressSegment progress,
    CancellationTokenSource cts)
  {
    var orgsList = await GetReportsList(
      db, GetMasterFormNum(group), progressBarVM, cts, progress.LoadStart, progress.LoadEnd);
    var prepared = PrepareFormListExport(group, orgsList, filters);

    switch (group)
    {
      case FormListExportGroup.Forms1:
      {
        var rowCounts = await LoadForm1RowCountsAsync(
          db,
          prepared.ReportIdsByForm,
          progressBarVM,
          progress.RowCountsStart,
          progress.RowCountsEnd,
          cts.Token);
        FillForm1SheetData(
          prepared.Orgs,
          rowCounts,
          worksheet,
          progressBarVM,
          progress.RowCountsEnd,
          progress.ExcelEnd);
        break;
      }
      default:
      {
        var rowCounts = await LoadSimpleFormRowCountsAsync(
          db,
          group,
          prepared.ReportIdsByForm,
          progressBarVM,
          progress.RowCountsStart,
          progress.RowCountsEnd,
          cts.Token);
        FillSimpleFormSheetData(
          prepared.Orgs,
          rowCounts,
          worksheet,
          group,
          progressBarVM,
          progress.RowCountsEnd,
          progress.ExcelEnd);
        break;
      }
    }

    ApplyFormListSheetStyle(worksheet, group);
  }

  private static FormListExportPreparedData PrepareFormListExport(
    FormListExportGroup group,
    List<FormListOrgExportData> orgsList,
    FormListExportFilters filters) =>
    group switch
    {
      FormListExportGroup.Forms1 => PrepareForm1Export(orgsList, filters.Form1StartDate, filters.Form1EndDate),
      FormListExportGroup.Forms2 => PrepareForm2Export(orgsList, filters.MinYear, filters.MaxYear),
      FormListExportGroup.Forms4 => PrepareForm4Export(orgsList, filters.MinYear, filters.MaxYear),
      FormListExportGroup.Forms5 => PrepareForm5Export(orgsList, filters.MinYear, filters.MaxYear),
      _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
    };

  private static void ApplyFormListSheetStyle(ExcelWorksheet worksheet, FormListExportGroup group)
  {
    ApplyFormListExcelStyle(worksheet, GetHeaderRowHeight(group));

    switch (group)
    {
      case FormListExportGroup.Forms4:
        ApplyForm4ListColumnWidths(worksheet);
        break;
      case FormListExportGroup.Forms5:
        ApplyForm5ListColumnWidths(worksheet);
        break;
    }
  }

  #endregion

  #region Headers

  private static void WriteFormListHeaders(ExcelWorksheet worksheet, FormListExportGroup group)
  {
    switch (group)
    {
      case FormListExportGroup.Forms1:
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "ОКПО";
        worksheet.Cells[1, 3].Value = "Сокращенное наименование";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала периода" + HeaderFilterLineBreak;
        worksheet.Cells[1, 6].Value = "Дата конца периода" + HeaderFilterLineBreak;
        worksheet.Cells[1, 7].Value = "Номер корректировки" + HeaderFilterLineBreak;
        worksheet.Cells[1, 8].Value = "Количество строк" + HeaderFilterLineBreak;
        worksheet.Cells[1, 9].Value = "Инвентаризация";
        break;

      case FormListExportGroup.Forms2:
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "ОКПО";
        worksheet.Cells[1, 3].Value = "Сокращенное наименование";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Отчетный год";
        worksheet.Cells[1, 6].Value = "Номер корректировки" + HeaderFilterLineBreak;
        worksheet.Cells[1, 7].Value = "Количество строк" + HeaderFilterLineBreak;
        break;

      case FormListExportGroup.Forms4:
        worksheet.Cells[1, 1].Value = "Код субъекта РФ";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование" + HeaderFilterLineBreak + "РИАЦ";
        worksheet.Cells[1, 3].Value = "Номер формы" + HeaderFilterLineBreak;
        worksheet.Cells[1, 4].Value = "Год";
        worksheet.Cells[1, 5].Value = "Номер корректировки" + HeaderFilterLineBreak;
        worksheet.Cells[1, 6].Value = "Количество строк" + HeaderFilterLineBreak;
        break;

      case FormListExportGroup.Forms5:
        worksheet.Cells[1, 1].Value = "Сокращенное наименование" + HeaderFilterLineBreak + "ВИАЦ";
        worksheet.Cells[1, 2].Value = "Номер формы" + HeaderFilterLineBreak;
        worksheet.Cells[1, 3].Value = "Год";
        worksheet.Cells[1, 4].Value = "Номер корректировки" + HeaderFilterLineBreak;
        worksheet.Cells[1, 5].Value = "Количество строк" + HeaderFilterLineBreak;
        break;

      default:
        throw new ArgumentOutOfRangeException(nameof(group), group, null);
    }
  }

  #endregion

  #region Form1SheetData

  private static void FillForm1SheetData(
    List<FormListOrgExportData> orgsList,
    Dictionary<int, (int RowCount, int Code10Count)> rowCounts,
    ExcelWorksheet worksheet,
    AnyTaskProgressBarVM progressBarVM,
    int progressExcelStart,
    int progressExcelEnd)
  {
    var totalRows = CountExportRows(orgsList);
    var excelFillRange = progressExcelEnd - progressExcelStart;
    double progressValue = progressExcelStart;
    var rowsWritten = 0;

    progressBarVM.SetProgressBar(progressExcelStart, "Запись в Excel");

    const int firstDataRow = 2;
    var row = firstDataRow;
    foreach (var org in orgsList
               .OrderBy(x => x.RegNo)
               .ThenBy(x => x.Okpo))
    {
      foreach (var rep in OrderForm1Reports(org.Reports))
      {
        rowCounts.TryGetValue(rep.ReportId, out var counts);
        worksheet.Cells[row, 1].Value = org.RegNo;
        worksheet.Cells[row, 2].Value = org.Okpo;
        worksheet.Cells[row, 3].Value = org.ShortJurLico;
        worksheet.Cells[row, 4].Value = rep.FormNum_DB;
        worksheet.Cells[row, 5].Value = ConvertToExcelDate(rep.StartPeriod_DB, worksheet, row, 5);
        worksheet.Cells[row, 6].Value = ConvertToExcelDate(rep.EndPeriod_DB, worksheet, row, 6);
        worksheet.Cells[row, 7].Value = rep.CorrectionNumber_DB;
        worksheet.Cells[row, 8].Value = counts.RowCount;
        worksheet.Cells[row, 9].Value = InventoryCheck(repRowsCount: counts.RowCount, countCode10: counts.Code10Count).TrimStart();
        row++;

        UpdateFillProgress(progressBarVM, ref progressValue, ref rowsWritten, totalRows, excelFillRange, progressExcelEnd,
          $"Запись в Excel: {org.RegNo}_{org.Okpo}");
      }
    }

    if (row > firstDataRow)
      ApplyAlternatingRowColors(worksheet, firstDataRow, row - 1);
  }

  private static async Task<Dictionary<int, (int RowCount, int Code10Count)>> LoadForm1RowCountsAsync(
    DBModel db,
    IReadOnlyDictionary<string, List<int>> reportIdsByForm,
    AnyTaskProgressBarVM progressBarVM,
    int progressRowCountsStart,
    int progressRowCountsEnd,
    CancellationToken token)
  {
    var result = new Dictionary<int, (int RowCount, int Code10Count)>();
    var formCount = Form1ChildFormNumbers.Length;

    for (var formIndex = 0; formIndex < formCount; formIndex++)
    {
      var formNum = Form1ChildFormNumbers[formIndex];
      SetRowCountsProgress(
        progressBarVM, formIndex + 1, formCount, progressRowCountsStart, progressRowCountsEnd, formNum);

      if (!reportIdsByForm.TryGetValue(formNum, out var reportIds) || reportIds.Count == 0)
        continue;

      switch (formNum)
      {
        case "1.1": await AppendForm1RowCountsAsync(db.form_11, reportIds, result, token); break;
        case "1.2": await AppendForm1RowCountsAsync(db.form_12, reportIds, result, token); break;
        case "1.3": await AppendForm1RowCountsAsync(db.form_13, reportIds, result, token); break;
        case "1.4": await AppendForm1RowCountsAsync(db.form_14, reportIds, result, token); break;
        case "1.5": await AppendForm1RowCountsAsync(db.form_15, reportIds, result, token); break;
        case "1.6": await AppendForm1RowCountsAsync(db.form_16, reportIds, result, token); break;
        case "1.7": await AppendForm1RowCountsAsync(db.form_17, reportIds, result, token); break;
        case "1.8": await AppendForm1RowCountsAsync(db.form_18, reportIds, result, token); break;
        case "1.9": await AppendForm1RowCountsAsync(db.form_19, reportIds, result, token); break;
      }
    }

    return result;
  }

  #endregion

  #region SimpleFormSheetData

  private static void FillSimpleFormSheetData(
    List<FormListOrgExportData> orgsList,
    Dictionary<int, int> rowCounts,
    ExcelWorksheet worksheet,
    FormListExportGroup group,
    AnyTaskProgressBarVM progressBarVM,
    int progressExcelStart,
    int progressExcelEnd)
  {
    var totalRows = CountExportRows(orgsList);
    var excelFillRange = progressExcelEnd - progressExcelStart;
    double progressValue = progressExcelStart;
    var rowsWritten = 0;

    progressBarVM.SetProgressBar(progressExcelStart, "Запись в Excel");

    const int firstDataRow = 2;
    var row = firstDataRow;

    foreach (var org in OrderOrgsForGroup(orgsList, group))
    {
      foreach (var rep in OrderReportsForGroup(org.Reports, group))
      {
        rowCounts.TryGetValue(rep.ReportId, out var rowCount);
        WriteSimpleFormRow(worksheet, row, org, rep, rowCount, group);
        row++;

        UpdateFillProgress(progressBarVM, ref progressValue, ref rowsWritten, totalRows, excelFillRange, progressExcelEnd,
          GetFillProgressStatus(org, group));
      }
    }

    if (row > firstDataRow)
      ApplyAlternatingRowColors(worksheet, firstDataRow, row - 1);
  }

  private static IEnumerable<FormListOrgExportData> OrderOrgsForGroup(
    List<FormListOrgExportData> orgsList,
    FormListExportGroup group) =>
    group switch
    {
      FormListExportGroup.Forms5 => orgsList.OrderBy(x => x.ShortJurLico),
      FormListExportGroup.Forms4 => orgsList.OrderBy(x => x.RegNo),
      _ => orgsList.OrderBy(x => x.RegNo).ThenBy(x => x.Okpo)
    };

  private static IEnumerable<FormReportListInfo> OrderReportsForGroup(
    IReadOnlyList<FormReportListInfo> reports,
    FormListExportGroup group) =>
    group switch
    {
      FormListExportGroup.Forms2 => OrderForm2Reports(reports),
      FormListExportGroup.Forms4 => OrderForm4Reports(reports),
      FormListExportGroup.Forms5 => OrderForm5Reports(reports),
      _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
    };

  private static void WriteSimpleFormRow(
    ExcelWorksheet worksheet,
    int row,
    FormListOrgExportData org,
    FormReportListInfo rep,
    int rowCount,
    FormListExportGroup group)
  {
    switch (group)
    {
      case FormListExportGroup.Forms2:
        worksheet.Cells[row, 1].Value = org.RegNo;
        worksheet.Cells[row, 2].Value = org.Okpo;
        worksheet.Cells[row, 3].Value = org.ShortJurLico;
        worksheet.Cells[row, 4].Value = rep.FormNum_DB;
        worksheet.Cells[row, 5].Value = rep.Year_DB;
        worksheet.Cells[row, 6].Value = rep.CorrectionNumber_DB;
        worksheet.Cells[row, 7].Value = rowCount;
        break;

      case FormListExportGroup.Forms4:
        worksheet.Cells[row, 1].Value = org.RegNo;
        worksheet.Cells[row, 2].Value = org.ShortJurLico;
        worksheet.Cells[row, 3].Value = rep.FormNum_DB;
        worksheet.Cells[row, 4].Value = rep.Year_DB;
        worksheet.Cells[row, 5].Value = rep.CorrectionNumber_DB;
        worksheet.Cells[row, 6].Value = rowCount;
        break;

      case FormListExportGroup.Forms5:
        worksheet.Cells[row, 1].Value = org.ShortJurLico;
        worksheet.Cells[row, 2].Value = rep.FormNum_DB;
        worksheet.Cells[row, 3].Value = rep.Year_DB;
        worksheet.Cells[row, 4].Value = rep.CorrectionNumber_DB;
        worksheet.Cells[row, 5].Value = rowCount;
        break;

      default:
        throw new ArgumentOutOfRangeException(nameof(group), group, null);
    }
  }

  private static string GetFillProgressStatus(FormListOrgExportData org, FormListExportGroup group) =>
    group switch
    {
      FormListExportGroup.Forms4 => $"Запись в Excel: {org.RegNo}",
      FormListExportGroup.Forms5 => $"Запись в Excel: {org.ShortJurLico}",
      _ => $"Запись в Excel: {org.RegNo}_{org.Okpo}"
    };

  private static async Task<Dictionary<int, int>> LoadSimpleFormRowCountsAsync(
    DBModel db,
    FormListExportGroup group,
    IReadOnlyDictionary<string, List<int>> reportIdsByForm,
    AnyTaskProgressBarVM progressBarVM,
    int progressRowCountsStart,
    int progressRowCountsEnd,
    CancellationToken token)
  {
    var result = new Dictionary<int, int>();

    switch (group)
    {
      case FormListExportGroup.Forms2:
        await LoadForm2RowCountsIntoAsync(
          db, reportIdsByForm, progressBarVM, progressRowCountsStart, progressRowCountsEnd, result, token);
        break;
      case FormListExportGroup.Forms4:
        await LoadForm4RowCountsIntoAsync(
          db, reportIdsByForm, progressBarVM, progressRowCountsStart, progressRowCountsEnd, result, token);
        break;
      case FormListExportGroup.Forms5:
        await LoadForm5RowCountsIntoAsync(
          db, reportIdsByForm, progressBarVM, progressRowCountsStart, progressRowCountsEnd, result, token);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(group), group, null);
    }

    return result;
  }

  private static async Task LoadForm2RowCountsIntoAsync(
    DBModel db,
    IReadOnlyDictionary<string, List<int>> reportIdsByForm,
    AnyTaskProgressBarVM progressBarVM,
    int progressRowCountsStart,
    int progressRowCountsEnd,
    Dictionary<int, int> result,
    CancellationToken token)
  {
    var formCount = Form2ChildFormNumbers.Length;
    for (var formIndex = 0; formIndex < formCount; formIndex++)
    {
      var formNum = Form2ChildFormNumbers[formIndex];
      SetRowCountsProgress(
        progressBarVM, formIndex + 1, formCount, progressRowCountsStart, progressRowCountsEnd, formNum);

      if (!reportIdsByForm.TryGetValue(formNum, out var reportIds) || reportIds.Count == 0)
        continue;

      switch (formNum)
      {
        case "2.1": await AppendForm2RowCountsAsync(db.form_21, reportIds, result, token); break;
        case "2.2": await AppendForm2RowCountsAsync(db.form_22, reportIds, result, token); break;
        case "2.3": await AppendForm2RowCountsAsync(db.form_23, reportIds, result, token); break;
        case "2.4": await AppendForm2RowCountsAsync(db.form_24, reportIds, result, token); break;
        case "2.5": await AppendForm2RowCountsAsync(db.form_25, reportIds, result, token); break;
        case "2.6": await AppendForm2RowCountsAsync(db.form_26, reportIds, result, token); break;
        case "2.7": await AppendForm2RowCountsAsync(db.form_27, reportIds, result, token); break;
        case "2.8": await AppendForm2RowCountsAsync(db.form_28, reportIds, result, token); break;
        case "2.9": await AppendForm2RowCountsAsync(db.form_29, reportIds, result, token); break;
        case "2.10": await AppendForm2RowCountsAsync(db.form_210, reportIds, result, token); break;
        case "2.11": await AppendForm2RowCountsAsync(db.form_211, reportIds, result, token); break;
        case "2.12": await AppendForm2RowCountsAsync(db.form_212, reportIds, result, token); break;
      }
    }
  }

  private static async Task LoadForm4RowCountsIntoAsync(
    DBModel db,
    IReadOnlyDictionary<string, List<int>> reportIdsByForm,
    AnyTaskProgressBarVM progressBarVM,
    int progressRowCountsStart,
    int progressRowCountsEnd,
    Dictionary<int, int> result,
    CancellationToken token)
  {
    const string formNum = "4.1";
    SetRowCountsProgress(progressBarVM, 1, 1, progressRowCountsStart, progressRowCountsEnd, formNum);

    if (reportIdsByForm.TryGetValue(formNum, out var reportIds) && reportIds.Count > 0)
      await AppendFormRowCountsAsync(db.form_41, reportIds, result, token);
  }

  private static async Task LoadForm5RowCountsIntoAsync(
    DBModel db,
    IReadOnlyDictionary<string, List<int>> reportIdsByForm,
    AnyTaskProgressBarVM progressBarVM,
    int progressRowCountsStart,
    int progressRowCountsEnd,
    Dictionary<int, int> result,
    CancellationToken token)
  {
    var formCount = Form5ChildFormNumbers.Length;
    for (var formIndex = 0; formIndex < formCount; formIndex++)
    {
      var formNum = Form5ChildFormNumbers[formIndex];
      SetRowCountsProgress(
        progressBarVM, formIndex + 1, formCount, progressRowCountsStart, progressRowCountsEnd, formNum);

      if (!reportIdsByForm.TryGetValue(formNum, out var reportIds) || reportIds.Count == 0)
        continue;

      switch (formNum)
      {
        case "5.1": await AppendFormRowCountsAsync(db.form_51, reportIds, result, token); break;
        case "5.2": await AppendFormRowCountsAsync(db.form_52, reportIds, result, token); break;
        case "5.3": await AppendFormRowCountsAsync(db.form_53, reportIds, result, token); break;
        case "5.4": await AppendFormRowCountsAsync(db.form_54, reportIds, result, token); break;
        case "5.5": await AppendFormRowCountsAsync(db.form_55, reportIds, result, token); break;
        case "5.6": await AppendFormRowCountsAsync(db.form_56, reportIds, result, token); break;
        case "5.7": await AppendFormRowCountsAsync(db.form_57, reportIds, result, token); break;
      }
    }
  }

  private static void UpdateFillProgress(
    AnyTaskProgressBarVM progressBarVM,
    ref double progressValue,
    ref int rowsWritten,
    int totalRows,
    int excelFillRange,
    int progressExcelEnd,
    string status)
  {
    if (totalRows <= 0)
      return;

    progressValue += (double)excelFillRange / totalRows;
    rowsWritten++;
    if (rowsWritten % ProgressUpdateRowInterval == 0 || rowsWritten == totalRows)
    {
      progressBarVM.SetProgressBar(
        Math.Min(progressExcelEnd, (int)Math.Floor(progressValue)),
        status);
    }
  }

  #endregion

  #region FiltersAndMetadata

  private static async Task<FormListExportFilters> PromptFiltersAsync(
    FormListExportGroup group,
    bool isBackgroundCommand,
    AnyTaskProgressBar progressBar,
    CancellationTokenSource cts)
  {
    if (isBackgroundCommand)
      return new FormListExportFilters(DateOnly.MinValue, DateOnly.MaxValue, 0, 9999);

    return group switch
    {
      FormListExportGroup.Forms1 => await PromptForm1DateRangeAsync(progressBar, cts),
      FormListExportGroup.Forms2 or FormListExportGroup.Forms4 or FormListExportGroup.Forms5 =>
        await PromptYearRangeAsync(progressBar, cts),
      _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
    };
  }

  private static async Task<FormListExportFilters> PromptAllFormListFiltersAsync(
    bool isBackgroundCommand,
    AnyTaskProgressBar progressBar,
    CancellationTokenSource cts)
  {
    if (isBackgroundCommand)
      return new FormListExportFilters(DateOnly.MinValue, DateOnly.MaxValue, 0, 9999);

    var form1Dates = await PromptForm1DateRangeAsync(progressBar, cts);
    var yearRange = await PromptYearRangeAsync(progressBar, cts);
    return form1Dates with { MinYear = yearRange.MinYear, MaxYear = yearRange.MaxYear };
  }

  private static async Task<FormListExportFilters> PromptForm1DateRangeAsync(
    AnyTaskProgressBar progressBar,
    CancellationTokenSource cts)
  {
    var res = await Dispatcher.UIThread.InvokeAsync(() =>
    {
      var window = new AskDatePeriodMessageWindow();
      return window.ShowDialog<(string command, DateOnly initialDate, DateOnly residualDate)>(Desktop.MainWindow);
    });

    if (res.command is not "Ок")
      await CancelCommandAndCloseProgressBarWindow(cts, progressBar);

    return new FormListExportFilters(res.initialDate, res.residualDate, 0, 9999);
  }

  private static async Task<FormListExportFilters> PromptYearRangeAsync(
    AnyTaskProgressBar? progressBar,
    CancellationTokenSource cts)
  {
    var res = await Dispatcher.UIThread.InvokeAsync(() =>
    {
      var window = new AskYearPeriodMessageWindow();
      return window.ShowDialog<(string command, int initialYear, int residualYear)>(Desktop.MainWindow);
    });

    if (res.command is not "Ок")
      await CancelCommandAndCloseProgressBarWindow(cts, progressBar);

    return new FormListExportFilters(DateOnly.MinValue, DateOnly.MaxValue, res.initialYear, res.residualYear);
  }

  private static async Task<bool> HasReportsForMasterFormAsync(
    DBModel db,
    string masterFormNum,
    CancellationToken token) =>
    await db.ReportsCollectionDbSet
      .AsNoTracking()
      .AnyAsync(x => x.DBObservableId != null && x.Master_DB.FormNum_DB == masterFormNum, token);

  private static string GetExportTypeName(FormListExportGroup group) => group switch
  {
    FormListExportGroup.Forms1 => "Список_форм_1",
    FormListExportGroup.Forms2 => "Список_форм_2",
    FormListExportGroup.Forms4 => "Список_форм_4",
    FormListExportGroup.Forms5 => "Список_форм_5",
    _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
  };

  private static string GetMasterFormNum(FormListExportGroup group) => group switch
  {
    FormListExportGroup.Forms1 => "1.0",
    FormListExportGroup.Forms2 => "2.0",
    FormListExportGroup.Forms4 => "4.0",
    FormListExportGroup.Forms5 => "5.0",
    _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
  };

  private static string GetSheetName(FormListExportGroup group) => group switch
  {
    FormListExportGroup.Forms1 => "Список всех форм 1",
    FormListExportGroup.Forms2 => "Список всех форм 2",
    FormListExportGroup.Forms4 => "Список всех форм 4",
    FormListExportGroup.Forms5 => "Список всех форм 5",
    _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
  };

  private static int GetProgressRowCountsEnd(FormListExportGroup group) => group switch
  {
    FormListExportGroup.Forms1 => ProgressRowCountsEndForm1,
    FormListExportGroup.Forms2 => ProgressRowCountsEndForm2,
    FormListExportGroup.Forms4 => ProgressRowCountsEndForm4,
    FormListExportGroup.Forms5 => ProgressRowCountsEndForm5,
    _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
  };

  private static FormListSheetProgressSegment CreateSingleFormProgressSegment(FormListExportGroup group) =>
    new(
      ProgressDbLoadStart,
      ProgressReportsListEnd,
      ProgressRowCountsStart,
      GetProgressRowCountsEnd(group),
      ProgressExcelFillEnd);

  private static FormListSheetProgressSegment CreateAllFormsSheetProgressSegment(int from, int to)
  {
    var range = Math.Max(1, to - from);
    var loadEnd = from + Math.Max(1, range / 8);
    var rowCountsEnd = from + range * 3 / 4;
    if (rowCountsEnd <= loadEnd)
      rowCountsEnd = loadEnd + 1;
    if (rowCountsEnd >= to)
      rowCountsEnd = to - 1;

    return new FormListSheetProgressSegment(from, loadEnd, loadEnd, rowCountsEnd, to);
  }

  private static double GetHeaderRowHeight(FormListExportGroup group) => group switch
  {
    FormListExportGroup.Forms1 => HeaderRowHeightForm1,
    FormListExportGroup.Forms2 => HeaderRowHeightForm2,
    FormListExportGroup.Forms4 => HeaderRowHeightForm4,
    FormListExportGroup.Forms5 => HeaderRowHeightForm5,
    _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
  };

  private static string GetReportsCountCheckStatus(FormListExportGroup group) => group switch
  {
    FormListExportGroup.Forms1 or FormListExportGroup.Forms2 => "Подсчёт количества организаций",
    FormListExportGroup.Forms4 => "Подсчёт количества РИАЦ",
    FormListExportGroup.Forms5 => "Подсчёт количества ВИАЦ",
    _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
  };

  #endregion
}
