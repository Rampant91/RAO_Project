using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.MainWindowTabs
{
    public class Forms5TabControlVM : INotifyPropertyChanged
    {
        #region Constructor
        public Forms5TabControlVM (MainWindowVM mainWindowVM)
        {
            _mainWindowVM = mainWindowVM;
        }
        #endregion

        #region Properties

        #region MainWindowVM
        private MainWindowVM _mainWindowVM;
        public MainWindowVM MainWindowVM
        {
            get
            {
                return _mainWindowVM;
            }
        }
        #endregion

        #region SearchText
        private string _searchText = "";

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ReportsCollection));
            }
        }
        #endregion


        #region ReportsCollection
        public ObservableCollection<Reports> ReportsCollection
        {
            get
            {
                if (!string.IsNullOrEmpty(SearchText))
                {
                    var search = SearchText.ToLower().Trim();
                    return new ObservableCollection<Reports>(StaticConfiguration.DBModel.ReportsCollectionDbSet
                        .AsEnumerable()
                        .Where(reps =>
                        (!string.IsNullOrEmpty(reps.Master_DB.Rows50[0].ShortName_DB)
                           && reps.Master_DB.Rows50[0].ShortName_DB.ToLower().Contains(search)))
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
                    ;
                    return result;
                }
            }
        }
        #endregion

        #region SelectedReports

        private Reports? _selectedReports;
        public Reports? SelectedReports
        {
            get
            {
                return _selectedReports;
            }
            set
            {
                _selectedReports = value;
                OnPropertyChanged();

                // UpdateReportCollection выполняется в CurrentPageForms
                // Чтобы не вызывать метод дважды используется if else
                if (CurrentPageForms != 1)
                    CurrentPageForms = 1;
                else
                    UpdateReportCollection();

                UpdateFormsPageInfo();
            }
        }
        #endregion

        #region PaginationOrgs

        public int TotalPagesOrgs
        {
            get
            {
                var result = TotalRowsOrgs / RowsCountOrgs;
                if (TotalRowsOrgs % RowsCountOrgs > 0)
                    result++;
                return result;
            }
        }
        public int TotalRowsOrgs
        {
            get
            {
                return StaticConfiguration.DBModel.ReportsCollectionDbSet
                    .Where(reps => reps.Master_DB.FormNum_DB == "5.0").Count();
            }
        }

        private int _rowsCountOrgs = 10;
        public int RowsCountOrgs
        {
            get
            {
                return _rowsCountOrgs;
            }
            set
            {
                _rowsCountOrgs = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ReportsCollection));
                OnPropertyChanged(nameof(TotalPagesOrgs));
            }
        }


        private int _currentPageOrgs = 1;
        public int CurrentPageOrgs
        {
            get
            {
                if (_currentPageOrgs > TotalPagesOrgs)
                    _currentPageOrgs = TotalPagesOrgs;
                return _currentPageOrgs;
            }
            set
            {
                _currentPageOrgs = value;
                OnPropertyChanged(nameof(ReportsCollection));
                OnPropertyChanged();
            }
        }
        #endregion

        #region ReportCollection

        public ObservableCollection<Report> ReportCollection
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


                result.OrderBy(x =>
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

        #endregion

        #region FormNumWhiteList
        private string _formNumWhiteList = "";
        public string FormNumWhiteList
        {
            get
            {
                return _formNumWhiteList;
            }
            set
            {
                _formNumWhiteList = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region SelectedReport

        private Report? _selectedReport;
        public Report? SelectedReport
        {
            get
            {
                return _selectedReport;
            }
            set
            {
                _selectedReport = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(InSelectedReportFormsCount));
            }
        }

        #endregion

        #region PaginationForms
        public int TotalPagesForms
        {
            get
            {
                var result = TotalRowsForms / RowsCountForms;
                if (TotalRowsForms % RowsCountForms > 0)
                    result++;
                return result;
            }
        }
        public int TotalRowsForms
        {
            get
            {
                if (SelectedReports is null) return 0;

                if (!string.IsNullOrEmpty(FormNumWhiteList))
                    return SelectedReports.Report_Collection.Where(rep => rep.FormNum_DB == FormNumWhiteList).Count();

                return SelectedReports.Report_Collection.Count;
            }
        }


        private int _rowsCountForms = 10;
        public int RowsCountForms
        {
            get
            {
                return _rowsCountForms;
            }
            set
            {
                _rowsCountForms = value;
                OnPropertyChanged();
                UpdateReportCollection();
                OnPropertyChanged(nameof(TotalPagesForms));
            }
        }


        private int _currentPageForms = 1;
        public int CurrentPageForms
        {
            get
            {
                if (_currentPageForms > TotalPagesForms)
                    _currentPageForms = TotalPagesForms;

                return _currentPageForms;
            }
            set
            {
                _currentPageForms = value;
                OnPropertyChanged();
                UpdateReportCollection();
            }
        }
        #endregion

        #region TotalReportCount
        public int TotalReportCount
        {
            get
            {
                return StaticConfiguration.DBModel.ReportCollectionDbSet
                    .Where(rep => rep.FormNum_DB.StartsWith($"{MainWindowVM.SelectedReportType}")
                        && !rep.FormNum_DB.EndsWith(".0"))
                    .CountAsync().Result;
            }
        }
        #endregion

        #region InSelectedReportFormsCount
        public int InSelectedReportFormsCount
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
                    .Rows.Count;
            }
        }
        #endregion

        #endregion

        #region Functions

        #region GoToFormNum
        public void GoToFormNum(string formNum)
        {
            if (FormNumWhiteList != formNum)
                FormNumWhiteList = formNum;
            else
                FormNumWhiteList = "";

            UpdateReportCollection();
            UpdateFormsPageInfo();
        }
        #endregion

        #region UpdateOrgsPageInfo
        public void UpdateOrgsPageInfo()
        {
            OnPropertyChanged(nameof(TotalRowsForms));
            OnPropertyChanged(nameof(TotalPagesForms));

        }
        #endregion

        #region UpdateFormsPageInfo
        public void UpdateFormsPageInfo()
        {
            OnPropertyChanged(nameof(TotalRowsForms));
            OnPropertyChanged(nameof(TotalPagesForms));

        }
        #endregion

        #region TotalReportCount
        public void UpdateTotalReportCount()
        {
            OnPropertyChanged(nameof(TotalReportCount));
        }
        #endregion

        #region UpdateReportCollection
        public void UpdateReportCollection()
        {
            OnPropertyChanged(nameof(ReportCollection));
        }

        #endregion

        #region UpdateReportsCollection
        public void UpdateReportsCollection()
        {
            OnPropertyChanged(nameof(ReportsCollection));
        }

        #endregion

        #endregion

        #region INotifyPropertyChanged

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
