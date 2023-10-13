using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form1;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm13 : DataGrid<Form13>
{
    public DataGridForm13()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm13(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
}