using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form1;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm19 : DataGrid<Form19>
{
    public DataGridForm19()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm19(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
}