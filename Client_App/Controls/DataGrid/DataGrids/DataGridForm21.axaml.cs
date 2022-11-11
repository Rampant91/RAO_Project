using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids
{
    public class DataGridForm21 : DataGrid<Form21>
    {
        public DataGridForm21() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm21(string Name) : base(Name)
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