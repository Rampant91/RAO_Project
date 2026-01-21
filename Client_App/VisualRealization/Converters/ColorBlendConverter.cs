using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.VisualRealization.Converters
{
    // Конвертер для смешивания цветов
    public class ColorBlendConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count < 2 || values[0] == AvaloniaProperty.UnsetValue || values[1] == AvaloniaProperty.UnsetValue)
                return Brushes.Transparent;

            SolidColorBrush? baseColorBrush = null;
            if (values[0] is string str1)
            {
                Color.TryParse(str1, out var baseColor);
                baseColorBrush = new SolidColorBrush(baseColor);
            }
            else if (values[0] is System.Drawing.Color systemBaseColor)
            {
                baseColorBrush = new SolidColorBrush(new Color(systemBaseColor.A, systemBaseColor.R, systemBaseColor.G, systemBaseColor.B));
            }
            else if (values[0] is SolidColorBrush colorBrush1)
            {
                baseColorBrush = colorBrush1;
            }

            SolidColorBrush? conditionColorBrush = null; 
            if (values[1] is string str2)
            {
                Color.TryParse(str2, out var baseColor);
                conditionColorBrush = new SolidColorBrush(baseColor);
            }
            else if (values[1] is System.Drawing.Color systemConditionColor)
            {
                conditionColorBrush = new SolidColorBrush(new Color(systemConditionColor.A, systemConditionColor.R, systemConditionColor.G, systemConditionColor.B));
            }
            else if (values[1] is SolidColorBrush colorBrush2)
            {
                conditionColorBrush = colorBrush2;
            }

            if (baseColorBrush is null)
                return Brushes.Transparent;
            if (conditionColorBrush is null)
                return baseColorBrush;

            return new SolidColorBrush(BlendColors(conditionColorBrush.Color, baseColorBrush.Color));
        }

        private Color BlendColors(Color top, Color bottom)
        {
            // Простое альфа-смешивание
            var alpha = top.A / 255.0;
            return Color.FromArgb(
                255,
                (byte)(top.R * alpha + bottom.R * (1 - alpha)),
                (byte)(top.G * alpha + bottom.G * (1 - alpha)),
                (byte)(top.B * alpha + bottom.B * (1 - alpha))
            );
        }
    }
}
