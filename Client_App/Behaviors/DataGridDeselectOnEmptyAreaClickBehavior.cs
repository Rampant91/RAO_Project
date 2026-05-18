using System;
using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.MainWindowTabs;

namespace Client_App.Behaviors;

/// <summary>
/// Behavior для DataGrid, который сбрасывает выбор при клике мимо строки (но внутри DataGrid)
/// </summary>
public class DataGridDeselectOnEmptyAreaClickBehavior : Behavior<DataGrid>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null) return;

        // Сразу подключаем обработчики
        AttachEventHandlers();
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        // Отключаем обработчики
        DetachEventHandlers();
    }

    private void AttachEventHandlers()
    {
        if (AssociatedObject is not null)
        {
            // Сначала отключаем, чтобы избежать дублирования
            AssociatedObject.PointerPressed -= OnDataGridPointerPressed;
            // Затем подключаем
            AssociatedObject.PointerPressed += OnDataGridPointerPressed;
        }
    }

    private void DetachEventHandlers()
    {
        if (AssociatedObject is not null)
        {
            AssociatedObject.PointerPressed -= OnDataGridPointerPressed;
        }
    }

    private void OnDataGridPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (AssociatedObject is null) return;

        // Проверяем, был ли клик по строке DataGrid
        var source = e.Source as Visual;
        var isClickOnRow = IsClickOnDataGridRow(source);

        // Если клик был мимо строки, сбрасываем выделение
        if (!isClickOnRow)
        {
            if (AssociatedObject.SelectedItems is { IsFixedSize: false } list)
            {
                try
                {
                    list.Clear();
                }
                catch
                {
                    // ignored
                }
            }

            AssociatedObject.SelectedItem = null;

            DeselectInViewModel();
        }
    }

    private bool IsClickOnDataGridRow(Visual? source)
    {
        // Проверяем, находится ли источник клика внутри DataGridRow
        while (source != null)
        {
            if (source is DataGridRow)
            {
                return true;
            }
            source = source.GetVisualParent() as Visual;
        }
        return false;
    }

    private void DeselectInViewModel()
    {
        if (AssociatedObject?.DataContext is FormsTabControlBaseVM vm)
        {
            // Определяем, какой DataGrid используется по имени
            if (AssociatedObject.Name == "ReportsDataGrid")
            {
                // Верхний DataGrid - сбрасываем SelectedReports
                vm.SelectedReports = null;
            }
            else if (AssociatedObject.Name == "ReportDataGrid")
            {
                // Нижний DataGrid - сбрасываем SelectedReport
                vm.SelectedReport = null;
            }
        }
        else if (AssociatedObject?.DataContext is BaseFormVM formVm)
        {
            if (AssociatedObject.Name == "dataGrid")
            {
                formVm.SelectedForm = null;
                formVm.SelectedForms.Clear();
            }
            else if (AssociatedObject.Name == "notesDataGrid")
            {
                formVm.SelectedNote = null;
                formVm.SelectedNotes.Clear();
            }
        }
    }
}
