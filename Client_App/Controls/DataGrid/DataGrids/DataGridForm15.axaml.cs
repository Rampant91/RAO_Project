using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm15 : DataGrid<Form15>
{
    public DataGridForm15()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm15(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}