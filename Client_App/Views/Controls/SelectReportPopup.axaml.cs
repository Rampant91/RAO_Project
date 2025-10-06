using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.SwitchReport;
using Client_App.ViewModels.Controls;
using Client_App.ViewModels.Forms;
using Models.Classes;
using Models.Collections;
using Models.Interfaces;
using System.Collections.Generic;

namespace Client_App.Controls;

public partial class SelectReportPopup : UserControl
{
    SelectReportPopupVM vm => DataContext as SelectReportPopupVM;
    public SelectReportPopup()
    {
        InitializeComponent();

    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        
    }
    public void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        Report newSelectedReport = (sender as ListBox).SelectedItem as Report;

        // ѕри инициализации View Model формы происходит ложное срабатывание событи€
        // ѕоэтому провер€ем был ли выбран элемент до срабатывани€,
        // ≈сли нет то значит событие было вызвано при инициализации DataContext`а формы

        if (args.RemovedItems.Count>0) // проверка прошлого выбранного итема.
            new SwitchToSelectedReportAsyncCommand(vm.FormVM).AsyncExecute(newSelectedReport);

    }
}