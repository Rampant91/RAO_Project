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
            
            var FR1 = this.FindControl<RadioButton>("first_rad_1");
            FR1.Checked += ((CalcVM)DataContext).Radio_Checked2;
            var FR2 = this.FindControl<RadioButton>("first_rad_2");
            FR2.Checked += ((CalcVM)DataContext).Radio_Checked2;

            var FFR1 = this.FindControl<RadioButton>("f_rad_1");
            FFR1.Checked += ((CalcVM)DataContext).Radio_Checked5;
            var FFR2 = this.FindControl<RadioButton>("f_rad_2");
            FFR2.Checked += ((CalcVM)DataContext).Radio_Checked5;

            var PR1 = this.FindControl<RadioButton>("p_rad_1");
            PR1.Checked += ((CalcVM)DataContext).Radio_Checked6;
            var PR2 = this.FindControl<RadioButton>("p_rad_2");
            PR2.Checked += ((CalcVM)DataContext).Radio_Checked6;

            var TR1 = this.FindControl<RadioButton>("thr_rad_1");
            TR1.Checked += ((CalcVM)DataContext).Radio_Checked3;
            var TR2 = this.FindControl<RadioButton>("thr_rad_2");
            TR2.Checked += ((CalcVM)DataContext).Radio_Checked3;
            var TR3 = this.FindControl<RadioButton>("thr_rad_3");
            TR3.Checked += ((CalcVM)DataContext).Radio_Checked3;
            var TR4 = this.FindControl<RadioButton>("thr_rad_4");
            TR4.Checked += ((CalcVM)DataContext).Radio_Checked3;
            var TR5 = this.FindControl<RadioButton>("thr_rad_5");
            TR5.Checked += ((CalcVM)DataContext).Radio_Checked3;
            var TR6 = this.FindControl<RadioButton>("thr_rad_6");
            TR6.Checked += ((CalcVM)DataContext).Radio_Checked3;
            var TR7 = this.FindControl<RadioButton>("thr_rad_7");
            TR7.Checked += ((CalcVM)DataContext).Radio_Checked3;
            var TR8 = this.FindControl<RadioButton>("thr_rad_8");
            TR8.Checked += ((CalcVM)DataContext).Radio_Checked3;
            var TR9 = this.FindControl<RadioButton>("thr_rad_9");
            TR9.Checked += ((CalcVM)DataContext).Radio_Checked3;

            var SR1 = this.FindControl<RadioButton>("sec_rad_1");
            SR1.Checked += ((CalcVM)DataContext).Radio_Checked4;
            var SR2 = this.FindControl<RadioButton>("sec_rad_2");
            SR2.Checked += ((CalcVM)DataContext).Radio_Checked4;
            var SR3 = this.FindControl<RadioButton>("sec_rad_3");
            SR3.Checked += ((CalcVM)DataContext).Radio_Checked4;
            var SR4 = this.FindControl<RadioButton>("sec_rad_4");
            SR4.Checked += ((CalcVM)DataContext).Radio_Checked4;
            var SR5 = this.FindControl<RadioButton>("sec_rad_5");
            SR5.Checked += ((CalcVM)DataContext).Radio_Checked4;
            var SR6 = this.FindControl<RadioButton>("sec_rad_6");
            SR6.Checked += ((CalcVM)DataContext).Radio_Checked4;


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
