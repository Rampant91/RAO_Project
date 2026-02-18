using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Behaviors
{
    public class ScrollViewerBehavior : Behavior<ScrollViewer>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
            {
                // Подписываемся на события
                AssociatedObject.PointerWheelChanged += AssociatedObject_PointerWheelChanged;
            }
        }

        

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.PointerWheelChanged -= AssociatedObject_PointerWheelChanged;
            }

            base.OnDetaching();
        }
        private void AssociatedObject_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            if (AssociatedObject == null) return;

            if (e.Delta.X < 0)
            {
                AssociatedObject.Background = Brushes.Green;
            }
            else if (e.Delta.X > 0)
            {
                AssociatedObject.Background = Brushes.Red;
            }
        }
    }
}
