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
using Models.Collections;
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41;

/// <summary>
/// Выгрузка в .xlsx операций с кодом 41 без парной записи при переводе РВ → РАО
/// (1.1↔1.5, 1.2↔1.6, 1.3↔1.6, 1.4↔1.6).
/// </summary>
public partial class ExcelExportCheckPairingOfCode41AsyncCommand : ExcelExportBaseAllAsyncCommand
{
    private const string OperationCode = "41";

    /// <summary>
    /// Частая ошибка на форме 1.5: вместо 41 указывают код получения 14.
    /// Такие строки загружаются как кандидаты для пары к 1.1, но не ищут себе пару
    /// и не попадают в непарные на листе 1.5.
    /// </summary>
    private const string Form15ReceiveMistypeOpCode = "14";

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

    public override bool CanExecute(object? parameter) =>
        IsWholeDbMode(parameter)
        || parameter is Reports
        || parameter is IKeyCollection
        || _mainWindowVM.SelectedReports is not null;

    public override async Task AsyncExecute(object? parameter)
    {
        var pairingParams = await AskPairingParamsAsync();
        if (pairingParams is null)
        {
            return;
        }

        var cts = new CancellationTokenSource();
        ExportType = "Непарные_операции_41";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));

        if (IsWholeDbMode(parameter))
        {
            await ExecuteForWholeDatabaseAsync(pairingParams, progressBar, cts);
            return;
        }

        if (!TryGetReports(parameter, out var selectedReports))
        {
            if (_mainWindowVM.SelectedReports is null)
            {
                await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                return;
            }

            selectedReports = _mainWindowVM.SelectedReports;
        }

        await ExecuteForSelectedOrganizationAsync(selectedReports, pairingParams, progressBar, cts);
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
                dialog.Vm.CheckPackNumber,
                dialog.Vm.CheckOperationCode),
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
                dialog.Vm.CheckPackNumber12To16,
                dialog.Vm.CheckCodeRao12To16),
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
                dialog.Vm.CheckPackNumber13To16,
                dialog.Vm.CheckCodeRao13To16),
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
                dialog.Vm.CheckPackNumber14To16,
                dialog.Vm.CheckCodeRao14To16));
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
        bool CheckPackNumber = true,
        /// <summary>Код операции (на 1.5 часто ошибочно 14 вместо 41).</summary>
        bool CheckOperationCode = true);

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
        bool CheckPackNumber = true,
        bool CheckCodeRao = true);

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
        bool CheckPackNumber = true,
        bool CheckCodeRao = true);

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
        bool CheckPackNumber = true,
        bool CheckCodeRao = true);

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
