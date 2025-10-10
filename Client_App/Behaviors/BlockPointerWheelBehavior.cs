using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Behaviors
{
    public class ScrollBlock
    {
        public static readonly AttachedProperty<bool> BlockPointerWheelProperty =
            AvaloniaProperty.RegisterAttached<ScrollBlock, Control, bool>(
                "BlockPointerWheel",
                defaultValue: false,
                inherits: false);

        public static bool GetBlockPointerWheel(Control element)
        {
            return element.GetValue(BlockPointerWheelProperty);
        }

        public static void SetBlockPointerWheel(Control element, bool value)
        {
            element.SetValue(BlockPointerWheelProperty, value);
        }

        static ScrollBlock()
        {
            BlockPointerWheelProperty.Changed.AddClassHandler<Control>((control, e) =>
            {
                if (e.NewValue is bool block && block)
                {
                    var behavior = new BlockPointerWheelBehavior();
                    Interaction.GetBehaviors(control).Add(behavior);
                }
                else
                {
                    var behaviors = Interaction.GetBehaviors(control);
                    var existingBehavior = behaviors.OfType<BlockPointerWheelBehavior>().FirstOrDefault();
                    if (existingBehavior != null)
                    {
                        behaviors.Remove(existingBehavior);
                    }
                }
            });
        }
    }
    public class BlockPointerWheelBehavior : Behavior<Control>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddHandler(
                InputElement.PointerWheelChangedEvent,
                OnPointerWheel,
                RoutingStrategies.Tunnel | RoutingStrategies.Bubble,
                handledEventsToo: true);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.RemoveHandler(InputElement.PointerWheelChangedEvent, OnPointerWheel);
        }

        private void OnPointerWheel(object sender, PointerWheelEventArgs e)
        {
            e.Handled = true;
        }
    }
}
