using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Client_App.ViewModels.MainWindowTabs;

public abstract class FormsTabControlBaseVM : INotifyPropertyChanged
{
    #region Constructor

    protected FormsTabControlBaseVM()
    {
        // Конструктор пуст - настройки загружаются лениво при первом доступе
    }

    protected FormsTabControlBaseVM(MainWindowVM mainWindowVM)
    {
        MainWindowVM = mainWindowVM;
    }

    #endregion

    #region Properties

    public MainWindowVM MainWindowVM { get; }

    private protected abstract ObservableCollection<Report> ReportCollection { get; }

    private protected abstract ObservableCollection<Reports> ReportsCollection { get; }

    #region SearchText

    private protected CancellationTokenSource? DebounceCts;

    private protected string _searchText = string.Empty;

    private protected abstract string SearchText { get; set; }

    #endregion

    private protected abstract int TotalPagesForms { get; }

    private protected abstract int TotalPagesOrgs { get; }

    private protected abstract int TotalRowsForms { get; }

    private protected abstract int TotalRowsOrgs { get; }

    #region CurrentPageForms

    private int _currentPageForms = 1;
    private protected int CurrentPageForms
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

    #region CurrentPageOrgs

    private int _currentPageOrgs = 1;
    private protected int CurrentPageOrgs
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

    #region RowsCountOrgs

    private protected int _rowsCountOrgs;

    public abstract int RowsCountOrgs { get; set; }

    private protected void NotifyRowsChanged()
    {
        OnPropertyChanged(nameof(RowsCountOrgs));
        OnPropertyChanged(nameof(ReportsCollection));
        OnPropertyChanged(nameof(TotalPagesOrgs));
    }

    #endregion

    #region SelectedReports

    private Reports? _selectedReports;
    public Reports? SelectedReports
    {
        get => _selectedReports;
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

    public int TotalReportCount => StaticConfiguration.DBModel.ReportCollectionDbSet
        .CountAsync(rep => rep.FormNum_DB.StartsWith($"{MainWindowVM.SelectedReportType}")
                           && !rep.FormNum_DB.EndsWith(".0"))
        .Result;

    #endregion

    #region Methods

    public void UpdateFormsPageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsForms));
        OnPropertyChanged(nameof(TotalPagesForms));
    }

    public abstract void UpdateOrgsPageInfo();

    public void UpdateReportCollection()
    {
        OnPropertyChanged(nameof(ReportCollection));
    }

    public void UpdateReportsCollection()
    {
        OnPropertyChanged(nameof(ReportsCollection));
    }

    public void UpdateTotalReportCount()
    {
        OnPropertyChanged(nameof(TotalReportCount));
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}