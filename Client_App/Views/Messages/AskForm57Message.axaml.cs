using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Input;

namespace Client_App;

public partial class AskForm57Message : Window, INotifyPropertyChanged
{
    private ObservableCollection<Report> _reportList;
    public ObservableCollection<Report> ReportList
    {
        get
        {
            return _reportList;
        }
        set
        {
            _reportList = value;
            OnPropertyChanged();
        }
    }

    private Report _selectedReport;
    public Report SelectedReport
    {
        get
        {
            return _selectedReport;
        }
        set
        {
            _selectedReport = value;
            OnPropertyChanged();
        }
    }

    private Reports _reports;
    public Reports Reports
    {
        get
        {
            return _reports;
        }
        set
        {
            _reports = value;
            OnPropertyChanged();
        }
    }

    public AskForm57Message(Report report)
    {
        Reports = report.Reports;
        ReportList = new ObservableCollection<Report>(Reports.Report_Collection
                .OrderBy(x => x.FormNum_DB)
                .ThenByDescending(x =>
                x.Year_DB == null
                || !int.TryParse(x.Year_DB, out _)
                ? int.MaxValue
                : int.Parse(x.Year_DB))
                .ThenByDescending(rep => rep.CorrectionNumber_DB));
        ReportList.Remove(report);

        DataContext = this;
        AvaloniaXamlLoader.Load(this);

        
    }
    public AskForm57Message()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = this;

        Reports = new Reports();
        ReportList = Reports.Report_Collection;
    }
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        this.Cursor = new Cursor(StandardCursorType.Wait);
        Report? result = SelectedReport;
        if (result != null)
        {
            var dBModel = StaticConfiguration.DBModel;
            var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Rows57)
                            .FirstOrDefault(x => x.Id == result.Id);
            result.Rows57 = dbReport.Rows57;
        }
        // Return the integer result from ViewModel

        this.Cursor = new Cursor(StandardCursorType.Arrow);
        Close(result);
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        // Return a cancellation indicator (could use null or sentinel value)
        Close(null);
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}