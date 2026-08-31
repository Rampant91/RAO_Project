using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Models.Collections;
using Models.Interfaces;

using MsBox.Avalonia.Enums;
namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing;

/// <summary>
/// Выгрузка в .xlsx непарных операций приёма/передачи.
/// Реализованы формы 1.1–1.6 (листы 1.7–1.8 — заглушки); режимы: выбранная организация или вся БД.
/// </summary>
public partial class ExcelExportCheckTransferReceiveAsyncCommand : ExcelExportBaseAllAsyncCommand
{
    /// <summary>
    /// Окно поиска кандидатов по дате операции (дней в обе стороны).
    /// Не критерий пары/подсветки: совпадение даты — только точное; отличие на 1 день уже ошибка.
    /// </summary>
    private const int OperationDateToleranceDays = 15;

    /// <summary>Ограничение Firebird для списка IN (...).</summary>
    private const int FirebirdInListMaxCount = 1000;

    /// <summary>
    /// Размер пакета при загрузке операций контрагентов (org-режим), в организациях на SQL IN.
    /// Меньше лимита Firebird IN — чтобы прогрессбар двигался чаще.
    /// </summary>
    private const int CounterpartOpsLoadChunkSize = 20;

    /// <summary>
    /// Размер страницы строк формы при whole-DB загрузке (keyset по Id).
    /// </summary>
    private const int WholeDbOpsPageSize = 5000;

    /// <summary>
    /// Размер страницы строк формы при загрузке контрагентов в режиме выбранной организации (keyset по Id).
    /// </summary>
    private const int SelectedOrgOpsPageSize = 2500;

    /// <summary>
    /// Коды передачи для классификации стороны операции (включая 26 — пара на формах 1.5+).
    /// SQL load-set для 1.1–1.4 уже — без 26/36; см. массивы кодов в Data.cs.
    /// </summary>
    private static readonly HashSet<string> TransferCodesForm11To14 = new(StringComparer.Ordinal)
    {
        "21", "22", "25", "26", "27", "28", "29"
    };

    /// <summary>Коды приёма (включая 36 — пара на формах 1.5+).</summary>
    private static readonly HashSet<string> ReceiveCodesForm11To14 = new(StringComparer.Ordinal)
    {
        "31", "32", "35", "36", "37", "38", "39"
    };

    /// <summary>
    /// Парность кодов передачи ↔ приёма (в обе стороны).
    /// 26↔36 — валидная пара, но применяется на формах 1.5–1.8; на 1.1/1.3 в load-set не входит.
    /// </summary>
    private static readonly Dictionary<string, string> TransferToReceiveCode = new(StringComparer.Ordinal)
    {
        ["21"] = "31",
        ["22"] = "32",
        ["25"] = "37",
        ["26"] = "36",
        ["27"] = "35",
        ["28"] = "38",
        ["29"] = "39"
    };

    private readonly MainWindowVM _mainWindowVM;

    public ExcelExportCheckTransferReceiveAsyncCommand(MainWindowVM mainWindowVM)
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
        var pairingParams = await AskTransferReceiveParamsAsync();
        if (pairingParams is null)
        {
            return;
        }

        if (!pairingParams.AnyFormEnabled)
        {
            await ShowNoFormsSelectedMessage();
            return;
        }

        var cts = new CancellationTokenSource();
        ExportType = "Проверка_приёма_передачи";
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

