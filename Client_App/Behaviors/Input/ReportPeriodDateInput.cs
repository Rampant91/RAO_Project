using System;
using System.Globalization;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Client_App.Behaviors.Input;

/// <summary>
/// Поля дат периода в шапке отчёта (формы 1.x): ввод дд.мм.гггг, макс. 10 символов, запятая → точка.
/// </summary>
public static class ReportPeriodDateInput
{
    public static readonly AttachedProperty<bool> IsEnabledProperty =
        AvaloniaProperty.RegisterAttached<Control, bool>("IsEnabled", typeof(ReportPeriodDateInput));

    private const int MaxLength = 10;

    static ReportPeriodDateInput()
    {
        IsEnabledProperty.Changed.AddClassHandler<TextBox>(OnIsEnabledChanged);
        IsEnabledProperty.Changed.AddClassHandler<Avalonia.Controls.MaskedTextBox>(OnMaskedIsEnabledChanged);
    }

    public static bool GetIsEnabled(Control element) => element.GetValue(IsEnabledProperty);

    public static void SetIsEnabled(Control element, bool value) => element.SetValue(IsEnabledProperty, value);

    private static void OnMaskedIsEnabledChanged(Avalonia.Controls.MaskedTextBox textBox, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is not true)
        {
            DetachMasked(textBox);
            return;
        }

        textBox.MaxLength = MaxLength;
        textBox.TextInput += OnTextInput;
        textBox.KeyDown += OnKeyDown;
        textBox.LostFocus += OnLostFocus;
    }

    private static void DetachMasked(Avalonia.Controls.MaskedTextBox textBox)
    {
        textBox.TextInput -= OnTextInput;
        textBox.KeyDown -= OnKeyDown;
        textBox.LostFocus -= OnLostFocus;
    }

    private static void OnIsEnabledChanged(TextBox textBox, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is not true)
        {
            Detach(textBox);
            return;
        }

        textBox.MaxLength = MaxLength;
        textBox.TextInput += OnTextInput;
        textBox.KeyDown += OnKeyDown;
        textBox.LostFocus += OnLostFocus;
    }

    private static void Detach(TextBox textBox)
    {
        textBox.TextInput -= OnTextInput;
        textBox.KeyDown -= OnKeyDown;
        textBox.LostFocus -= OnLostFocus;
    }

    private static void OnTextInput(object? sender, TextInputEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Text) || sender is not TextBox textBox)
            return;

        var ch = e.Text[0];
        if (ch == ',')
        {
            e.Handled = true;
            InsertAtCaret(textBox, ".");
            return;
        }

        if (!char.IsDigit(ch) && ch != '.')
            e.Handled = true;
    }

    private static void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not TextBox)
            return;

        if (IsControlKey(e))
            return;

        if (e.Key == Key.OemComma || e.Key == Key.OemPeriod)
            return;

        if (IsDigitKey(e))
            return;

        e.Handled = true;
    }

    private static void OnLostFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is not TextBox textBox)
            return;

        var text = (textBox.Text ?? string.Empty).Trim();
        if (string.IsNullOrEmpty(text))
            return;

        if (DateOnly.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out var date)
            || DateOnly.TryParseExact(text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            var normalized = date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            if (!string.Equals(textBox.Text, normalized, StringComparison.Ordinal))
                textBox.Text = normalized;
        }
    }

    private static void InsertAtCaret(TextBox textBox, string text)
    {
        var current = textBox.Text ?? string.Empty;
        var start = Math.Clamp(textBox.SelectionStart, 0, current.Length);
        var end = Math.Clamp(textBox.SelectionEnd, 0, current.Length);
        if (end < start)
            (start, end) = (end, start);

        var next = current.Remove(start, end - start).Insert(start, text);
        if (next.Length > MaxLength)
            return;

        textBox.Text = next;
        textBox.CaretIndex = start + text.Length;
    }

    private static bool IsControlKey(KeyEventArgs e) =>
        e.Key is Key.Back or Key.Delete or Key.Left or Key.Right or Key.Home or Key.End
            or Key.Tab or Key.Enter or Key.Escape or Key.PageUp or Key.PageDown
        || e.KeyModifiers.HasFlag(KeyModifiers.Control);

    private static bool IsDigitKey(KeyEventArgs e) =>
        e.Key is >= Key.D0 and <= Key.D9 or >= Key.NumPad0 and <= Key.NumPad9;
}
