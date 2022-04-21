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
using Client_App.ViewModels;
using Avalonia.Controls.ApplicationLifetimes;
using System.Reactive;
using Avalonia.Media;

namespace Client_App.Views
{
    public partial class RAOCodeCalcWindow : ReactiveWindow<ViewModels.RAOCodeCalcWindowVM>
    {
        public RAOCodeCalcWindow(ViewModels.RAOCodeCalcWindowVM dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }
        public RAOCodeCalcWindow()
        {
            DataContext = new RAOCodeCalcWindowVM();
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
