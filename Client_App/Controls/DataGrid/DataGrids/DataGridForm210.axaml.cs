using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids
{
    public class DataGridForm210 : DataGrid<Form210>
    {
        public DataGridForm210() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm210(string Name) : base(Name)
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