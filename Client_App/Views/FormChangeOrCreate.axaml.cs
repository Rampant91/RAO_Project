using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.ComponentModel;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ReactiveUI.Validation;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Formatters;
using Collections;
using Avalonia.Controls.ApplicationLifetimes;

namespace Client_App.Views
{
    public class FormChangeOrCreate : ReactiveWindow<ViewModels.ChangeOrCreateVM>
    {
        string _param = "";
        public DBRealization.DBModel dbm;
        public FormChangeOrCreate(string param, string DBPath, Report rep, DBRealization.DBModel dbm)
        {
            var tmp = new ViewModels.ChangeOrCreateVM();
            this.dbm = dbm;
            if (DBPath != null)
            {
                tmp.DBPath = DBPath;
                tmp.Storage = rep;
            }
            else
            {
                this.Close();
            }
            tmp.FormType = param;
            this.DataContext = tmp;
            _param = param;

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Init();
        }

        public FormChangeOrCreate()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            dbm.UndoChanges();
            dbm.SaveChanges();

            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                ((ViewModels.MainWindowVM)desktop.MainWindow.DataContext).UpdateAll();
            }
        }

        void Form1Init(in Panel panel)
        {
            if (_param == "1/0")
                panel.Children.Add(Long_Visual.Form1_Visual.Form10_Visual());
            if (_param == "1/1")
                panel.Children.Add(Long_Visual.Form1_Visual.Form11_Visual(this.FindNameScope()));
            if (_param == "1/2")
                panel.Children.Add(Long_Visual.Form1_Visual.Form12_Visual());
            if (_param == "1/3")
                panel.Children.Add(Long_Visual.Form1_Visual.Form13_Visual());
            if (_param == "1/4")
                panel.Children.Add(Long_Visual.Form1_Visual.Form14_Visual());
            if (_param == "1/5")
                panel.Children.Add(Long_Visual.Form1_Visual.Form15_Visual());
            if (_param == "1/6")
                panel.Children.Add(Long_Visual.Form1_Visual.Form16_Visual());
            if (_param == "1/7")
                panel.Children.Add(Long_Visual.Form1_Visual.Form17_Visual());
            if (_param == "1/8")
                panel.Children.Add(Long_Visual.Form1_Visual.Form18_Visual());
            if (_param == "1/9")
                panel.Children.Add(Long_Visual.Form1_Visual.Form19_Visual());
        }

        void Init()
        {
            var panel = this.FindControl<Panel>("ChangingPanel");
            Form1Init(panel);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
