using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm17 : DataGrid<Form17>
{
    public DataGridForm17()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm17(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}