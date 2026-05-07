using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;

namespace Client_App.Behaviors;

public class AutoCompleteBoxValidationBehavior : Behavior<AutoCompleteBox>
{
    private string? _originalValue;
    private bool _valueSelectedFromDropDown;
    private bool _skipOriginalValueUpdate;

    public static readonly StyledProperty<ICollection<short?>?> ValidValuesProperty =
        Avalonia.AvaloniaProperty.Register<AutoCompleteBoxValidationBehavior, ICollection<short?>?>(
            nameof(ValidValues),
            defaultValue: null);

    public ICollection<short?>? ValidValues
    {
        get => GetValue(ValidValuesProperty);
        set => SetValue(ValidValuesProperty, value);
    }

    // Для строковых значений (коды операции и др.)
    public static readonly StyledProperty<ICollection<string>?> ValidCodesProperty =
        Avalonia.AvaloniaProperty.Register<AutoCompleteBoxValidationBehavior, ICollection<string>?>(
            nameof(ValidCodes),
            defaultValue: null);

    public ICollection<string>? ValidCodes
    {
        get => GetValue(ValidCodesProperty);
        set => SetValue(ValidCodesProperty, value);
    }

    // Маска ввода: максимальная длина (0 = без ограничения)
    public static readonly StyledProperty<int> MaxInputLengthProperty =
        Avalonia.AvaloniaProperty.Register<AutoCompleteBoxValidationBehavior, int>(
            nameof(MaxInputLength),
            defaultValue: 0);

    public int MaxInputLength
    {
        get => GetValue(MaxInputLengthProperty);
        set => SetValue(MaxInputLengthProperty, value);
    }

    // Маска ввода: regex pattern для валидации ввода (null = без ограничений)
    // Примеры: "^\\d{0,2}$" - только цифры, макс 2 символа; "^[А-Яа-я\\s]*$" - только русские буквы
    public static readonly StyledProperty<string?> InputPatternProperty =
        Avalonia.AvaloniaProperty.Register<AutoCompleteBoxValidationBehavior, string?>(
            nameof(InputPattern),
            defaultValue: null);

    public string? InputPattern
    {
        get => GetValue(InputPatternProperty);
        set => SetValue(InputPatternProperty, value);
    }

    // Запрещённое значение (например, "41" для кодов операции)
    // Код является валидным и присутствует в списке, но при ручном вводе или выборе из списка
    // возвращается предыдущее значение и показывается модальное сообщение
    public static readonly StyledProperty<string?> ProhibitedValueProperty =
        AvaloniaProperty.Register<AutoCompleteBoxValidationBehavior, string?>(
            nameof(ProhibitedValue));

    public string? ProhibitedValue
    {
        get => GetValue(ProhibitedValueProperty);
        set => SetValue(ProhibitedValueProperty, value);
    }

    // Сообщение при попытке ввести запрещённое значение
    public static readonly StyledProperty<string?> ProhibitedMessageProperty =
        AvaloniaProperty.Register<AutoCompleteBoxValidationBehavior, string?>(
            nameof(ProhibitedMessage));

    public string? ProhibitedMessage
    {
        get => GetValue(ProhibitedMessageProperty);
        set => SetValue(ProhibitedMessageProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject != null)
        {
            AssociatedObject.GotFocus += OnGotFocus;
            AssociatedObject.LostFocus += OnLostFocus;
            AssociatedObject.TextChanged += OnTextChanged;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.GotFocus -= OnGotFocus;
            AssociatedObject.LostFocus -= OnLostFocus;
            AssociatedObject.TextChanged -= OnTextChanged;
        }

        base.OnDetaching();
    }

    private void OnTextChanged(object? sender, EventArgs e)
    {
        // При ручном вводе текста сбрасываем флаг выбора из dropdown
        // Это гарантирует, что валидация будет выполнена при потере фокуса
        _valueSelectedFromDropDown = false;

        // Применяем маску ввода (максимальная длина и разрешённые символы)
        ApplyInputMask();
    }

