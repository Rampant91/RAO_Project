using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System;
using System.ComponentModel;

namespace Client_App.Views
{
    public class MainWindow : ReactiveWindow<ViewModels.MainWindowVM>
    {
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
            Short_Visual.Form1_Visual.FormF_Visual(tab10, tab1X, tab1B);

            Panel tab20 = this.FindControl<Panel>("Forms_p2_0");
            Panel tab2X = this.FindControl<Panel>("Forms_p2_X");
            Panel tab2B = this.FindControl<Panel>("Forms_p2_B");
            Short_Visual.Form2_Visual.FormF_Visual(tab20, tab2X, tab2B);

            Panel tab30 = this.FindControl<Panel>("Forms_p3_0");
            Panel tab3X = this.FindControl<Panel>("Forms_p3_X");
            Panel tab3B = this.FindControl<Panel>("Forms_p3_B");
            Short_Visual.Form3_Visual.FormF_Visual(tab30, tab3X, tab3B);

            Panel tab40 = this.FindControl<Panel>("Forms_p4_0");
            Panel tab4X = this.FindControl<Panel>("Forms_p4_X");
            Panel tab4B = this.FindControl<Panel>("Forms_p4_B");
            Short_Visual.Form4_Visual.FormF_Visual(tab40, tab4X, tab4B);

            Panel tab50 = this.FindControl<Panel>("Forms_p5_0");
            Panel tab5X = this.FindControl<Panel>("Forms_p5_X");
            Panel tab5B = this.FindControl<Panel>("Forms_p5_B");
            Short_Visual.Form5_Visual.FormF_Visual(tab50, tab5X, tab5B);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
