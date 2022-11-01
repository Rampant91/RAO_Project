using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid
{
    public class DataGridForm18 : DataGrid<Form18>
    {
        public DataGridForm18() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm18(string Name) : base(Name)
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