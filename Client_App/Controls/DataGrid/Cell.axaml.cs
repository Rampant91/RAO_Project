using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

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
            this.BindingPath = BindingPath;
            this.IsReadOnly = IsReadOnly;
            InitializeComponent();

            Focusable = false;
        }

        public Cell(string BindingPath, bool IsReadOnly)
        {
            this.BindingPath = BindingPath;
            this.IsReadOnly = IsReadOnly;
            InitializeComponent();
            Focusable = false;
        }

        public Cell()
        {
            InitializeComponent();
        }

        public string BindingPath { get; set; } = "";

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

            var t = (TextBox) ((Panel) ((Border) Content).Child).Children[0];
            t.IsEnabled = !IsReadOnly;
            t.AddHandler(PointerPressedEvent, InputElement_OnPointerPressed,handledEventsToo:true);
        }

        private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                var pnl = (Panel)(((StackPanel)((Row)this.Parent).Parent).Parent);
                var pnl2=(Panel)(((ScrollViewer)pnl.Parent).Parent);
                var pnl3 = (Panel)(((Grid)pnl2.Parent).Parent);
                var pnl4 = (Panel)(((ScrollViewer)pnl3.Parent).Parent);
                var pnl5 = (Panel)(((Grid)pnl4.Parent).Parent);

                var dataGrid = (Client_App.Controls.DataGrid.DataGrid)(((Border)pnl5.Parent).Parent);
                dataGrid.ContextMenu.Open();
            }
        }
    }
}