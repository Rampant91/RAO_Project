using Avalonia.Markup.Xaml;
using Models;
using Models.Forms;

namespace Client_App.Controls.DataGrid.DataGrids;

public partial class DataGridNote : DataGrid<Note>
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
}