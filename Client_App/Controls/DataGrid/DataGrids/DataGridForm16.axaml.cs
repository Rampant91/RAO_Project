using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form1;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm16 : DataGrid<Form16>
{
    public DataGridForm16()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm16(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
}