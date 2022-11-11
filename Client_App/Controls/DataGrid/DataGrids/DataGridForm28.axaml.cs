using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm28 : DataGrid<Form28>
{
    public DataGridForm28()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm28(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}