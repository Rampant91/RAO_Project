using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid
{
    public class DataGridForm27 : DataGrid<Form27>
    {
        public DataGridForm27() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm27(string Name) : base(Name)
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