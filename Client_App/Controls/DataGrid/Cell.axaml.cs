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
        public static readonly DirectProperty<Cell, int> CellRowProperty =
            AvaloniaProperty.RegisterDirect<Cell, int>(
                nameof(CellRow),
                o => o.CellRow,
                (o, v) => o.CellRow = v);

        public int cellRow = -1;

        public Cell(object DataContext, string BindingPath, bool IsReadOnly)
        {
            this.DataContext = DataContext;
            this.IsReadOnly = IsReadOnly;
            InitializeComponent();

            Focusable = false;
        }

        public bool RightHandler { get; set; } = true;

        public Cell(string BindingPath, bool IsReadOnly,bool RightHandler=true)
        {
            this.IsReadOnly = IsReadOnly;
            InitializeComponent();
            Focusable = false;
            this.RightHandler = RightHandler;
        }

        public Cell()
        {
            InitializeComponent();
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
        void SetBindingToText()
        {
            if (this.DataContext != null)
            {
                var t = (TextBox)((Panel)((Border)Content).Child).Children[0];
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
                var t = (TextBox)((Panel)((Border)Content).Child).Children[0];
                if (t != null)
                {
                    t.DataContext = this.DataContext;
                }
            }
        }

        public bool IsReadOnly { get; set; }

        public int CellRow
        {
            get => cellRow;
            set
            {
                if (value != null) SetAndRaise(CellRowProperty, ref cellRow, value);
            }
        }

        public int CellColumn { get; set; } = -1;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.BindingPath = BindingPath;
            var t = (TextBox) ((Panel) ((Border) Content).Child).Children[0];
            t.IsEnabled = !IsReadOnly;
        }
    }
}