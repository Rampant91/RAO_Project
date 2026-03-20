using Avalonia.Data;
using Avalonia.Data.Converters;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.VisualRealization.Converters
{
    //Этот конвертер используется чтобы деактивировать кнопки белого списка в FormsTabControl
    //Настроить CanExecute у команды не получилось, т.к. нужно его обновлять после каждого изменения SelectedReports 
    public class CheckReportsAnyMatchingFormNumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Reports selectedReports
                && parameter is string formNum)
            {
                return selectedReports.Report_Collection.Any(rep => rep.FormNum_DB == formNum);
            }
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
