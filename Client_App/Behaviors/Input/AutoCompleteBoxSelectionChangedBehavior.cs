using System.Linq;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using Client_App.ViewModels.Forms.Forms1.Items;

namespace Client_App.Behaviors.Input;

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
        if (e.AddedItems.Count == 0) return;

        var selectedItem = e.AddedItems[0];
        string? codeValue = null;

        // Пробуем получить Code свойство через reflection (работает для CategoryItem и OperationCodeItem)
        var codeProperty = selectedItem.GetType().GetProperty("Code");
        if (codeProperty != null)
        {
            var value = codeProperty.GetValue(selectedItem);
            codeValue = value?.ToString();
        }
        // Fallback для CategoryItem напрямую
        else if (selectedItem is CategoryItem categoryItem)
        {
            codeValue = categoryItem.Code?.ToString();
        }

        if (string.IsNullOrEmpty(codeValue)) return;

        AssociatedObject.Text = codeValue;

        // Помечаем, что значение было выбрано из списка
        var behaviors = Interaction.GetBehaviors(AssociatedObject);
        var validationBehavior = behaviors.OfType<AutoCompleteBoxValidationBehavior>().FirstOrDefault();
        validationBehavior?.MarkValueSelectedFromDropDown();
    }
}
