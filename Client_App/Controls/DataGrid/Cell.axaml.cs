using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;

namespace Client_App.Controls.DataGrid
{
    public class Cell : UserControl
    {

        public Cell(Control ctrl)
        {
            this.Control = ctrl;
            InitializeComponent();
        }

        public int Row { get; set; }
        public int Column { get; set; }

        public Cell()
        {
            InitializeComponent();
        }

        public Control Control { get; set; }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            var t = ((Panel) ((Border) Content).Child);
            t.Children.Add(Control);

        }
    }
}