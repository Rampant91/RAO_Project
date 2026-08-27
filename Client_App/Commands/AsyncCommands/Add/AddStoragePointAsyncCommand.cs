using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.StoragePoints;
using Client_App.Views;
using Models.DBRealization;
using Models.StoragePoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Add
{
    public class AddStoragePointAsyncCommand(StoragePointsMenuWindowVM storagePointMenuVM) : BaseAsyncCommand
    {
        StoragePointsMenuWindowVM _storagePointMenuVM => storagePointMenuVM;
        public override async Task AsyncExecute(object? parameter)
        {
            try
            {
                var mainWindow = Desktop.MainWindow as MainWindow;
                var mainWindowVM = mainWindow.DataContext as MainWindowVM;

                var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                        .FirstOrDefault(w => w.Name == "StoragePointMenu");

                var storagePointVM = new StoragePointWindowVM();

                var dbm = StaticConfiguration.DBModel;
                dbm.storage_point.Add(storagePointVM.StoragePoint);

                var storagePointWindow = new StoragePointWindow(storagePointVM);

                await storagePointWindow.ShowDialog(owner);

                _storagePointMenuVM.UpdateStoragePoints();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
