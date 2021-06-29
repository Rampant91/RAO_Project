using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Controls.DataGrid
{
    public class Row : StackPanel
    {
        public Row()
        {
            Initialization();
        }

        public int SRow { get; set; } = -1;

        private void Initialization()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}