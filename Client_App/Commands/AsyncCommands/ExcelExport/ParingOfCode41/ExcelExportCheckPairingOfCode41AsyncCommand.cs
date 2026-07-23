using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Interfaces;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ParingOfCode41;

/// <summary>
/// Выгрузка в .xlsx операций с кодом 41 без парной записи при переводе РВ → РАО
/// (1.1↔1.5, 1.2↔1.6, 1.3↔1.6, 1.4↔1.6).
/// </summary>
public partial class ExcelExportCheckPairingOfCode41AsyncCommand : ExcelExportBaseAllAsyncCommand
{
    private const string OperationCode = "41";

    /// <summary>
    /// Ограничение Firebird для списка IN (...); берём запас ниже лимита 1500.
    /// </summary>
    private const int FirebirdInListMaxCount = 1000;

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

        var pairingParams = await AskPairingParamsAsync();
        if (pairingParams is null)
        {
            return;
        }
        var pairing11To15Params = pairingParams.Pairing11To15;
        var pairing12To16Params = pairingParams.Pairing12To16;
        var pairing13To16Params = pairingParams.Pairing13To16;
        var pairing14To16Params = pairingParams.Pairing14To16;

        var cts = new CancellationTokenSource();
        ExportType = "Непарные_операции_41";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        var regNum = RemoveForbiddenChars(selectedReports.Master_DB.RegNoRep.Value);
        var okpo = RemoveForbiddenChars(selectedReports.Master_DB.OkpoRep.Value);
        var fileName = $"{regNum}_{okpo}_непарные_операции_41";

