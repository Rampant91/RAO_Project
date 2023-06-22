using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form2;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm26 : DataGrid<Form26>
{
    public DataGridForm26()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm26(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}