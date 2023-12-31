using Avalonia.Markup.Xaml;
using Models.Collections;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridReports : DataGrid<Reports>
{
    public DataGridReports()
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}