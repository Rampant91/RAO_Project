using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm23 : DataGrid<Form23>
{
    public DataGridForm23()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm23(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}