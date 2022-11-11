using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm211 : DataGrid<Form211>
{
    public DataGridForm211()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm211(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}