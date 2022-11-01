using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid
{
    public class DataGridForm16 : DataGrid<Form16>
    {
        public DataGridForm16() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm16(string Name) : base(Name)
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