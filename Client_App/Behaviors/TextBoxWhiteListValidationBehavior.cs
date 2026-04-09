using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using System;
using System.Text.RegularExpressions;

namespace Client_App.Behaviors;

public class TextBoxWhiteListValidationBehavior : Behavior<TextBox>
{
    //Regex строка для ситуативно разрешенных символов
    public static readonly StyledProperty<string> WhiteListRegexProperty =
        AvaloniaProperty.Register<TextBoxWhiteListValidationBehavior, string>(nameof(WhiteListRegex));

    public string WhiteListRegex
    {
        get => GetValue(WhiteListRegexProperty);
        set => SetValue(WhiteListRegexProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject != null)
        {
            // Используем AddHandler с RoutingStrategies.Tunnel для перехвата события
            AssociatedObject.AddHandler(InputElement.TextInputEvent, TextInput, RoutingStrategies.Tunnel);

            // Обработка вставки из буфера обмена
            AssociatedObject.AddHandler(TextBox.PastingFromClipboardEvent, PastingFromClipboard, RoutingStrategies.Tunnel);
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.RemoveHandler(InputElement.TextInputEvent, TextInput);
            AssociatedObject.RemoveHandler(TextBox.PastingFromClipboardEvent, PastingFromClipboard);
        }

        base.OnDetaching();
    }


    private bool IsTextAllowed(string text)
    {
        var regex = new Regex(WhiteListRegex);
        return regex.IsMatch(text);
    }

    private void TextInput(object sender, TextInputEventArgs e)
    {

        e.Handled = !IsTextAllowed(e.Text);
    }

    private async void PastingFromClipboard(object? sender, RoutedEventArgs e)
    {
        // Отменяем стандартную вставку
        e.Handled = true;

        var clipboard = Application.Current?.Clipboard;
        if (clipboard is null)
            return;

        try
        {
            var text = await clipboard.GetTextAsync();
            if (string.IsNullOrEmpty(text))
                return;

            // Проверяем весь вставляемый текст
            if (IsTextAllowed(text))
            {
                // Вставляем разрешенный текст в текущую позицию курсора
                var textBox = AssociatedObject;
                var caretIndex = textBox.CaretIndex;
                textBox.Text = textBox.Text.Insert(caretIndex, text);
                textBox.CaretIndex = caretIndex + text.Length;
            }
            // Если текст не прошел проверку, просто ничего не делаем
        }
        catch (TimeoutException)
        {
            // Игнорируем ошибку таймаута при получении текста из буфера
        }
    }
    //private void PreviewKeyDown(object sender, KeyEventArgs e)
    //{
    //    // Разрешаем служебные клавиши (Backspace, Delete, стрелки и т.д.)
    //    if (IsControlKey(e))
    //    {
    //        return;
    //    }

    //    //// Разрешаем цифры с основной клавиатуры и цифрового блока
    //    //if (IsKeyInWhiteList(e))
    //    //{
    //    //    return;
    //    //}

    //    // Блокируем все остальные клавиши
    //    e.Handled = true;
    //}

    //private static bool IsControlKey(KeyEventArgs e)
    //{
    //    return e.Key == Key.Back ||
    //           e.Key == Key.Delete ||
    //           e.Key == Key.Left ||
    //           e.Key == Key.Right ||
    //           e.Key == Key.Home ||
    //           e.Key == Key.End ||
    //           e.Key == Key.Tab ||
    //           e.Key == Key.Enter ||
    //           e.Key == Key.Escape ||
    //           e.Key == Key.CapsLock ||
    //           e.Key == Key.PageUp ||
    //           e.Key == Key.PageDown ||
    //           e.KeyModifiers.HasFlag(KeyModifiers.Control); // Разрешаем Ctrl+C, Ctrl+V и т.д.
    //}

}