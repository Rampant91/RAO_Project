using System;
using System.Collections.Generic;
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

namespace Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing;

/// <summary>
/// Выгрузка в .xlsx непарных операций приёма/передачи (формы 1.1–1.8).
/// Реализована сверка для форм 1.1 и 1.3: выбранная организация или вся БД.
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
    /// Размер пакета при загрузке операций контрагентов (org-режим).
    /// Меньше лимита Firebird IN — чтобы прогрессбар двигался; 50 — компромисс скорость/плавность.
    /// Whole-DB bulk грузит формы целиком (один scan), без этого чанка.
    /// </summary>
    private const int CounterpartOpsLoadChunkSize = 20;

    private static readonly HashSet<string> TransferCodesForm11To14 = new(StringComparer.Ordinal)
    {
        "21", "22", "25", "27", "28", "29"
    };

    private static readonly HashSet<string> ReceiveCodesForm11To14 = new(StringComparer.Ordinal)
    {
        "31", "32", "35", "37", "38", "39"
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

        var form11 = new TransferReceiveFormParams(
            dialog.Vm.CheckOperationCode,
            dialog.Vm.CheckOperationDate,
            dialog.Vm.CheckPassportNumber,
            dialog.Vm.CheckType,
            dialog.Vm.CheckRadionuclids,
            dialog.Vm.CheckFactoryNumber,
            dialog.Vm.CheckQuantity,
            dialog.Vm.CheckActivity,
            dialog.Vm.CheckCreatorOkpo,
            dialog.Vm.CheckCreationDate,
            dialog.Vm.CheckProviderOrRecieverOkpo,
            dialog.Vm.CheckPackNumber);

        var form13 = new TransferReceiveFormParams(
            CheckOperationCode: dialog.Vm.CheckOperationCode13,
            CheckOperationDate: dialog.Vm.CheckOperationDate13,
            CheckPassportNumber: dialog.Vm.CheckPassportNumber13,
            CheckType: dialog.Vm.CheckType13,
            CheckRadionuclids: dialog.Vm.CheckRadionuclids13,
            CheckFactoryNumber: dialog.Vm.CheckFactoryNumber13,
            CheckQuantity: false,
            CheckActivity: dialog.Vm.CheckActivity13,
            CheckCreatorOkpo: dialog.Vm.CheckCreatorOkpo13,
            CheckCreationDate: dialog.Vm.CheckCreationDate13,
            CheckProviderOrRecieverOkpo: dialog.Vm.CheckProviderOrRecieverOkpo13,
            CheckPackNumber: dialog.Vm.CheckPackNumber13,
            CheckAggregateState: dialog.Vm.CheckAggregateState13);

        return new TransferReceiveParamsSet(form11, form13);
    }

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

    #region Progress

    /// <summary>
    /// Редкие обновления прогрессбара (не чаще чем раз в <see cref="MinIntervalMs"/> мс),
    /// плюс всегда границы этапа — без заметной потери производительности.
    /// </summary>
    private sealed class ProgressReporter(Action<int, string>? report, int percentMin, int percentMax)
    {
        private const int MinIntervalMs = 300;
        private long _lastReportTicks = long.MinValue / 2;

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

    #endregion

    #region Messages

    private static async Task ShowNoUnpairedOperationsMessage(
        AnyTaskProgressBar progressBar,
        bool wholeDatabase = false)
    {
        var contentMessage = wholeDatabase
            ? "Непарные операции приёма/передачи по формам 1.1 и 1.3 по всей базе не обнаружены."
            : "Непарные операции приёма/передачи по формам 1.1 и 1.3 у выбранной организации не обнаружены.";

        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = "Выгрузка в .xlsx",
                ContentHeader = "Уведомление",
                ContentMessage = contentMessage,
                MinWidth = 400,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .Show(progressBar ?? Desktop.MainWindow));
    }

    private static async Task ShowNoFormsSelectedMessage()
    {
        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = "Проверка приёма-передачи",
                ContentHeader = "Уведомление",
                ContentMessage =
                    "Не выбрано ни одного поля для форм 1.1 и 1.3. Отметьте параметры хотя бы для одной формы — иначе проверку выполнять нечего.",
                MinWidth = 420,
                MinHeight = 160,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .Show(Desktop.MainWindow));
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
