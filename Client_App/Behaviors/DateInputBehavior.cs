using System;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Behaviors;

/// <summary>
/// Restricts CalendarDatePicker typed input to digits and '.' in dd.mm.yyyy shape.
/// </summary>
public partial class CalendarDatePickerBehavior : Behavior<CalendarDatePicker>
{
    private TextBox? _textBox;
    private bool _isAttaching;

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is null)
            return;

        AssociatedObject.TemplateApplied += OnTemplateApplied;
        AssociatedObject.AttachedToVisualTree += OnAttachedToVisualTree;
        // Do not call ApplyTemplate here: it raises TemplateApplied and would recurse.
        TryAttachToTextBox();
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is not null)
        {
            AssociatedObject.TemplateApplied -= OnTemplateApplied;
            AssociatedObject.AttachedToVisualTree -= OnAttachedToVisualTree;
        }

        DetachFromTextBox();
        base.OnDetaching();
    }

    private void OnTemplateApplied(object? sender, TemplateAppliedEventArgs e) => TryAttachToTextBox();

    private void OnAttachedToVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e) =>
        TryAttachToTextBox();

    private void TryAttachToTextBox()
    {
        if (AssociatedObject is null || _isAttaching)
            return;

        _isAttaching = true;
        try
        {
            // Safe under _isAttaching: TemplateApplied re-entry is ignored (no recursion).
            AssociatedObject.ApplyTemplate();

            var textBox = AssociatedObject.GetTemplateChildren().OfType<TextBox>().FirstOrDefault()
                          ?? AssociatedObject.GetVisualDescendants().OfType<TextBox>().FirstOrDefault();

            if (textBox is null || ReferenceEquals(textBox, _textBox))
                return;

            DetachFromTextBox();
            _textBox = textBox;
            _textBox.AddHandler(InputElement.TextInputEvent, OnTextInput, RoutingStrategies.Tunnel);
            _textBox.AddHandler(InputElement.KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
        }
        finally
        {
            _isAttaching = false;
        }
    }

    private void DetachFromTextBox()
    {
        if (_textBox is null)
            return;

        _textBox.RemoveHandler(InputElement.TextInputEvent, OnTextInput);
        _textBox.RemoveHandler(InputElement.KeyDownEvent, OnKeyDown);
        _textBox = null;
    }

    private static void OnTextInput(object? sender, TextInputEventArgs e)
    {
        if (e.Text is null || sender is not TextBox textBox)
            return;

        if (!char.IsDigit(e.Text, 0) && e.Text != ".")
        {
            e.Handled = true;
            return;
        }

        var currentText = textBox.Text ?? string.Empty;
        var start = Math.Min(textBox.SelectionStart, textBox.SelectionEnd);
        var end = Math.Max(textBox.SelectionStart, textBox.SelectionEnd);
        var newText = currentText[..start] + e.Text + currentText[end..];

        if (!IsValidDateInput(newText))
            e.Handled = true;
    }

    private static void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key is Key.Back or Key.Delete or Key.Tab or Key.Enter or
            Key.Left or Key.Right or Key.Home or Key.End)
            return;

        if (e.KeyModifiers.HasFlag(KeyModifiers.Control))
            return;

        if (e.Key is Key.OemPeriod or Key.Oem2)
            return;

        if (e.Key is >= Key.D0 and <= Key.D9 or >= Key.NumPad0 and <= Key.NumPad9)
            return;

        e.Handled = true;
    }

    private static bool IsValidDateInput(string input)
    {
        if (string.IsNullOrEmpty(input))
            return true;

        if (!DateInputRegex().IsMatch(input))
            return false;

        var parts = input.Split('.');

        // Day — only when both digits are present.
        if (parts is [{ Length: 2 }, ..]
            && int.TryParse(parts[0], out var day)
            && day is < 1 or > 31)
            return false;

        // Month — only when both digits are present.
        if (parts is [_, { Length: 2 }, ..]
            && int.TryParse(parts[1], out var month)
            && month is < 1 or > 12)
            return false;

        // Year — only when all four digits are present.
        if (parts is [_, _, { Length: 4 }]
            && int.TryParse(parts[2], out var year)
            && year is < 1900 or > 2100)
            return false;

        return true;
    }

    [GeneratedRegex(@"^[0-9]{0,2}(\.[0-9]{0,2}(\.[0-9]{0,4})?)?$")]
    private static partial Regex DateInputRegex();
}
