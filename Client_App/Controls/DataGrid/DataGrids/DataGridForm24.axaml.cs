using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid
{
    public class DataGridForm24 : DataGrid<Form24>
    {
        public DataGridForm24() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm24(string Name) : base(Name)
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