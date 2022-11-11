using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids
{
    public class DataGridForm14 : DataGrid<Form14>
    {
        public DataGridForm14() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm14(string Name) : base(Name)
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