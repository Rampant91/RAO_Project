using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Forms.Forms2;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.Forms;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Client_App.Views.Forms.Forms1;

public partial class Form_10 : BaseWindow<Form_10VM>
{
    private protected static readonly IClassicDesktopStyleApplicationLifetime Desktop =
        (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;

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

        var db = StaticConfiguration.DBModel;
        var window = Desktop.Windows.FirstOrDefault(x => x.Name is "1.0");
        var reportsAlreadyExist = db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(reps => reps.Master_DB)
            .ThenInclude(report => report.Rows10)
            .Where(reps => reps.DBObservable != null)
            .ToList()
            .Any(x => x.Master_DB.RegNoRep.Value == _vm.Storage.RegNoRep.Value
                      && !string.IsNullOrWhiteSpace(_vm.Storage.RegNoRep.Value)
                      && x.Master_DB.OkpoRep.Value == _vm.Storage.OkpoRep.Value
                      && !string.IsNullOrWhiteSpace(_vm.Storage.OkpoRep.Value)
                      && x.Master_DB.Id != _vm.Storage.Id);

        if (reportsAlreadyExist)
        {
            args.Cancel = true;

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Ошибка при сохранении титульного листа организации",
                    ContentHeader = "Ошибка",
                    ContentMessage =
                        $"Не удалось сохранить изменения в титульном листе организации, " +
                        $"поскольку организация с данными ОКПО и рег.№ уже существует в базе данных. " +
                        $"Убедитесь в правильности заполнения ОКПО и рег.№.",
                    MinWidth = 400,
                    MaxWidth = 600,
                    MinHeight = 150,
                    MaxHeight = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(window ?? Desktop.MainWindow));

            return;
        }

        var jurForm10 = vm.Storage.Rows10[0];
        var obForm10 = vm.Storage.Rows10[1];

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
            var answer = await Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Да" },
                        new ButtonDefinition { Name = "Нет" }
                    ],
                    ContentTitle = "Форма 1.0",
                    ContentHeader = "Уведомление",
                    ContentMessage = "При заполнении данных обособленного территориального подразделения, " +
                                     $"{Environment.NewLine}также необходимо заполнить данные юридического лица. " +
                                     $"{Environment.NewLine}Вы уверены, что хотите закрыть форму, " +
                                     $"оставив данные юридического лица незаполненными?",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow));

            if (answer is not "Да")
            {
                args.Cancel = true;
                return;
            }
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
            case "Нет":
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

    #endregion
}