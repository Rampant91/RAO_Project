using Client_App.ViewModels;
using Client_App.ViewModels.Passports;
using Client_App.Views;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands
{
    public class OpenPassportMenuWindowAsyncCommand : BaseAsyncCommand
    {
        public override async Task AsyncExecute(object? parameter)
        {
            try
            {
                var mainWindow = Desktop.MainWindow as MainWindow;
                var mainWindowVM = mainWindow.DataContext as MainWindowVM;

                var passportMenuVM = new PassportsMenuWindowVM();

                var passportMenu = new PassportsMenuWindow(passportMenuVM);
                await passportMenu.ShowDialog(mainWindow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
