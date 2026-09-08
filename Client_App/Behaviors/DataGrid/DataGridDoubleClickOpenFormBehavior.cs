using Avalonia;
using Avalonia.Controls;
using AvaloniaDataGrid = Avalonia.Controls.DataGrid;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using Client_App.Commands.AsyncCommands;
using Client_App.ViewModels;
using Client_App.Views;
using Models.Collections;

namespace Client_App.Behaviors.DataGrid;

public class DataGridDoubleClickOpenFormBehavior : Behavior<AvaloniaDataGrid>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject != null)
        {
            // Подписываемся на события
            AssociatedObject.DoubleTapped += DataGrid_DoubleTapped; ;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.DoubleTapped -= DataGrid_DoubleTapped;
        }

        base.OnDetaching();
    }

    private void DataGrid_DoubleTapped(object? sender, RoutedEventArgs e)
    {
        if (AssociatedObject?.SelectedItem != null)
        {
            var desktop = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;
            var mainWindow = (desktop.MainWindow as MainWindow)!;
            var mainWindowVM = (mainWindow.DataContext as MainWindowVM)!;

            if (AssociatedObject?.SelectedItem is Reports reports)
            {
                BaseAsyncCommand command;
                if (reports.Master_DB.FormNum_DB.Split('.')[0] is "1")
                    command = new NewChangeReportsAsyncCommand(mainWindowVM.Forms1TabControlVM);
                else if (reports.Master_DB.FormNum_DB.Split('.')[0] is "2")
                    command = new NewChangeReportsAsyncCommand(mainWindowVM.Forms2TabControlVM);
                else if (reports.Master_DB.FormNum_DB.Split('.')[0] is "4")
                    command = new NewChangeReportsAsyncCommand(mainWindowVM.Forms4TabControlVM);
                else if (reports.Master_DB.FormNum_DB.Split('.')[0] is "5")
                    command = new NewChangeReportsAsyncCommand(mainWindowVM.Forms5TabControlVM);
                else return;

                command.AsyncExecute(reports);
            }
            else if (AssociatedObject?.SelectedItem is Report report)
            {
                BaseAsyncCommand command;
                if (report.FormNum_DB.Split('.')[0] is "1")
                    command = new NewChangeReportAsyncCommand(mainWindowVM.Forms1TabControlVM);
                else if (report.FormNum_DB.Split('.')[0] is "2")
                    command = new ChangeFormAsyncCommand();
                else if (report.FormNum_DB.Split('.')[0] is "4")
                    command = new NewChangeReportAsyncCommand(mainWindowVM.Forms4TabControlVM);
                else if (report.FormNum_DB.Split('.')[0] is "5")
                    command = new NewChangeReportAsyncCommand(mainWindowVM.Forms5TabControlVM);
                else return;

                command.Execute(report);
            }
        }

    }
}