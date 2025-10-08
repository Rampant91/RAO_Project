using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Commands.SyncCommands;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Forms.Forms1;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Views.Forms.Forms1;

public partial class Form_16 : BaseWindow<Form_16VM>
{
    //private Form_16VM _vm = null!;

    private bool _isCloseConfirmed;

    #region Constructors

    public Form_16()
    {
        InitializeComponent();
        DataContext = new Form_16VM();
        Show();
    }

    public Form_16(Form_16VM vm)
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

    #endregion

    #region CopyExecutorData_Click
    
    //Временное узкоспециализированное решение
    private void CopyExecutorData_Click(object sender, RoutedEventArgs e)
    {
        var command = new NewCopyExecutorDataAsyncCommand((Form_16VM)DataContext);
        if (command.CanExecute(null))
        {
            command.Execute(null);
        }
    }

    #endregion

    #region DataGrid_KeyUp

    private void DataGrid_KeyUp(object? sender, KeyEventArgs e)
    {
        if (DataContext is not Form_16VM vm)
            return;

        var dataGrid = this.FindControl<DataGrid>("dataGrid");
        var dataContext = dataGrid?.DataContext;
        if (dataContext is null || dataGrid is null)
            return;

        var selectedForms = vm.SelectedForms;

        if (!dataGrid.IsPointerOver || e.KeyModifiers is not KeyModifiers.Control) return;

        switch (e.Key)
        {
            case Key.A: // Select All
            {
                vm.SelectAll.Execute(null);
                e.Handled = true;

                break;
            }
            case Key.T: // Add Row
            {
                vm.AddRow.Execute(null);
                e.Handled = true;

                break;
            }
            case Key.N: // Add N Rows
            {
                vm.AddRows.Execute(null);
                e.Handled = true;

                break;
            }
            case Key.I: // Add N Rows Before
            {
                if (selectedForms is { Count: > 0 })
                {
                    vm.AddRowsIn.Execute(selectedForms);
                    e.Handled = true;
                }

                break;
            }
            case Key.D: // Delete Selected Rows
            {
                if (selectedForms is { Count: > 0 })
                {
                    vm.DeleteRows.Execute(selectedForms);
                    e.Handled = true;
                }

                break;
            }
            case Key.O: // Set Number Order
            {
                vm.SetNumberOrder.Execute(null);
                e.Handled = true;

                break;
            }
            case Key.K: // Clear Rows
            {
                if (selectedForms is { Count: > 0 })
                {
                    vm.DeleteDataInRows.Execute(selectedForms);
                    e.Handled = true;
                }

                break;
            }
        }

        if (vm.DataGridIsEditing) return;
        switch (e.Key)
        {
            case Key.C: // Copy Rows
            {
                if (selectedForms is { Count: > 0 })
                {
                    vm.CopyRows.Execute(selectedForms);
                    e.Handled = true;
                }

                break;
            }
            case Key.V: // Paste Rows
            {
                if (selectedForms is { Count: > 0 })
                {
                    vm.PasteRows.Execute(selectedForms);
                    e.Handled = true;
                }

                break;
            }
        }
    }

    #endregion

    #region OnStandartClosing

    private async void OnStandardClosing(object? sender, CancelEventArgs args)
    {
        args.Cancel = true; // Сразу запрещаем закрытие окна, т.к. из-за асинхроности окно может закрыться в любой момент


        _isCloseConfirmed = true; // перед выходом из обработчика события стоит проверка на _isCloseConfirmed,
                                  // если true, то окно закроется,
                                  // если false, то не закроется
        if (DataContext is not Form_16VM vm) return;

        try
        {
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
            var db = StaticConfiguration.DBModel;

            var modifiedEntities = db.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged);

            if (modifiedEntities.All(x => x.Entity is Report rep && rep.FormNum_DB != vm.FormType)
                || !db.ChangeTracker.HasChanges() || vm.SkipChangeTacking)
            {
                if (vm.SkipChangeTacking) vm.SkipChangeTacking = false;
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
                ContentMessage = $"Сохранить форму {vm.FormType}?",
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
                    await dbm.SaveChangesAsync();
                    await new SaveReportAsyncCommand(vm).AsyncExecute(null);
                    if (desktop.Windows.Count == 1)
                    {
                        desktop.MainWindow.WindowState = OwnerPrevState;
                    }
                    break;
                }
            case "Нет":
                {
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
                    ;
                    break;
                }
            case "Отмена":
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

    #region CheckPeriod

    /// <summary>
    /// Проверяет наличие отчёта с пересекающимся периодом.
    /// </summary>
    /// <param name="vm">Модель открытого отчёта.</param>
    /// <returns>Сообщение о наличии пересечения.</returns>
    private async Task CheckPeriod(Form_16VM vm)
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
                        .ShowDialog(this));

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
    private async Task RemoveEmptyForms(Form_16VM vm)
    {
        var desktop = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;
        List<Form> formToDeleteList = [];
        var lst = vm.Report[vm.FormType].ToList<Form16>();
        foreach (var form in lst)
        {
            if (string.IsNullOrWhiteSpace(form.OperationCode_DB)
                && string.IsNullOrWhiteSpace(form.OperationDate_DB)
                && string.IsNullOrWhiteSpace(form.CodeRAO_DB)
                && string.IsNullOrWhiteSpace(form.StatusRAO_DB)
                && string.IsNullOrWhiteSpace(form.Volume_DB)
                && string.IsNullOrWhiteSpace(form.Mass_DB)
                && string.IsNullOrWhiteSpace(form.QuantityOZIII_DB)
                && string.IsNullOrWhiteSpace(form.MainRadionuclids_DB)
                && string.IsNullOrWhiteSpace(form.TritiumActivity_DB)
                && string.IsNullOrWhiteSpace(form.BetaGammaActivity_DB)
                && string.IsNullOrWhiteSpace(form.AlphaActivity_DB)
                && string.IsNullOrWhiteSpace(form.TransuraniumActivity_DB)
                && string.IsNullOrWhiteSpace(form.ActivityMeasurementDate_DB)
                && form.DocumentVid_DB is null
                && string.IsNullOrWhiteSpace(form.DocumentNumber_DB)
                && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                && string.IsNullOrWhiteSpace(form.ProviderOrRecieverOKPO_DB)
                && string.IsNullOrWhiteSpace(form.TransporterOKPO_DB)
                && string.IsNullOrWhiteSpace(form.StoragePlaceName_DB)
                && string.IsNullOrWhiteSpace(form.StoragePlaceCode_DB)
                && string.IsNullOrWhiteSpace(form.RefineOrSortRAOCode_DB)
                && string.IsNullOrWhiteSpace(form.PackName_DB)
                && string.IsNullOrWhiteSpace(form.PackType_DB)
                && string.IsNullOrWhiteSpace(form.PackNumber_DB)
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
                .ShowDialog(this));

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

    #region OperationCode

    private string OperationCodeStartValue { get; set; }

    private void OperationCodeTextBox_OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            OperationCodeStartValue = textBox.Text;
        }
    }

    private void OperationCodeTextBox_OnLostFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is TextBox { Text: "41" } textBox && OperationCodeStartValue is not "41")
        {
            textBox.Text = string.Empty;

            Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams()
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Перевод РВ в РАО",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Операцию перевода РВ в РАО необходимо осуществлять посредством кнопки \"Перевести РВ в РАО\"" +
                                     $"{Environment.NewLine}(правый щелчкок мыши по нужной строчке в форме 1.2/1.3/1.4).",
                    MinWidth = 450,
                    MinHeight = 170,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(this));
        }
    }

    #endregion
}