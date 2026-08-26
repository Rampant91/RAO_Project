using Avalonia.Threading;
using Client_App.ViewModels.ProgressBar;
using System;

namespace Client_App.Commands.AsyncCommands.CheckForm;

/// <summary>
/// Отображение прогресса загрузки снимка и построчной проверки в <see cref="AnyTaskProgressBarVM"/>.
/// </summary>
public sealed class ReportCheckProgress
{
    private const int LoadPhaseShare = 40;

    private readonly AnyTaskProgressBarVM? _vm;
    private readonly int _minPercent;
    private readonly int _maxPercent;
    private readonly string? _exportType;
    private int _totalRows;

    private ReportCheckProgress(AnyTaskProgressBarVM? vm, int minPercent, int maxPercent, string? exportType)
    {
        _vm = vm;
        _minPercent = minPercent;
        _maxPercent = maxPercent;
        _exportType = exportType;
    }

    public static ReportCheckProgress ForStandalone(AnyTaskProgressBarVM vm, string exportType = "Проверка_формы") =>
        new(vm, 0, 100, exportType);

    public static ReportCheckProgress ForExportPhase(
        AnyTaskProgressBarVM vm,
        int minPercent,
        int maxPercent,
        string exportType) =>
        new(vm, minPercent, maxPercent, exportType);

    public void SetOrgHeader(string regNo, string okpo, string formNum, string? periodHint = null)
    {
        if (_vm == null)
        {
            return;
        }

        var period = string.IsNullOrEmpty(periodHint) ? string.Empty : $" {periodHint}";
        Post(() =>
        {
            _vm.ExportType = _exportType ?? _vm.ExportType;
            _vm.ExportName = $"{regNo}_{okpo} — форма {formNum}{period}";
        });
    }

    public void OnLoadStarted()
    {
        SetMappedPercent(0, "Загрузка данных для проверки");
    }

    public void OnLoadComplete(int totalRows)
    {
        _totalRows = totalRows;
        var status = totalRows > 0
            ? $"Загрузка строк {totalRows}/{totalRows}"
            : "Загрузка строк 0/0";
        SetMappedPercent(LoadPhaseShare, status);
    }

    public void OnCheckRow(int currentRow, int totalRows)
    {
        _totalRows = totalRows;
        if (totalRows <= 0)
        {
            SetMappedPercent(100, "Проверка завершена");
            return;
        }

        var checkShare = 100 - LoadPhaseShare;
        var rowFraction = (double)currentRow / totalRows;
        var mapped = LoadPhaseShare + (int)(checkShare * rowFraction);
        SetMappedPercent(mapped, $"Проверка строк {currentRow}/{totalRows}");
    }

    private void SetMappedPercent(int phasePercent, string loadStatus)
    {
        if (_vm == null)
        {
            return;
        }

        var span = Math.Max(0, _maxPercent - _minPercent);
        var value = _minPercent + span * phasePercent / 100;
        Post(() => _vm.SetProgressBar(value, loadStatus, _vm.ExportName, _vm.ExportType));
    }

    private void Post(System.Action action)
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            action();
        }
        else
        {
            Dispatcher.UIThread.Post(action);
        }
    }
}
