using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using Client_App.ViewModels.Forms.Forms1.Items;
using Client_App.ViewModels.Forms.Forms1.Providers.AutoCompleteProviders;
using Models.Forms;
using System;

namespace Client_App.Behaviors.Input
{
    public class AutoCompleteBoxProviderBehavior : Behavior<AutoCompleteBox>
    {
        public static readonly StyledProperty<IAutoCompleteProvider> ProviderProperty =
            AvaloniaProperty.Register<AutoCompleteBoxProviderBehavior, IAutoCompleteProvider>(
                nameof(Provider));

        public object Provider
        {
            get => GetValue(ProviderProperty);
            set => SetValue(ProviderProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.DropDownClosed += OnDropDownClosed;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.DropDownClosed -= OnDropDownClosed;
            }
            base.OnDetaching();
        }

        private void OnDropDownClosed(object? sender, EventArgs e)
        {
            var box = AssociatedObject;
            if (box == null) return;

            var item = box.SelectedItem;
            if (item is null) return;

            // Если выбранный элемент не null – значит пользователь что-то выбрал
            if (Provider is IAutoCompleteProvider provider
                && box.DataContext is Form form)
            {
                provider.WriteItemInForm(form, item);
            }
        }
    }
}