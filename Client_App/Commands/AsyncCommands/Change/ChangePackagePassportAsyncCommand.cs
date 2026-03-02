using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.ViewModels;
using Client_App.ViewModels.Passports;
using Client_App.Views;
using Models.DBRealization;
using Models.Passports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Change
{
    public class ChangePackagePassportAsyncCommand : BaseAsyncCommand
    {
        public override async Task AsyncExecute(object? parameter)
        {
            if (parameter is not PackagePassport passport) return;
            try
            {
                var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                        .FirstOrDefault(w => w.Name == "PassportMenu");

                var passportVM = new PackagePassportWindowVM(passport);


                var packagePassportWindow = new PackagePassportWindow(passportVM);
                await packagePassportWindow.ShowDialog(owner);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
