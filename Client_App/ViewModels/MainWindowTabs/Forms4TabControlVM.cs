using Client_App.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Client_App.ViewModels.MainWindowTabs;

public class Forms4TabControlVM : FormsTabControlBaseVM
{
    #region Fields

    private protected override byte DefaultFormsPerPage => 10;

    private protected override byte DefaultOrgsPerPage => 8;

    private protected override char FormNum => '4';

    #endregion

    #region Constructor

    public Forms4TabControlVM() { }

    public Forms4TabControlVM(MainWindowVM mainWindowVM) : base(mainWindowVM) { }

    #endregion

    #region Properties

    private protected override int FilteredRowsOrgs
    {
        get
        {
            var page = MainWindowListQuery.GetOrgPageForm40(
                StaticConfiguration.DBModel, SearchText, page: 1, pageSize: 1);
            return page.TotalCount;
        }
    }

    private protected override ObservableCollection<Report> ReportCollection
    {
        get
        {
            if (SelectedReports is null) return null;

            var page = MainWindowListQuery.GetReportPage(
                StaticConfiguration.DBModel,
                SelectedReports.Id,
                formNumWhiteList: null,
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
            var page = MainWindowListQuery.GetOrgPageForm40(
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
                ["4.1"] = q => q.Include(x => x.Rows41).SelectMany(x => x.Rows41)
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
            if (SelectedReports != null)
                return MainWindowListQuery.CountReports(StaticConfiguration.DBModel, SelectedReports.Id);
            return 0;
        }
    }

    #endregion

    #region Functions

    private protected override void NotifySearchTextChanged()
    {
        OnPropertyChanged(nameof(ReportsCollection));
        OnPropertyChanged(nameof(FilteredRowsOrgs));
        OnPropertyChanged(nameof(TotalPagesOrgs));
    }

    public void UpdateOrgsPageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsForms));
        OnPropertyChanged(nameof(TotalPagesForms));

    }

    #endregion
}