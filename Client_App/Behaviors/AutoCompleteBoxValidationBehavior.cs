using Avalonia.Controls;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Behaviors;

public class AutoCompleteBoxValidationBehavior : Behavior<AutoCompleteBox>
{
    private string? _originalValue;
    private bool _valueSelectedFromDropDown;

    public static readonly StyledProperty<ICollection<short?>?> ValidValuesProperty =
        Avalonia.AvaloniaProperty.Register<AutoCompleteBoxValidationBehavior, ICollection<short?>?>(
            nameof(ValidValues),
            defaultValue: null);

    public ICollection<short?>? ValidValues
    {
        get => GetValue(ValidValuesProperty);
        set => SetValue(ValidValuesProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject != null)
        {
            AssociatedObject.GotFocus += OnGotFocus;
            AssociatedObject.LostFocus += OnLostFocus;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.GotFocus -= OnGotFocus;
            AssociatedObject.LostFocus -= OnLostFocus;
        }

        base.OnDetaching();
    }

    private void OnGotFocus(object? sender, System.EventArgs e)
    {
        _originalValue = AssociatedObject?.Text;
        _valueSelectedFromDropDown = false;
    }

    private void OnLostFocus(object? sender, System.EventArgs e)
    {
        if (AssociatedObject == null)
            return;

        string currentValue = AssociatedObject.Text ?? string.Empty;

        // Если dropdown открыт, не проверяем (пользователь кликнул на список)
        if (AssociatedObject.IsDropDownOpen)
            return;

        // Если значение было выбрано из списка, не проверяем
        if (_valueSelectedFromDropDown)
            return;

        // Если значение не изменилось, не проверяем
        if (currentValue == _originalValue)
            return;

        // Проверяем, есть ли значение в списке допустимых категорий
        if (!IsValidCategory(currentValue))
        {
            // Возвращаем старое значение
            AssociatedObject.Text = _originalValue;
        }
    }

    private bool IsValidCategory(string categoryStr)
    {
        if (string.IsNullOrWhiteSpace(categoryStr) || ValidValues == null)
            return false;

        if (short.TryParse(categoryStr, out var categoryValue))
        {
            return ValidValues.Contains(categoryValue);
        }

        return false;
    }

    // Метод для пометки, что значение было выбрано из списка
    public void MarkValueSelectedFromDropDown()
    {
        _valueSelectedFromDropDown = true;
    }
}
