using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid
{
    public class DataGridForm12 : DataGrid<Form12>
    {
        public DataGridForm12() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm12(string Name) : base(Name)
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