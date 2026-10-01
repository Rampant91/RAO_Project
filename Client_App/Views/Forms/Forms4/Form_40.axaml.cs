using MsBox.Avalonia;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.Logger;
using Client_App.Resources;
using Client_App.Services;
using Client_App.ViewModels.Forms.Forms4;
using Client_App.Views.Forms;
using MsBox.Avalonia.Dto;
using Models.DBRealization;
using Models.Forms;

namespace Client_App.Views.Forms.Forms4;

public partial class Form_40 : BaseWindow<Form_40VM>
{
    private Form_40VM _vm = null!;

    public Form_40() { }

    public Form_40(Form_40VM vm)
    {
        _vm = vm;
        AvaloniaXamlLoader.Load(this);
        FormWindowNames.SetWindowKey(this, nameof(Form_40));
        DataContext = vm;
        Closing += OnStandardClosing;
    }

    #region OnStandartClosing

    private async void OnStandardClosing(object? sender, CancelEventArgs args)
    {
        if (DataContext is not Form_40VM vm) return;

        var desktop = (IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!;

        // New card: not in DB yet (DBO != null until first SaveReport).
        if (vm.DBO is not null)
        {
            if (!OrganizationListRefresh.Form40HasAnyFilledTitleFields(vm.Storage))
            {
                vm.DBO = null;
                desktop.MainWindow.WindowState = WindowState.Normal;
                return;
            }

            args.Cancel = true;
            await HandleNewOrganizationDraftClosingAsync(vm, desktop).ConfigureAwait(true);
            return;
        }

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

        var res = Dispatcher.UIThread.InvokeAsync(async () => await MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    FormDialogTexts.YesButton,
                    FormDialogTexts.NoButton
                ],
                ContentTitle = FormDialogTexts.SaveChangesTitle,
                ContentHeader = FormDialogTexts.NotificationHeader,
                ContentMessage = FormDialogTexts.SaveFormMessage(vm.FormType),
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            }).ShowWindowDialogAsync(desktop.MainWindow));

        #endregion

        await res.WaitAsync(new CancellationToken());
        var dbm = StaticConfiguration.DBModel;
        switch (res.Result)
        {
            case FormDialogTexts.Yes:
                {
                    try
                    {
                        await dbm.SaveChangesAsync();
                        await new SaveReportAsyncCommand(vm).AsyncExecute(null);
                    }
                    catch { }

                    if (desktop.Windows.Count == 1)
                    {
                        desktop.MainWindow.WindowState = WindowState.Normal;
                    }
                    return;
                }
            case FormDialogTexts.No:
                {
                    flag = true;
                    dbm.Restore();
                    try
                    {
                        await dbm.SaveChangesAsync();
                    }
                    catch { }

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

    private async Task HandleNewOrganizationDraftClosingAsync(
        Form_40VM vm,
        IClassicDesktopStyleApplicationLifetime desktop)
    {
        var answer = await Dispatcher.UIThread.InvokeAsync(async () => await MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    FormDialogTexts.YesButton,
                    FormDialogTexts.NoButton
                ],
                ContentTitle = FormDialogTexts.CreateOrganizationTitle,
                ContentHeader = FormDialogTexts.NotificationHeader,
                ContentMessage = FormDialogTexts.SaveNewOrganizationCardMessage,
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            }).ShowWindowDialogAsync(desktop.MainWindow));

        if (answer is FormDialogTexts.Yes)
        {
            try
            {
                await new SaveReportAsyncCommand(vm).AsyncExecute(null);
            }
            catch { }
        }
        else
        {
            vm.DBO = null;
        }

        desktop.MainWindow.WindowState = WindowState.Normal;
        Close();
    }

    #endregion
}
