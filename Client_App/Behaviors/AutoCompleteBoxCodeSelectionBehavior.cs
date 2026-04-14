using System.Linq;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Behaviors;

public class AutoCompleteBoxCodeSelectionBehavior : Behavior<AutoCompleteBox>
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
        if (AssociatedObject?.SelectedItem == null) return;
        
        // Try to get Code property using reflection
        var codeProperty = AssociatedObject.SelectedItem.GetType().GetProperty("Code");
        if (codeProperty != null)
        {
            var codeValue = codeProperty.GetValue(AssociatedObject.SelectedItem);
            AssociatedObject.Text = codeValue?.ToString() ?? string.Empty;
        }
            
        // Mark value as selected from dropdown
        var behaviors = Interaction.GetBehaviors(AssociatedObject);
        var validationBehavior = behaviors.OfType<AutoCompleteBoxValidationBehavior>().FirstOrDefault();
        validationBehavior?.MarkValueSelectedFromDropDown();
    }
}
