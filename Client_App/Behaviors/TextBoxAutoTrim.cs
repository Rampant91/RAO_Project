using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace Client_App.Behaviors
{
    public static class TextBoxAutoTrim
    {
        // Attached Property для включения функциональности
        public static readonly AttachedProperty<bool> IsEnabledProperty =
            AvaloniaProperty.RegisterAttached<TextBox, bool>(
                "IsEnabled",
                typeof(TextBoxAutoTrim),
                false,
                false
            );

        // Приватные Attached Properties для хранения состояния
        private static readonly AttachedProperty<bool> IsUserInputProperty =
            AvaloniaProperty.RegisterAttached<TextBox, bool>(
                "IsUserInput",
                typeof(TextBoxAutoTrim),
                false
            );

        private static readonly AttachedProperty<string> LastTextProperty =
            AvaloniaProperty.RegisterAttached<TextBox, string>(
                "LastText",
                typeof(TextBoxAutoTrim),
                null
            );

        static TextBoxAutoTrim()
        {
            IsEnabledProperty.Changed.AddClassHandler<TextBox>(OnIsEnabledChanged);
        }

        public static bool GetIsEnabled(TextBox textBox) => textBox.GetValue(IsEnabledProperty);
        public static void SetIsEnabled(TextBox textBox, bool value) => textBox.SetValue(IsEnabledProperty, value);

        private static void OnIsEnabledChanged(TextBox textBox, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool enabled)
            {
                if (enabled)
                {
                    textBox.TextInput += OnTextInput;
                    textBox.LostFocus += OnLostFocus;
                    textBox.GetObservable(TextBox.TextProperty).Subscribe(text => OnTextChanged(textBox, text));
                }
                else
                {
                    textBox.TextInput -= OnTextInput;
                    textBox.LostFocus -= OnLostFocus;
                }
            }
        }

        private static void OnTextInput(object sender, TextInputEventArgs e)
        {
            if (sender is TextBox textBox)
                textBox.SetValue(IsUserInputProperty, true);
        }

        private static void OnTextChanged(TextBox textBox, string text)
        {
            var isUserInput = textBox.GetValue(IsUserInputProperty);
            var lastText = textBox.GetValue(LastTextProperty);

            if (!isUserInput && text != lastText)
                textBox.SetValue(LastTextProperty, text);
        }

        private static void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var originalText = textBox.Text;
                var trimmedText = originalText?.Trim();

                if (originalText != trimmedText)
                    textBox.Text = trimmedText;

                textBox.SetValue(LastTextProperty, trimmedText);
                textBox.SetValue(IsUserInputProperty, false);
            }
        }
    }
}