using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form2;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm27 : DataGrid<Form27>
{
    public DataGridForm27()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm27(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
}