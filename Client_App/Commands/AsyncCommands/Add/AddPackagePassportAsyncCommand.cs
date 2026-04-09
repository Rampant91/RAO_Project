using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Passports;
using Client_App.Views;
using Models.DBRealization;
using Models.Passports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Add
{
    public class AddPackagePassportAsyncCommand(PassportsMenuWindowVM passportMenuVM) : BaseAsyncCommand
    {
        PassportsMenuWindowVM _passportMenuVM => passportMenuVM;
        public override async Task AsyncExecute(object? parameter)
        {
            try
            {
                var mainWindow = Desktop.MainWindow as MainWindow;
                var mainWindowVM = mainWindow.DataContext as MainWindowVM;

                var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                        .FirstOrDefault(w => w.Name == "PassportMenu");

                var passportVM = new PackagePassportWindowVM();
                passportVM.Passport.ContentCharacteristics.Add(new CharacteristicPrimaryPackage(passportVM.Passport));

                var dbm = StaticConfiguration.DBModel;

                dbm.package_passport.Add(passportVM.Passport);

                var packagePassportWindow = new PackagePassportWindow(passportVM);
                await packagePassportWindow.ShowDialog(owner);

                _passportMenuVM.UpdatePassports();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
