using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Behaviors
{
    public class ChainingScrollViewerBehavior : Behavior<ScrollViewer>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
            {
                // Подписываемся на события
                AssociatedObject.AddHandler(
               InputElement.PointerWheelChangedEvent,
               AssociatedObject_PointerWheelChanged,
               RoutingStrategies.Tunnel);
            }
        }

        

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.RemoveHandler(InputElement.PointerWheelChangedEvent, AssociatedObject_PointerWheelChanged);
            }

            base.OnDetaching();
        }
        private void AssociatedObject_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            if (AssociatedObject == null) return;

            var parentScrollViewer = FindVisualParent<ScrollViewer>((Visual)AssociatedObject.GetVisualParent());
            if (parentScrollViewer == null) return;

            AssociatedObject.InvalidateMeasure();
            AssociatedObject.InvalidateArrange();

            var offset = AssociatedObject.Offset;
            var extent = AssociatedObject.Extent;
            var viewport = AssociatedObject.Viewport;

            var maxOffsetX = extent.Width - viewport.Width;
            var maxOffsetY = extent.Height - viewport.Height;

            if ((e.Delta.X < 0 && offset.X == maxOffsetX)
                || (e.Delta.X > 0 && offset.X == 0)
                || (e.Delta.Y < 0 && offset.Y == maxOffsetY)
                || (e.Delta.Y > 0 && offset.Y == 0))
            {
                parentScrollViewer.Offset -= e.Delta * 30;
            }
        }

        public static T FindVisualParent<T>(Visual visual) where T : Visual
        {
            while (visual != null && !(visual is T))
            {
                visual = (Visual)visual.GetVisualParent();
            }
            return visual as T;
        }
    }
}
