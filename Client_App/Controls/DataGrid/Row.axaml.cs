using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace Client_App.Controls.DataGrid
{
    public class Row : StackPanel
    {
        public static readonly DirectProperty<Row, bool> RowHideProperty =
           AvaloniaProperty.RegisterDirect<Row, bool>(
           nameof(RowHide),
           o => o.RowHide,
           (o, v) => o.RowHide = v);

        public bool rowHide = false;
        public bool RowHide
        {
            get => rowHide;
            set
            {
                if (value != null) 
                {
                    SetAndRaise(RowHideProperty, ref rowHide, value);
                    Hiding();
                }
            }
        }
        public void Hiding()
        {
            this.IsVisible = !RowHide;
        }
        public Row()
        {
            Initialization();
        }

        public int SRow { get; set; } = -1;

        private void Initialization()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}