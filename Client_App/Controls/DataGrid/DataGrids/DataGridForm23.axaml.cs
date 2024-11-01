using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form2;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm23 : DataGrid<Form23>
{
    public DataGridForm23()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm23(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}