using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Threading;
using Client_App.ViewModels.Forms;
using Client_App.Views;
using Models.Collections;
using System;
using System.Threading.Tasks;

namespace Client_App.Services;

/// <summary>
/// Плавная смена окна отчёта: preload → Main minimized → close A → show B (без мигания Main).
/// </summary>
public static class FormReportWindowNavigator
{
    private static IClassicDesktopStyleApplicationLifetime Desktop =>
        (Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;

    /// <summary>
    /// Переключить на другой отчёт: при той же форме — in-place reload, иначе — замена окна.
    /// </summary>
    public static async Task SwitchAsync(Window currentWindow, BaseFormVM formVm, Report targetReport)
    {
        targetReport.Reports ??= formVm.Reports ?? formVm.Report.Reports;

        if (string.Equals(targetReport.FormNum.Value, formVm.FormType, StringComparison.Ordinal))
        {
            await ReloadInPlaceAsync(currentWindow, formVm, targetReport).ConfigureAwait(true);
            return;
        }

        await ReplaceAsync(currentWindow, targetReport).ConfigureAwait(true);
    }

    /// <summary>
    /// Закрыть текущее окно формы и открыть целевой отчёт без мигания MainWindow.
    /// </summary>
    public static async Task ReplaceAsync(Window currentWindow, Report targetReport)
    {
        var main = Desktop.MainWindow as MainWindow;

        var restoreWhenDone = WindowState.Normal;
        if (currentWindow is IFormOwnerStateWindow currentForm)
        {
            if (currentForm.OwnerPrevState != WindowState.Minimized)
                restoreWhenDone = currentForm.OwnerPrevState;
        }

        currentWindow.Cursor = new Cursor(StandardCursorType.Wait);

        Window? nextWindow;
        try
        {
            // Пока видно старое окно — грузим данные нового.
            nextWindow = await FormReportWindowOpener.CreateWindowAsync(targetReport).ConfigureAwait(true);
        }
        catch
        {
            currentWindow.Cursor = Cursor.Default;
            throw;
        }

        if (nextWindow == null)
        {
            currentWindow.Cursor = Cursor.Default;
            return;
        }

        if (nextWindow is IFormOwnerStateWindow nextForm)
            nextForm.OwnerPrevState = restoreWhenDone;

        if (currentWindow.DataContext is BaseFormVM formVm)
            formVm.SkipChangeTacking = true;

        // Close восстановит Main в OwnerPrevState — оставляем Minimized, чтобы не мигал Normal.
        if (currentWindow is IFormOwnerStateWindow ownerState)
            ownerState.OwnerPrevState = WindowState.Minimized;

        if (main != null && main.WindowState != WindowState.Minimized)
            main.WindowState = WindowState.Minimized;

        main?.SetReportOpeningOverlay(true);

        var closedTcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        void OnClosed(object? _, EventArgs __)
        {
            currentWindow.Closed -= OnClosed;
            closedTcs.TrySetResult();
        }

        currentWindow.Closed += OnClosed;
        await Dispatcher.UIThread.InvokeAsync(() => currentWindow.Close());
        await closedTcs.Task.ConfigureAwait(true);

        // Не await ShowDialog до закрытия B — иначе блокируется Switch/Перевод РВ
        // (старый ChangeForm открывал новое окно из async void Closed).
        _ = ShowReplacementAsync(nextWindow, main);
    }

    private static async Task ShowReplacementAsync(Window nextWindow, MainWindow? main)
    {
        try
        {
            await FormReportWindowOpener.ShowFormDialogAsync(nextWindow, main).ConfigureAwait(true);
        }
        finally
        {
            main?.SetReportOpeningOverlay(false);
        }
    }

    private static async Task ReloadInPlaceAsync(Window currentWindow, BaseFormVM formVm, Report targetReport)
    {
        var main = Desktop.MainWindow as MainWindow;
        currentWindow.Cursor = new Cursor(StandardCursorType.Wait);
        main?.SetReportOpeningOverlay(true);
        try
        {
            if (await ReportExportLock.TryBlockReportAccessAsync(targetReport.Id))
                return;

            var loaded = await FormReportWindowOpener.LoadReportDataAsync(targetReport).ConfigureAwait(true);
            if (loaded == null)
                return;

            await formVm.ReloadFromReportAsync(loaded).ConfigureAwait(true);
        }
        finally
        {
            currentWindow.Cursor = Cursor.Default;
            main?.SetReportOpeningOverlay(false);
        }
    }
}
