using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form2;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm210 : DataGrid<Form210>
{
    public DataGridForm210()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm210(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
}