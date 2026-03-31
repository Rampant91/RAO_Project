using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client_App.Behaviors
{
    public class TextBoxDateOnlyLostFocusBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
            {
                AssociatedObject.LostFocus += TextBox_LostFocus;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {

                AssociatedObject.LostFocus -= TextBox_LostFocus;
            }

            base.OnDetaching();
        }

        private void TextBox_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
                if (!DateOnly.TryParse(textBox.Text, out var dateOnly))
                {
                    textBox.Text = DateOnly.MinValue.ToString("dd.MM.yyyy");
                }
                else
                {
                    // 1/1/2001 - такая запись будет проходить,
                    //  но корректней будет записать, как 01.01.2001
                    textBox.Text = dateOnly.ToString("dd.MM.yyyy"); 
                }
        }


    }
}
