using Avalonia.Controls;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Passports;
using Client_App.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Passports
{
    public class AddPackagePassportAsyncCommand : BaseAsyncCommand
    {

        public override async Task AsyncExecute(object? parameter)
        {
            try
            {
                var mainWindow = Desktop.MainWindow as MainWindow;
                var mainWindowVM = mainWindow.DataContext as MainWindowVM;

                var packagePassportWindow = new PackagePassportWindow(new PackagePassportWindowVM());
                await packagePassportWindow.ShowDialog(mainWindow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
