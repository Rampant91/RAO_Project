using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Behaviors
{
    public class TextBoxExponentionalFormatLostFocusBehavior : Behavior<TextBox>
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
            {
                var expStr = textBox.Text.Trim();

                expStr = expStr.Replace(" ", "");  //удаляем все пробелы
                expStr = expStr.Replace('.', ','); //точка на запятую
                expStr = expStr.Replace('е', 'e'); //меняем кириллицу на латиницу
                expStr = expStr.Replace('Е', 'E'); //меняем кириллицу на латиницу

                if (!double.TryParse(expStr, out var doubleValue))
                {   
                    textBox.Text = 0.ToString();
                }
                else
                {
                    var length = int.Min(6, doubleValue.ToString().Length - 1); // Максимум 6 знаков после запятой

                    textBox.Text = doubleValue.ToString($"e{length}");
                }
            }
        }


    }
}
