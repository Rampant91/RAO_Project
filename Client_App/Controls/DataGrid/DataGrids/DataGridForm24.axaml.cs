using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm24 : DataGrid<Form24>
{
    public DataGridForm24()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm24(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}