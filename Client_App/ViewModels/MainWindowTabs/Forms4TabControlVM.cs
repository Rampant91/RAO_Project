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
    public class Forms4TabControlVM : INotifyPropertyChanged
    {
        #region Constructor
        public Forms4TabControlVM (MainWindowVM mainWindowVM)
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

        #region Reports40

        public ObservableCollection<Reports> Reports40 => ReportsStorage.LocalReports.Reports_Collection40;

        #endregion


        #region ReportsCollection
        public ObservableCollection<Reports> ReportsCollection
        {
            get
            {
                if (!string.IsNullOrEmpty(SearchText))
                {
                    var search = SearchText.ToLower().Trim();
                    return new ObservableCollection<Reports>(Reports40
                    .Where(reps => reps.Master_DB.Rows40[0].CodeSubjectRF_DB.ToString().Contains(search)
                    || reps.Master_DB.Rows40[0].SubjectRF_DB.ToLower().Contains(search)
                    || (!string.IsNullOrEmpty(reps.Master_DB.Rows40[0].ShortNameOrganUprav_DB)
                       && reps.Master_DB.Rows40[0].ShortNameOrganUprav_DB.ToLower().Contains(search))
                      )
                    .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                    .Take(RowsCountOrgs));
                }
                else
                    return new ObservableCollection<Reports>(Reports40
                    .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                    .Take(RowsCountOrgs));
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
                OnPropertyChanged(nameof(ReportCollection));
                UpdatePageInfo();
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
                return Reports40.Count;
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

                CurrentPageForms = 1;
                return new ObservableCollection<Report>(
                    SelectedReports
                    .Report_Collection
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
                UpdatePageInfo();
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
            }
        }
        #endregion


        #region TotalForms
        public int TotalForms
        {
            get
            {

                var result = StaticConfiguration.DBModel.ReportCollectionDbSet
                    .Where(rep => rep.FormNum_DB.StartsWith($"{SelectedReportType}")
                        && !rep.FormNum_DB.EndsWith(".0"))
                    .CountAsync().Result;
                return result;
            }
        }
        #endregion

        #region NumFormInReport
        public int NumFormInReport
        {
            get
            {
                if (SelectedReport != null)
                    return ReportsStorage.GetReportRowsCount(SelectedReport).Result;
                return 0;
            }
        }
        #endregion

        #endregion

        #region Functions

        #region UpdatePageInfo
        public void UpdatePageInfo()
        {
            OnPropertyChanged(nameof(TotalRowsOrgs));
            OnPropertyChanged(nameof(TotalPagesOrgs));

            OnPropertyChanged(nameof(TotalRowsForms));
            OnPropertyChanged(nameof(TotalPagesForms));

            OnPropertyChanged(nameof(TotalForms));
            OnPropertyChanged(nameof(NumFormInReport));
        }
        #endregion

        #region UpdateReport
        public void UpdateReport()
        {
            OnPropertyChanged(nameof(ReportCollection));
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
