using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.VisualTree;
using Client_App.ViewModels.Forms.Forms1.Items;

namespace Client_App.Behaviors.Controls;

/// <summary>
/// Для <c>AutoCompleteBox.OksmGuide</c>: липкий фильтр в попапе (код + название).
/// Фильтр без фокуса — иначе Avalonia 0.10 AutoCompleteBox закрывает список на LostFocus PART_TextBox,
/// и клик проваливается в DataGrid.
/// Закрытие: выбор пункта, Escape, клик вне попапа (light-dismiss выключен намеренно).
/// </summary>
public static class OksmGuideDropdown
{
    public static readonly AttachedProperty<bool> IsEnabledProperty =
        AvaloniaProperty.RegisterAttached<AutoCompleteBox, bool>(
            "IsEnabled",
            typeof(OksmGuideDropdown),
            defaultValue: false);

    private static readonly AttachedProperty<IDisposable> SubscriptionProperty =
        AvaloniaProperty.RegisterAttached<AutoCompleteBox, IDisposable>(
            "Subscription",
            typeof(OksmGuideDropdown));

    private static readonly AttachedProperty<GuideParts> PartsProperty =
        AvaloniaProperty.RegisterAttached<AutoCompleteBox, GuideParts>(
            "Parts",
            typeof(OksmGuideDropdown));

    static OksmGuideDropdown()
    {
        IsEnabledProperty.Changed.AddClassHandler<AutoCompleteBox>(OnIsEnabledChanged);
    }

    public static bool GetIsEnabled(AutoCompleteBox box) => box.GetValue(IsEnabledProperty);
    public static void SetIsEnabled(AutoCompleteBox box, bool value) => box.SetValue(IsEnabledProperty, value);

    private static void OnIsEnabledChanged(AutoCompleteBox box, AvaloniaPropertyChangedEventArgs e)
    {
        DisposeSubscription(box);

        if (e.NewValue is not true)
            return;

        box.TemplateApplied += OnTemplateApplied;
        box.DropDownOpened += OnDropDownOpened;
        box.DropDownClosing += OnDropDownClosing;
        box.DropDownClosed += OnDropDownClosed;
        box.DetachedFromVisualTree += OnDetached;

        box.SetValue(SubscriptionProperty, new ActionDisposable(() =>
        {
            box.TemplateApplied -= OnTemplateApplied;
            box.DropDownOpened -= OnDropDownOpened;
            box.DropDownClosing -= OnDropDownClosing;
            box.DropDownClosed -= OnDropDownClosed;
            box.DetachedFromVisualTree -= OnDetached;
            ClearParts(box);
        }));

        if (box.IsInitialized)
            TryResolveParts(box, null);
    }

    private static void DisposeSubscription(AutoCompleteBox box)
    {
        box.GetValue(SubscriptionProperty)?.Dispose();
        box.SetValue(SubscriptionProperty, null);
        ClearParts(box);
    }

