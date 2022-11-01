using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid
{
    public class DataGridForm212 : DataGrid<Form212>
    {
        public DataGridForm212() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridForm212(string Name) : base(Name)
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