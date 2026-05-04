using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Forms.Forms1.Items;
using Client_App.ViewModels.Forms.Forms1.Providers;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;

namespace Client_App.Controls;

public partial class RadionuclidsSelector : UserControl
{
    public static readonly DirectProperty<RadionuclidsSelector, string> TextProperty =
        AvaloniaProperty.RegisterDirect<RadionuclidsSelector, string>(
            nameof(Text),
            o => o.Text,
            (o, v) => o.Text = v,
            defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    private string _text = "";
    public string Text
    {
        get => _text;
        set
        {
            if (SetAndRaise(TextProperty, ref _text, value))
            {
                // Проверяем валидацию при изменении текста
                ValidateAndShowError();
            }
        }
    }

    private void ValidateAndShowError()
    {
        if (ErrorIndicator == null) return;

        var hasErrors = HasValidationErrors(Text);
        ErrorIndicator.IsVisible = hasErrors;
    }

    private bool HasValidationErrors(string? text)
    {
        if (string.IsNullOrEmpty(text)) return false;

        var parts = text.Split(';')
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrEmpty(p))
            .ToList();

        foreach (var part in parts)
        {
            // "-" допустим
            if (part == "-") continue;

            // Проверяем, есть ли в справочнике
            if (!RadionuclidsProvider.AllRadionuclids.Any(r =>
                r.Name.Equals(part, StringComparison.OrdinalIgnoreCase)))
            {
                return true; // Найден невалидный нуклид
            }
        }

        return false;
    }

    public RadionuclidsSelector()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        AddFilter = this.FindControl<TextBox>("AddFilter");
        AddList = this.FindControl<ListBox>("AddList");
        AddPopup = this.FindControl<Popup>("AddPopup");
        RemoveList = this.FindControl<ListBox>("RemoveList");
        RemovePopup = this.FindControl<Popup>("RemovePopup");
        ErrorIndicator = this.FindControl<Border>("ErrorIndicator");

        // Подписываемся на изменения текста в основном TextBox для валидации
        var textBox = this.FindControl<TextBox>("RadionuclidsTextBox");
        if (textBox != null)
        {
            textBox.GetObservable(TextBox.TextProperty)
                .Subscribe(_ => ValidateAndShowError());
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
    }

    private TextBox? AddFilter;
    private ListBox? AddList;
    private Popup? AddPopup;
    private ListBox? RemoveList;
    private Popup? RemovePopup;
    private Border? ErrorIndicator;

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

        AddList.Items = new ObservableCollection<RadionuclidItem>(availableItems);
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

        AddList.Items = new ObservableCollection<RadionuclidItem>(filtered);
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

        RemoveList.Items = allItems;
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

            // Если нет в справочнике - добавляем в список невалидных
            if (!RadionuclidsProvider.AllRadionuclids.Any(r =>
                r.Name.Equals(part, StringComparison.OrdinalIgnoreCase)))
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
