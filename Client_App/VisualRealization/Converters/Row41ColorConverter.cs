using Avalonia.Data.Converters;
using Avalonia.Media;
using Models.Forms.Form4;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.VisualRealization.Converters
{
    public class RowColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Form41 form41)
            {
                if (form41.LicenseOrRegistrationInfo_DB is "" or null
                    && form41.NumOfFormsWithInventarizationInfo_DB <=0
                    && form41.NumOfFormsWithoutInventarizationInfo_DB <=0
                    && form41.NumOfForms212_DB <= 0
                    && form41.Note_DB is "" or null)
                    return new SolidColorBrush(new Color(50, 255, 255, 0));

                if ((form41.NumOfFormsWithInventarizationInfo_DB > 0
                    ||form41.NumOfFormsWithoutInventarizationInfo_DB >0
                    ||form41.NumOfForms212_DB>0)
                    && form41.LicenseOrRegistrationInfo_DB is "" or null)
                    return new SolidColorBrush(new Color(50, 139, 0, 255));

                if (form41.NumOfFormsWithInventarizationInfo_DB <= 0)
                    return new SolidColorBrush(new Color(50, 255, 0, 0));
            }
            return Brushes.Transparent; // Значение по умолчанию
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
