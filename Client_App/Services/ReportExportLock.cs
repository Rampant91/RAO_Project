using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using MsBox.Avalonia.Dto;
using Models.Collections;

using MsBox.Avalonia.Enums;
namespace Client_App.Services;

/// <summary>
/// Soft-lock отчёта и его организации (форма x.0) на время выгрузки в .RAODB / Excel.
/// Пока блокировка активна, нельзя открывать/редактировать/удалять этот отчёт и карточку организации.
/// </summary>
public static class ReportExportLock
{
    private static readonly object Sync = new();
    private static readonly Dictionary<int, int> ReportRefCounts = new();
    private static readonly Dictionary<int, int> OrganizationRefCounts = new();

    /// <summary>
    /// Захватывает блокировку отчёта и организации. Обязательно вызвать <see cref="IDisposable.Dispose"/> по завершении.
    /// </summary>
    public static IDisposable Acquire(int reportId, int organizationId)
    {
        if (reportId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(reportId));
        }

        if (organizationId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(organizationId));
        }

        lock (Sync)
        {
            Increment(ReportRefCounts, reportId);
            Increment(OrganizationRefCounts, organizationId);
        }

        return new Scope(reportId, organizationId);
    }

    public static bool IsReportLocked(int reportId)
    {
        if (reportId <= 0)
        {
            return false;
        }

        lock (Sync)
        {
            return ReportRefCounts.TryGetValue(reportId, out var count) && count > 0;
        }
    }

    public static bool IsOrganizationLocked(int organizationId)
    {
        if (organizationId <= 0)
        {
            return false;
        }

        lock (Sync)
        {
            return OrganizationRefCounts.TryGetValue(organizationId, out var count) && count > 0;
        }
    }

    /// <summary>
    /// Если отчёт заблокирован выгрузкой — показывает сообщение и возвращает <c>true</c>.
    /// </summary>
    public static async Task<bool> TryBlockReportAccessAsync(int reportId, Window? owner = null)
    {
        if (!IsReportLocked(reportId))
        {
            return false;
        }

        await ShowBlockedMessageAsync(
            "Отчёт сейчас выгружается. Открытие, редактирование и удаление недоступны до завершения выгрузки.",
            owner);
        return true;
    }

    /// <summary>
    /// Если организация (форма x.0) заблокирована выгрузкой — показывает сообщение и возвращает <c>true</c>.
    /// </summary>
    public static async Task<bool> TryBlockOrganizationAccessAsync(int organizationId, Window? owner = null)
    {
        if (!IsOrganizationLocked(organizationId))
        {
            return false;
        }

        await ShowBlockedMessageAsync(
            "Организация (форма x.0) участвует в выгрузке отчёта. Открытие, редактирование и удаление недоступны до завершения выгрузки.",
            owner);
        return true;
    }

    /// <summary>
    /// Id организации (<see cref="Reports"/>) для отчёта: из навигации или из локального кэша.
    /// </summary>
    public static int ResolveOrganizationId(Report report, Reports? selectedOrganization = null)
    {
        if (selectedOrganization is { Id: > 0 })
        {
            return selectedOrganization.Id;
        }

        if (report.Reports is { Id: > 0 })
        {
            return report.Reports.Id;
        }

        var fromStorage = ReportsStorage.LocalReports?.Reports_Collection
            ?.FirstOrDefault(r => r.Report_Collection.Any(x => x.Id == report.Id));
        return fromStorage?.Id ?? 0;
    }

    private static void Release(int reportId, int organizationId)
    {
        lock (Sync)
        {
            Decrement(ReportRefCounts, reportId);
            Decrement(OrganizationRefCounts, organizationId);
        }
    }

    private static void Increment(Dictionary<int, int> map, int id)
    {
        map[id] = map.TryGetValue(id, out var count) ? count + 1 : 1;
    }

    private static void Decrement(Dictionary<int, int> map, int id)
    {
        if (!map.TryGetValue(id, out var count))
        {
            return;
        }

        if (count <= 1)
        {
            map.Remove(id);
        }
        else
        {
            map[id] = count - 1;
        }
    }

    private static async Task ShowBlockedMessageAsync(string message, Window? owner)
    {
        var desktop = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        var window = owner ?? desktop?.MainWindow;
        if (window is null)
        {
            return;
        }

        await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
            .GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.Ok,
                ContentTitle = "Выгрузка",
                ContentHeader = "Уведомление",
                ContentMessage = message,
                MinWidth = 400,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true
            }).ShowWindowDialogAsync(window));
    }

    private sealed class Scope(int reportId, int organizationId) : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            Release(reportId, organizationId);
        }
    }
}
