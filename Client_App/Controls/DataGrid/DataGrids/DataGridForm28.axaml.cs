using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid
{
    public class DataGridForm28 : DataGrid<Form28>
    {
        public DataGridForm28() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm28(string Name) : base(Name)
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