    private static async Task<TransferReceiveParamsSet?> AskTransferReceiveParamsAsync()
    {
        GetTransferReceiveParams? dialog = null;
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            dialog = new GetTransferReceiveParams();
            await dialog.ShowDialog(Desktop.MainWindow);
        });

        if (dialog is null || !dialog.Vm.Ok)
        {
            return null;
        }

        return MapParamsFromDialogVm(dialog.Vm);
    }

    /// <summary>
    /// Маппинг VM диалога → набор параметров (без ShowDialog). Для unit-тестов и Ask*.
    /// </summary>
    internal static TransferReceiveParamsSet MapParamsFromDialogVm(Client_App.ViewModels.Messages.GetTransferReceiveParamsVM vm)
    {
        var form11 = new TransferReceiveFormParams(
            CheckOperationCode: vm.CheckOperationCode,
            CheckOperationDate: vm.CheckOperationDate,
            CheckPassportNumber: vm.CheckPassportNumber,
            CheckType: vm.CheckType,
            CheckRadionuclids: vm.CheckRadionuclids,
            CheckFactoryNumber: vm.CheckFactoryNumber,
            CheckQuantity: vm.CheckQuantity,
            CheckActivity: vm.CheckActivity,
            CheckCreatorOkpo: vm.CheckCreatorOkpo,
            CheckCreationDate: vm.CheckCreationDate,
            CheckProviderOrRecieverOkpo: vm.CheckProviderOrRecieverOkpo,
            CheckPackNumber: vm.CheckPackNumber);

        var form12 = new TransferReceiveFormParams(
            CheckOperationCode: vm.CheckOperationCode12,
            CheckOperationDate: vm.CheckOperationDate12,
            CheckPassportNumber: vm.CheckPassportNumber12,
            CheckType: vm.CheckName12,
            CheckRadionuclids: false,
            CheckFactoryNumber: vm.CheckFactoryNumber12,
            CheckQuantity: false,
            CheckActivity: false,
            CheckMass: vm.CheckMass12,
            CheckCreatorOkpo: vm.CheckCreatorOkpo12,
            CheckCreationDate: vm.CheckCreationDate12,
            CheckProviderOrRecieverOkpo: vm.CheckProviderOrRecieverOkpo12,
            CheckPackNumber: vm.CheckPackNumber12,
            CheckPackType: vm.CheckPackType12);

        var form13 = new TransferReceiveFormParams(
            CheckOperationCode: vm.CheckOperationCode13,
            CheckOperationDate: vm.CheckOperationDate13,
            CheckPassportNumber: vm.CheckPassportNumber13,
            CheckType: vm.CheckType13,
            CheckRadionuclids: vm.CheckRadionuclids13,
            CheckFactoryNumber: vm.CheckFactoryNumber13,
            CheckQuantity: false,
            CheckActivity: vm.CheckActivity13,
            CheckCreatorOkpo: vm.CheckCreatorOkpo13,
            CheckCreationDate: vm.CheckCreationDate13,
            CheckProviderOrRecieverOkpo: vm.CheckProviderOrRecieverOkpo13,
            CheckPackNumber: vm.CheckPackNumber13,
            CheckAggregateState: vm.CheckAggregateState13);

        var form14 = new TransferReceiveFormParams(
            CheckOperationCode: vm.CheckOperationCode14,
            CheckOperationDate: vm.CheckOperationDate14,
            CheckPassportNumber: vm.CheckPassportNumber14,
            CheckType: vm.CheckName14,
            CheckRadionuclids: vm.CheckRadionuclids14,
            CheckFactoryNumber: false,
            CheckQuantity: false,
            CheckActivity: vm.CheckActivity14,
            CheckMass: vm.CheckMass14,
            CheckCreatorOkpo: false,
            CheckCreationDate: false,
            CheckProviderOrRecieverOkpo: vm.CheckProviderOrRecieverOkpo14,
            CheckPackNumber: vm.CheckPackNumber14,
            CheckAggregateState: vm.CheckAggregateState14,
            CheckSort: vm.CheckSort14,
            CheckVolume: vm.CheckVolume14,
            CheckActivityMeasurementDate: vm.CheckActivityMeasurementDate14);

        var form15 = new TransferReceiveFormParams(
            CheckOperationCode: vm.CheckOperationCode15,
            CheckOperationDate: vm.CheckOperationDate15,
            CheckPassportNumber: vm.CheckPassportNumber15,
            CheckType: vm.CheckType15,
            CheckRadionuclids: vm.CheckRadionuclids15,
            CheckFactoryNumber: vm.CheckFactoryNumber15,
            CheckQuantity: vm.CheckQuantity15,
            CheckActivity: vm.CheckActivity15,
            CheckCreatorOkpo: false,
            CheckCreationDate: vm.CheckCreationDate15,
            CheckStatusRao: vm.CheckStatusRao15,
            CheckProviderOrRecieverOkpo: vm.CheckProviderOrRecieverOkpo15,
            CheckPackName: vm.CheckPackName15,
            CheckPackType: vm.CheckPackType15,
            CheckPackNumber: vm.CheckPackNumber15,
            CheckSubsidy: vm.CheckSubsidy15,
            CheckFcpNumber: vm.CheckFcpNumber15);

        var form16 = new TransferReceiveFormParams(
            CheckOperationCode: vm.CheckOperationCode16,
            CheckOperationDate: vm.CheckOperationDate16,
            CheckRadionuclids: vm.CheckRadionuclids16,
            CheckQuantity: vm.CheckQuantity16,
            CheckMass: vm.CheckMass16,
            CheckVolume: vm.CheckVolume16,
            CheckActivityMeasurementDate: vm.CheckActivityMeasurementDate16,
            CheckStatusRao: vm.CheckStatusRao16,
            CheckProviderOrRecieverOkpo: vm.CheckProviderOrRecieverOkpo16,
            CheckPackType: vm.CheckPackType16,
            CheckPackNumber: vm.CheckPackNumber16,
            CheckSubsidy: vm.CheckSubsidy16,
            CheckFcpNumber: vm.CheckFcpNumber16,
            CheckCodeRao: vm.CheckCodeRao16,
            CheckTritiumActivity: vm.CheckTritiumActivity16,
            CheckBetaGammaActivity: vm.CheckBetaGammaActivity16,
            CheckAlphaActivity: vm.CheckAlphaActivity16,
            CheckTransuraniumActivity: vm.CheckTransuraniumActivity16,
            AllowEmptySerialQuantityDrain: false);

        return TransferReceiveParamsSet.Create(form11, form12, form13, form14, form15, form16);
    }

    #endregion

    #region FileDialog

    private static async Task<(string fullPath, bool openTemp)> ExcelGetFullPathWithUniqueIndex(
        string fileName, CancellationTokenSource cts, AnyTaskProgressBar? progressBar = null)
    {
        var res = await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
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
            }).ShowWindowDialogAsync(Desktop.MainWindow));

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

    #region Progress

    /// <summary>
    /// Прогресс в UI-потоке Avalonia: иначе при загрузке на thread-pool
    /// привязки прогрессбара могут не перерисовываться до конца длинного SQL.
    /// </summary>
    private static Action<int, string> BindProgressToUi(AnyTaskProgressBarVM progressBarVM, string exportName) =>
        (percent, text) =>
        {
            void Apply() => progressBarVM.SetProgressBar(percent, text, exportName);

            if (Dispatcher.UIThread.CheckAccess())
            {
                Apply();
                return;
            }

            // Ждём отрисовки, чтобы статус менялся между страницами SQL, а не «залипал».
            Dispatcher.UIThread.InvokeAsync(Apply).GetAwaiter().GetResult();
        };

    /// <summary>
    /// Редкие обновления прогрессбара (не чаще чем раз в <see cref="MinIntervalMs"/> мс),
    /// плюс всегда границы этапа — без заметной потери производительности.
    /// </summary>
    private sealed class ProgressReporter(Action<int, string>? report, int percentMin, int percentMax)
    {
        private const int MinIntervalMs = 300;
        private long _lastReportTicks = long.MinValue / 2;
        private readonly object _gate = new();

        public void Status(string text) =>
            report?.Invoke(percentMin, text);

        public void Report(int done, int total, string text) =>
            ReportCore(done, total, text, force: false);

        /// <summary>Обновить сразу (перед/после SQL-пакета), без троттлинга.</summary>
        public void ReportNow(int done, int total, string text) =>
            ReportCore(done, total, text, force: true);

        private void ReportCore(int done, int total, string text, bool force)
        {
            if (report is null)
            {
                return;
            }

            lock (_gate)
            {
                var now = Environment.TickCount64;
                var isBoundary = done <= 0 || total <= 0 || done >= total;
                if (!force && !isBoundary && now - _lastReportTicks < MinIntervalMs)
                {
                    return;
                }

                _lastReportTicks = now;
                var percent = percentMin;
                if (total > 0)
                {
                    var t = Math.Clamp(done / (double)total, 0, 1);
                    percent = percentMin + (int)((percentMax - percentMin) * t);
                }

                report(percent, text);
            }
        }
    }

    #endregion

    #region Messages

    private static async Task ShowNoUnpairedOperationsMessage(
        AnyTaskProgressBar progressBar,
        bool wholeDatabase = false)
    {
        var contentMessage = wholeDatabase
            ? "Непарные операции приёма/передачи по формам 1.1–1.6 по всей базе не обнаружены."
            : "Непарные операции приёма/передачи по формам 1.1–1.6 у выбранной организации не обнаружены.";

        await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
            .GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.Ok,
                ContentTitle = "Выгрузка в .xlsx",
                ContentHeader = "Уведомление",
                ContentMessage = contentMessage,
                MinWidth = 400,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowWindowDialogAsync(progressBar ?? Desktop.MainWindow));
    }

    private static async Task ShowNoFormsSelectedMessage()
    {
        await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
            .GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.Ok,
                ContentTitle = "Проверка приёма-передачи",
                ContentHeader = "Уведомление",
                ContentMessage =
                    "Не выбрано ни одного поля для форм 1.1–1.6. Отметьте параметры хотя бы для одной формы — иначе проверку выполнять нечего.",
                MinWidth = 420,
                MinHeight = 160,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowWindowDialogAsync(Desktop.MainWindow));
    }

    private static async Task CleanupAndClose(AnyTaskProgressBar progressBar, string tmpDbPath)
    {
        TryDeleteTempDataBase(tmpDbPath);

        progressBar.AnyTaskProgressBarVM.SetProgressBar(100, "Завершение выгрузки");
        await progressBar.CloseAsync();
    }

    #endregion

    #region Operation code helpers

    private static bool IsTransferCodeForm11(string? opCode) =>
        TransferCodesForm11To14.Contains(opCode?.Trim() ?? string.Empty);

    private static bool IsReceiveCodeForm11(string? opCode) =>
        ReceiveCodesForm11To14.Contains(opCode?.Trim() ?? string.Empty);

    private static bool IsTransferOrReceiveCodeForm11(string? opCode) =>
        IsTransferCodeForm11(opCode) || IsReceiveCodeForm11(opCode);

    /// <summary>Коды парные по таблице (21↔31 и т.д.).</summary>
    private static bool OpCodesArePaired(string? leftCode, string? rightCode)
    {
        var left = leftCode?.Trim() ?? string.Empty;
        var right = rightCode?.Trim() ?? string.Empty;
        if (left.Length == 0 || right.Length == 0)
        {
            return false;
        }

        return TransferToReceiveCode.TryGetValue(left, out var expectedRight) && expectedRight == right
               || TransferToReceiveCode.TryGetValue(right, out var expectedLeft) && expectedLeft == left;
    }

    #endregion
}
