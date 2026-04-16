using System.Linq;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using Client_App.ViewModels.Forms.Forms1.Items;

namespace Client_App.Behaviors;

public class AutoCompleteBoxSelectionChangedBehavior : Behavior<AutoCompleteBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject != null)
        {
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        base.OnDetaching();
    }

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // Устанавливаем текст и помечаем выбор СРАЗУ при выборе из списка
        // Используем e.AddedItems, так как SelectedItem может быть ещё не установлен
        if (e.AddedItems.Count == 0 || e.AddedItems[0] is not CategoryItem selectedItem) return;

        AssociatedObject.Text = selectedItem.Code?.ToString() ?? string.Empty;
            
        // Помечаем, что значение было выбрано из списка
        var behaviors = Interaction.GetBehaviors(AssociatedObject);
        var validationBehavior = behaviors.OfType<AutoCompleteBoxValidationBehavior>().FirstOrDefault();
        validationBehavior?.MarkValueSelectedFromDropDown();
    }
}
