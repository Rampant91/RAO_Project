using Avalonia.Controls;
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
        public DateTime StartPeriod
        {
            get 
            {
                var startPeriod = CurrentReport.StartPeriod_DB.Split('.');
                int date = Convert.ToInt32(startPeriod[0]);
                int month = Convert.ToInt32(startPeriod[1]);
                int year = Convert.ToInt32(startPeriod[2]);
                return new DateTime(year, month, date);
            }
        }
        public DateTime EndPeriod
        {
            get
            {
                var startPeriod = CurrentReport.EndPeriod_DB.Split('.');
                int date = Convert.ToInt32(startPeriod[0]);
                int month = Convert.ToInt32(startPeriod[1]);
                int year = Convert.ToInt32(startPeriod[2]);
                return new DateTime(year, month, date);
            }
        }
        public Form_12VM() { }
        public Form_12VM(Report report) 
        {
            _currentReport = report;
            FormList = report.Rows12;
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