        progressBarVM.SetProgressBar(5, "Запрос пути сохранения", ExportType, "Выгрузка в .xlsx");
        var (fullPath, openTemp) = await ExcelGetFullPathWithUniqueIndex(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(8, "Загрузка справочника радионуклидов", ExportType, "Выгрузка в .xlsx");
        if (!TryLoadRDictionary(out var rDictionaryError))
        {
            await ShowRDictionaryLoadErrorMessage(progressBar, rDictionaryError);
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
            return;
        }

        progressBarVM.SetProgressBar(10, "Создание временной БД", ExportType, "Выгрузка в .xlsx");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(18, "Загрузка операций 41 формы 1.1");
        var form11Operations = await LoadOperation41ListAsync(db, selectedReports.Id, "1.1", cts.Token, pairing11To15Params);
        progressBarVM.SetProgressBar(21, "Загрузка операций 41 формы 1.2");
        var form12Operations = await LoadOperation41ListAsync(db, selectedReports.Id, "1.2", cts.Token);
        progressBarVM.SetProgressBar(24, "Загрузка операций 41 формы 1.3");
        var form13Operations = await LoadOperation41ListAsync(db, selectedReports.Id, "1.3", cts.Token);
        progressBarVM.SetProgressBar(27, "Загрузка операций 41 формы 1.4");
        var form14Operations = await LoadOperation41ListAsync(db, selectedReports.Id, "1.4", cts.Token);
        progressBarVM.SetProgressBar(30, "Загрузка операций 41 формы 1.5");
        var form15Operations = await LoadOperation41ListAsync(db, selectedReports.Id, "1.5", cts.Token, pairing11To15Params);
        progressBarVM.SetProgressBar(33, "Загрузка операций 41 формы 1.6");
        var form16Operations = await LoadOperation41ListAsync(db, selectedReports.Id, "1.6", cts.Token);

        progressBarVM.SetProgressBar(35, "Сопоставление 1.1↔1.5");
        var unpairedForm11 = GetUnpairedOperations11To15(form11Operations, form15Operations, pairing11To15Params);
        var unpairedForm15 = GetUnpairedOperations11To15(form15Operations, form11Operations, pairing11To15Params);

        _form11ClosestMatchHighlights = BuildClosestMatchHighlights(unpairedForm11, form15Operations, pairing11To15Params);
        _form15ClosestMatchHighlights = BuildClosestMatchHighlights(unpairedForm15, form11Operations, pairing11To15Params);

        progressBarVM.SetProgressBar(38, "Сопоставление 1.2↔1.6");
        var unpairedForm12 = GetUnpairedOperations12To16(form12Operations, form16Operations, pairing12To16Params);
        _form12ClosestMatchHighlights = BuildClosestMatchHighlights12To16(unpairedForm12, form16Operations, pairing12To16Params);

        progressBarVM.SetProgressBar(41, "Сопоставление 1.3↔1.6");
        var unpairedForm13 = GetUnpairedOperations13To16(form13Operations, form16Operations, pairing13To16Params);
        _form13ClosestMatchHighlights = BuildClosestMatchHighlights13To16(unpairedForm13, form16Operations, pairing13To16Params);

        progressBarVM.SetProgressBar(44, "Сопоставление 1.4↔1.6");
        var unpairedForm14 = GetUnpairedOperations14To16(form14Operations, form16Operations, pairing14To16Params);
        _form14ClosestMatchHighlights = BuildClosestMatchHighlights14To16(unpairedForm14, form16Operations, pairing14To16Params);

        progressBarVM.SetProgressBar(47, "Сопоставление стороны 1.6");
        var unpairedForm16 = GetUnpairedForm16(
            form16Operations,
            form12Operations,
            form13Operations,
            form14Operations,
            pairing12To16Params,
            pairing13To16Params,
            pairing14To16Params);
        _form16ClosestMatchHighlights = BuildClosestMatchHighlights16(
            unpairedForm16,
            form12Operations,
            form13Operations,
            form14Operations,
            pairing12To16Params,
            pairing13To16Params,
            pairing14To16Params);

        if (unpairedForm11.Count == 0 && unpairedForm15.Count == 0
            && unpairedForm12.Count == 0 && unpairedForm13.Count == 0
            && unpairedForm14.Count == 0 && unpairedForm16.Count == 0)
        {
            await ShowNoUnpairedOperationsMessage(progressBar);
            await CleanupAndClose(progressBar, tmpDbPath);
            return;
        }

        progressBarVM.SetProgressBar(50, "Загрузка организации");
        var masterReports = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Include(reps => reps.Master_DB)
            .ThenInclude(master => master.Rows10)
            .FirstAsync(reps => reps.Id == selectedReports.Id, cts.Token);

        progressBarVM.SetProgressBar(52, "Загрузка непарных строчек 1.1");
        var reportsForForm11 = await BuildReportsForExportAsync(db, masterReports, unpairedForm11, "1.1", cts.Token);
        progressBarVM.SetProgressBar(55, "Загрузка непарных строчек 1.2");
        var reportsForForm12 = await BuildReportsForExportAsync(db, masterReports, unpairedForm12, "1.2", cts.Token);
        progressBarVM.SetProgressBar(58, "Загрузка непарных строчек 1.3");
        var reportsForForm13 = await BuildReportsForExportAsync(db, masterReports, unpairedForm13, "1.3", cts.Token);
        progressBarVM.SetProgressBar(61, "Загрузка непарных строчек 1.4");
        var reportsForForm14 = await BuildReportsForExportAsync(db, masterReports, unpairedForm14, "1.4", cts.Token);
        progressBarVM.SetProgressBar(64, "Загрузка непарных строчек 1.5");
        var reportsForForm15 = await BuildReportsForExportAsync(db, masterReports, unpairedForm15, "1.5", cts.Token);
        progressBarVM.SetProgressBar(67, "Загрузка непарных строчек 1.6");
        var reportsForForm16 = await BuildReportsForExportAsync(db, masterReports, unpairedForm16, "1.6", cts.Token);

        progressBarVM.SetProgressBar(70, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        SetExportActivitiesCache(unpairedForm13, unpairedForm14);
        FillPairingExcel(
            excelPackage,
            reportsForForm11,
            reportsForForm12,
            reportsForForm13,
            reportsForForm14,
            reportsForForm15,
            reportsForForm16,
            progressBarVM);

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

    #region Parameters dialog

    private static async Task<PairingParamsSet?> AskPairingParamsAsync()
    {
        GetPairingCode41Params? dialog = null;
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            dialog = new GetPairingCode41Params();
            await dialog.ShowDialog(Desktop.MainWindow);
        });

        if (dialog is null || !dialog.Vm.Ok)
        {
            return null;
        }

        return new PairingParamsSet(
            new Pairing11To15Params(
                dialog.Vm.CheckOperationDate,
                dialog.Vm.CheckPassportNumber,
                dialog.Vm.CheckType,
                dialog.Vm.CheckRadionuclids,
                dialog.Vm.CheckFactoryNumber,
                dialog.Vm.CheckActivity,
                dialog.Vm.CheckQuantity,
                dialog.Vm.CheckCreationDate,
                dialog.Vm.CheckDocumentVid,
                dialog.Vm.CheckDocumentNumber,
                dialog.Vm.CheckDocumentDate,
                dialog.Vm.CheckProviderOrRecieverOkpo,
                dialog.Vm.CheckTransporterOkpo,
                dialog.Vm.CheckPackName,
                dialog.Vm.CheckPackType,
                dialog.Vm.CheckPackNumber),
            new Pairing12To16Params(
                dialog.Vm.CheckOperationDate12To16,
                dialog.Vm.CheckMass12To16,
                dialog.Vm.CheckBetaGammaActivity12To16,
                dialog.Vm.CheckAlphaActivity12To16,
                dialog.Vm.CheckActivityMeasurementDate12To16,
                dialog.Vm.CheckDocumentVid12To16,
                dialog.Vm.CheckDocumentNumber12To16,
                dialog.Vm.CheckDocumentDate12To16,
                dialog.Vm.CheckPackName12To16,
                dialog.Vm.CheckPackType12To16,
                dialog.Vm.CheckPackNumber12To16),
            new Pairing13To16Params(
                dialog.Vm.CheckOperationDate13To16,
                dialog.Vm.CheckMainRadionuclids13To16,
                dialog.Vm.CheckTritiumActivity13To16,
                dialog.Vm.CheckBetaGammaActivity13To16,
                dialog.Vm.CheckAlphaActivity13To16,
                dialog.Vm.CheckTransuraniumActivity13To16,
                dialog.Vm.CheckActivityMeasurementDate13To16,
                dialog.Vm.CheckDocumentVid13To16,
                dialog.Vm.CheckDocumentNumber13To16,
                dialog.Vm.CheckDocumentDate13To16,
                dialog.Vm.CheckPackName13To16,
                dialog.Vm.CheckPackType13To16,
                dialog.Vm.CheckPackNumber13To16),
            new Pairing14To16Params(
                dialog.Vm.CheckOperationDate14To16,
                dialog.Vm.CheckVolume14To16,
                dialog.Vm.CheckMass14To16,
                dialog.Vm.CheckMainRadionuclids14To16,
                dialog.Vm.CheckTritiumActivity14To16,
                dialog.Vm.CheckBetaGammaActivity14To16,
                dialog.Vm.CheckAlphaActivity14To16,
                dialog.Vm.CheckTransuraniumActivity14To16,
                dialog.Vm.CheckActivityMeasurementDate14To16,
                dialog.Vm.CheckDocumentVid14To16,
                dialog.Vm.CheckDocumentNumber14To16,
                dialog.Vm.CheckDocumentDate14To16,
                dialog.Vm.CheckPackName14To16,
                dialog.Vm.CheckPackType14To16,
                dialog.Vm.CheckPackNumber14To16));
    }

