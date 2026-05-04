using Client_App.Resources.CustomComparers;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Client_App.ViewModels.MainWindowTabs;

public class Forms1TabControlVM : FormsTabControlBaseVM
{
    #region Fields

    private protected override byte DefaultFormsPerPage => 10;

    private protected override byte DefaultOrgsPerPage => 8;

    private protected override char FormNum => '1';

    #endregion

    #region Constructor

    public Forms1TabControlVM() { }

    public Forms1TabControlVM(MainWindowVM mainWindowVM) : base(mainWindowVM) { }

    #endregion

    #region Properties

    /// <summary>
    /// Всего организаций с учётом фильтра.
    /// </summary>
    private protected override int FilteredRowsOrgs
    {
        get
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                var search = SearchText.ToLower().Trim();
                return StaticConfiguration.DBModel.ReportsCollectionDbSet
                    .AsEnumerable()
                    .Where(x => x.DBObservable != null)
                    .Where(reps => reps.Master_DB.FormNum_DB == "1.0")
                    .Count(reps => reps.Master_DB.RegNoRep.Value.ToLower().Contains(search)
                                   || reps.Master_DB.OkpoRep.Value.ToLower().Contains(search)
                                   || GetAdditionalSearchConditions(reps, search));
            }
            return TotalRowsOrgs;
        }
    }

    protected override bool GetAdditionalSearchConditions(Reports reps, string search)
    {
        return reps.Master_DB.Rows10[0].ShortJurLico_DB.Contains(search, StringComparison.CurrentCultureIgnoreCase)
               || reps.Master_DB.Rows10[1].ShortJurLico_DB.Contains(search, StringComparison.CurrentCultureIgnoreCase);
    }

    #region FormNumWhiteList

    private string _formNumWhiteList = string.Empty;
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

            var result = SelectedReports
                    .Report_Collection
                    .AsEnumerable();

            if (!string.IsNullOrEmpty(FormNumWhiteList))
            {
                result = result.Where(rep => rep.FormNum_DB == FormNumWhiteList);
            }

            result = result.OrderBy(x => 
                {
                    if (int.TryParse(x.FormNum_DB.Split('.')[1], out var result))
                        return result;
                    return int.MinValue;
                })
                // Сортируем по валидным датам, некорректные уходят в начало/конец
                .ThenByDescending(x => x.StartPeriod_DB == null || 
                                       !DateOnly.TryParse(x.StartPeriod_DB, out _) 
                    ? DateOnly.MaxValue
                    : DateOnly.Parse(x.StartPeriod_DB))
                .ThenByDescending(x => x.EndPeriod_DB == null || 
                                       !DateOnly.TryParse(x.EndPeriod_DB, out _)
                    ? DateOnly.MaxValue
                    : DateOnly.Parse(x.EndPeriod_DB))
                .ThenBy(rep => rep.CorrectionNumber_DB)
                .Skip((CurrentPageForms - 1) * RowsCountForms)
                .Take(RowsCountForms);

            return new ObservableCollection<Report>(result);
        }
    }

    private protected override ObservableCollection<Reports> ReportsCollection
    {
        get
        {
            var comparator = new CustomReportsComparer();
            if (!string.IsNullOrEmpty(SearchText))
            {
                var search = SearchText.ToLower().Trim();

                return new ObservableCollection<Reports>(StaticConfiguration.DBModel.ReportsCollectionDbSet
                    .AsEnumerable()
                    .Where(x => x.DBObservable != null)
                    .Where(reps => reps.Master_DB.FormNum_DB == "1.0")
                    .Where(reps => reps.Master_DB.RegNoRep.Value.ToLower().Contains(search)
                                   || reps.Master_DB.OkpoRep.Value.ToLower().Contains(search)
                                   || reps.Master_DB.Rows10[0].ShortJurLico_DB.ToLower().Contains(search)
                                   || reps.Master_DB.Rows10[1].ShortJurLico_DB.ToLower().Contains(search))
                    .OrderBy(reps => reps.Master_DB.RegNoRep.Value, comparator)
                    .ThenBy(reps => reps.Master_DB.OkpoRep.Value, comparator)
                    .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                    .Take(RowsCountOrgs));
            }
            else
            {
                return new ObservableCollection<Reports>(StaticConfiguration.DBModel.ReportsCollectionDbSet
                    .AsEnumerable()
                    .Where(x => x.DBObservable != null)
                    .Where(reps => reps.Master_DB.FormNum_DB == "1.0")
                    .OrderBy(reps => reps.Master_DB.RegNoRep.Value, comparator)
                    .ThenBy(reps => reps.Master_DB.OkpoRep.Value, comparator)
                    .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                    .Take(RowsCountOrgs));
            }
        }
    }

    private protected override Dictionary<string, Func<IQueryable<Report>, IQueryable<object>>> RowSelectors
    {
        get
        {
            return new Dictionary<string, Func<IQueryable<Report>, IQueryable<object>>>
            {
                ["1.1"] = q => q.Include(x => x.Rows11).SelectMany(x => x.Rows11),
                ["1.2"] = q => q.Include(x => x.Rows12).SelectMany(x => x.Rows12),
                ["1.3"] = q => q.Include(x => x.Rows13).SelectMany(x => x.Rows13),
                ["1.4"] = q => q.Include(x => x.Rows14).SelectMany(x => x.Rows14),
                ["1.5"] = q => q.Include(x => x.Rows15).SelectMany(x => x.Rows15),
                ["1.6"] = q => q.Include(x => x.Rows16).SelectMany(x => x.Rows16),
                ["1.7"] = q => q.Include(x => x.Rows17).SelectMany(x => x.Rows17),
                ["1.8"] = q => q.Include(x => x.Rows18).SelectMany(x => x.Rows18),
                ["1.9"] = q => q.Include(x => x.Rows19).SelectMany(x => x.Rows19),
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

            if (!string.IsNullOrEmpty(FormNumWhiteList))
                return SelectedReports.Report_Collection.Count(rep => rep.FormNum_DB == FormNumWhiteList);

            return SelectedReports.Report_Collection.Count;
        }
    }

    #endregion

    #region Functions

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
        OnPropertyChanged(nameof(TotalReportCount));
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