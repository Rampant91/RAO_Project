using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form2;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridForm212 : DataGrid<Form212>
{
    public DataGridForm212()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm212(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
}