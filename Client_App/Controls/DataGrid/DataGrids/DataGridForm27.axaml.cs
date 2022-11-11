using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm27 : DataGrid<Form27>
{
    public DataGridForm27()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm27(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}