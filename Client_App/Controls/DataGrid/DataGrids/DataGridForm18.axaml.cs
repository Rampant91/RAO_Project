using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form1;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm18 : DataGrid<Form18>
{
    public DataGridForm18()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm18(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
}