using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid
{
    public class DataGridForm29 : DataGrid<Form29>
    {
        public DataGridForm29() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm29(string Name) : base(Name)
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