using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia.Controls.Templates;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Behaviors
{
    public class CalendarDatePickerBehavior : Behavior<CalendarDatePicker>
    {
        public static readonly AttachedProperty<bool> EnableDateInputProperty =
            AvaloniaProperty.RegisterAttached<CalendarDatePicker, bool>(
                "EnableDateInput", 
                typeof(CalendarDatePickerBehavior), 
                false);

        public static bool GetEnableDateInput(CalendarDatePicker element)
        {
            return element.GetValue(EnableDateInputProperty);
        }

        public static void SetEnableDateInput(CalendarDatePicker element, bool value)
        {
            element.SetValue(EnableDateInputProperty, value);
        }

        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                // Принудительно применяем шаблон
                AssociatedObject.ApplyTemplate();
                
                // Ищем TextBox с задержкой, чтобы шаблон успел загрузиться
                var timer = new Avalonia.Threading.DispatcherTimer
                {
                    Interval = System.TimeSpan.FromMilliseconds(100)
                };
                
                timer.Tick += (sender, e) =>
                {
                    // Пробуем несколько способов найти TextBox
                    TextBox? textBox = null;
                    
                    // Способ 1: Через GetTemplateChildren
                    textBox = AssociatedObject.GetTemplateChildren()
                        .OfType<TextBox>()
                        .FirstOrDefault();
                    
                    // Способ 2: Через GetVisualDescendants
                    if (textBox == null)
                    {
                        textBox = AssociatedObject.GetVisualDescendants()
                            .OfType<TextBox>()
                            .FirstOrDefault();
                    }
                    
                    // Отладочная информация
                    System.Diagnostics.Debug.WriteLine($"CalendarDatePickerBehavior: TextBox найден: {textBox != null}");
                    if (textBox != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"CalendarDatePickerBehavior: TextBox.Name = {textBox.Name}");
                        textBox.AddHandler(TextBox.TextInputEvent, OnTextInput, RoutingStrategies.Tunnel);
                        textBox.AddHandler(InputElement.KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
                        textBox.PropertyChanged += OnTextChanged;
                    }
                    
                    timer.Stop();
                };
                
                timer.Start();
            }
            base.OnAttached();
        }

        private static T? FindVisualChild<T>(Control parent) where T : Control
        {
            if (parent == null) return null;

            var children = parent.GetVisualChildren();
            foreach (var child in children)
            {
                if (child is T result)
                    return result;

                if (child is Control childControl)
                {
                    var childOfChild = FindVisualChild<T>(childControl);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                // Ищем TextBox в шаблоне
                var textBox = AssociatedObject.GetTemplateChildren()
                    .OfType<TextBox>()
                    .FirstOrDefault();
                    
                if (textBox != null)
                {
                    textBox.RemoveHandler(TextBox.TextInputEvent, OnTextInput);
                    textBox.RemoveHandler(InputElement.KeyDownEvent, OnKeyDown);
                    textBox.PropertyChanged -= OnTextChanged;
                }
            }
            base.OnDetaching();
        }

        private void OnTextInput(object? sender, TextInputEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"CalendarDatePickerBehavior: OnTextInput вызван, Text = {e.Text}");
            
            if (e.Text != null)
            {
                // Разрешаем только цифры и точки
                if (!char.IsDigit(e.Text, 0) && e.Text != ".")
                {
                    System.Diagnostics.Debug.WriteLine($"CalendarDatePickerBehavior: Блокируем символ '{e.Text}'");
                    e.Handled = true;
                    return;
                }

                // Проверяем корректность формата после добавления символа
                if (sender is TextBox textBox)
                {
                    var currentText = textBox.Text ?? "";
                    var newText = currentText.Insert(textBox.SelectionStart, e.Text);
                    
                    if (!IsValidDateInput(newText))
                    {
                        System.Diagnostics.Debug.WriteLine($"CalendarDatePickerBehavior: Некорректная дата '{newText}'");
                        e.Handled = true;
                    }
                }
            }
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            // Разрешаем Backspace, Delete, Tab, Enter, стрелки
            if (e.Key is Key.Back or Key.Delete or Key.Tab or Key.Enter or 
                       Key.Left or Key.Right or Key.Home or Key.End)
                return;

            // Разрешаем только цифры и точку
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || 
                (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || 
                e.Key == Key.OemPeriod)
                return;

            e.Handled = true;
        }

        private void OnTextChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TextBox.TextProperty && sender is TextBox textBox)
            {
                var text = textBox.Text ?? "";
                
                // Форматируем дату автоматически
                var formatted = FormatDate(text);
                
                if (formatted != text)
                {
                    var cursorPos = textBox.SelectionStart;
                    textBox.Text = formatted;
                    
                    // Восстанавливаем позицию курсора
                    var newCursorPos = CalculateNewCursorPosition(cursorPos, text, formatted);
                    textBox.SelectionStart = newCursorPos;
                }
            }
        }

        private static bool IsValidDateInput(string input)
        {
            if (string.IsNullOrEmpty(input)) return true;

            // Проверяем, что строка соответствует формату дд.мм.гггг
            var pattern = @"^[0-9]{0,2}(\.[0-9]{0,2}(\.[0-9]{0,4})?)?$";
            if (!Regex.IsMatch(input, pattern)) return false;

            // Проверяем диапазоны значений
            var parts = input.Split('.');
            
            // День
            if (parts.Length >= 1 && parts[0].Length > 0)
            {
                if (int.TryParse(parts[0], out int day) && (day < 1 || day > 31))
                    return false;
            }

            // Месяц
            if (parts.Length >= 2 && parts[1].Length > 0)
            {
                if (int.TryParse(parts[1], out int month) && (month < 1 || month > 12))
                    return false;
            }

            // Год
            if (parts.Length >= 3 && parts[2].Length > 0)
            {
                if (int.TryParse(parts[2], out int year) && (year < 1900 || year > 2100))
                    return false;
            }

            return true;
        }

        private static string FormatDate(string input)
        {
            var digits = new string(input.Where(char.IsDigit).ToArray());
            
            if (digits.Length == 0) return "";
            
            var result = "";
            
            // День
            if (digits.Length >= 1)
                result += digits[0];
            if (digits.Length >= 2)
                result += digits[1];
            
            // Первая точка - автоматически после 2 цифр (день)
            if (digits.Length >= 2)
                result += ".";
            
            // Месяц
            if (digits.Length >= 3)
                result += digits[2];
            if (digits.Length >= 4)
                result += digits[3];
            
            // Вторая точка - автоматически после 4 цифр (месяц)
            if (digits.Length >= 4)
                result += ".";
            
            // Год
            if (digits.Length >= 5)
                result += digits[4];
            if (digits.Length >= 6)
                result += digits[5];
            if (digits.Length >= 7)
                result += digits[6];
            if (digits.Length >= 8)
                result += digits[7];
            
            return result;
        }

        private static int CalculateNewCursorPosition(int originalPos, string originalText, string formattedText)
        {
            // Если курсор в конце, ставим в конец
            if (originalPos >= originalText.Length)
                return formattedText.Length;
            
            // Считаем цифры до позиции курсора
            var digitsBeforeCursor = originalText.Take(originalPos).Count(char.IsDigit);
            
            // Находим соответствующую позицию в отформатированном тексте
            var resultPos = 0;
            var digitCount = 0;
            
            foreach (char c in formattedText)
            {
                if (char.IsDigit(c))
                {
                    digitCount++;
                    if (digitCount > digitsBeforeCursor)
                        break;
                }
                resultPos++;
            }
            
            return Math.Min(resultPos, formattedText.Length);
        }
    }
}
