using Client_App.ViewModels.Forms;
using Models.Collections;
using ReactiveUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client_App.ViewModels.Controls;

public class SelectReportPopupVM : INotifyPropertyChanged
{
    #region Constructor

    public SelectReportPopupVM(BaseFormVM formVM)
    {
        _formVM = formVM;
        _reportCollection = FormVM.Report.Reports.Report_Collection.ToList().FindAll(x => x.FormNum.Value == formVM.FormType);
        _selectedReport = Report;
        OpenPopupCommand = ReactiveCommand.Create(() =>
        {
            PopupIsOpen = !PopupIsOpen;
        });
        SwitchNextReportCommand = ReactiveCommand.Create(() =>
        {
            int index = ReportCollection.IndexOf(SelectedReport);
            if (index - 1 >= 0)
                SelectedReport = ReportCollection[index - 1];
        });
        SwitchPreviousReportCommand = ReactiveCommand.Create(() =>
        {
            var index = ReportCollection.IndexOf(SelectedReport);
            if (index + 1 < ReportCollection.Count)
                SelectedReport = ReportCollection[index + 1];
        });
    }

    #endregion

    #region Commands

    public ICommand OpenPopupCommand { get; }

    public ICommand SwitchNextReportCommand { get; }

    public ICommand SwitchPreviousReportCommand { get; }

    #endregion

    #region Properties

    private bool _popupIsOpen;
    public bool PopupIsOpen
    {
        get => _popupIsOpen;
        set
        {
            _popupIsOpen = value;
            OnPropertyChanged();
        }
    }

    private Report _selectedReport;
    public Report SelectedReport
    {
        get => _selectedReport;
        set
        {
            _selectedReport = value;
            OnPropertyChanged();
        }
    }

    private BaseFormVM _formVM;
    public BaseFormVM FormVM => _formVM;

    public Report Report => FormVM.Report;

    private List<Report> _reportCollection;
    public List<Report> ReportCollection => _reportCollection; 
   
    #endregion

    #region OnPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}