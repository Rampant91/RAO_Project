using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids
{
    public class DataGridForm23 : DataGrid<Form23>
    {
        public DataGridForm23() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm23(string Name) : base(Name)
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