using Avalonia.Markup.Xaml;
using Models.Collections;

namespace Client_App.Controls.DataGrid
{
    public class DataGridReport : DataGrid<Report>
    {
        public DataGridReport() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridReport(string Name) : base(Name)
        {
            InitializeComponent();
            this.Init();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

}