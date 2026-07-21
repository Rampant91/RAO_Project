using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.MainWindowTabs;

namespace Client_App.Behaviors.DataGridBehaviors;

/// <summary>
/// Сбрасывает выделение DataGrid при клике внутри грида, но мимо строки.
/// </summary>
public class DataGridClearSelectionOnEmptyAreaClickBehavior : Behavior<DataGrid>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null) return;

        AttachEventHandlers();
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        DetachEventHandlers();
    }

    private void AttachEventHandlers()
    {
        if (AssociatedObject is not null)
        {
            AssociatedObject.PointerPressed -= OnDataGridPointerPressed;
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

        var source = e.Source as Visual;
        var isClickOnRow = IsClickOnDataGridRow(source);

        if (!isClickOnRow)
            ClearSelection();
    }

    private void ClearSelection()
    {
        if (AssociatedObject is null) return;

        if (AssociatedObject.SelectionMode == DataGridSelectionMode.Extended
            && AssociatedObject.SelectedItems is { IsReadOnly: false, Count: > 0 } list)
        {
            list.Clear();
        }
        else
        {
            AssociatedObject.SelectedItem = null;
        }

        ClearSelectionInViewModel();
    }

    private static bool IsClickOnDataGridRow(Visual? source)
    {
        while (source != null)
        {
            if (source is DataGridRow)
                return true;

            source = source.GetVisualParent() as Visual;
        }

        return false;
    }

    private void ClearSelectionInViewModel()
    {
        if (AssociatedObject?.DataContext is FormsTabControlBaseVM vm)
        {
            if (AssociatedObject.Name == "ReportsDataGrid")
                vm.SelectedReports = null;
            else if (AssociatedObject.Name == "ReportDataGrid")
                vm.SelectedReport = null;
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
