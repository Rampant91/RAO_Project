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

        public Cell()
        {
            InitializeComponent();
        }

        public Cell(Control ctrl)
        {
            this.Control = ctrl;
            InitializeComponent();
        }

        #region BorderColor
        public static readonly DirectProperty<Cell, SolidColorBrush> BorderColorProperty =
                AvaloniaProperty.RegisterDirect<Cell, SolidColorBrush>(
        nameof(BorderColor),
        o => o.BorderColor,
        (o, v) => o.BorderColor = v);

        private SolidColorBrush _BorderColor = null;

        public SolidColorBrush BorderColor
        {
            get => _BorderColor;
            set
            {
                if (value != null)
                {
                    SetAndRaise(BorderColorProperty, ref _BorderColor, value);
                }
            }
        }
        #endregion

        #region Row
        public static readonly DirectProperty<Cell, int> RowProperty =
            AvaloniaProperty.RegisterDirect<Cell, int>(
                nameof(Row),
                o => o.Row,
                (o, v) => o.Row = v);

        private int _Row = -1;
        public int Row
        {
            get => _Row;
            set
            {
                SetAndRaise(RowProperty, ref _Row, value);
            }
        }
        #endregion

        #region Column
        public static readonly DirectProperty<Cell, int> ColumnProperty =
            AvaloniaProperty.RegisterDirect<Cell, int>(
                nameof(Column),
                o => o.Column,
                (o, v) => o.Column = v);

        private int _Column = -1;
        public int Column
        {
            get => _Column;
            set
            {
                SetAndRaise(ColumnProperty, ref _Column, value);
            }
        }
        #endregion

        Control _Control = null;
        public Control Control 
        {
            get 
            { 
                return _Control;
            }
            set 
            {
                if(_Control!=value&&value!=null)
                {
                    _Control = value;

                    var t = ((Panel)((Border)Content).Child);
                    t.Children.Add(_Control);

                    if (!(Control is TextBlock))
                    {
                        this.PointerPressed += PointerPressedEventHandler;
                        this.PointerMoved += PointerMovedEventHandler;
                        this.PointerReleased += PointerReleasedEventHandler;
                    }
                }
            } 
        }

        public void PointerPressedEventHandler(object sender,PointerEventArgs args)
        {
            var t = (IDataGrid)this.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent;
            string tmp = "";
            if(args.GetPointerPoint(this).Properties.PointerUpdateKind==PointerUpdateKind.LeftButtonPressed)
            {
                tmp = "Left";
            }
            if (args.GetPointerPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed)
            {
                tmp = "Right";
            }
            t.MethodFromCell("Pressed:"+tmp+":"+Row+":"+Column);
        }
        public void PointerMovedEventHandler(object sender, PointerEventArgs args)
        {
            var t = (IDataGrid)this.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent;
            string tmp = "";
            if (args.GetPointerPoint(this).Properties.IsLeftButtonPressed)
            {
                tmp = "Left";
            }
            if (args.GetPointerPoint(this).Properties.IsRightButtonPressed)
            {
                tmp = "Right";
            }
            //t.MethodFromCell("Moved:"+tmp+":"+Row+":"+Column);
        }
        public void PointerReleasedEventHandler(object sender, PointerEventArgs args)
        {
            var t = (IDataGrid)this.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent;
            string tmp = "";
            if (args.GetPointerPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                tmp = "Left";
            }
            if (args.GetPointerPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonReleased)
            {
                tmp = "Right";
            }
            t.MethodFromCell("Released:" + tmp + ":" + Row + ":" + Column);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            if (Control != null)
            {
                var t = ((Panel)((Border)Content).Child);
                t.Children.Add(Control);
            }
        }
    }
}