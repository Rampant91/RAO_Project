using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form1;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm11 : DataGrid<Form11>
{
    public DataGridForm11() : base()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm11(string Name) : base(Name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}