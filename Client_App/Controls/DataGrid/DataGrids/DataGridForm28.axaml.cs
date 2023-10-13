using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form2;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm28 : DataGrid<Form28>
{
    public DataGridForm28()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm28(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
}