using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm212 : DataGrid<Form212>
{
    public DataGridForm212()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm212(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}