using Avalonia.Markup.Xaml;
using Models.Collections;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridReport : DataGrid<Report>
{
    public DataGridReport()
    {
        InitializeComponent();
        Init();
    }
    public DataGridReport(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}