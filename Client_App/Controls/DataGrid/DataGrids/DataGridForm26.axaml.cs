using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid
{
    public class DataGridForm26 : DataGrid<Form26>
    {
        public DataGridForm26() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm26(string Name) : base(Name)
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