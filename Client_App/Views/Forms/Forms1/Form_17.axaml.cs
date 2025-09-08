using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.Controls.DataGrid;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Commands.SyncCommands;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace Client_App.Views.Forms.Forms1;

public partial class Form_17 : BaseWindow<Form_17VM>
{

    //private Form_17VM _vm = null!;

    public Form_17()
    {
        InitializeComponent();
        DataContext = new Form_17VM();
        Show();
    }

    public Form_17(Form_17VM vm)
    {
        InitializeComponent();
        DataContext = vm;
        Closing += OnStandardClosing;
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        WindowState = WindowState.Maximized;
    }

    //Временное узкоспециализированное решение
    private void CopyExecutorData_Click(object sender, RoutedEventArgs e)
    {
        var command = new NewCopyExecutorDataAsyncCommand((Form_17VM)DataContext);
        if (command.CanExecute(null))
        {
            command.Execute(null);
        }
    }

    #region OnStandartClosing

    private async void OnStandardClosing(object? sender, CancelEventArgs args)
    {
        if (DataContext is not Form_17VM vm) return;
        try
        {
            await RemoveEmptyForms(vm);
            await CheckPeriod(vm);
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }
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
                    new SortFormSyncCommand(vm).Execute(null);
                    await dbm.SaveChangesAsync();

                    var lst = vm.Report[vm.FormType];

                    foreach (var key in lst)
                    {
                        var item = (Form)key;
                        if (item.Id == 0)
                        {
                            vm.Report[vm.Report.FormNum_DB].Remove(item);
                        }
                    }

                    var lstNote = vm.Report.Notes.ToList<Note>();
                    foreach (var item in lstNote.Where(item => item.Id == 0))
                    {
                        vm.Report.Notes.Remove(item);
                    }

                    if (vm.FormType is not "1.0" and not "2.0")
                    {
                        if (vm.FormType.Split('.')[0] == "1")
                        {
                            vm.Report.OnPropertyChanged(nameof(vm.Report.StartPeriod));
                            vm.Report.OnPropertyChanged(nameof(vm.Report.EndPeriod));
                            vm.Report.OnPropertyChanged(nameof(vm.Report.CorrectionNumber));
                        }
                        else if (vm.FormType.Split('.')[0] == "2")
                        {
                            vm.Report.OnPropertyChanged(nameof(vm.Report.Year));
                            vm.Report.OnPropertyChanged(nameof(vm.Report.CorrectionNumber));
                        }
                    }
                    else
                    {
                        vm.Report.OnPropertyChanged(nameof(vm.Report.RegNoRep));
                        vm.Report.OnPropertyChanged(nameof(vm.Report.ShortJurLicoRep));
                        vm.Report.OnPropertyChanged(nameof(vm.Report.OkpoRep));
                    }

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

    #region CheckPeriod

    /// <summary>
    /// Проверяет наличие отчёта с пересекающимся периодом.
    /// </summary>
    /// <param name="vm">Модель открытого отчёта.</param>
    /// <returns>Сообщение о наличии пересечения.</returns>
    private static async Task CheckPeriod(Form_17VM vm)
    {
        var desktop = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;
        if (vm.Report.FormNum_DB is "1.0" or "2.0") return;
        var reps = vm.Reports;
        var reportCollection = reps.Report_Collection;
        var rep = vm.Report;
        if (DateOnly.TryParse(rep.StartPeriod_DB, out var startPeriod)
            && DateOnly.TryParse(rep.EndPeriod_DB, out var endPeriod))
        {
            foreach (var currentReport in reportCollection.Where(x => x.FormNum_DB == rep.FormNum_DB && x.Id != rep.Id))
            {
                if (DateOnly.TryParse(currentReport.StartPeriod_DB, out var currentRepStartPeriod)
                    && DateOnly.TryParse(currentReport.EndPeriod_DB, out var currentRepEndPeriod)
                    && startPeriod < currentRepEndPeriod && endPeriod > currentRepStartPeriod)
                {
                    #region MessageFindIntersection

                    await Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams()
                        {
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentTitle = "Пересечение",
                            ContentHeader = "Уведомление",
                            ContentMessage = $"У организации {reps.Master_DB.RegNoRep.Value}_{reps.Master_DB.OkpoRep.Value} " +
                                             $"{Environment.NewLine}присутствует отчёт по форме " +
                                             $"{currentReport.FormNum_DB} {currentReport.StartPeriod_DB}-{currentReport.EndPeriod_DB}" +
                                             $"{Environment.NewLine}пересекающийся с введённым периодом " +
                                             $"{rep.StartPeriod_DB}-{rep.EndPeriod_DB}.",
                            MinWidth = 450,
                            MinHeight = 170,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(desktop.MainWindow));

                    #endregion

                    return;
                }
            }
        }
    }

    #endregion

    #region RemoveEmptyForms

    /// <summary>
    /// Проверяет на пустые строчки и предлагает их удалить.
    /// </summary>
    /// <param name="vm">Модель открытого отчёта.</param>
    /// <returns>Сообщение с предложением удалить пустые строчки.</returns>
    private static async Task RemoveEmptyForms(Form_17VM vm)
    {
        var desktop = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;
        List<Form> formToDeleteList = [];
        var lst = vm.Report[vm.FormType].ToList<Form17>();
        foreach (var form in lst)
        {
            if (string.IsNullOrWhiteSpace(form.OperationCode_DB)
                && string.IsNullOrWhiteSpace(form.OperationDate_DB)
                && string.IsNullOrWhiteSpace(form.PackName_DB)
                && string.IsNullOrWhiteSpace(form.PackType_DB)
                && string.IsNullOrWhiteSpace(form.PackFactoryNumber_DB)
                && string.IsNullOrWhiteSpace(form.PackNumber_DB)
                && string.IsNullOrWhiteSpace(form.FormingDate_DB)
                && string.IsNullOrWhiteSpace(form.PassportNumber_DB)
                && string.IsNullOrWhiteSpace(form.Volume_DB)
                && string.IsNullOrWhiteSpace(form.Mass_DB)
                && string.IsNullOrWhiteSpace(form.Radionuclids_DB)
                && string.IsNullOrWhiteSpace(form.SpecificActivity_DB)
                && form.DocumentVid_DB is null
                && string.IsNullOrWhiteSpace(form.DocumentNumber_DB)
                && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                && string.IsNullOrWhiteSpace(form.ProviderOrRecieverOKPO_DB)
                && string.IsNullOrWhiteSpace(form.TransporterOKPO_DB)
                && string.IsNullOrWhiteSpace(form.StoragePlaceName_DB)
                && string.IsNullOrWhiteSpace(form.StoragePlaceCode_DB)
                && string.IsNullOrWhiteSpace(form.CodeRAO_DB)
                && string.IsNullOrWhiteSpace(form.StatusRAO_DB)
                && string.IsNullOrWhiteSpace(form.VolumeOutOfPack_DB)
                && string.IsNullOrWhiteSpace(form.MassOutOfPack_DB)
                && string.IsNullOrWhiteSpace(form.Quantity_DB)
                && string.IsNullOrWhiteSpace(form.TritiumActivity_DB)
                && string.IsNullOrWhiteSpace(form.BetaGammaActivity_DB)
                && string.IsNullOrWhiteSpace(form.AlphaActivity_DB)
                && string.IsNullOrWhiteSpace(form.TransuraniumActivity_DB)
                && string.IsNullOrWhiteSpace(form.RefineOrSortRAOCode_DB)
                && string.IsNullOrWhiteSpace(form.Subsidy_DB)
                && string.IsNullOrWhiteSpace(form.FcpNumber_DB)
                && string.IsNullOrWhiteSpace(form.ContractNumber_DB))
            {
                formToDeleteList.Add(form);
            }
        }

        if (formToDeleteList.Count != 0)
        {
            #region MessageRemoveEmptyForms

            var res = await Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Да" },
                        new ButtonDefinition { Name = "Нет" }
                    ],
                    ContentTitle = "Сохранение изменений",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"В форме {vm.FormType} присутствуют пустые строчки." +
                                     $"{Environment.NewLine}Вы хотите их удалить?",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow));

            #endregion

            if (res is "Да")
            {
                await using var db = new DBModel(StaticConfiguration.DBPath);
                foreach (var form in formToDeleteList)
                {
                    vm.Report.Rows.Remove(form);
                }

                var minItem = formToDeleteList.Min(x => x.Order);
                await vm.Report.SortAsync();
                var itemQ = vm.Report.Rows
                    .GetEnumerable()
                    .Where(x => x.Order > minItem)
                    .Select(x => (Form)x)
                    .ToArray();
                foreach (var form in itemQ)
                {
                    form.NumberInOrder_DB = (int)minItem;
                    form.NumberInOrder.OnPropertyChanged();
                    minItem++;
                }
                await db.SaveChangesAsync();
            }
        }
    }

    #endregion

    #endregion
}