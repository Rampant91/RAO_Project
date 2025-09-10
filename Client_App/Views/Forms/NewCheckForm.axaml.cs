using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms;
using Models.CheckForm;

namespace Client_App.Views.Forms;

public class NewCheckForm : BaseWindow<CheckFormVM>
{
    #region Constructor

    public NewCheckForm() { }

    public NewCheckForm(BaseFormVM formVM, List<CheckError> checkError)
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = new NewCheckFormVM(formVM, checkError);

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
        if (sender is DataGrid && e.Row is not null)
        {
            e.Row.Background = e.Row.DataContext switch
            {
                CheckError { IsCritical: true } => Brushes.RosyBrown,

                CheckError { IsCritical: false } => e.Row.GetIndex() % 2 == 0
                    ? Brushes.LightBlue
                    : Brushes.White,

                _ => e.Row.Background
            };
        }
    }

    #endregion
}