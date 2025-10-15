using Client_App.ViewModels.Forms;
using Models.Collections;
using Models.JSON.ExecutorData;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.Controls
{
    public class ExecutorDataControlVM
    {

        private Report _report;
        public ExecutorDataControlVM(Report report)
        {
            _report = report;

            OpenPopupCommand = ReactiveCommand.Create(() =>
            {
                PopupIsOpen = !PopupIsOpen;
            });
        }

        #region Property
        #region ExecutorData

        public ExecutorData ExecutorData
        {
            get
            {
                return new ExecutorData() 
                { 
                    ExecEmail = Email, 
                    ExecPhone = Phone, 
                    GradeExecutor = Grade, 
                    FIOexecutor = FIO 
                };
            }
            set
            {
                Email = value.ExecEmail;
                Phone = value.ExecPhone;
                Grade = value.GradeExecutor;
                FIO = value.FIOexecutor;
            }
        }

        public string FIO
        {
            get
            {
                return _report.FIOexecutor.Value;
            }
            set
            {
                _report.FIOexecutor.Value = value;
            }
        }
        public string Grade
        {
            get
            {
                return _report.GradeExecutor.Value;
            }
            set
            {
                _report.GradeExecutor.Value = value;
            }
        }
        public string Phone
        {
            get
            {
                return _report.ExecPhone.Value;
            }
            set
            {
                _report.ExecPhone.Value = value;
            }
        }
        public string Email
        {
            get
            {
                return _report.ExecEmail.Value;
            }
            set
            {
                _report.ExecEmail.Value = value;
            }
        }
        #endregion

        private bool _popupIsOpen = false;
        public bool PopupIsOpen
        {
            get { return _popupIsOpen; }
            set
            {
                _popupIsOpen = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ExecutorData> _executorsList;
        public ObservableCollection<ExecutorData> ExecutorList
        {
            get
            {
                return _executorsList;
            }
            set
            {
                _executorsList = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand CreateExecutor;
        public ICommand GetExecutor;
        public ICommand DeleteExecutor;

        public ICommand OpenPopupCommand;


        #endregion

        #region OnPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

}
