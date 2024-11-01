using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form2;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm29 : DataGrid<Form29>
{
    public DataGridForm29()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm29(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}