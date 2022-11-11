using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm210 : DataGrid<Form210>
{
    public DataGridForm210()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm210(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}