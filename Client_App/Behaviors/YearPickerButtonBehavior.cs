using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;
using System.Collections;
using System.Reactive.Linq;

namespace Client_App.Behaviors
{
    public class YearPickerButtonBehavior : AvaloniaObject
    {
        
        public static readonly AttachedProperty<bool> IsEnabledProperty =
            AvaloniaProperty.RegisterAttached<YearPickerButtonBehavior, Button, bool>(
                "IsEnabled", default, false, BindingMode.OneTime);

        public static readonly AttachedProperty<IEnumerable> ItemsSourceProperty =
            AvaloniaProperty.RegisterAttached<YearPickerButtonBehavior, Button, IEnumerable>(
                "ItemsSource", default, false, BindingMode.OneWay);

        public static readonly AttachedProperty<int> SelectedYearProperty =
            AvaloniaProperty.RegisterAttached<YearPickerButtonBehavior, Button, int>(
                "SelectedYear", default, false, BindingMode.TwoWay);

        static YearPickerButtonBehavior()
        {
            IsEnabledProperty.Changed.Subscribe(args =>
            {
                if (args.Sender is Button button)
                {
                    if (args.NewValue.Value)
                    {
                        button.Click += OnButtonClick;
                    }
                    else
                    {
                        button.Click -= OnButtonClick;
                    }
                }
            });
        }

        private static void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var popup = new Popup
                {
                    PlacementTarget = button,
                    Width = 400,
                    Height = 600,
                    PlacementMode = PlacementMode.Right,
                    IsLightDismissEnabled = true
                };
                var border = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = Brushes.LightGray,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(4),
                    Padding = new Thickness(10)
                };
                var stackPanel = new StackPanel();

                // Year selector
                var yearPicker = new NumericUpDown
                {
                    Minimum = 1900,
                    Maximum = DateTime.Now.Year,
                    Value = GetSelectedYear(button),
                    Margin = new Thickness(10)
                };

                // ListView for items
                var listView = new ListBox
                {
                    Items = GetItemsSource(button),
                    Margin = new Thickness(10),
                    Height = 300
                };

                // Update list when year changes
                yearPicker.ValueChanged += (s, args) =>
                {
                    SetSelectedYear(button, (int)args.NewValue);
                    // Here you would typically filter your data based on the selected year
                };

                stackPanel.Children.Add(yearPicker);
                stackPanel.Children.Add(listView);

                border.Child = stackPanel;
                popup.Child = border;
                popup.IsOpen = true;

                // Close popup when clicking outside
                popup.Closed += (s, args) => popup.IsOpen = false;
            }
        }

        public static void SetIsEnabled(Button element, bool value) => element.SetValue(IsEnabledProperty, value);
        public static bool GetIsEnabled(Button element) => element.GetValue(IsEnabledProperty);

        public static void SetItemsSource(Button element, IEnumerable value) => element.SetValue(ItemsSourceProperty, value);
        public static IEnumerable GetItemsSource(Button element) => element.GetValue(ItemsSourceProperty);

        public static void SetSelectedYear(Button element, int value) => element.SetValue(SelectedYearProperty, value);
        public static int GetSelectedYear(Button element) => element.GetValue(SelectedYearProperty);
    }
}
