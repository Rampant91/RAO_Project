using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using Avalonia;
using Avalonia.Data;

namespace Client_App.Controls.DataGrid
{
    public class Cell : UserControl
    {
        public Cell(object DataContext, string BindingPath, bool IsReadOnly)
        {
            this.DataContext = DataContext;
            this.BindingPath = BindingPath;
            this.IsReadOnly = IsReadOnly;
            InitializeComponent();

            this.Focusable = false;
        }

        public Cell(string BindingPath, bool IsReadOnly)
        {
            this.BindingPath = BindingPath;
            this.IsReadOnly = IsReadOnly;
            InitializeComponent();

            this.Focusable = false;
        }

        public Cell()
        {
            InitializeComponent();
        }

        public string BindingPath { get; set; } = "";

        public bool IsReadOnly { get; set; }

        public static readonly DirectProperty<Cell, int> CellRowProperty =
            AvaloniaProperty.RegisterDirect<Cell, int>(
                nameof(CellRow),
                o => o.CellRow,
                (o, v) => o.CellRow = v);

        public int cellRow = -1;
        public int CellRow
        {
            get => cellRow;
            set
            {
                if (value != null)
                {
                    SetAndRaise(CellRowProperty, ref cellRow, value);
                }
            }
        } 

        public int CellColumn { get; set; } = -1;

        public event PropertyChangedEventHandler PropertyChanged;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            var t = (TextBox) ((Panel) ((Border) Content).Child).Children[0];
            t.IsEnabled = !IsReadOnly;
            t.ContextMenu = null;
        }

        public void PanelPointerDown(object sender, PointerPressedEventArgs args)
        {
            var mouse = args.GetCurrentPoint((Cell) sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
            {
                OnPropertyChanged(this, "Down");
            }
        }

        public void PanelPointerMoved(object sender, PointerEventArgs args)
        {
            OnPropertyChanged(this, "Move");
        }

        public void PanelPointerUp(object sender, PointerReleasedEventArgs args)
        {
            var mouse = args.GetCurrentPoint((Cell) sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                OnPropertyChanged(this, "Up");
            }
        }

        //Property Changed
        protected void OnPropertyChanged(object obj, [CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null) PropertyChanged(obj, new PropertyChangedEventArgs(prop));
        }
        //Property Changed
    }
}