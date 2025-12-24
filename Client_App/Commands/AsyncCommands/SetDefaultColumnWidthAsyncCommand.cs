using Avalonia.Controls;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

public class SetDefaultColumnWidthAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not DataGrid dataGrid) return;

        var columns = dataGrid.Columns;

        columns[0].Width = new DataGridLength(40);
        for (var i = 1; i < columns.Count; i++)
        {
            columns[i].Width = dataGrid.ColumnWidth;
        }
    }
}