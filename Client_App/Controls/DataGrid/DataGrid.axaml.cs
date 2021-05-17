using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Avalonia.Collections;

namespace Client_App.Controls.DataGrid
{

    public partial class DataGrid : UserControl
    {
        public static readonly DirectProperty<DataGrid, IEnumerable<Report>> ItemsProperty =
            AvaloniaProperty.RegisterDirect<DataGrid, IEnumerable<Report>>(
                nameof(Items),
                o => o.Items,
                (o, v) => o.Items = v);

        private IEnumerable<Report> _items = new AvaloniaList<Report> ();

        public IEnumerable<Report> Items
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

        public DataGrid()
        {
            InitializeComponent();
            this.DataContext = new Support.DataGrid_DataContext(this);
            Type = "1/1";
            MakeHeader();
        }

        public void MakeHeader()
        {
            var HeaderControl = Support.RenderDataGridHeader.Render.GetControl(Type);
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
