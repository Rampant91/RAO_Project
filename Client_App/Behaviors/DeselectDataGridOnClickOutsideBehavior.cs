using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Behaviors;
public class DeselectDataGridOnClickOutsideBehavior : Behavior<DataGrid>
{
    private Window _parentWindow;
    private bool _isAttached;

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null) return;

        // Ждем полной загрузки контрола для получения родительского окна
        AssociatedObject.AttachedToVisualTree += OnAttachedToVisualTree;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (AssociatedObject is not null)
        {
            AssociatedObject.AttachedToVisualTree -= OnAttachedToVisualTree;
        }

        DetachEventHandlers();
    }

    private void OnAttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
    {
        // Получаем родительское окно
        _parentWindow = AssociatedObject?.GetVisualRoot() as Window;

        if (_parentWindow is not null && !_isAttached)
        {
            AttachEventHandlers();
            _isAttached = true;
        }
    }

    private void AttachEventHandlers()
    {
        if (_parentWindow is not null)
        {
            _parentWindow.PointerPressed += OnWindowPointerPressed;
        }
    }

    private void DetachEventHandlers()
    {
        if (_parentWindow is not null)
        {
            _parentWindow.PointerPressed -= OnWindowPointerPressed;
            _isAttached = false;
        }
    }

    private void OnWindowPointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (AssociatedObject is null) return;

        // Проверяем, был ли клик внутри DataGrid
        var source = e.Source as Control;
        var isClickInsideDataGrid = IsSourceInsideDataGrid(source);

        // Если клик был вне DataGrid, сбрасываем выделение
        if (!isClickInsideDataGrid)
        {
            AssociatedObject.SelectedItem = null;
        }
    }

    private bool IsSourceInsideDataGrid(Control source)
    {
        // Проверяем, находится ли источник клика внутри DataGrid
        while (source != null)
        {
            if (source == AssociatedObject)
            {
                return true;
            }
            source = (Control)source.Parent;
        }
        return false;
    }
}