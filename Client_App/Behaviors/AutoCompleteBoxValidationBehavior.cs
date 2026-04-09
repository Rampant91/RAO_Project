using Avalonia.Controls;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Behaviors;

public class AutoCompleteBoxValidationBehavior : Behavior<AutoCompleteBox>
{
    private string? _originalValue;

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
    }

    private void OnLostFocus(object? sender, System.EventArgs e)
    {
        if (AssociatedObject == null || _originalValue == null)
            return;

        string currentValue = AssociatedObject.Text ?? string.Empty;

        // Если значение не изменилось, не проверяем
        if (currentValue == _originalValue)
            return;

        // Проверяем, есть ли значение в списке допустимых кодов
        if (!IsValidCode(currentValue))
        {
            // Возвращаем старое значение
            AssociatedObject.Text = _originalValue;
        }
    }

    private bool IsValidCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || ValidCodes == null)
            return false;

        return ValidCodes.Contains(code);
    }
}
