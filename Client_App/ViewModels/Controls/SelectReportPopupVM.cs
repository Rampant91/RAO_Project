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

        _reportCollection = FormVM.Report.Reports.Report_Collection.ToList();

        _selectedReport = Report;
        CurrentFormNum = Report.FormNum_DB;

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
            int index = ReportCollection.IndexOf(SelectedReport);
            if (index + 1 < ReportCollection.Count)
                SelectedReport = ReportCollection[index + 1];
        });
        SetCurrentFormNum = ReactiveCommand.Create((string newFormNum) =>
        {
            CurrentFormNum = newFormNum;
        });
    }

    #endregion

    #region Commands

    public ICommand OpenPopupCommand { get; }

    public ICommand SwitchNextReportCommand { get; }

    public ICommand SwitchPreviousReportCommand { get; }
    public ICommand SetCurrentFormNum { get; }

    #endregion

    #region Properties

    #region PopupIsOpen
    private bool _popupIsOpen = false;
    public bool PopupIsOpen
    {
        get => _popupIsOpen;
        set
        {
            _popupIsOpen = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region YearMode
    public bool YearMode
    {
        get
        {
            var formType = FormVM.FormType;
            if (formType[0] is '2' or '4')
                return true;

            return false;
        }
    }
    #endregion

    public bool IsForm1
    {
        get
        {
            return FormVM.FormType[0] is '1';
        }
    }
    public bool IsForm2
    {
        get
        {
            return FormVM.FormType[0] is '2';
        }
    }

    #region CurrentFormNum
    private string _currentFormNum;
    public string CurrentFormNum
    {
        get
        {
            return _currentFormNum;
        }
        set
        {
            _currentFormNum = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ReportCollection));
            if (_currentFormNum == Report.FormNum_DB)
                SelectedReport = Report;
        }
    }
    #endregion

    #region SelectedReport
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
    #endregion

    #region BaseFormVM
    private BaseFormVM _formVM;
    public BaseFormVM FormVM => _formVM;
    #endregion

    #region Report
    public Report Report => FormVM.Report;
    #endregion

    #region ReportCollections
    private List<Report> _reportCollection;

    public List<Report> ReportCollection
    {
        get
        {
            if (!string.IsNullOrEmpty(CurrentFormNum))
                return _reportCollection.FindAll(x => x.FormNum.Value == CurrentFormNum);
            else
                return _reportCollection;
        }
    }

    #endregion

    #endregion


    #region OnPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}