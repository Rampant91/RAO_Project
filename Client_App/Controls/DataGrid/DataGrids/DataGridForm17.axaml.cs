using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form1;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm17 : DataGrid<Form17>
{
    public DataGridForm17()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm17(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}