using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input; 
using Client_App.Commands.AsyncCommands.TmpNewCommands;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_12VM : BaseVM, INotifyPropertyChanged
{
    private ObservableCollection<Form12> _formList = [];
    public ObservableCollection<Form12> FormList 
    {
        get
        {
            return _formList;
        }
        set
        {
            _formList = value;
            OnPropertyChanged();
        }
    }
    private ObservableCollection<Note> _noteList = [];
    public ObservableCollection<Note> NoteList 
    {
        get => _noteList;
        set
        {
            _noteList = value;
            OnPropertyChanged();
        }
    }

    public Reports CurrentReports => _currentReport.Reports;

    private Report _currentReport;
    public Report CurrentReport
    {
        get => _currentReport;
        set
        {
            _currentReport = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Form12> _selectedForms = new ObservableCollection<Form12>();
    public ObservableCollection<Form12> SelectedForms
    {
        get => _selectedForms;
        set
        {
            _selectedForms = value ?? new ObservableCollection<Form12>();
            OnPropertyChanged();
        }
    }

    private int _rowCount = 30;
    public int RowCount
    {
        get => _rowCount;
        set
        {
            if (value <= 0)
                _rowCount = 1;
            else
                _rowCount = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(PageCount));
            FormList = GetFormList(CurrentPage, RowCount);
        }
    }

    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            if (value<=0)
                _currentPage = 1;
            else if(value>PageCount)
                _currentPage = PageCount;
            else
                _currentPage = value;
            OnPropertyChanged();
            FormList = GetFormList(CurrentPage, RowCount);
        }
    }

    public int PageCount
    {
        get
        {
            var result = CurrentReport.Rows12.Count / RowCount;
            if (CurrentReport.Rows12.Count % RowCount != 0)
                result++;
            return result;
        }

    }
    private bool _isHeaderExpanded =true;
    public bool IsHeaderExpanded
    {
        get => _isHeaderExpanded;
        set
        {
            _isHeaderExpanded = value;
            OnPropertyChanged();
        }
    }

    #region Commands

    public ICommand DeleteDataInRows{ get; set; }                        //  Удаляет данные из выбранных ячеек
    public ICommand SourceTransmission { get; set; }                        //  Удаляет данные из выбранных ячеек

    #endregion

    public Form_12VM()
    {
        SelectedForms = new ObservableCollection<Form12>();
        InitCommands();
    }
    public Form_12VM(Report report) 
    {
        _currentReport = report;
        FormList = GetFormList(CurrentPage, RowCount);
        NoteList = CurrentReport.Notes;
        SelectedForms = new ObservableCollection<Form12>();
        InitCommands();
    }

    private void InitCommands()
    {
        DeleteDataInRows = new NewDeleteDataInRowsAsyncCommand();
        SourceTransmission = new NewSourceTransmissionAsyncCommand(this);
    }

    private ObservableCollection<Form12> GetFormList (int page, int rowCount)
    {

        ObservableCollection<Form12> formList = new ObservableCollection<Form12>( CurrentReport.Rows12.Skip((page-1)*rowCount).Take(rowCount));
        return formList; 
    }
}