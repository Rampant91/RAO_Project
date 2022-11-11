using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids
{
    public class DataGridForm211 : DataGrid<Form211>
    {
        public DataGridForm211() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm211(string Name) : base(Name)
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