using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia;
using System;

namespace Client_App.Controls.MaskedTextBox;

/// <summary>
/// Кастомный MaskedTextBox для даты.
/// </summary>
public class MaskedTextBoxForDate : Avalonia.Controls.MaskedTextBox
{
    /// <summary>
    /// Проверяет, что вставляется дата, в противном случае ничего не вставляет.
    /// Добавлен, поскольку у стандартного MaskedTextBox вызывается метод от наследуемого TextBox'а,
    /// что позволяет вставить любые запрещённые символы игнорируя маску.
    /// </summary>
    public new async void Paste()
    {
        var eventArgs = new RoutedEventArgs(PastingFromClipboardEvent);
        RaiseEvent(eventArgs);
        if (eventArgs.Handled)
        {
            return;
        }

        var text = await ((IClipboard)AvaloniaLocator.Current.GetService(typeof(IClipboard))!).GetTextAsync();

        if (string.IsNullOrEmpty(text))
        {
            return;
        }
        if (DateOnly.TryParse(text.Trim(), out var date)) Text = date.ToShortDateString();
    }
}