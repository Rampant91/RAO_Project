using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Collections;
using System.Collections.ObjectModel;

namespace Client_App.Controls.DataGrid
{

    public partial class DataGrid : UserControl
    {
        public static readonly DirectProperty<DataGrid, ObservableCollection<Report>> ItemsProperty =
            AvaloniaProperty.RegisterDirect<DataGrid, ObservableCollection<Report>>(
                nameof(Items),
                o => o.Items,
                (o, v) => o.Items = v);
        private ObservableCollection<Report> _items = new ObservableCollection<Report>();
        public ObservableCollection<Report> Items
        {
            get { return _items; }
            set { SetAndRaise(ItemsProperty, ref _items, value); }
        }

        public static readonly DirectProperty<DataGrid, string> TypeProperty =
            AvaloniaProperty.RegisterDirect<DataGrid, string>(
                nameof(Type),
                o => o.Type,
                (o, v) => o.Type = v);
        private string _type = "";
        public string Type
        {
            get { return _type; }
            set { SetAndRaise(TypeProperty, ref _type, value); }
        }

        public Control RowControl { get; set; }
        public Control HeaderControl { get; set; }

        public DataGrid()
        {
            InitializeComponent();
            HeaderControl = Support.RenderDataGridHeader.Render.GetControl(Type);
            RowControl= Support.RenderDataGridRow.Render.GetControl(Type);
        }

        public void UpdateHeader()
        {
            if(HeaderControl!=null)
            {
                var panel=this.FindControl<Panel>("Columns");
                panel.Children.Add(HeaderControl);
            }
            else
            {
                HeaderControl = Support.RenderDataGridHeader.Render.GetControl(Type);
                var panel = this.FindControl<Panel>("Columns");
                panel.Children.Add(HeaderControl);
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
