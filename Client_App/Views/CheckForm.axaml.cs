using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels;
using Models.CheckForm;
using System.Collections.Generic;
using Avalonia.Media;

namespace Client_App.Views;

public class CheckForm : BaseWindow<CheckFormVM>
{
    #region Constructor

    public CheckForm() { }

    public CheckForm(ChangeOrCreateVM changeOrCreateVM, List<CheckError> checkError)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new CheckFormVM(changeOrCreateVM, checkError);

        var dataGrid = this.Get<DataGrid>("CheckErrorsDataGrid");
        dataGrid.LoadingRow += DataGrid_LoadingRow;

        Show();
    }

    #endregion

    #region Events

    /// <summary>
    /// Устанавливает цвет строки в зависимости от того, является ли ошибка критической.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        if (sender is DataGrid)
        {
            if (e.Row?.DataContext is CheckError { IsCritical: true })
            {
                e.Row.Background = Brushes.RosyBrown;
            }
        }
    }

    #endregion
}