using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids
{
    public class DataGridForm13 : DataGrid<Form13>
    {
        public DataGridForm13() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm13(string Name) : base(Name)
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