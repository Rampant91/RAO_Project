using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form2;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm22 : DataGrid<Form22>
{
    public DataGridForm22()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm22(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}