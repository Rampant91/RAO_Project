using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
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

        #region ChooseColor
        public static readonly DirectProperty<Cell, SolidColorBrush> ChooseColorProperty =
                AvaloniaProperty.RegisterDirect<Cell, SolidColorBrush>(
        nameof(ChooseColor),
        o => o.ChooseColor,
        (o, v) => o.ChooseColor = v);

        private SolidColorBrush _ChooseColor = null;

        public SolidColorBrush ChooseColor
        {
            get => _ChooseColor;
            set
            {
                SetAndRaise(ChooseColorProperty, ref _ChooseColor, value);
            }
        }
        #endregion

        #region Row
        public static readonly DirectProperty<Cell, int> RowProperty =
            AvaloniaProperty.RegisterDirect<Cell, int>(
                nameof(Row),
                o => o.Row,
                (o, v) => o.Row = v, defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

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

        #region Control
        IControl _Control = null;
        public IControl Control 
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

                    var t = (Panel)((Border)Content).Child;
                    t.Children.Add(_Control);
                }
            } 
        }
        #endregion

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            if (Control != null)
            {
                var t = (Panel)((Border)Content).Child;
                t.Children.Add(Control);
            }
        }
    }
}