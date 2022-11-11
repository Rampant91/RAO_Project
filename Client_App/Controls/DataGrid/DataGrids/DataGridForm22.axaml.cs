using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm22 : DataGrid<Form22>
{
    public DataGridForm22()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm22(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}