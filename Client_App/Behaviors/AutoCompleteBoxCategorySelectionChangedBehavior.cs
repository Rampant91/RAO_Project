using System.Linq;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using Client_App.ViewModels.Forms.Forms1.Items;

namespace Client_App.Behaviors;

public class AutoCompleteBoxCategorySelectionChangedBehavior : Behavior<AutoCompleteBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject != null)
        {
            AssociatedObject.DropDownClosed += OnDropDownClosed;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.DropDownClosed -= OnDropDownClosed;
        }

        base.OnDetaching();
    }

    private void OnDropDownClosed(object? sender, System.EventArgs e)
    {
        if (AssociatedObject is not { SelectedItem: CategoryItem selectedItem }) return;
        AssociatedObject.Text = selectedItem.Code?.ToString() ?? string.Empty;
            
        // Помечаем, что значение было выбрано из списка
        var behaviors = Interaction.GetBehaviors(AssociatedObject);
        var validationBehavior = behaviors.OfType<AutoCompleteBoxCategoryValidationBehavior>().FirstOrDefault();
        validationBehavior?.MarkValueSelectedFromDropDown();
    }
}
