using MsBox.Avalonia;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Passports;
using Client_App.Views;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.Passports;
using System;
using System.ComponentModel;
using System.Linq;

namespace Client_App;

public partial class PackagePassportWindow : BaseWindow<PackagePassportWindowVM>
{

    public PackagePassportWindowVM? VM
    {
        get
        {
            if (DataContext is PackagePassportWindowVM)
                return DataContext as PackagePassportWindowVM;
            else
                return null;

        }
    }
    public PackagePassportWindow()
    {
        InitializeComponent();

        Closing += OnStandardClosing;
    }
    public PackagePassportWindow(PackagePassportWindowVM vm)
    {
        InitializeComponent();
        DataContext = vm;

        Closing += OnStandardClosing;


    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        #if DEBUG
            this.AttachDevTools();
        #endif
        WindowState = WindowState.Maximized;


        //�������
        //��� ����� ���������� ������� �� ������������ 
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var dataGrid2 = this.FindControl<DataGrid>("dataGrid2");
            dataGrid2.InvalidateMeasure();
        });


    }

    #region OnStandartClosing

    bool _isCloseConfirmed;

    private async void OnStandardClosing(object? sender, CancelEventArgs args)
    {
        args.Cancel = true; // ����� ��������� �������� ����, �.�. ��-�� ������������ ���� ����� ��������� � ����� ������


        _isCloseConfirmed = true; // ����� ������� �� ����������� ������� ����� �������� �� _isCloseConfirmed,
                                  // ���� true, �� ���� ���������,
                                  // ���� false, �� �� ���������
        if (DataContext is not PackagePassportWindowVM vm) return;


        var desktop = (IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!;
        try
        {
            var db = StaticConfiguration.DBModel;

            var modifiedEntities = db.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged);

            if (modifiedEntities.Count() <= 0 
                || vm.SkipChangeTracking)
            {
                if (vm.SkipChangeTracking) vm.SkipChangeTracking = false;
                desktop.MainWindow.WindowState = OwnerPrevState;

                if (_isCloseConfirmed) //����� �� ����������� �������
                {
                    Closing -= OnStandardClosing;
                    Close();
                }

                return;
            }
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }

        args.Cancel = true;

        #region MessageSaveChanges

        var res = await Dispatcher.UIThread.InvokeAsync(async () => await MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "��" },
                    new ButtonDefinition { Name = "���" },
                    new ButtonDefinition { Name = "������" }
                ],
                ContentTitle = "���������� ���������",
                ContentHeader = "�����������",
                ContentMessage = $"��������� ������� �� �������� ������� ������������� �������?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            }).ShowWindowDialogAsync(this));

        #endregion

        var dbm = StaticConfiguration.DBModel;
        switch (res)
        {
            case "��":
                {
                    _isCloseConfirmed = true;


                    try
                    {
                        await dbm.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.UIThread.InvokeAsync(async () => await MessageBoxManager
                        .GetMessageBoxCustom(new MessageBoxCustomParams
                        {
                            ButtonDefinitions =
                            [
                                new ButtonDefinition { Name = "��" },
                            ],
                            ContentTitle = "���������� ���������",
                            ContentHeader = "������",
                            ContentMessage = $"��������� ������ �� ����� ������� ����������:\n" +
                                $"{ex.Message}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        }).ShowWindowDialogAsync(this));
                    }

                    if (desktop.Windows.Count == 1)
                    {
                        desktop.MainWindow.WindowState = OwnerPrevState;

                        break;
                    }

                    args.Cancel = false;

                    break;
                }
            case "���":
                {
                    _isCloseConfirmed = true;
                    dbm.Restore();
                    await dbm.SaveChangesAsync();


                    break;
                }
            case "������" or null:
                {
                    _isCloseConfirmed = false;
                    return;
                }
        }
        desktop.MainWindow.WindowState = OwnerPrevState;

        if (_isCloseConfirmed)      //����� �� ����������� �������
        {
            Closing -= OnStandardClosing;
            Close();
        }
    }

    private void Binding(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }



    #endregion
}