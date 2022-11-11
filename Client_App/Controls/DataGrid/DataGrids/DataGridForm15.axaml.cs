using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids
{
    public class DataGridForm15 : DataGrid<Form15>
    {
        public DataGridForm15() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm15(string Name) : base(Name)
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