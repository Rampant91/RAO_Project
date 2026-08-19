using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.Import;
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
using System.Windows.Input;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Add;

namespace Client_App.ViewModels.MainWindowTabs;

public class Forms2TabControlVM : FormsTabControlBaseVM
{
    #region Fields

    private protected override byte DefaultFormsPerPage => MainWindowPagingDefaults.DefaultFormsPerPage;

    private protected override byte DefaultOrgsPerPage => MainWindowPagingDefaults.DefaultOrgsPerPage;

    private protected override char FormNum => '2';

    private readonly ObservableCollection<Report> _reportCollection = new();
    private int _reportLoadGeneration;

    #endregion

    #region Commands

    public ICommand OldAddReport { get; private set; }
    public ICommand CheckReportFromMain { get; private set; }
    public ICommand ExcelExportFormAnalysis { get; private set; }
    public ICommand ImportExcel { get; private set; }
    public ICommand ImportRaodb { get; private set; } 
    
    #endregion

    #region Constructor

    public Forms2TabControlVM()
    {
        InitializeCommands();
    }

    public Forms2TabControlVM(MainWindowVM mainWindowVM) : base(mainWindowVM)
    {
        InitializeCommands();
    }

    private void InitializeCommands()
    {
        OldAddReport = new OldAddReportAsyncCommand();
        CheckReportFromMain = new CheckReportFromMainAsyncCommand(this);
        ExcelExportFormAnalysis = new ExcelExportFormAnalysisAsyncCommand(this);
        ImportExcel = new ImportExcelAsyncCommand(this);
        ImportRaodb = new ImportRaodbAsyncCommand(this);
    }

    #endregion

    #region Properties

    protected override bool GetAdditionalSearchConditions(Reports reps, string search)
    {
        return reps.Master_DB.Rows20[0].ShortJurLico_DB.ToLower().Contains(search, StringComparison.CurrentCultureIgnoreCase)
               || reps.Master_DB.Rows20[1].ShortJurLico_DB.ToLower().Contains(search, StringComparison.CurrentCultureIgnoreCase);
    }

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

    protected override bool NotifySelectedReportsOnReportPageUpdate => true;

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
                ["2.1"] = q => q.Include(x => x.Rows21).SelectMany(x => x.Rows21),
                ["2.2"] = q => q.Include(x => x.Rows22).SelectMany(x => x.Rows22),
                ["2.3"] = q => q.Include(x => x.Rows23).SelectMany(x => x.Rows23),
                ["2.4"] = q => q.Include(x => x.Rows24).SelectMany(x => x.Rows24),
                ["2.5"] = q => q.Include(x => x.Rows25).SelectMany(x => x.Rows25),
                ["2.6"] = q => q.Include(x => x.Rows26).SelectMany(x => x.Rows26),
                ["2.7"] = q => q.Include(x => x.Rows27).SelectMany(x => x.Rows27),
                ["2.8"] = q => q.Include(x => x.Rows28).SelectMany(x => x.Rows28),
                ["2.9"] = q => q.Include(x => x.Rows29).SelectMany(x => x.Rows29),
                ["2.10"] = q => q.Include(x => x.Rows210).SelectMany(x => x.Rows210),
                ["2.11"] = q => q.Include(x => x.Rows211).SelectMany(x => x.Rows211),
                ["2.12"] = q => q.Include(x => x.Rows212).SelectMany(x => x.Rows212),
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
            ReplaceCollection(_reportCollection, cached, r => r.Id);
            OnPropertyChanged(nameof(ReportCollection));
            OnPropertyChanged(nameof(TotalRowsForms));
            OnPropertyChanged(nameof(TotalPagesForms));
            OnPropertyChanged(nameof(SelectedReports));
            WarmSelectedOrgAndPrefetchReports();
            return;
        }

        var generation = Interlocked.Increment(ref _reportLoadGeneration);
        if (_reportCollection.Count > 0)
            _reportCollection.Clear();
        OnPropertyChanged(nameof(ReportCollection));

        var dbPath = StaticConfiguration.DBPath;
        _ = Task.Run(() =>
        {
            try
            {
                using var db = new DBModel(dbPath);
                var items = _cache.GetReportPage(db, orgId, filter, page, pageSize);
                Dispatcher.UIThread.Post(() =>
                {
                    if (generation != _reportLoadGeneration || SelectedReports?.Id != orgId)
                        return;
                    ReplaceCollection(_reportCollection, items, r => r.Id);
                    OnPropertyChanged(nameof(ReportCollection));
                    OnPropertyChanged(nameof(TotalRowsForms));
                    OnPropertyChanged(nameof(TotalPagesForms));
                    OnPropertyChanged(nameof(SelectedReports));
                    WarmSelectedOrgAndPrefetchReports();
                });
            }
            catch { /* retry on next select */ }
        });
    }

    public void SetWhiteList(string formNum)
    {
        FormNumWhiteList = FormNumWhiteList != formNum
            ? formNum
            : string.Empty;

        UpdateReportCollection();
        UpdateFormsPageInfo();
    }

    #endregion
}