    private void ApplyInputMask()
    {
        if (AssociatedObject == null) return;

        var text = AssociatedObject.Text ?? string.Empty;
        var originalText = text;

        // Применяем regex pattern если задан
        if (!string.IsNullOrEmpty(InputPattern))
        {
            // Если текст не соответствует pattern, откатываем к последнему валидному
            if (!Regex.IsMatch(text, InputPattern))
            {
                // Пытаемся найти максимальный валидный префикс
                var validPrefix = "";
                for (var i = 1; i <= text.Length; i++)
                {
                    var prefix = text[..i];
                    if (Regex.IsMatch(prefix, InputPattern))
                    {
                        validPrefix = prefix;
                    }
                    else
                    {
                        break;
                    }
                }
                text = validPrefix;
            }
        }

        // Обрезаем по максимальной длине (если задана отдельно или не ограничена regex)
        if (MaxInputLength > 0 && text.Length > MaxInputLength)
        {
            text = text[..MaxInputLength];
        }

        // Если текст изменился, обновляем
        if (text != originalText)
        {
            AssociatedObject.Text = text;
        }
    }

    private void OnGotFocus(object? sender, EventArgs e)
    {
        // Если фокус возвращается после выбора из dropdown, не обновляем _originalValue,
        // иначе запрещённое значение станет «оригинальным» и проверка не сработает
        if (_skipOriginalValueUpdate)
        {
            _skipOriginalValueUpdate = false;
        }
        else
        {
            _originalValue = AssociatedObject?.Text;
        }
        _valueSelectedFromDropDown = false;
    }

    private void OnLostFocus(object? sender, EventArgs e)
    {
        if (AssociatedObject == null)
            return;

        string currentValue = AssociatedObject.Text ?? string.Empty;

        // Если dropdown открыт, не проверяем (пользователь кликнул на список)
        if (AssociatedObject.IsDropDownOpen)
            return;

        // Если значение не изменилось, не проверяем
        if (currentValue == _originalValue)
            return;

        // Проверяем запрещённое значение (например, код 41) - ВСЕГДА,
        // даже если выбрано из выпадающего списка
        if (!string.IsNullOrEmpty(ProhibitedValue) && currentValue == ProhibitedValue)
        {
            // Возвращаем предыдущее валидное значение
            AssociatedObject.Text = _originalValue;

            // Показываем модальное сообщение
            if (!string.IsNullOrEmpty(ProhibitedMessage))
            {
                ShowProhibitedMessage();
            }
            return;
        }

        // Если значение было выбрано из списка и не является запрещённым, не проверяем дальше
        if (_valueSelectedFromDropDown)
            return;

        // Проверяем, есть ли значение в списке допустимых (для ручного ввода)
        if (!IsValidValue(currentValue))
        {
            // Возвращаем старое значение
            AssociatedObject.Text = _originalValue;
        }
    }

    private void ShowProhibitedMessage()
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams()
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Ошибка ввода",
                    ContentHeader = "Недопустимое значение",
                    ContentMessage = ProhibitedMessage,
                    MinWidth = 450,
                    MinHeight = 170,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(AssociatedObject?.GetVisualRoot() as Window)
                .ConfigureAwait(false);
        });
    }

    private bool IsValidValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        // Сначала проверяем строковые коды (если заданы)
        if (ValidCodes != null && ValidCodes.Any(c => c != null && c == value))
            return true;

        // Затем проверяем числовые значения (только если ValidCodes не задан - защита от проваливания)
        if (ValidCodes == null && ValidValues != null && short.TryParse(value, out var numericValue))
        {
            return ValidValues.Contains(numericValue);
        }

        return false;
    }

    // Метод для пометки, что значение было выбрано из списка
    public void MarkValueSelectedFromDropDown()
    {
        _valueSelectedFromDropDown = true;
        _skipOriginalValueUpdate = true;
    }
}
