using Avalonia;
using Client_App.Resources.CustomComparers;
using Client_App.Views;
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
    public class Forms2TabControlVM : INotifyPropertyChanged
    {
        #region Constructor
        public Forms2TabControlVM (MainWindowVM mainWindowVM)
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
                if(CurrentPageOrgs != 1)
                    CurrentPageOrgs = 1;
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
                var comparator = new CustomReportsComparer();
                if (!string.IsNullOrEmpty(SearchText))
                {
                    var search = SearchText.ToLower().Trim();
                    return new ObservableCollection<Reports>(StaticConfiguration.DBModel.ReportsCollectionDbSet
                        .AsEnumerable()
                        .Where(reps => reps.Master_DB.FormNum_DB == "2.0")
                        .Where(reps => reps.Master_DB.RegNoRep.Value.ToLower().Contains(search)
                        || reps.Master_DB.OkpoRep.Value.ToLower().Contains(search)
                        || reps.Master_DB.Rows20[0].ShortJurLico_DB.ToLower().Contains(search)
                        || reps.Master_DB.Rows20[1].ShortJurLico_DB.ToLower().Contains(search))
                        .OrderBy(reps => reps.Master_DB.RegNoRep.Value, comparator)
                        .ThenBy(reps => reps.Master_DB.OkpoRep.Value, comparator)
                        .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                        .Take(RowsCountOrgs).ToList());
                }
                else
                    return new ObservableCollection<Reports>(StaticConfiguration.DBModel.ReportsCollectionDbSet
                        .AsEnumerable()
                        .Where(reps => reps.Master_DB.FormNum_DB == "2.0")
                        .OrderBy(reps => reps.Master_DB.RegNoRep.Value, comparator)
                        .ThenBy(reps => reps.Master_DB.OkpoRep.Value, comparator)
                        .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                        .Take(RowsCountOrgs).ToList());
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
                return StaticConfiguration.DBModel.ReportsCollectionDbSet.CountAsync(reps => reps.Master_DB.FormNum_DB == "2.0").Result;
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

                return new ObservableCollection<Report>(
                    SelectedReports
                    .Report_Collection
                    .AsEnumerable()
                    .OrderBy(x => x.FormNum_DB)
                    .ThenBy(x => x.Year_DB == null ||
                                 !int.TryParse(x.Year_DB, out _) ?
                                 int.MinValue :
                                 int.Parse(x.Year_DB))
                    .Skip((CurrentPageForms - 1) * RowsCountForms)
                    .Take(RowsCountForms));
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
                if (SelectedReports != null)
                    return SelectedReports.Report_Collection.Count;
                return 0;
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
                OnPropertyChanged(nameof(TotalPagesForms)); 
                UpdateReportCollection();
            }
        }


        private int _currentPageForms = 1;
        public int CurrentPageForms
        {
            get
            {
                if ((_currentPageForms > TotalPagesForms) && (TotalPagesForms > 0))
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
                    .CountAsync(rep => rep.FormNum_DB.StartsWith($"{MainWindowVM.SelectedReportType}")
                        && !rep.FormNum_DB.EndsWith(".0")).Result;
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
                    .Include(rep => rep.Rows21)
                    .Include(rep => rep.Rows22)
                    .Include(rep => rep.Rows23)
                    .Include(rep => rep.Rows24)
                    .Include(rep => rep.Rows25)
                    .Include(rep => rep.Rows26)
                    .Include(rep => rep.Rows27)
                    .Include(rep => rep.Rows28)
                    .Include(rep => rep.Rows29)
                    .Include(rep => rep.Rows210)
                    .Include(rep => rep.Rows211)
                    .Include(rep => rep.Rows212)
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
            if (SelectedReports is null) return;

            var report = SelectedReports.Report_Collection.FirstOrDefault(rep => rep.FormNum_DB == formNum);
            if (report == null) return;

            var index = SelectedReports.Report_Collection.IndexOf(report);
            CurrentPageForms = (index / RowsCountForms) + 1;
        }
        #endregion

        #region UpdateOrgsPageInfo
        public void UpdateOrgsPageInfo()
        {
            OnPropertyChanged(nameof(TotalRowsOrgs));
            OnPropertyChanged(nameof(TotalPagesOrgs));
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
