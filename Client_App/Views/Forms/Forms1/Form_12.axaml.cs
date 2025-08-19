using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
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

public partial class Form_12 : BaseWindow<Form_12VM>
{

    //private Form_12VM _vm = null!;

    public Form_12() 
    {
        InitializeComponent();
        Show();
    }

    public Form_12(Form_12VM vm)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = vm;
        Closing += OnStandardClosing;
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new Form_12VM();
    }

    //Временное узкоспециализированное решение
    private void CopyExecutorData_Click(object sender, RoutedEventArgs e)
    {
        var command = new NewCopyExecutorDataAsyncCommand((Form_12VM)DataContext);
        if (command.CanExecute(null))
        {
            command.Execute(null);
        }
    }

    #region PaginationTextBoxValidation
    // Валидация для контроля паджинации

    private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
    private static bool IsTextAllowed(string text)
    {
        return !_regex.IsMatch(text);
    }
    private void TextBox_TextInput(object sender, TextInputEventArgs e)
    {
        e.Handled = !IsTextAllowed(e.Text);
    }
    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Разрешаем служебные клавиши (Backspace, Delete, стрелки и т.д.)
        if (IsControlKey(e))
        {
            return;
        }

        // Разрешаем цифры с основной клавиатуры и цифрового блока
        if (IsDigitKey(e))
        {
            return;
        }

        // Блокируем все остальные клавиши
        e.Handled = true;
    }

    private bool IsControlKey(KeyEventArgs e)
    {
        return e.Key == Key.Back ||
               e.Key == Key.Delete ||
               e.Key == Key.Left ||
               e.Key == Key.Right ||
               e.Key == Key.Home ||
               e.Key == Key.End ||
               e.Key == Key.Tab ||
               e.Key == Key.Enter ||
               e.Key == Key.Escape ||
               e.Key == Key.CapsLock ||
               e.Key == Key.PageUp ||
               e.Key == Key.PageDown ||
               e.KeyModifiers.HasFlag(KeyModifiers.Control); // Разрешаем Ctrl+C, Ctrl+V и т.д.
    }

    private bool IsDigitKey(KeyEventArgs e)
    {
        // Цифры на основной клавиатуре (0-9)
        if (e.Key >= Key.D0 && e.Key <= Key.D9)
            return true;

        // Цифры на цифровом блоке (NumPad0-NumPad9)
        if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            return true;

        return false;
    }
    #endregion

    #region OnStandartClosing

    private async void OnStandardClosing(object? sender, CancelEventArgs args)
    {
        if (DataContext is not ChangeOrCreateVM vm) return;
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

                var lst = vm.Storage[vm.FormType];

                foreach (var key in lst)
                {
                    var item = (Form)key;
                    if (item.Id == 0)
                    {
                        vm.Storage[vm.Storage.FormNum_DB].Remove(item);
                    }
                }

                var lstNote = vm.Storage.Notes.ToList<Note>();
                foreach (var item in lstNote.Where(item => item.Id == 0))
                {
                    vm.Storage.Notes.Remove(item);
                }

                if (vm.FormType is not "1.0" and not "2.0")
                {
                    if (vm.FormType.Split('.')[0] == "1")
                    {
                        vm.Storage.OnPropertyChanged(nameof(vm.Storage.StartPeriod));
                        vm.Storage.OnPropertyChanged(nameof(vm.Storage.EndPeriod));
                        vm.Storage.OnPropertyChanged(nameof(vm.Storage.CorrectionNumber));
                    }
                    else if (vm.FormType.Split('.')[0] == "2")
                    {
                        vm.Storage.OnPropertyChanged(nameof(vm.Storage.Year));
                        vm.Storage.OnPropertyChanged(nameof(vm.Storage.CorrectionNumber));
                    }
                }
                else
                {
                    vm.Storage.OnPropertyChanged(nameof(vm.Storage.RegNoRep));
                    vm.Storage.OnPropertyChanged(nameof(vm.Storage.ShortJurLicoRep));
                    vm.Storage.OnPropertyChanged(nameof(vm.Storage.OkpoRep));
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
    private static async Task CheckPeriod(ChangeOrCreateVM vm)
    {
        var desktop = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;
        if (vm.Storage.FormNum_DB is "1.0" or "2.0") return;
        var reps = vm.Storages;
        var reportCollection = reps.Report_Collection;
        var rep = vm.Storage;
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
    private static async Task RemoveEmptyForms(ChangeOrCreateVM vm)
    {
        var desktop = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;
        List<Form> formToDeleteList = [];
        var lst = vm.Storage[vm.FormType];
        foreach (var key in lst)
        {
            var form = (Form12)key;
            if (string.IsNullOrWhiteSpace(form.OperationCode_DB)
                && string.IsNullOrWhiteSpace(form.OperationDate_DB)
                && string.IsNullOrWhiteSpace(form.PassportNumber_DB)
                && string.IsNullOrWhiteSpace(form.NameIOU_DB)
                && string.IsNullOrWhiteSpace(form.FactoryNumber_DB)
                && string.IsNullOrWhiteSpace(form.Mass_DB)
                && string.IsNullOrWhiteSpace(form.CreatorOKPO_DB)
                && string.IsNullOrWhiteSpace(form.CreationDate_DB)
                && string.IsNullOrWhiteSpace(form.SignedServicePeriod_DB)
                && form.PropertyCode_DB is null
                && string.IsNullOrWhiteSpace(form.Owner_DB)
                && form.DocumentVid_DB is null
                && string.IsNullOrWhiteSpace(form.DocumentNumber_DB)
                && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                && string.IsNullOrWhiteSpace(form.ProviderOrRecieverOKPO_DB)
                && string.IsNullOrWhiteSpace(form.TransporterOKPO_DB)
                && string.IsNullOrWhiteSpace(form.PackName_DB)
                && string.IsNullOrWhiteSpace(form.PackType_DB)
                && string.IsNullOrWhiteSpace(form.PackNumber_DB))
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
                    vm.Storage.Rows.Remove(form);
                }
                var minItem = formToDeleteList.Min(x => x.Order);
                await vm.Storage.SortAsync();
                var itemQ = vm.Storage.Rows
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