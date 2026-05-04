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
        set => SetAndRaise(TextProperty, ref _text, value);
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

        // Подписываемся на изменения текста фильтра (Avalonia 0.10 не имеет события TextChanged)
        AddFilter?.GetObservable(TextBox.TextProperty)
            .Subscribe(AddFilter_TextChanged);

        // Подписываемся на изменение выделения в списках
        AddList?.GetObservable(ListBox.SelectedItemProperty)
            .Subscribe(AddList_SelectionChanged);
        RemoveList?.GetObservable(ListBox.SelectedItemProperty)
            .Subscribe(RemoveList_SelectionChanged);
    }

    private TextBox? AddFilter;
    private ListBox? AddList;
    private Popup? AddPopup;
    private ListBox? RemoveList;
    private Popup? RemovePopup;

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
    }

    private void RemoveBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (RemovePopup == null || RemoveList == null) return;

        if (RemovePopup.IsOpen)
        {
            RemovePopup.IsOpen = false;
            return;
        }

        var currentItems = RadionuclidsProvider.GetCurrentRadionuclids(Text);
        if (currentItems.Count == 0) return;

        RemoveList.Items = new ObservableCollection<RadionuclidItem>(currentItems);
        RemovePopup.IsOpen = true;
    }

    private void RemoveList_SelectionChanged(object? selectedItemObj)
    {
        if (selectedItemObj is not RadionuclidItem selectedItem) return;
        if (RemovePopup?.IsOpen != true) return; // Игнорируем если popup закрыт

        Text = RadionuclidsProvider.RemoveRadionuclid(Text, selectedItem.Name);
        RemovePopup.IsOpen = false;
    }
}
