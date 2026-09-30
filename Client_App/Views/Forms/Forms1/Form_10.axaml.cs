using MsBox.Avalonia;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.Logger;
using Client_App.Resources;
using Client_App.Services;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.Views.Forms;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.Forms;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MsBox.Avalonia.Enums;

namespace Client_App.Views.Forms.Forms1;

public partial class Form_10 : BaseWindow<Form_10VM>
{
    private protected static readonly IClassicDesktopStyleApplicationLifetime Desktop =
        (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;

    private Form_10VM _vm = null!;

    public Form_10() { }

    public Form_10(Form_10VM vm)
    {
        _vm = vm;
        AvaloniaXamlLoader.Load(this);
        FormWindowNames.SetWindowKey(this, nameof(Form_10));
        DataContext = vm;
        Closing += OnStandardClosing;
    }

    #region OnStandartClosing

    private async void OnStandardClosing(object? sender, CancelEventArgs args)
    {
        if (DataContext is not Form_10VM vm) return;

        var desktop = (IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!;

        // Новая карточка: ещё не в БД (DBO != null до первого SaveReport).
        if (vm.DBO is not null)
        {
            if (!OrganizationListRefresh.Form10HasAnyFilledTitleFields(vm.Storage))
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

        var db = StaticConfiguration.DBModel;
        var window = FormWindowNames.FindOpenFormWindow("1.0") ?? this;

        var query = db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(reps => reps.Master_DB).ThenInclude(report => report.Rows10)
            .Include(reps => reps.Master_DB).ThenInclude(report => report.Rows20)
            .Where(reps => reps.DBObservable != null);

        var regNum = _vm.Storage.RegNoRep.Value;
        var okpo = _vm.Storage.OkpoRep.Value;
        bool reportsAlreadyExist;

        if (string.IsNullOrWhiteSpace(regNum)
            && string.IsNullOrWhiteSpace(okpo)
            || !await query
                .AnyAsync(reps => reps.Master_DB.Rows10
                    .Any(form10 => form10.RegNo_DB == regNum
                                   && form10.Okpo_DB == okpo)))
        {
            reportsAlreadyExist = false;
        }
        else
        {
            reportsAlreadyExist = query
                .ToList()
                .Any(x => x.Master_DB.FormNum_DB == vm.FormType
                          && x.Master_DB.RegNoRep.Value == regNum
                          && x.Master_DB.OkpoRep.Value == okpo
                          && x.Master_DB.Id != _vm.Storage.Id);
        }

        var jurForm10 = vm.Storage.Rows10[0];

        if ((!string.IsNullOrWhiteSpace(jurForm10.SubjectRF_DB)
            || !string.IsNullOrWhiteSpace(jurForm10.JurLico_DB)
            || !string.IsNullOrWhiteSpace(jurForm10.ShortJurLico_DB)
            || !string.IsNullOrWhiteSpace(jurForm10.JurLicoAddress_DB)
            || !string.IsNullOrWhiteSpace(jurForm10.JurLicoFactAddress_DB)
            || !string.IsNullOrWhiteSpace(jurForm10.GradeFIO_DB)
            || !string.IsNullOrWhiteSpace(jurForm10.Okpo_DB)
            || !string.IsNullOrWhiteSpace(jurForm10.Okved_DB)
            || !string.IsNullOrWhiteSpace(jurForm10.Okogu_DB)
            || !string.IsNullOrWhiteSpace(jurForm10.Oktmo_DB)
            || !string.IsNullOrWhiteSpace(jurForm10.Inn_DB)
            || !string.IsNullOrWhiteSpace(jurForm10.Kpp_DB)
            || !string.IsNullOrWhiteSpace(jurForm10.Okopf_DB)
            || !string.IsNullOrWhiteSpace(jurForm10.Okfs_DB))
            && (string.IsNullOrWhiteSpace(jurForm10.SubjectRF_DB)
                || string.IsNullOrWhiteSpace(jurForm10.JurLico_DB)
                || string.IsNullOrWhiteSpace(jurForm10.ShortJurLico_DB)
                || string.IsNullOrWhiteSpace(jurForm10.JurLicoAddress_DB)
                || string.IsNullOrWhiteSpace(jurForm10.JurLicoFactAddress_DB)
                || string.IsNullOrWhiteSpace(jurForm10.GradeFIO_DB)
                || string.IsNullOrWhiteSpace(jurForm10.Okpo_DB)
                || string.IsNullOrWhiteSpace(jurForm10.Okved_DB)
                || string.IsNullOrWhiteSpace(jurForm10.Okogu_DB)
                || string.IsNullOrWhiteSpace(jurForm10.Oktmo_DB)
                || string.IsNullOrWhiteSpace(jurForm10.Inn_DB)
                || string.IsNullOrWhiteSpace(jurForm10.Kpp_DB)
                || string.IsNullOrWhiteSpace(jurForm10.Okopf_DB)
                || string.IsNullOrWhiteSpace(jurForm10.Okfs_DB)))
        {
            var answer = await Dispatcher.UIThread.InvokeAsync(async () => await MessageBoxManager
                .GetMessageBoxCustom(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        FormDialogTexts.YesButton,
                        FormDialogTexts.NoButton
                    ],
                    ContentTitle = FormDialogTexts.SaveChangesTitle,
                    ContentHeader = FormDialogTexts.NotificationHeader,
                    ContentMessage = FormDialogTexts.IncompleteJuridicalPersonFieldsCloseMessage,
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(desktop.MainWindow));

            if (answer is not FormDialogTexts.Yes)
            {
                args.Cancel = true;
                return;
            }
        }

        var flag = false;

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
        Form_10VM vm,
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