    private static void OnDetached(object sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is AutoCompleteBox box)
            DisposeSubscription(box);
    }

    private static void OnTemplateApplied(object sender, TemplateAppliedEventArgs e)
    {
        if (sender is AutoCompleteBox box)
            TryResolveParts(box, e.NameScope);
    }

    private static void OnDropDownOpened(object sender, EventArgs e)
    {
        if (sender is not AutoCompleteBox box)
            return;

        var parts = TryResolveParts(box, null);
        if (parts is null)
            return;

        parts.AllowClose = false;
        parts.FilterArmed = false;
        UpdateFilterChrome(parts);

        if (parts.Popup is not null)
        {
            parts.Popup.IsLightDismissEnabled = false;
            parts.Popup.StaysOpen = true;
        }

        AttachOutsideClose(box, parts);
        ApplyFilter(box, parts);
    }

    private static void OnDropDownClosing(object sender, CancelEventArgs e)
    {
        if (sender is not AutoCompleteBox box)
            return;

        var parts = box.GetValue(PartsProperty);
        if (parts is null)
            return;

        if (parts.AllowClose)
        {
            parts.AllowClose = false;
            return;
        }

        // Только пока клик ещё внутри попапа (гонка LostFocus). FilterArmed сам по себе не блокирует закрытие.
        if (IsPointerOverPopup(parts))
            e.Cancel = true;
    }

    private static void OnDropDownClosed(object sender, EventArgs e)
    {
        if (sender is not AutoCompleteBox box)
            return;

        var parts = box.GetValue(PartsProperty);
        if (parts is null)
            return;

        DetachOutsideClose(parts);

        parts.FilterArmed = false;
        UpdateFilterChrome(parts);

        if (parts.Filter is not null && !string.IsNullOrEmpty(parts.Filter.Text))
            parts.Filter.Text = string.Empty;

        ResetListItems(box, parts);
    }

    private static void AttachOutsideClose(AutoCompleteBox box, GuideParts parts)
    {
        DetachOutsideClose(parts);

        var root = box.GetVisualRoot() as IInputElement;
        if (root is null)
            return;

        void Handler(object s, PointerPressedEventArgs e) => OnRootPointerPressed(box, parts, e);

        parts.OutsideCloseRoot = root;
        parts.OutsideCloseHandler = Handler;
        root.AddHandler(InputElement.PointerPressedEvent, Handler, RoutingStrategies.Tunnel);
    }

    private static void DetachOutsideClose(GuideParts parts)
    {
        if (parts.OutsideCloseRoot is null || parts.OutsideCloseHandler is null)
            return;

        parts.OutsideCloseRoot.RemoveHandler(InputElement.PointerPressedEvent, parts.OutsideCloseHandler);
        parts.OutsideCloseRoot = null;
        parts.OutsideCloseHandler = null;
    }

    private static void OnRootPointerPressed(AutoCompleteBox box, GuideParts parts, PointerPressedEventArgs e)
    {
        if (!box.IsDropDownOpen)
            return;

        if (e.Source is not IControl source)
            return;

        // Клик по самому справочнику или по ячейке AutoCompleteBox — не закрываем.
        if (IsInside(source, parts.PopupChild) || IsInside(source, parts.Filter) || IsInside(source, parts.List)
            || IsInside(source, box))
            return;

        CloseDropDown(box, parts);
    }

    private static void CloseDropDown(AutoCompleteBox box, GuideParts parts)
    {
        parts.AllowClose = true;
        parts.FilterArmed = false;
        UpdateFilterChrome(parts);
        box.IsDropDownOpen = false;
    }

    private static GuideParts TryResolveParts(AutoCompleteBox box, INameScope nameScope)
    {
        var existing = box.GetValue(PartsProperty);
        if (existing?.Filter is not null && existing.List is not null)
            return existing;

        TextBox filter = null;
        ListBox list = null;
        Popup popup = null;

        if (nameScope is not null)
        {
            filter = nameScope.Find<TextBox>("PART_OksmFilterBox");
            list = nameScope.Find<ListBox>("PART_SelectingItemsControl");
            popup = nameScope.Find<Popup>("PART_Popup");
        }

        filter ??= FindNamedLogicalDescendant<TextBox>(box, "PART_OksmFilterBox");
        list ??= FindNamedLogicalDescendant<ListBox>(box, "PART_SelectingItemsControl");
        popup ??= FindNamedLogicalDescendant<Popup>(box, "PART_Popup");

        if (filter is null || list is null)
            return existing;

        ClearParts(box);

        // Без фокуса: иначе LostFocus PART_TextBox закрывает dropdown и клик уходит в DataGrid.
        filter.Focusable = false;
        filter.IsHitTestVisible = true;
        filter.CaretBrush = Brushes.Transparent;

        var parts = new GuideParts
        {
            Owner = box,
            Filter = filter,
            List = list,
            Popup = popup,
            PopupChild = popup?.Child as Control
        };

        filter.AddHandler(
            InputElement.PointerPressedEvent,
            OnFilterPointerPressed,
            RoutingStrategies.Tunnel | RoutingStrategies.Bubble);

        list.AddHandler(
            InputElement.PointerPressedEvent,
            OnListPointerPressed,
            RoutingStrategies.Tunnel);

        list.SelectionChanged += OnListSelectionChanged;

        box.AddHandler(
            InputElement.KeyDownEvent,
            OnOwnerKeyDown,
            RoutingStrategies.Tunnel);

        box.AddHandler(
            InputElement.TextInputEvent,
            OnOwnerTextInput,
            RoutingStrategies.Tunnel);

        var textSub = filter.GetObservable(TextBox.TextProperty).Subscribe(_ => ApplyFilter(box, parts));
        parts.TextSubscription = textSub;

        box.SetValue(PartsProperty, parts);
        return parts;
    }

    private static void ClearParts(AutoCompleteBox box)
    {
        var parts = box.GetValue(PartsProperty);
        if (parts is null)
            return;

        DetachOutsideClose(parts);

        if (parts.Filter is not null)
            parts.Filter.RemoveHandler(InputElement.PointerPressedEvent, OnFilterPointerPressed);

        if (parts.List is not null)
        {
            parts.List.RemoveHandler(InputElement.PointerPressedEvent, OnListPointerPressed);
            parts.List.SelectionChanged -= OnListSelectionChanged;
        }

        parts.Owner?.RemoveHandler(InputElement.KeyDownEvent, OnOwnerKeyDown);
        parts.Owner?.RemoveHandler(InputElement.TextInputEvent, OnOwnerTextInput);
        parts.TextSubscription?.Dispose();
        box.SetValue(PartsProperty, null);
    }

    private static void OnFilterPointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (sender is not TextBox filter)
            return;

        var box = FindOwningAutoCompleteBox(filter);
        var parts = box?.GetValue(PartsProperty);
        if (parts is null || box is null)
            return;

        parts.AllowClose = false;
        parts.FilterArmed = true;
        UpdateFilterChrome(parts);

        // Не даём событию уйти в DataGrid / сменить ячейку.
        e.Handled = true;

        if (!box.IsDropDownOpen)
            box.IsDropDownOpen = true;

        ApplyFilter(box, parts);
    }

    private static void OnListPointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (sender is not ListBox list)
            return;

        var box = FindOwningAutoCompleteBox(list);
        var parts = box?.GetValue(PartsProperty);
        if (parts is null)
            return;

        parts.FilterArmed = false;
        UpdateFilterChrome(parts);
    }

    private static void OnListSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListBox list)
            return;

        if (e.AddedItems is null || e.AddedItems.Count == 0)
            return;

        var box = FindOwningAutoCompleteBox(list);
        var parts = box?.GetValue(PartsProperty);
        if (parts is null || box is null || !box.IsDropDownOpen)
            return;

        parts.AllowClose = true;
        parts.FilterArmed = false;
        UpdateFilterChrome(parts);
    }

    private static void OnOwnerKeyDown(object sender, KeyEventArgs e)
    {
        if (sender is not AutoCompleteBox box || !box.IsDropDownOpen)
            return;

        var parts = box.GetValue(PartsProperty);
        if (parts is null)
            return;

        if (e.Key == Key.Escape)
        {
            CloseDropDown(box, parts);
            e.Handled = true;
            return;
        }

        if (!parts.FilterArmed || parts.Filter is null)
            return;

        // Навигация по списку — оставляем AutoCompleteBox.
        if (e.Key is Key.Up or Key.Down or Key.PageUp or Key.PageDown or Key.Enter or Key.Tab)
            return;

        var text = parts.Filter.Text ?? string.Empty;

        if (e.Key == Key.Back)
        {
            if (text.Length > 0)
                parts.Filter.Text = text[..^1];
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Delete)
        {
            parts.Filter.Text = string.Empty;
            e.Handled = true;
        }
    }

    private static void OnOwnerTextInput(object sender, TextInputEventArgs e)
    {
        if (sender is not AutoCompleteBox box || !box.IsDropDownOpen)
            return;

        var parts = box.GetValue(PartsProperty);
        if (parts is null || !parts.FilterArmed || parts.Filter is null)
            return;

        if (string.IsNullOrEmpty(e.Text))
            return;

        parts.Filter.Text = (parts.Filter.Text ?? string.Empty) + e.Text;
        e.Handled = true;
    }

    private static void UpdateFilterChrome(GuideParts parts)
    {
        if (parts.Filter is null)
            return;

        parts.Filter.BorderBrush = parts.FilterArmed
            ? Brushes.DodgerBlue
            : Brushes.Gray;
        parts.Filter.BorderThickness = new Thickness(parts.FilterArmed ? 2 : 1);
    }

    private static bool IsPointerOverPopup(GuideParts parts) =>
        parts.Filter?.IsPointerOver == true
        || parts.List?.IsPointerOver == true
        || parts.PopupChild?.IsPointerOver == true;

    private static bool IsInside(IControl source, IControl ancestor)
    {
        if (ancestor is null || source is null)
            return false;

        for (IControl walk = source; walk is not null; walk = walk.Parent as IControl)
        {
            if (walk == ancestor)
                return true;
        }

        return false;
    }

    private static AutoCompleteBox FindOwningAutoCompleteBox(IControl from)
    {
        for (IControl walk = from; walk is not null; walk = walk.Parent as IControl)
        {
            if (walk is AutoCompleteBox box)
                return box;
        }

        return null;
    }

    private static T FindNamedLogicalDescendant<T>(IControl root, string name) where T : class, IControl
    {
        foreach (var child in root.GetLogicalDescendants())
        {
            if (child is T match && match.Name == name)
                return match;
        }

        return null;
    }

    private static void ApplyFilter(AutoCompleteBox box, GuideParts parts)
    {
        if (parts.List is null)
            return;

        var filterText = (parts.Filter?.Text ?? string.Empty).Trim();
        var source = EnumerateItems(box.Items).ToList();

        parts.List.Items = filterText.Length == 0
            ? source
            : source.Where(item => Matches(item, filterText)).ToList();
    }

    private static void ResetListItems(AutoCompleteBox box, GuideParts parts)
    {
        if (parts.List is null)
            return;

        parts.List.Items = EnumerateItems(box.Items).ToList();
    }

    private static bool Matches(object item, string filterText)
    {
        if (item is OksmItem oksm)
            return Contains(oksm.Code, filterText) || Contains(oksm.Country, filterText);

        return Contains(item?.ToString(), filterText);
    }

    private static bool Contains(string value, string filterText) =>
        !string.IsNullOrEmpty(value)
        && value.Contains(filterText, StringComparison.OrdinalIgnoreCase);

    private static IEnumerable<object> EnumerateItems(IEnumerable items)
    {
        if (items is null)
            yield break;

        foreach (var item in items)
        {
            if (item is not null)
                yield return item;
        }
    }

    private sealed class GuideParts
    {
        public AutoCompleteBox Owner;
        public TextBox Filter;
        public ListBox List;
        public Popup Popup;
        public Control PopupChild;
        public IDisposable TextSubscription;
        public bool AllowClose;
        public bool FilterArmed;
        public IInputElement OutsideCloseRoot;
        public EventHandler<PointerPressedEventArgs> OutsideCloseHandler;
    }

    private sealed class ActionDisposable : IDisposable
    {
        private Action _dispose;

        public ActionDisposable(Action dispose) => _dispose = dispose;

        public void Dispose()
        {
            var action = _dispose;
            _dispose = null;
            action?.Invoke();
        }
    }
}
