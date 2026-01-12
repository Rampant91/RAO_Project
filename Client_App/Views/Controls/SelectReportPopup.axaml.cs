using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.SwitchReport;
using Client_App.ViewModels.Controls;
using Client_App.ViewModels.Forms;
using Models.Classes;
using Models.Collections;
using Models.Interfaces;
using System.Collections.Generic;

namespace Client_App.Views.Controls;

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

    private void ListBox_PointerReleased(object sender, PointerReleasedEventArgs e)
    {
        // Проверяем, что клик был левой кнопкой мыши
        if (e.InitialPressMouseButton != MouseButton.Left)
            return;

        // Находим элемент ListBoxItem, на который был произведен клик
        var listBoxItem = FindVisualParent<ListBoxItem>(e.Source as Control);

        if (listBoxItem == null || listBoxItem.DataContext is not Report selectedReport)
            return;

        if (selectedReport != vm.Report)
            new SwitchToSelectedReportAsyncCommand(vm.FormVM).AsyncExecute(selectedReport);

    }
    // Вспомогательные методы для поиска в визуальном дереве
    public static T FindVisualParent<T>(Visual visual) where T : Visual
    {
        while (visual != null && !(visual is T))
        {
            visual = (Visual)visual.GetVisualParent();
        }
        return visual as T;
    }
}