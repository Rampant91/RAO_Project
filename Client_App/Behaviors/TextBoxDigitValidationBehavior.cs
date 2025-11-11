using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using System.Text.RegularExpressions;

namespace Client_App.Behaviors;

public class TextBoxDigitValidationBehavior : Behavior<TextBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject != null)
        {
            AssociatedObject.TextInput += TextInput;
            AssociatedObject.KeyDown += PreviewKeyDown;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {

            AssociatedObject.TextInput -= TextInput;
            AssociatedObject.KeyDown -= PreviewKeyDown;
        }

        base.OnDetaching();
    }

    private static readonly Regex _regex = new("[^0-9]+"); //regex that matches disallowed text

    private static bool IsTextAllowed(string text)
    {
        return !_regex.IsMatch(text);
    }

    private static void TextInput(object sender, TextInputEventArgs e)
    {

        e.Handled = !IsTextAllowed(e.Text);
    }

    private static void PreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Разрешаем служебные клавиши (Backspace, Delete, стрелки и т.д.)
        if (IsControlKey(e))
        {
            return;
        }

        // Разрешаем цифры с основной клавиатуры и цифрового блока
        if (IsDigitKey(e))
        {
            return;
        }

        // Блокируем все остальные клавиши
        e.Handled = true;
    }

    private static bool IsControlKey(KeyEventArgs e)
    {
        return e.Key == Key.Back ||
               e.Key == Key.Delete ||
               e.Key == Key.Left ||
               e.Key == Key.Right ||
               e.Key == Key.Home ||
               e.Key == Key.End ||
               e.Key == Key.Tab ||
               e.Key == Key.Enter ||
               e.Key == Key.Escape ||
               e.Key == Key.CapsLock ||
               e.Key == Key.PageUp ||
               e.Key == Key.PageDown ||
               e.KeyModifiers.HasFlag(KeyModifiers.Control); // Разрешаем Ctrl+C, Ctrl+V и т.д.
    }

    private static bool IsDigitKey(KeyEventArgs e)
    {
        // Цифры на основной клавиатуре (0-9) и на цифровом блоке (NumPad0-NumPad9)
        return e.Key switch
        {
            >= Key.D0 and <= Key.D9 or >= Key.NumPad0 and <= Key.NumPad9 => true,
            _ => false
        };
    }
}