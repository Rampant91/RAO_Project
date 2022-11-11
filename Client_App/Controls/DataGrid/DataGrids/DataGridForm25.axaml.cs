using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm25 : DataGrid<Form25>
{
    public DataGridForm25()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm25(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}