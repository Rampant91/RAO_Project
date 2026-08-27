using System;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Passports;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;

namespace Client_App.Views.Passports;

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


        //Костыль
        //Без этого содержимое таблицы не отображается 
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
        args.Cancel = true; // Сразу запрещаем закрытие окна, т.к., из-за асинхронности окно может закрыться в любой момент


        _isCloseConfirmed = true; // перед выходом из обработчика события стоит проверка на _isCloseConfirmed,
                                  // если true, то окно закроется,
                                  // если false, то не закроется
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

                if (_isCloseConfirmed) //выход из обработчика события
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

        var res = await Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" },
                    new ButtonDefinition { Name = "Отмена" }
                ],
                ContentTitle = "Сохранение изменений",
                ContentHeader = "Уведомление",
                ContentMessage = $"Сохранить паспорт на упаковку твердых радиоактивных отходов?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(this));

        #endregion

        var dbm = StaticConfiguration.DBModel;
        switch (res)
        {
            case "Да":
                {
                    _isCloseConfirmed = true;


                    try
                    {
                        await dbm.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions =
                            [
                                new ButtonDefinition { Name = "Ок" },
                            ],
                            ContentTitle = "Сохранение изменений",
                            ContentHeader = "Ошибка",
                            ContentMessage = $"Произошла ошибка во время попытки сохранения:\n" +
                                $"{ex.Message}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(this));
                    }

                    if (desktop.Windows.Count == 1)
                    {
                        desktop.MainWindow.WindowState = OwnerPrevState;

                        break;
                    }

                    args.Cancel = false;

                    break;
                }
            case "Нет":
                {
                    _isCloseConfirmed = true;
                    dbm.Restore();
                    await dbm.SaveChangesAsync();


                    break;
                }
            case "Отмена" or null:
                {
                    _isCloseConfirmed = false;
                    return;
                }
        }
        desktop.MainWindow.WindowState = OwnerPrevState;

        if (_isCloseConfirmed)      //выход из обработчика события
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