using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Client_App.ViewModels.Forms.Forms1.Items;
using Client_App.ViewModels.Forms.Forms1.Providers;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Controls;

public partial class RadionuclidsSelector : UserControl
{
    public static readonly DirectProperty<RadionuclidsSelector, string> TextProperty =
        AvaloniaProperty.RegisterDirect<RadionuclidsSelector, string>(
            nameof(Text),
            o => o.Text,
            (o, v) => o.Text = v,
            defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public static readonly StyledProperty<bool> AllowEmptyProperty =
        AvaloniaProperty.Register<RadionuclidsSelector, bool>(nameof(AllowEmpty), false);

    public static readonly StyledProperty<bool> SingleRadionuclideModeProperty =
        AvaloniaProperty.Register<RadionuclidsSelector, bool>(nameof(SingleRadionuclideMode), false);

    private string _text = "";
    private CancellationTokenSource? _debounceCts;
    private string? _lastValidatedText;

    public bool AllowEmpty
    {
        get => GetValue(AllowEmptyProperty);
        set => SetValue(AllowEmptyProperty, value);
    }

    public bool SingleRadionuclideMode
    {
        get => GetValue(SingleRadionuclideModeProperty);
        set => SetValue(SingleRadionuclideModeProperty, value);
    }

    public string Text
    {
        get => _text;
        set
        {
            if (SetAndRaise(TextProperty, ref _text, value))
            {
                // Скрываем индикатор до завершения валидации
                if (ErrorIndicator != null)
                    ErrorIndicator.IsVisible = false;

                // Обновляем состояние кнопки Add
                UpdateAddButtonState();

                // Проверяем валидацию с задержкой при изменении текста
                ValidateAndShowErrorDebounced();
            }
        }
    }

    private void ValidateAndShowErrorDebounced()
    {
        // Не гоняем таймер при recycle строки DataGrid, если текст тот же
        if (string.Equals(Text, _lastValidatedText, StringComparison.Ordinal))
            return;

        // Отменяем предыдущий таймер
        _debounceCts?.Cancel();
        _debounceCts = new CancellationTokenSource();

        // Задержка 300мс перед валидацией
        Task.Delay(300, _debounceCts.Token)
            .ContinueWith(t =>
            {
                if (!t.IsCanceled)
                {
                    Dispatcher.UIThread.InvokeAsync(ValidateAndShowError);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void ValidateAndShowError()
    {
        if (ErrorIndicator == null) return;

        var hasErrors = HasValidationErrors(Text);
        ErrorIndicator.IsVisible = hasErrors;
        _lastValidatedText = Text;
    }

    private bool HasValidationErrors(string? text)
    {
        // Пустая ячейка - проверяем настройку AllowEmpty
        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            return !AllowEmpty;

        var parts = text.Split(';')
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrEmpty(p))
            .ToList();

        // В режиме SingleRadionuclideMode допустим только 1 нуклид
        if (SingleRadionuclideMode && parts.Count > 1)
            return true;

        // Если есть несколько частей и одна из них "-", это ошибка
        // "-" может быть только единственным элементом
        if (parts.Count > 1 && parts.Contains("-"))
            return true;

        foreach (var part in parts)
        {
            // "-" допустим только если это единственный элемент
            if (part == "-") continue;

            if (!RadionuclidsProvider.IsKnownRadionuclid(part))
                return true;
        }

        return false;
    }

    private void UpdateAddButtonState()
    {
        if (AddBtn == null) return;

        // В режиме SingleRadionuclideMode кнопка Add неактивна если уже есть нуклид
        if (SingleRadionuclideMode)
        {
            var parts = Text?.Split(';')
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
                .ToList() ?? [];
            AddBtn.IsEnabled = parts.Count == 0;
        }
        else
        {
            AddBtn.IsEnabled = true;
        }
    }

    public RadionuclidsSelector()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        // Подписываемся на изменения текста в основном TextBox для валидации с задержкой
        var textBox = this.FindControl<TextBox>("RadionuclidsTextBox");
        if (textBox != null)
        {
            textBox.GetObservable(TextBox.TextProperty)
                .Subscribe(text =>
                {
                    if (string.Equals(text, _lastValidatedText, StringComparison.Ordinal))
                        return;
                    ValidateAndShowErrorDebounced();
                });
        }

        // Подписываемся на изменения текста фильтра (Avalonia 0.10 не имеет события TextChanged)
        AddFilter?.GetObservable(TextBox.TextProperty)
            .Subscribe(AddFilter_TextChanged);

        // Подписываемся на изменение выделения в списках
        AddList?.GetObservable(ListBox.SelectedItemProperty)
            .Subscribe(AddList_SelectionChanged);
        RemoveList?.GetObservable(ListBox.SelectedItemProperty)
            .Subscribe(RemoveList_SelectionChanged);

        // Проверяем валидацию при инициализации
        ValidateAndShowError();

        // Обновляем состояние кнопки Add
        UpdateAddButtonState();

        // Подписываемся на изменение свойств режима
        this.GetObservable(AllowEmptyProperty).Subscribe(_ =>
        {
            _lastValidatedText = null;
            ValidateAndShowErrorDebounced();
        });
        this.GetObservable(SingleRadionuclideModeProperty).Subscribe(_ =>
        {
            UpdateAddButtonState();
            _lastValidatedText = null;
            ValidateAndShowErrorDebounced();
        });
    }

    private void AddBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (AddPopup == null || AddList == null) return;

        if (AddPopup.IsOpen)
        {
            AddPopup.IsOpen = false;
            return;
        }

        var availableItems = RadionuclidsProvider.GetAvailableRadionuclids(Text);
        if (availableItems.Count == 0) return;

        AddList.ItemsSource = new ObservableCollection<RadionuclidItem>(availableItems);
        AddFilter!.Text = "";
        AddPopup.IsOpen = true;
        AddFilter.Focus();
    }

    private void AddFilter_TextChanged(string? filter)
    {
        if (AddList == null) return;

        var f = (filter ?? "").ToLower().Trim();
        var availableItems = RadionuclidsProvider.GetAvailableRadionuclids(Text);

        var filtered = string.IsNullOrEmpty(f)
            ? availableItems
            : availableItems.Where(r => r.Name.ToLower().Contains(f)).ToList();

        AddList.ItemsSource = new ObservableCollection<RadionuclidItem>(filtered);
    }

    private void AddList_SelectionChanged(object? selectedItemObj)
    {
        if (selectedItemObj is not RadionuclidItem selectedItem) return;
        if (AddPopup?.IsOpen != true) return; // Игнорируем если popup закрыт

        Text = RadionuclidsProvider.AddRadionuclid(Text, selectedItem.Name);
        AddPopup.IsOpen = false;

        // Обновляем индикатор ошибки после добавления
        ValidateAndShowError();
    }

    private void RemoveBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (RemovePopup == null || RemoveList == null) return;

        if (RemovePopup.IsOpen)
        {
            RemovePopup.IsOpen = false;
            return;
        }

        // Получаем валидные радионуклиды
        var currentItems = RadionuclidsProvider.GetCurrentRadionuclids(Text);

        // Добавляем невалидные записи (которых нет в справочнике)
        var invalidItems = GetInvalidEntries(Text);
        var allItems = new ObservableCollection<RadionuclidItem>(currentItems);
        foreach (var invalid in invalidItems)
        {
            allItems.Add(new RadionuclidItem { Name = invalid });
        }

        if (allItems.Count == 0) return;

        RemoveList.ItemsSource = allItems;
        RemovePopup.IsOpen = true;
    }

    private List<string> GetInvalidEntries(string? text)
    {
        if (string.IsNullOrEmpty(text)) return [];

        var parts = text.Split(';')
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrEmpty(p))
            .ToList();

        var invalid = new List<string>();
        foreach (var part in parts)
        {
            // "-" считается валидным
            if (part == "-") continue;

            if (!RadionuclidsProvider.IsKnownRadionuclid(part))
            {
                invalid.Add(part);
            }
        }

        return invalid;
    }

    private void RemoveList_SelectionChanged(object? selectedItemObj)
    {
        if (selectedItemObj is not RadionuclidItem selectedItem) return;
        if (RemovePopup?.IsOpen != true) return; // Игнорируем если popup закрыт

        Text = RadionuclidsProvider.RemoveRadionuclid(Text, selectedItem.Name);
        RemovePopup.IsOpen = false;

        // Обновляем индикатор ошибки после удаления
        ValidateAndShowError();
    }
}
