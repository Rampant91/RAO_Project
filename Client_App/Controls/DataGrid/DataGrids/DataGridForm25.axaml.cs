using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form2;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm25 : DataGrid<Form25>
{
    public DataGridForm25()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm25(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
}