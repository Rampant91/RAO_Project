using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.ViewModels.MainWindowTabs;

public class Forms5TabControlVM : FormsTabControlBaseVM
{
    #region Constructor

    public Forms5TabControlVM() { }

    public Forms5TabControlVM(MainWindowVM mainWindowVM) : base(mainWindowVM) { }

    #endregion

    #region Properties

    private protected override char FormNum => '5';

    private protected override int InSelectedReportFormsCount
    {
        get
        {
            if (SelectedReport is null) return 0;
            return StaticConfiguration.DBModel.ReportCollectionDbSet
                .Include(rep => rep.Rows51)
                .Include(rep => rep.Rows52)
                .Include(rep => rep.Rows53)
                .Include(rep => rep.Rows54)
                .Include(rep => rep.Rows55)
                .Include(rep => rep.Rows56)
                .Include(rep => rep.Rows57)
                .FirstOrDefault(rep => rep.Id == SelectedReport.Id)
                ?.Rows.Count ?? 0;
        }
    }

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
                                       !DateOnly.TryParse(x.StartPeriod_DB, out _) ?
                    DateOnly.MaxValue :
                    DateOnly.Parse(x.StartPeriod_DB))
                .ThenByDescending(x => x.EndPeriod_DB == null ||
                                       !DateOnly.TryParse(x.EndPeriod_DB, out _) ?
                    DateOnly.MaxValue :
                    DateOnly.Parse(x.EndPeriod_DB))
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

    public int RowsCountForms
    {
        get => _rowsCountForms;
        set
        {
            _rowsCountForms = value;
            OnPropertyChanged();
            UpdateReportCollection();
            OnPropertyChanged(nameof(TotalPagesForms));
        }
    }

    private protected override string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText == value) return;

            _searchText = value;
            OnPropertyChanged();

            // Отменяем предыдущий таймер
            DebounceCts?.Cancel();

            // Создаем новый таймер
            DebounceCts = new CancellationTokenSource();

            // Задержка 300мс перед фильтрацией
            Task.Delay(300, DebounceCts.Token)
                .ContinueWith(t =>
                {
                    if (!t.IsCanceled)
                    {
                        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            if (CurrentPageOrgs != 1)
                                CurrentPageOrgs = 1;

                            OnPropertyChanged(nameof(ReportsCollection));
                            OnPropertyChanged(nameof(TotalPagesOrgs));
                        });
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    #region PaginationOrgs

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

    private protected override int TotalRowsOrgs => StaticConfiguration.DBModel.ReportsCollectionDbSet
        .Where(x => x.DBObservable != null)
        .Count(reps => reps.Master_DB.FormNum_DB == "5.0");

    private new int _rowsCountOrgs = 10;
    public override int RowsCountOrgs
    {
        get => _rowsCountOrgs;
        set
        {
            if (_rowsCountOrgs != value)
            {
                _rowsCountOrgs = value;
                NotifyRowsChanged();
            }
        }
    }

    #endregion

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

    #region PaginationForms
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

    #endregion

    #region Functions

    public void GoToFormNum(string formNum)
    {
        if (FormNumWhiteList != formNum)
            FormNumWhiteList = formNum;
        else
            FormNumWhiteList = "";

        UpdateReportCollection();
        UpdateFormsPageInfo();
    }

    public override void UpdateOrgsPageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsForms));
        OnPropertyChanged(nameof(TotalPagesForms));

    }

    #endregion
}