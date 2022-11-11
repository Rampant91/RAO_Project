using Avalonia.Markup.Xaml;
using Models;

namespace Client_App.Controls.DataGrid.DataGrids;

public class DataGridNote : DataGrid<Note>
{
    public DataGridNote()
    {
        InitializeComponent();
        Init();
    }
    public DataGridNote(string name) : base(name)
    {
        InitializeComponent();
        Init();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}