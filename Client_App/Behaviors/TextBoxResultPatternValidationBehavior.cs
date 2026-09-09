using System;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Behaviors;

/// <summary>
/// Shared live-input patterns for calculator (and similar) numeric fields.
/// Cyrillic e variants are included so RU keyboard can type the exponent marker.
/// </summary>
public static class CalculatorInputPatterns
{
    /// <summary>Positive number in plain or exponential form (partial input allowed while typing).</summary>
    public const string PositiveExponential =
        "^(\\+)?\\d*([.,]\\d*)?([eE\u0435\u0415][+\\-]?\\d*)?$";

    /// <summary>Positive decimal (no sign, no exponent); partial input allowed while typing.</summary>
    public const string PositiveDecimal = "^\\d*([.,]\\d*)?$";

    /// <summary>Digits and '.' only (no auto-format); empty allowed by the behavior.</summary>
    public const string DateDigitsAndDots = "^[0-9.]*$";
}

/// <summary>
/// Blocks TextBox input when the resulting text would not match <see cref="Pattern"/>.
/// Empty text is always allowed. Use for live filters (exponential activity, positive decimals, etc.).
/// </summary>
public class TextBoxResultPatternValidationBehavior : Behavior<TextBox>
{
    public static readonly StyledProperty<string> PatternProperty =
        AvaloniaProperty.Register<TextBoxResultPatternValidationBehavior, string>(nameof(Pattern));

    public string Pattern
    {
        get => GetValue(PatternProperty);
        set => SetValue(PatternProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is null)
            return;

        AssociatedObject.AddHandler(InputElement.TextInputEvent, OnTextInput, RoutingStrategies.Tunnel);
        AssociatedObject.AddHandler(TextBox.PastingFromClipboardEvent, OnPastingFromClipboard, RoutingStrategies.Tunnel);
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is not null)
        {
            AssociatedObject.RemoveHandler(InputElement.TextInputEvent, OnTextInput);
            AssociatedObject.RemoveHandler(TextBox.PastingFromClipboardEvent, OnPastingFromClipboard);
        }

        base.OnDetaching();
    }

    private void OnTextInput(object? sender, TextInputEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Text) || AssociatedObject is null)
            return;

        var next = BuildResultText(AssociatedObject, e.Text);
        if (!IsAllowed(next))
            e.Handled = true;
    }

    private async void OnPastingFromClipboard(object? sender, RoutedEventArgs e)
    {
        e.Handled = true;

        var clipboard = TopLevel.GetTopLevel(AssociatedObject!)?.Clipboard;
        if (clipboard is null || AssociatedObject is null)
            return;

        try
        {
            var text = await clipboard.GetTextAsync();
            if (string.IsNullOrEmpty(text))
                return;

            var next = BuildResultText(AssociatedObject, text);
            if (!IsAllowed(next))
                return;

            var textBox = AssociatedObject;
            var start = Math.Min(textBox.SelectionStart, textBox.SelectionEnd);
            var end = Math.Max(textBox.SelectionStart, textBox.SelectionEnd);
            var current = textBox.Text ?? string.Empty;
            textBox.Text = current[..start] + text + current[end..];
            textBox.CaretIndex = start + text.Length;
        }
        catch (TimeoutException)
        {
            // Clipboard may time out; ignore and keep current text.
        }
    }

    private bool IsAllowed(string text)
    {
        if (string.IsNullOrEmpty(text))
            return true;

        var pattern = Pattern;
        return !string.IsNullOrEmpty(pattern) && Regex.IsMatch(text, pattern);
    }

    private static string BuildResultText(TextBox textBox, string incoming)
    {
        var text = textBox.Text ?? string.Empty;
        var start = Math.Clamp(Math.Min(textBox.SelectionStart, textBox.SelectionEnd), 0, text.Length);
        var end = Math.Clamp(Math.Max(textBox.SelectionStart, textBox.SelectionEnd), 0, text.Length);
        return text.Remove(start, end - start).Insert(start, incoming);
    }
}
