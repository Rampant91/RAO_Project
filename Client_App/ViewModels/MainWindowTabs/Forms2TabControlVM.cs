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
using System.Windows.Input;
using Client_App.Commands.AsyncCommands.Add;

namespace Client_App.ViewModels.MainWindowTabs;

public class Forms2TabControlVM : FormsTabControlBaseVM
{
    #region Fields

    private protected override byte DefaultFormsPerPage => 10;

    private protected override byte DefaultOrgsPerPage => 8;

    private protected override char FormNum => '2';

    #endregion

    #region Commands

    /// <summary>
    /// Создать и открыть новое окно с отчётом по форме x.x для выбранной организации (использует старую команду).
    /// </summary>
    public ICommand OldAddReport { get; private set; }

    /// <summary>
    /// Проверить выбранный отчёт из главного окна
    /// </summary>
    public ICommand CheckReportFromMain { get; private set; }

    /// <summary>
    /// Выбранная форма -> Выгрузка Excel -> Для анализа
    /// </summary>
    public ICommand ExcelExportFormAnalysis { get; private set; }

    /// <summary>
    /// Выбранная организация -> Импортировать отчёт в организацию -> Из Excel
    /// </summary>
    public ICommand ImportExcel { get; private set; }

    /// <summary>
    /// Выбранная организация -> Импортировать отчёт в организацию -> Из Raodb
    /// </summary>
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

    private protected override int FilteredRowsOrgs
    {
        get
        {
            return MainWindowListQuery.CountOrgsForm12(
                StaticConfiguration.DBModel, "2.0", SearchText);
        }
    }

    protected override bool GetAdditionalSearchConditions(Reports reps, string search)
    {
        return reps.Master_DB.Rows20[0].ShortJurLico_DB.ToLower().Contains(search, StringComparison.CurrentCultureIgnoreCase)
               || reps.Master_DB.Rows20[1].ShortJurLico_DB.ToLower().Contains(search, StringComparison.CurrentCultureIgnoreCase);
    }

    protected override void CheckAndResetFilterIfNeeded()
    {
        if (string.IsNullOrEmpty(FormNumWhiteList) || SelectedReports == null)
            return;

        var hasMatchingReports = OrgReportsQuery.HasFormNumAsync(
                StaticConfiguration.DBModel, SelectedReports.Id, FormNumWhiteList)
            .GetAwaiter().GetResult();

        if (!hasMatchingReports)
            FormNumWhiteList = string.Empty;
    }

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

    private protected override ObservableCollection<Report> ReportCollection
    {
        get
        {
            if (SelectedReports is null) return null;

            var page = MainWindowListQuery.GetReportPage(
                StaticConfiguration.DBModel,
                SelectedReports.Id,
                FormNumWhiteList,
                CurrentPageForms,
                RowsCountForms,
                orderByYear: true);

            return new ObservableCollection<Report>(page);
        }
    }

    private protected override ObservableCollection<Reports> ReportsCollection
    {
        get
        {
            var page = MainWindowListQuery.GetOrgPageForm12(
                StaticConfiguration.DBModel,
                "2.0",
                SearchText,
                CurrentPageOrgs,
                RowsCountOrgs);
            return new ObservableCollection<Reports>(page.Items);
        }
    }

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

    private protected override int TotalPagesForms
    {
        get
        {
            var result = TotalRowsForms / RowsCountForms;
            if (TotalRowsForms % RowsCountForms > 0)
                result++;
            return result;
        }
    }

    private protected override int TotalPagesOrgs
    {
        get
        {
            var result = FilteredRowsOrgs / RowsCountOrgs;
            if (FilteredRowsOrgs % RowsCountOrgs > 0)
                result++;
            return result;
        }
    }

    private protected override int TotalRowsForms
    {
        get
        {
            if (SelectedReports is null) return 0;

            return MainWindowListQuery.CountReports(
                StaticConfiguration.DBModel,
                SelectedReports.Id,
                string.IsNullOrEmpty(FormNumWhiteList) ? null : FormNumWhiteList);
        }
    }

    #endregion

    #region Functions

    /// <summary>
    /// Возвращает количество строчек форм у отчёта.
    /// </summary>
    /// <param name="rep">Отчёт, у которого нужно посчитать количество строчек форм.</param>
    /// <returns>Количество строчек форм.</returns>
    private static int GetReportRowsCount(Report? rep)
    {
        if (rep == null) return 0;
        while (StaticConfiguration.IsFileLocked(null)) Thread.Sleep(50); 
        
        using var db = new DBModel(StaticConfiguration.DBPath);

        var query = db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id);

        var result = rep.FormNum_DB switch
        {
            "2.1" => query.Include(x => x.Rows21)
                .SelectMany(x => x.Rows21)
                .Count(),

            "2.2" => query.Include(x => x.Rows22)
                .SelectMany(x => x.Rows22)
                .Count(),

            "2.3" => query.Include(x => x.Rows23)
                .SelectMany(x => x.Rows23)
                .Count(),

            "2.4" => query.Include(x => x.Rows24)
                .SelectMany(x => x.Rows24)
                .Count(),

            "2.5" => query.Include(x => x.Rows25)
                .SelectMany(x => x.Rows25)
                .Count(),

            "2.6" => query.Include(x => x.Rows26)
                .SelectMany(x => x.Rows26)
                .Count(),

            "2.7" => query.Include(x => x.Rows27)
                .SelectMany(x => x.Rows27)
                .Count(),

            "2.8" => query.Include(x => x.Rows28)
                .SelectMany(x => x.Rows28)
                .Count(),

            "2.9" => query.Include(x => x.Rows29)
                .SelectMany(x => x.Rows29)
                .Count(),

            "2.10" => query.Include(x => x.Rows210)
                .SelectMany(x => x.Rows210)
                .Count(),

            "2.11" => query.Include(x => x.Rows211)
                .SelectMany(x => x.Rows211)
                .Count(),

            "2.12" => query.Include(x => x.Rows212)
                .SelectMany(x => x.Rows212)
                .Count(),

            _ => 0
        };
        return result;
    }

    public void SetWhiteList(string formNum)
    {
        FormNumWhiteList = FormNumWhiteList != formNum
            ? formNum
            : string.Empty;

        UpdateReportCollection();
        UpdateFormsPageInfo();
    }

    private protected override void NotifySearchTextChanged()
    {
        OnPropertyChanged(nameof(ReportsCollection));
        OnPropertyChanged(nameof(FilteredRowsOrgs));
        OnPropertyChanged(nameof(TotalPagesOrgs));
    }

    public void UpdateOrgsPageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsOrgs));
        OnPropertyChanged(nameof(TotalPagesOrgs));
        UpdateTotalReportCount();
        UpdateTotalReportsCount();
    }

    #endregion
}