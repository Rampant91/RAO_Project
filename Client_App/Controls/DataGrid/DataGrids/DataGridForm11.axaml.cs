using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids
{
    public class DataGridForm11 : DataGrid<Form11>
    {
        public DataGridForm11() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm11(string Name) : base(Name)
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