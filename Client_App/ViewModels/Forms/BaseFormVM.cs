using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Commands.AsyncCommands.Delete;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.Commands.AsyncCommands.SwitchReport;
using Client_App.Commands.SyncCommands;
using Client_App.ViewModels.Controls;
using Models.Collections;
using Models.Forms;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Models.Attributes;

namespace Client_App.ViewModels.Forms;

public abstract class BaseFormVM : BaseVM, INotifyPropertyChanged
{
    #region Properties

    public abstract string FormType { get; }

    public string WindowTitle 
    {
        get
        {
            var formNum = FormType.Replace(".", "");
            return $"{((Form_ClassAttribute)Type.GetType($"Models.Forms.Form{formNum[0]}.Form{formNum},Models")!.GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name} " 
                   + $"{Reports.Master_DB.RegNoRep.Value}  "
                   + $"{Reports.Master_DB.ShortJurLicoRep.Value}  "
                   + $"{Reports.Master_DB.OkpoRep.Value}";
        }
    
    }

    private ObservableCollection<Form> _formList = [];
    public ObservableCollection<Form> FormList
    {
        get => _formList;
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

    #region Report

    private Report _report;
    public Report Report
    {
        get => _report;
        set
        {
            _report = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Reports

    private Reports _reports;
    public Reports Reports
    {
        get => _reports;
        set
        {
            if (_reports != value)
            {
                _reports = value;
                OnPropertyChanged();
            }
        }
    }
    #endregion

    #region DBO

    private DBObservable _DBO;
    public DBObservable DBO
    {
        get => _DBO;
        set
        {
            if (_DBO != value)
            {
                _DBO = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    private Form? _selectedForm;
    public Form? SelectedForm
    {
        get => _selectedForm;
        set
        {
            _selectedForm = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Form> _selectedForms = [];
    public ObservableCollection<Form> SelectedForms
    {
        get => _selectedForms;
        set
        {
            if (_selectedForms != value)
            {
                UnsubscribeSelectedForms(_selectedForms);
                _selectedForms = value;
                SubscribeSelectedForms(_selectedForms);
                OnPropertyChanged();
                OnPropertyChanged(nameof(AnyRowSelected));
                OnPropertyChanged(nameof(OnlyOneRowSelected));
            }
        }
    }

    #region AnyRowSelected

    private bool _anyRowSelected;
    public bool AnyRowSelected
    {
        get => SelectedForms.Any();
        set
        {
            if (_anyRowSelected != value)
            {
                _anyRowSelected = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    private bool _dataGridIsEditing;
    public bool DataGridIsEditing
    {
        get => _dataGridIsEditing;
        set
        {
            if (_dataGridIsEditing != value)
            {
                _dataGridIsEditing = value;
                OnPropertyChanged();
            }
        }
    }

    #region OnlyOneRowSelected

    private bool _onlyOneRowSelected;
    public bool OnlyOneRowSelected
    {
        get => SelectedForms.Count == 1;
        set
        {
            if (_onlyOneRowSelected != value)
            {
                _onlyOneRowSelected = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region SkipChangeTacking

    private bool _skipChangeTacking;
    public bool SkipChangeTacking
    {
        get => _skipChangeTacking;
        set
        {
            if (_skipChangeTacking != value)
            {
                _skipChangeTacking = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    private Note _selectedNote;
    public Note SelectedNote
    {
        get => _selectedNote;
        set
        {
            _selectedNote = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Note> _selectedNotes = [];
    public ObservableCollection<Note> SelectedNotes
    {
        get => _selectedNotes;
        set
        {
            if (_selectedNotes != value)
            {
                _selectedNotes = value;
                OnPropertyChanged();
            }
        }
    }

    private int _rowCount = 30;
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

    private int _currentPage = 1;
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
            var result = Report.Rows.Count / RowCount;
            if (Report.Rows.Count % RowCount != 0)
                result++;
            return result;
        }

    }

    public int TotalRows => Report.Rows.Count;


    private bool _isAutoReplaceEnabled = true;
    public bool IsAutoReplaceEnabled
    {
        get => _isAutoReplaceEnabled;
        set
        {
            if (_isAutoReplaceEnabled != value)
            {
                _isAutoReplaceEnabled = value;
                Report.AutoReplace = value;
                OnPropertyChanged();
            }
        }
    }

    private int _selectedYear = DateTime.Now.Year;
    public int SelectedYear
    {
        get => _selectedYear;
        set
        {
            _selectedYear = value;
            OnPropertyChanged();
        }
    }

    private ExecutorDataControlVM _executorDataControlVM;
    public ExecutorDataControlVM ExecutorDataControlVM
    {
        get
        {
            return _executorDataControlVM;
        }
    }

    private SelectReportPopupVM _selectReportVM;
    public SelectReportPopupVM SelectReportPopupVM
    {
        get
        {
            return _selectReportVM;
        }
        set
        {
            _selectReportVM = value;
        }
    }
    #endregion

    #region Constructors

    public BaseFormVM() 
    { 
        SubscribeSelectedForms(_selectedForms);
    }

    /// <summary>
    /// Редактирование ранее созданного отчёта.
    /// </summary>
    /// <param name="report">Отчёт.</param>
    public BaseFormVM(Report report)
    {
        _report = report;
        _reports = report.Reports; 
        //_DBO = report.Reports.DBObservable;
        UpdateFormList();
        UpdatePageInfo();
        NoteList = Report.Notes;

        SubscribeSelectedForms(_selectedForms);
        InitializeUserControls();
    }

    public void InitializeUserControls()
    {
        _selectReportVM = new SelectReportPopupVM(this);
        _executorDataControlVM = new ExecutorDataControlVM(this.Report);
    }
    #endregion

    #region Commands

    public ICommand CheckForm => new NewCheckFormAsyncCommand(this);    //  Кнопка "Проверить"
    //public ICommand CopyExecutorDate => new NewCopyExecutorDataAsyncCommand(this); //После привязки кнопка неактивна
    public ICommand SourceTransmissionAll => new NewSourceTransmissionAllAsyncCommand(this);    //  Кнопка "Перевести данные предыдущей формы"
    public ICommand AddRow => new NewAddRowAsyncCommand(this);
    public ICommand AddRows => new NewAddRowsAsyncCommand(this);
    public ICommand AddRowsIn => new NewAddRowsInAsyncCommand(this);
    public ICommand DeleteDataInRows => new NewDeleteDataInRowsAsyncCommand();
    public ICommand DeleteRows => new NewDeleteRowsAsyncCommand(this);
    public ICommand SortForm => new NewSortFormSyncCommand(this);
    public ICommand SetNumberOrder => new NewSetNumberOrderSyncCommand(this);
    public ICommand SortForms => new NewSortFormSyncCommand(this);
    public ICommand CopyRows => new NewCopyRowsAsyncCommand();
    public ICommand PasteRows => new NewPasteRowsAsyncCommand(this);
    public ICommand SelectAll => new SelectAllRowsAsyncCommand(this);
    public ICommand SaveReport => new SaveReportAsyncCommand(this);
    public ICommand AddNote => new NewAddNoteAsyncCommand(this);
    public ICommand AddNotes => new NewAddNotesAsyncCommand(this);
    public ICommand CopyNotes => new NewCopyNotesAsyncCommand();
    public ICommand PasteNotes => new NewPasteNotesAsyncCommand(this);
    public ICommand DeleteNotes => new NewDeleteNoteAsyncCommand(this);
    public ICommand SetDefaultColumnWidth => new SetDefaultColumnWidthAsyncCommand();
    #endregion

    #region SelectionChangeWiring

    /// <summary>
    /// Обновление списка выделенных строчек.
    /// </summary>
    /// <param name="collection"></param>
    private void SubscribeSelectedForms(ObservableCollection<Form> collection)
    {
        if (collection is INotifyCollectionChanged notify)
        {
            notify.CollectionChanged += SelectedForms_CollectionChanged;
        }
    }

    private void UnsubscribeSelectedForms(ObservableCollection<Form> collection)
    {
        if (collection is INotifyCollectionChanged notify)
        {
            notify.CollectionChanged -= SelectedForms_CollectionChanged;
        }
    }

    private void SelectedForms_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(AnyRowSelected));
        OnPropertyChanged(nameof(OnlyOneRowSelected));
    }

    #endregion

    #region  UpdateNoteList

    public void UpdateNoteList()
    {
        NoteList = new ObservableCollection<Note>( Report.Notes.ToList<Note>());
    }

    #endregion

    #region UpdateFormList

    /// <summary>
    /// Обновляет отображение ячеек DataGrid'а.
    /// </summary>
    public void UpdateFormList()
    {
        FormList = new ObservableCollection<Form>(
            Report.Rows
                .ToList<Form>()
                .Skip((CurrentPage - 1) * RowCount)
                .Take(RowCount)); //Нужна оптимизация
    }

    #endregion

    #region UpdatePageInfo

    /// <summary>
    /// Обновляет отображаемое количество страниц и строчек.
    /// </summary>
    public void UpdatePageInfo()
    {
        OnPropertyChanged(nameof(TotalPages));
        OnPropertyChanged(nameof(TotalRows));
    }

    #endregion

    #region OnPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}