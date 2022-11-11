using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids
{
    public class DataGridForm22 : DataGrid<Form22>
    {
        public DataGridForm22() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm22(string Name) : base(Name)
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