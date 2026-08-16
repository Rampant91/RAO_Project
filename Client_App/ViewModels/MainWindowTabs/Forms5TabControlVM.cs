using Client_App.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Client_App.ViewModels.MainWindowTabs;

public class Forms5TabControlVM : FormsTabControlBaseVM
{
    #region Fields

    private protected override byte DefaultFormsPerPage => 10;

    private protected override byte DefaultOrgsPerPage => 8;

    private protected override char FormNum => '5';

    #endregion

    #region Constructor

    public Forms5TabControlVM() { }

    public Forms5TabControlVM(MainWindowVM mainWindowVM) : base(mainWindowVM) { }

    #endregion

    #region Properties

    private protected override int FilteredRowsOrgs
    {
        get
        {
            return MainWindowListQuery.GetOrgPageForm50(
                StaticConfiguration.DBModel, SearchText, page: 1, pageSize: 1).TotalCount;
        }
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
                string.IsNullOrEmpty(FormNumWhiteList) ? null : FormNumWhiteList,
                CurrentPageForms,
                RowsCountForms);

            return new ObservableCollection<Report>(page);
        }
    }

    private protected override ObservableCollection<Reports> ReportsCollection
    {
        get
        {
            var page = MainWindowListQuery.GetOrgPageForm50(
                StaticConfiguration.DBModel,
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
            var result = TotalRowsOrgs / RowsCountOrgs;
            if (TotalRowsOrgs % RowsCountOrgs > 0)
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

    public void SetWhiteList(string formNum)
    {
        if (FormNumWhiteList != formNum)
            FormNumWhiteList = formNum;
        else
            FormNumWhiteList = "";

        UpdateReportCollection();
        UpdateFormsPageInfo();
    }

    private protected override void NotifySearchTextChanged()
    {
        OnPropertyChanged(nameof(ReportsCollection));
        OnPropertyChanged(nameof(TotalPagesOrgs));
    }

    public void UpdateOrgsPageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsForms));
        OnPropertyChanged(nameof(TotalPagesForms));

    }

    #endregion
}