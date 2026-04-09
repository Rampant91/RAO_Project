using System.Linq;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using Client_App.ViewModels.Forms.Forms1;

namespace Client_App.Behaviors;

public class AutoCompleteBoxSelectionChangedBehavior : Behavior<AutoCompleteBox>
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
        if (AssociatedObject is not { SelectedItem: OperationCodeItem selectedItem }) return;
        AssociatedObject.Text = selectedItem.Code;
            
        // Помечаем, что значение было выбрано из списка
        var behaviors = Interaction.GetBehaviors(AssociatedObject);
        var validationBehavior = behaviors.OfType<AutoCompleteBoxValidationBehavior>().FirstOrDefault();
        validationBehavior?.MarkValueSelectedFromDropDown();
    }
}
