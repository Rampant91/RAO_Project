using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids
{
    public class DataGridForm25 : DataGrid<Form25>
    {
        public DataGridForm25() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm25(string Name) : base(Name)
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