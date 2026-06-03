using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Messages;
using MessageBox.Avalonia.DTO;

namespace Client_App.Views.Messages;

public class GroupBulkExportReportsMessageWindow : BaseWindow<GroupBulkExportReportsMessageVM>
{
    private GroupBulkExportReportsMessageVM Vm => (GroupBulkExportReportsMessageVM)DataContext!;

    public GroupBulkExportReportsMessageWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        DataContext = new GroupBulkExportReportsMessageVM();
        AvaloniaXamlLoader.Load(this);
    }

    private async void OnBrowseExcelClicked(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            AllowMultiple = false,
            Filters =
            [
                new FileDialogFilter { Name = "Excel", Extensions = ["xlsx", "XLSX"] }
            ]
        };
        var paths = await dialog.ShowAsync(this);
        if (paths is { Length: > 0 })
        {
            Vm.ExcelFilePath = paths[0];
        }
    }

    private async void OnBrowseFolderClicked(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog();
        var path = await dialog.ShowAsync(this);
        if (!string.IsNullOrEmpty(path))
        {
            Vm.OutputFolderPath = path;
        }
    }

    private void OnOkButtonClicked(object? sender, RoutedEventArgs e)
    {
        var forms = Vm.GetSelectedFormNumbers();
        if (string.IsNullOrWhiteSpace(Vm.ExcelFilePath)
            || string.IsNullOrWhiteSpace(Vm.OutputFolderPath)
            || forms.Count == 0)
        {
            _ = ShowValidationMessageAsync(
                "Укажите файл .xlsx, папку для выгрузки и выберите хотя бы одну форму.");
            return;
        }

        var hasStart = DateOnly.TryParse(Vm.InitialDate, out var periodStart);
        var hasEnd = DateOnly.TryParse(Vm.ResidualDate, out var periodEnd);
        if (!hasStart && !hasEnd)
        {
            periodStart = DateOnly.MinValue;
            periodEnd = DateOnly.MaxValue;
        }
        else
        {
            if (!hasStart) periodStart = DateOnly.MinValue;
            if (!hasEnd) periodEnd = DateOnly.MaxValue;
            if (periodStart > periodEnd)
            {
                _ = ShowValidationMessageAsync("Период указан некорректно: дата начала позже даты окончания.");
                return;
            }
        }

        Vm.Ok = true;
        Close(new GroupBulkExportReportsDialogResult
        {
            ExcelFilePath = Vm.ExcelFilePath,
            OutputFolderPath = Vm.OutputFolderPath,
            FormNumbers = forms,
            PeriodStart = periodStart,
            PeriodEnd = periodEnd
        });
    }

    private void OnCancelButtonClicked(object? sender, RoutedEventArgs e)
    {
        Close(null);
    }

    private async System.Threading.Tasks.Task ShowValidationMessageAsync(string message)
    {
        await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = "Групповая выгрузка",
                ContentHeader = "Уведомление",
                ContentMessage = message,
                MinWidth = 400,
                MinHeight = 120,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(this);
    }
}
