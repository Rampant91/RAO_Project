using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Controls.DataGrid
{
    public class Row : StackPanel
    {
        int _SRow = -1;
        public int SRow
        {
            get { return _SRow; }
            set { _SRow = value; }
        }
        public Row() : base()
        {
            Initialization();
        }
        void Initialization()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
