using System.Linq;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using Client_App.ViewModels.Forms.Forms1.Items;

namespace Client_App.Behaviors;

public class AutoCompleteBoxSelectionChangedBehavior : Behavior<AutoCompleteBox>
{
    private bool _selectionChangedDuringDropDown;

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject != null)
        {
            AssociatedObject.SelectionChanged += OnSelectionChanged;
            AssociatedObject.DropDownClosed += OnDropDownClosed;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
            AssociatedObject.DropDownClosed -= OnDropDownClosed;
        }

        base.OnDetaching();
    }

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems?.Count > 0)
            _selectionChangedDuringDropDown = true;
    }

    private void OnDropDownClosed(object? sender, System.EventArgs e)
    {
        if (!_selectionChangedDuringDropDown)
        {
            _selectionChangedDuringDropDown = false;
            return;
        }

        _selectionChangedDuringDropDown = false;

        if (AssociatedObject is not { SelectedItem: OperationCodeItem selectedItem }) return;
        AssociatedObject.Text = selectedItem.Code;

        // Помечаем, что значение было выбрано из списка
        var behaviors = Interaction.GetBehaviors(AssociatedObject);
        var validationBehavior = behaviors.OfType<AutoCompleteBoxValidationBehavior>().FirstOrDefault();
        validationBehavior?.MarkValueSelectedFromDropDown();
    }
}
