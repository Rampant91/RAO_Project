using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Forms.Forms1;
using Models.CheckForm;
using System.Collections.Generic;

namespace Client_App.Views
{

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
}