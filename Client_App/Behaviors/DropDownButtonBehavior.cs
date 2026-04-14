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
        if (AssociatedObject.Parent is Grid grid && grid.Children[0] is AutoCompleteBox autoCompleteBox)
        {
            autoCompleteBox.Focus();
            ShowDropdown(autoCompleteBox);
        }
    }

    private static void ShowDropdown(AutoCompleteBox autoCompleteBox)
    {
        if (autoCompleteBox.IsDropDownOpen) return;

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
    }
}