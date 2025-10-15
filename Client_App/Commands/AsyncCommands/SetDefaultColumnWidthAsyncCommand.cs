using Avalonia.Controls;
using Client_App.Properties.ColumnWidthSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands
{
    public class SetDefaultColumnWidthAsyncCommand : BaseAsyncCommand
    {
        public override async Task AsyncExecute(object? parameter)
        {
            if (parameter is not DataGrid dataGrid) return;

            var columns = dataGrid.Columns;

            columns[0].Width = new DataGridLength(40);
            for (int i = 1; i < columns.Count; i++)
            {
                columns[i].Width = dataGrid.ColumnWidth;
            }
        }
    }
}
