using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using AvaloniaEdit.Utils;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Client_App.Behaviors
{
    public class NumericUpDownDigitValidationBehavior : Behavior<NumericUpDown>    {
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
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void TextInput(object sender, TextInputEventArgs e)
        {

            e.Handled = !IsTextAllowed(e.Text);
        }
        private void PreviewKeyDown(object sender, KeyEventArgs e)
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

        private bool IsControlKey(KeyEventArgs e)
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

        private bool IsDigitKey(KeyEventArgs e)
        {
            // Цифры на основной клавиатуре (0-9)
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
                return true;

            // Цифры на цифровом блоке (NumPad0-NumPad9)
            if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                return true;

            return false;
        }
    }
}
