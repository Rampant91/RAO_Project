using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm21 : DataGrid<Form21>
{
    public DataGridForm21()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm21(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}