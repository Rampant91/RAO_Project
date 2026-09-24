using System;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Behaviors.Controls;

public class DropDownButtonBehavior : Behavior<Button>
{
    private static readonly MethodInfo? PopulateDropDownMethod =
        typeof(AutoCompleteBox).GetMethod(
            "PopulateDropDown",
            BindingFlags.NonPublic | BindingFlags.Instance);

    private static readonly MethodInfo? OpeningDropDownMethod =
        typeof(AutoCompleteBox).GetMethod(
            "OpeningDropDown",
            BindingFlags.NonPublic | BindingFlags.Instance);

    private static readonly FieldInfo? IgnorePropertyChangeField =
        typeof(AutoCompleteBox).GetField(
            "_ignorePropertyChange",
            BindingFlags.NonPublic | BindingFlags.Instance);

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
            ShowDropdown(autoCompleteBox);
        }
        // Fallback: direct parent is Grid (original behavior for non-UserControl usage)
        else if (AssociatedObject.Parent is Grid directGrid && directGrid.Children[0] is AutoCompleteBox directAutoCompleteBox)
        {
            ShowDropdown(directAutoCompleteBox);
        }
    }

    /// <summary>
    /// Открывает список после завершения Click: иначе popup, открытый в обработчике кнопки,
    /// сразу закрывается, а очищенный Text не восстанавливается.
    /// </summary>
    private static void ShowDropdown(AutoCompleteBox autoCompleteBox)
    {
        if (autoCompleteBox.IsDropDownOpen) return;

        var originalText = autoCompleteBox.Text;

        // Откладываем открытие до конца Click — иначе Avalonia закрывает popup вместе с кликом.
        Dispatcher.UIThread.Post(() =>
        {
            if (autoCompleteBox.IsDropDownOpen) return;

            autoCompleteBox.Focus();

            // Временно очищаем текст, чтобы показать все значения (MinimumPrefixLength=0)
            autoCompleteBox.Text = string.Empty;

            // Restore должен быть подписан до любого открытия — иначе при раннем закрытии
            // (или при IsDropDownOpen=true после Populate) значение останется пустым.
            EventHandler? closedHandler = null;
            closedHandler = (_, _) =>
            {
                autoCompleteBox.DropDownClosed -= closedHandler;
                if (string.IsNullOrEmpty(autoCompleteBox.Text) && !string.IsNullOrEmpty(originalText))
                {
                    autoCompleteBox.Text = originalText;
                }
            };
            autoCompleteBox.DropDownClosed += closedHandler;

            PopulateDropDownMethod?.Invoke(autoCompleteBox, [autoCompleteBox, EventArgs.Empty]);
            OpeningDropDownMethod?.Invoke(autoCompleteBox, [false]);

            if (autoCompleteBox.IsDropDownOpen) return;

            ForceOpenDropDown(autoCompleteBox);
        });
    }

    /// <summary>
    /// Выставляет IsDropDownOpen, подавляя PropertyChanged — иначе OnIsDropDownOpenChanged
    /// снова вызывает TextUpdated и может сразу закрыть список.
    /// </summary>
    private static void ForceOpenDropDown(AutoCompleteBox autoCompleteBox)
    {
        if (IgnorePropertyChangeField?.GetValue(autoCompleteBox) is false)
        {
            IgnorePropertyChangeField.SetValue(autoCompleteBox, true);
        }

        autoCompleteBox.SetValue(AutoCompleteBox.IsDropDownOpenProperty, true);
    }
}
