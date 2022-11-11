using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm29 : DataGrid<Form29>
{
    public DataGridForm29()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm29(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}