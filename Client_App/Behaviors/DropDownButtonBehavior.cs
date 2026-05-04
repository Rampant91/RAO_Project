using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Behaviors;

public class DropDownButtonBehavior : Behavior<Button>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject != null)
        {
            AssociatedObject.Click += OnButtonClick;
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        if (AssociatedObject != null)
        {
            AssociatedObject.Click -= OnButtonClick;
        }
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        // Try to find AutoCompleteBox in the parent hierarchy
        // When button is inside UserControl: Button -> UserControl -> Grid -> AutoCompleteBox
        var parent = AssociatedObject.Parent;
        if (parent?.Parent is Grid grid && grid.Children[0] is AutoCompleteBox autoCompleteBox)
        {
            autoCompleteBox.Focus();
            ShowDropdown(autoCompleteBox);
        }
        // Fallback: direct parent is Grid (original behavior for non-UserControl usage)
        else if (AssociatedObject.Parent is Grid directGrid && directGrid.Children[0] is AutoCompleteBox directAutoCompleteBox)
        {
            directAutoCompleteBox.Focus();
            ShowDropdown(directAutoCompleteBox);
        }
    }

    private static void ShowDropdown(AutoCompleteBox autoCompleteBox)
    {
        if (autoCompleteBox.IsDropDownOpen) return;

        // Сохраняем исходный текст и временно очищаем для показа всех значений
        var originalText = autoCompleteBox.Text;
        autoCompleteBox.Text = string.Empty;

        typeof(Avalonia.Controls.AutoCompleteBox).GetMethod(
                "PopulateDropDown",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(autoCompleteBox, [autoCompleteBox, EventArgs.Empty]);

        typeof(Avalonia.Controls.AutoCompleteBox).GetMethod(
                "OpeningDropDown",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(autoCompleteBox, [false]);

        if (autoCompleteBox.IsDropDownOpen) return;

        // We *must* set the field and not the property to avoid the changed event being raised
        // (which prevents the dropdown opening)
        var ipc = typeof(Avalonia.Controls.AutoCompleteBox).GetField(
            "_ignorePropertyChange",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if ((bool)ipc?.GetValue(autoCompleteBox)! == false) ipc?.SetValue(autoCompleteBox, true);

        autoCompleteBox.SetValue(Avalonia.Controls.AutoCompleteBox.IsDropDownOpenProperty, true);

        // Подписываемся на закрытие дропдауна для восстановления текста если ничего не выбрано
        EventHandler? closedHandler = null;
        closedHandler = (s, e) =>
        {
            autoCompleteBox.DropDownClosed -= closedHandler;
            // Если текст пустой (ничего не выбрано), восстанавливаем исходный
            if (string.IsNullOrEmpty(autoCompleteBox.Text) && !string.IsNullOrEmpty(originalText))
            {
                autoCompleteBox.Text = originalText;
            }
        };
        autoCompleteBox.DropDownClosed += closedHandler;
    }
}