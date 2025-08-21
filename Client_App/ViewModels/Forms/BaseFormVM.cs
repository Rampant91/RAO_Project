using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Commands.AsyncCommands.Delete;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.Commands.SyncCommands;
using Models.Collections;
using Models.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms;

public abstract class BaseFormVM : BaseVM, INotifyPropertyChanged
{
    #region Properties

    public abstract string FormType { get; }
    public string WindowTitle
    {
        get
        {
            return $"Форма {FormType} :  "
                   + $"{CurrentReports.Master_DB.RegNoRep.Value}  "
                   + $"{CurrentReports.Master_DB.ShortJurLicoRep.Value}  "
                   + $"{CurrentReports.Master_DB.OkpoRep.Value}";
        }
    }
    protected ObservableCollection<Form> _formList = [];
    public ObservableCollection<Form> FormList
    {
        get => _formList;
        set
        {
            _formList = value;
            OnPropertyChanged();
        }
    }
    protected ObservableCollection<Note> _noteList = [];
    public ObservableCollection<Note> NoteList
    {
        get => _noteList;
        set
        {
            _noteList = value;
            OnPropertyChanged();
        }
    }
    protected Report _currentReport;
    public Report CurrentReport
    {
        get => _currentReport;
        set
        {
            _currentReport = value;
            OnPropertyChanged();
        }
    }
    public Reports CurrentReports => _currentReport.Reports;

    protected Form _selectedForm;
    public Form SelectedForm
    {
        get => _selectedForm;
        set
        {
            _selectedForm = value;
            OnPropertyChanged();
        }
    }
    protected ObservableCollection<Form> _selectedForms = [];
    public ObservableCollection<Form> SelectedForms
    {
        get => _selectedForms;
        set
        {
            if (_selectedForms != value)
            {
                _selectedForms = value;
                OnPropertyChanged();
            }
        }
    }
    protected int _rowCount = 30;
    public int RowCount
    {
        get => _rowCount;
        set
        {
            var result = value;
            if (value <= 0)
                result = 1;
            else if (value > 50) //Хардкод максимального кол-ва загруженных строк
                result = 50;

            if (_rowCount != result)
            {
                _rowCount = result;
                OnPropertyChanged();
                UpdateFormList();
                UpdatePageInfo();
            }
        }
    }

    protected int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            var result = value;
            if (value <= 0)
                result = 1;
            else if (value > TotalPages)
                result = TotalPages;

            if (result != _currentPage)
            {
                _currentPage = result;
                OnPropertyChanged();
                UpdateFormList();
                UpdatePageInfo();
            }
        }
    }

    public int TotalPages
    {
        get
        {
            var result = CurrentReport.Rows.Count / RowCount;
            if (CurrentReport.Rows.Count % RowCount != 0)
                result++;
            return result;
        }

    }
    public int TotalRows => CurrentReport.Rows.Count;

    protected bool _isAutoReplaceEnabled = true;
    public bool IsAutoReplaceEnabled
    {
        get => _isAutoReplaceEnabled;
        set
        {
            if (_isAutoReplaceEnabled != value)
            {
                _isAutoReplaceEnabled = value;
                CurrentReport.AutoReplace = value;
                OnPropertyChanged();
            }
        }
    }
    protected bool _isHeaderExpanded = true;
    public bool IsHeaderExpanded
    {
        get => _isHeaderExpanded;
        set
        {
            _isHeaderExpanded = value;
            OnPropertyChanged();
        }
    }

    protected bool _isFooterExpanded = true;
    public bool IsFooterExpanded
    {
        get => _isFooterExpanded;
        set
        {
            _isFooterExpanded = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Constructors

    public BaseFormVM() { }
    public BaseFormVM(Report report)
    {
        _currentReport = report;
        UpdateFormList();
        UpdatePageInfo();
        NoteList = CurrentReport.Notes;

    }

    #endregion

    #region Commands

    public ICommand CheckForm => new NewCheckFormAsyncCommand(this);    //  Кнопка "Проверить"
    //public ICommand CopyExecutorDate => new NewCopyExecutorDataAsyncCommand(this); //После привязки кнопка неактивна
    public ICommand SourceTransmissionAll => new NewSourceTransmissionAllAsyncCommand(this);    //  Кнопка "Перевести данные предыдущей формы"
    public ICommand AddRow => new NewAddRowAsyncCommand(this);
    public ICommand AddRows => new NewAddRowsAsyncCommand(this);
    public ICommand AddRowsIn => new NewAddRowsInAsyncCommand(this);
    public ICommand DeleteDataInRows => new DeleteDataInRowsAsyncCommand();
    public ICommand DeleteRows => new NewDeleteRowsAsyncCommand(this);
    public ICommand SortForm => new NewSortFormSyncCommand(this);
    public ICommand SetNumberOrder => new NewSetNumberOrderSyncCommand(this);
    public ICommand CopyRows => new NewCopyRowsAsyncCommand();
    public ICommand PasteRows => new NewPasteRowsAsyncCommand(this);
    public ICommand SelectAll => new SelectAllRowsAsyncCommand(this);

    #endregion

    public async void UpdateFormList()
    {
        FormList = new ObservableCollection<Form>(CurrentReport.Rows.ToList<Form>().Skip((CurrentPage - 1) * RowCount).Take(RowCount)); //Нужна оптимизация
    }
    public async void UpdatePageInfo()
    {
        OnPropertyChanged(nameof(TotalPages));
        OnPropertyChanged(nameof(TotalRows));
    }

    #region OnPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}