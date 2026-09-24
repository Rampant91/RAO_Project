using System;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Behaviors.Controls;

public class DropDownButtonBehavior : Behavior<Button>
{
    private static readonly MethodInfo? PopulateDropDownMethod =
        typeof(AutoCompleteBox).GetMethod(
            "PopulateDropDown",
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

    /// <summary>
    /// Показать весь список: очистить фильтр, наполнить _view через PopulateDropDown,
    /// затем всегда открыть через force-open (_ignorePropertyChange).
    /// OpeningDropDown через reflection не вызываем — он ставит IsDropDownOpen=true
    /// в «битом» состоянии (флаг true, popup не виден), после чего ранний return
    /// пропускал force-open и оставлял очищенный Text без DropDownClosed.
    /// </summary>
    private static void ShowDropdown(AutoCompleteBox autoCompleteBox)
    {
        var originalText = autoCompleteBox.Text;

        // Пока готовим открытие, DropDownClosed от промежуточного close не должен
        // восстанавливать текст (иначе снова включится фильтр по старому значению).
        var restoreOnClose = false;
        EventHandler? closedHandler = null;
        closedHandler = (_, _) =>
        {
            if (!restoreOnClose) return;
            autoCompleteBox.DropDownClosed -= closedHandler;
            if (string.IsNullOrEmpty(autoCompleteBox.Text) && !string.IsNullOrEmpty(originalText))
            {
                autoCompleteBox.Text = originalText;
            }
        };
        autoCompleteBox.DropDownClosed += closedHandler;

        // Временно очищаем, чтобы при MinimumPrefixLength=0 в список попали все значения
        autoCompleteBox.Text = string.Empty;

        // Наполняет внутренний _view; может сам выставить IsDropDownOpen=true
        PopulateDropDownMethod?.Invoke(autoCompleteBox, [autoCompleteBox, EventArgs.Empty]);

        // Если populate уже «открыл» — сбрасываем и открываем заново рабочим путём.
        // Без ignore: нужно реально закрыть popup, не только свойство.
        if (autoCompleteBox.IsDropDownOpen)
        {
            autoCompleteBox.SetValue(AutoCompleteBox.IsDropDownOpenProperty, false);
        }

        ForceOpenDropDown(autoCompleteBox);
        restoreOnClose = true;

        // Страховка: список так и не открылся — сразу вернуть текст, не ждать Closed
        if (!autoCompleteBox.IsDropDownOpen)
        {
            autoCompleteBox.DropDownClosed -= closedHandler;
            if (!string.IsNullOrEmpty(originalText))
            {
                autoCompleteBox.Text = originalText;
            }
        }
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
