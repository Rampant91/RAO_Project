using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids
{
    public class DataGridForm17 : DataGrid<Form17>
    {
        public DataGridForm17() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm17(string Name) : base(Name)
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