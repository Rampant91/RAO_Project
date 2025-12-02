using Client_App.Properties.ColumnWidthSettings;
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
    public class ExecutorDataControlVM : INotifyPropertyChanged
    {

        private Report _report;
        public ExecutorDataControlVM(Report report)
        {
            _report = report;
            _executorList = new ObservableCollection<ExecutorData>(ExecutorDataManager.GetAllExecutorData());
            OpenPopupCommand = ReactiveCommand.Create(() =>
            {
                PopupIsOpen = !PopupIsOpen;
            });
            AddExecutor = ReactiveCommand.Create(() =>
            {
                ExecutorDataManager.AddExecutorData(Executor);
                ExecutorList = new ObservableCollection<ExecutorData>(ExecutorDataManager.GetAllExecutorData());
            });
            DeleteExecutor = ReactiveCommand.Create<ExecutorData>((executor) => {
                ExecutorDataManager.DeleteExecutorData(executor);
                ExecutorList = new ObservableCollection<ExecutorData>(ExecutorDataManager.GetAllExecutorData());
            });
            GetExecutor = ReactiveCommand.Create<ExecutorData>((executor) =>
            {
                Executor = ExecutorDataManager.GetExecutorData(executor);
            });

        }

        #region Property

        #region ExecutorData

        public ExecutorData Executor
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
                OnPropertyChanged();
            }
        }

        public string? FIO
        {
            get
            {
                return _report.FIOexecutor.Value;
            }
            set
            {
                _report.FIOexecutor.Value = value;
                OnPropertyChanged();
            }
        }
        public string? Grade
        {
            get
            {
                return _report.GradeExecutor.Value;
            }
            set
            {
                _report.GradeExecutor.Value = value;
                OnPropertyChanged();
            }
        }
        public string? Phone
        {
            get
            {
                return _report.ExecPhone.Value;
            }
            set
            {
                _report.ExecPhone.Value = value;
                OnPropertyChanged();
            }
        }
        public string? Email
        {
            get
            {
                return _report.ExecEmail.Value;
            }
            set
            {
                _report.ExecEmail.Value = value;
                OnPropertyChanged();
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

        private ObservableCollection<ExecutorData> _executorList;
        public ObservableCollection<ExecutorData> ExecutorList
        {
            get
            {
                return _executorList;
            }
            set
            {
                _executorList = value;

                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand AddExecutor { get; set; }
        public ICommand GetExecutor { get; set; }
        public ICommand DeleteExecutor { get; set; }

        public ICommand OpenPopupCommand { get; set; }


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
