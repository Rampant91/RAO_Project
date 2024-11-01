using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form2;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm24 : DataGrid<Form24>
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