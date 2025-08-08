using Avalonia.Controls;
using AvaloniaEdit.Utils;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Commands.AsyncCommands.Save;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1
{
    public class Form_12VM : BaseVM, INotifyPropertyChanged
    {
        private ObservableCollection<Form12> _formList = new ObservableCollection<Form12>();

        public string FormType { get { return "1.2"; } }
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
                OnPropertyChanged(nameof(PageCount));
                FormList = GetFormList(CurrentPage, RowCount);
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

        public Form_12VM() { }
        public Form_12VM(Report report) 
        {
            _currentReport = report;
            FormList = GetFormList(CurrentPage, RowCount);
            NoteList = CurrentReport.Notes;
        }

        #region Commands

        public ICommand CheckForm => new NewCheckFormAsyncCommand(this);    //  Кнопка "Проверить"
        //public ICommand CopyExecutorDate => new NewCopyExecutorDataAsyncCommand(this); //После привязки кнопка неактивная

        #endregion


        private ObservableCollection<Form12> GetFormList (int page, int rowCount)
        {

            ObservableCollection<Form12> formList = new ObservableCollection<Form12>( CurrentReport.Rows12.Skip((page-1)*rowCount).Take(rowCount));
            return formList; 
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
