using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace Client_App.Behaviors;

/// <summary>
/// Подсказки пунктов меню: Placement=Top (не пересекается с cascade справа),
/// длинный текст переносится, tip закрывается при открытии подменю.
/// </summary>
public static class MenuItemToolTipHelper
{
    private const double TipMaxWidth = 320;

    public static readonly AttachedProperty<bool> CloseTipWhenSubmenuOpenProperty =
        AvaloniaProperty.RegisterAttached<MenuItem, bool>(
            "CloseTipWhenSubmenuOpen",
            typeof(MenuItemToolTipHelper),
            defaultValue: false);

    static MenuItemToolTipHelper()
    {
        ToolTip.TipProperty.Changed.AddClassHandler<MenuItem>(OnTipChanged);
        CloseTipWhenSubmenuOpenProperty.Changed.AddClassHandler<MenuItem>(OnCloseTipWhenSubmenuOpenChanged);
    }

    public static bool GetCloseTipWhenSubmenuOpen(MenuItem element) =>
        element.GetValue(CloseTipWhenSubmenuOpenProperty);

    public static void SetCloseTipWhenSubmenuOpen(MenuItem element, bool value) =>
        element.SetValue(CloseTipWhenSubmenuOpenProperty, value);

    private static void OnTipChanged(MenuItem item, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is null)
        {
            return;
        }

        // Строка → TextBlock с переносом (иначе Avalonia рисует одну линию и обрезает).
        if (e.NewValue is string text)
        {
            ToolTip.SetTip(item, CreateWrappingTip(text));
            return;
        }

        ApplyMenuTipPresentation(item);
    }

    private static void OnCloseTipWhenSubmenuOpenChanged(MenuItem item, AvaloniaPropertyChangedEventArgs e)
    {
        item.PropertyChanged -= OnMenuItemPropertyChanged;
        item.RemoveHandler(InputElement.PointerEnterEvent, OnPointerEntered);

        if (e.NewValue is true)
        {
            EnsureWrappingTip(item);
            ApplyMenuTipPresentation(item);
            item.PropertyChanged += OnMenuItemPropertyChanged;
            item.AddHandler(InputElement.PointerEnterEvent, OnPointerEntered);
            if (item.IsSubMenuOpen)
            {
                ToolTip.SetIsOpen(item, false);
            }
        }
    }

    private static void OnPointerEntered(object? sender, PointerEventArgs e)
    {
        if (sender is MenuItem item)
        {
            EnsureWrappingTip(item);
            ApplyMenuTipPresentation(item);
        }
    }

    private static void OnMenuItemPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (sender is not MenuItem item || e.Property != MenuItem.IsSubMenuOpenProperty)
        {
            return;
        }

        if (item.IsSubMenuOpen)
        {
            ToolTip.SetIsOpen(item, false);
        }
    }

    private static void EnsureWrappingTip(MenuItem item)
    {
        if (ToolTip.GetTip(item) is string text)
        {
            ToolTip.SetTip(item, CreateWrappingTip(text));
        }
    }

    private static TextBlock CreateWrappingTip(string text) =>
        new()
        {
            Text = text,
            TextWrapping = TextWrapping.Wrap,
            TextAlignment = TextAlignment.Left,
            MaxWidth = TipMaxWidth,
            FontSize = 13,
            Foreground = Brush.Parse("#2F2F2F"),
            LineHeight = 18
        };

    private static void ApplyMenuTipPresentation(MenuItem item)
    {
        if (ToolTip.GetTip(item) is null)
        {
            return;
        }

        // Top: не пересекается с cascade справа. Left у края экрана Avalonia часто
        // переворачивает в Right (constraint flip) — снова поверх подменю.
        ToolTip.SetPlacement(item, PlacementMode.Top);
        ToolTip.SetHorizontalOffset(item, 0);
        ToolTip.SetVerticalOffset(item, -6);
        ToolTip.SetShowDelay(item, 700);
    }
}
