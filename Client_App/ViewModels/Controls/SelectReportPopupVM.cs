using Client_App.Commands.AsyncCommands.SwitchReport;
using Client_App.Services;
using Client_App.Services.DataAccess;
using Client_App.ViewModels.Forms;
using Models.Collections;
using Models.DBRealization;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;

namespace Client_App.ViewModels.Controls;

public class SelectReportPopupVM : INotifyPropertyChanged
{
    #region Constructor

    public SelectReportPopupVM(BaseFormVM formVM)
    {
        _formVM = formVM;

        // Пока stubs не подгружены — только текущий отчёт (без блокировки UI).
        _reportCollection = [Report];
        _loadedForms.Add(Report.FormNum_DB);

        _selectedReport = Report;
        CurrentFormNum = Report.FormNum_DB;

        OpenPopupCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var opening = !PopupIsOpen;
            PopupIsOpen = !PopupIsOpen;
            if (opening)
                await EnsureFormShellsLoadedAsync(CurrentFormNum);
        });
        SwitchNextReportCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await EnsureFormShellsLoadedAsync(CurrentFormNum);
            var list = ReportCollection;
            var index = IndexOfReport(list, SelectedReport);
            if (index - 1 >= 0)
            {
                SelectedReport = list[index - 1];
                await new SwitchToSelectedReportAsyncCommand(formVM).AsyncExecute(SelectedReport);
            }
        });
        SwitchPreviousReportCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await EnsureFormShellsLoadedAsync(CurrentFormNum);
            var list = ReportCollection;
            var index = IndexOfReport(list, SelectedReport);
            if (index + 1 < list.Count)
            {
                SelectedReport = list[index + 1];
                await new SwitchToSelectedReportAsyncCommand(formVM).AsyncExecute(SelectedReport);
            }
        });
        SetCurrentFormNum = ReactiveCommand.CreateFromTask(async (string newFormNum) =>
        {
            CurrentFormNum = newFormNum;
            await EnsureFormShellsLoadedAsync(newFormNum);
        });

        // Фоновая подгрузка текущей формы сразу при открытии окна отчёта.
        _ = PrefetchCurrentFormAsync();
    }

    #endregion

    #region Commands

    public ICommand OpenPopupCommand { get; }

    public ICommand SwitchNextReportCommand { get; }

    public ICommand SwitchPreviousReportCommand { get; }

    public ICommand SetCurrentFormNum { get; }

    #endregion

    #region Properties

    #region PopupIsOpen
    private bool _popupIsOpen;
    public bool PopupIsOpen
    {
        get => _popupIsOpen;
        set
        {
            _popupIsOpen = value;
            OnPropertyChanged();
        }
    }
    #endregion

    private string _yearSearch;
    public string YearSearch
    {
        get => _yearSearch;
        set
        {
            _yearSearch = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ReportCollection));
        }
    }

    #region YearMode
    public bool YearMode
    {
        get
        {
            var formType = FormVM.FormType;
            return formType[0] is '2' or '4';
        }
    }
    #endregion

    public bool IsForm1 => FormVM.FormType[0] is '1';
    public bool IsForm2 => FormVM.FormType[0] is '2';

    #region CurrentFormNum
    private string _currentFormNum;
    public string CurrentFormNum
    {
        get => _currentFormNum;
        set
        {
            _currentFormNum = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ReportCollection));
            if (_currentFormNum == Report.FormNum_DB)
                SelectedReport = Report;
        }
    }
    #endregion

    #region SelectedReport
    private Report _selectedReport;
    public Report SelectedReport
    {
        get => _selectedReport;
        set
        {
            _selectedReport = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region BaseFormVM
    private readonly BaseFormVM _formVM;
    public BaseFormVM FormVM => _formVM;
    #endregion

    #region Report
    public Report Report => FormVM.Report;
    #endregion

    #region ReportCollections
    private List<Report> _reportCollection;
    private readonly HashSet<string> _loadedForms = new(StringComparer.Ordinal);
    private readonly Dictionary<string, List<Report>> _shellsByForm = new(StringComparer.Ordinal);
    private readonly object _loadGate = new();

    public List<Report> ReportCollection
    {
        get
        {
            var result = _reportCollection;

            if (!string.IsNullOrEmpty(CurrentFormNum))
                result = result.FindAll(x => x.FormNum_DB == CurrentFormNum
                                            || (x.FormNum?.Value == CurrentFormNum));

            if (int.TryParse(YearSearch, out var year))
            {
                if (YearMode)
                    result = result.FindAll(x => x.Year_DB == year);
                else
                    result = result
                        .FindAll(x =>
                            DateTime.TryParse(x.StartPeriod_DB, out _)
                            && DateTime.Parse(x.StartPeriod_DB).Year <= year)
                        .FindAll(x =>
                            DateTime.TryParse(x.EndPeriod_DB, out _)
                            && DateTime.Parse(x.EndPeriod_DB).Year >= year);
            }

            return result;
        }
    }

    #endregion

    #endregion

    private async Task PrefetchCurrentFormAsync()
    {
        try
        {
            await EnsureFormShellsLoadedAsync(Report.FormNum_DB);
        }
        catch
        {
            // best-effort
        }
    }

    /// <summary>
    /// Подгружает только stubs нужной формы (из warm-cache или лёгкий SQL), без полной загрузки строк.
    /// </summary>
    private async Task EnsureFormShellsLoadedAsync(string formNum)
    {
        if (string.IsNullOrEmpty(formNum))
            return;

        lock (_loadGate)
        {
            if (_shellsByForm.TryGetValue(formNum, out var cached))
            {
                ApplyCollection(cached, formNum);
                return;
            }
        }

        var orgId = FormVM.Reports?.Id ?? FormVM.Report.Reports?.Id ?? 0;
        if (orgId == 0)
            return;

        var orderByYear = YearMode;
        var prefer = Report;
        var dbPath = StaticConfiguration.DBPath;

        List<Report> shells;
        try
        {
            shells = await Task.Run(() =>
            {
                List<ReportListStub> stubs;
                if (Forms1WarmCache.Instance.TryGetReportStubs(orgId, out var warm)
                    && warm.Any(s => s.FormNum == formNum))
                {
                    stubs = warm.Where(s => s.FormNum == formNum).ToList();
                }
                else
                {
                    stubs = MainWindowDbGate.Run(dbPath, db =>
                        MainWindowListQuery.LoadReportStubsForForm(db, orgId, formNum));
                }

                return MainWindowListQuery.CreateReportShellsFromStubs(
                    stubs, formNum, orderByYear, prefer);
            });
        }
        catch (Exception ex)
        {
            FirebirdLogger.LogError("SelectReportPopupVM.EnsureFormShellsLoadedAsync failed", ex);
            shells = [Report];
        }

        if (shells.Count == 0)
            shells = [Report];

        // Привязка к org — для последующего Switch / ChangeOrCreateVM.
        var org = FormVM.Reports ?? FormVM.Report.Reports;
        if (org != null)
        {
            foreach (var shell in shells)
            {
                if (shell.Reports is null)
                    shell.Reports = org;
            }
        }

        lock (_loadGate)
        {
            _shellsByForm[formNum] = shells;
            _loadedForms.Add(formNum);
        }

        await Dispatcher.UIThread.InvokeAsync(() => ApplyCollection(shells, formNum));
    }

    private void ApplyCollection(List<Report> shells, string formNum)
    {
        if (!string.Equals(CurrentFormNum, formNum, StringComparison.Ordinal))
            return;

        _reportCollection = shells;
        OnPropertyChanged(nameof(ReportCollection));
    }

    private static int IndexOfReport(List<Report> list, Report selected)
    {
        var index = list.IndexOf(selected);
        if (index < 0)
            index = list.FindIndex(r => r.Id == selected.Id);
        return index;
    }

    #region OnPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
