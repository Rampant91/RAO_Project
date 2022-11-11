using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm19 : DataGrid<Form19>
{
    public DataGridForm19()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm19(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}