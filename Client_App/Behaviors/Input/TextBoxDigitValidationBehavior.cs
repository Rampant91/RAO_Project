using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using System;
using System.Text.RegularExpressions;

namespace Client_App.Behaviors.Input;

/// <summary>
/// Разрешает в TextBox только цифры. Опционально <see cref="MaxValue"/> (например 255 для byte).
/// </summary>
public class TextBoxDigitValidationBehavior : Behavior<TextBox>
{
    private static readonly Regex Disallowed = new("[^0-9]+");

    public static readonly StyledProperty<int?> MaxValueProperty =
        AvaloniaProperty.Register<TextBoxDigitValidationBehavior, int?>(nameof(MaxValue));

    /// <summary>Максимальное целое после ввода; null — без верхней границы.</summary>
    public int? MaxValue
    {
        get => GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is null)
            return;

        AssociatedObject.TextInput += OnTextInput;
        AssociatedObject.KeyDown += OnPreviewKeyDown;
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is not null)
        {
            AssociatedObject.TextInput -= OnTextInput;
            AssociatedObject.KeyDown -= OnPreviewKeyDown;
        }

        base.OnDetaching();
    }

    private void OnTextInput(object? sender, TextInputEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Text) || AssociatedObject is null)
            return;

        if (Disallowed.IsMatch(e.Text))
        {
            e.Handled = true;
            return;
        }

        if (MaxValue is not int max)
            return;

        var next = BuildResultText(AssociatedObject, e.Text);
        if (next.Length == 0)
            return;

        if (!int.TryParse(next, out var value) || value > max)
            e.Handled = true;
    }

    private void OnPreviewKeyDown(object? sender, KeyEventArgs e)
    {
        if (IsControlKey(e))
            return;

        if (IsDigitKey(e))
            return;

        e.Handled = true;
    }

    private static string BuildResultText(TextBox textBox, string incoming)
    {
        var text = textBox.Text ?? string.Empty;
        var start = Math.Clamp(textBox.SelectionStart, 0, text.Length);
        var end = Math.Clamp(textBox.SelectionEnd, 0, text.Length);
        if (end < start)
            (start, end) = (end, start);

        return text.Remove(start, end - start).Insert(start, incoming);
    }

    private static bool IsControlKey(KeyEventArgs e) =>
        e.Key is Key.Back or Key.Delete or Key.Left or Key.Right or Key.Home or Key.End
            or Key.Tab or Key.Enter or Key.Escape or Key.CapsLock or Key.PageUp or Key.PageDown
        || e.KeyModifiers.HasFlag(KeyModifiers.Control);

    private static bool IsDigitKey(KeyEventArgs e) =>
        e.Key is >= Key.D0 and <= Key.D9 or >= Key.NumPad0 and <= Key.NumPad9;
}
