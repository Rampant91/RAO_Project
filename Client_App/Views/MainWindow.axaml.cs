using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.ApplicationLifetimes;
using Models.Client_Model;
using System.Collections.ObjectModel;
using System.Collections;
using System;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using MessageBox.Avalonia;
using System.Threading.Tasks;
using System.Collections.Generic;
using Avalonia.Interactivity;

namespace Client_App.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
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
        private void Init()
        {
            Panel tab10 = this.FindControl<Panel>("Forms_p1_0");
            Panel tab1X = this.FindControl<Panel>("Forms_p1_X");
            Panel tab1B = this.FindControl<Panel>("Forms_p1_B");

            if (tab10 != null&& tab1X != null&& tab1B != null)
            {
                Short_Visual.Form1_Visual.FormF_Visual(tab10,tab1X,tab1B);
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
