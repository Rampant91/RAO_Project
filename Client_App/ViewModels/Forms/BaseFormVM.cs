using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Commands.AsyncCommands.Delete;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.Commands.SyncCommands;
using Client_App.Views;
using Client_App.Views.Forms;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms
{
    public abstract class BaseFormVM : BaseVM, INotifyPropertyChanged
    {
        #region Properties

        public abstract  string FormType { get; }
        private ObservableCollection<Form> _formList = new ObservableCollection<Form>();
        public ObservableCollection<Form> FormList
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
        private ObservableCollection<Note> _noteList = new ObservableCollection<Note>();
        public ObservableCollection<Note> NoteList
        {
            get
            {
                return _noteList;
            }
            set
            {
                _noteList = value;
                OnPropertyChanged();
            }
        }
        private Report _currentReport;
        public Report CurrentReport
        {
            get
            {
                return _currentReport;
            }
            set
            {
                _currentReport = value;
                OnPropertyChanged();
            }
        }
        public Reports CurrentReports
        {
            get
            {
                return _currentReport.Reports;
            }
        }
        private Form _selectedForm;
        public Form SelectedForm
        {
            get
            {
                return _selectedForm;
            }
            set
            {
                _selectedForm = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Form> _selectedForms = new();
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
        private int _rowCount = 30;
        public int RowCount
        {
            get
            {
                return _rowCount;
            }
            set
            {
                if (value <= 0)
                    _rowCount = 1;
                else
                    _rowCount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPages));
                UpdateFormList();
            }
        }

        private int _currentPage = 1;
        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                if (value <= 0)
                    _currentPage = 1;
                else if (value > TotalPages)
                    _currentPage = TotalPages;
                else
                    _currentPage = value;
                OnPropertyChanged();
                UpdateFormList();
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
        public int TotalRows
        {
            get
            {
                return CurrentReport.Rows.Count;
            }
        }
        private bool _isAutoReplaceEnabled = true;
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
        private bool _isHeaderExpanded = true;
        public bool IsHeaderExpanded
        {
            get
            {
                return _isHeaderExpanded;
            }
            set
            {
                _isHeaderExpanded = value;
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
        public ICommand DeleteRows => new NewDeleteRowsAsyncCommand(this);
        public ICommand SortForm => new NewSortFormSyncCommand(this);
        public ICommand SetNumberOrder => new NewSetNumberOrderSyncCommand(this);
        public ICommand CopyRows => new NewCopyRowsAsyncCommand();
        public ICommand PasteRows => new NewPasteRowsAsyncCommand(this);
        public ICommand SelectAll => new SelectAllRowsAsyncCommand(this);

        #endregion

        public async void UpdateFormList()
        {
            FormList = new ObservableCollection<Form>(CurrentReport.Rows.ToList<Form>().Skip((CurrentPage - 1) * RowCount).Take(RowCount));
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
}
