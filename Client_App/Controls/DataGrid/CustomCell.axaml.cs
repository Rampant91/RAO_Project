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
    public class CustomCell : Cell
    {

        public CustomCell(Control ctrl,object DataContext, string BindingPath, bool IsReadOnly)
        {
            this.DataContext = DataContext;
            this.BindingPath = BindingPath;
            this.IsReadOnly = IsReadOnly;
            this.Control = ctrl;

            InitializeComponent();

            Focusable = false;
        }
        public CustomCell(Control ctrl,string BindingPath, bool IsReadOnly)
        {
            this.BindingPath = BindingPath;
            this.IsReadOnly = IsReadOnly;
            this.Control = ctrl;

            InitializeComponent();

            Focusable = false;
        }

        public CustomCell()
        {
            InitializeComponent();
        }
        void SetBindingToText()
        {
            if (this.DataContext == null)
            {
                var t = (Control)((Panel)((Border)Content).Child).Children[0];
                if (t != null)
                {
                    Binding b = new()
                    {
                        Path = BindingPath,

                    };
                    t.Bind(TextBox.DataContextProperty, b);
                }
            }
            else
            {
                var t = (Control)((Panel)((Border)Content).Child).Children[0];
                if (t != null)
                {
                    t.DataContext = this.DataContext;
                }
            }
        }

        public static readonly DirectProperty<Cell, string> BindingPathProperty =
            AvaloniaProperty.RegisterDirect<Cell, string>(
        nameof(BindingPath),
        o => o.BindingPath,
        (o, v) => o.BindingPath = v);

        string bindingPath = "";
        public string BindingPath
        {
            get => bindingPath;
            set
            {
                if (value != null)
                {
                    SetAndRaise(BindingPathProperty, ref bindingPath, value);
                    SetBindingToText();
                }
            }
        }

        public Control Control { get; set; }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            var t = ((Panel) ((Border) Content).Child);
            t.Children.Add(Control);

            this.BindingPath = BindingPath;

        }
    }
}