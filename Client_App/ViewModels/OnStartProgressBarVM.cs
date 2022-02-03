using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Avalonia.Data;
using Models.Collections;
using ReactiveUI;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Views;
using Models.DBRealization;
using DynamicData;
using Models.Abstracts;
using Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Spravochniki;
using System.IO;
using System.Reactive.Linq;
using System.Reactive;
using System.Threading;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Collections;
using FirebirdSql.Data.FirebirdClient;

namespace Client_App.ViewModels
{
    public class OnStartProgressBarVM : BaseVM, INotifyPropertyChanged
    {
        public Task MainTask { get; set; }
        public OnStartProgressBarVM()
        {
            ShowDialog = new Interaction<MainWindowVM, object>();
            MainTask=new Task(() => Start());
            MainTask.GetAwaiter().OnCompleted(async ()=> await ShowDialog.Handle(VMDataContext));
            MainTask.Start();
        }
        private double _OnStartProgressBar = 0;
        public double OnStartProgressBar
        {
            get => _OnStartProgressBar;
            set
            {
                if (_OnStartProgressBar != value)
                {
                    _OnStartProgressBar = value;
                    OnPropertyChanged(nameof(OnStartProgressBar));
                }
            }
        }
        public MainWindowVM VMDataContext {get;set;}=null;
        public async Task Start()
        {
            VMDataContext = new MainWindowVM();
            VMDataContext.PropertyChanged += OnVMPropertyChanged;
            await VMDataContext.Init();
        }
        public void OnVMPropertyChanged(object sender,PropertyChangedEventArgs args)
        {
            if(args.PropertyName==nameof(OnStartProgressBar))
            {
                OnStartProgressBar = VMDataContext.OnStartProgressBar;
            }
        }
        public Interaction<MainWindowVM, object> ShowDialog { get; private set; }

        #region INotifyPropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
