using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources.CustomComparers.SnkComparers;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Выгрузка в .xlsx операций с кодом 41 без парной записи между формами 1.1 и 1.5.
/// </summary>
public class ExcelExportCheckPairingOfCode41AsyncCommand : ExcelExportBaseAllAsyncCommand
{
    private const string OperationCode = "41";
    private static readonly Operation41PairingKeyComparer PairingKeyComparer = new();
    private readonly MainWindowVM _mainWindowVM;

    public ExcelExportCheckPairingOfCode41AsyncCommand(MainWindowVM mainWindowVM)
    {
        _mainWindowVM = mainWindowVM;
        mainWindowVM.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(MainWindowVM.SelectedReports))
            {
                OnCanExecuteChanged();
            }
        };
    }

    public override bool CanExecute(object? parameter) => _mainWindowVM.SelectedReports is not null;

    public override async Task AsyncExecute(object? parameter)
    {
        if (!TryGetReports(parameter, out var selectedReports))
        {
            return;
        }

        var cts = new CancellationTokenSource();
        ExportType = "Непарные_операции_41";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        var regNum = RemoveForbiddenChars(selectedReports.Master_DB.RegNoRep.Value);
        var okpo = RemoveForbiddenChars(selectedReports.Master_DB.OkpoRep.Value);
        var fileName = $"{regNum}_{okpo}_не_парные_операции_41";

        progressBarVM.SetProgressBar(5, "Запрос пути сохранения", ExportType, "Выгрузка в .xlsx");
        var (fullPath, openTemp) = await ExcelGetFullPathWithUniqueIndex(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(10, "Создание временной БД", ExportType, "Выгрузка в .xlsx");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(20, "Загрузка операций по форме 1.1");
        var form11Operations = await LoadOperation41ListAsync(db, selectedReports.Id, "1.1", cts.Token);

        progressBarVM.SetProgressBar(28, "Загрузка операций по форме 1.5");
        var form15Operations = await LoadOperation41ListAsync(db, selectedReports.Id, "1.5", cts.Token);

        progressBarVM.SetProgressBar(35, "Сопоставление операций");
        var unpairedForm11 = GetUnpairedOperations(form11Operations, form15Operations);
        var unpairedForm15 = GetUnpairedOperations(form15Operations, form11Operations);

        if (unpairedForm11.Count == 0 && unpairedForm15.Count == 0)
        {
            await ShowNoUnpairedOperationsMessage(progressBar);
            await CleanupAndClose(progressBar, tmpDbPath);
            return;
        }

        progressBarVM.SetProgressBar(45, "Загрузка организации");
        var masterReports = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Include(reps => reps.Master_DB)
            .ThenInclude(master => master.Rows10)
            .FirstAsync(reps => reps.Id == selectedReports.Id, cts.Token);

        progressBarVM.SetProgressBar(55, "Загрузка непарных строчек формы 1.1");
        var reportsForForm11 = await BuildReportsForExportAsync(
            db, masterReports, unpairedForm11, "1.1", cts.Token);

        progressBarVM.SetProgressBar(65, "Загрузка непарных строчек формы 1.5");
        var reportsForForm15 = await BuildReportsForExportAsync(
            db, masterReports, unpairedForm15, "1.5", cts.Token);

        progressBarVM.SetProgressBar(75, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(80, "Заполнение листов");
        FillPairingExcel(excelPackage, reportsForForm11, reportsForForm15);

        progressBarVM.SetProgressBar(95, "Сохранение");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);

        await CleanupAndClose(progressBar, tmpDbPath);
    }

    #region Reports parameter

    private static bool TryGetReports(object? parameter, out Reports reports)
    {
        reports = null!;
        switch (parameter)
        {
            case Reports reps:
                reports = reps;
                return true;
            case IKeyCollection collection:
                reports = collection.ToList<Reports>().First();
                return true;
            default:
                return false;
        }
    }

    #endregion

    #region Database

    /// <summary>
    /// Загрузка лёгких DTO операций 41 через организацию и её отчёты.
    /// </summary>
    private static async Task<List<Operation41PairingDto>> LoadOperation41ListAsync(
        DBModel db, int repsId, string formNum, CancellationToken cancellationToken)
    {
        var operations = formNum switch
        {
            "1.1" => await db.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Where(reps => reps.Id == repsId)
                .SelectMany(reps => reps.Report_Collection
                    .Where(rep => rep.FormNum_DB == "1.1")
                    .SelectMany(rep => rep.Rows11))
                .Where(form => form.OperationCode_DB == OperationCode)
                .Select(form => new Operation41PairingDto
                {
                    Id = form.Id,
                    ReportId = form.ReportId ?? 0,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PasNum = form.PassportNumber_DB,
                    FacNum = form.FactoryNumber_DB,
                    Type = form.Type_DB,
                    Radionuclids = form.Radionuclids_DB
                })
                .ToListAsync(cancellationToken),

            "1.5" => await db.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Where(reps => reps.Id == repsId)
                .SelectMany(reps => reps.Report_Collection
                    .Where(rep => rep.FormNum_DB == "1.5")
                    .SelectMany(rep => rep.Rows15))
                .Where(form => form.OperationCode_DB == OperationCode)
                .Select(form => new Operation41PairingDto
                {
                    Id = form.Id,
                    ReportId = form.ReportId ?? 0,
                    OpCode = form.OperationCode_DB,
                    OpDate = form.OperationDate_DB,
                    PasNum = form.PassportNumber_DB,
                    FacNum = form.FactoryNumber_DB,
                    Type = form.Type_DB,
                    Radionuclids = form.Radionuclids_DB
                })
                .ToListAsync(cancellationToken),

            _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
        };

        return operations
            .Where(form => string.Equals(form.OpCode.Trim(), OperationCode, StringComparison.Ordinal))
            .ToList();
    }

    private static List<Operation41PairingDto> GetUnpairedOperations(
        List<Operation41PairingDto> source,
        List<Operation41PairingDto> reference)
    {
        var referenceKeys = new HashSet<(string PasNum, string FacNum, string Radionuclids, string Type, string OpDate)>(
            reference.Select(ToPairingKey),
            PairingKeyComparer);

        return source
            .Where(operation => !referenceKeys.Contains(ToPairingKey(operation)))
            .ToList();
    }

    private static (string PasNum, string FacNum, string Radionuclids, string Type, string OpDate) ToPairingKey(
        Operation41PairingDto dto) =>
        (dto.PasNum, dto.FacNum, dto.Radionuclids, dto.Type, dto.OpDate);

    /// <summary>
    /// Загрузка отчётов с непарными строчками для выгрузки в Excel.
    /// </summary>
    private static async Task<Reports> BuildReportsForExportAsync(
        DBModel db,
        Reports masterReports,
        List<Operation41PairingDto> unpairedOperations,
        string formNum,
        CancellationToken cancellationToken)
    {
        var result = new Reports { Master = masterReports.Master };

        if (unpairedOperations.Count == 0)
        {
            return result;
        }

        var formIds = unpairedOperations.Select(operation => operation.Id).ToList();
        var reportIds = unpairedOperations
            .Where(operation => operation.ReportId != 0)
            .Select(operation => operation.ReportId)
            .Distinct()
            .OrderBy(id => id)
            .ToList();

        foreach (var reportId in reportIds)
        {
            var report = formNum switch
            {
                "1.1" => await db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include(rep => rep.Rows11
                        .Where(form => formIds.Contains(form.Id))
                        .OrderBy(form => form.NumberInOrder_DB))
                    .FirstAsync(rep => rep.Id == reportId, cancellationToken),

                "1.5" => await db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include(rep => rep.Rows15
                        .Where(form => formIds.Contains(form.Id))
                        .OrderBy(form => form.NumberInOrder_DB))
                    .FirstAsync(rep => rep.Id == reportId, cancellationToken),

                _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
            };

            result.Report_Collection.Add(report);
        }

        return result;
    }

    #endregion

    #region Excel

    private void FillPairingExcel(ExcelPackage excelPackage, Reports reportsForForm11, Reports reportsForForm15)
    {
        Worksheet = excelPackage.Workbook.Worksheets.Add("Форма 1.1");
        SetupHeaders("1.1");
        CurrentReports = reportsForForm11;
        CurrentRow = Worksheet.Dimension!.End.Row + 1;
        WriteForm11Rows();
        AddPairingExcelTable(Worksheet, CurrentRow - 1, columnCount: 29, "tbl_Pair41_Form11");

        Worksheet = excelPackage.Workbook.Worksheets.Add("Форма 1.5");
        SetupHeaders("1.5");
        CurrentReports = reportsForForm15;
        CurrentRow = Worksheet.Dimension!.End.Row + 1;
        WriteForm15Rows();
        AddPairingExcelTable(Worksheet, CurrentRow - 1, columnCount: 32, "tbl_Pair41_Form15");
    }

    /// <summary>
    /// Оформляет диапазон как таблицу Excel (фильтр, чередование строк), как в выгрузке инвентаризаций СНК.
    /// </summary>
    private static void AddPairingExcelTable(ExcelWorksheet sheet, int lastRow, int columnCount, string tableName)
    {
        if (lastRow < 2)
        {
            return;
        }

        var table = sheet.Tables.Add(sheet.Cells[1, 1, lastRow, columnCount], tableName);
        table.TableStyle = TableStyles.Medium2;
        table.ShowRowStripes = true;
    }

    private void SetupHeaders(string formNum)
    {
        FillFormHeadersWithoutNotes(formNum);

        if (OperatingSystem.IsWindows())
        {
            Worksheet.Cells.AutoFitColumns();
        }

        ApplyPairingSheetColumnSizing(formNum);

        Worksheet.View.FreezePanes(2, 1);
    }

    /// <summary>
    /// Шире колонки с датами (+10 % к автоширине), перенос слов только в заголовках.
    /// </summary>
    private void ApplyPairingSheetColumnSizing(string formNum)
    {
        var lastCol = formNum == "1.1" ? 29 : 32;

        var dateColumns = formNum == "1.1"
            ? new[] { 4, 5, 9, 17, 24 }
            : new[] { 4, 5, 9, 16, 20 };

        foreach (var colIndex in dateColumns)
        {
            var column = Worksheet.Column(colIndex);
            column.Width *= 1.1;
        }

        var wrapColumns = formNum == "1.1"
            ? new[] { 3, 4, 5, 6, 10, 15, 16, 17, 20, 25, 26, 27 }
            : new[] { 3, 4, 5, 6, 10, 15, 16, 21, 22, 23 };

        foreach (var colIndex in wrapColumns)
        {
            var headerCell = Worksheet.Cells[1, colIndex];
            headerCell.Style.WrapText = true;
            headerCell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        }

        var passportCol = Worksheet.Column(10);
        if (passportCol.Width < 20)
        {
            passportCol.Width = 20;
        }

        Worksheet.Row(1).Height = formNum == "1.5" ? 52 : 44;
        Worksheet.Cells[1, 1, 1, lastCol].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        Worksheet.Cells[1, 1, 1, lastCol].Style.WrapText = true;
    }

    /// <summary>
    /// Заголовки колонок формы без листа примечаний.
    /// </summary>
    private void FillFormHeadersWithoutNotes(string formNum)
    {
        switch (formNum)
        {
            case "1.1":
                Worksheet.Cells[1, 1].Value = "Рег.№";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Дата начала периода";
                Worksheet.Cells[1, 5].Value = "Дата конца периода";
                Worksheet.Cells[1, 6].Value = "Номер корректировки";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Код";
                Worksheet.Cells[1, 9].Value = "Дата";
                Worksheet.Cells[1, 10].Value = "Номер паспорта (сертификата)";
                Worksheet.Cells[1, 11].Value = "Тип";
                Worksheet.Cells[1, 12].Value = "Радионуклиды";
                Worksheet.Cells[1, 13].Value = "Заводской номер";
                Worksheet.Cells[1, 14].Value = "Количество, шт";
                Worksheet.Cells[1, 15].Value = "Суммарная активность, Бк";
                Worksheet.Cells[1, 16].Value = "Код ОКПО изготовителя";
                Worksheet.Cells[1, 17].Value = "Дата выпуска";
                Worksheet.Cells[1, 18].Value = "Категория";
                Worksheet.Cells[1, 19].Value = "НСС, мес";
                Worksheet.Cells[1, 20].Value = "Код формы собственности";
                Worksheet.Cells[1, 21].Value = "Код ОКПО правообладателя";
                Worksheet.Cells[1, 22].Value = "Вид документа";
                Worksheet.Cells[1, 23].Value = "Номер документа";
                Worksheet.Cells[1, 24].Value = "Дата документа";
                Worksheet.Cells[1, 25].Value = "ОКПО поставщика или получателя";
                Worksheet.Cells[1, 26].Value = "ОКПО перевозчика";
                Worksheet.Cells[1, 27].Value = "Наименование упаковки";
                Worksheet.Cells[1, 28].Value = "Тип УКТ";
                Worksheet.Cells[1, 29].Value = "Номер УКТ";
                break;

            case "1.5":
                Worksheet.Cells[1, 1].Value = "Рег.№";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Дата начала периода";
                Worksheet.Cells[1, 5].Value = "Дата конца периода";
                Worksheet.Cells[1, 6].Value = "Номер корректировки";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Код";
                Worksheet.Cells[1, 9].Value = "Дата";
                Worksheet.Cells[1, 10].Value = "Номер паспорта (сертификата) ЗРИ, акта определения характеристик ОЗИИ";
                Worksheet.Cells[1, 11].Value = "Тип";
                Worksheet.Cells[1, 12].Value = "Радионуклиды";
                Worksheet.Cells[1, 13].Value = "Заводской номер";
                Worksheet.Cells[1, 14].Value = "Количество, шт";
                Worksheet.Cells[1, 15].Value = "Суммарная активность, Бк";
                Worksheet.Cells[1, 16].Value = "Дата выпуска";
                Worksheet.Cells[1, 17].Value = "Статус РАО";
                Worksheet.Cells[1, 18].Value = "Вид документа";
                Worksheet.Cells[1, 19].Value = "Номер документа";
                Worksheet.Cells[1, 20].Value = "Дата документа";
                Worksheet.Cells[1, 21].Value = "ОКПО поставщика или получателя";
                Worksheet.Cells[1, 22].Value = "ОКПО перевозчика";
                Worksheet.Cells[1, 23].Value = "Наименование упаковки";
                Worksheet.Cells[1, 24].Value = "Тип УКТ";
                Worksheet.Cells[1, 25].Value = "Номер УКТ";
                Worksheet.Cells[1, 26].Value = "Наименование места хранения";
                Worksheet.Cells[1, 27].Value = "Код места хранения";
                Worksheet.Cells[1, 28].Value = "Код переработки / сортировки РАО";
                Worksheet.Cells[1, 29].Value = "Субсидия, %";
                Worksheet.Cells[1, 30].Value = "Номер мероприятия ФЦП";
                Worksheet.Cells[1, 31].Value = "Номер договора";
                Worksheet.Cells[1, 32].Value = "Текст статуса РАО";
                break;
        }
    }

    private void WriteForm11Rows()
    {
        var repList = CurrentReports.Report_Collection
            .Where(rep => rep.FormNum_DB == "1.1" && rep.Rows11 != null)
            .OrderBy(rep => DateOnly.TryParse(rep.StartPeriod_DB, out var startDate) ? startDate : DateOnly.MaxValue)
            .ThenBy(rep => DateOnly.TryParse(rep.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
            .ToList();

        foreach (var rep in repList)
        {
            foreach (var repForm in rep.Rows11.OrderBy(form => form.NumberInOrder_DB))
            {
                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, CurrentRow, 4);
                Worksheet.Cells[CurrentRow, 5].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, CurrentRow, 5);
                Worksheet.Cells[CurrentRow, 6].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 7].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 9);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.PassportNumber_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.Type_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelString(repForm.FactoryNumber_DB);
                Worksheet.Cells[CurrentRow, 14].Value = repForm.Quantity_DB is null ? "-" : repForm.Quantity_DB;
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDouble(repForm.Activity_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelString(repForm.CreatorOKPO_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDate(repForm.CreationDate_DB, Worksheet, CurrentRow, 17);
                Worksheet.Cells[CurrentRow, 18].Value = repForm.Category_DB is null ? "-" : repForm.Category_DB;
                Worksheet.Cells[CurrentRow, 19].Value = repForm.SignedServicePeriod_DB is null ? "-" : repForm.SignedServicePeriod_DB;
                Worksheet.Cells[CurrentRow, 20].Value = repForm.PropertyCode_DB is null ? "-" : repForm.PropertyCode_DB;
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelString(repForm.Owner_DB);
                Worksheet.Cells[CurrentRow, 22].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 24);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 29].Value = ConvertToExcelString(repForm.PackNumber_DB);
                CurrentRow++;
            }
        }
    }

    private void WriteForm15Rows()
    {
        var repList = CurrentReports.Report_Collection
            .Where(rep => rep.FormNum_DB == "1.5" && rep.Rows15 != null)
            .OrderBy(rep => DateOnly.TryParse(rep.StartPeriod_DB, out var startDate) ? startDate : DateOnly.MaxValue)
            .ThenBy(rep => DateOnly.TryParse(rep.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
            .ToList();

        foreach (var rep in repList)
        {
            foreach (var repForm in rep.Rows15.OrderBy(form => form.NumberInOrder_DB))
            {
                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, CurrentRow, 4);
                Worksheet.Cells[CurrentRow, 5].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, CurrentRow, 5);
                Worksheet.Cells[CurrentRow, 6].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 7].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 9);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.PassportNumber_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.Type_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelString(repForm.FactoryNumber_DB);
                Worksheet.Cells[CurrentRow, 14].Value = repForm.Quantity_DB is null ? "-" : repForm.Quantity_DB;
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDouble(repForm.Activity_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDate(repForm.CreationDate_DB, Worksheet, CurrentRow, 16);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelString(repForm.StatusRAO_DB);
                Worksheet.Cells[CurrentRow, 18].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 19].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 20].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 20);
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.PackNumber_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.StoragePlaceName_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelString(repForm.StoragePlaceCode_DB);
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelString(repForm.RefineOrSortRAOCode_DB);
                Worksheet.Cells[CurrentRow, 29].Value = ConvertToExcelString(repForm.Subsidy_DB);
                Worksheet.Cells[CurrentRow, 30].Value = ConvertToExcelString(repForm.FcpNumber_DB);
                Worksheet.Cells[CurrentRow, 31].Value = ConvertToExcelString(repForm.ContractNumber_DB);
                Worksheet.Cells[CurrentRow, 32].Value = StatusRaoToDescription(repForm.StatusRAO_DB);
                CurrentRow++;
            }
        }
    }

    private static string StatusRaoToDescription(string? status)
    {
        var tmp = status?.Trim();
        return tmp switch
        {
            "1" => "накопленные",
            "2" => "федеральные",
            "3" => "собственность субъекта РФ",
            "4" => "муниципальная собственность",
            "6" => "бесхозяйные",
            "9" => "прочая собственность",
            "-" or "" or null => "-",
            _ => "вновь образованные"
        };
    }

    private static async Task<(string fullPath, bool openTemp)> ExcelGetFullPathWithUniqueIndex(
        string fileName, CancellationTokenSource cts, AnyTaskProgressBar? progressBar = null)
    {
        var res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Сохранить" },
                    new ButtonDefinition { Name = "Открыть временную копию" }
                ],
                CanResize = true,
                ContentTitle = "Выгрузка в .xlsx",
                ContentHeader = "Уведомление",
                ContentMessage = "Что бы вы хотели сделать с данной выгрузкой?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));

        switch (res)
        {
            case "Открыть временную копию":
            {
                var tmpFolder = Path.Combine(BaseVM.SystemDirectory, "RAO", "temp");
                Directory.CreateDirectory(tmpFolder);
                return (GetUniqueXlsxPath(tmpFolder, fileName), true);
            }
            case "Сохранить":
            {
                SaveFileDialog dial = new();
                dial.Filters.Add(new FileDialogFilter { Name = "Excel", Extensions = { "xlsx" } });
                dial.InitialFileName = fileName;
                var selectedPath = await dial.ShowAsync(Desktop.MainWindow);
                if (string.IsNullOrEmpty(selectedPath))
                {
                    await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                }

                if (!selectedPath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    selectedPath += ".xlsx";
                }

                var directory = Path.GetDirectoryName(selectedPath)!;
                var baseName = Path.GetFileNameWithoutExtension(selectedPath);
                return (GetUniqueXlsxPath(directory, baseName), false);
            }
            default:
                await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                return (string.Empty, false);
        }
    }

    private static string GetUniqueXlsxPath(string directory, string baseName)
    {
        var candidate = Path.Combine(directory, baseName + ".xlsx");
        if (!File.Exists(candidate))
        {
            return candidate;
        }

        for (var index = 1; ; index++)
        {
            candidate = Path.Combine(directory, $"{baseName}_{index}.xlsx");
            if (!File.Exists(candidate))
            {
                return candidate;
            }
        }
    }

    #endregion

    #region Messages

    private static async Task ShowNoUnpairedOperationsMessage(AnyTaskProgressBar progressBar)
    {
        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = "Выгрузка в .xlsx",
                ContentHeader = "Уведомление",
                ContentMessage = "Операции с кодом 41 без парных записей между формами 1.1 и 1.5 не обнаружены.",
                MinWidth = 400,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .Show(progressBar ?? Desktop.MainWindow));
    }

    private static async Task CleanupAndClose(AnyTaskProgressBar progressBar, string tmpDbPath)
    {
        try
        {
            File.Delete(tmpDbPath);
        }
        catch
        {
            // ignored
        }

        progressBar.AnyTaskProgressBarVM.SetProgressBar(100, "Завершение выгрузки");
        await progressBar.CloseAsync();
    }

    #endregion

    #region DTO

    /// <summary>
    /// Минимальный набор полей для поиска непарных операций 41.
    /// </summary>
    private sealed class Operation41PairingDto
    {
        public int Id { get; init; }
        public int ReportId { get; init; }
        public string OpCode { get; init; } = string.Empty;
        public string OpDate { get; init; } = string.Empty;
        public string PasNum { get; init; } = string.Empty;
        public string FacNum { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Radionuclids { get; init; } = string.Empty;
    }

    #endregion
}
