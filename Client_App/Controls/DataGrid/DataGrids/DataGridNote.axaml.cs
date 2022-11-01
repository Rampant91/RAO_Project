using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid
{
    public class DataGridNote : DataGrid<Note>
    {
        public DataGridNote() : base()
        {
            InitializeComponent();
            this.Init();
        }
        public DataGridNote(string Name) : base(Name)
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