using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Data;
using Models.Collections;
using System.Threading.Tasks;
using ReactiveUI;
using Client_App.Controls.DataGrid;
using Client_App.ViewModels;

namespace Client_App.Views
{
    public class Calc : ReactiveWindow<ViewModels.CalcVM>
    {

        #region Contructures
        public Calc(ViewModels.CalcVM dataContext)
        {
            DataContext = dataContext;
            Init();
        }
        public Calc()
        {
            DataContext = new ViewModels.CalcVM();
            Init();
        }
        private void Init()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            var TRAO = this.FindControl<RadioButton>("TRAO");
            TRAO.Checked += ((CalcVM)DataContext).Radio_Checked;
            var WRAO = this.FindControl<RadioButton>("WRAO");
            WRAO.Checked += ((CalcVM)DataContext).Radio_Checked;
            var GRAO = this.FindControl<RadioButton>("GRAO");
            GRAO.Checked += ((CalcVM)DataContext).Radio_Checked;
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        #endregion


        #region Events
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
        #endregion

    }
}
