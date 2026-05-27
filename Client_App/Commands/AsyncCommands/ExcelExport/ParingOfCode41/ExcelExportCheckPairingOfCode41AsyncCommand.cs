using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources.CustomComparers.SnkComparers;
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

        var pairing11To15Params = await AskPairing11To15ParamsAsync();
        if (pairing11To15Params is null)
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

        progressBarVM.SetProgressBar(35, "Сопоставление операций");
        var unpairedForm11 = GetUnpairedOperations11To15(form11Operations, form15Operations, pairing11To15Params);
        var unpairedForm15 = GetUnpairedOperations11To15(form15Operations, form11Operations, pairing11To15Params);

        _form11ClosestMatchHighlights = BuildClosestMatchHighlights(unpairedForm11, form15Operations, pairing11To15Params);
        _form15ClosestMatchHighlights = BuildClosestMatchHighlights(unpairedForm15, form11Operations, pairing11To15Params);

        var unpairedForm12 = GetUnpairedOperations(
            form12Operations, form16Operations, Operation41PairingProfile.Form12To16, ToPairingKey12, ToPairingKey16For12);
        var unpairedForm13 = GetUnpairedOperations(
            form13Operations, form16Operations, Operation41PairingProfile.Form13To16, ToPairingKey13, ToPairingKey16For13);
        var unpairedForm14 = GetUnpairedOperations(
            form14Operations, form16Operations, Operation41PairingProfile.Form14To16, ToPairingKey14, ToPairingKey16For14);

        var unpairedForm16 = Operation41PairingMatcher.FindUnpairedForm16(
            form16Operations,
            form12Operations,
            form13Operations,
            form14Operations,
            ToPairingKey16For12,
            ToPairingKey16For13,
            ToPairingKey16For14,
            ToPairingKey12,
            ToPairingKey13,
            ToPairingKey14);

        if (unpairedForm11.Count == 0 && unpairedForm15.Count == 0
            && unpairedForm12.Count == 0 && unpairedForm13.Count == 0
            && unpairedForm14.Count == 0 && unpairedForm16.Count == 0)
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

        progressBarVM.SetProgressBar(51, "Загрузка непарных строчек 1.1");
        var reportsForForm11 = await BuildReportsForExportAsync(db, masterReports, unpairedForm11, "1.1", cts.Token);
        progressBarVM.SetProgressBar(55, "Загрузка непарных строчек 1.2");
        var reportsForForm12 = await BuildReportsForExportAsync(db, masterReports, unpairedForm12, "1.2", cts.Token);
        progressBarVM.SetProgressBar(59, "Загрузка непарных строчек 1.3");
        var reportsForForm13 = await BuildReportsForExportAsync(db, masterReports, unpairedForm13, "1.3", cts.Token);
        progressBarVM.SetProgressBar(63, "Загрузка непарных строчек 1.4");
        var reportsForForm14 = await BuildReportsForExportAsync(db, masterReports, unpairedForm14, "1.4", cts.Token);
        progressBarVM.SetProgressBar(67, "Загрузка непарных строчек 1.5");
        var reportsForForm15 = await BuildReportsForExportAsync(db, masterReports, unpairedForm15, "1.5", cts.Token);
        progressBarVM.SetProgressBar(71, "Загрузка непарных строчек 1.6");
        var reportsForForm16 = await BuildReportsForExportAsync(db, masterReports, unpairedForm16, "1.6", cts.Token);

        progressBarVM.SetProgressBar(75, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        progressBarVM.SetProgressBar(80, "Заполнение листов");
        FillPairingExcel(
            excelPackage,
            reportsForForm11,
            reportsForForm12,
            reportsForForm13,
            reportsForForm14,
            reportsForForm15,
            reportsForForm16);

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

    private static async Task<Pairing11To15Params?> AskPairing11To15ParamsAsync()
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

        return new Pairing11To15Params(
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
            dialog.Vm.CheckPackNumber);
    }

    internal sealed record Pairing11To15Params(
        bool CheckOperationDate,
        bool CheckPassportNumber,
        bool CheckType,
        bool CheckRadionuclids,
        bool CheckFactoryNumber,
        bool CheckActivity,
        bool CheckQuantity,
        bool CheckCreationDate,
        bool CheckDocumentVid,
        bool CheckDocumentNumber,
        bool CheckDocumentDate,
        bool CheckProviderOrRecieverOkpo,
        bool CheckTransporterOkpo,
        bool CheckPackName,
        bool CheckPackType,
        bool CheckPackNumber);

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
