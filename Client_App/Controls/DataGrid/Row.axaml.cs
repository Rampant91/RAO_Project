using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Controls.DataGrid
{
    public class Row : StackPanel
    {
        private int _SRow = -1;
        public int SRow
        {
            get => _SRow;
            set => _SRow = value;
        }
        public Row() : base()
        {
            Initialization();
        }

        private void Initialization()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
