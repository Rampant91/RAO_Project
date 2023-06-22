using Avalonia.Markup.Xaml;
using Models;
using Models.Forms.Form1;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridForm14 : DataGrid<Form14>
{
    public DataGridForm14()
    {
        InitializeComponent();
        Init();
    }
    public DataGridForm14(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}