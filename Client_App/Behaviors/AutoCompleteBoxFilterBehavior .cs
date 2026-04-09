using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Behaviors
{
    public class AutoCompleteBoxFilterBehavior  : Behavior<AutoCompleteBox>
    {
        public static readonly StyledProperty<AutoCompleteFilterPredicate<object>> FilterProperty =
         AvaloniaProperty.Register<AutoCompleteBoxFilterBehavior, AutoCompleteFilterPredicate<object>>(nameof(Filter));

        public AutoCompleteFilterPredicate<object> Filter
        {
            get => GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ItemFilter = Filter; // Прямое присваивание, типы совпадают
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.ItemFilter = null;
        }
    }
}
