using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Behaviors.Controls;

/// <summary>
/// Кнопка открытия выпадающего списка у соседнего <see cref="AutoCompleteBox"/>.
/// Два режима — по тому, допускает ли ячейка значение вне справочника:
/// <list type="bullet">
/// <item>
/// <b>List-only</b> (код операции и т.п.): чтобы показать все пункты, временно очищаем Text
/// (сброс фильтра); при закрытии без выбора восстанавливаем исходный текст.
/// </item>
/// <item>
/// <b>Free-text</b> (класс <c>OksmGuide</c>: ОКСМ + ручной ОКПО / «прим.»): Text не трогаем;
/// на время открытия ставим <see cref="AutoCompleteFilterMode.None"/>, иначе Contains
/// по тексту вне справочника даёт пустой _view.
/// </item>
/// </list>
/// </summary>
public class DropDownButtonBehavior : Behavior<Button>
{
    /// <summary>
    /// Маркер free-text ячеек в XAML (<c>Classes="OksmGuide"</c>).
    /// Сейчас используется для справочника ОКСМ, где в ячейке легитимны значения не из списка.
    /// </summary>
    private const string FreeTextGuideClass = "OksmGuide";

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject != null)
        {
            AssociatedObject.Click += OnButtonClick;
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        if (AssociatedObject != null)
        {
            AssociatedObject.Click -= OnButtonClick;
        }
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        // Button внутри UserControl: Button -> UserControl -> Grid -> AutoCompleteBox
        var parent = AssociatedObject.Parent;
        if (parent?.Parent is Grid grid && grid.Children[0] is AutoCompleteBox autoCompleteBox)
        {
            autoCompleteBox.Focus();
            ShowDropdown(autoCompleteBox);
        }
        // Без UserControl: Button -> Grid -> AutoCompleteBox
        else if (AssociatedObject.Parent is Grid directGrid && directGrid.Children[0] is AutoCompleteBox directAutoCompleteBox)
        {
            directAutoCompleteBox.Focus();
            ShowDropdown(directAutoCompleteBox);
        }
    }

    private static void ShowDropdown(AutoCompleteBox autoCompleteBox)
    {
        if (AllowsFreeTextValues(autoCompleteBox))
        {
            ShowDropdownPreservingText(autoCompleteBox);
            return;
        }

        ShowDropdownClearingFilter(autoCompleteBox);
    }

    /// <summary>
    /// Ячейка допускает значение не из Items (free-text + справочник).
    /// </summary>
    private static bool AllowsFreeTextValues(AutoCompleteBox autoCompleteBox) =>
        autoCompleteBox.Classes.Contains(FreeTextGuideClass);

    /// <summary>
    /// Free-text: показать весь справочник, не меняя текст ячейки.
    /// Порядок:
    /// 1) FilterMode = None — иначе Contains по ОКПО/«прим.» оставляет _view пустым;
    /// 2) подписка на DropDownClosed с флагом acceptClose — Populate при повторном клике
    ///    синхронно шлёт Closed; рано вернуть Contains нельзя, иначе список снова пустой;
    /// 3) PopulateDropDown → OpeningDropDown;
    /// 4) повторный Populate на случай синхронного закрытия на шаге 3;
    /// 5) при необходимости ForceOpenDropDown;
    /// 6) если открылось — acceptClose = true (фильтр вернётся при реальном закрытии),
    ///    иначе сразу откатить FilterMode.
    /// </summary>
    private static void ShowDropdownPreservingText(AutoCompleteBox autoCompleteBox)
    {
        if (autoCompleteBox.IsDropDownOpen) return;

        var previousFilterMode = autoCompleteBox.FilterMode;
        autoCompleteBox.FilterMode = AutoCompleteFilterMode.None;

        var acceptClose = false;
        EventHandler? closedHandler = null;
        closedHandler = (_, _) =>
        {
            if (!acceptClose) return;
            autoCompleteBox.DropDownClosed -= closedHandler;
            autoCompleteBox.FilterMode = previousFilterMode;
        };
        autoCompleteBox.DropDownClosed += closedHandler;

        var populateDropDown = typeof(AutoCompleteBox).GetMethod(
            "PopulateDropDown",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        populateDropDown?.Invoke(autoCompleteBox, [autoCompleteBox, EventArgs.Empty]);

        typeof(AutoCompleteBox).GetMethod(
                "OpeningDropDown",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(autoCompleteBox, [false]);

        // После возможного синхронного Closed внутри Populate снова снимаем фильтр и наполняем.
        if (autoCompleteBox.FilterMode != AutoCompleteFilterMode.None)
            autoCompleteBox.FilterMode = AutoCompleteFilterMode.None;
        populateDropDown?.Invoke(autoCompleteBox, [autoCompleteBox, EventArgs.Empty]);

        if (!autoCompleteBox.IsDropDownOpen)
            ForceOpenDropDown(autoCompleteBox);

        if (autoCompleteBox.IsDropDownOpen)
        {
            acceptClose = true;
        }
        else
        {
            autoCompleteBox.DropDownClosed -= closedHandler;
            autoCompleteBox.FilterMode = previousFilterMode;
        }
    }

    /// <summary>
    /// List-only: временно очищаем Text, чтобы сбросить фильтр и показать все пункты.
    /// Порядок: сохранить Text → очистить → Populate → OpeningDropDown →
    /// при необходимости ForceOpen → на DropDownClosed вернуть текст, если ничего не выбрали.
    /// </summary>
    private static void ShowDropdownClearingFilter(AutoCompleteBox autoCompleteBox)
    {
        if (autoCompleteBox.IsDropDownOpen) return;

        var originalText = autoCompleteBox.Text;
        autoCompleteBox.Text = string.Empty;

        typeof(AutoCompleteBox).GetMethod(
                "PopulateDropDown",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(autoCompleteBox, [autoCompleteBox, EventArgs.Empty]);

        typeof(AutoCompleteBox).GetMethod(
                "OpeningDropDown",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(autoCompleteBox, [false]);

        if (autoCompleteBox.IsDropDownOpen) return;

        ForceOpenDropDown(autoCompleteBox);

        EventHandler? closedHandler = null;
        closedHandler = (_, _) =>
        {
            autoCompleteBox.DropDownClosed -= closedHandler;
            if (string.IsNullOrEmpty(autoCompleteBox.Text) && !string.IsNullOrEmpty(originalText))
            {
                autoCompleteBox.Text = originalText;
            }
        };
        autoCompleteBox.DropDownClosed += closedHandler;
    }

    /// <summary>
    /// Открыть список через свойство, подавив PropertyChanged:
    /// иначе OnIsDropDownOpenChanged снова зовёт TextUpdated и может сразу закрыть popup.
    /// </summary>
    private static void ForceOpenDropDown(AutoCompleteBox autoCompleteBox)
    {
        var ipc = typeof(AutoCompleteBox).GetField(
            "_ignorePropertyChange",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if ((bool)ipc?.GetValue(autoCompleteBox)! == false)
            ipc?.SetValue(autoCompleteBox, true);

        autoCompleteBox.SetValue(AutoCompleteBox.IsDropDownOpenProperty, true);
    }
}
