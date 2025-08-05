using Avalonia.Controls;
using AvaloniaEdit.Utils;
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
using System.Text;
using System.Threading.Tasks;

namespace Client_App.ViewModels.Forms.Forms1
{
    public class Form_12VM : BaseVM, INotifyPropertyChanged
    {
        private ObservableCollection<Form12> _formList = new ObservableCollection<Form12>();
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

        public Form_12VM() { }
        public Form_12VM(Report report) 
        {
            _currentReport = report;
            FormList = GetFormList(CurrentPage, RowCount);
        }


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
