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


        private Report _report;
        public Form_12VM() { }
        public Form_12VM(Report report) 
        {
            _report = report;
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
