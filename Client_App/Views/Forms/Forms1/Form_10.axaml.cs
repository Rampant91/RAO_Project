using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Forms.Forms1;
using Models.DBRealization;
using Models.Forms;
using System;
using System.ComponentModel;
using System.Threading;
using Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Avalonia.Controls;

namespace Client_App.Views.Forms.Forms1;

public partial class Form_10 : BaseWindow<Form_10VM>
{
    private Form_10VM _vm = null!;

    public Form_10() { }

    public Form_10(Form_10VM vm)
    {
        AvaloniaXamlLoader.Load(this);
        _vm = vm;
        Closing += OnStandardClosing;
    }

    #region OnStandartClosing

    private async void OnStandardClosing(object? sender, CancelEventArgs args)
    {
        if (DataContext is not Form_10VM vm) return;

        var desktop = (IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!;
        try
        {
            if (!StaticConfiguration.DBModel.ChangeTracker.HasChanges())
            {
                desktop.MainWindow.WindowState = WindowState.Normal;
                return;
            }
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }

        var flag = false;

        #region MessageRemoveEmptyForms

        var res = Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" }
                ],
                ContentTitle = "Сохранение изменений",
                ContentHeader = "Уведомление",
                ContentMessage = $"Сохранить форму {vm.FormType}?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(desktop.MainWindow));

        #endregion

        await res.WaitAsync(new CancellationToken());
        var dbm = StaticConfiguration.DBModel;
        switch (res.Result)
        {
            case "Да":
            {
                await dbm.SaveChangesAsync();
                await new SaveReportAsyncCommand(vm).AsyncExecute(null);
                if (desktop.Windows.Count == 1)
                {
                    desktop.MainWindow.WindowState = WindowState.Normal;
                }
                return;
            }
            case "Нет":
            {
                flag = true;
                dbm.Restore();
                await dbm.SaveChangesAsync();

                var lst = vm.Storage[vm.FormType];

                foreach (var key in lst)
                {
                    var item = (Form)key;
                    if (item.Id == 0)
                    {
                        vm.Storage[vm.Storage.FormNum_DB].Remove(item);
                    }
                }

                vm.Storage.OnPropertyChanged(nameof(vm.Storage.RegNoRep));
                vm.Storage.OnPropertyChanged(nameof(vm.Storage.ShortJurLicoRep));
                vm.Storage.OnPropertyChanged(nameof(vm.Storage.OkpoRep));

                break;
            }
        }
        desktop.MainWindow.WindowState = WindowState.Normal;
        if (flag)
        {
            Close();
        }
        args.Cancel = true;
    }

    #endregion
}