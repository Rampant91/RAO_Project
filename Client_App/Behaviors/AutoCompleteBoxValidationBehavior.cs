using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using System.Collections.Generic;
using System.Linq;

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

    // Для строковых значений (коды операции и др.)
    public static readonly StyledProperty<ICollection<string>?> ValidCodesProperty =
        Avalonia.AvaloniaProperty.Register<AutoCompleteBoxValidationBehavior, ICollection<string>?>(
            nameof(ValidCodes),
            defaultValue: null);

    public ICollection<string>? ValidCodes
    {
        get => GetValue(ValidCodesProperty);
        set => SetValue(ValidCodesProperty, value);
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

        // Проверяем, есть ли значение в списке допустимых
        if (!IsValidValue(currentValue))
        {
            // Возвращаем старое значение
            AssociatedObject.Text = _originalValue;
        }
    }

    private bool IsValidValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        // Сначала проверяем строковые коды (если заданы)
        // Используем Any с проверкой на null для поддержки ICollection<string?>
        if (ValidCodes != null && ValidCodes.Any(c => c != null && c == value))
            return true;

        // Затем проверяем числовые значения (если заданы)
        if (ValidValues != null && short.TryParse(value, out var numericValue))
        {
            return ValidValues.Contains(numericValue);
        }

        return false;
    }

    // Метод для пометки, что значение было выбрано из списка
    public void MarkValueSelectedFromDropDown()
    {
        _valueSelectedFromDropDown = true;
    }
}
