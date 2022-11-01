using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid
{
    public class DataGridForm19 : DataGrid<Form19>
    {
        public DataGridForm19() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm19(string Name) : base(Name)
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