using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Data;
using Models.Collections;

namespace Client_App.Views
{
    public class MainWindow : ReactiveWindow<ViewModels.MainWindowVM>
    {

        public static readonly DirectProperty<MainWindow, IEnumerable<IKey>> SelectedReportsProperty =
            AvaloniaProperty.RegisterDirect<MainWindow, IEnumerable<IKey>>(
                nameof(SelectedReports),
                o => o.SelectedReports,
                (o, v) => o.SelectedReports = v);

        private IEnumerable<IKey> _selectedReports = new ObservableCollectionWithItemPropertyChanged<IKey>();

        public IEnumerable<IKey> SelectedReports
        {
            get => _selectedReports;
            set
            {
                if (value != null) SetAndRaise(SelectedReportsProperty, ref _selectedReports, value);
            }
        }
        public MainWindow()
        {
            DataContext = new ViewModels.MainWindowVM();
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            Init();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
        private void Init()
        {
            Panel tab10 = this.FindControl<Panel>("Forms_p1_0");
            Panel tab1X = this.FindControl<Panel>("Forms_p1_X");
            Panel tab1B = this.FindControl<Panel>("Forms_p1_B");
            Short_Visual.Form1_Visual.FormF_Visual(this,tab10, tab1X, tab1B);

            Panel tab20 = this.FindControl<Panel>("Forms_p2_0");
            Panel tab2X = this.FindControl<Panel>("Forms_p2_X");
            Panel tab2B = this.FindControl<Panel>("Forms_p2_B");
            Short_Visual.Form2_Visual.FormF_Visual(this,tab20, tab2X, tab2B);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
