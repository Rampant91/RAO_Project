using MsBox.Avalonia;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.ViewModels.StoragePoints;
using Client_App.Views;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Models.DBRealization;
using Models.StoragePoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Change
{
    public class DeleteStoragePointAsyncCommand(StoragePointsMenuWindowVM storagePointMenuVM) : BaseAsyncCommand
    {
        StoragePointsMenuWindowVM _storagePointMenuVM => storagePointMenuVM;
        public override async Task AsyncExecute(object? parameter)
        {
            if (parameter is not StoragePoint storagePoint) return;
            try
            {

                var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxCustom(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Да", IsDefault = true },
                        new ButtonDefinition { Name = "Нет", IsCancel = true }
                    ],
                    ContentTitle = "Уведомление",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Вы действительно хотите удалить паспорт?",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                }).ShowWindowDialogAsync(Desktop.MainWindow));

                if (answer is "Да")
                {
                    var dbm = StaticConfiguration.DBModel;
                    dbm.storage_point.Remove(storagePoint);
                    dbm.SaveChangesAsync();
                    _storagePointMenuVM.UpdateStoragePoints();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
