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
        public Control HeaderControl { get; set; }

        public DataGrid()
        {
            InitializeComponent();
            this.DataContext = new Support.DataGrid_DataContext(this);
            Type = "1/1";
            MakeHeader();
            HeaderControl = Support.RenderDataGridHeader.Render.GetControl(Type);
        }

        public void AddRow()
        {
            var panel = this.FindControl<Panel>("Rows");
            var rp = new Report();
            this.Items.Add(rp);
            var id = this.Items.IndexOf(rp);
            panel.Children.Add(Support.RenderDataGridRow.Render.GetControl(Type,id));
        }

        public void DeleteRow(Report obj)
        {
            var panel = this.FindControl<Panel>("Rows");
            var id=this.Items.IndexOf(obj);
            this.Items.RemoveAt(id);
            panel.Children.RemoveAt(id);
        }

        public void MakeHeader()
        {
            var panel = this.FindControl<Panel>("Columns");
            panel.Children.Clear();
            panel.Children.Add(HeaderControl);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
