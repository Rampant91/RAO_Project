using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Layout;

namespace Client_App.Controls.DataGrid
{
    public class DataGridRow : Grid
    {
        #region Row
        public static readonly DirectProperty<DataGridRow, int> RowProperty =
            AvaloniaProperty.RegisterDirect<DataGridRow, int>(
                nameof(Row),
                o => o.Row,
                (o, v) => o.Row = v,defaultBindingMode:Avalonia.Data.BindingMode.TwoWay);

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

        #region ChooseColor
        public static readonly DirectProperty<DataGridRow, SolidColorBrush> ChooseColorProperty =
            AvaloniaProperty.RegisterDirect<DataGridRow, SolidColorBrush>(
                nameof(ChooseColor),
                o => o.ChooseColor,
                (o, v) => o.ChooseColor = v);

        private SolidColorBrush _ChooseColor = new(Color.Parse("White"));
        public SolidColorBrush ChooseColor
        {
            get => _ChooseColor;
            set
            {
                SetAndRaise(ChooseColorProperty, ref _ChooseColor, value);
            }
        }
        #endregion

        //#region Orientation
        //public static readonly DirectProperty<DataGridRow, Orientation> OrientationProperty =
        //    AvaloniaProperty.RegisterDirect<DataGridRow, Orientation>(
        //        nameof(Orientation),
        //        o => o.Orientation,
        //        (o, v) => o.Orientation = v);

        //private Orientation _Orientation = Orientation.Horizontal;
        //public Orientation Orientation
        //{
        //    get => _Orientation;
        //    set
        //    {
        //        SetAndRaise(OrientationProperty, ref _Orientation, value);
        //    }
        //}
        //#endregion

        //#region Children
        //public static readonly DirectProperty<DataGridRow, Avalonia.Controls.Controls> ChildrenProperty =
        //    AvaloniaProperty.RegisterDirect<DataGridRow, Avalonia.Controls.Controls>(
        //        nameof(Children),
        //        o => o.Children,
        //        (o, v) => o.Children = v);

        //private Avalonia.Controls.Controls _Children = new Avalonia.Controls.Controls();
        //public Avalonia.Controls.Controls Children
        //{
        //    get => _Children;
        //    set
        //    {
        //        SetAndRaise(ChildrenProperty, ref _Children, value);
        //    }
        //}
        //#endregion

        public DataGridRow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
