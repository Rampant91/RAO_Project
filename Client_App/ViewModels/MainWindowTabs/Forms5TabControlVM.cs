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
            if (!string.IsNullOrEmpty(SearchText))
            {
                var search = SearchText.ToLower().Trim();
                return new ObservableCollection<Reports>(StaticConfiguration.DBModel.ReportsCollectionDbSet
                    .AsEnumerable()
                    .Where(reps =>
                        (!string.IsNullOrEmpty(reps.Master_DB.Rows50[0].ShortName_DB) && reps.Master_DB.Rows50[0].ShortName_DB.ToLower().Contains(search))
                        || (string.IsNullOrEmpty(reps.Master_DB.Rows50[0].ShortName_DB) && reps.Master_DB.Rows50[0].Name_DB.ToLower().Contains(search)))
                    .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                    .Take(RowsCountOrgs));
            }
            else
            {
                var result = new ObservableCollection<Reports>(StaticConfiguration.DBModel.ReportsCollectionDbSet
                    .AsEnumerable()
                    .Where(reps => reps.Master_DB.FormNum_DB == "5.0")
                    .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                    .Take(RowsCountOrgs));

                return result;
            }
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

            if (!string.IsNullOrEmpty(FormNumWhiteList))
                return SelectedReports.Report_Collection.Count(rep => rep.FormNum_DB == FormNumWhiteList);

            return SelectedReports.Report_Collection.Count;
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