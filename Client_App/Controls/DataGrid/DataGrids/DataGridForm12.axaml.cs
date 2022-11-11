using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm12 : DataGrid<Form12>
{
    public DataGridForm12()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm12(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}