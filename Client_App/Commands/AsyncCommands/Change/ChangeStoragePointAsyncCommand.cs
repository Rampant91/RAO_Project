using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.ViewModels;
using Client_App.ViewModels.StoragePoints;
using Client_App.Views;
using Models.DBRealization;
using Models.StoragePoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Change
{
    public class ChangeStoragePointAsyncCommand : BaseAsyncCommand
    {
        public override async Task AsyncExecute(object? parameter)
        {
            if (parameter is not int storagePointId) return;
            try
            {
                
                var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                        .FirstOrDefault(w => w.Name == "StoragePointMenu");

                var storagePointVM = new StoragePointWindowVM(storagePointId);

                var packageStoragePointWindow = new StoragePointWindow(storagePointVM);
                await packageStoragePointWindow.ShowDialog(owner);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
