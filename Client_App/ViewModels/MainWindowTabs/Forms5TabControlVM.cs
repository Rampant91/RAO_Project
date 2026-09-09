using Client_App.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace Client_App.ViewModels.MainWindowTabs;

public class Forms5TabControlVM : FormsTabControlBaseVM
{
    #region Fields

    private protected override byte DefaultFormsPerPage => MainWindowPagingDefaults.DefaultFormsPerPage;

    private protected override byte DefaultOrgsPerPage => MainWindowPagingDefaults.DefaultOrgsPerPage;

    private protected override char FormNum => '5';

    private readonly ObservableCollection<Report> _reportCollection = new();
    private int _reportLoadGeneration;

    #endregion

    #region Constructor

    public Forms5TabControlVM() { }

    public Forms5TabControlVM(MainWindowVM mainWindowVM) : base(mainWindowVM) { }

    #endregion

    #region Properties

    protected override void CheckAndResetFilterIfNeeded()
    {
        if (string.IsNullOrEmpty(FormNumWhiteList) || SelectedReports == null)
            return;

        var has = _cache.TryHasFormNum(SelectedReports.Id, FormNumWhiteList);
        if (has == false)
            FormNumWhiteList = string.Empty;
    }

    protected override string? GetReportFormFilter() =>
        string.IsNullOrEmpty(FormNumWhiteList) ? null : FormNumWhiteList;

    #region FormNumWhiteList

    private string _formNumWhiteList = "";
    public string FormNumWhiteList
    {
        get => _formNumWhiteList;
        set
        {
            _formNumWhiteList = value;
            OnPropertyChanged();
        }
    }

    #endregion

    private protected override ObservableCollection<Report> ReportCollection => _reportCollection;

    private protected override Dictionary<string, Func<IQueryable<Report>, IQueryable<object>>> RowSelectors
    {
        get
        {
            return new Dictionary<string, Func<IQueryable<Report>, IQueryable<object>>>
            {
                ["5.1"] = q => q.Include(x => x.Rows51).SelectMany(x => x.Rows51),
                ["5.2"] = q => q.Include(x => x.Rows52).SelectMany(x => x.Rows52),
                ["5.3"] = q => q.Include(x => x.Rows53).SelectMany(x => x.Rows53),
                ["5.4"] = q => q.Include(x => x.Rows54).SelectMany(x => x.Rows54),
                ["5.5"] = q => q.Include(x => x.Rows55).SelectMany(x => x.Rows55),
                ["5.6"] = q => q.Include(x => x.Rows56).SelectMany(x => x.Rows56),
                ["5.7"] = q => q.Include(x => x.Rows57).SelectMany(x => x.Rows57),
            };
        }
    }

    private protected override int TotalRowsForms
    {
        get
        {
            if (SelectedReports is null) return 0;

            var filter = GetReportFormFilter();
            if (_cache.TryCountReports(SelectedReports.Id, filter, out var count))
                return count;
            return _reportCollection.Count;
        }
    }

    #endregion

    #region Functions

    public override void UpdateReportCollection()
    {
        if (SelectedReports is null)
        {
            Interlocked.Increment(ref _reportLoadGeneration);
            SetReportsLoading(false);
            if (_reportCollection.Count > 0)
                _reportCollection.Clear();
            OnPropertyChanged(nameof(ReportCollection));
            return;
        }

        var orgId = SelectedReports.Id;
        var filter = GetReportFormFilter();
        var page = CurrentPageFormsValue;
        var pageSize = RowsCountForms;

        if (_cache.TryGetReportPage(orgId, filter, page, pageSize, out var cached))
        {
            Interlocked.Increment(ref _reportLoadGeneration);
            SetReportsLoading(false);
            ReplaceCollection(_reportCollection, cached, r => r.Id);
            OnPropertyChanged(nameof(ReportCollection));
            OnPropertyChanged(nameof(TotalRowsForms));
            OnPropertyChanged(nameof(TotalPagesForms));
            WarmSelectedOrgAndPrefetchReports();
            return;
        }

        var generation = Interlocked.Increment(ref _reportLoadGeneration);
        ScheduleReportsLoadingIndicator(generation, () => _reportLoadGeneration);
        if (_reportCollection.Count > 0)
            _reportCollection.Clear();
        OnPropertyChanged(nameof(ReportCollection));

        var dbPath = StaticConfiguration.DBPath;
        _ = Task.Run(() =>
        {
            try
            {
                var items = MainWindowDbGate.Run(dbPath, db =>
                    _cache.GetReportPage(db, orgId, filter, page, pageSize));
                Dispatcher.UIThread.Post(() =>
                {
                    if (generation != _reportLoadGeneration || SelectedReports?.Id != orgId)
                        return;
                    SetReportsLoading(false);
                    ReplaceCollection(_reportCollection, items, r => r.Id);
                    OnPropertyChanged(nameof(ReportCollection));
                    OnPropertyChanged(nameof(TotalRowsForms));
                    OnPropertyChanged(nameof(TotalPagesForms));
                    WarmSelectedOrgAndPrefetchReports();
                });
            }
            catch
            {
                Dispatcher.UIThread.Post(() =>
                {
                    if (generation == _reportLoadGeneration)
                        SetReportsLoading(false);
                });
            }
        });
    }

    public void SetWhiteList(string formNum)
    {
        if (FormNumWhiteList != formNum)
            FormNumWhiteList = formNum;
        else
            FormNumWhiteList = "";

        UpdateReportCollection();
        UpdateFormsPageInfo();
    }

    #endregion
}
