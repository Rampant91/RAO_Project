using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Client_App.Controls.AutoCompleteBox
{
    public class AutoCompleteBoxBehaviour : Behavior<Avalonia.Controls.AutoCompleteBox>
    {
        protected override async void OnAttached()
        {
            if (AssociatedObject is not null)
            {
                AssociatedObject.KeyUp += OnKeyUp;
                AssociatedObject.DropDownOpening += DropDownOpening;
                AssociatedObject.GotFocus += OnGotFocus;
                AssociatedObject.TextChanged += OnTextChanged;

                //await Task.Delay(500).ContinueWith(_ => Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(CreateDropdownButton));
            }
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject is not null)
            {
                AssociatedObject.KeyUp -= OnKeyUp;
                AssociatedObject.DropDownOpening -= DropDownOpening;
                AssociatedObject.GotFocus -= OnGotFocus;
            }
            base.OnDetaching();
        }

        //have to use KeyUp as AutoCompleteBox eats some of the KeyDown events
        private void OnKeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key is Avalonia.Input.Key.Down)
            {
                if (string.IsNullOrEmpty(AssociatedObject?.Text))
                {
                    ShowDropdown();
                }
            }
        }

        private void DropDownOpening(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AssociatedObject is null) return;

            var prop = AssociatedObject.GetType().GetProperty(
                "TextBox",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            var tb = (TextBox?)prop?.GetValue(AssociatedObject);
            if (tb is { IsReadOnly: true })
            {
                e.Cancel = true;
            }
        }

        private void ShowDropdown()
        {
            if (AssociatedObject is null || AssociatedObject.IsDropDownOpen) return;

            typeof(Avalonia.Controls.AutoCompleteBox).GetMethod(
                    "PopulateDropDown", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(AssociatedObject, [AssociatedObject, EventArgs.Empty]);

            typeof(Avalonia.Controls.AutoCompleteBox).GetMethod(
                    "OpeningDropDown", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(AssociatedObject, [false]);

            if (AssociatedObject.IsDropDownOpen) return;

            //We *must* set the field and not the property as we need to avoid the changed event being raised (which prevents the dropdown opening).
            var ipc = typeof(Avalonia.Controls.AutoCompleteBox).GetField(
                "_ignorePropertyChange", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if ((bool)ipc?.GetValue(AssociatedObject) == false) ipc?.SetValue(AssociatedObject, true);

            AssociatedObject.SetValue(Avalonia.Controls.AutoCompleteBox.IsDropDownOpenProperty, true);
        }

        //private void CreateDropdownButton()
        //{
        //    if (AssociatedObject == null) return;

        //    var prop = AssociatedObject.GetType().GetProperty("TextBox", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        //    var tb = (TextBox?)prop?.GetValue(AssociatedObject);
                
        //    var btn = new Button
        //    {
        //        Content = "🢗",
        //        Margin = new Thickness(3),
        //        ClickMode = ClickMode.Press
        //    };
        //    btn.Click += (_, _) => ShowDropdown();

        //    if (tb != null) tb.InnerRightContent = btn;
        //}

        private void OnGotFocus(object? sender, RoutedEventArgs e)
        {
            if (AssociatedObject is not null 
                && (string.IsNullOrWhiteSpace(AssociatedObject.Text) 
                    || AssociatedObject.Items
                        .Cast<string>()
                        .Any(x => x.Contains(AssociatedObject.Text))))
            {
                ShowDropdown();
            }
            //CreateDropdownButton();
        }

        private void OnTextChanged(object? sender, EventArgs e)
        {
            if (AssociatedObject is not null 
                && !string.IsNullOrWhiteSpace(AssociatedObject.SearchText) 
                && AssociatedObject.Text is null)
            {
                var textProp = typeof(Avalonia.Controls.AutoCompleteBox).GetField(
                    "_text",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (textProp?.GetValue(AssociatedObject) is null)
                {
                    textProp?.SetValue(AssociatedObject, AssociatedObject.SearchText);
                }
                base.OnPropertyChanged((AvaloniaPropertyChangedEventArgs<object>)e);
            }
        }
    }
}
