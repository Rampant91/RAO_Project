using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Client_App.Controls.DataGrid
{
    public partial class DataGridRow : StackPanel
    {
        #region Row
        public static readonly DirectProperty<DataGridRow, int> RowProperty =
            AvaloniaProperty.RegisterDirect<DataGridRow, int>(
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

        #region ChooseColor
        public static readonly DirectProperty<DataGridRow, SolidColorBrush> ChooseColorProperty =
            AvaloniaProperty.RegisterDirect<DataGridRow, SolidColorBrush>(
                nameof(ChooseColor),
                o => o.ChooseColor,
                (o, v) => o.ChooseColor = v);

        private SolidColorBrush _ChooseColor = new SolidColorBrush(Color.Parse("White"));
        public SolidColorBrush ChooseColor
        {
            get => _ChooseColor;
            set
            {
                SetAndRaise(ChooseColorProperty, ref _ChooseColor, value);
            }
        }
        #endregion

        public DataGridRow() : base()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