    public sealed record PairingParamsSet(
        Pairing11To15Params Pairing11To15,
        Pairing12To16Params Pairing12To16,
        Pairing13To16Params Pairing13To16,
        Pairing14To16Params Pairing14To16);

    public sealed record Pairing11To15Params(
        bool CheckOperationDate = true,
        bool CheckPassportNumber = true,
        bool CheckType = true,
        bool CheckRadionuclids = true,
        bool CheckFactoryNumber = true,
        bool CheckActivity = true,
        bool CheckQuantity = true,
        bool CheckCreationDate = true,
        bool CheckDocumentVid = true,
        bool CheckDocumentNumber = true,
        bool CheckDocumentDate = true,
        bool CheckProviderOrRecieverOkpo = true,
        bool CheckTransporterOkpo = true,
        bool CheckPackName = true,
        bool CheckPackType = true,
        bool CheckPackNumber = true);

    public sealed record Pairing12To16Params(
        bool CheckOperationDate = true,
        bool CheckMass = true,
        bool CheckBetaGammaActivity = true,
        bool CheckAlphaActivity = true,
        bool CheckActivityMeasurementDate = true,
        bool CheckDocumentVid = true,
        bool CheckDocumentNumber = true,
        bool CheckDocumentDate = true,
        bool CheckPackName = true,
        bool CheckPackType = true,
        bool CheckPackNumber = true);

    public sealed record Pairing13To16Params(
        bool CheckOperationDate = true,
        bool CheckMainRadionuclids = true,
        bool CheckTritiumActivity = true,
        bool CheckBetaGammaActivity = true,
        bool CheckAlphaActivity = true,
        bool CheckTransuraniumActivity = true,
        bool CheckActivityMeasurementDate = true,
        bool CheckDocumentVid = true,
        bool CheckDocumentNumber = true,
        bool CheckDocumentDate = true,
        bool CheckPackName = true,
        bool CheckPackType = true,
        bool CheckPackNumber = true);

    public sealed record Pairing14To16Params(
        bool CheckOperationDate = true,
        bool CheckVolume = true,
        bool CheckMass = true,
        bool CheckMainRadionuclids = true,
        bool CheckTritiumActivity = true,
        bool CheckBetaGammaActivity = true,
        bool CheckAlphaActivity = true,
        bool CheckTransuraniumActivity = true,
        bool CheckActivityMeasurementDate = true,
        bool CheckDocumentVid = true,
        bool CheckDocumentNumber = true,
        bool CheckDocumentDate = true,
        bool CheckPackName = true,
        bool CheckPackType = true,
        bool CheckPackNumber = true);

    #endregion

    #region FileDialog

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
                    return (string.Empty, false);
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
                ContentMessage = "Операции с кодом 41 без парных записей при переводе РВ → РАО (1.1↔1.5, 1.2↔1.6, 1.3↔1.6, 1.4↔1.6) не обнаружены.",
                MinWidth = 400,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .Show(progressBar ?? Desktop.MainWindow));
    }

    private static async Task ShowRDictionaryLoadErrorMessage(AnyTaskProgressBar progressBar, string message)
    {
        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = "Выгрузка в .xlsx",
                ContentHeader = "Ошибка",
                ContentMessage = message,
                MinWidth = 450,
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
}
