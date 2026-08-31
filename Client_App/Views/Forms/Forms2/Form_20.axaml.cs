using MsBox.Avalonia;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Forms.Forms2;
using Client_App.Views;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.Forms;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;

using MsBox.Avalonia.Enums;
namespace Client_App.Views.Forms.Forms2;

public partial class Form_20 : BaseWindow<Form_20VM>{
    private protected static readonly IClassicDesktopStyleApplicationLifetime Desktop =
        (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;

    private Form_20VM _vm = null!;

    public Form_20() { }

    public Form_20(Form_20VM vm)
    {
        DataContext = vm;
        _vm = vm;
        AvaloniaXamlLoader.Load(this);
        Name = "2.0";
        Closing += OnStandardClosing;
    }

    #region OnStandartClosing

    private async void OnStandardClosing(object? sender, CancelEventArgs args)
    {
        if (DataContext is not Form_20VM vm) return;

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
        var window = Desktop.Windows.FirstOrDefault(x => x.Name is "2.0");

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

        if (string.IsNullOrWhiteSpace(_vm.Storage.RegNoRep.Value)
            && string.IsNullOrWhiteSpace(_vm.Storage.OkpoRep.Value) 
            || !await query
                .AnyAsync(reps => reps.Master_DB.Rows20
                    .Any(form20 => form20.RegNo_DB == regNum
                                   && form20.Okpo_DB == okpo)))
        {
            reportsAlreadyExist = false;
        }
        else
        {
            reportsAlreadyExist = query
                .ToList()
                .Any(x => x.Master_DB.FormNum_DB == _vm.FormType
                          && x.Master_DB.RegNoRep.Value == regNum
                          && x.Master_DB.OkpoRep.Value == okpo
                          && x.Master_DB.Id != _vm.Storage.Id);
        }

        if (reportsAlreadyExist)
        {
            args.Cancel = true;

            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "������ ��� ���������� ���������� ����� �����������",
                    ContentHeader = "������",
                    ContentMessage =
                        $"�� ������� ��������� ��������� � ��������� ����� �����������, " +
                        $"��������� ����������� � ������� ���� � ���.� ��� ���������� � ���� ������. " +
                        $"��������� � ������������ ���������� ���� � ���.�.",
                    MinWidth = 400,
                    MaxWidth = 600,
                    MinHeight = 150,
                    MaxHeight = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(window ?? Desktop.MainWindow));

            return;
        }

        var flag = false;

        #region MessageRemoveEmptyForms

        var res = Dispatcher.UIThread.InvokeAsync(async () => await MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "��" },
                    new ButtonDefinition { Name = "���" }
                ],
                ContentTitle = "���������� ���������",
                ContentHeader = "�����������",
                ContentMessage = $"��������� ����� {vm.FormType}?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            }).ShowWindowDialogAsync(desktop.MainWindow));

        #endregion

        await res.WaitAsync(new CancellationToken());
        var dbm = StaticConfiguration.DBModel;
        switch (res.Result)
        {
            case "��":
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
            case "���":
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